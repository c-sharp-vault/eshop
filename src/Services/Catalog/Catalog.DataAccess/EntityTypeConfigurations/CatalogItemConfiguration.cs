using Catalog.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Data;

namespace Catalog.DataAccess.EntityTypeConfigurations {
	internal class CatalogItemConfiguration : IEntityTypeConfiguration<CatalogItem> {
		public void Configure(EntityTypeBuilder<CatalogItem> builder) {
			builder.ToTable("CatalogItems");

			builder.HasKey(x => x.ID);

			builder.Property(x => x.ID).UseHiLo("catalog_item_hilo").IsRequired();

			builder.HasIndex(x => x.Name).IsUnique();

			builder.Property(x => x.Name).IsRequired(true).HasMaxLength(50);

			builder.Property(x => x.Description).IsRequired(false).HasMaxLength(200);

			builder.Property(x => x.Price).IsRequired(true);

			builder.Property(x => x.PictureFileName).IsRequired(false).HasDefaultValue("placeholder.png");

			builder.Ignore(x => x.PictureUri);

			builder.HasOne(x => x.CatalogType).WithMany(x => x.CatalogItems).HasForeignKey(x => x.CatalogTypeId);

			builder.HasOne(x => x.CatalogBrand).WithMany(x => x.CatalogItems).HasForeignKey(x => x.CatalogBrandId);

			builder.Property(x => x.AvailableStock).HasDefaultValue(0);

			builder.Property(x => x.RestockThreshold).HasDefaultValue(10);

			builder.Property(x => x.MaxStockThreshold).HasDefaultValue(1000);

			builder.Property(x => x.OnReorder).HasDefaultValue(false);
		}
	}
}
