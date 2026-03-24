function setupPasswordToggle(inputId, iconId) {
    const toggleIcon = document.getElementById(iconId);
    const passInput = document.getElementById(inputId);

    if (toggleIcon && passInput) {
        toggleIcon.addEventListener('click', function () {
            // Đổi type từ password sang text và ngược lại
            const type = passInput.getAttribute('type') === 'password' ? 'text' : 'password';
            passInput.setAttribute('type', type);

            // Đổi icon từ nhắm mắt sang mở mắt
            this.classList.toggle('bi-eye');
            this.classList.toggle('bi-eye-slash');
        });
    }
}

// Kích hoạt khi trang đã load xong
document.addEventListener("DOMContentLoaded", function () {
    // Gọi hàm cho trang Register
    setupPasswordToggle('Input_Password', 'toggleEye1');
    setupPasswordToggle('Input_ConfirmPassword', 'toggleEye2');

    setupPasswordToggle('Input_Password', 'toggleEye1');
    setupPasswordToggle('Input_ConfirmPassword', 'toggleEye2');

    setupPasswordToggle('Input_Password', 'toggleEyeLogin');
});