$(document).ready(function () {

    var clientId = $("#ClientId").val();

    var createClaimModal = new abp.ModalManager({
        viewUrl: "/BackEnd/Client/CreateClaimModal"
    });

    var editClaimModal = new abp.ModalManager({
        viewUrl: "/BackEnd/Client/EditClaimModal"
    })

    var createSecretModel = new abp.ModalManager({
        viewUrl: "/BackEnd/Client/CreateSecretModal"
    })

    var claimTable = $("#ClaimTable").DataTable(
        abp.libs.datatables.normalizeConfiguration({
            ajax: {
                url: "/BackEnd/Client/ListClaims?clientId=" + clientId,

                type: "POST"
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
                                    editClaimModal.open({
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
                                        type: "POST",
                                        data: JSON.stringify({
                                            client_id: data.record.clientId,
                                            claim_id: data.record.id
                                        }),

                                        success: function () {
                                            claimTable.ajax.reload();
                                        }
                                    });

                                }
                            }
                        ]
                    }
                }

            ]
        })
    );

    var secretTable = $("#SecretTable").DataTable(
        abp.libs.datatables.normalizeConfiguration({
            ajax: {
                url: "/BackEnd/Client/ListClientSecrets?clientId=" + clientId,

                type: "POST"
            },

            columnDefs: [
                { title: "Type", data: "type" },
                { title: "Description", data: "description" },
              
                {
                    title: "Actions",
                    rowAction: {
                        items: [
                            {
                                text: "Delete",
                                confirmMessage: function (record) {
                                    return "Are you sure to delete this claim";
                                },
                                action: function (data) {
                                    abp.ajax({
                                        url: "/BackEnd/Client/RemoveSecret",
                                        type: "POST",
                                        data: JSON.stringify({
                                            client_id: data.record.clientId,
                                            secret_id: data.record.id
                                        }),

                                        success: function () {
                                            secretTable.ajax.reload();
                                        }
                                    });

                                }
                            }
                        ]
                    }
                }

            ]
        })
    );
    $(`#AddClaimButton`).on('click', function () {
        createClaimModal.open({
            clientId: clientId
        });
    })

    $("#AddSecretButton").on('click', function () {
        createSecretModel.open({
            clientId: clientId
        })
    })

    createClaimModal.onResult(function (data) {
        claimTable.ajax.reload();
    })

    editClaimModal.onResult(function (data) {
        claimTable.ajax.reload();
    })

    createSecretModel.onResult(function (data) {
        secretTable.ajax.reload();
    })

    $("#EditFormButton").on('click', function () {
        console.log('cc');
        $("#EditForm").submit();
    })
})
