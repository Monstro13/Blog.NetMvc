$(document).ready(function () {
    //установка фильтров
    var currentFilter = filter.all;
    getItems(currentFilter);

    //категория - все новости
    $(document).on('click', '#all-news-admin', function () {
        $('#pager-curr a').text(1);
        currentFilter = filter.all;
        getItems(currentFilter);
    });

    // категория - неподтвержденыые новости
    $(document).on('click', '#unConfirm-news-admin', function() {
        $('#pager-curr a').text(1);
        currentFilter = filter.unConfirm;
        getItems(currentFilter);
    });

    // категория - запрещенные новости
    $(document).on('click', '#banned-news-admin', function() {
        $('#pager-curr a').text(1);
        currentFilter = filter.banned;
        getItems(currentFilter);
    });

    // пагинация- следующая страница
    $(document).on('click', '#pager-next', function() {
        $('#pager-curr a').text(parseInt($('#pager-curr a').text()) + 1);
        getItems(currentFilter);
    });

    //пагинция - предыдущая страница
    $(document).on('click', '#pager-prev', function() {
        $('#pager-curr a').text(parseInt($('#pager-curr a').text()) - 1);
        getItems(currentFilter);
    });

    // поиск по новостям
    $(document).on('keypress', '#searcher', function(e) {
        if (e.keyCode == 13) {
            getItems(currentFilter);
        }
    });

    // поиск
    $(document).on('click', '#loop-searcher, #loop-searcher img', function() {
        getItems(currentFilter);
    });

    // просмотр страницы новости
    $(document).on('click', '.details-news-admin', function() {
        var id = $(this.parentNode.parentNode).attr('data-id');
        window.location.href = "/Home/News?newsId=" + id;
    });

    // объявить новость спамом
    $(document).on('click', '.spam-news-admin', function () {
        var id = parseInt($(this.parentNode.parentNode).attr('data-id'));
        $.ajax({
            url: "/Admin/SpamNews",
            type: "POST",
            data: { newsId: id },
            success: function (data) {
                if (data == "True") {
                    $('.modal').modal('hide');
                    bootbox.alert("Новость помечена как спам!", function() {
                        getItems(currentFilter);
                    });
                } else {
                    $('.modal').modal('hide');
                    bootbox.alert("<p>Ошибка!</p>Возможно новость уже помечена как спам!", function () {
                        getItems(currentFilter);
                    });
                }
            },
            dataType: "json",
            async: false
        });
    });
    
    // подтвердить новость
    $(document).on('click', '.confirm-news-admin', function () {
        var id = parseInt($(this.parentNode.parentNode).attr('data-id'));
        $.ajax({
            url: "/Admin/ConfirmNews",
            type: "POST",
            data: { newsId: id },
            success: function (data) {
                if (data == "True") {
                    $('.modal').modal('hide');
                    bootbox.alert("Новость подтверждена!", function () {
                        getItems(currentFilter);
                    });
                } else {
                    $('.modal').modal('hide');
                    bootbox.alert("<p>Ошибка!</p>Возможно новость уже помечена как спам или же уже подтверждена!", function () {
                        getItems(currentFilter);
                    });
                }
            },
            dataType: "json",
            async: false
        });
    });

});

// загрузка новостей на страницу администрирования
function getItems(filter) {
    var patern = $('#searcher').val();
    if (parseInt($('#pager-curr a').text()) < 1) $('#pager-curr a').text(1);
    var page = parseInt($('#pager-curr a').text());

    var container = $('#news-admin tbody');
    container.empty();
    
    var template = $("#template-news-item-admin");

    $.ajax({
        url: "/Admin/GetNews",
        type: "POST",
        data: { searchPattern: patern, page: page, filter: filter },
        success: function (data) {
            if (data.News.length > 0) {
                data.News.forEach(function(item) {
                    container.append(_.template(template.text(), { item: item }));
                });
            } else {
                $('#pager-curr a').text(parseInt($('#pager-curr a').text()) - 1);
                if (parseInt($('#pager-curr a').text()) < 1) $('#pager-curr a').text(1);
            }
        },
        dataType: "json",
        async: false
    });
}

//фильтр
var filter = {
    all: 0,
    unConfirm: 1,
    banned: 2
};