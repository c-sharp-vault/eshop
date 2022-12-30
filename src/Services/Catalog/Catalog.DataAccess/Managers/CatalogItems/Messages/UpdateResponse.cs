
using Catalog.DataAccess.DTOs.CatalogItem;

namespace Catalog.DataAccess.Managers.CatalogItems.Messages {
	public class UpdateSingleResponse : ResponseBase {
		public CatalogItemReadDTO CatalogItem { get; set; }
	}
}