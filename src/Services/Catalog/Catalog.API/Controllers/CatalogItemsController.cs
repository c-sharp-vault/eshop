using AutoMapper;
using Catalog.DataAccess;
using Catalog.DataAccess.DTOs.CatalogItem;
using Catalog.DataAccess.Managers.CatalogItems;
using Catalog.DataAccess.Managers.CatalogItems.Messages;
using Catalog.Infrastructure.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers {

	[ApiController]
	[Route("api/v1/catalog/items")]
	public class CatalogItemsController : ControllerBase, ICatalogItemsController {

		#region Fields

		private readonly ICatalogItemManager _catalogItemManager;
		private readonly IMapper _mapper;
		private readonly ILogger<CatalogItemsController> _logger;
		private readonly CatalogOptions _catalogOptions;

		#endregion

		#region Constructors

		public CatalogItemsController(ICatalogItemManager catalogItemManager, IMapper mapper,
			ILogger<CatalogItemsController> logger, IOptions<CatalogOptions> catalogOptions) {
			_catalogItemManager = catalogItemManager;
			_mapper = mapper;
			_logger = logger;
			_catalogOptions = catalogOptions.Value;
		}

		#endregion

		#region Actions

		[HttpGet("{id}", Name = "GetSingleAsync")]
		[ProducesResponseType(type: typeof(GetSingleResponse), statusCode: (int)HttpStatusCode.OK)]
		[ProducesResponseType(type: typeof(GetSingleResponse), statusCode: (int)HttpStatusCode.BadRequest)]
		public async Task<IActionResult> GetSingleAsync([FromRoute] int id) {

			GetSingleResponse response = await _catalogItemManager.GetSingleAsync(id);

			if (response.Success) return Ok(response);
			return BadRequest(response);
		}

		[HttpGet(Name = "GetRangeAsync")]
		[ProducesResponseType(type: typeof(GetRangeResponse), statusCode: (int) HttpStatusCode.OK)]
		[ProducesResponseType(type: typeof(GetRangeResponse), statusCode: (int) HttpStatusCode.BadRequest)]
		public async Task<IActionResult> GetRangeAsync(byte pageSize = 10, byte pageIndex = 1, bool includeNested = false) {

			GetRangeResponse response = await _catalogItemManager.GetRangeAsync(pageSize, pageIndex, includeNested);

			if (response.Success) return Ok(response);
			return BadRequest(response);
		}

		[HttpPost(Name = "CreateSingleAsync")]
		[ProducesResponseType(type: typeof(CreateSingleResponse), statusCode: (int) HttpStatusCode.Created)]
		[ProducesResponseType(type: typeof(CreateSingleResponse), statusCode: (int) HttpStatusCode.BadRequest)]
		public async Task<IActionResult> CreateSingleAsync(CatalogItemCreateSingleDTO catalogItemDTO) {

			CreateSingleResponse response = await _catalogItemManager.CreateSingleAsync(catalogItemDTO);

			if (response.Success) Ok(response);
			return BadRequest(response);
		}

		[HttpPut(Name = "UpdateSingleAsync")]
		[ProducesResponseType(type: typeof(UpdateSingleResponse), statusCode: (int) HttpStatusCode.Accepted)]
		[ProducesResponseType(type: typeof(UpdateSingleResponse), statusCode: (int) HttpStatusCode.NotFound)]
		public async Task<IActionResult> UpdateSingleAsync(CatalogItemUpdateDTO catalogItemUpdateDTO) {

			UpdateSingleResponse response = await _catalogItemManager.UpdateSingleAsync(catalogItemUpdateDTO);

			if (response.Success) return Ok(response);
			return BadRequest(response);
		}

		[HttpDelete("{id}", Name = "RemoveSingleAsync")]
		[ProducesResponseType(type: typeof(RemoveSingleResponse), statusCode: (int) HttpStatusCode.Accepted)]
		[ProducesResponseType(type: typeof(RemoveSingleResponse), statusCode: (int) HttpStatusCode.NotFound)]
		public async Task<IActionResult> RemoveSingleAsync([FromRoute] int id) {

			RemoveSingleResponse response = await _catalogItemManager.RemoveSingleAsync(id);

			if (response.Success) return Ok(response);
			return BadRequest(response);
		}

		#endregion
	}
}