using Catalog.API.DTOs.CatalogItem;
using Catalog.API.Utils;
using Catalog.Core.Models;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Catalog.IntegrationTest.V1.Controller {
	public class CatalogItemsControllerTest : IntegrationTest {

		[Test]
		public async Task GetAll_WhitoutAnyCatalogItemsInDB_ReturnsEmptyList() {
			// Arrange
			var url = APIRoutes.Items.GetAll.Replace("{pageSize}", "10").Replace("{pageIndex}", "1");

			// Act
			var response = await _httpClient.GetAsync(url);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
			response.Content.ReadFromJsonAsync<IEnumerable<CatalogItemReadDTO>>().Result.Should().BeEmpty();
		}

		[TestCase(10, 1)]
		[TestCase(10, 2)]
		[TestCase(10, 3)]
		[TestCase(15, 1)]
		[TestCase(15, 2)]
		[TestCase(30, 1)]
		[Test]
		public async Task GetAll_WhithCatalogItemsInDB_ReturnsPopulatedList(int pageSize, int pageIndex) {
			try {
				// Arrange
				SeedData();
				var url = APIRoutes.Items.GetAll.Replace("{pageSize}", pageSize.ToString())
												.Replace("{pageIndex}", pageIndex.ToString());

				// Act
				var response = await _httpClient.GetAsync(url);

				// Assert
				response.StatusCode.Should().Be(HttpStatusCode.OK);
				var result = response.Content.ReadFromJsonAsync<IEnumerable<CatalogItemReadDTO>>().Result;
				_mapper.Map<List<CatalogItem>>(result).Should().HaveCount(pageSize).And
					   .BeEquivalentTo(_testList.Skip(pageSize * (pageIndex - 1)).Take(pageSize), cfg => cfg
					   .Excluding(x => x.CatalogBrand.CreatedOn).Excluding(x => x.CatalogBrand.CatalogItems)
					   .Excluding(x => x.CatalogType.CreatedOn).Excluding(x => x.CatalogType.CatalogItems)
					   .Excluding(x => x.CreatedOn).Excluding(x => x.UpdatedOn));
			} finally { 
				RemoveData();
			}
		}
	}
}
