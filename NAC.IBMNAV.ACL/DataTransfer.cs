using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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



    }
}
