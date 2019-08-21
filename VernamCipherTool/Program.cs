using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VernamCipherTool.Enums;
using VernamCipherTool.Extensions;

namespace VernamCipherTool
{
    class Program
    {
        #region help message

        private static readonly string HelpMessage = @"
VernamCipherTool [type of operation] ""[message]"" [passphrase]

Types of operation:
 encode 
 decode";
        #endregion

        static void Main(string[] args)
        {
            var r = Enum.TryParse(args[0], true, out Operation op);

            if (!r)
            {
                Console.WriteLine(HelpMessage);
                return;
            }

            var message = args[1];
            var passphrase = args[2];
            var result = GetResult(op, message, passphrase);

            Console.WriteLine($"{result.Type} {result.ResultMessage}");
        }

        #region operations      

        public static Result GetResult(Operation op, string message, string cipher)
        {
            var str = message.GetMessageBytes(op);
            var cph = Encoding.Default.GetBytes(cipher);

            var chunks = GetChunkedString(str, cph);
            var finalStr = GetMergedStr(chunks, cph, op);

            return new Result(finalStr, op);
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
}
