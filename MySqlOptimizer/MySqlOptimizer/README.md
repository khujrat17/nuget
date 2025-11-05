# MySqlOptimizer

**MySqlOptimizer** is a C# library designed to simplify MySQL database operations. It provides methods for executing `SELECT`, `INSERT`, `UPDATE`, `DELETE`, and stored procedures using both raw SQL and parameterized queries to prevent SQL injection attacks.

Version **2.0.0** introduces async support, transactions, bulk insert, stored procedure output parameters, and optional query logging while maintaining full backward compatibility with v1.x.

---

## Features

### Existing Features (v1.x)

* Execute Select: Retrieve data into a `DataTable`.
* Execute Insert: Insert new records using raw SQL or parameterized queries.
* Execute Update: Update existing records using raw SQL or parameterized queries.
* Execute Delete: Delete records using raw SQL or parameterized queries.
* Execute Stored Procedure (input only): Call stored procedures with input parameters.
* Check Connection: Validate the database connection.
* ExecuteScalar & ExecuteReaderWithParameters: Retrieve single values or read result sets.

### New Features in v2.0.0

* Async Support: Non-blocking methods (`ExecuteSelectAsync`, `ExecuteNonQueryAsync`, `ExecuteScalarAsync`).
* Transaction Support: Execute multiple queries atomically using `ExecuteTransaction`.
* Bulk Insert: Insert large datasets efficiently using `BulkInsert`.
* Stored Procedure Output Parameters: Retrieve output values with `ExecuteStoredProcedureWithOutput`.
* Query Logging: Optional logging of SQL queries via `QueryLogger`.

---

## Installation

### Prerequisites

* .NET Framework (4.5.2, 4.7.2) or .NET 7.0
* [MySql.Data NuGet package](https://www.nuget.org/packages/MySql.Data/)
* [Newtonsoft.Json NuGet package](https://www.nuget.org/packages/Newtonsoft.Json/) (optional)

### Install Packages

```bash
Install-Package MySql.Data
Install-Package Newtonsoft.Json
Install-Package MySqlOptimizer -Version 2.0.0
```

---

## Usage

### Create an Instance

```csharp
using MySqlOptimizer;

MysqlShort mysqlShort = new MysqlShort();
```

### Execute Select

```csharp
string connectionString = "your_connection_string_here";
string selectQuery = "SELECT * FROM your_table_name";

DataTable result = mysqlShort.ExecuteSelect(connectionString, selectQuery);
```

### Execute Insert (Parameterized)

```csharp
string insertQuery = "INSERT INTO your_table_name (column1, column2) VALUES (@value1, @value2)";
MySqlParameter[] parameters = new MySqlParameter[]
{
    new MySqlParameter("@value1", "value1"),
    new MySqlParameter("@value2", "value2")
};

int rowsInserted = mysqlShort.ExecuteInsert(connectionString, insertQuery, parameters);
```

### Async Queries (v2.0.0)

```csharp
DataTable dtAsync = await mysqlShort.ExecuteSelectAsync(connectionString, selectQuery);
int rowsAffected = await mysqlShort.ExecuteNonQueryAsync(connectionString, insertQuery, parameters);
object scalarResult = await mysqlShort.ExecuteScalarAsync(connectionString, "SELECT COUNT(*) FROM your_table_name");
```

### Transactions (v2.0.0)

```csharp
var queries = new List<string>
{
    "INSERT INTO users(name,email) VALUES('Alice','alice@example.com')",
    "INSERT INTO users(name,email) VALUES('Bob','bob@example.com')"
};

bool success = mysqlShort.ExecuteTransaction(connectionString, queries);
```

### Bulk Insert (v2.0.0)

```csharp
DataTable dt = new DataTable();
dt.Columns.Add("name");
dt.Columns.Add("email");

dt.Rows.Add("Alice", "alice@example.com");
dt.Rows.Add("Bob", "bob@example.com");

mysqlShort.BulkInsert(connectionString, "users", dt);
```

### Stored Procedure with Output Parameters (v2.0.0)

```csharp
var inputParams = new Dictionary<string, object> { {"@UserId", 1} };
var outputParams = new List<MySqlParameter>
{
    new MySqlParameter("@UserName", MySqlDbType.VarChar, 100) { Direction = ParameterDirection.Output }
};

var result = mysqlShort.ExecuteStoredProcedureWithOutput(connectionString, "GetUserNameById", inputParams, outputParams);
Console.WriteLine(result["@UserName"]);
```

### Query Logging (v2.0.0)

```csharp
mysqlShort.QueryLogger = query => Console.WriteLine("Executing SQL: " + query);
```

---

## Comparison: v1.x vs v2.0.0

| Feature                             | v1.x | v2.0.0 |
| ----------------------------------- | ---- | ------ |
| Execute SELECT/INSERT/UPDATE/DELETE | ✅    | ✅      |
| Parameterized Queries               | ✅    | ✅      |
| Stored Procedure (input only)       | ✅    | ✅      |
| ExecuteScalar / ExecuteReader       | ✅    | ✅      |
| Async Support                       | ❌    | ✅      |
| Transactions                        | ❌    | ✅      |
| Bulk Insert                         | ❌    | ✅      |
| Stored Procedure Output             | ❌    | ✅      |
| Query Logging                       | ❌    | ✅      |

---

## Error Handling

All methods include basic exception handling with descriptive messages. Developers can implement custom error handling as needed.

---

## License

MIT License — see the LICENSE file for details.

---

## Contributing

Contributions are welcome! Submit pull requests or open issues for enhancements or bug fixes.

**Contact:**
Shaikh Khujrat — [khujratshaikh1284@gmail.com](mailto:khujratshaikh1284@gmail.com)

---

## Migration Guide: v2.0.0

1. **Installation**

```bash
dotnet add package MySqlOptimizer --version 2.0.0
```

2. **Backward Compatibility**
   All v1.x code works as-is.

```csharp
var db = new MysqlShort();
var dt = db.ExecuteSelect(connectionString, "SELECT * FROM users");
int rows = db.ExecuteInsert(connectionString, "INSERT INTO users(name,email) VALUES('John','john@example.com')");
```

3. **Adopt New Features (Optional)**

* Async Methods
* Transaction Support
* Bulk Inserts
* Stored Procedures with Output Parameters
* Query Logging

4. **Migration Checklist**

| Task                        | Notes                                               |
| --------------------------- | --------------------------------------------------- |
| Upgrade NuGet               | `dotnet add package MySqlOptimizer --version 2.0.0` |
| Test existing queries       | All v1.x methods are unchanged                      |
| Optional: adopt async       | Replace blocking calls with async methods           |
| Optional: enable logging    | Set `QueryLogger`                                   |
| Optional: use transactions  | Wrap multiple queries with `ExecuteTransaction`     |
| Optional: bulk inserts      | Use `BulkInsert` for large datasets                 |
| Optional: output parameters | Use `ExecuteStoredProcedureWithOutput`              |

✅ **Conclusion:** Existing projects using v1.x will continue to work perfectly. New features in v2.0.0 are optional and safe to adopt.
