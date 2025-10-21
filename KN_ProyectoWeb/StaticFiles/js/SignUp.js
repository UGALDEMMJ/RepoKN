function getName() {

    let id = $("#Id").val();
    $("#Name").val("");
    if (id.length >= 9) {

        $.ajax({
            type: "GET",
            url: "https://apis.gometa.org/cedulas/" + id,
            dataType: "json",
            success: function (result) {
                $("#Name").val(result.nombre);
            }
        })
    }
}

$(function () {

        $("#SignUpForm").validate({
            rules: {
                Id: {
                    required: true
                },
                Name: {
                    required: true
                },
                Email: {
                    required: true,
                    email: true
                },
                Password: {
                    required: true
                }
            },
            messages: {
                Id: {
                    required: "* Required",
                },
                Name: {
                    required: "* Required",
                },
                Email: {
                    required: "* Required",
                    email: "* Formato",
                },
                Password: {
                    required: "* Required",
                }
            }
       });
});

