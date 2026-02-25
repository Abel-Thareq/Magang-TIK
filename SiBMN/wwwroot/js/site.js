// Si BMN - Global JavaScript

function toggleSidebar() {
    document.getElementById('sidebar').classList.toggle('show');
}

// Close sidebar on click outside (mobile)
document.addEventListener('click', function (e) {
    const sidebar = document.getElementById('sidebar');
    const toggleBtns = document.querySelectorAll('.sidebar-toggle');
    if (sidebar && sidebar.classList.contains('show')) {
        let clickedToggle = false;
        toggleBtns.forEach(btn => {
            if (btn.contains(e.target)) clickedToggle = true;
        });
        if (!sidebar.contains(e.target) && !clickedToggle) {
            sidebar.classList.remove('show');
        }
    }
});

// Format currency
function formatRupiah(angka) {
    var number_string = angka.toString().replace(/[^,\d]/g, ''),
        split = number_string.split(','),
        sisa = split[0].length % 3,
        rupiah = split[0].substr(0, sisa),
        ribuan = split[0].substr(sisa).match(/\d{3}/gi);

    if (ribuan) {
        var separator = sisa ? '.' : '';
        rupiah += separator + ribuan.join('.');
    }

    return 'Rp ' + rupiah + (split[1] !== undefined ? ',' + split[1] : '');
}

// Auto-calculate jumlah harga
function calculateTotal() {
    var volume = parseFloat(document.getElementById('JumlahDiminta')?.value) || 0;
    var harga = parseFloat(document.getElementById('HargaSatuan')?.value) || 0;
    var total = volume * harga;
    var totalField = document.getElementById('JumlahHarga');
    if (totalField) {
        totalField.value = formatRupiah(total);
    }
}

// Handle asal barang radio buttons
function handleAsalBarang() {
    var asalBarang = document.querySelector('input[name="AsalBarang"]:checked')?.value;
    var importField = document.getElementById('importField');
    var pdnField = document.getElementById('pdnField');

    if (importField && pdnField) {
        if (asalBarang === 'Import') {
            importField.classList.add('show');
            pdnField.classList.remove('show');
        } else if (asalBarang === 'PDN') {
            importField.classList.remove('show');
            pdnField.classList.add('show');
        } else {
            importField.classList.remove('show');
            pdnField.classList.remove('show');
        }
    }
}

// Filter ruang by gedung
function filterRuangByGedung() {
    var gedung = document.getElementById('gedungSelect')?.value;
    var ruangSelect = document.getElementById('IdRuang');

    if (!gedung || !ruangSelect) return;

    fetch('/api/BarangApi/GetRuangs?gedung=' + encodeURIComponent(gedung))
        .then(response => response.json())
        .then(data => {
            ruangSelect.innerHTML = '<option value="">-- Pilih Ruang --</option>';
            data.forEach(function (ruang) {
                var option = document.createElement('option');
                option.value = ruang.idRuang;
                option.textContent = ruang.namaRuang;
                ruangSelect.appendChild(option);
            });
        });
}

// Confirm delete
function confirmDelete(message) {
    return confirm(message || 'Apakah Anda yakin ingin menghapus data ini?');
}
