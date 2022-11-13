using AutoMapper;
using Catalog.API.DTOs.CatalogItem;
using Catalog.Core.Models;
using Catalog.DataAccess;
using Catalog.DataAccess.Exceptions;
using Catalog.Infrastructure.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Catalog.API.Controllers {
	[ApiController]
	[Route("api/v1/catalog/[controller]")]
	public class ItemsController : ControllerBase {
		#region Private Fields

		private readonly IUnitOfWork unitOfWork;
		private readonly IMapper mapper;
		private readonly ILogger<ItemsController> logger;
		private readonly CatalogOptions catalogOptions;

		#endregion
		
		#region Constructors

		public ItemsController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ItemsController> logger, IOptions<CatalogOptions> catalogOptions) {
			this.unitOfWork = unitOfWork;
			this.mapper = mapper;
			this.logger = logger;
			this.catalogOptions = catalogOptions.Value;
		}

		#endregion

		#region Public Methods (Actions)
		[HttpGet(Name = "GetAllAsync")]
		[ProducesResponseType(type: typeof(IEnumerable<CatalogItemReadDTO>), statusCode: (int) HttpStatusCode.OK)]
		[ProducesResponseType(statusCode: (int) HttpStatusCode.BadRequest)]
		public async Task<IActionResult> GetAllAsync([FromQuery] byte pageSize = 10, [FromQuery] byte pageIndex = 1) {
			logger.LogInformation("--> Returning all CatalogItems");

			IEnumerable<CatalogItem> catalogItems = await this.unitOfWork.CatalogItemRepository.GetAllAsync(pageSize, pageIndex);
			return Ok(mapper.Map<IEnumerable<CatalogItemReadDTO>>(catalogItems));
		}

		[HttpGet("{id}", Name = "GetByIDAsync")]
		[ProducesResponseType(type: typeof(CatalogItemReadDTO), statusCode: (int) HttpStatusCode.OK)]
		[ProducesResponseType(statusCode: (int) HttpStatusCode.NotFound)]
		public async Task<IActionResult> GetByIDAsync(int id) {
			logger.LogInformation($"--> Returning Catalog with ID = {id}");

			if (!await unitOfWork.CatalogItemRepository.ExistsAsync(id)) return NotFound($"Inexistent CatalogItem with ID = '{id}'");

			CatalogItem catalogitem = await unitOfWork.CatalogItemRepository.GetAsync(id);
			return Ok(mapper.Map<CatalogItemReadDTO>(catalogitem));
		}

		[HttpPost(Name = "CreateSingleAsync")]
		[ProducesResponseType(type: typeof(CreatedAtRouteResult), statusCode: (int) HttpStatusCode.Created)]
		[ProducesResponseType(type: typeof(BadRequestResult), statusCode: (int) HttpStatusCode.BadRequest)]
		public async Task<IActionResult> CreateSingleAsync([FromBody] CatalogItemCreateSingleDTO catalogItemCreateDTO) {
			logger.LogInformation($"Creating CatatlogItem: {JsonSerializer.Serialize(catalogItemCreateDTO)}");

			CatalogItem catalogItem = mapper.Map<CatalogItem>(catalogItemCreateDTO);
			if (await unitOfWork.CatalogItemRepository.NameExistsAsync(catalogItem))
				return BadRequest($"{typeof(CatalogItem)} with Name = {catalogItem.Name} already exists.");

			CatalogBrand catalogBrand = await unitOfWork.CatalogBrandRepository.GetAsync(catalogItemCreateDTO.CatalogBrandId);
			if (catalogBrand == null) return NotFound($"{typeof(CatalogBrand)} with ID = {catalogItem.CatalogBrandId} not found.");

			CatalogType catalogType = await unitOfWork.CatalogTypeRepository.GetAsync(catalogItemCreateDTO.CatalogTypeId);
			if (catalogType == null) return NotFound($"{typeof(CatalogType)} with ID = {catalogItem.CatalogTypeId} not found.");

			catalogItem.CatalogBrand = catalogBrand;
			catalogItem.CatalogType = catalogType;
			await unitOfWork.CatalogItemRepository.AddAsync(catalogItem);
			await unitOfWork.CompleteAsync();
			CatalogItemReadDTO catalogItemReadDTO = mapper.Map<CatalogItemReadDTO>(catalogItem);

			return CreatedAtRoute(nameof(GetByIDAsync), new { id = catalogItem.CatalogItemID.ToString() }, catalogItemReadDTO);
		}

		//[HttpPost]
		//[ProducesResponseType(type: typeof(AcceptedResult), statusCode: (int)HttpStatusCode.Accepted)]
		//[ProducesResponseType(type: typeof(BadRequestResult), statusCode: (int)HttpStatusCode.BadRequest)]
		//public async Task<IActionResult> CreateRangeAsync([FromBody] IList<CatalogItemCreateDTO> CatalogItemCreateDTO) {
		//	logger.LogInformation($"Creating CatatlogItems: {JsonSerializer.Serialize(CatalogItemCreateDTO)}");

		//	IList<CatalogItem> catalogItems = mapper.Map<IList<CatalogItem>>(CatalogItemCreateDTO);
		//	foreach (CatalogItem catalogItem in catalogItems) {
		//		if (await unitOfWork.CatalogItemRepository.NameExistsAsync(catalogItem))
		//			return BadRequest($"{typeof(CatalogItem)} with Name = {catalogItem.Name} already exists.");

		//		CatalogBrand catalogBrand;
		//		try {
		//			catalogBrand = await unitOfWork.CatalogBrandRepository.GetAsync(catalogItem.CatalogBrandId);
		//		} catch (RecordNotFoundException ex) {
		//			throw new ArgumentException(ex.Message);
		//		}

		//		if (catalogBrand == null) return NotFound($"{typeof(CatalogBrand)} with ID = {catalogItem.CatalogBrandId} not found.");

		//		CatalogType catalogType = await unitOfWork.CatalogTypeRepository.GetAsync(catalogItem.CatalogTypeId);
		//		if (catalogType == null) return NotFound($"{typeof(CatalogType)} with ID = {catalogItem.CatalogTypeId} not found.");
		//	}

		//	await unitOfWork.CatalogItemRepository.AddRangeAsync(catalogItems);
		//	await unitOfWork.CompleteAsync();

		//	return Accepted(mapper.Map<List<CatalogItemReadDTO>>(catalogItems));
		//}

		[HttpPut(Name = "UpdateSingleAsync")]
		[ProducesResponseType(type: typeof(AcceptedAtActionResult), statusCode: (int) HttpStatusCode.Accepted)]
		[ProducesResponseType(type: typeof(NotFoundObjectResult), statusCode: (int) HttpStatusCode.NotFound)]
		public async Task<IActionResult> UpdateSingleAsync([FromBody] CatalogItemUpdateDTO catalogItemUpdateDTO) {
			logger.LogInformation($"Updating CatalogItem: {JsonSerializer.Serialize(catalogItemUpdateDTO)}");

			CatalogItem? catalogItem = await unitOfWork.CatalogItemRepository.GetAsync(catalogItemUpdateDTO.CatalogItemID);
			if (catalogItem == null) return NotFound($"{typeof(CatalogItem)} with ID = {catalogItemUpdateDTO.CatalogItemID} not found.");

			CatalogBrand catalogBrand = await unitOfWork.CatalogBrandRepository.GetAsync(catalogItemUpdateDTO.CatalogBrandId);
			if (catalogBrand == null) return NotFound($"{typeof(CatalogBrand)} with ID = {catalogItem.CatalogBrandId} not found.");

			CatalogType catalogType = await unitOfWork.CatalogTypeRepository.GetAsync(catalogItemUpdateDTO.CatalogTypeId);
			if (catalogType == null) return NotFound($"{typeof(CatalogType)} with ID = {catalogItem.CatalogTypeId} not found.");

			catalogItem.Name = catalogItemUpdateDTO.Name;
			catalogItem.Description = catalogItemUpdateDTO.Description;
			catalogItem.Price = catalogItemUpdateDTO.Price;
			catalogItem.PictureFileName = catalogItemUpdateDTO.PictureFileName;
			catalogItem.CatalogTypeId = catalogItemUpdateDTO.CatalogTypeId;
			catalogItem.AvailableStock = catalogItemUpdateDTO.AvailableStock;
			catalogItem.RestockThreshold = catalogItemUpdateDTO.RestockThreshold;
			catalogItem.MaxStockThreshold = catalogItemUpdateDTO.MaxStockThreshold;
			catalogItem.OnReorder = catalogItemUpdateDTO.OnReorder;
			catalogItem.CatalogBrand = catalogBrand;
			catalogItem.CatalogType = catalogType;
			catalogItem.UpdatedBy = Environment.UserName;
			catalogItem.UpdatedOn = DateTime.Now;
			unitOfWork.Complete();
			CatalogItemReadDTO catalogItemReadDTO = mapper.Map<CatalogItemReadDTO>(catalogItem);

			CatalogItem? catalogItemBis = await unitOfWork.CatalogItemRepository.GetAsync(catalogItemUpdateDTO.CatalogItemID);

			return AcceptedAtRoute(nameof(GetByIDAsync), new { id = catalogItemReadDTO.CatalogItemID.ToString()}, catalogItemReadDTO);
		}

		[HttpDelete("{id}", Name = "RemoveSingleAsync")]
		[ProducesResponseType(type: typeof(AcceptedResult), statusCode: (int) HttpStatusCode.Accepted)]
		[ProducesResponseType(type: typeof(NotFoundObjectResult), statusCode: (int) HttpStatusCode.NotFound)]
		public async Task<IActionResult> RemoveSingleAsync(int id) {
			logger.LogInformation($"Deleting CatalogItem with ID = {id}");

			CatalogItem? catalogItem = await unitOfWork.CatalogItemRepository.GetAsync(id);
			if (catalogItem == null) return NotFound($"{typeof(CatalogItem)} with ID = {id} not found.");

			await unitOfWork.CatalogItemRepository.RemoveAsync(catalogItem.CatalogItemID);
			await unitOfWork.CompleteAsync();

			return Accepted(nameof(RemoveSingleAsync), $"{nameof(CatalogItem)} with ID = {id} was succesfullly deleted");
		}

		#endregion

		#region Private Methods

		protected async Task Dispose() => await unitOfWork.DisposeAsync();
		
		#endregion
	}
}
