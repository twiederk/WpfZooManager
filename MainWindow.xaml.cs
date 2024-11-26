using System.Data.Entity;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace WpfZooManager
{
    public partial class MainWindow : Window
    {
        IZooManagerRepository zooManagerRepository;

        private string connectionString;

        public MainWindow()
        {
            InitializeComponent();

            // Show the login window
            LoginWindow loginWindow = new LoginWindow();
            if (loginWindow.ShowDialog() == true)
            {
                string username = loginWindow.Username;
                string password = loginWindow.Password;

                // Create the connection string using the provided username and password
                connectionString = $"Server=tcp:myzoomanager.database.windows.net,1433;Initial Catalog=ZooDB2;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;User ID={username};Password={password}";

                // Initialize the main window with the connection string
                InitializeMainWindow();
            }
            else
            {
                // Close the application if the login was not successful
                Application.Current.Shutdown();
            }
        }

        private void InitializeMainWindow()
        {
            IDbConnection db = new SqlConnection(connectionString);
            zooManagerRepository = new ZooManagerRepository(db);
            ShowZoos();
            ShowAnimals();
        }
        private void ShowZoos()
        {
            try
            {
                var zoos = zooManagerRepository.AllZoos();

                listZoos.DisplayMemberPath = "Name";
                listZoos.SelectedValuePath = "Id";
                listZoos.ItemsSource = zoos;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error");
            }
        }

        private void ShowAnimals()
        {
            try
            {
                var animals = zooManagerRepository.AllAnimals();

                listAnimals.DisplayMemberPath = "Name";
                listAnimals.SelectedValuePath = "Id";
                listAnimals.ItemsSource = animals;
            }
            catch (Exception e)
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
                    listAssociatedAnimals.ItemsSource = null;
                    return;
                }

                List<Animal> animals = zooManagerRepository.GetAssociatedAnimals(new Zoo { Id = (int)listZoos.SelectedValue });
                listAssociatedAnimals.DisplayMemberPath = "Name";
                listAssociatedAnimals.SelectedValuePath = "Id";
                listAssociatedAnimals.ItemsSource = animals;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error");
            }

        }

        private void listZoos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listZoos.SelectedItem != null)
            {
                Zoo selectedZoo = (Zoo)listZoos.SelectedItem;
                myTextBox.Text = selectedZoo.Name;
            }
            else
            {
                myTextBox.Text = string.Empty;
            }
            ShowAssociatedAnimals();

        }

        private void listAnimals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listAnimals.SelectedItem != null)
            {
                Animal selectedAnimal = (Animal)listAnimals.SelectedItem;
                myTextBox.Text = selectedAnimal.Name;
            }
            else
            {
                myTextBox.Text = string.Empty;
            }

        }

        private void DeleteZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var zoo = new Zoo { Id = (int)listZoos.SelectedValue };
                zooManagerRepository.DeleteZoo(zoo);
                ShowZoos();
                ShowAssociatedAnimals();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void DeleteAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var animal = new Animal { Id = (int)listAnimals.SelectedValue };
                zooManagerRepository.DeleteAnimal(animal);         
                ShowAnimals();
                ShowAssociatedAnimals();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void AddZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               var zoo = new Zoo { Name = myTextBox.Text };
               zooManagerRepository.AddZoo(zoo);
               ShowZoos();
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.Message, "Error");
            }
        }


        private void AddAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var animal = new Animal { Name = myTextBox.Text };
                zooManagerRepository.AddAnimal(animal);
                ShowAnimals();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void RemoveAnimalFromZoo_Click(object sender, RoutedEventArgs e)
        {
            if (listZoos.SelectedItem == null || listAnimals.SelectedItems == null)
            {
                return;
            }
            try
            {
                zooManagerRepository.RemoveAnimalFromZoo(
                    new Zoo { Id = (int)listZoos.SelectedValue },
                    new Animal { Id = (int)listAssociatedAnimals.SelectedValue }
                );
                ShowAssociatedAnimals();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void UpdateZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var zoo = new Zoo { Id = (int)listZoos.SelectedValue, Name = myTextBox.Text };
                zooManagerRepository.UpdateZoo(zoo);
                ShowZoos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void UpdateAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var animal = new Animal { Id = (int)listAnimals.SelectedValue, Name = myTextBox.Text };
                zooManagerRepository.UpdateAnimal(animal);
                ShowAnimals();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void NewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("New Menu Item Clicked");
        }

        private void listAnimals_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // Get the selected item from listAnimals
                var selectedItem = listAnimals.SelectedItem;

                if (selectedItem != null)
                {
                    // Create a DataObject containing the selected item
                    DataObject dataObject = new DataObject(selectedItem);

                    // Initialize the drag & drop operation
                    DragDrop.DoDragDrop(listAnimals, dataObject, DragDropEffects.Move);
                }
            }
        }

        private void listAssociatedAnimals_Drop(object sender, DragEventArgs dragEvent)
        {
            if (dragEvent.Data.GetDataPresent(typeof(Animal)))
            {
                // Get the dropped data
                Animal droppedAnimal = dragEvent.Data.GetData(typeof(Animal)) as Animal;

                if (droppedAnimal != null)
                {
                    zooManagerRepository.AddAnimalToZoo(
                        new Zoo { Id = (int)listZoos.SelectedValue },
                        new Animal { Id = droppedAnimal.Id }
                    );
                    ShowAssociatedAnimals();
                }
            }
        }
    }

}