using Catalog.Core.Models;
using Catalog.Infrastructure.Records;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace Catalog.DataAccess.EntityTypeConfigurations {
	internal class CatalogBrandConfiguration : EntityTypeConfigurationBase<CatalogBrand> {
		protected override void ConfigureEntity(EntityTypeBuilder<CatalogBrand> builder) {
			builder.ToTable("CatalogBrands", Constants.CatalogSchemaName).HasKey(x => x.CatalogBrandID);

			builder.Property(x => x.CatalogBrandID).ValueGeneratedOnAdd().IsRequired();

			builder.Property(x => x.Brand).IsRequired(true).HasMaxLength(100);

			builder.HasMany(x => x.CatalogItems).WithOne(x => x.CatalogBrand).HasForeignKey(x => x.CatalogTypeID);
		}

		protected override IEnumerable<CatalogBrand> GetSeedData() =>
			new List<CatalogBrand> {
				new CatalogBrand { CatalogBrandID = 1, Brand = "N/A" },
				new CatalogBrand { CatalogBrandID = 2, Brand = "Coca-Cola" },
				new CatalogBrand { CatalogBrandID = 3, Brand = "Terrabusi" },
				new CatalogBrand { CatalogBrandID = 4, Brand = "Marlboro" },
				new CatalogBrand { CatalogBrandID = 5, Brand = "Quilmes" }
			};
	}
}
