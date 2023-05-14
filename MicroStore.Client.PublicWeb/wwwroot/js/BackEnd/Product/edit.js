
$(document).ready(function () {

    var prouctId = $('#Id').val();

    $('.summernote').summernote({
        height: 150,
        name: 'LongDescription',
        codemirror: { // codemirror options
            theme: 'monokai'
        }
    });


    $("#CategorySelect").select2({
        placeholder: 'select product categories'
    });

    $("#ManufacturerSelect").select2({
        placeholder: 'select product manufacturer'
    });

    var productImageCreateModal = new abp.ModalManager({
        viewUrl: '/Administration/Product/CreateProductImageModal'
    });

    var productImageEditMoadl = new abp.ModalManager({
        viewUrl: '/Administration/Product/EditProductImageModal'
    })


    var productImageTable = $("#ProductImagesTable").DataTable(
        abp.libs.datatables.normalizeConfiguration({
            ajax: {
                url: '/Administration/Product/ListProductImages/' + prouctId,
                type: 'POST'
            },

            searching: false,


            columnDefs: [
                {
                    title: "Image",
                    data: "image"
                },
                {
                    title: 'Display Order',
                    data: 'displayOrder',
                },
                {
                    title: 'Actions',
                    rowAction: {
                        items: [
                            {
                                text: 'Edit',
                                action: function (data) {
                                    productImageEditMoadl.open({
                                        productId: data.record.productId,
                                        productImageId: data.record.id,
                                    })
                                }
                            },
                            {
                                text: 'Delete',

                                confirmMessage: function (data) {
                                    return "Are you sure to delete this product image ";
                                },

                                action: function (data) {
                                    abp.ajax({
                                        url: "/Administration/Product/RemoveProductImage",
                                        type: "POST",
                                        data: JSON.stringify({
                                            ProductId: data.record.productId,
                                            ProductImageId: data.record.id
                                        })
                                    })

                                }
                            }
                        ]
                    }
                }

            ]
        })
    );


    productImageCreateModal.onResult(function () {
        dataTable.ajax.reload();
    });

    productImageEditMoadl.onResult(function () {
        dataTable.ajax.reload();
    })

    $("#CreateProductImageButton").on('click', function (e) {
        e.preventDefault();
        productImageCreateModal.open({
            productId: prouctId
        });
    })

})