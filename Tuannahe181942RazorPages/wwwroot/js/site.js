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

        // Tải nội dung Partial View
        $.get(url, function (data) {
            modalBody.html(data);

            // === SỬA LỖI (Thêm vào) ===
            // Sau khi tải form vào, bảo jQuery Validator "đọc" form mới này
            var form = modalBody.find('form');
            $.validator.unobtrusive.parse(form);
            // =========================
        }).fail(function () {
            modalBody.html('<p>Could not load content. Please try again.</p>');
        });
    });


    // =================================================================
    // 2. HÀM SUBMIT FORM BÊN TRONG POPUP
    // =================================================================
    $(document).on('submit', '#mainModal form', function (e) {
        e.preventDefault(); // Ngăn submit
        var form = $(this);

        // === SỬA LỖI (Thêm vào) ===
        // Kiểm tra xem form có hợp lệ (valid) theo các quy tắc không
        if (!form.valid()) {
            return; // Nếu không, dừng lại và hiển thị lỗi
        }
        // =========================

        var url = form.attr("action");
        var formData = form.serialize();

        var modal = $('#mainModal');
        var modalBody = modal.find('.modal-body');

        // Gửi dữ liệu bằng AJAX
        $.post(url, formData, function (result) {

            if (result.success) { // Nếu C# trả về { success = true }
                modal.modal('hide');
                location.reload();
            } else {
                // Nếu C# trả về Partial View (do ModelState.IsValid = false)
                modalBody.html(result);

                // === SỬA LỖI (Thêm vào) ===
                // Chúng ta vừa tải lại form (với thông báo lỗi)
                // Phải bảo jQuery Validator "đọc" lại lần nữa
                var newForm = modalBody.find('form');
                $.validator.unobtrusive.parse(newForm);
                // =========================
            }
        }).fail(function () {
            alert("An error occurred while saving data. Please try again.");
        });
    });

});