$(function () {
    $("td[title]").dblclick(function () {
        var title = $(this).attr('title'); alert(title);
    });
    $("td[title]").each(function () {
        $(this).attr('title', $(this).attr('title') + "\r\n" + "【双击弹框】");
    });
});