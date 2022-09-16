using System.ComponentModel.DataAnnotations;

namespace Catalog.Core.Models {
	public class CatalogBrand : IEntityType {
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

		public CatalogBrand(String brand) {
			this._brand = brand;
		}
	}
}
