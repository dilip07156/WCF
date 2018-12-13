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
                                        CompanyHotelID As TLGXCommonHotelId,
                                        HotelName As AccommodationName,
                                        city As City,
                                        country As Country,
                                        FullAddress AS Address,
                                        PostalCode As PostalCode,
                                        Create_Date AS CreateDate,
                                        Create_User AS CreateUser,
                                        Edit_Date AS EditDate,
                                        Edit_User As Edituser,
										TLGXAccoId As TLGXAccoId
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
                            TLGXCommonHotelId = item.TLGXCommonHotelId,
                            TLGXAccoId = item.TLGXAccoId
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
                                        arF.Accommodation_Id,
                                        arF.Accommodation_RoomInfo_Id,
                                        AmenityType,
                                        AmenityName,
                                        arF.Description,
                                        arF.Create_Date,
                                        arF.Create_User,
                                        arF.Edit_Date,
                                        arF.Edit_user,
										ari.TLGXAccoRoomId,
										ac.CompanyHotelID As TLGXCommonHotelId,
										ac.TLGXAccoId
                                        FROM Accommodation_RoomFacility arF with(nolock) 
										JOIN Accommodation_RoomInfo ari with(nolock) on arf.Accommodation_RoomInfo_Id = ari.Accommodation_RoomInfo_Id
										join Accommodation ac with(nolock) on ari.Accommodation_Id = ac.Accommodation_Id  ");
                    int skip = batchNo * batchSize;
                    sbOrderby.Append("  ORDER BY arF.Accommodation_RoomFacility_Id OFFSET " + (skip).ToString() + " ROWS FETCH NEXT " + batchSize.ToString() + " ROWS ONLY ");

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
                            Edituser = item.Edit_user,
                            TLGXCommonHotelId = item.TLGXCommonHotelId,
                            TLGXAccoRoomId = item.TLGXAccoRoomId,
                            TLGXAccoId = item.TLGXAccoId
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

        #region ***  MasterAccommodationRoomInformation  ***
        public string ML_DataTransferMasterAccommodationRoomInformation(Guid LogId)
        {
            DataContracts.ML.DC_ML_DL_MasterAccoRoomInfo _obj = new DataContracts.ML.DC_ML_DL_MasterAccoRoomInfo();
            DataContracts.ML.DC_ML_DL_MasterAccoRoomInfo _objToSend = new DataContracts.ML.DC_ML_DL_MasterAccoRoomInfo();
            _objToSend.Mode = "offline";
            _objToSend.Transaction = "1";

            List<DataContracts.DC_ML_MasterAccoRoomInfo_Data> _objAcoo = new List<DataContracts.DC_ML_MasterAccoRoomInfo_Data>();
            List<DC_ML_DL_AccoRoom_ExtendedAttributes_Data> _objAccoRoomAttributes = new List<DC_ML_DL_AccoRoom_ExtendedAttributes_Data>();

            int TotalCount = 0;
            int MLDataInsertedCount = 0;
            try
            {
                UpdateDistLogInfo(LogId, PushStatus.RUNNNING);
                _objAcoo = GetMasterAccoRoomInformationDataForMLTrans();
                _objAccoRoomAttributes = GetMasterAccoRoomExtendedAttrsForMLTrans();
                TotalCount = _objAcoo.Count();

                foreach (var item in _objAcoo)
                {
                    _objToSend.MasterAccommodationRoomInformation.Add(new DataContracts.ML.DC_ML_DL_MasterAccoRoomInfo_Data
                    {
                        AccommodationRoomInfoId = Convert.ToString(item.Accommodation_RoomInfo_Id),
                        AccommodationId = Convert.ToString(item.Accommodation_Id),
                        TLGXCommonHotelId = item.TLGXCommonHotelId,
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
                        RoomInfo_TX = item.RoomInfo_TX,
                        TLGXAccoRoomId = item.TLGXAccoRoomId,
                        TLGXAccoId = item.TLGXAccoId,
                        ExtractedAttributes = _objAccoRoomAttributes.Where(w => w.Accommodation_RoomInfo_Id == item.Accommodation_RoomInfo_Id).Select(s => new ExtractedAttributes { Key = s.SystemAttributeKeyword, Value = s.Accommodation_RoomInfo_Attribute }).ToList()
                    });
                    _objAccoRoomAttributes.RemoveAll(x => x.Accommodation_RoomInfo_Id == item.Accommodation_RoomInfo_Id);
                    MLDataInsertedCount = MLDataInsertedCount + 1;

                    if (MLDataInsertedCount % 1000 == 0 || MLDataInsertedCount == TotalCount)
                    {
                        _objToSend.BatchId = Guid.NewGuid().ToString();
                        
                        DHSVCProxy.PostDataNewtonsoft(ProxyFor.MachingLearningDataTransfer, System.Configuration.ConfigurationManager.AppSettings["MLSVCURL_DataApi_Post_MasterAccommodationRoomInformation"], _objToSend, typeof(DataContracts.ML.DC_ML_DL_MasterAccoRoomInfo), typeof(DC_ML_Message), out object result);

                        _objToSend.MasterAccommodationRoomInformation.Clear();

                        UpdateDistLogInfo(LogId, PushStatus.RUNNNING, TotalCount, MLDataInsertedCount);
                        
                    }
                }
            }
            catch (Exception ex)
            {
                UpdateDistLogInfo(LogId, PushStatus.ERROR, TotalCount, MLDataInsertedCount);
                throw;
            }
            return string.Empty;
        }
        private List<DataContracts.DC_ML_MasterAccoRoomInfo_Data> GetMasterAccoRoomInformationDataForMLTrans()
        {
            List<DataContracts.DC_ML_MasterAccoRoomInfo_Data> _objAcoo = new List<DataContracts.DC_ML_MasterAccoRoomInfo_Data>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    context.Database.CommandTimeout = 0;
                    context.Configuration.AutoDetectChangesEnabled = false;

                    StringBuilder sbSelect = new StringBuilder();
                    sbSelect.Append(@"SELECT 
                                       ARI.Accommodation_RoomInfo_Id,
                                       ARI.Accommodation_Id,
                                       Ac.CompanyHotelID AS TLGXCommonHotelId,
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
                                        ARI.Create_User,
                                        ARI.Create_Date,
                                        ARI.Edit_User,
                                        ARI.Edit_Date,
                                        TX_RoomName as RoomInfo_TX,
                                        TLGXAccoRoomId,
                                        AC.TLGXAccoId
                                        FROM Accommodation_RoomInfo ARI with(nolock) 
										join Accommodation AC with(nolock) on ARI.Accommodation_Id = AC.Accommodation_Id ");

                    try { _objAcoo = context.Database.SqlQuery<DataContracts.DC_ML_MasterAccoRoomInfo_Data>(sbSelect.ToString()).ToList(); } catch (Exception ex) { }
                }

                return _objAcoo;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private List<DC_ML_DL_AccoRoom_ExtendedAttributes_Data> GetMasterAccoRoomExtendedAttrsForMLTrans()
        {
            List<DC_ML_DL_AccoRoom_ExtendedAttributes_Data> _objAcooRoomAttrs = new List<DC_ML_DL_AccoRoom_ExtendedAttributes_Data>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    context.Database.CommandTimeout = 0;
                    context.Configuration.AutoDetectChangesEnabled = false;

                    StringBuilder sbSelect = new StringBuilder();
                    StringBuilder sbOrderby = new StringBuilder();
                    //Accommodation_RoomInfo_Attribute_Id,
                    sbSelect.Append(@"SELECT   
                                        Accommodation_RoomInfo_Id,
                                        Accommodation_RoomInfo_Attribute,
                                        SystemAttributeKeyword 
                                        FROM Accommodation_RoomInfo_Attributes with(nolock); ");

                    StringBuilder sbfinal = new StringBuilder();
                    sbfinal.Append(sbSelect);

                    try { _objAcooRoomAttrs = context.Database.SqlQuery<DC_ML_DL_AccoRoom_ExtendedAttributes_Data>(sbfinal.ToString()).ToList(); } catch (Exception ex) { }
                }

                return _objAcooRoomAttrs;
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

            DataContracts.ML.DC_ML_DL_RoomTypeMatch _objToSend = new DataContracts.ML.DC_ML_DL_RoomTypeMatch();

            int TotalCount = 0;
            int MLDataInsertedCount = 0;
            try
            {
                UpdateDistLogInfo(LogId, PushStatus.RUNNNING);

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    context.Configuration.AutoDetectChangesEnabled = false;
                    context.Database.CommandTimeout = 0;

                    int BatchSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DataTransferBatchSize"]);
                    string DataTransferForTrainingDataMappingStatus = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DataTransferForTrainingDataMappingStatus"]);

                    _obj = GetRoomTypeMatchDataForMLTrans(0, 0);

                    TotalCount = _obj.RoomTypeMatching.Count();

                    int NoOfBatch = TotalCount / BatchSize;
                    int mod = TotalCount % BatchSize;
                    if (mod > 0)
                        NoOfBatch = NoOfBatch + 1;

                    for (int BatchNo = 0; BatchNo < NoOfBatch; BatchNo++)
                    {
                        #region To update CounterIn DistributionLog

                        if (_obj != null)
                        {
                            _objToSend.BatchId = Guid.NewGuid().ToString();
                            _objToSend.Mode = _obj.Mode;
                            _objToSend.Transaction = _obj.Transaction;
                            _objToSend.RoomTypeMatching = _obj.RoomTypeMatching.OrderBy(o => o.AccommodationSupplierRoomTypeMappingId).Skip(BatchNo * BatchSize).Take(BatchSize).ToList();

                            object result = null;
                            DHSVCProxy.PostDataNewtonsoft(ProxyFor.MachingLearningDataTransfer, System.Configuration.ConfigurationManager.AppSettings["MLSVCURL_DataApi_Post_RoomTypeMatching"], _objToSend, typeof(DataContracts.ML.DC_ML_DL_RoomTypeMatch), typeof(DC_ML_Message), out result);

                            MLDataInsertedCount = MLDataInsertedCount + _objToSend.RoomTypeMatching.Count();
                            UpdateDistLogInfo(LogId, PushStatus.RUNNNING, TotalCount, MLDataInsertedCount);
                        }

                        #endregion

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

            //string DataTransferForTrainingDataMappingStatus = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DataTransferForTrainingDataMappingStatus"]);
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    context.Database.CommandTimeout = 0;

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

                                        SRTM.RoomViewCode AS SupplierRoomView, 
                                        ISNULL(SRTM.Bedrooms,'') AS SupplierRoomBedType, 
                                        ISNULL(SRTM.BedTypeCode,'') AS SupplierRoomBedTypeCode, 
                                        SRTM.Smoking AS SupplierRoomSmoking, 

                                        SRTM.SupplierRoomCategory AS SupplierRoomCategory, 
                                        SRTM.SupplierRoomCategoryId AS SupplierRoomCategoryId, 
                                        SRTM.Create_Date AS SupplierRoomCreateDate, 
                                        SRTM.Create_User AS SupplierRoomCreateUser, 
                                        SRTMV.UserEditDate AS SupplierRoomEditDate, 
                                        SRTMV.Edit_User AS SupplierRoomEditUser, 
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
                                        SRTMV.UserMappingStatus AS  SupplierRoomMappingStatus, 
                                        SRTMV.MapId AS  MapId, 
                                        SRTMV.Accommodation_RoomInfo_Id AS  AccommodationRoomInfoId, 
                                        SRTM.RoomDescription AS  SupplierRoomRoomDescription, 
                                        SRTM.RoomSize AS  SupplierRoomRoomSize,
                                        AC.CompanyHotelID AS TLGXCommonHotelId,
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
                                        ARI.Create_User AS AccoCreateUser,
                                        ARI.Create_Date AS AccoCreateUser,
                                        ARI.Edit_User as AccoEditUser,
                                        ARI.Edit_Date as AccoEditDate,
                                        SRTM.MatchingScore,
										ARI.TLGXAccoRoomId,
                                        AC.TLGXAccoId
                                        FROM Accommodation_SupplierRoomTypeMapping_Values SRTMV WITH(NOLOCK)
                                        JOIN Accommodation_SupplierRoomTypeMapping SRTM WITH (NOLOCK) ON SRTMV.Accommodation_SupplierRoomTypeMapping_Id = SRTM.Accommodation_SupplierRoomTypeMapping_Id
                                        JOIN Accommodation_RoomInfo ARI WITH (NOLOCK) ON SRTMV.Accommodation_RoomInfo_Id = ARI.Accommodation_RoomInfo_Id 
										JOIN Accommodation AC with(nolock) on ARI.Accommodation_Id = AC.Accommodation_Id 
                                        where SRTMV.UserMappingStatus IN ('MAPPED','UNMAPPED') AND ISNULL(SRTM.IsNotTraining, 0 ) = 0 ; ");


                    StringBuilder sbfinal = new StringBuilder();
                    sbfinal.Append(sbSelect);

                    context.Configuration.AutoDetectChangesEnabled = false;
                    try { _objAcooRoomMatching = context.Database.SqlQuery<DataContracts.DC_ML_RoomTypeMatch_Data>(sbfinal.ToString()).ToList(); } catch (Exception ex) { }

                    #region get room attributes

                    List<DataContracts.DC_ML_SupplierAcco_RoomExtendedAttributes_Data> SRTA = new List<DataContracts.DC_ML_SupplierAcco_RoomExtendedAttributes_Data>();
                    try
                    {
                        StringBuilder sbSelectSRTA = new StringBuilder();
                        sbSelectSRTA.Append(@"SELECT   
                                                RoomTypeMapAttribute_Id,RoomTypeMap_Id,
                                                SupplierRoomTypeAttribute,SystemAttributeKeyword
                                                FROM Accommodation_SupplierRoomTypeAttributes SRTMA with(nolock) 
                                                INNER JOIN Accommodation_SupplierRoomTypeMapping_Values SRTMV with(nolock) ON SRTMA.RoomTypeMap_Id = SRTMV.Accommodation_SupplierRoomTypeMapping_Id 
                                                WHERE SRTMV.UserMappingStatus IN ('MAPPED','UNMAPPED') ");

                        try { SRTA = context.Database.SqlQuery<DataContracts.DC_ML_SupplierAcco_RoomExtendedAttributes_Data>(sbSelectSRTA.ToString()).ToList(); } catch (Exception ex) { }
                    }
                    catch (Exception ex)
                    {
                    }
                    #endregion get room attributes

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
                            SupplierRoomBedType = Convert.ToString(item.SupplierRoomBedType) + " - " + Convert.ToString(item.SupplierRoomBedTypeCode),
                            SupplierRoomSmoking = item.SupplierRoomSmoking,
                            SupplierRoomView = item.SupplierRoomView,
                            SupplierRoomExtractedAttributes = SRTA.Where(w => w.RoomTypeMap_Id == item.AccommodationSupplierRoomTypeMappingId).Select(s => new DC_ML_RoomTypeMatch_ExtractedAttributes { Key = s.SystemAttributeKeyword, Value = s.SupplierRoomTypeAttribute }).ToList(),
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
                            SimilarityIndicator = (item.SupplierRoomMappingStatus == "MAPPED" ? true : false),//  Convert.ToBoolean(item.SimilarityIndicator),
                            SimilarityScore = (item.SupplierRoomMappingStatus == "MAPPED" ? 1 : 0),
                            TLGXAccoRoomId = item.TLGXAccoRoomId,
                            TLGXAccoId = item.TLGXAccoId
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
                    context.Database.CommandTimeout = 0;
                    context.Configuration.AutoDetectChangesEnabled = false;
                    StringBuilder sbSelect = new StringBuilder();
                    StringBuilder sbOrderby = new StringBuilder();
                    sbSelect.Append(@"  SELECT 
                                        APM.Accommodation_ProductMapping_Id,
                                        APM.Accommodation_Id,
                                        APM.SupplierProductReference,
                                        APM.SupplierName,
                                        APM.Supplier_Id AS SupplierId,
                                        Acc.CompanyHotelID AS TLGXCommonHotelId,
                                        APM.ProductName AS AccommodationName,
                                        APM.CountryName AS Country,
                                        APM.CityName AS City,
                                        APM.Address ,
                                        APM.PostCode AS PostalCode,
                                        APM.Create_Date AS CreateDate,
                                        APM.Create_User AS CreateUser,
                                        APM.Edit_Date AS EditDate,
                                        APM.Edit_User AS Edituser,
                                        Acc.TLGXAccoId
                                        from Accommodation_ProductMapping APM WITH(NOLOCK)
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
                            TLGXCommonHotelId = item.TLGXCommonHotelId,
                            TLGXAccoId = item.TLGXAccoId
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
                        MLDataInsertedCount = MLDataInsertedCount + _obj.SupplierAccommodationRoomData.Count(); ;
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
                    context.Database.CommandTimeout = 0;
                    context.Configuration.AutoDetectChangesEnabled = false;
                    StringBuilder sbSelect = new StringBuilder();
                    StringBuilder sbOrderby = new StringBuilder();
                    sbSelect.Append(@"SELECT  
                                        Accommodation_SupplierRoomTypeMapping_Id,Asrtm.Accommodation_Id,Supplier_Id
                                      ,SupplierName,SupplierRoomId,SupplierRoomTypeCode
                                      ,SupplierRoomName,TX_RoomName,SupplierRoomCategory,SupplierRoomCategoryId
                                      ,Asrtm.Create_Date,Asrtm.Create_User,Asrtm.Edit_Date,Asrtm.Edit_User,MaxAdults
                                      ,MaxChild,MaxInfants,MaxGuestOccupancy,Quantity,RatePlan
                                      ,RatePlanCode,SupplierProductName,SupplierProductId,Tx_StrippedName,Tx_ReorderedName
                                      ,MappingStatus,MapId,Accommodation_RoomInfo_Id,RoomSize,BathRoomType
                                      ,RoomViewCode,FloorName,FloorNumber,Amenities,RoomLocationCode
                                      ,ChildAge,ExtraBed,Bedrooms,Smoking,BedTypeCode,MinGuestOccupancy,PromotionalVendorCode,BeddingConfig
									  ,AC.TLGXAccoId,Ac.CompanyHotelID As TLGXCommonHotelId
                                      FROM Accommodation_SupplierRoomTypeMapping Asrtm with(nolock) 
									  left join Accommodation Ac with(nolock) on Asrtm.Accommodation_Id = Ac.Accommodation_Id  ");
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
                            TLGXCommonHotelId = item.TLGXCommonHotelId,
                            TLGXAccoId = item.TLGXAccoId
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

        public List<DataContracts.ML.DL_ML_DL_EntityStatus> GetMLDataApiTransferStatus()
        {
            List<DataContracts.ML.DL_ML_DL_EntityStatus> _obj = new List<DataContracts.ML.DL_ML_DL_EntityStatus>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    StringBuilder sbSelect = new StringBuilder();
                    sbSelect.Append(@"select t.[Element] AS EntityType, t.Edit_Date AS LastUpdate,t.Status,Cast(TotalCount As nvarchar(100)) AS TotalCount ,Cast(MongoPushCount As nvarchar(100)) AS PushedCount
                                    ,CASE WHEN TotalCount != 0 THEN cast(round((((Cast(MongoPushCount As numeric))/cast(TotalCount As numeric)) * 100.0),2)as numeric(36,2)) ELSE 0 END AS Per                                    
                                     from DistributionLayerRefresh_Log t with(Nolock)
                                     INNER JOIN (
                                        select [Element], max(Edit_Date) as MaxDate
                                        from DistributionLayerRefresh_Log with(Nolock) where Element Like 'MLDATA%'
                                        group by [Element]
                                    ) tm on t.[Element] = tm.[Element] and t.Edit_Date = tm.MaxDate
                                ");
                    context.Configuration.AutoDetectChangesEnabled = false;
                    try { _obj = context.Database.SqlQuery<DataContracts.ML.DL_ML_DL_EntityStatus>(sbSelect.ToString()).ToList(); } catch (Exception ex) { }

                    return _obj;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        //RealTime Data Tranasction
        #region To Delete Training Data
        public void ML_DataTransfer_DeleteTrainingData(Guid accommodation_SupplierRoomTypeMapping_Id)
        {
            try
            {
                using (DHSVCProxyAsync DHP = new DHSVCProxyAsync())
                {
                    var objToDelete = new DC_ML_DL_SupplierAcco_Room_Data_Delete();
                    objToDelete.Transaction = Convert.ToString(Guid.NewGuid());
                    objToDelete.AccommodationSupplierRoomTypeMappingIds.Add(Convert.ToString(accommodation_SupplierRoomTypeMapping_Id));
                    object result = null;
                    DHSVCProxy.PostDataNewtonsoft(ProxyFor.MachingLearningDataTransfer, System.Configuration.ConfigurationManager.AppSettings["MLSVCURL_DataApi_RoomTypeMatching_Delete"], objToDelete, typeof(DataContracts.ML.DC_ML_DL_SupplierAcco_Room_Data_Delete), typeof(DC_ML_DL_SupplierAcco_Room_Data_SuccessResponse), out result, "DELETE");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region
        enum MappingStatus { MAPPED = 0, UNMAPPED, AUTOMAPPED, REVIEW }
        public void ML_DataTransfer_TrainingDataPushToAIML(Guid accommodation_SupplierRoomTypeMapping_Id)
        {
            try
            {
                //Getting Data from DB
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var result = context.Accommodation_SupplierRoomTypeMapping.Find(accommodation_SupplierRoomTypeMapping_Id);

                    if (result != null)
                    {

                        List<Accommodation_SupplierRoomTypeMapping_Values> SupplierRoomTypeMappingValue = new List<Accommodation_SupplierRoomTypeMapping_Values>();
                        SupplierRoomTypeMappingValue = context.Accommodation_SupplierRoomTypeMapping_Values.Where(rv => rv.Accommodation_SupplierRoomTypeMapping_Id == accommodation_SupplierRoomTypeMapping_Id && rv.Accommodation_RoomInfo_Id != null).ToList();

                        var maxValue = SupplierRoomTypeMappingValue.Max(x => x.MatchingScore);


                        var accodetails = context.Accommodation.Find(result.Accommodation_Id);

                        if (SupplierRoomTypeMappingValue != null)
                        {

                            DC_ML_DL_SupplierAcco_Room_Data_RealTime _objToSendAIML;

                            foreach (var item in SupplierRoomTypeMappingValue)
                            {
                                if ((item.SystemEditDate ?? DateTime.MinValue) > (item.UserEditDate ?? DateTime.MinValue))
                                {
                                    continue;
                                }
                                if (item.UserMappingStatus.ToUpper() == MappingStatus.MAPPED.ToString() && item.MatchingScore != maxValue)
                                {
                                    continue;
                                }
                                //Creating Data to send AIML
                                var roominfo = context.Accommodation_RoomInfo.Find(item.Accommodation_RoomInfo_Id);
                                //Creating Data to send AIML
                                _objToSendAIML = new DC_ML_DL_SupplierAcco_Room_Data_RealTime()
                                {
                                    AccommodationSupplierRoomTypeMappingId = Convert.ToString(item.Accommodation_SupplierRoomTypeMapping_Id),
                                    AccommodationId = Convert.ToString(result.Accommodation_Id),
                                    SupplierId = Convert.ToString(result.Supplier_Id),
                                    SupplierName = result.SupplierName,
                                    SupplierRoomId = result.SupplierRoomId,
                                    SupplierRoomTypeCode = result.SupplierRoomTypeCode,
                                    SupplierRoomName = result.SupplierRoomName,
                                    TXRoomName = result.TX_RoomName,
                                    SupplierRoomView = result.RoomViewCode,
                                    SupplierRoomBedType = result.BedTypeCode,
                                    SupplierRoomSmoking = result.Smoking,
                                    SupplierRoomCategory = result.SupplierRoomCategory,
                                    SupplierRoomCategoryId = result.SupplierRoomCategoryId,
                                    SupplierRoomCreateDate = Convert.ToString(result.Create_Date),
                                    SupplierRoomCreateUser = result.Create_User,
                                    SupplierRoomEditDate = Convert.ToString(item.UserEditDate),
                                    SupplierRoomEditUser = item.Edit_User,

                                    SupplierRoomMaxAdults = result.MaxAdults,
                                    SupplierRoomMaxChild = result.MaxChild,
                                    SupplierRoomMaxInfants = result.MaxInfants,
                                    MaxGuestOccupancy = result.MaxGuestOccupancy,
                                    SupplierRoomQuantity = result.Quantity,
                                    SupplierRoomRatePlan = result.RatePlan,
                                    RatePlanCode = result.RatePlanCode,
                                    SupplierRoomSupplierProductName = result.SupplierProductName,
                                    SupplierRoomSupplierProductId = result.SupplierProductId,
                                    TxStrippedName = result.Tx_StrippedName,
                                    TxReorderedName = result.Tx_ReorderedName,

                                    SupplierRoomMappingStatus = item.UserMappingStatus,
                                    MapId = item.MapId,
                                    AccommodationRoomInfoId = Convert.ToString(item.Accommodation_RoomInfo_Id),

                                    SupplierRoomRoomDescription = result.RoomDescription,
                                    SupplierRoomRoomSize = result.RoomSize,
                                    TLGXCommonHotelId = accodetails.CompanyHotelID,
                                    AccoRoomId = Convert.ToString(item.Accommodation_RoomInfo_Id),
                                    AccoRoomView = roominfo.RoomView,
                                    AccoNoOfRooms = roominfo.NoOfRooms,
                                    AccoRoomName = roominfo.RoomName,
                                    AccoNoOfInterconnectingRooms = roominfo.NoOfInterconnectingRooms,
                                    AccoDescription = roominfo.Description,
                                    AccoRoomSize = roominfo.RoomSize,
                                    AccoRoomDecor = roominfo.RoomDecor,
                                    AccoSmoking = roominfo.Smoking,
                                    AccoFloorName = roominfo.FloorName,
                                    AccoFloorNumber = roominfo.FloorNumber,
                                    AccoMysteryRoom = roominfo.MysteryRoom,
                                    AccoBathRoomType = roominfo.BathRoomType,
                                    AccoBedType = roominfo.BedType,
                                    AccoCompanyRoomCategory = roominfo.CompanyRoomCategory,
                                    AccoRoomCategory = roominfo.RoomCategory,
                                    AccoCreateDate = Convert.ToString(accodetails.Create_Date),
                                    AccoCreateUser = accodetails.Create_User,
                                    AccoEditDate = Convert.ToString(accodetails.Edit_Date),
                                    AccoEditUser = accodetails.Edit_User
                                };

                                //Setting SimilarityIndicator and SimilarityScore
                                #region -- Setting SimilarityIndicator and SimilarityScore
                                if (item.UserMappingStatus == MappingStatus.MAPPED.ToString())
                                {
                                    _objToSendAIML.SimilarityIndicator = true;
                                    _objToSendAIML.SimilarityScore = 1;
                                }
                                else if (item.UserMappingStatus == MappingStatus.UNMAPPED.ToString() && item.SystemMappingStatus != null)
                                {
                                    _objToSendAIML.SimilarityIndicator = false;
                                    _objToSendAIML.SimilarityScore = 0;
                                }
                                else
                                {
                                    continue;
                                }
                                #endregion

                                //Get Attribute master
                                #region -- Get Attribute master
                                var attribtes = context.Accommodation_SupplierRoomTypeAttributes.Where(srta => srta.RoomTypeMap_Id == accommodation_SupplierRoomTypeMapping_Id).ToList();
                                if (attribtes != null && attribtes.Count > 0)
                                {
                                    foreach (var itemattribtes in attribtes)
                                    {
                                        _objToSendAIML.SupplierRoomExtractedAttributes.Add(
                                            new DC_ML_DL_SupplierAcco_Room_Attributes_Data_RealTime()
                                            {
                                                Key = itemattribtes.SystemAttributeKeyword,
                                                Value = itemattribtes.SupplierRoomTypeAttribute
                                            });
                                    }
                                }
                                #endregion

                                DC_ML_DL_SupplierAcco_Room_RealTime _objMain = new DC_ML_DL_SupplierAcco_Room_RealTime();
                                _objMain.RoomTypeMatching.Add(_objToSendAIML);
                                _objMain.Mode = "Online";
                                _objMain.BatchId = Convert.ToString(Guid.NewGuid());
                                _objMain.Transaction = Convert.ToString(Guid.NewGuid());
                                object resultobj = null;
                                DHSVCProxy.PostDataNewtonsoft(ProxyFor.MachingLearningDataTransfer, System.Configuration.ConfigurationManager.AppSettings["MLSVCURL_DataApi_Post_RoomTypeMatching"], _objMain, typeof(DataContracts.ML.DC_ML_DL_SupplierAcco_Room_RealTime), typeof(DC_ML_DL_SupplierAcco_Room_Data_SuccessResponse), out resultobj);

                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {

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
