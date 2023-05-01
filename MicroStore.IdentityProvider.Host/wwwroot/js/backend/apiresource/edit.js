console.log("Hello");

$(document).ready(function () {
    console.log("Hello");
    var apiResourceId = $("#ApiResourceId").val();
    var apiResourceTable = $("#ApiResourceSecrets").DataTable(
        abp.libs.datatables.normalizeConfiguration({

            ajax: {
                type: "POST",
                url: "/BackEnd/ApiResource/ListApiResourceSecrets/" + apiResourceId
            },

            columnDefs: [
                { title: "Description", data: "description" },
                {
                    title: "Acitons",

                    rowAction: {
                        items: [
                            {
                                text: "Delete",
                                confirmMessage: function(data) {
                                    return "Are you sure to delete this api resource secret ";
                                },

                                action: function (data) {
                                    abp.ajax({
                                        url: "/BackEnd/ApiResource/DeleteApiResourceSecret",
                                        type: "POST",
                                        data: JSON.stringify({
                                            apiResourceId: apiResourceId,
                                            apiResourceSecretId: data.record.id
                                        })
                                    });

                                    data.ajax.reload();
                                }
                            }
                        ]
                    }
                }
            ]
        })
    )

    var createApiSecretModal = new abp.ModalManager({
        viewUrl: "/BackEnd/ApiResource/CreateApiResourceSecretModel/" + apiResourceId
    });


    $("#CreateApiSecretButton").on('click', function () {
        console.log('clicked111');
        createApiSecretModal.open({
            apiResourceId: apiResourceId
        });
    });

    createApiSecretModal.onResult(function (data) {
        apiResourceTable.ajax.reload();
    });
})