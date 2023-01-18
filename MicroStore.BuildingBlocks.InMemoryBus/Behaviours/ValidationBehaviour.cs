using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Volo.Abp.Validation;
namespace MicroStore.BuildingBlocks.InMemoryBus.Behaviours
{
    public class ValidationBehaviour<TRequest,TResponse> : RequestMiddleware<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {

        private readonly IObjectValidator _objectValidator;

        public ValidationBehaviour(IObjectValidator objectValidator)
        {
            _objectValidator = objectValidator;
        }

        public override async Task<ResponseResult<TResponse>> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var validationErrors = await _objectValidator.GetErrorsAsync(request, typeof(TRequest).Name).ConfigureAwait(false);

            if (validationErrors.Any())
            {
                return ResponseResult.Failure<TResponse>((int)HttpStatusCode.BadRequest, ConvertValidationErrors(validationErrors));
            }

            return await next();
        }

        private ErrorInfo ConvertValidationErrors(List<ValidationResult> validationResults)
        {
            return new ErrorInfo
            {
                Message = "Validation error",
                ValidationErrors = validationResults.Select(x => new ValidationErrorInfo
                {
                    Members = x.MemberNames.ToArray(),
                    Message = x.ErrorMessage
                }).ToArray()
            };

        }
    }
}
