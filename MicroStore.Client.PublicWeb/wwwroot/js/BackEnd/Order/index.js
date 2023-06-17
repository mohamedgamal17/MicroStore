$(function () {
    $("#OrderTable").DataTable(
        abp.libs.datatables.normalizeConfiguration({
            ajax: {
                type: "POST"
            },
            paging: true,
            serverSide: true,
            columnDefs: [
                {
                    title: "Order Number",
                    data: "orderNumber"
                },

                {
                    title: "Current State",
                    data: "currentState"
                },
                {
                    title: "Total Price",
                    data: "totalPrice"
                },
                {
                    title: "Actions",
                    render:function (data, type, row) {
                       return `<a href="Order/Details/${row.id}" class="btn btn-info"> View</a>`
                    }
                }
            ],
        })
    )

})