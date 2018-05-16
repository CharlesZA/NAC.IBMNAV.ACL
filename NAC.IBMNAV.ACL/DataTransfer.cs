using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace NAC.IBMNAV.ACL
{
    /// <summary>
    /// This class calls the upload and download from the iseries using the IBM i Access Client Solutions
    /// </summary>
    public sealed class DataTransfer
    {
        public DataTransfer()
        {
            EnableUpload = false;
            ACSBundleLocation = "";
            DataDefinitionLocation = "";
        }

        /// <summary>
        /// This property contains the location of the acsbundle.jar file.
        /// </summary>
        public string ACSBundleLocation { get; set; }


        /// <summary>
        /// This property will set the command for upload. The default if download.
        /// </summary>
        public bool EnableUpload { get; set; }

        /// <summary>
        /// This property will set the data definition file for that will be sent with the upload or download request.
        /// </summary>
        public string DataDefinitionLocation { get; set; }

        public bool ExecuteTransfer()
        {
            try
            {
                int exitCode;
                ProcessStartInfo processInfo;
                Process process;
                string command;
                string pluginCommand;
                string fileDefinitionLocation;

                fileDefinitionLocation = SetFileDefinitionLocation();
                pluginCommand = SetPluginCommand();
                CheckACSBundleLocation();


                command = @"java -jar " + ACSBundleLocation + " " + pluginCommand + " " + fileDefinitionLocation;

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


                // Log event
                LogExecureTransferEvent(output, error, exitCode);

            }
            catch (Exception ex)
            {
                LogExecuteTransferExceptionEvent(ex);

                throw;
            }

            return true;
        }

        private void CheckACSBundleLocation()
        {
            if (String.IsNullOrWhiteSpace(ACSBundleLocation))
            {
                throw new ArgumentNullException("acsbundle.jar location has not been specified");
            }

            if (File.Exists(ACSBundleLocation) == false)
            {
                throw new FileNotFoundException($"The IBM iSeries Client Access application could not be located in {ACSBundleLocation}");
            }

        }

        private void LogExecureTransferEvent(string output, string error, int exitCode)
        {
            // Success Message
            string sSource;
            string sLog;
            string sEvent;

            sSource = "NAC.IBMNAV.ACL";
            sLog = "Application";

            if (EnableUpload)
            {
                sEvent = "DataTransfer::ExecuteTransfer(): Upload Action ";
            }
            sEvent = "DataTransfer::ExecuteTransfer(): Download Action ";

            try
            {
                if (!EventLog.SourceExists(sSource))
                    EventLog.CreateEventSource(sSource, sLog);

                EventLog.WriteEntry(sSource, sEvent + output, EventLogEntryType.Information, 234);

            }
            catch (System.Security.SecurityException)
            {

                // bypass
            }

        }

        private static void LogExecuteTransferExceptionEvent(Exception ex)
        {
            string sSource;
            string sLog;
            string sEvent;

            sSource = "NAC.IBMNAV.ACL";
            sLog = "Application";
            sEvent = "DataTransfer::ExecuteTransfer() Error: ";

            try
            {
                if (!EventLog.SourceExists(sSource))
                    EventLog.CreateEventSource(sSource, sLog);

                EventLog.WriteEntry(sSource, sEvent + ex.Message, EventLogEntryType.Error, 234);

            }
            catch (System.Security.SecurityException)
            {

                // bypass
            }
        }

        private string SetFileDefinitionLocation()
        {
            // todo: add file extension checks here eg: dtfx and dttx

            if (File.Exists(DataDefinitionLocation) == false)
            {
                throw new FileNotFoundException($"DataDefinitionLocation could not be found: {DataDefinitionLocation}");
            }

            string extensionToCheck = Path.GetExtension(DataDefinitionLocation);
            if (extensionToCheck != ".dtfx" && extensionToCheck != ".dttx")
            {
                throw new FileNotFoundException("Invalid Data File definition");
            }

            if (EnableUpload)
            {
                if (extensionToCheck != ".dttx")
                {
                    throw new FileNotFoundException("Invalid Data File definition. The extionsion must be dttx.");
                }
            }
            else
            {
                if (extensionToCheck != ".dtfx")
                {
                    throw new FileNotFoundException("Invalid Data File definition. The extionsion must be dtfx.");
                }
            }

            return "/FILE=" + DataDefinitionLocation;
        }

        private string SetPluginCommand()
        {
            string pluginCommand;
            if (EnableUpload)
            {
                pluginCommand = "/PLUGIN=UPLOAD";
            }
            else
            {
                pluginCommand = "/PLUGIN=DOWNLOAD";
            }

            return pluginCommand;
        }
    }
}
