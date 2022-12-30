using System;

namespace Catalog.DataAccess.DTOs.CatalogType {
	public class CatalogTypeReadDTO : EntityTypeDTO {
		public int CatalogTypeID { get; set; }

		public string Type { get; set; }
	}
}