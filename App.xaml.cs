using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace WpfZooManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var collection = new ServiceCollection();

            collection.AddSingleton<MainWindow>();

            string connectionString = "Data Source=ZooManager.db";
            SQLiteConnection sqliteConnection = new SQLiteConnection(connectionString);
            sqliteConnection.Open();
            collection.AddSingleton<SQLiteConnection>(sqliteConnection);

            IDbConnection db = new SQLiteConnection("Data Source=ZooManager.db");
            collection.AddSingleton<IDbConnection>(db);

            var serviceProvider = collection.BuildServiceProvider();
            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }

}
