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
                    'width': '330px',
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
});

function print_bolleto(id) {
    if ($('#controller').val() == "Compliance") {
        url = "../Compliance/Print_Bolleto/";
    }
    else if ($('#controller').val() == "Release") {
        url = "../Release/Print_Release/";
    }
    else {
        var url = "../Manifest_Control_Section/Print_Waybill";
    }
    $.get(url, { id : id }, function (data) {
        if (data != '') {
            var restorepage = document.body.innerHTML;
            var printcontent = data;
            document.body.innerHTML = printcontent;
            window.print();
            document.body.innerHTML = restorepage;
        }
        else {
            var form = 'Invalid Request';
        }
    });
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

/*Add Popup for User*/
function addpopup() {
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
    var url = path + "/Create";
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

/*Add Popup for Levi View*/
function levi_entry_view(id) {
    if ($('#identifier').val() == "Manage_Levy") {
        url = "../Manage_Levis/Details";
    }
    else {
        var pathstring = String(window.location);
        var patharray = pathstring.split("/");
        var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
        var url = path + "/Details";
    }
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
    var url = "../Manage_Tax_Calculation/Add_Goods_Price";
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

/*Add Popup for Tax Calculation Details View*/
function addpopup_view_tax_dxetails(way_bill_id) {
    var url = "";
    if ($('#controller').val() == "Accounting And Reverification") {
        url = "../Accounting_Re_Verification/Details/";
    }
    else if ($('#controller').val() == "Compliance") {
        url = "../Compliance/Details/";
    }
    else if ($('#controller').val() == "Release") {
        url = "../Release/Details/";
    }
    else if ($('#controller').val() == "Manage_Tax") {
        url = "../Manage_Tax_Calculation/Details/";
    }
    else if ($('#controller').val() == "Bolleto_Dogonale") {
        url = "../Manage_Bollete_Dogonale/Details/";
    }
    else if ($('#controller').val() == "Examination Unit") {
        url = "../Examination_Unit/Details/";
    }
    $.get(url, { way_bill_id: way_bill_id }, function (data) {
        if (data != '') {            
            $("html, body").animate({ scrollTop: 0 }, 500);
            $('.popup__content').html(data);
            $('.popup-background').addClass('popup-widget-overlay');
            if ($('#controller').val() == "Release") {
                $('.popup-popup-content').css({
                    'display': 'block',
                    'height': 'auto',
                    'left': '27%',
                    'position': 'absolute',
                    'top': '40px',
                    'width': '700px',
                    'z-index': '999',
                    'padding-right': '40px'
                });
            }
            else {
                $('.popup-popup-content').css({
                    'display': 'block',
                    'height': 'auto',
                    'left': '14%',
                    'position': 'absolute',
                    'top': '40px',
                    'width': '1000px',
                    'z-index': '999',
                    'padding-right': '40px'
                });
            }
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
    url = path + "/Create_Way_Bill";
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
    var url = "../Payment/Search_Way_Bill";
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
    var url = "../Release/Search_Way_Bill";
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


/*Popup for View In Way Bill*/
function waybill_view(id) {
   
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
    var url = path + "/View_Way_Bill/";
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
    var url = "";
    if ($('#controller').val() == "Accounting And Reverification") {
        url = "../Accounting_Re_Verification/Reject_Waybill/";
    }
    else if ($('#controller').val() == "Compliance") {
        url = "../Compliance/Reject_Waybill/";
    }
    else if ($('#controller').val() == "Examination Unit") {
        url = "../Examination_Unit/Reject_Waybill/";
    }
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
                'z-index': '999'
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
    var url = path + "/Reject_Reason_View/";
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
    var url = path + "/Create_Tally_Sheet";
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
    var url = path + "/Delete/";
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

/*Popup for delete Way bill reject*/
function addpopup_delete_reject(id) {
    var pathstring = String(window.location);
    var patharray = pathstring.split("/");
    var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
    var url = path + "/Delete_Reject/";
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
    if ($('#identifier').val() == "Manage_Levy") {
        url = "../Manage_Levis/Edit";
    }
    else {
        var pathstring = String(window.location);
        var patharray = pathstring.split("/");
        var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
        var url = path + "/Edit/";
    }
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
    var url = path + "/Edit_Reject/";
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

/*Popup for DB Search*/
function addpopup_search() {
    var url = "";
    if ($('#identifier').val() == "Manage_Levy") {
        url = "../Manage_Levis/DbSearch";
    }
    else {
        var pathstring = String(window.location);
        var patharray = pathstring.split("/");
        var path = patharray[0] + '//' + patharray[2] + '/' + patharray[3];
        url = path + "/DbSearch";
    }
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
    var url = path + "/DbSearch_new";
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
    var url = path + "/ViewExchangeRate";
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

/*Close Popup*/
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