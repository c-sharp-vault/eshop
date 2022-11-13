using AutoMapper;
using Catalog.API.DTOs;
using Catalog.API.DTOs.CatalogBrand;
using Catalog.API.DTOs.CatalogItem;
using Catalog.API.DTOs.CatalogType;
using Catalog.Core.Models;

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

			CreateMap<CatalogItemUpdateDTO, CatalogItem>()
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
