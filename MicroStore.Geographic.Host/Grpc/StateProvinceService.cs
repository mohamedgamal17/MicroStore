using FluentValidation;
using FluentValidation.Results;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MicroStore.Geographic.Application.Dtos;
using MicroStore.Geographic.Application.Models;
using Volo.Abp.DependencyInjection;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.Geographic.Application.StateProvinces;
namespace MicroStore.Geographic.Host.Grpc
{
    public class StateProvinceGrpcService : StateProvinceService.StateProvinceServiceBase
    {
        private readonly IStateProvinceApplicationService _stateProvinceApplicationService;

        public StateProvinceGrpcService(IStateProvinceApplicationService stateProvinceApplicationService)
        {
            _stateProvinceApplicationService = stateProvinceApplicationService;
        }

        public IAbpLazyServiceProvider LazyServiceProvider { get; set; }

        private StateProvinceResponse PrepareStateProvinceResponse(StateProvinceDto stateProvince)
        {
            return new StateProvinceResponse
            {
                Id = stateProvince.Id,
                Name = stateProvince.Name,
                Abbrevation = stateProvince.Abbreviation,
                CreatedAt = Timestamp.FromDateTime(stateProvince.CreationTime)
            };
        }

        public override async Task<StateProvinceResponse> Create(CreateStateProvinceRequest request, ServerCallContext context)
        {
            var model = PrepareStateProvinceModel(request);

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }

            var result = await _stateProvinceApplicationService.CreateAsync(request.CountryId, model);

            return PrepareStateProvinceResponse(result.Value);
        }

        public override async Task<StateProvinceResponse> Update(UpdateStateProvinceRequest request, ServerCallContext context)
        {
            var model = PrepareStateProvinceModel(request);

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }

            var result = await _stateProvinceApplicationService.UpdateAsync(request.CountryId, request.StateProvinceId,model);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareStateProvinceResponse(result.Value);
        }

        public override async Task<StateProvinceListResponse> GetList(StateProvinceListRequest request, ServerCallContext context)
        {
            var result = await _stateProvinceApplicationService.ListAsync(request.CountryId);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareStateProvinceListResponse(result.Value);
        }

        public override async Task<StateProvinceResponse> GetById(GetStateProvinceByIdRequest request, ServerCallContext context)
        {
            var result = await _stateProvinceApplicationService.GetAsync(request.CountryId,request.StateProvinceId);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }


            return PrepareStateProvinceResponse(result.Value);
        }

        public override async Task<StateProvinceResponse> GetByCode(GetStateProvinceByCodeRequest request, ServerCallContext context)
        {
            var result = await _stateProvinceApplicationService.GetByCodeAsync(request.CountryCode, request.StateProvinceCode);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareStateProvinceResponse(result.Value);
        }

        private StateProvinceModel PrepareStateProvinceModel(CreateStateProvinceRequest request)
        {
            return new StateProvinceModel
            {
                Name = request.Name,
                Abbreviation = request.Abbrevation
            };
        }

        private StateProvinceModel PrepareStateProvinceModel(UpdateStateProvinceRequest request)
        {
            return new StateProvinceModel
            {
                Name = request.Name,
                Abbreviation = request.Abbrevation
            };
        }
        private StateProvinceListResponse PrepareStateProvinceListResponse(List<StateProvinceDto> stateProvinces)
        {
            var response = new StateProvinceListResponse();

            stateProvinces.ForEach(state =>
            {
                response.Items.Add(PrepareStateProvinceResponse(state));
            });

            return response;
        }

        private async Task<ValidationResult> ValidateModel<TModel>(TModel model)
        {
            var validator = ResolveValidator<TModel>();

            if (validator == null) return new ValidationResult();

            return await validator.ValidateAsync(model);
        }
        private IValidator<T>? ResolveValidator<T>()
        {
            return LazyServiceProvider.LazyGetService<IValidator<T>>();
        }
    }
}
