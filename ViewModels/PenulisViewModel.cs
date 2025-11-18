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
    public partial class PenulisViewModel : ViewModelBase
    {
        private readonly PerpusDbContext _dbContext;

        [ObservableProperty]
        private ObservableCollection<Penulis> _penulisList = new();

        [ObservableProperty]
        private Penulis? _selectedPenulis;

        [ObservableProperty]
        private string _penulisNama = string.Empty;

        [ObservableProperty]
        private string _penulisAlamat = string.Empty;

        [ObservableProperty]
        private string _penulisEmail = string.Empty;

        [ObservableProperty]
        private string _penulisNotelp = string.Empty;

        [ObservableProperty]
        private string _searchText = string.Empty;

        public PenulisViewModel(PerpusDbContext dbContext)
        {
            _dbContext = dbContext;
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var data = await _dbContext.Penulis.ToListAsync();
            PenulisList = new ObservableCollection<Penulis>(data);
        }

        [RelayCommand]
        private async Task InsertAsync()
        {
            if (string.IsNullOrWhiteSpace(PenulisNama)) return;

            var penulis = new Penulis
            {
                PenulisId = Guid.NewGuid().ToString(),
                PenulisNama = PenulisNama,
                PenulisAlamat = PenulisAlamat,
                PenulisEmail = PenulisEmail,
                PenulisNotelp = PenulisNotelp
            };

            _dbContext.Penulis.Add(penulis);
            await _dbContext.SaveChangesAsync();
            await LoadDataAsync();

            PenulisNama = string.Empty;
            PenulisAlamat = string.Empty;
            PenulisEmail = string.Empty;
            PenulisNotelp = string.Empty;
        }

        [RelayCommand]
        private async Task UpdateAsync()
        {
            if (SelectedPenulis == null) return;

            SelectedPenulis.PenulisNama = PenulisNama;
            SelectedPenulis.PenulisAlamat = PenulisAlamat;
            SelectedPenulis.PenulisEmail = PenulisEmail;
            SelectedPenulis.PenulisNotelp = PenulisNotelp;

            await _dbContext.SaveChangesAsync();
            await LoadDataAsync();
        }

        [RelayCommand]
        private async Task DeleteAsync()
        {
            if (SelectedPenulis == null) return;

            _dbContext.Penulis.Remove(SelectedPenulis);
            await _dbContext.SaveChangesAsync();
            await LoadDataAsync();

            SelectedPenulis = null;
            PenulisNama = string.Empty;
            PenulisAlamat = string.Empty;
            PenulisEmail = string.Empty;
            PenulisNotelp = string.Empty;
        }

        [RelayCommand]
        private void SelectPenulis(Penulis penulis)
        {
            SelectedPenulis = penulis;
            PenulisNama = penulis.PenulisNama;
            PenulisAlamat = penulis.PenulisAlamat;
            PenulisEmail = penulis.PenulisEmail;
            PenulisNotelp = penulis.PenulisNotelp;
        }

        partial void OnSearchTextChanged(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _ = LoadDataAsync();
            }
            else
            {
                var filtered = PenulisList.Where(p =>
                    p.PenulisNama.Contains(value, StringComparison.OrdinalIgnoreCase) ||
                    p.PenulisAlamat.Contains(value, StringComparison.OrdinalIgnoreCase) ||
                    p.PenulisEmail.Contains(value, StringComparison.OrdinalIgnoreCase)).ToList();
                PenulisList = new ObservableCollection<Penulis>(filtered);
            }
        }

        [RelayCommand]
        private void UpdatePenulis(Penulis penulis)
        {
            SelectedPenulis = penulis;
            PenulisNama = penulis.PenulisNama;
            PenulisAlamat = penulis.PenulisAlamat;
            PenulisEmail = penulis.PenulisEmail;
            PenulisNotelp = penulis.PenulisNotelp;
        }

        [RelayCommand]
        private async Task DeletePenulisAsync(Penulis penulis)
        {
            _dbContext.Penulis.Remove(penulis);
            await _dbContext.SaveChangesAsync();
            await LoadDataAsync();
        }
    }
}
