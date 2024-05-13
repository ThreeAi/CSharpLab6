using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLab6.Model
{
    public enum ClimbingWallType
    {
        None,
        Difficalty,
        Speed,
        Bouldering
    }
    internal class ClimbingBuilding : INotifyPropertyChanged
    {
        private int buildingId;
        private ClimbingWallType type;
        private string address;
        private string city;
        private ObservableCollection<Visit> visits = new ObservableCollection<Visit>();

        public int BuildingId
        {
            get => buildingId;
            set
            {
                if (value == buildingId) return;
                buildingId = value;
                OnPropertyChanged();
            }
        }

        public ClimbingWallType Type
        {
            get => type;
            set
            {
                if (value == type) return;
                type = value;
                OnPropertyChanged();
            }
        }

        public string Address
        {
            get => address;
            set
            {
                if (value == address) return;
                address = value;
                OnPropertyChanged();
            }
        }

        public string City
        {
            get => city;
            set
            {
                if (value == city) return;
                city = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Visit> Visits
        {
            get => visits;
            set
            {
                if (Equals(value, visits)) return;
                visits = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
