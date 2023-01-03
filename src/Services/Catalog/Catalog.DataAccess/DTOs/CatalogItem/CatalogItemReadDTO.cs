using Catalog.DataAccess.DTOs.CatalogBrand;
using Catalog.DataAccess.DTOs.CatalogType;
using System;

namespace Catalog.DataAccess.DTOs.CatalogItem {
	public class CatalogItemReadDTO : EntityTypeDTO {
		private string _name = string.Empty;
		private string _description = string.Empty;
		private decimal _price;
		private string _pictureFileName = string.Empty;
		private int _catalogTypeId;
		private CatalogTypeReadDTO _catalogType = new CatalogTypeReadDTO();
		private int _catalogBrandId;
		private CatalogBrandReadDTO _catalogBrand = new CatalogBrandReadDTO();
		private int _availableStock;
		private int _restockThreshold;
		private int _maxStockThreshold;
		private bool _onReorder;

		public int CatalogItemID { get; set; }

		public string Name {
			get { return _name; }
			set { _name = value; }
		}

		public string Description {
			get { return _description; }
			set { _description = value; }
		}

		public decimal Price {
			get { return _price; }
			set { _price = value; }
		}

		public string PictureFileName {
			get { return _pictureFileName; }
			set { _pictureFileName = value; }
		}

		public int CatalogTypeId {
			get => _catalogTypeId;
			set => _catalogTypeId = value;
		}

		public CatalogTypeReadDTO CatalogType {
			get { return _catalogType; }
			set { _catalogType = value; }
		}

		public int CatalogBrandId {
			get => _catalogBrandId;
			set => _catalogBrandId = value;
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
