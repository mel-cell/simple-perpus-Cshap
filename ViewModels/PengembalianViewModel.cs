using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using pr.Data;
using pr.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace pr.ViewModels
{
    public partial class PengembalianViewModel : ViewModelBase
    {
        private readonly PerpusDbContext _dbContext;

        [ObservableProperty]
        private ObservableCollection<Peminjaman> _peminjamanAktifList = new();

        [ObservableProperty]
        private ObservableCollection<Pengembalian> _pengembalianList = new();

        [ObservableProperty]
        private Peminjaman? _selectedPeminjaman;

        [ObservableProperty]
        private DateTimeOffset? _tanggalPengembalian = DateTimeOffset.Now;

        [ObservableProperty]
        private string _kondisiBuku = string.Empty;

        [ObservableProperty]
        private decimal _denda;

        [ObservableProperty]
        private string _searchText = string.Empty;

        public PengembalianViewModel(PerpusDbContext dbContext)
        {
            _dbContext = dbContext;
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var peminjamanAktif = await _dbContext.Peminjaman
                .Where(p => p.Status == "Dipinjam")
                .ToListAsync();
            PeminjamanAktifList = new ObservableCollection<Peminjaman>(peminjamanAktif);

            var pengembalianData = await _dbContext.Pengembalian.ToListAsync();
            PengembalianList = new ObservableCollection<Pengembalian>(pengembalianData);
        }

        [RelayCommand]
        private async Task KembalikanBukuAsync()
        {
            if (SelectedPeminjaman == null || TanggalPengembalian == null) return;

            // Hitung denda jika terlambat
            var denda = 0m;
            if (TanggalPengembalian.Value.DateTime > SelectedPeminjaman.TanggalKembali)
            {
                var hariTerlambat = (TanggalPengembalian.Value.DateTime - SelectedPeminjaman.TanggalKembali).Days;
                denda = hariTerlambat * 1000m; // Misal denda 1000 per hari
            }

            var pengembalian = new Pengembalian
            {
                PengembalianId = Guid.NewGuid().ToString(),
                PeminjamanId = SelectedPeminjaman.PeminjamanId,
                TanggalPengembalian = TanggalPengembalian.Value.DateTime,
                KondisiBuku = KondisiBuku,
                Denda = denda
            };

            // Update status peminjaman
            SelectedPeminjaman.Status = "Dikembalikan";

            _dbContext.Pengembalian.Add(pengembalian);
            await _dbContext.SaveChangesAsync();
            await LoadDataAsync();

            SelectedPeminjaman = null;
            TanggalPengembalian = DateTimeOffset.Now;
            KondisiBuku = string.Empty;
            Denda = 0;
        }

        partial void OnSearchTextChanged(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _ = LoadDataAsync();
            }
            else
            {
                var filtered = PengembalianList.Where(p =>
                    p.PengembalianId.Contains(value, StringComparison.OrdinalIgnoreCase) ||
                    p.PeminjamanId.Contains(value, StringComparison.OrdinalIgnoreCase)).ToList();
                PengembalianList = new ObservableCollection<Pengembalian>(filtered);
            }
        }

        [RelayCommand]
        private void UpdatePengembalian(Pengembalian pengembalian)
        {
            // Find the corresponding peminjaman for the form
            SelectedPeminjaman = PeminjamanAktifList.FirstOrDefault(p => p.PeminjamanId == pengembalian.PeminjamanId);
            TanggalPengembalian = pengembalian.TanggalPengembalian;
            KondisiBuku = pengembalian.KondisiBuku;
            Denda = pengembalian.Denda;
        }

        [RelayCommand]
        private async Task DeletePengembalianAsync(Pengembalian pengembalian)
        {
            _dbContext.Pengembalian.Remove(pengembalian);
            await _dbContext.SaveChangesAsync();
            await LoadDataAsync();
        }
    }
}
