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
        [WebInvoke(Method = "GET", UriTemplate = "ML/Master/Accommodation/{Logid}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        void ML_DataTransferMasterAccommodation(string Logid);
        #endregion


        #region *** MasterAccommodationRoomFacilities ***
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "ML/Master/AccommodationRoomFacilities/{Logid}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        void ML_DataTransferMasterAccommodationRoomFacilities(string Logid);
        #endregion
        #region *** MasterAccommodationRoomInformation ***
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "ML/Master/AccommodationRoomInformation/{Logid}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        void ML_DataTransferMasterAccommodationRoomInformation(string Logid);
        #endregion
        #region *** RoomTypeMatching ***
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "ML/RoomTypeMatching/{Logid}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        void ML_DataTransferRoomTypeMatching(string Logid);
        #endregion
        #region *** SupplierAccommodationData ***
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "ML/Supplier/AccommodationData/{Logid}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        void ML_DataTransferSupplierAccommodationData(string Logid);
        #endregion
        #region *** SupplierAccommodationRoomData ***
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "ML/Supplier/AccommodationRoomData/{Logid}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        void ML_DataTransferSupplierAccommodationRoomData(string Logid);
        #endregion
        #region *** SupplierAccommodationRoomExtendedAttributes ***
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "ML/Supplier/AccommodationRoomExtendedAttributes/{Logid}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        void ML_DataTransferSupplierAccommodationRoomExtendedAttributes(string Logid);
        #endregion

        #region *** SupplierAccommodationRoomExtendedAttributes ***
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "ML/DataAPITransfer/status", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DataContracts.ML.DL_ML_DL_EntityStatus> GetMLDataApiTransferStatus();
        #endregion
    }
}
