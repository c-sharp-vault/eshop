
using Catalog.DataAccess.DTOs.CatalogItem;

namespace Catalog.DataAccess.Managers.CatalogItems.Messages {
	public class UpdateSingleRequest : RequestBase {
		public CatalogItemUpdateSingleDTO CatalogItem { get; set; }
	}
}
