$(document).ready(function () {
	getTime();
	
});

//得到当前的时间并且显示在页面上
var getTime = function () {
    var d = new Date();
    var time = d.getFullYear().toString() + "年 " +
        (d.getMonth() + 1).toString() + "月 " +
        d.getDate().toString() + "号 ";
    var list = ["星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期日"]
    for (var i = 0; i < list.length; i++) {
        if (d.getDay() - 1 == i) {
            time += list[i];
        }
    }
	document.querySelector("#time").innerHTML = time
}




//轮播图
$(function () {
	var index = 0;
	var timer = null;
	var option = $('.Cachart_container>ul>li').length;
	var imgwidth = $('.Cachart_container ul li').width();

	var $li = $('.Cachart_container ul li')[0];
	$('.Cachart_container>ul').append($li);



	go();
	$(".Cachart_container").mouseleave(function () {
		$(".Cachart_container ul .NewTitle").fadeOut("slow");
	})
	$(".Cachart_container").hover(function () {
		clearInterval(timer)
		$(".Cachart_container ul .NewTitle").fadeIn("slow");
		/*$(".Cachart_container ul .NewTitle").css("display", "");*/
	}, function () {
		go();
	})
	function go() {
		$(window).resize(function () {
			imgwidth = $('.Cachart_container ul li').width();
		});
		timer = setInterval(function () {
			if (index < option) {
				index++;
				$(".Cachart_container ul").stop().animate({
					left: -imgwidth * index - 20 * index + 'px'
				})
				$(".Cachart_container ul .NewTitle").fadeOut("");
				/*$(".Cachart_container ul .NewTitle").css("display", "none");*/
				console.log("success")
			}
			if (index == option) {
				$(".Cachart_container ul").stop().animate({
					left: -imgwidth * index + 'px'
				})

				index = 0;
				$(".Cachart_container ul").animate({
					left: -imgwidth * index + 'px'
				}, 0)
			}

			$("ol li").eq(index).addClass('current').siblings().removeClass();
		}, 3000)
	}
	$("ol li").mouseover(function () {
		index = $(this).index();
		$(".Cachart_container ul").animate({
			left: -imgwidth * index - 20 * index + 'px'
		})
		$("ol li").eq(index).addClass('current').siblings().removeClass();
	})
})