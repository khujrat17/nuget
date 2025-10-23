CustomLog 2.02 - Thread-Safe, Multi-Framework Logging Library for .NET

CustomLog is a lightweight, thread-safe logging library for .NET applications.
This document compares version 1.0 with the latest version 2.2 and provides a migration guide.

1️⃣ CustomLog 1.0 (Original Version)

Simple file logging using File.AppendAllText.

Methods:

Log(string message) – logs general message.

LogInfo(string message) – logs info message.

LogWarning(string message) – logs warning message.

LogError(string message) – logs error message.

Writes all logs to a single file.

No log levels besides method names.

No thread-safety; concurrent writes may fail.

No log rotation or retention.

No exception logging; only string messages.

Compatible only with the framework it was compiled for.

Usage Example:

var logger = new SLogger(@"C:\Logs\app.log");

logger.LogInfo("App started");
logger.LogWarning("Low memory");
logger.LogError("An error occurred");

2️⃣ CustomLog 2.0 (Latest Version)
New Features

Multi-framework support:

Works for .NET Framework 4.5+ and .NET 6/7.

Thread-safe logging using lock.

Log levels using LogLevel enum (Info, Warning, Error).

Rolling logs: new file per day automatically.

Log retention: old logs deleted after configurable days.

Exception logging: logs Exception object including InnerExceptions.

Optional console output.

Fully backward-compatible with version 1.0:

Existing methods (LogInfo, LogWarning, LogError) still work.

Usage Example
var logger = new SLogger(@"C:\Logs");

// Logging messages
logger.LogInfo("App started");
logger.LogWarning("Low memory warning");
logger.LogError("Something went wrong");

// Logging exceptions
try
{
    int x = 0;
    int y = 5 / x;
}
catch (Exception ex)
{
    logger.LogException(ex); // Logs message, stack trace, inner exceptions
}

// Optional: log with console output
logger.Log("Custom message with console", LogLevel.Info, true);

// Clear all logs
logger.ClearLogs();


+-----------------+--------------------+---------------------------+
| Feature         | Version 1.0        | Version 2.0              |
+-----------------+--------------------+---------------------------+
| Thread-safety   | ❌                  | ✅ Lock-based thread-safe |
| Multi-framework | ❌                  | ✅ .NET 4.5+ / 6 / 7     |
| Log levels      | ❌                  | ✅ Info / Warning / Error |
| Rolling logs    | ❌                  | ✅ Daily log files        |
| Log retention   | ❌                  | ✅ Auto-delete old logs   |
| Exception log   | ❌                  | ✅ Includes inner exceptions |
| Console output  | ❌                  | ✅ Optional               |
| Backward-comp.  | N/A                 | ✅ All 1.0 methods work  |
+-----------------+--------------------+---------------------------+


Advantages Over Version 1.0
Feature	Version 1.0	Version 2.0
Thread-safety	❌	✅ Lock-based thread-safe
Multi-framework	❌	✅ .NET 4.5+ / 6 / 7
Log levels	❌	✅ Info / Warning / Error
Rolling logs	❌	✅ Daily log files
Log retention	❌	✅ Auto-delete old logs
Exception logging	❌	✅ Includes inner exceptions
Console output	❌	✅ Optional
Backward-compatible	N/A	✅ All 1.0 methods work

Visual Upgrade Flow

CustomLog 1.0
  │
  │ Upgrade
  ▼
CustomLog 2.0
 ├─ Thread-safe
 ├─ Multi-framework (.NET 4.5+ & 6/7)
 ├─ Log levels: Info/Warning/Error
 ├─ Rolling logs (daily files)
 ├─ Retention: auto-delete old logs
 ├─ Exception logging (with inner exceptions)
 ├─ Optional console output
 └─ Fully backward-compatible with 1.0

3️⃣ Migration Guide (1.0 → 2.0)
Basic Logging

1.0:

logger.Log("Hello world");
logger.LogInfo("Info");
logger.LogWarning("Warning");
logger.LogError("Error");


2.0 (Backward-compatible):

logger.Log("Hello world");                     // still works
logger.LogInfo("Info");                        // still works
logger.LogWarning("Warning");                  // still works
logger.LogError("Error");                      // still works
logger.Log("Custom message", LogLevel.Info);  // new overload with LogLevel

Exception Logging

1.0: Not supported.

2.0: supported.

try
{
    // some code that may fail
}
catch (Exception ex)
{
    logger.LogException(ex); // logs message + stack trace + inner exceptions
}

Console Output

Optional in 2.0:

logger.Log("Message with console output", LogLevel.Info, true);

Rolling Logs & Retention

Automatically creates daily log files:

C:\Logs\2025-10-23_log.txt
C:\Logs\2025-10-24_log.txt


Old log files beyond retention period (default 30 days) are deleted automatically.

4️⃣ Installation
Install-Package CustomLog -Version 2.0.0

5️⃣ License

MIT

✅ This version of the README is well-structured for NuGet and GitHub, easy to read, and includes:

Version comparison

Feature highlights

Migration guide

Usage examples

Installation & license info