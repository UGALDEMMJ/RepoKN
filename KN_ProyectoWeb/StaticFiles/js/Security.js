$(function () {

    $("#SecurityForm").validate({
        rules: {
            Password: {
                required: true,
                maxlength: 10
            },
            ConfirmPassword: {
                required: true,
                equalTo: "#Password",
                maxlength: 10
            }
        },
        messages: {
            Password: {
                required: "* Required",
                maxlength: "* Max 10 characteres"
            },
            ConfirmPassword: {
                required: "* Required",
                equalTo: "* Password must be the same",
                maxlength: "* Max 10 characteres"
            }
        }
    });

});