using System;
using System.Security.Cryptography;
using System.Text;

namespace Stalkr.In
{
    public class ChecksumStalkr : IChecksumStalkr
    {
        public string GetSha256Digest(string content)
        {
            if (String.IsNullOrEmpty(content))
                return String.Empty;
            
            var contentBytes = Encoding.UTF8.GetBytes(content);
            
            using var sha256 = new SHA256Managed();
            sha256.ComputeHash(contentBytes);

            var buffer = new StringBuilder(sha256.Hash.Length);

            foreach (var hashByte in sha256.Hash)
                buffer.Append(hashByte.ToString("x2"));

            return buffer.ToString();
        }
    }
}