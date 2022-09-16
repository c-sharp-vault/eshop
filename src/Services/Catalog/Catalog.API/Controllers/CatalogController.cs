using Catalog.Core.Models;
using Catalog.DataAccess;
using Catalog.Infrastructure.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers {
	[Route("api/v1/[controller]")]
	public class CatalogController : ControllerBase {
		private readonly IUnitOfWork _unitOfWork;
		private readonly CatalogOptions _catalogOptions;
		
		public CatalogController(IUnitOfWork unitOfWork, IOptions<CatalogOptions> catalogOptions) {
			_unitOfWork = unitOfWork;
			_catalogOptions = catalogOptions.Value;
		}

		// GET api/v1/[controller]/items[?pageSize=3&pageIndex=10]
		[HttpGet]
		[Route("items")]
		[ProducesResponseType(typeof(IEnumerable<CatalogItem>), (int) HttpStatusCode.OK)]
		[ProducesResponseType((int) HttpStatusCode.BadRequest)]
		public async Task<IActionResult> GetItemsAsync([FromQuery] byte pageSize = 10, [FromQuery] byte pageIndex = 0, String ids = null) {
			IEnumerable<CatalogItem> catalogItems;

			// if any ids were provided...
			if (!String.IsNullOrEmpty(ids)) {
				catalogItems = await GetItemsByIdAsync(ids);

				if (!catalogItems.Any()) {
					return BadRequest("ids value invalid. Must be a comma-separated list of numbers");
				}

				return Ok(catalogItems);
			}

			catalogItems = await this._unitOfWork.CatalogItemRepository.GetAllAsync(pageSize, pageIndex);

			return Ok(catalogItems);
		}

		private async Task<IEnumerable<CatalogItem>> GetItemsByIdAsync(string ids) {
			IEnumerable<(bool Ok, int Value)> numericIds = ids.Split(',').Select(x => (Ok: int.TryParse(x, out int id), Value: id)); 
			if (!numericIds.All(x => x.Ok)) {
				return new List<CatalogItem>();
			}

			IEnumerable<int> idsToSelect = numericIds.Select(x => x.Value);

			IEnumerable<CatalogItem> items = await _unitOfWork.CatalogItemRepository.FindAllAsync(x => idsToSelect.Contains(x.Id));

			return items;
		}

		protected async Task Dispose() {
			await _unitOfWork.DisposeAsync();
		}
	}
}
