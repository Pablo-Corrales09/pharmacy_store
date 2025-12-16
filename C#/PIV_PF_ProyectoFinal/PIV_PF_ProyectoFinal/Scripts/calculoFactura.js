document.addEventListener('DOMContentLoaded', function () {

    function calcularSubtotal() {
        var precioTexto = document.getElementById('precio_producto')?.value || '';
        var cantidad = parseInt(document.getElementById('cantidad')?.value) || 1;
        var tipoPago = document.getElementById('metodo_pago')?.value || '';

        // Limpiar el símbolo ₡ si existe
        var precio = parseFloat(precioTexto.replace('₡', '').trim()) || 0;

        var subtotalBase = precio * cantidad;
        var subtotal = subtotalBase;

        if (tipoPago === 'Tarjeta') {
            subtotal = subtotalBase * 1.02;
        }

        var subTotalInput = document.getElementById('sub_total');
        if (subTotalInput) {
            subTotalInput.value = subtotal.toFixed(2).replace(',', '.');
        }

        console.log('Cálculo - Precio:', precio, 'Cant:', cantidad, 'Pago:', tipoPago, 'Total:', subtotal.toFixed(2));
    }

    var descripcionProducto = document.getElementById('descripcion_producto');
    const precioProducto = document.getElementById('precio_producto');
    const valorUnitarioHidden = document.getElementById('valor_unitario');
    const codigoProductoHidden = document.getElementById('codigo_producto_hidden');

    if (descripcionProducto) {
        descripcionProducto.addEventListener('change', function () {
            var selectedOption = this.options[this.selectedIndex];

            if (!selectedOption || selectedOption.value === '') {
                precioProducto.value = '';
                valorUnitarioHidden.value = '0';
                codigoProductoHidden.value = '';
                calcularSubtotal();
                console.log('Producto deseleccionado');
                return;
            }

            var precio = selectedOption.getAttribute('data-precio') || '0';
            var codigo = selectedOption.getAttribute('data-codigoproducto') || '';

            console.log('Producto seleccionado:');
            console.log('  Descripción:', selectedOption.value);
            console.log('  Precio:', precio);
            console.log('  Código:', codigo);

            // Mostrar precio con símbolo ₡
            precioProducto.value = precio ? '₡' + parseFloat(precio).toFixed(2) : '';

            // Guardar valores sin símbolo para envío al servidor
            valorUnitarioHidden.value = precio;
            codigoProductoHidden.value = codigo;

            calcularSubtotal();
        });
    }

    document.getElementById('cantidad')?.addEventListener('input', calcularSubtotal);
    document.getElementById('metodo_pago')?.addEventListener('change', calcularSubtotal);

    // Validación antes de submit
    var formulario = document.getElementById('formFactura');
    if (formulario) {
        formulario.addEventListener('submit', function (e) {
            var codigo = document.getElementById('codigo_producto_hidden').value;
            var precio = document.getElementById('valor_unitario').value;
            var cantidad = document.getElementById('cantidad').value;
            var cliente = document.getElementById('cedula_cliente').value;
            var metodoPago = document.getElementById('metodo_pago').value;

            console.log('=== DATOS QUE SE VAN A ENVIAR ===');
            console.log('Cliente:', cliente);
            console.log('Método Pago:', metodoPago);
            console.log('Código Producto:', codigo);
            console.log('Precio Unitario:', precio);
            console.log('Cantidad:', cantidad);

            var errores = [];

            if (!cliente) {
                errores.push('Debes seleccionar un cliente');
            }

            if (!metodoPago) {
                errores.push('Debes seleccionar un método de pago');
            }

            if (!codigo || codigo === '0' || codigo === '') {
                errores.push('Debes seleccionar un producto válido');
            }

            if (!precio || precio === '0' || parseFloat(precio) <= 0) {
                errores.push('El producto debe tener un precio válido');
            }

            if (!cantidad || parseInt(cantidad) <= 0) {
                errores.push('La cantidad debe ser mayor a 0');
            }

            if (errores.length > 0) {
                e.preventDefault();
                alert('Errores en el formulario:\n\n' + errores.join('\n'));
                return false;
            }

            console.log('✓ Validación exitosa - Enviando formulario');
        });
    }
}); document.addEventListener('DOMContentLoaded', function () {

    function calcularSubtotal() {
        var precioTexto = document.getElementById('precio_producto')?.value || '';
        var cantidad = parseInt(document.getElementById('cantidad')?.value) || 1;
        var tipoPago = document.getElementById('metodo_pago')?.value || '';

        // Limpiar el símbolo ₡ si existe
        var precio = parseFloat(precioTexto.replace('₡', '').trim()) || 0;

        var subtotalBase = precio * cantidad;
        var subtotal = subtotalBase;

        if (tipoPago === 'Tarjeta') {
            subtotal = subtotalBase * 1.02;
        }

        var subTotalInput = document.getElementById('sub_total');
        if (subTotalInput) {
            subTotalInput.value = subtotal.toFixed(2).replace(',', '.');
        }

        console.log('Cálculo - Precio:', precio, 'Cant:', cantidad, 'Pago:', tipoPago, 'Total:', subtotal.toFixed(2));
    }

    var descripcionProducto = document.getElementById('descripcion_producto');
    const precioProducto = document.getElementById('precio_producto');
    const valorUnitarioHidden = document.getElementById('valor_unitario');
    const codigoProductoHidden = document.getElementById('codigo_producto_hidden');

    if (descripcionProducto) {
        descripcionProducto.addEventListener('change', function () {
            var selectedOption = this.options[this.selectedIndex];

            if (!selectedOption || selectedOption.value === '') {
                precioProducto.value = '';
                valorUnitarioHidden.value = '0';
                codigoProductoHidden.value = '';
                calcularSubtotal();
                console.log('Producto deseleccionado');
                return;
            }

            var precio = selectedOption.getAttribute('data-precio') || '0';
            var codigo = selectedOption.getAttribute('data-codigoproducto') || '';

            console.log('Producto seleccionado:');
            console.log('  Descripción:', selectedOption.value);
            console.log('  Precio:', precio);
            console.log('  Código:', codigo);

            // Mostrar precio con símbolo ₡
            precioProducto.value = precio ? '₡' + parseFloat(precio).toFixed(2) : '';

            // Guardar valores sin símbolo para envío al servidor
            valorUnitarioHidden.value = precio;
            codigoProductoHidden.value = codigo;

            calcularSubtotal();
        });
    }

    document.getElementById('cantidad')?.addEventListener('input', calcularSubtotal);
    document.getElementById('metodo_pago')?.addEventListener('change', calcularSubtotal);

    // Validación antes de submit
    var formulario = document.getElementById('formFactura');
    if (formulario) {
        formulario.addEventListener('submit', function (e) {
            var codigo = document.getElementById('codigo_producto_hidden').value;
            var precio = document.getElementById('valor_unitario').value;
            var cantidad = document.getElementById('cantidad').value;
            var cliente = document.getElementById('cedula_cliente').value;
            var metodoPago = document.getElementById('metodo_pago').value;

            console.log('=== DATOS QUE SE VAN A ENVIAR ===');
            console.log('Cliente:', cliente);
            console.log('Método Pago:', metodoPago);
            console.log('Código Producto:', codigo);
            console.log('Precio Unitario:', precio);
            console.log('Cantidad:', cantidad);

            var errores = [];

            if (!cliente) {
                errores.push('Debes seleccionar un cliente');
            }

            if (!metodoPago) {
                errores.push('Debes seleccionar un método de pago');
            }

            if (!codigo || codigo === '0' || codigo === '') {
                errores.push('Debes seleccionar un producto válido');
            }

            if (!precio || precio === '0' || parseFloat(precio) <= 0) {
                errores.push('El producto debe tener un precio válido');
            }

            if (!cantidad || parseInt(cantidad) <= 0) {
                errores.push('La cantidad debe ser mayor a 0');
            }

            if (errores.length > 0) {
                e.preventDefault();
                alert('Errores en el formulario:\n\n' + errores.join('\n'));
                return false;
            }

            console.log('✓ Validación exitosa - Enviando formulario');
        });
    }
});