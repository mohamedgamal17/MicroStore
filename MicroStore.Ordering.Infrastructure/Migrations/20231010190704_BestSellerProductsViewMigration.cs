using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroStore.Ordering.Infrastructure.Migrations
{
    public partial class BestSellerProductsViewMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string viewSql = @"
                            CREATE VIEW [vw_BestSellerProducts]
                            WITH SCHEMABINDING
                            AS
                            SELECT [item].[ExternalProductId] AS [ProductId],
	                            SUM([item].[Quantity]) AS [Quantity],
	                            SUM([item].[Quantity] * [item].[UnitPrice]) AS [Amount],
	                            COUNT_BIG(*) AS [TotalTransactions]
                            FROM [dbo].[OrderStateEntity] AS [order]
                            INNER JOIN [dbo].[OrderItemEntity] AS [item]
                            on [order].[CorrelationId] = [item].[OrderStateEntityCorrelationId] 
                            WHERE [order].[CurrentState] = 'Completed'
                            GROUP BY [item].[ExternalProductId]
                          ";

            string clusterSql = @"
                                 CREATE UNIQUE CLUSTERED INDEX [PK_ProductId]
                                 ON [dbo].[vw_BestSellerProducts]([ProductId])
                                ";

            migrationBuilder.Sql(viewSql);
            migrationBuilder.Sql(clusterSql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sql = "DROP VIEW [dbo].[vw_BestSellerProducts]";

            migrationBuilder.Sql(sql);
        }
    }
}
