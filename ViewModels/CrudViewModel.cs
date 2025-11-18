using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using pr.Data;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using pr.Models;
using System.Reflection;
using System;

namespace pr.ViewModels;

public partial class CrudViewModel : ViewModelBase
{
    private readonly PerpusDbContext _dbContext;
    private readonly System.Type _entityType;
    private readonly string _idPropertyName;

    [ObservableProperty]
    private ObservableCollection<object> _items = new();

    [ObservableProperty]
    private object? _selectedItem;

    [ObservableProperty]
    private bool _isEditing = false;

    [ObservableProperty]
    private string _tableTitle = string.Empty;

    public CrudViewModel(PerpusDbContext dbContext, System.Type entityType, string idPropertyName)
    {
        _dbContext = dbContext;
        _entityType = entityType;
        _idPropertyName = idPropertyName;
        TableTitle = $"Master {entityType.Name}";
        _ = LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        var method = typeof(PerpusDbContext).GetMethod("Set", BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null)?.MakeGenericMethod(_entityType);
        if (method != null)
        {
            var dbSet = method.Invoke(_dbContext, null);
            if (dbSet is IQueryable queryable)
            {
                var toListMethod = typeof(EntityFrameworkQueryableExtensions).GetMethod("ToListAsync", BindingFlags.Public | BindingFlags.Static)?.MakeGenericMethod(_entityType);
                if (toListMethod != null)
                {
                    var task = (Task)toListMethod.Invoke(null, new object[] { queryable });
                    if (task != null)
                    {
                        await task;
                        var resultProperty = task.GetType().GetProperty("Result");
                        if (resultProperty != null)
                        {
                            var list = resultProperty.GetValue(task) as IEnumerable<object>;
                            if (list != null)
                            {
                                Items = new ObservableCollection<object>(list);
                            }
                        }
                    }
                }
            }
        }
    }

    [RelayCommand]
    private void AddNew()
    {
        SelectedItem = System.Activator.CreateInstance(_entityType);
        IsEditing = true;
    }

    [RelayCommand]
    private void Edit(object item)
    {
        SelectedItem = item;
        IsEditing = true;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (SelectedItem != null)
        {
            var property = _entityType.GetProperty(_idPropertyName);
            var idValue = property?.GetValue(SelectedItem);

            if (idValue != null && !string.IsNullOrEmpty(idValue.ToString()))
            {
                // Update
                _dbContext.Update(SelectedItem);
            }
            else
            {
                // Insert
                _dbContext.Add(SelectedItem);
            }

            await _dbContext.SaveChangesAsync();
            IsEditing = false;
            SelectedItem = null;
            await LoadDataAsync();
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        IsEditing = false;
        SelectedItem = null;
    }

    [RelayCommand]
    private async Task DeleteAsync(object item)
    {
        if (item != null)
        {
            _dbContext.Remove(item);
            await _dbContext.SaveChangesAsync();
            await LoadDataAsync();
        }
    }
}
