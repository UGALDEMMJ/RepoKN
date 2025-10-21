$(function () {
    $("#ForgotPasswordForm").validate({
        rules: {
            Email: {
                required: true,
                email: true
            }
        },
        messages: {
            Email: {
                required: "* Required",
                email: "* Format"
            }
        }
    });
});