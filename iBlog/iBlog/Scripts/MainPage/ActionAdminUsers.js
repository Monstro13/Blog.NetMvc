$(document).ready(function () {
    var currentFilter = filter.all;
    getItems(currentFilter);

    $(document).on('click', '#all-users-admin', function () {
        $('#pager-curr a').text(1);
        currentFilter = filter.all;
        getItems(currentFilter);
    });

    $(document).on('click', '#unConfirm-users-admin', function () {
        $('#pager-curr a').text(1);
        currentFilter = filter.unConfirm;
        getItems(currentFilter);
    });

    $(document).on('click', '#banned-users-admin', function () {
        $('#pager-curr a').text(1);
        currentFilter = filter.banned;
        getItems(currentFilter);
    });

    $(document).on('click', '#pager-next', function () {
        $('#pager-curr a').text(parseInt($('#pager-curr a').text()) + 1);
        getItems(currentFilter);
    });

    $(document).on('click', '#pager-prev', function () {
        $('#pager-curr a').text(parseInt($('#pager-curr a').text()) - 1);
        getItems(currentFilter);
    });

    $(document).on('keypress', '#searcher', function (e) {
        if (e.keyCode == 13) {
            getItems(currentFilter);
        }
    });

    $(document).on('click', '#loop-searcher, #loop-searcher img', function () {
        getItems(currentFilter);
    });
    
    // установить права админа пользователю
    $(document).on('click', '.set-admin-user', function () {
        var id = parseInt($(this.parentNode.parentNode).attr('data-id'));
        $.ajax({
            url: "/Admin/SetAdmin",
            type: "POST",
            data: { usersId: id },
            success: function (data) {
                if (data == "True") {
                    $('.modal').modal('hide');
                    bootbox.alert("Пользователь получил права Администратора!", function () {
                        getItems(currentFilter);
                    });
                } else {
                    $('.modal').modal('hide');
                    bootbox.alert("<p>Ошибка!</p>Возможно пользователь заблокирован или уже Адмиистратор!", function () {
                        getItems(currentFilter);
                    });
                }
            },
            dataType: "json",
            async: false
        });
    });
    
    //забрать права админа у пользователя
    $(document).on('click', '.get-admin-user', function () {
        var id = parseInt($(this.parentNode.parentNode).attr('data-id'));
        $.ajax({
            url: "/Admin/DeleteAdminRules",
            type: "POST",
            data: { usersId: id },
            success: function (data) {
                if (data == "True") {
                    $('.modal').modal('hide');
                    bootbox.alert("Пользователь потерял права Администратора!", function () {
                        getItems(currentFilter);
                    });
                } else {
                    $('.modal').modal('hide');
                    bootbox.alert("<p>Ошибка!</p>Возможно пользователь последний из адвинистраторов, или же это вы!", function () {
                        getItems(currentFilter);
                    });
                }
            },
            dataType: "json",
            async: false
        });
    });

    // заблокировть пользователя
    $(document).on('click', '.set-block-user', function () {
        var id = parseInt($(this.parentNode.parentNode).attr('data-id'));
        $.ajax({
            url: "/Admin/BlockUser",
            type: "POST",
            data: { usersId: id },
            success: function (data) {
                if (data == "True") {
                    $('.modal').modal('hide');
                    bootbox.alert("Пользователь заблокирован!", function () {
                        getItems(currentFilter);
                    });
                } else {
                    $('.modal').modal('hide');
                    bootbox.alert("<p>Ошибка!</p>Возможно пользователь уже заблокирован или же его аккаунт не активирован!", function () {
                        getItems(currentFilter);
                    });
                }
            },
            dataType: "json",
            async: false
        });
    });
    
    // разблокировать пользователя
    $(document).on('click', '.get-block-user', function () {
        var id = parseInt($(this.parentNode.parentNode).attr('data-id'));
        $.ajax({
            url: "/Admin/UnBlockUser",
            type: "POST",
            data: { usersId: id },
            success: function (data) {
                if (data == "True") {
                    $('.modal').modal('hide');
                    bootbox.alert("Пользователь разблокирован!", function () {
                        getItems(currentFilter);
                    });
                } else {
                    $('.modal').modal('hide');
                    bootbox.alert("<p>Ошибка!</p>Повторите позже!", function () {
                        getItems(currentFilter);
                    });
                }
            },
            dataType: "json",
            async: false
        });
    });

});

function getItems(filter) {
    var patern = $('#searcher').val();
    if (parseInt($('#pager-curr a').text()) < 1) $('#pager-curr a').text(1);
    var page = parseInt($('#pager-curr a').text());

    var container = $('#users-admin tbody');
    container.empty();

    var template = $("#template-users-item-admin");

    $.ajax({
        url: "/Admin/GetUsers",
        type: "POST",
        data: { searchPattern: patern, page: page, filter: filter },
        success: function (data) {
            if (data.Users.length > 0) {
                data.Users.forEach(function (item) {
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

var filter = {
    all: 0,
    unConfirm: 1,
    banned: 2
};