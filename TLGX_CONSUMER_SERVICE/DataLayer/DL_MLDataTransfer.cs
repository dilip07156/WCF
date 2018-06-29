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

        #region *** MasterAccommodationRecord ***
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
        #region ***  MasterAccommodationRoomInformation ***
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
                        _obj = GetRoomTypeMatchDataForMLTrans(BatchSize, BatchNo);
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

        private DC_ML_DL_RoomTypeMatch GetRoomTypeMatchDataForMLTrans(int batchSize, int batchNo)
        {
            DataContracts.ML.DC_ML_DL_RoomTypeMatch _obj = new DataContracts.ML.DC_ML_DL_RoomTypeMatch();
            List<DataContracts.ML.DC_ML_DL_RoomTypeMatch_Data> _objData = new List<DataContracts.ML.DC_ML_DL_RoomTypeMatch_Data>();
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

                    _obj.RoomTypeMatching = new List<DataContracts.ML.DC_ML_DL_RoomTypeMatch_Data>();
                    foreach (var item in _objAcooRoomFacility)
                    {
                        ////_obj.MasterAccommodationRecord.Add(item);
                        //_obj.MasterAccommodationRoomFacilities.Add(new DataContracts.ML.DC_ML_DL_MasterAccoRoomFacility_Data
                        //{
                        //    AccommodationRoomFacilityId = Convert.ToString(item.Accommodation_RoomFacility_Id) ?? string.Empty,
                        //    AccommodationRoomInfoId = Convert.ToString(item.Accommodation_RoomInfo_Id) ?? string.Empty,
                        //    AccommodationId = Convert.ToString(item.Accommodation_Id) ?? string.Empty,
                        //    AmenityName = item.AmenityName ?? string.Empty,
                        //    AmenityType = item.AmenityType ?? string.Empty,
                        //    Description = item.Description ?? string.Empty,
                        //    CreateDate = Convert.ToString(item.Create_Date) ?? string.Empty,
                        //    CreateUser = item.Create_User ?? string.Empty,
                        //    EditDate = Convert.ToString(item.Edit_Date) ?? string.Empty,
                        //    Edituser = item.Edit_user ?? string.Empty
                        //});
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
        #region *** SupplierAccommodationRoomData ***
        public string ML_DataTransferSupplierAccommodationRoomData()
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
            return string.Empty;
        }
        #endregion
        #region *** SupplierAccommodationRoomExtendedAttributes ***
        public string ML_DataTransferSupplierAccommodationRoomExtendedAttributes()
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
            return string.Empty;
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
