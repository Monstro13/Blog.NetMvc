$(document).ready(function () {
    // работа с игнор-листом
    var popupIgnors = $("#popup-ignors");
    popupIgnors.modal({ show: false });
    $(document).on('click', '.ignorBtn', function () {
        $.ajax({
            url: "/Manage/GetIgnorList",
            type: "POST",
            success: function (data) {
                if (data.Ignors.length > 0) {
                    popupIgnors.find('.ignors').empty();
                    for (var i = 0; i < data.Ignors.length; i++) {
                        popupIgnors.find('.ignors').append("<div class=\"item-ignor clearfix\"><div class=\"legend-ignor float-left login-autor\"><div>Пользователь: &nbsp;<a class=\"extend-info-user\" href=\"javascript:void(0)\">" + data.Ignors[i].Login + "</a></div></div><div class=\"float-right delete-from-ignor\"><a class=\"\" data-currentUserId=\"" + data.Ignors[i].CurrentUserId + "\" data-ignorId=\"" + data.Ignors[i].IgnorId + "\" href=\"javascript:void(0)\">Удалить из списка</a></div></div>");
                        if (popupIgnors.find('.ignors').css('height') > 500) $('body').css('overflow', 'hidden');
                        popupIgnors.modal('show');
                    }
                } else {
                    popupIgnors.find('.ignors').empty();
                    popupIgnors.find('.ignors').append("<div class=\"empty-list-ignor\">Список пуст</div>");
                    popupIgnors.modal('show');
                }
            },
            dataType: "json",
            async: false
        });
    });

    //удаление из игнор-листа
    $(document).on('click', '.delete-from-ignor a', function () {
        var user = $(this).attr("data-currentUserId");
        var ignor = $(this).attr("data-ignorId");

        $.ajax({
            url: "/Manage/DeleteFromIgnor",
            type: "POST",
            data: { userId: user, ignorUserId: ignor },
            success: function (dat) {
                if (dat == "True") {
                    $.ajax({
                        url: "/Manage/GetIgnorList",
                        type: "POST",
                        success: function (data) {
                            if (data.Ignors.length > 0) {
                                popupIgnors.find('.ignors').empty();
                                for (var i = 0; i < data.Ignors.length; i++) {
                                    popupIgnors.find('.ignors').append("<div class=\"item-ignor clearfix\"><div class=\"legend-ignor float-left login-autor\"><div>Пользователь: &nbsp;<a class=\"extend-info-user\" href=\"javascript:void(0)\">" + data.Ignors[i].Login + "</a></div></div><div class=\"float-right delete-from-ignor\"><a class=\"\" data-currentUserId=\"" + data.Ignors[i].CurrentUserId + "\" data-ignorId=\"" + data.Ignors[i].IgnorId + "\" href=\"javascript:void(0)\">Удалить из списка</a></div></div>");
                                    if (popupIgnors.find('.ignors').css('height') > 500) $('body').css('overflow', 'hidden');
                                    popupIgnors.modal('show');
                                }
                            } else {
                                popupIgnors.find('.ignors').empty();
                                popupIgnors.find('.ignors').append("<div class=\"empty-list-ignor\">Список пуст</div>");
                                popupIgnors.modal('show');
                            }
                        },
                        dataType: "json",
                        async: false
                    });
                } else {
                    $('.modal').modal('hide');
                    bootbox.alert("<p>Произошла внутренняя ошибка!</p>Попробуйте повторить позже!");
                }
            },
            dataType: "json",
            async: false
        });

    });

    //попап настроек
    var popupOptions = $("#popup-option");
    popupOptions.modal({ show: false });

    $(document).on('click', '.optionBtn', function () {
        popupOptions.modal('show');
    });

    //попап изменения пароля
    var changePassword = $('#popup-newPassword');
    changePassword.modal({ show: false });

    $(document).on('click', '.new-password a', function () {
        $('.modal').modal('hide');
        changePassword.modal('show');
    });

    //манипуляции с данными на попапе изменения пароля
    $(".change-password-field").focusout(function () {
        var redShadow = "3px 3px 0px #ff9999";
        var greenShadow = "3px 3px 0px #99ff99";

        if (this.name == "oldPassword") {
            var old = this.value;
            var obj = this;
            if (baseValid(old)) {
                $.ajax({
                    url: "/Manage/ControlOldPass",
                    type: "POST",
                    data: { old: old },
                    success: function (data) {
                        if (data == "True") {
                            obj.style.boxShadow = greenShadow;
                        } else {
                            this.style.boxShadow = redShadow;
                            SetError(obj, "Неверный пароль!");
                        }
                    },
                    dataType: "json",
                    async: false
                });
            } else {
                this.style.boxShadow = redShadow;
                SetError(this, "Поле не заполнено или используются недопустимые символы!");
            }
        } else if (this.name == "confirmPassword") {
            var newConfirmPs = this.value;
            var newPs = $("#new-password").val();
            if (baseValid(newConfirmPs)) {
                if (newConfirmPs == newPs) {
                    this.style.boxShadow = greenShadow;
                } else {
                    this.style.boxShadow = redShadow;
                    SetError(this, "Пароли не совпадают!");
                }
            } else {
                this.style.boxShadow = redShadow;
                SetError(this, "Поле не заполнено или используются недопустимые символы!");
            }
        } else if (!baseValid(this.value)) {
            this.style.boxShadow = redShadow;
            SetError(this, "Поле не заполнено или используются недопустимые символы!");
        } else this.style.boxShadow = greenShadow;
    });

    //сохранение нового пароля
    $(document).on('click', '#save-new-password', function () {
        var oldPassword = $("#old-password").val();
        var newPassword = $("#new-password").val();
        var confirmPassword = $("#new-password-confirm").val();

        if (baseValid(oldPassword)) {
            if (baseValid(newPassword)) {
                if (baseValid(confirmPassword)) {
                    if (newPassword == confirmPassword) {
                        $.ajax({
                            url: "/Manage/SetNewPassword",
                            type: "POST",
                            data: { old: oldPassword, newPs: newPassword, confirmPs: confirmPassword },
                            success: function (data) {
                                $('.modal').modal('hide');
                                if (data == "Ok") {
                                    bootbox.alert("Пароль успешно изменен!");
                                }
                                else if (data == "oldIsBad") {
                                    bootbox.alert("Старый пароль указан неверно!");
                                } else if (data == "newAndConfirmIsNotEquals") {
                                    bootbox.alert("Пароли не совпали!");
                                } else {
                                    bootbox.alert("<p>Внутренняя ошибка!</p>Попробуйте позже!");
                                }
                            },
                            dataType: "json",
                            async: false
                        });
                    } else {
                        SetError(document.getElementById("new-password-confirm"), "Пароли не совпадают!");
                    }
                } else {
                    SetError(document.getElementById("new-password-confirm"), "Поле не заполнено или используются недопустимые символы!");
                }
            } else {
                SetError(document.getElementById("new-password"), "Поле не заполнено или используются недопустимые символы!");
            }
        } else {
            SetError(document.getElementById("old-password"), "Поле не заполнено или используются недопустимые символы!");
        }

    });

    //попап смены категории пользователя
    var popupCat = $("#popup-userCategory");
    popupCat.modal('hide');

    $(document).on('click', '.user-category a', function () {
        $('.modal').modal('hide');
        
        $.ajax({
            url: "/Manage/GetAllCategories",
            type: "POST",
            success: function (data) {
                $('#popup-userCategory .modal-body .categories-list').empty();
                if (data.Categories.length > 0) {
                    for (var i = 0; i < data.Categories.length; i++) {
                        $('#popup-userCategory .modal-body .categories-list').append("<li><a href=\"javascript:void(0)\" class=\"item-category\" data-catId=\"" + data.Categories[i].Id + "\">" + data.Categories[i].Title + "</a></li>");
                    }
                } else {
                    $('#popup-userCategory .modal-body .categories-list').append("<a href=\"javascript:void(0)\">Список пуст</a>");
                }
            },
            dataType: "json",
            async: false
        });

        popupCat.modal('show');
    });

    //показ формы ввода секретного кода категории
    $(document).on('click', '.item-category', function() {
        var id = $(this).attr('data-catid');
        if (parseInt(id) != 1)
            $('.category-info').css('display', 'block');
        else {
            $('.category-info').css('display', 'none');
        }

        $("#idCategory").val(id);
        $("#popup-userCategory .modal-footer a").css('display', 'block').empty().append("Сохранить: " + $(this).text());
    });

    //сохранение категории после смены
    $(document).on('click', '.save-category', function () {
        $(".modal").modal('hide');
        var id = $("#idCategory").val();
        var error = false;
        var code = 1;

        if (parseInt(id) < 0) {
            $('.modal').modal('hide');
            bootbox.alert("Вы не выбрали категорию!");
            error = true;
        }
        else if (parseInt(id) > 1) {
            code = $("#code-category").val();
            if (!baseValid(code)) {
                bootbox.alert("Код не валиден!");
                error = true;
            }
        }
        
        if (!error) {
            $.ajax({
                url: "/Manage/ChangeCategory",
                type: "POST",
                data: { catId: id, code: code },
                success: function (data) {
                    $('.modal').modal('hide');
                    if (data == "True") {
                        bootbox.alert("Категория успешно изменена!");
                    } else {
                        bootbox.alert("Произошла ошибка! Проверьте данные и повторите снова!");
                    }
                },
                dataType: "json",
                async: false
            });
        }

    });
});