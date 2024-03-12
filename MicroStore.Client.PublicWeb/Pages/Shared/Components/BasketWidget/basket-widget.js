﻿(function () {
    abp.widgets.BasketWidget = function ($wrapper) {
        var widgetManager = $wrapper.data("abp-widget-manager");
        var init = function (filters) {
            $wrapper
                .find(".basket-item-remove")
                .click(function () {
                    $("#main-spinner").addClass("show")
                    var $this = $(this);
                    var productId = $this.parents(".basket-list-item").attr("data-product-id");
                    abp.ajax({
                        url: "/api/basket",
                        method: "DELETE",
                        data: JSON.stringify({
                            productId: productId,
                            quantity: -1
                        }),

                        success: function () {
                            widgetManager.refresh();
                            $('.abp-widget-wrapper[data-widget-name="CartWidget"]')
                                .data("abp-widget-manager")
                                .refresh();

                            abp.notify.success("Removed the product from your basket.", "Removed basket item")
                        },
                        error: function (err) {
                            abp.notify.error("Error", "Unexpected error has been happened")
                        },
                        complete: function () {
                            $("#main-spinner").removeClass("show")
                        }
                    })
                });

            $wrapper
                .find(".quantity-right-plus")
                .click(function () {
                    $("#main-spinner").addClass("show")
                    var $this = $(this);
                    var productId = $this.parents(".basket-list-item").attr("data-product-id");
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

                            abp.notify.success("Added product to your basket.", "Successfully added")
                        },

                        error: function (err) {
                            abp.notify.error("Error", "Unexpected error has been happened")
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

                            abp.notify.success("Removed the product from your basket.", "Removed basket item")
                        },

                        error: function (err) {
                            abp.notify.error("Error", "Unexpected error has been happened")
                        },
                        complete: function () {
                            $("#main-spinner").removeClass("show")

                        }

                    })
                });
        };

        return {
            init: init
        }
    };
})();

