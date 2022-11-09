using Catalog.Core.Models;
using System;

namespace Catalog.API.DTOs.CatalogItem {
	public class CatalogItemUpdateDTO : EntityTypeDTO {
		private string _name = string.Empty;
		private string _description = string.Empty;
		private decimal _price;
		private string _pictureFileName = string.Empty;
		private string _pictureUri = string.Empty;
		private int _catalogTypeId;
		private int _catalogBrandId;
		private int _availableStock;
		private int _restockThreshold;
		private int _maxStockThreshold;
		private bool _onReorder;

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


		public string PictureUri {
			get { return _pictureUri; }
			set { _pictureUri = value; }
		}

		public int CatalogTypeId {
			get { return _catalogTypeId; }
			set { _catalogTypeId = value; }
		}

		public int CatalogBrandId {
			get { return _catalogBrandId; }
			set { _catalogBrandId = value; }
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
