$(document).ready(function () {
    var apiResourceTable = $("#ApiResourceTable").DataTable(
        abp.libs.datatables.normalizeConfiguration({
            ajax: {
                type: "POST",
               data: function (data) {
                    data.name = $("#Name").val();
                },
            },
            searching: false,
            serverSide: true,
            processing: true,
            ordering: false,
            columnDefs: [
                { title: "Name", data: "name" },
                { title: "Display Name", data: "displayName" },
                {
                    title: "Actions",

                    rowAction: {
                        items: [
                            {
                                text: "Edit",
                                action: function (data) {
                                    location.href = `ApiResource/Edit/${data.record.id}`
                                }
                            },

                            {
                                text: "Delete",
                                confirmMessage: function (data) {
                                    return "Are you sure to delete this api scope ";
                                },

                                action: function (data) {
                                    abp.ajax({
                                        url: "/BackEnd/ApiResource/Delete/" + data.record.id,
                                        type: "POST",
                                        success: function () {
                                            apiResourceTable.ajax.reload();
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

})