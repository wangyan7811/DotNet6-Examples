using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReadWriteAppSettingsJson.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadWriteAppSettingsJson.Models;

namespace ReadWriteAppSettingsJson.Helpers.Tests
{
    [TestClass()]
    public class ConfigurationJsonHelperTests
    {
        [TestMethod()]
        public void ReadTest()
        {
            //Arrange 
            var expected = "";

            //Act
            var actual = new ConfigurationJsonHelper().Read<Rootobject>();

            //Assert
            Assert.AreEqual(expected, actual, "You are wrong!!!");

        }

        [TestMethod()]
        public void WriteTest()
        {
            //Arrange 
            var expected = "";

            //Act
            var actual=new ConfigurationJsonHelper().Write<Rootobject>(null);

            //Assert
            Assert.AreEqual(expected, "", "You are wrong!!!");
        }

        
    }
}