using FluentValidation;
using FluentValidation.Results;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.Catalog.Application.Abstractions.Manufacturers;
using Volo.Abp.DependencyInjection;
namespace MicroStore.Catalog.Api.Grpc
{
    public class ManufacturerGrpcService : ManufacturerService.ManufacturerServiceBase
    {
        private readonly IManufacturerCommandService _manufacturerCommandService;

        private readonly IManufacturerQueryService _manufacturerQueryService;

        public ManufacturerGrpcService(IManufacturerQueryService manufacturerQueryService)
        {
            _manufacturerQueryService = manufacturerQueryService;
        }

        public ManufacturerGrpcService(IManufacturerCommandService manufacturerCommandService)
        {
            _manufacturerCommandService = manufacturerCommandService;
        }

        public IAbpLazyServiceProvider LazyServiceProvider { get; set; }

        public override async Task<ManufacturerResponse> Create(CreateManufacturerRequest request, ServerCallContext context)
        {
            var model = PrepareManufacturerModel(request);

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }

            var result = await _manufacturerCommandService.CreateAsync(model);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }


            return PrepareManufacturerResponse(result.Value);

        }


        public override async Task<ManufacturerResponse> Update(UpdateManufacturerRequest request, ServerCallContext context)
        {
            var model = PrepareManufacturerModel(request);

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }

            var result = await _manufacturerCommandService.UpdateAsync(request.Id,model);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareManufacturerResponse(result.Value);

        }

        public override async Task<ManufacturerListResponse> GetList(ManufacturerListRequest request, ServerCallContext context)
        {
            var queryParams = new ManufacturerListQueryModel
            {
                Name = request.Name,
                SortBy = request.SortBy,
                Desc = request.Desc
            };

            var validationResult = await ValidateModel(queryParams);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }

            var result = await _manufacturerQueryService.ListAsync(queryParams);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareManufacturerListResponse(result.Value);
        }


        public override async Task<ManufacturerResponse> GetById(GetManufacturerByIdRequest request, ServerCallContext context)
        {
            var result = await _manufacturerQueryService.GetAsync(request.Id);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareManufacturerResponse(result.Value);
        }


        private ManufacturerModel PrepareManufacturerModel(CreateManufacturerRequest request)
        {
            return new ManufacturerModel
            {
                Name = request.Name,
                Description = request.Description
            };
        }

        private ManufacturerModel PrepareManufacturerModel(UpdateManufacturerRequest request)
        {
            return new ManufacturerModel
            {
                Name = request.Name,
                Description = request.Description
            };
        }
        private ManufacturerResponse PrepareManufacturerResponse(ManufacturerDto manufacturer)
        {
            return new ManufacturerResponse
            {
                Id = manufacturer.Id,
                Name = manufacturer.Name,
                Description = manufacturer.Description,
                CreatedAt = Timestamp.FromDateTime(manufacturer.CreationTime)
            };
        }

        private ManufacturerListResponse PrepareManufacturerListResponse(List<ManufacturerDto> manufacturers)
        {
            var response = new ManufacturerListResponse();

            manufacturers.ForEach(x => response.Data.Add(PrepareManufacturerResponse(x)));

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
