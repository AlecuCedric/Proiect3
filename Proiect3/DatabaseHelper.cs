using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect3
{
    internal class DatabaseHelper
    {
        private string connectionString = "Server=G713RS;Database=spitaldb;Trusted_Connection=True;";

        public SqlConnection GetConnection() { 
            return new SqlConnection(connectionString);
        }
    }
}