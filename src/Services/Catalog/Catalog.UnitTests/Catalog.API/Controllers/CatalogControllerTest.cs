using AutoMapper;
using Catalog.API.Controllers;
using Catalog.DataAccess.DTOs.CatalogItem;
using Catalog.API.Profiles;
using Catalog.Infrastructure.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Amido.NAuto;
using Catalog.DataAccess.Managers.CatalogItems;
using Catalog.DataAccess.Managers.CatalogItems.Messages;
using Catalog.Core.Models;

namespace Catalog.UnitTests.Catalog.API.Controllers {

	[TestFixture]
	public class CatalogControllerTest {

		private readonly IMapper _mapper;
		private readonly Mock<ILogger<CatalogItemsController>> _mockedLogger;
		private Mock<ICatalogItemManager> _mockedManager;
		private Mock<IOptions<CatalogOptions>> _mockedOptions;

		public CatalogControllerTest() {
			_mapper = new Mapper(new MapperConfiguration(x => x.AddProfile(new CatalogItemProfile())));
			_mockedLogger = new Mock<ILogger<CatalogItemsController>>();
			_mockedManager = new Mock<ICatalogItemManager>();
			_mockedOptions = new Mock<IOptions<CatalogOptions>>();
		}

		private IList<CatalogItemReadDTO> CatalogItems {
			get => NAuto.GetRandomList<CatalogItemReadDTO>(x => x.CatalogItemID, 3);
		}

		[SetUp]
		public void Setup() {

		}

		[Test]
		public async Task GetRangeAsync_WithValidArguments_ReturnsExpectedCollection() {

			// Arrange
			IEnumerable<CatalogItemReadDTO> expectedCatalogItems = CatalogItems;
			_mockedManager.Setup(_ => _.GetRangeAsync(It.IsAny<GetRangeRequest>()))
									 .ReturnsAsync(new GetRangeResponse {
										 CatalogItems = expectedCatalogItems,
										 Success = true
									 });
			CatalogItemsController catalogController = new CatalogItemsController(catalogItemManager: _mockedManager.Object,
																				  mapper: _mapper,
																				  logger: _mockedLogger.Object,
																				  catalogOptions: _mockedOptions.Object);

			// Act
			IActionResult actionResult = await catalogController.GetRangeAsync(new GetRangeRequest {
				PageSize = 10,
				PageIndex = 1,
				IncludeNested = true,
			});

			// Assert
			Assert.IsAssignableFrom(typeof(OkObjectResult), actionResult);
			OkObjectResult okObjectResult = (OkObjectResult) actionResult;

			Assert.IsAssignableFrom(typeof(GetRangeResponse), okObjectResult.Value);
			GetRangeResponse getRangeResponse = (GetRangeResponse) okObjectResult.Value;

			IEnumerable<CatalogItemReadDTO> actualCatalogItems = getRangeResponse.CatalogItems;

			actualCatalogItems.Count().Should().Be(actualCatalogItems.Count());
			actualCatalogItems.Should().BeEquivalentTo(expectedCatalogItems);
		}

		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		public async Task GetSingleAsync_WithValidIds_ReturnsMatchingItems(int id) {
			// Arrange
			var expectedCatalogItem = CatalogItems.Single(x => x.CatalogItemID == id);
			_mockedManager.Setup(x => x.GetSingleAsync(It.IsAny<GetSingleRequest>()))
									   .ReturnsAsync(new GetSingleResponse {
											CatalogItem = expectedCatalogItem
									   });

			CatalogItemsController catalogController = new CatalogItemsController(catalogItemManager: _mockedManager.Object, 
																				  mapper: _mapper, 
																				  logger: _mockedLogger.Object, 
																				  catalogOptions: _mockedOptions.Object);

			// Act
			IActionResult actionResult = await catalogController.GetSingleAsync(new GetSingleRequest { CatalogItemID = id });

			// Assert
			Assert.IsAssignableFrom(typeof(OkObjectResult), actionResult);
			OkObjectResult okObjectResult = (OkObjectResult) actionResult;

			Assert.IsAssignableFrom(typeof(GetSingleResponse), okObjectResult.Value);
			GetSingleResponse getSingleResponse = (GetSingleResponse) okObjectResult.Value;

			CatalogItemReadDTO actualCatalogItem = getSingleResponse.CatalogItem;
			actualCatalogItem.Should().BeEquivalentTo(expectedCatalogItem);
		}

		[TestCase(4)]
		[TestCase(5)]
		[TestCase(6)]
		public async Task GetSingleAsync_WithInvalidIds_ReturnsNotFoundResult(int id) {
			// Arrange
			var expectedErrorMessage = new string[] { $"{nameof(CatalogItem)} entity with ID = {id} not found." };
			_mockedManager.Setup(x => x.GetSingleAsync(It.IsAny<GetSingleRequest>()))
									   .ReturnsAsync(new GetSingleResponse {
										   Success = false,
										   ErrorMessages = expectedErrorMessage
									   });

			CatalogItemsController catalogController = new CatalogItemsController(catalogItemManager: _mockedManager.Object,
																				  mapper: _mapper,
																				  logger: _mockedLogger.Object,
																				  catalogOptions: _mockedOptions.Object);

			// Act
			IActionResult actionResult = await catalogController.GetSingleAsync(new GetSingleRequest { CatalogItemID = id });

			// Assert
			Assert.IsAssignableFrom(typeof(BadRequestObjectResult), actionResult);
			BadRequestObjectResult badRequestObjectResult = (BadRequestObjectResult) actionResult;

			Assert.IsAssignableFrom(typeof(GetSingleResponse), badRequestObjectResult.Value);
			GetSingleResponse getSingleResponse = (GetSingleResponse)badRequestObjectResult.Value;

			getSingleResponse.Success.Should().BeFalse();
			getSingleResponse.ErrorMessages.Should().BeEquivalentTo(expectedErrorMessage);
		}
	}
}