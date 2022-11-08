
using System;
using System.Collections.Generic;

namespace Catalog.Core.Models
{
    public class CatalogBrand : EntityType {
		private String brand;
		private ICollection<CatalogItem> catalogItems;

		public CatalogBrand() : base() {
			brand = String.Empty;
			catalogItems = new List<CatalogItem>();
		}

		public String Brand {
			get => brand;
			set => brand = value ?? throw new ArgumentNullException(nameof(brand), "Value can't be null");
		}

		public ICollection<CatalogItem> CatalogItems {
			get => catalogItems;
			set => catalogItems = value ?? throw new ArgumentNullException(nameof(catalogItems), "Value can't be null");
		}
	}
}
