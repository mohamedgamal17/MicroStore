using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable
namespace MicroStore.Ordering.Infrastructure.Migrations
{
    public partial class OrderReportViewMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string viewSql = @"CREATE VIEW [dbo].[vw_OrderSalesReports]
                            WITH SCHEMABINDING
                            AS
                            SELECT CONVERT(DATE,[order].[SubmissionDate]) AS [Date], 
	                             [order].[CurrentState] AS [CurrentState],
	                             SUM([order].[TotalPrice]) AS [TotalPrice],
	                             SUM([order].[TaxCost]) AS [TotalTaxPrice],
	                             SUM([order].[ShippingCost]) AS [TotalShippingPrice],
	                             COUNT_BIG(*) AS [TotalOrders]
                            FROM [dbo].[OrderStateEntity] AS [order]
                            GROUP BY [order].CurrentState , CONVERT(DATE,[order].[SubmissionDate])
                        ";
            string clusterIndexSql = @"CREATE UNIQUE CLUSTERED INDEX [PK_Order_SubmissionDate_CurrentState]
                                        ON [dbo].[vw_OrderSalesReports]([Date] ASC ,                                    [CurrentState] ASC)
                                      ";



            migrationBuilder.Sql(viewSql);

            migrationBuilder.Sql(clusterIndexSql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sql = @"DROP VIEW [dbo].[vw_OrderSalesReports]";

            migrationBuilder.Sql(sql);
        }
    }
}
