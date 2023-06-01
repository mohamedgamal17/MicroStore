(function () {
    abp.widgets.BasketWidget = function ($wrapper) {
        var widgetManager = $wrapper.data("abp-widget-manager");

        var init = function (filters) {
            console.log("inited");
            $wrapper
                .find(".basket-item-remove")
                .click(function () {
                    var $this = $(this);
                    console.log($this.parents(".basket-list-item"))
                    var productId = $this.parents(".basket-list-item").attr("data-product-id");

                    abp.ajax({
                        url: "/api/basket",
                        method: "DELETE",
                        data: JSON.stringify({
                            productId: productId
                        }),

                        success: function () {
                            widgetManager.refresh();
                            $('.abp-widget-wrapper[data-widget-name="CartWidget"]')
                                .data("abp-widget-manager")
                                .refresh();

                            abp.notify.info("Removed the product from your basket.", "Removed basket item");
                        }
                    })
                });

            $wrapper
                .find(".basket-item-increase")
                .click(function () {
                    var $this = $(this);
                    console.log($this.parents(".basket-list-item"))
                    var productId = $this.parents(".basket-list-item").attr("data-product-id");
                    console.log(productId);
                    abp.ajax({
                        url: "/api/basket",
                        method: "POST",
                        data: JSON.stringify({
                            productId: productId,
                            quantity: 1
                        }),

                        success: function () {
                            widgetManager.refresh();
                            $('.abp-widget-wrapper[data-widget-name="CartWidget"]')
                                .data("abp-widget-manager")
                                .refresh();
                        }

                    })
                });

            $wrapper
                .find(".basket-item-decrease")
                .click(function () {
                    var $this = $(this);
                    var productId = $this.parents(".basket-list-item").attr("data-product-id");

                    abp.ajax({
                        url: "/api/basket",
                        method: "DELETE",
                        data: JSON.stringify({
                            productId: productId,
                            quantity: 1
                        }),
                        success: function () {
                            widgetManager.refresh();
                            $('.abp-widget-wrapper[data-widget-name="CartWidget"]')
                                .data("abp-widget-manager")
                                .refresh();
                        }

                    })
                });
        };

        return {
            init: init
        }
    };
})();

