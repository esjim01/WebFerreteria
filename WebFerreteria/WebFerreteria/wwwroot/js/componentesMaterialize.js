//$(function () {
//    $('.datepicker').datepicker

//    $('select').formSelect();

//    $('.modal').modal();

//    $('.timepicker').timepicker();

//});

$(function () {
    // Inicialización del DatePicker
    $('.datepicker').datepicker({
        format: 'dd/mm/yyyy',
        autoClose: true,
        i18n: {
            months: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
            monthsShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
            weekdays: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
            weekdaysShort: ['Dom', 'Lun', 'Mar', 'Mie', 'Jue', 'Vie', 'Sab'],
            today: 'Hoy',
            clear: 'Limpiar',
            cancel: 'Cancelar',
            done: 'Aceptar'
        },
        onClose: function () {
            combinarFechaHora();
        }
    });

    // Inicialización del TimePicker
    $('.timepicker').timepicker({
        twelveHour: false, // Formato 24 horas
        autoClose: true,
        onCloseEnd: function () {
            combinarFechaHora();
        }
    });

    // Inicialización de otros componentes
    $('select').formSelect();
    $('.modal').modal();

    // Función para combinar fecha y hora en el campo FechaCita
    function combinarFechaHora() {
        var fecha = $('#fechaPicker').val(); // Obtener valor del datepicker
        var hora = $('#horaPicker').val();    // Obtener valor del timepicker

        if (fecha && hora) {
            // Convertir a formato ISO (yyyy-MM-ddTHH:mm)
            var fechaObj = new Date(fecha.split('/').reverse().join('-') + 'T' + hora);

            // Asignar al campo oculto FechaCita
            $('#FechaCita').val(fechaObj.toISOString());
        }
    }

    // Si hay un valor existente en FechaCita, establecerlo en los pickers
    var fechaExistente = $('#FechaCita').val();
    if (fechaExistente) {
        var fecha = new Date(fechaExistente);

        // Formatear fecha para el datepicker (dd/mm/yyyy)
        var fechaFormateada = ('0' + fecha.getDate()).slice(-2) + '/' +
            ('0' + (fecha.getMonth() + 1)).slice(-2) + '/' +
            fecha.getFullYear();

        // Formatear hora para el timepicker (HH:mm)
        var horaFormateada = ('0' + fecha.getHours()).slice(-2) + ':' +
            ('0' + fecha.getMinutes()).slice(-2);

        $('#fechaPicker').val(fechaFormateada);
        $('#horaPicker').val(horaFormateada);

        // Actualizar instancias de los pickers
        M.updateTextFields();
    }
});
