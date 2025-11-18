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
    public partial class PenerbitViewModel : ViewModelBase
    {
        private readonly PerpusDbContext _dbContext;

        [ObservableProperty]
        private ObservableCollection<Penerbit> _penerbitList = new();

        [ObservableProperty]
        private Penerbit? _selectedPenerbit;

        [ObservableProperty]
        private string _penerbitNama = string.Empty;

        [ObservableProperty]
        private string _penerbitAlamat = string.Empty;

        [ObservableProperty]
        private string _penerbitEmail = string.Empty;

        [ObservableProperty]
        private string _penerbitNotelp = string.Empty;

        [ObservableProperty]
        private string _searchText = string.Empty;

        public PenerbitViewModel(PerpusDbContext dbContext)
        {
            _dbContext = dbContext;
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var data = await _dbContext.Penerbit.ToListAsync();
            PenerbitList = new ObservableCollection<Penerbit>(data);
        }

        [RelayCommand]
        private async Task InsertAsync()
        {
            if (string.IsNullOrWhiteSpace(PenerbitNama)) return;

            var penerbit = new Penerbit
            {
                PenerbitId = Guid.NewGuid().ToString(),
                PenerbitNama = PenerbitNama,
                PenerbitAlamat = PenerbitAlamat,
                PenerbitEmail = PenerbitEmail,
                PenerbitNotelp = PenerbitNotelp
            };

            _dbContext.Penerbit.Add(penerbit);
            await _dbContext.SaveChangesAsync();
            await LoadDataAsync();

            PenerbitNama = string.Empty;
            PenerbitAlamat = string.Empty;
            PenerbitEmail = string.Empty;
            PenerbitNotelp = string.Empty;
        }

        [RelayCommand]
        private async Task UpdateAsync()
        {
            if (SelectedPenerbit == null) return;

            SelectedPenerbit.PenerbitNama = PenerbitNama;
            SelectedPenerbit.PenerbitAlamat = PenerbitAlamat;
            SelectedPenerbit.PenerbitEmail = PenerbitEmail;
            SelectedPenerbit.PenerbitNotelp = PenerbitNotelp;

            await _dbContext.SaveChangesAsync();
            await LoadDataAsync();
        }

        [RelayCommand]
        private async Task DeleteAsync()
        {
            if (SelectedPenerbit == null) return;

            _dbContext.Penerbit.Remove(SelectedPenerbit);
            await _dbContext.SaveChangesAsync();
            await LoadDataAsync();

            SelectedPenerbit = null;
            PenerbitNama = string.Empty;
            PenerbitAlamat = string.Empty;
            PenerbitEmail = string.Empty;
            PenerbitNotelp = string.Empty;
        }

        [RelayCommand]
        private void SelectPenerbit(Penerbit penerbit)
        {
            SelectedPenerbit = penerbit;
            PenerbitNama = penerbit.PenerbitNama;
            PenerbitAlamat = penerbit.PenerbitAlamat;
            PenerbitEmail = penerbit.PenerbitEmail;
            PenerbitNotelp = penerbit.PenerbitNotelp;
        }

        partial void OnSearchTextChanged(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _ = LoadDataAsync();
            }
            else
            {
                var filtered = PenerbitList.Where(p =>
                    p.PenerbitNama.Contains(value, StringComparison.OrdinalIgnoreCase) ||
                    p.PenerbitAlamat.Contains(value, StringComparison.OrdinalIgnoreCase) ||
                    p.PenerbitEmail.Contains(value, StringComparison.OrdinalIgnoreCase)).ToList();
                PenerbitList = new ObservableCollection<Penerbit>(filtered);
            }
        }

        [RelayCommand]
        private void UpdatePenerbit(Penerbit penerbit)
        {
            SelectedPenerbit = penerbit;
            PenerbitNama = penerbit.PenerbitNama;
            PenerbitAlamat = penerbit.PenerbitAlamat;
            PenerbitEmail = penerbit.PenerbitEmail;
            PenerbitNotelp = penerbit.PenerbitNotelp;
        }

        [RelayCommand]
        private async Task DeletePenerbitAsync(Penerbit penerbit)
        {
            _dbContext.Penerbit.Remove(penerbit);
            await _dbContext.SaveChangesAsync();
            await LoadDataAsync();
        }
    }
}
