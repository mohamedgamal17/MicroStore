using MicroStore.Ordering.Application.Domain;

namespace MicroStore.Ordering.Application.Extensions
{
    public static class ProductReportExtensions
    {

        public static  IQueryable<ProductSalesReport> ProjectToProductSummaryReport<TKey>(this IQueryable<IGrouping<TKey, OrderStateEntity >> query )
        {
            var projection = from or in query
                             orderby or.First().SubmissionDate ascending
                             select new ProductSalesReport
                             {
                                 Quantity = or.Sum(x => x.OrderItems.First().Quantity),
                                 Date = or.First().SubmissionDate

                             };


            return projection;
        }
    }
}
