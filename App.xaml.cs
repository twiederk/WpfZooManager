using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
            collection.AddSingleton(sqliteConnection);

            IDbConnection db = new SQLiteConnection("Data Source=ZooManager.db");
            collection.AddSingleton(db);
            collection.AddSingleton<IZooManagerRepository, ZooManagerRepository>();

            var serviceProvider = collection.BuildServiceProvider();
            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }

}
