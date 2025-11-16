// wwwroot/js/site.js

$(function () {

    // =================================================================
    // 1. HÀM MỞ POPUP KHI BẤM NÚT .btn-popup
    // =================================================================
    $(document).on('click', '.btn-popup', function (e) {
        e.preventDefault();

        var button = $(this);
        var url = button.attr("href");
        var title = button.data("title");

        var modal = $('#mainModal');
        var modalBody = modal.find('.modal-body');
        var modalTitle = modal.find('.modal-title');

        modalTitle.text(title);
        modalBody.html('<p>Loading content...</p>');
        modal.modal('show');

        // Tải nội dung Partial View bằng AJAX
        $.get(url, function (data) {
            modalBody.html(data);

            // Bảo jQuery Validator "đọc" form mới vừa được tải vào
            // Điều này RẤT QUAN TRỌNG
            var form = modalBody.find('form');
            if ($.validator && $.validator.unobtrusive) {
                $.validator.unobtrusive.parse(form);
            }

        }).fail(function () {
            modalBody.html('<p>Could not load content. Please try again.</p>');
        });
    });


    // =================================================================
    // 2. HÀM SUBMIT FORM BÊN TRONG POPUP
    // =================================================================
    $(document).on('submit', '#mainModal form', function (e) {

        // NGĂN chặn hành vi submit HTML truyền thống
        // Đây là lý do code của bạn đang hỏng (không có dòng này)
        e.preventDefault();

        var form = $(this);

        // Kiểm tra validation (chỉ khi thư viện tồn tại)
        if ($.validator && !form.valid()) {
            return; // Nếu không valid, dừng lại và hiển thị lỗi
        }

        var url = form.attr("action");
        var formData = form.serialize();
        var modal = $('#mainModal');
        var modalBody = modal.find('.modal-body');

        // Gửi dữ liệu bằng AJAX POST
        $.post(url, formData, function (result) {

            // Nếu C# trả về { success = true }
            if (result.success) {
                modal.modal('hide');
                location.reload(); // Tải lại trang
            } else {
                // Nếu C# trả về lỗi (ModelState.IsValid == false)
                // C# sẽ trả về lại Partial View với lỗi
                modalBody.html(result);

                // Bảo jQuery Validator "đọc" lại form mới (với lỗi)
                var newForm = modalBody.find('form');
                if ($.validator && $.validator.unobtrusive) {
                    $.validator.unobtrusive.parse(newForm);
                }
            }
        }).fail(function () {
            alert("An error occurred while saving data. Please try again.");
        });
    });

});