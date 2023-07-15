$(document).ready(function () {
    var table = $("#ProductsTable").DataTable(
        abp.libs.datatables.normalizeConfiguration({
            ajax: {
                type: "POST",
                data: function (data) {
                    data.name = $("#Name").val();
                    data.category = $("#Category").val();
                    data.manufacturer = $("Manufacturer").val();
                    data.tag = $("#Tag").val();
                    data.minPrice = $("#MinPrice").val();
                    data.maxPrice = $("#MaxPrice").val();
                }
            },

            paging: true,
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
                    orderable: false,
                    title: "Actions",
                    render: function (data, type, row) {
                        return `<a  href="Product/Edit/${row.id}" class="btn btn-info" >Edit</a>`
                    }
                }
            ]
        })

    );


    $("#AdvancedSearchForm").on('submit', function (evt) {
        evt.preventDefault();
        table.ajax.reload();
    });

    $("#Category").select2({
        placeholder: "Please select category",
    })

    $("#Manufacturer").select2({
        placeholder: "Please select manufacturer",
    })

});