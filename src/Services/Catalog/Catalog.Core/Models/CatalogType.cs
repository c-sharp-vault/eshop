using System;
using System.Collections.Generic;

namespace Catalog.Core.Models {
	public class CatalogType : EntityType {
		private String type = String.Empty;
		private ICollection<CatalogItem> catalogItems;

		public CatalogType() : base() {
			type = String.Empty;
			catalogItems = new List<CatalogItem>();
		}

		public String Type {
			get => type;
			set => type = value ?? throw new ArgumentNullException(nameof(type), "Value can't be null");
		}

		public ICollection<CatalogItem> CatalogItems {
			get => catalogItems;
			set => catalogItems = value ?? throw new ArgumentNullException(nameof(catalogItems), "Value can't be null");
		}
	}
}
