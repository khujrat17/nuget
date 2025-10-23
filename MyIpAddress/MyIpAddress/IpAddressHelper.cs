using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net.Mail;
using System.Threading.Tasks;

#if NET6_0_OR_GREATER || NET7_0
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
#endif

namespace MyIpAddress
{
    public class IpAddressHelper
    {
#if NET6_0_OR_GREATER || NET7_0
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public IpAddressHelper(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
#else
        // Legacy constructor for .NET Framework (1.0 compatible)
        public IpAddressHelper() { }
#endif

        // 1️⃣ Validate IPv4/IPv6
        public bool IsValidIpAddress(string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
                return false;

            return IPAddress.TryParse(ipAddress, out _);
        }

        // 2️⃣ Get local IPv4
        public string GetLocalIPv4Address()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip.ToString();
            }
            return string.Empty;
        }

#if NET6_0_OR_GREATER || NET7_0
        // 3️⃣ IsAccessGranted for ASP.NET projects
        public bool IsAccessGranted(string configKey)
        {
            string clientIP = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
            if (string.IsNullOrEmpty(clientIP)) return false;

            string allowedIPs = _configuration[configKey] ?? "";
            string[] ipList = allowedIPs.Split(',', StringSplitOptions.RemoveEmptyEntries);

            foreach (var ip in ipList)
            {
                if (ip.Contains("/"))
                {
                    if (IsIpInRange(clientIP, ip)) return true;
                }
                else if (clientIP == ip) return true;
            }
            return false;
        }

        // 4️⃣ Get public IP (async)
        public async Task<string> GetPublicIpAsync()
        {
            using HttpClient client = new HttpClient();
            try
            {
                return await client.GetStringAsync("https://api.ipify.org") ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
#endif

        // 5️⃣ Local IPv6
        public string GetLocalIPv6Address()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetworkV6)
                    return ip.ToString();
            }
            return string.Empty;
        }

        // 6️⃣ Reverse DNS lookup
        public string ReverseDnsLookup(string ipAddress)
        {
            if (!IsValidIpAddress(ipAddress)) return string.Empty;

            try
            {
#if NET6_0_OR_GREATER || NET7_0
                var host = Dns.GetHostEntry(ipAddress);
                return host.HostName;
#else
                // Legacy .NET
                var addresses = Dns.GetHostAddresses(ipAddress);
                if (addresses.Length > 0) return addresses[0].ToString();
                return string.Empty;
#endif
            }
            catch
            {
                return string.Empty;
            }
        }

        // 7️⃣ Check IP in CIDR
        public bool IsIpInRange(string ipAddress, string cidr)
        {
            if (!IsValidIpAddress(ipAddress)) return false;

            string[] parts = cidr.Split('/');
            if (parts.Length != 2) return false;

            IPAddress network = IPAddress.Parse(parts[0]);
            int bits = int.Parse(parts[1]);

            byte[] ipBytes = IPAddress.Parse(ipAddress).GetAddressBytes();
            byte[] networkBytes = network.GetAddressBytes();

            if (ipBytes.Length != networkBytes.Length) return false;

            int fullBytes = bits / 8;
            int remainingBits = bits % 8;

            for (int i = 0; i < fullBytes; i++)
                if (ipBytes[i] != networkBytes[i]) return false;

            if (remainingBits > 0)
            {
                int mask = (byte)(~(0xFF >> remainingBits));
                if ((ipBytes[fullBytes] & mask) != (networkBytes[fullBytes] & mask))
                    return false;
            }

            return true;
        }

        // 8️⃣ Ping host
        public bool PingHost(string host, int timeout = 1000)
        {
            try
            {
                Ping ping = new Ping();
                var reply = ping.Send(host, timeout);
                return reply.Status == IPStatus.Success;
            }
            catch
            {
                return false;
            }
        }

        // 9️⃣ MAC addresses
        public string[] GetLocalMacAddresses()
        {
            return NetworkInterface.GetAllNetworkInterfaces()
                                   .Where(nic => nic.OperationalStatus == OperationalStatus.Up)
                                   .Select(nic => nic.GetPhysicalAddress().ToString())
                                   .Where(mac => !string.IsNullOrWhiteSpace(mac))
                                   .ToArray();
        }

        // 🔟 Subnet mask from CIDR
        public string GetSubnetMask(int cidr)
        {
            if (cidr < 0 || cidr > 32) return string.Empty;

            // Create 32-bit mask with 'cidr' bits set to 1
            uint mask = cidr == 0 ? 0 : 0xffffffff << (32 - cidr);

            // Convert to bytes in big-endian order
            byte[] bytes = BitConverter.GetBytes(mask);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            // Create IPAddress
            return new IPAddress(bytes).ToString();
        }


        // 11️⃣ Hostname validation
        public bool IsValidHostName(string hostname)
        {
            if (string.IsNullOrWhiteSpace(hostname)) return false;

            try
            {
                var host = Dns.GetHostEntry(hostname);
                return host != null;
            }
            catch
            {
                return false;
            }
        }

        // 12️⃣ Email validation
        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;

            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
