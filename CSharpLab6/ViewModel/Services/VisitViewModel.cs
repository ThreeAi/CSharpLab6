using CSharpLab6.Model;
using CSharpLab6.ViewModel.Repositories;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CSharpLab6.ViewModel.Services
{
    internal class VisitViewModel : INotifyPropertyChanged
    {
        private readonly VisitRepository _visitRepository = new VisitRepository();
        private readonly ClientRepository _clientRepository = new ClientRepository();
        private readonly ClimbingBuildingRepository _buildingRepository = new ClimbingBuildingRepository();
        private ObservableCollection<Visit> _visits;
        private Visit _selectedVisit;
        private ObservableCollection<string> _clientsLastName;
        private ObservableCollection<string> _buildingsAddress;

        public ObservableCollection<Visit> Visits
        {
            get { return _visits; }
            set
            {
                _visits = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> ClientsLastName 
        {
            get { return _clientsLastName; }
            set
            {
                _clientsLastName = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> BuildingsAddress
        {
            get { return _buildingsAddress; }
            set
            {
                _buildingsAddress = value;
                OnPropertyChanged();
            }
        }

        public Visit SelectedVisit
        {
            get { return _selectedVisit; }
            set
            {
                _selectedVisit = value;
                OnPropertyChanged();
            }
        }

        public VisitViewModel()
        {
            LoadVisits();
        }

        public void LoadVisits()
        {
            Visits = new ObservableCollection<Visit>(_visitRepository.GetAllVisits());
            ClientsLastName = new ObservableCollection<string>(_clientRepository.GetAllClients().Select(c => c.LastName));
            BuildingsAddress = new ObservableCollection<string>(_buildingRepository.GetAllClimbingBuildings().Select(b => b.Address));
        }

        private RelayCommand _addVisitCommand;

        public RelayCommand AddVisitCommand
        {
            get
            {
                if (_addVisitCommand != null)
                    return _addVisitCommand;
                else
                {
                    _addVisitCommand = new RelayCommand(
                        () => ExecuteAddVisitCommand(),
                        () => SelectedVisit != null
                    );

                    return _addVisitCommand;
                }
            }
        }

        private void ExecuteAddVisitCommand()
        {
            if (SelectedVisit != null)
            {
                _visitRepository.AddVisit(SelectedVisit);
                LoadVisits();
            }
        }

        private RelayCommand _deleteVisitCommand;

        public RelayCommand DeleteVisitCommand
        {
            get
            {
                if (_deleteVisitCommand != null)
                    return _deleteVisitCommand;
                else
                {
                    _deleteVisitCommand = new RelayCommand(
                        () => ExecuteDeleteVisitCommand(),
                        () => Visits.Contains(SelectedVisit));

                    return _deleteVisitCommand;
                }
            }
        }

        private void ExecuteDeleteVisitCommand()
        {
            if (SelectedVisit != null)
            {
                _visitRepository.DeleteVisit(SelectedVisit);
                LoadVisits();
            }
        }

        public void UpdateSelectedVisit(Visit selectedVisit)
        {
            SelectedVisit = selectedVisit;
        }

        public IEnumerable<string> ClientLastNames => Visits.Select(v => v.Client.LastName).Distinct().ToList();
        public IEnumerable<string> BuildingAddresses => Visits.Select(v => v.ClimbingBuilding.Address).Distinct().ToList();

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
