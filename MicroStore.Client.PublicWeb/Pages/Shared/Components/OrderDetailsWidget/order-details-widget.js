(function () {
    abp.widgets.OrderDetailsWidget = function ($wrapper) {
        var init = function (filter) {
            $(".buy-again").click(function () {
                $("#main-spinner").addClass("show")
                var $this = $(this);
                var productId = $this.parents("[data-product-id]").attr("data-product-id");
                abp.ajax({
                    url: "/api/basket",
                    method: "POST",
                    data: JSON.stringify({
                        productId: productId,
                        quantity: 1
                    }),

                    success: function () {
                        $(".abp-widget-wrapper[data-widget-name='CartWidget']")
                            .each(function () {
                                var widgetManager = new abp.WidgetManager({
                                    wrapper: $(this)
                                });
                                widgetManager.init($(this));

                                widgetManager.refresh();
                            })


                        abp.notify.info("Added product to your basket.", "Successfully added")
                    },

                    complete: function () {
                        $("#main-spinner").removeClass("show")
                    }
                });

            })
        };


        return {
            init : init
        }
    }
})();