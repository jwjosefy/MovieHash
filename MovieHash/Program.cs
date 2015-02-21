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

            var hash = Hashing.ToHexadecimal(Hashing.ComputeMovieHash(path));
            Console.WriteLine("OpenSubtitles Hash ==>  {0}", hash);

            var url = URLBASE + hash;
            Process.Start(url);
        }
    }
}
