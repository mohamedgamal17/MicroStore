$(function () {

    var countryId = $("#CountryId").val();

    var createStateModal = new abp.ModalManager({
        viewUrl: '/Administration/Country/CreateOrEditStateModal'
    });

    var stateProvinceTable = $("#StateProvincesTable").DataTable(
        abp.libs.datatables.normalizeConfiguration({
            ajax: {
                url: "/Administration/Country/ListStateProvinces/" + countryId,
                type: "POST",
            },
            processing:true,
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

                    rowAction: {
                        items: [
                            {
                                text: 'Edit',
                                action: function (data) {
                                    createStateModal.open({
                                        countryId: data.record.countryId,
                                        stateId: data.record.id
                                    });
                                }
                            },

                            {
                                text: 'Delete',

                                confirmMessage: function (data) {
                                    return `Are you sure you want to delete state ${data.name}`
                                },

                                action: function (data) {
                                    abp.ajax({
                                        url: "/Administration/Country/RemoveStateProvince",
                                        type: "POST",
                                        data: JSON.stringify({
                                            countryId: data.record.countryId,
                                            stateId: data.record.id
                                        }),
                                        success: function (response, status) {                                       
                                            abp.notify.info("Successfully deleted!");
                                            stateProvinceTable.ajax.reload() 
                                        }
                                    })
                                                             
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

    createStateModal.onResult(function (data) {
        stateProvinceTable.ajax.reload();
    })



});
