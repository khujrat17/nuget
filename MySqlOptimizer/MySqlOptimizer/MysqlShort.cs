using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MySqlOptimizer
{
    public class MysqlShort
    {
        // Optional query logger
        public Action<string>? QueryLogger { get; set; }

        private void LogQuery(string query)
        {
            QueryLogger?.Invoke(query);
        }

        #region SELECT

        public DataTable ExecuteSelect(string connectionString, string query)
        {
            LogQuery(query);
            DataTable dt = new DataTable();
            try
            {
                using (var con = new MySqlConnection(connectionString))
                using (var cmd = new MySqlCommand(query, con))
                using (var da = new MySqlDataAdapter(cmd))
                {
                    con.Open();
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while executing the query.", ex);
            }
            return dt;
        }

        public DataTable ExecuteSelect(string connectionString, string storedProcedureName, Dictionary<string, object> parameters)
        {
            LogQuery(storedProcedureName);
            DataTable dt = new DataTable();
            try
            {
                using (var con = new MySqlConnection(connectionString))
                using (var cmd = new MySqlCommand(storedProcedureName, con) { CommandType = CommandType.StoredProcedure })
                {
                    if (parameters != null)
                        foreach (var param in parameters)
                            cmd.Parameters.AddWithValue(param.Key, param.Value);

                    using (var da = new MySqlDataAdapter(cmd))
                    {
                        con.Open();
                        da.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while executing the stored procedure.", ex);
            }
            return dt;
        }

        public async Task<DataTable> ExecuteSelectAsync(string connectionString, string query)
        {
            LogQuery(query);
            var dt = new DataTable();
            using var con = new MySqlConnection(connectionString);
            using var cmd = new MySqlCommand(query, con);
            await con.OpenAsync();
            using var da = new MySqlDataAdapter(cmd);
            da.Fill(dt); // MySqlDataAdapter doesn't support async; can consider Dapper for full async
            return dt;
        }

        #endregion

        #region INSERT / UPDATE / DELETE

        public int ExecuteInsert(string connectionString, string query) => ExecuteNonQuery(connectionString, query);
        public int ExecuteInsert(string connectionString, string query, MySqlParameter[] parameters) => ExecuteNonQuery(connectionString, query, parameters);

        public int ExecuteUpdate(string connectionString, string query) => ExecuteNonQuery(connectionString, query);
        public int ExecuteUpdate(string connectionString, string query, MySqlParameter[] parameters) => ExecuteNonQuery(connectionString, query, parameters);

        public int ExecuteDelete(string connectionString, string query) => ExecuteNonQuery(connectionString, query);
        public int ExecuteDelete(string connectionString, string query, MySqlParameter[] parameters) => ExecuteNonQuery(connectionString, query, parameters);

        private int ExecuteNonQuery(string connectionString, string query, MySqlParameter[]? parameters = null)
        {
            LogQuery(query);
            int rowsAffected = 0;
            try
            {
                using var con = new MySqlConnection(connectionString);
                using var cmd = new MySqlCommand(query, con);
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);
                con.Open();
                rowsAffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while executing non-query operation.", ex);
            }
            return rowsAffected;
        }

        public async Task<int> ExecuteNonQueryAsync(string connectionString, string query, MySqlParameter[]? parameters = null)
        {
            LogQuery(query);
            using var con = new MySqlConnection(connectionString);
            using var cmd = new MySqlCommand(query, con);
            if (parameters != null)
                cmd.Parameters.AddRange(parameters);
            await con.OpenAsync();
            return await cmd.ExecuteNonQueryAsync();
        }

        #endregion

        #region SCALAR

        public object ExecuteScalar(string connectionString, string query)
        {
            LogQuery(query);
            object? result;
            try
            {
                using var con = new MySqlConnection(connectionString);
                using var cmd = new MySqlCommand(query, con);
                con.Open();
                result = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while executing scalar query.", ex);
            }
            return result!;
        }

        public async Task<object> ExecuteScalarAsync(string connectionString, string query)
        {
            LogQuery(query);
            using var con = new MySqlConnection(connectionString);
            using var cmd = new MySqlCommand(query, con);
            await con.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return result!;
        }

        #endregion

        #region CONNECTION CHECK

        public bool CheckConnection(string connectionString)
        {
            using var con = new MySqlConnection(connectionString);
            try
            {
                con.Open();
                return con.State == ConnectionState.Open;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region STORED PROCEDURE WITH OUTPUT
        public bool ExecuteStoredProcedure(string connectionString, string storedProcedureName, Dictionary<string, object> parameters)
        {
            try
            {
                using MySqlConnection mySqlConnection = new MySqlConnection(connectionString);
                using MySqlCommand mySqlCommand = new MySqlCommand(storedProcedureName, mySqlConnection);
                mySqlCommand.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                {
                    foreach (KeyValuePair<string, object> parameter in parameters)
                    {
                        mySqlCommand.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                }

                mySqlConnection.Open();
                return mySqlCommand.ExecuteNonQuery() > 0;
            }
            catch (Exception innerException)
            {
                throw new Exception("An error occurred while executing the stored procedure.", innerException);
            }
        }

        public Dictionary<string, object> ExecuteStoredProcedureWithOutput(
            string connectionString,
            string storedProcedureName,
            Dictionary<string, object>? inputParams,
            List<MySqlParameter>? outputParams)
        {
            LogQuery(storedProcedureName);
            using var con = new MySqlConnection(connectionString);
            using var cmd = new MySqlCommand(storedProcedureName, con) { CommandType = CommandType.StoredProcedure };

            if (inputParams != null)
                foreach (var p in inputParams)
                    cmd.Parameters.AddWithValue(p.Key, p.Value);

            if (outputParams != null)
                foreach (var p in outputParams)
                    cmd.Parameters.Add(p);

            con.Open();
            cmd.ExecuteNonQuery();

            return outputParams?.ToDictionary(p => p.ParameterName, p => p.Value) ?? new Dictionary<string, object>();
        }

        #endregion

        #region TRANSACTION SUPPORT

        public bool ExecuteTransaction(string connectionString, List<string> queries)
        {
            using var con = new MySqlConnection(connectionString);
            con.Open();
            using var transaction = con.BeginTransaction();
            using var cmd = con.CreateCommand();
            cmd.Transaction = transaction;

            try
            {
                foreach (var query in queries)
                {
                    LogQuery(query);
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                }
                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        #endregion

        #region BULK INSERT

        public void BulkInsert(string connectionString, string tableName, DataTable data)
        {
            using var con = new MySqlConnection(connectionString);
            con.Open();
            using var transaction = con.BeginTransaction();
            using var cmd = con.CreateCommand();
            cmd.Transaction = transaction;

            try
            {
                foreach (DataRow row in data.Rows)
                {
                    var columns = string.Join(",", data.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
                    var values = string.Join(",", data.Columns.Cast<DataColumn>().Select(c => $"@{c.ColumnName}"));
                    cmd.CommandText = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";
                    cmd.Parameters.Clear();
                    foreach (DataColumn col in data.Columns)
                        cmd.Parameters.AddWithValue($"@{col.ColumnName}", row[col.ColumnName]);
                    cmd.ExecuteNonQuery();
                }
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        #endregion

        #region EXECUTE READER

        public MySqlDataReader ExecuteReaderWithParameters(string connectionString, string query, MySqlParameter[] parameters)
        {
            LogQuery(query);
            var con = new MySqlConnection(connectionString);
            var cmd = new MySqlCommand(query, con);
            if (parameters != null)
                cmd.Parameters.AddRange(parameters);

            try
            {
                con.Open();
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                con.Close();
                throw new Exception("An error occurred while executing reader with parameters.", ex);
            }
        }

        #endregion
    }
}
