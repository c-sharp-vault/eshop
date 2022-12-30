
using Catalog.DataAccess.DTOs.CatalogItem;

namespace Catalog.DataAccess.Managers.CatalogItems.Messages {
	public class GetSingleResponse : ResponseBase {
		public CatalogItemReadDTO CatalogItem { get; set; }
	}
}
