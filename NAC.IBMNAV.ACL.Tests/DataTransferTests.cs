using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NAC.IBMNAV.ACL;
using System.Diagnostics;

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

            // assert
        }

        [TestMethod]
        public void TestFileUploadToAmassis()
        {

            // Just a simple app to call the java acs jar file.
            //string javalocation = "C:\\Program Files\\Java\\jre1.8.0_151\\bin\\java.exe";
            string acslocation = "C:\\Users\\Public\\IBM\\ClientSolutions\\acsbundle.jar";
            string pluginCommand = "/PLUGIN=UPLOAD";
            string dataDefinition = "/FILE=E:\\IBM_DATA_TRANSFER_DEFINITIONS\\DATALIB_IFRETDF.dttx";

            string command = @"java -jar " + acslocation + " " + pluginCommand + " " + dataDefinition;

            int exitCode;
            ProcessStartInfo processInfo;
            Process process;

            processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            // *** Redirect the output ***
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            process = Process.Start(processInfo);
            process.WaitForExit();

            // *** Read the streams ***
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            exitCode = process.ExitCode;

            Console.WriteLine("output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output));
            Console.WriteLine("error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error));
            Console.WriteLine("ExitCode: " + exitCode.ToString(), "ExecuteCommand");
            process.Close();



            // arrange
            // assign
            // assert
            Assert.IsTrue(String.IsNullOrEmpty(error));
        }

        [TestMethod]
        public void TestFileDownloadFromAmassis()
        {

            // Just a simple app to call the java acs jar file.
            //string javalocation = "C:\\Program Files\\Java\\jre1.8.0_151\\bin\\java.exe";
            string acslocation = "C:\\Users\\Public\\IBM\\ClientSolutions\\acsbundle.jar";
            string pluginCommand = "/PLUGIN=DOWNLOAD";
            string dataDefinition = "/FILE=E:\\IBM_DATA_TRANSFER_DEFINITIONS\\DATALIB_IFBATDF.dtfx";

            string command = @"java -jar " + acslocation + " " + pluginCommand + " " + dataDefinition;

            int exitCode;
            ProcessStartInfo processInfo;
            Process process;

            processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            // *** Redirect the output ***
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            process = Process.Start(processInfo);
            process.WaitForExit();

            // *** Read the streams ***
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            exitCode = process.ExitCode;

            Console.WriteLine("output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output));
            Console.WriteLine("error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error));
            Console.WriteLine("ExitCode: " + exitCode.ToString(), "ExecuteCommand");
            process.Close();




            // arrange
            // assign
            // assert

            Assert.IsTrue(String.IsNullOrEmpty(error));
        }
    }
}
