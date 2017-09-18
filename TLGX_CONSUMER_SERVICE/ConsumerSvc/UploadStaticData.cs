using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using OperationContracts;
using DataContracts;
using BusinessLayer;
using System.Data;

namespace ConsumerSvc
{
    public partial class Consumer : IConsumer
    {
        #region "Mapping Config Attributes"
        public List<DataContracts.UploadStaticData.DC_SupplierImportAttributes> GetStaticDataMappingAttributes(DataContracts.UploadStaticData.DC_SupplierImportAttributes_RQ obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.GetStaticDataMappingAttributes(obj);
            }
        }

        public DataContracts.DC_Message AddStaticDataMappingAttribute(DataContracts.UploadStaticData.DC_SupplierImportAttributes obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.AddStaticDataMappingAttribute(obj);
            }
        }

        public DataContracts.DC_Message UpdateStaticDataMappingAttribute(List<DataContracts.UploadStaticData.DC_SupplierImportAttributes> obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.UpdateStaticDataMappingAttribute(obj);
            }
        }

        public DataContracts.DC_Message UpdateStaticDataMappingAttributeStatus(List<DataContracts.UploadStaticData.DC_SupplierImportAttributes> obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.UpdateStaticDataMappingAttributeStatus(obj);
            }
        }
        #endregion

        #region "Mapping Config Attributes Values"
        public List<DataContracts.UploadStaticData.DC_SupplierImportAttributeValues> GetStaticDataMappingAttributeValues(DataContracts.UploadStaticData.DC_SupplierImportAttributeValues_RQ obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.GetStaticDataMappingAttributeValues(obj);
            }
        }

        public DataContracts.DC_Message AddStaticDataMappingAttributeValues(DataContracts.UploadStaticData.DC_SupplierImportAttributeValues obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.AddStaticDataMappingAttributeValues(obj);
            }
        }

        public DataContracts.DC_Message UpdateStaticDataMappingAttributeValues(List<DataContracts.UploadStaticData.DC_SupplierImportAttributeValues> obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.UpdateStaticDataMappingAttributeValues(obj);
            }
        }

        public DataContracts.DC_Message UpdateStaticDataMappingAttributeValueStatus(List<DataContracts.UploadStaticData.DC_SupplierImportAttributeValues> obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.UpdateStaticDataMappingAttributeValueStatus(obj);
            }
        }
        public List<string> GetStaticDataMappingAttributeValuesForFilter(DataContracts.UploadStaticData.DC_SupplierImportAttributeValues_RQ RQ)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.GetStaticDataMappingAttributeValuesForFilter(RQ);
            }
        }
        #endregion


        #region "Upload File"
        public List<DataContracts.UploadStaticData.DC_SupplierImportFileDetails> GetStaticDataFileDetail(DataContracts.UploadStaticData.DC_SupplierImportFileDetails_RQ obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.GetStaticDataFileDetail(obj);
            }
        }

        public DataContracts.DC_Message AddStaticDataFileDetail(DataContracts.UploadStaticData.DC_SupplierImportFileDetails obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.AddStaticDataFileDetail(obj);
            }
        }

        public DataContracts.DC_Message UpdateStaticDataFileDetail(List<DataContracts.UploadStaticData.DC_SupplierImportFileDetails> obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.UpdateStaticDataFileDetail(obj);
            }
        }

        public DataContracts.DC_Message UpdateStaticDataFileDetailStatus(DataContracts.UploadStaticData.DC_SupplierImportFileDetails obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.UpdateStaticDataFileDetailStatus(obj);
            }
        }
        #endregion


        #region "Logging"
        public DataContracts.DC_Message AddStaticDataUploadErrorLog(DataContracts.UploadStaticData.DC_SupplierImportFile_ErrorLog obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.AddStaticDataUploadErrorLog(obj);
            }
        }
        public List<DataContracts.UploadStaticData.DC_SupplierImportFile_ErrorLog> GetStaticDataUploadErrorLog(DataContracts.UploadStaticData.DC_SupplierImportFile_ErrorLog_RQ obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.GetStaticDataUploadErrorLog(obj);
            }
        }
        public List<DataContracts.UploadStaticData.DC_SupplierImportFile_Progress> GetStaticDataUploadProcessLog(DataContracts.UploadStaticData.DC_SupplierImportFile_Progress_RQ obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.GetStaticDataUploadProcessLog(obj);
            }
        }
        public List<DataContracts.UploadStaticData.DC_SupplierImportFile_VerboseLog> GetStaticDataUploadVerboseLog(DataContracts.UploadStaticData.DC_SupplierImportFile_VerboseLog_RQ obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.GetStaticDataUploadVerboseLog(obj);
            }
        }

        public List<DataContracts.UploadStaticData.DC_SupplierImportFile_Statistics> GetStaticDataUploadStatistics(DataContracts.UploadStaticData.DC_SupplierImportFile_Statistics_RQ RQ)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.GetStaticDataUploadStatistics(RQ);
            }
        }
        #endregion

        #region Process Or Test Uploaded Files
        public DataContracts.DC_Message StaticFileUploadProcessFile(DataContracts.UploadStaticData.DC_SupplierImportFileDetails obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.StaticFileUploadProcessFile(obj);
            }
        }

        public DataSet StaticFileUpload_TestFile_Read(DataContracts.UploadStaticData.DC_SupplierImportFileDetails_TestProcess obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.StaticFileUpload_TestFile_Read(obj);
            }
        }

        public DataSet StaticFileUpload_TestFile_Transform(DataContracts.UploadStaticData.DC_SupplierImportFileDetails_TestProcess obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.StaticFileUpload_TestFile_Transform(obj);
            }
        }
        #endregion
        #region File Progress DashBoard
        public DataContracts.DC_FileProgressDashboard getFileProgressDashBoardData(string fileid)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.getFileProgressDashBoardData(fileid);
            }
        }
        #endregion

    }
}