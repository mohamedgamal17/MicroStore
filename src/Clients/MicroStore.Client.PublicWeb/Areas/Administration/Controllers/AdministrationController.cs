using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using MicroStore.AspNetCore.UI;
using MicroStore.Client.PublicWeb.Consts;
using MicroStore.Client.PublicWeb.Extensions;
using MicroStore.Client.PublicWeb.Infrastructure;
using Volo.Abp.AspNetCore.Mvc;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Controllers
{
    [Area(AreaNames.Administration)]
    [CheckProfileActionCompletedFilterAttribute]
    public  abstract class AdministrationController : AbpController
    {
        public UINotificationManager NotificationManager => LazyServiceProvider.LazyGetRequiredService<UINotificationManager>();



        protected async Task<ValidationResult> ValidateModel<TModel>(TModel model)
        {
            var validator = ResolveValidator<TModel>();

            if (validator == null) return new ValidationResult();

            var result = await validator.ValidateAsync(model);

            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
            }

            return result;
        }

        private IValidator<T>? ResolveValidator<T>()
        {
            return LazyServiceProvider.LazyGetService<IValidator<T>>();
        }
    }
}
