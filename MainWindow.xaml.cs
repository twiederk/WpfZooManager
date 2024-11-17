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
using System.Data.SQLite;


namespace WpfZooManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OdbcConnection odbcConnection;
        SQLiteConnection sqliteConnection;

        public MainWindow()
        {
            InitializeComponent();

            string connectionString = "Data Source=C:\\Daten\\sourcecode\\csharp\\WpfZooManager\\ZooManager.db";
            sqliteConnection = new SQLiteConnection(connectionString);
            sqliteConnection.Open();
            ShowZoos();
            ShowAnimals();

        }

        private void ShowZoos()
        {
            try
            {
                string query = "SELECT * FROM zoo";
                SQLiteDataAdapter sqLiteDataAdapter = new SQLiteDataAdapter(query, sqliteConnection);

                using (sqLiteDataAdapter)
                {
                    DataTable zooTable = new DataTable();
                    sqLiteDataAdapter.Fill(zooTable);

                    listZoos.DisplayMemberPath = "location";
                    listZoos.SelectedValuePath = "id";
                    listZoos.ItemsSource = zooTable.DefaultView;
                }
            } catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error");
            }
        }

        private void ShowAnimals()
        {
            try
            {
                string query = "SELECT * FROM animal";
                SQLiteDataAdapter sqLiteDataAdapter = new SQLiteDataAdapter(query, sqliteConnection);

                using (sqLiteDataAdapter)
                {
                    DataTable animalTable = new DataTable();
                    sqLiteDataAdapter.Fill(animalTable);

                    listAnimals.DisplayMemberPath = "name";
                    listAnimals.SelectedValuePath = "id";
                    listAnimals.ItemsSource = animalTable.DefaultView;
                }
            } catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error");
            }
        }

        private void ShowAssociatedAnimals()
        {
            try
            {

                if (listZoos.SelectedValue == null)
                {
                    MessageBox.Show("Please select a zoo.", "Error");
                    return;
                }

                string query = "SELECT za.id, a.name FROM animal a, zoo_animal za WHERE a.id = za.animal_id AND za.zoo_id = ?";

                SQLiteCommand sqliteCommand = new SQLiteCommand(query, sqliteConnection);
                SQLiteDataAdapter sqliteDataAdapter = new SQLiteDataAdapter(sqliteCommand);


                using (sqliteDataAdapter)
                {
                    sqliteCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);

                    DataTable animalTable = new DataTable();
                    sqliteDataAdapter.Fill(animalTable);

                    listAssociatedAnimals.DisplayMemberPath = "name";
                    listAssociatedAnimals.SelectedValuePath = "id";
                    listAssociatedAnimals.ItemsSource = animalTable.DefaultView;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error");
            }

        }

        private void listZoos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowAssociatedAnimals();
        }
    }
}