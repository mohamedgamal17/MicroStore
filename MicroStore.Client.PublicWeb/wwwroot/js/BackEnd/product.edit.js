
$(document).ready(function () {

    const toast = swal.mixin({
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        didOpen: function (toast) {
            toast.addEventListener('mouseenter', swal.stopTimer),
                toast.addEventListener('mouseleave', swal.resumeTimer)
        }
    });

    $('.summernote').summernote({
        height: 150,
        name: 'LongDescription',
        codemirror: { // codemirror options
            theme: 'monokai'
        }
    });

    $("#category-select").select2({
        placeholder: 'select product categories'
    });

    var table = $("#productimages-grid").DataTable({

        ajax: {
            url: '@(Url.Action("ListProductImages",new {id = Model.Id}))',
            dataSrc: 'data',
            type: 'POST'
        },

        columns: [
            { data: "image", name: 'image' },
            { data: "displayOrder", name: 'DisplayOrder' },
            {
                mRender: function (data, type, row) {
                    return `<button role="button" class="btn btn-info" data-row-edit-id="${row.id}">Edit</button>`
                }
            },
            {
                mRender: function (data, type, row) {
                    return `<button role="button" class="btn btn-danger" data-row-delete-id="${row.id}">Delete</button>`
                }
            },
        ],

        initComplete: function () {
            $("button[data-row-edit-id]").each(function () {
                $(this).on('click', function () {

                    var rowId = $(this).attr("data-row-edit-id");

                    var row = table.data().filter(function (x) { return x.id == rowId });

                    $("UpdateProductImage_Id").val(row.id);

                    $("UpdateProductImage_DisplayOrder").val(row.displayOrder);

                });
            });
        }

    });


    $("#CreateProductImageForm").on('submit', function () {
        $.ajax({
            url: this.action,
            method: this.method,
            enctype: this.enctype,
            data: $(this).serialize(),
            processData: false,

            success: function (result) {
                console.log(result);

                toast.fire({
                    icon: "success",
                    title: "product image has been created"
                });

                table.ajax.reload();
            },

            error: function (error) {

                toast.fire({
                    icon: "error",
                    title : "UnExpected Error"
                })
            }
        })
    })



    $("#UpdateProductImageForm").on('submit', function () {

        $.ajax({

            url: this.action,
            method: this.method,
            enctype: this.enctype,
            data: $(this).serialize(),
            processData: false,

            success: function (result) {

                console.log(result);

                $("#UpdateProductImageModa").modal("hide");

                toast.fire({
                    icon: 'success',
                    title: 'product image has been updated'
                });

                table.ajax.reload();
            },

            error : function (error) {

                console.log(error);

                toast.fire({
                    icon: 'error',
                    title: 'UnExpected error happen'
                })

                $("#UpdateProductImageModa").modal("hide");
            }
        })
    })
})
