using System.Data;
using System.Data.SqlClient;
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

            //string server = Environment.GetEnvironmentVariable("DB_SERVER");
            //string database = Environment.GetEnvironmentVariable("DB_DATABASE");
            //string userId = Environment.GetEnvironmentVariable("DB_USER");
            //string password = Environment.GetEnvironmentVariable("DB_PASSWORD");

            //string connectionString = $"Server=tcp:{server},1433;Initial Catalog={database};Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;User ID={userId};Password={password}";
            //IDbConnection db = new SqlConnection(connectionString);
            //collection.AddSingleton(db);
            //collection.AddSingleton<IZooManagerRepository, ZooManagerRepository>();

            var serviceProvider = collection.BuildServiceProvider();
            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }

}
