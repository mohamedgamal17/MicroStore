$(document).ready(function () {
    $("#PaymentRequestsTable").DataTable(
        abp.libs.datatables.normalizeConfiguration({
            ajax: {
                type: "POST"
            },
            paging: true,
            serverSide: true,
            columnDefs: [
                { title: "Order Id", data: "orderId" },
                { title: 'Order Number', data: "orderNumber" },
                { title: 'User Id', data: "userId" },
                { title: 'Total Cost', data: "totalCost" },
                { title: 'Status', data: "status" },
                { title: 'Created At', data: "createdAt" },
                {
                    title: "Actions",
                    render: function (data, type, row) {
                        return `<a href="PaymentRequest/Details/${row.id}" class="btn btn-info">View</a>`
                    }
                }
            ]

        })
    );
})