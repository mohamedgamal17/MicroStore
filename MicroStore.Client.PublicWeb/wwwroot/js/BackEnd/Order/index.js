$(function () {
    var orderTable = $("#OrderTable").DataTable(
        abp.libs.datatables.normalizeConfiguration({
            ajax: {
                type: "POST",
                data: function (data) {
                    data.startSubmissionDate = $("#StartSubmissionDate").val();
                    data.endSubmissionDate = $("#EndSubmissionDate").val();
                    data.orderNumber = $("#OrderNumber").val();
                    data.states = $("#States").val();
                    console.log($("#States").val())
                },
                fnServerParams: function (data) {
                    var startSubmissionDate = $("#StartSubmissionDate").val();
                    var endSubmissionDate = $("#EndSubmissionDate").val();
                    var orderNumber = $("#OrderNumber").val();
                    var states = $("#States").val();
                    data.push({ name: "startSubmissionDate", value: startSubmissionDate })
                    data.push({ name: "endSubmissionDate", value: endSubmissionDate })
                    data.push({ name: "orderNumber", value: orderNumber })
                    data.push({ name: "states", value: states })
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
                    title: "Current State",
                    data: "currentState"
                },
                {
                    title: "Total Price",
                    data: "totalPrice"
                },
                {
                    title: "Actions",
                    render: function (data, type, row) {
                        return `<a href="Order/Details/${row.id}" class="btn btn-info"> View</a>`
                    }
                }
            ],
        })
    )

    $("#States").select2({
        placeholder: 'select order states'
    })


    $("#AdvancedSearchForm").on('submit', function (evt) {
        evt.preventDefault();
        console.log($("#States").val())
        orderTable.ajax.reload();
    });

})