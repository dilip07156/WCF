using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    [DataContract]
    public static class ReadOnlyMessage
    {
        //Application Status Message
        [DataMember]
        public static string strAlreadyExist = " already exist in system.";
        [DataMember]
        public static string strnotExist = " not exist in system.";
        [DataMember]
        public static string strAddedSuccessfully = " has been added successfully.";
        [DataMember]
        public static string strUpdatedSuccessfully = " has been updated successfully.";
        [DataMember]
        public static string strFailed = "Service Request Failed";
        [DataMember]
        public static string strDeleted = " has been deleted successfully.";
        [DataMember]
        public static string strUnDeleted = " has been un deleted successfully.";
        [DataMember]
        public static string strStopped = " has been Stopped.";
        [DataMember]
        public static string strPaused = " has been Paused.";

        public enum StatusCode
        {
            Plain,

            Success,

            Information,

            Warning,

            Danger,

            Primary,

            Duplicate,

            Failed,

            Stopped,

            Paused,
        };


    }
}
