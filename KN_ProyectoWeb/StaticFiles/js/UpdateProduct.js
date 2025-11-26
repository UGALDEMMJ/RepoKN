$(function () {

    $("#FormUpdateProduct").validate({
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
            Cantidad: {
                required: true,
                number: true
            },
            ConsecutiveCategory: {
                required: true
            },
            ImageProduct: {
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
                decimal: "* Ingrese un número válido"
            },
            Cantidad: {
                required: "* Requerido",
                number: "* Ingrese un número válido"
            },
            ConsecutiveCategory: {
                required: "* Requerido",
            },
            ImageProduct: {
                extension: "Solo se permiten archivos .png",
                filesize: "El tamaño máximo es de 2 MB"
            }
        }
    });

    $.validator.addMethod("filesize", function (value, element, param) {
        if (element.files.length === 0) {
            return true;
        }
        return element.files[0].size <= param;
    }, "El archivo es demasiado grande.");

    $.validator.addMethod("extension", function (value, element, param) {
        if (!value) {
            return true;
        }
        return new RegExp("\\.(" + param + ")$", "i").test(value);
    }, "Formato de archivo no permitido.");

    $.validator.addMethod("decimal", function (value, element) {
        return this.optional(element) || /^\d+(\,\d{1,2})?$/.test(value);
    }, "Ingrese un número válido (use solo un punto decimal).");

});