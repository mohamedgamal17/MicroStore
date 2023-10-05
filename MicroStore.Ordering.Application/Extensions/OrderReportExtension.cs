using MicroStore.Ordering.Application.Dtos;
using MicroStore.Ordering.Application.StateMachines;
namespace MicroStore.Ordering.Application.Extensions
{
    public static class OrderReportExtension
    {
        public static IQueryable<OrderSummaryReport> ProjectToOrderSummaryReport<TKey>(this IQueryable<IGrouping<TKey, OrderStateEntity>> query, string dateFormate)
        {
            var projection = from or in query
                             orderby or.First().SubmissionDate ascending
                             select new OrderSummaryReport
                             {
                                 TotalOrders = or.Count(),
                                 SumShippingTotalCost = or.Sum(x => x.ShippingCost),
                                 SumTaxTotalCost = or.Sum(x => x.TaxCost),
                                 SumSubTotalCost = or.Sum(x => x.SubTotal),
                                 SumTotalCost = or.Sum(x => x.TotalPrice),
                                 Date = or.First().SubmissionDate.ToString(dateFormate)

                             };

            return projection;
        }
    }
}
