using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace MicroStore.Client.PublicWeb.Pages
{
    public class ProductsModel : PageModel
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; } = 24;
        public string? Category { get; set; }
        public string? Manufacturer { get; set; }
        public bool IsFeatured { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public ProductsModel()
        {
        }

        public IActionResult OnGet(int currentPage = 1, bool isFeatured = false, string? category = null, string? manufacturer = null, double? minPrice = null, double? maxPrice = null)
        {
            CurrentPage = currentPage;
            Category = category;
            Manufacturer = manufacturer;
            IsFeatured = isFeatured;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
            return Page();
        }
    }
}
