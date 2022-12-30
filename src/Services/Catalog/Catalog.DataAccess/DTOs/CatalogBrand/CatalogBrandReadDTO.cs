using System;

namespace Catalog.DataAccess.DTOs.CatalogBrand {
	public class CatalogBrandReadDTO : EntityTypeDTO {
		public int CatalogBrandID { get; set; }

		public string Brand { get; set; }
	}
}
