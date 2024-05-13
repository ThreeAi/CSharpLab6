using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLab6.Model
{
    internal class Client : INotifyPropertyChanged
    {
        private int clientId;
        private string name;
        private string lastName;
        private string furtherName;
        private ObservableCollection<Visit> visits = new ObservableCollection<Visit>();

        public int ClientId
        {
            get => clientId;
            set
            {
                if (value == clientId) return;
                clientId = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => name;
            set
            {
                if (value == name) return;
                name = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get => lastName;
            set
            {
                if (value == lastName) return;
                lastName = value;
                OnPropertyChanged();
            }
        }

        public string FurtherName
        {
            get => furtherName;
            set
            {
                furtherName = value;
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
