using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrambleBerry.Kitchen.HttpHandlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace BrambleBerry.Kitchen.HttpHandlers.Tests
{
    [TestClass()]
    public class ProtectedMediaHandlerTests
    {
        [TestMethod()]
        public void IsMediaRequestTest_MediaLocations()
        {
            Assert.IsTrue(ProtectedMediaHandler.IsMediaRequest("/media/1021/rub26ruler.jpg"));
            Assert.IsTrue(ProtectedMediaHandler.IsMediaRequest("/media/234234/asdasd.png"));
            Assert.IsTrue(ProtectedMediaHandler.IsMediaRequest("/media/23423/sdfgsdfg.gif"));
            Assert.IsTrue(ProtectedMediaHandler.IsMediaRequest("/media/324234/asdfasdf.jpg"));
        }
        [TestMethod()]
        public void IsMediaRequest_NonMediaLocationsTest()
        {
            Assert.IsFalse(ProtectedMediaHandler.IsMediaRequest("/test"));
            Assert.IsFalse(ProtectedMediaHandler.IsMediaRequest("/test/"));
            Assert.IsFalse(ProtectedMediaHandler.IsMediaRequest("/test/media/test"));
            Assert.IsFalse(ProtectedMediaHandler.IsMediaRequest("/test/media/test/"));
            Assert.IsFalse(ProtectedMediaHandler.IsMediaRequest("/media"));
            Assert.IsFalse(ProtectedMediaHandler.IsMediaRequest("/media/"));
        }

        [TestMethod()]
        public void ExtractPropertyIdFromUrlTest()
        {
            Assert.AreEqual(1021, ProtectedMediaHandler.ExtractPropertyIdFromUrl("/media/1021/rub26ruler.jpg"));
            Assert.AreEqual(234234, ProtectedMediaHandler.ExtractPropertyIdFromUrl("/media/234234/asdasd.png"));
            Assert.AreEqual(23423, ProtectedMediaHandler.ExtractPropertyIdFromUrl("/media/23423/sdfgsdfg.gif"));
            Assert.AreEqual(324234, ProtectedMediaHandler.ExtractPropertyIdFromUrl("/media/324234/asdfasdf.jpg"));
        }
        [TestMethod()]
        public void ExtractPropertyIdFromUrl_InvalidTest()
        {
            Assert.IsNull(ProtectedMediaHandler.ExtractPropertyIdFromUrl("/test"));
            Assert.IsNull(ProtectedMediaHandler.ExtractPropertyIdFromUrl("/test/"));
            Assert.IsNull(ProtectedMediaHandler.ExtractPropertyIdFromUrl("/test/media/test"));
            Assert.IsNull(ProtectedMediaHandler.ExtractPropertyIdFromUrl("/media/sdaasd/asdfasdf.jpg"));
        }
    }
}
