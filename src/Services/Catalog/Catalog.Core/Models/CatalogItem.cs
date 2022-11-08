using System;
using Catalog.Infrastructure.Enums;

namespace Catalog.Core.Models {
    public class CatalogItem : EntityType {
		private String name;
		private String description;
		private decimal price;
		private String pictureFileName;
		private int availableStock;
		private int restockThreshold;
		private int maxStockThreshold;
		private CatalogType catalogType;
		private CatalogBrand catalogBrand;

		public CatalogItem() : base() {
			name = String.Empty;
			description = String.Empty;
			price = PriceLimit.MIN;
			pictureFileName = "placeholder.png";
			availableStock = AvailableStockLimit.MIN;
			restockThreshold = RestockThresholdLimit.MIN;
			maxStockThreshold = MaxStockThresholdLimit.MIN;
			catalogType = new CatalogType();
			catalogBrand = new CatalogBrand();
		}

		public String Name {
			get => name;
			set => name = value ?? throw new ArgumentNullException(nameof(name), "Value can't be null");
		}

		public String Description {
			get => description;
			set => description = value ?? throw new ArgumentNullException(nameof(description), "Value can't be null");
		}

		public decimal Price {
			get => price;
			set => price =  (value >= PriceLimit.MIN) && (value <= PriceLimit.MAX) ? 
							value : 
							throw new ArgumentOutOfRangeException(nameof(price), 
								$"Value must be between {PriceLimit.MIN} and {PriceLimit.MAX}");
		}

		public String PictureFileName {
			get => pictureFileName;
			set => pictureFileName = value ?? throw new ArgumentNullException(nameof(pictureFileName), "Value can't be null");
		}


		public String PictureUri {
			get => @$"C:\Users\Fedex\source\repos\eShop\src\Services\Catalog\Catalog.API\Assets\Images\{pictureFileName}";
		}

		public int AvailableStock {
			get => availableStock;
			set => availableStock = (availableStock >= AvailableStockLimit.MIN) && 
									(availableStock <= AvailableStockLimit.MAX) ? value : 
										throw new ArgumentOutOfRangeException(nameof(availableStock), 
											$"Value must be between {AvailableStockLimit.MIN} and {AvailableStockLimit.MAX}");
		}

		public int RestockThreshold {
			get => restockThreshold;
			set => restockThreshold = (restockThreshold >= RestockThresholdLimit.MIN) && 
									  (restockThreshold <= RestockThresholdLimit.MAX) ? value : 
										throw new ArgumentOutOfRangeException(nameof(restockThreshold), 
											$"Value must be between {RestockThresholdLimit.MIN} and {RestockThresholdLimit.MAX}");
		}

		public int MaxStockThreshold {
			get => maxStockThreshold;
			set => maxStockThreshold = (maxStockThreshold >= MaxStockThresholdLimit.MIN) && 
									   (maxStockThreshold <= MaxStockThresholdLimit.MAX) ? value : 
										throw new ArgumentOutOfRangeException(nameof(maxStockThreshold), 
											$"Value must be between {MaxStockThresholdLimit.MIN} and {MaxStockThresholdLimit.MAX}");
		}

		public int CatalogTypeId { get; set; }

		public CatalogType CatalogType {
			get => catalogType;
			set => catalogType = value ?? throw new ArgumentNullException(nameof(catalogType), "Value can't be null");
		}

		public int CatalogBrandId { get; set; }

		public CatalogBrand CatalogBrand {
			get => catalogBrand;
			set => catalogBrand = value ?? throw new ArgumentNullException(nameof(catalogBrand), "Value can't be null");
		}

		public bool OnReorder { get; set; }
	}
}
