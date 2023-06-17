
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
                    data: "image",
                    render: function (data) {
                        return `<img src="${data}" class="img" style="maxwidth:45px;max-height:45px">`
                    }
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

                                action: function (dataTable) {
                                    abp.ajax({
                                        url: "/Administration/Product/RemoveProductImage",
                                        type: "POST",
                                        data: JSON.stringify({
                                            ProductId: dataTable.record.productId,
                                            ProductImageId: dataTable.record.id
                                        }),
                                    })

                                    dataTable.table.ajax.reload();

                                }
                            }
                        ]
                    }
                }

            ]
        })
    );


    productImageCreateModal.onResult(function () {
        productImageTable.ajax.reload();
    });

    productImageEditMoadl.onResult(function () {
        productImageTable.ajax.reload();
    })

    $("#CreateProductImageButton").on('click', function (e) {
        e.preventDefault();
        productImageCreateModal.open({
            productId: prouctId
        });
    })

})