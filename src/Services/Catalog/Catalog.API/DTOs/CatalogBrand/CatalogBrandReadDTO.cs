using System;

namespace Catalog.API.DTOs.CatalogBrand {
	public class CatalogBrandReadDTO {
		private int _id;
		private String _brand;

		public int Id {
			get { return _id; }
			set { _id = value; }
		}

		public String Brand {
			get { return _brand; }
			set { _brand = value; }
		}

		public CatalogBrandReadDTO(String brand = "Undefined") {
			this._brand = brand;
		}
	}
}
