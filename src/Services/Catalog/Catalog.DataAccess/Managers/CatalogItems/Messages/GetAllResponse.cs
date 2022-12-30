using Catalog.DataAccess.DTOs.CatalogItem;
using System.Collections.Generic;
using System.ComponentModel;

namespace Catalog.DataAccess.Managers.CatalogItems.Messages {
    public class GetRangeResponse : ResponseBase {
        public IEnumerable<CatalogItemReadDTO> CatalogItems { get; set; }
    }
}