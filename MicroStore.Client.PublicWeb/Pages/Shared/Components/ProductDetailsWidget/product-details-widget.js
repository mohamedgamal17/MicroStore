(function () {
    abp.widgets.ProductDetailsWidget = function ($wrapper) {

        var init = function (filter) {

            $(".quantity-left-minus").prop('disabled', true);
             

            $wrapper
                .find(".quantity-left-minus")
                .click(function (){
                    var quantityInput = $("input[name='quantity']");
                    var quantity = parseInt(quantityInput.val());

                    if (quantity > 1)
                    {
                        quantity -= 1;

                        if (quantity == 1) {
                            $(this).prop("disabled", true)                
                        }
                        quantityInput.val(quantity)
                    }               
                })

            $wrapper
                .find(".quantity-right-plus")
                .click(function () {
                    var quantityInput = $("input[name='quantity']");
                    var quantity = parseInt(quantityInput.val());
                    quantity += 1;
                    quantityInput.val(quantity);
                    if (quantity > 1)
                    {
                        $(".quantity-left-minus").prop('disabled', false)
                    }

                })
            $(".add-basket-button")
                .click(function () {
                    var $this = $(this);
                    var productId = $this.parents("[data-product-id]").attr("data-product-id")
                    var quantity = parseInt($("input[name='quantity']").val());
                    abp.ajax({
                        url: "/api/basket",
                        method: "POST",
                        data: JSON.stringify({
                            productId: productId,
                            quantity: quantity
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
                        }
                    });
                });

            $(".product-image-thumb")
                .click(function () {
                    var $this = $(this);
                    var activeThumb = $this.parent().find(".active");
                    activeThumb.removeClass(".active");
                    $this.addClass("active");
                    var imageSrc = $this.attr("data-image-src");
                    $(".product-image").attr("src", imageSrc);
                })


        };

        return {
            init : init
        }
    }


})();