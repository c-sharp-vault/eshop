using Catalog.Core.Models;
using Catalog.DataAccess.DTOs.CatalogItem;
using Catalog.DataAccess.Managers.CatalogItems.Messages;
using Catalog.Infrastructure.Extensions.Enumerable;
using Catalog.IntegrationTests.Initialization;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;


namespace Catalog.IntegrationTests.Services {
	public class CatalogItemEndpointTests : APIEndpointTestBase<CatalogItemEndpointData> {

		#pragma warning disable NUnit2005

		[Test, Order(1)]
		public async Task ReadRangeCatalogItem() {
			// Arrange
			var queryParams = "?pageSize=4&pageIndex=1&includeNested=false";
			var catalogItems = TestData.CatalogItems.ToList();

			// Act
			var response = await GETAsync<GetRangeResponse>(TestURLs.CatalogItem.ReadRangeEndpoint, queryParams);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.IsNotNull(response.CatalogItems);
			Assert.AreEqual(4, response.CatalogItems.ToList().Count);

			foreach (var (item, index) in response.CatalogItems.WithIndex()) {
				CheckEquality(catalogItems[index], item);
			}
		}

		[Test, Order(2)]
		public async Task ReadSingleCatalogItem() {
			// Arrange
			var catalogItem = TestData.CatalogItems.First();

			// Act
			var response = await GETAsync<GetSingleResponse>(TestURLs.CatalogItem.ReadSingleEndpoint, $"/{catalogItem.CatalogItemID}");

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.IsNotNull(response.CatalogItem);
			CheckEquality(catalogItem, response.CatalogItem);
		}

		[Test, Order(3)]
		public async Task CreateSingleCatalogItem() {
			// Arrange
			var request = new CatalogItemCreateSingleDTO {
				Name = "New Item #1",
				Description = "New Item #1",
				Price = 19.5m,
				PictureFileName = "1.png",
				CatalogTypeId = 1,
				CatalogBrandId = 1,
				AvailableStock = 100,
				RestockThreshold = 10,
				MaxStockThreshold = 1,
				OnReorder = false
			};

			// Act
			var createCatalogItemResponse = await POSTAsync<CatalogItemCreateSingleDTO, CreateSingleResponse>(TestURLs.CatalogItem.CreateSingleEndpoint, request);

			// Assert
			Assert.IsNotNull(createCatalogItemResponse);
			Assert.IsNotNull(createCatalogItemResponse.CatalogItem);
			Assert.IsTrue(createCatalogItemResponse.Success);
		}

		[Test, Order(4)]
		public async Task UpdateSingleCatalogItem() {
			// Arrange
			var catalogItem = TestData.CatalogItems.First();
			var request = new CatalogItemUpdateDTO {
				CatalogItemID = catalogItem.CatalogItemID,
				Name = "New Name",
				Description = "New Description",
				Price = 99.99m,
				PictureFileName = "abc.jpg",
				CatalogTypeID = 3,
				CatalogBrandID = 3,
				AvailableStock = 999,
				RestockThreshold = 9999,
				MaxStockThreshold = 9999,
				OnReorder = true
			};

			// Act
			var response = await PUTAsync<CatalogItemUpdateDTO, UpdateSingleResponse>(TestURLs.CatalogItem.UpdateSingleEndpoint, request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.IsNotNull(response.CatalogItem);
			CheckEquality(request, response.CatalogItem);
		}


		[Test, Order(5)]
		public async Task DeleteSingleCatalogItem() {
			// Arrange
			var queryParams = "?pageSize=4&pageIndex=1&includeNested=false";
			var catalogItem = TestData.CatalogItems.First();

			// Act
			var response = await DELETEAsync<RemoveSingleResponse>(TestURLs.CatalogItem.DeleteSingleEndpoint, $"/{catalogItem.CatalogItemID}");

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);

			var catalogItems = GETAsync<GetRangeResponse>(TestURLs.CatalogItem.ReadRangeEndpoint, queryParams).Result.CatalogItems;
			Assert.IsFalse(catalogItems.ToList().Any(x => x.CatalogItemID == catalogItem.CatalogItemID));

		}

		#region Helpers

		private void CheckEquality(CatalogItem entity, CatalogItemReadDTO readDTO) {
			Assert.AreEqual(entity.Name, readDTO.Name);
			Assert.AreEqual(entity.Description, readDTO.Description);
			Assert.AreEqual(entity.Price, readDTO.Price);
			Assert.AreEqual(entity.PictureFileName, readDTO.PictureFileName);
			Assert.AreEqual(entity.AvailableStock, readDTO.AvailableStock);
			Assert.AreEqual(entity.RestockThreshold, readDTO.RestockThreshold);
			Assert.AreEqual(entity.MaxStockThreshold, readDTO.MaxStockThreshold);
			Assert.AreEqual(entity.CatalogTypeID, readDTO.CatalogTypeID);
			Assert.AreEqual(entity.CatalogBrandID, readDTO.CatalogBrandID);
			Assert.AreEqual(entity.OnReorder, readDTO.OnReorder);
		}

		private void CheckEquality(CatalogItemUpdateDTO updateDTO, CatalogItemReadDTO readDTO) {
			Assert.AreEqual(updateDTO.Name, readDTO.Name);
			Assert.AreEqual(updateDTO.Description, readDTO.Description);
			Assert.AreEqual(updateDTO.Price, readDTO.Price);
			Assert.AreEqual(updateDTO.PictureFileName, readDTO.PictureFileName);
			Assert.AreEqual(updateDTO.AvailableStock, readDTO.AvailableStock);
			Assert.AreEqual(updateDTO.RestockThreshold, readDTO.RestockThreshold);
			Assert.AreEqual(updateDTO.MaxStockThreshold, readDTO.MaxStockThreshold);
			Assert.AreEqual(updateDTO.CatalogTypeID, readDTO.CatalogTypeID);
			Assert.AreEqual(updateDTO.CatalogBrandID, readDTO.CatalogBrandID);
			Assert.AreEqual(updateDTO.OnReorder, readDTO.OnReorder);
		}

		#endregion
	}
}
