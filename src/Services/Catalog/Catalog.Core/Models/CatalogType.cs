namespace Catalog.Core.Models {
	public class CatalogType : IEntityType {
		private int _id;
		private String _type;
		private ICollection<CatalogItem> _catalogItems;

		public int Id {
			get { return _id; }
			set { _id = value; }
		}

		public String Type {
			get { return _type; }
			set { _type = value; }
		}

		public ICollection<CatalogItem> CatalogItems {
			get {
				return _catalogItems;
			}
			set {
				if (value == null) {
					throw new ArgumentNullException($"{nameof(CatalogType.CatalogItems)} can't be null");
				}

				_catalogItems = value;
			}
		}

		public CatalogType(String type = "Undefined") {
			this._type = type;
			this.CatalogItems = new List<CatalogItem>();
		}
	}
}
