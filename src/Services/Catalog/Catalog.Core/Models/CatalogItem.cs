using System;
using Catalog.Infrastructure.Enums;

namespace Catalog.Core.Models {
	public class CatalogItem : IEntity {
		private String _name;
		private String _description;
		private decimal _price;
		private String _pictureFileName;
		private int _availableStock;
		private int _restockThreshold;
		private int _maxStockThreshold;
		private CatalogType _catalogType;
		private CatalogBrand _catalogBrand;
		private string _createdBy;
		private DateTime _createdOn;
		private string _updatedBy;
		private DateTime _updatedOn;

		public CatalogItem() {
			_name = String.Empty;
			_description = String.Empty;
			_price = PriceLimit.MIN;
			_pictureFileName = "placeholder.png";
			_availableStock = AvailableStockLimit.MIN;
			_restockThreshold = RestockThresholdLimit.MIN;
			_maxStockThreshold = MaxStockThresholdLimit.MIN;
			_catalogType = new CatalogType();
			_catalogBrand = new CatalogBrand();
			_createdBy = Environment.UserName;
			_createdOn = DateTime.Now;
			_updatedBy = Environment.UserName;
			_updatedOn = DateTime.Now;
		}

		public int CatalogItemID { get; set; }

		public String Name {
			get => _name;
			set => _name = value ?? throw new ArgumentNullException(nameof(_name), "Value can't be null");
		}

		public String Description {
			get => _description;
			set => _description = value ?? throw new ArgumentNullException(nameof(_description), "Value can't be null");
		}

		public decimal Price {
			get => _price;
			set => _price = (value >= PriceLimit.MIN) && (value <= PriceLimit.MAX) ?
							value :
							throw new ArgumentOutOfRangeException(nameof(_price),
								$"Value must be between {PriceLimit.MIN} and {PriceLimit.MAX}");
		}

		public String PictureFileName {
			get => _pictureFileName;
			set => _pictureFileName = value ?? throw new ArgumentNullException(nameof(_pictureFileName), "Value can't be null");
		}


		public String PictureUri {
			get => @$"C:\Users\Fedex\source\repos\eShop\src\Services\Catalog\Catalog.API\Assets\Images\{_pictureFileName}";
		}

		public int AvailableStock {
			get => _availableStock;
			set => _availableStock = (_availableStock >= AvailableStockLimit.MIN) &&
									(_availableStock <= AvailableStockLimit.MAX) ? value :
										throw new ArgumentOutOfRangeException(nameof(_availableStock),
											$"Value must be between {AvailableStockLimit.MIN} and {AvailableStockLimit.MAX}");
		}

		public int RestockThreshold {
			get => _restockThreshold;
			set => _restockThreshold = (_restockThreshold >= RestockThresholdLimit.MIN) &&
									  (_restockThreshold <= RestockThresholdLimit.MAX) ? value :
										throw new ArgumentOutOfRangeException(nameof(_restockThreshold),
											$"Value must be between {RestockThresholdLimit.MIN} and {RestockThresholdLimit.MAX}");
		}

		public int MaxStockThreshold {
			get => _maxStockThreshold;
			set => _maxStockThreshold = (_maxStockThreshold >= MaxStockThresholdLimit.MIN) &&
									   (_maxStockThreshold <= MaxStockThresholdLimit.MAX) ? value :
										throw new ArgumentOutOfRangeException(nameof(_maxStockThreshold),
											$"Value must be between {MaxStockThresholdLimit.MIN} and {MaxStockThresholdLimit.MAX}");
		}

		public int CatalogTypeId { get; set; }

		public CatalogType CatalogType {
			get => _catalogType;
			set => _catalogType = value ?? throw new ArgumentNullException(nameof(_catalogType), "Value can't be null");
		}

		public int CatalogBrandId { get; set; }

		public CatalogBrand CatalogBrand {
			get => _catalogBrand;
			set => _catalogBrand = value ?? throw new ArgumentNullException(nameof(_catalogBrand), "Value can't be null");
		}

		public bool OnReorder { get; set; }

		public string CreatedBy { 
			get => _createdBy;
		}

		public DateTime CreatedOn { 
			get => _createdOn; 
		}

		public string UpdatedBy { 
			get => _updatedBy; 
			set => _updatedBy = value; 
		}

		public DateTime UpdatedOn { 
			get => _updatedOn; 
			set => _updatedOn = value; 
		}
	}
}
