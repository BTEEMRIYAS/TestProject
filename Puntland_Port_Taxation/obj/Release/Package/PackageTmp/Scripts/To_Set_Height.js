$(document).ready(function () {
    var r_height = $('#manage_admin_page_right_side').height();
    var l_height = $('#manage_admin_page_left_side').height();
    if (r_height > l_height) {
        document.getElementById('manage_admin_page_left_side').style.height = r_height + 'px';
    }
});