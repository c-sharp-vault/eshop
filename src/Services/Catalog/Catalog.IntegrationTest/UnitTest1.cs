using System.Net.Http;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc.Testing;
using Catalog.API.Utils;
using Catalog.API;

namespace Catalog.IntegrationTest;

[TestFixture]
public class Tests {
    private HttpClient _client;

    public Tests() {
        var appFactory = new WebApplicationFactory<Program>();
        _client = appFactory.CreateClient();
    }

    [SetUp]
    public void Setup() {
    }

    [Test]
    public async void Test1() {
        //var response = await _client.GetAsync(APIRoutes.Items.GetAll.Replace("pageSize", "10").Replace("pageIndex", "1"));
        var response = await _client.GetAsync("https://localhost:7207/api/v1/catalog/items?pageSize=10&pageIndex=1");
    }
}