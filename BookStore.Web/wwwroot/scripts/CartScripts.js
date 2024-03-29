﻿/**************************************************************************************/
/* Add book to cart */
/**************************************************************************************/

$(function () {   
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

/**************************************************************************************/
/* Increment book */
/**************************************************************************************/
$(function () {
    $("div.incrementquantaty").click(function (e) {
        e.preventDefault();

        var id = $(this).data("id");
        var url = "/cart/IncrementProduct";

        $.getJSON(url, { bookId: id }, function (data) {
            $("div.qty" + id).html(data.qty);

            var price = data.qty * data.price;
            var priceHtml = price.toFixed(2) + "$";
            var summaryBookQty = data.qty + " x " + data.title;

            $("div.summary-book-qty" + id).html(summaryBookQty);

            $("div.amount" + id).html(priceHtml);

            var gt = parseFloat($("div.total-amount").text());
            var grandtotal = ((gt + data.price).toFixed(2)) + "$";

            var cartcount = parseInt($("span.cart-count").text());

            $("span.cart-count").text(cartcount+1);
            $("span.cart-price").text(grandtotal);
            $("div.total-amount").text(grandtotal);
        });
    });
});
/**************************************************************************************/
/* Decrement book */
/**************************************************************************************/

$(function () {

    $("div.decrementquantaty").click(function (e) {
        e.preventDefault();

        var id = $(this).data("id");
        var url = "/cart/DecrementProduct";

        $.getJSON(url,
            { bookId: id },
            function (data) {

                if (data.qty == 0) {
                    $("div#book" + id).fadeOut("fast",
                        function () {
                            location.reload();
                        });
                }
                else {
                    $("div.qty" + id).html(data.qty);

                    var price = data.qty * data.price;
                    var priceHtml = price.toFixed(2) + "$";
                    var summaryBookQty = data.qty + " x " + data.title;

                    $("div.summary-book-qty" + id).html(summaryBookQty);
                    $("div.amount" + id).html(priceHtml);

                    var gt = parseFloat($("div.total-amount").text());
                    var grandtotal = (gt - data.price).toFixed(2) + "$";

                    var cartcount = parseInt($("span.cart-count").text());

                    $("span.cart-count").text(cartcount - 1);
                    $("span.cart-price").text(grandtotal);
                    $("div.total-amount").text(grandtotal);
                }
        });
    });
});

//    /**************************************************************************************/
//    /* Remove book */
//    /**************************************************************************************/

$(function () {

    $("div.removebook").click(function (e) {
        e.preventDefault();

        var id = $(this).data("id");
        var url = "/cart/RemoveProduct";
        var quantity = parseInt($("div.qty" + id).text());

        $.get(url,
            { bookId: id, bookQuantity: quantity },
            function (data) {
                location.reload();
            });
    });
});  