using System.Net.Http;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc.Testing;
using Catalog.API.Utils;
using Catalog.API;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Catalog.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Catalog.Core.Models;
using System.Collections.Generic;
using Catalog.API.DTOs.CatalogItem;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using AutoMapper.Internal;
using System.Reflection;
using Microsoft.OpenApi.Writers;
using AutoMapper;
using Catalog.API.Profiles;
using Amido.NAuto;

namespace Catalog.IntegrationTest;

[TestFixture]
public class IntegrationTest {
    protected WebApplicationFactory<Program> _appFactory;
    protected IServiceScopeFactory? _serviceFactory;
    protected HttpClient _httpClient;
    protected IMapper _mapper;
	protected IList<CatalogItem> _testList;

    protected IntegrationTest() {
		_appFactory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => {
            builder.UseEnvironment("Testing").ConfigureServices(services => {
                services.RemoveAll(typeof(CatalogContext)).AddDbContext<CatalogContext>(options => {
                    options.UseInMemoryDatabase("TestDB");
                });
            });
        });
        _httpClient = _appFactory.CreateClient();
		_serviceFactory = _appFactory.Services.GetService<IServiceScopeFactory>();
		_mapper = new Mapper(new MapperConfiguration(x => x.AddProfile(new CatalogItemProfile())));
	}

    [SetUp]
	protected void Setup() {
		_testList = NAuto.GetRandomList<CatalogItem>(x => x.ID, 30, 1);
		foreach (var testElement in _testList) testElement.Name += $"Fake name for item #{testElement.ID}";
	}

	[TearDown]
	protected void TearDown() {
		_testList = null;
	}

	protected bool SeedData() {
		using var scope = _serviceFactory?.CreateScope();
		var context = scope?.ServiceProvider?.GetRequiredService<CatalogContext>();
		context?.CatalogItems.AddRange(_testList);
		return context?.SaveChanges() > 0;
	}

	protected bool RemoveData() {
		using var scope = _serviceFactory?.CreateScope();
		var context = scope?.ServiceProvider?.GetRequiredService<CatalogContext>();
		context?.CatalogItems.RemoveRange(_testList);
		return context?.SaveChanges() > 0;
	}
}