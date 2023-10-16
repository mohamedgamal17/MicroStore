using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.Client.PublicWeb.Infrastructure;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using Volo.Abp.BlobStoring;

namespace MicroStore.Client.PublicWeb.Pages
{
    public class ImageSearchModel : PageModel
    {
        private readonly IBlobContainer<MultiMediaBlobContainer> _blobContainer;
        public List<Product> Products { get; set; }
        public string  TargetImage { get; set; }
        public bool HasErorr { get; set; }

        [BindProperty]
        public IFormFile Image { get; set; }
        public ImageSearchModel(IBlobContainer<MultiMediaBlobContainer> blobContainer)
        {
            _blobContainer = blobContainer;
        }

        public IActionResult OnGet(string image)
        {
            TargetImage = image;

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                HasErorr = true;

                return Page();
            }
           
            using(var ms = new MemoryStream())
            {

                string imageName = string.Format("{0}{1}", Guid.NewGuid().ToString(), Path.GetExtension(Image.FileName));

                await Image.CopyToAsync(ms);

                await _blobContainer.SaveAsync(imageName, ms);

                return RedirectToPage("ImageSearch", new { image = imageName });
            }

        }
    }
}
