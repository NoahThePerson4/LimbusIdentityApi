﻿using Testcontainers.MsSql;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using LimbusIdentityApi;
using LimbusIdentityApi.Data;
using Microsoft.Extensions.DependencyInjection;
using LimbusIdentityApi.Dtos;
using System.Net;
using FluentAssertions;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
namespace LimbusIdentity.TestIntegration;

[Collection("DatabaseTests")]
public class IntegrationTestPassives
: IClassFixture<WebApplicationFactory<IApiMarker>>, IAsyncLifetime
{
    private readonly WebApplicationFactory<IApiMarker> _factory;
    private readonly MsSqlContainer _msSqlContainer;
    private ITestOutputHelper _output;
    public IntegrationTestPassives(WebApplicationFactory<IApiMarker> factory, ITestOutputHelper outPut)
    {
        _output = outPut;

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
    public async Task GetPassive_ReturnsOkAndObject_WhenPassiveExists()
    {
        // Arrange
        var httpClient = _factory.CreateClient();
        var passive = new Passive
        {
            Id = 1,
            Name = "Test Passive",
            Cost = "3 Test Owned",
            Support = true,
            Description = "A test passive"
        };

        // Act
        var createResponse = await httpClient.PostAsJsonAsync("/passives", passive);
        createResponse.EnsureSuccessStatusCode();

        // Assert
        var result = await httpClient.GetAsync($"/passives/{passive.Id}");
        var existingPassive = await result.Content.ReadFromJsonAsync<PassiveDto>();

        result.StatusCode.Should().Be(HttpStatusCode.OK);
        existingPassive.Should().BeEquivalentTo(passive);
    }

    [Fact]
    public async Task GetPassive_ReturnsNotFound_WhenPassiveDoesNotExists()
    {
        //Arrange
        var httpClient = _factory.CreateClient();

        //Act
        var result = await httpClient.GetAsync($"/passives/{1}");

        //Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);

    }

    [Fact]
    public async Task PostPassive_ReturnsBadRequest_WhenPassiveFailsToValidate()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var passive = new Passive
        {
            Id = 1,
            Name = "string",
            Cost = "string",
            Support = true,
            Description = "string"
        };

        //Act
        var result = await httpClient.PostAsJsonAsync("/passives", passive);
        var content = await result.Content.ReadAsStringAsync();
        var validationErrors = await result.Content.ReadFromJsonAsync<List<ValidationFailure>>();

        //Assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        validationErrors.Should().NotBeNull();
        validationErrors.Should().Contain(e => e.PropertyName == "Name" && e.ErrorMessage == "You left your name on the default Name.");
        validationErrors.Should().Contain(e => e.PropertyName == "Cost" && e.ErrorMessage == "You left your Cost on the default Cost.");
        validationErrors.Should().Contain(e => e.PropertyName == "Description" && e.ErrorMessage == "You left your Description on the default Description.");
    }

    [Fact]
    public async Task UpdatePassive_ReturnsNoContent_WhenPassiveExists()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var passive = new Passive
        {
            Id = 1,
            Name = "Test Passive",
            Cost = "3 Test Owned",
            Support = true,
            Description = "A test passive"
        };

        var passiveNew = new Passive
        {
            Id = 1,
            Name = "Test New",
            Cost = "3 Test Owned",
            Support = false,
            Description = "A test passive"
        };

        //Act
        var createResponse = await httpClient.PostAsJsonAsync("/passives", passive);
        createResponse.EnsureSuccessStatusCode();
        var updateResponse = await httpClient.PutAsJsonAsync($"/passives/{1}", passiveNew);
        updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        //Assert
        var result = await httpClient.GetAsync($"passives/{1}");
        var updatedPassive = await result.Content.ReadFromJsonAsync<PassiveDto>();
        updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        updatedPassive.Should().BeEquivalentTo(passiveNew);
    }

    [Fact]
    public async Task UpdatePassive_ReturnsNotFound_WhenPassiveDoesNotExist()
    {
        //Arrange
        var httpClient = _factory.CreateClient();

        var passiveNew = new Passive
        {
            Id = 999,
            Name = "Test New",
            Cost = "3 Test Owned",
            Support = false,
            Description = "A test passive"
        };

        //Act
        var updateResponse = await httpClient.PutAsJsonAsync($"/passives/{999}", passiveNew);

        //Assert
        updateResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdatePassive_ReturnsBadRequest_WhenPassiveFailsToValidate()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var passivePost = new Passive
        {
            Id = 1,
            Name = "Test Passive",
            Cost = "3 Test Owned",
            Support = true,
            Description = "A test passive"
        };
        var passive = new Passive
        {
            Id = 1,
            Name = "string",
            Cost = "string",
            Support = true,
            Description = "string"
        };

        //Act
        await httpClient.PostAsJsonAsync("/passives", passivePost);
        var result = await httpClient.PutAsJsonAsync("/passives/1", passive);
        var content = await result.Content.ReadAsStringAsync();
        var validationErrors = await result.Content.ReadFromJsonAsync<List<ValidationFailure>>();

        //Assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        validationErrors.Should().NotBeNull();
        validationErrors.Should().Contain(e => e.PropertyName == "Name" && e.ErrorMessage == "You left your name on the default Name.");
        validationErrors.Should().Contain(e => e.PropertyName == "Cost" && e.ErrorMessage == "You left your Cost on the default Cost.");
        validationErrors.Should().Contain(e => e.PropertyName == "Description" && e.ErrorMessage == "You left your Description on the default Description.");
    }

    [Fact]
    public async Task DeletePassive_ReturnsNoContent_AfterDeleting()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        var passiveId = 1;
        var passive = new Passive
        {
            Id = passiveId,
            Name = "Test Passive",
            Cost = "3 Test Owned",
            Support = true,
            Description = "A test passive"
        };

        //Act
        var create = await httpClient.PostAsJsonAsync("/passives", passive);
        create.EnsureSuccessStatusCode();

        var result = await httpClient.DeleteAsync($"/passives/1");
        var deleted = await httpClient.GetAsync($"/passives/{passiveId}");

        //Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);

    }

    public async Task DisposeAsync()
    {
        await _msSqlContainer.DisposeAsync().AsTask();
    }
}
