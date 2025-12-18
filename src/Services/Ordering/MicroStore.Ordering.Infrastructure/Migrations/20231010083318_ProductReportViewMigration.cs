using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroStore.Ordering.Infrastructure.Migrations
{
    public partial class ProductReportViewMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sql = @"CREATE VIEW [dbo].[vw_ProductSalesReports]
                            WITH SCHEMABINDING
                            AS
                            SELECT [orderItem].[ExternalProductId] AS [ProductId] , 
	                                CONVERT(DATE,[orderEntity].[SubmissionDate])  AS [Date] , 
		                            SUM([orderItem].[Quantity]) AS [Quantity],
		                            SUM([orderItem].[UnitPrice] * [orderItem].[Quantity]) AS [TotalPrice],
	                                COUNT_BIG(*) AS [TotalTransactions]
                            FROM [dbo].[OrderStateEntity] as [orderEntity]
                            INNER JOIN [dbo].[OrderItemEntity] as [orderItem]
                            ON [orderItem].[OrderStateEntityCorrelationId] = [orderEntity].[CorrelationId]
                            WHERE [orderEntity].[CurrentState] = 'Completed'
                            GROUP by [orderItem].[ExternalProductId], CONVERT(DATE,[orderEntity].[SubmissionDate]) 
                         ";
            string clusterIndexSql = @"CREATE UNIQUE CLUSTERED INDEX [IX_Product_ProductId_Date]
                                ON [dbo].[vw_ProductSalesReports]([ProductId] ASC, [Date]  ASC)";



            migrationBuilder.Sql(sql);

            migrationBuilder.Sql(clusterIndexSql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sql = @"DROP VIEW [dbo].[vw_ProductSalesReports]";

            migrationBuilder.Sql(sql);
        }
    }
}
