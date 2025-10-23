MySqlOptimizer

MySqlOptimizer is a C# library designed to simplify database operations with MySQL. It provides methods for executing common SQL commands such as SELECT, INSERT, UPDATE, DELETE, and stored procedures using both raw SQL queries and parameterized queries to prevent SQL injection attacks.

Version 2.0.0 introduces advanced features like async support, transactions, bulk insert, stored procedure output parameters, and optional query logging while maintaining full backward compatibility with v1.x.

Features
Existing Features (v1.x)
Feature	Description
Execute Select	Retrieve data from the database into a DataTable.
Execute Insert	Insert new records using raw SQL or parameterized queries.
Execute Update	Update existing records using raw SQL or parameterized queries.
Execute Delete	Remove records using raw SQL or parameterized queries.
Execute Stored Procedure (input only)	Call stored procedures with input parameters.
Check Connection	Validate the database connection.
ExecuteScalar & ExecuteReaderWithParameters	Retrieve single values or read result sets.
New Features in v2.0.0
Feature	Description
Async Support	Non-blocking methods like ExecuteSelectAsync, ExecuteNonQueryAsync, ExecuteScalarAsync.
Transaction Support	Execute multiple queries atomically using ExecuteTransaction.
Bulk Insert	Insert large datasets efficiently using BulkInsert.
Stored Procedure Output Parameters	Use ExecuteStoredProcedureWithOutput to retrieve output values.
Query Logging	Optional logging of SQL queries with QueryLogger delegate.
Installation
Prerequisites

.NET Framework (4.5.2, 4.7.2) or .NET 7.0

MySql.Data NuGet package

Newtonsoft.Json NuGet package (optional, if JSON serialization is needed)

Install NuGet Packages
Install-Package MySql.Data
Install-Package Newtonsoft.Json
Install-Package MySqlOptimizer -Version 2.0.0

Usage Examples
Create an Instance
using MySqlOptimizer;

// Create an instance of the MysqlShort class
MysqlShort mysqlShort = new MysqlShort();

Execute Select
string connectionString = "your_connection_string_here";
string selectQuery = "SELECT * FROM your_table_name";

DataTable result = mysqlShort.ExecuteSelect(connectionString, selectQuery);

Execute Insert (Raw SQL)
string insertQuery = "INSERT INTO your_table_name (column1, column2) VALUES ('value1', 'value2')";
int rowsInserted = mysqlShort.ExecuteInsert(connectionString, insertQuery);

Execute Insert (Parameterized)
string insertQuery = "INSERT INTO your_table_name (column1, column2) VALUES (@value1, @value2)";
MySqlParameter[] parameters = new MySqlParameter[]
{
    new MySqlParameter("@value1", "value1"),
    new MySqlParameter("@value2", "value2")
};

int rowsInserted = mysqlShort.ExecuteInsert(connectionString, insertQuery, parameters);

Execute Update (Raw SQL)
string updateQuery = "UPDATE your_table_name SET column1 = 'new_value' WHERE id = 1";
int rowsUpdated = mysqlShort.ExecuteUpdate(connectionString, updateQuery);

Execute Update (Parameterized)
string updateQuery = "UPDATE your_table_name SET column1 = @newValue WHERE id = @id";
MySqlParameter[] updateParameters = new MySqlParameter[]
{
    new MySqlParameter("@newValue", "new_value"),
    new MySqlParameter("@id", 1)
};

int rowsUpdated = mysqlShort.ExecuteUpdate(connectionString, updateQuery, updateParameters);

Execute Delete (Raw SQL)
string deleteQuery = "DELETE FROM your_table_name WHERE id = 1";
int rowsDeleted = mysqlShort.ExecuteDelete(connectionString, deleteQuery);

Execute Delete (Parameterized)
string deleteQuery = "DELETE FROM your_table_name WHERE id = @id";
MySqlParameter[] deleteParameters = new MySqlParameter[]
{
    new MySqlParameter("@id", 1)
};

int rowsDeleted = mysqlShort.ExecuteDelete(connectionString, deleteQuery, deleteParameters);

Execute Async Queries
DataTable dtAsync = await mysqlShort.ExecuteSelectAsync(connectionString, selectQuery);
int rowsAffected = await mysqlShort.ExecuteNonQueryAsync(connectionString, insertQuery, parameters);
object scalarResult = await mysqlShort.ExecuteScalarAsync(connectionString, "SELECT COUNT(*) FROM your_table_name");

Execute Transaction
var queries = new List<string>
{
    "INSERT INTO users(name,email) VALUES('Alice','alice@example.com')",
    "INSERT INTO users(name,email) VALUES('Bob','bob@example.com')"
};
bool success = mysqlShort.ExecuteTransaction(connectionString, queries);

Bulk Insert
DataTable dt = new DataTable();
dt.Columns.Add("name");
dt.Columns.Add("email");

dt.Rows.Add("Alice", "alice@example.com");
dt.Rows.Add("Bob", "bob@example.com");

mysqlShort.BulkInsert(connectionString, "users", dt);

Stored Procedure with Output Parameters
var inputParams = new Dictionary<string, object> { {"@UserId", 1} };
var outputParams = new List<MySqlParameter>
{
    new MySqlParameter("@UserName", MySqlDbType.VarChar, 100) { Direction = ParameterDirection.Output }
};

var result = mysqlShort.ExecuteStoredProcedureWithOutput(connectionString, "GetUserNameById", inputParams, outputParams);
Console.WriteLine(result["@UserName"]);

Query Logging
mysqlShort.QueryLogger = query => Console.WriteLine("Executing SQL: " + query);

Comparison with v1.x
Feature	v1.x	v2.0.0
Execute SELECT/INSERT/UPDATE/DELETE	✅	✅
Parameterized Queries	✅	✅
Stored Procedure (input only)	✅	✅
ExecuteScalar / ExecuteReader	✅	✅
Async Support	❌	✅
Transactions	❌	✅
Bulk Insert	❌	✅
Stored Procedure Output	❌	✅
Query Logging	❌	✅

Key Improvements:

Async methods allow non-blocking database calls.

Transactions ensure atomic execution of multiple queries.

Bulk insert improves performance for large datasets.

Output parameters in stored procedures simplify data retrieval.

Optional query logging aids in debugging and diagnostics.

Error Handling

All methods include basic exception handling with descriptive messages. Developers can implement custom error handling as needed.

License

This project is licensed under the MIT License. See the LICENSE file for details.

Contributing

Contributions are welcome! Submit pull requests or open issues for enhancements or bug fixes.

Contact

For questions or suggestions, contact Shaikh Khujrat at khujratshaikh1284@gmail.com.

MySqlOptimizer v2.0.0 Migration Guide

Version 2.0.0 introduces new features while maintaining full backward compatibility with v1.x.

1️⃣ Installation

Update the NuGet package to v2.0.0:

dotnet add package MySqlOptimizer --version 2.0.0


Existing v1.x code will continue to work. New features are optional.

2️⃣ Backward Compatibility

All v1.x methods are intact:

ExecuteSelect()

ExecuteInsert()

ExecuteUpdate()

ExecuteDelete()

ExecuteScalar()

ExecuteReaderWithParameters()

CheckConnection()

Stored procedures without output parameters

Example existing code still works:

var db = new MysqlShort();
var dt = db.ExecuteSelect(connectionString, "SELECT * FROM users");
int rows = db.ExecuteInsert(connectionString, "INSERT INTO users(name,email) VALUES('John','john@example.com')");

3️⃣ New Features
a) Async Methods
DataTable dt = await db.ExecuteSelectAsync(connectionString, "SELECT * FROM users");

b) Transaction Support
var queries = new List<string>
{
    "INSERT INTO users(name,email) VALUES('Alice','alice@example.com')",
    "INSERT INTO users(name,email) VALUES('Bob','bob@example.com')"
};
bool success = db.ExecuteTransaction(connectionString, queries);

c) Bulk Insert
DataTable dt = new DataTable();
dt.Columns.Add("name");
dt.Columns.Add("email");
dt.Rows.Add("Alice", "alice@example.com");
dt.Rows.Add("Bob", "bob@example.com");

db.BulkInsert(connectionString, "users", dt);

d) Stored Procedures with Output Parameters
var inputParams = new Dictionary<string, object> { {"@UserId", 1} };
var outputParams = new List<MySqlParameter>
{
    new MySqlParameter("@UserName", MySqlDbType.VarChar, 100) { Direction = ParameterDirection.Output }
};

var result = db.ExecuteStoredProcedureWithOutput(connectionString, "GetUserNameById", inputParams, outputParams);
Console.WriteLine(result["@UserName"]);

e) Query Logging
db.QueryLogger = query => Console.WriteLine("Executing SQL: " + query);

4️⃣ Migration Checklist
Task	Notes
Upgrade NuGet	dotnet add package MySqlOptimizer --version 2.0.0
Test existing queries	All v1.x methods are unchanged
Optional: adopt async	Replace blocking calls with ExecuteSelectAsync, ExecuteNonQueryAsync, ExecuteScalarAsync
Optional: enable logging	Set QueryLogger to debug queries
Optional: use transactions	Wrap multiple queries with ExecuteTransaction
Optional: bulk inserts	Use BulkInsert for large datasets
Optional: output parameters	Use ExecuteStoredProcedureWithOutput
5️⃣ Notes for Users

No breaking changes: v1.x code works as-is.

New features are opt-in: existing workflows are unaffected.

Async methods: MySqlDataAdapter is not fully async; consider Dapper for full async scenarios if needed.

Logging: Can be enabled without changing existing methods.

Transactions/Bulk Inserts: Recommended for performance and atomic operations.

✅ Conclusion:
Existing projects using v1.x will continue to work perfectly. New features in v2.0.0 are optional and will not interfere with old code.

This document is now fully structured with tables, headings, code blocks, and clear migration instructions, ready to include in your README.md and MigrationGuide.md for NuGet.