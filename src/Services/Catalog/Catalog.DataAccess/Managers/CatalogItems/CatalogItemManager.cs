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

		public async Task<GetSingleResponse> GetSingleAsync(GetSingleRequest request) {

			GetSingleResponse response = new GetSingleResponse();

			try {
				CatalogItem catalogItem = await _unitOfWork.CatalogItemRepository.GetByIDAsync(request.CatalogItemID);
				response.CatalogItem = _mapper.Map<CatalogItemReadDTO>(catalogItem);
			} catch (Exception ex) {
				response.Success = false;
				response.AddErrorMessage(ex.Message);
			}

			return response;
		}

		public async Task<GetRangeResponse> GetRangeAsync(GetRangeRequest request) {

			GetRangeResponse response = new GetRangeResponse();

			try {
				IEnumerable<CatalogItem> catalogItems = await _unitOfWork.CatalogItemRepository.GetAllAsync(request.PageSize, request.PageIndex, request.IncludeNested);
				response.CatalogItems = _mapper.Map<IEnumerable<CatalogItemReadDTO>>(catalogItems);
			} catch (Exception ex) {
				response.Success = false;
				response.AddErrorMessage(ex.Message);
			}

			return response;
		}

		public async Task<CreateSingleResponse> CreateSingleAsync(CreateSingleRequest request) {

			CreateSingleResponse response = new CreateSingleResponse();

			try {
				CatalogItem catalogItem = await _unitOfWork.CatalogItemRepository.CreateAsync(_mapper.Map<CatalogItem>(request.CatalogItem));
				response.CatalogItem = _mapper.Map<CatalogItemReadDTO>(catalogItem);
			} catch (Exception ex) {
				response.Success = false;
				response.AddErrorMessage(ex.Message);
			}

			return response;
		}

		public async Task<UpdateSingleResponse> UpdateSingleAsync(UpdateSingleRequest request) {

			UpdateSingleResponse response = new UpdateSingleResponse();

			try {
				CatalogItem catalogItem = await _unitOfWork.CatalogItemRepository.UpdateAsync(_mapper.Map<CatalogItem>(request.CatalogItem));
				response.CatalogItem = _mapper.Map<CatalogItemReadDTO>(catalogItem);
			} catch (Exception ex) {
				response.Success = false;
				response.AddErrorMessage(ex.Message);
			}

			return response;
		}

		public async Task<RemoveSingleResponse> RemoveSingleAsync(RemoveSingleRequest request) {

			RemoveSingleResponse response = new RemoveSingleResponse();

			try {
				await _unitOfWork.CatalogItemRepository.RemoveAsync(request.CatalogItemID);
			} catch (Exception ex) {
				response.Success = false;
				response.AddErrorMessage(ex.Message);
			}

			return response;
		}
	}
}