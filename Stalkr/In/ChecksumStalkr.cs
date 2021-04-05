using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Stalkr.In
{
    public class ChecksumStalkr : IChecksumStalkr
    {
        private readonly ILogger<ChecksumStalkr> _logger;

        public ChecksumStalkr(ILogger<ChecksumStalkr> logger)
        {
            _logger = logger;
        }
        
        public string GetSha256Digest(string content)
        {
            if (String.IsNullOrEmpty(content))
                return String.Empty;

            var stopwatch = Stopwatch.StartNew();
            var contentBytes = Encoding.UTF8.GetBytes(content);
            
            using var sha256 = new SHA256Managed();
            sha256.ComputeHash(contentBytes);

            var buffer = new StringBuilder(sha256.Hash.Length);

            foreach (var hashByte in sha256.Hash)
                buffer.Append(hashByte.ToString("x2"));

            var checksum = buffer.ToString();
            
            stopwatch.Stop();

            _logger.LogDebug(
                "Calculated Checksum {Checksum} for input with length {ContentLength} in {Duration}ms",
                checksum,
                content.Length,
                stopwatch.ElapsedMilliseconds
            );

            return checksum;
        }
    }
}