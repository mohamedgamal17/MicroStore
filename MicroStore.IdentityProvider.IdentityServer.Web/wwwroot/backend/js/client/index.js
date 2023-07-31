$(document).ready(function () {
    var apiResourceTable = $("#ClientTable").DataTable(
        abp.libs.datatables.normalizeConfiguration({
            ajax: {
                type: "POST",
                data: function (data) {
                    data.clientId = $("#ClientId").val();
                },
            },
            searching: false,
            serverSide: true,
            processing: true,
           "ordering": false,
            columnDefs: [
                { title: "Client Id", data: "clientId" },
                { title: "Client Name", data: "clientName" },
                {
                    title: "Actions",
                    rowAction: {
                        items: [
                            {
                                text: "Edit",
                                action: function (data) {
                                    location.href = `/BackEnd/Client/Edit/${data.record.id}`
                                }
                            },

                            {
                                text: "Delete",
                                confirmMessage:  function(data) {
                                    return "Are you sure to delete this api scope ";
                                },

                                action: function (data) {
                                    abp.ajax({
                                        url: "/BackEnd/Client/Delete/" + data.record.id,
                                        type: "POST",
                                    });

                                    recrod.data.ajax.reload();
                                }
                            }
                        ]
                    }
                }
            ]
        })
    )

    var propertTable = $(`#PropertiesTable`).DataTable(
        abp.libs.datatables.normalizeConfiguration({
            searching: false,
            pagination: false,
            ajax: {
                url: "Cli",
                type: "POST"
            },

            columnDefs: [
                { title: "Key", data: "key" },
                { title: "Value", data: "value" },
                {
                    title: "Actions",
                    rowAction: {
                        items: [
                            {
                                text: "Edit",
                                action: function (data) {
                                    editModal.open({
                                        clientId: options.clientId,
                                        propertyId: data.record.id
                                    })
                                }
                            },
                            {
                                text: "Delete",
                                confirmMessage: function (record) {
                                    return "Are you sure to delete this property";
                                },
                                action: function (data) {
                                    abp.ajax({

                                        url: options.deleteAction.url,
                                        type: options.deleteAction.type,
                                        data: JSON.stringify({
                                            clientId: data.record.clientId,
                                            propertyId: data.record.id
                                        }),

                                        success: function () {
                                            data.table.ajax.reload();
                                        }
                                    });


                                }
                            }
                        ]
                    }
                }

            ]
        })
    )

    $("#AdvancedForm").on('submit', function (evt) {
        evt.preventDefault();
        apiResourceTable.ajax.reload();
    })

})