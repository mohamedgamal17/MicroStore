using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.AspNetCore.UI;
using MicroStore.Client.PublicWeb.Controllers;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Cart;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Cart;
namespace MicroStore.Client.PublicWeb.Pages.FrontEnd
{
    public class CartModel : PageModel
    {
        private readonly IWorkContext _workContext;

        private readonly BasketAggregateService _basketAggregateService;

        private readonly BasketService _basketService;
        public BasketAggregate Basket { get; set; }
        public double SubTotal { get; set; } = 0;
        public double TotalPrice { get; set; } = 0;


        public CartModel(BasketAggregateService basketAggregateService, IWorkContext workContext, BasketService basketService)
        {
            _basketAggregateService = basketAggregateService;
            _workContext = workContext;
            _basketService = basketService;
        }

        public async Task<IActionResult> OnGet()
        {
            await BuildModel();

            return Page();
        }


        public async Task<IActionResult> OnPostRemoveItem(RemoveBasketItemModel  model)
        {
            var options = new BasketRemoveItemRequestOptions
            {
                ProductIds = new string[] { model.ProductId.ToString() }
            };

            var basketResponse = await _basketService.RemoveItemsAsync(_workContext.TryToGetCurrentUserId(), options);

            await BuildModel();

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateBasket(BasketModel model)
        {
            var options = new BasketRequestOptions
            {
                Items = model.Items.Where(x=> x.Quantity > 0).Select(x => new BaskeItemRequestOptions { ProductId = x.ProductId.ToString(), Quantity = x.Quantity }).ToList()
            };


            await _basketService.UpdateAsync(_workContext.TryToGetCurrentUserId(), options);

            await BuildModel();

            return Page();
        }


        private async Task BuildModel()
        {
            var userId = _workContext.TryToGetCurrentUserId();

            Basket = await _basketAggregateService.RetriveBasket(userId);

            Basket.Items.ForEach((item) => SubTotal += item.Quantity * item.Price);

            Basket.Items.ForEach((item) => TotalPrice += item.Quantity * item.Price);

        }



    }
}
