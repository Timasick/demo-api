using DemoApi.Domain;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace DemoApi.Tests.Integration;

[Collection(ApiCollection.Name)]
public class ProjectsApiTests(WebApplicationFactory<Program> factory)
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetProjects_ReturnsSuccessStatusCode()
    {
        // Act
        var response = await _client.GetAsync("/api/projects");

        // Assert
        response.EnsureSuccessStatusCode();
        var projects = await response.Content.ReadFromJsonAsync<List<Project>>();
        Assert.NotNull(projects);
    }

    [Fact]
    public async Task GetStats_ReturnsNotFound_WhenProjectDoesNotExist()
    {
        // Запрос статистики для несуществующего проекта
        var response = await _client.GetAsync("/api/projects/99999/stats");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Create_ReturnsCreatedWithLocation()
    {
        var response = await _client.PostAsJsonAsync("/api/projects", new { name = TestApi.Unique("project") });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers.Location);
    }

    [Fact]
    public async Task Create_DuplicateName_ReturnsConflict()
    {
        var name = TestApi.Unique("duplicate");
        (await _client.PostAsJsonAsync("/api/projects", new { name })).EnsureSuccessStatusCode();

        var response = await _client.PostAsJsonAsync("/api/projects", new { name });

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task Create_EmptyName_ReturnsBadRequest()
    {
        var response = await _client.PostAsJsonAsync("/api/projects", new { name = "" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Get_Unknown_ReturnsNotFound()
    {
        var response = await _client.GetAsync($"/api/projects/{int.MaxValue}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    [Fact]
    public async Task Rename_ReturnsSuccess_WhenProjectExists()
    {
        var name = TestApi.Unique("rename");
        var create = await _client.PostAsJsonAsync("/api/projects", new { name });
        var project = await create.Content.ReadFromJsonAsync<Project>();

        Assert.NotNull(project);

        var newName = TestApi.Unique("renamed");

        var response = await _client.PutAsJsonAsync(
            $"/api/projects/{project.Id}",
            new { name = newName });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var updated = await response.Content.ReadFromJsonAsync<Project>();
        Assert.NotNull(updated);
        Assert.Equal(newName, updated.Name);
    }

    [Fact]
    public async Task Rename_DuplicateName_ReturnsConflict()
    {
        var firstName = TestApi.Unique("first");
        var secondName = TestApi.Unique("second");

        var first = await _client.PostAsJsonAsync(
            "/api/projects", new { name = firstName });

        var second = await _client.PostAsJsonAsync(
            "/api/projects", new { name = secondName });

        var firstProject = await first.Content.ReadFromJsonAsync<Project>();
        Assert.NotNull(firstProject);

        var response = await _client.PutAsJsonAsync(
            $"/api/projects/{firstProject.Id}",
            new { name = secondName });

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }
}
