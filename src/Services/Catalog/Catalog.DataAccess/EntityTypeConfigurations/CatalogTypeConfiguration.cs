using Catalog.Core.Models;
using Catalog.Infrastructure.Records;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace Catalog.DataAccess.EntityTypeConfigurations {
	internal class CatalogTypeConfiguration : EntityTypeConfigurationBase<CatalogType> {
		protected override void ConfigureEntity(EntityTypeBuilder<CatalogType> builder) {
			builder.ToTable("CatalogTypes", Constants.CatalogSchemaName).HasKey(x => x.CatalogTypeID);

			builder.Property(x => x.CatalogTypeID).ValueGeneratedOnAdd().IsRequired();

			builder.Property(x => x.Type).IsRequired(true).HasMaxLength(100);

			builder.HasMany(x => x.CatalogItems).WithOne(x => x.CatalogType).HasForeignKey(x => x.CatalogTypeID);
		}

		protected override IEnumerable<CatalogType> GetSeedData() =>
			new List<CatalogType> {
				new CatalogType { CatalogTypeID = 1, Type = "N/A" },
				new CatalogType { CatalogTypeID = 2, Type = "Gaseosas" },
				new CatalogType { CatalogTypeID = 3, Type = "Cigarrillos" },
				new CatalogType { CatalogTypeID = 4, Type = "Alfajores & Obleas" },
				new CatalogType { CatalogTypeID = 5, Type = "Cervezas" }
			};
	}
}
