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
    public partial class BukuViewModel : ViewModelBase
    {
        private readonly PerpusDbContext _dbContext;

        [ObservableProperty]
        private ObservableCollection<Buku> _bukuList = new();

        [ObservableProperty]
        private ObservableCollection<Penulis> _penulisList = new();

        [ObservableProperty]
        private ObservableCollection<Penerbit> _penerbitList = new();

        [ObservableProperty]
        private ObservableCollection<Kategori> _kategoriList = new();

        [ObservableProperty]
        private ObservableCollection<Rak> _rakList = new();

        [ObservableProperty]
        private Buku? _selectedBuku;

        [ObservableProperty]
        private string _bukuJudul = string.Empty;

        [ObservableProperty]
        private Penulis? _selectedPenulis;

        [ObservableProperty]
        private Penerbit? _selectedPenerbit;

        [ObservableProperty]
        private Kategori? _selectedKategori;

        [ObservableProperty]
        private Rak? _selectedRak;

        [ObservableProperty]
        private string _bukuTahun = string.Empty;

        [ObservableProperty]
        private int _bukuStok;

        [ObservableProperty]
        private string _searchText = string.Empty;

        [ObservableProperty]
        private bool _isBusy;

        public BukuViewModel(PerpusDbContext dbContext)
        {
            _dbContext = dbContext;
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            IsBusy = true;
            ErrorMessage = null;
            try
            {
                var bukuData = await _dbContext.Buku.ToListAsync();
                BukuList = new ObservableCollection<Buku>(bukuData);

                var penulisData = await _dbContext.Penulis.ToListAsync();
                PenulisList = new ObservableCollection<Penulis>(penulisData);

                var penerbitData = await _dbContext.Penerbit.ToListAsync();
                PenerbitList = new ObservableCollection<Penerbit>(penerbitData);

                var kategoriData = await _dbContext.Kategori.ToListAsync();
                KategoriList = new ObservableCollection<Kategori>(kategoriData);

                var rakData = await _dbContext.Rak.ToListAsync();
                RakList = new ObservableCollection<Rak>(rakData);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Gagal memuat data: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task InsertAsync()
        {
            if (string.IsNullOrWhiteSpace(BukuJudul) ||
                SelectedPenulis == null ||
                SelectedPenerbit == null ||
                SelectedKategori == null ||
                SelectedRak == null) return;

            var buku = new Buku
            {
                BukuId = Guid.NewGuid().ToString(),
                BukuJudul = BukuJudul,
                BukuPenulis = SelectedPenulis.PenulisId,
                BukuPenerbit = SelectedPenerbit.PenerbitId,
                BukuTahun = BukuTahun,
                BukuKategori = SelectedKategori.KategoriId,
                BukuRak = SelectedRak.RakId,
                BukuStok = BukuStok
            };

            _dbContext.Buku.Add(buku);
            await _dbContext.SaveChangesAsync();
            await LoadDataAsync();

            BukuJudul = string.Empty;
            SelectedPenulis = null;
            SelectedPenerbit = null;
            SelectedKategori = null;
            SelectedRak = null;
            BukuTahun = string.Empty;
            BukuStok = 0;
        }

        [RelayCommand]
        private async Task UpdateAsync()
        {
            if (SelectedBuku == null) return;

            SelectedBuku.BukuJudul = BukuJudul;
            SelectedBuku.BukuPenulis = SelectedPenulis?.PenulisId ?? string.Empty;
            SelectedBuku.BukuPenerbit = SelectedPenerbit?.PenerbitId ?? string.Empty;
            SelectedBuku.BukuTahun = BukuTahun;
            SelectedBuku.BukuKategori = SelectedKategori?.KategoriId ?? string.Empty;
            SelectedBuku.BukuRak = SelectedRak?.RakId ?? string.Empty;
            SelectedBuku.BukuStok = BukuStok;

            await _dbContext.SaveChangesAsync();
            await LoadDataAsync();
        }

        [RelayCommand]
        private async Task DeleteAsync()
        {
            if (SelectedBuku == null) return;

            _dbContext.Buku.Remove(SelectedBuku);
            await _dbContext.SaveChangesAsync();
            await LoadDataAsync();

            SelectedBuku = null;
            BukuJudul = string.Empty;
            SelectedPenulis = null;
            SelectedPenerbit = null;
            SelectedKategori = null;
            SelectedRak = null;
            BukuTahun = string.Empty;
            BukuStok = 0;
        }

        partial void OnSelectedBukuChanged(Buku? value)
        {
            if (value != null)
            {
                BukuJudul = value.BukuJudul;
                SelectedPenulis = PenulisList.FirstOrDefault(p => p.PenulisId == value.BukuPenulis);
                SelectedPenerbit = PenerbitList.FirstOrDefault(p => p.PenerbitId == value.BukuPenerbit);
                SelectedKategori = KategoriList.FirstOrDefault(k => k.KategoriId == value.BukuKategori);
                SelectedRak = RakList.FirstOrDefault(r => r.RakId == value.BukuRak);
                BukuTahun = value.BukuTahun;
                BukuStok = value.BukuStok;
            }
        }

        partial void OnSearchTextChanged(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _ = LoadDataAsync();
            }
            else
            {
                var filtered = BukuList.Where(b =>
                    b.BukuJudul.Contains(value, StringComparison.OrdinalIgnoreCase) ||
                    b.BukuTahun.Contains(value, StringComparison.OrdinalIgnoreCase)).ToList();
                BukuList = new ObservableCollection<Buku>(filtered);
            }
        }

        [RelayCommand]
        private void UpdateBuku(Buku buku)
        {
            SelectedBuku = buku;
            BukuJudul = buku.BukuJudul;
            SelectedPenulis = PenulisList.FirstOrDefault(p => p.PenulisId == buku.BukuPenulis);
            SelectedPenerbit = PenerbitList.FirstOrDefault(p => p.PenerbitId == buku.BukuPenerbit);
            SelectedKategori = KategoriList.FirstOrDefault(k => k.KategoriId == buku.BukuKategori);
            SelectedRak = RakList.FirstOrDefault(r => r.RakId == buku.BukuRak);
            BukuTahun = buku.BukuTahun;
            BukuStok = buku.BukuStok;
        }

        [RelayCommand]
        private async Task DeleteBukuAsync(Buku buku)
        {
            _dbContext.Buku.Remove(buku);
            await _dbContext.SaveChangesAsync();
            await LoadDataAsync();
        }
    }
}
