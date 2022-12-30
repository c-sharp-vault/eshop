using System;
using System.Collections.Generic;

namespace Catalog.Core.Models {
	public class CatalogType : Entity {
		private string _type;
		private ICollection<CatalogItem> _catalogItems;

		public CatalogType() : base() {
			_type = string.Empty;
			_catalogItems = new List<CatalogItem>();
		}

		public int CatalogTypeID { get; set; }

		public string Type {
			get => _type;
			set => _type = value ?? throw new ArgumentNullException(nameof(Type));
		}

		public ICollection<CatalogItem> CatalogItems {
			get => _catalogItems;
			set => _catalogItems = value ?? throw new ArgumentNullException(nameof(CatalogItems));
		}
	}
}