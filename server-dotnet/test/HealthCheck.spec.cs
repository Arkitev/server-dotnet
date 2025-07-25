﻿using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace server_dotnet.tests;

public class HealthEndpointTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task GET_HealthEndpoint_ReturnsStatusHealthy()
    {
        var client = factory.CreateClient();
        var response = await client.GetAsync("/health");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var payload = await response.Content.ReadFromJsonAsync<HealthDto>();
        payload.Should().NotBeNull();
        payload!.Status.Should().Be("Healthy");
    }

    [Fact]
    public async Task GET_ReadinessEndpoint_ReturnsStatusHealthy()
    {
        var client = factory.CreateClient();
        var response = await client.GetAsync("/readiness");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var payload = await response.Content.ReadFromJsonAsync<HealthDto>();
        payload.Should().NotBeNull();
        payload!.Status.Should().Be("Healthy");
    }

    private class HealthDto
    {
        public string? Status { get; set; }
    }
}