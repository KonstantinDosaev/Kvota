onBlazorReady = () => {
    $('.slider').slick({
        lazyLoad: 'ondemand',
        arrows: true,

    });
}
window.onHomeReady = () => {
    $('.homeslider').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        autoplay: true,
        autoplaySpeed: 5000,
        arrows: true,
    });
    $('.product-home__slider').slick({
        slidesToShow: 4,
        slidesToScroll: 1,
        arrows: true,
        infinite: false,
        responsive: [
            {
                breakpoint: 1600,
                settings: {
                    slidesToShow: 3,
                }
            },
            {
                breakpoint: 1260,
                settings: {
                    slidesToShow: 2,
                }
            },
            {
                breakpoint: 660,
                settings: {
                    slidesToShow: 1,
                }
            }
        ]
           
    });

}
unslickSlider = () => {
    $('.slider').slick('unslick');
}

window.triggerFileDownload = (fileName, url) => {
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
}

