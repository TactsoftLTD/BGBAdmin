/*!
*   IDIM default 1.1.3
*   Author : Md Kakuya Taslim
*   Copyright @BGB
*/

var baseUrl = "";

$(document).ready(function () {
    selection2(".armyId");
    userSelection2(".userId");
    if ($.isFunction($.fn.select2)) {
        $(".select2").select2();
    }

    if ($.isFunction($.fn.iCheck)) {

        $('input[type="checkbox"].iCheck').iCheck({
            checkboxClass: 'icheckbox_square-green',
            radioClass: 'iradio_square-green',
            increaseArea: '20%'
        });

        $('input[type="radio"].iCheck').iCheck({
            checkboxClass: 'icheckbox_square-green',
            radioClass: 'iradio_square-green',
            increaseArea: '20%'
        });
    }

    var icon = $("input#Icon");
    if (icon.length) {
        $(icon).keyup(function () {
            $(this).siblings(".input-group-addon").find("i").removeClass().addClass("fa fa-" + $(this).val());
        });
    }

    var color = $("input#Color");
    if (color.length) {
        $(color).keyup(function () {
            $(this).siblings(".input-group-addon").find("i").css("color", "#" + $(this).val());
        });
    }

    var formValidate = $('form')[0];
    if (typeof formValidate != "undefined" && $.data(formValidate, 'validator')) {
        $.data(formValidate, 'validator').settings.ignore = ".hidden";
    }

    $.ajaxSetup({
        beforeSend: function () {
            $(".main-wrapper").loading({
                stoppable: true
            });
        },
        complete: function () {
            $(".main-wrapper").loading('stop');
        },
        success: function () { }
    });
});

if ($.isFunction($.fn.dataTable)) {
    $.extend($.fn.dataTable.defaults,
        {
            aaSorting: [],
            lengthMenu: [[10, 25, 50, 100], [10, 25, 50, 100]],
            order: []
        });
}

function selection2(selector, selected) {
    if ($.isFunction($.fn.select2)) {
        $(selector).select2({
            placeholder: "Search Regiment",
            minimumInputLength: 3,
            ajax: {
                url: baseUrl + '/GeneralInformation/Get',
                dataType: 'json',
                quietMillis: 250,
                data: function (param) {
                    return { term: param };
                },
                results: function (data) {
                    return { results: data.Results };
                }
            },
            initSelection: function (element, callback) {
                if (typeof selected != "undefined") {
                    callback({ id: selected.id, text: selected.text });
                }
            }
        });
    }
}

function userSelection2(selector, selected) {
    if ($.isFunction($.fn.select2)) {
        $(selector).select2({
            placeholder: "Search User",
            minimumInputLength: 3,
            ajax: {
                url: baseUrl + '/User/Get',
                dataType: 'json',
                quietMillis: 250,
                data: function (param) {
                    return { term: param };
                },
                results: function (data) {
                    return { results: data.Results };
                }
            },
            initSelection: function (element, callback) {
                if (typeof selected != "undefined") {
                    callback({ id: selected.id, text: selected.text });
                }
            }
        });
    }
}

function boolToIcon(data) {
    var icon = data === true ? "check" : "remove";
    var type = data === true ? "success" : "danger";

    return "<span class='text-" + type + "'><i class='fa fa-" + icon + "'></i></span>";
}

function toIcon(icon) {
    return "<span class='text-default'><i class='fa fa-" + icon + "'></i></span>";
}

function arrayToObject(array) {
    var obj = {};
    for (var i = 0; i < array.length; i++) {
        obj[array[i]['name']] = array[i]['value'];
    }
    return obj;
}

function ajaxCallDetail(selector) {
    var success = false;
    if (selector.valid()) {
        var data = selector.serialize();

        if (typeof items !== 'undefined')
            data = data + '&' + $.param({ 'Items': items });

        $.ajax({
            processData: false,
            data: data,
            dataType: 'json',
            async: false,
            contentType: 'application/x-www-form-urlencoded; charset=utf-8',
            type: selector.attr('method'),
            url: selector.attr('action'),
            success: function (response) {
                success = response.success;
                $("#message").html(jsonMessage(response));
            }
        });
    }
    return success;
}



function jsonMessage(m) {
    if (typeof m === 'undefined')
        return "";

    var type = typeof m.Success !== 'undefined' && m.Success === true ? "success" : "danger";
    var icon = typeof m.Success !== 'undefined' && m.Success === true ? "check" : "remove";

    return "<div class='col-lg-12'>"
        + "<div class='alert  alert-" + type + " alert-dismissible' role='alert'>"
        + "<button type='button' class='close' data-dismiss='alert' aria-label='Close'>"
        + "<span aria-hidden='true'>&times;</span>"
        + "</button>"
        + "<i class='fa fa-" + icon + "'></i> "
        + m.Message
        + "</div>"
        + "</div>";
}

function message(m) {
    if (typeof m !== 'undefined')
        return "";

    return "<div class='col-lg-12'>"
        + "<div class='alert  alert-info alert-dismissible' role='alert'>"
        + "<button type='button' class='close' data-dismiss='alert' aria-label='Close'>"
        + "<span aria-hidden='true'>&times;</span>"
        + "</button>"
        + "<i class='fa " + m.icon + "'></i>"
        + m.Text
        + "</div>"
        + "</div>";
}

function emptyAttachment() {
    $(".qq-upload-success").hide();
    $("#AttachementId").val(0);
    $(".remove-file-button").hide();
    $(".qq-upload-list").hide();
    $(".uploaded-file").hide();
}

var select2Cascade = (function (window, $) {

    function select2Cascade(parent, child, url, selectTitle, select2Options) {
        var afterActions = [];
        var options = select2Options || {};

        // Register functions to be called after cascading data loading done
        this.then = function (callback) {
            afterActions.push(callback);
            return this;
        };

        parent.select2(select2Options).on("change", function (e) {

            child.prop("disabled", true);
            var _this = this;

            $.getJSON(url.replace(':parentId:', $(this).val()), function (items) {
                var newOptions = '<option value="">' + selectTitle + '</option>';
                var data = items.data;
                for (var id in data) {
                    var selected = data[id].Selected == true ? "Selected" : "";
                    newOptions += '<option value="' + data[id].Value + '"' + selected + '>' + data[id].Text + '</option>';
                }

                child.select2('destroy').html(newOptions).prop("disabled", false)
                    .select2(options);

                afterActions.forEach(function (callback) {
                    callback(parent, child, items);
                });
            });
        });
    }

    return select2Cascade;
})(window, $);

// idle login
var idle = function () {
    var t;
    var timeout = time;
    window.onload = resetTimer;
    document.onmousemove = resetTimer;
    document.onkeypress = resetTimer;
    document.onload = resetTimer;
    document.onmousedown = resetTimer; // touchscreen presses
    document.ontouchstart = resetTimer;
    document.onclick = resetTimer;     // touchpad clicks
    document.onscroll = resetTimer;    // scrolling with arrow keys

    function logout() {
        window.location.href = lourl;
    }

    function resetTimer() {
        //console.log("reset");
        timeout = time;
        clearTimeout(t);
        t = setTimeout(logout, time);
    }

    setInterval(function () {
        timeout -= 1000;
        //console.log(timeout);
    }, 1000);
};

typeof time != "undefined" && time > 0 && idle();

$(".input-pin input").keyup(function (e) {
    var k = e.keyCode || e.which, i;
    (k >= 48 && k <= 57 || k >= 96 && k <= 105) &&
        (i = parseInt($(this).attr("tabindex")) + 1, $("input[tabindex=" + i + "]").focus());
});

// custom
//purchase budget create, edit
//$(document).on("change", "#PurchaseQuantity, #UnitPrice", function () {
//    $("#TotalPrice").val($("#PurchaseQuantity").val() * $("#UnitPrice").val());
//});