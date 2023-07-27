$(document).ready(function () {
    console.log("SS");
    console.log("SS");
    console.log("SS");
    console.log("SSsssssssssssssssssaaaaa");
    var apiResourceTable = $("#ClientTable").DataTable(
        abp.libs.datatables.normalizeConfiguration({
            ajax: {
                type: "POST"
            },
            searching: false,
            serverSide: true,
            columnDefs: [
                { title: "Client Id", data: "clientId" },
                { title: "Client Name", data: "clientName" },
                {
                    title: "Actions",

                    rowAction: {
                        items: [
                            {
                                text: "Edit",
                                action: function (data) {
                                    location.href = `/BackEnd/Client/Edit/${data.record.id}`
                                }
                            },

                            {
                                text: "Delete",
                                confirmMessage:  function(data) {
                                    return "Are you sure to delete this api scope ";
                                },

                                action: function (data) {
                                    abp.ajax({
                                        url: "/BackEnd/Client/Delete/" + data.record.id,
                                        type: "POST",
                                    });

                                    recrod.data.ajax.reload();
                                }
                            }
                        ]
                    }
                }
            ]
        })
    )

})