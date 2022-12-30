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
using Catalog.DataAccess.DTOs.CatalogItem;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using AutoMapper.Internal;
using System.Reflection;
using Microsoft.OpenApi.Writers;
using AutoMapper;
using Catalog.API.Profiles;
using Amido.NAuto;
using System.Linq;
using Microsoft.AspNetCore.TestHost;

namespace Catalog.IntegrationTest;

[TestFixture]
public class IntegrationTest {
    protected WebApplicationFactory<Program> _appFactory;
    protected IServiceScopeFactory _serviceFactory;
	protected HttpClient _httpClient;
    protected IMapper _mapper;
	protected IList<CatalogItem> _testList;

    protected IntegrationTest() {
		_appFactory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => {
			builder.UseEnvironment("Testing").ConfigureTestServices(services => {
				services.RemoveAll(typeof(CatalogContext)).AddDbContext<CatalogContext>(options => {
					options.UseInMemoryDatabase("TestDB");
				});
			});
		});
		//_appFactory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => builder.UseEnvironment("Testing"));
		_httpClient = _appFactory.CreateClient();
		_serviceFactory = _appFactory.Services.GetService<IServiceScopeFactory>();
		_mapper = new Mapper(new MapperConfiguration(x => x.AddProfile(new CatalogItemProfile())));
	}

	[SetUp]
	protected void Setup() {
	}

	[TearDown]
	protected void TearDown() {
	}

	// Seeding via CatalogContext
	protected async Task<bool> SeedData() {
		using var scope = _serviceFactory?.CreateScope();
		var context = scope?.ServiceProvider?.GetRequiredService<CatalogContext>();
		var catalogBrand = context?.CatalogBrands.First();
		var catalogType = context?.CatalogTypes.First();
		_testList = NAuto.GetRandomList<CatalogItem>(x => x.CatalogItemID, 30, 1);
		foreach (var element in _testList) {
			element.Name += $"Fake name for item #{element.CatalogItemID}";
			element.CatalogBrandID = catalogBrand.CatalogBrandID;
			element.CatalogBrand = catalogBrand;
			element.CatalogTypeID = catalogType.CatalogTypeID;
			element.CatalogType = catalogType;
		}
		await context?.CatalogItems.AddRangeAsync(_testList);
		return context?.SaveChanges() > 0;
	}

	// Removing via CatalogContext
	protected bool RemoveData() {
		using var scope = _serviceFactory?.CreateScope();
		var context = scope?.ServiceProvider?.GetRequiredService<CatalogContext>();
		context?.CatalogItems.RemoveRange(_testList);
		return context?.SaveChanges() > 0;
	}

	// Seeding via UnitOfWork
	//protected async Task<bool> SeedData() {
	//	var response = await _httpClient?.PostAsJsonAsync(APIRoutes.Items.Create, _testList);
	//	return response.IsSuccessStatusCode;
	//}

	// Removing via UnitOfWork
	//protected async Task<bool> RemoveData() {
	//	List<bool> allSuccessfullResponses = new List<bool>();
	//	foreach (CatalogItem item in _testList) {
	//		var response = await _httpClient?.PostAsJsonAsync(APIRoutes.Items.Delete, item.ID);
	//		allSuccessfullResponses.Add(response.IsSuccessStatusCode);
	//	}
	//	return allSuccessfullResponses.All(x => x == true);
	//}
}