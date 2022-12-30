using System.Collections;
using System.Collections.Generic;

namespace Catalog.DataAccess.DTOs.CatalogItem {
	public class CatalogItemCreateRangeDTO {
		public IList<CatalogItemCreateSingleDTO> CatalogItems { get; set; }
	}
}
