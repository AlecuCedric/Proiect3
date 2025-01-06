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
    /// Interaction logic for Medicamente.xaml
    /// </summary>
    public partial class Medicamente : Window
    {
        private string connectionString = "Server=G713RS;Database=spitaldb;Trusted_Connection=True;Encrypt=False;";
        public Medicamente()
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
                    SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM Medicamente", conn);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dataGridMedicamente.ItemsSource = dataTable.DefaultView;
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
                string query = "INSERT INTO Medicamente (Denumire) VALUES (@Denumire)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Denumire", txtDenumire.Text);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            LoadData();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if a row is selected in the DataGrid
            if (dataGridMedicamente.SelectedItem != null)
            {
                // Cast the selected row into a DataRowView (or the type of your data model)
                DataRowView row = (DataRowView)dataGridMedicamente.SelectedItem;

                // Get the MedicID (primary key) of the selected record
                long MedicamentID = (long)row["MedicamentID"];  // Adjust if the name is different

                // Create SQL query for updating the Medic table
                string query = "UPDATE Medicamente SET Denumire = @Denumire WHERE MedicamentID = @MedicamentID";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);

                    // Add parameters for the SQL query
                    cmd.Parameters.AddWithValue("MedicamentID", MedicamentID);
                    cmd.Parameters.AddWithValue("@Denumire", txtDenumire.Text);
                    

                    try
                    {
                        // Open the connection
                        conn.Open();

                        // Execute the query
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Check if any rows were affected
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Medicamente updated successfully.");
                            LoadData();  // Refresh the DataGrid
                        }
                        else
                        {
                            MessageBox.Show("Medicamente update failed. Please check the data and try again.");
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
            if (dataGridMedicamente.SelectedItem != null)
            {
                // Cast the selected row into a DataRowView (or the type of your data model)
                DataRowView row = (DataRowView)dataGridMedicamente.SelectedItem;

                // Get the MedicID (primary key) of the selected record
                long MedicamentID = (long)row["MedicamentID"];  // Adjust if the name is different

                // Ask for confirmation before deleting the record
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this record?", "Delete Confirmation", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    // Create SQL query for deleting the Medic record
                    string query = "DELETE FROM Medicamente WHERE MedicamentID = @MedicamentID";

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        SqlCommand cmd = new SqlCommand(query, conn);

                        // Add the MedicID parameter to the query
                        cmd.Parameters.AddWithValue("@MedicamentID", MedicamentID);

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
