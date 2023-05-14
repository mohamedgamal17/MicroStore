$(document).ready(function () {


    var table = $("#ProductsTable").DataTable(
        abp.libs.datatables.normalizeConfiguration({
            ajax: {
                type: "POST"
            },

            serverSide: true,
            searching: false,
            columnDefs: [
                {
                    title: 'Name',
                    data: 'name'
                },
                {
                    title: 'Sku',
                    data: 'sku',

                },
                {
                    title: 'Price',
                    data: 'price'
                },
                {
                    title: "Actions",
                    render: function (data, type, row) {
                        return `<a  href="Product/Edit/${row.id}" class="btn btn-info" >Edit</a>`
                    }
                }
            ]
        })

    );

});