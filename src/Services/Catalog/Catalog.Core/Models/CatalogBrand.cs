using System.ComponentModel.DataAnnotations;

namespace Catalog.Core.Models {
	public class CatalogBrand : IEntity {
		private int _id;
		private String _brand;

		[Key]
		[Required]
		public int Id {
			get { return _id; }
			set { _id = value; }
		}

		[Required]
		public String Brand {
			get { return _brand; }
			set { _brand = value; }
		}

		public CatalogBrand(String brand = "Otra") {
			this._brand = brand;
		}
	}
}
