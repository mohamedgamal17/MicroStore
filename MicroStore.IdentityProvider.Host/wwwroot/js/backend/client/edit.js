$(document).ready(function () {

    var clientId = $("#ClientId").val();

    var createModal = new abp.ModalManager({
        viewUrl: "/BackEnd/Client/CreateClaimModal"
    });

    var editModal = new abp.ModalManager({
        viewUrl: "/BackEnd/Client/EditClaimModal"
    })
    var claimTable =$("#ClaimTable").DataTable(
        abp.libs.datatables.normalizeConfiguration({
            ajax: {
                url: "/BackEnd/Client/ListClaims?clientId=" + clientId,

                type : "POST"
            },
            columnDefs: [
                { title: "Type", data: "type" },
                { title: "Value", data: "value" },
                {
                    title: "Actions",
                    rowAction: {
                        items: [
                            {
                                text: "Edit",
                                action: function (data) {
                                    editModal.open({
                                        clientId: data.record.clientId,
                                        claimId: data.record.id
                                    })
                                }
                            },
                            {
                                text: "Delete",
                                confirmMessage: function (record) {
                                    return "Are you sure to delete this claim";
                                },
                                action: function (data) {
                                    abp.ajax({

                                        url: "/BackEnd/Client/RemoveClaim",
                                        type:"POST",
                                        data: JSON.stringify({
                                            clientId: data.record.clientId,
                                            claimId: data.record.id
                                        }),

                                        success: function () {
                                            data.table.ajax.reload();
                                        }
                                    });

                                    record.data.ajax.reload();
                                }
                            }
                        ]
                    }
                }

            ]
        })
    );
    $(`#AddClaimButton`).on('click', function () {
        createModal.open({
            clientId: clientId
        });
    })
    createModal.onResult(function (data) {
        claimTable.ajax.reload();
    })

    editModal.onResult(function (data) {
        claimTable.ajax.reload();
    })
})