using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UvpClient.Services;

namespace UvpClient.UnitTest.ViewModels
{
    [TestClass]
    public class MeViewModelTest
    {
        [TestMethod]
        public void TestGetCommand() {
            var getMeRequested = false;
            var stubIStudentService = new StubIStudentService();
        }
    }
}
