$(document).ready(function () {
    $('.summernote').summernote({
        height: 150,
        name: 'Model.LongDescription',
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
});