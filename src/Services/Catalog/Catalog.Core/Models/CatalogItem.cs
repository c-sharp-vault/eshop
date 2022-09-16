namespace Catalog.Core.Models {
	public class CatalogItem : IEntityType {
		private int _id;
		private String _name;
		private String _description;
		private decimal _price;
		private String _pictureFileName;
		private String _pictureUri;
		private int _catalogTypeId;
		private CatalogType _catalogType;
		private int _catalogBrandId;
		private CatalogBrand _catalogBrand;
		private int _availableStock;
		private int _restockThreshold;
		private int _maxStockThreshold;
		private bool _onReorder;

		public int Id {
			get { return _id; }
			set { _id = value; }
		}

		public String Name {
			get { return _name; }
			set { _name = value; }
		}

		public String Description {
			get { return _description; }
			set { _description = value; }
		}

		public decimal Price {
			get { return _price; }
			set { _price = value; }
		}

		public String PictureFileName {
			get { return _pictureFileName; }
			set { _pictureFileName = value; }
		}


		public String PictureUri {
			get { return _pictureUri; }
			set { _pictureUri = value; }
		}

		public int CatalogTypeId {
			get { return _catalogTypeId; }
			set { _catalogTypeId = value; }
		}

		public CatalogType CatalogType {
			get { return _catalogType; }
			set { _catalogType = value; }
		}

		public int CatalogBrandId {
			get { return _catalogBrandId; }
			set { _catalogBrandId = value; }
		}

		public CatalogBrand CatalogBrand {
			get { return _catalogBrand; }
			set { _catalogBrand = value; }
		}

		public int AvailableStock {
			get { return _availableStock; }
			set { _availableStock = value; }
		}

		public int RestockThreshold {
			get { return _restockThreshold; }
			set { _restockThreshold = value; }
		}

		public int MaxStockThreshold {
			get { return _maxStockThreshold; }
			set { _maxStockThreshold = value; }
		}

		public bool OnReorder {
			get { return _onReorder; }
			set { _onReorder = value; }
		}

		public CatalogItem() { }

		public CatalogItem(string name, string description, CatalogType catalogType, CatalogBrand catalogBrand, decimal price, int availableStock = 0, 
						   string pictureFileName = "placeholder.png", int restockThreshold = 10, 
						   int maxStockThreshold = 1000, bool onReorder = false) {
			this._name = name;
			this._description = description;
			this._price = price;
			this._pictureFileName = pictureFileName;
			this._pictureUri = @"C:\Users\Fedex\source\repos\eShop\src\Services\Catalog\Catalog.API\Assets\Images\" + pictureFileName;
			this._catalogType = catalogType;
			this._catalogBrand = catalogBrand;
			this._availableStock = availableStock;
			this.RestockThreshold = restockThreshold;
			this._maxStockThreshold = maxStockThreshold;
			this._onReorder = onReorder;
		}
	}
}
