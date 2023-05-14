$(document).ready(function () {

    var countryId = $("#CountryId").val();

    var stateModal = new abp.ModalManager({
        viewUrl: '/Administration/Country/CreateOrEditStateModal'
    });

    var table = $("#StateProvincesTable").DataTable(
        abp.libs.datatables.normalizeConfiguration({
            ajax: {
                url: "/Administration/Country/ListStateProvinces/" + countryId,
                type: "POST",
            },

            columnDefs: [
                {
                    title: "Name",
                    data: "name"
                },
                {
                    title: "Abbreviation",
                    data: "abbreviation"
                },
                {
                    title: "Actions",

                    rowActions: {
                        items: [
                            {
                                text: 'Edit',
                                action: function (data) {
                                    stateModal.open({
                                        countryId: data.countryId,
                                        stateId: data.id
                                    });
                                }
                            },

                            {
                                text: 'Delete',

                                confirmMessage: function (data) {
                                    return `Are you sure you want to delete state ${data.name}`
                                },

                                action: function (data) {
                                    abp.libs.ajax({
                                        url: "/Administration/Country/RemoveStateProvince",
                                        type: "POST",
                                        data: JSON.stringify({
                                            countryId: data.record.countryId,
                                            stateId: data.record.id
                                        })
                                    })

                                    data.table.ajax.reload();
                                }
                            }

                        ]
                    }
                }
            ]
        })
    );

    $("#CreateStateButton").on('click', function (event) {
        event.preventDefault();
        createStateModal.open({
            countryId: countryId
        })
    })

    stateModal.onResult(function (data) {
        table.ajax.reload();
    })



});
