using System.ComponentModel.DataAnnotations;

namespace Catalog.Core.Models {
	public class CatalogBrand : IEntityType {
		private int _id;
		private String _brand;
		private ICollection<CatalogItem> _catalogItems;

		public int Id {
			get { return _id; }
			set { _id = value; }
		}

		public String Brand {
			get { return _brand; }
			set { _brand = value; }
		}

		public ICollection<CatalogItem> CatalogItems {
			get {
				return _catalogItems;
			}
			set {
				if (value == null) {
					throw new ArgumentNullException($"{nameof(CatalogBrand.CatalogItems)} can't be null");
				}

				_catalogItems = value;
			}
		}


		public CatalogBrand(String brand = "Undefined") {
			this._brand = brand;
			this._catalogItems = new List<CatalogItem>();
		}
	}
}
