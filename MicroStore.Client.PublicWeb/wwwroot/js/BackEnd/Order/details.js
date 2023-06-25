$(document).ready(function () {
    var orderId = $("#Id").val();

    var cancelOrderModal = new abp.ModalManager({
        viewUrl : "/Administration/Order/CancelOrderModal" 
    })

    $("#cancelOrderButton").click(function () {
        cancelOrderModal.open({
            id : orderId
        })
    })

    $("#completeOrderButton").click(function () {
        $("#completeOrderForm").submit()
    })

    cancelOrderModal.onResult(function () {
        location.reload()
    })
})