(function () {
    abp.widgets.ProductFilterWidget = function ($wrapper) {
        var widgetManager = $wrapper.data("abp-widget-manager");
        var init = function ($filter) {
            $(".js-range-slider").ionRangeSlider({
                type: "double",
                onFinish: function (data) {
                    $(".js-input-from").val(data.from);
                    $(".js-input-to").val(data.to);
                    $('.abp-widget-wrapper[data-widget-name="ProductListWidget"]')
                        .data("abp-widget-manager")
                        .refresh();
                } 
            })

            $wrapper
                .find("input[class*='product-filter']")
                .change(function () {
                    $('.abp-widget-wrapper[data-widget-name="ProductListWidget"]')
                        .data("abp-widget-manager")
                        .refresh();
                })
        }

        return {
            init: init
        }
    }
})()