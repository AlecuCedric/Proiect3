using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
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
    /// Interaction logic for Pacient.xaml
    /// </summary>
    public partial class Pacient : Window
    {
        private string connectionString = "Server=G713RS;Database=spitaldb;Trusted_Connection=True;Encrypt=False;";
        public Pacient()
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
                    SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM Pacient", conn);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dataGridPacient.ItemsSource = dataTable.DefaultView;
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

                try
                {
                    // Step 1: Insert into Users table
                    string userQuery = "INSERT INTO Users (Username, PasswordHash, Role) VALUES (@Username, HASHBYTES('SHA2_256', @Password), @Role); SELECT SCOPE_IDENTITY();";
                    using (SqlCommand cmdUser = new SqlCommand(userQuery, conn))
                    {
                        // Generate username and password
                        string username = (txtNumePacient.Text + txtPrenumePacient.Text).ToLower();
                        string password = username; // Initial password is the same as the username

                        cmdUser.Parameters.AddWithValue("@Username", username);
                        cmdUser.Parameters.AddWithValue("@Password", password);
                        cmdUser.Parameters.AddWithValue("@Role", "User"); // Role is "User" for Pacient

                        // Execute and get the new UserID
                        int userId = Convert.ToInt32(cmdUser.ExecuteScalar());

                        // Step 2: Insert into Pacient table
                        string pacientQuery = "INSERT INTO Pacient (UserID, CNP, NumePacient, PrenumePacient, Adresa, Asigurare) VALUES (@UserID, @CNP, @NumePacient, @PrenumePacient, @Adresa, @Asigurare)";
                        using (SqlCommand cmdPacient = new SqlCommand(pacientQuery, conn))
                        {
                            cmdPacient.Parameters.AddWithValue("@UserID", userId);
                            cmdPacient.Parameters.AddWithValue("@CNP", txtCNP.Text);
                            cmdPacient.Parameters.AddWithValue("@NumePacient", txtNumePacient.Text);
                            cmdPacient.Parameters.AddWithValue("@PrenumePacient", txtPrenumePacient.Text);
                            cmdPacient.Parameters.AddWithValue("@Adresa", txtAdresa.Text);
                            cmdPacient.Parameters.AddWithValue("@Asigurare", txtAsigurare.Text);

                            cmdPacient.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Pacient added successfully!");
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

            // Refresh the DataGrid
            LoadData();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if a row is selected in the DataGrid
            if (dataGridPacient.SelectedItem != null)
            {
                // Cast the selected row into a DataRowView (or the type of your data model)
                DataRowView row = (DataRowView)dataGridPacient.SelectedItem;

                // Get the MedicID (primary key) of the selected record
                long PacientID = (long)row["PacientID"];  // Adjust if the name is different

                // Create SQL query for updating the Medic table
                string query = "UPDATE Pacient SET CNP = @CNP, NumePacient = @NumePacient, PrenumePacient = @PrenumePacient, Adresa = @Adresa, Asigurare = @Asigurare WHERE PacientID = @PacientID";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);

                    // Add parameters for the SQL query
                    cmd.Parameters.AddWithValue("PacientID", PacientID);
                    cmd.Parameters.AddWithValue("@CNP", txtCNP.Text);
                    cmd.Parameters.AddWithValue("@NumePacient", txtNumePacient.Text);  
                    cmd.Parameters.AddWithValue("@PrenumePacient", txtPrenumePacient.Text); 
                    cmd.Parameters.AddWithValue("@Adresa", txtAdresa.Text);
                    cmd.Parameters.AddWithValue("@Asigurare", txtAsigurare.Text);

                    try
                    {
                        // Open the connection
                        conn.Open();

                        // Execute the query
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Check if any rows were affected
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Pacient updated successfully.");
                            LoadData();  // Refresh the DataGrid
                        }
                        else
                        {
                            MessageBox.Show("Pacient update failed. Please check the data and try again.");
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
            if (dataGridPacient.SelectedItem != null)
            {
                // Cast the selected row into a DataRowView (or the type of your data model)
                DataRowView row = (DataRowView)dataGridPacient.SelectedItem;

                // Get the MedicID (primary key) of the selected record
                long PacientID = (long)row["PacientID"];  // Adjust if the name is different

                // Ask for confirmation before deleting the record
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this record?", "Delete Confirmation", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    // Create SQL query for deleting the Medic record
                    string query = "DELETE FROM Pacient WHERE PacientID = @PacientID";

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        SqlCommand cmd = new SqlCommand(query, conn);

                        // Add the MedicID parameter to the query
                        cmd.Parameters.AddWithValue("@PacientID", PacientID);

                        try
                        {
                            // Open the connection
                            conn.Open();

                            // Execute the delete query
                            int rowsAffected = cmd.ExecuteNonQuery();

                            // Check if any rows were deleted
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Pacient deleted successfully.");
                                LoadData();  // Refresh the DataGrid to reflect the changes
                            }
                            else
                            {
                                MessageBox.Show("Pacient deletion failed. Please try again.");
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
