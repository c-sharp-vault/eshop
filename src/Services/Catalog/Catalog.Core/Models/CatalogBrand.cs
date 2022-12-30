
using System;
using System.Collections.Generic;

namespace Catalog.Core.Models {
    public class CatalogBrand : Entity {
		private string _brand;
		private ICollection<CatalogItem> _catalogItems;

		public CatalogBrand() : base() {
			_brand = string.Empty;
			_catalogItems = new List<CatalogItem>();
		}

		public int CatalogBrandID { get; set; }

		public string Brand {
			get => _brand;
			set => _brand = value ?? throw new ArgumentNullException(nameof(_brand), "Value can't be null");
		}

		public ICollection<CatalogItem> CatalogItems {
			get => _catalogItems;
			set => _catalogItems = value ?? throw new ArgumentNullException(nameof(_catalogItems), "Value can't be null");
		}
	}
}