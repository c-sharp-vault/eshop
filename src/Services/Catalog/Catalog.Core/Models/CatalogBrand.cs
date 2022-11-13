
using System;
using System.Collections.Generic;

namespace Catalog.Core.Models
{
    public class CatalogBrand : IEntity {
		private String _brand;
		private ICollection<CatalogItem> _catalogItems;
		private string _createdBy;
		private DateTime _createdOn;
		private string _updatedBy;
		private DateTime _updatedOn;

		public CatalogBrand() : base() {
			_brand = String.Empty;
			_catalogItems = new List<CatalogItem>();
			_createdBy = Environment.UserName;
			_createdOn = DateTime.Now;
			_updatedBy = Environment.UserName;
			_updatedOn = DateTime.Now;
		}

		public int CatalogBrandID { get; set; }

		public String Brand {
			get => _brand;
			set => _brand = value ?? throw new ArgumentNullException(nameof(_brand), "Value can't be null");
		}

		public ICollection<CatalogItem> CatalogItems {
			get => _catalogItems;
			set => _catalogItems = value ?? throw new ArgumentNullException(nameof(_catalogItems), "Value can't be null");
		}

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