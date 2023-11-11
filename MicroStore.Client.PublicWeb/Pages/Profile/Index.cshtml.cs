using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.AspNetCore.UI;
using MicroStore.Client.PublicWeb.Extensions;
using MicroStore.Client.PublicWeb.Infrastructure;
using MicroStore.Client.PublicWeb.Models;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Profiling;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Profiling;
using Volo.Abp.BlobStoring;
namespace MicroStore.Client.PublicWeb.Pages.Profile
{
    [Authorize]
    public class IndexModel : PageModel
    {

        private readonly UserProfileService _userPorfileService;

        private readonly UINotificationManager _notificationManager;

        private readonly IBlobContainer<MultiMediaBlobContainer> _blobContainer;
        public User UserProfile { get; set; }
        public UserProfileModel Profile { get; set; }
        public IndexModel(UserProfileService userPorfileService, UINotificationManager notificationManager, IBlobContainer<MultiMediaBlobContainer> blobContainer)
        {
            _userPorfileService = userPorfileService;
            _notificationManager = notificationManager;
            _blobContainer = blobContainer;
        }
        public override async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            try
            {
                UserProfile = await _userPorfileService.GetAsync();

                await next();

            }
            catch (MicroStoreClientException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {

                context.Result = RedirectToPage("/Profile/Create", new { returnUrl = context.HttpContext.Request.Path });
            }
        }
        public IActionResult OnGet()
        {
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var requestOptions = new ProfileRequestOptions
            {
                FirstName = Profile.FirstName,
                LastName = Profile.LastName,
                BirthDate = Profile.BirthDate,
                Phone = Profile.Phone,
                Gender = Profile.Gender,
                Avatar = await GetUserImageLink(Profile.Avatar)
            };


            await _userPorfileService.UpdateAsync(requestOptions);

            _notificationManager.Success("Your profile has been successfully updated!");

            return Page();
        }

        public async Task<string?> GetUserImageLink(IFormFile? formFile)
        {
            if (formFile == null) return null;

            string imageName = string.Format("{0}{1}", Guid.NewGuid().ToString(), Path.GetExtension(Profile.Avatar.FileName));

            using (MemoryStream memoryStream = new MemoryStream())
            {
                await Profile.Avatar.CopyToAsync(memoryStream);

                await _blobContainer.SaveAsync(imageName, memoryStream.ToArray());
            }

            return HttpContext.GenerateFileLink(imageName);
        }

    }
}
