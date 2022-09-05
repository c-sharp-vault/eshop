using System.ComponentModel.DataAnnotations;

namespace Catalog.Core.Models {
	public class CatalogItem : IEntity {
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

		[Key]
		[Required]
		public int Id {
			get { return _id; }
			set { _id = value; }
		}

		[Required]
		public String Name {
			get { return _name; }
			set { _name = value; }
		}

		public String Description {
			get { return _description; }
			set { _description = value; }
		}

		[Required]
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

		[Required]
		public int CatalogTypeId {
			get { return _catalogTypeId; }
			set { _catalogTypeId = value; }
		}

		[Required]
		public CatalogType CatalogType {
			get { return _catalogType; }
			set { _catalogType = value; }
		}

		[Required]
		public int CatalogBrandId {
			get { return _catalogBrandId; }
			set { _catalogBrandId = value; }
		}

		[Required]
		public CatalogBrand CatalogBrand {
			get { return _catalogBrand; }
			set { _catalogBrand = value; }
		}

		[Required]
		public int AvailableStock {
			get { return _availableStock; }
			set { _availableStock = value; }
		}

		[Required]
		public int RestockThreshold {
			get { return _restockThreshold; }
			set { _restockThreshold = value; }
		}

		[Required]
		public int MaxStockThreshold {
			get { return _maxStockThreshold; }
			set { _maxStockThreshold = value; }
		}

		public bool OnReorder {
			get { return _onReorder; }
			set { _onReorder = value; }
		}

		//public CatalogItem(string name, string description, decimal price, string pictureFileName, string pictureUri, int catalogTypeId, CatalogType catalogType, int catalogBrandId, CatalogBrand catalogBrand, int availableStock, int restockThreshold, int maxStockThreshold, bool onReorder) {
		//	if (catalogBrand == null) {
		//		throw new ArgumentNullException($"Must provide an argument for {typeof(CatalogBrand)}");
		//	}

		//	Name = name;
		//	Description = description;
		//	Price = price;
		//	PictureFileName = pictureFileName;
		//	PictureUri = pictureUri;
		//	CatalogTypeId = catalogTypeId;
		//	CatalogType = catalogType;
		//	CatalogBrandId = catalogBrandId;
		//	CatalogBrand = catalogBrand;
		//	AvailableStock = availableStock;
		//	RestockThreshold = restockThreshold;
		//	MaxStockThreshold = maxStockThreshold;
		//	OnReorder = onReorder;
		//}	
	}
}
