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
    public partial class SiswaViewModel : ViewModelBase
    {
        private readonly PerpusDbContext _dbContext;

        [ObservableProperty]
        private ObservableCollection<User> _siswaList = new();

        [ObservableProperty]
        private User? _selectedSiswa;

        [ObservableProperty]
        private string _siswaName = string.Empty;

        [ObservableProperty]
        private string _siswaAlamat = string.Empty;

        [ObservableProperty]
        private string _siswaUsername = string.Empty;

        [ObservableProperty]
        private string _siswaEmail = string.Empty;

        [ObservableProperty]
        private string _siswaNotelp = string.Empty;

        [ObservableProperty]
        private string _searchText = string.Empty;

        public SiswaViewModel(PerpusDbContext dbContext)
        {
            _dbContext = dbContext;
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var data = await _dbContext.Users.ToListAsync();
            SiswaList = new ObservableCollection<User>(data);
        }

        [RelayCommand]
        private async Task InsertAsync()
        {
            if (string.IsNullOrWhiteSpace(SiswaName)) return;

            var user = new User
            {
                UserId = Guid.NewGuid().ToString(),
                UserName = SiswaName,
                UserAlamat = SiswaAlamat,
                UserUsername = SiswaUsername,
                UserEmail = SiswaEmail,
                UserNotelp = SiswaNotelp,
                UserPassword = "defaultpassword", // You might want to handle this differently
                UserLevel = UserRole.anggota
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            await LoadDataAsync();

            SiswaName = string.Empty;
            SiswaAlamat = string.Empty;
            SiswaUsername = string.Empty;
            SiswaEmail = string.Empty;
            SiswaNotelp = string.Empty;
        }

        [RelayCommand]
        private async Task UpdateAsync()
        {
            if (SelectedSiswa == null) return;

            SelectedSiswa.UserName = SiswaName;
            SelectedSiswa.UserAlamat = SiswaAlamat;
            SelectedSiswa.UserUsername = SiswaUsername;
            SelectedSiswa.UserEmail = SiswaEmail;
            SelectedSiswa.UserNotelp = SiswaNotelp;

            await _dbContext.SaveChangesAsync();
            await LoadDataAsync();
        }

        [RelayCommand]
        private async Task DeleteAsync()
        {
            if (SelectedSiswa == null) return;

            _dbContext.Users.Remove(SelectedSiswa);
            await _dbContext.SaveChangesAsync();
            await LoadDataAsync();

            SelectedSiswa = null;
            SiswaName = string.Empty;
            SiswaAlamat = string.Empty;
            SiswaUsername = string.Empty;
            SiswaEmail = string.Empty;
            SiswaNotelp = string.Empty;
        }

        [RelayCommand]
        private void SelectSiswa(User user)
        {
            SelectedSiswa = user;
            SiswaName = user.UserName;
            SiswaAlamat = user.UserAlamat;
            SiswaUsername = user.UserUsername;
            SiswaEmail = user.UserEmail;
            SiswaNotelp = user.UserNotelp;
        }

        partial void OnSearchTextChanged(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _ = LoadDataAsync();
            }
            else
            {
                var filtered = SiswaList.Where(s =>
                    s.UserName.Contains(value, StringComparison.OrdinalIgnoreCase) ||
                    s.UserUsername.Contains(value, StringComparison.OrdinalIgnoreCase) ||
                    s.UserEmail.Contains(value, StringComparison.OrdinalIgnoreCase)).ToList();
                SiswaList = new ObservableCollection<User>(filtered);
            }
        }

        [RelayCommand]
        private void UpdateUser(User user)
        {
            SelectedSiswa = user;
            SiswaName = user.UserName;
            SiswaAlamat = user.UserAlamat;
            SiswaUsername = user.UserUsername;
            SiswaEmail = user.UserEmail;
            SiswaNotelp = user.UserNotelp;
        }

        [RelayCommand]
        private async Task DeleteUserAsync(User user)
        {
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
            await LoadDataAsync();
        }
    }
}
