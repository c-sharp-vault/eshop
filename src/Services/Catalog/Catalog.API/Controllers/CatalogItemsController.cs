using AutoMapper;
using Catalog.DataAccess.DTOs.CatalogItem;
using Catalog.DataAccess.Managers.CatalogItems;
using Catalog.DataAccess.Managers.CatalogItems.Messages;
using Catalog.Infrastructure.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers {

	[ApiController]
	[Route("api/v1/catalog-items")]
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

		[HttpGet("get-single/{id}")]
		[ProducesResponseType(type: typeof(GetSingleResponse), statusCode: (int) HttpStatusCode.OK)]
		[ProducesResponseType(type: typeof(GetSingleResponse), statusCode: (int) HttpStatusCode.BadRequest)]
		public async Task<IActionResult> GetSingleAsync([FromRoute] int id) {

			GetSingleResponse response = await _catalogItemManager.GetSingleAsync(id);

			if (response.Success) return Ok(response);
			return BadRequest(response);
		}

		[HttpGet("get-range")]
		[ProducesResponseType(type: typeof(GetRangeResponse), statusCode: (int) HttpStatusCode.OK)]
		[ProducesResponseType(type: typeof(GetRangeResponse), statusCode: (int) HttpStatusCode.BadRequest)]
		public async Task<IActionResult> GetRangeAsync(byte pageSize = 10, byte pageIndex = 1, bool includeNested = false) {

			GetRangeResponse response = await _catalogItemManager.GetRangeAsync(pageSize, pageIndex, includeNested);

			if (response.Success) return Ok(response);
			return BadRequest(response);
		}

		[HttpPost("create-single")]
		[ProducesResponseType(type: typeof(CreateSingleResponse), statusCode: (int) HttpStatusCode.Created)]
		[ProducesResponseType(type: typeof(CreateSingleResponse), statusCode: (int) HttpStatusCode.BadRequest)]
		public async Task<IActionResult> CreateSingleAsync(CatalogItemCreateSingleDTO catalogItemDTO) {

			CreateSingleResponse response = await _catalogItemManager.CreateSingleAsync(catalogItemDTO);

			if (response.Success) Ok(response);
			return BadRequest(response);
		}

		[HttpPost("create-range")]
		[ProducesResponseType(type: typeof(CreateRangeResponse), statusCode: (int) HttpStatusCode.Created)]
		[ProducesResponseType(type: typeof(CreateRangeResponse), statusCode: (int) HttpStatusCode.BadRequest)]
		public async Task<IActionResult> CreateRangeAsync(CatalogItemCreateRangeDTO catalogItemDTO) {

			//CreateSingleResponse response = await _catalogItemManager.CreateRangeAsync(catalogItemDTO);

			//if (response.Success) Ok(response);
			//return BadRequest(response);
			return Ok();
		}

		[HttpPut("update-single")]
		[ProducesResponseType(type: typeof(UpdateSingleResponse), statusCode: (int) HttpStatusCode.Accepted)]
		[ProducesResponseType(type: typeof(UpdateSingleResponse), statusCode: (int) HttpStatusCode.BadRequest)]
		public async Task<IActionResult> UpdateSingleAsync(CatalogItemUpdateDTO catalogItemUpdateDTO) {

			UpdateSingleResponse response = await _catalogItemManager.UpdateSingleAsync(catalogItemUpdateDTO);

			if (response.Success) return Ok(response);
			return BadRequest(response);
		}

		[HttpPut("update-range")]
		[ProducesResponseType(type: typeof(UpdateRangeResponse), statusCode: (int) HttpStatusCode.Accepted)]
		[ProducesResponseType(type: typeof(UpdateRangeResponse), statusCode: (int) HttpStatusCode.BadRequest)]
		public async Task<IActionResult> UpdateRangeAsync(CatalogItemUpdateDTO catalogItemUpdateDTO) {

			//UpdateRangeResponse response = await _catalogItemManager.UpdateRangeAsync(catalogItemUpdateDTO);

			//if (response.Success) return Ok(response);
			//return BadRequest(response);

			return Ok();
		}

		[HttpDelete("remove-single/{id}")]
		[ProducesResponseType(type: typeof(RemoveSingleResponse), statusCode: (int) HttpStatusCode.Accepted)]
		[ProducesResponseType(type: typeof(RemoveSingleResponse), statusCode: (int) HttpStatusCode.BadRequest)]
		public async Task<IActionResult> RemoveSingleAsync([FromRoute] int id) {

			RemoveSingleResponse response = await _catalogItemManager.RemoveSingleAsync(id);

			if (response.Success) return Ok(response);
			return BadRequest(response);
		}

		[HttpDelete("remove-range")]
		[ProducesResponseType(type: typeof(RemoveRangeResponse), statusCode: (int)HttpStatusCode.Accepted)]
		[ProducesResponseType(type: typeof(RemoveRangeResponse), statusCode: (int)HttpStatusCode.BadRequest)]
		public async Task<IActionResult> RemoveRangeAsync([FromRoute] int id) {

			//RemoveRangeResponse response = await _catalogItemManager.RemoveRangeAsync(id);

			//if (response.Success) return Ok(response);
			//return BadRequest(response);

			return Ok();
		}

		#endregion
	}
}