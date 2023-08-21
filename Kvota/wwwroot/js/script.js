onBlazorReady = () => {
    $('.slider').slick({
        lazyLoad: 'ondemand',
        arrows: true,

    });
  /*  $('.slider').slick('slickGoTo', 0)*/;
}
unslickSlider = () => {
    $('.slider').slick('unslick');
}