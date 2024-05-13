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

        //private void TabItem_Loaded(object sender, RoutedEventArgs e)
        //{
        //    var viewModel = tabVisits.DataContext as VisitViewModel;
        //    viewModel?.LoadVisits();
        //}

        private void Fill()
        {
            //Client client = new Client();
            //client.Name = "artem";
            //client.LastName = "lastname";
            //client.FurtherName = "furthername";

            //Context.Clients.Add(client);
            //Context.SaveChanges();
        }
    }
}