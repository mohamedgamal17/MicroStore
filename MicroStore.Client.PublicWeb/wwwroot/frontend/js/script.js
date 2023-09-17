/*-----------------------------------------------------------------------------------

 Template Name:Multikart
 Template URI: themes.pixelstrap.com/multikart
 Description: This is E-commerce website
 Author: Pixelstrap
 Author URI: https://themeforest.net/user/pixelstrap

 ----------------------------------------------------------------------------------- */
// 01.Pre loader
// 02.Tap on Top
// 03.Age verify modal
// 04.Mega menu js
// 05.Image to background js
// 06.Filter js
// 07.Left offer toggle
// 08.Toggle nav
// 09.Footer according
// 10.Add to cart quantity Counter
// 11.Product page Quantity Counter
// 12.Full slider
// 13.Slick slider
// 14.Header z-index js
// 15.Tab js
// 16.Category page
// 17.Filter sidebar js
// 18.Add to cart
// 19.Add to wishlist
// 20.Color Picker
// 21.RTL & Dark-light
// 22.Menu js
// 23.Theme-setting
// 24.Add to cart sidebar js
// 25.Tooltip


(function ($) {
    "use strict";

    /*=====================
     01.Pre loader
     ==========================*/
    $(window).on('load', function () {
        setTimeout(function () {
            $('.loader_skeleton').fadeOut('slow');
            $('body').css({
                'overflow': 'auto'
            });
        }, 500);
        $('.loader_skeleton').remove('slow');
        $('body').css({
            'overflow': 'hidden'
        });
    });
    $('#preloader').fadeOut('slow', function () {
        $(this).remove();
    });


    /*=====================  
     02.Tap on Top
     ==========================*/
    $(window).on('scroll', function () {
        if ($(this).scrollTop() > 600) {
            $('.tap-top').fadeIn();
        } else {
            $('.tap-top').fadeOut();
        }
    });
    $('.tap-top').on('click', function () {
        $("html, body").animate({
            scrollTop: 0
        }, 600);
        return false;
    });


    /*=====================
     03. Age verify modal
     ==========================*/
    $(window).on('load', function () {
        $('#ageModal').modal('show');
    });



    /*=====================
     04. Mega menu js
     ==========================*/
    if ($(window).width() > '1200') {
        $('#hover-cls').hover(
            function () {
                $('.sm').addClass('hover-unset');
            }
        )

    }
    if ($(window).width() > '1200') {
        $('#sub-menu > li').hover(
            function () {
                if ($(this).children().hasClass('has-submenu')) {
                    $(this).parents().find('nav').addClass('sidebar-unset');
                }
            },
            function () {
                $(this).parents().find('nav').removeClass('sidebar-unset');
            }
        )
    }


    /*=====================
     05. Image to background js
     ==========================*/
    $(".bg-top").parent().addClass('b-top');
    $(".bg-bottom").parent().addClass('b-bottom');
    $(".bg-center").parent().addClass('b-center');
    $(".bg_size_content").parent().addClass('b_size_content');
    $(".bg-img").parent().addClass('bg-size');
    $(".bg-img.blur-up").parent().addClass('blur-up lazyload');

    jQuery('.bg-img').each(function () {

        var el = $(this),
            src = el.attr('src'),
            parent = el.parent();

        parent.css({
            'background-image': 'url(' + src + ')',
            'background-size': 'cover',
            'background-position': 'center',
            'display': 'block'
        });

        el.hide();
    });


    /*=====================
     06. Filter js
     ==========================*/
    $(".filter-button").click(function () {
        $(this).addClass('active').siblings('.active').removeClass('active');
        var value = $(this).attr('data-filter');
        if (value == "all") {
            $('.filter').show('1000');
        } else {
            $(".filter").not('.' + value).hide('3000');
            $('.filter').filter('.' + value).show('3000');
        }
    });

    $("#formButton").click(function () {
        $("#form1").toggle();
    });


    /*=====================
     07. left offer toggle
     ==========================*/
    $(".heading-right h3").click(function () {
        $(".offer-box").toggleClass("toggle-cls");
    });


    /*=====================
     08. toggle nav
     ==========================*/
    $('.toggle-nav').on('click', function () {
        $('.sm-horizontal').css("right", "0px");
    });
    $(".mobile-back").on('click', function () {
        $('.sm-horizontal').css("right", "-410px");
    });
    var window_width = jQuery(window).width();
    if ((window_width) > '1199') {
        $("#toggle-sidebar").click(function () {
            $(".marketplace-sidebar").slideToggle('slow');
        });
    }
    if ((window_width) < '1199') {
        $("#toggle-sidebar-res").click(function () {
            $(".marketplace-sidebar").addClass('open-side');
        });
        $(".marketplace-sidebar .sidebar-back").click(function () {
            $(".marketplace-sidebar").removeClass('open-side');
        });
    }
    $("#toggle_sidebar-res").click(function () {
        $(".left-header-sm").addClass('open-side');
    });
    $(".left-header-sm .sidebar-back").click(function () {
        $(".left-header-sm").removeClass('open-side');
    });
    $('.header-style-7 .bar-style ').on('click', function () {
        $('.shop-sidebar').addClass("show");
    });
    $('.sidebar-back, .sidebar-overlay').on('click', function () {
        $('.shop-sidebar').removeClass("show");
    });
    $('.header-style-7 .bar-style, .category-mobile-button').on('click', function () {
        $('.sidebar-overlay').addClass("show");
    });
    $('.sidebar-back, .sidebar-overlay').on('click', function () {
        $('.sidebar-overlay').removeClass("show");
    });
    $('.category-mobile-button').on('click', function () {
        $('.category-shop-section .nav').addClass("show");
    });
    $('.sidebar-back, .sidebar-overlay').on('click', function () {
        $('.category-shop-section .nav').removeClass("show");
    });
    $('.close-btn').on('click', function () {
        console.log("click");
        $('.top-panel-adv').addClass("hide");
    });
    

    /*=========================
     09. left category slider height
     ==========================*/
    var window_width = jQuery(window).width();
    if ((window_width) > '1199') {
        var category_height = $("#sidebar-height").height();
        $('.height-apply').css({ 'height': category_height })
    }


    /*=====================
     10. footer according
     ==========================*/
    var contentwidth = jQuery(window).width();
    if ((contentwidth) < '767') {
        jQuery('.footer-title h4').append('<span class="according-menu"></span>');
        jQuery('.footer-title').on('click', function () {
            jQuery('.footer-title').removeClass('active');
            jQuery('.footer-contant').slideUp('normal');
            if (jQuery(this).next().is(':hidden') == true) {
                jQuery(this).addClass('active');
                jQuery(this).next().slideDown('normal');
            }
        });
        jQuery('.footer-contant').hide();
    } else {
        jQuery('.footer-contant').show();
    }

    if ($(window).width() < '1183') {
        jQuery('.menu-title h5').append('<span class="according-menu"></span>');
        jQuery('.menu-title').on('click', function () {
            jQuery('.menu-title').removeClass('active');
            jQuery('.menu-content').slideUp('normal');
            if (jQuery(this).next().is(':hidden') == true) {
                jQuery(this).addClass('active');
                jQuery(this).next().slideDown('normal');
            }
        });
        jQuery('.menu-content').hide();
    } else {
        jQuery('.menu-content').show();
    }



    /* ===============================
        13-Slick Slider
     =================================*/

    $('.product-slick').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        arrows: true,
        fade: true,
        asNavFor: '.slider-nav'
    });

    $('.slider-nav').slick({
        vertical: false,
        slidesToShow: 3,
        slidesToScroll: 1,
        asNavFor: '.product-slick',
        arrows: false,
        dots: false,
        focusOnSelect: true
    });


    /*=====================
     15.Header z-index js
     ==========================*/
    if ($(window).width() < 1199) {
        $('.header-2 .navbar .sidebar-bar, .header-2 .navbar .sidebar-back, .header-2 .mobile-search img').on('click', function () {
            if ($('#mySidenav').hasClass('open-side')) {
                $('.header-2 #main-nav .toggle-nav').css('z-index', '99');
            } else {
                $('.header-2 #main-nav .toggle-nav').css('z-index', '1');
            }
        });
        $('.sidebar-overlay').on('click', function () {
            $('.header-2 #main-nav .toggle-nav').css('z-index', '99');
        });
        $('.header-2 #search-overlay .closebtn').on('click', function () {
            $('.header-2 #main-nav .toggle-nav').css('z-index', '99');
        });
        $('.layout3-menu .mobile-search .ti-search, .header-2 .mobile-search .ti-search').on('click', function () {
            $('.layout3-menu #main-nav .toggle-nav, .header-2 #main-nav .toggle-nav').css('z-index', '1');
        });
    }


    /*=====================
     16.Tab js
     ==========================*/
    $("#tab-1").css("display", "Block");
    $(".default").css("display", "Block");
    $(".tabs li a").on('click', function () {
        event.preventDefault();
        $('.tab_product_slider').slick('unslick');
        $('.product-4').slick('unslick');
        $(this).parent().parent().find("li").removeClass("current");
        $(this).parent().addClass("current");
        var currunt_href = $(this).attr("href");
        $('#' + currunt_href).show();
        $(this).parent().parent().parent().find(".tab-content").not('#' + currunt_href).css("display", "none");
        $(".product-4").slick({
            arrows: true,
            dots: false,
            infinite: false,
            speed: 300,
            slidesToShow: 4,
            slidesToScroll: 1,
            responsive: [{
                breakpoint: 1200,
                settings: {
                    slidesToShow: 3,
                    slidesToScroll: 3
                }
            },
            {
                breakpoint: 991,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 2
                }
            },
            {
                breakpoint: 420,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1
                }
            }
            ]
        });
    });
    $(".tabs li a").on('click', function () {
        event.preventDefault();
        $('.tab_product_slider').slick('unslick');
        $('.product-5').slick('unslick');
        $(this).parent().parent().find("li").removeClass("current");
        $(this).parent().addClass("current");
        var currunt_href = $(this).attr("href");
        $('#' + currunt_href).show();
        $(this).parent().parent().parent().find(".tab-content").not('#' + currunt_href).css("display", "none");
        // var slider_class = $(this).parent().parent().parent().find(".tab-content").children().attr("class").split(' ').pop();
        $(".product-5").slick({
            arrows: true,
            dots: false,
            infinite: false,
            speed: 300,
            slidesToShow: 5,
            slidesToScroll: 1,
            responsive: [{
                breakpoint: 1367,
                settings: {
                    slidesToShow: 4,
                    slidesToScroll: 4
                }
            },
            {
                breakpoint: 1024,
                settings: {
                    slidesToShow: 3,
                    slidesToScroll: 3,
                    infinite: true
                }
            },
            {
                breakpoint: 768,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 2
                }
            },
            {
                breakpoint: 576,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1
                }
            }
            ]

        });
    });


    $("#tab-1").css("display", "Block");
    $(".default").css("display", "Block");
    $(".tabs li a").on('click', function () {
        event.preventDefault();
        $('.tab_product_slider').slick('unslick');
        $('.product-3').slick('unslick');
        $(this).parent().parent().find("li").removeClass("current");
        $(this).parent().addClass("current");
        var currunt_href = $(this).attr("href");
        $('#' + currunt_href).show();
        $(this).parent().parent().parent().parent().find(".tab-content").not('#' + currunt_href).css("display", "none");
        $(".product-3").slick({
            arrows: true,
            dots: false,
            infinite: false,
            speed: 300,
            slidesToShow: 3,
            slidesToScroll: 1,
            responsive: [{
                breakpoint: 991,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 2
                }
            }]
        });
    });


    /*=====================
     17 .category page
     ==========================*/
    $('.collapse-block-title').on('click', function (e) {
        e.preventDefault;
        var speed = 300;
        var thisItem = $(this).parent(),
            nextLevel = $(this).next('.collection-collapse-block-content');
        if (thisItem.hasClass('open')) {
            thisItem.removeClass('open');
            nextLevel.slideUp(speed);
        } else {
            thisItem.addClass('open');
            nextLevel.slideDown(speed);
        }
    });
    $('.color-selector ul li').on('click', function (e) {
        $(".color-selector ul li").removeClass("active");
        $(this).addClass("active");
    });
    $('.color-w-name ul li').on('click', function (e) {
        $(".color-w-name ul li").removeClass("active");
        $(this).addClass("active");
    });
    //list layout view
    $('.list-layout-view').on('click', function (e) {
        $('.collection-grid-view').css('opacity', '0');
        $(".product-wrapper-grid").css("opacity", "0.2");
        $('.shop-cart-ajax-loader').css("display", "block");
        $('.product-wrapper-grid').addClass("list-view");
        $(".product-wrapper-grid").children().children().removeClass();
        $(".product-wrapper-grid").children().children().addClass("col-lg-12");
        setTimeout(function () {
            $(".product-wrapper-grid").css("opacity", "1");
            $('.shop-cart-ajax-loader').css("display", "none");
        }, 500);
    });
    //grid layout view
    $('.grid-layout-view').on('click', function (e) {
        $('.collection-grid-view').css('opacity', '1');
        $('.product-wrapper-grid').removeClass("list-view");
        $(".product-wrapper-grid").children().children().removeClass();
        $(".product-wrapper-grid").children().children().addClass("col-lg-3");

    });
    $('.product-2-layout-view').on('click', function (e) {
        if ($('.product-wrapper-grid').hasClass("list-view")) { } else {
            $(".product-wrapper-grid").children().children().removeClass();
            $(".product-wrapper-grid").children().children().addClass("col-lg-6");
        }
    });
    $('.product-3-layout-view').on('click', function (e) {
        if ($('.product-wrapper-grid').hasClass("list-view")) { } else {
            $(".product-wrapper-grid").children().children().removeClass();
            $(".product-wrapper-grid").children().children().addClass("col-lg-4 col-6");
        }
    });
    $('.product-4-layout-view').on('click', function (e) {
        if ($('.product-wrapper-grid').hasClass("list-view")) { } else {
            $(".product-wrapper-grid").children().children().removeClass();
            $(".product-wrapper-grid").children().children().addClass("col-xl-3 col-6");
        }
    });
    $('.product-6-layout-view').on('click', function (e) {
        if ($('.product-wrapper-grid').hasClass("list-view")) { } else {
            $(".product-wrapper-grid").children().children().removeClass();
            $(".product-wrapper-grid").children().children().addClass("col-lg-2");
        }
    });


    /*=====================
     18.filter sidebar js
     ==========================*/
    $('.sidebar-popup').on('click', function (e) {
        $('.open-popup').toggleClass('open');
        $('.collection-filter').css("left", "-15px");
    });
    $('.filter-btn').on('click', function (e) {
        $('.collection-filter').css("left", "-15px");
    });
    $('.filter-back').on('click', function (e) {
        $('.collection-filter').css("left", "-365px");
        $('.sidebar-popup').trigger('click');
    });
    $('.account-sidebar').on('click', function (e) {
        $('.dashboard-left').css("left", "0");
    });
    $('.filter-back').on('click', function (e) {
        $('.dashboard-left').css("left", "-365px");
    });

    $(function () {
        $(".product-load-more .col-grid-box").slice(0, 8).show();
        $(".loadMore").on('click', function (e) {
            e.preventDefault();
            $(".product-load-more .col-grid-box:hidden").slice(0, 4).slideDown();
            if ($(".product-load-more .col-grid-box:hidden").length === 0) {
                $(".loadMore").text('no more products');
            }
        });
    });

    var t;

    $(function () {
        $(".infinite-product .product-box").slice(0, 8).show();
        $(".load-product").on('click', function (e) {
            e.preventDefault();
            $(this).addClass('loading');
            t = setTimeout(function () {
                $(".load-product").removeClass("loading");
                $(".infinite-product .product-box:hidden").slice(0, 4).slideDown();
                if ($(".infinite-product .product-box:hidden").length === 0) {
                    $(".load-product").text('no more products');
                    $(".load-product").addClass('mt-4');
                }
            }, 2500);

        });
    });


  


    /*=====================
     20.Add to wishlist
     ==========================*/
    $('.product-box a .ti-heart , .product-box a .fa-heart').on('click', function () {

        $.notify({
            icon: 'fa fa-check',
            title: 'Success!',
            message: 'Item Successfully added in wishlist'
        }, {
            element: 'body',
            position: null,
            type: "info",
            allow_dismiss: true,
            newest_on_top: false,
            showProgressbar: true,
            placement: {
                from: "top",
                align: "right"
            },
            offset: 20,
            spacing: 10,
            z_index: 1031,
            delay: 5000,
            animate: {
                enter: 'animated fadeInDown',
                exit: 'animated fadeOutUp'
            },
            icon_type: 'class',
            template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
                '<button type="button" aria-hidden="true" class="btn-close" data-notify="dismiss"></button>' +
                '<span data-notify="icon"></span> ' +
                '<span data-notify="title">{1}</span> ' +
                '<span data-notify="message">{2}</span>' +
                '<div class="progress" data-notify="progressbar">' +
                '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
                '</div>' +
                '<a href="{3}" target="{4}" data-notify="url"></a>' +
                '</div>'
        });
    });


})(jQuery);


/*=====================
 21. Dark Light
 ==========================*/

var body_event = $("body");
body_event.on("click", ".dark-btn", function () {
    $(this).toggleClass('dark');
    $('body').removeClass('dark');
    if ($('.dark-btn').hasClass('dark')) {
        $('.dark-btn').text('Light');
        $('body').addClass('dark');
    } else {
        $('#theme-dark').remove();
        $('.dark-btn').text('Dark');
    }

    return false;
});


/*=====================
 22. Menu js
 ==========================*/
function openNav() {
    document.getElementById("mySidenav").classList.add('open-side');
}

function closeNav() {
    document.getElementById("mySidenav").classList.remove('open-side');
}
$(function () {
    $('#main-menu').smartmenus({
        subMenusSubOffsetX: 1,
        subMenusSubOffsetY: -8
    });
    $('#sub-menu').smartmenus({
        subMenusSubOffsetX: 1,
        subMenusSubOffsetY: -8
    });
});


/*=====================
 23.Tooltip
 ==========================*/
$(window).on('load', function () {
    $('[data-toggle="tooltip"]').tooltip()
});

/*=====================
 24. Cookiebar
 ==========================*/
window.setTimeout(function () {
    $(".cookie-bar").addClass('show')
}, 5000);

$('.cookie-bar .btn, .cookie-bar .btn-close').on('click', function () {
    $(".cookie-bar").removeClass('show')
});

/*=====================
 25. Recently puchase modal
 ==========================*/
setInterval(function () {
    $(".recently-purchase").toggleClass('show')
}, 20000);

$('.recently-purchase .close-popup').on('click', function () {
    $(".recently-purchase").removeClass('show')
});


/*=====================
 26. other js
 ==========================*/
var width_content = jQuery(window).width();
if ((width_content) > '991') {

    $(".filter-bottom-title").click(function () {
        $(".filter-bottom-content").slideToggle("");
    });
    $(".close-filter-bottom").click(function () {
        $(".filter-bottom-content").slideUp("");
    });
}
else {
    $(".filter-bottom-title").click(function () {
        $(".filter-bottom-content").toggleClass("open");
    });
    $(".close-filter-bottom").click(function () {
        $(".filter-bottom-content").removeClass("open");
    });
}

if ((width_content) < '991') {
    $('.filter-bottom-title').on('click', function (e) {
        $('.filter-bottom-content').css("left", "-15px");
    });
}

$('.color-variant li').on('click', function (e) {
    $(".color-variant li").removeClass("active");
    $(this).addClass("active");
});

$('.custom-variations li').on('click', function (e) {
    $(".custom-variations li").removeClass("active");
    $(this).addClass("active");
});

$('.size-box ul li').on('click', function (e) {
    $(".size-box ul li").removeClass("active");
    $('#selectSize').removeClass('cartMove');
    $(this).addClass("active");
    $(this).parent().addClass('selected');
});

$('#cartEffect').on('click', function (e) {
    if ($("#selectSize .size-box ul").hasClass('selected')) {
        $('#cartEffect').text("Added to bag ");
        $('.added-notification').addClass("show");
        setTimeout(function () {
            $('.added-notification').removeClass("show");
        }, 5000);
    } else {
        $('#selectSize').addClass('cartMove');
    }
});

// modern product box plus effect
$('.add-extent .animated-btn').on('click', function (e) {
    $(this).parents(".add-extent").toggleClass("show");
});
