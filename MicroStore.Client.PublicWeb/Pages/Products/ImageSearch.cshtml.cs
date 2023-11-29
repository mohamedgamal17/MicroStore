using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.Client.PublicWeb.Infrastructure;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MimeMapping;
namespace MicroStore.Client.PublicWeb.Pages.Products
{
    [CheckProfilePageCompletedFilter]
    public class ImageSearchModel : PageModel
    {
        private readonly IObjectStorageProvider _objectStorageProvider;



        public List<Product> Products { get; set; }
        public string TargetImage { get; set; }
        public string ImageLink { get;  set; }
        public bool HasErorr { get; set; }

        [BindProperty]
        public IFormFile Image { get; set; }

        public ImageSearchModel(IObjectStorageProvider objectStorageProvider)
        {
            _objectStorageProvider = objectStorageProvider;
        }
        public async Task<IActionResult> OnGet(string image)
        {
            TargetImage = image;
            ImageLink = await _objectStorageProvider.CalculatePublicReferenceUrl(image);

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                HasErorr = true;

                return Page();
            }

            using (var ms = new MemoryStream())
            {

                string imageName = string.Format("{0}{1}", Guid.NewGuid().ToString(), Path.GetExtension(Image.FileName));

                await Image.CopyToAsync(ms);

                ms.Seek(0, SeekOrigin.Begin);

                var args = new S3ObjectSaveArgs
                {
                    Name = imageName,
                    Data = ms,
                    ContentType = MimeUtility.GetMimeMapping(Image.FileName)
                };

                await _objectStorageProvider.SaveAsync(args);

                return RedirectToPage("/Products/ImageSearch", new { image = imageName });
            }

        }

    }
}
