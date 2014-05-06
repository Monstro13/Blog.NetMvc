$(document).ready(function () {

    $('#add-to-news').click(function() {
        window.location.href = "/Home/AddNews";
    });

    var scrollStart = 900; // Начало расширения страницы начинается, когда прокрутка доходит вниз до 900 пикселов
    $(window).scroll(function () {

        var scroll = $(window).scrollTop();
        $('body').html();

        if (scroll > scrollStart) {
            $('#right-body').css('width', '800'); // задаем нужную ширину элементу при прокрутке больше 900 пикс
            $('#right-body').css('marginRight', '85px');
        } else {
            $('#right-body').css('width', '680'); // задаем ширину, которая должна быть при прокрутке менее 900 пикс
            $('#right-body').css('marginRight', '0px');
        }
    });

    LoadTags();
    LoadContacts();

    //попап для регистрации
    var popup = $("#popup-register");
    popup.modal({ show: false });
    $("#Btn-register").click(function () {
        //$("body").css("overflow", "hidden");
        popup.modal("show");
    });

    //попап для активации аккаунта
    var actPopup = $("#popup-activate");
    actPopup.modal({ show: false });
    $("#activate-accaunt-go").click(function () {
        //$("body").css("overflow", "hidden");
        actPopup.modal("show");
    });

    //попап для восстановления пароля
    var recPopup = $("#popup-recovery");
    recPopup.modal({ show: false });
    $("#go-recovery").click(function () {
        //$("body").css("overflow", "hidden");
        recPopup.modal("show");
    });

    //$(".close").click(function() {
    //    $("body").css("overflow", "visible");
    //});

    //смена пола
    $("#man").click(function (e) {
        e.preventDefault();
        $("#sex-field").val(1);
        $("#register-sex").val("Мужчина");
    });

    //смена пола
    $("#woman").click(function (e) {
        e.preventDefault();
        $("#sex-field").val(0);
        $("#register-sex").val("Женщина");
    });

    //взаимоисключение полов
    $("#register-sex").click(function (e) {
        if ($("#sex-field").val() == 0) {
            $("#sex-field").val(1);
            $("#register-sex").val("Мужчина");
        } else if ($("#sex-field").val() == 1) {
            $("#sex-field").val(0);
            $("#register-sex").val("Женщина");
        }
    });

    //вход на сайт
    $("#Btn-sign-in").click(function () {
        var login = $("#loginput").val();
        var password = $("#passinput").val();
        var remember = $("#login-rememberme").attr("checked") == true ? "true" : "false";

        var redShadow = "3px 3px 0px #ff9999";
        var greenShadow = "3px 3px 0px #99ff99";

        if (baseValid(login) && baseValid(password)) {
            $.ajax({
                url: "/Apis/SignIn",
                type: "POST",
                data: { login: login, password: password, remember: remember },
                success: function (data) {
                    if (data == "Ok") {
                        window.location.reload();
                    } else {
                        $("#loginput")[0].style.boxShadow = redShadow;
                        $("#passinput")[0].style.boxShadow = redShadow;
                        SetError($("#loginput")[0], "Данная комбинация логина и пароля не найдена!");
                    }
                },
                dataType: "json",
                async: false
            });
        } else {
            $("#loginput")[0].style.boxShadow = redShadow;
            $("#passinput")[0].style.boxShadow = redShadow;
            SetError($("#loginput")[0], "Одно из полей не заполнено, либо используются запрещенные символы!");
        }

    });

    //скрывание подсказок при входе
    $("#loginput, #passinput").focus(function () {
        $("#loginput")[0].previousElementSibling.style.display = "none";
    });

    //выделение текущей подсказки
    $(".popup").mouseover(function () {
        var group = $(".popup");
        group.each(function (index, item) {
            item.style.zIndex = 10;
        });

        this.style.zIndex = 1000;
    });

    //скрытие подсказки при изменении входных данных
    $(".field, .activate-field, .recovery-field, .change-password-field").focus(function () {
        this.previousElementSibling.style.display = "none";
    });

    //скрытие подсказки при смене пола
    $("#register-sex, #register-sex-field").focus(function () {
        $("#sex-field")[0].previousElementSibling.style.display = "none";
    });

    //проверка полей регистрации при потере фокуса
    $(".field").focusout(function () {
        var redShadow = "3px 3px 0px #ff9999";
        var greenShadow = "3px 3px 0px #99ff99";

        if (this.name == "Email") {
            var email = this.value;
            var obj = this;
            if (isValidEmail(email)) {
                $.ajax({
                    url: "/Apis/IsEmailExists",
                    type: "POST",
                    data: { mail: email },
                    success: function (data) {
                        if (data == "True") {
                            obj.style.boxShadow = redShadow;
                            SetError(obj, "Данный адрес электронной почты занят!");
                        } else
                            obj.style.boxShadow = greenShadow;
                    },
                    dataType: "json",
                    async: false
                });
            } else {
                this.style.boxShadow = redShadow;
                SetError(this, "Неккоректный адрес электронной почты!");
            }
        } else if (this.name == "Login") {
            var login = this.value;
            var obj = this;
            if (baseValid(login)) {
                $.ajax({
                    url: "/Apis/IsLoginExists",
                    type: "POST",
                    data: { login: login },
                    success: function (data) {
                        if (data == "True") {
                            obj.style.boxShadow = redShadow;
                            SetError(obj, "Логин уже занят!");
                        } else
                            obj.style.boxShadow = greenShadow;
                    },
                    dataType: "json",
                    async: false
                });
            } else {
                this.style.boxShadow = redShadow;
                SetError(this, "Недопустимое значение логина!");
            }
        } else if (!baseValid(this.value)) {
            this.style.boxShadow = redShadow;
            SetError(this, "Поле не заполнено или используются недопустимые символы!");
        } else this.style.boxShadow = greenShadow;
    });

    //регистрация пользователя с проверкой данных
    $("#popup-register-go").click(function () {
        var fields = $(".field");

        var isvalid = true;
        var redShadow = "3px 3px 0px #ff9999";
        var greenShadow = "3px 3px 0px #99ff99";

        for (var i = 0; i < fields.length; i++) {
            if (fields[i].name == "Email") {
                var email = fields[i].value;
                if (isValidEmail(email)) {
                    $.ajax({
                        url: "/Apis/IsEmailExists",
                        type: "POST",
                        data: { mail: email },
                        success: function (data) {
                            if (data == "True") {
                                fields[i].style.boxShadow = redShadow;
                                SetError(fields[i], "Данный адрес электронной почты занят!");
                                isvalid = false;
                            } else
                                fields[i].style.boxShadow = greenShadow;
                        },
                        dataType: "json",
                        async: false
                    });
                } else {
                    fields[i].style.boxShadow = redShadow;
                    SetError(fields[i], "Неккоректный адрес электронной почты!");
                    isvalid = false;
                }
            } else if (fields[i].name == "Sex") {
                var sex = parseInt(fields[i].value);
                if (sex < 0) {
                    $("#register-sex")[0].style.boxShadow = redShadow;
                    $("#register-sex-field")[0].style.boxShadow = redShadow;
                    SetError(fields[i], "Не выбран пол!");
                    isvalid = false;
                } else {
                    $("#register-sex")[0].style.boxShadow = greenShadow;
                    $("#register-sex-field")[0].style.boxShadow = greenShadow;
                }
            } else if (fields[i].name == "Login") {
                var login = fields[i].value;
                if (baseValid(login)) {
                    $.ajax({
                        url: "/Apis/IsLoginExists",
                        type: "POST",
                        data: { login: login },
                        success: function (data) {
                            if (data == "True") {
                                fields[i].style.boxShadow = redShadow;
                                SetError(fields[i], "Логин уже занят!");
                                isvalid = false;
                            } else
                                fields[i].style.boxShadow = greenShadow;
                        },
                        dataType: "json",
                        async: false
                    });
                } else {
                    fields[i].style.boxShadow = redShadow;
                    SetError(fields[i], "Недопустимое значение логина!");
                    isvalid = false;
                }
            } else if (fields[i].name == "confirmPassword") {
                var firstPassword = $("#popup-register-password").val();
                var secondPassword = fields[i].value;
                if (baseValid(firstPassword) && baseValid(secondPassword)) {
                    if (firstPassword == secondPassword) {
                        fields[i].style.boxShadow = greenShadow;
                    } else {
                        fields[i].style.boxShadow = redShadow;
                        SetError(fields[i], "Пароли не совпадают!");
                        isvalid = false;
                    }
                } else {
                    fields[i].style.boxShadow = redShadow;
                    SetError(fields[i], "Пароли не совпадают!");
                    isvalid = false;
                }
            } else {
                if (!baseValid(fields[i].value)) {
                    fields[i].style.boxShadow = redShadow;
                    SetError(fields[i], "Поле не заполнено или используются недопустимые символы!");
                    isvalid = false;
                } else fields[i].style.boxShadow = greenShadow;
            }

            if (!isvalid) return false;
        }

        if (!isvalid) return false;

        var information = {};
        fields.each(function (index, at) {
            information[at.name] = at.value;
        });

        var info = JSON.stringify(information);

        $.ajax({
            url: "/Apis/RegisterUser",
            type: "POST",
            data: { info: info },
            success: function (answer) {
                if (answer == "Ok") {
                    window.location.reload();
                } else {
                    if (answer == "EmailExists")
                        bootbox.alert('<b>Ошибка</b>: Попытка зарегистрировать занятый адрес электронной почты.');
                    else if (answer == "LoginExists")
                        bootbox.alert('<b>Ошибка</b>: Попытка зарегистрировать занятый логин.');
                    else if (answer == "DiffPass")
                        bootbox.alert('<b>Ошибка</b>: Введенные пароли не совпали.');
                    else if (answer == "Error")
                        bootbox.alert('<b>Ошибка</b>: Произошла внутренняя ошибка.');
                    else {
                        window.location.href = answer;
                    }
                }
            },
            dataType: "json",
            async: false
        });
    });

    //активация аккаунта с проверкой полей активации
    $("#popup-activate-go").click(function () {
        var fields = $(".activate-field");

        var isvalid = true;
        var redShadow = "3px 3px 0px #ff9999";
        var greenShadow = "3px 3px 0px #99ff99";

        for (var i = 0; i < fields.length; i++) {
            if (fields[i].name == "Email") {
                var email = fields[i].value;
                if (isValidEmail(email)) {

                    fields[i].style.boxShadow = greenShadow;

                } else {
                    fields[i].style.boxShadow = redShadow;
                    SetError(fields[i], "Неккоректный адрес электронной почты!");
                    isvalid = false;
                }
            } else if (fields[i].name == "Login") {
                var login = fields[i].value;
                if (baseValid(login)) {

                    fields[i].style.boxShadow = greenShadow;

                } else {
                    fields[i].style.boxShadow = redShadow;
                    SetError(fields[i], "Недопустимое значение логина!");
                    isvalid = false;
                }
            } else {
                if (!baseValid(fields[i].value)) {
                    fields[i].style.boxShadow = redShadow;
                    SetError(fields[i], "Поле не заполнено или используются недопустимые символы!");
                    isvalid = false;
                } else fields[i].style.boxShadow = greenShadow;
            }

            if (!isvalid) return false;
        }

        if (!isvalid) return false;

        $.ajax({
            url: "/Apis/ActivateAccount",
            type: "POST",
            data: { email: $("#activate-email").val(), login: $("#activate-login").val(), password: $("#activate-password").val() },
            success: function (answer) {
                window.location.href = answer;
            },
            dataType: "json",
            async: false
        });
    });

    //проверка полей активации при потере фокуса
    $(".activate-field").focusout(function () {
        var redShadow = "3px 3px 0px #ff9999";
        var greenShadow = "3px 3px 0px #99ff99";

        if (this.name == "Email") {
            var email = this.value;
            if (isValidEmail(email)) {
                this.style.boxShadow = greenShadow;

            } else {
                this.style.boxShadow = redShadow;
                SetError(this, "Неккоректный адрес электронной почты!");
            }
        } else if (this.name == "Login") {
            var login = this.value;
            if (baseValid(login)) {

                this.style.boxShadow = greenShadow;

            } else {
                this.style.boxShadow = redShadow;
                SetError(this, "Недопустимое значение логина!");
            }
        } else if (!baseValid(this.value)) {
            this.style.boxShadow = redShadow;
            SetError(this, "Поле не заполнено или используются недопустимые символы!");
        } else this.style.boxShadow = greenShadow;
    });

    //восстановление пароля
    $("#popup-recovery-go").click(function () {
        var fields = $(".recovery-field");

        var isvalid = true;
        var redShadow = "3px 3px 0px #ff9999";
        var greenShadow = "3px 3px 0px #99ff99";

        for (var i = 0; i < fields.length; i++) {
            if (fields[i].name == "Email") {
                var email = fields[i].value;
                if (isValidEmail(email)) {

                    fields[i].style.boxShadow = greenShadow;

                } else {
                    fields[i].style.boxShadow = redShadow;
                    SetError(fields[i], "Неккоректный адрес электронной почты!");
                    isvalid = false;
                }
            } else if (fields[i].name == "Login") {
                var login = fields[i].value;
                if (baseValid(login)) {

                    fields[i].style.boxShadow = greenShadow;

                } else {
                    fields[i].style.boxShadow = redShadow;
                    SetError(fields[i], "Недопустимое значение логина!");
                    isvalid = false;
                }
            }

            if (!isvalid) return false;
        }

        if (!isvalid) return false;
        recPopup.modal("hide");

        $.ajax({
            url: "/Apis/RecoveryPassword",
            type: "POST",
            data: { email: $("#recovery-email").val(), login: $("#recovery-login").val() },
            success: function (answer) {
                if (answer == "Ok") {
                    bootbox.alert("<p>Новый пароль выслан на указанную почту.</p> Настоятельно рекомендуем заменить его!");
                } else {
                    bootbox.alert("<p>Пароль не был изменен в следствии внутренней ошибки.</p> Проверьте вводимые данные и попытайтесь снова!");
                }
            },
            dataType: "json",
            async: false
        });
    });

    //проверка полей восстановления при потере фокуса
    $(".recovery-field").focusout(function () {
        var redShadow = "3px 3px 0px #ff9999";
        var greenShadow = "3px 3px 0px #99ff99";

        if (this.name == "Email") {
            var email = this.value;
            if (isValidEmail(email)) {
                this.style.boxShadow = greenShadow;

            } else {
                this.style.boxShadow = redShadow;
                SetError(this, "Неккоректный адрес электронной почты!");
            }
        } else if (this.name == "Login") {
            var login = this.value;
            if (baseValid(login)) {

                this.style.boxShadow = greenShadow;

            } else {
                this.style.boxShadow = redShadow;
                SetError(this, "Недопустимое значение логина!");
            }
        }
    });

    //пагинация
    $(".prev-page").click(function() {
        var pattern = $("#searcher").val();

        $(".prev-page .search-pattern").val(pattern);

        var form = $("#prev-form");
        form.submit();
    });
    
    //пагинация
    $(".next-page").click(function () {
        var pattern = $("#searcher").val();

        $(".next-page .search-pattern").val(pattern);

        var form = $("#next-form");
        form.submit();
    });

    //поиск
    $("#loop-searcher").click(function() {
        var pattern = $("#searcher").val();
        $("#current-search-pattern").val(pattern);
        $("#current-page-number").val(1);

        var form = $("#curr-form");
        form.submit();
    });

    //поиск
    $("#searcher").keypress(function (e) {
        if (e.keyCode == 13) {
            var pattern = $("#searcher").val();
            $("#current-search-pattern").val(pattern);
            $("#current-page-number").val(1);

            var form = $("#curr-form");
            form.submit();
        }
    });

    //выбор тэга из облака
    $(".tag-link").click(function (e) {
        e.preventDefault();
        var tag = $(this).text();

        if (tag.toLowerCase().indexOf("#") < 0)
            tag = '#' + tag;

        $("#current-search-pattern").val(tag);
        $("#current-page-number").val(1);

        var form = $("#curr-form");
        form.submit();
    });

    //выставление оценки новости
    $(".up-rating img, .down-rating img").click(function() {
        var command = $(this.parentNode.parentNode).hasClass("up-rating") ? "up" : "down";
        var postId = $(this.parentNode.parentNode.parentNode).find("input").val();
        var obj = this;

        $.ajax({
            url: "/Apis/ChangeRating",
            type: "POST",
            data: {command: command, postId: postId},
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

    SetCurrentSearchPattern();
    setInterval(ClearWarning, 5000);
});

//всплывание подсказки
function SetError(item, error) {
    var patient = item.previousElementSibling;

    $(patient).find("p").empty().append(error);
    patient.style.display = "block";
}

//валидация эмайла
function isValidEmail(email) {
    var result = true,
        regexp = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

    if (!regexp.test(email)) {
        result = false;
    }
    return result;
}

//базовая валидация (буквы и цифры)
function baseValid(value) {
    if (value.length > 0) {
        var regexp = /^[а-яА-ЯёЁa-zA-Z0-9]+$/;
        if (regexp.test(value))
            return true;
    }
    return false;
}

function LoadTags() {

    var tags = $("#tags ul");

    $.ajax({
        url: "/Apis/LoadTags",
        type: "POST",
        data: {},
        success: function (answer) {
            if (answer.Tags.length > 0) {
                for (var i = 0; i < answer.Tags.length; i++) {
                    tags.append("<li><a class=\"tag-link\" href=\"/#\">" + answer.Tags[i].Title + "</a></li>");
                };
            }
        },
        dataType: "json",
        async: false
    });
}

function LoadContacts() {
    var contacts = $("#contacts");

    $.ajax({
        url: "/Apis/LoadContacts",
        type: "POST",
        data: {},
        success: function (answer) {
            if (answer.Contacts.length > 0) {
                for (var i = 0; i < answer.Contacts.length; i++) {
                    contacts.append("<div class=\"item-contact\"><label class=\"label label-info contact-label\">" + answer.Contacts[i] + ":</label><label class=\"label label-info value-label\">" + answer.Values[i] + "</label></div>");
                };
            }
        },
        dataType: "json",
        async: false
    });
}

function ClearWarning() {
    $(".popup").hide();
}

function SetCurrentSearchPattern() {
    
    var inputField = $("#searcher");
    var pattern = $("#current-search-pattern").val();

    if (pattern != "")
        inputField.val(pattern);
}