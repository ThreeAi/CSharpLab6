using CSharpLab6.Model;
using CSharpLab6.ViewModel.Repositories;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
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
using System.Windows.Input;

namespace CSharpLab6.ViewModel.Services
{
    internal class ClientViewModel : INotifyPropertyChanged
    {
        private ClientRepository _clientRepository = new ClientRepository();
        private ObservableCollection<Client> _clients;
        private Client _selectedClient;

        public ObservableCollection<Client> Clients
        {
            get { return _clients; }
            set
            {
                _clients = value;
                OnPropertyChanged();
            }
        }

        public Client SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                _selectedClient = value;
                OnPropertyChanged();
            }
        }

        public ClientViewModel()
        {
            LoadClients();
        }

        private void LoadClients()
        {
            Clients = new ObservableCollection<Client>(_clientRepository.GetAllClients());
        }


        private RelayCommand _delCommand;

        public RelayCommand DeleteClientCommand
        {
            get
            {
                if (_delCommand != null)
                    return _delCommand;
                else
                {
                    _delCommand = new RelayCommand(
                        () => ExecuteDelCommand(),
                        () => Clients.Contains(SelectedClient));

                    return _delCommand;
                }
            }
        }

        private void ExecuteDelCommand()
        {
            if (SelectedClient != null)
            {
                _clientRepository.DeleteClient(SelectedClient);
                LoadClients();
            }
        }

        private RelayCommand _addClientCommand;

        public RelayCommand AddClientCommand
        {
            get
            {
                if (_addClientCommand != null)
                    return _addClientCommand;
                else
                {
                    _addClientCommand = new RelayCommand(
                        () => ExecuteAddClientCommand(),
                        () => SelectedClient != null && !_clientRepository.GetAllClients().Contains(SelectedClient)

                    );

                    return _addClientCommand;
                }
            }
        }

        private void ExecuteAddClientCommand()
        {
            if (SelectedClient != null)
            {
                _clientRepository.AddClient(SelectedClient);
                LoadClients();
            }
        }

        private RelayCommand _updateClientCommand;

        public RelayCommand UpdateClientCommand
        {
            get
            {
                if (_updateClientCommand != null)
                    return _updateClientCommand;
                else
                {
                    _updateClientCommand = new RelayCommand(
                        () => ExecuteUpdateClientCommand(),
                        () => SelectedClient != null
                    );

                    return _updateClientCommand;
                }
            }
        }

        private void ExecuteUpdateClientCommand()
        {
            if (SelectedClient != null)
            {
                _clientRepository.UpdateClient(SelectedClient);
                LoadClients();
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
            return Clients.Any();
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
                    SaveToJson(Clients.ToList(), filePath);
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
                    var importedData = LoadFromJson<Client>(filePath);

                    await _clientRepository.SaveClientsAsync(importedData);
                    LoadClients();
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

        public void UpdateSelectedClient(Client client)
        {
            SelectedClient = client;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
