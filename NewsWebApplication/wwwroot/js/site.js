//改变导航栏的样式使得它适应滚动条
$(function () {
    //监听滚动条事件，如果滚动条超过顶部导航栏的两倍，则将导航栏固定在顶部
    var header = document.querySelector('.navbar');
    //console.log(header.clientHeight)
    headerClassName = header.className;
    $(window).scroll(function () {
        var top = $(window).scrollTop();
        if (top > 2 * header.clientHeight) {
            header.className += ' navbar-fixed-top';
        } else {
            header.className = headerClassName;
        }
    })
})


/*显示用户弹出框*/
function showInfoBox() {
    if ($('.User-infobox').css('display') == 'none') {
        $('.User-infobox').css("display", "block");
    } else {
        $('.User-infobox').css("display", "none");
    }
}
function hideInfoBox() {
    $('.User-infobox').css("display", "none");
}

