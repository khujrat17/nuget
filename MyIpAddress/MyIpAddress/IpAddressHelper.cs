using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MyIpAddress
{
    public class IpAddressHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public IpAddressHelper(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public bool IsValidIpAddress(string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
                return false;

            return IPAddress.TryParse(ipAddress, out _);
        }

        public string GetLocalIPv4Address()
        {
            string localIP = string.Empty;

            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break; // Return the first IPv4 address found
                }
            }

            return localIP;
        }

        public bool IsAccessGranted(string IPs)
        {
            string clientIP = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

            // Retrieve the allowed IPs from configuration
            string allowedIPs = _configuration[IPs];

            // Check if the client's IP is in the allowed list
            if (allowedIPs != null && allowedIPs.Split(',').Contains(clientIP))
            {
                return true; // IP is allowed
            }
            else
            {
                return false; // IP is not allowed
            }
        }
    }
}