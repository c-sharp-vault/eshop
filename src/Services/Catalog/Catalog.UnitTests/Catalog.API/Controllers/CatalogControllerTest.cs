using AutoMapper;
using Catalog.API.Controllers;
using Catalog.API.DTOs.CatalogItem;
using Catalog.API.Profiles;
using Catalog.Core.Models;
using Catalog.DataAccess;
using Catalog.DataAccess.Repositories;
using Catalog.Infrastructure.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Amido.NAuto;

namespace Catalog.UnitTests.Catalog.API.Controllers
{
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

		private List<CatalogItem> GetExpectedCatalogItemList() => NAuto.GetRandomList<CatalogItem>(x => x.Id, 3);

		private List<CatalogItem> GetAnotherCatalogItemList() => NAuto.GetRandomList<CatalogItem>(x => x.Id, 3);

		private void TestCatalogItemListsEquality(IEnumerable<CatalogItem> catalogItemsResult, IEnumerable<CatalogItem> catalogItemsFiltered = null) {
			IEnumerable<CatalogItem> catalogItems = catalogItemsFiltered != null? catalogItemsFiltered : _catalogItemList;
			catalogItems.Should().BeEquivalentTo(catalogItemsResult, cfg => cfg.Excluding(x => x.CreatedBy).Excluding(x => x.CreatedOn)
				.Excluding(x => x.UpdatedBy).Excluding(x => x.UpdatedOn).Excluding(x => x.CatalogBrandId).Excluding(x => x.CatalogTypeId)
				.Excluding(x => x.CatalogBrand.CreatedOn).Excluding(x => x.CatalogType.CreatedOn));
		}

		private void TestCatalogItemListsInequality(IEnumerable<CatalogItem> catalogItemsResult) =>
			_catalogItemList.Should().NotBeEquivalentTo(catalogItemsResult, cfg => cfg.Excluding(x => x.CreatedBy).Excluding(x => x.CreatedOn)
				.Excluding(x => x.UpdatedBy).Excluding(x => x.UpdatedOn).Excluding(x => x.CatalogBrandId).Excluding(x => x.CatalogTypeId)
				.Excluding(x => x.CatalogBrand.CreatedOn).Excluding(x => x.CatalogType.CreatedOn));
	}
}