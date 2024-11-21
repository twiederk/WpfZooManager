﻿using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Data.SQLite;
using Dapper;


namespace WpfZooManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SQLiteConnection sqliteConnection;
        IDbConnection db;


        public MainWindow()
        {
            InitializeComponent();

            string connectionString = "Data Source=ZooManager.db";
            sqliteConnection = new SQLiteConnection(connectionString);
            sqliteConnection.Open();

            db = new SQLiteConnection("Data Source=ZooManager.db");

            ShowZoos();
            ShowAnimals();
        }

        private void ShowZoos()
        {
            try
            {
                var sql = "SELECT * FROM zoo";
                var zoos = db.Query<Zoo>(sql).ToList();

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
                string sql = "SELECT * FROM animal";
                var animals = db.Query<Animal>(sql).ToList();

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
                string query = "DELETE FROM zoo WHERE id = ?";
                SQLiteCommand sqliteCommand = new SQLiteCommand(query, sqliteConnection);
                sqliteCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                sqliteCommand.ExecuteNonQuery();

                string query2 = "DELETE FROM zoo_animal WHERE zoo_id = ?";
                SQLiteCommand sqliteCommand2 = new SQLiteCommand(query2, sqliteConnection);
                sqliteCommand2.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                sqliteCommand2.ExecuteNonQuery();
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
                string query = "DELETE FROM animal WHERE id = ?";
                SQLiteCommand sqliteCommand = new SQLiteCommand(query, sqliteConnection);
                sqliteCommand.Parameters.AddWithValue("@AnimalId", listAnimals.SelectedValue);
                sqliteCommand.ExecuteNonQuery();               

                string query2 = "DELETE FROM zoo_animal WHERE animal_id = ?";
                SQLiteCommand sqliteCommand2 = new SQLiteCommand(query2, sqliteConnection);
                sqliteCommand2.Parameters.AddWithValue("@AnimalId", listAnimals.SelectedValue);
                sqliteCommand2.ExecuteNonQuery();               
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
                string query = "INSERT INTO zoo (location) VALUES (?)";
                SQLiteCommand sqliteCommand = new SQLiteCommand(query, sqliteConnection);
                sqliteCommand.Parameters.AddWithValue("@location", myTextBox.Text);
                sqliteCommand.ExecuteNonQuery();
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
                string query = "INSERT INTO animal (name) VALUES (?)";
                SQLiteCommand sqliteCommand = new SQLiteCommand(query, sqliteConnection);
                sqliteCommand.Parameters.AddWithValue("@Name", myTextBox.Text);
                sqliteCommand.ExecuteNonQuery();
                ShowAnimals();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void RemoveAnimalFromZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "DELETE FROM zoo_animal WHERE id = ?";

                SQLiteCommand sqliteCommand = new SQLiteCommand(query, sqliteConnection);
                sqliteCommand.Parameters.AddWithValue("@Id", listAssociatedAnimals.SelectedValue);

                sqliteCommand.ExecuteNonQuery();               
                ShowAssociatedAnimals();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void AddAnimalToZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "INSERT INTO zoo_animal (zoo_id, animal_id) VALUES (?, ?)";
                SQLiteCommand sqliteCommand = new SQLiteCommand(query, sqliteConnection);
                sqliteCommand.Parameters.AddWithValue("@zoo_id", listZoos.SelectedValue);
                sqliteCommand.Parameters.AddWithValue("@animal_id", listAnimals.SelectedValue);
                sqliteCommand.ExecuteNonQuery();
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
                string query = "UPDATE zoo SET location = ? WHERE id = ?";
                SQLiteCommand sqliteCommand = new SQLiteCommand(query, sqliteConnection);
                sqliteCommand.Parameters.AddWithValue("@location", myTextBox.Text);
                sqliteCommand.Parameters.AddWithValue("@id", listZoos.SelectedValue);
                sqliteCommand.ExecuteNonQuery();
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
                string query = "UPDATE animal SET name = ? WHERE id = ?";
                SQLiteCommand sqliteCommand = new SQLiteCommand(query, sqliteConnection);
                sqliteCommand.Parameters.AddWithValue("@name", myTextBox.Text);
                sqliteCommand.Parameters.AddWithValue("@id", listAnimals.SelectedValue);
                sqliteCommand.ExecuteNonQuery();
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