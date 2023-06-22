$(function () {
    $("#ShipmentTable").DataTable(
        abp.libs.datatables.normalizeConfiguration({
            ajax: {
                method:"POST"
            },
            paging: true,
            serverSide: true,
            columnDefs: [
                {
                    title: "Order Number",
                    data: "orderNumber"
                },
                {
                    title: "User Id",
                    data: "userId"
                },
                {
                    title: "Status",
                    data: "status"
                },

                {
                    title: "Action",
                    render: function (data, type, row) {
                        return `<a href="Shipment/Details/${row.id}" class="btn btn-info">View</a>`
                    }

                }
            ]

        })
    );


})