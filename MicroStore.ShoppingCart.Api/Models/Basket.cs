using MicroStore.BuildingBlocks.Results;

namespace MicroStore.ShoppingCart.Api.Models
{
    public class Basket
    {
        public string UserId { get; set; }

        public List<BasketItem> Items { get; set; } 


        public Basket()
        {
            Items = new List<BasketItem>();
        }

        public void AddProduct(string productId , int quantity)
        {
           

            BasketItem? basketItem = Items.SingleOrDefault(x => x.ProductId == productId);

            if(basketItem == null)
            {
                basketItem = new BasketItem
                {
                    ProductId = productId
                };

                Items.Add(basketItem);
            }

            basketItem.Quantity += quantity;

            if(basketItem.Quantity <= 0)
            {
                Items!.Remove(basketItem);
            }
        }


        public UnitResult RemoveProduct(string productId)
        {

            BasketItem? basketItem = Items.SingleOrDefault(x => x.ProductId == productId);

            if(basketItem == null)
            {
                return UnitResult.Failure(string.Empty,"Basket item not found");
            }


            Items!.Remove(basketItem);


            return UnitResult.Success();
        }



        public void Migrate(Basket basket)
        {
            basket.Items.ForEach(item =>
            {
                AddProduct(item.ProductId, item.Quantity);
            });
        }



        public void Clear()
        {
            Items.Clear();
        }
    }
}
