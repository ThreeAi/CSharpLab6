using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CSharpLab6.DbConnection;
using CSharpLab6.Model;
using CSharpLab6.ViewModel.Services;
using Microsoft.EntityFrameworkCore;

namespace CSharpLab6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MyDbContext Context { get; }
        public MainWindow()
        {
            InitializeComponent();
            Context = new MyDbContext();
            Fill();
            tabClients.DataContext = new ClientViewModel(); 
            tabClimbingBuildings.DataContext = new ClimbingBuildingViewModel();
            tabVisits.DataContext = new VisitViewModel();
        }

        private void DataGrid_SelectionClientChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem is Client selectedClient)
            {
                var viewModel = tabClients.DataContext as ClientViewModel;
                viewModel?.UpdateSelectedClient(selectedClient);
            }
        }

        private void DataGrid_SelectionBuildingChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem is ClimbingBuilding selectedBuilding)
            {
                var viewModel = tabClimbingBuildings.DataContext as ClimbingBuildingViewModel;
                viewModel?.UpdateSelectedBuilding(selectedBuilding);
            }
        }

        private void DataGrid_SelectionVisitChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem is Visit selectedBuilding)
            {
                var viewModel = tabVisits.DataContext as VisitViewModel;
                viewModel?.UpdateSelectedVisit(selectedBuilding);
            }
        }

        private void Fill()
        {
            Client client1 = new Client()
            {
                Name = "Artem",
                LastName = "Alexandrov",
                FurtherName = "Alexeevich"
            };

            Client client2 = new Client()
            {
                Name = "Ivan",
                LastName = "Ivanov",
                FurtherName = "Ivanovich"
            };

            Context.Clients.AddRange(client1, client2);

            ClimbingBuilding building1 = new ClimbingBuilding()
            {
                Address = "Gazovaia st. 13",
                City = "St.Peterurg",
                Type = ClimbingWallType.Difficalty
            };

            ClimbingBuilding building2 = new ClimbingBuilding()
            {
                Address = "Lomonosova st. 4",
                City = "Moscow",
                Type = ClimbingWallType.Bouldering
            };

            Context.ClimbingBuildings.AddRange(building1, building2);

            DateTime visitDate1 = DateTime.Now.AddDays(-7);
            DateTime visitDate2 = DateTime.Now.AddDays(-3);

            Visit visit1 = new Visit()
            {
                ClientId = client1.ClientId,
                Client = client1,
                BuildingId = building1.BuildingId,
                ClimbingBuilding = building2,
                VisitDate = visitDate1.ToUniversalTime()
            };

            Visit visit3 = new Visit()
            {
                ClientId = client1.ClientId,
                Client = client1,
                BuildingId = building1.BuildingId,
                ClimbingBuilding = building2,
                VisitDate = visitDate2.ToUniversalTime()
            };

            Visit visit2 = new Visit()
            {
                ClientId = client2.ClientId,
                Client = client2,
                BuildingId = building2.BuildingId,
                ClimbingBuilding = building1,
                VisitDate = visitDate2.ToUniversalTime(),
            };

            Context.Visits.AddRange(visit1, visit2, visit3);
            Context.SaveChanges();
        }
    }
}