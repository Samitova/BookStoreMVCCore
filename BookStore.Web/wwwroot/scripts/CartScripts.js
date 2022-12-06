$(function () {
    /*****************************************AddToCart*********************************************/
    $("a.addtocart").click(function (e) {
        e.preventDefault();

        var bookId = e.target.id;
        var url = "/cart/AddToCartPartial";

        $.get(url, {
            id: bookId
        }, function myfunction(data) {
            $(".ajaxcart").html(data);
        }).done(function () {
            $("#ajaxmsg_" + bookId).addClass("ib");

            setTimeout(function () {
                $("#ajaxmsg_" + bookId).fadeOut("fast");
                $("#ajaxmsg_" + bookId).removeClass("ib");
            }, 2000);
        });
    });
});