using Catalog.Core.Models;
using System;

namespace Catalog.API.DTOs.CatalogItem {
	public class CatalogItemCreateDTO : EntityTypeDTO {
		private String _name = String.Empty;
		private String _description = String.Empty;
		private decimal _price;
		private String _pictureFileName = String.Empty;
		private String _pictureUri = String.Empty;
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
