﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;
using DataLayer;
using System.Data;
using DataContracts.STG;
using DataContracts.Mapping;

namespace BusinessLayer
{
    public class BL_UploadStaticData : IDisposable
    {
        public void Dispose()
        {
        }

        #region "Mapping Config Attributes"
        public List<DataContracts.UploadStaticData.DC_SupplierImportAttributes> GetStaticDataMappingAttributes(DataContracts.UploadStaticData.DC_SupplierImportAttributes_RQ obj)
        {
            using (DataLayer.DL_UploadStaticData objBL = new DataLayer.DL_UploadStaticData())
            {
                return objBL.GetStaticDataMappingAttributes(obj);
            }
        }

        public DataContracts.DC_Message AddStaticDataMappingAttribute(DataContracts.UploadStaticData.DC_SupplierImportAttributes obj)
        {
            DataContracts.DC_Message dc = new DataContracts.DC_Message();
            Guid _SupplierImportAttribute_Id = obj.SupplierImportAttribute_Id;
            Guid _Supplier_Id = obj.Supplier_Id;
            string For = obj.For;
            bool toAdd = true;

            if (string.IsNullOrWhiteSpace(For))
            {
                if (_Supplier_Id == Guid.Empty)
                {
                    toAdd = false;
                    dc.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Failed;
                    dc.StatusMessage = "Supplier Id not passed";
                }
            }
            if (toAdd)
            {
                using (DataLayer.DL_UploadStaticData objBL = new DataLayer.DL_UploadStaticData())
                {
                    dc = objBL.AddStaticDataMappingAttribute(obj);
                }
            }
            return dc;
        }
        public DataContracts.DC_Message UpdateStaticDataMappingAttribute(List<DataContracts.UploadStaticData.DC_SupplierImportAttributes> obj)
        {
            using (DataLayer.DL_UploadStaticData objBL = new DataLayer.DL_UploadStaticData())
            {
                return objBL.UpdateStaticDataMappingAttribute(obj);
            }
        }
        public DataContracts.DC_Message UpdateStaticDataMappingAttributeStatus(List<DataContracts.UploadStaticData.DC_SupplierImportAttributes> obj)
        {
            using (DataLayer.DL_UploadStaticData objBL = new DataLayer.DL_UploadStaticData())
            {
                return objBL.UpdateStaticDataMappingAttributeStatus(obj);
            }
        }
        #endregion

        #region "Mapping Config Attributes Values"
        public List<DataContracts.UploadStaticData.DC_SupplierImportAttributeValues> GetStaticDataMappingAttributeValues(DataContracts.UploadStaticData.DC_SupplierImportAttributeValues_RQ obj)
        {
            using (DataLayer.DL_UploadStaticData objBL = new DataLayer.DL_UploadStaticData())
            {
                return objBL.GetStaticDataMappingAttributeValues(obj);
            }
        }

        public DataContracts.DC_Message AddStaticDataMappingAttributeValues(DataContracts.UploadStaticData.DC_SupplierImportAttributeValues obj)
        {
            DataContracts.DC_Message dc = new DataContracts.DC_Message();
            using (DataLayer.DL_UploadStaticData objBL = new DataLayer.DL_UploadStaticData())
            {
                return objBL.AddStaticDataMappingAttributeValues(obj);
            }
        }
        public DataContracts.DC_Message UpdateStaticDataMappingAttributeValues(List<DataContracts.UploadStaticData.DC_SupplierImportAttributeValues> obj)
        {
            using (DataLayer.DL_UploadStaticData objBL = new DataLayer.DL_UploadStaticData())
            {
                return objBL.UpdateStaticDataMappingAttributeValues(obj);
            }
        }
        public DataContracts.DC_Message UpdateStaticDataMappingAttributeValueStatus(List<DataContracts.UploadStaticData.DC_SupplierImportAttributeValues> obj)
        {
            using (DataLayer.DL_UploadStaticData objBL = new DataLayer.DL_UploadStaticData())
            {
                return objBL.UpdateStaticDataMappingAttributeValueStatus(obj);
            }
        }

        public List<string> GetStaticDataMappingAttributeValuesForFilter(DataContracts.UploadStaticData.DC_SupplierImportAttributeValues_RQ RQ)
        {
            using (DataLayer.DL_UploadStaticData objBL = new DataLayer.DL_UploadStaticData())
            {
                return objBL.GetStaticDataMappingAttributeValuesForFilter(RQ);
            }
        }
        #endregion

        #region "Upload File"
        public List<DataContracts.UploadStaticData.DC_SupplierImportFileDetails> GetStaticDataFileDetail(DataContracts.UploadStaticData.DC_SupplierImportFileDetails_RQ obj)
        {
            using (DataLayer.DL_UploadStaticData objBL = new DataLayer.DL_UploadStaticData())
            {
                return objBL.GetStaticDataFileDetail(obj);
            }
        }
        public DataContracts.DC_Message AddStaticDataFileDetail(DataContracts.UploadStaticData.DC_SupplierImportFileDetails obj)
        {
            using (DataLayer.DL_UploadStaticData objBL = new DataLayer.DL_UploadStaticData())
            {
                return objBL.AddStaticDataFileDetail(obj);
            }
        }

        public DataContracts.UploadStaticData.DC_SupplierImportFileDetails AddStaticDataFileDetailForMongo(string SupplierId, string Entity)
        {
            using (DataLayer.DL_UploadStaticData objBL = new DataLayer.DL_UploadStaticData())
            {
                return objBL.AddStaticDataFileDetailForMongo(SupplierId, Entity);
            }
        }

        public DataContracts.DC_Message UpdateStaticDataFileDetail(List<DataContracts.UploadStaticData.DC_SupplierImportFileDetails> obj)
        {
            using (DataLayer.DL_UploadStaticData objBL = new DataLayer.DL_UploadStaticData())
            {
                return objBL.UpdateStaticDataFileDetail(obj);
            }
        }
        public DataContracts.DC_Message UpdateStaticDataFileDetailStatus(DataContracts.UploadStaticData.DC_SupplierImportFileDetails obj)
        {
            using (DataLayer.DL_UploadStaticData objBL = new DataLayer.DL_UploadStaticData())
            {
                return objBL.UpdateStaticDataFileDetailStatus(obj);
            }
        }


        #endregion

        #region "Logging"
        public DataContracts.DC_Message AddStaticDataUploadErrorLog(DataContracts.UploadStaticData.DC_SupplierImportFile_ErrorLog obj)
        {
            using (DataLayer.DL_UploadStaticData objBL = new DataLayer.DL_UploadStaticData())
            {
                return objBL.AddStaticDataUploadErrorLog(obj);
            }
        }

        public List<DataContracts.UploadStaticData.DC_SupplierImportFile_ErrorLog> GetStaticDataUploadErrorLog(DataContracts.UploadStaticData.DC_SupplierImportFile_ErrorLog_RQ obj)
        {
            using (DataLayer.DL_UploadStaticData objBL = new DataLayer.DL_UploadStaticData())
            {
                return objBL.GetStaticDataUploadErrorLog(obj);
            }
        }

        public List<DataContracts.UploadStaticData.DC_SupplierImportFile_Statistics> GetStaticDataUploadStatistics(DataContracts.UploadStaticData.DC_SupplierImportFile_Statistics_RQ RQ)
        {
            using (DataLayer.DL_UploadStaticData objBL = new DataLayer.DL_UploadStaticData())
            {
                return objBL.GetStaticDataUploadStatistics(RQ);
            }
        }
        public DataContracts.DC_Message AddStaticDataUploadProcessLog(DataContracts.UploadStaticData.DC_SupplierImportFile_Progress obj)
        {
            using (DataLayer.DL_UploadStaticData objBL = new DataLayer.DL_UploadStaticData())
            {
                return objBL.AddStaticDataUploadProcessLog(obj);
            }
        }

        public DataContracts.DC_Message AddStaticDataUploadStatistics(DataContracts.UploadStaticData.DC_SupplierImportFile_Statistics obj)
        {
            using (DataLayer.DL_UploadStaticData objBL = new DataLayer.DL_UploadStaticData())
            {
                return objBL.AddStaticDataUploadStatistics(obj);
            }
        }
        public List<DataContracts.UploadStaticData.DC_SupplierImportFile_Progress> GetStaticDataUploadProcessLog(DataContracts.UploadStaticData.DC_SupplierImportFile_Progress_RQ obj)
        {
            using (DataLayer.DL_UploadStaticData objBL = new DataLayer.DL_UploadStaticData())
            {
                return objBL.GetStaticDataUploadProcessLog(obj);
            }
        }
        public DataContracts.DC_Message AddStaticDataUploadVerboseLog(DataContracts.UploadStaticData.DC_SupplierImportFile_VerboseLog obj)
        {
            using (DataLayer.DL_UploadStaticData objBL = new DataLayer.DL_UploadStaticData())
            {
                return objBL.AddStaticDataUploadVerboseLog(obj);
            }
        }
        public List<DataContracts.UploadStaticData.DC_SupplierImportFile_VerboseLog> GetStaticDataUploadVerboseLog(DataContracts.UploadStaticData.DC_SupplierImportFile_VerboseLog_RQ obj)
        {
            using (DataLayer.DL_UploadStaticData objBL = new DataLayer.DL_UploadStaticData())
            {
                return objBL.GetStaticDataUploadVerboseLog(obj);
            }
        }
        #endregion

        #region "STG Tables"
        public DataContracts.DC_Message AddSTGCountryData(List<DataContracts.STG.DC_stg_SupplierCountryMapping> obj)
        {
            using (DataLayer.DL_UploadStaticData objBL = new DataLayer.DL_UploadStaticData())
            {
                return objBL.AddSTGCountryData(obj);
            }
        }
        public DataContracts.DC_Message AddSTGCityData(List<DataContracts.STG.DC_stg_SupplierCityMapping> obj)
        {
            using (DataLayer.DL_UploadStaticData objBL = new DataLayer.DL_UploadStaticData())
            {
                return objBL.AddSTGCityData(obj);
            }
        }
        public DataContracts.DC_Message AddSTGProductData(List<DataContracts.STG.DC_stg_SupplierProductMapping> obj)
        {
            using (DataLayer.DL_UploadStaticData objBL = new DataLayer.DL_UploadStaticData())
            {
                return objBL.AddSTGProductData(obj);
            }
        }

        public DataContracts.DC_Message AddSTGRoomTypeData(List<DataContracts.STG.DC_stg_SupplierHotelRoomMapping> obj)
        {
            using (DataLayer.DL_UploadStaticData objBL = new DataLayer.DL_UploadStaticData())
            {
                return objBL.AddSTGRoomTypeData(obj);
            }
        }

        public List<DataContracts.STG.DC_stg_SupplierCountryMapping> GetSTGCountryData(DataContracts.STG.DC_stg_SupplierCountryMapping_RQ obj)
        {
            using (DataLayer.DL_UploadStaticData objBL = new DataLayer.DL_UploadStaticData())
            {
                return objBL.GetSTGCountryData(obj);
            }
        }

        public List<DataContracts.STG.DC_stg_SupplierCityMapping> GetSTGCityData(DataContracts.STG.DC_stg_SupplierCityMapping_RQ obj)
        {
            using (DataLayer.DL_UploadStaticData objBL = new DataLayer.DL_UploadStaticData())
            {
                return objBL.GetSTGCityData(obj);
            }
        }
        public List<DC_stg_SupplierProductMapping> GetSTGHotelData(DC_stg_SupplierProductMapping_RQ obj)
        {
            using (DataLayer.DL_UploadStaticData objBL = new DataLayer.DL_UploadStaticData())
            {
                return objBL.GetSTGHotelData(obj);
            }
        }
        public List<DC_stg_SupplierHotelRoomMapping> GetSTGRoomTypeData(DC_stg_SupplierHotelRoomMapping_RQ obj)
        {
            using (DataLayer.DL_UploadStaticData objBL = new DataLayer.DL_UploadStaticData())
            {
                return objBL.GetSTGRoomTypeData(obj);
            }
        }

        public List<DC_Accommodation_SupplierRoomTypeMapping_Online> RoomTypeMappingOnline_Insert(List<DC_Accommodation_SupplierRoomTypeMapping_Online> obj)
        {
            using (DataLayer.DL_UploadStaticData objBL = new DataLayer.DL_UploadStaticData())
            {
                return objBL.RoomTypeMappingOnline_Insert(obj);
            }
        }

        public int Get_STG_Record_Count(string SupplierImportFile_Id, string Entity)
        {
            if (Guid.TryParse(SupplierImportFile_Id, out Guid gSupplierImportFile_Id))
            {
                using (DataLayer.DL_UploadStaticData objDL = new DataLayer.DL_UploadStaticData())
                {
                    return objDL.Get_STG_Record_Count(gSupplierImportFile_Id, Entity);
                }
            }
            else
            {
                return 0;
            }
        }

        public DataContracts.DC_Message DeDupe_EntityMapping_FromSTG(string SupplierImportFile_Id, string Entity)
        {
            if (Guid.TryParse(SupplierImportFile_Id, out Guid gSupplierImportFile_Id))
            {
                using (DataLayer.DL_UploadStaticData objDL = new DataLayer.DL_UploadStaticData())
                {
                    return objDL.DeDupe_EntityMapping_FromSTG(gSupplierImportFile_Id, Entity);
                }
            }
            else
            {
                return new DataContracts.DC_Message { StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Danger, StatusMessage = "Invalid Supplier Import File Id" };
            }
        }

        public DataContracts.DC_Message STG_Cleanup(string SupplierImportFile_Id, string Entity)
        {
            if (Guid.TryParse(SupplierImportFile_Id, out Guid gSupplierImportFile_Id))
            {
                using (DataLayer.DL_UploadStaticData objDL = new DataLayer.DL_UploadStaticData())
                {
                    return objDL.STG_Cleanup(gSupplierImportFile_Id, Entity);
                }
            }
            else
            {
                return new DataContracts.DC_Message { StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Danger, StatusMessage = "Invalid Supplier Import File Id" };
            }
        }

        #endregion

        #region Process Or Test Uploaded Files
        public DataContracts.DC_Message StaticFileUploadProcessFile(DataContracts.UploadStaticData.DC_SupplierImportFileDetails obj)
        {
            using (DL_UploadStaticData objBL = new DL_UploadStaticData())
            {
                return objBL.StaticFileUploadProcessFile(obj);
            }
        }

        public DataSet StaticFileUpload_TestFile_Read(DataContracts.UploadStaticData.DC_SupplierImportFileDetails_TestProcess obj)
        {
            using (DL_UploadStaticData objBL = new DL_UploadStaticData())
            {
                return objBL.StaticFileUpload_TestFile_Read(obj);
            }
        }

        public DataSet StaticFileUpload_TestFile_Transform(DataContracts.UploadStaticData.DC_SupplierImportFileDetails_TestProcess obj)
        {
            using (DL_UploadStaticData objBL = new DL_UploadStaticData())
            {
                return objBL.StaticFileUpload_TestFile_Transform(obj);
            }
        }
        #endregion

        #region  File Progress DashBoard
        public DataContracts.DC_FileProgressDashboard getFileProgressDashBoardData(string fileid)
        {
            Guid gfileid;

            if (!Guid.TryParse(fileid, out gfileid))
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
            }
            else
            {
                using (DL_UploadStaticData objBL = new DL_UploadStaticData())
                {
                    return objBL.getFileProgressDashBoardData(gfileid);
                }
            }
        }
        #endregion

        #region
        public DataContracts.DC_Message UpdateSupplierImportFileDetails(DataContracts.UploadStaticData.DC_SupplierImportFileDetails obj)
        {
            using (DataLayer.DL_UploadStaticData objBL = new DataLayer.DL_UploadStaticData())
            {
                return objBL.UpdateSupplierImportFileDetails(obj);
            }
        }

        public DataContracts.DC_Message UpdateSupplierImportFileDetailsFromNewToUploaded(DataContracts.UploadStaticData.DC_SupplierImportFileDetails obj)
        {
            using (DataLayer.DL_UploadStaticData objBL = new DataLayer.DL_UploadStaticData())
            {
                return objBL.UpdateSupplierImportFileDetailsFromNewToUploaded(obj);
            }
        }
        #endregion


        #region File Processing Check
        //GAURAV_TMAP_746
        public DataContracts.DC_Message FileProcessingCheckInSupplierImportFileDetails(string SupplierId)
        {
            using (DataLayer.DL_UploadStaticData objBL = new DataLayer.DL_UploadStaticData())
            {
                return objBL.FileProcessingCheckInSupplierImportFileDetails(SupplierId);
            }
        }
        #endregion
    }
}
