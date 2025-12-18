$(document).ready(function () {
    var apiScopeTable = $("#ApiScopeTable").DataTable(
        abp.libs.datatables.normalizeConfiguration({
            ajax: {
                type: "POST",
                data: function (data) {
                    data.name = $("#Name").val();
                },
            },

            pagination : false,
            searching: false,
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

                                    location.href=  `/BackEnd/ApiScope/Edit/${data.record.id}`
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

                                        success: function () {
                                            apiScopeTable.ajax.reload();
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

    $("#AdvancedSearchForm").on('submit', function (evt) {
        evt.preventDefault();
        apiScopeTable.ajax.reload();
    })

})