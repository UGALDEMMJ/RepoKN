$(function () {

    $("#FormAddProduct").validate({
        rules: {
            Name: {
                required: true
            },
            Description: {
                required: true
            },
            Price: {
                required: true,
                decimal: true
            },
            Quantity: {
                required: true,
                number: true
            },
            ConsecutiveCategory: {
                required: true
            },
            ImageProduct: {
                required: true,
                extension: "png",
                filesize: 2 * 1024 * 1024 // 2 MB en bytes
            }
        },
        messages: {
            Name: {
                required: "* Requerido",
            },
            Description: {
                required: "* Requerido",
            },
            Price: {
                required: "* Requerido",
                decimal: "* Use a valid number"
            },
            Quantity: {
                required: "* Requerido",
                number: "* Use a valid number"
            },
            ConsecutiveCategory: {
                required: "* Requerido",
            },
            ImageProduct: {
                required: "* Requerido",
                extension: "Only .png format",
                filesize: "Max size 2 MB"
            }
        }
    });

    $.validator.addMethod("regex", function (value, element, pattern) {
        return this.optional(element) || pattern.test(value);
    });

    $.validator.addMethod("filesize", function (value, element, param) {
        if (element.files.length === 0) {
            return false;
        }
        return element.files[0].size <= param;
    }, "The file size is to big.");

    $.validator.addMethod("extension", function (value, element, param) {
        return this.optional(element) || new RegExp("\\.(" + param + ")$", "i").test(value);
    }, "Incorrect format.");

});