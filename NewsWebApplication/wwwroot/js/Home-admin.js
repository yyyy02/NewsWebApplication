function Showuser() {
    $('#NewList').css("display", "none");
    $('#FeedbackList').css("display", "none");
    $('#UserList').css("display", "");
    $('#User').css("color", "rgb(69,141,255)");
    $('#New').css("color", "rgb(0,0,0)");
    $('#Feedback').css("color", "rgb(0,0,0)");
}

function Shownew() {
    $('#UserList').css("display", "none");
    $('#FeedbackList').css("display", "none");
    $('#NewList').css("display", "");
    $('#New').css("color", "rgb(69,141,255)");
    $('#User').css("color", "rgb(0,0,0)");
    $('#Feedback').css("color", "rgb(0,0,0)");
}
function Showfeedback() {
    $('#FeedbackList').css("display", "");
    $('#UserList').css("display", "none");
    $('#NewList').css("display", "none");
    $('#Feedback').css("color", "rgb(69,141,255)");
    $('#New').css("color", "rgb(0,0,0)");
    $('#User').css("color", "rgb(0,0,0)");
}

function delAllNews() {
    if (confirm("确定要删除吗")) {
        $('.NewTable').css("display", "none");
    }
}

function delAllUsers() {
    if (confirm("确定要删除吗")) {
        $('.UserTable').css("display", "none");
    }
}

function changeUser() {
    layer.confirm('', {
        title: '修改',
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
        $.ajax({
            url: "/Home/changeUser",
            type: "POST",
            data: { name: $('#pername').val(), password: $('#perpass').val(), email: $('#peremail').val() },
            sucess: function (msg) {
                alert("Signup successfully")
            }
        })
        alert("修改成功")
        layer.closeAll();
        location.reload();
    });
    $(".layui-layer-content").append("<br/>用户：<input type=\"text\" class=\"perin\" id= \"pername\" class=\"layui-input\" placeholder=\"输入用户名\"/>")
    $(".layui-layer-content").append("<br/>密码：<input type=\"password\" class=\"perin\" id= \"perpass\" class=\"layui-input\" placeholder=\"输入密码\"/>")
    $(".layui-layer-content").append("<br/>邮箱：<input type=\"email\" class=\"perin\" id= \"peremail\" class=\"layui-input\" placeholder=\"输入邮箱\"/>")
}

/*修改个人信息*/
function addUser() {
    layer.confirm('', {
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
