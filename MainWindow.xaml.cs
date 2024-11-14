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
        public MainWindow()
        {
            InitializeComponent();

            // Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Daten\sourcecode\csharp\WpfToDoApp\WpfToDoDB.accdb
            string connectionString = "Driver={Microsoft Access Driver (*.mdb, *.accdb)}; Dbq=C:\\Daten\\sourcecode\\csharp\\WpfToDoApp\\WpfToDoDB.accdb; Uid=Admin; Pwd=;";

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {

                string query = "SELECT id, location FROM Zoo"; 
                OdbcCommand command = new OdbcCommand(query, connection);
                connection.Open();
                
                OdbcDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    TextBlock todoItem = new TextBlock
                    {
                        Text = reader.GetString(1),
                        Margin = new Thickness(10)
                    };
                    ZooList.Children.Add(todoItem);

                }

            }
        }
    }
}