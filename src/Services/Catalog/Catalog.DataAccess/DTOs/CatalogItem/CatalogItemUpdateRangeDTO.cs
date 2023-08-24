using System.Collections.Generic;

namespace Catalog.DataAccess.DTOs.CatalogItem {
    public class CatalogItemUpdateRangeDTO {
		public IReadOnlyCollection<CatalogItemUpdateSingleDTO> CatalogItems { get; set; }
	}
}
