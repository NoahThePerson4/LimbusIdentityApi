using FluentAssertions;
using LimbusIdentityApi.Data;
using LimbusIdentityApi.Dtos;
using LimbusIdentityApi;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.MsSql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using FluentValidation.Results;

namespace LimbusIdentity.Test;

[Collection("DatabaseTests")]
public class IntegrationTestSkills
: IClassFixture<WebApplicationFactory<IApiMarker>>, IAsyncLifetime
{
    private readonly WebApplicationFactory<IApiMarker> _factory;
    private readonly MsSqlContainer _msSqlContainer;

    public IntegrationTestSkills(WebApplicationFactory<IApiMarker> factory)
    {
        _msSqlContainer = new MsSqlBuilder()
            .WithName("sa")
            .WithPassword("MySaPassword123")
            .Build();

        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IdentityDbContext));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<IdentityDbContext>(options =>
                {
                    options.UseSqlServer($"{_msSqlContainer.GetConnectionString()}");
                });
            });
        });
    }

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();

        //NOTE THIS IS VERY IMPORTANT
        //YOU NEED TO APPLY MIGRATIONS TO NEW DATABASE
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
            context.Database.Migrate();
        }
    }

    [Fact]
    public async Task GetSkills_ShouldReturnOkAndObject_WhenSkillExists()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var skillId = 1;
        var skill = new Skill { 
            Id = skillId,
            Image = "https://MyCoolImage.png",
            Name = "Test",
            Type = "Blunt",
            Sin = "Gloom",
            OffenseLevel = 40,
            MinRoll = 2,
            MaxRoll = 4,
            SkillEffect = "None",
            CoinEffects = new List<string>{"None"}
            };

        var createResponse = await httpClient.PostAsJsonAsync("skills", skill);
        createResponse.EnsureSuccessStatusCode();
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
            var savedSkill = await context.Skills.FirstOrDefaultAsync(s => s.Id == skillId);
            Assert.NotNull(savedSkill);
        }


        //Act
        var response = await httpClient.GetAsync($"skills/{skillId}");
        var getSkill = await response.Content.ReadFromJsonAsync<SkillDto>();

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        getSkill.Should().BeEquivalentTo(skill);
    }

    [Fact]
    public async Task GetSkills_ShouldReturnNotFound_WhenSkillDoesNotExist()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var skillId = 1;

        //Act
        var response = await httpClient.GetAsync($"skills/{skillId}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PostSkills_ShouldReturnBadRequest_WhenSkillFailsToValidate()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var skill = new Skill
        {
            Id = 1,
            Image = "https://MyCoolImage.png",
            Name = "string",
            Type = "Gloom",
            Sin = "Cool",
            OffenseLevel = -5,
            MinRoll = 5,
            MaxRoll = 3,
            SkillEffect = "None",
            CoinEffects = new List<string> { "None" }
        };

        //Act
        var result = await httpClient.PostAsJsonAsync("skills",skill);
        var validationErrors = await result.Content.ReadFromJsonAsync<List<ValidationFailure>>();


        //Assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        validationErrors.Should().NotBeNull();
        validationErrors.Should().Contain(e => e.PropertyName == "Name" && e.ErrorMessage == "You left the skill on the default name.");
        validationErrors.Should().Contain(e => e.PropertyName == "Type" && e.ErrorMessage == "Coin Type must be either Slash, Pierce, or Blunt.");
        validationErrors.Should().Contain(e=> e.PropertyName == "Sin" && e.ErrorMessage == "Sin Type must be either Wrath. Lust, Sloth, Gluttony, Gloom, Pride, or Envy.");
        validationErrors.Should().Contain(e => e.PropertyName == "OffenseLevel" && e.ErrorMessage == "Offense Level can't be negative.");
        validationErrors.Should().Contain(e => e.PropertyName == "MinRoll" && e.ErrorMessage == "Min Roll can't be higher than your Max Roll.");
    }

    [Fact]
    public async Task UpdateSkills_ShouldReturnNoContent_WhenSkillExists()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var skillId = 1;
        var skill = new Skill
        {
            Id = skillId,
            Image = "https://MyCoolImage.png",
            Name = "Test",
            Type = "Blunt",
            Sin = "Gloom",
            OffenseLevel = 40,
            MinRoll = 2,
            MaxRoll = 4,
            SkillEffect = "None",
            CoinEffects = new List<string> { "None" }
        };

        var newSkill = new Skill
        {
            Id = skillId,
            Image = "https://MyCoolImage.png",
            Name = "Test",
            Type = "Blunt",
            Sin = "Gloom",
            OffenseLevel = 40,
            MinRoll = 2,
            MaxRoll = 4,
            SkillEffect = "None",
            CoinEffects = new List<string> { "None" }
        };
        var createResponse = await httpClient.PostAsJsonAsync("/skills",skill);
        createResponse.EnsureSuccessStatusCode();

        //Act
        var update = await httpClient.PutAsJsonAsync($"/skills/{skillId}", newSkill);
        var updateResponse = await httpClient.GetAsync($"/skills/{skillId}");
        var updateResult = await updateResponse.Content.ReadFromJsonAsync<SkillDto>();

        //Assert
        update.StatusCode.Should().Be(HttpStatusCode.NoContent);
        updateResult.Should().BeEquivalentTo(newSkill);
    }

    [Fact]
    public async Task UpdateSkills_ShouldReturnNotFound_WhenSkillDoesNotExist()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var skillId = 1;
        var skill = new Skill
        {
            Id = skillId,
            Image = "https://MyCoolImage.png",
            Name = "Test",
            Type = "Blunt",
            Sin = "Gloom",
            OffenseLevel = 40,
            MinRoll = 2,
            MaxRoll = 4,
            SkillEffect = "None",
            CoinEffects = new List<string> { "None" }
        };

        //Act
        var response = await httpClient.PutAsJsonAsync($"skills/{skillId}", skill);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateSkills_ShouldReturnBadRequest_WhenSkillFailsToValidate()
    {
        //Arrange
        var httpClient = _factory.CreateClient();

        var skillPost = new Skill
        {
            Id = 1,
            Image = "https://MyCoolImage.png",
            Name = "Test",
            Type = "Blunt",
            Sin = "Gloom",
            OffenseLevel = 40,
            MinRoll = 2,
            MaxRoll = 4,
            SkillEffect = "None",
            CoinEffects = new List<string> { "None" }
        };

        var skill = new Skill
        {
            Id = 1,
            Image = "https://MyCoolImage.png",
            Name = "string",
            Type = "Gloom",
            Sin = "Cool",
            OffenseLevel = -5,
            MinRoll = 5,
            MaxRoll = 3,
            SkillEffect = "None",
            CoinEffects = new List<string> { "None" }
        };

        //Act
        await httpClient.PostAsJsonAsync("skills", skillPost);
        var result = await httpClient.PutAsJsonAsync("skills/1", skill);
        var content = result.Content.ReadAsStringAsync();
        var validationErrors = await result.Content.ReadFromJsonAsync<List<ValidationFailure>>();


        //Assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        validationErrors.Should().NotBeNull();
        validationErrors.Should().Contain(e => e.PropertyName == "Name" && e.ErrorMessage == "You left the skill on the default name.");
        validationErrors.Should().Contain(e => e.PropertyName == "Type" && e.ErrorMessage == "Coin Type must be either Slash, Pierce, or Blunt.");
        validationErrors.Should().Contain(e => e.PropertyName == "Sin" && e.ErrorMessage == "Sin Type must be either Wrath. Lust, Sloth, Gluttony, Gloom, Pride, or Envy.");
        validationErrors.Should().Contain(e => e.PropertyName == "OffenseLevel" && e.ErrorMessage == "Offense Level can't be negative.");
        validationErrors.Should().Contain(e => e.PropertyName == "MinRoll" && e.ErrorMessage == "Min Roll can't be higher than your Max Roll.");
    }

    [Fact]
    public async Task DeleteSkill_ReturnsNoContent_AfterDeleting()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var skillId = 1;
        var skill = new Skill
        {
            Id = 1,
            Image = "https://MyCoolImage.png",
            Name = "Test",
            Type = "Blunt",
            Sin = "Gloom",
            OffenseLevel = 40,
            MinRoll = 2,
            MaxRoll = 4,
            SkillEffect = "None",
            CoinEffects = new List<string> { "None" }
        };

        //Act
        var create = await httpClient.PostAsJsonAsync("/skills", skill);
        create.EnsureSuccessStatusCode();

        var result = await httpClient.DeleteAsync($"/skills/{skillId}");
        var deleted = await httpClient.GetAsync($"/skills/{skillId}");

        //Assert
        create.StatusCode.Should().Be(HttpStatusCode.Created);
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        deleted.StatusCode.Should().Be(HttpStatusCode.NotFound);

    }

    public async Task DisposeAsync()
    {
        await _msSqlContainer.DisposeAsync().AsTask();
    }
}

