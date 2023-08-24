using AutoMapper;
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

namespace Catalog.API.Controllers
{

    [ApiController]
	[Route("api/v1/catalog/items")]
	public class CatalogItemsController : ControllerBase, ICatalogItemsController {

		#region Fields

		private readonly ICatalogItemManager catalogItemManager;
		private readonly IMapper mapper;
		private readonly ILogger<CatalogItemsController> logger;
		private readonly CatalogOptions catalogOptions;

		#endregion

		#region Constructors

		public CatalogItemsController(ICatalogItemManager catalogItemManager, IMapper mapper,
			ILogger<CatalogItemsController> logger, IOptions<CatalogOptions> catalogOptions) {
			this.catalogItemManager = catalogItemManager;
			this.mapper = mapper;
			this.logger = logger;
			this.catalogOptions = catalogOptions.Value;
		}

		#endregion

		#region Actions

		[HttpGet]
		[Produces("application/json")]
		[ProducesResponseType(type: typeof(GetRangeResponse), statusCode: (int) HttpStatusCode.OK)]
		[ProducesResponseType(type: typeof(GetRangeResponse), statusCode: (int) HttpStatusCode.BadRequest)]
		public async Task<IActionResult> GetRangeAsync([FromQuery] byte pageSize = 10, [FromQuery] byte pageIndex = 1, 
			[FromQuery] bool includeNested = false) {

			GetRangeResponse response = await this.catalogItemManager.GetRangeAsync(pageSize, pageIndex, includeNested);

			if (response.Success) return Ok(response);
			return BadRequest(response);
		}

		[HttpGet("{id}")]
		[Produces("application/json")]
		[ProducesResponseType(type: typeof(GetSingleResponse), statusCode: (int) HttpStatusCode.OK)]
		[ProducesResponseType(type: typeof(GetSingleResponse), statusCode: (int) HttpStatusCode.BadRequest)]
		public async Task<IActionResult> GetSingleAsync([FromRoute] int id) {

			GetSingleResponse response = await this.catalogItemManager.GetSingleAsync(id);

			if (response.Success) return Ok(response);
			return BadRequest(response);
		}

		[IgnoreAntiforgeryToken]
		[HttpPost]
		[Produces("application/json")]
		[ProducesResponseType(type: typeof(CreateSingleResponse), statusCode: (int) HttpStatusCode.OK)]
		[ProducesResponseType(type: typeof(CreateSingleResponse), statusCode: (int) HttpStatusCode.BadRequest)]
		public async Task<IActionResult> CreateSingleAsync(CatalogItemCreateSingleDTO catalogItemDTO) {

			CreateSingleResponse response = await this.catalogItemManager.CreateSingleAsync(catalogItemDTO);

			if (response.Success) return Ok(response);
			return BadRequest(response);
		}

		// [HttpPost("create-range")]
		// [Produces("application/json")]
		// [ProducesResponseType(type: typeof(CreateRangeResponse), statusCode: (int) HttpStatusCode.Created)]
		// [ProducesResponseType(type: typeof(CreateRangeResponse), statusCode: (int) HttpStatusCode.BadRequest)]
		// public async Task<IActionResult> CreateRangeAsync(CatalogItemCreateRangeDTO catalogItemDTO) {

		// 	CreateRangeResponse response = await this.catalogItemManager.CreateRangeAsync(catalogItemDTO);

		// 	if (response.Success) Ok(response);
		// 	return BadRequest(response);
		// }

		[HttpPut]
		[Produces("application/json")]
		[ProducesResponseType(type: typeof(UpdateSingleResponse), statusCode: (int) HttpStatusCode.Accepted)]
		[ProducesResponseType(type: typeof(UpdateSingleResponse), statusCode: (int) HttpStatusCode.BadRequest)]
		public async Task<IActionResult> UpdateSingleAsync(CatalogItemUpdateSingleDTO catalogItemUpdateDTO) {

			UpdateSingleResponse response = await this.catalogItemManager.UpdateSingleAsync(catalogItemUpdateDTO);

			if (response.Success) return Ok(response);
			return BadRequest(response);
		}

		// [HttpPut("update-range")]
		// [Produces("application/json")]
		// [ProducesResponseType(type: typeof(UpdateRangeResponse), statusCode: (int) HttpStatusCode.Accepted)]
		// [ProducesResponseType(type: typeof(UpdateRangeResponse), statusCode: (int) HttpStatusCode.BadRequest)]
		// public async Task<IActionResult> UpdateRangeAsync(CatalogItemUpdateRangeDTO catalogItemUpdateRangeDTO) {

		// 	UpdateRangeResponse response = await this.catalogItemManager.UpdateRangeAsync(catalogItemUpdateRangeDTO);

		// 	if (response.Success) return Ok(response);
		// 	return BadRequest(response);
		// }

		[HttpDelete("{id}")]
		[Produces("application/json")]
		[ProducesResponseType(type: typeof(RemoveSingleResponse), statusCode: (int) HttpStatusCode.Accepted)]
		[ProducesResponseType(type: typeof(RemoveSingleResponse), statusCode: (int) HttpStatusCode.BadRequest)]
		public async Task<IActionResult> RemoveSingleAsync([FromRoute] int id) {

			RemoveSingleResponse response = await this.catalogItemManager.RemoveSingleAsync(id);

			if (response.Success) return Ok(response);
			return BadRequest(response);
		}

		// [HttpDelete("remove-range")]
		// [Produces("application/json")]
		// [ProducesResponseType(type: typeof(RemoveRangeResponse), statusCode: (int)HttpStatusCode.Accepted)]
		// [ProducesResponseType(type: typeof(RemoveRangeResponse), statusCode: (int)HttpStatusCode.BadRequest)]
		// public async Task<IActionResult> RemoveRangeAsync(int[] ids) {

		// 	RemoveRangeResponse response = await this.catalogItemManager.RemoveRangeAsync(ids);

		// 	if (response.Success) return Ok(response);
		// 	return BadRequest(response);
		// }

		#endregion
	}
}