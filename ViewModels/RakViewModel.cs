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
    public partial class RakViewModel : ViewModelBase
    {
        private readonly PerpusDbContext _dbContext;

        [ObservableProperty]
        private ObservableCollection<Rak> _rakList = new();

        [ObservableProperty]
        private Rak? _selectedRak;

        [ObservableProperty]
        private string _rakNama = string.Empty;

        [ObservableProperty]
        private string _rakLokasi = string.Empty;

        [ObservableProperty]
        private string _searchText = string.Empty;

        public RakViewModel(PerpusDbContext dbContext)
        {
            _dbContext = dbContext;
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var data = await _dbContext.Rak.ToListAsync();
            RakList = new ObservableCollection<Rak>(data);
        }

        [RelayCommand]
        private async Task InsertAsync()
        {
            if (string.IsNullOrWhiteSpace(RakNama)) return;

            var rak = new Rak
            {
                RakId = Guid.NewGuid().ToString(),
                RakNama = RakNama,
                RakLokasi = RakLokasi
            };

            _dbContext.Rak.Add(rak);
            await _dbContext.SaveChangesAsync();
            await LoadDataAsync();

            RakNama = string.Empty;
            RakLokasi = string.Empty;
        }

        [RelayCommand]
        private async Task UpdateAsync()
        {
            if (SelectedRak == null) return;

            SelectedRak.RakNama = RakNama;
            SelectedRak.RakLokasi = RakLokasi;

            await _dbContext.SaveChangesAsync();
            await LoadDataAsync();
        }

        [RelayCommand]
        private async Task DeleteAsync()
        {
            if (SelectedRak == null) return;

            _dbContext.Rak.Remove(SelectedRak);
            await _dbContext.SaveChangesAsync();
            await LoadDataAsync();

            SelectedRak = null;
            RakNama = string.Empty;
            RakLokasi = string.Empty;
        }

        [RelayCommand]
        private void SelectRak(Rak rak)
        {
            SelectedRak = rak;
            RakNama = rak.RakNama;
            RakLokasi = rak.RakLokasi;
        }

        partial void OnSearchTextChanged(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _ = LoadDataAsync();
            }
            else
            {
                var filtered = RakList.Where(r =>
                    r.RakNama.Contains(value, StringComparison.OrdinalIgnoreCase) ||
                    r.RakLokasi.Contains(value, StringComparison.OrdinalIgnoreCase)).ToList();
                RakList = new ObservableCollection<Rak>(filtered);
            }
        }

        [RelayCommand]
        private void UpdateRak(Rak rak)
        {
            SelectedRak = rak;
            RakNama = rak.RakNama;
            RakLokasi = rak.RakLokasi;
        }

        [RelayCommand]
        private async Task DeleteRakAsync(Rak rak)
        {
            _dbContext.Rak.Remove(rak);
            await _dbContext.SaveChangesAsync();
            await LoadDataAsync();
        }
    }
}
