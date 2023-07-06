$(document).ready(function () {
    var table = $("#UsersTable").DataTable(
        abp.libs.datatables.normalizeConfiguration({
            ajax: {
                url: "/BackEnd/User",
                type:"POST"
            },
            searching: false,
            serverSide: true,
          
            columnDefs: [
                {
                    title: "User Name",
                    data:"userName"
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

})