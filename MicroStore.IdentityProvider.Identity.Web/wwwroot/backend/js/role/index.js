$(document).ready(function () {
    var roleTable = $("#RolesTable").DataTable(
        abp.libs.datatables.normalizeConfiguration({
            ajax: {
                url: "/BackEnd/Role",
                type: "POST",
                data: function (data) {
                    data.name = $("#Name").val();
                },
            },
            serverSide: false,
            searching: false,
            processing: true,
            columnDefs: [
                {
                    title: "Name",
                    data:"name"
                },
                {
                    title: "Description",
                    data: "description",
                    orderable: false,
                },
                {
                    title: "Actions",
                    orderable:false,
                    rowAction: {
                        items: [
                            {
                                text: "Edit",
                                action: function (data) {
                                    location.href = `/BackEnd/Role/Edit/${data.record.id}`
                                }
                            },
                            {
                                text: "Delete",
                                confirmMessage: function (data) {
                                    return "Are you sure to delete this role";
                                },

                                action: function (data) {
                                    abp.ajax({
                                        url: "/BackEnd/Role/Delete/" + data.record.id,
                                        type: "POST",
                                        success: function () {
                                            roleTable.ajax.reload();
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
        roleTable.ajax.reload();
    })

})