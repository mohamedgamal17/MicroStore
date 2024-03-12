(function () {
    abp.widgets.CartWidget = function ($wrapper) {
        var widgetManager = $wrapper.data("abp-widget-manager");
        var init = function ($filters) {
            $wrapper.find(".cart-widget-remove")
                .click(function () {
                    $("#main-spinner").addClass("show")
                    var $this = $(this);
                    var productId = $this.parents(".cart-item-wrapper").attr("data-product-id");
                    abp.ajax({
                        url: "/api/basket",
                        method: "DELETE",
                        data: JSON.stringify({
                            productId: productId
                        }),
                        success: function () {

                            widgetManager.refresh();

                            $(document).trigger("top_cart.item.removed", {
                                source: "top_cart",
                                productId: productId,
                                occuredAt: Date.now()
                            });

                       

                            abp.notify.info("Removed the product from your basket.", "Removed basket item");
                        },
                        complete: function () {
                            $("#main-spinner").removeClass("show")
                        }
                    })
                })
        }


        return {
            init: init
        }
    }
})();