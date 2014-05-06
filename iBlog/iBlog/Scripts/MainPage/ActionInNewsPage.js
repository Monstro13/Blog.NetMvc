$(document).ready(function () {
    //работа со слайдером
    $(".slider").each(function () // обрабатываем каждый слайдер
    {
        var obj = $(this);
        $(obj).append("<div class='nav'></div>");

        $(obj).find("li").each(function () {
            $(obj).find(".nav").append("<span rel='" + $(this).index() + "'></span>"); // добавляем блок навигации
            $(this).addClass("slider" + $(this).index());
        });

        $(obj).find("span").first().addClass("on"); // делаем активным первый элемент меню
    });

    //работа с рейтингом комментариев
    $(document).on('click', ".up-komment-rating img, .down-komment-rating img", function () {
        var command = $(this.parentNode.parentNode).hasClass("up-komment-rating") ? "up" : "down";
        var kommentId = $(this.parentNode.parentNode.parentNode).find("input").val();
        var obj = this;

        $.ajax({
            url: "/Apis/ChangeKommentRating",
            type: "POST",
            data: { command: command, kommentId: kommentId },
            success: function (answer) {
                if (answer == "True") {
                    var label = $(obj.parentNode.parentNode.parentNode).find("label");
                    var value = label.text();
                    if (command == "up")
                        label.empty().append(parseInt(value) + 1);
                    else
                        label.empty().append(parseInt(value) - 1);
                }
            },
            dataType: "json",
            async: false
        });
    });

    //ответ на комментарии, добавление статуса "ответ" к комментарию
    $(document).on('click', '.to-answer', function () {
        var kommentId = $($(this.parentNode.parentNode).find(".komment-text-wrapper")).find(".kommentar-id").val();
        var login = $($(this.parentNode).find(".komment-author")).find("a").text();
        $(".komments-block-body").find(".id-komment-to-answer").val(kommentId);

        $(".komments-block-header .new-text-komments").focus();
        $(".komments-block-header .new-text-komments").val(login + ", ");
        $(".komments-block-header .new-text-komments").addClass("answer-to-kommentar");

        var inputscroll = $(".komments-block-header textarea").scrollHeight;
        $(window).scrollTop(inputscroll);
    });

    //само добавление комментария
    $(".input-to-add-komment").click(function () {
        
        if ($(".new-text-komments").val().length >= 2999) {
            $(".new-text-komments").val($(".new-text-komments").val().substr(0, 2998));
            return false;
        }

        var answerId = -1;

        if ($(".komments-block-header .new-text-komments").hasClass("answer-to-kommentar")) {
            var kommentId = $(".komments-block-body").find(".id-komment-to-answer").val();
            $(".komments-block-body").find(".id-komment-to-answer").val(-1);

            var textAnswer = $(".komments-block-header textarea").val();
            
            var obj = $(this);
            $.ajax({
                url: "/Apis/AddAnswer",
                type: "POST",
                data: { kommentId: kommentId, answerText: textAnswer },
                success: function (answer) {
                    if (parseInt(answer) < 0) {
                        bootbox.alert("<p>Ошибка</p>Попытайтесь снова!");
                    }
                    obj.removeClass("answer-to-kommentar");
                    answerId = parseInt(answer);
                },
                dataType: "json",
                async: false
            });
        }

        var komment = $(".new-text-komments").val();
        var postId = $(".change-rating .post-id").val();
        var userlogin = "";
        var date = "";
        var rating = 0;
        kommentId = 0;

        if (komment.length > 0) {
            $.ajax({
                url: "/Apis/AddKomment",
                type: "POST",
                data: { komment: komment, postId: postId, answerId: answerId },
                success: function (answer) {
                    if (answer.Login.length > 0) {
                        userlogin = answer.Login;
                        date = answer.Date;
                        kommentId = answer.Id;

                        $(".komments-block-body").append("<div class=\"komment-wrapper\"><div class=\"hoch-line-news clearfix\"><div class=\"date-part float-left\">" + date + "</div><div class=\"rating-part float-right\"><span class=\"change-rating\"><input type=\"hidden\" class=\"kommentar-id\" value=\"" + kommentId + "\" /><span class=\"up-komment-rating\"><a class=\"btn\"><img src=\"/Images/Rating/rating_y_up.png\" /></a></span><label class=\"label-rating\">0</label><span class=\"down-komment-rating\"><a class=\"btn\"><img src=\"/Images/Rating/rating_t_down.png\" /></a></span></span></div></div><div class=\"komment-text-wrapper clearfix pos-rel\"><input type=\"hidden\" class=\"kommentar-id\" value=\"" + kommentId + "\"/><div class=\"komment-text\">" + komment + "</div><div class=\"change-and-delete float-right\"><div class=\"change-text-komment\"><a href=\"javascript:void(0)\"><img src=\"/Images/OptionPost/edit.png\"/></a></div><div class=\"delete-komment\"><a href=\"javascript:void(0)\"><img src=\"/Images/OptionPost/delete.png\"/></a></div></div></div><div class=\"down-line-news clearfix\"><div class=\"komment-author float-left\">Автор:&nbsp;<a class=\"extend-info-user\" href=\"javascript:void(0)\">" + userlogin + "</a></div><div class=\"to-answer float-right\"><a href=\"javascript:void(0)\">Ответить</a></div></div></div>");
                    }
                },
                dataType: "json",
                async: false
            });
        }

        $(".new-text-komments").val('');
        var scroll = $(window).scrollTop();
        $(window).scrollTop(scroll + 110);

    });

    //добавление комментария по нажатию энтер
    $(".new-text-komments").keypress(function (e) {

        if ($(this).val().length >= 2999) {
            $(this).val($(this).val().substr(0, 2998));
            return false;
        }

        var answerId = -1;

        if ($(".komments-block-header .new-text-komments").hasClass("answer-to-kommentar")) {
            if (e.keyCode == 13) {
                var kommentId = $(".komments-block-body").find(".id-komment-to-answer").val();
                $(".komments-block-body").find(".id-komment-to-answer").val(-1);

                var textAnswer = $(".komments-block-header textarea").val();

                var obj = $(this);
                $.ajax({
                    url: "/Apis/AddAnswer",
                    type: "POST",
                    data: { kommentId: kommentId, answerText: textAnswer },
                    success: function (answer) {
                        if (parseInt(answer) < 0) {
                            bootbox.alert("<p>Ошибка</p>Попытайтесь снова!");
                        }
                        obj.removeClass("answer-to-kommentar");
                        answerId = parseInt(answer);
                    },
                    dataType: "json",
                    async: false
                });
            }
        }

        if (e.keyCode == 13) {
            var komment = $(".new-text-komments").val();
            var postId = $(".change-rating .post-id").val();
            var userlogin = "";
            var date = "";
            var rating = 0;
            kommentId = 0;

            if (komment.length > 0) {
                $.ajax({
                    url: "/Apis/AddKomment",
                    type: "POST",
                    data: { komment: komment, postId: postId, answerId: answerId },
                    success: function (answer) {
                        if (answer.Login.length > 0) {
                            userlogin = answer.Login;
                            date = answer.Date;
                            kommentId = answer.Id;

                            $(".komments-block-body").append("<div class=\"komment-wrapper\"><div class=\"hoch-line-news clearfix\"><div class=\"date-part float-left\">" + date + "</div><div class=\"rating-part float-right\"><span class=\"change-rating\"><input type=\"hidden\" class=\"kommentar-id\" value=\"" + kommentId + "\" /><span class=\"up-komment-rating\"><a class=\"btn\"><img src=\"/Images/Rating/rating_y_up.png\" /></a></span><label class=\"label-rating\">0</label><span class=\"down-komment-rating\"><a class=\"btn\"><img src=\"/Images/Rating/rating_t_down.png\" /></a></span></span></div></div><div class=\"komment-text-wrapper clearfix pos-rel\"><input type=\"hidden\" class=\"kommentar-id\" value=\"" + kommentId + "\"/><div class=\"komment-text\">" + komment + "</div><div class=\"change-and-delete float-right\"><div class=\"change-text-komment\"><a href=\"javascript:void(0)\"><img src=\"/Images/OptionPost/edit.png\"/></a></div><div class=\"delete-komment\"><a href=\"javascript:void(0)\"><img src=\"/Images/OptionPost/delete.png\"/></a></div></div></div><div class=\"down-line-news clearfix\"><div class=\"komment-author float-left\">Автор:&nbsp;<a class=\"extend-info-user\" href=\"javascript:void(0)\">" + userlogin + "</a></div><div class=\"to-answer float-right\"><a href=\"javascript:void(0)\">Ответить</a></div></div></div>");
                        }
                    },
                    dataType: "json",
                    async: false
                });
            }

            $(".new-text-komments").val('');
            var scroll = $(window).scrollTop();
            $(window).scrollTop(scroll + 110);
            return false;
        }
    });

    //если хотели ответить, но передумали то просто убрали курсор, ответ отменен
    $(".new-text-komments").focusout(function() {
        if ($(this).hasClass("answer-to-kommentar")) {
            $(this).removeClass("answer-to-kommentar");
            $(".komments-block-body").find(".id-komment-to-answer").val(-1);
            $(this).val("");
        }
    });

    //если редактирование комментария возможно то отобразить панель
    $(document).on('mouseover', '.komment-text-wrapper', function () {
        var obj = $($(this).find(".change-and-delete"));
        var kommentId = $($(this).find(".kommentar-id")).val();

        $.ajax({
            url: "/Apis/ChangeIsPossible",
            type: "POST",
            data: { kommentarId: kommentId },
            success: function (answer) {
                if (answer == "True")
                    obj.fadeIn(150);
            },
            dataType: "json",
            async: false
        });
    });

    //скрыть панель, если курсор вне области редактирования
    $(document).on('mouseleave', '.komment-text-wrapper', function () {
        $($(this).find(".change-and-delete")).fadeOut(150);
    });

    //удаление комментария
    $(document).on('click', '.delete-komment img', function () {
        var kommentId = $(this.parentNode.parentNode.parentNode.parentNode).find(".kommentar-id").val();
        var kommentWrap = $(this.parentNode.parentNode.parentNode.parentNode.parentNode);

        $.ajax({
            url: "/Apis/DeleteKomment",
            type: "POST",
            data: { kommentarId: kommentId },
            success: function (answer) {
                if (answer == "True") {
                    kommentWrap.empty().append("<input type=\"hidden\" value=\"" + kommentId + "\">").append("<a class=\"recovery-komment\" href=\"javascript:void(0)\">Восстановить</a>");
                    kommentWrap.css('textAlign', 'center');
                }
                else bootbox.alert("<p>Ошибка!</p>Возможно комментарий уже оценили!");
            },
            dataType: "json",
            async: false
        });

    });

    //открытие области для редактирования содержимого комментария
    $(document).on('click', '.change-text-komment img', function () {
        var kommentWrap = $(this.parentNode.parentNode.parentNode.parentNode);
        var heig = kommentWrap.find('.komment-text').css('height');

        var text = kommentWrap.find('.komment-text').text();
        kommentWrap.append("<textarea class=\"changed-text-kommentar\" style=\"height:" + heig + ";top:0px; \">" + text + "</textarea>");
    });

    //сохранения содержимого комментария по нажатию энтер
    $(document).on('keypress', '.changed-text-kommentar', function (e) {
        if (e.keyCode == 13) {
            var kommentId = $(this.parentNode).find(".kommentar-id").val();
            var text = $(this).val();
            var obj = $(this);
            var parent = $(this.parentNode);

            $.ajax({
                url: "/Apis/ChangeKommentar",
                type: "POST",
                data: { kommentarId: kommentId, message: text },
                success: function (answer) {
                    if (answer == "True") {
                        obj.remove();
                        parent.find(".komment-text").text(text);
                    } else {
                        bootbox.alert("<p>Ошибка</p>Возможно комментарий уже оценен ранее!");
                    }
                },
                dataType: "json",
                async: false
            });

            return false;
        }
    });

    //восстаносить комментарий по нажатию ссылки
    $(document).on('click', '.recovery-komment', function () {
        var kommentId = $(this.previousElementSibling).val();
        var kommentWrap = $(this.parentNode);
        var userlogin = "";
        var date = "";
        var komment = "";

        $.ajax({
            url: "/Apis/RecoveryKomment",
            type: "POST",
            data: { kommentId: kommentId },
            success: function (answer) {
                if (answer.Id != 0) {
                    userlogin = answer.Login;
                    date = answer.Date;
                    kommentId = answer.Id;
                    komment = answer.Message;

                    kommentWrap.empty().append("<div class=\"hoch-line-news clearfix\"><div class=\"date-part float-left\">" + date + "</div><div class=\"rating-part float-right\"><span class=\"change-rating\"><input type=\"hidden\" class=\"kommentar-id\" value=\"" + kommentId + "\" /><span class=\"up-komment-rating\"><a class=\"btn\"><img src=\"/Images/Rating/rating_y_up.png\" /></a></span><label class=\"label-rating\">0</label><span class=\"down-komment-rating\"><a class=\"btn\"><img src=\"/Images/Rating/rating_t_down.png\" /></a></span></span></div></div><div class=\"komment-text-wrapper clearfix pos-rel\"><input type=\"hidden\" class=\"kommentar-id\" value=\"" + kommentId + "\"/><div class=\"komment-text\">" + komment + "</div><div class=\"change-and-delete float-right\"><div class=\"change-text-komment\"><a href=\"javascript:void(0)\"><img src=\"/Images/OptionPost/edit.png\"/></a></div><div class=\"delete-komment\"><a href=\"javascript:void(0)\"><img src=\"/Images/OptionPost/delete.png\"/></a></div></div></div><div class=\"down-line-news clearfix\"><div class=\"komment-author float-left\">Автор:&nbsp;<a class=\"extend-info-user\" href=\"javascript:void(0)\">" + userlogin + "</a></div><div class=\"to-answer float-right\"><a href=\"javascript:void(0)\">Ответить</a></div></div>");
                    kommentWrap.css('textAlign', 'left');
                }
            },
            dataType: "json",
            async: false
        });

    });

});



function sliderJS(obj, sl) // slider function
{
    var ul = $(sl).find("ul"); // находим блок
    var bl = $(sl).find("li.slider" + obj); // находим любой из элементов блока
    var step = $(bl).width(); // ширина объекта
    $(ul).animate({ marginLeft: "-" + step * obj }, 350); // 500 это скорость перемотки
}

$(document).on("click", ".slider .nav span", function () // slider click navigate
{
    var sl = $(this).closest(".slider"); // находим, в каком блоке был клик
    $(sl).find("span").removeClass("on"); // убираем активный элемент
    $(this).addClass("on"); // делаем активным текущий
    var obj = $(this).attr("rel"); // узнаем его номер
    sliderJS(obj, sl); // слайдим
    return false;
});