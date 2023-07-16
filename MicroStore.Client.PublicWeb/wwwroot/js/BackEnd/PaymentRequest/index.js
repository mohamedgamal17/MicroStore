$(function () {
     var paymentTable = $("#PaymentRequestsTable").DataTable(
        abp.libs.datatables.normalizeConfiguration({
            ajax: {
                type: "POST",
                data: function (data) {
                    data.startDate = $("#StartDate").val();
                    data.endDate = $("#EndDate").val();
                    data.status = $("#Status").val();
                    data.minPrice = $("#MinPrice").val();
                    data.maxPrice = $("#MaxPrice").val();
                    data.orderNumber = $("#OrderNumber").val();
                }
            },
            serverSide: true,
            processing: true,
            paging: true,
            searching: false,

      
            columnDefs: [
                {
                    title: 'Order Number',
                    data: "orderNumber",
                    orderable: false
                },
                {
                    title: 'Total Cost',
                    data: "totalCost"
                },
                {
                    title: 'Status',
                    data: "status",
                    orderable: false
                },
                {
                    title: 'Created At',
                    data: "createdAt"
                },
                {
                    orderable: false,
                    title: "Actions",
                    render: function (data, type, row) {
                        return `<a href="PaymentRequest/Details/${row.id}" class="btn btn-info">View</a>`
                    }
                }
            ]

        })
    );

    $("#Status").select2();
    $("#AdvancedSearchForm").on('submit', function (evt) {
        evt.preventDefault();
        paymentTable.ajax.reload();
    });
})