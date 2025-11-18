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
    public partial class PeminjamanViewModel : ViewModelBase
    {
        private readonly PerpusDbContext _dbContext;

        [ObservableProperty]
        private ObservableCollection<Siswa> _siswaList = new();

        [ObservableProperty]
        private ObservableCollection<Buku> _bukuList = new();

        [ObservableProperty]
        private ObservableCollection<Peminjaman> _peminjamanList = new();

        [ObservableProperty]
        private Siswa? _selectedSiswa;

        [ObservableProperty]
        private Buku? _selectedBuku;

        [ObservableProperty]
        private DateTimeOffset? _tanggalPinjam = DateTimeOffset.Now;

        [ObservableProperty]
        private DateTimeOffset? _tanggalKembali = DateTimeOffset.Now.AddDays(7);

        [ObservableProperty]
        private string _searchText = string.Empty;

        public PeminjamanViewModel(PerpusDbContext dbContext)
        {
            _dbContext = dbContext;
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var siswaData = await _dbContext.Siswa.ToListAsync();
            SiswaList = new ObservableCollection<Siswa>(siswaData);

            var bukuData = await _dbContext.Buku.ToListAsync();
            BukuList = new ObservableCollection<Buku>(bukuData);

            var peminjamanData = await _dbContext.Peminjaman.ToListAsync();
            PeminjamanList = new ObservableCollection<Peminjaman>(peminjamanData);
        }

        [RelayCommand]
        private async Task SimpanPeminjamanAsync()
        {
            if (SelectedSiswa == null || SelectedBuku == null) return;

            var peminjaman = new Peminjaman
            {
                PeminjamanId = Guid.NewGuid().ToString(),
                UserId = SelectedSiswa.SiswaId,
                BukuId = SelectedBuku.BukuId,
                TanggalPinjam = TanggalPinjam.Value.DateTime,
                TanggalKembali = TanggalKembali.Value.DateTime,
                Status = "Dipinjam"
            };

            _dbContext.Peminjaman.Add(peminjaman);
            await _dbContext.SaveChangesAsync();
            await LoadDataAsync();

            SelectedSiswa = null;
            SelectedBuku = null;
            TanggalPinjam = DateTimeOffset.Now;
            TanggalKembali = DateTimeOffset.Now.AddDays(7);
        }

        partial void OnSearchTextChanged(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _ = LoadDataAsync();
            }
            else
            {
                var filtered = PeminjamanList.Where(p =>
                    p.PeminjamanId.Contains(value, StringComparison.OrdinalIgnoreCase) ||
                    p.UserId.Contains(value, StringComparison.OrdinalIgnoreCase) ||
                    p.BukuId.Contains(value, StringComparison.OrdinalIgnoreCase)).ToList();
                PeminjamanList = new ObservableCollection<Peminjaman>(filtered);
            }
        }

        [RelayCommand]
        private void UpdatePeminjaman(Peminjaman peminjaman)
        {
            // Find the corresponding Siswa and Buku for the form dropdowns
            SelectedSiswa = SiswaList.FirstOrDefault(s => s.SiswaId == peminjaman.UserId);
            SelectedBuku = BukuList.FirstOrDefault(b => b.BukuId == peminjaman.BukuId);
            TanggalPinjam = peminjaman.TanggalPinjam;
            TanggalKembali = peminjaman.TanggalKembali;
        }

        [RelayCommand]
        private async Task DeletePeminjamanAsync(Peminjaman peminjaman)
        {
            _dbContext.Peminjaman.Remove(peminjaman);
            await _dbContext.SaveChangesAsync();
            await LoadDataAsync();
        }
    }
}
