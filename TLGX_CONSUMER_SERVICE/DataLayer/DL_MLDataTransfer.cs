using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using DataContracts.ML;

namespace DataLayer
{
    public enum PushStatus
    {
        SCHEDULED,
        RUNNNING,
        COMPLETED,
        ERROR
    }

    public class DL_MLDataTransfer : IDisposable
    {

        public void Dispose()
        {
        }

        #region *** MasterAccommodationRecord Done***
        public void ML_DataTransferMasterAccommodation(Guid LogId)
        {
            DataContracts.ML.DC_ML_DL_MasterAccoRecord _obj = new DataContracts.ML.DC_ML_DL_MasterAccoRecord();
            int TotalCount = 0;
            int MLDataInsertedCount = 0;
            try
            {
                UpdateDistLogInfo(LogId, PushStatus.RUNNNING);
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    //int total = 0;
                    //Get Batch Size
                    int BatchSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DataTransferBatchSize"]);
                    //Get Total Count
                    string strTotalCount = @"SELECT COUNT(1) FROM Accommodation with(nolock)";
                    context.Configuration.AutoDetectChangesEnabled = false;
                    try { TotalCount = context.Database.SqlQuery<int>(strTotalCount.ToString()).FirstOrDefault(); } catch (Exception ex) { }
                    int NoOfBatch = TotalCount / BatchSize;
                    int mod = TotalCount % BatchSize;
                    if (mod > 0)
                        NoOfBatch = NoOfBatch + 1;
                    for (int BatchNo = 0; BatchNo < NoOfBatch; BatchNo++)
                    {
                        _obj = GetMasterAccoDataForMLTrans(BatchSize, BatchNo);
                        #region To update CounterIn DistributionLog
                        MLDataInsertedCount = MLDataInsertedCount + _obj.MasterAccommodationRecord.Count();
                        UpdateDistLogInfo(LogId, PushStatus.RUNNNING, TotalCount, MLDataInsertedCount);
                        #endregion
                        object result = null;
                        DHSVCProxy.PostDataNewtonsoft(ProxyFor.MachingLearningDataTransfer, System.Configuration.ConfigurationManager.AppSettings["MLSVCURL_DataApi_Post_MasterAccommodationRecord"], _obj, typeof(DataContracts.ML.DC_ML_DL_MasterAccoRecord), typeof(DC_ML_Message), out result);
                    }
                    UpdateDistLogInfo(LogId, PushStatus.COMPLETED, TotalCount, MLDataInsertedCount);
                }
            }
            catch (Exception ex)
            {
                UpdateDistLogInfo(LogId, PushStatus.ERROR, TotalCount, MLDataInsertedCount);
                throw;
            }
            // return string.Empty;
        }

        private DC_ML_DL_MasterAccoRecord GetMasterAccoDataForMLTrans(int batchSize, int batchNo)
        {
            DataContracts.ML.DC_ML_DL_MasterAccoRecord _obj = new DataContracts.ML.DC_ML_DL_MasterAccoRecord();
            List<DataContracts.ML.DC_ML_DL_MasterAccoRecord_Data> _objData = new List<DataContracts.ML.DC_ML_DL_MasterAccoRecord_Data>();
            List<DataContracts.DC_ML_Acco_Data> _objAcoo = new List<DataContracts.DC_ML_Acco_Data>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    StringBuilder sbSelect = new StringBuilder();
                    StringBuilder sbOrderby = new StringBuilder();
                    sbSelect.Append(@"SELECT  
                                        Accommodation_Id As AccommodationId,
                                        CompanyHotelID As TLGXHotelId,
                                        HotelName As AccommodationName,
                                        city As City,
                                        country As Country,
                                        FullAddress AS Address,
                                        PostalCode As PostalCode,
                                        Create_Date AS CreateDate,
                                        Create_User AS CreateUser,
                                        Edit_Date AS EditDate,
                                        Edit_User As Edituser
                                        FROM Accommodation with(nolock)  ");
                    int skip = batchNo * batchSize;
                    sbOrderby.Append("  ORDER BY Accommodation_Id OFFSET " + (skip).ToString() + " ROWS FETCH NEXT " + batchSize.ToString() + " ROWS ONLY ");

                    StringBuilder sbfinal = new StringBuilder();
                    sbfinal.Append(sbSelect);
                    sbfinal.Append(sbOrderby);

                    context.Configuration.AutoDetectChangesEnabled = false;
                    try { _objAcoo = context.Database.SqlQuery<DataContracts.DC_ML_Acco_Data>(sbfinal.ToString()).ToList(); } catch (Exception ex) { }

                    _obj.MasterAccommodationRecord = new List<DataContracts.ML.DC_ML_DL_MasterAccoRecord_Data>();
                    foreach (var item in _objAcoo)
                    {
                        //_obj.MasterAccommodationRecord.Add(item);
                        _obj.MasterAccommodationRecord.Add(new DataContracts.ML.DC_ML_DL_MasterAccoRecord_Data
                        {
                            AccommodationId = Convert.ToString(item.AccommodationId),
                            AccommodationName = item.AccommodationName,
                            City = item.City,
                            Country = item.Country,
                            Address = item.Address,
                            CreateDate = Convert.ToString(item.CreateDate),
                            CreateUser = item.CreateUser,
                            EditDate = Convert.ToString(item.EditDate),
                            Edituser = item.Edituser,
                            PostalCode = item.PostalCode,
                            TLGXHotelId = item.TLGXHotelId
                        });
                    }
                    _obj.Mode = "offline";
                    _obj.BatchId = Convert.ToString(batchNo);
                    _obj.Transaction = "1";

                }

                return _obj;
            }
            catch (Exception ex)
            {
                throw;
            }


        }
        #endregion

        #region *** MasterAccommodationRoomFacilities ***
        public string ML_DataTransferMasterAccommodationRoomFacilities(Guid LogId)
        {
            DataContracts.ML.DC_ML_DL_MasterAccoRoomFacility _obj = new DataContracts.ML.DC_ML_DL_MasterAccoRoomFacility();
            int TotalCount = 0;
            int MLDataInsertedCount = 0;
            try
            {
                UpdateDistLogInfo(LogId, PushStatus.RUNNNING);

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    int total = 0;
                    //Get Batch Size
                    int BatchSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DataTransferBatchSize"]);
                    //Get Total Count
                    string strTotalCount = @"SELECT COUNT(1) FROM Accommodation_RoomFacility with(nolock)";
                    context.Configuration.AutoDetectChangesEnabled = false;
                    try { total = context.Database.SqlQuery<int>(strTotalCount.ToString()).FirstOrDefault(); } catch (Exception ex) { }
                    int NoOfBatch = total / BatchSize;
                    int mod = total % BatchSize;
                    if (mod > 0)
                        NoOfBatch = NoOfBatch + 1;
                    for (int BatchNo = 0; BatchNo < NoOfBatch; BatchNo++)
                    {
                        _obj = GetMasterAccoFacilityDataForMLTrans(BatchSize, BatchNo);
                        #region To update CounterIn DistributionLog
                        MLDataInsertedCount = MLDataInsertedCount + BatchSize;
                        UpdateDistLogInfo(LogId, PushStatus.RUNNNING, TotalCount, MLDataInsertedCount);
                        #endregion
                        object result = null;
                        DHSVCProxy.PostDataNewtonsoft(ProxyFor.MachingLearningDataTransfer, System.Configuration.ConfigurationManager.AppSettings["MLSVCURL_DataApi_Post_MasterAccommodationRoomFacilities"], _obj, typeof(DataContracts.ML.DC_ML_DL_MasterAccoRoomFacility), typeof(DC_ML_Message), out result);
                    }
                    UpdateDistLogInfo(LogId, PushStatus.COMPLETED, TotalCount, MLDataInsertedCount);
                }


            }
            catch (Exception ex)
            {
                UpdateDistLogInfo(LogId, PushStatus.ERROR, TotalCount, MLDataInsertedCount);
                throw;
            }
            return string.Empty;
        }
        private DC_ML_DL_MasterAccoRoomFacility GetMasterAccoFacilityDataForMLTrans(int batchSize, int batchNo)
        {
            DataContracts.ML.DC_ML_DL_MasterAccoRoomFacility _obj = new DataContracts.ML.DC_ML_DL_MasterAccoRoomFacility();
            List<DataContracts.ML.DC_ML_DL_MasterAccoRoomFacility_Data> _objData = new List<DataContracts.ML.DC_ML_DL_MasterAccoRoomFacility_Data>();
            List<DataContracts.DC_ML_MasterAccoRoomFacility_Data> _objAcooRoomFacility = new List<DataContracts.DC_ML_MasterAccoRoomFacility_Data>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    StringBuilder sbSelect = new StringBuilder();
                    StringBuilder sbOrderby = new StringBuilder();
                    sbSelect.Append(@"SELECT  
                                        Accommodation_RoomFacility_Id,
                                        Accommodation_Id,
                                        Accommodation_RoomInfo_Id,
                                        AmenityType,
                                        AmenityName,
                                        Description,
                                        Create_Date,
                                        Create_User,
                                        Edit_Date,
                                        Edit_user
                                        FROM Accommodation_RoomFacility with(nolock)  ");
                    int skip = batchNo * batchSize;
                    sbOrderby.Append("  ORDER BY Accommodation_Id OFFSET " + (skip).ToString() + " ROWS FETCH NEXT " + batchSize.ToString() + " ROWS ONLY ");

                    StringBuilder sbfinal = new StringBuilder();
                    sbfinal.Append(sbSelect);
                    sbfinal.Append(sbOrderby);

                    context.Configuration.AutoDetectChangesEnabled = false;
                    try { _objAcooRoomFacility = context.Database.SqlQuery<DataContracts.DC_ML_MasterAccoRoomFacility_Data>(sbfinal.ToString()).ToList(); } catch (Exception ex) { }

                    _obj.MasterAccommodationRoomFacilities = new List<DataContracts.ML.DC_ML_DL_MasterAccoRoomFacility_Data>();
                    foreach (var item in _objAcooRoomFacility)
                    {
                        //_obj.MasterAccommodationRecord.Add(item);
                        _obj.MasterAccommodationRoomFacilities.Add(new DataContracts.ML.DC_ML_DL_MasterAccoRoomFacility_Data
                        {
                            AccommodationRoomFacilityId = Convert.ToString(item.Accommodation_RoomFacility_Id),
                            AccommodationRoomInfoId = Convert.ToString(item.Accommodation_RoomInfo_Id),
                            AccommodationId = Convert.ToString(item.Accommodation_Id),
                            AmenityName = item.AmenityName,
                            AmenityType = item.AmenityType,
                            Description = item.Description,
                            CreateDate = Convert.ToString(item.Create_Date),
                            CreateUser = item.Create_User,
                            EditDate = Convert.ToString(item.Edit_Date),
                            Edituser = item.Edit_user
                        });
                    }
                    _obj.Mode = "offline";
                    _obj.BatchId = Convert.ToString(batchNo);
                    _obj.Transaction = "1";

                }

                return _obj;
            }
            catch (Exception ex)
            {
                throw;
            }


        }
        #endregion


        #region ***  MasterAccommodationRoomInformation Done ***
        public string ML_DataTransferMasterAccommodationRoomInformation(Guid LogId)
        {
            DataContracts.ML.DC_ML_DL_MasterAccoRoomInfo _obj = new DataContracts.ML.DC_ML_DL_MasterAccoRoomInfo();
            int TotalCount = 0;
            int MLDataInsertedCount = 0;
            try
            {
                UpdateDistLogInfo(LogId, PushStatus.RUNNNING);
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    //int total = 0;
                    //Get Batch Size
                    int BatchSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DataTransferBatchSize"]);
                    //Get Total Count
                    string strTotalCount = @"SELECT COUNT(1) FROM Accommodation_RoomInfo with(nolock)";
                    context.Configuration.AutoDetectChangesEnabled = false;
                    try { TotalCount = context.Database.SqlQuery<int>(strTotalCount.ToString()).FirstOrDefault(); } catch (Exception ex) { }
                    int NoOfBatch = TotalCount / BatchSize;
                    int mod = TotalCount % BatchSize;
                    if (mod > 0)
                        NoOfBatch = NoOfBatch + 1;
                    for (int BatchNo = 0; BatchNo < NoOfBatch; BatchNo++)
                    {
                        _obj = GetMasterAccoRoomInformationDataForMLTrans(BatchSize, BatchNo);
                        #region To update CounterIn DistributionLog
                        MLDataInsertedCount = MLDataInsertedCount + _obj.MasterAccommodationRoomInformation.Count();
                        UpdateDistLogInfo(LogId, PushStatus.RUNNNING, TotalCount, MLDataInsertedCount);
                        #endregion
                        object result = null;
                        DHSVCProxy.PostDataNewtonsoft(ProxyFor.MachingLearningDataTransfer, System.Configuration.ConfigurationManager.AppSettings["MLSVCURL_DataApi_Post_MasterAccommodationRoomInformation"], _obj, typeof(DataContracts.ML.DC_ML_DL_MasterAccoRoomInfo), typeof(DC_ML_Message), out result);
                    }
                    UpdateDistLogInfo(LogId, PushStatus.COMPLETED, TotalCount, MLDataInsertedCount);
                }


            }
            catch (Exception ex)
            {
                UpdateDistLogInfo(LogId, PushStatus.ERROR, TotalCount, MLDataInsertedCount);
                throw;
            }
            return string.Empty;
        }
        private DC_ML_DL_MasterAccoRoomInfo GetMasterAccoRoomInformationDataForMLTrans(int batchSize, int batchNo)
        {
            DataContracts.ML.DC_ML_DL_MasterAccoRoomInfo _obj = new DataContracts.ML.DC_ML_DL_MasterAccoRoomInfo();
            List<DataContracts.ML.DC_ML_DL_MasterAccoRoomInfo_Data> _objData = new List<DataContracts.ML.DC_ML_DL_MasterAccoRoomInfo_Data>();
            List<DataContracts.DC_ML_MasterAccoRoomInfo_Data> _objAcoo = new List<DataContracts.DC_ML_MasterAccoRoomInfo_Data>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    context.Database.CommandTimeout = 0;
                    context.Configuration.AutoDetectChangesEnabled = false;

                    StringBuilder sbSelect = new StringBuilder();
                    StringBuilder sbOrderby = new StringBuilder();
                    sbSelect.Append(@"SELECT  
                                       Accommodation_RoomInfo_Id,
                                        Accommodation_Id,
                                        Legacy_Htl_Id,
                                        RoomId,
                                        RoomView,
                                        NoOfRooms AS NoOfRooms,
                                        RoomName,
                                        NoOfInterconnectingRooms AS NoOfInterconnectingRooms,
                                        Description,
                                        RoomSize,
                                        RoomDecor,
                                        Smoking,
                                        FloorName,
                                        FloorNumber,
                                        MysteryRoom,
                                        BathRoomType,
                                        BedType,
                                        CompanyRoomCategory,
                                        RoomCategory,
                                        Category,
                                        Create_User,
                                        Create_Date,
                                        Edit_User,
                                        Edit_Date
                                        FROM Accommodation_RoomInfo with(nolock)  ");
                    int skip = batchNo * batchSize;
                    sbOrderby.Append("  ORDER BY CAST(REPLACE(TLGXAccoRoomId,'ACCOROOM','') AS INT) OFFSET " + (skip).ToString() + " ROWS FETCH NEXT " + batchSize.ToString() + " ROWS ONLY ");

                    StringBuilder sbfinal = new StringBuilder();
                    sbfinal.Append(sbSelect);
                    sbfinal.Append(sbOrderby);

                    try { _objAcoo = context.Database.SqlQuery<DataContracts.DC_ML_MasterAccoRoomInfo_Data>(sbfinal.ToString()).ToList(); } catch (Exception ex) { }

                    _obj.MasterAccommodationRoomInformation = new List<DataContracts.ML.DC_ML_DL_MasterAccoRoomInfo_Data>();
                    foreach (var item in _objAcoo)
                    {
                        //_obj.MasterAccommodationRecord.Add(item);
                        _obj.MasterAccommodationRoomInformation.Add(new DataContracts.ML.DC_ML_DL_MasterAccoRoomInfo_Data
                        {
                            AccommodationRoomInfoId = Convert.ToString(item.Accommodation_RoomInfo_Id),
                            AccommodationId = Convert.ToString(item.Accommodation_Id),
                            TLGXHotelId = Convert.ToString(item.Legacy_Htl_Id),
                            RoomId = item.RoomId,
                            RoomView = item.RoomView,
                            NoOfRooms = item.NoOfRooms,
                            RoomName = item.RoomName,
                            NoOfInterconnectingRooms = item.NoOfInterconnectingRooms,
                            Description = item.Description,
                            RoomSize = item.RoomSize,
                            RoomDecor = item.RoomDecor,
                            Smoking = item.Smoking,
                            FloorName = item.FloorName,
                            FloorNumber = item.FloorNumber,
                            MysteryRoom = item.MysteryRoom,
                            BathRoomType = item.BathRoomType,
                            BedType = item.BedType,
                            CompanyRoomCategory = item.CompanyRoomCategory,
                            RoomCategory = item.RoomCategory,
                            Category = item.Category,
                            CreateDate = Convert.ToString(item.Create_Date),
                            CreateUser = item.Create_User,
                            EditDate = Convert.ToString(item.Edit_Date),
                            EditUser = item.Edit_User,

                        });
                    }
                    _obj.Mode = "offline";
                    _obj.BatchId = Convert.ToString(batchNo);
                    _obj.Transaction = "1";
                }

                return _obj;
            }
            catch (Exception ex)
            {
                throw;
            }


        }
        #endregion


        #region *** RoomTypeMatching ***
        public string ML_DataTransferRoomTypeMatching(Guid LogId)
        {
            DataContracts.ML.DC_ML_DL_RoomTypeMatch _obj = new DataContracts.ML.DC_ML_DL_RoomTypeMatch();
            int TotalCount = 0;
            int MLDataInsertedCount = 0;
            try
            {
                UpdateDistLogInfo(LogId, PushStatus.RUNNNING);

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    //int total = 0;
                    //Get Batch Size
                    int BatchSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DataTransferBatchSize"]);
                    string DataTransferForTrainingDataMappingStatus = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DataTransferForTrainingDataMappingStatus"]);

                    //Get Total Count
                    string strTotalCount = @"SELECT COUNT(1) FROM Accommodation_SupplierRoomTypeMapping SRTM with(nolock)
                                             JOIN Accommodation_RoomInfo ARI WITH (NOLOCK) ON SRTM.Accommodation_RoomInfo_Id = ARI.Accommodation_RoomInfo_Id
                                             where SRTM.MappingStatus IN (" + DataTransferForTrainingDataMappingStatus + ")";
                    context.Configuration.AutoDetectChangesEnabled = false;
                    try { TotalCount = context.Database.SqlQuery<int>(strTotalCount.ToString()).FirstOrDefault(); } catch (Exception ex) { }
                    int NoOfBatch = TotalCount / BatchSize;
                    int mod = TotalCount % BatchSize;
                    if (mod > 0)
                        NoOfBatch = NoOfBatch + 1;
                    for (int BatchNo = 0; BatchNo < NoOfBatch; BatchNo++)
                    {
                        _obj = GetRoomTypeMatchDataForMLTrans(BatchSize, BatchNo);
                        #region To update CounterIn DistributionLog
                        MLDataInsertedCount = MLDataInsertedCount + BatchSize;
                        UpdateDistLogInfo(LogId, PushStatus.RUNNNING, TotalCount, MLDataInsertedCount);
                        #endregion
                        object result = null;
                        DHSVCProxy.PostDataNewtonsoft(ProxyFor.MachingLearningDataTransfer, System.Configuration.ConfigurationManager.AppSettings["MLSVCURL_DataApi_Post_RoomTypeMatching"], _obj, typeof(DataContracts.ML.DC_ML_DL_RoomTypeMatch), typeof(DC_ML_Message), out result);
                    }
                    UpdateDistLogInfo(LogId, PushStatus.COMPLETED, TotalCount, MLDataInsertedCount);
                }


            }
            catch (Exception ex)
            {
                UpdateDistLogInfo(LogId, PushStatus.ERROR, TotalCount, MLDataInsertedCount);
                throw;
            }
            return string.Empty;
        }

        private DC_ML_DL_RoomTypeMatch GetRoomTypeMatchDataForMLTrans(int batchSize, int batchNo)
        {
            DataContracts.ML.DC_ML_DL_RoomTypeMatch _obj = new DataContracts.ML.DC_ML_DL_RoomTypeMatch();
            List<DataContracts.ML.DC_ML_DL_RoomTypeMatch_Data> _objData = new List<DataContracts.ML.DC_ML_DL_RoomTypeMatch_Data>();
            List<DataContracts.DC_ML_RoomTypeMatch_Data> _objAcooRoomMatching = new List<DataContracts.DC_ML_RoomTypeMatch_Data>();

            string DataTransferForTrainingDataMappingStatus = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DataTransferForTrainingDataMappingStatus"]);
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    StringBuilder sbSelect = new StringBuilder();
                    StringBuilder sbOrderby = new StringBuilder();
                    sbSelect.Append(@"SELECT  
                                        SRTM.Accommodation_SupplierRoomTypeMapping_Id AS AccommodationSupplierRoomTypeMappingId,
                                        SRTM.Accommodation_Id AS AccommodationId,
                                        SRTM.Supplier_Id AS SupplierId, 
                                        SRTM.SupplierName AS SupplierName, 
                                        SRTM.SupplierRoomId AS SupplierRoomId, 
                                        SRTM.SupplierRoomTypeCode AS SupplierRoomTypeCode, 
                                        SRTM.SupplierRoomName AS SupplierRoomName, 
                                        SRTM.TX_RoomName AS TXRoomName, 
                                        SRTM.SupplierRoomCategory AS SupplierRoomCategory, 
                                        SRTM.SupplierRoomCategoryId AS SupplierRoomCategoryId, 
                                        SRTM.Create_Date AS SupplierRoomCreateDate, 
                                        SRTM.Create_User AS SupplierRoomCreateUser, 
                                        SRTM.Edit_Date AS SupplierRoomEditDate, 
                                        SRTM.Edit_User AS SupplierRoomEditUser, 
                                        SRTM.MaxAdults AS SupplierRoomMaxAdults, 
                                        SRTM.MaxChild AS SupplierRoomMaxChild, 
                                        SRTM.MaxInfants AS  SupplierRoomMaxInfants, 
                                        SRTM.MaxGuestOccupancy  AS  MaxGuestOccupancy, 
                                        SRTM.Quantity  AS  SupplierRoomQuantity, 
                                        SRTM.RatePlan  AS  SupplierRoomRatePlan, 
                                        SRTM.RatePlanCode AS  RatePlanCode, 
                                        SRTM.SupplierProductName AS  SupplierRoomSupplierProductName, 
                                        SRTM.SupplierProductId AS  SupplierRoomSupplierProductId, 
                                        SRTM.Tx_StrippedName AS  TxStrippedName, 
                                        SRTM.Tx_ReorderedName AS  TxReorderedName, 
                                        SRTM.MappingStatus AS  SupplierRoomMappingStatus, 
                                        SRTM.MapId AS  MapId, 
                                        SRTM.Accommodation_RoomInfo_Id AS  AccommodationRoomInfoId, 
                                        SRTM.RoomDescription AS  SupplierRoomRoomDescription, 
                                        SRTM.RoomSize AS  SupplierRoomRoomSize,
                                        ARI.Legacy_Htl_Id AS TLGXCommonHotelId,
                                        ARI.RoomId AS AccoRoomId,
                                        ARI.RoomView AS AccoRoomView,
                                        ARI.NoOfRooms AS AccoNoOfRooms,
                                        ARI.RoomName AS AccoRoomName,
                                        ARI.NoOfInterconnectingRooms AS AccoNoOfInterconnectingRooms,
                                        ARI.Description AS AccoDescription,
                                        ARI.RoomSize AS AccoRoomSize,
                                        ARI.RoomDecor AS AccoRoomDecor,
                                         ISNULL(ARI.Smoking,0) AS AccoSmoking,
                                        ARI.FloorName AS AccoFloorName,
                                        ARI.FloorNumber As AccoFloorNumber,
                                        ISNULL(ARI.MysteryRoom,0) AS AccoMysteryRoom,
                                        ARI.BathRoomType AS AccoBathRoomType,
                                        ARI.BedType AS AccoBedType,
                                        ARI.CompanyRoomCategory AS AccoCompanyRoomCategory,
                                        ARI.RoomCategory AS AccoRoomCategory,
                                        ARI.Create_User AS AccoCreateUser,ARI.Create_Date AS AccoCreateUser,
                                        ARI.Edit_User as AccoEditUser,ARI.Edit_Date as AccoEditDate,SRTM.MatchingScore,
                                        '' AS SimilarityIndicator         
                                        FROM Accommodation_SupplierRoomTypeMapping SRTM WITH (NOLOCK) 
                                        JOIN Accommodation_RoomInfo ARI WITH (NOLOCK) ON SRTM.Accommodation_RoomInfo_Id = ARI.Accommodation_RoomInfo_Id ");
                    sbSelect.Append(" where SRTM.MappingStatus IN (");
                    sbSelect.Append(DataTransferForTrainingDataMappingStatus);
                    sbSelect.Append(" )  ");

                    int skip = batchNo * batchSize;
                    sbOrderby.Append("  ORDER BY SRTM.Accommodation_SupplierRoomTypeMapping_Id OFFSET " + (skip).ToString() + " ROWS FETCH NEXT " + batchSize.ToString() + " ROWS ONLY ");

                    StringBuilder sbfinal = new StringBuilder();
                    sbfinal.Append(sbSelect);
                    sbfinal.Append(sbOrderby);

                    context.Configuration.AutoDetectChangesEnabled = false;
                    try { _objAcooRoomMatching = context.Database.SqlQuery<DataContracts.DC_ML_RoomTypeMatch_Data>(sbfinal.ToString()).ToList(); } catch (Exception ex) { }

                    _obj.RoomTypeMatching = new List<DataContracts.ML.DC_ML_DL_RoomTypeMatch_Data>();
                    foreach (var item in _objAcooRoomMatching)
                    {
                        _obj.RoomTypeMatching.Add(new DataContracts.ML.DC_ML_DL_RoomTypeMatch_Data
                        {

                            AccommodationSupplierRoomTypeMappingId = Convert.ToString(item.AccommodationSupplierRoomTypeMappingId),
                            AccommodationId = Convert.ToString(item.AccommodationId),
                            SupplierId = Convert.ToString(item.SupplierId),
                            SupplierName = item.SupplierName,
                            SupplierRoomId = item.SupplierRoomId,
                            SupplierRoomTypeCode = item.SupplierRoomTypeCode,
                            SupplierRoomName = item.SupplierRoomName,
                            TXRoomName = item.TXRoomName,
                            SupplierRoomCategory = item.SupplierRoomCategory,
                            SupplierRoomCategoryId = item.SupplierRoomCategoryId,
                            SupplierRoomCreateDate = Convert.ToString(item.SupplierRoomCreateDate),
                            SupplierRoomCreateUser = item.SupplierRoomCreateUser,
                            SupplierRoomEditDate = Convert.ToString(item.SupplierRoomEditDate),
                            SupplierRoomEditUser = item.SupplierRoomEditUser,
                            SupplierRoomMaxAdults = item.SupplierRoomMaxAdults,
                            SupplierRoomMaxChild = item.SupplierRoomMaxChild,
                            SupplierRoomMaxInfants = item.SupplierRoomMaxInfants,
                            MaxGuestOccupancy = item.MaxGuestOccupancy,
                            SupplierRoomQuantity = item.SupplierRoomQuantity,
                            SupplierRoomRatePlan = item.SupplierRoomRatePlan,
                            RatePlanCode = item.RatePlanCode,
                            SupplierRoomSupplierProductName = item.SupplierRoomSupplierProductName,
                            SupplierRoomSupplierProductId = item.SupplierRoomSupplierProductId,
                            TxStrippedName = item.TxStrippedName,
                            TxReorderedName = item.TxReorderedName,
                            SupplierRoomMappingStatus = item.SupplierRoomMappingStatus,
                            MapId = item.MapId,
                            AccommodationRoomInfoId = Convert.ToString(item.AccommodationRoomInfoId),
                            SupplierRoomRoomDescription = item.SupplierRoomRoomDescription,
                            SupplierRoomRoomSize = item.SupplierRoomRoomSize,
                            TLGXCommonHotelId = item.TLGXCommonHotelId,
                            AccoRoomId = item.AccoRoomId,
                            AccoRoomView = item.AccoRoomView,
                            AccoNoOfRooms = item.AccoNoOfRooms,
                            AccoRoomName = item.AccoRoomName,
                            AccoNoOfInterconnectingRooms = item.AccoNoOfInterconnectingRooms,
                            AccoDescription = item.AccoDescription,
                            AccoRoomSize = item.AccoRoomSize,
                            AccoRoomDecor = item.AccoRoomDecor,
                            AccoSmoking = item.AccoSmoking,
                            AccoFloorName = item.AccoFloorName,
                            AccoFloorNumber = item.AccoFloorNumber,
                            AccoMysteryRoom = item.AccoMysteryRoom,
                            AccoBathRoomType = item.AccoBathRoomType,
                            AccoBedType = item.AccoBedType,
                            AccoCompanyRoomCategory = item.AccoCompanyRoomCategory,
                            AccoRoomCategory = item.AccoRoomCategory,
                            AccoCreateDate = Convert.ToString(item.AccoCreateDate),
                            AccoCreateUser = item.AccoCreateUser,
                            AccoEditDate = Convert.ToString(item.AccoEditDate),
                            AccoEditUser = item.AccoEditUser,
                            SimilarityIndicator = (item.SimilarityIndicator == string.Empty ? Convert.ToBoolean(0) : Convert.ToBoolean(item.SimilarityIndicator)),
                            SimilarityScore = Convert.ToDouble(item.SimilarityScore)
                        });
                    }
                    _obj.Mode = "offline";
                    _obj.BatchId = Convert.ToString(batchNo);
                    _obj.Transaction = "1";

                }

                return _obj;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion


        #region *** SupplierAccommodationData ***
        public string ML_DataTransferSupplierAccommodationData(Guid LogId)
        {
            DataContracts.ML.DC_ML_DL_SupplierAcco _obj = new DataContracts.ML.DC_ML_DL_SupplierAcco();
            int TotalCount = 0;
            int MLDataInsertedCount = 0;
            try
            {
                UpdateDistLogInfo(LogId, PushStatus.RUNNNING);

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    //int total = 0;
                    //Get Batch Size
                    int BatchSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DataTransferBatchSize"]);
                    //Get Total Count
                    string strTotalCount = @"SELECT COUNT(1) FROM Accommodation_ProductMapping with(nolock)";
                    context.Configuration.AutoDetectChangesEnabled = false;
                    try { TotalCount = context.Database.SqlQuery<int>(strTotalCount.ToString()).FirstOrDefault(); } catch (Exception ex) { }
                    int NoOfBatch = TotalCount / BatchSize;
                    int mod = TotalCount % BatchSize;
                    if (mod > 0)
                        NoOfBatch = NoOfBatch + 1;
                    for (int BatchNo = 0; BatchNo < NoOfBatch; BatchNo++)
                    {
                        _obj = GetSupplierAccoDataForMLTrans(BatchSize, BatchNo);
                        #region To update CounterIn DistributionLog
                        MLDataInsertedCount = MLDataInsertedCount + _obj.SupplierAccommodationData.Count();
                        UpdateDistLogInfo(LogId, PushStatus.RUNNNING, TotalCount, MLDataInsertedCount);
                        #endregion
                        object result = null;
                        DHSVCProxy.PostDataNewtonsoft(ProxyFor.MachingLearningDataTransfer, System.Configuration.ConfigurationManager.AppSettings["MLSVCURL_DataApi_Post_SupplierAccommodationData"], _obj, typeof(DataContracts.ML.DC_ML_DL_SupplierAcco), typeof(DC_ML_Message), out result);
                    }
                    UpdateDistLogInfo(LogId, PushStatus.COMPLETED, TotalCount, MLDataInsertedCount);
                }


            }
            catch (Exception ex)
            {
                UpdateDistLogInfo(LogId, PushStatus.ERROR, TotalCount, MLDataInsertedCount);
                throw;
            }
            return string.Empty;
        }

        private DC_ML_DL_SupplierAcco GetSupplierAccoDataForMLTrans(int batchSize, int batchNo)
        {
            DataContracts.ML.DC_ML_DL_SupplierAcco _obj = new DataContracts.ML.DC_ML_DL_SupplierAcco();
            List<DataContracts.ML.DC_ML_DL_SupplierAcco_Data> _objData = new List<DataContracts.ML.DC_ML_DL_SupplierAcco_Data>();
            List<DataContracts.DC_ML_SupplierAcco_Data> _objSuppAcoo = new List<DataContracts.DC_ML_SupplierAcco_Data>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    StringBuilder sbSelect = new StringBuilder();
                    StringBuilder sbOrderby = new StringBuilder();
                    sbSelect.Append(@"  SELECT 
                                        APM.Accommodation_ProductMapping_Id,
                                        APM.Accommodation_Id,
                                        APM.SupplierProductReference,
                                        APM.SupplierName,
                                        APM.Supplier_Id AS SupplierId,
                                        Acc.CompanyHotelID AS TLGXHotelId,
                                        APM.ProductName AS AccommodationName,
                                        APM.CountryName AS Country,
                                        APM.CityName AS City,
                                        APM.Address ,
                                        APM.PostCode AS PostalCode,
                                        APM.Create_Date AS CreateDate,
                                        APM.Create_User AS CreateUser,
                                        APM.Edit_Date AS EditDate,
                                        APM.Edit_User AS Edituser from Accommodation_ProductMapping APM WITH(NOLOCK)
                                        LEFT JOIN Accommodation Acc WITH (NOLOCK) ON APM.Accommodation_Id = Acc.Accommodation_Id ");
                    int skip = batchNo * batchSize;
                    sbOrderby.Append("  ORDER BY APM.Accommodation_ProductMapping_Id OFFSET " + (skip).ToString() + " ROWS FETCH NEXT " + batchSize.ToString() + " ROWS ONLY ");

                    StringBuilder sbfinal = new StringBuilder();
                    sbfinal.Append(sbSelect);
                    sbfinal.Append(sbOrderby);

                    context.Configuration.AutoDetectChangesEnabled = false;
                    try { _objSuppAcoo = context.Database.SqlQuery<DataContracts.DC_ML_SupplierAcco_Data>(sbfinal.ToString()).ToList(); } catch (Exception ex) { }

                    _obj.SupplierAccommodationData = new List<DataContracts.ML.DC_ML_DL_SupplierAcco_Data>();
                    foreach (var item in _objSuppAcoo)
                    {
                        _obj.SupplierAccommodationData.Add(new DataContracts.ML.DC_ML_DL_SupplierAcco_Data
                        {
                            Accommodation_ProductMapping_Id = Convert.ToString(item.Accommodation_ProductMapping_Id),
                            AccommodationId = Convert.ToString(item.Accommodation_Id),
                            AccommodationName = item.AccommodationName,
                            Address = item.Address,
                            City = item.City,
                            Country = item.Country,
                            CreateDate = Convert.ToString(item.CreateDate),
                            CreateUser = item.CreateUser,
                            EditDate = Convert.ToString(item.EditDate),
                            Edituser = item.Edituser,
                            PostalCode = item.PostalCode,
                            SupplierId = Convert.ToString(item.SupplierId),
                            SupplierName = item.SupplierName,
                            SupplierProductReference = item.SupplierProductReference,
                            TLGXHotelId = item.TLGXHotelId
                        });
                    }
                    _obj.Mode = "offline";
                    _obj.BatchId = Convert.ToString(batchNo);
                    _obj.Transaction = "1";

                }

                return _obj;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion


        #region *** SupplierAccommodationRoomData Done***
        public string ML_DataTransferSupplierAccommodationRoomData(Guid LogId)
        {
            DataContracts.ML.DC_ML_DL_SupplierAcco_Room _obj = new DataContracts.ML.DC_ML_DL_SupplierAcco_Room();
            int TotalCount = 0;
            int MLDataInsertedCount = 0;
            try
            {
                UpdateDistLogInfo(LogId, PushStatus.RUNNNING);
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    //int total = 0;
                    //Get Batch Size
                    int BatchSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DataTransferBatchSize"]);
                    //Get Total Count
                    string strTotalCount = @"SELECT COUNT(1) FROM Accommodation_SupplierRoomTypeMapping with(nolock)";
                    context.Configuration.AutoDetectChangesEnabled = false;
                    try { TotalCount = context.Database.SqlQuery<int>(strTotalCount.ToString()).FirstOrDefault(); } catch (Exception ex) { }
                    int NoOfBatch = TotalCount / BatchSize;
                    int mod = TotalCount % BatchSize;
                    if (mod > 0)
                        NoOfBatch = NoOfBatch + 1;
                    for (int BatchNo = 0; BatchNo < NoOfBatch; BatchNo++)
                    {
                        _obj = GetSupplierAcco_RoomDataForMLTrans(BatchSize, BatchNo);
                        object result = null;
                        #region To update CounterIn DistributionLog
                        MLDataInsertedCount = MLDataInsertedCount + BatchSize;
                        UpdateDistLogInfo(LogId, PushStatus.RUNNNING, TotalCount, MLDataInsertedCount);
                        #endregion
                        DHSVCProxy.PostDataNewtonsoft(ProxyFor.MachingLearningDataTransfer, System.Configuration.ConfigurationManager.AppSettings["MLSVCURL_DataApi_Post_SupplierAccommodationRoomData"], _obj, typeof(DataContracts.ML.DC_ML_DL_SupplierAcco_Room), typeof(DC_ML_Message), out result);
                    }
                    UpdateDistLogInfo(LogId, PushStatus.COMPLETED, TotalCount, MLDataInsertedCount);
                }
            }
            catch (Exception ex)
            {
                UpdateDistLogInfo(LogId, PushStatus.ERROR, TotalCount, MLDataInsertedCount);
                throw;
            }
            return string.Empty;
        }

        private DC_ML_DL_SupplierAcco_Room GetSupplierAcco_RoomDataForMLTrans(int batchSize, int batchNo)
        {
            DataContracts.ML.DC_ML_DL_SupplierAcco_Room _obj = new DataContracts.ML.DC_ML_DL_SupplierAcco_Room();
            List<DataContracts.ML.DC_ML_DL_SupplierAcco_Room_Data> _objData = new List<DataContracts.ML.DC_ML_DL_SupplierAcco_Room_Data>();
            List<DataContracts.DC_ML_SupplierAcco_Room_Data> _objDataSystem = new List<DataContracts.DC_ML_SupplierAcco_Room_Data>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    StringBuilder sbSelect = new StringBuilder();
                    StringBuilder sbOrderby = new StringBuilder();
                    sbSelect.Append(@"SELECT  
                                       Accommodation_SupplierRoomTypeMapping_Id,Accommodation_Id,Supplier_Id
                                      ,SupplierName,SupplierRoomId,SupplierRoomTypeCode
                                      ,SupplierRoomName,TX_RoomName,SupplierRoomCategory,SupplierRoomCategoryId
                                      ,Create_Date,Create_User,Edit_Date,Edit_User,MaxAdults
                                      ,MaxChild,MaxInfants,MaxGuestOccupancy,Quantity,RatePlan
                                      ,RatePlanCode,SupplierProductName,SupplierProductId,Tx_StrippedName,Tx_ReorderedName
                                      ,MappingStatus,MapId,Accommodation_RoomInfo_Id,RoomSize,BathRoomType
                                      ,RoomViewCode,FloorName,FloorNumber,Amenities,RoomLocationCode
                                      ,ChildAge,ExtraBed,Bedrooms,Smoking,BedTypeCode,MinGuestOccupancy,PromotionalVendorCode,BeddingConfig
                                      FROM Accommodation_SupplierRoomTypeMapping with(nolock)  ");
                    int skip = batchNo * batchSize;
                    sbOrderby.Append("  ORDER BY Accommodation_SupplierRoomTypeMapping_Id OFFSET " + (skip).ToString() + " ROWS FETCH NEXT " + batchSize.ToString() + " ROWS ONLY ");

                    StringBuilder sbfinal = new StringBuilder();
                    sbfinal.Append(sbSelect);
                    sbfinal.Append(sbOrderby);

                    context.Configuration.AutoDetectChangesEnabled = false;
                    try { _objDataSystem = context.Database.SqlQuery<DataContracts.DC_ML_SupplierAcco_Room_Data>(sbfinal.ToString()).ToList(); } catch (Exception ex) { }

                    _obj.SupplierAccommodationRoomData = new List<DataContracts.ML.DC_ML_DL_SupplierAcco_Room_Data>();
                    foreach (var item in _objDataSystem)
                    {
                        //_obj.MasterAccommodationRecord.Add(item);
                        _obj.SupplierAccommodationRoomData.Add(new DataContracts.ML.DC_ML_DL_SupplierAcco_Room_Data
                        {

                            AccommodationSupplierRoomTypeMappingId = Convert.ToString(item.Accommodation_SupplierRoomTypeMapping_Id),
                            AccommodationId = Convert.ToString(item.Accommodation_Id),
                            SupplierId = Convert.ToString(item.Supplier_Id),
                            SupplierName = item.SupplierName,
                            SupplierRoomId = item.SupplierRoomId,
                            SupplierRoomTypeCode = item.SupplierRoomTypeCode,
                            SupplierRoomName = item.SupplierRoomName,
                            TXRoomName = item.TX_RoomName,
                            SupplierRoomCategory = item.SupplierRoomCategory,
                            SupplierRoomCategoryId = item.SupplierRoomCategoryId,
                            CreateDate = Convert.ToString(item.Create_Date),
                            CreateUser = item.Create_User,
                            EditDate = Convert.ToString(item.Edit_Date),
                            EditUser = item.Edit_User,
                            MaxAdults = item.MaxAdults,
                            MaxChild = item.MaxChild,
                            MaxInfants = item.MaxInfants,
                            MaxGuestOccupancy = item.MaxGuestOccupancy,
                            Quantity = item.Quantity,
                            RatePlan = item.RatePlan,
                            RatePlanCode = item.RatePlanCode,
                            SupplierProductName = item.SupplierProductName,
                            SupplierProductId = item.SupplierProductId,
                            TxStrippedName = item.Tx_StrippedName,
                            TxReorderedName = item.Tx_ReorderedName,
                            MappingStatus = item.MappingStatus,
                            MapId = item.MapId,
                            AccommodationRoomInfoId = Convert.ToString(item.Accommodation_RoomInfo_Id),
                            RoomSize = item.RoomSize,
                            BathRoomType = item.BathRoomType,
                            RoomViewCode = item.RoomViewCode,
                            FloorName = item.FloorName,
                            FloorNumber = item.FloorNumber,
                            Amenities = item.Amenities,
                            RoomLocationCode = item.RoomLocationCode,
                            ChildAge = item.ChildAge,
                            ExtraBed = item.ExtraBed,
                            Bedrooms = item.Bedrooms,
                            Smoking = item.Smoking,
                            BedTypeCode = item.BedTypeCode,
                            MinGuestOccupancy = item.MinGuestOccupancy,
                            PromotionalVendorCode = item.PromotionalVendorCode,
                            BeddingConfig = item.BeddingConfig,
                        });
                    }
                    _obj.Mode = "offline";
                    _obj.BatchId = Convert.ToString(batchNo);
                    _obj.Transaction = "1";

                }

                return _obj;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion


        #region *** SupplierAccommodationRoomExtendedAttributes  Done***
        public string ML_DataTransferSupplierAccommodationRoomExtendedAttributes(Guid LogId)
        {
            DataContracts.ML.DC_ML_DL_SupplierAcco_RoomExtendedAttributes _obj = new DataContracts.ML.DC_ML_DL_SupplierAcco_RoomExtendedAttributes();
            int TotalCount = 0;
            int MLDataInsertedCount = 0;
            try
            {
                UpdateDistLogInfo(LogId, PushStatus.RUNNNING);

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    //  int total = 0;
                    //Get Batch Size
                    int BatchSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DataTransferBatchSize"]);
                    //Get Total Count
                    string strTotalCount = @"SELECT COUNT(1) FROM Accommodation_SupplierRoomTypeAttributes with(nolock)";
                    context.Configuration.AutoDetectChangesEnabled = false;
                    try { TotalCount = context.Database.SqlQuery<int>(strTotalCount.ToString()).FirstOrDefault(); } catch (Exception ex) { }
                    int NoOfBatch = TotalCount / BatchSize;
                    int mod = TotalCount % BatchSize;
                    if (mod > 0)
                        NoOfBatch = NoOfBatch + 1;
                    for (int BatchNo = 0; BatchNo < NoOfBatch; BatchNo++)
                    {
                        _obj = GetSupplierAcco_RoomExtendedAttributesDataForMLTrans(BatchSize, BatchNo);
                        #region To update CounterIn DistributionLog
                        MLDataInsertedCount = MLDataInsertedCount + BatchSize;
                        UpdateDistLogInfo(LogId, PushStatus.RUNNNING, TotalCount, MLDataInsertedCount);
                        #endregion
                        object result = null;
                        DHSVCProxy.PostDataNewtonsoft(ProxyFor.MachingLearningDataTransfer, System.Configuration.ConfigurationManager.AppSettings["MLSVCURL_DataApi_Post_SupplierAccommodationRoomExtendedAttributes"], _obj, typeof(DataContracts.ML.DC_ML_DL_SupplierAcco_RoomExtendedAttributes), typeof(DC_ML_Message), out result);
                    }
                    UpdateDistLogInfo(LogId, PushStatus.COMPLETED, TotalCount, MLDataInsertedCount);
                }
            }
            catch (Exception ex)
            {
                UpdateDistLogInfo(LogId, PushStatus.ERROR, TotalCount, MLDataInsertedCount);
                throw;
            }
            return string.Empty;
        }

        private DC_ML_DL_SupplierAcco_RoomExtendedAttributes GetSupplierAcco_RoomExtendedAttributesDataForMLTrans(int batchSize, int batchNo)
        {
            DataContracts.ML.DC_ML_DL_SupplierAcco_RoomExtendedAttributes _obj = new DataContracts.ML.DC_ML_DL_SupplierAcco_RoomExtendedAttributes();
            List<DataContracts.ML.DC_ML_DL_SupplierAcco_RoomExtendedAttributes_Data> _objData = new List<DataContracts.ML.DC_ML_DL_SupplierAcco_RoomExtendedAttributes_Data>();
            List<DataContracts.DC_ML_SupplierAcco_RoomExtendedAttributes_Data> _objDataSystem = new List<DataContracts.DC_ML_SupplierAcco_RoomExtendedAttributes_Data>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    StringBuilder sbSelect = new StringBuilder();
                    StringBuilder sbOrderby = new StringBuilder();
                    sbSelect.Append(@"SELECT  
                                        RoomTypeMapAttribute_Id,
                                        RoomTypeMap_Id,
                                        SupplierRoomTypeAttribute,
                                        SystemAttributeKeyword
                                        FROM Accommodation_SupplierRoomTypeAttributes with(nolock)  ");
                    int skip = batchNo * batchSize;
                    sbOrderby.Append("  ORDER BY RoomTypeMapAttribute_Id OFFSET " + (skip).ToString() + " ROWS FETCH NEXT " + batchSize.ToString() + " ROWS ONLY ");

                    StringBuilder sbfinal = new StringBuilder();
                    sbfinal.Append(sbSelect);
                    sbfinal.Append(sbOrderby);

                    context.Configuration.AutoDetectChangesEnabled = false;
                    try { _objDataSystem = context.Database.SqlQuery<DataContracts.DC_ML_SupplierAcco_RoomExtendedAttributes_Data>(sbfinal.ToString()).ToList(); } catch (Exception ex) { }

                    _obj.SupplierAccommodationRoomExtendedAttributes = new List<DataContracts.ML.DC_ML_DL_SupplierAcco_RoomExtendedAttributes_Data>();
                    foreach (var item in _objDataSystem)
                    {
                        //_obj.MasterAccommodationRecord.Add(item);
                        _obj.SupplierAccommodationRoomExtendedAttributes.Add(new DataContracts.ML.DC_ML_DL_SupplierAcco_RoomExtendedAttributes_Data
                        {
                            RoomTypeMapAttributeId = Convert.ToString(item.RoomTypeMapAttribute_Id),
                            RoomTypeMapId = Convert.ToString(item.RoomTypeMap_Id),
                            SupplierRoomTypeAttribute = Convert.ToString(item.SupplierRoomTypeAttribute),
                            SystemAttributeKeyword = Convert.ToString(item.SystemAttributeKeyword)
                        });
                    }
                    _obj.Mode = "offline";
                    _obj.BatchId = Convert.ToString(batchNo);
                    _obj.Transaction = "1";

                }

                return _obj;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion


        private void UpdateDistLogInfo(Guid LogId, PushStatus status, int totalCount = 0, int insertedCount = 0)
        {
            IncomingWebRequestContext woc = WebOperationContext.Current.IncomingRequest;
            string CallingAgent = woc.Headers["CallingAgent"];
            string CallingUser = woc.Headers["CallingUser"];
            string Status = string.Empty;
            string EditUser = CallingUser;

            if (status == PushStatus.RUNNNING)
            {
                Status = "Running";
            }
            else if (status == PushStatus.COMPLETED)
            {
                Status = "Completed";
            }
            else if (status == PushStatus.ERROR)
            {
                Status = "Error";
            }

            StringBuilder setNewStatus = new StringBuilder();
            setNewStatus.Append("UPDATE DistributionLayerRefresh_Log SET TotalCount = " + totalCount.ToString() + " , MongoPushCount = " + insertedCount.ToString() + ", Status ='" + Status + "',  Edit_Date = getDate(),  Edit_User='" + EditUser + "' WHERE Id= '" + LogId + "';");
            using (ConsumerEntities context = new ConsumerEntities())
            {

                context.Configuration.AutoDetectChangesEnabled = false;
                var Log = context.DistributionLayerRefresh_Log.Find(LogId);
                if (Log != null)
                    context.Database.ExecuteSqlCommand(setNewStatus.ToString());
            }
            setNewStatus = null;
        }
    }



    public class DC_ML_Message
    {
        public string Mode { get; set; }
        public string BatchId { get; set; }
        public string Transaction { get; set; }
        public string Status { get; set; }

    }

}
