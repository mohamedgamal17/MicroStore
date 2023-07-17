(function () {
    abp.widgets.CheckoutInformationWidget = function ($wrapper) {

        var widgetmanager = $wrapper.data("abp-widget-manager");

        var init = function (filter) {
            $wrapper.find("select[data-address-select='Country']")
                .change(function () {
                    var countryElement = $(this);
                    var countryIsoCode = countryElement.val();
                    var stateProvinceELement =countryElement
                        .parents("[data-address]")
                        .find("select[data-address-select='StateProvince']")[0];

                    abp.ajax({
                        url: `/api/country/code/${countryIsoCode}`,
                        method: "GET",
                        success: function (result) {
                            console.log(stateProvinceELement)
                            $(stateProvinceELement).empty();
                            $(stateProvinceELement)
                                .append("<option selected>Please Select State Province</option>");
                            console.log("appended");
                            if (result.stateProvinces) {
                                console.log(result.stateProvinces)
                                $.each(result.stateProvinces, function (index, val) {
                             
                                    stateProvinceELement.append(new Option(val.name, val.abbreviation));

                                })
                         
                            }

                        },

                        error: function () {
                            abp.notifiy.error("UnExpected Error has been happend please refersh the page")
                        }
                    });
                });

        };


        return {
            init: init
        }
    }

})();