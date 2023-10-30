$(function () {
    $("#CountryTable").DataTable(
        abp.libs.datatables.normalizeConfiguration({
            ajax: {
                type: "POST"
            },
            processing: true,
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
                    title: 'Actions',
                    rowAction: {
                        items:
                            [
                            {
                                    text: "Edit",
                                    action: function (data) {
                                        window.location.href = `Country/Edit/${data.record.id}`
                                    }
                            },
                            {
                                text: 'Report',
                                action: function (data) {
                                    window.location.href = `Country/SalesReport?code=${data.record.twoLetterIsoCode}`
                                }
                            }
                            ]
                    }
                },
            ]
        })
    );
})