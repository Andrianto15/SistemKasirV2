// JS for Sidebar Menu
const menu = document.getElementById('menu-label');
const sidebar = document.getElementsByClassName('sidebar')[0];

menu.addEventListener('click', function () {
    sidebar.classList.toggle('hide');
})

$(document).ready(function () {
    $('table.datatable').DataTable();
});

// JS for Menu Transaksi
document.addEventListener('change', function (e) {
    if (e.target.classList.contains('hargaProduk')
        || e.target.classList.contains('jumlahTotal')) {
        CalculateTotalTransaksi();
        CalculateJumlah();
    }
})

function DeleteItem(btn) {
    var table = document.getElementById('detailTransaksiTable');
    var rows = table.getElementsByTagName('tr');

    var visibleRowCount = 0;

    var listRow = document.querySelectorAll("[id*='IsDeleted']");
    for (i = 0; i < listRow.length; i++) {
        if (listRow[i].value === "False") {
            visibleRowCount++;
        }
    }

    if (visibleRowCount <= 1) {
        alert('Baris ini tidak bisa dihapus');
        return;
    }

    var btnIdx = btn.id.replaceAll('deleteBtn-', '');
    var idOfIsDeletedRow = btnIdx + "__IsDeleted";

    var hideIsDeletedId = document.querySelector("[id$='" + idOfIsDeletedRow + "']").id;

    document.getElementById(hideIsDeletedId).value = "true";

    $(btn).closest('tr').hide();

    CalculateTotalTransaksi();
    CalculateJumlah();
}

function AddItem(btn) {
    var table = document.getElementById('detailTransaksiTable');
    var rows = table.getElementsByTagName('tr');

    var rowOuterHtml = rows[rows.length - 1].outerHTML;

    var lastrowIdx = rows.length - 2; //document.getElementById('hdnLastIndex').value;

    var nextrowIdx = eval(lastrowIdx) + 1;

    //document.getElementById('hdnLastIndex').value = nextrowIdx;

    rowOuterHtml = rowOuterHtml.replaceAll('_' + lastrowIdx + '_', '_' + nextrowIdx + '_');
    rowOuterHtml = rowOuterHtml.replaceAll('[' + lastrowIdx + ']', '[' + nextrowIdx + ']');
    rowOuterHtml = rowOuterHtml.replaceAll('-' + lastrowIdx, '-' + nextrowIdx);

    // tambah row baru
    var newRow = table.insertRow();
    newRow.innerHTML = rowOuterHtml;

    var x = document.getElementsByTagName('input');

    for (var i = 0; i < x.length; i++) {
        if (x[i].type === "number" && x[i].id.indexOf('_' + nextrowIdx + '_') > 0) {
            x[i].value = 0;
        } else if (x[i].type === "text" && x[i].id.indexOf('_' + nextrowIdx + '_') > 0) {
            x[i].value = null;
        }
    }

    rebindValidators();
}

function CalculateTotalTransaksi() {
    var x = document.getElementsByClassName('hargaProduk');
    var totalHarga = 0;

    for (var i = 0; i < x.length; i++) {
        var idOfIsDeletedRow = i + "__IsDeleted";

        var hideIsDeletedId = document.querySelector("[id$='" + idOfIsDeletedRow + "']").id;

        if (document.getElementById(hideIsDeletedId).value !== "true") {
            totalHarga += eval(x[i].value);
        }
    }

    document.getElementById('txtHargaTotal').value = totalHarga;
    document.getElementById('TotalHargaTransaksi').value = totalHarga;

    return;
}

function CalculateJumlah() {
    var x = document.getElementsByClassName('jumlahTotal');
    var totalProduk = 0;

    for (var i = 0; i < x.length; i++) {
        var idOfIsDeletedRow = i + "__IsDeleted";

        var hideIsDeletedId = document.querySelector("[id$='" + idOfIsDeletedRow + "']").id;

        if (document.getElementById(hideIsDeletedId).value !== "true") {
            totalProduk += eval(x[i].value);
        }
    }

    document.getElementById('txtJumlahTotal').value = totalProduk;

    return;
}

//function CalculateHargaPerProduk() {
//    var btnIdx = btn.id.replaceAll('deleteBtn-', '');
//    var idOfJumlah = btnIdx + "__Jumlah";
//    var idOfHarga = btnIdx + "__Harga";

//    var jumlahId = document.querySelector("[id$='" + idOfJumlah + "']").id;
//    var hargaId = document.querySelector("[id$='" + idOfHarga + "']").id;

//    var jumlah = document.getElementById(jumlahId).value;
//    var harga = document.getElementById(hargaId).value;

//    document.getElementById('Total').value = jumlah * harga;

//    return;
//}

function rebindValidators() {
    var $form = $("#createTransaksiForm");

    $form.unbind();

    $form.data("validator", null);

    $.validator.unobtrusive.parse($form);

    $form.validate($form.data("unobtrusiveValidation").option);
}

// JS for Delete Modal
// 1. Delete Kategori Modal
$('#deleteKategoriModal').on('show.bs.modal', function (event) {
    const button = event.relatedTarget

    const paramId = button.getAttribute('data-bs-paramid')
    const paramValue = button.getAttribute('data-bs-paramvalue')

    const deleteModal = $(this)

    deleteModal.find('.modal-body input').val(paramId)
    deleteModal.find('#descriptionValue').text(paramValue)
})

$("#modalDeleteKategoriButton").click(function () {
    $("#deleteKategoriForm").submit();
});

// 2. Delete Produk Modal
$('#deleteProdukModal').on('show.bs.modal', function (event) {
    const button = event.relatedTarget

    const paramId = button.getAttribute('data-bs-paramid')
    const paramValue = button.getAttribute('data-bs-paramvalue')

    const deleteModal = $(this)

    deleteModal.find('.modal-body input').val(paramId)
    deleteModal.find('#descriptionValue').text(paramValue)
})

$("#modalDeleteProdukButton").click(function () {
    $("#deleteProdukForm").submit();
});

// 3. Delete User Modal
$('#deleteUserModal').on('show.bs.modal', function (event) {
    const button = event.relatedTarget

    const paramId = button.getAttribute('data-bs-paramid')
    const paramValue = button.getAttribute('data-bs-paramvalue')

    const deleteModal = $(this)

    deleteModal.find('.modal-body input').val(paramId)
    deleteModal.find('#descriptionValue').text(paramValue)
})

$("#modalDeleteUserButton").click(function () {
    $("#deleteUserForm").submit();
});

// 4. Delete Transaksi Modal
$('#deleteTransaksiModal').on('show.bs.modal', function (event) {
    const button = event.relatedTarget

    const paramId = button.getAttribute('data-bs-paramid')
    const paramValue = button.getAttribute('data-bs-paramvalue')

    const deleteModal = $(this)

    deleteModal.find('.modal-body input').val(paramId)
    deleteModal.find('#descriptionValue').text(paramValue)
})

$("#modalDeleteTransaksiButton").click(function () {
    $("#deleteTransaksiForm").submit();
});