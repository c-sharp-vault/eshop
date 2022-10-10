using AutoMapper;
using Catalog.API.Controllers;
using Catalog.API.DTOs.CatalogBrand;
using Catalog.API.DTOs.CatalogItem;
using Catalog.API.DTOs.CatalogType;
using Catalog.API.Profiles;
using Catalog.Core.Models;
using Catalog.DataAccess;
using Catalog.DataAccess.Repositories;
using Catalog.Infrastructure.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Catalog.UnitTests.Catalog.API.Controllers {
	[TestFixture]
	public class CatalogControllerTest {
		private List<CatalogItem> _catalogItemList;
		private List<CatalogItem> _anotherCatalogItemList;
		private MapperConfiguration _mapperConfiguration = new MapperConfiguration(x => x.AddProfile(new CatalogItemProfile()));
		private IMapper _mapper = new Mapper(new MapperConfiguration(x => x.AddProfile(new CatalogItemProfile())));
		private Mock<ILogger<ItemsController>> _mockedLogger;
		private Mock<ICatalogItemRepository> _mockedCatalogItemRepository;
		private Mock<IUnitOfWork> _mockedUnitOfWork;
		private Mock<CatalogOptions> _mockedCatalogOptions;
		private Mock<IOptions<CatalogOptions>> _mockedOptions;

		[SetUp]
		public void Setup() {
			_catalogItemList = GetExpectedCatalogItemList();
			_catalogItemList.ElementAt(0).Id = 1;
			_catalogItemList.ElementAt(0).CatalogBrandId = 1;
			_catalogItemList.ElementAt(0).CatalogBrand.Id = 1;
			_catalogItemList.ElementAt(0).CatalogTypeId = 1;
			_catalogItemList.ElementAt(0).CatalogType.Id = 1;

			_catalogItemList.ElementAt(1).Id = 2;
			_catalogItemList.ElementAt(1).CatalogBrandId = 2;
			_catalogItemList.ElementAt(1).CatalogBrand.Id = 2;
			_catalogItemList.ElementAt(1).CatalogTypeId = 2;
			_catalogItemList.ElementAt(1).CatalogType.Id = 2;

			_catalogItemList.ElementAt(2).Id = 3;
			_catalogItemList.ElementAt(2).CatalogBrandId = 3;
			_catalogItemList.ElementAt(2).CatalogBrand.Id = 3;
			_catalogItemList.ElementAt(2).CatalogTypeId = 3;
			_catalogItemList.ElementAt(2).CatalogType.Id = 3;

			_anotherCatalogItemList = GetAnotherCatalogItemList();
			_anotherCatalogItemList.ElementAt(0).Id = 4;
			_anotherCatalogItemList.ElementAt(0).CatalogBrandId = 4;
			_anotherCatalogItemList.ElementAt(0).CatalogBrand.Id = 4;
			_anotherCatalogItemList.ElementAt(0).CatalogTypeId = 4;
			_anotherCatalogItemList.ElementAt(0).CatalogType.Id = 4;

			_anotherCatalogItemList.ElementAt(1).Id = 5;
			_anotherCatalogItemList.ElementAt(1).CatalogBrandId = 5;
			_anotherCatalogItemList.ElementAt(1).CatalogBrand.Id = 5;
			_anotherCatalogItemList.ElementAt(1).CatalogTypeId = 5;
			_anotherCatalogItemList.ElementAt(1).CatalogType.Id = 5;

			_anotherCatalogItemList.ElementAt(2).Id = 6;
			_anotherCatalogItemList.ElementAt(2).CatalogBrandId = 6;
			_anotherCatalogItemList.ElementAt(2).CatalogBrand.Id = 6;
			_anotherCatalogItemList.ElementAt(2).CatalogTypeId = 6;
			_anotherCatalogItemList.ElementAt(2).CatalogType.Id = 6;

			_mockedCatalogItemRepository = new Mock<ICatalogItemRepository>();
			_mockedUnitOfWork = new Mock<IUnitOfWork>();
			_mockedUnitOfWork.SetupGet<ICatalogItemRepository>(x => x.CatalogItemRepository)
							 .Returns(_mockedCatalogItemRepository.Object);
			_mockedLogger = new Mock<ILogger<ItemsController>>();
			_mockedCatalogOptions = new Mock<CatalogOptions>();
			_mockedOptions = new Mock<IOptions<CatalogOptions>>();
			_mockedOptions.SetupGet<CatalogOptions>(x => x.Value).Returns(_mockedCatalogOptions.Object);
		}

		[Test]
		public async Task GetItemsAsync_NoneArguments_ReturnsMatchingItems() {
			// Arrange
			_mockedCatalogItemRepository.Setup<Task<IEnumerable<CatalogItem>>>(x => x.GetAllAsync(It.IsAny<byte>(), It.IsAny<byte>()))
										.Returns(Task.FromResult((IEnumerable<CatalogItem>) _catalogItemList));
			ItemsController catalogController = new ItemsController(unitOfWork: _mockedUnitOfWork.Object, 
																		mapper: _mapper, 
																		logger: _mockedLogger.Object, 
																		catalogOptions: _mockedOptions.Object);

			// Act
			IActionResult actionResult =  await catalogController.GetItemsAsync();
			OkObjectResult okObjectResult = (OkObjectResult) actionResult;
			IEnumerable<CatalogItemReadDTO> catalogItemsResult = (IEnumerable<CatalogItemReadDTO>) okObjectResult.Value;

			// Assert
			Assert.That(_catalogItemList.Count(), Is.EqualTo(catalogItemsResult.Count()), $"IEnumerable<CatalogItemReadDTO>.Count() mismatch");
			TestCatalogItemListsEquality(_mapper.Map<IEnumerable<CatalogItem>>(catalogItemsResult));
		}

		[Test]
		public async Task GetItemsAsync_NoneArguments_ReturnsNonMatchingItems() {
			// Arrange
			_mockedCatalogItemRepository.Setup<Task<IEnumerable<CatalogItem>>>(x => x.GetAllAsync(It.IsAny<byte>(), It.IsAny<byte>()))
										.Returns(Task.FromResult((IEnumerable<CatalogItem>) _anotherCatalogItemList));
			ItemsController catalogController = new ItemsController(unitOfWork: _mockedUnitOfWork.Object, mapper: _mapper, logger: _mockedLogger.Object, catalogOptions: _mockedOptions.Object);

			// Act
			IActionResult actionResult = await catalogController.GetItemsAsync();

			// Assert
			Assert.IsAssignableFrom(typeof(OkObjectResult), actionResult);
			OkObjectResult okObjectResult = (OkObjectResult) actionResult;

			Assert.IsAssignableFrom(typeof(List<CatalogItemReadDTO>), okObjectResult.Value);
			List<CatalogItemReadDTO> catalogItemReadDTOs = (List<CatalogItemReadDTO>) okObjectResult.Value;

			Assert.That(_catalogItemList.Count(), !Is.EqualTo(catalogItemReadDTOs.Count()), $"IEnumerable<CatalogItemReadDTO>.Count() mismatch");
			TestCatalogItemListsInequality(_mapper.Map<IEnumerable<CatalogItem>>(catalogItemReadDTOs));

		}

		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		public async Task GetItemAsync_WithValidIds_ReturnsMatchingItems(int id) {
			// Arrange
			IEnumerable<int> idsToSelect = _catalogItemList.Select(x => x.Id);
			_mockedCatalogItemRepository.Setup(x => x.ExistsAsync(It.IsAny<int>()))
										.Returns(Task.FromResult(_catalogItemList.Find(x => x.Id == id) != null));
			_mockedCatalogItemRepository.Setup(x => x.GetAsync(It.IsAny<int>()))
										.Returns(Task.FromResult(_catalogItemList.Find(x => idsToSelect.Contains(x.Id))));
			ItemsController catalogController = new ItemsController(unitOfWork: _mockedUnitOfWork.Object, mapper: _mapper, logger: _mockedLogger.Object, catalogOptions: _mockedOptions.Object);

			// Act
			IActionResult actionResult = await catalogController.GetItemAsync(id);

			// Assert
			Assert.IsAssignableFrom(typeof(OkObjectResult), actionResult);
			OkObjectResult okObjectResult = (OkObjectResult) actionResult;

			Assert.IsAssignableFrom(typeof(CatalogItemReadDTO), okObjectResult.Value);
			CatalogItemReadDTO catalogItemReadDTO = (CatalogItemReadDTO) okObjectResult.Value;

			TestCatalogItemListsEquality(new List<CatalogItem>() { _mapper.Map<CatalogItem>(catalogItemReadDTO) }, _catalogItemList.Where(x => x.Id == catalogItemReadDTO.Id));
		}

		[TestCase(4)]
		[TestCase(5)]
		[TestCase(6)]
		public async Task GetItemAsync_WithInvalidIds_ReturnsBadRequest(int id) {
			// Arrange
			IEnumerable<int> idsToSelect = _catalogItemList.Select(x => x.Id);
			_mockedCatalogItemRepository.Setup(x => x.ExistsAsync(It.IsAny<int>()))
										.Returns(Task.FromResult(_catalogItemList.Find(x => x.Id == id) != null));
			_mockedCatalogItemRepository.Setup(x => x.GetAsync(It.IsAny<int>()))
										.Returns(Task.FromResult(_catalogItemList.Find(x => idsToSelect.Contains(x.Id))));
			ItemsController catalogController = new ItemsController(unitOfWork: _mockedUnitOfWork.Object, mapper: _mapper, logger: _mockedLogger.Object, catalogOptions: _mockedOptions.Object);

			// Act
			IActionResult actionResult = await catalogController.GetItemAsync(id);

			// Assert
			Assert.IsAssignableFrom(typeof(BadRequestObjectResult), actionResult);
		}

		private List<CatalogItem> GetExpectedCatalogItemList() {
			return new List<CatalogItem>() {
				new CatalogItem(name: ".NET Bot Black Hoodie", description: "An excellent .NET Bot black hoodie", price: 19.5m, catalogType: new CatalogType(type: "T-Shirt"), catalogBrand: new CatalogBrand(brand: ".NET")),
				new CatalogItem(name: "Roslyn Red Pin", description: "A beautiful Roslyn red pin", price: 8.5m, catalogType: new CatalogType(type: "Pin"), catalogBrand: new CatalogBrand(brand: "C#")),
				new CatalogItem(name: "Azure Mug", description: "A beautiful mug with the Azure blue logo", price: 12m, catalogType: new CatalogType(type: "Mug"), catalogBrand: new CatalogBrand(brand: "Azure")),
			};
		}

		private List<CatalogItem> GetAnotherCatalogItemList() {
			return new List<CatalogItem>() {
				new CatalogItem(name: ".NET Bot Black Hoodie+", description: "An excellent .NET Bot black hoodie+", price: 19m, catalogType: new CatalogType(type: "T-Shirt+"), catalogBrand: new CatalogBrand(brand: ".NET+")),
				new CatalogItem(name: "Roslyn Red Pin+", description: "A beautiful Roslyn red pin+", price: 8m, catalogType: new CatalogType(type: "Pin+"), catalogBrand: new CatalogBrand(brand: "C#+")),
				new CatalogItem(name: "Azure Mug+", description: "A beautiful mug with the Azure blue logo+", price: 12.1m, catalogType: new CatalogType(type: "Mug+"), catalogBrand: new CatalogBrand(brand: "Azure+")),
				new CatalogItem(name: "Kudu Purple Hoodie+", description: "A beautiful Kudu Purple Hoodie+", price: 10.5m, catalogType: new CatalogType(type: "T-Shirt+"), catalogBrand: new CatalogBrand(brand: "Other+")),
			};
		}

		private void TestCatalogItemListsEquality(IEnumerable<CatalogItem> catalogItemsResult, IEnumerable<CatalogItem> catalogItemsFiltered = null) {
			IEnumerable<CatalogItem> catalogItems = catalogItemsFiltered != null? catalogItemsFiltered : _catalogItemList;
			catalogItems.Select((element, index) => new { Element = element, Index = index }).ToList().ForEach(obj => {
				Assert.That(catalogItemsResult.ElementAt(obj.Index).Name, Is.EquivalentTo(obj.Element.Name), $"{nameof(CatalogItem)}.{nameof(CatalogItem.Name)} mismatch at index {obj.Index} of List<CatalogItem>");
				Assert.That(catalogItemsResult.ElementAt(obj.Index).Description, Is.EqualTo(obj.Element.Description), $"{nameof(CatalogItem)}.{nameof(CatalogItem.Description)} mismatch at index {obj.Index} of List<CatalogItem>");
				Assert.That(catalogItemsResult.ElementAt(obj.Index).Price, Is.EqualTo(obj.Element.Price), $"{nameof(CatalogItem)}.{nameof(CatalogItem.Price)} mismatch at index {obj.Index} of List<CatalogItem>");
				Assert.That(catalogItemsResult.ElementAt(obj.Index).CatalogType.Type, Is.EqualTo(obj.Element.CatalogType.Type), $"{nameof(CatalogType)}.{nameof(CatalogType.Type)} mismatch at index {obj.Index} of List<CatalogItem>");
				Assert.That(catalogItemsResult.ElementAt(obj.Index).CatalogBrand.Brand, Is.EqualTo(obj.Element.CatalogBrand.Brand), $"{nameof(CatalogBrand)}.{nameof(CatalogBrand.Brand)} mismatch at index {obj.Index} of List<CatalogItem>");
			});
		}

		private void TestCatalogItemListsInequality(IEnumerable<CatalogItem> catalogItemsResult) {
			_catalogItemList.Select((element, index) => new { Element = element, Index = index }).ToList().ForEach(obj => {
				Assert.That(catalogItemsResult.ElementAt(obj.Index).Name, !Is.EquivalentTo(obj.Element.Name), $"{nameof(CatalogItem)}.{nameof(CatalogItem.Name)} match at index {obj.Index} of List<CatalogItem>");
				Assert.That(catalogItemsResult.ElementAt(obj.Index).Description, !Is.EqualTo(obj.Element.Description), $"{nameof(CatalogItem)}.{nameof(CatalogItem.Description)} match at index {obj.Index} of List<CatalogItem>");
				Assert.That(catalogItemsResult.ElementAt(obj.Index).Price, !Is.EqualTo(obj.Element.Price), $"{nameof(CatalogItem)}.{nameof(CatalogItem.Price)} match at index {obj.Index} of List<CatalogItem>");
				Assert.That(catalogItemsResult.ElementAt(obj.Index).CatalogType.Type, !Is.EqualTo(obj.Element.CatalogType.Type), $"{nameof(CatalogType)}.{nameof(CatalogType.Type)} match at index {obj.Index} of List<CatalogItem>");
				Assert.That(catalogItemsResult.ElementAt(obj.Index).CatalogBrand.Brand, !Is.EqualTo(obj.Element.CatalogBrand.Brand), $"{nameof(CatalogBrand)}.{nameof(CatalogBrand.Brand)} match at index {obj.Index} of List<CatalogItem>");
			});
		}
	}
}