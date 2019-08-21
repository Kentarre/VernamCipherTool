using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VernamCipherTool
{
    class Program
    {
        private static readonly string _encodedString = "Encoded string: ";
        private static readonly string _decodedString = "Decoded string: ";

        static void Main(string[] args)
        {
            if (args.Length == 0 || !args[0].StartsWith("--"))
            {
                ShowHelp();
                return;
            }

            Console.Write("Message: ");
            var message = Console.ReadLine();

            Console.Write("Passphrase: ");
            var passphrase = Console.ReadLine();

            Enum.TryParse(args[0].Replace("-", ""), out Operation op);

            var resultMessage = op == Operation.Encode ? _encodedString : _decodedString;

            Console.WriteLine($"{resultMessage} {Start(op, message, passphrase)}");
        }

        #region operations      

        private static void ShowHelp()
        {
            Console.WriteLine("VernamCipher.exe [type of operation]\n\nType of operations:\n --Decode \n --Encode");
        }

        public static string Start(Operation op, string message, string cipher)
        {
            var str = message.GetMessageBytes(op);
            var cph = Encoding.Default.GetBytes(cipher);

            var chunks = GetChunkedString(str, cph);
            var finalStr = GetMergedStr(chunks, cph, op);

            return finalStr;
        }

        private static List<List<byte>> GetChunkedString(byte[] str, byte[] cph, List<List<byte>> tmpChunkList = null)
        {
            var chunks = tmpChunkList ?? new List<List<byte>>();
            var chunk = str.Skip(chunks.Sum(x => x.Count)).Take(cph.Length).ToList();

            if (chunk.Count == 0)
                return tmpChunkList;

            chunks.Add(chunk);

            return GetChunkedString(str, cph, chunks);
        }

        private static string GetMergedStr(List<List<byte>> chunks, byte[] cph, Operation op)
        {
            var byteList = new List<byte>();

            foreach (var chunk in chunks)
                byteList.AddRange(chunk.Select((t, i) =>
                    Convert.ToByte(op == Operation.Encode ? t + cph[i] : t - cph[i])));

            return op == Operation.Encode
                ? Convert.ToBase64String(byteList.ToArray())
                : Encoding.Default.GetString(byteList.ToArray());
        }

        #endregion
    }
    public enum Operation
    {
        Encode,
        Decode
    }

    public static class StringExtension
    {
        public static byte[] GetMessageBytes(this string s, Operation op)
        {
            return op == Operation.Encode
                ? Encoding.Default.GetBytes(s)
                : Convert.FromBase64String(s);
        }
    }
}
