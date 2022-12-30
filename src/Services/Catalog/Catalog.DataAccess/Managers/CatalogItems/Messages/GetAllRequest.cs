
namespace Catalog.DataAccess.Managers.CatalogItems.Messages {
    public class GetRangeRequest : RequestBase {
        public byte PageSize { get; set; }
        public byte PageIndex { get; set; }
        public bool IncludeNested { get; set; }
    }
}