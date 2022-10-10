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
				.ForMember(destinationMember => destinationMember.CatalogBrand, memberOptions => memberOptions.MapFrom(src => src.CatalogBrand))
				.ForMember(destinationMember => destinationMember.CatalogType, memberOptions => memberOptions.MapFrom(src => src.CatalogType)).ReverseMap();

			CreateMap<CatalogItemCreateDTO, CatalogItem>()
				.ForMember(destinationMember => destinationMember.Id, memberOptions => memberOptions.Ignore())
				.ForMember(destinationMember => destinationMember.CatalogBrand, memberOptions => memberOptions.Ignore())
				.ForMember(destinationMember => destinationMember.CatalogType, memberOptions => memberOptions.Ignore());

			CreateMap<CatalogItemUpdateDTO, CatalogItem>()
				.ForMember(destinationMember => destinationMember.Id, memberOptions => memberOptions.Ignore())
				.ForMember(destinationMember => destinationMember.CatalogBrand, memberOptions => memberOptions.Ignore())
				.ForMember(destinationMember => destinationMember.CatalogType, memberOptions => memberOptions.Ignore());

			CreateMap<CatalogItemReadDTO, CatalogItemPublishDTO>();
		}
	}
}
