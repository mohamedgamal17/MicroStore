$(document).ready(function () {
    $("#CategoriesTable").DataTable(
        abp.libs.datatables.normalizeConfiguration({
            paging: true,
            processing: true,
            searching: false,
            scrollX: true,
            ajax: {
                type: "POST"
            },
            columnDefs: [

                {
                    title: "Name",
                    data: "name"
                },
                {
                    title: "Description",
                    data: "description"
                },
                {
                    title: "Action",
                    render: function (data, type, row) {
                        return `<a href="Category/Edit/${row.id}" class="btn btn-info">Edit</a>`
                    }
                }
            ]
        })
    )
})