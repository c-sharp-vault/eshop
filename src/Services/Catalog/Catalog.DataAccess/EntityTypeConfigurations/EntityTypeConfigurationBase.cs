using Catalog.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace Catalog.DataAccess.EntityTypeConfigurations {
	internal abstract class EntityTypeConfigurationBase<T> : IEntityTypeConfiguration<T> where T : class, IEntity {
		public void Configure(EntityTypeBuilder<T> builder) {
			// This is the main configuration, should be implemented by each entity
			ConfigureEntity(builder);

			// All entities will have the same audit columns, added at the end
			ConfigureAuditFields(builder);

			// If an entity overrides GetSeedData(), initial values will be created
			builder.HasData(GetSeedData());
		}

		protected abstract void ConfigureEntity(EntityTypeBuilder<T> builder);

		private void ConfigureAuditFields(EntityTypeBuilder<T> builder) {
			builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(100);
			builder.Property(x => x.CreatedOn).HasColumnType("date");
			builder.Property(x => x.UpdatedBy).IsRequired().HasMaxLength(100);
			builder.Property(x => x.UpdatedOn).HasColumnType("date");
		}

		protected virtual IEnumerable<T> GetSeedData() => new List<T>();
	}
}
