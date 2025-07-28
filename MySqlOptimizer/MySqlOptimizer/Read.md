MySqlOptimizer

MySqlOptimizer is a C# library designed to simplify database operations with MySQL. It provides methods for executing common SQL commands such as SELECT, INSERT, UPDATE, and DELETE using both raw SQL queries and parameterized queries to prevent SQL injection attacks.

Features:
- Execute Select: Retrieve data from the database and fill it into a DataTable.
- Execute Insert: Insert new records into the database using raw SQL or parameterized queries.
- Execute Update: Update existing records in the database using raw SQL or parameterized queries.
- Execute Delete: Remove records from the database using raw SQL or parameterized queries.

Installation:
To use the MySqlOptimizer library, ensure you have the following prerequisites:
- .NET Framework or .NET Core
- MySql.Data NuGet package
- Newtonsoft.Json NuGet package (if needed for JSON serialization)

You can install the required NuGet packages using the following commands:
Install-Package MySql.Data
Install-Package Newtonsoft.Json

Usage:
Creating an Instance:
To use the library, create an instance of the MysqlShort class.

using MySqlOptimizer;

// Create an instance of the MysqlShort class
MysqlShort mysqlShort = new MysqlShort();

Example of Using the Methods:

Execute Select:
string connectionString = "your_connection_string_here";
string selectQuery = "SELECT * FROM your_table_name";

DataTable result = mysqlShort.ExecuteSelect(connectionString, selectQuery);

Execute Insert (Raw SQL):
string insertQuery = "INSERT INTO your_table_name (column1, column2) VALUES ('value1', 'value2')";
int rowsInserted = mysqlShort.ExecuteInsert(connectionString, insertQuery);

Execute Insert (Parameterized):
string insertQuery = "INSERT INTO your_table_name (column1, column2) VALUES (@value1, @value2)";
MySqlParameter[] parameters = new MySqlParameter[]
{
    new MySqlParameter("@value1", "value1"),
    new MySqlParameter("@value2", "value2")
};

int rowsInserted = mysqlShort.ExecuteInsert(connectionString, insertQuery, parameters);

Execute Update (Raw SQL):
string updateQuery = "UPDATE your_table_name SET column1 = 'new_value' WHERE id = 1";
int rowsUpdated = mysqlShort.ExecuteUpdate(connectionString, updateQuery);

Execute Update (Parameterized):
string updateQuery = "UPDATE your_table_name SET column1 = @newValue WHERE id = @id";
MySqlParameter[] updateParameters = new MySqlParameter[]
{
    new MySqlParameter("@newValue", "new_value"),
    new MySqlParameter("@id", 1)
};

int rowsUpdated = mysqlShort.ExecuteUpdate(connectionString, updateQuery, updateParameters);

Execute Delete (Raw SQL):
string deleteQuery = "DELETE FROM your_table_name WHERE id = 1";
int rowsDeleted = mysqlShort.ExecuteDelete(connectionString, deleteQuery);

Execute Delete (Parameterized):
string deleteQuery = "DELETE FROM your_table_name WHERE id = @id";
MySqlParameter[] deleteParameters = new MySqlParameter[]
{
    new MySqlParameter("@id", 1)
};

int rowsDeleted = mysqlShort.ExecuteDelete(connectionString, deleteQuery, deleteParameters);

Error Handling:
Each method includes basic error handling that throws an exception with a descriptive message if an error occurs during the execution of the SQL command. You should implement your own error handling as needed when using these methods.

License:
This project is licensed under the MIT License - see the LICENSE file for details.

Contributing:
Contributions are welcome! Please feel free to submit a pull request or open an issue for any enhancements or bug fixes.

Contact:
For any questions or suggestions, please contact [Your Name] (your_email@example.com).