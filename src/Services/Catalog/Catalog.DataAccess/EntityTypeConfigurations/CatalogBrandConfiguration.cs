using Catalog.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.DataAccess.EntityTypeConfigurations {
	public class CatalogBrandConfiguration : IEntityTypeConfiguration<CatalogBrand> {
		public void Configure(EntityTypeBuilder<CatalogBrand> builder) {
			builder.ToTable("CatalogBrands");

			builder.HasKey(x => x.Id);

			builder.Property(x => x.Id).UseHiLo("catalog_brand_hilo").IsRequired();

			builder.Property(x => x.Brand).IsRequired().HasMaxLength(100);

			builder.HasMany(x => x.CatalogItems).WithOne(x => x.CatalogBrand).HasForeignKey(x => x.CatalogTypeId);
		}
	}
}
