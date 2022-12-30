using Catalog.Core.Models;
using Catalog.Infrastructure.Records;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace Catalog.DataAccess.EntityTypeConfigurations {
	internal class CatalogItemConfiguration : EntityTypeConfigurationBase<CatalogItem> {
		protected override void ConfigureEntity(EntityTypeBuilder<CatalogItem> builder) {
			builder.ToTable("CatalogItems", Constants.CatalogSchemaName).HasKey(x => x.CatalogItemID);

			builder.Property(x => x.CatalogItemID).ValueGeneratedOnAdd().IsRequired();

			builder.HasIndex(x => x.Name).IsUnique();

			builder.Property(x => x.Name).IsRequired(true).HasMaxLength(50);

			builder.Property(x => x.Description).IsRequired(false).HasMaxLength(200);

			builder.Property(x => x.Price).IsRequired(true);

			builder.Property(x => x.PictureFileName).IsRequired(false).HasDefaultValue("placeholder.png");

			builder.HasOne(x => x.CatalogType).WithMany(x => x.CatalogItems).HasForeignKey(x => x.CatalogTypeID);

			builder.HasOne(x => x.CatalogBrand).WithMany(x => x.CatalogItems).HasForeignKey(x => x.CatalogBrandID);

			builder.Property(x => x.AvailableStock).HasDefaultValue(0);

			builder.Property(x => x.RestockThreshold).HasDefaultValue(10);

			builder.Property(x => x.MaxStockThreshold).HasDefaultValue(1000);

			builder.Property(x => x.OnReorder).HasDefaultValue(false);
		}

		protected override IEnumerable<CatalogItem> GetSeedData() =>
			new List<CatalogItem> {
				new CatalogItem { CatalogItemID = 1, Name = "Sin Azúcar 1.5l", CatalogBrandID = 2, CatalogTypeID = 2 },
				new CatalogItem { CatalogItemID = 2, Name = "Clásica 500ml", CatalogBrandID = 5, CatalogTypeID = 5 },
				new CatalogItem { CatalogItemID = 3, Name = "Tita", CatalogBrandID = 3, CatalogTypeID = 4 },
				new CatalogItem { CatalogItemID = 4, Name = "Ice Blast 8", CatalogBrandID = 4, CatalogTypeID = 3 }
			};
	}
}
