using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Proiect3
{
    /// <summary>
    /// Interaction logic for Medic.xaml
    /// </summary>
    public partial class Medic : Window
    {
        private string connectionString = "Server=G713RS;Database=spitaldb;Trusted_Connection=True;Encrypt=False;";
        public Medic()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM Medic", conn);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dataGridMedic.ItemsSource = dataTable.DefaultView;
                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally 
                {
                    conn.Close();
                }
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Step 1: Insert into Users table
                string userQuery = "INSERT INTO Users (Username, PasswordHash, Role) VALUES (@Username, HASHBYTES('SHA2_256', @Password), @Role); SELECT SCOPE_IDENTITY();";
                using (SqlCommand cmdUser = new SqlCommand(userQuery, conn))
                {
                    // Generate username and password
                    string username = (txtNumeMedic.Text + txtPrenumeMedic.Text).ToLower();
                    string password = username; // Initial password is the same as the username

                    cmdUser.Parameters.AddWithValue("@Username", username);
                    cmdUser.Parameters.AddWithValue("@Password", password);
                    cmdUser.Parameters.AddWithValue("@Role", "Admin");

                    try
                    {
                        // Execute and get the new UserID
                        int userId = Convert.ToInt32(cmdUser.ExecuteScalar());

                        // Step 2: Insert into Medic table
                        string medicQuery = "INSERT INTO Medic (UserID, NumeMedic, PrenumeMedic, Specializare) VALUES (@UserID, @NumeMedic, @PrenumeMedic, @Specializare)";
                        using (SqlCommand cmdMedic = new SqlCommand(medicQuery, conn))
                        {
                            cmdMedic.Parameters.AddWithValue("@UserID", userId);
                            cmdMedic.Parameters.AddWithValue("@NumeMedic", txtNumeMedic.Text);
                            cmdMedic.Parameters.AddWithValue("@PrenumeMedic", txtPrenumeMedic.Text);
                            cmdMedic.Parameters.AddWithValue("@Specializare", txtSpecializare.Text);

                            cmdMedic.ExecuteNonQuery();
                        }

                        MessageBox.Show("Medic added successfully!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }

                conn.Close();
            }

            // Refresh the DataGrid
            LoadData();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if a row is selected in the DataGrid
            if (dataGridMedic.SelectedItem != null)
            {
                DataRowView row = (DataRowView)dataGridMedic.SelectedItem;

                // Get the MedicID (primary key) of the selected record
                long medicID = (long)row["MedicID"];

                //SQL query for updating the Medic table
                string query = "UPDATE Medic SET NumeMedic = @NumeMedic, PrenumeMedic = @PrenumeMedic, Specializare = @Specializare WHERE MedicID = @MedicID";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);

                    // Add parameters for the SQL query
                    cmd.Parameters.AddWithValue("@MedicID", medicID);
                    cmd.Parameters.AddWithValue("@NumeMedic", txtNumeMedic.Text);
                    cmd.Parameters.AddWithValue("@PrenumeMedic", txtPrenumeMedic.Text);
                    cmd.Parameters.AddWithValue("@Specializare", txtSpecializare.Text);

                    try
                    {
                        // Open the connection
                        conn.Open();

                        // Execute the query
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Check if any rows were affected
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Medic updated successfully.");
                            LoadData();  // Refresh the DataGrid
                        }
                        else
                        {
                            MessageBox.Show("Medic update failed. Please check the data and try again.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a row to edit.");
            }
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if a row is selected in the DataGrid
            if (dataGridMedic.SelectedItem != null)
            {
                DataRowView row = (DataRowView)dataGridMedic.SelectedItem;

                // Get the MedicID (primary key) of the selected record
                long medicID = (long)row["MedicID"];

                // Ask for confirmation before deleting the record
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this record?", "Delete Confirmation", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    // Create SQL query for deleting the Medic record
                    string query = "DELETE FROM Medic WHERE MedicID = @MedicID";

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        SqlCommand cmd = new SqlCommand(query, conn);

                        cmd.Parameters.AddWithValue("@MedicID", medicID);

                        try
                        {
                            // Open the connection
                            conn.Open();

                            // Execute the delete query
                            int rowsAffected = cmd.ExecuteNonQuery();

                            // Check if any rows were deleted
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Medic deleted successfully.");
                                LoadData();  // Refresh the DataGrid to reflect the changes
                            }
                            else
                            {
                                MessageBox.Show("Medic deletion failed. Please try again.");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a row to delete.");
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            string username = "current_username";
            MainWindow MainWindow = new MainWindow(username);
            MainWindow.Show();
            this.Close();
        }
    }
}
