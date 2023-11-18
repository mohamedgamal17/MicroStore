﻿namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Billing
{

    public class PaymentRequestOptions
    {
        public string OrderId { get; set; }
        public string OrderNumber { get; set; }
        public double ShippingCost { get; set; }
        public double TaxCost { get; set; }
        public double SubTotal { get; set; }
        public double TotalCost { get; set; }
        public List<PaymentProductCreateRequestOptions> Items { get; set; }
    }
  
    public class PaymentCreateRequestOptions : PaymentRequestOptions
    {
        public string UserName { get; set; }
    }

    public class PaymentProcessRequestOptions
    {
        public string GatewayName { get; set; }
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
    }


    public class PaymentCompleteRequestOptions
    {
        public string GatewayName { get; set; }
        public string SessionId { get; set; }
    }

    public class PaymentProductCreateRequestOptions
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
    }

    public class PaymentListRequestOptions : PagingAndSortingRequestOptions
    {
        public string OrderNumber { get; set; }
        public string Status { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
