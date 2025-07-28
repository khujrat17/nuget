using MySql.Data.MySqlClient;
using System.Data;



namespace MySqlOptimizer
{
    public class MysqlShort
    {
        public DataTable ExecuteSelect(string connectionString, string query)
        {
            DataTable dt = new DataTable();
            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        con.Open(); // Open the connection
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            da.Fill(dt); // Fill the DataTable with the result of the query
                        }
                    }
                }
            }
            catch (Exception ex)
            {
              
              throw new Exception("An error occurred while executing the query.", ex);
            }

            return dt; // Return the populated DataTable
        }



        public DataTable ExecuteSelect(string connectionString, string storedProcedureName, Dictionary<string, object> parameters)
        {
            DataTable dt = new DataTable();
            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(storedProcedureName, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure; // Set the command type to StoredProcedure

                        // Add parameters to the command
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                            {
                                cmd.Parameters.AddWithValue(param.Key, param.Value);
                            }
                        }

                        con.Open(); // Open the connection
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            da.Fill(dt); // Fill the DataTable with the result of the query
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while executing the stored procedure.", ex);
            }

            return dt; // Return the populated DataTable
        }

        public int ExecuteInsert(string connectionString, string query)
        {
            int rowsAffected = 0; // To track the number of rows affected
            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        con.Open(); // Open the connection
                        rowsAffected = cmd.ExecuteNonQuery(); // Execute the INSERT command
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                throw new Exception("An error occurred while executing the insert operation.", ex);
            }

            return rowsAffected; // Return the number of rows affected
        }


        public int ExecuteInsert(string connectionString, string query, MySqlParameter[] parameters)
        {
            int rowsAffected = 0; // To track the number of rows affected
            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        // Add parameters to the command
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                        con.Open(); // Open the connection
                        rowsAffected = cmd.ExecuteNonQuery(); // Execute the INSERT command
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                throw new Exception("An error occurred while executing the insert operation.", ex);
            }

            return rowsAffected; // Return the number of rows affected
        }

        public int ExecuteUpdate(string connectionString, string query)
        {
            int rowsAffected = 0; // To track the number of rows affected
            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        con.Open(); // Open the connection
                        rowsAffected = cmd.ExecuteNonQuery(); // Execute the UPDATE command
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                throw new Exception("An error occurred while executing the update operation.", ex);
            }

            return rowsAffected; // Return the number of rows affected
        }


        public int ExecuteUpdate(string connectionString, string query, MySqlParameter[] parameters)
        {
            int rowsAffected = 0; // To track the number of rows affected
            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        // Add parameters to the command
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                        con.Open(); // Open the connection
                        rowsAffected = cmd.ExecuteNonQuery(); // Execute the UPDATE command
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                throw new Exception("An error occurred while executing the update operation.", ex);
            }

            return rowsAffected; // Return the number of rows affected
        }


        public int ExecuteDelete(string connectionString, string query)
        {
            int rowsAffected = 0; // To track the number of rows affected
            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        con.Open(); // Open the connection
                        rowsAffected = cmd.ExecuteNonQuery(); // Execute the DELETE command
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                throw new Exception("An error occurred while executing the delete operation.", ex);
            }

            return rowsAffected; // Return the number of rows affected
        }



        public int ExecuteDelete(string connectionString, string query, MySqlParameter[] parameters)
        {
            int rowsAffected = 0; // To track the number of rows affected
            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        // Add parameters to the command
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                        con.Open(); // Open the connection
                        rowsAffected = cmd.ExecuteNonQuery(); // Execute the DELETE command
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                throw new Exception("An error occurred while executing the delete operation.", ex);
            }

            return rowsAffected; // Return the number of rows affected
        }



        //CheckConnection
        public bool CheckConnection(string connectionString)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    return con.State == ConnectionState.Open; // Return true if the connection is open
                }
                catch
                {
                    return false; // Return false if there was an error
                }
            }
        }


        //ExecuteScalar
        public object ExecuteScalar(string connectionString, string query)
        {
            object result = null;
            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        con.Open(); // Open the connection
                        result = cmd.ExecuteScalar(); // Execute the query and get the first column of the first row
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while executing the scalar query.", ex);
            }

            return result; // Return the result
        }


        public bool ExecuteStoredProcedure(string connectionString, string storedProcedureName, Dictionary<string, object> parameters)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(storedProcedureName, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure; // Set the command type to StoredProcedure

                        // Add parameters to the command
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                            {
                                cmd.Parameters.AddWithValue(param.Key, param.Value);
                            }
                        }

                        con.Open(); // Open the connection
                        int rowsAffected = cmd.ExecuteNonQuery(); // Execute the stored procedure

                        return rowsAffected > 0; // Return true if at least one row was affected
                    }
                }
            }
            catch (Exception ex)
            {
                // You can log the exception or handle it as needed
                throw new Exception("An error occurred while executing the stored procedure.", ex);
            }
        }


        //ExecuteReaderWithParameters
        public MySqlDataReader ExecuteReaderWithParameters(string connectionString, string query, MySqlParameter[] parameters)
        {
            MySqlConnection con = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand(query, con);

            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters); // Add parameters to the command
            }

            try
            {
                con.Open(); // Open the connection
                return cmd.ExecuteReader(CommandBehavior.CloseConnection); // Return the reader
            }
            catch (Exception ex)
            {
                con.Close(); // Close the connection in case of error
                throw new Exception("An error occurred while executing the reader with parameters.", ex);
            }
        }
    }
}
