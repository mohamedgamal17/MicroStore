(function () {
    abp.widgets.AddressListWidget = function ($wrapper) {
        var widgetManager = $wrapper.data("abp-widget-manager");
        var init = function ($filter) {
            $wrapper.find(".remove-address")
                .click(function (evt) {
                    evt.preventDefault();
                    var parent = $(this).parents(".address-box-wrapper")
                    var addressId = parent.attr('data-address-id')
                    abp.message.confirm('Are you sure to delete this address ?')
                        .then(function (confirmed) {
                            console.log("conf")
                            var formData = new FormData();
                            formData.append("addressId", addressId);
                            console.log("conf")

                            abp.ajax({
                                url: '/Profile/Index?handler=DeleteAddress',
                                method: "POST",
                                data: formData,
                                processData: false,
                                contentType: false, 
                                success: function () {
                                    widgetManager.refresh()
                                    abp.message.success('Your changes have been successfully saved!', 'Congratulations');
                                }
                            }) 
                        })
                })
        }

        return {
            init: init
        }
    }
})();