
namespace Catalog.API.Utils {
    public static class APIRoutes {
        private static readonly string _baseURL = "http//localhost:5207/api/v1/catalog";

        public static class Items {
            private static readonly string _itemsControllerURL = string.Concat(_baseURL, "/items");

            // HTTP GET
            public static string GetAll { get => string.Concat(_itemsControllerURL, "?pageSize={pageSize}&pageIndex={pageIndex}"); }
            // HTTP GET
            public static string GetByID { get => string.Concat(_itemsControllerURL, "/{id}"); }
            // HTTP POST
            public static string Create { get => _itemsControllerURL; }
            // HTTP PUT
            public static string Update { get => _itemsControllerURL; }
            // HTTP DELETE
            public static string Delete { get => string.Concat(_itemsControllerURL, "/{id}"); }
        }
    }    
}