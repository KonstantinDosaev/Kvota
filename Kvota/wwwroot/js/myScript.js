window.onBlazorReady = () => {
    $('.slider').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        autoplay: true,
        autoplaySpeed: 2000,
        arrows: false,
        rows: 3,
        adaptiveHeight: false
    });
}