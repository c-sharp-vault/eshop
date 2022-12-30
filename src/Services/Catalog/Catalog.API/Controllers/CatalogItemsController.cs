using AutoMapper;
using Catalog.DataAccess;
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
	[Route("api/v1/catalog/[controller]")]
	public class ItemsController : ControllerBase {

		#region Fields

		private readonly ICatalogItemManager _catalogItemManager;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ILogger<ItemsController> _logger;
		private readonly CatalogOptions _catalogOptions;

		#endregion

		#region Constructors

		public ItemsController(ICatalogItemManager catalogItemManager, IUnitOfWork unitOfWork, IMapper mapper, 
			ILogger<ItemsController> logger, IOptions<CatalogOptions> catalogOptions) {
			_catalogItemManager = catalogItemManager;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_logger = logger;
			_catalogOptions = catalogOptions.Value;
		}

		#endregion

		[HttpGet("get-single", Name = "GetSingleAsync")]
		[ProducesResponseType(type: typeof(GetSingleResponse), statusCode: (int) HttpStatusCode.OK)]
		[ProducesResponseType(type: typeof(GetSingleResponse), statusCode: (int) HttpStatusCode.BadRequest)]
		public async Task<IActionResult> GetSingleAsync(GetSingleRequest request) {

			GetSingleResponse response = await _catalogItemManager.GetSingleAsync(request);

			return response.Success
				? Ok(response)
				: BadRequest(response);
		}

		#region Actions

		[HttpGet("get-range", Name = "GetRangeAsync")]
		[ProducesResponseType(type: typeof(GetRangeResponse), statusCode: (int) HttpStatusCode.OK)]
		[ProducesResponseType(type: typeof(GetRangeResponse), statusCode: (int) HttpStatusCode.BadRequest)]
		public async Task<IActionResult> GetRangeAsync(GetRangeRequest request) {

			GetRangeResponse response = await _catalogItemManager.GetRangeAsync(request);

			return response.Success
				? Ok(response)
				: BadRequest(response);
		}

		[HttpPost("create-single", Name = "CreateSingleAsync")]
		[ProducesResponseType(type: typeof(CreateSingleResponse), statusCode: (int) HttpStatusCode.Created)]
		[ProducesResponseType(type: typeof(CreateSingleResponse), statusCode: (int) HttpStatusCode.BadRequest)]
		public async Task<IActionResult> CreateSingleAsync(CreateSingleRequest request) {

			CreateSingleResponse response = await _catalogItemManager.CreateSingleAsync(request);

			return response.Success
				? Created(nameof(GetSingleAsync), response)
				: BadRequest(response);
		}

		[HttpPut("update-single", Name = "UpdateSingleAsync")]
		[ProducesResponseType(type: typeof(AcceptedAtActionResult), statusCode: (int) HttpStatusCode.Accepted)]
		[ProducesResponseType(type: typeof(NotFoundObjectResult), statusCode: (int) HttpStatusCode.NotFound)]
		public async Task<IActionResult> UpdateSingleAsync(UpdateSingleRequest request) {

			UpdateSingleResponse response = await _catalogItemManager.UpdateSingleAsync(request);

			return response.Success
				? Accepted(nameof(GetSingleAsync), response)
				: BadRequest(response);
		}

		[HttpDelete("remove-single", Name = "RemoveSingleAsync")]
		[ProducesResponseType(type: typeof(AcceptedResult), statusCode: (int) HttpStatusCode.Accepted)]
		[ProducesResponseType(type: typeof(NotFoundObjectResult), statusCode: (int) HttpStatusCode.NotFound)]
		public async Task<IActionResult> RemoveSingleAsync(RemoveSingleRequest request) {

			RemoveSingleResponse response = await _catalogItemManager.RemoveSingleAsync(request);

			return response.Success
				? Accepted(response)
				: BadRequest(response);
		}

		#endregion

		#region Private Methods

		protected async Task Dispose() => await _unitOfWork.DisposeAsync();

		#endregion
	}
}