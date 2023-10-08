using MicroStore.Ordering.Application.Dtos;
using MicroStore.Ordering.Application.StateMachines;

namespace MicroStore.Ordering.Application.Extensions
{
    public static class ProductReportExtensions
    {

        public static  IQueryable<ProductSummaryReport> ProjectToProductSummaryReport<TKey>(this IQueryable<IGrouping<TKey, OrderStateEntity >> query )
        {
            var projection = from or in query
                             orderby or.First().SubmissionDate ascending
                             select new ProductSummaryReport
                             {
                                 Min = or.Min(x => x.OrderItems.First().Quantity),
                                 Max = or.Max(x => x.OrderItems.First().Quantity),
                                 Units = or.Sum(x => x.OrderItems.First().Quantity),
                                 Average = or.Average(x => x.OrderItems.First().Quantity),
                                 Date = or.First().SubmissionDate.ToString("MM-dd-yyyy")

                             };


            return projection;
        }
    }
}
