$(function () {
   var shipmentTable = $("#ShipmentTable").DataTable(
        abp.libs.datatables.normalizeConfiguration({
            ajax: {
                method: "POST",
                data: function (data) {
                    data.startDate = $("#StartDate").val();
                    data.endDate = $("#EndDate").val();
                    data.status = $("#Status").val();
                    data.country = $("#Country").val();
                    data.trackingNumber = $("#TrackingNumber").val();
                    data.orderNumber = $("#OrderNumber").val();
                }
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
                    data: "userId",
                    orderable: false,
                },
                {
                    title: "Status",
                    data: "status",
                    orderable: false,
                },
                {
                    title: "Created At",
                    data : "createdAt"
                },

                {
                    orderable: false,
                    title: "Action",
                    render: function (data, type, row) {
                        return `<a href="Shipment/Details/${row.id}" class="btn btn-info">View</a>`
                    }

                }
            ]

        })
    );

    $("#Status").select2();

    $("#Country").select2();


    $("#AdvancedSearchForm").on('submit', function (evt) {
        evt.preventDefault();
        shipmentTable.ajax.reload();
    });
})