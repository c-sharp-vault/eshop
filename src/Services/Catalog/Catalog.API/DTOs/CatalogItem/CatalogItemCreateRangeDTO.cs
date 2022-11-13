using System.Collections;
using System.Collections.Generic;

namespace Catalog.API.DTOs.CatalogItem {
	public class CatalogItemCreateRangeDTO {
		public IList<CatalogItemCreateSingleDTO> CatalogItems { get; set; }
	}
}
