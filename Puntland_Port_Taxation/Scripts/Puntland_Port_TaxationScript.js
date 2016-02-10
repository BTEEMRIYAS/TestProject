$(document).ready(function () {
    $("#down_sign").click(function (e) {
        $("#comment_box_visibility").slideToggle("medium");
        e.preventDefault();
    });
    $("#inner_master_data_dashboard").click(function (e) {
        if (!$(e.target).closest('#inner_master_data_items').length) {
            $('#inner_master_data_items').slideToggle("medium");
        }
        $("#manage_action_link_plus_inner_md").toggle();
        $("#manage_action_link_minus_inner_md").toggle();
    });
    $("#inner_user_mangement_dashboard").click(function (e) {
        if (!$(e.target).closest('#inner_user_management_items').length) {
            $('#inner_user_management_items').slideToggle("medium");
        }
        $("#manage_action_link_plus_inner_um").toggle();
        $("#manage_action_link_minus_inner_um").toggle();
    });
    $("#inner_ship_arrival_dash_board").click(function (e) {
        if (!$(e.target).closest('#inner_ship_arrival_items').length) {
            $('#inner_ship_arrival_items').slideToggle("medium");
        }
        $("#manage_action_link_plus_inner_sa").toggle();
        $("#manage_action_link_minus_inner_sa").toggle();
    });
    $("#inner_import_dash_board").click(function (e) {
        if (!$(e.target).closest('#inner_import_items').length) {
            $('#inner_import_items').slideToggle("medium");
        }
        $("#manage_action_link_plus_inner_import").toggle();
        $("#manage_action_link_minus_inner_import").toggle();
    });
    $("#inner_export_dash_board").click(function (e) {
        if (!$(e.target).closest('#inner_export_items').length) {
            $('#inner_export_items').slideToggle("medium");
        }
        $("#manage_action_link_plus_inner_export").toggle();
        $("#manage_action_link_minus_inner_export").toggle();
    });
    $("#inner_evaluation_dash_board").click(function (e) {
        if (!$(e.target).closest('#inner_evaluation_items').length) {
            $('#inner_evaluation_items').slideToggle("medium");
        }
        $("#manage_action_link_plus_inner_evaluation").toggle();
        $("#manage_action_link_minus_inner_evaluation").toggle();
    });
    $("#inner_bank_dash_board").click(function (e) {
        if (!$(e.target).closest('#inner_bank_items').length) {
            $('#inner_bank_items').slideToggle("medium");
        }
        $("#manage_action_link_plus_inner_bank").toggle();
        $("#manage_action_link_minus_inner_bank").toggle();
    });
    $("#inner_manage_levy_dash_board").click(function (e) {
        if (!$(e.target).closest('#inner_levy_items').length) {
            $('#inner_levy_items').slideToggle("medium");
        }
        $("#manage_action_link_plus_inner_levy").toggle();
        $("#manage_action_link_minus_inner_levy").toggle();
    });
    $("#inner_bolleto_dash_board").click(function (e) {
        if (!$(e.target).closest('#inner_bolleto_items').length) {
            $('#inner_bolleto_items').slideToggle("medium");
        }
        $("#manage_action_link_plus_inner_bolleto").toggle();
        $("#manage_action_link_minus_inner_bolleto").toggle();
    });
    $("#inner_cargo_dash_board").click(function (e) {
        if (!$(e.target).closest('#inner_cargo_items').length) {
            $('#inner_cargo_items').slideToggle("medium");
        }
        $("#manage_action_link_plus_inner_cargo").toggle();
        $("#manage_action_link_minus_inner_cargo").toggle();
    });
    $(document).click(function (e) {
        if (!$(e.target).closest('#down_sign, #comment_box_visibility').length) {
            $("#comment_box_visibility").slideUp();
        }
        if (!$(e.target).closest('#inner_master_data_dashboard, #inner_goods_link, #inner_master_data_items').length) {
            $("#inner_master_data_items").slideUp();
            $("#manage_action_link_plus_inner_md").show();
            $("#manage_action_link_minus_inner_md").hide();
        }
        if (!$(e.target).closest('#inner_user_mangement_dashboard').length) {
            $("#inner_user_management_items").slideUp();
            $("#manage_action_link_plus_inner_um").show();
            $("#manage_action_link_minus_inner_um").hide();
        }
        if (!$(e.target).closest('#inner_ship_arrival_dash_board').length) {
            $("#inner_ship_arrival_items").slideUp();
            $("#manage_action_link_plus_inner_sa").show();
            $("#manage_action_link_minus_inner_sa").hide();
        }
        if (!$(e.target).closest('#inner_import_dash_board').length) {
            $("#inner_import_items").slideUp();
            $("#manage_action_link_plus_inner_import").show();
            $("#manage_action_link_minus_inner_import").hide();
        }
        if (!$(e.target).closest('#inner_export_dash_board').length) {
            $("#inner_export_items").slideUp();
            $("#manage_action_link_plus_inner_export").show();
            $("#manage_action_link_minus_inner_export").hide();
        }
        if (!$(e.target).closest('#inner_evaluation_dash_board').length) {
            $("#inner_evaluation_items").slideUp();
            $("#manage_action_link_plus_inner_evaluation").show();
            $("#manage_action_link_minus_inner_evaluation").hide();
        }
        if (!$(e.target).closest('#inner_bank_dash_board').length) {
            $("#inner_bank_items").slideUp();
            $("#manage_action_link_plus_inner_bank").show();
            $("#manage_action_link_minus_inner_bank").hide();
        }
        if (!$(e.target).closest('#inner_manage_levy_dash_board').length) {
            $("#inner_levy_items").slideUp();
            $("#manage_action_link_plus_inner_levy").show();
            $("#manage_action_link_minus_inner_levy").hide();
        }
        if (!$(e.target).closest('#inner_bolleto_dash_board').length) {
            $("#inner_bolleto_items").slideUp();
            $("#manage_action_link_plus_inner_bolleto").show();
            $("#manage_action_link_minus_inner_bolleto").hide();
        }
        if (!$(e.target).closest('#inner_cargo_dash_board').length) {
            $("#inner_cargo_items").slideUp();
            $("#manage_action_link_plus_inner_cargo").show();
            $("#manage_action_link_minus_inner_cargo").hide();
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
                    'left': '36%',
                    'position': 'absolute',
                    'top': '220px',
                    'width': '370px',
                    'z-index': '999'
                });
            }
            else {
                var form = 'Invalid Request';
            }
        });
    }
    $('#tablepaging').dataTable();
    $('#tablepaging_length').html('&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="javascript:void(0);" onclick="addpopup()" class="dbsearch" ><div class="add_admin">Add</div></a>&nbsp;<a href="javascript:void(0);" onclick="addpopup_search()" class="dbsearch" ><div class="dbsearchbutton">Search</div></a>');
    $('#tablepaging_paginate').html('');
    $('#tablepaging_info').html('');
    $('#tablepaging_filter').html('');  
});

function exchange_rate() {
    $('#ExchangeRate_file').click();
}

function print_way_bill(id) {
    var url = "../Manifest_Control_Section/Print_Waybill";
    $.get(url, { id: id }, function (data) {
        if (data != '') {
            var restorepage = document.body.innerHTML;
            var printcontent = data;
            document.body.innerHTML = printcontent;

            setTimeout(function () {
                window.print();
                document.body.innerHTML = restorepage;
                window.location.reload();
            }, 700);

        }
        else {
            var form = 'Invalid Request';
        }
    });
}

function payment_print() {
    var url = "../Payment/Submit_Manifest";
    $.get(url, {}, function (data) {
        if (data != '') {
            var restorepage = document.body.innerHTML;
            var printcontent = data;
            document.body.innerHTML = printcontent;
          
            setTimeout(function () {
                window.print();
                document.body.innerHTML = restorepage;
                window.location.reload();
            }, 700);

        }
        else {
            var form = 'Invalid Request';
        }
    });
}


//function print_bolleto(way_bill_id) {
//    var url = "../Home/print_bolleto";
//    $.get(url, { way_bill_id: way_bill_id }, function (data) {
//        if (data != '') {
//            $("html, body").animate({ scrollTop: 0 }, 500);
//            $('.popup__content').html(data);
//            $('.popup-background').addClass('popup-widget-overlay');
//            $('.popup-popup-content').css({
//                'display': 'block',
//                'height': 'auto',
//                'left': '36%',
//                'position': 'absolute',
//                'top': '40px',
//                'width': '320px',
//                'z-index': '999',
//                'padding-right': '10px'
//            });
//        }
//        else {
//            var form = 'Invalid Request';
//        }
//    });
//}

function print_bolleto(way_bill_id) {
    var currency_id = $('#currency_id').val();
    if (currency_id == null) {
        currency_id = 100;
    }
    var url = "../Home/Home_Print_Bolleto";
    $.get(url, { currency_id: currency_id, way_bill_id: way_bill_id }, function (data) {
        if (data != '') {
            var restorepage = document.body.innerHTML;
            var printcontent = data;
            document.body.innerHTML = printcontent;
            $('#comma_seperate_sos_val,#comma_seperate_usd_val').number(true, 2);

            setTimeout(function () {
                window.print();
                document.body.innerHTML = restorepage;
                window.location.reload();
            }, 700);
        }
        else {
            var form = 'Invalid Request';
        }
    });
}

function print_e_bolleto(way_bill_id) {
    var currency_id = $('#currency_id').val();
    if (currency_id == null) {
        currency_id = 100;
    }
    var url = "../Home/Home_Print_E_Bolleto";
    $.get(url, { currency_id: currency_id, way_bill_id: way_bill_id }, function (data) {
        if (data != '') {
            var restorepage = document.body.innerHTML;
            var printcontent = data;
            document.body.innerHTML = printcontent;
            $('#comma_seperate_sos_val,#comma_seperate_usd_val').number(true, 2);

            setTimeout(function () {
                window.print();
                document.body.innerHTML = restorepage;
                window.location.reload();
            }, 700);

        }
        else {
            var form = 'Invalid Request';
        }
    });
}

//Print Penalty Details
function print_penalty(way_bill_id) {
    var currency_id = $('#currency_id').val();
    if (currency_id == null) {
        currency_id = 100;
    }
    var url = "../Home/Home_Print_Penalty_Details";
    $.get(url, { currency_id: currency_id, way_bill_id: way_bill_id }, function (data) {
        if (data != '') {

            var restorepage = document.body.innerHTML;
            var printcontent = data;
            document.body.innerHTML = printcontent;
            $('#comma_seperate_sos_val,#comma_seperate_usd_val').number(true, 2);
            setTimeout(function () {
                window.print();
                document.body.innerHTML = restorepage;
                window.location.reload();
            }, 700);
        }
        else {
            var form = 'Invalid Request';
        }
    });
}

//Print Export Penalty Details
function print_e_penalty(way_bill_id) {
    var currency_id = $('#currency_id').val();
    if (currency_id == null) {
        currency_id = 100;
    }
    var url = "../Home/E_Home_Print_Penalty_Details";
    $.get(url, { currency_id: currency_id, way_bill_id: way_bill_id }, function (data) {
        if (data != '') {
            var restorepage = document.body.innerHTML;
            var printcontent = data;
            document.body.innerHTML = printcontent;
            $('#comma_seperate_sos_val,#comma_seperate_usd_val').number(true, 2);
            setTimeout(function () {
                window.print();
                document.body.innerHTML = restorepage;
                window.location.reload();
            }, 700);
        }
        else {
            var form = 'Invalid Request';
        }
    });
}

function print_receipt(way_bill_id) {
    var currency_id = $('#currency_id').val();
    if (currency_id == null) {
        currency_id = 100;
    }
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
    var oldURL = path;
    var index = 0;
    var newURL = oldURL;
    index = oldURL.indexOf('?');
    if (index == -1) {
        index = oldURL.indexOf('#');
    }
    if (index != -1) {
        newURL = oldURL.substring(0, index);
    }
    var url = newURL + "/Payment_Print_Receipt";
    //var url = "../Payment/Payment_Print_Receipt";
    $.get(url, { currency_id: currency_id, way_bill_id: way_bill_id }, function (data) {
        if (data != '') {
            var restorepage = document.body.innerHTML;
            var printcontent = data;
            document.body.innerHTML = printcontent;
            $('#comma_seperate_sos_val,#comma_seperate_usd_val').number(true, 2);


            setTimeout(function () {
                window.print();
                document.body.innerHTML = restorepage;
                window.location.reload();
            }, 700);
        }
        else {
            var form = 'Invalid Request';
        }
    });
}

function print_penalty_receipt(way_bill_id) {
    var currency_id = $('#currency_id').val();
    if (currency_id == null) {
        currency_id = 100;
    }
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
    var oldURL = path;
    var index = 0;
    var newURL = oldURL;
    index = oldURL.indexOf('?');
    if (index == -1) {
        index = oldURL.indexOf('#');
    }
    if (index != -1) {
        newURL = oldURL.substring(0, index);
    }
    var url = newURL + "/Payment_Print_Penalty_Receipt";
    //var url = "../Payment/Payment_Print_Penalty_Receipt";
    $.get(url, { currency_id: currency_id, way_bill_id: way_bill_id }, function (data) {
        if (data != '') {
            var restorepage = document.body.innerHTML;
            var printcontent = data;
            document.body.innerHTML = printcontent;
            $('#comma_seperate_sos_val,#comma_seperate_usd_val').number(true, 2);

            setTimeout(function () {
                window.print();
                document.body.innerHTML = restorepage;
                window.location.reload();
            }, 700);


        }
        else {
            var form = 'Invalid Request';
        }
    });
}

function Save_As_excel(way_bill_id) {
    var url = "../Home/Save_Excel";
    $.get(url, { way_bill_id: way_bill_id }, function (data) {
        if (data != '') {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content').html(data);
            $('.popup-background').addClass('popup-widget-overlay');
            $('.popup-popup-content').css({
                'display': 'block',
                'height': 'auto',
                'left': '36%',
                'position': 'absolute',
                'top': '40px',
                'width': '320px',
                'z-index': '999',
                'padding-right': '10px'
            });
        }
        else {
            var form = 'Invalid Request';
        }
    });
}

function export_to_excel() {
    var url = "../Home/ExportData_ToExcel";
    var currency_id = $('#currency_id').val();
    var way_bill_id = $('#way_bill_id').val();
    $.get(url, { currency_id: currency_id, way_bill_id: way_bill_id });
    //    if (data != '') {
    //        $("html, body").animate({ scrollTop: 0 }, 500);
    //        $('.popup__content').html(data);
    //        $('.popup-background').addClass('popup-widget-overlay');
    //        $('.popup-popup-content').css({
    //            'display': 'block',
    //            'height': 'auto',
    //            'left': '36%',
    //            'position': 'absolute',
    //            'top': '40px',
    //            'width': '320px',
    //            'z-index': '999',
    //            'padding-right': '10px'
    //        });
    //    }
    //    else {
    //        var form = 'Invalid Request';
    //    }
    //});
}

function inner_master_dat_call() {
    var url = "../Home/Inner_Master_Data_Dashboard";
    $.get(url, {}, function (data) {
        if (data != '') {
            $('#inner_master_data_items').html(data);
        }
        else {
            var form = 'Invalid Request';
        }
    });
}
function inner_user_management_call() {
    var url = "../Home/Inner_User_Management_Dashboard";
    $.get(url, {}, function (data) {
        if (data != '') {
            $('#inner_user_management_items').html(data);
        }
        else {
            var form = 'Invalid Request';
        }
    });
}
function inner_ship_arrival_management_call() {
    var url = "../Home/Inner_Ship_Arrival_Dashboard";
    $.get(url, {}, function (data) {
        if (data != '') {
            $('#inner_ship_arrival_items').html(data);
        }
        else {
            var form = 'Invalid Request';
        }
    });
}
function inner_import_management_call() {
    var url = "../Home/Inner_Import_Management_Dashboard";
    $.get(url, {}, function (data) {
        if (data != '') {
            $('#inner_import_items').html(data);
        }
        else {
            var form = 'Invalid Request';
        }
    });
}
function inner_export_management_call() {
    var url = "../Home/Inner_Export_Management_Dashboard";
    $.get(url, {}, function (data) {
        if (data != '') {
            $('#inner_export_items').html(data);
        }
        else {
            var form = 'Invalid Request';
        }
    });
}
function inner_evaluation_management_call() {
    var url = "../Home/Inner_Evaluation_Management_Dashboard";
    $.get(url, {}, function (data) {
        if (data != '') {
            $('#inner_evaluation_items').html(data);
        }
        else {
            var form = 'Invalid Request';
        }
    });
}
function inner_bank_management_call() {
    var url = "../Home/Inner_Bank_Management_Dashboard";
    $.get(url, {}, function (data) {
        if (data != '') {
            $('#inner_bank_items').html(data);
        }
        else {
            var form = 'Invalid Request';
        }
    });
}
function inner_levy_management_call() {
    var url = "../Home/Inner_Levy_Management_Dashboard";
    $.get(url, {}, function (data) {
        if (data != '') {
            $('#inner_levy_items').html(data);
        }
        else {
            var form = 'Invalid Request';
        }
    });
}
function inner_bolleto_management_call() {
    var url = "../Home/Inner_Bolleto_Management_Dashboard";
    $.get(url, {}, function (data) {
        if (data != '') {
            $('#inner_bolleto_items').html(data);
        }
        else {
            var form = 'Invalid Request';
        }
    });
}
function inner_cargo_management_call() {
    var url = "../Home/Inner_Cargo_Management_Dashboard";
    $.get(url, {}, function (data) {
        if (data != '') {
            $('#inner_cargo_items').html(data);
        }
        else {
            var form = 'Invalid Request';
        }
    });
}

/*Add Popup for User*/
function addpopup() {
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
    var oldURL = path;
    var index = 0;
    var newURL = oldURL;
    index = oldURL.indexOf('?');
    if (index == -1) {
        index = oldURL.indexOf('#');
    }
    if (index != -1) {
        newURL = oldURL.substring(0, index);
    }
    var url = newURL + "/Create";
    $.get(url, {}, function (data) {
        if (data != '') {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content').html(data);
            $('.popup-background').addClass('popup-widget-overlay');
            $('.popup-popup-content').css({
                'display': 'block',
                'height': 'auto',
                'left': '43%',
                'position': 'absolute',
                'top': '40px',
                'width': '463px',
                'z-index': '999'
            });
        }
        else {
            var form = 'Invalid Request';
        }
    });
}

/*Add Popup for Submit Payment*/
function submit_payment(way_bill_id) {
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
    var oldURL = path;
    var index = 0;
    var newURL = oldURL;
    index = oldURL.indexOf('?');
    if (index == -1) {
        index = oldURL.indexOf('#');
    }
    if (index != -1) {
        newURL = oldURL.substring(0, index);
    }
    var url = newURL + "/Submit_Payment";
    $.get(url, { way_bill_id: way_bill_id }, function (data) {
        if (data != '') {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content').html(data);
            $('.popup-background').addClass('popup-widget-overlay');
            $('.popup-popup-content').css({
                'display': 'block',
                'height': 'auto',
                'left': '43%',
                'position': 'absolute',
                'top': '40px',
                'width': '525px',
                'z-index': '999'
            });
        }
        else {
            var form = 'Invalid Request';
        }
    });
}

/*Add Popup for Submit Payment*/
function submit_penalty_payment(way_bill_id) {
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
    var oldURL = path;
    var index = 0;
    var newURL = oldURL;
    index = oldURL.indexOf('?');
    if (index == -1) {
        index = oldURL.indexOf('#');
    }
    if (index != -1) {
        newURL = oldURL.substring(0, index);
    }
    var url = newURL + "/Submit_Penalty_Payment";
    $.get(url, { way_bill_id: way_bill_id }, function (data) {
        if (data != '') {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content').html(data);
            $('.popup-background').addClass('popup-widget-overlay');
            $('.popup-popup-content').css({
                'display': 'block',
                'height': 'auto',
                'left': '43%',
                'position': 'absolute',
                'top': '40px',
                'width': '525px',
                'z-index': '999'
            });
        }
        else {
            var form = 'Invalid Request';
        }
    });
}

/*Add Popup for Levi View*/
function levi_entry_view(id) {
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
    var oldURL = path;
    var index = 0;
    var newURL = oldURL;
    index = oldURL.indexOf('?');
    if (index == -1) {
        index = oldURL.indexOf('#');
    }
    if (index != -1) {
        newURL = oldURL.substring(0, index);
    }
    var url = newURL + "/Details";
    $.get(url, { id : id }, function (data) {
        if (data != '') {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content').html(data);
            $('.popup-background').addClass('popup-widget-overlay');
            $('.popup-popup-content').css({
                'display': 'block',
                'height': 'auto',
                'left': '43%',
                'position': 'absolute',
                'top': '40px',
                'width': '463px',
                'z-index': '999'
            });
        }
        else {
            var form = 'Invalid Request';
        }
    });
}

/*Popup for adding goods price*/
function add_goods_price(way_bill_id) {
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
    var oldURL = path;
    var index = 0;
    var newURL = oldURL;
    index = oldURL.indexOf('?');
    if (index == -1) {
        index = oldURL.indexOf('#');
    }
    if (index != -1) {
        newURL = oldURL.substring(0, index);
    }
    var url = newURL + "/Add_Goods_Price";
    $.get(url, { way_bill_id: way_bill_id }, function (data) {
        if (data != '') {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content').html(data);
            $('.popup-background').addClass('popup-widget-overlay');
            $('.popup-popup-content').css({
                'display': 'block',
                'height': 'auto',
                'left': '15%',
                'position': 'absolute',
                'top': '40px',
                'width': '990px',
                'z-index': '999',
                'padding-right': '40px'
            });
        }
        else {
            var form = 'Invalid Request';
        }
    });
}

/*Popup for adding penalty goods price*/
function add_penalty_goods_price(way_bill_id) {
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
    var oldURL = path;
    var index = 0;
    var newURL = oldURL;
    index = oldURL.indexOf('?');
    if (index == -1) {
        index = oldURL.indexOf('#');
    }
    if (index != -1) {
        newURL = oldURL.substring(0, index);
    }
    var url = newURL + "/Add_Penalty_Goods_Price";
    $.get(url, { way_bill_id: way_bill_id }, function (data) {
        if (data != '') {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content').html(data);
            $('.popup-background').addClass('popup-widget-overlay');
            $('.popup-popup-content').css({
                'display': 'block',
                'height': 'auto',
                'left': '15%',
                'position': 'absolute',
                'top': '40px',
                'width': '990px',
                'z-index': '999',
                'padding-right': '40px'
            });
        }
        else {
            var form = 'Invalid Request';
        }
    });
}

/*Popup for Assign Way Bill*/
function assign_way_bill(way_bill_id) {
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
    var oldURL = path;
    var index = 0;
    var newURL = oldURL;
    index = oldURL.indexOf('?');
    if (index == -1) {
        index = oldURL.indexOf('#');
    }
    if (index != -1) {
        newURL = oldURL.substring(0, index);
    }
    var url = newURL + "/Assign_Way_Bill";
    $.get(url, { way_bill_id: way_bill_id }, function (data) {
        if (data != '') {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content').html(data);
            $('.popup-background').addClass('popup-widget-overlay');
            $('.popup-popup-content').css({
                'display': 'block',
                'height': 'auto',
                'left': '43%',
                'position': 'absolute',
                'top': '40px',
                'width': '463px',
                'z-index': '999'
            });
        }
        else {
            var form = 'Invalid Request';
        }
    });
}

/*Add Popup for select currency View*/
//function addpopup_view_tax_dxetails(way_bill_id) {
//    var pathstring = String(window.location);
//    var patharray = pathstring.split("/");
//    var controllerName = patharray[3];
//    var url = "../Home/View_Tax_Details";
//    $.get(url, { way_bill_id: way_bill_id, controllerName: controllerName }, function (data) {
//        if (data != '') {
//            $("html, body").animate({ scrollTop: 0 }, 500);
//            $('.popup__content').html(data);
//            $('.popup-background').addClass('popup-widget-overlay');
//            $('.popup-popup-content').css({
//                'display': 'block',
//                'height': 'auto',
//                'left': '36%',
//                'position': 'absolute',
//                'top': '40px',
//                'width': '320px',
//                'z-index': '999',
//                'padding-right': '10px'
//            });
//        }
//        else {
//            var form = 'Invalid Request';
//        }
//    });
//    document.getElementById("popup__content").className = "popup__content";
//}

/*Add Popup for Tax Calculation Details View*/
function addpopup_view_tax_dxetails(way_bill_id) {
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var controllerName = patharray[3];
    var url = "../Home/Display_Tax_Details";
    var currency_id = $('#currency_id').val();
    if (currency_id == null) {
        currency_id = 100;
    }
    $.get(url, { currency_id: currency_id, way_bill_id: way_bill_id, controllerName: controllerName }, function (data) {
        if (data != '') {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content_error').html(data);
            $('.popup-background_error').addClass('popup-widget-overlay');
            $(".popup-popup-content").animate({ left: "3%" }, 800);
                $('.popup-popup-content_error').css({
                    'display': 'block',
                    'height': 'auto',
                    'left': '18%',
                    'position': 'absolute',
                    'top': '40px',
                    'width': '950px',
                    'z-index': '999',
                    'padding-right': '20px',
                    'padding-left': '20px'
                });
        }
        else {
            var form = 'Invalid Request';
        }
    });
}

/*Add Popup for Tax Calculation Export Details View*/
function addpopup_view_e_tax_dxetails(way_bill_id) {
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var controllerName = patharray[3];
    var url = "../Home/E_Display_Tax_Details";
    var currency_id = $('#currency_id').val();
    if (currency_id == null) {
        currency_id = 100;
    }
    $.get(url, { currency_id: currency_id, way_bill_id: way_bill_id, controllerName: controllerName }, function (data) {
        if (data != '') {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content_error').html(data);
            $('.popup-background_error').addClass('popup-widget-overlay');
            $(".popup-popup-content").animate({ left: "3%" }, 800);
            $('.popup-popup-content_error').css({
                'display': 'block',
                'height': 'auto',
                'left': '18%',
                'position': 'absolute',
                'top': '40px',
                'width': '950px',
                'z-index': '999',
                'padding-right': '20px',
                'padding-left': '20px'
            });
        }
        else {
            var form = 'Invalid Request';
        }
    });
}


/*Add Popup for Penalty Calculation Details View*/
function addpopup_view_penalty_details(way_bill_id) {
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var controllerName = patharray[3];
    var url = "../Home/Display_Penalty_Details";
    var currency_id = $('#currency_id').val();
    if (currency_id == null) {
        currency_id = 100;
    }
    $.get(url, { currency_id: currency_id, way_bill_id: way_bill_id, controllerName: controllerName }, function (data) {
        if (data != '') {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content_error').html(data);
            $('.popup-background_error').addClass('popup-widget-overlay');
            $(".popup-popup-content").animate({ left: "3%" }, 800);
            $('.popup-popup-content_error').css({
                'display': 'block',
                'height': 'auto',
                'left': '18%',
                'position': 'absolute',
                'top': '40px',
                'width': '950px',
                'z-index': '999',
                'padding-right': '20px',
                'padding-left': '20px'
            });
        }
        else {
            var form = 'Invalid Request';
        }
    });
}

/*Add Popup for Export Penalty Calculation Details View*/
function addpopup_e_view_penalty_details(way_bill_id) {
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var controllerName = patharray[3];
    var url = "../Home/E_Display_Penalty_Details";
    var currency_id = $('#currency_id').val();
    if (currency_id == null) {
        currency_id = 100;
    }
    $.get(url, { currency_id: currency_id, way_bill_id: way_bill_id, controllerName: controllerName }, function (data) {
        if (data != '') {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content_error').html(data);
            $('.popup-background_error').addClass('popup-widget-overlay');
            $(".popup-popup-content").animate({ left: "3%" }, 800);
            $('.popup-popup-content_error').css({
                'display': 'block',
                'height': 'auto',
                'left': '18%',
                'position': 'absolute',
                'top': '40px',
                'width': '950px',
                'z-index': '999',
                'padding-right': '20px',
                'padding-left': '20px'
            });
        }
        else {
            var form = 'Invalid Request';
        }
    });
}

/*Add Way Bill Popup for User*/
function add_way_bill_popup() {
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
    var oldURL = path;
    var index = 0;
    var newURL = oldURL;
    index = oldURL.indexOf('?');
    if (index == -1) {
        index = oldURL.indexOf('#');
    }
    if (index != -1) {
        newURL = oldURL.substring(0, index);
    }
    var url = newURL + "/Create_Way_Bill";
    $.get(url, {}, function (data) {
        if (data != 0) {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content').html(data);
            $('.popup-background').addClass('popup-widget-overlay');
            $('.popup-popup-content').css({
                'display': 'block',
                'height': 'auto',
                'left': '43%',
                'position': 'absolute',
                'top': '40px',
                'width': '463px',
                'z-index': '999'
            });
        }
        else {
            alert("Way Bill Already Submitted");
            var form = 'Invalid Request';
        }
    });
}

/*Search Way Bill Popup in manifest Control*/
function Search_Way_Bill() {
    if ($('#controller').val() == "Examination Unit") {
        url = "../Examination_Unit/Search_Way_Bill/";
    }
    else {
        var url = "../Manifest_Control_Section/Search_Way_Bill";
    }
    $.get(url, {}, function (data) {
        if (data != 0) {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content').html(data);
            $('.popup-background').addClass('popup-widget-overlay');
            $('.popup-popup-content').css({
                'display': 'block',
                'height': 'auto',
                'left': '43%',
                'position': 'absolute',
                'top': '40px',
                'width': '463px',
                'z-index': '999'
            });
        }
        else {
            var form = 'Invalid Request';
        }
    });
}

/*Search Way Bill Popup in Accounting_Re_Verification*/
function Search_Way_Bill_In_accounting() {
    var url = "../Accounting_Re_Verification/Search_Way_Bill";
    $.get(url, {}, function (data) {
        if (data != 0) {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content').html(data);
            $('.popup-background').addClass('popup-widget-overlay');
            $('.popup-popup-content').css({
                'display': 'block',
                'height': 'auto',
                'left': '43%',
                'position': 'absolute',
                'top': '40px',
                'width': '463px',
                'z-index': '999'
            });
        }
        else {
            alert("Way Bill Already Submitted");
            var form = 'Invalid Request';
        }
    });
}

/*Search Way Bill Popup in Compliance*/
function Search_Way_Bill_In_Compliance() {
    var url = "../Compliance/Search_Way_Bill";
    $.get(url, {}, function (data) {
        if (data != 0) {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content').html(data);
            $('.popup-background').addClass('popup-widget-overlay');
            $('.popup-popup-content').css({
                'display': 'block',
                'height': 'auto',
                'left': '43%',
                'position': 'absolute',
                'top': '40px',
                'width': '463px',
                'z-index': '999'
            });
        }
        else {
            alert("Way Bill Already Submitted");
            var form = 'Invalid Request';
        }
    });
}

/*Search Way Bill Popup in Payment*/
function Search_Way_Bill_In_Payment() {
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
    var oldURL = path;
    var index = 0;
    var newURL = oldURL;
    index = oldURL.indexOf('?');
    if (index == -1) {
        index = oldURL.indexOf('#');
    }
    if (index != -1) {
        newURL = oldURL.substring(0, index);
    }
    var url = newURL + "/Search_Way_Bill";
    $.get(url, {}, function (data) {
        if (data != 0) {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content').html(data);
            $('.popup-background').addClass('popup-widget-overlay');
            $('.popup-popup-content').css({
                'display': 'block',
                'height': 'auto',
                'left': '43%',
                'position': 'absolute',
                'top': '40px',
                'width': '463px',
                'z-index': '999'
            });
        }
        else {
            alert("Way Bill Already Submitted");
            var form = 'Invalid Request';
        }
    });
}

/*Search Way Bill Popup in Release*/
function Search_Way_Bill_In_Release() {
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
    var oldURL = path;
    var index = 0;
    var newURL = oldURL;
    index = oldURL.indexOf('?');
    if (index == -1) {
        index = oldURL.indexOf('#');
    }
    if (index != -1) {
        newURL = oldURL.substring(0, index);
    }
    var url = newURL + "/Search_Way_Bill";
    $.get(url, {}, function (data) {
        if (data != 0) {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content').html(data);
            $('.popup-background').addClass('popup-widget-overlay');
            $('.popup-popup-content').css({
                'display': 'block',
                'height': 'auto',
                'left': '43%',
                'position': 'absolute',
                'top': '40px',
                'width': '463px',
                'z-index': '999'
            });
        }
        else {
            alert("Way Bill Already Submitted");
            var form = 'Invalid Request';
        }
    });
}

/*Popup for compare Way Bill and Cargo Manifest*/
function compare_wb_cm(way_bill_id) {
    url = "../Manifest_Control_Section/Details/";
    $.get(url, { way_bill_id: way_bill_id }, function (data) {
    if (data != 0) {
        $("html, body").animate({ scrollTop: 0 }, 500);
        $('.popup__content').html(data);
        $('.popup-background').addClass('popup-widget-overlay');
        $('.popup-popup-content').css({
            'display': 'block',
            'height': 'auto',
            'left': '28%',
            'position': 'absolute',
            'top': '40px',
            'width': '910px',
            'z-index': '999',
            'padding-right': '20px',
            'padding-bottom' : '40px'
        });
    }
    else {
        var form = 'Invalid Request';
    }
});
}


/*Popup for View In Way Bill*/
function waybill_view(id) {
    var url = "";
    if ($('#controller').val() == "Examination Unit") {
        url = "../Examination_Unit/Details";
    }
    else if ($('#controller').val() == "Release") {
        url = "../Release/Details";
    }
    else if ($('#controller').val() == "Export Department") {
        url = "../Export_Department/Details";
    }
    else {
        var pathstring = String(window.location);
        var patharray = pathstring.split("/");
        var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
        var oldURL = path;
        var index = 0;
        var newURL = oldURL;
        index = oldURL.indexOf('?');
        if (index == -1) {
            index = oldURL.indexOf('#');
        }
        if (index != -1) {
            newURL = oldURL.substring(0, index);
        }
        url = newURL + "/View_Way_Bill/";
    }
    $.get(url, { id: id }, function (data) {
        if (data != 0) {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content').html(data);
            $('.popup-background').addClass('popup-widget-overlay');
            $('.popup-popup-content').css({
                'display': 'block',
                'height': 'auto',
                'left': '28%',
                'position': 'absolute',
                'top': '40px',
                'width': '910px',
                'z-index': '999',
                'padding-right': '40px'
            });
        }
        else {
            var form = 'Invalid Request';
        }
    });
}

function total_ggods_view(id)
{
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
    var oldURL = path;
    var index = 0;
    var newURL = oldURL;
    index = oldURL.indexOf('?');
    if (index == -1) {
        index = oldURL.indexOf('#');
    }
    if (index != -1) {
        newURL = oldURL.substring(0, index);
    }
    var url = newURL + "/Total_Goods_View";
    $.get(url, { id: id }, function (data) {
    if (data != 0) {
        $("html, body").animate({ scrollTop: 0 }, 500);
        $('.popup__content').html(data);
        $('.popup-background').addClass('popup-widget-overlay');
        $('.popup-popup-content').css({
            'display': 'block',
            'height': 'auto',
            'left': '28%',
            'position': 'absolute',
            'top': '40px',
            'width': '910px',
            'z-index': '999',
            'padding-right': '40px'
        });
    }
    else {
        var form = 'Invalid Request';
    }
});
}

/*Popup for View Bolleto*/
function bolleto_view(id) {
    var url = "../Manage_Import/View_Bolleto"
    $.get(url, { id: id }, function (data) {
        if (data != 0) {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content').html(data);
            $('.popup-background').addClass('popup-widget-overlay');
            $('.popup-popup-content').css({
                'display': 'block',
                'height': 'auto',
                'left': '28%',
                'position': 'absolute',
                'top': '40px',
                'width': '910px',
                'z-index': '999',
                'padding-right': '40px'
            });
        }
        else {
            var form = 'Invalid Request';
        }
    });
}

/*Popup for Reject Way Bill*/
function Reject_Reason(way_bill_id) {
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
    var oldURL = path;
    var index = 0;
    var newURL = oldURL;
    index = oldURL.indexOf('?');
    if (index == -1) {
        index = oldURL.indexOf('#');
    }
    if (index != -1) {
        newURL = oldURL.substring(0, index);
    }
    var url = newURL + "/Reject_Waybill";
    $.get(url, { way_bill_id: way_bill_id }, function (data) {
        if (data != 0) {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content_error').html(data);
            $('.popup-background_error').addClass('popup-widget-overlay');
            $('.popup-popup-content_error').css({
                'display': 'block',
                'padding-left': '0px',
                'padding-right' : '0px',
                'height': 'auto',
                'left': '43%',
                'position': 'absolute',
                'top': '150px',
                'width': '435px',
                'z-index': '999'
            });
        }
        else {
            var form = 'Invalid Request';
        }
    });
}

/*Popup for Recheck Way Bill*/
function Recheck_Reason(way_bill_id) {
    url = "../Examination_Unit/Recheck_Waybill/";
    $.get(url, { way_bill_id: way_bill_id }, function (data) {
        if (data != 0) {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content').html(data);
            $('.popup-background').addClass('popup-widget-overlay');
            $('.popup-popup-content').css({
                'display': 'block',
                'height': 'auto',
                'left': '43%',
                'position': 'absolute',
                'top': '40px',
                'width': '463px',
                'z-index': '999'
            });
        }
        else {
            var form = 'Invalid Request';
        }
    });
}

/*Popup for Reject Way Bill in Manifest Control*/
function Reject_Reason_manifest(way_bill_id) {
    var url = "../Manifest_Control_Section/Reject_Waybill/";
    $.get(url, { way_bill_id: way_bill_id }, function (data) {
        if (data != 0) {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content').html(data);
            $('.popup-background').addClass('popup-widget-overlay');
            $('.popup-popup-content').css({
                'display': 'block',
                'height': 'auto',
                'left': '43%',
                'position': 'absolute',
                'top': '40px',
                'width': '463px',
                'z-index': '999',
                'padding-bottom': '0px',
                'padding-right': '0px'
            });
        }
        else {
            alert("Way Bill Already Submitted");
            var form = 'Invalid Request';
        }
    });
}

/*Popup for View Reject Reason*/
function view_reject_reason(way_bill_id) {
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
    var oldURL = path;
    var index = 0;
    var newURL = oldURL;
    index = oldURL.indexOf('?');
    if (index == -1) {
        index = oldURL.indexOf('#');
    }
    if (index != -1) {
        newURL = oldURL.substring(0, index);
    }
    var url = newURL + "/Reject_Reason_View/";
    $.get(url, { way_bill_id: way_bill_id }, function (data) {
        if (data != 0) {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content').html(data);
            $('.popup-background').addClass('popup-widget-overlay');
            $('.popup-popup-content').css({
                'display': 'block',
                'height': 'auto',
                'left': '43%',
                'position': 'absolute',
                'top': '40px',
                'width': '463px',
                'z-index': '999'
            });
        }
        else {
            alert("Way Bill Already Submitted");
            var form = 'Invalid Request';
        }
    });
}

/*Popup for View Recheck Reason*/
function view_recheck_reason(way_bill_id) {
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
    var oldURL = path;
    var index = 0;
    var newURL = oldURL;
    index = oldURL.indexOf('?');
    if (index == -1) {
        index = oldURL.indexOf('#');
    }
    if (index != -1) {
        newURL = oldURL.substring(0, index);
    }
    var url = newURL + "/Recheck_Reason_View/";
    $.get(url, { way_bill_id: way_bill_id }, function (data) {
        if (data != 0) {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content').html(data);
            $('.popup-background').addClass('popup-widget-overlay');
            $('.popup-popup-content').css({
                'display': 'block',
                'height': 'auto',
                'left': '43%',
                'position': 'absolute',
                'top': '40px',
                'width': '463px',
                'z-index': '999'
            });
        }
        else {
            alert("Way Bill Already Submitted");
            var form = 'Invalid Request';
        }
    });
}

function delete_Penalty_Goods(row,way_bill_id) {
    var i = row.parentNode.parentNode.rowIndex;
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
    var oldURL = path;
    var index = 0;
    var newURL = oldURL;
    index = oldURL.indexOf('?');
    if (index == -1) {
        index = oldURL.indexOf('#');
    }
    if (index != -1) {
        newURL = oldURL.substring(0, index);
    }
    var url = newURL + "/DeleteConfirmed";
    $.ajax({
        //url: '../Release/DeleteConfirmed',
        url: url,
        type: 'POST',
        data: JSON.stringify({ id: way_bill_id }),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            document.getElementById('tablepaging_release').deleteRow(i);
        }
    });
}

///*Popup for View In AC - Reverification*/
//function waybill_view_ac(id) {
//    var url = "../Accounting_Re_Verification/View_Way_Bill/";
//    $.get(url, { id: id }, function (data) {
//        if (data != 0) {
//            $("html, body").animate({ scrollTop: 0 }, 500);
//            $('.popup__content').html(data);
//            $('.popup-background').addClass('popup-widget-overlay');
//            $('.popup-popup-content').css({
//                'display': 'block',
//                'height': 'auto',
//                'left': '6%',
//                'position': 'absolute',
//                'top': '40px',
//                'width': '1625px',
//                'z-index': '999',
//                'padding-right': '40px'
//            });
//        }
//        else {
//            alert("Way Bill Already Submitted");
//            var form = 'Invalid Request';
//        }
//    });
//}

/*Popup for View In Compliance*/
function waybill_view_compliance(id) {
    var url = "../Compliance/View_Way_Bill/";
    $.get(url, { id: id }, function (data) {
        if (data != 0) {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content').html(data);
            $('.popup-background').addClass('popup-widget-overlay');
            $('.popup-popup-content').css({
                'display': 'block',
                'height': 'auto',
                'left': '6%',
                'position': 'absolute',
                'top': '40px',
                'width': '1625px',
                'z-index': '999',
                'padding-right': '40px'
            });
        }
        else {
            alert("Way Bill Already Submitted");
            var form = 'Invalid Request';
        }
    });
}

/*Add Tally Sheet Popup for User*/
function add_tally_sheet_popup() {
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
    var oldURL = path;
    var index = 0;
    var newURL = oldURL;
    index = oldURL.indexOf('?');
    if (index == -1) {
        index = oldURL.indexOf('#');
    }
    if (index != -1) {
        newURL = oldURL.substring(0, index);
    }
    var url = newURL + "/Create_Tally_Sheet";
    $.get(url, {}, function (data) {
        if (data != '') {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content').html(data);
            $('.popup-background').addClass('popup-widget-overlay');
            $('.popup-popup-content').css({
                'display': 'block',
                'height': 'auto',
                'left': '43%',
                'position': 'absolute',
                'top': '40px',
                'width': '463px',
                'z-index': '999'
            });
        }
        else {
            var form = 'Invalid Request';
        }
    });
}

/*Popup for delete*/
function addpopup_delete(id) {
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
    var oldURL = path;
    var index = 0;
    var newURL = oldURL;
    index = oldURL.indexOf('?');
    if (index == -1) {
        index = oldURL.indexOf('#');
    }
    if (index != -1) {
        newURL = oldURL.substring(0, index);
    }
    var url = newURL + "/Delete/";
    $.get(url, { id: id }, function (data) {
        if (data != 0) {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content').html(data);
            $('.popup__content').css({
                'padding': '25px 0px'
            });
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
            alert("Cannot Delete Last Entry");
            var form = 'Invalid Request';
        }
    });
}

///*Popup for  Warning about cargo manifest export*/
//function create_warning(id) {
//    var ship_departure_id = $('#ship_departure_id').val()
//    alert(ship_departure_id);
//    var url = "../Tally_Sheet_Export/Create_Warning";
//    $.post(url, { ship_departure_id: ship_departure_id }, function (data) {
//        if (data != 0) {
//            $("html, body").animate({ scrollTop: 0 }, 500);
//            $('.popup__content').html(data);
//            $('.popup__content').css({
//                'padding': '25px 0px'
//            });
//            $('.popup-background').addClass('popup-widget-overlay');
//            $('.popup-popup-content').css({
//                'display': 'block',
//                'height': 'auto',
//                'left': '43%',
//                'position': 'absolute',
//                'top': '150px',
//                'width': '463px',
//                'z-index': '999'
//            });
//        }
//        else {
//            alert("Cannot Delete Last Entry");
//            var form = 'Invalid Request';
//        }
//    });
//}

/*Popup for delete Way bill reject*/
function addpopup_delete_reject(id) {
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
    var oldURL = path;
    var index = 0;
    var newURL = oldURL;
    index = oldURL.indexOf('?');
    if (index == -1) {
        index = oldURL.indexOf('#');
    }
    if (index != -1) {
        newURL = oldURL.substring(0, index);
    }
    var url = newURL + "/Delete_Reject/";
    $.get(url, { id: id, count: count }, function (data) {
        if (data != 0) {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content').html(data);
            $('.popup__content').css({
                'padding': '25px 0px'
            });
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
            alert("Cannot Delete Last Entry");
            var form = 'Invalid Request';
        }
    });
}

/*Popup for edit*/
function addpopup_edit(id) {
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
    var oldURL = path;
    var index = 0;
    var newURL = oldURL;
    index = oldURL.indexOf('?');
    if (index == -1) {
        index = oldURL.indexOf('#');
    }
    if (index != -1) {
        newURL = oldURL.substring(0, index);
    }
    var url = newURL + "/Edit/";
    $.get(url, { id: id }, function (data) {
        if (data != 0) {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content').html(data);
            $('.popup-background').addClass('popup-widget-overlay');
            $('.popup-popup-content').css({
                'display': 'block',
                'height': 'auto',
                'left': '43%',
                'position': 'absolute',
                'top': '40px',
                'width': '463px',
                'z-index': '999'
            });
        }
        else {
            alert("Way Bill Already Submitted");
            var form = 'Invalid Request';
        }
    });
}

/*Popup for edit_reject in way bill*/
function addpopup_edit_reject(id) {
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
    var oldURL = path;
    var index = 0;
    var newURL = oldURL;
    index = oldURL.indexOf('?');
    if (index == -1) {
        index = oldURL.indexOf('#');
    }
    if (index != -1) {
        newURL = oldURL.substring(0, index);
    }
    var url = newURL + "/Edit_Reject/";
    $.get(url, { id: id }, function (data) {
        if (data != 0) {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content').html(data);
            $('.popup-background').addClass('popup-widget-overlay');
            $('.popup-popup-content').css({
                'display': 'block',
                'height': 'auto',
                'left': '43%',
                'position': 'absolute',
                'top': '40px',
                'width': '463px',
                'z-index': '999'
            });
        }
        else {
            alert("Way Bill Already Submitted");
            var form = 'Invalid Request';
        }
    });
}

/*Popup for my_profile*/
function addpopup_my_profile(id) {
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
    var oldURL = path;
    var index = 0;
    var newURL = oldURL;
    index = oldURL.indexOf('?');
    if (index == -1) {
        index = oldURL.indexOf('#');
    }
    if (index != -1) {
        newURL = oldURL.substring(0, index);
    }
    var url = newURL + "/../Manage_User/Edit/";
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

/*Popup for change password*/
function addpopup_change_password(id) {
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
    var oldURL = path;
    var index = 0;
    var newURL = oldURL;
    index = oldURL.indexOf('?');
    if (index == -1) {
        index = oldURL.indexOf('#');
    }
    if (index != -1) {
        newURL = oldURL.substring(0, index);
    }
    var url = newURL + "/../Manage_User/change_password/";
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
    var oldURL = path;
    var index = 0;
    var newURL = oldURL;
    index = oldURL.indexOf('?');
    if (index == -1) {
        index = oldURL.indexOf('#');
    }
    if (index != -1) {
        newURL = oldURL.substring(0, index);
    }
    var url = newURL + "/../Manage_User/change_user_name/";
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

/*Popup for DB Search*/
function addpopup_search() {
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
    var oldURL = path;
    var index = 0;
    var newURL = oldURL;
    index = oldURL.indexOf('?');
    if (index == -1) {
        index = oldURL.indexOf('#');
    }
    if (index != -1) {
        newURL = oldURL.substring(0, index);
    }
    var url = newURL + "/DbSearch";
    $.get(url, {}, function (data) {
        if (data != '') {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content').html(data);
            $('.popup-background').addClass('popup-widget-overlay');
            $('.popup-popup-content').css({
                'display': 'block',
                'height': 'auto',
                'left': '43%',
                'position': 'absolute',
                'top': '40px',
                'width': '463px',
                'z-index': '999'
            });
        }
        else {
            var form = 'Invalid Request';
        }
    });
}

/*Popup for DB Search Details Waybill*/
function addpopup_search_detail() {
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
    var oldURL = path;
    var index = 0;
    var newURL = oldURL;
    index = oldURL.indexOf('?');
    if (index == -1) {
        index = oldURL.indexOf('#');
    }
    if (index != -1) {
        newURL = oldURL.substring(0, index);
    }
    var url = newURL + "/DbSearch_new";
    $.get(url, { }, function (data) {
        if (data != '') {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content').html(data);
            $('.popup-background').addClass('popup-widget-overlay');
            $('.popup-popup-content').css({
                'display': 'block',
                'height': 'auto',
                'left': '43%',
                'position': 'absolute',
                'top': '40px',
                'width': '463px',
                'z-index': '999'
            });
        }
        else {
            var form = 'Invalid Request';
        }
    });
}

/*view exchange rate Popup*/
function addpopup_ViewExRate() {
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
    var oldURL = path;
    var index = 0;
    var newURL = oldURL;
    index = oldURL.indexOf('?');
    if (index == -1) {
        index = oldURL.indexOf('#');
    }
    if (index != -1) {
        newURL = oldURL.substring(0, index);
    }
    var url = newURL + "/ViewExchangeRate";
    $.get(url, {}, function (data) {
        if (data != '') {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content').html(data);
            $('.popup-background').addClass('popup-widget-overlay');
            $('.popup-popup-content').css({
                'display': 'block',
                'height': 'auto',
                'left': '43%',
                'position': 'absolute',
                'top': '40px',
                'width': '463px',
                'z-index': '999'
            });
        }
        else {
            var form = 'Invalid Request';
        }
    });
}

/*view payment config Popup*/
function payment_config_view() {
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
    var oldURL = path;
    var index = 0;
    var newURL = oldURL;
    index = oldURL.indexOf('?');
    if (index == -1) {
        index = oldURL.indexOf('#');
    }
    if (index != -1) {
        newURL = oldURL.substring(0, index);
    }
    var url = newURL + "/View_Payment_Config";
    $.get(url, {}, function (data) {
        if (data != '') {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content').html(data);
            $('.popup-background').addClass('popup-widget-overlay');
            $('.popup-popup-content').css({
                'display': 'block',
                'height': 'auto',
                'left': '43%',
                'position': 'absolute',
                'top': '40px',
                'width': '322px',
                'z-index': '999'
            });
        }
        else {
            var form = 'Invalid Request';
        }
    });
}

/*Popup for forgot password*/
function forgot_password() {
    var url = "../Home/Forgot_Password";
    $.get(url, {}, function (data) {
        if (data != '') {
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content').html(data);
            $('.popup-background').addClass('popup-widget-overlay');
            $('.popup-popup-content').css({
                'display': 'block',
                'height': 'auto',
                'left': '34%',
                'position': 'absolute',
                'top': '180px',
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

    $('.popup-background_error').removeClass('popup-widget-overlay');

    $('.popup-popup-content_error').css({
        'display': 'none'
    });
    $('.popup__content_error').html('');

    $('.popup-background_tax').removeClass('popup-widget-overlay');
    $('.popup__content_tax').css({
        'padding': '15px 0px 15px 0px'
    });
    $('.popup-popup-content_tax').css({
        'display': 'none'
    });
    $('.popup__content_tax').html('');
}

/*Close Popup Error*/
function closePopup_error() {
    document.getElementById("popup_release").style.opacity = 1;
    $('.popup-background_error').removeClass('popup-widget-overlay');
    $('.popup-background_error').removeClass('popup-widget-overlay_error');
    $('.popup-popup-content_error').css({
        'display': 'none'
    });
    $('.popup-more-content').css({
        'display': 'none'
    });
    $('.popup__content_error').html('');
    $('.more__content').html('');
}