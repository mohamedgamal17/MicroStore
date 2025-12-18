$(document).ready(function () {
    var userTable = $("#UsersTable").DataTable(
        abp.libs.datatables.normalizeConfiguration({
            ajax: {
                url: "/BackEnd/User",
                type: "POST",
                data: function (data) {
                    data.userName = $("#UserName").val();
                    data.role = $("#Role").val();
                },
            },
            searching: false,
            serverSide: true,
            processing: true,
            orderable: false,
            columnDefs: [
                {
                    title: "User Name",
                    data: "userName",                  
                },
                {
                    title: "Email",
                    data:"email"
                },
                {
                    title: "Actions",
                    rowAction: {
                        items: [
                            {
                                text: "Edit",
                                action: function (data) {
                                    location.href = `/BackEnd/User/Edit/${data.record.id}`
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
        userTable.ajax.reload();
    })
})