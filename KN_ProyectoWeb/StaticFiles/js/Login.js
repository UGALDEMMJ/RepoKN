$(function () {
    $("#LoginForm").validate({
        rules: {
            Email: {
                required: true,
                email: true
            },
            Password: {
                required: true
            }
        },
        messages: {
            Email: {
                required: "* Required",
                email: "* Format"
            },
            Password: {
                required: "* Required"
            }
        }
    });
});