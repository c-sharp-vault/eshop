﻿using Catalog.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.DataAccess.EntityTypeConfigurations {
	internal class CatalogTypeConfiguration : IEntityTypeConfiguration<CatalogType> {
		public void Configure(EntityTypeBuilder<CatalogType> builder) {
			builder.ToTable("CatalogTypes");

			builder.HasKey(x => x.ID);

			builder.Property(x => x.ID).UseHiLo("catalog_type_hilo").IsRequired();

			builder.Property(x => x.Type).IsRequired(true).HasMaxLength(100);

			builder.HasMany(x => x.CatalogItems).WithOne(x => x.CatalogType).HasForeignKey(x => x.CatalogTypeId);
		}
	}
}
