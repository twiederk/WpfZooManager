using System.Data;
using System.Data.Odbc;
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


namespace WpfZooManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OdbcConnection odbcConnection;

        public MainWindow()
        {
            InitializeComponent();

            string connectionString = "Driver={Microsoft Access Driver (*.mdb, *.accdb)}; Dbq=C:\\Daten\\sourcecode\\csharp\\WpfZooManager\\WpfZooManagerDB.accdb; Uid=Admin; Pwd=;";
            odbcConnection = new OdbcConnection(connectionString);
            ShowZoos();

        }

        private void ShowZoos()
        {
            try
            {
                string query = "SELECT * FROM Zoo";
                OdbcDataAdapter odbcDataAdapter = new OdbcDataAdapter(query, odbcConnection);

                using (odbcDataAdapter)
                {
                    DataTable zooTable = new DataTable();
                    odbcDataAdapter.Fill(zooTable);

                    listZoos.DisplayMemberPath = "Location";
                    listZoos.SelectedValuePath = "ID";
                    listZoos.ItemsSource = zooTable.DefaultView;
                }
            } catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error");
            }

        }

        private void ShowAssociatedAnimals()
        {
            //try
            {

                if (listZoos.SelectedValue == null)
                {
                    MessageBox.Show("Please select a zoo.", "Error");
                    return;
                }

                string query = "SELECT * FROM Animal a INNER JOIN ZooAnimal za ON a.ID = za.AnimalId WHERE za.ZooId = ?";

                OdbcCommand odbcCommand = new OdbcCommand(query, odbcConnection);
                OdbcDataAdapter odbcDataAdapter = new OdbcDataAdapter(odbcCommand);

                using (odbcDataAdapter)
                {
                    odbcCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);

                    DataTable animalTable = new DataTable();
                    odbcDataAdapter.Fill(animalTable);

                    listAssociatedAnimals.DisplayMemberPath = "AnimalName";
                    listAssociatedAnimals.SelectedValuePath = "ID";
                    listAssociatedAnimals.ItemsSource = animalTable.DefaultView;
                }
            }
            /*
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error");
            }
            */

        }

        private void listZoos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowAssociatedAnimals();
        }
    }
}