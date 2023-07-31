$(document).ready(function () {
    var apiResourceId = $("#ApiResourceId").val();
    var apiResourceSecretTable = $("#ApiResourceSecrets").DataTable(
        abp.libs.datatables.normalizeConfiguration({
            ajax: {
                type: "POST",
                url: "/BackEnd/ApiResource/ListApiResourceSecrets/" + apiResourceId
            },
            searching: false,
            processing: true,
            ordering: false,
            columnDefs: [
                { title: "Type", data: "type" },
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
                                            api_resource_id: apiResourceId,
                                            secret_id: data.record.id
                                        }),
                                        success: function () {
                                            apiResourceSecretTable.ajax.reload()
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

    var createApiSecretModal = new abp.ModalManager({
        viewUrl: "/BackEnd/ApiResource/CreateApiResourceSecretModel/" + apiResourceId
    });


    $("#CreateApiSecretButton").on('click', function () {
        createApiSecretModal.open({
            apiResourceId: apiResourceId
        });
    });

    createApiSecretModal.onResult(function (data) {
        apiResourceSecretTable.ajax.reload();
    });
})