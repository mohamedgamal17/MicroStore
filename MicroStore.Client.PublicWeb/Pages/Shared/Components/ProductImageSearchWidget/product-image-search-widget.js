(function () {
    abp.widgets.ProductImageSearchWidget = function ($wrapper) {
        var widgetManager = $wrapper.data("abp-widget-manager");
        var init = function ($filter) {
            $wrapper
                .find(".quantity-right-plus")
                .click(function () {
                    $("#main-spinner").addClass("show")
                    var $this = $(this);
                    var parent = $this.parents(".product-box")
                    var productId = parent.attr("data-product-id");
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
                            var qunatityInput = parent.find(".input-number");

                            var newValue = parseInt(qunatityInput.val()) + 1;

                            qunatityInput.val(newValue);

                            if (newValue == 1) {
                                parent.childeren(".qty-box").addClass("open");
                            }

                        },
                        complete: function () {
                            $("#main-spinner").removeClass("show")
                        }

                    })
                });

            $wrapper
                .find(".quantity-left-minus")
                .click(function () {
                    $("#main-spinner").addClass("show")

                    var $this = $(this);
                    var parent = $this.parents(".product-box")
                    var productId = parent.attr("data-product-id");
                    abp.ajax({
                        url: "/api/basket",
                        method: "DELETE",
                        data: JSON.stringify({
                            productId: productId,
                            quantity: 1
                        }),
                        success: function () {
                            $('.abp-widget-wrapper[data-widget-name="CartWidget"]')
                                .data("abp-widget-manager")
                                .refresh();
                            console.log(parent);

                            var qunatityInput = parent.find(".input-number");

                            var newValue = parseInt(qunatityInput.val()) - 1;

                            qunatityInput.val(newValue);

                            if (newValue == 0) {
                                parent.find(".qty-box").removeClass("open");
                            }


                        },
                        complete: function () {
                            $("#main-spinner").removeClass("show")
                        }

                    })
                });

            $wrapper
                .find(".add_cart").click(function () {
                    var $this = $(this);
                    var parent = $this.parents(".product-box")
                    var productId = parent.attr("data-product-id");
                    $("#main-spinner").addClass("show")
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

                            parent.find(".input-number").val(1);

                            parent.find(".qty-box").addClass("open");
                        },
                        complete: function () {
                            $("#main-spinner").removeClass("show")
                        }
                    });
                })

            $wrapper
                .find("select[name='Length']")
                .change(function () {
                    widgetManager.refresh();
                })

            $(document).on("top_cart.item.removed", function (evt ,data) {
                var productBox = $wrapper.find(`.product-box[data-product-id = '${data.productId}']`);
                if (productBox)
                {
                     productBox.find(".input-number").val(0);
                productBox.find(".qty-box").removeClass("open");
                }
                
            })
        }

        return {
            init: init
        }
    }
})();

