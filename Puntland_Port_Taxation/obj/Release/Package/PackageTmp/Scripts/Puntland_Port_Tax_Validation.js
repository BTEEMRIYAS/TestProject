/*JavaScript Validation Code*/
function deptment_add() {
    if (formsubmit('', 'add_dept', [['required', 'department_name']], '')) {
        return true;
    }
    else {
        return false;
    }
}
function function_add() {
    if (formsubmit('', 'function', [['required', 'function_name']], '')) {
        return true;
    }
    else {
        return false;
    }
}
function role_map_add() {
    var x = 1;
    if ($(".check_box").is(":checked")) {
        $('#error_function_id').html('');
    }
    else {
        x = 0;
        $('#error_function_id').html('required');
    }
    if (formsubmit('', 'role_map', [['required', 'role_id']], '')) {
    }
    else {
        x = 0;
    }
    if (x == 0) {
        return false;
    }
}
function unit_add() {
    if (formsubmit('', 'add_unit', [['required', 'unit_name', 'unit_code']], '')) {
        return true;
    }
    else {
        return false;
    }
}
function payment_config_add() {
    var x = 1;
    var sos = parseFloat($('#sos').val());
    var usd = parseFloat($('#usd').val());
    var total = sos + usd;
    if (formsubmit('', 'payment_config', [['required', 'sos', 'usd'], ['tax_rate', 'sos', 'usd']], '')) {
    }
    else {
        x = 0;
    }
    if (total != 100) {
        $('#error_usd').html('Total should be 100');
        x = 0;
    }
    else {
        $('#error_usd').html('');
    }
    if (x == 0) {
        return false;
    }
    else {
        return true;
    }
}
function payment_details() {
    var x = 1;
    var sos_by_cash = parseFloat($('#sos_by_cash').val());
    var sos_by_cheque = parseFloat($('#sos_by_cheque').val());
    var usd_by_cash = parseFloat($('#usd_by_cash').val());
    var usd_by_cheque = parseFloat($('#usd_by_cheque').val());
    var sos_total = sos_by_cash + sos_by_cheque;
    var sos_amount = $('#sos_part_total').val();
    var usd_total = usd_by_cash + usd_by_cheque;
    var usd_amount = $('#usd_part_total').val();
    if (sos_total != sos_amount) {
        $('#error_sos_by_cheque').html('SOS(Cash + Cheque != Amount)');
        x = 0;
    }
    else {
        $('#error_sos_by_cheque').html('');
    }
    if (usd_total != usd_amount) {
        $('#error_usd_by_cheque').html('USD(Cash + Cheque != Amount)');
        x = 0;
    }
    else {
        $('#error_usd_by_cheque').html('');
    }
    if (formsubmit('', 'payment', [['required', 'currency_id_tobe_paid']], '')) {
    }
    else {
        x = 0;
    }
    if (x == 0) {
        return false;
    }
    else {
        return true;
    }
}
function goods_add() {
    var x = 1;
    var old = $('#old_tariff').val();
    var new_val = parseFloat($('#goods_tariff').val()).toFixed(2);
    if (old != new_val) {
        if ($('#upload_document').val() == '') {
            $('#error_upload_document').html('required');
            x = 0;
        }
    }
    if (formsubmit('', 'goods', [['required', 'goods_category_id', 'goods_subcategory_id', 'goods_type_id', 'goods_code', 'goods_name', 'goods_tariff', 'currency_id', 'ispercentage', 'unit_of_measure_id'], ['tax_rate', 'goods_tariff']], '')) {
    }
    else {
        x = 0;
    }
    if (x == 0) {
        return false;
    }
    else {
        return true;
    }
}
function goods_price_add() {
    var x = 1;
    $('.sum_decimal').each(function () {      
        if ($(this).val() != '') {
            if (/^[\d,]+(\.\d{1,2})?$/.test($(this).val())) {
                $(this).parent().find('.error_goods_price').html('');
            }
            else {
                x = 0;
                $(this).parent().find('.error_goods_price').html('Enter a valid decimal');
            }
        }
        else {
            x = 0;
            $(this).parent().find('.error_goods_price').html('Required');          
        }
    });
    $('.sum_dropdown').each(function () {

        if ($(this).val() != '') {
            $(this).parent().find('.error_goods_price').html('');
        }
        else {
            x = 0;
            $(this).parent().find('.error_goods_price').html('Required');
        }
    });
    if (x == 0) {
        return false;
    }
}

function goods_price_add_penalty() {
    var x = 1;
    if ($('#penalty').val() != '') {
        if (/^[1-9]\d*$/i.test($('#penalty').val())) {
            $('.error_penalty').html('');
        }
        else {
            x = 0;
            $('.error_penalty').html('Enter a valid decimal');
        }
    }
    else {
        x = 0;
        $('.error_penalty').html('Required');
    }
    $('.sum_decimal').each(function () {
        if ($(this).val() != '') {
            if (/^[\d,]+(\.\d{1,2})?$/.test($(this).val())) {
                $(this).parent().find('.error_goods_price').html('');
            }
            else {
                x = 0;
                $(this).parent().find('.error_goods_price').html('Enter a valid decimal');
            }
        }
        else {
            x = 0;
            $(this).parent().find('.error_goods_price').html('Required');
        }
    });
    $('.sum_dropdown').each(function () {

        if ($(this).val() != '') {
            $(this).parent().find('.error_goods_price').html('');
        }
        else {
            x = 0;
            $(this).parent().find('.error_goods_price').html('Required');
        }
    });
    if (x == 0) {
        return false;
    }
}

function assign_way_bill_add() {
    if (formsubmit('', 'assign_way_bill', [['required', 'user_id']], '')) {
        return true;
    }
    else {
        return false;
    }
}
function category_add() {
    if (formsubmit('', 'category', [['required', 'goods_category_code', 'goods_category_name']], '')) {
        return true;
    }
    else {
        return false;
    }
}
function subcategory_add() {
    if (formsubmit('', 'subcategory', [['required', 'goods_category_id', 'goods_subcategory_code', 'goods_subcategory_name']], '')) {
        return true;
    }
    else {
        return false;
    }
}
function type_add() {
    if (formsubmit('', 'goods_type', [['required', 'goods_category_id', 'goods_subcategory_id', 'goods_type_code', 'goods_type_name']], '')) {
        return true;
    }
    else {
        return false;
    }
}
function role_add() {
    if (formsubmit('', 'role', [['required', 'role_name']], '')) {
        return true;
    }
    else {
        return false;
    }
}
function user_add() {
    if (formsubmit('', 'user', [['required', 'first_name', 'middle_name', 'last_name', 'email_id', 'department_id', 'role_id'], ['first_name', 'first_name', 'middle_name', 'last_name'], ['email_id', 'email_id']], '')) {
        return true;
    }
    else {
        return false;
    }
}
function user_add_login() {
    if (formsubmit('', 'user', [['required', 'first_name', 'middle_name', 'last_name', 'email_id'], ['first_name', 'first_name', 'middle_name', 'last_name'], ['email_id', 'email_id']], '')) {
        return true;
    }
    else {
        return false;
    }
}
function userReg_change_password() {
    var old = $('#old_password').val();
    if (old == '') {
        $('#error_old_password').html('required');
    }
    if (formsubmit('', 'user_change_password', [['required', 'new_password', 'confirm_password'], ['old_password', 'new_password', 'confirm_password'], ['equal', ['new_password', 'confirm_password']]], '')) {
        if (document.getElementById('error_old_password').innerHTML == "Incorrect Password" || document.getElementById('error_old_password').innerHTML == "required") {
            return false;
        }
        else {
            return true;
        }
    }
    else {
        return false;
    }
}
function check_old_password() {
    var old = $('#old_password').val();
    var url = "../Manage_User/check_password/";
    $.get(url, { old: old }, function (data) {
        if (data == 0) {
            $('#error_old_password').html('Incorrect Password');
        }
        else {
            $('#error_old_password').html('');
        }
    });
}
function userReg_change_user_name() {
    var old = $('#old_user_name').val();
    if (old == '') {
        $('#error_old_user_name').html('required');
    }
    if (formsubmit('', 'user_change_user_name', [['required', 'new_user_name', 'confirm_user_name'], ['equal', ['new_user_name', 'confirm_user_name']]], '')) {
        if (document.getElementById('error_old_user_name').innerHTML == "Incorrect User Name" || document.getElementById('error_old_user_name').innerHTML == "required") {
            return false;
        }
        else {
            return true;
        }
        }
    else {
        return false;
    }
}
function check_old_user_name() {
    var old = $('#old_user_name').val();
    var url = "../Manage_User/check_user_name/";
    $.get(url, { old: old }, function (data) {
        if (data == 0) {
            $('#error_old_user_name').html('Incorrect User Name');
        }
        else {
            $('#error_old_user_name').html('');
        }
    });
}
function forgot_password_validation() {
    if (formsubmit('', 'password_forgot', [['required', 'user_name', 'email_id'], ['email_id', 'email_id']], '')) {
        return true;
    }
    else {
        return false;
    }
}
function ship_add() {
    if (formsubmit('', 'ship', [['required', 'ship_name', 'ship_code_1']], '')) {
        return true;
    }
    else {
        return false;
    }
}
function ship_arrival_add() {
    if (formsubmit('', 'ship_arrival', [['required', 'shipp_id', 'geography_id', 'country_id', 'state_id', 'day_code'], ['date_of_birth', 'day_code']], '')) {
        return true;
    }
    else {
        return false;
    }
}
function importer_add() {
    if (formsubmit('', 'importer', [['required', 'importer_first_name', 'importer_middle_name', 'importer_last_name', 'importer_mob_no', 'importer_type_id', 'importer_country_id'], ['first_name', 'importer_first_name', 'importer_middle_name', 'importer_last_name'], ['email_id', 'importer_email_id'], ['MobileNo', 'importer_mob_no']], '')) {
        return true;
    }
    else {
        return false;
    }
}
function import_add() {
    if (formsubmit('', 'import', [['required', 'importer_id', 'ship_arrival_id']], '')) {
        return true;
    }
    else {
        return false;
    }
}
function export_add() {
    if (formsubmit('', 'export', [['required', 'exporter_id', 'ship_departure_id']], '')) {
        return true;
    }
    else {
        return false;
    }
}
function importer_type_add() {
    if (formsubmit('', 'importer_type', [['required', 'importer_type_name']], '')) {
        return true;
    }
    else {
        return false;
    }
}
function country_add() {
    if (formsubmit('', 'country', [['required', 'geography_id', 'country_name']], '')) {
        return true;
    }
    else {
        return false;
    }
}
function tally_sheet_add() {
    if (formsubmit('', 'tally_sheet', [['required', 'ship_arrival_id']], '')) {
        return true;
    }
    else {
        return false;
    }
}
function e_tally_sheet_add() {
    var ship_departure_id = $('#ship_departure_id').val();
    if (ship_departure_id != '') {
        if(confirm("Do you really want to mark the ship for departure"))
        {
            return true;
        }
        else {
            closePopup();
        }
    }
    else {
        $('#error_ship_departure_id').html('required');
        return false;
    }
}
function create_tally_sheet_add() {
    if (formsubmit('', 'create_tally_sheet', [['required', 'way_bill_code', 'importer_name', 'goods_category_id', 'goods_subcategory_id', 'goods_type_id', 'goods', 'total_quantity', 'unit_of_measure'], ['positive_integer', 'total_quantity']], '')) {
        return true;
    }
    else {
        return false;
    }
}
    function edit_tally_sheet_add() {
        if (formsubmit('', 'edit_tally_sheet', [['required', 'tally_sheet_code', 'ship_arrival_id', 'way_bill_code', 'goods', 'unit_of_measure_id', 'total_quantity'], ['positive_integer', 'total_quantity']], '')) {
            return true;
        }
        else {
            return false;
        }
    }
    function way_bill_add() {
        if (formsubmit('', 'way_bill', [['required', 'way_bill_code', 'import_id', 'mark']], '')) {
            return true;
        }
        else {
            return false;
        }
    }

    function e_way_bill_add() {
        if (formsubmit('', 'way_bill', [['required', 'e_way_bill_code', 'export_id', 'mark']], '')) {
            return true;
        }
        else {
            return false;
        }
    }

    function Add_Penalty_Goods() {
        var count = $('#tablepaging_release tr').length;
        var is_damaged = "";
        var goods = $("#goods_id option:selected").text();
        var quantity = $('#total_quantity').val();
        var unit_of_measure = $("#unit_of_measure_id option:selected").text();
        var is_damaged_true = document.getElementById('is_damaged').checked;
        var is_damaged_false = document.getElementById('is_damaged_false').checked;
        if (is_damaged_true == true) {
            is_damaged = "Yes";
        }
        else {
            is_damaged = "No";
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
        var url = newURL + "/Save_Penaltized_Goods";
        if (formsubmit('', 'create_way_bill', [['required', 'goods_category_id', 'goods_subcategory_id', 'goods_type_id', 'goods_id', 'unit_of_measure_id', 'total_quantity'], ['positive_integer', 'total_quantity']], '')) {
            var str = $("form").serialize();
            $.ajax({
                //url: '../Release/Save_Penaltized_Goods',
                url: url,
                type: 'POST',
                data: JSON.stringify({ obj: str }),
                dataType: 'json',
                contentType: 'application/json',
                success: function (data) {
                    var html = '<tr class="damaged_goods">' +
                                '<td class="left_border">' + goods + '</td>' +
                                '<td>' + quantity + '</td>' +
                                '<td>' + unit_of_measure + '</td>' +
                                '<td>' + is_damaged + '</td>' +
                                '<td><a href= "javascript:void(0);" onclick="delete_Penalty_Goods(this,'+ data +')"><div class="delete_icon"></div></a></td>' +
                                '</tr>'
                    $(html).appendTo($("#tablepaging_release"));
                    $('#reject_penalty').show();
                    closePopup_error();
                }
            });
        }
        else {
            return false;
        }
    }


    function penalty_goods_add(way_bill_id) {
        document.getElementById("popup_release").style.opacity = 0.6;
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
        $.get(url, { way_bill_id: way_bill_id }, function (data) {
            if (data != '') {
                $("html, body").animate({ scrollTop: 0 }, 500);
                $('.popup__content_error').html(data);
                $('.popup-background_error').addClass('popup-widget-overlay_error');
                $('.popup-popup-content_error').css({
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

    function create_way_bill_add() {
        if (formsubmit('', 'create_way_bill', [['required', 'goods_category_id', 'goods_subcategory_id', 'goods_type_id', 'goods_id', 'unit_of_measure_id', 'total_quantity'], ['positive_integer', 'total_quantity']], '')) {
            var str = $("form").serialize();
            $.ajax({
                url: '../Manage_Way_Bill/Create_Way_Bill_new',
                type: 'POST',
                //data: JSON.stringify({ id: id }),
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ obj: str }),
                success: function (data) {
                    $('#goods_category_id').val('');
                    $('#goods_subcategory_id').val('');
                    $('#goods_type_id').val('');
                    $('#goods_id').val('');
                    $('#total_quantity').val('');
                    $('select#unit_of_measure_id').html('<option value="">Select Unit Of Measure</option>');
                    document.getElementById("is_damaged_false").checked = true;
                    alert("Goods Added Successfully");
                }
            });
        }
        else {
            return false;
        }
    }

    function create_e_way_bill_add() {
        if (formsubmit('', 'create_way_bill', [['required', 'goods_category_id', 'goods_subcategory_id', 'goods_type_id', 'goods_id', 'unit_of_measure_id', 'total_quantity'], ['positive_integer', 'total_quantity']], '')) {
            var str = $("form").serialize();
            $.ajax({
                url: '../Way_Bill_Export/Create_Way_Bill_new',
                type: 'POST',
                //data: JSON.stringify({ id: id }),
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ obj: str }),
                success: function (data) {
                    $('#goods_category_id').val('');
                    $('#goods_subcategory_id').val('');
                    $('#goods_type_id').val('');
                    $('#goods_id').val('');
                    $('#total_quantity').val('');
                    $('select#unit_of_measure_id').html('<option value="">Select Unit Of Measure</option>');
                    document.getElementById("is_damaged_false").checked = true;
                    alert("Goods Added Successfully");
                }
            });
        }
        else {
            return false;
        }
    }

    function create_way_bill_submit() {
        if (formsubmit('', 'create_way_bill', [['required', 'goods_category_id', 'goods_subcategory_id', 'goods_type_id', 'goods_id', 'unit_of_measure_id', 'total_quantity'], ['positive_integer', 'total_quantity']], '')) {
            return true;
        }
        else {
            return false;
        }
    }
    function way_bill_edit() {
        if (formsubmit('', 'edit_way_bill', [['required', 'way_bill_code', 'import_id', 'mark', 'goods_category_id', 'goods_subcategory_id', 'goods_type_id', 'goods_id', 'unit_of_measure_id', 'total_quantity'], ['positive_integer', 'total_quantity']], '')) {
            return true;
        }
        else {
            return false;
        }
    }
    function tax_calculation_add() {
        if (formsubmit('', 'tax_calculation', [['required', 'way_bill_id', 'import_id', 'goods_id', 'quantity', 'unit_of_measure', 'price', 'currency_id']], '')) {
            return true;
        }
        else {
            return false;
        }
    }
    function tax_calculation_add_new() {
        if (formsubmit('', 'tax_calculation', [['required', 'way_bill_id']], '')) {
            return true;
        }
        else {
            return false;
        }
    }
    function bolleto_dogonale_add() {
        if (formsubmit('', 'bolleto_dogonale', [['required', 'way_bill_id', 'import_id', 'importer_name', 'ship_name', 'ship_arrival_date', 'goods_id', 'quantity', 'unit_of_measure', 'price', 'currency_id']], '')) {
            return true;
        }
        else {
            return false;
        }
    }
    function levi_entry_add() {
        var x = 1;
        var old = $('#old_levy').val();
        var new_val = parseFloat($('#levi').val()).toFixed(2);
        if ($('#create_edit').val() == "edit") {
            if (old != new_val) {
                if ($('#upload_document').val() == '') {
                    $('#error_upload_document').html('required');
                    x = 0;
                }
            }
        }
        else if ($('#create_edit').val() == "create") {
            if ($('#upload_document').val() == '') {
                $('#error_upload_document').html('required');
                x = 0;
            }
        }
        if (formsubmit('', 'levi_entry', [['required', 'levi_type_id', 'levi_name', 'levi', 'currency_id'], ['levi_rate', 'levi']], '')) {
        }
        else {
            x = 0;
        }
        if (x == 0) {
            return false;
        }
        else {
            return true;
        }
    }
    function reject_reason() {
        if (formsubmit('', 'reject', [['required', 'reason']], '')) {
            return true;
        }
        else {
            return false;
        }
    }
    function recheck_reason() {
        if (formsubmit('', 'recheck', [['required', 'reason', 'rechecked_by']], '')) {
            return true;
        }
        else {
            return false;
        }
    }
    function view_tax_details() {
        if ($('#currency_id').val() == '') {
            $('#error_currency_id').html('required');
            return false;
        }
        else {
            return true;
        }
    }
    function state_add() {
        if (formsubmit('', 'state', [['required', 'geography_id', 'country_id', 'state_name']], '')) {
            return true;
        }
        else {
            return false;
        }
    }
    function geography_add() {
        if (formsubmit('', 'geography', [['required', 'geography_name']], '')) {
            return true;
        }
        else {
            return false;
        }
    }

    function checkrequired(id) {
        var fname = $('#' + id).val();

        if (fname == "") {
            $('#' + id).addClass('input__error');
            return true;
        } else {
            $('#' + id).removeClass('input__error');
            return false;
        }
    };

    function checkemail(id) {
        var nameRegex = /^([a-z0-9\+_\-]+)(\.[a-z0-9\+_\-]+)*@([a-z0-9\-]+\.)+[a-z]{2,6}$/i;
        var fname = document.getElementById("name").value;
        var fname = $('#' + id).val();

        if (fname == "") {
            $('#' + id).addClass('input__error');
            return true;
        } else if (!(nameRegex.test(fname))) {
            $('#' + id).addClass('input__error');
            return true;
        } else {
            $('#' + id).removeClass('input__error');
            return false;
        }
    };



    /*Entire Form Validation*/

    function formsubmit(NowBlock, formName, reqFieldArr, nextAction) {
        var curForm = new formObj(NowBlock, formName, reqFieldArr, nextAction);
        if (curForm.valid) {
            return true;
        }

        else {
            return false;
            curForm.paint();
            curForm.listen();
        }
    }

    function formObj(NowBlock, formName, reqFieldArr, nextAction) {

        var filledCount = 0;
        var fieldArr = new Array();
        var k = 0;
        this.formNM = formName;

        /*if(document.forms[this.formNM].elements['submit_tp'].value == '1ax')
        {
            this.nextaction = nextAction;
            this.now = NowBlock;
        }*/

        for (i = 0; i < reqFieldArr.length; i++) {
            if (reqFieldArr[i][0] == 'required') {
                for (j = reqFieldArr[i].length - 1; j >= 1; j--) {
                    fieldArr[k] = new fieldObj(this.formNM, reqFieldArr[i][j], 'required');
                    if (fieldArr[k].filled == true) {
                        filledCount++;
                    }
                    k++;
                }
            }
            if (reqFieldArr[i][0] == 'email_id') {
                for (j = reqFieldArr[i].length - 1; j >= 1; j--) {
                    fieldArr[k] = new fieldObj(this.formNM, reqFieldArr[i][j], 'email_id');
                    if (fieldArr[k].filled == true) {
                        filledCount++;
                    }
                    k++;
                }
            }
            if (reqFieldArr[i][0] == 'username') {
                for (j = reqFieldArr[i].length - 1; j >= 1; j--) {
                    fieldArr[k] = new fieldObj(this.formNM, reqFieldArr[i][j], 'username');
                    if (fieldArr[k].filled == true) {
                        filledCount++;
                    }
                    k++;
                }
            }
            if (reqFieldArr[i][0] == 'old_password') {
                for (j = reqFieldArr[i].length - 1; j >= 1; j--) {
                    fieldArr[k] = new fieldObj(this.formNM, reqFieldArr[i][j], 'old_password');
                    if (fieldArr[k].filled == true) {
                        filledCount++;
                    }
                    k++;
                }
            }
            if (reqFieldArr[i][0] == 'MobileNo') {
                for (j = reqFieldArr[i].length - 1; j >= 1; j--) {
                    fieldArr[k] = new fieldObj(this.formNM, reqFieldArr[i][j], 'MobileNo');
                    if (fieldArr[k].filled == true) {
                        filledCount++;
                    }
                    k++;
                }
            }
            if (reqFieldArr[i][0] == 'Pin') {
                for (j = reqFieldArr[i].length - 1; j >= 1; j--) {
                    fieldArr[k] = new fieldObj(this.formNM, reqFieldArr[i][j], 'Pin');
                    if (fieldArr[k].filled == true) {
                        filledCount++;
                    }
                    k++;
                }
            }
            if (reqFieldArr[i][0] == 'positive_integer') {
                for (j = reqFieldArr[i].length - 1; j >= 1; j--) {
                    fieldArr[k] = new fieldObj(this.formNM, reqFieldArr[i][j], 'positive_integer');
                    if (fieldArr[k].filled == true) {
                        filledCount++;
                    }
                    k++;
                }
            }
            if (reqFieldArr[i][0] == 'first_name') {
                for (j = reqFieldArr[i].length - 1; j >= 1; j--) {
                    fieldArr[k] = new fieldObj(this.formNM, reqFieldArr[i][j], 'first_name');
                    if (fieldArr[k].filled == true) {
                        filledCount++;
                    }
                    k++;
                }
            }
            if (reqFieldArr[i][0] == 'given_names') {
                for (j = reqFieldArr[i].length - 1; j >= 1; j--) {
                    fieldArr[k] = new fieldObj(this.formNM, reqFieldArr[i][j], 'given_names');
                    if (fieldArr[k].filled == true) {
                        filledCount++;
                    }
                    k++;
                }
            }
            if (reqFieldArr[i][0] == 'equal') {
                for (j = reqFieldArr[i].length - 1; j >= 1; j--) {
                    fieldArr[k] = new fieldObj(this.formNM, reqFieldArr[i][j], 'equal');
                    if (fieldArr[k].filled == true) {
                        filledCount++;
                    }
                    k++;
                }
            }
            if (reqFieldArr[i][0] == 'notequal') {
                for (j = reqFieldArr[i].length - 1; j >= 1; j--) {
                    fieldArr[k] = new fieldObj(this.formNM, reqFieldArr[i][j], 'notequal');
                    if (fieldArr[k].filled == true) {
                        filledCount++;
                    }
                    k++;
                }
            }
            if (reqFieldArr[i][0] == 'tax_rate') {
                for (j = reqFieldArr[i].length - 1; j >= 1; j--) {
                    fieldArr[k] = new fieldObj(this.formNM, reqFieldArr[i][j], 'tax_rate');
                    if (fieldArr[k].filled == true) {
                        filledCount++;
                    }
                    k++;
                }
            }
            if (reqFieldArr[i][0] == 'levi_rate') {
                for (j = reqFieldArr[i].length - 1; j >= 1; j--) {
                    fieldArr[k] = new fieldObj(this.formNM, reqFieldArr[i][j], 'levi_rate');
                    if (fieldArr[k].filled == true) {
                        filledCount++;
                    }
                    k++;
                }
            }
            if (reqFieldArr[i][0] == 'date_of_birth') {
                for (j = reqFieldArr[i].length - 1; j >= 1; j--) {
                    fieldArr[k] = new fieldObj(this.formNM, reqFieldArr[i][j], 'date_of_birth');
                    if (fieldArr[k].filled == true) {
                        filledCount++;
                    }
                    k++;
                }
            }
        }
        if (filledCount == fieldArr.length) {
            this.valid = true;
        }
        else {
            this.valid = false;
        }


        this.paint = function () {
            for (i = fieldArr.length - 1; i >= 0; i--) {
                if (fieldArr[i].filled == false)
                    fieldArr[i].paintInRed();
                else
                    fieldArr[i].unPaintInRed();
            }
        }
        this.listen = function () {
            for (i = fieldArr.length - 1; i >= 0; i--) {
                fieldArr[i].fieldListen();
            }
        }
    }

    formObj.prototype.send = function () {
        if (document.forms[this.formNM].elements['submit_tp'].value == '1ax') {
            var to = document.forms[this.formNM].elements['submit_action'].value;
            var tofunction = document.forms[this.formNM].elements['submit_fn'].value;
            var now = this.now;
            var next = this.nextaction;

            var str = $('#' + this.formNM).serialize();

            var url = path + "index.php/" + to + "/" + tofunction;
            $.post(url, { fieldval: str }, function (data) {
                if (data == 'next') {
                    if (next != 'none') {
                        document.getElementById(now).style.display = "none";
                        document.getElementById(next).style.display = "block";
                        return true;
                    }
                }
                else {
                    //document.getElementById('set_notset').value = 'notset';
                }
            });
        }

        if (document.getElementById('submit_tp').value == '') {
            document.forms[this.formNM].submit();
            return true;
        }
    };

    function fieldObj(formName, fName, typeOchk) {

        if (typeOchk != 'equal' && typeOchk != 'notequal') {
            var curField = document.forms[formName].elements[fName];
        }
        this.filled = getValueBool(typeOchk);

        this.paintInRed = function () {
            //document.getElementById('error_'+fName).innerHTML = 'required';
            //curField.addClassName('red');		
        }

        this.unPaintInRed = function () {
            //curField.removeClassName('red');
            //document.getElementById('error_'+fName).innerHTML = '';
        }

        this.fieldListen = function () {
            curField.onkeyup = function () {
                if (curField.value != '') {
                    curField.removeClassName('red');
                }
                else {
                    curField.addClassName('red');
                }
            }
        }

        function getValueBool(type) {
            if (type == 'required') {
                if ($.trim(curField.value) != '') {
                    document.getElementById('error_' + fName).innerHTML = '';
                    return true;
                }
                else {
                    var chkvalue = document.getElementById('error_' + fName).innerHTML.trim();
                    if (chkvalue == '') {
                        document.getElementById('error_' + fName).innerHTML = 'Required';
                        fail_border(fName);
                    }
                    return false;
                }
            }
            if (type == 'email_id') {
                if ($.trim(curField.value) != '') {
                    if (/^([a-z0-9\+_\-]+)(\.[a-z0-9\+_\-]+)*@([a-z0-9\-]+\.)+[a-z]{2,6}$/i.test(curField.value)) {
                        document.getElementById('error_' + fName).innerHTML = '';
                        return true;
                    }
                    else {
                        var chkvalue = document.getElementById('error_' + fName).innerHTML.trim();

                        if (chkvalue == '') {
                            document.getElementById('error_' + fName).innerHTML = 'Enter a valid email';
                            fail_border(fName);
                        }
                        return false;
                    }
                }
                else {
                    return true;
                }
            }
            if (type == 'username') {
                if ($.trim(curField.value) != '') {
                    if (curField.value.length >= 6 && curField.value.length < 26) {
                        if (/^[a-zA-Z0-9\_\.]+$/i.test(curField.value)) {
                            document.getElementById('error_' + fName).innerHTML = '';
                            return true;
                        }
                        else {
                            var chkvalue = document.getElementById('error_' + fName).innerHTML.trim();

                            if (chkvalue == '') {
                                document.getElementById('error_' + fName).innerHTML = 'you can use only letters (a-z), numbers, and periods';
                                fail_border(fName);
                            }
                            return false;
                        }
                    }
                    else {
                        var chkvalue = document.getElementById('error_' + fName).innerHTML.trim();

                        if (chkvalue == '') {
                            document.getElementById('error_' + fName).innerHTML = 'Please use between 6 and 25 characters.';
                            fail_border(fName);
                        }
                        return false;
                    }
                }
                else {
                    return true;
                }
            }
            if (type == 'old_password') {
                if ($.trim(curField.value) != '') {
                    if (curField.value.length >= 6) {
                        document.getElementById('error_' + fName).innerHTML = '';
                        return true;
                    }
                    else {
                        var chkvalue = document.getElementById('error_' + fName).innerHTML.trim();

                        if (chkvalue == '') {
                            document.getElementById('error_' + fName).innerHTML = 'At least 6 characters long';
                            fail_border(fName);
                        }
                        return false;
                    }
                }
                else {
                    return true;
                }
            }
            if (type == 'MobileNo') {
                if ($.trim(curField.value) != '') {
                    if (/^[0-9]{7,10}$/i.test(curField.value)) {
                        document.getElementById('error_' + fName).innerHTML = '';
                        return true;
                    }
                    else {
                        var chkvalue = document.getElementById('error_' + fName).innerHTML.trim();

                        if (chkvalue == '') {
                            document.getElementById('error_' + fName).innerHTML = 'Enter a valid mobile number';
                            fail_border(fName);
                        }
                        return false;
                    }
                }
                else {
                    return true;
                }
            }
            if (type == 'positive_integer') {
                if ($.trim(curField.value) != '') {
                    if (/^[1-9]\d*$/i.test(curField.value)) {
                        document.getElementById('error_' + fName).innerHTML = '';
                        return true;
                    }
                    else {
                        var chkvalue = document.getElementById('error_' + fName).innerHTML.trim();

                        if (chkvalue == '') {
                            document.getElementById('error_' + fName).innerHTML = 'Enter positive number';
                            fail_border(fName);
                        }
                        return false;
                    }
                }
                else {
                    return true;
                }
            }
            if (type == 'positive_integer') {
                if ($.trim(curField.value) != '') {
                    if (/^[1-9]\d*$/i.test(curField.value)) {
                        document.getElementById('error_' + fName).innerHTML = '';
                        return true;
                    }
                    else {
                        var chkvalue = document.getElementById('error_' + fName).innerHTML.trim();

                        if (chkvalue == '') {
                            document.getElementById('error_' + fName).innerHTML = 'Enter a valid number';
                            fail_border(fName);
                        }
                        return false;
                    }
                }
                else {
                    return true;
                }
            }
            if (type == 'first_name') {
                if ($.trim(curField.value) != '') {
                    if (/^\s*[a-zA-Z-/]+\s*$/i.test(curField.value)) {
                        document.getElementById('error_' + fName).innerHTML = '';
                        return true;
                    }
                    else {
                        var chkvalue = document.getElementById('error_' + fName).innerHTML.trim();

                        if (chkvalue == '') {
                            document.getElementById('error_' + fName).innerHTML = 'Should be an alphabet';
                            fail_border(fName);
                        }
                        return false;
                    }
                }
                else {
                    return true;
                }
            }
            if (type == 'given_names') {
                if ($.trim(curField.value) != '') {
                    if (/^[a-zA-Z ]+$/i.test(curField.value)) {
                        document.getElementById('error_' + fName).innerHTML = '';
                        return true;
                    }
                    else {
                        var chkvalue = document.getElementById('error_' + fName).innerHTML.trim();

                        if (chkvalue == '') {
                            document.getElementById('error_' + fName).innerHTML = 'Should be an alphabet';
                            fail_border(fName);
                        }
                        return false;
                    }
                }
                else {
                    return true;
                }
            }
            if (type == 'equal') {

                FfName = fName[0];
                LfName = fName[1];
                var FcurField = document.forms[formName].elements[FfName];
                var LcurField = document.forms[formName].elements[LfName];
                if ($.trim(FcurField.value) != '') {
                    if ($.trim(FcurField.value) == $.trim(LcurField.value)) {
                        document.getElementById('error_' + LfName).innerHTML = '';
                        return true;
                    }
                    else {
                        var chkvalue = document.getElementById('error_' + LfName).innerHTML.trim();
                        if (chkvalue == '') {
                            document.getElementById('error_' + LfName).innerHTML = 'Sholud be same';
                            fail_border(LfName);
                        }
                        return false;
                    }
                }
                else {
                    return true;
                }
            }
            if (type == 'notequal') {
                var FfName = fName[0];
                var Fvalue = document.forms[formName].elements[FfName].value;
                var Tvalue = fName[1];

                if (Fvalue != Tvalue) {
                    elem = document.forms[formName].elements[FfName].setAttribute("style", "border: 1px solid #ccc; box-shadow: none;");
                    return true;
                }
                else {
                    elem = document.forms[formName].elements[FfName].setAttribute("style", "border: 1px solid #EF672F; box-shadow: 0 0 2px #EF672F;");
                    return false;
                }
            }
            if (type == 'tax_rate') {
                if ($.trim(curField.value) != '') {
                    if (/^[\d,]+(\.\d{1,2})?$/.test(curField.value)) {
                        document.getElementById('error_' + fName).innerHTML = '';
                        return true;
                    }
                    else {
                        var chkvalue = document.getElementById('error_' + fName).innerHTML.trim();

                        if (chkvalue == '') {
                            document.getElementById('error_' + fName).innerHTML = 'Enter a valid decimal value';
                            fail_border(fName);
                        }
                        return false;
                    }
                }
                else {
                    return true;
                }
            }
            if (type == 'levi_rate') {
                if ($.trim(curField.value) != '') {
                    if (/^[\d,]+(\.\d{1,4})?$/.test(curField.value)) {
                        document.getElementById('error_' + fName).innerHTML = '';
                        return true;
                    }
                    else {
                        var chkvalue = document.getElementById('error_' + fName).innerHTML.trim();

                        if (chkvalue == '') {
                            document.getElementById('error_' + fName).innerHTML = 'Enter a valid decimal value';
                            fail_border(fName);
                        }
                        return false;
                    }
                }
                else {
                    return true;
                }
            }
            if (type == 'date_of_birth') {
                if ($.trim(curField.value) != '') {
                    if (/^[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])$/.test(curField.value)) {
                        document.getElementById('error_' + fName).innerHTML = '';
                        return true;
                    }
                    else {
                        var chkvalue = document.getElementById('error_' + fName).innerHTML.trim();

                        if (chkvalue == '') {
                            document.getElementById('error_' + fName).innerHTML = 'Enter a valid date';
                            fail_border(fName);
                        }
                        return false;
                    }
                }
                else {
                    return true;
                }
            }
        }
    }

    /*END*/


    function success_border(id) {
        $('#' + id).removeClass('input__error');
        // elem = document.getElementById(id);
        // elem.setAttribute("style","border: 1px solid #D3D3D3; box-shadow: none;");
        // document.getElementById('error_'+id).innerHTML = '';
        // document.getElementById('error_'+id).style.display = 'none';
    }

    function fail_border(id) {
        $('#' + id).addClass('input__error');
        // elem = document.getElementById(id);
        // elem.setAttribute("style","border: 1px solid #EF672F; box-shadow: 0 0 2px #EF672F;");	
        // document.getElementById('error_'+id).style.display = 'block';	
    }

    function focus_border(id) {
        elem = document.getElementById(id);
        elem.setAttribute("style", "outline:none; border-color: #4D90FE; box-shadow: 0 0 2px #4D90FE;");
    }
    function back_toclearBorder(id) {
        elem = document.getElementById(id);
        elem.setAttribute("style", "border: 1px solid #D3D3D3; box-shadow: none;");
    }