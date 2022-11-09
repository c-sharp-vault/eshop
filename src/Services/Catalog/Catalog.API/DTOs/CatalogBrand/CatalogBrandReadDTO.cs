using System;

namespace Catalog.API.DTOs.CatalogBrand {
	public class CatalogBrandReadDTO {
		private int _id;
		private string _brand;

		public int Id {
			get { return _id; }
			set { _id = value; }
		}

		public string Brand {
			get { return _brand; }
			set { _brand = value; }
		}

		public CatalogBrandReadDTO(string brand = "Undefined") {
			this._brand = brand;
		}
	}
}
