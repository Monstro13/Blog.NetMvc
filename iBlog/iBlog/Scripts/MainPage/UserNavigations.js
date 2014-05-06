$(document).ready(function () {

    //выйти с сайта
    $(".exit").click(function (e) {
        e.preventDefault();

        $.ajax({
            url: "/Apis/Exit",
            type: "POST",
            success: function (data) {
                if (data == "Ok") {
                    window.location.reload();
                } else {
                }
            },
            dataType: "json",
            async: false
        });
    });

    //перейти в личный кабинет
    $('.privat').click(function (e) {
        e.preventDefault();

        window.location.href = '/Manage/Privat';
    });

    //вернуться на главную страницу
    $('.go-home').click(function (e) {
        e.preventDefault();

        window.location.href = '/Home/Index';
    });

    //перейти на страницу администрирования новостей
    $(document).on('click', '.admin-show-news', function(e) {
        e.preventDefault();

        window.location.href = '/Admin/News';
    });

    //перейти на страницу администрирования пользователей 
    $(document).on('click', '.admin-show-users', function(e) {
        e.preventDefault();

        window.location.href = '/Admin/Users';
    });

    //подгрузка непрочитанных сообщений 
    $(function () {
        $.ajax({
            url: "/Apis/UnWatchedMessages",
            type: "POST",
            success: function (data) {
                if (parseInt(data) > 0) {
                    $('.unwatched-messages').css('display', 'block');
                    $('.unwatched-messages .count-messages').empty().text(data);
                }
            },
            dataType: "json",
            async: false
        });
    });

    //прокрутка страницы
    $("#up").click(function () {
        //Необходимо прокрутить в начало страницы
        var curPos = $(document).scrollTop();
        var scrollTime = curPos / 1.73;
        $("body,html").animate({ "scrollTop": 0 }, scrollTime);
    });

    $(window).scroll(function () { show_scrollTop(); }); show_scrollTop();

    //попап с диалогами
    var popupDialogs = $("#popup-dialogs");
    popupDialogs.modal({ show: false });
    $(document).on('click', '.unwatched-messages img, .unwMess', function () {
        $.ajax({
            url: "/Manage/GetAllDialogs",
            type: "POST",
            success: function (data) {
                if (data.Dialogs.length > 0) {
                    popupDialogs.find('.dialogs').empty();
                    for (var i = 0; i < data.Dialogs.length; i++) {
                        popupDialogs.find('.dialogs').append("<div class=\"item-dialog clearfix\"><div class=\"legend-dialog float-left login-autor\"><div>Диалог с пользователем: &nbsp;<a class=\"extend-info-user\" href=\"javascript:void(0)\">" + data.Dialogs[i].LoginCompanion + "</a></div><div class=\"title-post-for-dialog\">Новость -<a data-newsId=\"" + data.Dialogs[i].NewsId + "\" href=\"javascript:void(0)\">" + data.Dialogs[i].NewsTitle + "</a></div></div><div class=\"float-right show-dialog\"><a class=\"" + (!data.Dialogs[i].IsWatched ? "unwatched-dialog" : "") + "\" data-currentUserId=\"" + data.Dialogs[i].CurrentUserId + "\" data-companionId=\"" + data.Dialogs[i].CompanionId + "\" href=\"javascript:void(0)\">Показать</a></div></div>");
                        if (popupDialogs.find('.dialogs').css('height') > 500) $('body').css('overflow', 'hidden');
                        popupDialogs.modal('show');
                    }
                } else {
                    popupDialogs.find('.dialogs').empty();
                    popupDialogs.find('.dialogs').append("<div class=\"empty-list-ignor\">Список пуст</div>");
                    popupDialogs.modal('show');
                }
            },
            dataType: "json",
            async: false
        });
    });

    $('.close').click(function () {
        $('body').css('overflow', 'visible');
    });

    //переход на страницу новости
    $(document).on('click', '.title-post-for-dialog a', function () {
        var id = $(this).attr("data-newsId");
        window.location.href = "/Home/News?newsId=" + id;
    });

    //показать конкретный диалог
    $(document).on('click', '.show-dialog a', function () {
        var currentUserid = $(this).attr('data-currentUserId');
        var companionid = $(this).attr('data-companionId');
        var newsId = $('.title-post-for-dialog a').attr('data-newsId');

        $.ajax({
            url: "/Manage/GetDialog",
            type: "POST",
            data: { userId: currentUserid, companionId: companionid, newsId: newsId },
            success: function (data) {
                if (data.length > 0) {
                    window.location.href = data;
                }
            },
            dataType: "json",
            async: false
        });

    });

    //показ подробной инфы о пользователе
    var infoPopup = $("#popup-unfo-user");
    infoPopup.modal({ show: false });
    $(document).on('click', ".extend-info-user", function () {
        infoPopup.css('zIndex', '10000');
        infoPopup.modal("show");
    });

    $(document).on('click', ".extend-info-user", function () {
        var login = $(this).text();

        $.ajax({
            url: "/Apis/GetUserInfoForUser",
            type: "POST",
            data: { login: login },
            success: function (answer) {
                if (answer != "Error") {
                    $("#iduser-for-add-to-blacklist").val(answer.UserId);
                    $("#popup-unfo-user .username-userforuser").text(answer.Name);
                    $("#popup-unfo-user .usersex-userforuser").text(answer.Sex);
                    $("#popup-unfo-user .userrating-userforuser").text(answer.Rating);
                    $("#popup-unfo-user .usertimeonsite-userforuser").text(answer.TimeOnSite);
                    $("#popup-unfo-user .userposts-userforuser").text(answer.CountPost);
                    $("#popup-unfo-user .userkomments-userforuser").text(answer.CountKomment);
                }
            },
            dataType: "json",
            async: false
        });

    });

    //добавление пользователя в игнор-лист
    $("#add-to-blacklist").click(function () {
        var id = $("#iduser-for-add-to-blacklist").val();

        $.ajax({
            url: "/Apis/AddToBlackList",
            type: "POST",
            data: { userId: id },
            success: function (answer) {
                $('.modal').modal('hide');
                if (answer == "True") {
                    bootbox.alert("<p>Пользователь добавлен в черный список!</p>Изменить свое решение вы можете в своих настройках!", function () {
                        window.location.reload();
                    });
                } else {
                    bootbox.alert("<p>Произошла внутренняя ошибка!</p>Возможно пользователь уже в черном списке!");
                }
            },
            dataType: "json",
            async: false
        });
    });

    //работа с попапами настроек сайта
    var formPopup = $('#popup-form');
    formPopup.modal('hide');

    var formForContact = $('.form-for-contact').find('.form-group');
    var formForCategory = $('.form-for-category').find('.form-group');
    var formForRubric = $('.form-for-rubric').find('.form-group');


    $(document).on('click', '.form-contact', function () {
        GetFormContact(formPopup, formForContact);
    });

    $(document).on('click', '.form-category', function() {
        GetFormCategory(formPopup, formForCategory);
    });
    
    $(document).on('click', '.form-rubric', function () {
        GetFormRubric(formPopup, formForRubric);
    });
    
    //---------------Добавление поля редактирования при настройках сайта---------------

    $(document).on('click', '.item-type-contact img', function () {
        var input = this.previousSibling;
        var spanTitle = this.previousSibling.previousSibling;

        $(input).addClass('activ-edit-type-contact');
        var wid = parseInt($(spanTitle).css('width').replace("px", "")) + 15;
        $(input).css('width', wid);
        $(input).val($(spanTitle).text());
        $(input).css('display', 'block');
    });
    
    $(document).on('click', '.item-title-category img', function () {
        var input = this.previousSibling;
        var spanTitle = this.previousSibling.previousSibling;

        $(input).addClass('activ-edit-title-category');
        var wid = parseInt($(spanTitle).css('width').replace("px", "")) + 15;
        $(input).css('width', wid);
        $(input).val($(spanTitle).text());
        $(input).css('display', 'block');
    });
    
    $(document).on('click', '.item-title-rubric img', function () {
        var input = this.previousSibling;
        var spanTitle = this.previousSibling.previousSibling;

        $(input).addClass('activ-edit-title-rubric');
        var wid = parseInt($(spanTitle).css('width').replace("px", "")) + 15;
        $(input).css('width', wid);
        $(input).val($(spanTitle).text());
        $(input).css('display', 'block');
    });

    $(document).on('click', '.item-value-contact img', function () {
        var input = this.previousSibling;
        var spanTitle = this.previousSibling.previousSibling;

        $(input).addClass('activ-edit-value-contact');
        var wid = parseInt($(spanTitle).css('width').replace("px", "")) + 15;
        $(input).css('width', wid);
        $(input).val($(spanTitle).text());
        $(input).css('display', 'block');
    });
    
    $(document).on('click', '.item-code-category img', function () {
        var input = this.previousSibling;
        var spanTitle = this.previousSibling.previousSibling;

        $(input).addClass('activ-edit-code-category');
        var wid = parseInt($(spanTitle).css('width').replace("px", "")) + 15;
        $(input).css('width', wid);
        $(input).val($(spanTitle).text());
        $(input).css('display', 'block');
    });

    //--------------------------------------------------------------------


    //---------------------Установка новых значений ---------------------------
    
    $(document).on('keypress', '.activ-edit-type-contact', function (e) {
        if (e.keyCode == 13) {
            var spanTitle = this.previousSibling;
            var id = $(this.parentNode.parentNode.parentNode).find('input').val();
            var obj = this;

            $.ajax({
                url: "/Admin/SetTypeContact",
                type: "POST",
                data: { contactId: id, text: $(obj).val() },
                success: function (answer) {
                    if (answer == "True") {
                        $(spanTitle).text(obj.value);
                    }
                    else {
                        $('.modal').modal('hide');
                        bootbox.alert("Произошла ошибка! Возможно данный тип уже существует!", function () {
                            GetFormContact(formPopup, formForContact);
                        });
                    }
                },
                dataType: "json",
                async: false
            });

            $(this).css('display', 'none');
        }
    });
    
    $(document).on('keypress', '.activ-edit-title-category', function (e) {
        if (e.keyCode == 13) {
            var spanTitle = this.previousSibling;
            var id = $(this.parentNode.parentNode.parentNode).find('input').val();
            var obj = this;

            $.ajax({
                url: "/Admin/SetTitleCategory",
                type: "POST",
                data: { categoryId: id, text: $(obj).val() },
                success: function (answer) {
                    if (answer == "True") {
                        $(spanTitle).text(obj.value);
                    }
                    else {
                        $('.modal').modal('hide');
                        bootbox.alert("Произошла ошибка! Возможно данная категория уже существует!", function () {
                            GetFormCategory(formPopup, formForCategory);
                        });
                    }
                },
                dataType: "json",
                async: false
            });

            $(this).css('display', 'none');
        }
    });
    
    $(document).on('keypress', '.activ-edit-title-rubric', function (e) {
        if (e.keyCode == 13) {
            var spanTitle = this.previousSibling;
            var id = $(this.parentNode.parentNode.parentNode).find('input').val();
            var obj = this;

            $.ajax({
                url: "/Admin/SetTitleRubric",
                type: "POST",
                data: { rubricId: id, text: $(obj).val() },
                success: function (answer) {
                    if (answer == "True") {
                        $(spanTitle).text(obj.value);
                    }
                    else {
                        $('.modal').modal('hide');
                        bootbox.alert("Произошла ошибка! Возможно данная рубрика уже существует!", function () {
                            GetFormRubric(formPopup, formForRubric);
                        });
                    }
                },
                dataType: "json",
                async: false
            });

            $(this).css('display', 'none');
        }
    });

    $(document).on('keypress', '.activ-edit-value-contact', function (e) {
        if (e.keyCode == 13) {
            var spanTitle = this.previousSibling;
            var id = $(this.parentNode.parentNode.parentNode).find('input').val();
            var obj = this;

            $.ajax({
                url: "/Admin/SetValueContact",
                type: "POST",
                data: { contactId: id, text: $(obj).val() },
                success: function (answer) {
                    if (answer == "True") {
                        $(spanTitle).text(obj.value);
                    }
                },
                dataType: "json",
                async: false
            });

            $(this).css('display', 'none');
        }
    });
    
    $(document).on('keypress', '.activ-edit-code-category', function (e) {
        if (e.keyCode == 13) {
            var spanTitle = this.previousSibling;
            var id = $(this.parentNode.parentNode.parentNode).find('input').val();
            var obj = this;

            $.ajax({
                url: "/Admin/SetCodeCategory",
                type: "POST",
                data: { categoryId: id, text: $(obj).val() },
                success: function (answer) {
                    if (answer == "True") {
                        $(spanTitle).text(obj.value);
                    }
                },
                dataType: "json",
                async: false
            });

            $(this).css('display', 'none');
        }
    });

    //------------------------------------------------------------------------------


    //-----------------Добавление удаление сущностей --------------------------- 
    
    $(document).on('click', '.delete-contact-admin img', function () {
        var id = $(this.parentNode.parentNode.parentNode).find('input').val();
        $.ajax({
            url: "/Admin/DeleteContact",
            type: "POST",
            data: { contactId: id },
            success: function (data) {
                if (data == "True") {
                    $('.modal').modal('hide');
                    bootbox.alert("Контакт удален!", function () {
                        GetFormContact(formPopup, formForContact);
                    });
                }
            },
            dataType: "json",
            async: false
        });
    });
    
    $(document).on('click', '.delete-category-admin img', function () {
        var id = $(this.parentNode.parentNode.parentNode).find('input').val();
        $.ajax({
            url: "/Admin/DeleteCategory",
            type: "POST",
            data: { categoryId: id },
            success: function (data) {
                if (data == "True") {
                    $('.modal').modal('hide');
                    bootbox.alert("Категория удалена!", function() {
                        GetFormCategory(formPopup, formForCategory);
                    });
                } else {
                    $('.modal').modal('hide');
                    bootbox.alert("Невозможно удалить категорию!", function () {
                        GetFormCategory(formPopup, formForCategory);
                    });
                }
            },
            dataType: "json",
            async: false
        });
    });
    
    $(document).on('click', '.delete-rubric-admin img', function () {
        var id = $(this.parentNode.parentNode.parentNode).find('input').val();
        $.ajax({
            url: "/Admin/DeleteRubric",
            type: "POST",
            data: { rubricId: id },
            success: function (data) {
                if (data == "True") {
                    $('.modal').modal('hide');
                    bootbox.alert("Рубрика удалена!", function () {
                        GetFormRubric(formPopup, formForRubric);
                    });
                } else {
                    $('.modal').modal('hide');
                    bootbox.alert("Невозможно удалить рубрику!", function () {
                        GetFormRubric(formPopup, formForRubric);
                    });
                }
            },
            dataType: "json",
            async: false
        });
    });

    $(document).on('click', '.add-contact-admin img', function () {
        var type = $("#type-contact").val();
        var value = $("#value-contact").val();

        if (baseValid(type) && baseValid(value)) {
            $.ajax({
                url: "/Admin/AddContact",
                type: "POST",
                data: { type: type, value: value },
                success: function (data) {
                    if (data == "True") {
                        $('.modal').modal('hide');
                        bootbox.alert("Контакт добавлен!", function () {
                            GetFormContact(formPopup, formForContact);
                        });
                    } else {
                        $('.modal').modal('hide');
                        bootbox.alert("<p>Произошла ошибка!</p>Возможно данный контакт уже существует! Попробуйте изменить тип!", function () {
                            GetFormContact(formPopup, formForContact);
                        });
                    }
                },
                dataType: "json",
                async: false
            });
        } else {
            $('.modal').modal('hide');
            bootbox.alert("Значения не валидны!", function () {
                GetFormContact(formPopup, formForContact);
            });
        }
    });

    $(document).on('click', '.add-category-admin img', function () {
        var title = $("#title-category").val();
        var code = $("#code-category").val();
        var desc = $('.description-category textarea').text() == "Это группа для ... " ? " " : $('.description-category textarea').text();

        if (baseValid(title) && baseValid(code)) {
            $.ajax({
                url: "/Admin/AddCategory",
                type: "POST",
                data: { title: title, code: code, desc: desc },
                success: function (data) {
                    if (data == "True") {
                        $('.modal').modal('hide');
                        bootbox.alert("Категория добавлена!", function () {
                            GetFormCategory(formPopup, formForCategory);
                        });
                    } else {
                        $('.modal').modal('hide');
                        bootbox.alert("<p>Произошла ошибка!</p>Возможно данная категория уже существует! Попробуйте изменить!", function () {
                            GetFormCategory(formPopup, formForCategory);
                        });
                    }
                },
                dataType: "json",
                async: false
            });
        } else {
            $('.modal').modal('hide');
            bootbox.alert("Значения не валидны!", function () {
                GetFormCategory(formPopup, formForCategory);
            });
        }
    });
    
    $(document).on('click', '.add-rubric-admin img', function () {
        var title = $("#title-rubric").val();
        var desc = $('.description-rubric textarea').text() == "Это рубрика значит ... " ? " " : $('.description-category textarea').text();

        if (baseValid(title)) {
            $.ajax({
                url: "/Admin/AddRubric",
                type: "POST",
                data: { title: title, desc: desc },
                success: function (data) {
                    if (data == "True") {
                        $('.modal').modal('hide');
                        bootbox.alert("Рубрика добавлена!", function () {
                            GetFormRubric(formPopup, formForRubric);
                        });
                    } else {
                        $('.modal').modal('hide');
                        bootbox.alert("<p>Произошла ошибка!</p>Возможно данная рубрика уже существует! Попробуйте изменить!", function () {
                            GetFormRubric(formPopup, formForRubric);
                        });
                    }
                },
                dataType: "json",
                async: false
            });
        } else {
            $('.modal').modal('hide');
            bootbox.alert("Значения не валидны!", function () {
                GetFormRubric(formPopup, formForRubric);
            });
        }
    });
    
    // ----------------------------------------------------------------------
    setInterval(equalsKomment, 1000);

});

//базовая валидация (буквы и цифры)
function baseValid(value) {
    if (value.length > 0) {
        var regexp = /^[а-яА-ЯёЁa-zA-Z0-9]+$/;
        if (regexp.test(value))
            return true;
    }
    return false;
}

function show_scrollTop() {
    var e = $("#up");
    ($(window).scrollTop() > 300) ? e.fadeIn(600) : e.fadeOut(500);
}

//раздвоение комментов на диалог
function equalsKomment() {
    var wrap = $('.komment-wrapper');

    wrap.each(function (index, item) {
        var login = $(item).find(' .extend-info-user').text();

        if ($('.token-identity').length > 0) {
            if ($('.user-hi span').text().indexOf(login) !== -1) {
                $(item).addClass('current-user');
            } else {
                $(item).addClass('companion-user');
            }
        }
    });
}

//-----------------Подгрузка формы настройки --------------------------------------------

function GetFormContact(formPopup, formForContact) {
    var form = formForContact;

    $.ajax({
        url: "/Admin/GetAllContacts",
        type: "POST",
        success: function (answer) {
            $('.modal').modal('hide');
            if (answer.Contacts.length > 0) {
                form.find('.all-contacs').empty();
                for (var i = 0; i < answer.Contacts.length; i++) {
                    form.find('.all-contacs').append("<div class=\"item-contact-admin\"><input class=\"id-for-contact\" type=\"hidden\" value=\"" + answer.Contacts[i].ContactId + "\" /><div class=\"wrap-item-contact clearfix pos-rel\"><div class=\"item-type-contact\" style=\"display: inline\"><span>" + answer.Contacts[i].Type + "</span><input type=\"text\" class=\"pos-abs\" style=\"display: none\"/><img width=\"20\" src=\"/Images/OptionPost/edit.png\"/></div><div class=\"item-value-contact pos-rel\" style=\"display: inline\"><span>" + answer.Contacts[i].Value + "</span><input type=\"text\" class=\"pos-abs\" style=\"display: none\"/><img width=\"20\" src=\"/Images/OptionPost/edit.png\"/></div><div class=\"delete-contact-admin float-right\" style=\"display: inline\"><img width=\"20\" src=\"/Images/OptionPost/delete.png\"/></div></div></div>");
                }
            }
        },
        dataType: "json",
        async: false
    });

    formPopup.find('.modal-title').empty().append("Настройки контактов сайта");
    formPopup.find('.modal-body').empty().append(form);
    formPopup.modal('show');
}

function GetFormCategory(formPopup, formForCategory) {
    var form = formForCategory;

    $.ajax({
        url: "/Admin/GetAllCategories",
        type: "POST",
        success: function (answer) {
            $('.modal').modal('hide');
            if (answer.Categories.length > 0) {
                form.find('.all-categories').empty();
                for (var i = 0; i < answer.Categories.length; i++) {
                    form.find('.all-categories').append("<div class=\"item-category-admin\"><input class=\"id-for-category\" type=\"hidden\" value=\"" + answer.Categories[i].Id + "\" /><div class=\"wrap-item-category clearfix pos-rel\"><div class=\"item-title-category\" style=\"display: inline\"><span>" + answer.Categories[i].Title + "</span><input type=\"text\" class=\"pos-abs\" style=\"display: none\"/><img width=\"20\" src=\"/Images/OptionPost/edit.png\"/></div><div class=\"item-code-category pos-rel\" style=\"display: inline\"><span>" + answer.Categories[i].Code + "</span><input type=\"text\" class=\"pos-abs\" style=\"display: none\"/><img width=\"20\" src=\"/Images/OptionPost/edit.png\"/></div><div class=\"delete-category-admin float-right\" style=\"display: inline\"><img width=\"20\" src=\"/Images/OptionPost/delete.png\"/></div></div></div>");
                }
            }
        },
        dataType: "json",
        async: false
    });

    formPopup.find('.modal-title').empty().append("Настройки категорий пользователей");
    formPopup.find('.modal-body').empty().append(form);
    formPopup.modal('show');
}

function GetFormRubric(formPopup, formForRubric) {
    var form = formForRubric;

    $.ajax({
        url: "/Admin/GetAllRubrics",
        type: "POST",
        success: function (answer) {
            $('.modal').modal('hide');
            if (answer.Rubrics.length > 0) {
                form.find('.all-rubrics').empty();
                for (var i = 0; i < answer.Rubrics.length; i++) {
                    form.find('.all-rubrics').append("<div class=\"item-rubric-admin\"><input class=\"id-for-rubric\" type=\"hidden\" value=\"" + answer.Rubrics[i].Id + "\" /><div class=\"wrap-item-rubric clearfix pos-rel\"><div class=\"item-title-rubric\" style=\"display: inline\"><span>" + answer.Rubrics[i].Rubric + "</span><input type=\"text\" class=\"pos-abs\" style=\"display: none\"/><img width=\"20\" src=\"/Images/OptionPost/edit.png\"/></div><div class=\"delete-rubric-admin float-right\" style=\"display: inline\"><img width=\"20\" src=\"/Images/OptionPost/delete.png\"/></div></div></div>");
                }
            }
        },
        dataType: "json",
        async: false
    });

    formPopup.find('.modal-title').empty().append("Настройки рубрик");
    formPopup.find('.modal-body').empty().append(form);
    formPopup.modal('show');
}

//----------------------------------------------------------------------------