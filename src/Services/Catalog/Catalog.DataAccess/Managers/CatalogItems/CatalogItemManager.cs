using AutoMapper;
using Catalog.DataAccess.DTOs.CatalogItem;
using Catalog.Core.Models;
using Catalog.DataAccess.Managers.CatalogItems.Messages;
using Catalog.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.API.Controllers;

namespace Catalog.DataAccess.Managers.CatalogItems {

	public class CatalogItemManager : ICatalogItemManager {

		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ILogger<CatalogItemManager> _logger;
		private readonly CatalogOptions _catalogOptions;

		public CatalogItemManager(IUnitOfWork unitOfWork,
			IMapper mapper, ILogger<CatalogItemManager> logger,
			IOptions<CatalogOptions> catalogOptions) {
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_logger = logger;
			_catalogOptions = catalogOptions.Value;
		}

		public async Task<GetSingleResponse> GetSingleAsync(int catalogItemID) {

			GetSingleResponse response = new GetSingleResponse();

			try {
				CatalogItem catalogItem = await _unitOfWork.CatalogItemRepository.GetSingleAsync(catalogItemID);
				response.CatalogItem = _mapper.Map<CatalogItemReadDTO>(catalogItem);
			} catch (Exception ex) {
				response.Success = false;
				response.AddErrorMessage(ex.Message);
			}

			return response;
		}

		public async Task<GetRangeResponse> GetRangeAsync(byte pageSize, byte pageIndex, bool includeNested) {

			GetRangeResponse response = new GetRangeResponse();

			try {
				IEnumerable<CatalogItem> catalogItems = await _unitOfWork.CatalogItemRepository.GetRangeAsync(pageSize, pageIndex, includeNested);
				response.CatalogItems = _mapper.Map<IEnumerable<CatalogItemReadDTO>>(catalogItems);
			} catch (Exception ex) {
				response.Success = false;
				response.AddErrorMessage(ex.Message);
			}

			return response;
		}

		public async Task<CreateSingleResponse> CreateSingleAsync(CatalogItemCreateSingleDTO catalogItemDTO) {

			CreateSingleResponse response = new CreateSingleResponse();

			try {
				CatalogItem catalogItem = await _unitOfWork.CatalogItemRepository.CreateAsync(_mapper.Map<CatalogItem>(catalogItemDTO));
				response.CatalogItem = _mapper.Map<CatalogItemReadDTO>(catalogItem);
			} catch (Exception ex) {
				response.Success = false;
				response.AddErrorMessage(ex.Message);
			}

			return response;
		}

		public Task<CreateRangeResponse> CreateRangeAsync(CatalogItemCreateRangeDTO catalogItemDTO) {
            throw new NotImplementedException();
        }

		public async Task<UpdateSingleResponse> UpdateSingleAsync(CatalogItemUpdateSingleDTO catalogItemUpdateDTO) {

			UpdateSingleResponse response = new UpdateSingleResponse();

			try {
				CatalogItem catalogItem = await _unitOfWork.CatalogItemRepository.UpdateAsync(_mapper.Map<CatalogItem>(catalogItemUpdateDTO));
				response.CatalogItem = _mapper.Map<CatalogItemReadDTO>(catalogItem);
			} catch (Exception ex) {
				response.Success = false;
				response.AddErrorMessage(ex.Message);
			}

			return response;
		}

		public Task<UpdateRangeResponse> UpdateRangeAsync(CatalogItemUpdateRangeDTO catalogItemUpdateRangeDTO) {
            throw new NotImplementedException();
        }

		public async Task<RemoveSingleResponse> RemoveSingleAsync(int catalogItemID) {

			RemoveSingleResponse response = new RemoveSingleResponse();

			try {
				await _unitOfWork.CatalogItemRepository.RemoveAsync(catalogItemID);
			} catch (Exception ex) {
				response.Success = false;
				response.AddErrorMessage(ex.Message);
			}

			return response;
		}

        public Task<RemoveRangeResponse> RemoveRangeAsync(int[] ids) {
            throw new NotImplementedException();
        }
    }
}