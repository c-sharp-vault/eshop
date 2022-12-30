
using Catalog.DataAccess.DTOs.CatalogItem;

namespace Catalog.DataAccess.Managers.CatalogItems.Messages {
	public class CreateSingleRequest : RequestBase {
		public CatalogItemCreateSingleDTO CatalogItem { get; set; }
	}
}
