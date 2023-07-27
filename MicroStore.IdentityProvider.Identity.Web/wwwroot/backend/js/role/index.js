$(document).ready(function () {
    var table = $("#RolesTable").DataTable(
        abp.libs.datatables.normalizeConfiguration({
            ajax: {
                url: "/BackEnd/Role",
                type:"POST"
            },
            serverSide: false,
            searching: false,
            paging: true,

            columnDefs: [
                {
                    title: "Name",
                    data:"name"
                },
                {
                    tite: "Description",
                    data: "description",
                    sorting: false,
                },
                {
                    title: "Actions",
                    sorting:false,
                    rowAction: {
                        items: [
                            {
                                text: "Edit",
                                action: function (data) {
                                    location.href = `/BackEnd/Role/Edit/${data.record.id}`
                                }
                            }

                        ]
                    }
                }
            ]
        })
    )

})