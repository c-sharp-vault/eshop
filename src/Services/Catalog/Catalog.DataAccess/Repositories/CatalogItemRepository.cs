using Catalog.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.DataAccess.Repositories {
	public class CatalogItemRepository : Repository<CatalogItem>, ICatalogItemRepository {

		private readonly CatalogContext _catalogContext;

		public CatalogItemRepository(CatalogContext catalogContext) : base(catalogContext) { 
			this._catalogContext = catalogContext;
		}
	}
}