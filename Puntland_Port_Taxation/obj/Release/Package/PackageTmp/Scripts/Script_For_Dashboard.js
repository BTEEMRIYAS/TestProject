$(document).ready(function () {
    $("#down_sign").click(function (e) {
        $("#comment_box_visibility").slideToggle("medium");
        e.preventDefault();
    });
    $("#master_data_dashboard").click(function (e) {
        if (!$(e.target).closest('#master_data_items').length) {
            $('#master_data_items').slideToggle("medium");
        }
        $("#manage_action_link_plus").toggle();
        $("#manage_action_link_minus").toggle();
    });
    $("#user_mangement_dashboard").click(function (e) {
        if (!$(e.target).closest('#user_management_items').length) {
            $('#user_management_items').slideToggle("medium");
        }
        $("#manage_action_link_plus_user").toggle();
        $("#manage_action_link_minus_user").toggle();
    });
    $("#ship_arrival_dash_board").click(function (e) {
        if (!$(e.target).closest('#ship_arrival_items').length) {
            $('#ship_arrival_items').slideToggle("medium");
        }
        $("#manage_action_link_plus_ship").toggle();
        $("#manage_action_link_minus_ship").toggle();
    });
    $("#import_dash_board").click(function (e) {
        if (!$(e.target).closest('#import_items').length) {
            $('#import_items').slideToggle("medium");
        }
        $("#manage_action_link_plus_import").toggle();
        $("#manage_action_link_minus_import").toggle();
    });
    $("#export_dash_board").click(function (e) {
        if (!$(e.target).closest('#import_items').length) {
            $('#export_items').slideToggle("medium");
        }
        $("#manage_action_link_plus_export").toggle();
        $("#manage_action_link_minus_export").toggle();
    });
    $("#evaluation_dash_board").click(function (e) {
        if (!$(e.target).closest('#evaluation_items').length) {
            $('#evaluation_items').slideToggle("medium");
        }
        $("#manage_action_link_plus_evaluation").toggle();
        $("#manage_action_link_minus_evaluation").toggle();
    });
    $("#bank_dash_board").click(function (e) {
        if (!$(e.target).closest('#bank_items').length) {
            $('#bank_items').slideToggle("medium");
        }
        $("#manage_action_link_plus_bank").toggle();
        $("#manage_action_link_minus_bank").toggle();
    });
    $("#levi_dash_board").click(function (e) {
        if (!$(e.target).closest('#levy_items').length) {
            $('#levy_items').slideToggle("medium");
        }
        $("#manage_action_link_plus_levy").toggle();
        $("#manage_action_link_minus_levy").toggle();
    });
    $("#bolleto_dash_board").click(function (e) {
        if (!$(e.target).closest('#bolleto_items').length) {
            $('#bolleto_items').slideToggle("medium");
        }
        $("#manage_action_link_plus_bolleto").toggle();
        $("#manage_action_link_minus_bolleto").toggle();
    });
    $("#cargo_dash_board").click(function (e) {
        if (!$(e.target).closest('#cargo_items').length) {
            $('#cargo_items').slideToggle("medium");
        }
        $("#manage_action_link_plus_cargo").toggle();
        $("#manage_action_link_minus_cargo").toggle();
    });
    $(document).click(function (e) {
        if (!$(e.target).closest('#down_sign, #comment_box_visibility').length) {
            $("#comment_box_visibility").slideUp();
        }
        if (!$(e.target).closest('#master_data_dashboard, #master_dat_goods_id, #master_data_items').length) {
            $("#master_data_items").slideUp();
            $("#manage_action_link_plus").show();
            $("#manage_action_link_minus").hide();
        }
        if (!$(e.target).closest('#user_mangement_dashboard').length) {
            $("#user_management_items").slideUp();
            $("#manage_action_link_plus_user").show();
            $("#manage_action_link_minus_user").hide();
        }
        if (!$(e.target).closest('#ship_arrival_dash_board').length) {
            $("#ship_arrival_items").slideUp();
            $("#manage_action_link_plus_ship").show();
            $("#manage_action_link_minus_ship").hide();
        }
        if (!$(e.target).closest('#import_dash_board').length) {
            $("#import_items").slideUp();
            $("#manage_action_link_plus_import").show();
            $("#manage_action_link_minus_import").hide();
        }
        if (!$(e.target).closest('#export_dash_board').length) {
            $("#export_items").slideUp();
            $("#manage_action_link_plus_export").show();
            $("#manage_action_link_minus_export").hide();
        }
        if (!$(e.target).closest('#evaluation_dash_board').length) {
            $("#evaluation_items").slideUp();
            $("#manage_action_link_plus_evaluation").show();
            $("#manage_action_link_minus_evaluation").hide();
        }
        if (!$(e.target).closest('#bank_dash_board').length) {
            $("#bank_items").slideUp();
            $("#manage_action_link_plus_bank").show();
            $("#manage_action_link_minus_bank").hide();
        }
        if (!$(e.target).closest('#levi_dash_board').length) {
            $("#levy_items").slideUp();
            $("#manage_action_link_plus_levy").show();
            $("#manage_action_link_minus_levy").hide();
        }
        if (!$(e.target).closest('#bolleto_dash_board').length) {
            $("#bolleto_items").slideUp();
            $("#manage_action_link_plus_bolleto").show();
            $("#manage_action_link_minus_bolleto").hide();
        }
        if (!$(e.target).closest('#cargo_dash_board').length) {
            $("#cargo_items").slideUp();
            $("#manage_action_link_plus_cargo").show();
            $("#manage_action_link_minus_cargo").hide();
        }
    });
    if ($('#error').val() != null) {
        var error_message = $('#error').val();
        var url = "../Home/Error_Message";
        $.get(url, { error_message: error_message }, function (data) {
            if (data != '') {
                $("html, body").animate({ scrollTop: 0 }, 500);
                $('.popup__content_error').html(data);
                $('.popup-background_error').addClass('popup-widget-overlay');
                $('.popup-popup-content_error').css({
                    'display': 'block',
                    'height': 'auto',
                    'left': '43%',
                    'position': 'absolute',
                    'top': '175px',
                    'width': '370px',
                    'z-index': '999'
                });
            }
            else {
                var form = 'Invalid Request';
            }
        });
    }
    var width_dashboard = document.getElementById("main_dashboard_padding").scrollWidth;
    var width_profile_image = document.getElementById("profile_image_part").scrollWidth;
    document.getElementById('super_admin_action_part').style.width = (width_dashboard - width_profile_image - 20) + 'px';
});

function master_dat_call() {
    var url = "../Home/Master_Data_Dashboard";
    $.get(url, {}, function (data) {
        if (data != '') {
            $('#master_data_items').html(data);
        }
        else {
            var form = 'Invalid Request';
        }
    });
}
function user_management_call() {
    var url = "../Home/User_Management_Dashboard";
    $.get(url, {}, function (data) {
        if (data != '') {
            $('#user_management_items').html(data);
        }
        else {
            var form = 'Invalid Request';
        }
    });
}
function ship_arrival_management_call() {
    var url = "../Home/Ship_Arrival_Dashboard";
    $.get(url, {}, function (data) {
        if (data != '') {
            $('#ship_arrival_items').html(data);
        }
        else {
            var form = 'Invalid Request';
        }
    });
}
function import_management_call() {
    var url = "../Home/Import_Management_Dashboard";
    $.get(url, {}, function (data) {
        if (data != '') {
            $('#import_items').html(data);
        }
        else {
            var form = 'Invalid Request';
        }
    });
}
function export_management_call() {
    var url = "../Home/Export_Management_Dashboard";
    $.get(url, {}, function (data) {
        if (data != '') {
            $('#export_items').html(data);
        }
        else {
            var form = 'Invalid Request';
        }
    });
}
function evaluation_management_call() {
    var url = "../Home/Evaluation_Management_Dashboard";
    $.get(url, {}, function (data) {
        if (data != '') {
            $('#evaluation_items').html(data);
        }
        else {
            var form = 'Invalid Request';
        }
    });
}
function bank_management_call() {
    var url = "../Home/Bank_Management_Dashboard";
    $.get(url, {}, function (data) {
        if (data != '') {
            $('#bank_items').html(data);
        }
        else {
            var form = 'Invalid Request';
        }
    });
}
function levy_management_call() {
    var url = "../Home/Levy_Management_Dashboard";
    $.get(url, {}, function (data) {
        if (data != '') {
            $('#levy_items').html(data);
        }
        else {
            var form = 'Invalid Request';
        }
    });
}
function bolleto_management_call() {
    var url = "../Home/Bolleto_Management_Dashboard";
    $.get(url, {}, function (data) {
        if (data != '') {
            $('#bolleto_items').html(data);
        }
        else {
            var form = 'Invalid Request';
        }
    });
}
function cargo_management_call() {
    var url = "../Home/Cargo_Management_Dashboard";
    $.get(url, {}, function (data) {
        if (data != '') {
            $('#cargo_items').html(data);
        }
        else {
            var form = 'Invalid Request';
        }
    });
}

/*Popup for change password*/
function addpopup_change_password(id) {
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
    var url = path + "/../Manage_User/change_password/";
    $.get(url, { id: id }, function (data) {
        if (data != '') {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content').html(data);
            $('.popup-background').addClass('popup-widget-overlay');
            $('.popup-popup-content').css({
                'display': 'block',
                'height': 'auto',
                'left': '43%',
                'position': 'absolute',
                'top': '150px',
                'width': '463px',
                'z-index': '999'
            });
        }
        else {
            var form = 'Invalid Request';
        }
    });
}

/*Popup for change User Name*/
function addpopup_change_user_name(id) {
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
    var url = path + "/../Manage_User/change_user_name/";
    $.get(url, { id: id }, function (data) {
        if (data != '') {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content').html(data);
            $('.popup-background').addClass('popup-widget-overlay');
            $('.popup-popup-content').css({
                'display': 'block',
                'height': 'auto',
                'left': '43%',
                'position': 'absolute',
                'top': '150px',
                'width': '463px',
                'z-index': '999'
            });
        }
        else {
            var form = 'Invalid Request';
        }
    });
}

/*Popup for my_profile*/
function addpopup_my_profile(id) {
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
    var url = path + "/../Manage_User/Edit/";
    $.get(url, { id: id }, function (data) {
        if (data != '') {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content').html(data);
            $('.popup-background').addClass('popup-widget-overlay');
            $('.popup-popup-content').css({
                'display': 'block',
                'height': 'auto',
                'left': '43%',
                'position': 'absolute',
                'top': '150px',
                'width': '463px',
                'z-index': '999'
            });
        }
        else {
            var form = 'Invalid Request';
        }
    });
}

/*Close Popup*/
function closePopup() {
    $('.popup-background').removeClass('popup-widget-overlay');
    $('.popup__content').css({
        'padding': '25px 0px 25px 45px'
    });
    $('.popup-popup-content').css({
        'display': 'none'
    });
    $('.popup-more-content').css({
        'display': 'none'
    });
    $('.popup__content').html('');
    $('.more__content').html('');
}

/*Close Popup Error*/
function closePopup_error() {
    $('.popup-background_error').removeClass('popup-widget-overlay');
    $('.popup__content_error').css({
        'padding': '15px 0px 15px 0px'
    });
    $('.popup-popup-content_error').css({
        'display': 'none'
    });
    $('.popup-more-content').css({
        'display': 'none'
    });
    $('.popup__content_error').html('');
    $('.more__content').html('');
}