$(function () {

    if ($('#tags-list').length > 0) {
        var tags = [];

        $.ajax({
            url: "/Apis/LoadAllTags",
            type: "POST",
            data: {},
            success: function (answer) {
                if (answer.Tags.length > 0) {
                    for (var i = 0; i < answer.Tags.length; i++) {
                        tags[i] = answer.Tags[i];
                    }
                    ;
                }
            },
            dataType: "json",
            async: false
        });

        $('#tags-list').tagbox({
            url: tags
        });
    }
});

$(function () {

    if ($('#link-list').length > 0) {
        var links = [];

        $.ajax({
            url: "/Apis/LoadAllLinks",
            type: "POST",
            data: {},
            success: function (answer) {
                if (answer.Links.length > 0) {
                    for (var i = 0; i < answer.Links.length; i++) {
                        links[i] = answer.Links[i];
                    }
                    ;
                }
            },
            dataType: "json",
            async: false
        });

        $('#link-list').tagbox({
            url: links,
            classname: 'LinksBox'
        });
    }
});