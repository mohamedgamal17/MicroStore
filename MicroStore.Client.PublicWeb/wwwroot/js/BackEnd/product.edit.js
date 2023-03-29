var editProductObj = editProductObj || {};

$(document).ready(function () {

    editProductObj.init = function (obj) {

          this.productImagesTable = $(`#${obj.productImageGrid.id}`).DataTable({
                ajax: obj.productImageGrid.ajax,
                paging: false,
                ordering: false,
                info: false,
                searching: false,
                serverSide: true,
                columns: obj.productImageGrid.columns
         });

       }

    

        editProductObj.createProductImage = function (form, event) {
            event.preventDefault();
            var formData = new FormData(form);

            $.ajax({
                url: form.action,
                data: formData,
                type: form.method,
                contentType: false,
                processData: false,
                success: function (data) {
                    editProductObj.productImagesTable.ajax.reload();
                },
                error: function (error) {
                    console.log('error');
                }


            })
        }

        editProductObj.updateProductImage = function (form, event) {
            event.preventDefault();

            console.log(form.action);

            var formData = new FormData(form);

            $.ajax({
                url: form.action,
                type: form.method,
                contentType: false,
                processData: false,
                data: formData,
                success: function (data) {
                    editProductObj.productImagesTable.ajax.reload();
                    console.log("response data " + data);
                }
            })
        }

        editProductObj.deleteProductImage = function (form, evt) {

            evt.preventDefault();

            var formData = new FormData(form);

            $.ajax({
                url: form.action,
                type: form.method,
                contentType: false,
                processData: false,
                data: formData,

                success: function (data) {
                    editProductObj.productImagesTable.ajax.reload();
                    console.log("response data " + data);
                }

            })

    }





})

/*
 *  productImageGrid : {
 *      id
 *      ajax ---> return to DataTableJs Docs and read ajax
 *      columns  ----> return to DataTableJs Docs and read about columns
 *  }
 *  
 *  productImageEdit : {
 *      htmlRender ---> refers to html element that must be rendered --> take function with params (data , type ,row)
 *          data : refers to product images data,
 *          type : refers to type of data,
 *          row : refers to current row in the table
 * 
 *  }
 *  
 *  productImageDelete : {
 *      
        htmlRender ---> refers to html element that must be rendered --> take function with params (data , type ,row)
 *          data : refers to product images data,
 *          type : refers to type of data,
 *          row : refers to current row in the table
 *  }
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * */