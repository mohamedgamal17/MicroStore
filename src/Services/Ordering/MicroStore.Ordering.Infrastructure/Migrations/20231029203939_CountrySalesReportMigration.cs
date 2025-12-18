using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroStore.Ordering.Infrastructure.Migrations
{
    public partial class CountrySalesReportMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string salesReportView = @"
                        CREATE VIEW [dbo].[vw_CountrySalesReports]
                            WITH SCHEMABINDING
                            AS
						    SELECT CONVERT(DATE,[order].[SubmissionDate]) AS [Date], 
	                                 [order].[CurrentState] AS [CurrentState],
								     [order].[ShippingAddress_CountryCode] AS [CountryCode],
	                                 SUM([order].[TotalPrice]) AS [TotalPrice],
	                                 SUM([order].[TaxCost]) AS [TotalTaxPrice],
	                                 SUM([order].[ShippingCost]) AS [TotalShippingPrice],
	                                 COUNT_BIG(*) AS [TotalOrders]
                                FROM [dbo].[OrderStateEntity] AS [order]
							    WHERE [order].CurrentState = 'Completed'
                                GROUP BY [order].CurrentState , CONVERT(DATE,[order].[SubmissionDate]) ,      [order].[ShippingAddress_CountryCode]

                        ";

            string salesSummaryView = @"
                                        CREATE VIEW [dbo].[vw_CountrySalesSummaryReports]
                                        WITH SCHEMABINDING
                                        AS
							            SELECT [order].[CurrentState] AS [CurrentState],
								             [order].[ShippingAddress_CountryCode] AS [CountryCode],
	                                         SUM([order].[TotalPrice]) AS [TotalPrice],
	                                         SUM([order].[TaxCost]) AS [TotalTaxPrice],
	                                         SUM([order].[ShippingCost]) AS [TotalShippingPrice],
	                                         COUNT_BIG(*) AS [TotalOrders]
                                        FROM [dbo].[OrderStateEntity] AS [order]
							            WHERE [order].CurrentState = 'Completed'
                                        GROUP BY [order].CurrentState ,  [order].[ShippingAddress_CountryCode]      
                                    ";


            migrationBuilder.Sql(salesReportView);
            migrationBuilder.Sql(salesSummaryView);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sql1 = "DROP VIEW [dbo].[vw_CountrySalesSummaryReports]";

            string sql2 = "DROP VIEW [dbo].[vw_CountrySalesReports]";

            migrationBuilder.Sql(sql1);

            migrationBuilder.Sql(sql2);
        }
    }
}
