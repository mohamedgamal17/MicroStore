var propertyGrid = propertyGrid || {};

$(document).ready(function () {
    propertyGrid.init = function (options) {

        var createModal = new abp.ModalManager({
            viewUrl: options.createModal.viewUrl
        });

        var editModal = new abp.ModalManager({
            viewUrl: options.editModal.viewUrl
        });


        var proertyTable = $(`#PropertiesTable`).DataTable(
            abp.libs.datatables.normalizeConfiguration({
                searching: false,
                pagination: false,
                ajax: {
                    url: options.listAction.url,
                    type: options.listAction.type
                },

                columnDefs: [
                    { title: "Key", data: "key" },
                    { title: "Value", data: "value" },
                    {
                        title: "Actions",
                        rowAction: {
                            items: [
                                {
                                    text: "Edit",
                                    action: function (data) {
                                        editModal.open({
                                            parentId: options.parentId,
                                            propertyId: data.record.propertyId
                                        })
                                    }
                                },
                                {
                                    text: "Delete",
                                    confirmMessage: function (record) {
                                        return "Are you sure to delete this property";
                                    },
                                    action: function (data) {
                                        abp.ajax({
                                            url: options.deleteAction.url,
                                            type: options.deleteAction.type,
                                            data: JSON.stringify({
                                                "parent_id": data.record.parentId,
                                                "property_id": data.record.propertyId
                                            }),

                                            success: function () {
                                                proertyTable.ajax.reload();
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

        $(`#CreatePropertyButton`).on('click', function () {
            createModal.open({
                parentId: options.parentId
            });
        })

        createModal.onResult(function (data) {
            proertyTable.ajax.reload();
        })

        editModal.onResult(function (data) {
            proertyTable.ajax.reload();
        })

    }

})


///*
// * 
// * {

// *       listAction : {
// *          url : string,
// *          type : string
// *       },
// *       deleteAction : {
// *          url : string,
// *          type :string
// *       },
// *       
// *       createModal :{
// *          viewUrl : string
// *       }
// *       
// *       editModal : {
// *          viewUrl : string
// *       }
// * }
// * 
// * 
// * 
// * 
// * 
// * /