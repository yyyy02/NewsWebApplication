let classes = ['left', 'center', 'right', 'after1', 'after2'];
let timer = setInterval(function () { before(); }, 5000);
function before() {
    for (let i = 0; i < classes.length; i++) {
        $('.b:eq(' + i + ')').css("background-color", "rgb(244,244,244)");
    }
    let last = classes.pop();
    classes.unshift(last);
    for (let i = 0; i < classes.length; i++) {
        $('#ul>li:eq(' + i + ')').attr("class", classes[i]);
    }
    for (let i = 0; i < classes.length; i++) {
        if ($('#ul>li:eq(' + i + ')').attr("class") == 'center') {
            $('.b:eq(' + i + ')').css("background-color", "rgb(255,80,0)");
        }
    }
}
function after() {
    for (let i = 0; i < classes.length; i++) {
        $('.b:eq(' + i + ')').css("background-color", "rgb(244,244,244)");
    }
    let first = classes.shift();
    classes.push(first);
    for (let i = 0; i < classes.length; i++) {
        $('#ul>li:eq(' + i + ')').attr("class", classes[i]);
    }
    for (let i = 0; i < classes.length; i++) {
        if ($('#ul>li:eq(' + i + ')').attr("class") == 'center') {
            $('.b:eq(' + i + ')').css("background-color", "rgb(255,80,0)");
        }
    }
}
for (let i = 0; i < classes.length; i++) {
    (function (i) {
        $('#ul>li:eq(' + i + ')').click(function () {
            if ($('#ul>li:eq(' + i + ')').attr("class") == "left") {
                after();
            } else if ($('#ul>li:eq(' + i + ')').attr("class") == "right") {
                before();
            } else {
                return false;
            }
        });
        $('.b:eq(' + i + ')').mouseover(function () {
            $('.b:eq(' + i + ')').css("background", "red");
            clearInterval(timer);
            while (classes[i] != 'center') {
                before();
            }
        });
        $('.b:eq(' + i + ')').mouseout(function () {
            $('.b:eq(' + i + ')').css("background", "rgb(244,244,244)");
            clearInterval(timer);
            timer = setInterval(function () {
                before();
            }, 5000);
        });
    })(i);
}

//$('.last').click(function () {
//    clearInterval(timer);
//    before();
//    timer = setInterval(function () {
//        before();
//    }, 5000);
//});
//$('.next').click(function () {
//    clearInterval(timer);
//    after();
//    timer = setInterval(function () {
//        before();
//    }, 5000);
//});
$('.Pic-list').mouseover(function () {
    $('.last,.next').css("display", "block");
    $(".roll-img .last ul .center").fadeIn("slow");
    clearInterval(timer);
});
$('.Pic-list').mouseout(function () {
    $('.last,.next').css("display", "none");
    $(".roll-img .last ul .center").fadeOut("slow");
    clearInterval(timer);
    timer = setInterval(function () {
        before();
    }, 5000);
});