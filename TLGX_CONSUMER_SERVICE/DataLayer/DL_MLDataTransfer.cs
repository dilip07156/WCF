using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using DataContracts.ML;

namespace DataLayer
{
    public class DL_MLDataTransfer : IDisposable
    {

        public void Dispose()
        {
        }

        #region *** MasterAccommodationRecord Done***
        public string ML_DataTransferMasterAccommodation()
        {
            DataContracts.ML.DC_ML_DL_MasterAccoRecord _obj = new DataContracts.ML.DC_ML_DL_MasterAccoRecord();
            try
            {

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    int total = 0;
                    //Get Batch Size
                    int BatchSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DataTransferBatchSize"]);
                    //Get Total Count
                    string strTotalCount = @"SELECT COUNT(1) FROM Accommodation with(nolock)";
                    context.Configuration.AutoDetectChangesEnabled = false;
                    try { total = context.Database.SqlQuery<int>(strTotalCount.ToString()).FirstOrDefault(); } catch (Exception ex) { }
                    int NoOfBatch = total / BatchSize;
                    int mod = total % BatchSize;
                    if (mod > 0)
                        NoOfBatch = NoOfBatch + 1;
                    for (int BatchNo = 0; BatchNo < NoOfBatch; BatchNo++)
                    {
                        _obj = GetMasterAccoDataForMLTrans(BatchSize, BatchNo);
                        object result = null;
                        DHSVCProxy.PostData(ProxyFor.MachingLearningDataTransfer, System.Configuration.ConfigurationManager.AppSettings["MLSVCURL_DataApi_Post_MasterAccommodationRecord"], _obj, typeof(DataContracts.ML.DC_ML_DL_MasterAccoRecord), typeof(DC_ML_Message), out result);
                    }

                }


            }
            catch (Exception ex)
            {

                throw;
            }
            return string.Empty;
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
                            AccommodationId = Convert.ToString(item.AccommodationId) ?? string.Empty,
                            AccommodationName = item.AccommodationName ?? string.Empty,
                            City = item.City ?? string.Empty,
                            Country = item.Country ?? string.Empty,
                            Address = item.Address ?? string.Empty,
                            CreateDate = Convert.ToString(item.CreateDate) ?? string.Empty,
                            CreateUser = item.CreateUser ?? string.Empty,
                            EditDate = Convert.ToString(item.EditDate) ?? string.Empty,
                            Edituser = item.Edituser ?? string.Empty,
                            PostalCode = item.PostalCode ?? string.Empty,
                            TLGXHotelId = item.TLGXHotelId ?? 0
                        });
                    }
                    _obj.Mode = "offline";
                    _obj.BatchId = Convert.ToString(batchNo);
                    _obj.Transaction = "1";

                }

                return _obj;
            }
            catch (Exception)
            {

                throw;
            }


        }
        #endregion



        #region *** MasterAccommodationRoomFacilities ***
        public string ML_DataTransferMasterAccommodationRoomFacilities()
        {
            DataContracts.ML.DC_ML_DL_MasterAccoRoomFacility _obj = new DataContracts.ML.DC_ML_DL_MasterAccoRoomFacility();
            try
            {

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
                        object result = null;
                        DHSVCProxy.PostData(ProxyFor.MachingLearningDataTransfer, System.Configuration.ConfigurationManager.AppSettings["MLSVCURL_DataApi_Post_MasterAccommodationRoomFacilities"], _obj, typeof(DataContracts.ML.DC_ML_DL_MasterAccoRoomFacility), typeof(DC_ML_Message), out result);
                    }

                }


            }
            catch (Exception ex)
            {

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
                            AccommodationRoomFacilityId = Convert.ToString(item.Accommodation_RoomFacility_Id) ?? string.Empty,
                            AccommodationRoomInfoId = Convert.ToString(item.Accommodation_RoomInfo_Id) ?? string.Empty,
                            AccommodationId = Convert.ToString(item.Accommodation_Id) ?? string.Empty,
                            AmenityName = item.AmenityName ?? string.Empty,
                            AmenityType = item.AmenityType ?? string.Empty,
                            Description = item.Description ?? string.Empty,
                            CreateDate = Convert.ToString(item.Create_Date) ?? string.Empty,
                            CreateUser = item.Create_User ?? string.Empty,
                            EditDate = Convert.ToString(item.Edit_Date) ?? string.Empty,
                            Edituser = item.Edit_user ?? string.Empty
                        });
                    }
                    _obj.Mode = "offline";
                    _obj.BatchId = Convert.ToString(batchNo);
                    _obj.Transaction = "1";

                }

                return _obj;
            }
            catch (Exception)
            {

                throw;
            }


        }
        #endregion


        #region ***  MasterAccommodationRoomInformation Done ***
        public string ML_DataTransferMasterAccommodationRoomInformation()
        {
            DataContracts.ML.DC_ML_DL_MasterAccoRoomInfo _obj = new DataContracts.ML.DC_ML_DL_MasterAccoRoomInfo();
            try
            {

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    int total = 0;
                    //Get Batch Size
                    int BatchSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DataTransferBatchSize"]);
                    //Get Total Count
                    string strTotalCount = @"SELECT COUNT(1) FROM Accommodation_RoomInfo with(nolock)";
                    context.Configuration.AutoDetectChangesEnabled = false;
                    try { total = context.Database.SqlQuery<int>(strTotalCount.ToString()).FirstOrDefault(); } catch (Exception ex) { }
                    int NoOfBatch = total / BatchSize;
                    int mod = total % BatchSize;
                    if (mod > 0)
                        NoOfBatch = NoOfBatch + 1;
                    for (int BatchNo = 0; BatchNo < NoOfBatch; BatchNo++)
                    {
                        _obj = GetMasterAccoRoomInformationDataForMLTrans(BatchSize, BatchNo);
                        object result = null;
                        DHSVCProxy.PostData(ProxyFor.MachingLearningDataTransfer, System.Configuration.ConfigurationManager.AppSettings["MLSVCURL_DataApi_Post_MasterAccommodationRoomInformation"], _obj, typeof(DataContracts.ML.DC_ML_DL_MasterAccoRoomInfo), typeof(DC_ML_Message), out result);
                    }

                }


            }
            catch (Exception ex)
            {

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
                    StringBuilder sbSelect = new StringBuilder();
                    StringBuilder sbOrderby = new StringBuilder();
                    sbSelect.Append(@"SELECT  
                                       Accommodation_RoomInfo_Id,
                                        Accommodation_Id,
                                        Legacy_Htl_Id,
                                        RoomId,
                                        RoomView,
                                        ISNULL(NoOfRooms,0) AS NoOfRooms,
                                        RoomName,
                                        ISNULL(NoOfInterconnectingRooms,0) AS NoOfInterconnectingRooms,
                                        Description,
                                        RoomSize,
                                        RoomDecor,
                                        ISNULL(Smoking,0) AS Smoking,
                                        FloorName,
                                        FloorNumber,
                                        ISNULL(MysteryRoom,0) AS MysteryRoom,
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
                    sbOrderby.Append("  ORDER BY Accommodation_Id OFFSET " + (skip).ToString() + " ROWS FETCH NEXT " + batchSize.ToString() + " ROWS ONLY ");

                    StringBuilder sbfinal = new StringBuilder();
                    sbfinal.Append(sbSelect);
                    sbfinal.Append(sbOrderby);

                    context.Configuration.AutoDetectChangesEnabled = false;
                    try { _objAcoo = context.Database.SqlQuery<DataContracts.DC_ML_MasterAccoRoomInfo_Data>(sbfinal.ToString()).ToList(); } catch (Exception ex) { }

                    _obj.MasterAccommodationRoomInformation = new List<DataContracts.ML.DC_ML_DL_MasterAccoRoomInfo_Data>();
                    foreach (var item in _objAcoo)
                    {
                        //_obj.MasterAccommodationRecord.Add(item);
                        _obj.MasterAccommodationRoomInformation.Add(new DataContracts.ML.DC_ML_DL_MasterAccoRoomInfo_Data
                        {
                            AccommodationRoomInfoId = Convert.ToString(item.Accommodation_RoomInfo_Id) ?? string.Empty,
                            AccommodationId = Convert.ToString(item.Accommodation_Id) ?? string.Empty,
                            TLGXHotelId = Convert.ToString(item.Legacy_Htl_Id) ?? string.Empty,
                            RoomId = item.RoomId ?? string.Empty,
                            RoomView = item.RoomView ?? string.Empty,
                            NoOfRooms = item.NoOfRooms,
                            RoomName = item.RoomName ?? string.Empty,
                            NoOfInterconnectingRooms = item.NoOfInterconnectingRooms,
                            Description = item.Description ?? string.Empty,
                            RoomSize = item.RoomSize ?? string.Empty,
                            RoomDecor = item.RoomDecor ?? string.Empty,
                            Smoking = item.Smoking,
                            FloorName = item.FloorName ?? string.Empty,
                            FloorNumber = item.FloorNumber ?? string.Empty,
                            MysteryRoom = item.MysteryRoom,
                            BathRoomType = item.BathRoomType ?? string.Empty,
                            BedType = item.BedType ?? string.Empty,
                            CompanyRoomCategory = item.CompanyRoomCategory ?? string.Empty,
                            RoomCategory = item.RoomCategory ?? string.Empty,
                            Category = item.Category ?? string.Empty,
                            CreateDate = Convert.ToString(item.Create_Date) ?? string.Empty,
                            CreateUser = item.Create_User ?? string.Empty,
                            EditDate = Convert.ToString(item.Edit_Date) ?? string.Empty,
                            EditUser = item.Edit_User ?? string.Empty,

                        });
                    }
                    _obj.Mode = "offline";
                    _obj.BatchId = Convert.ToString(batchNo);
                    _obj.Transaction = "1";

                }

                return _obj;
            }
            catch (Exception)
            {

                throw;
            }


        }
        #endregion


        #region *** RoomTypeMatching ***
        public string ML_DataTransferRoomTypeMatching()
        {
            DataContracts.ML.DC_ML_DL_RoomTypeMatch _obj = new DataContracts.ML.DC_ML_DL_RoomTypeMatch();
            try
            {

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    int total = 0;
                    //Get Batch Size
                    int BatchSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DataTransferBatchSize"]);
                    string DataTransferForTrainingDataMappingStatus = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DataTransferForTrainingDataMappingStatus"]);

                    //Get Total Count
                    string strTotalCount = @"SELECT COUNT(1) FROM Accommodation_SupplierRoomTypeMapping SRTM with(nolock)
                                             JOIN Accommodation_RoomInfo ARI WITH (NOLOCK) ON SRTM.Accommodation_RoomInfo_Id = ARI.Accommodation_RoomInfo_Id
                                             where SRTM.MappingStatus IN (" + DataTransferForTrainingDataMappingStatus + ")";
                    context.Configuration.AutoDetectChangesEnabled = false;
                    try { total = context.Database.SqlQuery<int>(strTotalCount.ToString()).FirstOrDefault(); } catch (Exception ex) { }
                    int NoOfBatch = total / BatchSize;
                    int mod = total % BatchSize;
                    if (mod > 0)
                        NoOfBatch = NoOfBatch + 1;
                    for (int BatchNo = 0; BatchNo < NoOfBatch; BatchNo++)
                    {
                        _obj = GetRoomTypeMatchDataForMLTrans(BatchSize, BatchNo);
                        object result = null;
                        DHSVCProxy.PostData(ProxyFor.MachingLearningDataTransfer, System.Configuration.ConfigurationManager.AppSettings["MLSVCURL_DataApi_Post_RoomTypeMatching"], _obj, typeof(DataContracts.ML.DC_ML_DL_RoomTypeMatch), typeof(DC_ML_Message), out result);
                    }

                }


            }
            catch (Exception ex)
            {

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
                                        ARI.Smoking AS AccoSmoking,
                                        ARI.FloorName AS AccoFloorName,
                                        ARI.FloorNumber As AccoFloorNumber,
                                        ARI.MysteryRoom AS AccoMysteryRoom,
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
                            AccommodationId = Convert.ToString(item.AccommodationId) ?? String.Empty,
                            SupplierId = Convert.ToString(item.SupplierId) ?? String.Empty,
                            SupplierName = item.SupplierName,
                            SupplierRoomId = item.SupplierRoomId,
                            SupplierRoomTypeCode = item.SupplierRoomTypeCode,
                            SupplierRoomName = item.SupplierRoomName,
                            TXRoomName = item.TXRoomName,
                            SupplierRoomCategory = item.SupplierRoomCategory,
                            SupplierRoomCategoryId = item.SupplierRoomCategoryId,
                            SupplierRoomCreateDate = Convert.ToString(item.SupplierRoomCreateDate) ?? String.Empty,
                            SupplierRoomCreateUser = item.SupplierRoomCreateUser,
                            SupplierRoomEditDate = Convert.ToString(item.SupplierRoomEditDate) ?? String.Empty,
                            SupplierRoomEditUser = item.SupplierRoomEditUser,
                            SupplierRoomMaxAdults = item.SupplierRoomMaxAdults ?? 0,
                            SupplierRoomMaxChild = item.SupplierRoomMaxChild ?? 0,
                            SupplierRoomMaxInfants = item.SupplierRoomMaxInfants ?? 0,
                            MaxGuestOccupancy = item.MaxGuestOccupancy ?? 0,
                            SupplierRoomQuantity = item.SupplierRoomQuantity ?? 0,
                            SupplierRoomRatePlan = item.SupplierRoomRatePlan,
                            RatePlanCode = item.RatePlanCode,
                            SupplierRoomSupplierProductName = item.SupplierRoomSupplierProductName,
                            SupplierRoomSupplierProductId = item.SupplierRoomSupplierProductId,
                            TxStrippedName = item.TxStrippedName,
                            TxReorderedName = item.TxReorderedName,
                            SupplierRoomMappingStatus = item.SupplierRoomMappingStatus,
                            MapId = Convert.ToString(item.MapId) ?? String.Empty,
                            AccommodationRoomInfoId = Convert.ToString(item.AccommodationRoomInfoId) ?? string.Empty,
                            SupplierRoomRoomDescription = item.SupplierRoomRoomDescription,
                            SupplierRoomRoomSize = item.SupplierRoomRoomSize,
                            TLGXCommonHotelId = Convert.ToString(item.TLGXCommonHotelId) ?? string.Empty,
                            AccoRoomId = item.AccoRoomId,
                            AccoRoomView = item.AccoRoomView,
                            AccoNoOfRooms = Convert.ToString(item.AccoNoOfRooms) ?? string.Empty,
                            AccoRoomName = item.AccoRoomName,
                            AccoNoOfInterconnectingRooms = Convert.ToString(item.AccoNoOfInterconnectingRooms) ?? string.Empty,
                            AccoDescription = item.AccoDescription,
                            AccoRoomSize = item.AccoRoomSize,
                            AccoRoomDecor = item.AccoRoomDecor,
                            AccoSmoking = Convert.ToString(item.AccoSmoking) ?? string.Empty,
                            AccoFloorName = item.AccoFloorName,
                            AccoFloorNumber = item.AccoFloorNumber,
                            AccoMysteryRoom = Convert.ToString(item.AccoMysteryRoom) ?? string.Empty,
                            AccoBathRoomType = item.AccoBathRoomType,
                            AccoBedType = item.AccoBedType,
                            AccoCompanyRoomCategory = item.AccoCompanyRoomCategory,
                            AccoRoomCategory = item.AccoRoomCategory,
                            AccoCreateDate = Convert.ToString(item.AccoCreateDate) ?? string.Empty,
                            AccoCreateUser = item.AccoCreateUser,
                            AccoEditDate = Convert.ToString(item.AccoEditDate) ?? string.Empty,
                            AccoEditUser = item.AccoEditUser,
                            SimilarityIndicator = item.SimilarityIndicator,
                            SimilarityScore = Convert.ToString(item.SimilarityScore) ?? string.Empty
                        });
                     }
                    _obj.Mode = "offline";
                    _obj.BatchId = Convert.ToString(batchNo);
                    _obj.Transaction = "1";

                }

                return _obj;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion


        #region *** SupplierAccommodationData ***
        public string ML_DataTransferSupplierAccommodationData()
        {
            DataContracts.ML.DC_ML_DL_SupplierAcco _obj = new DataContracts.ML.DC_ML_DL_SupplierAcco();
            try
            {

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    int total = 0;
                    //Get Batch Size
                    int BatchSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DataTransferBatchSize"]);
                    //Get Total Count
                    string strTotalCount = @"SELECT COUNT(1) FROM Accommodation with(nolock)";
                    context.Configuration.AutoDetectChangesEnabled = false;
                    try { total = context.Database.SqlQuery<int>(strTotalCount.ToString()).FirstOrDefault(); } catch (Exception ex) { }
                    int NoOfBatch = total / BatchSize;
                    int mod = total % BatchSize;
                    if (mod > 0)
                        NoOfBatch = NoOfBatch + 1;
                    for (int BatchNo = 0; BatchNo < NoOfBatch; BatchNo++)
                    {
                        _obj = GetSupplierAccoDataForMLTrans(BatchSize, BatchNo);
                        object result = null;
                        DHSVCProxy.PostData(ProxyFor.MachingLearningDataTransfer, System.Configuration.ConfigurationManager.AppSettings["MLSVCURL_DataApi_Post_MasterAccommodationRecord"], _obj, typeof(DataContracts.ML.DC_ML_DL_MasterAccoRecord), typeof(DC_ML_Message), out result);
                    }

                }


            }
            catch (Exception ex)
            {

                throw;
            }
            return string.Empty;
        }

        private DC_ML_DL_SupplierAcco GetSupplierAccoDataForMLTrans(int batchSize, int batchNo)
        {
            DataContracts.ML.DC_ML_DL_SupplierAcco _obj = new DataContracts.ML.DC_ML_DL_SupplierAcco();
            List<DataContracts.ML.DC_ML_DL_SupplierAcco_Data> _objData = new List<DataContracts.ML.DC_ML_DL_SupplierAcco_Data>();
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

                    _obj.SupplierAccommodationData = new List<DataContracts.ML.DC_ML_DL_SupplierAcco_Data>();
                    foreach (var item in _objAcooRoomFacility)
                    {
                        //_obj.MasterAccommodationRecord.Add(item);
                        _obj.SupplierAccommodationData.Add(new DataContracts.ML.DC_ML_DL_SupplierAcco_Data
                        {
                            //AccommodationRoomFacilityId = Convert.ToString(item.Accommodation_RoomFacility_Id) ?? string.Empty,
                            //AccommodationRoomInfoId = Convert.ToString(item.Accommodation_RoomInfo_Id) ?? string.Empty,
                            //AccommodationId = Convert.ToString(item.Accommodation_Id) ?? string.Empty,
                            //AmenityName = item.AmenityName ?? string.Empty,
                            //AmenityType = item.AmenityType ?? string.Empty,
                            //Description = item.Description ?? string.Empty,
                            //CreateDate = Convert.ToString(item.Create_Date) ?? string.Empty,
                            //CreateUser = item.Create_User ?? string.Empty,
                            //EditDate = Convert.ToString(item.Edit_Date) ?? string.Empty,
                            //Edituser = item.Edit_user ?? string.Empty
                        });
                    }
                    _obj.Mode = "offline";
                    _obj.BatchId = Convert.ToString(batchNo);
                    _obj.Transaction = "1";

                }

                return _obj;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion


        #region *** SupplierAccommodationRoomData Done***
        public string ML_DataTransferSupplierAccommodationRoomData()
        {
            DataContracts.ML.DC_ML_DL_SupplierAcco_Room _obj = new DataContracts.ML.DC_ML_DL_SupplierAcco_Room();
            try
            {

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    int total = 0;
                    //Get Batch Size
                    int BatchSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DataTransferBatchSize"]);
                    //Get Total Count
                    string strTotalCount = @"SELECT COUNT(1) FROM Accommodation_SupplierRoomTypeMapping with(nolock)";
                    context.Configuration.AutoDetectChangesEnabled = false;
                    try { total = context.Database.SqlQuery<int>(strTotalCount.ToString()).FirstOrDefault(); } catch (Exception ex) { }
                    int NoOfBatch = total / BatchSize;
                    int mod = total % BatchSize;
                    if (mod > 0)
                        NoOfBatch = NoOfBatch + 1;
                    for (int BatchNo = 0; BatchNo < NoOfBatch; BatchNo++)
                    {
                        _obj = GetSupplierAcco_RoomDataForMLTrans(BatchSize, BatchNo);
                        object result = null;
                        DHSVCProxy.PostData(ProxyFor.MachingLearningDataTransfer, System.Configuration.ConfigurationManager.AppSettings["MLSVCURL_DataApi_Post_SupplierAccommodationRoomData"], _obj, typeof(DataContracts.ML.DC_ML_DL_SupplierAcco_Room), typeof(DC_ML_Message), out result);
                    }

                }
            }
            catch (Exception ex)
            {

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

                            AccommodationSupplierRoomTypeMappingId = Convert.ToString(item.Accommodation_SupplierRoomTypeMapping_Id) ?? String.Empty,
                            AccommodationId = Convert.ToString(item.Accommodation_Id) ?? String.Empty,
                            SupplierId = Convert.ToString(item.Supplier_Id) ?? String.Empty,
                            SupplierName = item.SupplierName ?? String.Empty,
                            SupplierRoomId = item.SupplierRoomId ?? String.Empty,
                            SupplierRoomTypeCode = item.SupplierRoomTypeCode ?? String.Empty,
                            SupplierRoomName = item.SupplierRoomName ?? String.Empty,
                            TXRoomName = item.TX_RoomName ?? String.Empty,
                            SupplierRoomCategory = item.SupplierRoomCategory ?? String.Empty,
                            SupplierRoomCategoryId = item.SupplierRoomCategoryId ?? String.Empty,
                            CreateDate = Convert.ToString(item.Create_Date) ?? string.Empty,
                            CreateUser = item.Create_User ?? String.Empty,
                            EditDate = Convert.ToString(item.Edit_Date) ?? String.Empty,
                            EditUser = item.Edit_User ?? String.Empty,
                            MaxAdults = item.MaxAdults ?? 0,
                            MaxChild = item.MaxChild ?? 0,
                            MaxInfants = item.MaxInfants ?? 0,
                            MaxGuestOccupancy = item.MaxGuestOccupancy ?? 0,
                            Quantity = item.Quantity ?? 0,
                            RatePlan = item.RatePlan ?? String.Empty,
                            RatePlanCode = item.RatePlanCode ?? String.Empty,
                            SupplierProductName = item.SupplierProductName ?? String.Empty,
                            SupplierProductId = item.SupplierProductId ?? String.Empty,
                            TxStrippedName = item.Tx_StrippedName ?? String.Empty,
                            TxReorderedName = item.Tx_ReorderedName ?? String.Empty,
                            MappingStatus = item.MappingStatus ?? String.Empty,
                            MapId = item.MapId ?? 0,
                            AccommodationRoomInfoId = Convert.ToString(item.Accommodation_RoomInfo_Id) ?? String.Empty,
                            RoomSize = item.RoomSize ?? String.Empty,
                            BathRoomType = item.BathRoomType ?? String.Empty,
                            RoomViewCode = item.RoomViewCode ?? String.Empty,
                            FloorName = item.FloorName ?? String.Empty,
                            FloorNumber = item.FloorNumber ?? 0,
                            Amenities = item.Amenities ?? String.Empty,
                            RoomLocationCode = item.RoomLocationCode ?? String.Empty,
                            ChildAge = item.ChildAge ?? 0,
                            ExtraBed = item.ExtraBed ?? String.Empty,
                            Bedrooms = item.Bedrooms ?? String.Empty,
                            Smoking = item.Smoking ?? String.Empty,
                            BedTypeCode = item.BedTypeCode ?? String.Empty,
                            MinGuestOccupancy = item.MinGuestOccupancy ?? 0,
                            PromotionalVendorCode = item.PromotionalVendorCode ?? String.Empty,
                            BeddingConfig = item.BeddingConfig ?? String.Empty,
                        });
                    }
                    _obj.Mode = "offline";
                    _obj.BatchId = Convert.ToString(batchNo);
                    _obj.Transaction = "1";

                }

                return _obj;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion


        #region *** SupplierAccommodationRoomExtendedAttributes  Done***
        public string ML_DataTransferSupplierAccommodationRoomExtendedAttributes()
        {
            DataContracts.ML.DC_ML_DL_SupplierAcco_RoomExtendedAttributes _obj = new DataContracts.ML.DC_ML_DL_SupplierAcco_RoomExtendedAttributes();
            try
            {

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    int total = 0;
                    //Get Batch Size
                    int BatchSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DataTransferBatchSize"]);
                    //Get Total Count
                    string strTotalCount = @"SELECT COUNT(1) FROM Accommodation_SupplierRoomTypeAttributes with(nolock)";
                    context.Configuration.AutoDetectChangesEnabled = false;
                    try { total = context.Database.SqlQuery<int>(strTotalCount.ToString()).FirstOrDefault(); } catch (Exception ex) { }
                    int NoOfBatch = total / BatchSize;
                    int mod = total % BatchSize;
                    if (mod > 0)
                        NoOfBatch = NoOfBatch + 1;
                    for (int BatchNo = 0; BatchNo < NoOfBatch; BatchNo++)
                    {
                        _obj = GetSupplierAcco_RoomExtendedAttributesDataForMLTrans(BatchSize, BatchNo);
                        object result = null;
                        DHSVCProxy.PostData(ProxyFor.MachingLearningDataTransfer, System.Configuration.ConfigurationManager.AppSettings["MLSVCURL_DataApi_Post_SupplierAccommodationRoomExtendedAttributes"], _obj, typeof(DataContracts.ML.DC_ML_DL_SupplierAcco_RoomExtendedAttributes), typeof(DC_ML_Message), out result);
                    }

                }
            }
            catch (Exception ex)
            {

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
                            RoomTypeMapAttributeId = Convert.ToString(item.RoomTypeMapAttribute_Id) ?? String.Empty,
                            RoomTypeMapId = Convert.ToString(item.RoomTypeMap_Id) ?? String.Empty,
                            SupplierRoomTypeAttribute = Convert.ToString(item.SupplierRoomTypeAttribute) ?? String.Empty,
                            SystemAttributeKeyword = Convert.ToString(item.SystemAttributeKeyword) ?? String.Empty
                        });
                    }
                    _obj.Mode = "offline";
                    _obj.BatchId = Convert.ToString(batchNo);
                    _obj.Transaction = "1";

                }

                return _obj;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }

    public class DC_ML_Message
    {
        public string Mode { get; set; }
        public string BatchId { get; set; }
        public string Transaction { get; set; }
        public string Status { get; set; }

    }

}
