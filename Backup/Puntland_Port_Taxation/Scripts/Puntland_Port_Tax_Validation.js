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
    if (formsubmit('', 'role_map', [['required', 'role_id', 'function_id']], '')) {
        return true;
    }
    else {
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
function goods_add() {
    if (formsubmit('', 'goods', [['required', 'goods_category_id', 'goods_subcategory_id', 'goods_type_id', 'goods_name', 'goods_tariff', 'unit_of_measure_id'], ['tax_rate', 'goods_tariff']], '')) {
        return true;
    }
    else {
        return false;
    }
}
function goods_price_add() {
    var x = 1;
    $('.sum').each(function () {
        
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
function category_add() {
    if (formsubmit('', 'category', [['required', 'goods_category_name']], '')) {
        return true;
    }
    else {
        return false;
    }
}
function subcategory_add() {
    if (formsubmit('', 'subcategory', [['required', 'goods_category_id', 'goods_subcategory_name']], '')) {
        return true;
    }
    else {
        return false;
    }
}
function type_add() {
    if (formsubmit('', 'goods_type', [['required', 'goods_category_id', 'goods_subcategory_id', 'goods_type_name']], '')) {
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
    if (formsubmit('', 'user', [['required', 'first_name', 'middle_name', 'last_name', 'email_id', 'department_id', 'role_id'], ['first_name', 'first_name'], ['middle_name', 'middle_name'], ['last_name', 'last_name'], ['email_id', 'email_id']], '')) {
        return true;
    }
    else {
        return false;
    }
}
function user_add_login() {
    if (formsubmit('', 'user', [['required', 'first_name', 'middle_name', 'last_name', 'email_id'], ['first_name', 'first_name'], ['middle_name', 'middle_name'], ['last_name', 'last_name'], ['email_id', 'email_id']], '')) {
        return true;
    }
    else {
        return false;
    }
}
function userReg_change_password() {
    if (formsubmit('', 'user_change_password', [['required', 'old_password', 'new_password', 'confirm_password'], ['old_password', 'old_password'], ['new_password', 'new_password'], ['confirm_password', 'confirm_password'], ['equal', ['new_password', 'confirm_password']], ['password_equal', ['password', 'old_password']]], '')) {
        return true;
    }
    else {
        return false;
    }
}
function ship_add() {
    if (formsubmit('', 'ship', [['required', 'ship_name', 'ship_code_1', 'ship_code_2', 'ship_code_3']], '')) {
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
    if (formsubmit('', 'importer', [['required', 'importer_first_name', 'importer_middle_name', 'importer_last_name', 'importer_email_id', 'importer_mob_no', 'importer_type_id', 'importer_country_id'], ['first_name', 'importer_first_name'], ['middle_name', 'importer_middle_name'], ['last_name', 'importer_last_name'], ['email_id', 'importer_email_id'], ['MobileNo','importer_mob_no']], '')) {
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
    if (formsubmit('', 'tally_sheet', [['required', 'tally_sheet_code', 'ship_arrival_id']], '')) {
        return true;
    }
    else {
        return false;
    }
}
function create_tally_sheet_add() {
    if (formsubmit('', 'create_tally_sheet', [['required', 'way_bill_code', 'goods', 'units', 'quantity', 'unit_of_measure', 'total_quantity']], '')) {
        return true;
    }
    else {
        return false;
    }
}
function edit_tally_sheet_add() {
    if (formsubmit('', 'edit_tally_sheet', [['required', 'tally_sheet_code', 'ship_arrival_id', 'way_bill_code', 'Goods', 'units', 'quantity', 'unit_of_measure', 'total_quantity']], '')) {
        return true;
    }
    else {
        return false;
    }
}
function way_bill_add() {
    if (formsubmit('', 'way_bill', [['required', 'way_bill_code', 'import_id']], '')) {
        return true;
    }
    else {
        return false;
    }
}
function create_way_bill_add() {
    if (formsubmit('', 'create_way_bill', [['required', 'goods_category_id', 'goods_subcategory_id', 'goods_type_id', 'goods_id', 'unit_of_measure_id', 'total_quantity'], ['tax_rate', 'total_quantity']], '')) {
        return true;
    }
    else {
        return false;
    }
}
function way_bill_edit() {
    if (formsubmit('', 'edit_way_bill', [['required', 'way_bill_code', 'import_id', 'goods_category_id', 'goods_subcategory_id', 'goods_type_id', 'goods_id', 'unit_of_measure_id', 'total_quantity'], ['tax_rate', 'total_quantity']], '')) {
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
    if (formsubmit('', 'levi_entry', [['required', 'levi_type_id', 'levi_name', 'levi'], ['tax_rate', 'levi']], '')) {
        return true;
    }
    else {
        return false;
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
        if (reqFieldArr[i][0] == 'new_password') {
            for (j = reqFieldArr[i].length - 1; j >= 1; j--) {
                fieldArr[k] = new fieldObj(this.formNM, reqFieldArr[i][j], 'new_password');
                if (fieldArr[k].filled == true) {
                    filledCount++;
                }
                k++;
            }
        }
        if (reqFieldArr[i][0] == 'confirm_password') {
            for (j = reqFieldArr[i].length - 1; j >= 1; j--) {
                fieldArr[k] = new fieldObj(this.formNM, reqFieldArr[i][j], 'confirm_password');
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
                fieldArr[k] = new fieldObj(this.formNM, reqFieldArr[i][j], 'Pin');
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
        if (reqFieldArr[i][0] == 'middle_name') {
            for (j = reqFieldArr[i].length - 1; j >= 1; j--) {
                fieldArr[k] = new fieldObj(this.formNM, reqFieldArr[i][j], 'middle_name');
                if (fieldArr[k].filled == true) {
                    filledCount++;
                }
                k++;
            }
        }
        if (reqFieldArr[i][0] == 'last_name') {
            for (j = reqFieldArr[i].length - 1; j >= 1; j--) {
                fieldArr[k] = new fieldObj(this.formNM, reqFieldArr[i][j], 'last_name');
                if (fieldArr[k].filled == true) {
                    filledCount++;
                }
                k++;
            }
        }
        if (reqFieldArr[i][0] == 'surname') {
            for (j = reqFieldArr[i].length - 1; j >= 1; j--) {
                fieldArr[k] = new fieldObj(this.formNM, reqFieldArr[i][j], 'surname');
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
        if (reqFieldArr[i][0] == 'password_equal') {
            for (j = reqFieldArr[i].length - 1; j >= 1; j--) {
                fieldArr[k] = new fieldObj(this.formNM, reqFieldArr[i][j], 'password_equal');
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
        if (type == 'new_password') {
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
        if (type == 'confirm_password') {
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
                if (/^[0-9]{10}$/i.test(curField.value)) {
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
        if (type == 'Pin') {
            if ($.trim(curField.value) != '') {
                if (/^[0-9]{6}$/i.test(curField.value)) {
                    document.getElementById('error_' + fName).innerHTML = '';
                    return true;
                }
                else {
                    var chkvalue = document.getElementById('error_' + fName).innerHTML.trim();

                    if (chkvalue == '') {
                        document.getElementById('error_' + fName).innerHTML = 'Enter a valid pin number';
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
                if (/^[a-zA-Z]+$/i.test(curField.value)) {
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
        if (type == 'middle_name') {
            if ($.trim(curField.value) != '') {
                if (/^[a-zA-Z]+$/i.test(curField.value)) {
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
        if (type == 'last_name') {
            if ($.trim(curField.value) != '') {
                if (/^[a-zA-Z]+$/i.test(curField.value)) {
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
        if (type == 'surname') {
            if ($.trim(curField.value) != '') {
                if (/^[a-zA-Z]+$/i.test(curField.value)) {
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
        if (type == 'password_equal') {

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
                        document.getElementById('error_' + LfName).innerHTML = 'Incorrect password';
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