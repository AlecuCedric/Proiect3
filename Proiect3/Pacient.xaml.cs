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
                string query = "INSERT INTO Pacient (CNP, NumePacient, PrenumePacient, Adresa, Asigurare) VALUES (@CNP, @NumePacient, @PrenumePacient, @Adresa, @Asigurare)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CNP", txtCNP.Text);
                cmd.Parameters.AddWithValue("@NumePacient", txtNumePacient.Text);
                cmd.Parameters.AddWithValue("@PrenumePacient", txtPrenumePacient.Text);
                cmd.Parameters.AddWithValue("@Adresa", txtAdresa.Text);
                cmd.Parameters.AddWithValue("@Asigurare", txtAsigurare.Text);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
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
                    cmd.Parameters.AddWithValue("CNP", txtCNP.Text);
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
            MainWindow MainWindow = new MainWindow();
            MainWindow.Show();
            this.Close();
        }
    }
}
