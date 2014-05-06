$(document).ready(function () {

    //подгрузить все рубрики
    LoadRubrics();

    //добавление поля ввода нового файла
    $(document).on('click', '#add-field-to-file', function () {
        $('.path-image').append("<input type=\"file\" name=\"Files\" class=\"path-to-image\" />");

        return false;
    });

    //выбор рубрики
    $(document).on('click', '.set-rubric ul a', function () {
        var id = $(this).attr('data-number');
        var rubric = $(this).text();
        $(".set-rubric #RubricId").val(id);
        $(".set-rubric .label-text-rubric label").css('color', 'green');
        $(".set-rubric .label-text-rubric label").empty().text(rubric);

    });

    //валидация заголовка новости
    $("#TitlePost").keypress(function () {
        var count = $("#TitlePost").val().length;
        if (count >= 200) return false;
        $($("#TitlePost")[0].previousElementSibling).find(".balance").empty().text(parseInt(200 - count));
    });

    //валидация текста новости
    $("#Text").keypress(function () {
        var count = $("#Text").val().length;
        if (count >= 3000) return false;
        $($("#Text")[0].previousElementSibling).find(".balance").empty().text(parseInt(3000 - count));
    });

    //отправка новости на рассмотрение 
    $(".send-to-add-news #send-news").click(function () {
        var form = $("#form-to-add");

        var error = false;

        if (parseInt($("#RubricId").val()) < 0) {
            $(".label-text-rubric .text-rubric").css("color", "red");
            $(".label-text-rubric .text-rubric").empty().text("Укажите рубрику новости!");
            if (!error) bootbox.alert("<p>Укажите рубрику новости!</p>");
            error = true;
            return false;
        }

        var group = $('.balance');
        group.each(function (index, item) {
            if (parseInt($(item).text()) < 0) {
                if (!error) bootbox.alert("<p>Текст сообщения и заголовка не может превышать установленного ограничения!</p>");
                error = true;
                return false;
            }
        });

        if ($('#TitlePost').val().length < 3) {
            if (!error) bootbox.alert("<p>Заголовок слишком короткий!</p>");
            error = true;
            return false;
        }

        if ($('#Text').val().length < 20) {
            if (!error) bootbox.alert("<p>Текст сообщения слишком короткий!</p>");
            error = true;
            return false;
        }

        var tags = $('.tagbox .tag');
        var links = $('.LinksBox .tag');

        if (tags.length > 0) {
            for (var i = 0; i < tags.length; i++) {
                $('.list-added-tags').append("<input type=\"hidden\" name=\"HashTags\" value=\"" + tags[i].textContent + "\" />");
            }
        }

        if (links.length > 0) {
            for (var i = 0; i < links.length; i++) {
                $('.list-added-links').append("<input type=\"hidden\" name=\"Links\" value=\"" + links[i].textContent + "\" />");
            }
        }

        form.submit();
        $(".send-to-add-news").css('display', 'none');
    });

    //установка настроек видимости новости
    $('.opt-cat').click(function () {
        $(this).find('input').prop('checked', !$(this).find('input').prop('checked'));
    });

    $('.opt-cat input').click(function () {
        $(this).prop('checked', !$(this).prop('checked'));
    });

    //подсветка валидации заголовка и текста
    setInterval(balanceTitle, 1000);
    setInterval(balanceText, 1000);
    setInterval(colorBalance, 1000);

});

function LoadRubrics() {
    var rubrics = $(".set-rubric .list-rubric");

    $.ajax({
        url: "/Apis/LoadRubrics",
        type: "POST",
        data: {},
        success: function (answer) {
            if (answer.Rubrics.length > 0) {
                for (var i = 0; i < answer.Rubrics.length; i++) {
                    rubrics.append("<li><a data-number=\"" + answer.Rubrics[i].Id + "\" class=\"btn-primary\" href=\"javascript:void(0)\">" + answer.Rubrics[i].Rubric + "</a></li>");
                };
            }
        },
        dataType: "json",
        async: false
    });
}

function balanceTitle() {
    var count = $("#TitlePost").val().length;
    $($("#TitlePost")[0].previousElementSibling).find(".balance").empty().text(parseInt(200 - count));
}

function balanceText() {
    var count = $("#Text").val().length;
    $($("#Text")[0].previousElementSibling).find(".balance").empty().text(parseInt(3000 - count));
}

function colorBalance() {
    var groupBalance = $('.balance');
    groupBalance.each(function (index, item) {
        if (parseInt($(item).text()) < 0) {
            $(item).css('color', 'red');
        } else {
            $(item).css('color', 'green');
        }
    });
}

