using AutoMapper;
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

			CreateMap<CatalogItem, CatalogItemReadDTO>()
				.ForMember(dst => dst.CatalogBrand, opt => opt.MapFrom(src => src.CatalogBrand))
				.ForMember(dst => dst.CatalogType, opt => opt.MapFrom(src => src.CatalogType))
				.ForMember(dst => dst.CatalogBrandId, opt => opt.MapFrom(src => src.CatalogBrandId))
				.ForMember(dst => dst.CatalogTypeId, opt => opt.MapFrom(src => src.CatalogTypeId))
				.ReverseMap();

			CreateMap<CatalogItemCreateDTO, CatalogItem>()
				.ForMember(dst => dst.ID, opt => opt.Ignore())
				.ForMember(dst => dst.CatalogBrand, opt => opt.Ignore())
				.ForMember(dst => dst.CatalogType, opt => opt.Ignore())
				.ForMember(dst => dst.CatalogBrandId, opt => opt.MapFrom(src => src.CatalogBrandId))
				.ForMember(dst => dst.CatalogTypeId, opt => opt.MapFrom(src => src.CatalogTypeId));

			CreateMap<CatalogItemUpdateDTO, CatalogItem>()
				.ForMember(dst => dst.CatalogBrand, opt => opt.Ignore())
				.ForMember(dst => dst.CatalogType, opt => opt.Ignore())
				.ForMember(dst => dst.CatalogBrandId, opt => opt.MapFrom(src => src.CatalogBrandId))
				.ForMember(dst => dst.CatalogTypeId, opt => opt.MapFrom(src => src.CatalogTypeId));

			CreateMap<CatalogItemReadDTO, CatalogItemPublishDTO>();
		}
	}
}
