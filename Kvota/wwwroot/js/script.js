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

window.triggerFileDownload = (fileName, url) => {
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
}

window.addEventListener('beforeunload', function () {
    Blazor.defaultReconnectionHandler._reconnectionDisplay = {};
});