using System;
using System.Text;
using VernamCipherTool.Enums;

namespace VernamCipherTool.Extensions
{
    public static class StringExtensions
    {
        public static byte[] GetMessageBytes(this string s, Operation op)
        {
            return op == Operation.Encode
                ? Encoding.Default.GetBytes(s)
                : Convert.FromBase64String(s);
        }
    }
}
