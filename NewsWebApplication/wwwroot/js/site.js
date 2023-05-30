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

/*退出*/
function Logout() {
    var sure = window.confirm("确认退出吗？")
    if (sure) {
        $.ajax({
            type: "POST",
            url: "/Login/Logout",
            success: function (msg) {
                alert("退出成功")
                location.reload();
            }
        })
    }
}
/*修改个人信息*/
function check() {
    //layer.confirm('123', {
    //    scrollbar: false,
    //    btn: ['1', '2']
    //}, function () {
    //    console.log("123")
    //})
    layer.confirm('',{
        //formType: 1,
        //placeholder: '输入注销原因',
        title: '个人信息',
        scrollbar: false,
        //area: ['800px', '350px'] //自定义文本域宽高
    }, function () {
        if ($('#pername').val() === "") {
            layer.tips("请输入用户名", $('#pername'));
            return;
        }
        if ($('#perpass').val() === "") {
            layer.tips("请输入密码", $('#perpass'));
            return;
        }
        if ($('#peremail').val() === "") {
            layer.tips("请输入邮箱", $('#peremail'));
            return;
        }
        console.log(document.getElementById('perpass').value);
        alert("修改成功")
        layer.closeAll();
    });
    $(".layui-layer-content").append("<br/>用户：<input type=\"text\" class=\"perin\" id= \"pername\" class=\"layui-input\" placeholder=\"输入用户名\"/>")
    $(".layui-layer-content").append("<br/>密码：<input type=\"password\" class=\"perin\" id= \"perpass\" class=\"layui-input\" placeholder=\"输入密码\"/>")
    $(".layui-layer-content").append("<br/>邮箱：<input type=\"email\" class=\"perin\" id= \"peremail\" class=\"layui-input\" placeholder=\"输入邮箱\"/>")
}
/*反馈*/
function feedback() {
    layer.prompt({
        formType: 2,
        title: '反馈',
        //area: ['800px', '350px'] //自定义文本域宽高
    }, function (value, index, elem) {
        $.ajax({
            type: "POST",
            url: "/Home/Feedback",
            data: { feedback: value },
            success: function (data) {
                console.log("123");
            }
        })
        alert('反馈成功'); //得到value
        console.log("反馈：",value)
        layer.close(index);
    });
}

function Showmore() {
    var RecommList = document.getElementsByClassName("RecList")
    var m = 0
    for (var i = 1; i < RecommList.length; i++) {
        if (RecommList[i].style.display == "none" && RecommList[i - 1].style.display != "none") {
            RecommList[i].style.display = ""
            m++;
        } if (m == 5) {
            break;
        } if (i == RecommList.length - 1) {
            document.getElementsByClassName("Showmore")[0].style.display = "none"
            document.getElementsByClassName("Nomore")[0].style.display = ""
        }
        
    }
}
/*历史记录*/
function history() {
    var UserId = '@ViewData["UserId"]';
    layer.open({
        title: '历史记录',
        scrollbar: false,
        area: 'auto',
        maxHeight: 100,
        minHeight: 50,
        maxmin: true,
        area: [400 + 'px', 500 + 'px'],
        resize: false,
    })
    $.ajax({
        type: "POST",
        url: "/NewsPage/GetHistory",
        data: { UserId: UserId },
        success: function (data) {
            for (var i = 0; i < data.d.length; i++) {

                $(".layui-layer-content").append("<ul class=\"history\"><li><div class=\"rankIcon\"></div><a href='/NewsPage/?Column=" + data.d[i].newColumn + "&Id=" + data.d[i].id + "'>" + data.d[i].newTitle + "</a></div></li>")
            }
        }
    })
    /*$(".layui-layer-content").append("<ul class=\"history\"><li><div class=\"rankIcon\"></div><a>江泽民伟大光辉的一生</a></div></li><li><div class=\"rankIcon\"></div><a>江泽民伟大光辉的一生</a></div></li><li><div class=\"rankIcon\"></div><a>江泽民伟大光辉的一生</a></div></li><li><div class=\"rankIcon\"></div><a>江泽民伟大光辉的一生</a></div></li></ul>")*/
}

/*搜索*/
function Search() {
    var search = document.getElementById('search-input').value
    $.ajax({
        type: "POST",
        url: "/Home/Search",
        data: { search: search },
        success: function (msg) {
            /*window.location.href = "@Url.Action("Search","Home")";*/
        }
    })
}


/*查看新闻的具体内容*/
function NewsDetail() {

    $.ajax({
        url: "/NewsPage/getData",
        type: 'post',
        dataType: "json",
        data: {
            id: 1,
        },//传递给后台的值
        success: function (data) {
            /*console.log(data.d);*/
        }
    });
}
