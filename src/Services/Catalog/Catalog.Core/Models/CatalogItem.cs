using System;
using Catalog.Infrastructure.Enums;
using System.Diagnostics.CodeAnalysis;

namespace Catalog.Core.Models {
	public class CatalogItem : Entity {
		private string _name;
		private string _description;
		private decimal _price;
		private string _pictureFileName;
		private int _availableStock;
		private int _restockThreshold;
		private int _maxStockThreshold;
		private CatalogType _catalogType;
		private CatalogBrand _catalogBrand;

		[SetsRequiredMembers]
		public CatalogItem() {
			_name = string.Empty;
			_description = string.Empty;
			_price = PriceLimit.MIN;
			_pictureFileName = "placeholder.png";
			_availableStock = AvailableStockLimit.MIN;
			_restockThreshold = RestockThresholdLimit.MIN;
			_maxStockThreshold = MaxStockThresholdLimit.MIN;
		}

		public int CatalogItemID { get; set; }

		public string Name {
			get => _name;
			set => _name = value ?? throw new ArgumentNullException(nameof(Name));
		}

		public string Description {
			get => _description;
			set => _description = value ?? throw new ArgumentNullException(nameof(Description));
		}

		public decimal Price {
			get => _price;
			set => _price = (value >= PriceLimit.MIN) && (value <= PriceLimit.MAX) 
								? value 
								: throw new ArgumentOutOfRangeException(nameof(Price),
									$"Value must be between {PriceLimit.MIN} and {PriceLimit.MAX}");
		}

		public string PictureFileName {
			get => _pictureFileName;
			set => _pictureFileName = value ?? throw new ArgumentNullException(nameof(PictureFileName));
		}

		public int AvailableStock {
			get => _availableStock;
			set => _availableStock = (_availableStock >= AvailableStockLimit.MIN) && (_availableStock <= AvailableStockLimit.MAX) 
										? value 
										: throw new ArgumentOutOfRangeException(nameof(AvailableStock),
											$"Value must be between {AvailableStockLimit.MIN} and {AvailableStockLimit.MAX}");
		}

		public int RestockThreshold {
			get => _restockThreshold;
			set => _restockThreshold = (_restockThreshold >= RestockThresholdLimit.MIN) && (_restockThreshold <= RestockThresholdLimit.MAX) 
											? value 
											: throw new ArgumentOutOfRangeException(nameof(RestockThreshold),
												$"Value must be between {RestockThresholdLimit.MIN} and {RestockThresholdLimit.MAX}");
		}

		public int MaxStockThreshold {
			get => _maxStockThreshold;
			set => _maxStockThreshold = (_maxStockThreshold >= MaxStockThresholdLimit.MIN) && (_maxStockThreshold <= MaxStockThresholdLimit.MAX) 
											? value 
											: throw new ArgumentOutOfRangeException(nameof(MaxStockThreshold),
												$"Value must be between {MaxStockThresholdLimit.MIN} and {MaxStockThresholdLimit.MAX}");
		}

		public int CatalogTypeID { get; set; }

		public CatalogType CatalogType {
			get => _catalogType;
			set => _catalogType = value ?? throw new ArgumentNullException(nameof(CatalogType));
		}

		public int CatalogBrandID { get; set; }

		public CatalogBrand CatalogBrand {
			get => _catalogBrand;
			set => _catalogBrand = value ?? throw new ArgumentNullException(nameof(CatalogBrand));
		}

		public bool OnReorder { get; set; }
	}
}
