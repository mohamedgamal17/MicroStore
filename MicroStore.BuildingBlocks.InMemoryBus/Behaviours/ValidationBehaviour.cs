﻿using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Volo.Abp.Validation;
namespace MicroStore.BuildingBlocks.InMemoryBus.Behaviours
{
    public class ValidationBehaviour<TRequest> : RequestMiddleware<TRequest, ResponseResult>
        where TRequest : IRequest<ResponseResult>
    {

        private readonly IObjectValidator _objectValidator;

        public ValidationBehaviour(IObjectValidator objectValidator)
        {
            _objectValidator = objectValidator;
        }

        public override async Task<ResponseResult> Handle(TRequest request, RequestHandlerDelegate<ResponseResult> next, CancellationToken cancellationToken)
        {
            var validationErrors = await _objectValidator.GetErrorsAsync(request, typeof(TRequest).Name);

            if (validationErrors.Any())
            {
                return ResponseResult.Failure((int)HttpStatusCode.BadRequest, ConvertValidationErrors(validationErrors));
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