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
            var hash = "88b7672be293a0ca";

            Program.GetSubtitle(URLBASE + hash);
            Assert.IsTrue(true);
        }
    }
}
