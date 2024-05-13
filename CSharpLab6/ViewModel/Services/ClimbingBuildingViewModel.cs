using CSharpLab6.Model;
using CSharpLab6.ViewModel.Repositories;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CSharpLab6.ViewModel.Services
{
    internal class ClimbingBuildingViewModel : INotifyPropertyChanged
    {
        private ClimbingBuildingRepository _buildingRepository = new ClimbingBuildingRepository();
        private ObservableCollection<ClimbingBuilding> _buildings;
        private ClimbingBuilding _selectedBuilding;

        public ObservableCollection<ClimbingBuilding> Buildings
        {
            get { return _buildings; }
            set
            {
                _buildings = value;
                OnPropertyChanged();
            }
        }

        public ClimbingBuilding SelectedBuilding
        {
            get { return _selectedBuilding; }
            set
            {
                _selectedBuilding = value;
                OnPropertyChanged();
            }
        }

        public ClimbingBuildingViewModel()
        {
            LoadBuildings();
        }

        private void LoadBuildings()
        {
            Buildings = new ObservableCollection<ClimbingBuilding>(_buildingRepository.GetAllClimbingBuildings());
        }

        private RelayCommand _delCommand;

        public RelayCommand DeleteBuildingCommand
        {
            get
            {
                if (_delCommand != null)
                    return _delCommand;
                else
                {
                    _delCommand = new RelayCommand(
                        () => ExecuteDelCommand(),
                        () => SelectedBuilding != null
                    );

                    return _delCommand;
                }
            }
        }

        private void ExecuteDelCommand()
        {
            if (SelectedBuilding != null)
            {
                _buildingRepository.DeleteClimbingBuilding(SelectedBuilding);
                LoadBuildings();
            }
        }

        private RelayCommand _addBuildingCommand;

        public RelayCommand AddBuildingCommand
        {
            get
            {
                if (_addBuildingCommand != null)
                    return _addBuildingCommand;
                else
                {
                    _addBuildingCommand = new RelayCommand(
                        () => ExecuteAddBuildingCommand(),
                        () => SelectedBuilding != null && !_buildingRepository.GetAllClimbingBuildings().Contains(SelectedBuilding)

                    );

                    return _addBuildingCommand;
                }
            }
        }

        private void ExecuteAddBuildingCommand()
        {
            if (SelectedBuilding != null)
            {
                _buildingRepository.AddClimbingBuilding(SelectedBuilding);
                LoadBuildings();
            }
        }

        private RelayCommand _updateBuildingCommand;

        public RelayCommand UpdateBuildingCommand
        {
            get
            {
                if (_updateBuildingCommand != null)
                    return _updateBuildingCommand;
                else
                {
                    _updateBuildingCommand = new RelayCommand(
                        () => ExecuteUpdateBuildingCommand(),
                        () => Buildings.Contains(SelectedBuilding)
                    );

                    return _updateBuildingCommand;
                }
            }
        }

        private void ExecuteUpdateBuildingCommand()
        {
            if (SelectedBuilding != null)
            {
                _buildingRepository.UpdateClimbingBuilding(SelectedBuilding);
                LoadBuildings();
            }
        }

        private RelayCommand _exportToJsonCommand;
        public RelayCommand ExportToJsonCommand
        {
            get
            {
                return _exportToJsonCommand ??= new RelayCommand(
                    () => ExecuteExportToJsonCommand(),
                    () => CanExportToJson()
                );
            }
        }

        private bool CanExportToJson()
        {
            return Buildings.Any();
        }

        private void ExecuteExportToJsonCommand()
        {
            try
            {
                var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                saveFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;
                    SaveToJson(Buildings.ToList(), filePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error exporting to JSON: {ex.Message}");
            }
        }

        public void SaveToJson<T>(List<T> data, string filePath)
        {
            string jsonString = JsonSerializer.Serialize(data);

            File.WriteAllText(filePath, jsonString);
        }

        private RelayCommand _importFromJsonCommand;

        public RelayCommand ImportFromJsonCommand
        {
            get
            {
                return _importFromJsonCommand ?? (_importFromJsonCommand = new RelayCommand(
                    () => ExecuteImportFromJsonCommand()
                ));
            }
        }

        private async void ExecuteImportFromJsonCommand()
        {
            try
            {
                var openFileDialog = new Microsoft.Win32.OpenFileDialog();
                openFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == true)
                {
                    string filePath = openFileDialog.FileName;
                    var importedData = LoadFromJson<ClimbingBuilding>(filePath);

                    await _buildingRepository.SaveClimbingBuildingsAsync(importedData);
                    LoadBuildings();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error importing from JSON: {ex.Message}");
            }
        }

        public List<T> LoadFromJson<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File '{filePath}' not found.");
            }

            string jsonString = File.ReadAllText(filePath);

            return JsonSerializer.Deserialize<List<T>>(jsonString);
        }

        public void UpdateSelectedBuilding(ClimbingBuilding building)
        {
            SelectedBuilding = building;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
