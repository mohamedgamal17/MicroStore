$(document).ready(function () {
    var clientTable = $("#ClientTable").DataTable(
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
                                        success: function () {
                                            clientTable.ajax.reload();
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
        clientTable.ajax.reload();
    })

})