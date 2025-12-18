using MicroStore.Ordering.Application.Domain;
namespace MicroStore.Ordering.Application.Extensions
{
    public static class OrderReportExtension
    {
        public static IQueryable<OrderSalesReport> ProjectToOrderSummaryReport<TKey>(this IQueryable<IGrouping<TKey, OrderStateEntity>> query)
        {
            var projection = from or in query
                             orderby or.First().SubmissionDate ascending
                             select new OrderSalesReport
                             {
                                 TotalOrders = or.Count(),
                                 TotalShippingPrice = or.Sum(x => x.ShippingCost),
                                 TotalTaxPrice = or.Sum(x => x.TaxCost),
                                 TotalPrice = or.Sum(x => x.TotalPrice),
                                 Date = or.First().SubmissionDate
                             };

            return projection;
        }
    }
}
