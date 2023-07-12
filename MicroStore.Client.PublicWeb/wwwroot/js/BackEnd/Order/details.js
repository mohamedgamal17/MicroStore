$(document).ready(function () {
    var orderId = $("#Id").val();

    var cancelOrderModal = new abp.ModalManager({
        viewUrl : "/Administration/Order/CancelOrderModal" 
    })

    var completeOrderModal = new abp.ModalManager({
        viewUrl: "/Administration/Order/CompleteOrderModal"
    })

    $("#cancelOrderButton").click(function () {
        cancelOrderModal.open({
            id : orderId
        })
    })

    $("#completeOrderButton").click(function () {
        completeOrderModal.open({
            id: orderId
        })
    })

    cancelOrderModal.onResult(function () {
        location.reload()
    })

    completeOrderModal.onResult(function () {
        location.reload()
    })
})