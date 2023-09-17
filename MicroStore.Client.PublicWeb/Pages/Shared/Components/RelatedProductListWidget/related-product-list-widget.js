(function () {
    abp.widgets.FeaturedProductListWidget = function ($wrapper) {
        var widgetManager = $wrapper.data("abp-widget-manager");
        var init = function ($filters) {
            $wrapper
                .find(".add-to-cart")
                .click(function () {
                    var $this = $(this);
                    var productId = $this.parents(".product-box").attr("data-product-id");
                    console.log(productId);
                    abp.ajax({
                        url: "/api/basket",
                        method: "POST",
                        data: JSON.stringify({
                            productId: productId,
                            quantity: 1
                        }),
                        success: function () {
                            $('.abp-widget-wrapper[data-widget-name="CartWidget"]')
                                .data("abp-widget-manager")
                                .refresh();
                        }

                    })
                });
        }

        return {
            init: init
        }
    }
})();