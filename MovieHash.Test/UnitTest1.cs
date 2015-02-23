using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MovieHash.Test
{
    [TestClass]
    public class UnitTest1
    {
        const string URLBASE = @"http://www.opensubtitles.org/pb/search/sublanguageid-pob/moviehash-";


        [TestMethod]
        public void TestLoadSubtitleFromHash()
        {
            var hash = "05f5f4ed29b1aa4e";

            MovieHash.Program.GetSubtitle(URLBASE + hash);
            Assert.Inconclusive();
        }
    }
}
