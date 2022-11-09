using System;

namespace Catalog.API.DTOs.CatalogType {
	public class CatalogTypeReadDTO {
		private int _id;
		private string _type;

		public int ID {
			get { return _id; }
			set { _id = value; }
		}

		public string Type {
			get { return _type; }
			set { _type = value; }
		}

		public CatalogTypeReadDTO(string type = "Undefined") {
			this._type = type;
		}
	}
}
