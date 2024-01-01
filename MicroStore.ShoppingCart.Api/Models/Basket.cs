using MicroStore.BuildingBlocks.Utils.Results;
using Volo.Abp.Domain.Entities;

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


        public Result<Unit> RemoveProduct(string productId , int? count )
        {
            BasketItem? basketItem = Items.SingleOrDefault(x => x.ProductId == productId);

            if(basketItem == null)
            {
                return new Result<Unit>(new EntityNotFoundException(typeof(BasketItem), productId));
            }


            if(count == null || basketItem.Quantity <= count.Value)
            {
                Items.Remove(basketItem);
            }
            else
            {
                basketItem.Quantity -= count.Value;
            }


            return Unit.Value;
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
