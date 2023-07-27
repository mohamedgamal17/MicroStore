$(document).ready(function () {
    var apiScopeTable = $("#ApiScopeTable").DataTable(
        abp.libs.datatables.normalizeConfiguration({
            ajax: {
                type : "POST"
            },

            pagination : false,
            searching  : false,
            columnDefs: [
                { title : "Name" , data : "name"},
                { title: "Description", data: "description" },

                {
                    title: "Actions",
                    rowAction: {
                        items: [
                            {
                                text: "Edit",
                                action: function (data) {

                                    location.href=  `ApiScope/Edit/${data.record.id}`
                                }
                            },

                            {
                                text: "Delete",
                                confirmMessage: function(data) {
                                    return "Are you sure to delete this api scope ";
                                },

                                action: function (recrod) {
                                    abp.ajax({
                                        url: "/BackEnd/ApiScope/Delete/" + record.id,
                                        type: "POST",
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