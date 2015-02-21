using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieHash
{
    class Program
    {
        const string URLBASE = @"http://www.opensubtitles.org/pb/search/sublanguageid-pob/moviehash-";

        private static byte[] ComputeMovieHash(string filename)
        {
            byte[] result;
            using (Stream input = File.OpenRead(filename))
            {
                result = ComputeMovieHash(input);
            }
            return result;
        }

        private static byte[] ComputeMovieHash(Stream input)
        {
            long lhash, streamsize;
            streamsize = input.Length;
            lhash = streamsize;

            long i = 0;
            byte[] buffer = new byte[sizeof(long)];
            while (i < 65536 / sizeof(long) && (input.Read(buffer, 0, sizeof(long)) > 0))
            {
                i++;
                lhash += BitConverter.ToInt64(buffer, 0);
            }

            input.Position = Math.Max(0, streamsize - 65536);
            i = 0;
            while (i < 65536 / sizeof(long) && (input.Read(buffer, 0, sizeof(long)) > 0))
            {
                i++;
                lhash += BitConverter.ToInt64(buffer, 0);
            }
            input.Close();
            byte[] result = BitConverter.GetBytes(lhash);
            Array.Reverse(result);
            return result;
        }

        private static string ToHexadecimal(byte[] bytes)
        {
            StringBuilder hexBuilder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                hexBuilder.Append(bytes[i].ToString("x2"));
            }
            return hexBuilder.ToString();
        }

        static void Main(string[] args)
        {
            if (!args.Any())
            {
                Console.WriteLine("Informar um arquivo no 1o parâmetro.");
                return;
            }

            string path;
            if (File.Exists(args[0]))
            {
                path = args[0];
            }
            else
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), args[0]);
            }

            var hash = ToHexadecimal(ComputeMovieHash(path));
            Console.WriteLine("OpenSubtitles Hash ==>  {0}", hash);

            var url = URLBASE + hash;
            Process.Start(url);
        }
    }
}
