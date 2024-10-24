using LimbusIdentityApi.Data;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using LimbusIdentityApi;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net;
using LimbusIdentityApi.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using LimbusIdentityApi.Repositories;
using NSubstitute;
using Microsoft.AspNetCore.Mvc;
namespace LimbusIdentity.Test;

public class UnitTestPassives
 : IClassFixture<WebApplicationFactory<IApiMarker>>
{
    private readonly WebApplicationFactory<IApiMarker> _factory;
    private readonly IPassiveRepository _substituteRepository;

    public UnitTestPassives(WebApplicationFactory<IApiMarker> factory)
    {
        _substituteRepository = Substitute.For<IPassiveRepository>();

        _substituteRepository.CreatePassive(Arg.Any<Passive>()).Returns(Task.CompletedTask);

        _substituteRepository.GetPassive(1).Returns(new Passive
        {
            Id = 1,
            Name = "Test Passive",
            Cost = "3 Test Owned",
            Support = true,
            Description = "A test passive"
        });

        _substituteRepository.DeletePassive(1).Returns(Task.CompletedTask);

        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IPassiveRepository));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddScoped(_ => _substituteRepository);
            });
        });
    }

    [Fact]
    public async Task GetPassive_ReturnsOkAndObject_WhenPassiveExists()
    {
        // Arrange
        var httpClient = _factory.CreateClient();

        // Act
        var result = await httpClient.GetAsync("/passives/1");
        var existingPassive = await result.Content.ReadFromJsonAsync<PassiveDto>();

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        existingPassive.Should().BeEquivalentTo(new PassiveDto
        (
            1,
            "Test Passive",
            "3 Test Owned",
            true,
            "A test passive"
        ));
    }
}