using AutoMapper;
using Catalog.API.DTOs.CatalogItem;
using Catalog.Core.Models;
using Catalog.DataAccess;
using Catalog.Infrastructure.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Catalog.API.Controllers {
	[ApiController]
	[Route("api/v1/catalog/[controller]")]
	public class ItemsController : ControllerBase {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ILogger<ItemsController> _logger;
		private readonly CatalogOptions _catalogOptions;
		
		public ItemsController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ItemsController> logger, IOptions<CatalogOptions> catalogOptions) {
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_logger = logger;
			_catalogOptions = catalogOptions.Value;
		}

		#region CatalogItem
		[HttpGet("{id}", Name = "GetItemAsync")]
		[ProducesResponseType(type: typeof(CatalogItemReadDTO), statusCode: (int) HttpStatusCode.OK)]
		[ProducesResponseType(statusCode: (int) HttpStatusCode.BadRequest)]
		public async Task<IActionResult> GetItemAsync(int id) {
			_logger.LogInformation($"--> Returning Catalog w/ Id = {id}");

			if (!await _unitOfWork.CatalogItemRepository.ExistsAsync(id)) {
				return BadRequest($"Inexistent CatalogItem w/ Id = '{id}'");
			}

			CatalogItem catalogitem = await _unitOfWork.CatalogItemRepository.GetAsync(id);

			return Ok(_mapper.Map<CatalogItemReadDTO>(catalogitem));
		}

		// GET api/v1/[controller]/items[?pageSize=3&pageIndex=10]
		[HttpGet(Name = "GetItemSAsync")]
		[ProducesResponseType(type: typeof(IEnumerable<CatalogItemReadDTO>), statusCode: (int) HttpStatusCode.OK)]
		[ProducesResponseType(statusCode: (int) HttpStatusCode.BadRequest)]
		public async Task<IActionResult> GetItemsAsync([FromQuery] byte pageSize = 10, [FromQuery] byte pageIndex = 0) {
			_logger.LogInformation("--> Returning all CatalogItems");

			IEnumerable<CatalogItem> catalogItems = await this._unitOfWork.CatalogItemRepository.GetAllAsync(pageSize, pageIndex);

			return Ok(_mapper.Map<IEnumerable<CatalogItemReadDTO>>(catalogItems));
		}

		[HttpPost]
		[ProducesResponseType(type: typeof(CreatedAtRouteResult), statusCode: (int) HttpStatusCode.Created)]
		[ProducesResponseType(type: typeof(BadRequestResult), statusCode: (int) HttpStatusCode.BadRequest)]
		public async Task<IActionResult> CreateItemAsync([FromBody] CatalogItemCreateDTO catalogItemCreateDTO) {
			_logger.LogInformation($"Creating CatatlogItem: {JsonSerializer.Serialize<CatalogItemCreateDTO>(catalogItemCreateDTO)}");

			CatalogItem catalogItem = _mapper.Map<CatalogItem>(catalogItemCreateDTO);

			if (await _unitOfWork.CatalogItemRepository.NameExistsAsync(catalogItem)) {
				_logger.LogError($"A {typeof(CatalogItem)} w/ Name = {catalogItem.Name} already exists.");
				return BadRequest($"A {typeof(CatalogItem)} w/ Name = {catalogItem.Name} already exists.");
			}

			await _unitOfWork.CatalogItemRepository.AddAsync(catalogItem);
			await _unitOfWork.CompleteAsync();
			CatalogItemReadDTO catalogItemReadDTO = _mapper.Map<CatalogItemReadDTO>(catalogItem);
			return CreatedAtRoute(nameof(GetItemAsync), new { id = catalogItem.Id.ToString() }, catalogItemReadDTO);
		}

		#endregion

		#region Private Methods

		protected async Task Dispose() {
			await _unitOfWork.DisposeAsync();
		}

		#endregion
	}
}
