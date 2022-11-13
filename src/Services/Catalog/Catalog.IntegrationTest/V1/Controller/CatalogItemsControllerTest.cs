using Catalog.API.DTOs.CatalogItem;
using Catalog.API.Utils;
using Catalog.Core.Models;
using Catalog.DataAccess;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using static Catalog.API.Utils.APIRoutes;
using Amido.NAuto;

namespace Catalog.IntegrationTest.V1.Controller {
	public class CatalogItemsControllerTest : IntegrationTest {

		#region GetAll

		[TestCase(6, 1)]
		[TestCase(6, 2)]
		[TestCase(8, 1)]
		[TestCase(8, 2)]
		[TestCase(10, 1)]
		[TestCase(10, 2)]
		[TestCase(12, 1)]
		[TestCase(12, 2)]
		[Test]
		public async Task GetAll_WhithCatalogItemsInDB_ReturnsNonEmptyList(int pageSize, int pageIndex) {
			// Arrange
			using var scope = _serviceFactory?.CreateScope();
			var context = scope?.ServiceProvider?.GetRequiredService<CatalogContext>();
			_ = context.CatalogBrands.ToList();
			_ = context.CatalogTypes.ToList();
			var expected = context?.CatalogItems.Include(x => x.CatalogBrand)
												.Include(x => x.CatalogType)
												.OrderBy(x => x.CatalogItemID)
												.Skip(pageSize * (pageIndex - 1))
												.Take(pageSize)
												.ToList();
			var url = APIRoutes.Items.GetAll.Replace("{pageIndex}", pageIndex.ToString())
											.Replace("{pageSize}", pageSize.ToString());

			// Act
			var response = await _httpClient.GetAsync(url);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
			var result = response.Content.ReadFromJsonAsync<IEnumerable<CatalogItemReadDTO>>().Result;
			_mapper.Map<IEnumerable<CatalogItem>>(result).Should().HaveCount(expected.Count).And
			.BeEquivalentTo(expected, cfg => cfg
			.Excluding(x => x.CatalogBrand.CatalogItems).Excluding(x => x.CatalogType.CatalogItems)
			.Excluding(x => x.CatalogBrand.CreatedOn).Excluding(x => x.CatalogType.CreatedOn)
			.Excluding(x => x.CatalogBrand.CreatedBy).Excluding(x => x.CatalogType.CreatedBy)
			.Excluding(x => x.CatalogBrand.UpdatedOn).Excluding(x => x.CatalogType.UpdatedOn)
			.Excluding(x => x.CatalogBrand.UpdatedBy).Excluding(x => x.CatalogType.UpdatedBy)
			.Excluding(x => x.CreatedOn).Excluding(x => x.CreatedOn)
			.Excluding(x => x.UpdatedOn).Excluding(x => x.UpdatedBy));
		}

		#endregion

		#region GetByID

		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		[TestCase(5)]
		[Test]
		public async Task GetByID_WhithCatalogItemsInDBAndValidID_ReturnsOKStatusAndElementSatisfiyingThatID(int id) {
			// Arrange
			using var scope = _serviceFactory?.CreateScope();
			var context = scope?.ServiceProvider?.GetRequiredService<CatalogContext>();
			_ = context.CatalogBrands.ToList();
			_ = context.CatalogTypes.ToList();
			var expected = context?.CatalogItems.Include(x => x.CatalogBrand)
												.Include(x => x.CatalogType).Single(x => x.CatalogItemID == id);
			var url = APIRoutes.Items.GetByID.Replace("{id}", id.ToString());

			// Act
			var response = await _httpClient.GetAsync(url);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
			var result = response.Content.ReadFromJsonAsync<CatalogItemReadDTO>().Result;
			 _mapper.Map<CatalogItem>(result).Should().BeEquivalentTo(expected, cfg => cfg
			.Excluding(x => x.CatalogBrand.CatalogItems).Excluding(x => x.CatalogType.CatalogItems)
			.Excluding(x => x.CatalogBrand.CreatedOn).Excluding(x => x.CatalogType.CreatedOn)
			.Excluding(x => x.CatalogBrand.CreatedBy).Excluding(x => x.CatalogType.CreatedBy)
			.Excluding(x => x.CatalogBrand.UpdatedOn).Excluding(x => x.CatalogType.UpdatedOn)
			.Excluding(x => x.CatalogBrand.UpdatedBy).Excluding(x => x.CatalogType.UpdatedBy)
			.Excluding(x => x.CreatedOn).Excluding(x => x.CreatedOn)
			.Excluding(x => x.UpdatedOn).Excluding(x => x.UpdatedBy));
		}

		[TestCase(31)]
		[TestCase(32)]
		[TestCase(33)]
		[TestCase(34)]
		[TestCase(35)]
		[Test]
		public async Task GetByID_WhithCatalogItemsInDBAndInvalidID_ReturnsBadRequestStatus(int id) {
			// Arrange
			var url = Items.GetByID.Replace("{id}", id.ToString());

			// Act
			var response = await _httpClient.GetAsync(url);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}

		#endregion

		#region Create

		[Test]
		public async Task Create_WhithCatalogItemsInDBAndValidID_ReturnsAcceptedStatusRemovedElementsAreMissingFromDB() {
			// Arrange
			var random = new Random();
			using var scope = _serviceFactory?.CreateScope();
			var context = scope?.ServiceProvider?.GetRequiredService<CatalogContext>();
			var objectToCreate = NAuto.AutoBuild<CatalogItemCreateSingleDTO>()
								.Construct()
								.With(x => x.Name = $"Test Name {random.Next()}")
								.With(x => x.Description = $"Test Description {random.Next()}")
								.With(x => x.CatalogBrandId = context.CatalogBrands.First().CatalogBrandID)
								.With(x => x.CatalogTypeId = context.CatalogTypes.First().CatalogTypeID)
								.Build();

			var createURL = Items.Create;

			// Act
			var createResponse = await _httpClient.PostAsJsonAsync(createURL, objectToCreate);

			// Assert
			var createResult = await createResponse.Content.ReadFromJsonAsync<CatalogItemReadDTO>();
			var createdObject = _mapper.Map<CatalogItem>(createResult);
			createdObject.Should().BeEquivalentTo(objectToCreate);

			// Cleanup
			var deleteURL = Items.Delete.Replace("{id}", createdObject.CatalogItemID.ToString());
			_httpClient.DeleteAsync(deleteURL);
		}

		#endregion

		#region Delete
		[Test]
		public async Task Delete_WhithCatalogItemsInDBAndValidID_ReturnsAcceptedStatusRemovedElementsAreMissingFromDB() {
			// Arrange
			var random = new Random();
			using var scope = _serviceFactory?.CreateScope();
			var context = scope?.ServiceProvider?.GetRequiredService<CatalogContext>();
			var objectToCreate = NAuto.AutoBuild<CatalogItemCreateSingleDTO>()
										.Construct()
										.With(x => x.Name = $"Test Name {random.Next()}")
										.With(x => x.Description = $"Test Description {random.Next()}")
										.With(x => x.CatalogBrandId = context.CatalogBrands.First().CatalogBrandID)
										.With(x => x.CatalogTypeId = context.CatalogTypes.First().CatalogTypeID)
										.Build();
			var createResponse = await _httpClient.PostAsJsonAsync(Items.Create, objectToCreate);
			var createdObject = await createResponse.Content.ReadFromJsonAsync<CatalogItemReadDTO>();

			var deleteURL = Items.Delete.Replace("{id}", createdObject.CatalogItemID.ToString());

			// Act
			var deleteResponse = _httpClient.DeleteAsync(deleteURL).Result;

			// Assert
			deleteResponse.StatusCode.Should().Be(HttpStatusCode.Accepted);
			context?.CatalogItems.Find(createdObject.CatalogItemID).Should().BeNull();
		}
		#endregion
	}
}
 