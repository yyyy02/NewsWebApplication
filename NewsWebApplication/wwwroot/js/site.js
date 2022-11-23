$(document).ready(function () {
    getTime();
});

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

//得到当前的时间并且显示在页面上
var getTime = function () {
    var d = new Date();
    var time = d.getFullYear().toString() + "年 " +
        (d.getMonth() + 1).toString() + "月 " +
        d.getDate().toString() + "号 ";
    var list = ["星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期日"]
    for (var i = 0; i < list.length; i++) {
        if (d.getDay()-1 == i) {
            time += list[i];
        }
    }
    document.querySelector("#time").innerHTML = time
}