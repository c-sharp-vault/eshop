using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Core.Models;
using Catalog.DataAccess.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Catalog.DataAccess.Repositories {

	public class CatalogItemRepository : Repository<CatalogItem>, ICatalogItemRepository {

		#region Fields

		private readonly CatalogDbContext catalogContext;

		#endregion

		public CatalogItemRepository(CatalogDbContext catalogContext) : base(catalogContext) {
			this.catalogContext = catalogContext;
		}

		public async Task<bool> NameExistsAsync(string name) =>

			await catalogContext.CatalogItems.AnyAsync(x => x.Name == name);

		public override async Task<CatalogItem> GetSingleAsync(int id) {

			if (id == 0) 
				throw new ArgumentOutOfRangeException(nameof(id), "The provided ID must be a positive integer.");

			if (!await ExistsAsync(id)) 
				throw new RecordNotFoundException($"{nameof(CatalogItem)} entity with ID = {id} not found.");

			CatalogItem catalogItem = await catalogContext.CatalogItems.FindAsync(id);
			await catalogContext.Entry(catalogItem).Reference(x => x.CatalogBrand).LoadAsync();
			await catalogContext.Entry(catalogItem).Reference(x => x.CatalogType).LoadAsync();

			return catalogItem;
		}

		public async Task<IReadOnlyCollection<CatalogItem>> GetRangeAsync(byte pageSize, byte pageIndex, bool includeNested) {
			IQueryable<CatalogItem> query = catalogContext.CatalogItems.OrderBy(x => x.CatalogItemID).Skip(pageSize * (pageIndex - 1)).Take(pageSize);

			if (includeNested) query = query.Include(x => x.CatalogBrand).Include(x => x.CatalogType);

			return await query.ToListAsync();
		}

		public override async Task<CatalogItem> CreateAsync(CatalogItem catalogItem) {

			if (catalogItem == null)
				throw new ArgumentNullException(nameof(CatalogItem));

			if (await NameExistsAsync(catalogItem.Name))
				throw new ArgumentException($"{typeof(CatalogItem)} entity with Name = {catalogItem.Name} already exists.");

			CatalogBrand catalogBrand = await catalogContext.CatalogBrands.FindAsync(catalogItem.CatalogBrandID);
			if (catalogBrand == null)
				throw new ArgumentException($"{nameof(CatalogBrand)} entity with ID = {catalogItem.CatalogBrandID} not found.");

			CatalogType catalogType = await catalogContext.CatalogTypes.FindAsync(catalogItem.CatalogTypeID);
			if (catalogType == null)
				throw new ArgumentException($"{nameof(CatalogType)} entity with ID = {catalogItem.CatalogTypeID} not found.");

			catalogItem.CatalogBrand = catalogBrand;
			catalogItem.CatalogType = catalogType;
			await catalogContext.CatalogItems.AddAsync(catalogItem);

			if (await catalogContext.SaveChangesAsync() == 0)
				throw new DbUpdateException($"Failed trying to save new {typeof(CatalogItem)} into the database.");

			return await GetSingleAsync(catalogItem.CatalogItemID);
		}

		public async Task<CatalogItem> UpdateAsync(CatalogItem catalogItemUpdateDTO) {

			if (catalogItemUpdateDTO == null)
				throw new ArgumentNullException(nameof(CatalogItem));

			CatalogItem catalogItem = await catalogContext.CatalogItems.FindAsync(catalogItemUpdateDTO.CatalogItemID);
			if (catalogItem == null)
				throw new ObjectNotFoundException($"{nameof(CatalogItem)} entity with ID = {catalogItemUpdateDTO.CatalogItemID} was not found.");

			if (catalogItemUpdateDTO.CatalogBrandID <= 0)
				throw new ArgumentOutOfRangeException(nameof(catalogItemUpdateDTO.CatalogBrandID),
					catalogItemUpdateDTO.CatalogBrandID,
					$"Can't assing non-natural numbers to {nameof(CatalogBrand)}'s foreign key.");

			if (catalogItemUpdateDTO.CatalogTypeID <= 0)
				throw new ArgumentOutOfRangeException(nameof(catalogItemUpdateDTO.CatalogTypeID),
					catalogItemUpdateDTO.CatalogTypeID,
					$"Can't assing non-natural numbers to {nameof(CatalogType)}'s foreign key.");

			CatalogBrand catalogBrand = await catalogContext.CatalogBrands.FindAsync(catalogItemUpdateDTO.CatalogBrandID);
			if (catalogBrand == null)
				throw new ObjectNotFoundException($"{nameof(CatalogBrand)} entity with ID = {catalogItem.CatalogBrandID} was not found.");

			CatalogType catalogType = await catalogContext.CatalogTypes.FindAsync(catalogItemUpdateDTO.CatalogTypeID);
			if (catalogType == null)
				throw new ObjectNotFoundException($"{nameof(CatalogType)} entity with ID = {catalogItem.CatalogTypeID} was not found.");

			catalogItem.Name = catalogItemUpdateDTO.Name;
			catalogItem.Description = catalogItemUpdateDTO.Description;
			catalogItem.Price = catalogItemUpdateDTO.Price;
			catalogItem.PictureFileName = catalogItemUpdateDTO.PictureFileName;
			catalogItem.CatalogTypeID = catalogItemUpdateDTO.CatalogTypeID;
			catalogItem.CatalogBrandID = catalogItemUpdateDTO.CatalogBrandID;
			catalogItem.AvailableStock = catalogItemUpdateDTO.AvailableStock;
			catalogItem.RestockThreshold = catalogItemUpdateDTO.RestockThreshold;
			catalogItem.MaxStockThreshold = catalogItemUpdateDTO.MaxStockThreshold;
			catalogItem.OnReorder = catalogItemUpdateDTO.OnReorder;
			catalogItem.UpdatedBy = Environment.UserName;
			catalogItem.UpdatedOn = DateTime.Now;

			//_unitOfWork.CatalogItemRepository.CatalogContext.Entry(catalogItem).CurrentValues.SetValues(catalogItemUpdateDTO);

			if (await catalogContext.SaveChangesAsync() == 0)
				throw new DbUpdateException($"Failed trying to save updated {typeof(CatalogItem)} into the database.");

			return catalogItem;
		}
	}
}