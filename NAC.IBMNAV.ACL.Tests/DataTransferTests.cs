using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NAC.IBMNAV.ACL;
using System.Diagnostics;
using System.IO;

namespace NAC.IBMNAV.ACL.Tests
{
    [TestClass]
    public class DataTransferTests
    {

        [TestMethod]
        public void ExecuteTransferTest()
        {
            // arrange
            DataTransfer dataTransfer = new DataTransfer();

            // assign
            dataTransfer.ACSBundleLocation = @"C:\Users\Public\IBM\ClientSolutions\acsbundle.jar";
            dataTransfer.DataDefinitionLocation = @"E:\IBM_DATA_TRANSFER_DEFINITIONS\DATALIB_IFBATDF.dtfx";

            dataTransfer.ExecuteTransfer();

            // assert

            Assert.IsTrue(File.Exists(@"E:\IBM DATA TRANSFER DATA\DATALIB_IFBATDF.txt"));
        }
    }
}
