(function () {
    abp.widgets.ProductFilterWidget = function ($wrapper) {
        var widgetManager = $wrapper.data("abp-widget-manager");
        var init = function ($filter) {
            $(".js-range-slider").ionRangeSlider({
                type: "double",
                onFinish: function (data) {
                    $("#main-spinner").addClass("show")
                    $(".js-input-from").val(data.from);
                    $(".js-input-to").val(data.to);
                    $('.abp-widget-wrapper[data-widget-name="ProductListWidget"]')
                        .data("abp-widget-manager")
                        .refresh();
                    $("#main-spinner").removeClass("show")
                } 
            })


            $wrapper
                .find("input[class*='product-filter']")
                .change(function () {
                    $("#main-spinner").addClass("show")
                    $('.abp-widget-wrapper[data-widget-name="ProductListWidget"]')
                        .data("abp-widget-manager")
                        .refresh();
                    $("#main-spinner").removeClass("show")
                })
        }

        return {
            init: init
        }
    }
})()