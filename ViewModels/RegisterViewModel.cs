using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using pr.Data;
using pr.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace pr.ViewModels
{
    public partial class RegisterViewModel : ViewModelBase 
    {
        public Action? OnNavigateToLogin;

        [ObservableProperty]
        private string _username = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private string _alamat = string.Empty;

        [ObservableProperty]
        private string _email = string.Empty;

        [ObservableProperty]
        private string _notelp = string.Empty;

        [ObservableProperty]
        private UserRole _level = UserRole.anggota; // Default to member, admin should be set manually in DB

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        private string _successMessage = string.Empty;

        [ObservableProperty]
        private bool _isError;

        private readonly PerpusDbContext _dbContext;

        public RegisterViewModel(PerpusDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [RelayCommand]
        private async Task RegisterAsync()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Alamat) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Notelp))
            {
                ErrorMessage = "Semua field harus diisi.";
                return;
            }

            try
            {
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(Password);
                var newUserId = await GenerateNextUserIdAsync(); // Panggil metod yang sudah diperbaiki

                var user = new User
                {
                    UserId = newUserId,
                    UserUsername = Username,
                    UserPassword = hashedPassword,
                    UserName = Name,
                    UserLevel = Level,
                    UserAlamat = Alamat,
                    UserEmail = Email,
                    UserNotelp = Notelp
                };

                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();

                SuccessMessage = "Registrasi berhasil! Silakan login.";
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                Exception innerEx = ex.InnerException;
                while (innerEx != null)
                {
                    errorMessage += $"\n--> Inner Error: {innerEx.Message}";
                    innerEx = innerEx.InnerException;
                }
                ErrorMessage = $"Registrasi gagal: {errorMessage}";
                SuccessMessage = string.Empty;
                IsError = true;
            }
        }

        // --- INI BAGIAN YANG DISESUAIKAN / DIPERBAIKI ---
        private async Task<string> GenerateNextUserIdAsync()
        {
            // 1. Ambil SEMUA ID yang cocok, jangan cuma 1
            var allUserIds = await _dbContext.Users
                .Where(u => u.UserId.StartsWith("User"))
                .Select(u => u.UserId) // Hanya ambil string ID-nya
                .ToListAsync(); // <-- Mengambil SEMUA ID ke memory

            int maxId = 0; // Mulai dari 0

            // 2. Loop di C# untuk menemukan angka ID terbesar
            foreach (var id in allUserIds)
            {
                if (id != null)
                {
                    // Ambil bagian angka dari "User123" -> "123"
                    string numberPart = id.Substring(4); // "User" ada 4 karakter
                    
                    if (int.TryParse(numberPart, out int currentId))
                    {
                        if (currentId > maxId)
                        {
                            maxId = currentId; // Temukan ID numerik terbesar
                        }
                    }
                }
            }

            // 3. ID berikutnya adalah maxId + 1
            // Jika maxId = 0, nextId = 1.
            // Jika maxId = 9, nextId = 10.
            // Jika maxId = 10, nextId = 11.
            int nextId = maxId + 1;
            return $"User{nextId}";
        }
        // --- BATAS PERBAIKAN ---

        [RelayCommand]
        private void NavigateToLogin()
        {
            OnNavigateToLogin?.Invoke();
        }
    }
}