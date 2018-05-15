using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

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

            }
            catch (Exception ex)
            {
                LogExecuteTransferEvent(ex);

                throw;
            }

            return true;
        }

        private static void LogExecuteTransferEvent(Exception ex)
        {
            string sSource;
            string sLog;
            string sEvent;

            sSource = "NAC.IBMNAV.ACL";
            sLog = "Application";
            sEvent = "DataTransfer::ExecuteTransfer() Error: ";

            if (!EventLog.SourceExists(sSource))
                EventLog.CreateEventSource(sSource, sLog);

            EventLog.WriteEntry(sSource, sEvent + ex.Message, EventLogEntryType.Error, 234);
        }

        private string SetFileDefinitionLocation()
        {
            // todo: add file extension checks here eg: dtfx and dttx
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
