(function () {
    abp.widgets.RelatedProductListWidget = function ($wrapper) {
        var widgetManager = $wrapper.data("abp-widget-manager");
        var init = function ($filters) {
            $wrapper
                .find(".add-to-cart")
                .click(function () {
                    $("#main-spinner").addClass("show")
                    var $this = $(this);
                    var productId = $this.parents(".product-box").attr("data-product-id");
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

                            abp.notify.info("Added product to your basket.", "Successfully added")
                        },
                        complete: function () {
                            $("#main-spinner").removeClass("show")
                        }

                    })
                });
        }

        return {
            init: init
        }
    }
})();