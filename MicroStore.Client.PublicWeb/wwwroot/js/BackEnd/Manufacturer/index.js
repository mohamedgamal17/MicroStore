$(document).ready(function () {
    var manufacturerTable = $("#ManufacturersGrid").DataTable(
        abp.libs.datatables.normalizeConfiguration({
            paging: true,
            processing: true,
            searching: false,
            scrollX: true,
            ajax: {
                type: "POST",
                data: function (data) {
                    data.name = $("#Name").val();
                },
            },

            columnDefs: [
                {
                    title: "Name",
                    data: "name",
                },
                {
                    title: "Description",
                    data: "description",
                    orderable: false,
                },
                {
                    orderable: false,
                    render: function (data, type, row) {
                        return `<a href="Manufacturer/Edit/${row.id}" class="btn btn-info">Edit</a>`
                }   },
            ]
        })
    )

    $("#AdvancedSearchForm").on('submit', function (evt) {
        evt.preventDefault();
        manufacturerTable.ajax.reload();
    });
})