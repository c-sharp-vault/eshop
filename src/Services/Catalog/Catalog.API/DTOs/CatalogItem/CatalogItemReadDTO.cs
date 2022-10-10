using Catalog.API.DTOs.CatalogBrand;
using Catalog.API.DTOs.CatalogType;
using System;

namespace Catalog.API.DTOs.CatalogItem {
	public class CatalogItemReadDTO {
		private int _id;
		private String _name = String.Empty;
		private String _description = String.Empty;
		private decimal _price;
		private String _pictureFileName = String.Empty;
		private String _pictureUri = String.Empty;
		private CatalogTypeReadDTO _catalogType = new CatalogTypeReadDTO();
		private CatalogBrandReadDTO _catalogBrand = new CatalogBrandReadDTO();
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

		public CatalogTypeReadDTO CatalogType {
			get { return _catalogType; }
			set { _catalogType = value; }
		}


		public CatalogBrandReadDTO CatalogBrand {
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
	}
}
