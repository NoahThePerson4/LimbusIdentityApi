using FluentAssertions;
using FluentValidation.Results;
using LimbusIdentityApi;
using LimbusIdentityApi.Data;
using LimbusIdentityApi.Dtos;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using Testcontainers.MsSql;

namespace LimbusIdentity.Test;
[Collection("DatabaseTests")]
public class IntegrationTestIdentities
    : IClassFixture<WebApplicationFactory<IApiMarker>>, IAsyncLifetime
{
    private readonly WebApplicationFactory<IApiMarker> _factory;
    private readonly MsSqlContainer _msSqlContainer;
    public IntegrationTestIdentities(WebApplicationFactory<IApiMarker> factory)
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

        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
            context.Database.Migrate();
        }
    }

    [Fact]
    public async Task GetIds_ReturnsOkAndObject_WhenIdsExists()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var ids = new Identity
        {
            Id = 1,
            Name = "Test Fixer",
            Sinner = "Yi Sang",
            Health = 1,
            Ineffective = "Slash",
            Fatal = "Blunt",
            DefenseLevel = 50,
            MinSpeed = 2,
            MaxSpeed = 5,
            Image = null,
            Passives = null,
            Skills = null
        };

        //Act
        var createResponse = await httpClient.PostAsJsonAsync("/identities", ids);
        createResponse.EnsureSuccessStatusCode();

        //Assert
        var result = await httpClient.GetAsync("/identities/1");
        var existingId = await result.Content.ReadFromJsonAsync<IdentityDto>();

        result.StatusCode.Should().Be(HttpStatusCode.OK);
        existingId.Should().BeEquivalentTo(ids, options => options
        .Excluding(e => e.Passives)
        .Excluding(e => e.Skills));
    }

    [Fact]
    public async Task GetIds_ReturnsNotFound_WhenIdsDoesNotExist()
    {
        //Arrange
        var httpClient = _factory.CreateClient();

        //Act
        var result = await httpClient.GetAsync("/identities/1");

        //Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateIds_ReturnsBadRequest_WhenIdsFailsToValidate()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var ids = new Identity
        {
            Id = 1,
            Name = "string",
            Sinner = "Hi",
            Health = -5,
            Ineffective = "Slash",
            Fatal = "Slash",
            DefenseLevel = -10,
            MinSpeed = 6,
            MaxSpeed = 3,
            Image = null,
            Passives = null,
            Skills = null
        };

        //Act
        var result = await httpClient.PostAsJsonAsync("/identities",ids);
        var content = await result.Content.ReadFromJsonAsync<List<ValidationFailure>>();
        
        //Assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        content.Should().Contain(e => e.PropertyName == "Name" && e.ErrorMessage == "You left your name on the default Name.");
        content.Should().Contain(e => e.PropertyName == "Sinner" & e.ErrorMessage == "Sorry that is not a valid Sinner name. For Rodya please put Rodion. For Ryōshū please put Ryoshu. The 12 Sinners are Yi Sang, Faust, Don Quixote, Ryoshu, Meursault, Hong Lu, Heathcliff, Ishmael, Rodion, Sinclair, Outis, Gregor.");
        content.Should().Contain(e => e.PropertyName == "Health" & e.ErrorMessage == "Health can't be negative.");
        content.Should().Contain(e => e.PropertyName == "Ineffective" & e.ErrorMessage == "You can't both be Fatal and Ineffective to the same Damage Type.");
        content.Should().Contain(e => e.PropertyName == "DefenseLevel" & e.ErrorMessage == "Defense Level can't be negative.");
        content.Should().Contain(e => e.PropertyName == "MinSpeed" & e.ErrorMessage == "Minimum Speed must be lower than Maximum Speed.");
    }

    [Fact]
    public async Task UpdateIds_ReturnsNoContent_WhenIdsExist()
    {
        var httpClient = _factory.CreateClient();
        var ids = new Identity
        {
            Id = 1,
            Name = "Test Fixer",
            Sinner = "Yi Sang",
            Health = 1,
            Ineffective = "Slash",
            Fatal = "Blunt",
            DefenseLevel = 50,
            MinSpeed = 2,
            MaxSpeed = 5,
            Image = null,
            Passives = null,
            Skills = null
        };

        var updateIds = new Identity
        {
            Id = 1,
            Name = "Test New Fixer",
            Sinner = "Faust",
            Health = 1,
            Ineffective = "Slash",
            Fatal = "Blunt",
            DefenseLevel = 50,
            MinSpeed = 2,
            MaxSpeed = 5,
            Image = null,
            Passives = null,
            Skills = null
        };

        var createRequest = await httpClient.PostAsJsonAsync("/identities", ids);
        createRequest.EnsureSuccessStatusCode();

        //Act
        var updateRequest = await httpClient.PutAsJsonAsync("/identities/1", updateIds);
        var resultStatus = await httpClient.GetAsync("/identities/1");
        var result = await resultStatus.Content.ReadFromJsonAsync<IdentityDto>();

        //Assert
        updateRequest.StatusCode.Should().Be(HttpStatusCode.NoContent);
        resultStatus.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().BeEquivalentTo(updateIds, options => options
        .Excluding(e => e.Skills)
        .Excluding(e => e.Passives));
    }

    [Fact]
    public async Task UpdateIds_ReturnsNotFound_WhenIdsDoesNotExist()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var ids = new Identity
        {
            Id = 1,
            Name = "Test Fixer",
            Sinner = "Yi Sang",
            Health = 1,
            Ineffective = "Slash",
            Fatal = "Blunt",
            DefenseLevel = 50,
            MinSpeed = 2,
            MaxSpeed = 5,
            Image = null,
            Passives = null,
            Skills = null
        };

        //Act
        var result = await httpClient.PutAsJsonAsync("/identities/1", ids);

        //Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateIds_ShouldReturnBadRequest_WhenIdsFailsToValidate()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var ids = new Identity
        {
            Id = 1,
            Name = "Test Fixer",
            Sinner = "Yi Sang",
            Health = 1,
            Ineffective = "Slash",
            Fatal = "Blunt",
            DefenseLevel = 50,
            MinSpeed = 2,
            MaxSpeed = 5,
            Image = null,
            Passives = null,
            Skills = null
        };

        var updateIds = new Identity
        {
            Id = 1,
            Name = "string",
            Sinner = "Hi",
            Health = -5,
            Ineffective = "Gloom",
            Fatal = "Pride",
            DefenseLevel = -10,
            MinSpeed = -5,
            MaxSpeed = -2,
            Image = null,
            Passives = null,
            Skills = null
        };
        var createResponse = await httpClient.PostAsJsonAsync("/identities", ids);
        createResponse.EnsureSuccessStatusCode();

        //Act
        var updateResponse = await httpClient.PutAsJsonAsync("/identities/1", updateIds);
        var content = await updateResponse.Content.ReadFromJsonAsync<List<ValidationFailure>>();

        //Assert
        updateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        content.Should().Contain(e => e.PropertyName == "Name" && e.ErrorMessage == "You left your name on the default Name.");
        content.Should().Contain(e => e.PropertyName == "Sinner" & e.ErrorMessage == "Sorry that is not a valid Sinner name. For Rodya please put Rodion. For Ryōshū please put Ryoshu. The 12 Sinners are Yi Sang, Faust, Don Quixote, Ryoshu, Meursault, Hong Lu, Heathcliff, Ishmael, Rodion, Sinclair, Outis, Gregor.");
        content.Should().Contain(e => e.PropertyName == "Health" & e.ErrorMessage == "Health can't be negative.");
        content.Should().Contain(e => e.PropertyName == "Ineffective" & e.ErrorMessage == "Resistance must be either Slash, Pierce, or Blunt.");
        content.Should().Contain(e => e.PropertyName == "Fatal" & e.ErrorMessage == "Weakness must be either Slash, Pierce, or Blunt.");
        content.Should().Contain(e => e.PropertyName == "DefenseLevel" & e.ErrorMessage == "Defense Level can't be negative.");
        content.Should().Contain(e => e.PropertyName == "MinSpeed" && e.ErrorMessage == "Minimum Speed can't be negative.");
        content.Should().Contain(e => e.PropertyName == "MaxSpeed" && e.ErrorMessage == "Maximum Speed can't be negative.");
    }

    public async Task DisposeAsync()
    {
        await _msSqlContainer.DisposeAsync().AsTask();
    }
}

