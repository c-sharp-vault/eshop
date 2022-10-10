using System;

namespace Catalog.API.DTOs.CatalogType {
	public class CatalogTypeReadDTO {
		private int _id;
		private String _type;

		public int Id {
			get { return _id; }
			set { _id = value; }
		}

		public String Type {
			get { return _type; }
			set { _type = value; }
		}

		public CatalogTypeReadDTO(String type = "Undefined") {
			this._type = type;
		}
	}
}
