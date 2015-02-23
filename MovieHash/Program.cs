using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using RestSharp;
using CsQuery;

namespace MovieHash
{
    public class Program
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

        public static void GetSubtitle(string url)
        {
            const string BASE_DOWNLOAD = "http://dl.opensubtitles.org/pb/download/sub/";

            var page = CQ.CreateFromUrl(url);   // Loads OpenSubtitle page with download link

            var links = page["a[href*='subtitleserve']"];   // Find download links

            if (links.Any())
            {
                // Extract file id and download count
                var subs = links.Select(e => new 
                { 
                    id = e.GetAttribute("href").Split('/').Last(),
                    count = Convert.ToInt32(e.InnerText.Replace("x", ""))
                });

                // Get the one most downloaded
                var sub = subs.FirstOrDefault(e => e.count == subs.Max(e2 => e2.count));

                // Download the zip
                var cli = new RestClient(BASE_DOWNLOAD + sub.id);
                var resp = cli.Get(new RestRequest());

                // Read as zip file
                using (var mem = new MemoryStream(resp.RawBytes))
                {
                    using (var zip = new ZipArchive(mem, ZipArchiveMode.Read))
                    {
                        Debug.WriteLine("Files: {0}", zip.Entries.Count);
                        foreach (var item in zip.Entries)
                        {
                            if (item.Name.EndsWith(".srt"))
                            {
                                // Found the subtitle                                
                                var srt = new StreamReader(item.Open()).ReadToEnd();
                            }
                        }
                    }
                }
            }            
        }
    }
}
