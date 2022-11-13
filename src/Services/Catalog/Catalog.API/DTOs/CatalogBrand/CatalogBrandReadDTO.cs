using System;

namespace Catalog.API.DTOs.CatalogBrand {
	public class CatalogBrandReadDTO : EntityTypeDTO {
		public int CatalogBrandID { get; set; }

		public string Brand { get; set; }

		public string CreatedBy { get; set; }

		public DateTime CreatedOn { get; set; }

		public string UpdatedBy { get; set; }

		public DateTime UpdatedOn { get; set; }
	}
}
