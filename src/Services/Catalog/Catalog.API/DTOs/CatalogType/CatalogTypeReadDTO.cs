using System;

namespace Catalog.API.DTOs.CatalogType {
	public class CatalogTypeReadDTO : EntityTypeDTO {
		public int CatalogTypeID { get; set; }

		public string Type { get; set; }

		public string CreatedBy { get; set; }

		public DateTime CreatedOn { get; set; }

		public string UpdatedBy { get; set; }

		public DateTime UpdatedOn { get; set; }
	}
}