$(document).ready(function () {
    $(".add-basket-button").click(function () {
        var $this = $(this);
        var productId = $this.attr("data-product-id");

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
                            wrapper : $(this)
                        });
                        widgetManager.init($(this));

                        widgetManager.refresh();
                    })


                abp.notify.info("Added product to your basket.", "Successfully added")
            }
        });

    })
})