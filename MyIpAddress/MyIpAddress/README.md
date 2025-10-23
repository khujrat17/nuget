# MyIpAddress 2.0.0

**MyIpAddress** is an advanced .NET utility library for IP/network management and validation tasks.  

Version 2.0.0 is fully backward compatible with version 1.0 and adds a range of advanced features for modern .NET projects while still supporting legacy .NET Framework (4.5+).

---

## 🆚 Version Comparison: 1.0 vs 2.0

| Feature | Version 1.0 | Version 2.0 |
|---------|-------------|-------------|
| Validate IPv4 | ✅ | ✅ |
| Get local IPv4 address | ✅ | ✅ |
| Access control via IP whitelist/blacklist | ✅ | ✅ (ASP.NET Core only, legacy safe) |
| Validate IPv6 | ❌ | ✅ |
| Get local IPv6 address | ❌ | ✅ |
| Get public IP | ❌ | ✅ |
| Reverse DNS lookup | ❌ | ✅ |
| Check IP in CIDR range | ❌ | ✅ |
| Ping host connectivity | ❌ | ✅ |
| Get MAC addresses of local network interfaces | ❌ | ✅ |
| Calculate subnet mask from CIDR | ❌ | ✅ |
| Hostname validation | ❌ | ✅ |
| Email validation | ❌ | ✅ |
| Multi-target support (.NET 4.5+, .NET 6/7) | ❌ | ✅ |
| Backward compatibility with v1.0 | N/A | ✅ |

---

## 📦 Installation

```powershell
Install-Package MyIpAddress -Version 2.0.0
Supports:

.NET Framework 4.5+

.NET 6.0

.NET 7.0

⚡ Features
IPv4 & IPv6 validation

Get local IPv4 & IPv6 addresses

Retrieve public IP

Reverse DNS lookup

Check if an IP is in a CIDR range

Ping host connectivity

Get MAC addresses of local network interfaces

Calculate subnet mask from CIDR

Validate hostnames and email addresses

Access control via whitelist/blacklist (ASP.NET Core)

Fully backward compatible with version 1.0

🛠 Example Usage
csharp
Copy code
using MyIpAddress;

// Legacy .NET Framework
var helper = new IpAddressHelper();

// ASP.NET Core
// var helper = new IpAddressHelper(httpContextAccessor, configuration);

// Validate IPv4 & IPv6
bool isValid = helper.IsValidIpAddress("192.168.0.1");
bool isValidIPv6 = helper.IsValidIpAddress("2001:0db8:85a3::8a2e:0370:7334");

// Get local IPs
string localIPv4 = helper.GetLocalIPv4Address();
string localIPv6 = helper.GetLocalIPv6Address();

// Get public IP
string publicIP = await helper.GetPublicIpAsync();

// Check CIDR range
bool inRange = helper.IsIpInRange("192.168.0.10", "192.168.0.0/24");

// Ping a host
bool canPing = helper.PingHost("google.com");

// Retrieve MAC addresses
string[] macs = helper.GetLocalMacAddresses();

// Subnet mask from CIDR
string subnet = helper.GetSubnetMask(24);

// Hostname & Email validation
bool isValidHost = helper.IsValidHostName("example.com");
bool isValidEmail = helper.IsValidEmail("test@example.com");

// Reverse DNS
string hostName = helper.ReverseDnsLookup("8.8.8.8");
🔧 Notes
ASP.NET Core dependency (IHttpContextAccessor & IConfiguration) is only used for IsAccessGranted.

All other methods are fully compatible with legacy .NET Framework.

Version 2.0 is safe to upgrade for any projects using version 1.0 without breaking changes.