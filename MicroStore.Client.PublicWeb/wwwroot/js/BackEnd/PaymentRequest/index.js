$(document).ready(function () {
    $("#PaymentRequestsTable").DataTable(
        abp.libs.datatables.normailizeConfiguration({
            ajax: {
                type: "POST"
            },
            paging: true,
            serverSide: true,
            columnDefs: [
                { title: "Order Id", data: "orderId" },
                { title: 'Order Number', data: "orderNumber" },
                { title: 'User Id', name: "userId" },
                { title: 'Total Cost', name: "totalCost" },
                { title: 'Status', name: "status" },
                { title: 'Created At', name: "createdAt" },
                {
                    title: "Actions",
                    render: function (data, type, row) {
                        return `<a href="PaymentRequest/Detail/${row.id}" class="btn btn-info">View</a>`
                    }
                }
            ]

        })
    );
})