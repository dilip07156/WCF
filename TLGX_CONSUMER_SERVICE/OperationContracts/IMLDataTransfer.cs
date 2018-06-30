using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace OperationContracts
{
    [ServiceContract]
    public interface IMLDataTransfer
    {
        #region *** MasterAccommodationRecord ***
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "ML/Master/Accommodation", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        string ML_DataTransferMasterAccommodation();
        #endregion


        #region *** MasterAccommodationRoomFacilities ***
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "ML/Master/AccommodationRoomFacilities", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        string ML_DataTransferMasterAccommodationRoomFacilities();
        #endregion
        #region *** MasterAccommodationRoomInformation ***
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "ML/Master/AccommodationRoomInformation", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        string ML_DataTransferMasterAccommodationRoomInformation();
        #endregion
        #region *** RoomTypeMatching ***
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "ML/RoomTypeMatching", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        string ML_DataTransferRoomTypeMatching();
        #endregion
        #region *** SupplierAccommodationData ***
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "ML/Supplier/AccommodationData", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        string ML_DataTransferSupplierAccommodationData();
        #endregion
        #region *** SupplierAccommodationRoomData ***
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "ML/Supplier/AccommodationRoomData", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        string ML_DataTransferSupplierAccommodationRoomData();
        #endregion
        #region *** SupplierAccommodationRoomExtendedAttributes ***
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "ML/Supplier/AccommodationRoomExtendedAttributes", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        string ML_DataTransferSupplierAccommodationRoomExtendedAttributes();
        #endregion
    }
}
