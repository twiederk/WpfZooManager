using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Data.SQLite;
using Dapper;


namespace WpfZooManager
{
    public partial class MainWindow : Window
    {
        IZooManagerRepository zooManagerRepository;

        public MainWindow(IZooManagerRepository zooManagerRepository)
        {
            InitializeComponent();

            this.zooManagerRepository = zooManagerRepository;

            ShowZoos();
            ShowAnimals();
        }

        private void ShowZoos()
        {
            try
            {
                var zoos = zooManagerRepository.AllZoos();

                listZoos.DisplayMemberPath = "Location";
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
                myTextBox.Text = selectedZoo.Location;
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
                var zoo = new Zoo { Location = myTextBox.Text };
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

        private void AddAnimalToZoo_Click(object sender, RoutedEventArgs e)
        {
            if (listZoos.SelectedItem == null || listAnimals.SelectedItems == null)
            {
                return;
            }
            try
            {
                zooManagerRepository.AddAnimalToZoo(
                    new Zoo { Id = (int)listZoos.SelectedValue },
                    new Animal { Id = (int)listAnimals.SelectedValue }
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
                var zoo = new Zoo { Id = (int)listZoos.SelectedValue, Location = myTextBox.Text };
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
    }

}