using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.UploadStaticData
{
    public class DC_ErrorLog_Format
    {
        public System.Guid Err_SupplierImportFile_Id { get; set; }
        public object Err_Object { get; set; }
        public string Err_Namespace { get; set; }
        public string Err_ClassName { get; set; }
        public string Err_MethodName { get; set; }
        public int Err_LineNumber { get; set; }
        public int Err_ErrorCode { get; set; }
        public string Err_Type { get; set; }
        public string Err_SimpleMessage { get; set; }
    }
    public static class Error_Enums_DataHandler
    {
        //Application Status Message
        public static string strAlreadyExist = " already exist in system.";
        public static string strAddedSuccessfully = " has been added successfully.";
        public static string strUpdatedSuccessfully = " has been updated successfully.";
        public static string strFailed = "Service Request Failed";

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
        };

        public enum ErrorCodes
        {
            [Description("No_Content")]
            No_Content = 204,

            [Description("Bad_Request")]
            Bad_Request = 400,

            [Description("Unauthorized_Request")]
            Unauthorized_Request = 401,

            [Description("Forbidden")]
            Forbidden = 403,

            [Description("Not_Found")]
            Not_Found = 404,

            [Description("Method_Not_Allowed")]
            Method_Not_Allowed = 405,

            [Description("Not_Acceptable")]
            Not_Acceptable = 406,

            [Description("Proxy_Authentication_Required")]
            Proxy_Authentication_Required = 407,

            [Description("Request_Timeout")]
            Request_Timeout = 408,

            [Description("Length_Required")]
            Length_Required = 411,

            [Description("Precondition_Failed")]
            Precondition_Failed = 412,

            [Description("Request_Entity_Too_Large")]
            Request_Entity_Too_Large = 413,

            [Description("Request_Too_Long")]
            Request_Too_Long = 414,

            [Description("Unsupported_Type")]
            Unsupported_Type = 415,

            [Description("Internal_Server_Error")]
            Internal_Server_Error = 500,

            [Description("Not_Implemented")]
            Not_Implemented = 501,

            [Description("Bad_Gateway")]
            Bad_Gateway = 502,

            [Description("Service_Unavailable")]
            Service_Unavailable = 503,

            [Description("Authentication_Required")]
            Authentication_Required = 511,

            [Description("CountryUpload_Generic")]
            CountryUpload_Generic = 610,

            [Description("CountryConfig_Map")]
            CountryConfig_Map = 611,

            [Description("CountryConfig_Distinct")]
            CountryConfig_Distinct = 612,

            [Description("CountryConfig_Encode")]
            CountryConfig_Encode = 613,

            [Description("CountryConfig_Decode")]
            CountryConfig_Decode = 614,

            [Description("CountryConfig_Filter")]
            CountryConfig_Filter = 615,

            [Description("CountryConfig_Format")]
            CountryConfig_Format = 616,

            [Description("CountryConfig_Generic")]
            CountryConfig_Generic = 617,

            [Description("CountryMatching_Generic")]
            CountryMatching_Generic = 618,

            [Description("CityUpload_Generic")]
            CityUpload_Generic = 710,

            [Description("CityConfig_Map")]
            CityConfig_Map = 711,

            [Description("CityConfig_Distinct")]
            CityConfig_Distinct = 712,

            [Description("CityConfig_Encode")]
            CityConfig_Encode = 713,

            [Description("CityConfig_Decode")]
            CityConfig_Decode = 714,

            [Description("CityConfig_Filter")]
            CityConfig_Filter = 715,

            [Description("CityConfig_Format")]
            CityConfig_Format = 716,

            [Description("CityConfig_Generic")]
            CityConfig_Generic = 717,

            [Description("CityMatching_Generic")]
            CityMatching_Generic = 718,

            [Description("HotelUpload_Generic")]
            HotelUpload_Generic = 810,

            [Description("HotelConfig_Map")]
            HotelConfig_Map = 811,

            [Description("HotelConfig_Distinct")]
            HotelConfig_Distinct = 812,

            [Description("HotelConfig_Encode")]
            HotelConfig_Encode = 813,

            [Description("HotelConfig_Decode")]
            HotelConfig_Decode = 814,

            [Description("HotelConfig_Filter")]
            HotelConfig_Filter = 815,

            [Description("HotelConfig_Format")]
            HotelConfig_Format = 816,

            [Description("HotelConfig_Generic")]
            HotelConfig_Generic = 817,

            [Description("HotelMatching_Generic")]
            HotelMatching_Generic = 818,

            [Description("RoomTypeUpload_Generic")]
            RoomTypeUpload_Generic = 910,

            [Description("RoomTypeConfig_Map")]
            RoomTypeConfig_Map = 911,

            [Description("RoomTypeConfig_Distinct")]
            RoomTypeConfig_Distinct = 912,

            [Description("RoomTypeConfig_Encode")]
            RoomTypeConfig_Encode = 913,

            [Description("RoomTypeConfig_Decode")]
            RoomTypeConfig_Decode = 914,

            [Description("RoomTypeConfig_Filter")]
            RoomTypeConfig_Filter = 915,

            [Description("RoomTypeConfig_Format")]
            RoomTypeConfig_Format = 916,

            [Description("RoomTypeConfig_Generic")]
            RoomTypeConfig_Generic = 917,
        };

    }
}
