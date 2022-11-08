
namespace Catalog.Infrastructure.Enums {
    public record struct PriceLimit {
        public const decimal MIN = 0.1m;
        public const decimal MAX = 100000.0m;
    }
}