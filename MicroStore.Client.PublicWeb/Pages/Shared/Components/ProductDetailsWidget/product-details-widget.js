(function () {
    abp.widgets.ProductDetailsWidget = function ($wrapper) {

        var init = function (filter) {

            $("#SubmitReviewForm").validate({
                rules: {
                    Rating: "required",
                    ReviewTitle: {
                        required: true,
                        minlength: 2,
                        maxlength: 256
                    },
                    ReviewText: {
                        required: true,
                        minlength: 2,
                        maxlength: 1000
                    }
                }
            });

            var stars = new StarRating('.star-rating');
            
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

            $("#SubmitReviewForm").on("submit", function (evt) {
                evt.preventDefault();

                var submitUrl = $("#SubmitReviewForm").attr("form-url");

                var formData = {               
                    Rating: $("input[name='Rating']").val(),
                    ReviewTitle: $("input[name='ReveiwTitle']").val(),
                    ReviewText: $("input[name='ReviewText']").val(),
                    ProductId: $("input[name='ProductId']").val()
                }


                abp.ajax({
                    type: "POST",
                    url: submitUrl,
                    data: formData,
                    success: function () {
                        abp.notify.info("Review", "Your reveiw has been submited.")
                        $(".abp-widget-wrapper[data-widget-name='ProductDetailsWidget']")
                            .refresh()
                    }
                })
            })


        };

        return {
            init : init
        }
    }


})();