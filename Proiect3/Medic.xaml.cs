﻿using Microsoft.Data.SqlClient;
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
                string query = "INSERT INTO Medic (NumeMedic, PrenumeMedic, Specializare) VALUES (@NumeMedic, @PrenumeMedic, @Specializare)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@NumeMedic", txtNumeMedic.Text);
                cmd.Parameters.AddWithValue("@PrenumeMedic", txtPrenumeMedic.Text);
                cmd.Parameters.AddWithValue("@Specializare", txtSpecializare.Text);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            LoadData(); // Refresh the DataGrid
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if a row is selected in the DataGrid
            if (dataGridMedic.SelectedItem != null)
            {
                // Cast the selected row into a DataRowView (or the type of your data model)
                DataRowView row = (DataRowView)dataGridMedic.SelectedItem;

                // Get the MedicID (primary key) of the selected record
                int medicID = (int)row["MedicID"];  // Adjust if the name is different

                // Create SQL query for updating the Medic table
                string query = "UPDATE Medic SET NumeMedic = @NumeMedic, PrenumeMedic = @PrenumeMedic, Specializare = @Specializare WHERE MedicID = @MedicID";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);

                    // Add parameters for the SQL query
                    cmd.Parameters.AddWithValue("@MedicID", medicID);
                    cmd.Parameters.AddWithValue("@NumeMedic", txtNumeMedic.Text);  // Assuming txtNumeMedic is your TextBox for "Nume"
                    cmd.Parameters.AddWithValue("@PrenumeMedic", txtPrenumeMedic.Text);  // Assuming txtPrenumeMedic is your TextBox for "Prenume"
                    cmd.Parameters.AddWithValue("@Specializare", txtSpecializare.Text);  // Assuming txtSpecializare is your TextBox for "Specializare"

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
                // Cast the selected row into a DataRowView (or the type of your data model)
                DataRowView row = (DataRowView)dataGridMedic.SelectedItem;

                // Get the MedicID (primary key) of the selected record
                int medicID = (int)row["MedicID"];  // Adjust if the name is different

                // Ask for confirmation before deleting the record
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this record?", "Delete Confirmation", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    // Create SQL query for deleting the Medic record
                    string query = "DELETE FROM Medic WHERE MedicID = @MedicID";

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        SqlCommand cmd = new SqlCommand(query, conn);

                        // Add the MedicID parameter to the query
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
    }
}