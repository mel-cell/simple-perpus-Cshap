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
    public partial class KategoriViewModel : ViewModelBase
    {
        private readonly PerpusDbContext _dbContext;

        [ObservableProperty]
        private ObservableCollection<Kategori> _kategoriList = new();

        [ObservableProperty]
        private Kategori? _selectedKategori;

        [ObservableProperty]
        private string _kategoriNama = string.Empty;

        [ObservableProperty]
        private string _kategoriDeskripsi = string.Empty;

        [ObservableProperty]
        private string _searchText = string.Empty;

        public KategoriViewModel(PerpusDbContext dbContext)
        {
            _dbContext = dbContext;
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var data = await _dbContext.Kategori.ToListAsync();
            KategoriList = new ObservableCollection<Kategori>(data);
        }

        [RelayCommand]
        private async Task InsertAsync()
        {
            if (string.IsNullOrWhiteSpace(KategoriNama)) return;

            var kategori = new Kategori
            {
                KategoriId = Guid.NewGuid().ToString(),
                KategoriNama = KategoriNama,
                KategoriDeskripsi = KategoriDeskripsi
            };

            _dbContext.Kategori.Add(kategori);
            await _dbContext.SaveChangesAsync();
            await LoadDataAsync();

            KategoriNama = string.Empty;
            KategoriDeskripsi = string.Empty;
        }

        [RelayCommand]
        private async Task UpdateAsync()
        {
            if (SelectedKategori == null) return;

            SelectedKategori.KategoriNama = KategoriNama;
            SelectedKategori.KategoriDeskripsi = KategoriDeskripsi;

            await _dbContext.SaveChangesAsync();
            await LoadDataAsync();
        }

        [RelayCommand]
        private async Task DeleteAsync()
        {
            if (SelectedKategori == null) return;

            _dbContext.Kategori.Remove(SelectedKategori);
            await _dbContext.SaveChangesAsync();
            await LoadDataAsync();

            SelectedKategori = null;
            KategoriNama = string.Empty;
            KategoriDeskripsi = string.Empty;
        }

        [RelayCommand]
        private void SelectKategori(Kategori kategori)
        {
            SelectedKategori = kategori;
            KategoriNama = kategori.KategoriNama;
            KategoriDeskripsi = kategori.KategoriDeskripsi;
        }

        partial void OnSearchTextChanged(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _ = LoadDataAsync();
            }
            else
            {
                var filtered = KategoriList.Where(k =>
                    k.KategoriNama.Contains(value, StringComparison.OrdinalIgnoreCase) ||
                    k.KategoriDeskripsi.Contains(value, StringComparison.OrdinalIgnoreCase)).ToList();
                KategoriList = new ObservableCollection<Kategori>(filtered);
            }
        }

        [RelayCommand]
        private void UpdateKategori(Kategori kategori)
        {
            SelectedKategori = kategori;
            KategoriNama = kategori.KategoriNama;
            KategoriDeskripsi = kategori.KategoriDeskripsi;
        }

        [RelayCommand]
        private async Task DeleteKategoriAsync(Kategori kategori)
        {
            _dbContext.Kategori.Remove(kategori);
            await _dbContext.SaveChangesAsync();
            await LoadDataAsync();
        }
    }
}
