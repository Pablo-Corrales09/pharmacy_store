document.addEventListener("DOMContentLoaded", function () {
    const filas = document.querySelectorAll("table tbody tr");

    filas.forEach(fila => {
        fila.addEventListener("click", function () {

            filas.forEach(f => f.classList.remove("selected"));
            this.classList.add("selected");

            let id = this.cells[0].innerText;
            console.log("Cliente seleccionado:", id);
        });
    });
});

