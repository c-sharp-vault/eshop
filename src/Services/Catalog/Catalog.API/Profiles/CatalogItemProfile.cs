using AutoMapper;
using Catalog.DataAccess.DTOs;
using Catalog.DataAccess.DTOs.CatalogBrand;
using Catalog.DataAccess.DTOs.CatalogItem;
using Catalog.DataAccess.DTOs.CatalogType;
using Catalog.Core.Models;
using Catalog.DataAccess.Managers.CatalogItems.Messages;

namespace Catalog.API.Profiles {
	public class CatalogItemProfile : Profile {
		public CatalogItemProfile() {
			// Source -> Target
			CreateMap<CatalogBrand, CatalogBrandReadDTO>().ReverseMap();

			CreateMap<CatalogType, CatalogTypeReadDTO>().ReverseMap();

			CreateMap<CatalogItem, CatalogItemReadDTO>().ReverseMap();

			CreateMap<CatalogItemCreateSingleDTO, CatalogItem>()
				.ForMember(src => src.CatalogType, dst => dst.Ignore())
				.ForMember(src => src.CatalogBrand, dst => dst.Ignore())
				.ForMember(src => src.CreatedBy, dst => dst.Ignore())
				.ForMember(src => src.CreatedOn, dst => dst.Ignore())
				.ForMember(src => src.UpdatedBy, dst => dst.Ignore())
				.ForMember(src => src.UpdatedOn, dst => dst.Ignore())
				.ReverseMap();

			CreateMap<CatalogItemUpdateSingleDTO, CatalogItem>()
				.ForMember(src => src.CatalogType, dst => dst.Ignore())
				.ForMember(src => src.CatalogBrand, dst => dst.Ignore())
				.ForMember(src => src.CreatedBy, dst => dst.Ignore())
				.ForMember(src => src.CreatedOn, dst => dst.Ignore())
				.ForMember(src => src.UpdatedBy, dst => dst.Ignore())
				.ForMember(src => src.UpdatedOn, dst => dst.Ignore())
				.ReverseMap();

			CreateMap<CatalogItemReadDTO, CatalogItemPublishDTO>().ReverseMap();
		}
	}
}
