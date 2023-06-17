$(document).ready(function () {

    $("#CountriesTable").DataTable(
        abp.libs.datatables.normailizeConfiguration({
            ajax: {
                type: "POST"
            },
            columnDefs: [
                {
                    title: "Name",
                    data: "name"
                },
                {
                    title: "Two Letter Iso Code",
                    data: "twoLetterIsoCode"
                }
                , {
                    title: "Three Letter Iso Code",
                    data: "threeLetterIsoCode"
                },
                {
                    title: "Numeric Iso Code",
                    data: "numericIsoCode"
                },
                {
                    title: "Actions",
                    render: function (data, type, row) {
                        return `<a href="Country/Edit/${row.id}" class="btn btn-info">Edit</a>`
                    }
                }
            ]
        })
    );
})