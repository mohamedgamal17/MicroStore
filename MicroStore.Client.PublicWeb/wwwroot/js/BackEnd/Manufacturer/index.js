$(document).ready(function () {

    $("#ManufacturersGrid").DataTable(
        abp.libs.datatables.normalizeConfiguration({
            ajax: {
                type: "POST"
            },

            columnDefs: [
                { tile: "Name", data: "name" },
                { tile: "Description", data: "description" },
                {
                    render: function (data, type, row) {
                        return `<a href="Manufacturer/Edit/${row.id}" class="btn btn-info">Edit</a>`
                }   },
            ]
        })
    )
})