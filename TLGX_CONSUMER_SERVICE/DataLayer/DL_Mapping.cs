using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;
using DataContracts.Mapping;
using DataContracts.UploadStaticData;
using EntityFramework.Extensions;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using DataContracts.STG;
using DataContracts;
using System.Globalization;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using DataContracts.ML;

namespace DataLayer
{
    public class DL_Mapping : IDisposable
    {
        public void Dispose()
        {
        }

        #region Declaration
        public DL_UploadStaticData USD = new DL_UploadStaticData();
        DC_SupplierImportFile_Progress PLog = new DC_SupplierImportFile_Progress();
        DL_Masters master = new DL_Masters();
        #endregion

        #region STG Handlers

        public void CallLogVerbose(Guid SupplierImportFile_Id, string Step, string Message, int? BatchNumber = null)
        {
            DC_SupplierImportFile_VerboseLog obj = new DC_SupplierImportFile_VerboseLog()
            {
                SupplierImportFile_VerboseLog_Id = Guid.NewGuid(),
                SupplierImportFile_Id = SupplierImportFile_Id,
                Step = Step,
                Message = Message,
                TimeStamp = DateTime.Now,
                BatchNumber = BatchNumber
            };
            DataContracts.DC_Message RM = USD.AddStaticDataUploadVerboseLog(obj);
        }

        public bool DeleteSTGMappingTableIDs(Guid File_Id)
        {
            bool ret = false;
            try
            {
                if (File_Id != null && File_Id != Guid.Empty)
                {
                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        DC_SupplierImportFileDetails_RQ RQ = new DC_SupplierImportFileDetails_RQ();
                        RQ.SupplierImportFile_Id = File_Id;
                        RQ.PageNo = 0;
                        RQ.PageSize = int.MaxValue;
                        var FileDetails = USD.GetStaticDataFileDetail(RQ);

                        string SupplierName = FileDetails[0].Supplier.ToString();
                        string Entity = FileDetails[0].Entity.ToString().ToUpper();

                        switch (Entity)
                        {
                            case "COUNTRY":
                                var stgCountries = (from y in context.stg_SupplierCountryMapping
                                                    where y.SupplierName.ToString().ToUpper() == SupplierName.ToString().ToUpper()
                                                    && y.SupplierImportFile_Id == File_Id
                                                    select y).ToList();
                                context.stg_SupplierCountryMapping.RemoveRange(stgCountries);
                                context.SaveChanges();
                                break;
                            case "CITY":
                                var stgCities = (from y in context.stg_SupplierCityMapping
                                                 where y.SupplierName.ToString().ToUpper() == SupplierName.ToString().ToUpper()
                                                    && y.SupplierImportFile_Id == File_Id
                                                 select y).ToList();
                                context.stg_SupplierCityMapping.RemoveRange(stgCities);
                                context.SaveChanges();
                                break;
                            case "HOTEL":
                                var stgHotel = (from y in context.stg_SupplierProductMapping
                                                where y.SupplierName.ToString().ToUpper() == SupplierName.ToString().ToUpper()
                                                    && y.SupplierImportFile_Id == File_Id
                                                select y).ToList();
                                context.stg_SupplierProductMapping.RemoveRange(stgHotel);
                                context.SaveChanges();
                                break;
                            case "ROOMTYPE":
                                var stgRoomType = (from y in context.stg_SupplierHotelRoomMapping
                                                   where y.SupplierName.ToString().ToUpper() == SupplierName.ToString().ToUpper()
                                                    && y.SupplierImportFile_Id == File_Id
                                                   select y).ToList();
                                context.stg_SupplierHotelRoomMapping.RemoveRange(stgRoomType);
                                context.SaveChanges();
                                break;
                        }

                        //var oldRecords = (from y in context.STG_Mapping_TableIds
                        //                  where y.File_Id == File_Id
                        //                  select y).ToList();
                        //context.STG_Mapping_TableIds.RemoveRange(oldRecords);
                        //context.SaveChanges();
                        ret = true;
                    }
                }
                return ret;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool AddSTGMappingTableIDs(List<DataContracts.STG.DC_STG_Mapping_Table_Ids> lstobj)
        {
            bool ret = false;
            if (lstobj.Count > 0)
            {
                Guid File_Id = Guid.Parse(lstobj[0].File_Id.ToString());
                try
                {
                    if (File_Id != null && File_Id != Guid.Empty)
                    {
                        using (ConsumerEntities context = new ConsumerEntities())
                        {
                            List<DataLayer.STG_Mapping_TableIds> lstobjnew = new List<STG_Mapping_TableIds>();

                            lstobjnew = (from a in lstobj
                                         select new STG_Mapping_TableIds()
                                         {
                                             STG_Mapping_Table_Id = Guid.NewGuid(),
                                             File_Id = File_Id,
                                             Mapping_Id = a.Mapping_Id,
                                             STG_Id = a.STG_Id,
                                             Batch = a.Batch
                                         }).ToList();

                            context.STG_Mapping_TableIds.AddRange(lstobjnew);
                            context.SaveChanges();
                            ret = true;
                        }
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return ret;
        }

        public bool AddSTGMappingTableIDs(DC_MappingMatch obj)
        {

            if (obj.SupplierDetail.Supplier_Id == null)
            {
                return false;
            }

            string curSupplier_Id = (obj.SupplierDetail.Supplier_Id ?? Guid.Empty).ToString();
            string File_Id = obj.File_Id.ToString();
            string CurrentBatch = (obj.CurrentBatch ?? 1).ToString();
            string mode = obj.FileMode;

            int batchno = (obj.CurrentBatch ?? 1);
            int batch = (obj.BatchSize ?? 250);
            int RowCounterFrom = 0;
            int RowCounterTo = 0;

            if (string.IsNullOrWhiteSpace(mode))
            {
                mode = "ALL";
            }

            RowCounterFrom = (batchno - 1) * batch;
            RowCounterTo = batchno * batch;

            string sql = string.Empty;

            //this is to update srtm acco id as per apm
            string sqlUpdateSrtm = string.Empty;

            //sql = sql + " UPDATE TOP (" + batch.ToString() + ") #tablename SET ReRun_SupplierImportFile_Id = '" + File_Id.ToString() + "', ReRun_Batch = " + (obj.CurrentBatch ?? 1).ToString() + " ";
            //sql = sql + " WHERE supplier_id = '" + curSupplier_Id.ToString() + "' ";
            //sql = sql + " and ISNULL(ReRun_SupplierImportFile_Id,cast(cast(0 as binary) as uniqueidentifier)) != '" + File_Id.ToString() + "' AND ISNULL(ReRun_Batch,0) != " + (obj.CurrentBatch ?? 1).ToString() + " ";

            sql = sql + " UPDATE #tablename SET ReRun_Batch = " + CurrentBatch;
            sql = sql + " WHERE supplier_id = '" + curSupplier_Id + "' AND ReRun_SupplierImportFile_Id = '" + File_Id + "' AND ReRun_Batch IS NULL ";
            sql = sql + " AND RowCounter > " + RowCounterFrom.ToString() + " AND RowCounter <= " + RowCounterTo.ToString() + ";";

            sqlUpdateSrtm = "UPDATE SRTM set SRTM.Accommodation_Id = APM.Accommodation_Id from Accommodation_SupplierRoomTypeMapping SRTM ";
            sqlUpdateSrtm = sqlUpdateSrtm + " INNER JOIN Accommodation_ProductMapping APM ON SRTM.Supplier_Id = APM.Supplier_Id AND SRTM.SupplierProductId = APM.SupplierProductReference ";
            sqlUpdateSrtm = sqlUpdateSrtm + " WHERE SRTM.Accommodation_Id IS NULL AND APM.Status IN ('MAPPED','AUTOMAPPED') ";
            sqlUpdateSrtm = sqlUpdateSrtm + " AND SRTM.ReRun_SupplierImportFile_Id = '" + File_Id + "' AND SRTM.ReRun_Batch = " + CurrentBatch + " AND SRTM.Supplier_id = '" + curSupplier_Id + "';";

            //if (mode == "ALL")
            //{
            //    sql = sql + " and #statuscolumn = 'UNMAPPED' ";
            //}
            //else if (mode != "ALL")
            //{
            //    if (obj.FileEntity.ToUpper().Trim() == "ROOMTYPE")
            //    {
            //        sql = sql + "and #statuscolumn IN ('UNMAPPED','REVIEW','ADD') ";
            //    }
            //    else
            //    {
            //        sql = sql + "and #statuscolumn IN ('UNMAPPED','REVIEW') ";
            //    }
            //}

            if (obj.FileEntity.ToUpper().Trim() == "HOTEL")
            {
                sql = sql.Replace("#tablename", "Accommodation_ProductMapping");
                //sql = sql.Replace("#statuscolumn", "Status");
            }

            if (obj.FileEntity.ToUpper().Trim() == "COUNTRY")
            {
                sql = sql.Replace("#tablename", "m_CountryMapping");
                //sql = sql.Replace("#statuscolumn", "Status");
            }

            if (obj.FileEntity.ToUpper().Trim() == "CITY")
            {
                sql = sql.Replace("#tablename", "m_CityMapping");
                //sql = sql.Replace("#statuscolumn", "Status");
            }

            if (obj.FileEntity.ToUpper().Trim() == "ROOMTYPE")
            {
                sql = sql.Replace("#tablename", "Accommodation_SupplierRoomTypeMapping");
                //sql = sql.Replace("#statuscolumn", "MappingStatus");
            }

            int isadded = 0;
            using (ConsumerEntities context = new ConsumerEntities())
            {
                context.Database.CommandTimeout = 0;

                try { isadded = context.Database.ExecuteSqlCommand(sql); } catch (Exception ex) { return false; }

                if (obj.FileEntity.ToUpper().Trim() == "ROOMTYPE")
                {
                    try { context.Database.ExecuteSqlCommand(sqlUpdateSrtm); } catch (Exception ex) { }
                }
            }

            if (isadded > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetSTGMappingIDTableCount(DC_SupplierImportFileDetails file)
        {
            int ret = 0;

            Guid File_Id = file.SupplierImportFile_Id;
            Guid Supplier_Id = file.Supplier_Id;
            string mode = file.Mode;

            if (string.IsNullOrWhiteSpace(mode))
            {
                mode = "ALL";
            }

            string sql = string.Empty;
            string sqlwhere = string.Empty;
            string sqlUpdate = string.Empty;

            sql = "select count(*) as ct from #tablename where ReRun_SupplierImportFile_Id = '" + File_Id.ToString() + "';";

            sqlwhere = " where supplier_id =  '" + Supplier_Id.ToString() + "' ";
            sqlUpdate = ";WITH RowNumbers AS (SELECT #ID, ROW_NUMBER() OVER(ORDER BY #ID) AS RowNo from #tablename with (nolock) #where) ";
            sqlUpdate = sqlUpdate + "UPDATE tbl SET tbl.RowCounter = row.RowNo, tbl.ReRun_SupplierImportFile_Id = '" + File_Id.ToString() + "', ReRun_Batch = NULL ";
            sqlUpdate = sqlUpdate + "FROM #tablename tbl INNER JOIN RowNumbers row ON tbl.#ID = row.#ID;";

            if (mode == "ALL")
            {
                sqlwhere = sqlwhere + " and #status = 'UNMAPPED' ";
            }
            else if (mode != "ALL")
            {
                if (file.Entity.ToUpper().Trim() != "ROOMTYPE")
                {
                    sqlwhere = sqlwhere + " and #status IN ('UNMAPPED', 'REVIEW') ";
                }
                else if (file.Entity.ToUpper().Trim() == "ROOMTYPE")
                {
                    sqlwhere = sqlwhere + " and #status IN ('UNMAPPED', 'REVIEW', 'ADD') ";
                }
            }

            //sql = sql + sqlwhere;
            sqlUpdate = sqlUpdate.Replace("#where", sqlwhere);

            if (file.Entity.ToUpper().Trim() == "HOTEL")
            {
                sql = sql.Replace("#tablename", "Accommodation_ProductMapping");
                sql = sql.Replace("#status", "status");

                sqlUpdate = sqlUpdate.Replace("#tablename", "Accommodation_ProductMapping");
                sqlUpdate = sqlUpdate.Replace("#status", "status");
                sqlUpdate = sqlUpdate.Replace("#ID", "Accommodation_ProductMapping_Id");
            }
            if (file.Entity.ToUpper().Trim() == "COUNTRY")
            {
                sql = sql.Replace("#tablename", "m_CountryMapping");
                sql = sql.Replace("#status", "status");

                sqlUpdate = sqlUpdate.Replace("#tablename", "m_CountryMapping");
                sqlUpdate = sqlUpdate.Replace("#status", "status");
                sqlUpdate = sqlUpdate.Replace("#ID", "CountryMapping_Id");
            }
            if (file.Entity.ToUpper().Trim() == "CITY")
            {
                sql = sql.Replace("#tablename", "m_CityMapping");
                sql = sql.Replace("#status", "status");

                sqlUpdate = sqlUpdate.Replace("#tablename", "m_CityMapping");
                sqlUpdate = sqlUpdate.Replace("#status", "status");
                sqlUpdate = sqlUpdate.Replace("#ID", "CityMapping_Id");
            }
            if (file.Entity.ToUpper().Trim() == "ROOMTYPE")
            {
                sql = sql.Replace("#tablename", "Accommodation_SupplierRoomTypeMapping");
                sql = sql.Replace("#status", "MappingStatus");

                sqlUpdate = sqlUpdate.Replace("#tablename", "Accommodation_SupplierRoomTypeMapping");
                sqlUpdate = sqlUpdate.Replace("#status", "MappingStatus");
                sqlUpdate = sqlUpdate.Replace("#ID", "Accommodation_SupplierRoomTypeMapping_Id");
            }
            using (ConsumerEntities context = new ConsumerEntities())
            {
                context.Database.CommandTimeout = 0;
                try { context.Database.ExecuteSqlCommand(sqlUpdate); } catch (Exception ex) { }
                try { ret = context.Database.SqlQuery<int>(sql).FirstOrDefault(); } catch (Exception ex) { }
            }

            return ret;
        }

        #endregion

        #region Accomodation Product Mapping

        public bool ShiftAccommodationMappings(DataContracts.Mapping.DC_Mapping_ShiftMapping_RQ obj)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var AccoMappingIds = context.Accommodation_ProductMapping.Where(w => w.Accommodation_Id == obj.Accommodation_From_Id).Select(s => s.Accommodation_ProductMapping_Id).ToList();
                    foreach (var AccoMappingId in AccoMappingIds)
                    {
                        Accommodation_ProductMapping apm = context.Accommodation_ProductMapping.Attach(new Accommodation_ProductMapping
                        {
                            Accommodation_ProductMapping_Id = AccoMappingId
                        });

                        apm.Accommodation_Id = obj.Accommodation_To_Id;
                        apm.Edit_Date = obj.Edit_Date;
                        apm.Edit_User = obj.Edit_User;
                        apm.Remarks = obj.Remarks;

                        context.Entry<Accommodation_ProductMapping>(apm).Property(ee => ee.Accommodation_Id).IsModified = true;
                        context.Entry<Accommodation_ProductMapping>(apm).Property(ee => ee.Edit_Date).IsModified = true;
                        context.Entry<Accommodation_ProductMapping>(apm).Property(ee => ee.Edit_User).IsModified = true;
                        context.Entry<Accommodation_ProductMapping>(apm).Property(ee => ee.Remarks).IsModified = true;

                        context.Configuration.ValidateOnSaveEnabled = false;

                        context.SaveChanges();
                    }

                }
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching accomodation product mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
            return true;
        }

        public string[] GetMappingHotelDataForTTFU(DataContracts.Masters.DC_Supplier obj)
        {
            string[] ret = new string[] { };
            if (obj != null)
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    context.Database.CommandTimeout = 0;
                    Guid File_Id = new Guid();
                    File_Id = Guid.Parse(obj.File_Id.ToString());
                    string CurSupplierName = obj.Name;
                    Guid? CurSupplier_Id = Guid.Parse(obj.Supplier_Id.ToString());

                    System.Linq.IQueryable<DataContracts.Mapping.DC_Accomodation_ProductMapping> prodMapList;
                    List<DC_Accomodation_ProductMapping> lstProdMap = new List<DC_Accomodation_ProductMapping>();
                    prodMapList = (from a in context.Accommodation_ProductMapping.AsNoTracking()
                                   where a.ReRun_SupplierImportFile_Id == obj.File_Id && ((a.ReRun_Batch == obj.CurrentBatch && (obj.CurrentBatch ?? 0) != 0) || ((obj.CurrentBatch ?? 0) == 0))
                                   select new DataContracts.Mapping.DC_Accomodation_ProductMapping
                                   {
                                       Accommodation_ProductMapping_Id = a.Accommodation_ProductMapping_Id
                                   });
                    //prodMapList = (from s in context.STG_Mapping_TableIds.AsNoTracking()
                    //                   //join stg in context.stg_SupplierProductMapping.AsNoTracking() on s.STG_Id equals stg.stg_AccoMapping_Id
                    //               where s.File_Id == obj.File_Id && ((s.Batch == obj.CurrentBatch && (obj.CurrentBatch ?? 0) != 0) || ((obj.CurrentBatch ?? 0) == 0))
                    //               select new DataContracts.Mapping.DC_Accomodation_ProductMapping
                    //               {
                    //                   Accommodation_ProductMapping_Id = s.Mapping_Id ?? Guid.Empty
                    //               });
                    lstProdMap = prodMapList.ToList();
                    List<string> str = new List<string>();
                    if (lstProdMap.Count > 0)
                    {
                        foreach (DC_Accomodation_ProductMapping rec in lstProdMap)
                        {
                            str.Add(rec.Accommodation_ProductMapping_Id.ToString());
                        }
                    }
                    ret = str.ToArray();
                }
            }
            return ret;
        }

        public List<DataContracts.Mapping.DC_Accomodation_ProductMapping> GetAccomodationProductMappingById(Guid Accommodation_ProductMapping_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    List<DataContracts.Mapping.DC_Accomodation_ProductMapping> lstAcco = new List<DataContracts.Mapping.DC_Accomodation_ProductMapping>();

                    StringBuilder sbQuery = new StringBuilder();
                    sbQuery.Append(@"SELECT APM.Accommodation_ProductMapping_Id AS Accommodation_ProductMapping_Id,APM.Accommodation_Id AS Accommodation_Id,
                                        APM.Supplier_Id AS Supplier_Id,
                                        APM.SupplierId AS SupplierId, APM.SupplierName AS SupplierName,APM.SupplierProductReference AS SupplierProductReference,
                                        APM.ProductName AS ProductName, APM.Street AS Street, APM.Street2 AS Street2,APM.Street3 AS Street3,
                                        APM.Street4 AS Street4,APM.CountryCode AS CountryCode,APM.CountryName AS CountryName,APM.CityCode AS CityCode,
                                        APM.CityName AS CityName,APM.StateCode AS StateCode,APM.StateName AS StateName,APM.PostCode AS PostCode,
                                        APM.TelephoneNumber AS TelephoneNumber,APM.Fax AS Fax,APM.Email AS Email,APM.Website AS Website
                                       ,ISNULL(APM.Latitude,APM.GeoLocation.Lat) AS Latitude
                                       ,ISNULL(APM.Longitude,APM.GeoLocation.Long) AS Longitude,
                                        APM.Status AS Status,APM.Create_Date AS Create_Date,APM.Create_User AS Create_User,
                                        APM.Edit_Date AS Edit_Date,APM.Edit_User AS Edit_User,ISNULL(APM.IsActive,0) AS IsActive,APM.SupplierProductReference AS ProductId,
                                        CM.Name AS SystemCountryName,C.Name AS SystemCityName,AC.HotelName AS SystemProductName,S.StateName AS SystemStateName,
                                        APM.Remarks AS Remarks,APM.MapId AS MapId,
                                        (CASE WHEN APM.ADDRESS IS NOT NULL THEN APM.ADDRESS
                                              ELSE ISNULL(APM.Street + ',  ', '') + ISNULL(APM.Street2 + ' ', '')
                                                + ISNULL(APM.Street3 + ' ', '')
                                                + ISNULL(APM.Street4 + ' ', '')
                                                + ISNULL(APM.PostCode,'') END
                                              ) AS FullAddress,
                                              (CASE WHEN AC.FullAddress IS NOT NULL THEN AC.FullAddress
                                              ELSE
                                                  ISNULL(AC.StreetNumber + ',  ', '')
                                                + ISNULL(AC.StreetName + ', ', '')
                                                + ISNULL(AC.Street3 + ', ', '')
                                                + ISNULL(AC.Street4 + ', ', '')
                                                + ISNULL(AC.Street5 + ',  ', '')
                                                + ISNULL(AC.PostalCode, '') END
	                                          ) AS SystemFullAddress,
                                        APM.StarRating AS StarRating,
                                        APM.HotelName_Tx AS HotelName_Tx,
                                        AC.Telephone_Tx AS SystemTelephone,
                                        AC.Location AS SystemLocation,
                                        AC.Latitude AS SystemLatitude,
                                        AC.Longitude AS SystemLongitude,
                                        APM.MatchedBy AS MatchedBy,
                                        APM.MatchedByString AS MatchedByString,
                                        APM.Country_Id AS Country_Id,
                                        APM.City_Id AS City_Id,
                                        APM.ProductType,
                                        AC.ProductCategorySubType AS SystemProductType
                                        FROM Accommodation_ProductMapping APM
                                        LEFT OUTER JOIN Accommodation AC ON APM.Accommodation_Id = AC.Accommodation_Id AND ISNULL(AC.ISACTIVE,0) = 1 
                                        LEFT OUTER JOIN m_CountryMaster CM ON LTRIM(RTRIM(APM.CountryName)) = LTRIM(RTRIM(CM.Name))
                                        LEFT OUTER JOIN m_CityMaster C ON (LTRIM(RTRIM(APM.CityName)) = LTRIM(RTRIM(C.Name)) AND LTRIM(RTRIM(APM.CountryName)) = LTRIM(RTRIM(C.CountryName)))
                                        LEFT OUTER JOIN m_States S ON LTRIM(RTRIM(APM.StateName)) = LTRIM(RTRIM(S.StateName))
                                        WHERE APM.Accommodation_ProductMapping_Id = '");
                    sbQuery.Append(Accommodation_ProductMapping_Id + "'");
                    sbQuery.Append(" ORDER BY APM.SupplierName,APM.ProductName,APM.SupplierProductReference");

                    try { lstAcco = context.Database.SqlQuery<DataContracts.Mapping.DC_Accomodation_ProductMapping>(sbQuery.ToString()).ToList(); } catch (Exception ex) { }




                    foreach (var item in lstAcco)
                    {
                        if (item.Accommodation_Id == null)
                        {
                            if (!string.IsNullOrWhiteSpace(item.ProductName))
                            {
                                var prodname = item.ProductName.ToLower().Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("hotel", "").ToUpper();
                                var prodcode = item.ProductId;
                                if (!string.IsNullOrWhiteSpace(prodname) && prodname.ToUpper() != "&NBSP;")
                                {
                                    var searchprod = context.Accommodation.Where(a => ((a.IsActive ?? false) == true) && (a.country == item.SystemCountryName) && (a.city == item.SystemCityName) && (a.HotelName ?? string.Empty).Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("hotel", "").ToUpper().Equals(item.ProductName.ToLower().Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("hotel", "").ToUpper())).FirstOrDefault();
                                    if (searchprod == null)
                                        searchprod = context.Accommodation.Where(a => ((a.IsActive ?? false) == true) && (a.country == item.SystemCountryName) && (a.city == item.SystemCityName) && (a.HotelName ?? string.Empty).Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("hotel", "").ToUpper().Contains(item.ProductName.ToLower().Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("hotel", "").ToUpper())).FirstOrDefault();
                                    if (!string.IsNullOrWhiteSpace(prodcode) && prodname.ToUpper() != "&NBSP;")
                                    {
                                        if (searchprod == null)
                                            searchprod = context.Accommodation.Where(a => ((a.IsActive ?? false) == true) && (a.country == item.SystemCountryName) && (a.city == item.SystemCityName) && (a.HotelName ?? string.Empty).ToUpper().Equals(prodcode)).FirstOrDefault();
                                    }
                                    if (searchprod != null)
                                    {
                                        item.SystemProductName = searchprod.HotelName;
                                        item.Accommodation_Id = searchprod.Accommodation_Id;
                                        item.SystemLatitude = searchprod.Latitude;
                                        item.SystemLongitude = searchprod.Longitude;
                                        item.SystemProductCode = Convert.ToString(searchprod.CompanyHotelID);
                                        item.SystemTelephone = searchprod.Telephone_Tx;
                                        item.SystemLocation = searchprod.Location;
                                        item.SystemCityName = searchprod.city;
                                        item.SystemCountryName = searchprod.country;
                                        //item.FullAddress = searchprod.FullAddress;
                                        item.SystemFullAddress = searchprod.FullAddress;
                                    }
                                }
                            }
                        }
                        else
                        {
                            var systemproduct = context.Accommodation.Where(a => ((a.IsActive ?? false) == true) && a.Accommodation_Id == item.Accommodation_Id).FirstOrDefault();
                            if (systemproduct != null)
                            {
                                item.SystemProductName = systemproduct.HotelName;
                                item.Accommodation_Id = systemproduct.Accommodation_Id;
                                item.SystemLatitude = systemproduct.Latitude;
                                item.SystemLongitude = systemproduct.Longitude;
                                item.SystemProductCode = Convert.ToString(systemproduct.CompanyHotelID);
                                item.SystemTelephone = systemproduct.Telephone_Tx;
                                item.SystemLocation = systemproduct.Location;
                                item.SystemCityName = systemproduct.city;
                                item.SystemCountryName = systemproduct.country;
                                //item.FullAddress = searchprod.FullAddress;
                                item.SystemFullAddress = systemproduct.FullAddress;
                            }
                        }


                    }
                    return lstAcco;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching accomodation product mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool HotelTTFUTelephone(DataContracts.Masters.DC_Supplier obj)
        {
            bool ret = true;
            if (obj != null)
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    Guid File_Id = new Guid();
                    File_Id = Guid.Parse(obj.File_Id.ToString());
                    string CurSupplierName = obj.Name;
                    Guid? CurSupplier_Id = Guid.Parse(obj.Supplier_Id.ToString());
                    List<DC_Accomodation_ProductMapping> telephones = new List<DC_Accomodation_ProductMapping>();
                    telephones = (from a in context.Accommodation_ProductMapping.AsNoTracking()
                                  where a.Supplier_Id == CurSupplier_Id && a.ReRun_SupplierImportFile_Id == obj.File_Id
                                  select new DC_Accomodation_ProductMapping
                                  {
                                      Accommodation_ProductMapping_Id = a.Accommodation_ProductMapping_Id,
                                      TelephoneNumber = a.TelephoneNumber ?? "",
                                      TelephoneNumber_tx = a.TelephoneNumber_tx ?? ""
                                  }
                                  ).ToList();

                    telephones = telephones.Select(c =>
                    {
                        c.TelephoneNumber = CommonFunctions.GetDigits(c.TelephoneNumber, 8);
                        return c;
                    }).ToList();

                    telephones.RemoveAll(p => p.TelephoneNumber == p.TelephoneNumber_tx);
                    foreach (var telephone in telephones)
                    {
                        var search = (from a in context.Accommodation_ProductMapping
                                      where a.Accommodation_ProductMapping_Id == telephone.Accommodation_ProductMapping_Id
                                      select a).FirstOrDefault();
                        if (search != null)
                        {
                            search.TelephoneNumber_tx = telephone.TelephoneNumber;
                            context.SaveChanges();
                        }
                    }
                }
            }
            return ret;
        }

        public bool HotelMappingMatch(DataContracts.Masters.DC_Supplier obj)
        {
            bool ret = true;
            Guid File_Id = new Guid();
            File_Id = Guid.Parse(obj.File_Id.ToString());
            PLog.SupplierImportFileProgress_Id = Guid.NewGuid();
            PLog.SupplierImportFile_Id = obj.File_Id;
            PLog.Step = "MAP";
            PLog.Status = "MAPPING";
            PLog.CurrentBatch = obj.CurrentBatch ?? 0;
            PLog.TotalBatch = obj.TotalBatch ?? 0;
            DL_UploadStaticData staticdata = new DL_UploadStaticData();

            if (obj != null)
            {
                string CurSupplierName = obj.Name;
                Guid CurSupplier_Id = Guid.Parse(obj.Supplier_Id.ToString());

                List<DataContracts.STG.DC_stg_SupplierProductMapping> clsSTGHotel = new List<DataContracts.STG.DC_stg_SupplierProductMapping>();
                List<DataContracts.STG.DC_stg_SupplierProductMapping> clsSTGHotelInsert = new List<DataContracts.STG.DC_stg_SupplierProductMapping>();
                List<DC_Accomodation_ProductMapping> clsMappingHotel = new List<DC_Accomodation_ProductMapping>();

                DataContracts.STG.DC_stg_SupplierProductMapping_RQ RQ = new DataContracts.STG.DC_stg_SupplierProductMapping_RQ();
                RQ.Supplier_Id = CurSupplier_Id;
                RQ.SupplierImportFile_Id = File_Id;

                clsSTGHotel = staticdata.GetSTGHotelData(RQ);

                PLog.PercentageValue = 15;
                USD.AddStaticDataUploadProcessLog(PLog);

                //Dupe check hotel logic
                CheckHotelAlreadyExist(File_Id, obj.CurrentBatch ?? 0, CurSupplier_Id, CurSupplierName, clsSTGHotel, out clsMappingHotel, out clsSTGHotelInsert);

                PLog.PercentageValue = 53;
                USD.AddStaticDataUploadProcessLog(PLog);

                clsMappingHotel.InsertRange(clsMappingHotel.Count, clsSTGHotelInsert.Select
                    (g => new DC_Accomodation_ProductMapping
                    {
                        Accommodation_ProductMapping_Id = Guid.NewGuid(),
                        SupplierProductReference = g.ProductId,
                        ProductName = g.ProductName,
                        ProductId = g.ProductId,
                        Country_Id = g.Country_Id,
                        City_Id = g.City_Id,
                        CountryName = g.CountryName,
                        CountryCode = g.CountryCode,
                        CityCode = g.CityCode,
                        CityName = g.CityName,
                        StateCode = g.StateCode,
                        StateName = g.StateName,
                        Supplier_Id = CurSupplier_Id,
                        SupplierName = g.SupplierName,
                        Status = "UNMAPPED",
                        Create_Date = DateTime.Now,
                        Create_User = "TLGX_DataHandler",
                        Latitude = g.Latitude,
                        Longitude = g.Longitude,
                        Email = g.Email,
                        Fax = g.Fax,
                        Google_Place_Id = g.Google_Place_Id,
                        PostCode = g.PostalCode,
                        Street = (g.Address == null ? (g.StreetNo + " " + g.StreetName) : g.Address),
                        Street2 = (g.Address == null ? g.Street2 : ""),
                        Street3 = (g.Address == null ? g.Street3 : ""),
                        Street4 = (g.Street4 ?? "") + " " + (g.Street5 ?? ""),
                        StarRating = g.StarRating,
                        SupplierId = g.SupplierId,
                        TelephoneNumber = g.TelephoneNumber,
                        Website = g.Website,
                        ActionType = "INSERT",
                        stg_AccoMapping_Id = g.stg_AccoMapping_Id,
                        FullAddress = (g.Address == null ? ((g.StreetNo ?? "") + (((g.StreetNo ?? "") != "") ? ", " : "")
                                       + (g.StreetName ?? "") + (((g.StreetName ?? "") != "") ? ", " : "")
                                       + (g.Street2 ?? "") + (((g.Street2 ?? "") != "") ? ", " : "")
                                       + (g.Street3 ?? "") + (((g.Street3 ?? "") != "") ? ", " : "")
                                       + (g.Street4 ?? "") + (((g.Street4 ?? "") != "") ? ", " : "")
                                       + (g.Street5 ?? "") + (((g.Street5 ?? "") != "") ? ", " : "") + (g.PostalCode ?? ""))
                                       : ((g.Address ?? "") + (((g.Address ?? "") != "") ? ", " : "") + (g.PostalCode ?? ""))).Trim().TrimEnd(',')
                        ,
                        SupplierImporrtFile_Id = obj.File_Id ?? Guid.Empty,
                        Batch = obj.CurrentBatch ?? 0,
                        ReRunSupplierImporrtFile_Id = obj.File_Id ?? Guid.Empty,
                        ReRunBatch = obj.CurrentBatch ?? 0,
                        ProductType = g.ProductType,
                        IsActive = true,
                        Remarks = string.Empty
                    }));

                PLog.PercentageValue = 60;
                USD.AddStaticDataUploadProcessLog(PLog);
                CallLogVerbose(File_Id, "MAP", "Updating / Inserting to database.", obj.CurrentBatch);

                if (clsMappingHotel.Count > 0)
                {
                    ret = UpdateAccomodationProductMapping(clsMappingHotel);
                }
            }
            PLog.PercentageValue = 100;
            USD.AddStaticDataUploadProcessLog(PLog);
            CallLogVerbose(File_Id, "MAP", "MAP Process Complete", obj.CurrentBatch);
            return ret;
        }

        public void CheckHotelAlreadyExist(Guid File_Id, int Batch, Guid CurSupplier_Id, string SupplierCode, List<DataContracts.STG.DC_stg_SupplierProductMapping> stg, out List<DC_Accomodation_ProductMapping> updateMappingList, out List<DataContracts.STG.DC_stg_SupplierProductMapping> insertSTGList)
        {
            using (ConsumerEntities context = new ConsumerEntities())
            {
                //Here we have to check City Wise unique supplier product codes for certain suppliers like 'GTA'

                List<DataContracts.Mapping.DC_Accomodation_ProductMapping> toUpdate = new List<DC_Accomodation_ProductMapping>();

                if (SupplierCode.ToUpper() == "GTA")
                {
                    //CheckByCityCode
                    List<DataContracts.Mapping.DC_Accomodation_ProductMapping> toUpdateByCityCode = new List<DC_Accomodation_ProductMapping>();
                    List<DataContracts.Mapping.DC_Accomodation_ProductMapping> toUpdateByCityName = new List<DC_Accomodation_ProductMapping>();

                    toUpdateByCityCode = (from a in context.Accommodation_ProductMapping.AsNoTracking()
                                          join s in context.stg_SupplierProductMapping.AsNoTracking() on
                                          new { a.Supplier_Id, a.SupplierProductReference, a.CityCode } equals new { s.Supplier_Id, SupplierProductReference = s.ProductId, s.CityCode }
                                          where s.SupplierImportFile_Id == File_Id
                                          select new DataContracts.Mapping.DC_Accomodation_ProductMapping
                                          {
                                              Accommodation_ProductMapping_Id = a.Accommodation_ProductMapping_Id,
                                              Accommodation_Id = a.Accommodation_Id,
                                              ProductName = s.ProductName,
                                              Street = s.StreetName,
                                              Street2 = s.Street2,
                                              Street3 = s.Street3,
                                              Street4 = s.Street4,
                                              CountryCode = s.CountryCode,
                                              CountryName = s.CountryName,
                                              CityCode = s.CityCode,
                                              CityName = s.CityName,
                                              StateCode = s.StateCode,
                                              StateName = s.StateName,
                                              FullAddress = s.Address,
                                              PostCode = s.PostalCode,
                                              TelephoneNumber = s.TelephoneNumber,
                                              Fax = s.Fax,
                                              Email = s.Email,
                                              Website = s.Website,
                                              Latitude = s.Latitude,
                                              Longitude = s.Longitude,
                                              Edit_Date = DateTime.Now,
                                              Edit_User = "TLGX_DataHandler",
                                              IsActive = true,
                                              StarRating = s.StarRating,
                                              Country_Id = s.Country_Id,
                                              City_Id = s.City_Id,
                                              ActionType = "UPDATE",
                                              stg_AccoMapping_Id = s.stg_AccoMapping_Id,
                                              ProductType = s.ProductType,
                                              ReRunSupplierImporrtFile_Id = File_Id,
                                              ReRunBatch = Batch,
                                              Status = a.Status
                                          }).ToList();

                    if (toUpdateByCityCode.Count == 0)
                    {
                        //CheckByCityName
                        toUpdateByCityName = (from a in context.Accommodation_ProductMapping.AsNoTracking()
                                              join s in context.stg_SupplierProductMapping.AsNoTracking() on
                                              new { a.Supplier_Id, a.SupplierProductReference, a.CountryCode, a.CityName } equals new { s.Supplier_Id, SupplierProductReference = s.ProductId, s.CountryCode, s.CityName }
                                              where s.SupplierImportFile_Id == File_Id
                                              select new DataContracts.Mapping.DC_Accomodation_ProductMapping
                                              {
                                                  Accommodation_ProductMapping_Id = a.Accommodation_ProductMapping_Id,
                                                  Accommodation_Id = a.Accommodation_Id,
                                                  ProductName = s.ProductName,
                                                  Street = s.StreetName,
                                                  Street2 = s.Street2,
                                                  Street3 = s.Street3,
                                                  Street4 = s.Street4,
                                                  CountryCode = s.CountryCode,
                                                  CountryName = s.CountryName,
                                                  CityCode = s.CityCode,
                                                  CityName = s.CityName,
                                                  StateCode = s.StateCode,
                                                  StateName = s.StateName,
                                                  FullAddress = s.Address,
                                                  PostCode = s.PostalCode,
                                                  TelephoneNumber = s.TelephoneNumber,
                                                  Fax = s.Fax,
                                                  Email = s.Email,
                                                  Website = s.Website,
                                                  Latitude = s.Latitude,
                                                  Longitude = s.Longitude,
                                                  Edit_Date = DateTime.Now,
                                                  Edit_User = "TLGX_DataHandler",
                                                  IsActive = true,
                                                  StarRating = s.StarRating,
                                                  Country_Id = s.Country_Id,
                                                  City_Id = s.City_Id,
                                                  ActionType = "UPDATE",
                                                  stg_AccoMapping_Id = s.stg_AccoMapping_Id,
                                                  ProductType = s.ProductType,
                                                  ReRunSupplierImporrtFile_Id = File_Id,
                                                  ReRunBatch = Batch,
                                                  Status = a.Status
                                              }).ToList();

                        if (toUpdateByCityName.Count > 0)
                        {
                            //toUpdateByCityName.RemoveAll(r => toUpdateByCityCode.Any(w => w.Accommodation_ProductMapping_Id == r.Accommodation_ProductMapping_Id));
                            toUpdateByCityCode.InsertRange(0, toUpdateByCityName);
                        }
                    }

                    toUpdate.InsertRange(0, toUpdateByCityCode);
                }
                else
                {
                    toUpdate = (from a in context.Accommodation_ProductMapping.AsNoTracking()
                                join s in context.stg_SupplierProductMapping.AsNoTracking() on
                                new { a.Supplier_Id, a.SupplierProductReference } equals new { s.Supplier_Id, SupplierProductReference = s.ProductId }
                                where s.SupplierImportFile_Id == File_Id
                                select new DataContracts.Mapping.DC_Accomodation_ProductMapping
                                {
                                    Accommodation_ProductMapping_Id = a.Accommodation_ProductMapping_Id,
                                    Accommodation_Id = a.Accommodation_Id,
                                    ProductName = s.ProductName,
                                    Street = s.StreetName,
                                    Street2 = s.Street2,
                                    Street3 = s.Street3,
                                    Street4 = s.Street4,
                                    CountryCode = s.CountryCode,
                                    CountryName = s.CountryName,
                                    CityCode = s.CityCode,
                                    CityName = s.CityName,
                                    StateCode = s.StateCode,
                                    StateName = s.StateName,
                                    FullAddress = s.Address,
                                    PostCode = s.PostalCode,
                                    TelephoneNumber = s.TelephoneNumber,
                                    Fax = s.Fax,
                                    Email = s.Email,
                                    Website = s.Website,
                                    Latitude = s.Latitude,
                                    Longitude = s.Longitude,
                                    Edit_Date = DateTime.Now,
                                    Edit_User = "TLGX_DataHandler",
                                    IsActive = true,
                                    StarRating = s.StarRating,
                                    Country_Id = s.Country_Id,
                                    City_Id = s.City_Id,
                                    ActionType = "UPDATE",
                                    stg_AccoMapping_Id = s.stg_AccoMapping_Id,
                                    ProductType = s.ProductType,
                                    ReRunSupplierImporrtFile_Id = File_Id,
                                    ReRunBatch = Batch,
                                    Status = a.Status
                                }).ToList();
                }

                insertSTGList = stg.Where(w => !toUpdate.Any(a => a.stg_AccoMapping_Id == w.stg_AccoMapping_Id)).ToList();
                updateMappingList = toUpdate;

                context.Dispose();
            }
        }

        public bool UpdateHotelMappingStatus(DC_MappingMatch obj)
        {
            bool retrn = false;
            DataContracts.Masters.DC_Supplier supdata = new DataContracts.Masters.DC_Supplier();
            supdata = obj.SupplierDetail;
            string configWhere = string.Empty;
            string curSupplier = string.Empty;
            Guid? curSupplier_Id = Guid.Empty;
            Guid File_Id = new Guid();
            File_Id = Guid.Parse(obj.File_Id.ToString());
            int totPriorities = obj.TotalPriorities;
            int curPriority = obj.CurrentPriority;
            int totConfigs = 0;
            int curConfig = 0;
            bool DontAppend = false;
            bool isCityMapJoins = false;
            int curConfigCount = 0;
            if (totPriorities <= 0)
                totPriorities = 1;
            int PerForEachPriority = 60 / totPriorities;

            if (supdata != null)
            {
                curSupplier = supdata.Name;
                curSupplier_Id = supdata.Supplier_Id;
            }

            try
            {
                PLog.SupplierImportFileProgress_Id = Guid.NewGuid();
                PLog.SupplierImportFile_Id = obj.File_Id;
                PLog.Step = "MATCH";
                PLog.Status = "MATCHING";
                PLog.TotalBatch = totPriorities;

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    int setunmap = 0;
                    string setUNMAPPED = "UPDATE APM ";
                    setUNMAPPED = setUNMAPPED + " SET Accommodation_Id = null, Status = 'UNMAPPED', MatchedBy = null, MatchedByString = null ";
                    setUNMAPPED = setUNMAPPED + " , Edit_Date = GETDATE(), Edit_User = 'TLGX_DataHandler' ";
                    setUNMAPPED = setUNMAPPED + " FROM Accommodation_ProductMapping APM ";
                    //setUNMAPPED = setUNMAPPED + " inner join STG_Mapping_TableIds S ON APM.Accommodation_ProductMapping_Id = S.Mapping_Id AND S.File_Id = '" + obj.File_Id.ToString() + "' ";
                    setUNMAPPED = setUNMAPPED + " WHERE ISNULL(APM.Status, '') = 'REVIEW' ";
                    setUNMAPPED = setUNMAPPED + " and APM.ReRun_SupplierImportFile_Id = '" + obj.File_Id.ToString() + "' ";
                    setUNMAPPED = setUNMAPPED + " and APM.ReRun_Batch = " + (obj.CurrentBatch).ToString();
                    try { setunmap = context.Database.ExecuteSqlCommand(setUNMAPPED); } catch (Exception ex) { }
                }

                foreach (int priority in obj.Priorities)
                {
                    //Get Priority Matching Columns
                    var curAttributeVals = obj.lstConfigs.Where(a => a.Priority == priority).ToList();
                    curConfigCount = curAttributeVals.Count();
                    List<DC_SupplierImportAttributeValues> configs = curAttributeVals;

                    PLog.CurrentBatch = curPriority;

                    string MatchByString = string.Empty;

                    //Get Matching Status from Master Attributes
                    string MatchingStatus = string.Empty;

                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        MatchingStatus = (from MA in context.m_masterattribute
                                          join MAV in context.m_masterattributevalue on MA.MasterAttribute_Id equals MAV.MasterAttribute_Id
                                          join PMAV in context.m_masterattributevalue on MAV.ParentAttributeValue_Id equals PMAV.MasterAttributeValue_Id
                                          where MA.Name == "MatchingPriority" && MAV.AttributeValue == priority.ToString()
                                          select PMAV.AttributeValue).FirstOrDefault();
                    }

                    if (string.IsNullOrWhiteSpace(MatchingStatus))
                    {
                        MatchingStatus = "REVIEW";
                    }

                    totConfigs = configs.Count;

                    curConfig = 0;
                    configWhere = "";
                    bool bIsGeoLookUp = false;
                    bool bIsFullIndexCheck = false;
                    int HotelRank = 0;
                    int AddressRank = 0;
                    int GeoDistance = 0;

                    foreach (DC_SupplierImportAttributeValues config in curAttributeVals)
                    {
                        if ((config.AttributeValueType ?? string.Empty) == "VALUE")
                        {
                            configWhere = configWhere + " " + config.AttributeName.Replace("Accommodation_ProductMapping.", "").Trim() + ",";
                        }
                        else
                        {
                            if (config.AttributeValue.Replace("Accommodation.", "").Trim() != "---ALL---")
                                configWhere = configWhere + " " + config.AttributeValue.Replace("Accommodation.", "").Trim() + ",";
                            else
                                configWhere = configWhere + " " + config.AttributeName.Replace("Accommodation_ProductMapping.", "").Trim() + ",";
                        }

                    }

                    configWhere = configWhere.Remove(configWhere.Length - 1);

                    CallLogVerbose(File_Id, "MATCH", "Matching Combination " + priority.ToString() + " consist of Match by " + configWhere, obj.CurrentBatch);

                    string PriorityJoins = string.Empty;
                    string PriorityJoinsMaster = string.Empty;
                    string CityMapJoins = string.Empty;
                    string MatchByStringAppend = string.Empty;
                    string WhereClause = string.Empty;
                    string UpdateClause = string.Empty;

                    foreach (DC_SupplierImportAttributeValues config in curAttributeVals)
                    {
                        if (string.IsNullOrWhiteSpace(MatchByString) && !string.IsNullOrWhiteSpace(config.Description))
                        {
                            MatchByString = config.Description;
                        }

                        DontAppend = false;
                        curConfig = curConfig + 1;
                        string CurrConfig = string.Empty;

                        if ((config.AttributeValueType ?? string.Empty) == "VALUE")
                        {
                            CurrConfig = config.AttributeName.Replace("Accommodation_ProductMapping.", "").Trim().ToUpper();
                        }
                        else
                        {
                            if (config.AttributeValue.Replace("Accommodation.", "").Trim() != "---ALL---")
                                CurrConfig = config.AttributeValue.Replace("Accommodation.", "").Trim().ToUpper();
                            else
                                CurrConfig = config.AttributeName.Replace("Accommodation_ProductMapping.", "").Trim().ToUpper();
                        }

                        if (CurrConfig == "COUNTRY")
                        {
                            PriorityJoins = PriorityJoins + " (ISNULL(A.country, '') != '' and ISNULL(ISNULL(mc.CountryName, cm.CountryName), '') != '' and ltrim(rtrim(upper(A.country))) = ltrim(rtrim(upper(ISNULL(mc.CountryName, cm.CountryName)))) ) ";
                            PriorityJoinsMaster = PriorityJoinsMaster + " (ISNULL(A.country, '') != '' and ISNULL(APM.CountryName, '') != '' and ltrim(rtrim(upper(A.country))) = ltrim(rtrim(upper(APM.CountryName))) ) ";

                            isCityMapJoins = true;
                        }
                        else if (CurrConfig == "CITY")
                        {
                            PriorityJoins = PriorityJoins + " (ISNULL(A.city, '') != '' and ISNULL(ISNULL(mc.Name, cm.CityName), '') != '' and ltrim(rtrim(upper(A.city))) = ltrim(rtrim(upper(ISNULL(mc.Name, cm.CityName)))) ) ";
                            PriorityJoinsMaster = PriorityJoinsMaster + " (ISNULL(A.city, '') != '' and ISNULL(APM.CityName, '') != '' and ltrim(rtrim(upper(A.city))) = ltrim(rtrim(upper(APM.CityName))) ) ";

                            isCityMapJoins = true;

                        }
                        else if (CurrConfig == "PostalCode".ToUpper())
                        {
                            PriorityJoins = PriorityJoins + " (ISNULL(A.PostalCode, '') != '' and ISNULL(APM.PostCode, '') != '' and ltrim(rtrim(upper(A.PostalCode))) = ltrim(rtrim(upper(APM.PostCode))) ) ";
                            PriorityJoinsMaster = PriorityJoinsMaster + " (ISNULL(A.PostalCode, '') != '' and ISNULL(APM.PostCode, '') != '' and ltrim(rtrim(upper(A.PostalCode))) = ltrim(rtrim(upper(APM.PostCode))) ) ";
                            isCityMapJoins = true;
                        }
                        else if (CurrConfig == "HotelName".ToUpper())
                        {
                            /*
                                PriorityJoins = PriorityJoins + " (ISNULL(A.HotelName, '') != '' and ISNULL(APM.ProductName, '') != '' ";
                                PriorityJoins = PriorityJoins + " and replace(replace(replace(replace(replace(replace(replace(replace(replace(replace(replace(ltrim(rtrim(upper(A.hotelname))), ' ',''), 'HOTEL',''), 'APARTMENT',''), replace(ltrim(rtrim(upper(A.city))), '''','') ,''),  replace(ltrim(rtrim(upper(A.country))), '''',''),''), '&',''), 'AND',''), 'THE',''), '-',''), '_',''), '''','') = ";
                                PriorityJoins = PriorityJoins + " replace(replace(replace(replace(replace(replace(replace(replace(replace(replace(replace(ltrim(rtrim(upper(APM.ProductName))), ' ',''), 'HOTEL',''), 'APARTMENT',''), replace(ltrim(rtrim(upper(ISNULL(mc.Name, cm.CityName)))), '''','') ,''),  replace(ltrim(rtrim(upper(ISNULL(mc.CountryName, cm.CountryName)))), '''',''),''), '&',''), 'AND',''), 'THE',''), '-',''), '_',''), '''','')  ) ";

                                PriorityJoinsMaster = PriorityJoinsMaster + " (ISNULL(A.HotelName, '') != '' and ISNULL(APM.ProductName, '') != '' ";
                                PriorityJoinsMaster = PriorityJoinsMaster + " and replace(replace(replace(replace(replace(replace(replace(replace(replace(replace(replace(ltrim(rtrim(upper(A.hotelname))), ' ',''), 'HOTEL',''), 'APARTMENT',''), replace(ltrim(rtrim(upper(A.city))), '''','') ,''),  replace(ltrim(rtrim(upper(A.country))), '''',''),''), '&',''), 'AND',''), 'THE',''), '-',''), '_',''), '''','') = ";
                                PriorityJoinsMaster = PriorityJoinsMaster + " replace(replace(replace(replace(replace(replace(replace(replace(replace(replace(replace(ltrim(rtrim(upper(APM.ProductName))), ' ',''), 'HOTEL',''), 'APARTMENT',''), replace(ltrim(rtrim(upper(APM.CityName))), '''','') ,''),  replace(ltrim(rtrim(upper(APM.CountryName))), '''',''),''), '&',''), 'AND',''), 'THE',''), '-',''), '_',''), '''','')  ) ";
                            */

                            //Not Required
                            //PriorityJoins = PriorityJoins + " (ISNULL(A.HotelName, '') != '' and ISNULL(APM.ProductName, '') != '' ";
                            PriorityJoins = PriorityJoins + " A.HotelName = APM.ProductName ";

                            //Not Required
                            //PriorityJoinsMaster = PriorityJoinsMaster + " (ISNULL(A.HotelName, '') != '' and ISNULL(APM.ProductName, '') != '' ";
                            PriorityJoinsMaster = PriorityJoinsMaster + " A.HotelName = APM.ProductName ";
                        }
                        else if (CurrConfig == "HotelName_Tx".ToUpper())
                        {
                            //Not Required
                            //PriorityJoins = PriorityJoins + " (ISNULL(A.HotelName_Tx, '') != '' and ISNULL(APM.HotelName_Tx, '') != '' ";
                            PriorityJoins = PriorityJoins + " A.HotelName_Tx = APM.HotelName_Tx ";

                            //Not Required
                            //PriorityJoinsMaster = PriorityJoinsMaster + " (ISNULL(A.HotelName_Tx, '') != '' and ISNULL(APM.HotelName_Tx, '') != '' ";
                            PriorityJoinsMaster = PriorityJoinsMaster + " A.HotelName_Tx = APM.HotelName_Tx ";
                        }
                        else if (CurrConfig == "LONGITUDE" || CurrConfig == "LONGITUDE_TX")
                        {
                            DontAppend = true;
                        }
                        else if (CurrConfig == "LATITUDE" || CurrConfig == "LATITUDE_TX")
                        {
                            DontAppend = false;
                            PriorityJoins = PriorityJoins + " (ISNULL(A.Latitude_Tx, '') != '' and ISNULL(APM.Latitude_Tx, '') != '' ";
                            PriorityJoins = PriorityJoins + " and ISNULL(A.Longitude_Tx, '') != '' and ISNULL(APM.Longitude_Tx, '') != '' ";
                            PriorityJoins = PriorityJoins + " and ltrim(rtrim(upper(A.Latitude_Tx))) = ltrim(rtrim(upper(APM.Latitude_Tx))) ";
                            PriorityJoins = PriorityJoins + " and ltrim(rtrim(upper(A.Longitude_Tx))) = ltrim(rtrim(upper(APM.Longitude_Tx))) ) ";

                            PriorityJoinsMaster = PriorityJoinsMaster + " (ISNULL(A.Latitude_Tx, '') != '' and ISNULL(APM.Latitude_Tx, '') != '' ";
                            PriorityJoinsMaster = PriorityJoinsMaster + " and ISNULL(A.Longitude_Tx, '') != '' and ISNULL(APM.Longitude_Tx, '') != '' ";
                            PriorityJoinsMaster = PriorityJoinsMaster + " and ltrim(rtrim(upper(A.Latitude_Tx))) = ltrim(rtrim(upper(APM.Latitude_Tx))) ";
                            PriorityJoinsMaster = PriorityJoinsMaster + " and ltrim(rtrim(upper(A.Longitude_Tx))) = ltrim(rtrim(upper(APM.Longitude_Tx))) ) ";
                        }
                        else if (CurrConfig == "GOOGLE_PLACE_ID")
                        {
                            PriorityJoins = PriorityJoins + " (ISNULL(A.GOOGLE_PLACE_ID, '') != '' and ISNULL(APM.GOOGLE_PLACE_ID, '') != '' and ltrim(rtrim(upper(A.GOOGLE_PLACE_ID))) = ltrim(rtrim(upper(APM.GOOGLE_PLACE_ID))) ) ";

                            PriorityJoinsMaster = PriorityJoinsMaster + " (ISNULL(A.GOOGLE_PLACE_ID, '') != '' and ISNULL(APM.GOOGLE_PLACE_ID, '') != '' and A.GOOGLE_PLACE_ID = APM.GOOGLE_PLACE_ID ) ";
                        }
                        else if (CurrConfig == "Address_Tx".ToUpper())
                        {
                            PriorityJoins = PriorityJoins + " (ISNULL(A.Address_Tx, '') != '' and ISNULL(APM.Address_Tx, '') != '' ";
                            PriorityJoins = PriorityJoins + " and A.Address_Tx = APM.Address_Tx ) ";

                            PriorityJoinsMaster = PriorityJoinsMaster + " (ISNULL(A.Address_Tx, '') != '' and ISNULL(APM.Address_Tx, '') != '' ";
                            PriorityJoinsMaster = PriorityJoinsMaster + " and A.Address_Tx = APM.Address_Tx ) ";

                            isCityMapJoins = true;
                        }
                        else if (CurrConfig == "TelephoneNumber_tx".ToUpper() || CurrConfig == "Telephone_tx".ToUpper())
                        {
                            PriorityJoins = PriorityJoins + " (ISNULL(A.Telephone_Tx, '') != '' and ISNULL(APM.TelephoneNumber_tx, '') != '' ";
                            PriorityJoins = PriorityJoins + " and A.Telephone_Tx = APM.TelephoneNumber_tx ) ";

                            PriorityJoinsMaster = PriorityJoinsMaster + " (ISNULL(A.Telephone_Tx, '') != '' and ISNULL(APM.TelephoneNumber_tx, '') != '' ";
                            PriorityJoinsMaster = PriorityJoinsMaster + " and A.Telephone_Tx = APM.TelephoneNumber_tx ) ";

                            isCityMapJoins = true;
                        }
                        else if (CurrConfig == "CompanyHotelID".ToUpper())
                        {
                            PriorityJoins = PriorityJoins + " (ISNULL(A.CompanyHotelID, '') != '' and ISNULL(APM.SupplierProductReference, '') != '' ";
                            PriorityJoins = PriorityJoins + " and ltrim(rtrim(upper(A.CompanyHotelID))) = ltrim(rtrim(upper(APM.SupplierProductReference))) ) ";

                            PriorityJoinsMaster = PriorityJoinsMaster + " (ISNULL(A.CompanyHotelID, '') != '' and ISNULL(APM.SupplierProductReference, '') != '' ";
                            PriorityJoinsMaster = PriorityJoinsMaster + " and ltrim(rtrim(upper(A.CompanyHotelID))) = ltrim(rtrim(upper(APM.SupplierProductReference))) ) ";
                        }
                        else if (CurrConfig == "GeoLocation".ToUpper())
                        {
                            bIsGeoLookUp = true;
                        }
                        else if (CurrConfig == "HOTELNAME_RANK" && (config.AttributeValueType ?? string.Empty) == "VALUE")
                        {
                            bIsFullIndexCheck = true;
                            PriorityJoins = PriorityJoins + " A.HotelName_Rank " + config.Comparison + " " + config.AttributeValue + " ";
                            PriorityJoinsMaster = PriorityJoinsMaster + " A.HotelName_Rank " + config.Comparison + " " + config.AttributeValue + " ";
                            MatchByStringAppend = MatchByStringAppend + "HR" + config.Comparison.Trim() + config.AttributeValue.Trim() + ",";
                            int.TryParse(config.AttributeValue, out HotelRank);
                        }
                        else if (CurrConfig == "ADDRESS_RANK" && (config.AttributeValueType ?? string.Empty) == "VALUE")
                        {
                            bIsFullIndexCheck = true;
                            PriorityJoins = PriorityJoins + " A.Address_Rank " + config.Comparison + " " + config.AttributeValue + " ";
                            PriorityJoinsMaster = PriorityJoinsMaster + " A.Address_Rank " + config.Comparison + " " + config.AttributeValue + " ";
                            MatchByStringAppend = MatchByStringAppend + "AR" + config.Comparison.Trim() + config.AttributeValue.Trim() + ",";
                            int.TryParse(config.AttributeValue, out AddressRank);
                        }
                        else if (CurrConfig == "GEOLOCATION_DISTANCE" && (config.AttributeValueType ?? string.Empty) == "VALUE")
                        {
                            bIsGeoLookUp = true;
                            PriorityJoins = PriorityJoins + " A.Geolocation_Distance " + config.Comparison + " " + config.AttributeValue + " ";
                            PriorityJoinsMaster = PriorityJoinsMaster + " A.Geolocation_Distance " + config.Comparison + " " + config.AttributeValue + " ";
                            MatchByStringAppend = MatchByStringAppend + "GD" + config.Comparison.Trim() + config.AttributeValue.Trim() + ",";
                            int.TryParse(config.AttributeValue, out GeoDistance);
                        }

                        if (curConfig != curConfigCount && (!DontAppend))
                        {
                            PriorityJoins = PriorityJoins + " AND ";
                            PriorityJoinsMaster = PriorityJoinsMaster + " AND ";
                        }
                    }

                    UpdateClause = UpdateClause + "UPDATE APM ";
                    UpdateClause = UpdateClause + " SET Accommodation_Id = A.Accommodation_Id, STATUS = '" + MatchingStatus + "', MatchedBy = " + priority.ToString() + " ";
                    UpdateClause = UpdateClause + " , MatchedByString = '" + MatchByString.ToString() + "' ";
                    UpdateClause = UpdateClause + " , Country_Id = ISNULL(A.Country_Id, APM.Country_Id), City_Id = ISNULL(A.City_Id, APM.City_Id) ";
                    UpdateClause = UpdateClause + " , Edit_Date = GETDATE(), Edit_User = 'TLGX_DataHandler' ";
                    UpdateClause = UpdateClause + " FROM Accommodation_ProductMapping APM ";

                    WhereClause = WhereClause + " WHERE APM.STATUS = 'UNMAPPED' AND ISNULL(A.IsActive, 0) = 1 ";
                    WhereClause = WhereClause + " AND APM.ReRun_Batch = " + (obj.CurrentBatch).ToString();
                    WhereClause = WhereClause + " AND APM.ReRun_SupplierImportFile_Id =  '" + obj.File_Id.ToString() + "';";

                    if (isCityMapJoins)
                    {
                        CityMapJoins = CityMapJoins + " inner join m_CityMapping cm on cm.supplier_Id = APM.Supplier_Id AND #PutJoinConditionHere# ";
                        CityMapJoins = CityMapJoins + " left outer join m_CityMaster mc on cm.City_Id = mc.City_Id ";
                    }

                    if (string.IsNullOrWhiteSpace(MatchByString))
                    {
                        MatchByString = "Not Defined";
                    }

                    MatchByStringAppend = MatchByStringAppend.Trim().TrimEnd(',').Trim();

                    string sqlFull = "";
                    int toupdate = 0;

                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        context.Database.CommandTimeout = 0;

                        if (bIsGeoLookUp && !bIsFullIndexCheck)
                        {
                            string LookUpToDistanceInMeters = System.Configuration.ConfigurationManager.AppSettings["HotelLookUpToDistanceInMeters"].ToString();

                            string HotelLookUpSQL = "DECLARE @TABLE TABLE (APM_ID UNIQUEIDENTIFIER, A_ID UNIQUEIDENTIFIER) ";
                            HotelLookUpSQL = HotelLookUpSQL + ";WITH APM (Accommodation_Product_Mapping_Id, GeoLocation, Country_Id, ProductName, HotelName_Tx)  AS ";
                            HotelLookUpSQL = HotelLookUpSQL + "(Select Accommodation_ProductMapping_Id, GeoLocation, Country_Id, ProductName, HotelName_Tx from Accommodation_ProductMapping WHERE GeoLocation IS NOT NULL ";
                            HotelLookUpSQL = HotelLookUpSQL + "and ReRun_SupplierImportFile_Id = '" + obj.File_Id.ToString() + "' ";
                            HotelLookUpSQL = HotelLookUpSQL + "and ReRun_Batch = " + (obj.CurrentBatch).ToString() + " ";
                            HotelLookUpSQL = HotelLookUpSQL + "and STATUS = 'UNMAPPED'";
                            HotelLookUpSQL = HotelLookUpSQL + ") ";
                            HotelLookUpSQL = HotelLookUpSQL + "INSERT INTO @TABLE SELECT APM.Accommodation_Product_Mapping_Id, ";
                            HotelLookUpSQL = HotelLookUpSQL + "(SELECT TOP 1 A.Accommodation_Id from Accommodation A WITH (NOLOCK) ";
                            HotelLookUpSQL = HotelLookUpSQL + "where A.Country_Id = APM.Country_Id ";
                            HotelLookUpSQL = HotelLookUpSQL + PriorityJoins;
                            HotelLookUpSQL = HotelLookUpSQL + "AND APM.GeoLocation.STDistance(A.GeoLocation) <= " + LookUpToDistanceInMeters + " ";
                            HotelLookUpSQL = HotelLookUpSQL + "ORDER BY APM.GeoLocation.STDistance(A.GeoLocation)) AS ACCOPRODNAME FROM APM; ";

                            HotelLookUpSQL = HotelLookUpSQL + "UPDATE APM SET APM.Accommodation_Id = TBL.A_ID, APM.MatchedBy = " + priority.ToString() + ", ";
                            HotelLookUpSQL = HotelLookUpSQL + "APM.MatchedByString = '" + MatchByString.ToString() + "', APM.Status = '" + MatchingStatus + "', ";
                            HotelLookUpSQL = HotelLookUpSQL + "Edit_Date = GETDATE(), Edit_User = 'TLGX_DataHandler', APM.Country_Id = A.Country_Id, APM.City_Id = A.City_Id, APM.Legacy_Htl_ID = A.CompanyHotelID ";
                            HotelLookUpSQL = HotelLookUpSQL + "FROM @TABLE TBL ";
                            HotelLookUpSQL = HotelLookUpSQL + "INNER JOIN Accommodation_ProductMapping APM ON TBL.APM_ID = APM.Accommodation_ProductMapping_Id ";
                            HotelLookUpSQL = HotelLookUpSQL + "INNER JOIN Accommodation A ON TBL.A_ID = A.Accommodation_Id AND ISNULL(A.IsActive, 0) = 1 ; ";

                            try
                            {
                                context.Database.ExecuteSqlCommand(HotelLookUpSQL);
                            }
                            catch (Exception ex)
                            {
                                CallLogVerbose(File_Id, MatchByString, ex.Message, obj.CurrentBatch);
                            }
                        }
                        else if (bIsFullIndexCheck && bIsGeoLookUp)
                        {
                            //StringBuilder sqlFullIndexGeoSpatial = new StringBuilder();
                            //sqlFullIndexGeoSpatial.Append(@"DECLARE @Results TABLE
                            //                                (
                            //                                  APM_ID				UNIQUEIDENTIFIER,
                            //                                  SUP_HTL_NME			NVARCHAR(500),
                            //                                  SUP_CON_ID			UNIQUEIDENTIFIER,
                            //                                  SUP_ADR				NVARCHAR(1000),
                            //                                  SUP_LOC				GEOGRAPHY,
                            //                                  HOTELNAME_RANK		INT,
                            //                                  ADDRESS_RANK			INT,
                            //                                  ACCO_ID				UNIQUEIDENTIFIER,
                            //                                  GEOLOCATION_DISTANCE	FLOAT
                            //                                )

                            //                                INSERT INTO @Results (APM_ID, SUP_HTL_NME, SUP_ADR, SUP_LOC, SUP_CON_ID)
                            //                                SELECT Accommodation_ProductMapping_Id, ProductName, Address, GeoLocation, Country_Id
                            //                                FROM Accommodation_ProductMapping WITH (NOLOCK) WHERE Status = 'UNMAPPED' ");
                            //sqlFullIndexGeoSpatial.AppendLine();
                            //sqlFullIndexGeoSpatial.Append("AND ReRun_SupplierImportFile_Id = '" + obj.File_Id.ToString() + "' ");
                            //sqlFullIndexGeoSpatial.AppendLine();
                            //sqlFullIndexGeoSpatial.Append("AND ReRun_Batch = " + obj.CurrentBatch.ToString() + " ");
                            //sqlFullIndexGeoSpatial.AppendLine();
                            //sqlFullIndexGeoSpatial.Append(@"DECLARE
                            //                                  @AccoMap_Id as UNIQUEIDENTIFIER
                            //                                , @HotelName AS NVARCHAR(1000)
                            //                                , @HotelNameTx AS NVARCHAR(1000)
                            //                                , @FullAddress AS NVARCHAR(1000)
                            //                                , @Country_Id as UNIQUEIDENTIFIER
                            //                                , @FromPoint geography
                            //                                , @HTL_RANK INT
                            //                                , @ADR_RANK INT
                            //                                , @ACCO_ID UNIQUEIDENTIFIER
                            //                                , @DIST FLOAT

                            //                                DECLARE db_cursor CURSOR FOR
                            //                                SELECT APM_ID, SUP_HTL_NME, SUP_ADR, SUP_LOC, SUP_CON_ID FROM @Results

                            //                                OPEN db_cursor
                            //                                FETCH NEXT FROM db_cursor INTO @AccoMap_Id, @HotelName, @FullAddress, @FromPoint, @Country_Id

                            //                                WHILE @@FETCH_STATUS = 0
                            //                                BEGIN

                            //                                    BEGIN TRY

                            //                                        SELECT @ACCO_ID = NULL, @HTL_RANK = NULL, @ADR_RANK = NULL, @DIST = NULL

                            //                                        SELECT TOP 1
                            //                                         @ACCO_ID = FT_TBL.Accommodation_Id
                            //                                        , @HTL_RANK = KEY_TBL.RANK
                            //                                        , @ADR_RANK = KEY_TBL_ADDR.RANK
                            //                                        , @DIST = @FromPoint.STDistance(FT_TBL.GeoLocation)
                            //                                        FROM Accommodation FT_TBL WITH (NOLOCK) 
                            //                                                INNER JOIN FREETEXTTABLE(Accommodation, HotelName, @HotelName) AS KEY_TBL ON FT_TBL.Accommodation_Id = KEY_TBL.[KEY]
                            //                                                INNER JOIN FREETEXTTABLE(Accommodation, FullAddress, @FullAddress) AS KEY_TBL_ADDR ON FT_TBL.Accommodation_Id = KEY_TBL_ADDR.[KEY]
                            //                                        WHERE FT_TBL.Country_Id = @Country_Id AND ISNULL(FT_TBL.IsActive, 0) = 1

                            //                                        ORDER BY

                            //                                        KEY_TBL.RANK DESC,
                            //                                        KEY_TBL_ADDR.RANK DESC,
                            //                                        @FromPoint.STDistance(FT_TBL.GeoLocation)

                            //                                        UPDATE @Results SET

                            //                                        HOTELNAME_RANK = @HTL_RANK,
                            //                                        ADDRESS_RANK = @ADR_RANK,
                            //                                        ACCO_ID = @ACCO_ID,
                            //                                        GEOLOCATION_DISTANCE = @DIST

                            //                                        WHERE APM_ID = @AccoMap_Id

                            //                                    END TRY

                            //                                    BEGIN CATCH

                            //                                    END CATCH;

                            //                                    FETCH NEXT FROM db_cursor INTO @AccoMap_Id, @HotelName, @FullAddress, @FromPoint, @Country_Id
                            //                                END

                            //                                CLOSE db_cursor
                            //                                DEALLOCATE db_cursor ");
                            //sqlFullIndexGeoSpatial.AppendLine();
                            //sqlFullIndexGeoSpatial.Append("UPDATE APM SET ");
                            //sqlFullIndexGeoSpatial.Append("APM.MatchedBy = " + priority.ToString() + " ");
                            //sqlFullIndexGeoSpatial.Append(", APM.Accommodation_Id = A.ACCO_ID ");
                            //sqlFullIndexGeoSpatial.Append(", APM.Status = '" + MatchingStatus + "' ");
                            //sqlFullIndexGeoSpatial.Append(", APM.MatchedByString = '" + MatchByString.ToString() + " + MCON(" + MatchByStringAppend + ")' ");
                            //sqlFullIndexGeoSpatial.Append("+ ' + VALUE(HR:' + CAST(A.HOTELNAME_RANK AS varchar) + ',AR:' + CAST(A.ADDRESS_RANK AS varchar) + ',GD:' + CAST(CAST(A.GEOLOCATION_DISTANCE as decimal(18,2)) AS varchar) + ')' ");
                            //sqlFullIndexGeoSpatial.Append(", APM.Edit_Date = GETDATE() ");
                            //sqlFullIndexGeoSpatial.Append(", APM.Edit_User = 'TLGX_DataHandler' ");
                            //sqlFullIndexGeoSpatial.Append("FROM Accommodation_ProductMapping APM ");
                            //sqlFullIndexGeoSpatial.Append("INNER JOIN @Results A ON APM.Accommodation_ProductMapping_Id = A.APM_ID ");
                            //sqlFullIndexGeoSpatial.Append("WHERE A.ACCO_ID IS NOT NULL ");

                            //if (!string.IsNullOrWhiteSpace(PriorityJoins))
                            //{
                            //    sqlFullIndexGeoSpatial.Append(" AND " + PriorityJoins);
                            //}

                            try
                            {
                                var totalRecordsEffected = context.sp_AccoFullTextSpatialMatch(obj.File_Id, obj.CurrentBatch, priority, MatchByString, MatchByStringAppend, MatchingStatus, HotelRank, AddressRank, GeoDistance, "TLGX_DataDandler");
                                //context.Database.ExecuteSqlCommand(sqlFullIndexGeoSpatial.ToString());
                            }
                            catch (Exception ex)
                            {
                                CallLogVerbose(File_Id, MatchByString, ex.Message, obj.CurrentBatch);
                            }

                        }
                        else
                        {
                            sqlFull = string.Empty;
                            sqlFull = sqlFull + UpdateClause;
                            sqlFull = sqlFull + CityMapJoins;
                            sqlFull = sqlFull + " inner join Accommodation A ON ";
                            sqlFull = sqlFull + PriorityJoins;
                            sqlFull = sqlFull + WhereClause;

                            if (string.IsNullOrWhiteSpace(CityMapJoins))
                            {
                                try
                                {
                                    context.Database.ExecuteSqlCommand(sqlFull);
                                }
                                catch (Exception ex)
                                {
                                    CallLogVerbose(File_Id, MatchByString, ex.Message, obj.CurrentBatch);
                                }
                            }
                            else
                            {
                                try { context.Database.ExecuteSqlCommand(sqlFull.Replace("#PutJoinConditionHere#", "(cm.CityCode = APM.CityCode)")); } catch (Exception ex) { CallLogVerbose(File_Id, MatchByString, ex.Message, obj.CurrentBatch); }
                                try { context.Database.ExecuteSqlCommand(sqlFull.Replace("#PutJoinConditionHere#", "(cm.cityname = APM.cityname and cm.CountryCode = APM.CountryCode)")); } catch (Exception ex) { CallLogVerbose(File_Id, MatchByString, ex.Message, obj.CurrentBatch); }
                                try { context.Database.ExecuteSqlCommand(sqlFull.Replace("#PutJoinConditionHere#", "(cm.cityname = APM.cityname and cm.CountryName = APM.CountryName)")); } catch (Exception ex) { CallLogVerbose(File_Id, MatchByString, ex.Message, obj.CurrentBatch); }
                            }

                            if (!string.IsNullOrWhiteSpace(CityMapJoins) && obj.Match_Direct_Master)
                            {
                                sqlFull = string.Empty;
                                sqlFull = sqlFull + UpdateClause;
                                sqlFull = sqlFull + " inner join Accommodation A ON ";
                                sqlFull = sqlFull + PriorityJoinsMaster;
                                sqlFull = sqlFull + WhereClause;

                                try
                                {
                                    context.Database.ExecuteSqlCommand(sqlFull);
                                }
                                catch (Exception ex)
                                {
                                    CallLogVerbose(File_Id, MatchByString, ex.Message, obj.CurrentBatch);
                                }
                            }

                        }
                    }

                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        context.Database.CommandTimeout = 0;

                        string sqlUpdatedRecords = "SELECT COUNT(*) FROM Accommodation_ProductMapping WITH (NOLOCK) WHERE ";
                        sqlUpdatedRecords = sqlUpdatedRecords + "ReRun_SupplierImportFile_Id = '" + obj.File_Id.ToString() + "' ";
                        sqlUpdatedRecords = sqlUpdatedRecords + "AND ReRun_Batch = " + (obj.CurrentBatch).ToString() + " ";
                        sqlUpdatedRecords = sqlUpdatedRecords + "AND MatchedBy = " + priority.ToString() + ";";

                        try
                        {
                            toupdate = context.Database.SqlQuery<int>(sqlUpdatedRecords).Single();
                        }
                        catch (Exception ex)
                        {
                            CallLogVerbose(File_Id, MatchByString, ex.Message, obj.CurrentBatch);
                        }
                    }

                    CallLogVerbose(File_Id, "MATCH", toupdate.ToString() + " Matches Found for Combination " + priority.ToString() + ".", obj.CurrentBatch);

                    if ((obj.FileMode ?? "ALL") == "ALL" && totPriorities == curPriority)
                    {
                        DataContracts.UploadStaticData.DC_SupplierImportFile_Statistics objStat = new DC_SupplierImportFile_Statistics();
                        objStat.SupplierImportFile_Statistics_Id = Guid.NewGuid();
                        objStat.SupplierImportFile_Id = obj.File_Id;
                        objStat.From = "MATCHING";
                        DataContracts.DC_Message stat = USD.AddStaticDataUploadStatistics(objStat);
                    }

                    if (curPriority == totPriorities)
                    {
                        PLog.PercentageValue = 100;
                        USD.AddStaticDataUploadProcessLog(PLog);
                    }

                    curPriority = curPriority + 1;

                    retrn = true;
                }

                return retrn;
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating hotel mapping" + Environment.NewLine + e.Message, ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool UpdateHotelMappingStatusDirectMaster(DC_MappingMatch obj)
        {
            bool retrn = false;
            DataContracts.Masters.DC_Supplier supdata = new DataContracts.Masters.DC_Supplier();
            supdata = obj.SupplierDetail;
            string configWhere = "";
            string curSupplier = "";
            Guid? curSupplier_Id = Guid.Empty;
            configWhere = "";
            Guid File_Id = new Guid();
            File_Id = Guid.Parse(obj.File_Id.ToString());
            int totPriorities = obj.TotalPriorities;
            int curPriority = obj.CurrentPriority;
            int totConfigs = 0;
            int curConfig = 0;
            if (totPriorities <= 0)
                totPriorities = 1;
            int PerForEachPriority = 60 / totPriorities;
            bool Match_Direct_Master = obj.Match_Direct_Master;
            IQueryable<Accommodation_ProductMapping> prodMapSearch;
            List<DC_Accomodation_ProductMapping> res = new List<DC_Accomodation_ProductMapping>();

            if (supdata != null)
            {
                curSupplier = supdata.Name;
                curSupplier_Id = supdata.Supplier_Id;
            }
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    #region "Acco mapping query"
                    prodMapSearch = (from a in context.Accommodation_ProductMapping.AsNoTracking()
                                     join s in context.STG_Mapping_TableIds.AsNoTracking() on a.Accommodation_ProductMapping_Id equals s.Mapping_Id
                                     where s.File_Id == supdata.File_Id && a.Accommodation_Id == null && a.Supplier_Id == curSupplier_Id && s.Batch == obj.CurrentBatch
                                     && a.Status.Trim().ToUpper() == "UNMAPPED"
                                     select a);

                    res = (from a in prodMapSearch
                           join mact in context.m_CityMaster.AsNoTracking() on new { country = (a.Country_Id ?? Guid.Empty) }
                           equals new { country = mact.Country_Id } into jact
                           from jdact in jact.DefaultIfEmpty()
                           where ((a.City_Id != null && (a.City_Id == jdact.City_Id)) || (a.City_Id == null && (a.CityName.ToUpper().Trim() == jdact.Name.ToUpper().Trim())))
                           select new DataContracts.Mapping.DC_Accomodation_ProductMapping
                           {
                               Accommodation_ProductMapping_Id = a.Accommodation_ProductMapping_Id,
                               Accommodation_Id = a.Accommodation_Id,
                               Address_tx = a.address_tx,
                               CityCode = a.CityCode,
                               CityName = a.CityName,
                               CountryCode = a.CountryCode,
                               CountryName = a.CountryName,
                               Create_Date = a.Create_Date,
                               Create_User = a.Create_User,
                               Edit_Date = a.Edit_Date,
                               Edit_User = a.Edit_User,
                               Email = a.Email,
                               Fax = a.Fax,
                               Latitude = a.Latitude,
                               Longitude = a.Longitude,
                               IsActive = (a.IsActive ?? true),
                               MapId = a.MapId,
                               PostCode = a.PostCode,
                               ProductName = a.ProductName,
                               Remarks = a.Remarks,
                               StarRating = a.StarRating,
                               StateCode = a.StateCode,
                               StateName = a.StateName,
                               Status = a.Status,
                               Street = a.Street,
                               Street2 = a.Street2,
                               Street3 = a.Street3,
                               Street4 = a.Street4,
                               SupplierId = a.SupplierId,
                               SupplierName = a.SupplierName,
                               SupplierProductReference = a.SupplierProductReference,
                               TelephoneNumber = a.TelephoneNumber,
                               TelephoneNumber_tx = a.TelephoneNumber_tx,
                               Website = a.Website,
                               //Country_Id = (jdact.Country_Id == Guid.Empty) ? ((act.Country_Id == Guid.Empty) ? a.Country_Id : act.Country_Id) : jdact.Country_Id,
                               //City_Id = (jdact.City_Id == Guid.Empty) ? ((act.City_Id == Guid.Empty) ? a.City_Id : act.City_Id) : jdact.City_Id,
                               Country_Id = (jdact.Country_Id == Guid.Empty) ? a.Country_Id : jdact.Country_Id,
                               City_Id = (jdact.City_Id == Guid.Empty) ? a.City_Id : jdact.City_Id,
                               SystemCityName = jdact.Name,
                               SystemCountryName = jdact.CountryName
                           }).ToList();
                    #endregion
                }
                foreach (int priority in obj.Priorities)
                {
                    if (res.Count == 0)
                    {
                        break;
                    }
                    var curAttributeVals = obj.lstConfigs.Where(a => a.Priority == priority).ToList();

                    List<DC_SupplierImportAttributeValues> configs = curAttributeVals;

                    //obj.CurrentPriority = curPriority;
                    bool isCountryCodeCheck = false;
                    bool isCountryNameCheck = false;
                    bool isCityCodeCheck = false;
                    bool isCityNameCheck = false;
                    bool isLatLongCheck = false;
                    bool isCodeCheck = false;
                    bool isNameCheck = false;
                    bool isPlaceIdCheck = false;
                    bool isAddressCheck = false;
                    bool isTelephoneCheck = false;
                    bool isPostCodeCheck = false;
                    totConfigs = configs.Count;
                    curConfig = 0;
                    CallLogVerbose(File_Id, "MATCH", "Applying Matching Combination " + curPriority.ToString() + " out of Total " + totPriorities + " Combinations.", obj.CurrentBatch);
                    configWhere = "";
                    foreach (DC_SupplierImportAttributeValues config in curAttributeVals)
                    {
                        if (config.AttributeValue.Replace("Accommodation.", "").Trim() != "---ALL---")
                            configWhere = configWhere + " " + config.AttributeValue.Replace("Accommodation.", "").Trim() + ",";
                        else
                            configWhere = configWhere + " " + config.AttributeName.Replace("Accommodation_ProductMapping.", "").Trim() + ",";
                    }
                    configWhere = configWhere.Remove(configWhere.Length - 1);
                    CallLogVerbose(File_Id, "MATCH", "Matching Combination " + curPriority.ToString() + " consist of Match by " + configWhere, obj.CurrentBatch);

                    foreach (DC_SupplierImportAttributeValues config in curAttributeVals)
                    {
                        curConfig = curConfig + 1;

                        string CurrConfig = "";
                        if (config.AttributeValue.Replace("Accommodation.", "").Trim() != "---ALL---")
                            CurrConfig = config.AttributeValue.Replace("Accommodation.", "").Trim().ToUpper();
                        else
                            CurrConfig = config.AttributeName.Replace("Accommodation_ProductMapping.", "").Trim().ToUpper();
                        //CallLogVerbose(File_Id, "MATCH", "Applying Check for " + CurrConfig, obj.CurrentBatch);

                        if (CurrConfig == "COUNTRY")
                            isCountryNameCheck = true;
                        else if (CurrConfig == "CITY")
                            isCityNameCheck = true;
                        else if (CurrConfig == "CompanyHotelID".ToUpper())
                            isCodeCheck = true;
                        else if (CurrConfig == "PostalCode".ToUpper())
                            isPostCodeCheck = true;
                        else if (CurrConfig == "HotelName".ToUpper())
                            isNameCheck = true;
                        else if (CurrConfig == "LATITUDE")
                            isLatLongCheck = true;
                        else if (CurrConfig == "GOOGLE_PLACE_ID")
                            isPlaceIdCheck = true;
                        else if (CurrConfig == "Address_Tx".ToUpper())
                            isAddressCheck = true;
                        else if (CurrConfig == "TelephoneNumber_tx".ToUpper() || CurrConfig == "Telephone_tx".ToUpper())
                            isTelephoneCheck = true;


                    }

                    if (isCountryNameCheck || isCityNameCheck || isCodeCheck || isNameCheck || isLatLongCheck || isPlaceIdCheck || isAddressCheck || isTelephoneCheck || isPostCodeCheck)
                    {
                        if (isCountryNameCheck || isCityNameCheck || isCodeCheck || isNameCheck || isLatLongCheck || isPlaceIdCheck || isPostCodeCheck)
                        {


                            //CallLogVerbose(File_Id, "MATCH", "Looking for Match in Master Data for Matching Combination " + curPriority.ToString() + ".", obj.CurrentBatch);
                            using (ConsumerEntities context = new ConsumerEntities())
                            {
                                res.RemoveAll(p => p.Accommodation_Id != null && p.Accommodation_Id != Guid.Empty); //Guid.Empty
                                res = res.Select(c =>
                                {
                                    c.Accommodation_Id = (context.Accommodation.AsNoTracking()
                                                    .Where(s => (
                                                                    //((isCountryNameCheck && s.country.ToUpper().Trim() == c.SystemCountryName.ToUpper().Trim()) || (!isCountryNameCheck)) &&
                                                                    //((isCityNameCheck && s.city.ToUpper().Trim() == c.SystemCityName.ToUpper().Trim()) || (!isCityNameCheck)) &&
                                                                    //((isCountryNameCheck && s.Country_Id == c.Country_Id) || (!isCountryNameCheck)) &&
                                                                    //((isCityNameCheck && s.City_Id == c.City_Id) || (!isCityNameCheck)) &&
                                                                    ((isCountryNameCheck && s.country.ToUpper().Trim() == c.SystemCountryName.ToUpper().Trim()) || (!isCountryNameCheck)) &&
                                                                    ((isCityNameCheck && s.city.ToUpper().Trim() == c.SystemCityName.ToUpper().Trim()) || (!isCityNameCheck)) &&
                                                                    ((isCodeCheck && s.CompanyHotelID.ToString() == c.SupplierProductReference) || (!isCodeCheck)) &&
                                                                    ((isPostCodeCheck && s.PostalCode.ToString() == c.PostCode) || (!isPostCodeCheck)) &&
                                                                    ((isNameCheck && s.HotelName.ToUpper().Replace("HOTEL", "").Replace(s.country ?? "", "").Replace(s.city ?? "", "").Replace("  ", " ").Trim() == c.ProductName.ToUpper().Replace("HOTEL", "").Replace(c.CountryName ?? "", "").Replace(c.CityName ?? "", "").Replace("  ", " ").Trim()) || (!isNameCheck)) &&
                                                                    ((isLatLongCheck && s.Latitude == c.Latitude && s.Longitude == c.Longitude) || (!isLatLongCheck)) &&
                                                                    ((isPlaceIdCheck && s.Google_Place_Id == c.Google_Place_Id) || (!isPlaceIdCheck)) &&
                                                                    ((isAddressCheck && s.Address_Tx != null && c.Address_tx != null && s.Address_Tx == c.Address_tx) || (!isAddressCheck)) &&
                                                                    ((isTelephoneCheck && s.Telephone_Tx != null && c.TelephoneNumber_tx != null && s.Telephone_Tx == c.TelephoneNumber_tx) || (!isTelephoneCheck))
                                                                )
                                                           )
                                                    .Select(s1 => s1.Accommodation_Id)
                                                    .FirstOrDefault()
                                                    );
                                    return c;
                                }).ToList();
                            }
                        }
                        #region "Address Check and Telephone Check"
                        else if (isAddressCheck)
                        {
                            using (ConsumerEntities context = new ConsumerEntities())
                            {
                                prodMapSearch = (from a in context.Accommodation_ProductMapping.AsNoTracking()
                                                 join s in context.STG_Mapping_TableIds.AsNoTracking() on a.Accommodation_ProductMapping_Id equals s.Mapping_Id
                                                 where s.File_Id == supdata.File_Id && a.Accommodation_Id == null && a.Supplier_Id == curSupplier_Id && s.Batch == obj.CurrentBatch
                                                 && a.Status.Trim().ToUpper() == "UNMAPPED"
                                                 select a);
                                res = (from a in prodMapSearch
                                       join ac in context.Accommodation.AsNoTracking() on a.address_tx equals ac.Address_Tx
                                       where ac.Address_Tx != null && ac.Address_Tx != ""
                                       select new DataContracts.Mapping.DC_Accomodation_ProductMapping
                                       {
                                           Accommodation_ProductMapping_Id = a.Accommodation_ProductMapping_Id,
                                           Accommodation_Id = ac.Accommodation_Id,
                                           Address_tx = a.address_tx,
                                           CityCode = a.CityCode,
                                           CityName = a.CityName,
                                           CountryCode = a.CountryCode,
                                           CountryName = a.CountryName,
                                           Create_Date = a.Create_Date,
                                           Create_User = a.Create_User,
                                           Edit_Date = a.Edit_Date,
                                           Edit_User = a.Edit_User,
                                           Email = a.Email,
                                           Fax = a.Fax,
                                           Latitude = a.Latitude,
                                           Longitude = a.Longitude,
                                           IsActive = (a.IsActive ?? true),
                                           MapId = a.MapId,
                                           PostCode = a.PostCode,
                                           ProductName = a.ProductName,
                                           Remarks = a.Remarks,
                                           StarRating = a.StarRating,
                                           StateCode = a.StateCode,
                                           StateName = a.StateName,
                                           Status = "REVIEW",
                                           Street = a.Street,
                                           Street2 = a.Street2,
                                           Street3 = a.Street3,
                                           Street4 = a.Street4,
                                           SupplierId = a.SupplierId,
                                           SupplierName = a.SupplierName,
                                           SupplierProductReference = a.SupplierProductReference,
                                           TelephoneNumber = a.TelephoneNumber,
                                           TelephoneNumber_tx = a.TelephoneNumber_tx,
                                           Website = a.Website
                                       }).ToList();
                            }
                        }
                        else if (isTelephoneCheck)
                        {
                            using (ConsumerEntities context = new ConsumerEntities())
                            {
                                prodMapSearch = (from a in context.Accommodation_ProductMapping.AsNoTracking()
                                                 join s in context.STG_Mapping_TableIds.AsNoTracking() on a.Accommodation_ProductMapping_Id equals s.Mapping_Id
                                                 where s.File_Id == supdata.File_Id && a.Accommodation_Id == null && a.Supplier_Id == curSupplier_Id && s.Batch == obj.CurrentBatch
                                                 && a.Status.Trim().ToUpper() == "UNMAPPED"
                                                 select a);
                                res = (from a in prodMapSearch
                                       join ac in context.Accommodation.AsNoTracking() on a.TelephoneNumber_tx equals ac.Telephone_Tx
                                       where ac.Telephone_Tx != null && ac.Telephone_Tx != ""
                                       select new DataContracts.Mapping.DC_Accomodation_ProductMapping
                                       {
                                           Accommodation_ProductMapping_Id = a.Accommodation_ProductMapping_Id,
                                           Accommodation_Id = ac.Accommodation_Id,
                                           Address_tx = a.address_tx,
                                           CityCode = a.CityCode,
                                           CityName = a.CityName,
                                           CountryCode = a.CountryCode,
                                           CountryName = a.CountryName,
                                           Create_Date = a.Create_Date,
                                           Create_User = a.Create_User,
                                           Edit_Date = a.Edit_Date,
                                           Edit_User = a.Edit_User,
                                           Email = a.Email,
                                           Fax = a.Fax,
                                           Latitude = a.Latitude,
                                           Longitude = a.Longitude,
                                           IsActive = (a.IsActive ?? true),
                                           MapId = a.MapId,
                                           PostCode = a.PostCode,
                                           ProductName = a.ProductName,
                                           Remarks = a.Remarks,
                                           StarRating = a.StarRating,
                                           StateCode = a.StateCode,
                                           StateName = a.StateName,
                                           Status = "REVIEW",
                                           Street = a.Street,
                                           Street2 = a.Street2,
                                           Street3 = a.Street3,
                                           Street4 = a.Street4,
                                           SupplierId = a.SupplierId,
                                           SupplierName = a.SupplierName,
                                           SupplierProductReference = a.SupplierProductReference,
                                           TelephoneNumber = a.TelephoneNumber,
                                           TelephoneNumber_tx = a.TelephoneNumber_tx,
                                           Website = a.Website
                                       }).ToList();
                            }
                        }
                        #endregion

                        //CallLogVerbose(File_Id, "MATCH", "Setting Appropriate Status.", obj.CurrentBatch);                           
                        //BatchExtensions.Update()

                        //insertSTGList = stg.Where(w => !toUpdate.Any(a => a.SupplierProductReference == w.ProductId)).ToList();
                        //updateMappingList = toUpdate.Where(w => w.ProductName != w.oldProductName).ToList();



                        var toupdate = res.Where(p => p.Accommodation_Id != Guid.Empty).Select(c =>
                        {
                            c.MatchedBy = curPriority - 1;
                            c.Status = ("REVIEW"); return c;
                        }).ToList();


                        CallLogVerbose(File_Id, "MATCH", toupdate.Count.ToString() + " Matches Found for Combination " + curPriority.ToString() + ".", obj.CurrentBatch);
                        //CallLogVerbose(File_Id, "MATCH", "Updating into Database.", obj.CurrentBatch);                            
                        //List<Accommodation_ProductMapping> dbapm = new List<Accommodation_ProductMapping>();
                        foreach (DC_Accomodation_ProductMapping a in toupdate)
                        {
                            Guid curAccommodation_ProductMapping_Id = a.Accommodation_ProductMapping_Id;
                            Guid curAccommodation_Id = a.Accommodation_Id ?? Guid.Empty;

                            if (curAccommodation_Id != Guid.Empty)
                            {
                                using (ConsumerEntities context = new ConsumerEntities())
                                {
                                    context.Accommodation_ProductMapping.Where(x => x.Accommodation_ProductMapping_Id == curAccommodation_ProductMapping_Id)
                                       .Update(t => new Accommodation_ProductMapping()
                                       {
                                           MatchedBy = curPriority - 1,
                                           Status = "REVIEW",
                                           Accommodation_Id = curAccommodation_Id
                                       });
                                }
                            }
                        }

                        //var list = new List<System.Guid>();
                        //foreach (DC_Accomodation_ProductMapping a in toupdate)
                        //{
                        //    list.Add(a.Accommodation_ProductMapping_Id);
                        //    //dbapm.Add(new Accommodation_ProductMapping() { Accommodation_ProductMapping_Id = a.Accommodation_ProductMapping_Id } );
                        //}

                        //if (toupdate.Count > 0)
                        //{
                        //    context.Accommodation_ProductMapping.Where(x => list.Contains(x.Accommodation_ProductMapping_Id))
                        //        //.Any(a => a.Accommodation_ProductMapping_Id == x.Accommodation_ProductMapping_Id))
                        //        .Update(t => new Accommodation_ProductMapping() { MatchedBy = curPriority - 1, Status = "REVIEW" });
                        //}
                        retrn = true;


                        //if (UpdateAccomodationProductMapping(toupdate))
                        //{
                        //    retrn = true;
                        //}


                    }
                    else
                    {
                        retrn = false;
                    }

                    curPriority = curPriority + 1;
                }
                //}
                //CallLogVerbose(File_Id, "MATCH", "Update Done.", obj.CurrentBatch);
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating hotel mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }

            return retrn;
        }

        public IList<DataContracts.Mapping.DC_Accomodation_ProductMapping> GetAccomodationProductMapping(DataContracts.Mapping.DC_Mapping_ProductSupplier_Search_RQ obj)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var prodMapSearch = from a in context.Accommodation_ProductMapping select a;

                    if (obj.Accommodation_Id != Guid.Empty)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.Accommodation_Id == obj.Accommodation_Id
                                        select a;
                    }

                    if (obj.Status != null)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where (a.Status.Trim().ToUpper() ?? "UNMAPPED") == obj.Status.Trim().ToUpper()
                                        select a;
                    }
                    if (obj.StatusExcept != null)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where (a.Status.Trim().ToUpper() ?? "UNMAPPED") != obj.StatusExcept.Trim().ToUpper()
                                        select a;
                    }
                    if (obj.MatchedBy != null)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where (a.MatchedBy ?? 99) == obj.MatchedBy
                                        select a;
                    }

                    int total;

                    total = prodMapSearch.Count();

                    var skip = obj.PageSize * obj.PageNo;

                    var canPage = skip < total;

                    //DataContracts.Masters.DC_M_masterattributelists attr = new DataContracts.Masters.DC_M_masterattributelists();
                    //DataContracts.Masters.DC_MasterAttribute _obj = new DataContracts.Masters.DC_MasterAttribute() { MasterFor = "ProductSupplierMapping", Name = "MatchingPriority" };
                    //attr = master.GetListAttributeAndValuesByFOR(_obj);

                    //if (!canPage)
                    //    return null;
                    System.Linq.IQueryable<DataContracts.Mapping.DC_Accomodation_ProductMapping> prodMapList;

                    if (obj.Accommodation_Id != Guid.Empty)
                    {
                        prodMapList = (from a in prodMapSearch
                                       join ac in context.Accommodation on a.Accommodation_Id equals ac.Accommodation_Id
                                       where ac.Accommodation_Id == obj.Accommodation_Id
                                       orderby a.ProductName //a.SupplierName, a.ProductName, a.SupplierProductReference
                                       select new DataContracts.Mapping.DC_Accomodation_ProductMapping
                                       {
                                           Accommodation_ProductMapping_Id = a.Accommodation_ProductMapping_Id,
                                           Accommodation_Id = a.Accommodation_Id,
                                           Supplier_Id = a.Supplier_Id,
                                           SupplierId = a.SupplierId,
                                           SupplierName = a.SupplierName,
                                           SupplierProductReference = a.SupplierProductReference,
                                           ProductName = a.ProductName,
                                           Street = a.Street,
                                           Street2 = a.Street2,
                                           Street3 = a.Street3,
                                           Street4 = a.Street4,
                                           CountryCode = a.CountryCode,
                                           CountryName = a.CountryName,
                                           CityCode = a.CityCode,
                                           CityName = a.CityName,
                                           StateCode = a.StateCode,
                                           StateName = a.StateName,
                                           PostCode = a.PostCode,
                                           TelephoneNumber = a.TelephoneNumber,
                                           Fax = a.Fax,
                                           Email = a.Email,
                                           Website = a.Website,
                                           Latitude = a.Latitude,
                                           Longitude = a.Longitude,
                                           Status = a.Status,
                                           Create_Date = a.Create_Date,
                                           Create_User = a.Create_User,
                                           Edit_Date = a.Edit_Date,
                                           Edit_User = a.Edit_User,
                                           IsActive = (a.IsActive ?? true),
                                           TotalRecords = total,
                                           ProductId = ac.CompanyHotelID.ToString(),
                                           SystemCountryName = ac.country,
                                           SystemCityName = ac.city,
                                           SystemProductName = ac.HotelName,
                                           Remarks = a.Remarks,
                                           MapId = a.MapId,
                                           StarRating = a.StarRating,
                                           MatchedBy = a.MatchedBy,
                                           MatchedByString = a.MatchedByString
                                           //,MatchedByString = attr.MasterAttributeValues.Where(x => x.AttributeValue == (a.MatchedBy ?? 99).ToString()).Select(x => (x.OTA_CodeTableValue ?? "")).FirstOrDefault()
                                       }).Skip(skip).Take(obj.PageSize);
                    }
                    else
                    {

                        prodMapList = (from a in prodMapSearch
                                       orderby a.ProductName//a.SupplierName, a.SupplierProductReference
                                       select new DataContracts.Mapping.DC_Accomodation_ProductMapping
                                       {
                                           Accommodation_ProductMapping_Id = a.Accommodation_ProductMapping_Id,
                                           Accommodation_Id = a.Accommodation_Id,
                                           Supplier_Id = a.Supplier_Id,
                                           SupplierId = a.SupplierId,
                                           SupplierName = a.SupplierName,
                                           SupplierProductReference = a.SupplierProductReference,
                                           ProductName = a.ProductName,
                                           Street = a.Street,
                                           Street2 = a.Street2,
                                           Street3 = a.Street3,
                                           Street4 = a.Street4,
                                           CountryCode = a.CountryCode,
                                           CountryName = a.CountryName,
                                           CityCode = a.CityCode,
                                           CityName = a.CityName,
                                           StateCode = a.StateCode,
                                           StateName = a.StateName,
                                           PostCode = a.PostCode,
                                           TelephoneNumber = a.TelephoneNumber,
                                           Fax = a.Fax,
                                           Email = a.Email,
                                           Website = a.Website,
                                           Latitude = a.Latitude,
                                           Longitude = a.Longitude,
                                           Status = a.Status,
                                           Create_Date = a.Create_Date,
                                           Create_User = a.Create_User,
                                           Edit_Date = a.Edit_Date,
                                           Edit_User = a.Edit_User,
                                           IsActive = (a.IsActive ?? true),
                                           TotalRecords = total,
                                           ProductId = a.SupplierProductReference,
                                           MapId = a.MapId,
                                           Remarks = a.Remarks,
                                           StarRating = a.StarRating,
                                           MatchedBy = a.MatchedBy,
                                           MatchedByString = a.MatchedByString
                                           //,MatchedByString = attr.MasterAttributeValues.Where(x => x.AttributeValue == (a.MatchedBy ?? 99).ToString()).Select(x => (x.OTA_CodeTableValue ?? "")).FirstOrDefault()
                                       }).Skip(skip).Take(obj.PageSize);
                    }

                    List<DC_Accomodation_ProductMapping> ret = new List<DC_Accomodation_ProductMapping>();
                    ret = prodMapList.ToList();

                    //ret = ret.Select(c => {
                    //    c.MatchedByString = attr.MasterAttributeValues.Where(x => x.AttributeValue == (c.MatchedBy ?? 99).ToString()).Select(x => (x.OTA_CodeTableValue ?? "")).FirstOrDefault();
                    //    return c;
                    //}).ToList();


                    return ret;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching accomodation product mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public List<DataContracts.Mapping.DC_Accomodation_ProductMapping> GetMappingHotelData(DataContracts.Mapping.DC_Mapping_ProductSupplier_Search_RQ obj)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var prodMapSearch = from a in context.Accommodation_ProductMapping.AsNoTracking() select a; //where a.Accommodation_Id == null 

                    if (!string.IsNullOrWhiteSpace(obj.SupplierId))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.SupplierId == obj.SupplierId
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.SupplierName))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.SupplierName == obj.SupplierName
                                        select a;
                    }

                    if (obj.Supplier_Id != null)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.Supplier_Id == obj.Supplier_Id
                                        select a;
                    }

                    if (obj.MatchedBy != null)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where (a.MatchedBy ?? 99) == obj.MatchedBy
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.Street))
                    {
                        string address_tx = CommonFunctions.RemoveSpecialCharacters(obj.Street);
                        prodMapSearch = from a in prodMapSearch
                                        where a.address_tx == address_tx
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.Street2))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.Street2.Contains(obj.Street2)
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.Street3))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.Street3.Contains(obj.Street3)
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.ProductName))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.ProductName.Contains(obj.ProductName)
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.PostCode))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.PostCode.Contains(obj.PostCode)
                                        select a;
                    }
                    if (!string.IsNullOrWhiteSpace(obj.TelephoneNumber))
                    {
                        string telephoneNumber_tx = CommonFunctions.GetCharacter(obj.TelephoneNumber, 8);
                        prodMapSearch = from a in prodMapSearch
                                        where a.TelephoneNumber_tx == telephoneNumber_tx
                                        select a;
                    }
                    /*if (!string.IsNullOrWhiteSpace(obj.CountryName))
                    {
                        if (!string.IsNullOrWhiteSpace(obj.Source) && obj.Source.ToUpper() == "SYSTEMDATA")
                        {
                            var distCountryMapping = (from a in context.m_CountryMapping.AsNoTracking() select new { a.Country_Id, a.CountryName }).Distinct();
                            prodMapSearch = from a in prodMapSearch
                                            join ct in distCountryMapping on a.CountryName equals ct.CountryName
                                            join mct in context.m_CountryMaster on ct.Country_Id equals mct.Country_Id
                                            where mct.Name == obj.CountryName
                                            select a;
                        }
                        else
                        {
                            prodMapSearch = from a in prodMapSearch
                                            where a.CountryName == obj.CountryName
                                            select a;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(obj.CityName))
                    {
                        if (!string.IsNullOrWhiteSpace(obj.Source) && obj.Source.ToUpper() == "SYSTEMDATA")
                        {
                            var distCityMapping = (from a in context.m_CityMapping.AsNoTracking() select new { a.City_Id, a.CityName }).Distinct();
                            prodMapSearch = from a in prodMapSearch
                                            join ct in distCityMapping on a.CityName equals ct.CityName
                                            join mct in context.m_CityMaster on ct.City_Id equals mct.City_Id
                                            where mct.Name == obj.CityName
                                            select a;
                        }
                        else
                        {
                            prodMapSearch = from a in prodMapSearch
                                            where a.CityName == obj.CityName
                                            select a;
                        }
                    }
                    */

                    if (!string.IsNullOrWhiteSpace(obj.CountryName))
                    {
                        if (!string.IsNullOrWhiteSpace(obj.Source) && obj.Source.ToUpper() == "SYSTEMDATA")
                        {
                            var distCountryMapping = (from a in context.m_CountryMapping.AsNoTracking() select new { a.Country_Id, a.CountryCode, a.CountryName, a.Supplier_Id }).Distinct();
                            prodMapSearch = from a in prodMapSearch
                                            join ct in distCountryMapping on new { a.Supplier_Id } equals new { ct.Supplier_Id }
                                            join mct in context.m_CountryMaster on ct.Country_Id equals mct.Country_Id
                                            where mct.Name == obj.CountryName
                                            && ((a.CountryName == null) ? (a.CountryCode == ct.CountryCode) : (a.CountryName == ct.CountryName))
                                            select a;
                        }
                        else
                        {
                            prodMapSearch = from a in prodMapSearch
                                            where a.CountryName == obj.CountryName
                                            select a;
                        }
                    }


                    if (!string.IsNullOrWhiteSpace(obj.CityName))
                    {
                        if (!string.IsNullOrWhiteSpace(obj.Source) && obj.Source.ToUpper() == "SYSTEMDATA")
                        {
                            var distCityMapping = (from a in context.m_CityMapping.AsNoTracking() select new { a.City_Id, a.CityName, a.Supplier_Id, a.Country_Id }).Distinct();
                            prodMapSearch = from a in prodMapSearch
                                            join ct in distCityMapping on new { a.Supplier_Id, a.Country_Id, a.CityName } equals new { ct.Supplier_Id, ct.Country_Id, ct.CityName }
                                            join mct in context.m_CityMaster on ct.City_Id equals mct.City_Id
                                            where mct.Name == obj.CityName
                                            select a;
                        }
                        else
                        {
                            prodMapSearch = from a in prodMapSearch
                                            where a.CityName == obj.CityName
                                            select a;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(obj.CityCode))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.CityCode == obj.CityCode
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.CountryCode))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.CountryCode == obj.CountryCode
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.Status))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where (a.Status ?? "UNMAPPED") == obj.Status
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.Chain))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        join m in context.Accommodation on a.Accommodation_Id equals m.Accommodation_Id
                                        where m.Chain == obj.Chain
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.Brand))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        join m in context.Accommodation on a.Accommodation_Id equals m.Accommodation_Id
                                        where m.Brand == obj.Brand
                                        select a;
                    }

                    if (obj.StatusExcept != null)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.Status.ToUpper() != obj.StatusExcept.ToUpper()
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.SupplierCountryName))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where (a.CountryName.ToUpper().Contains(obj.SupplierCountryName.ToUpper()))
                                        select a;
                    }
                    if (!string.IsNullOrWhiteSpace(obj.SupplierCityName))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.CityName.Contains(obj.SupplierCityName)
                                        select a;
                    }
                    if (!string.IsNullOrWhiteSpace(obj.SupplierProductName))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.ProductName.Contains(obj.SupplierProductName)
                                        select a;
                    }
                    if (!string.IsNullOrWhiteSpace(obj.StarRating))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.StarRating.Contains(obj.StarRating)
                                        select a;
                    }

                    int total;

                    //DataContracts.Masters.DC_M_masterattributelists attr = new DataContracts.Masters.DC_M_masterattributelists();
                    //DataContracts.Masters.DC_MasterAttribute _obj = new DataContracts.Masters.DC_MasterAttribute() { MasterFor = "ProductSupplierMapping", Name = "MatchingPriority" };
                    //attr = master.GetListAttributeAndValuesByFOR(_obj);
                    //total = prodMapSearch.Count();

                    //var skip = obj.PageSize * obj.PageNo;

                    //var canPage = skip < total;

                    var prodMapList = (from a in prodMapSearch
                                       select new DataContracts.Mapping.DC_Accomodation_ProductMapping
                                       {
                                           Accommodation_ProductMapping_Id = a.Accommodation_ProductMapping_Id,
                                           Accommodation_Id = a.Accommodation_Id,
                                           Supplier_Id = a.Supplier_Id,
                                           SupplierId = a.SupplierId,
                                           SupplierName = a.SupplierName,
                                           SupplierProductReference = a.SupplierProductReference,
                                           ProductName = a.ProductName,
                                           oldProductName = a.ProductName,
                                           Street = a.Street,
                                           Street2 = a.Street2,
                                           Street3 = a.Street3,
                                           Street4 = a.Street4,
                                           CountryCode = a.CountryCode,
                                           CountryName = a.CountryName,
                                           CityCode = a.CityCode,
                                           CityName = a.CityName,
                                           StateCode = a.StateCode,
                                           StateName = a.StateName,
                                           PostCode = a.PostCode,
                                           TelephoneNumber = a.TelephoneNumber,
                                           Fax = a.Fax,
                                           Email = a.Email,
                                           Website = a.Website,
                                           Latitude = a.Latitude,
                                           Longitude = a.Longitude,
                                           Status = a.Status,
                                           Create_Date = a.Create_Date,
                                           Create_User = a.Create_User,
                                           Edit_Date = a.Edit_Date,
                                           Edit_User = a.Edit_User,
                                           IsActive = (a.IsActive ?? true),
                                           ProductId = a.SupplierProductReference,
                                           Remarks = a.Remarks,
                                           MapId = a.MapId,
                                           StarRating = a.StarRating,
                                           Country_Id = a.Country_Id,
                                           City_Id = a.City_Id,
                                           MatchedBy = a.MatchedBy,
                                           MatchedByString = a.MatchedByString
                                           //,MatchedByString = attr.MasterAttributeValues.Where(x => x.AttributeValue == (a.MatchedBy ?? 99).ToString()).Select(x => (x.OTA_CodeTableValue ?? "")).FirstOrDefault()

                                       });//.Skip(skip).Take(obj.PageSize);

                    var result = prodMapList.ToList();
                    //result = result.Select(c => {
                    //    c.MatchedByString = attr.MasterAttributeValues.Where(x => x.AttributeValue == (c.MatchedBy ?? 99).ToString()).Select(x => (x.OTA_CodeTableValue ?? "")).FirstOrDefault();
                    //    return c;
                    //}).ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching accomodation product mapping by supplier search", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public List<DataContracts.Mapping.DC_Accomodation_ProductMapping> GetProductSupplierMappingSearch(DataContracts.Mapping.DC_Mapping_ProductSupplier_Search_RQ obj)
        {
            try
            {
                int skip = 0;
                int total = 0;

                skip = obj.PageSize * obj.PageNo;

                StringBuilder sbsqlselectcount = new StringBuilder();
                StringBuilder sbsqlselect = new StringBuilder();
                StringBuilder sbsqlorderby = new StringBuilder();
                StringBuilder sbsqlfrom = new StringBuilder();
                StringBuilder sbsqlwhere = new StringBuilder();
                StringBuilder sbsqlcmjoin = new StringBuilder();
                StringBuilder sbsqlctmjoin = new StringBuilder();

                sbsqlwhere.AppendLine(" where 1=1 ");

                if (obj.Supplier_Id.HasValue)
                {
                    sbsqlwhere.AppendLine(" and apm.Supplier_Id = '" + obj.Supplier_Id.ToString() + "' ");
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(obj.SupplierId))
                    {
                        sbsqlwhere.AppendLine(" and apm.SupplierId = '" + obj.SupplierId.ToString() + "' ");
                    }

                    if (!string.IsNullOrWhiteSpace(obj.SupplierName))
                    {
                        sbsqlwhere.AppendLine(" and apm.SupplierName = '" + obj.SupplierName.ToString() + "' ");
                    }
                }

                if (obj.MatchedBy != null)
                {
                    sbsqlwhere.AppendLine(" and apm.MatchedBy = " + obj.MatchedBy.ToString() + " ");
                }

                if (!string.IsNullOrWhiteSpace(obj.Street))
                {
                    string address_tx = CommonFunctions.RemoveSpecialCharacters(obj.Street);
                    sbsqlwhere.AppendLine(" and apm.address_tx = '" + address_tx.ToString() + "' ");
                }

                if (!string.IsNullOrWhiteSpace(obj.Street2))
                {
                    sbsqlwhere.AppendLine(" and apm.Street2 like '%" + obj.Street2.ToString() + "%' ");
                }

                if (!string.IsNullOrWhiteSpace(obj.Street3))
                {
                    sbsqlwhere.AppendLine(" and apm.Street3 like '%" + obj.Street3.ToString() + "%' ");
                }

                if (!string.IsNullOrWhiteSpace(obj.ProductName))
                {
                    sbsqlwhere.AppendLine(" and replace(ltrim(rtrim(apm.ProductName)), 'hotel','') like '%" + obj.ProductName.Replace("hotel", "").Trim() + "%' ");
                }

                if (!string.IsNullOrWhiteSpace(obj.PostCode))
                {
                    sbsqlwhere.AppendLine(" and apm.PostCode like '%" + obj.PostCode.ToString() + "%' ");
                }

                if (!string.IsNullOrWhiteSpace(obj.TelephoneNumber))
                {
                    string telephoneNumber_tx = CommonFunctions.GetCharacter(obj.TelephoneNumber, 8);
                    sbsqlwhere.AppendLine(" and apm.TelephoneNumber_tx = '" + telephoneNumber_tx.ToString() + "' ");
                }

                if (!string.IsNullOrWhiteSpace(obj.StarRating))
                {
                    sbsqlwhere.AppendLine(" and apm.StarRating like '%" + obj.StarRating.ToString().Trim() + "%' ");
                }

                if (!string.IsNullOrWhiteSpace(obj.Status))
                {
                    sbsqlwhere.AppendLine(" and apm.Status = '" + obj.Status.ToString().Trim() + "' ");
                }
                else
                {
                    sbsqlwhere.AppendLine(" and apm.Status != 'DELETE' ");
                }

                if (!string.IsNullOrWhiteSpace(obj.Chain))
                {
                    sbsqlwhere.AppendLine(" and a.Chain = '" + obj.Chain.ToString() + "' ");
                }

                if (!string.IsNullOrWhiteSpace(obj.Brand))
                {
                    sbsqlwhere.AppendLine(" and a.Brand = '" + obj.Brand.ToString() + "' ");
                }

                if (obj.StatusExcept != null)
                {
                    sbsqlwhere.AppendLine(" and apm.Status != '" + obj.StatusExcept.ToString().Trim() + "' ");
                }

                if (!string.IsNullOrWhiteSpace(obj.Via) && Convert.ToString(obj.Via) == "CROSS")
                {
                    sbsqlwhere.AppendLine(" and apm.Status NOT IN ('AUTOMAPPED','DELETE') ");
                }

                if ((obj.Country_Id != null) && obj.Country_Id != Guid.Empty)
                {
                    sbsqlwhere.AppendLine(" and apm.Country_Id = '" + obj.Country_Id.ToString() + "' ");
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(obj.CountryCode))
                    {
                        sbsqlwhere.AppendLine(" and apm.CountryCode = '" + obj.CountryCode.ToString().Trim() + "' ");
                    }
                    else if (!string.IsNullOrWhiteSpace(obj.CountryName))
                    {
                        if (!string.IsNullOrWhiteSpace(obj.Source) && obj.Source.ToUpper() == "SYSTEMDATA")
                        {
                            sbsqlcmjoin.AppendLine(" inner join m_CountryMaster mc with (nolock) on apm.Country_Id = mc.Country_Id ");
                            sbsqlwhere.AppendLine(" and mc.Name = '" + obj.CountryName.ToString().Trim() + "' ");
                        }
                        else
                        {
                            sbsqlcmjoin.AppendLine(" inner join m_CountryMapping cm with (nolock) on apm.Supplier_Id = cm.Supplier_Id ");
                            sbsqlcmjoin.AppendLine(" and ((apm.CountryCode is not null and apm.CountryCode = cm.CountryCode) OR (apm.CountryCode is null and apm.CountryName = cm.CountryName)) ");
                            sbsqlwhere.AppendLine(" and cm.CountryName = '" + obj.CountryName.ToString().Trim() + "' ");
                        }
                    }
                }

                if ((obj.City_Id != null) && obj.City_Id != Guid.Empty)
                {
                    sbsqlwhere.AppendLine(" and apm.City_Id = '" + obj.City_Id.ToString() + "' ");
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(obj.CityCode))
                    {
                        sbsqlwhere.AppendLine(" and apm.CityCode = '" + obj.CityCode.ToString().Trim() + "' ");
                    }
                    else if (!string.IsNullOrWhiteSpace(obj.CityName))
                    {
                        if (!string.IsNullOrWhiteSpace(obj.Source) && obj.Source.ToUpper() == "SYSTEMDATA")
                        {
                            sbsqlctmjoin.AppendLine(" inner join m_CityMaster mct on ctm.City_Id = mct.City_Id ");
                            sbsqlwhere.AppendLine(" and mct.Name = '" + obj.CityName.ToString().Trim() + "' ");
                        }
                        else
                        {
                            sbsqlctmjoin.AppendLine(" inner join m_CityMapping ctm on apm.Supplier_Id = ctm.Supplier_Id and apm.Country_Id = ctm.Country_Id ");
                            sbsqlctmjoin.AppendLine(" and ((apm.CityCode is not null and apm.CityCode = ctm.CityCode) OR (apm.CityCode is null and apm.CityName = ctm.CityName)) ");
                            sbsqlwhere.AppendLine(" and ctm.CityName = '" + obj.CityName.ToString().Trim() + "' ");
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(obj.SupplierCountryName))
                {
                    sbsqlwhere.AppendLine(" and apm.CountryName = '" + obj.SupplierCountryName.ToString().Trim().ToUpper() + "' ");
                }

                if (!string.IsNullOrWhiteSpace(obj.SupplierCityName))
                {
                    sbsqlwhere.AppendLine(" and apm.CityName = '" + obj.SupplierCityName.ToString().Trim().ToUpper() + "' ");
                }

                if (!string.IsNullOrWhiteSpace(obj.SupplierProductName))
                {
                    bool isname = false;
                    if (!string.IsNullOrWhiteSpace(obj.Via))
                    {
                        if (obj.Via.Trim().ToUpper() == "CROSS" && !string.IsNullOrWhiteSpace(obj.HotelName_TX))
                        {
                            sbsqlwhere.AppendLine(" and apm.HotelName_Tx = '" + obj.HotelName_TX.ToString().Trim() + "' ");
                        }
                        else
                            isname = true;
                    }
                    else
                    {
                        isname = true;
                    }

                    if (isname)
                    {
                        sbsqlwhere.AppendLine(" and apm.ProductName like '%" + obj.SupplierProductName.ToString().Trim() + "%' ");
                    }
                }

                if (!string.IsNullOrWhiteSpace(obj.ProductType))
                {
                    sbsqlctmjoin.AppendLine(" inner join m_MasterAttributeValueMapping mavmp on mavmp.SupplierMasterAttributeValue = apm.ProductType ");
                    sbsqlctmjoin.AppendLine(" inner join m_masterattributevalue mav on mav.MasterAttributeValue_Id = mavmp.SystemMasterAttributeValue_Id ");
                    sbsqlwhere.AppendLine(" and mav.MasterAttributeValue_Id = '" + obj.ProductType + "'  ");
                }

                if (!string.IsNullOrWhiteSpace(obj.SupplierProductCode))
                {
                    sbsqlwhere.AppendLine(" and apm.SupplierProductReference = '" + obj.SupplierProductCode.ToString().Trim() + "' ");
                }

                if (obj.Priority != null)
                {
                    sbsqlwhere.AppendLine(" and a.Priority=" + obj.Priority + "  ");
                }

                #region Select from Tables

                sbsqlfrom.AppendLine(" from Accommodation_ProductMapping apm with (nolock) left join Accommodation a with (nolock) on apm.Accommodation_Id = a.Accommodation_Id AND ISNULL(A.ISACTIVE,0) = 1 ");
                sbsqlfrom.AppendLine(sbsqlctmjoin.ToString());
                sbsqlfrom.AppendLine(sbsqlcmjoin.ToString());

                #endregion

                #region Select Count of all records

                sbsqlselectcount.AppendLine("select count(1) ");
                sbsqlselectcount.AppendLine(sbsqlfrom.ToString());
                sbsqlselectcount.AppendLine(sbsqlwhere.ToString());

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    context.Configuration.AutoDetectChangesEnabled = false;
                    context.Database.CommandTimeout = 0;
                    total = context.Database.SqlQuery<int>(sbsqlselectcount.ToString()).First();
                }

                #endregion

                #region Select Query

                sbsqlselect.Append(@"select 
	                                apm.Accommodation_ProductMapping_Id, apm.Accommodation_Id, apm.Supplier_Id, 
	                                apm.SupplierId, apm.SupplierName,  apm.SupplierProductReference, apm.ProductName, 
	                                apm.ProductName as oldProductName,apm.Street, apm.Street2, apm.Street3, apm.Street4, 
	                                apm.PostCode,  apm.CountryCode, apm.CountryName, apm.CityCode, apm.CityName,  apm.StateCode, 
	                                apm.StateName, apm.TelephoneNumber, apm.Fax, apm.Email, apm.Website
                                    ,ISNULL(APM.Latitude,APM.GeoLocation.Lat) AS Latitude
                                    ,ISNULL(APM.Longitude,APM.GeoLocation.Long) AS Longitude
                                    ,apm.Status, apm.Create_Date, apm.Create_User, apm.Edit_Date, apm.Edit_User, 
	                                ISNULL(apm.IsActive,1) AS IsActive, apm.SupplierProductReference as ProductId, 
	                                apm.Remarks,  apm.MapId,
                                    (CASE WHEN APM.ADDRESS IS NOT NULL THEN APM.ADDRESS ELSE ISNULL(APM.Street + ',  ', '') + ISNULL(APM.Street2 + ' ', '')
                                        + ISNULL(APM.Street3 + ' ', '')
                                        + ISNULL(APM.Street4 + ' ', '')
                                        + ISNULL(APM.PostCode , '')
                                        END
                                    ) AS FullAddress,
	                                apm.StarRating, apm.MatchedBy, apm.MatchedByString,  a.HotelName as SystemProductName, 
	                                a.city as SystemCityName, a.country as SystemCountryName, a.FullAddress as SystemFullAddress, 
	                                a.Location, apm.ProductType, a.ProductCategorySubType as SystemProductType ");

                if (total <= skip)
                {
                    int PageIndex = 0;
                    int intReminder = total % obj.PageSize;
                    int intQuotient = total / obj.PageSize;

                    if (intReminder > 0 || (intReminder == 0 && intQuotient == 0))
                    {
                        PageIndex = intQuotient;
                    }
                    else if (intReminder == 0 && intQuotient > 0)
                    {
                        PageIndex = intQuotient - 1;
                    }

                    skip = obj.PageSize * PageIndex;
                }

                #endregion

                #region OrderBy and Offset
                sbsqlorderby.AppendLine(" ORDER BY apm.ProductName OFFSET ");
                sbsqlorderby.AppendLine(skip.ToString());
                sbsqlorderby.AppendLine(" ROWS FETCH NEXT ");
                sbsqlorderby.AppendLine(obj.PageSize.ToString());
                sbsqlorderby.AppendLine(" ROWS ONLY ");
                #endregion

                #region Complete Query

                sbsqlselect.AppendLine(sbsqlfrom.ToString());
                sbsqlselect.AppendLine(sbsqlwhere.ToString());
                sbsqlselect.AppendLine(sbsqlorderby.ToString());

                #endregion

                List<DataContracts.Mapping.DC_Accomodation_ProductMapping> result = new List<DC_Accomodation_ProductMapping>();

                if (total > 0)
                {
                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        context.Configuration.AutoDetectChangesEnabled = false;
                        context.Database.CommandTimeout = 0;
                        result = context.Database.SqlQuery<DataContracts.Mapping.DC_Accomodation_ProductMapping>(sbsqlselect.ToString()).ToList();
                        if (result != null)
                        {
                            result.ForEach(u =>
                            {
                                u.TotalRecords = total;
                                u.PageIndex = obj.PageNo;
                            });
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(obj.CalledFromTLGX))
                {
                    if (!string.IsNullOrWhiteSpace(obj.Status) && !string.IsNullOrWhiteSpace(obj.CityName) && obj.Status.ToLower().Trim() == "unmapped")
                    {
                        foreach (var item in result)
                        {
                            if (!string.IsNullOrWhiteSpace(item.ProductName))
                            {

                                var prodname = item.ProductName.ToLower().Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("hotel", "").Replace(item.CityName != null ? item.CityName.ToLower() : " ", "").Replace(item.CountryName != null ? item.CountryName.ToLower() : " ", "").ToUpper();
                                var prodcode = item.ProductId;
                                if (!string.IsNullOrWhiteSpace(prodname) && prodname.ToUpper() != "&NBSP;")
                                {
                                    using (ConsumerEntities context = new ConsumerEntities())
                                    {
                                        context.Configuration.AutoDetectChangesEnabled = false;
                                        var searchprod = context.Accommodation.Where(a => ((a.IsActive ?? false) == true) && (a.country == item.SystemCountryName) && (a.city == item.SystemCityName) && (a.HotelName ?? string.Empty).Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("hotel", "").ToUpper().Equals(prodname)).Select(s => new { s.Accommodation_Id, s.HotelName, s.FullAddress }).FirstOrDefault();
                                        if (searchprod == null)
                                            searchprod = context.Accommodation.Where(a => ((a.IsActive ?? false) == true) && (a.country == item.SystemCountryName) && (a.city == item.SystemCityName) && (a.HotelName ?? string.Empty).Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("hotel", "").ToUpper().Contains(prodname)).Select(s => new { s.Accommodation_Id, s.HotelName, s.FullAddress }).FirstOrDefault();

                                        if (searchprod != null)
                                        {
                                            item.mstAcco_Id = Convert.ToString(searchprod.Accommodation_Id);
                                            item.mstHotelName = searchprod.HotelName;
                                            item.FullAddress = searchprod.FullAddress;
                                        }
                                    }
                                }
                            }

                        }
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching accomodation product mapping by supplier search" + Environment.NewLine + e.Message, ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool UpdateAccomodationProductMapping(List<DataContracts.Mapping.DC_Accomodation_ProductMapping> obj)
        {
            Guid SupplierImportFile_Id = Guid.Empty;
            int Batch = 0;
            int length = 0;

            if (obj.Count > 0)
            {
                SupplierImportFile_Id = obj[0].ReRunSupplierImporrtFile_Id;
                if (SupplierImportFile_Id == Guid.Empty)
                {
                    SupplierImportFile_Id = obj[0].SupplierImporrtFile_Id;
                }
            }

            StringBuilder sbUpdateSRTMStatus = new StringBuilder();

            List<DataContracts.Mapping.DC_SupplierRoomName_AttributeList> AttributeList = new List<DataContracts.Mapping.DC_SupplierRoomName_AttributeList>();
            string TX_Value = string.Empty;
            string SX_Value = string.Empty;


            using (ConsumerEntities context = new ConsumerEntities())
            {
                //Get All Related Keywords
                List<DataContracts.Masters.DC_Keyword> Keywords = new List<DataContracts.Masters.DC_Keyword>();
                if (SupplierImportFile_Id != Guid.Empty)
                {
                    using (DL_Masters objDL = new DL_Masters())
                    {
                        Keywords = objDL.SearchKeyword(new DataContracts.Masters.DC_Keyword_RQ { EntityFor = "HotelName", PageNo = 0, PageSize = int.MaxValue, Status = "ACTIVE", AliasStatus = "ACTIVE" });
                    }
                }

                List<string> datatypes = new List<string> { "nvarchar", "varchar" };
                var columnLength = CommonFunctions.GetSqlTableColumnInfo("Accommodation_ProductMapping", datatypes);

                foreach (var PM in obj)
                {
                    if (SupplierImportFile_Id == Guid.Empty)
                    {
                        SupplierImportFile_Id = PM.ReRunSupplierImporrtFile_Id;
                        if (SupplierImportFile_Id == Guid.Empty)
                        {
                            SupplierImportFile_Id = obj[0].SupplierImporrtFile_Id;
                        }
                    }

                    if (Batch == 0)
                    {
                        Batch = PM.ReRunBatch;
                    }

                    if (PM.Accommodation_ProductMapping_Id == null)
                    {
                        continue;
                    }

                    if (SupplierImportFile_Id != Guid.Empty)
                    {
                        PM.HotelName_Tx = CommonFunctions.HotelNameTX(PM.ProductName, PM.CityName, PM.CountryName, ref Keywords);
                        PM.TelephoneNumber_tx = CommonFunctions.GetDigits(PM.TelephoneNumber, 8);
                        PM.Latitude_Tx = CommonFunctions.LatLongTX(PM.Latitude);
                        PM.Longitude_Tx = CommonFunctions.LatLongTX(PM.Longitude);
                        PM.Address_tx = CommonFunctions.TTFU(ref Keywords, ref AttributeList, ref TX_Value, ref SX_Value, PM.FullAddress, new string[] { PM.CityName, PM.CountryName });
                    }

                    try
                    {
                        DataLayer.Accommodation_ProductMapping search = new DataLayer.Accommodation_ProductMapping();

                        search = context.Accommodation_ProductMapping.Find(PM.Accommodation_ProductMapping_Id);

                        if (search != null)
                        {
                            #region RoomTypeMapCheck
                            if (search.Accommodation_Id != null && PM.Status != "AUTOMAPPED" && PM.Status != "MAPPED")
                            {
                                sbUpdateSRTMStatus.Clear();
                                sbUpdateSRTMStatus.AppendLine(" UPDATE Accommodation_SupplierRoomTypeMapping SET MappingStatus='UNMAPPED' , Accommodation_Id=null , Accommodation_RoomInfo_Id=null , MatchingScore=null, ");
                                sbUpdateSRTMStatus.AppendLine(" Edit_User= '" + PM.Edit_User + "' , Edit_Date = getdate() ");
                                sbUpdateSRTMStatus.AppendLine(" Where Supplier_Id='" + search.Supplier_Id + "' and Accommodation_Id='" + search.Accommodation_Id + "' and SupplierProductId='" + search.SupplierProductReference + "';");
                            }
                            else if (search.Accommodation_Id != PM.Accommodation_Id && (PM.Status == "AUTOMAPPED" || PM.Status == "MAPPED"))
                            {
                                sbUpdateSRTMStatus.Clear();
                                sbUpdateSRTMStatus.AppendLine(" UPDATE Accommodation_SupplierRoomTypeMapping SET MappingStatus='UNMAPPED' , Accommodation_Id='" + PM.Accommodation_Id + "', Accommodation_RoomInfo_Id=null , MatchingScore=null, ");
                                sbUpdateSRTMStatus.AppendLine(" Edit_User= '" + PM.Edit_User + "' , Edit_Date= getdate() ");
                                sbUpdateSRTMStatus.AppendLine(" Where Supplier_Id='" + search.Supplier_Id + "' and Accommodation_Id='" + search.Accommodation_Id + "' and SupplierProductId='" + search.SupplierProductReference + "';");
                            }
                            else if (search.Accommodation_Id == PM.Accommodation_Id && (search.Status != PM.Status) && (PM.Status == "AUTOMAPPED" || PM.Status == "MAPPED"))
                            {
                                sbUpdateSRTMStatus.Clear();
                                sbUpdateSRTMStatus.AppendLine(" UPDATE Accommodation_SupplierRoomTypeMapping SET Accommodation_Id = '" + PM.Accommodation_Id + "', ");
                                sbUpdateSRTMStatus.AppendLine(" Edit_User= '" + PM.Edit_User + "', Edit_Date = getdate() ");
                                sbUpdateSRTMStatus.AppendLine(" Where Supplier_Id='" + search.Supplier_Id + "' and SupplierProductId='" + search.SupplierProductReference + "';");
                            }
                            #endregion

                            bool isReMap = false;

                            search.Accommodation_Id = PM.Accommodation_Id;

                            if (PM.Supplier_Id != null)
                            {
                                search.Supplier_Id = PM.Supplier_Id;
                            }

                            if (PM.MatchedBy != null)
                            {
                                search.MatchedBy = PM.MatchedBy;
                            }

                            search.IsActive = PM.IsActive;
                            search.Edit_Date = PM.Edit_Date;
                            search.Edit_User = PM.Edit_User;

                            if (search.ProductType != PM.ProductType && !string.IsNullOrWhiteSpace(PM.ProductType))
                            {
                                length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => search.ProductType)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                                search.ProductType = CommonFunctions.SubString(PM.ProductType, length);
                            }

                            if (search.ProductName != PM.ProductName && !string.IsNullOrWhiteSpace(PM.ProductName))
                            {
                                isReMap = true;

                                length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => search.ProductName)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                                search.ProductName = CommonFunctions.SubString(PM.ProductName, length);

                                length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => search.HotelName_Tx)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                                search.HotelName_Tx = CommonFunctions.SubString(PM.HotelName_Tx, length);
                            }

                            if (search.address != PM.FullAddress && !string.IsNullOrWhiteSpace(PM.FullAddress))
                            {
                                isReMap = true;

                                length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => search.address)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                                search.address = CommonFunctions.SubString(PM.FullAddress, length);

                                length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => search.address_tx)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                                search.address_tx = CommonFunctions.SubString(PM.Address_tx, length);
                            }

                            if (search.Latitude != PM.Latitude && !string.IsNullOrWhiteSpace(PM.Latitude))
                            {
                                isReMap = true;

                                length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => search.Latitude)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                                search.Latitude = CommonFunctions.SubString(PM.Latitude, length);

                                length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => search.Latitude_Tx)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                                search.Latitude_Tx = PM.Latitude_Tx;
                            }

                            if (search.Longitude != PM.Longitude && !string.IsNullOrWhiteSpace(PM.Longitude))
                            {
                                isReMap = true;

                                length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => search.Longitude)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                                search.Longitude = CommonFunctions.SubString(PM.Longitude, length);

                                length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => search.Longitude_Tx)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                                search.Longitude_Tx = CommonFunctions.SubString(PM.Longitude_Tx, length);
                            }

                            if (search.TelephoneNumber != PM.TelephoneNumber && !string.IsNullOrWhiteSpace(PM.TelephoneNumber))
                            {
                                isReMap = true;

                                length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => search.TelephoneNumber)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                                search.TelephoneNumber = CommonFunctions.SubString(PM.TelephoneNumber, length);

                                length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => search.TelephoneNumber_tx)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                                search.TelephoneNumber_tx = CommonFunctions.SubString(PM.TelephoneNumber_tx, length);
                            }

                            if (search.Country_Id != PM.Country_Id && PM.Country_Id != null)
                            {
                                isReMap = true;
                                search.Country_Id = PM.Country_Id;
                            }

                            if (search.City_Id != PM.City_Id && PM.City_Id != null)
                            {
                                isReMap = true;
                                search.City_Id = PM.City_Id;
                            }

                            if (search.CityCode != PM.CityCode && !string.IsNullOrWhiteSpace(PM.CityCode))
                            {
                                isReMap = true;

                                length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => search.CityCode)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                                search.CityCode = CommonFunctions.SubString(PM.CityCode, length);
                            }

                            if (search.CityName != PM.CityName && !string.IsNullOrWhiteSpace(PM.CityName))
                            {
                                isReMap = true;

                                length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => search.CityName)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                                search.CityName = CommonFunctions.SubString(PM.CityName, length);
                            }

                            if (search.CountryName != PM.CountryName && !string.IsNullOrWhiteSpace(PM.CountryName))
                            {
                                isReMap = true;

                                length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => search.CountryName)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                                search.CountryName = CommonFunctions.SubString(PM.CountryName, length);
                            }

                            if (search.CountryCode != PM.CountryCode && !string.IsNullOrWhiteSpace(PM.CountryCode))
                            {
                                isReMap = true;

                                length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => search.CountryCode)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                                search.CountryCode = CommonFunctions.SubString(PM.CountryCode, length);
                            }

                            if (search.StateName != PM.StateName && !string.IsNullOrWhiteSpace(PM.StateName))
                            {
                                length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => search.StateName)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                                search.StateName = CommonFunctions.SubString(PM.StateName, length);
                            }

                            if (search.StateCode != PM.StateCode && !string.IsNullOrWhiteSpace(PM.StateCode))
                            {
                                length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => search.StateCode)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                                search.StateCode = CommonFunctions.SubString(PM.StateCode, length);
                            }

                            if (search.Email != PM.Email && !string.IsNullOrWhiteSpace(PM.Email))
                            {
                                length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => search.Email)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                                search.Email = CommonFunctions.SubString(PM.Email, length);
                            }

                            if (search.Fax != PM.Fax && !string.IsNullOrWhiteSpace(PM.Fax))
                            {
                                length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => search.Fax)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                                search.Fax = CommonFunctions.SubString(PM.Fax, length);
                            }

                            if (search.Website != PM.Website && !string.IsNullOrWhiteSpace(PM.Website))
                            {
                                length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => search.Website)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                                search.Website = CommonFunctions.SubString(PM.Website, length);
                            }

                            if (search.StarRating != PM.StarRating && !string.IsNullOrWhiteSpace(PM.StarRating))
                            {
                                length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => search.StarRating)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                                search.StarRating = CommonFunctions.SubString(PM.StarRating, length);
                            }

                            if (search.PostCode != PM.PostCode && !string.IsNullOrWhiteSpace(PM.PostCode))
                            {
                                length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => search.PostCode)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                                search.PostCode = CommonFunctions.SubString(PM.PostCode, length);
                            }

                            if (search.Street != PM.Street && !string.IsNullOrWhiteSpace(PM.Street))
                            {
                                length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => search.Street)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                                search.Street = CommonFunctions.SubString(PM.Street, length);
                            }

                            if (search.Street2 != PM.Street2 && !string.IsNullOrWhiteSpace(PM.Street2))
                            {
                                length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => search.Street2)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                                search.Street2 = CommonFunctions.SubString(PM.Street2, length);
                            }

                            if (search.Street3 != PM.Street3 && !string.IsNullOrWhiteSpace(PM.Street3))
                            {
                                length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => search.Street3)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                                search.Street3 = CommonFunctions.SubString(PM.Street3, length);
                            }

                            if (search.Street4 != PM.Street4 && !string.IsNullOrWhiteSpace(PM.Street4))
                            {
                                length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => search.Street4)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                                search.Street4 = CommonFunctions.SubString(PM.Street4, length);
                            }

                            #region This code is written as business don't require REMAP logic as of now and this logic needs to be improved further for better match result
                            isReMap = false;
                            #endregion

                            if (isReMap && search.Status == "MAPPED" && search.Accommodation_Id != null)
                            {
                                PM.Status = "REMAP";

                                if (string.IsNullOrWhiteSpace(PM.Remarks))
                                {
                                    PM.Remarks = "Supplier mandatory values changed. Please remap the record.";
                                }
                            }
                            else if (isReMap && search.Status == "AUTOMAPPED" && search.Accommodation_Id != null)
                            {
                                PM.Status = "UNMAPPED";

                                if (string.IsNullOrWhiteSpace(PM.Remarks))
                                {
                                    PM.Remarks = "Supplier mandatory values changed.";
                                }

                                sbUpdateSRTMStatus.Clear();
                                sbUpdateSRTMStatus.AppendLine(" UPDATE Accommodation_SupplierRoomTypeMapping SET MappingStatus='UNMAPPED' , Accommodation_Id = null , Accommodation_RoomInfo_Id=null , MatchingScore=null, ");
                                sbUpdateSRTMStatus.AppendLine(" Edit_User= '" + PM.Edit_User + "' , Edit_Date = getdate() ");
                                sbUpdateSRTMStatus.AppendLine(" Where Supplier_Id='" + search.Supplier_Id + "' and Accommodation_Id='" + search.Accommodation_Id + "' and SupplierProductId='" + search.SupplierProductReference + "';");
                            }

                            length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => search.Status)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                            search.Status = CommonFunctions.SubString(PM.Status, length);

                            length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => search.Remarks)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                            search.Remarks = CommonFunctions.SubString(PM.Remarks, length);

                            search.SupplierImportFile_Id = PM.SupplierImporrtFile_Id;
                            search.Batch = PM.Batch;
                            search.ReRun_SupplierImportFile_Id = PM.ReRunSupplierImporrtFile_Id;
                            search.ReRun_Batch = PM.ReRunBatch;

                            context.SaveChanges();

                        }

                        if (search == null)
                        {
                            if ((PM.Status == "AUTOMAPPED" || PM.Status == "MAPPED") && PM.Accommodation_Id != null)
                            {
                                sbUpdateSRTMStatus.Clear();
                                sbUpdateSRTMStatus.Append(" UPDATE Accommodation_SupplierRoomTypeMapping SET Accommodation_Id='" + PM.Accommodation_Id + "', ");
                                sbUpdateSRTMStatus.Append(" Edit_User= '" + PM.Edit_User + "', Edit_Date = getdate() ");
                                sbUpdateSRTMStatus.Append(" Where Supplier_Id='" + search.Supplier_Id + "' and SupplierProductId='" + search.SupplierProductReference + "';");
                            }

                            if (PM.Accommodation_ProductMapping_Id == Guid.Empty)
                            {
                                PM.Accommodation_ProductMapping_Id = Guid.NewGuid();
                            }

                            DataLayer.Accommodation_ProductMapping objNew = new Accommodation_ProductMapping();
                            objNew.Accommodation_ProductMapping_Id = PM.Accommodation_ProductMapping_Id;
                            objNew.Accommodation_Id = PM.Accommodation_Id;
                            objNew.Create_Date = PM.Create_Date;
                            objNew.Create_User = PM.Create_User;
                            objNew.SupplierId = PM.SupplierId;
                            objNew.Google_Place_Id = PM.Google_Place_Id;
                            objNew.IsActive = PM.IsActive;
                            objNew.SupplierProductReference = PM.SupplierProductReference;
                            objNew.Supplier_Id = PM.Supplier_Id;
                            objNew.Country_Id = PM.Country_Id;
                            objNew.City_Id = PM.City_Id;
                            objNew.SupplierImportFile_Id = PM.SupplierImporrtFile_Id;
                            objNew.Batch = PM.Batch;
                            objNew.ReRun_SupplierImportFile_Id = PM.ReRunSupplierImporrtFile_Id;
                            objNew.ReRun_Batch = PM.ReRunBatch;

                            length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => objNew.CityCode)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                            objNew.CityCode = CommonFunctions.SubString(PM.CityCode, length);

                            length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => objNew.CityName)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                            objNew.CityName = CommonFunctions.SubString(PM.CityName, length);

                            length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => objNew.CountryCode)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                            objNew.CountryCode = CommonFunctions.SubString(PM.CountryCode, length);

                            length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => objNew.CountryName)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                            objNew.CountryName = CommonFunctions.SubString(PM.CountryName, length);

                            length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => objNew.Email)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                            objNew.Email = CommonFunctions.SubString(PM.Email, length);

                            length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => objNew.Fax)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                            objNew.Fax = CommonFunctions.SubString(PM.Fax, length);

                            length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => objNew.Latitude)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                            objNew.Latitude = CommonFunctions.SubString(PM.Latitude, length);

                            length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => objNew.Longitude)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                            objNew.Longitude = CommonFunctions.SubString(PM.Longitude, length);

                            length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => objNew.PostCode)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                            objNew.PostCode = CommonFunctions.SubString(PM.PostCode, length);

                            length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => objNew.ProductName)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                            objNew.ProductName = CommonFunctions.SubString(PM.ProductName, length);

                            length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => objNew.Remarks)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                            objNew.Remarks = CommonFunctions.SubString(PM.Remarks, length);

                            length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => objNew.StarRating)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                            objNew.StarRating = CommonFunctions.SubString(PM.StarRating, length);

                            length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => objNew.StateCode)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                            objNew.StateCode = CommonFunctions.SubString(PM.StateCode, length);

                            length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => objNew.StateName)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                            objNew.StateName = CommonFunctions.SubString(PM.StateName, length);

                            length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => objNew.Status)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                            objNew.Status = CommonFunctions.SubString(PM.Status, length);

                            length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => objNew.Street)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                            objNew.Street = CommonFunctions.SubString(PM.Street, length);

                            length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => objNew.Street2)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                            objNew.Street2 = CommonFunctions.SubString(PM.Street2, length);

                            length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => objNew.Street3)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                            objNew.Street3 = CommonFunctions.SubString(PM.Street3, length);

                            length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => objNew.Street4)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                            objNew.Street4 = CommonFunctions.SubString(PM.Street4, length);

                            length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => objNew.SupplierName)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                            objNew.SupplierName = CommonFunctions.SubString(PM.SupplierName, length);

                            length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => objNew.TelephoneNumber)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                            objNew.TelephoneNumber = CommonFunctions.SubString(PM.TelephoneNumber, length);

                            length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => objNew.Website)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                            objNew.Website = CommonFunctions.SubString(PM.Website, length);

                            length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => objNew.address)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                            objNew.address = CommonFunctions.SubString(PM.FullAddress, length);

                            length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => objNew.Latitude_Tx)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                            objNew.Latitude_Tx = CommonFunctions.SubString(PM.Latitude_Tx, length);

                            length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => objNew.Longitude_Tx)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                            objNew.Longitude_Tx = CommonFunctions.SubString(PM.Longitude_Tx, length);

                            length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => objNew.HotelName_Tx)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                            objNew.HotelName_Tx = CommonFunctions.SubString(PM.HotelName_Tx, length);

                            length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => objNew.TelephoneNumber_tx)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                            objNew.TelephoneNumber_tx = CommonFunctions.SubString(PM.TelephoneNumber_tx, length);

                            length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => objNew.address_tx)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                            objNew.address_tx = CommonFunctions.SubString(PM.Address_tx, length);

                            length = columnLength.Where(x => x.COLUMN_NAME == CommonFunctions.GetPropertyName(() => objNew.ProductType)).Select(x => x.CHARACTER_MAXIMUM_LENGTH).FirstOrDefault();
                            objNew.ProductType = CommonFunctions.SubString(PM.ProductType, length);

                            context.Accommodation_ProductMapping.Add(objNew);
                            context.SaveChanges();

                        }

                        #region Change RoomType Mapping as Per acco Mapping status update

                        if (sbUpdateSRTMStatus.Length > 0)
                        {
                            try { context.Database.ExecuteSqlCommand(sbUpdateSRTMStatus.ToString()); } catch (Exception ex) { }
                        }

                        #endregion

                        #region  Push Updated Data in Mongo
                        if (PM.Accommodation_ProductMapping_Id != Guid.Empty)
                        {
                            DL_MongoPush _obj = new DL_MongoPush();
                            _obj.SyncHotelMapping(PM.Accommodation_ProductMapping_Id);
                            _obj.SyncHotelMappingLite(PM.Accommodation_ProductMapping_Id);
                        }
                        #endregion
                    }
                    catch (Exception e)
                    {
                        throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating accomodation product mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
                    }
                }

                if (SupplierImportFile_Id != Guid.Empty)
                {
                    string sql = "";
                    sql = "UPDATE Accommodation_ProductMapping Set GeoLocation = geography::Point(Latitude, Longitude, 4326) ";
                    sql = sql + " WHERE Latitude IS NOT NULL and Longitude IS NOT NULL ";
                    sql = sql + " AND TRY_CONVERT(float, Latitude) IS NOT NULL AND TRY_CONVERT(float, Longitude) IS NOT NULL ";
                    sql = sql + " AND (TRY_CONVERT(float, Latitude) >= -90 AND TRY_CONVERT(float, Latitude) <= 90) ";
                    sql = sql + " AND (TRY_CONVERT(float, Longitude) >= -180 AND TRY_CONVERT(float, Longitude) <= 180) ";
                    sql = sql + " AND ReRun_SupplierImportFile_Id = '" + SupplierImportFile_Id + "' AND ReRun_Batch = " + Batch + ";";
                    try { context.Database.ExecuteSqlCommand(sql); } catch (Exception ex) { }
                    //context.USP_UpdateMapID("product");
                }

                #region Update No Of Hits
                var updatableAliases = (from k in Keywords
                                        from ka in k.Alias
                                        where ka.NewHits != 0
                                        select ka).ToList();
                if (updatableAliases.Count > 0)
                {
                    using (DL_Masters objDL = new DL_Masters())
                    {
                        objDL.DataHandler_Keyword_Update_NoOfHits(updatableAliases);
                    }
                }

                #endregion

            }
            return true;
        }

        #endregion

        #region Supplier Room Type Mapping

        public DataContracts.DC_Message UpdateAccomodationSupplierRoomTypeMappingValues(List<DataContracts.Mapping.DC_Accommodation_SupplierRoomTypeMapping_Values> obj)
        {
            try
            {
                if (obj == null || obj.Count == 0)
                {
                    return new DC_Message { StatusCode = ReadOnlyMessage.StatusCode.Warning, StatusMessage = "Request message has no records." };
                }
                bool IsNotTrainingflag = true;

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    context.Database.CommandTimeout = 0;
                    IncomingWebRequestContext woc = WebOperationContext.Current.IncomingRequest;
                    string CallingAgent = woc.Headers["CallingAgent"];
                    string CallingUser = woc.Headers["CallingUser"];

                    //get all records for this supplier room record.
                    Guid _acoo_supRoomId = Guid.Parse(Convert.ToString(obj[0].Accommodation_SupplierRoomTypeMapping_Id));
                    var allaccoSuppRoomTypeMapVals = context.Accommodation_SupplierRoomTypeMapping_Values.Where(w => w.Accommodation_SupplierRoomTypeMapping_Id == _acoo_supRoomId).ToList();

                    foreach (DC_Accommodation_SupplierRoomTypeMapping_Values item in obj)
                    {
                        if (item.Accommodation_SupplierRoomTypeMapping_Id != null && item.Accommodation_RoomInfo_Id != null && !string.IsNullOrWhiteSpace(item.UserMappingStatus))
                        {
                            var accoSuppRoomTypeMapVal = allaccoSuppRoomTypeMapVals.Where(w => w.Accommodation_SupplierRoomTypeMapping_Id == item.Accommodation_SupplierRoomTypeMapping_Id && w.Accommodation_RoomInfo_Id == item.Accommodation_RoomInfo_Id).FirstOrDefault();
                            if (accoSuppRoomTypeMapVal != null)
                            {
                                accoSuppRoomTypeMapVal.Edit_User = CallingUser;
                                accoSuppRoomTypeMapVal.UserEditDate = DateTime.Now;
                                accoSuppRoomTypeMapVal.UserMappingStatus = item.UserMappingStatus;
                            }
                            else
                            {
                                Accommodation_SupplierRoomTypeMapping_Values dc = new Accommodation_SupplierRoomTypeMapping_Values();
                                dc.Accommodation_SupplierRoomTypeMapping_Value_Id = Guid.NewGuid();
                                dc.Accommodation_SupplierRoomTypeMapping_Id = item.Accommodation_SupplierRoomTypeMapping_Id;
                                dc.Accommodation_RoomInfo_Id = item.Accommodation_RoomInfo_Id;
                                dc.Create_User = CallingUser;
                                dc.Edit_User = CallingUser;
                                dc.Create_Date = DateTime.Now;
                                dc.UserEditDate = DateTime.Now;
                                dc.UserMappingStatus = item.UserMappingStatus;
                                context.Accommodation_SupplierRoomTypeMapping_Values.Add(dc);
                            }
                        }
                        IsNotTrainingflag = item.IsNotTraining ?? true;
                    }

                    if (context.ChangeTracker.HasChanges())
                    {
                        var AUTOMAPPED_CNT = allaccoSuppRoomTypeMapVals.Where(w => w.SystemMappingStatus == "AUTOMAPPED").Count();
                        var MAPPED_CNT = allaccoSuppRoomTypeMapVals.Where(w => w.UserMappingStatus == "MAPPED").Count();
                        var REVIEW_CNT = allaccoSuppRoomTypeMapVals.Where(w => w.SystemMappingStatus == "REVIEW" || w.UserMappingStatus == "REVIEW").Count();

                        var srtm = context.Accommodation_SupplierRoomTypeMapping.Find(obj[0].Accommodation_SupplierRoomTypeMapping_Id);

                        if (srtm != null)
                        {
                            if (AUTOMAPPED_CNT != 0 && srtm.MappingStatus != "AUTOMAPPED")
                            {
                                srtm.MappingStatus = "AUTOMAPPED";
                                srtm.Edit_User = CallingUser;
                                srtm.Edit_Date = DateTime.Now;
                            }
                            else if (AUTOMAPPED_CNT == 0 && MAPPED_CNT != 0 && srtm.MappingStatus != "MAPPED")
                            {
                                srtm.MappingStatus = "MAPPED";
                                srtm.Edit_User = CallingUser;
                                srtm.Edit_Date = DateTime.Now;
                            }
                            else if (AUTOMAPPED_CNT == 0 && MAPPED_CNT == 0 && REVIEW_CNT != 0 && srtm.MappingStatus != "REVIEW")
                            {
                                srtm.MappingStatus = "REVIEW";
                                srtm.Edit_User = CallingUser;
                                srtm.Edit_Date = DateTime.Now;
                            }
                            else if (AUTOMAPPED_CNT == 0 && MAPPED_CNT == 0 && REVIEW_CNT == 0 && srtm.MappingStatus != "UNMAPPED")
                            {
                                srtm.MappingStatus = "UNMAPPED";
                                srtm.Edit_User = CallingUser;
                                srtm.Edit_Date = DateTime.Now;
                            }
                        }

                        context.SaveChanges();


                    }

                    //Call Training Data To push 
                    if (!IsNotTrainingflag)
                    {
                        DeleteOrSendTraingData(Guid.Parse(Convert.ToString(obj[0].Accommodation_SupplierRoomTypeMapping_Id)), IsNotTrainingflag);
                    }
                }
                return new DataContracts.DC_Message { StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Success, StatusMessage = "All Valid Records are successfully updated." };
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating accomodation product supplier mapping" + ex.Message, ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public DataContracts.DC_Message UpdateAccomodationSupplierRoomTypeMapping_TrainingFlag(List<DataContracts.Mapping.DC_Accommodation_SupplierRoomTypeMap_Update> obj)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    context.Database.CommandTimeout = 0;
                    IncomingWebRequestContext woc = WebOperationContext.Current.IncomingRequest;
                    string CallingAgent = woc.Headers["CallingAgent"];
                    string CallingUser = woc.Headers["CallingUser"];

                    foreach (var item in obj)
                    {
                        var accoSuppRoomTypeMap = context.Accommodation_SupplierRoomTypeMapping.Find(item.Accommodation_SupplierRoomTypeMapping_Id);
                        if (accoSuppRoomTypeMap != null)
                        {
                            //If IsNotTraining is true & system falg is true then delete else do nothing
                            DeleteOrSendTraingData(item.Accommodation_SupplierRoomTypeMapping_Id, item.IsNotTraining);
                            accoSuppRoomTypeMap.IsNotTraining = item.IsNotTraining;
                            accoSuppRoomTypeMap.Edit_User = item.Edit_User ?? CallingUser;
                            accoSuppRoomTypeMap.Edit_Date = DateTime.Now;
                        }
                    }
                    context.SaveChanges();
                }
                return new DataContracts.DC_Message { StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Success, StatusMessage = "Flag Updated successfully." };
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating Training flag" + ex.Message, ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }


        public void DeleteOrSendTraingData(Guid Accommodation_SupplierRoomTypeMapping_Id, bool IsNotTraining)
        {
            string strURI;
            string baseAddress = Convert.ToString(OperationContext.Current.Host.BaseAddresses[0]);

            if (IsNotTraining)
            {
                //Calling Delete API is located
                strURI = string.Format(baseAddress + System.Configuration.ConfigurationManager.AppSettings["MLSVCURL_DataApi_RoomTypeMatching_DeleteTrainingData"] + Accommodation_SupplierRoomTypeMapping_Id.ToString());
            }
            else
            {
                //Sent releavent data to training system. string baseAddress = Convert.ToString(OperationContext.Current.Host.BaseAddresses[0]);
                strURI = string.Format(baseAddress + System.Configuration.ConfigurationManager.AppSettings["MLSVCURL_DataApi_RoomTypeMatching_TrainingDataPushToAIML"] + Accommodation_SupplierRoomTypeMapping_Id.ToString());
            }
            using (DHSVCProxyAsync DHP = new DHSVCProxyAsync())
            {
                DHP.GetAsync(ProxyFor.MachingLearningDataTransfer, strURI);
            }
        }


        public IList<DataContracts.Mapping.DC_SupplierRoomTypeAttributes> GetAttributesForAccomodationSupplierRoomTypeMapping(Guid SupplierRoomtypeMappingID)
        {
            List<DC_SupplierRoomTypeAttributes> lstAttributes = new List<DC_SupplierRoomTypeAttributes>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    lstAttributes = (from asrta in context.Accommodation_SupplierRoomTypeAttributes
                                     join k in context.m_keyword
                                     on asrta.SystemAttributeKeyword_Id equals k.Keyword_Id
                                     where asrta.RoomTypeMap_Id == SupplierRoomtypeMappingID
                                     select new DC_SupplierRoomTypeAttributes
                                     {
                                         Accommodation_SupplierRoomTypeMap_Id = asrta.RoomTypeMapAttribute_Id,
                                         SupplierRoomTypeAttribute = asrta.SupplierRoomTypeAttribute,
                                         SystemAttributeKeyword = asrta.SystemAttributeKeyword,
                                         SystemAttributeKeyword_Id = asrta.SystemAttributeKeyword_Id,
                                         IconClass = k.Icon
                                     }).ToList();
                }
                return lstAttributes;
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating accomodation product supplier mapping attributes", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public IList<DataContracts.Mapping.DC_Accomodation_SupplierRoomTypeMapping> GetAccomodationSupplierRoomTypeMapping(int PageNo, int PageSize, Guid Accomodation_Id, Guid Supplier_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var prodSupMapSearch = from a in context.Accommodation_SupplierRoomTypeMapping
                                           where a.Accommodation_Id == Accomodation_Id
                                           select a;

                    if (Supplier_Id != Guid.Empty)
                    {
                        prodSupMapSearch = from a in prodSupMapSearch
                                           where a.Supplier_Id == Supplier_Id
                                           select a;
                    }

                    int total;

                    total = prodSupMapSearch.Count();

                    var skip = PageSize * PageNo;

                    var canPage = skip < total;

                    if (!canPage)
                        return null;

                    var prodSupMapList = (from a in prodSupMapSearch
                                          orderby a.Accommodation_SupplierRoomTypeMapping_Id
                                          select new DataContracts.Mapping.DC_Accomodation_SupplierRoomTypeMapping
                                          {
                                              Accommodation_SupplierRoomTypeMapping_Id = a.Accommodation_SupplierRoomTypeMapping_Id,
                                              Supplier_Id = a.Supplier_Id,
                                              Accommodation_Id = a.Accommodation_Id,
                                              //Commented for table structure change 
                                              ////Accommodation_RoomInfo_Id = a.Accommodation_RoomInfo_Id,
                                              ////Amenities = a.Amenities,
                                              ////BathRoomType = a.BATHROOMTYPE,
                                              ////BedTypeCode = a.BEDTYPECODE,
                                              ////Description = a.ROOM_DESCRIPTION,
                                              ////FloorName = a.FLOOR_NAME,
                                              ////FloorNumber = a.FLOORNUMBER,
                                              ////MaxAdults = a.MAX_ADULTS,
                                              ////MaxChild = a.MAX_CHILD,
                                              ////MaxGuest = a.MAX_GUEST_OCCUPANCY,
                                              ////MaxInfant = a.MAX_INFANT,
                                              ////Quantity = a.QUANTITY,
                                              ////RoomLocationCode = a.RoomLocationCode,
                                              ////RoomTypeCode = a.ROOMTYPECODE,
                                              ////RoomViewCode = a.ROOMVIEWCODE,
                                              //SizeMeasurement = a.SizeMeasurement,
                                              //SupplierHotelCode = a.SupplierHotelCode,
                                              //SupplierHotelName = a.SupplierHotelName,
                                              SupplierName = a.SupplierName,
                                              //SupplierProvider = a.SupplierProvider,
                                              //SupplierRoomCategory = a.SupplierRoomCategory,
                                              //SupplierRoomId = a.SupplierRoomId,
                                              //SupplierRoomTypeCode = a.SupplierRoomTypeCode,
                                          }).Skip(skip).Take(PageSize);

                    return prodSupMapList.ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching accomodation product supplier mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool UpdateAccomodationSupplierRoomTypeMapping(DataContracts.Mapping.DC_Accomodation_SupplierRoomTypeMapping obj)
        {
            if (obj.Accommodation_SupplierRoomTypeMapping_Id == null || obj.Accommodation_RoomInfo_Id == null)
            {
                return false;
            }

            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = context.Accommodation_SupplierRoomTypeMapping.Find(obj.Accommodation_SupplierRoomTypeMapping_Id);
                    if (search != null)
                    {
                        //if (obj. != (search.IsActive ?? true))
                        //{
                        //    search.IsActive = PM.IsActive;
                        //    search.Edit_Date = PM.Edit_Date;
                        //    search.Edit_User = PM.Edit_User;
                        //}
                        //else if (PM.Status != (search.Status ?? string.Empty))
                        //{
                        //    search.Status = PM.Status;
                        //    search.Edit_Date = PM.Edit_Date;
                        //    search.Edit_User = PM.Edit_User;
                        //}
                        //else
                        //{
                        // search.Accommodation_RoomInfo_Id = obj.Accommodation_RoomInfo_Id;
                        //}
                        context.SaveChanges();
                    }

                }

                return true;
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus
                {
                    ErrorMessage = "Error while updating accomodation product supplier mapping",
                    ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError
                });
            }
        }

        public void CheckRoomTypeAlreadyExist(Guid File_Id, Guid CurSupplier_Id, List<DataContracts.STG.DC_stg_SupplierHotelRoomMapping> stg, out List<DC_Accommodation_SupplierRoomTypeMap_SearchRS> updateMappingList, out List<DataContracts.STG.DC_stg_SupplierHotelRoomMapping> insertSTGList)
        {
            #region Dynamic Query
            StringBuilder sbSelect = new StringBuilder();
            StringBuilder sbFrom = new StringBuilder();
            StringBuilder sbJoin = new StringBuilder();

            StringBuilder sbWhere = new StringBuilder();
            try
            {
                string strSuppliercode = string.Empty;
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    strSuppliercode = Convert.ToString((from x in context.Supplier where x.Supplier_Id == CurSupplier_Id select x.Code).FirstOrDefault());
                }
                #region Select
                sbSelect.Append(@"SELECT 
	                                ASTM.SupplierProductName As ProductName,	ASTM.Accommodation_Id As Accommodation_Id,
	                                ASTM.SupplierName AS SupplierName,	ASTM.Accommodation_RoomInfo_Id AS Accommodation_RoomInfo_Id,
	                                ASTM.SupplierRoomName AS Accommodation_RoomInfo_Name,	SHRM.RoomName AS SupplierRoomName,
	                                ASTM.SupplierRoomName AS OldSupplierRoomName,	ASTM.Accommodation_SupplierRoomTypeMapping_Id AS Accommodation_SupplierRoomTypeMapping_Id,
	                                ASTM.SupplierProductId AS SupplierProductId,	ASTM.MaxAdults AS MaxAdults,
	                                ASTM.MaxChild AS MaxChild,	ASTM.MapId AS MaxChild,
	                                ASTM.MappingStatus AS MappingStatus,	ASTM.MaxGuestOccupancy AS MaxGuestOccupancy,
	                                ASTM.MaxInfants AS MaxInfants,	ASTM.Quantity AS Quantity,
	                                ASTM.SupplierRoomCategory AS SupplierRoomCategory,	ASTM.RatePlan AS RatePlan,
	                                ASTM.RatePlanCode AS RatePlanCode,	ASTM.SupplierProductName AS SupplierProductName,
	                                ASTM.SupplierRoomCategoryId AS SupplierRoomCategoryId,	ASTM.SupplierRoomId AS SupplierRoomId,
	                                ASTM.SupplierRoomTypeCode AS SupplierRoomTypeCode,	ASTM.Supplier_Id AS Supplier_Id,
	                                ASTM.Tx_ReorderedName AS Tx_ReorderedName,	ASTM.TX_RoomName AS TX_RoomName,
	                                ASTM.Tx_StrippedName AS Tx_StrippedName, ASTM.RoomDescription AS RoomDescription,
	                                SHRM.stg_SupplierHotelRoomMapping_Id AS stg_SupplierHotelRoomMapping_Id,
	                                ASTM.stg_SupplierHotelRoomMapping_Id AS Oldstg_SupplierHotelRoomMapping_Id,
	                                ASTM.RoomSize AS RoomSize,	ISNULL(ASTM.SupplierImportFile_Id , CAST(0x0 AS UNIQUEIDENTIFIER)) AS SupplierImporrtFile_Id,
	                                ISNULL(ASTM.Batch ,'0') AS Batch,
	                                ISNULL(ASTM.ReRun_SupplierImportFile_Id, CAST(0x0 AS UNIQUEIDENTIFIER)) AS ReRunSupplierImporrtFile_Id,
	                                ISNULL(ASTM.ReRun_Batch,'0') AS ReRunBatch,
	                                CASE WHEN ASTM.SupplierRoomName != SHRM.RoomName THEN 'UPDATE' ELSE '' END AS ActionType");
                #endregion
                sbFrom.Append(" FROM Accommodation_SupplierRoomTypeMapping ASTM WITH (NOLOCK) ");
                sbJoin.Append(" Join stg_SupplierHotelRoomMapping SHRM  WITH (NOLOCK) ON ");
                sbJoin.Append(@"    ASTM.Supplier_Id = SHRM.Supplier_Id 
	                            AND ASTM.SupplierRoomTypeCode = SHRM.SupplierRoomTypeCode 
	                            AND ASTM.SupplierProductId = SHRM.SupplierProductId ");

                if (strSuppliercode != string.Empty && strSuppliercode.ToLower() == "gta")
                {
                    sbJoin.Append(" AND ISNULL(ASTM.CityCode,ASTM.CityName) = ISNULL(SHRM.CityCode ,SHRM.CityName) ");
                    sbJoin.Append(" AND ISNULL(ASTM.CountryCode ,ASTM.CountryName) = ISNULL(SHRM.CountryCode,SHRM.CountryName) ");
                }
                else if (strSuppliercode != string.Empty && strSuppliercode.ToLower() == "mansley")
                {
                    sbJoin.Append(" AND ASTM.BedTypeCode = SHRM.BedTypeCode ");
                }
                sbWhere.Append(" WHERE ASTM.Supplier_Id = '" + CurSupplier_Id + "' ");
                sbWhere.Append("AND SHRM.SupplierImportFile_Id = '" + File_Id + "' ");

                StringBuilder sbfinalquery = new StringBuilder();
                sbfinalquery.Append(sbSelect); sbfinalquery.Append(" ");
                sbfinalquery.Append(sbFrom); sbfinalquery.Append(" ");
                sbfinalquery.Append(sbJoin); sbfinalquery.Append(" ");
                sbfinalquery.Append(sbWhere);

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    context.Configuration.AutoDetectChangesEnabled = false;
                    var toUpdate = context.Database.SqlQuery<DataContracts.Mapping.DC_Accommodation_SupplierRoomTypeMap_SearchRS>(sbfinalquery.ToString()).ToList();
                    insertSTGList = stg.Where(w => !toUpdate.Any(a => a.SupplierProductId == w.SupplierProductId && a.SupplierRoomTypeCode == w.SupplierRoomTypeCode)).ToList();
                    updateMappingList = toUpdate.Where(w => w.SupplierRoomName != w.OldSupplierRoomName).ToList();

                }

            }
            catch (Exception ex)
            {
                LogErrorMessage(File_Id, ex, "CheckRoomTypeAlreadyExist", "DL_Mapping", "CheckRoomTypeAlreadyExist", (int)Error_Enums_DataHandler.ErrorCodes.RoomTypeConfig_Generic, "", "CheckRoomTypeAlreadyExist failed");
                throw;
            }

            #endregion
        }

        public void CheckRoomTypeAlreadyExist_Old(Guid File_Id, Guid CurSupplier_Id, List<DataContracts.STG.DC_stg_SupplierHotelRoomMapping> stg, out List<DC_Accommodation_SupplierRoomTypeMap_SearchRS> updateMappingList, out List<DataContracts.STG.DC_stg_SupplierHotelRoomMapping> insertSTGList)
        {
            bool ret = false;

            using (ConsumerEntities context = new ConsumerEntities())
            {
                //var prodMapList = context.Accommodation_ProductMapping.AsNoTracking().Where(w => stg.Any(a => a.ProductId == w.SupplierProductReference)).Select(s => s.SupplierProductReference ).ToList();
                var accomap = (from a in context.Accommodation_SupplierRoomTypeMapping
                               where a.Supplier_Id == CurSupplier_Id
                               select a);
                var toUpdate = (from a in accomap
                                    //join s in stg on a.SupplierProductReference equals s.ProductId
                                join s in context.stg_SupplierHotelRoomMapping on new { Supplier_Id = a.Supplier_Id, SupplierProductReference = a.SupplierRoomTypeCode, ProductId = a.SupplierProductId, CityCode = (a.CityCode ?? a.CityName), CountryCode = (a.CountryCode ?? a.CountryName) }
                                equals new { Supplier_Id = s.Supplier_Id, SupplierProductReference = s.SupplierRoomTypeCode, ProductId = s.SupplierProductId, CityCode = (s.CityCode ?? s.CityName), CountryCode = (s.CountryCode ?? s.CountryName) }
                                where s.SupplierImportFile_Id == File_Id
                                select new DataContracts.Mapping.DC_Accommodation_SupplierRoomTypeMap_SearchRS
                                {
                                    ProductName = a.SupplierProductName,
                                    Accommodation_Id = a.Accommodation_Id,
                                    SupplierName = a.SupplierName,
                                    Accommodation_RoomInfo_Id = a.Accommodation_RoomInfo_Id,
                                    Accommodation_RoomInfo_Name = a.SupplierRoomName,
                                    SupplierRoomName = s.RoomName,
                                    OldSupplierRoomName = a.SupplierRoomName,
                                    Accommodation_SupplierRoomTypeMapping_Id = a.Accommodation_SupplierRoomTypeMapping_Id,
                                    SupplierProductId = a.SupplierProductId,
                                    MaxAdults = a.MaxAdults,
                                    MaxChild = a.MaxChild,
                                    MapId = a.MapId,
                                    MappingStatus = a.MappingStatus,
                                    MaxGuestOccupancy = a.MaxGuestOccupancy,
                                    MaxInfants = a.MaxInfants,
                                    Quantity = a.Quantity,
                                    SupplierRoomCategory = a.SupplierRoomCategory,
                                    RatePlan = a.RatePlan,
                                    RatePlanCode = a.RatePlanCode,
                                    SupplierProductName = a.SupplierProductName,
                                    SupplierRoomCategoryId = a.SupplierRoomCategoryId,
                                    SupplierRoomId = a.SupplierRoomId,
                                    SupplierRoomTypeCode = a.SupplierRoomTypeCode,
                                    Supplier_Id = a.Supplier_Id,
                                    Tx_ReorderedName = a.Tx_ReorderedName,
                                    TX_RoomName = a.TX_RoomName,
                                    Tx_StrippedName = a.Tx_StrippedName,
                                    RoomDescription = a.RoomDescription,
                                    stg_SupplierHotelRoomMapping_Id = s.stg_SupplierHotelRoomMapping_Id,
                                    Oldstg_SupplierHotelRoomMapping_Id = a.stg_SupplierHotelRoomMapping_Id,
                                    ActionType = (a.SupplierRoomName != s.RoomName) ? "UPDATE" : "",
                                    RoomSize = a.RoomSize,
                                    SupplierImporrtFile_Id = a.SupplierImportFile_Id ?? Guid.Empty,
                                    Batch = a.Batch ?? 0,
                                    ReRunSupplierImporrtFile_Id = a.ReRun_SupplierImportFile_Id ?? Guid.Empty,
                                    ReRunBatch = a.ReRun_Batch ?? 0

                                    //stg_AccoMapping_Id = (a.ProductName != s.ProductName) ? s.stg_AccoMapping_Id : Guid.Empty
                                }).ToList();


                insertSTGList = stg.Where(w => !toUpdate.Any(a => a.SupplierProductId == w.SupplierProductId && a.SupplierRoomTypeCode == w.SupplierRoomTypeCode)).ToList();
                updateMappingList = toUpdate.Where(w => w.SupplierRoomName != w.OldSupplierRoomName).ToList();
            }
        }

        public bool RoomTypeMappingMatch(DataContracts.Masters.DC_Supplier obj)
        {
            try
            {


                bool ret = true;
                Guid File_Id = new Guid();
                File_Id = Guid.Parse(obj.File_Id.ToString());
                PLog.SupplierImportFileProgress_Id = Guid.NewGuid();
                PLog.SupplierImportFile_Id = obj.File_Id;
                PLog.Step = "MAP";
                PLog.Status = "MAPPING";
                PLog.CurrentBatch = obj.CurrentBatch ?? 0;
                PLog.TotalBatch = obj.TotalBatch ?? 0;
                DL_UploadStaticData staticdata = new DL_UploadStaticData();
                List<DC_SupplierImportFileDetails> file = new List<DC_SupplierImportFileDetails>();
                DC_SupplierImportFileDetails_RQ fileRQ = new DC_SupplierImportFileDetails_RQ();
                fileRQ.SupplierImportFile_Id = File_Id;
                file = staticdata.GetStaticDataFileDetail(fileRQ);
                if (obj != null)
                {
                    string CurSupplierName = obj.Name;
                    Guid CurSupplier_Id = Guid.Parse(obj.Supplier_Id.ToString());

                    List<DataContracts.STG.DC_stg_SupplierHotelRoomMapping> clsSTGHotel = new List<DataContracts.STG.DC_stg_SupplierHotelRoomMapping>();
                    List<DataContracts.STG.DC_stg_SupplierHotelRoomMapping> clsSTGHotelInsert = new List<DataContracts.STG.DC_stg_SupplierHotelRoomMapping>();
                    List<DC_Accommodation_SupplierRoomTypeMap_SearchRS> clsMappingHotel = new List<DC_Accommodation_SupplierRoomTypeMap_SearchRS>();

                    //CallLogVerbose(File_Id, "MAP", "Fetching Staged Room Types.", obj.CurrentBatch);
                    DataContracts.STG.DC_stg_SupplierHotelRoomMapping_RQ RQ = new DataContracts.STG.DC_stg_SupplierHotelRoomMapping_RQ();
                    RQ.SupplierName = CurSupplierName;
                    RQ.Supplier_Id = CurSupplier_Id;
                    RQ.PageNo = 0;
                    RQ.PageSize = int.MaxValue;
                    RQ.SupplierImportFile_Id = File_Id;
                    //Getting Stg Data 
                    clsSTGHotel = staticdata.GetSTGRoomTypeData(RQ);
                    PLog.PercentageValue = 15;
                    USD.AddStaticDataUploadProcessLog(PLog);

                    //CallLogVerbose(File_Id, "MAP", "Fetching Existing Mapping Data.", obj.CurrentBatch);
                    /*DC_Accommodation_SupplierRoomTypeMap_SearchRQ RQM = new DC_Accommodation_SupplierRoomTypeMap_SearchRQ();
                    if (CurSupplier_Id != Guid.Empty)
                        RQM.Supplier_Id = CurSupplier_Id;
                    if (!string.IsNullOrWhiteSpace(CurSupplierName))
                        RQM.SupplierName = CurSupplierName;
                    RQM.PageNo = 0;
                    RQM.PageSize = int.MaxValue;
                    RQM.CalledFromTLGX = "TLGX";
                    clsMappingHotel = SupplierRoomTypeMapping_Search(RQM);*/

                    CheckRoomTypeAlreadyExist(File_Id, CurSupplier_Id, clsSTGHotel, out clsMappingHotel, out clsSTGHotelInsert);
                    PLog.PercentageValue = 26;
                    USD.AddStaticDataUploadProcessLog(PLog);

                    /*CallLogVerbose(File_Id, "MAP", "Updating Existing Room Types.", obj.CurrentBatch);
                    clsMappingHotel = clsMappingHotel.Select(c =>
                    {
                        c.SupplierRoomName = (clsSTGHotel
                        .Where(s => s.SupplierProductId == c.SupplierProductId && s.SupplierRoomTypeCode == c.SupplierRoomTypeCode && s.SupplierRoomId == c.SupplierRoomId)
                        .Select(s1 => s1.RoomName)
                        .FirstOrDefault()
                        ) ?? c.SupplierRoomName;
                        //c.Edit_Date = DateTime.Now;
                        //c.Edit_User = "TLGX_DataHandler";
                        c.stg_SupplierHotelRoomMapping_Id = (clsSTGHotel
                        .Where(s => s.SupplierProductId == c.SupplierProductId && s.SupplierRoomTypeCode == c.SupplierRoomTypeCode && s.SupplierRoomId == c.SupplierRoomId)
                        .Select(s1 => s1.stg_SupplierHotelRoomMapping_Id)
                        .FirstOrDefault()
                        ); //?? c.stg_SupplierHotelRoomMapping_Id;
                        c.ActionType = "UPDATE";
                        return c;
                    }).ToList();*/

                    //List<DataContracts.STG.DC_STG_Mapping_Table_Ids> lstobj = new List<DataContracts.STG.DC_STG_Mapping_Table_Ids>();
                    //lstobj.InsertRange(lstobj.Count, clsMappingHotel.Where(a => a.stg_SupplierHotelRoomMapping_Id != null && a.ActionType == "UPDATE"
                    //    && (a.stg_SupplierHotelRoomMapping_Id ?? Guid.Empty) != Guid.Empty).Select
                    //   (g => new DataContracts.STG.DC_STG_Mapping_Table_Ids
                    //   {
                    //       STG_Mapping_Table_Id = Guid.NewGuid(),
                    //       File_Id = obj.File_Id,
                    //       STG_Id = g.stg_SupplierHotelRoomMapping_Id,
                    //       Mapping_Id = g.Accommodation_SupplierRoomTypeMapping_Id,
                    //       Batch = obj.CurrentBatch ?? 0
                    //   }));


                    //PLog.PercentageValue = 37;
                    //USD.AddStaticDataUploadProcessLog(PLog);

                    // CallLogVerbose(File_Id, "MAP", "Checking for New Room Types in File.", obj.CurrentBatch);
                    //clsSTGHotelInsert = clsSTGHotel.Where(p => !clsMappingHotel.Any(p2 => (p2.SupplierName.ToString().Trim().ToUpper() == p.SupplierName.ToString().Trim().ToUpper())
                    // && (
                    //    (((p2.SupplierProductName ?? string.Empty).ToString().Trim().ToUpper() == (p.SupplierProductName ?? string.Empty).ToString().Trim().ToUpper()))
                    //    && (((p2.SupplierProductId ?? string.Empty).ToString().Trim().ToUpper() == (p.SupplierProductId ?? string.Empty).ToString().Trim().ToUpper()))
                    //    && (((p2.SupplierRoomTypeCode ?? string.Empty).ToString().Trim().ToUpper() == (p.SupplierRoomTypeCode ?? string.Empty).ToString().Trim().ToUpper()))
                    //    && (((p2.SupplierRoomId ?? string.Empty).ToString().Trim().ToUpper() == (p.SupplierRoomId ?? string.Empty).ToString().Trim().ToUpper()))
                    //    && (((p2.SupplierRoomName ?? string.Empty).ToString().Trim().ToUpper() == (p.RoomName ?? string.Empty).ToString().Trim().ToUpper()))
                    //    && (((p2.SupplierRoomCategoryId ?? string.Empty).ToString().Trim().ToUpper() == (p.SupplierRoomCategoryId ?? string.Empty).ToString().Trim().ToUpper()))
                    //))).ToList();
                    PLog.PercentageValue = 48;
                    USD.AddStaticDataUploadProcessLog(PLog);

                    clsSTGHotel.RemoveAll(p => clsSTGHotelInsert.Any(p2 => (p2.stg_SupplierHotelRoomMapping_Id == p.stg_SupplierHotelRoomMapping_Id)));

                    //clsMappingHotel.RemoveAll(p => p.SupplierRoomName == p.OldSupplierRoomName && (((p.stg_SupplierHotelRoomMapping_Id == Guid.Empty) ? p.Oldstg_SupplierHotelRoomMapping_Id : p.stg_SupplierHotelRoomMapping_Id) == p.Oldstg_SupplierHotelRoomMapping_Id));

                    //CallLogVerbose(File_Id, "MAP", "Removing UnEdited Data.", obj.CurrentBatch);
                    clsMappingHotel.RemoveAll(p => p.SupplierRoomName == p.OldSupplierRoomName);

                    //PLog.PercentageValue = 53;
                    //USD.AddStaticDataUploadProcessLog(PLog);
                    //CallLogVerbose(File_Id, "MAP", "Inserting New Room Types.", obj.CurrentBatch);
                    clsMappingHotel.InsertRange(clsMappingHotel.Count, clsSTGHotelInsert.Select
                        (g => new DC_Accommodation_SupplierRoomTypeMap_SearchRS
                        {
                            Accommodation_SupplierRoomTypeMapping_Id = Guid.NewGuid(),
                            SupplierRoomName = g.RoomName,
                            OldSupplierRoomName = g.RoomName,
                            SupplierRoomCategoryId = g.SupplierRoomCategoryId,
                            Accommodation_Id = null,
                            Accommodation_RoomInfo_Id = null,
                            Accommodation_RoomInfo_Name = null,
                            CommonProductId = null,
                            Location = null,
                            MapId = null,
                            MappingStatus = "UNMAPPED",
                            MaxAdults = g.MaxAdults,
                            MaxChild = g.MaxChild,
                            MaxGuestOccupancy = g.MaxGuestOccupancy,
                            MaxInfants = g.MaxInfant,
                            ProductName = null,
                            Quantity = (!string.IsNullOrWhiteSpace(g.Quantity)) ? Convert.ToInt32(g.Quantity) : 0,
                            RatePlan = g.RatePlan,
                            RatePlanCode = g.RatePlanCode,
                            RoomTypeAttributes = null,
                            SupplierName = g.SupplierName,
                            SupplierProductId = g.SupplierProductId,
                            SupplierProductName = g.SupplierProductName,
                            SupplierRoomCategory = g.SupplierRoomCategory,
                            SupplierRoomId = g.SupplierRoomId,
                            SupplierRoomTypeCode = g.SupplierRoomTypeCode,
                            Supplier_Id = g.Supplier_Id,
                            stg_SupplierHotelRoomMapping_Id = g.stg_SupplierHotelRoomMapping_Id,
                            ActionType = "INSERT",
                            RoomSize = g.RoomSize,
                            SupplierImporrtFile_Id = obj.File_Id ?? Guid.Empty,
                            Batch = obj.CurrentBatch ?? 0,
                            ReRunSupplierImporrtFile_Id = obj.File_Id ?? Guid.Empty,
                            ReRunBatch = obj.CurrentBatch ?? 0,
                            RoomDescription = g.RoomDescription,
                            BathRoomType = g.BathRoomType,
                            RoomViewCode = g.RoomViewCode,
                            FloorName = g.FloorName,
                            FloorNumber = g.FloorNumber,
                            SupplierProvider = g.SupplierProvider,
                            Amenities = g.Amenities,
                            RoomLocationCode = g.RoomLocationCode,
                            ChildAge = g.ChildAge,
                            ExtraBed = g.ExtraBed,
                            Bedrooms = g.Bedrooms,
                            Smoking = g.Smoking,
                            BedTypeCode = g.BedTypeCode,
                            PromotionalVendorCode = g.PromotionalVendorCode,
                            BeddingConfig = g.BeddingConfig,
                            MinGuestOccupancy = g.MinGuestOccupancy,
                            CityCode = g.CityCode,
                            CityName = g.CityName,
                            StateCode = g.StateCode,
                            StateName = g.StateName,
                            CountryCode = g.CountryCode,
                            CountryName = g.CountryName

                            //Newly added 
                        }));

                    //lstobj.InsertRange(lstobj.Count, clsMappingHotel.Where(a => a.stg_SupplierHotelRoomMapping_Id != null && a.ActionType == "INSERT"
                    //    && (a.stg_SupplierHotelRoomMapping_Id ?? Guid.Empty) != Guid.Empty)
                    //.Select
                    //   (g => new DataContracts.STG.DC_STG_Mapping_Table_Ids
                    //   {
                    //       STG_Mapping_Table_Id = Guid.NewGuid(),
                    //       File_Id = obj.File_Id,
                    //       STG_Id = g.stg_SupplierHotelRoomMapping_Id,
                    //       Mapping_Id = g.Accommodation_SupplierRoomTypeMapping_Id,
                    //       Batch = obj.CurrentBatch ?? 0
                    //   }));
                    //bool idinsert = AddSTGMappingTableIDs(lstobj);

                    //PLog.PercentageValue = 58;
                    //USD.AddStaticDataUploadProcessLog(PLog);
                    CallLogVerbose(File_Id, "MAP", "Updating / Inserting Database.", obj.CurrentBatch);
                    if (clsMappingHotel.Count > 0)
                    {
                        ret = SupplierRoomTypeMapping_InsertUpdate(clsMappingHotel);
                        /*if (obj.CurrentBatch == 1)
                        {
                            DataContracts.UploadStaticData.DC_SupplierImportFile_Statistics objStat = new DC_SupplierImportFile_Statistics();
                            objStat.SupplierImportFile_Statistics_Id = Guid.NewGuid();
                            objStat.SupplierImportFile_Id = obj.File_Id;
                            objStat.FinalStatus = file[0].STATUS;
                            objStat.TotalRows = clsMappingHotel.Count;
                            objStat.Process_Date = DateTime.Now;
                            objStat.Process_User = file[0].PROCESS_USER;
                            DataContracts.DC_Message stat = USD.AddStaticDataUploadStatistics(objStat);
                        }*/
                    }
                }

                PLog.PercentageValue = 100;
                USD.AddStaticDataUploadProcessLog(PLog);
                CallLogVerbose(File_Id, "MAP", "MAP Process Completed", obj.CurrentBatch);
                return ret;
            }
            catch (Exception ex)
            {
                LogErrorMessage(obj.File_Id ?? Guid.Empty, ex, "RoomTypeMappingMatch", "DL_Mapping", "RoomTypeMappingMatch", (int)Error_Enums_DataHandler.ErrorCodes.RoomTypeConfig_Map, "", "RoomTypeMappingMatch failed for batch " + Convert.ToString(obj.CurrentBatch ?? 0));
                throw;
            }
        }

        public void DataHandler_RoomName_Attributes_Update(DC_SupplierRoomName_Details SRNDetails)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (SRNDetails.AttributeList != null && SRNDetails.RoomTypeMap_Id != null)
                    {
                        context.Accommodation_SupplierRoomTypeAttributes.AddRange((from a in SRNDetails.AttributeList
                                                                                   select new Accommodation_SupplierRoomTypeAttributes
                                                                                   {
                                                                                       RoomTypeMapAttribute_Id = Guid.NewGuid(),
                                                                                       RoomTypeMap_Id = SRNDetails.RoomTypeMap_Id,
                                                                                       SupplierRoomTypeAttribute = a.SupplierRoomTypeAttribute,
                                                                                       SystemAttributeKeyword = a.SystemAttributeKeyword,
                                                                                       SystemAttributeKeyword_Id = a.SystemAttributeKeywordID
                                                                                   }).ToList());
                        context.SaveChanges();
                    }

                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus
                {
                    ErrorMessage = "Error while inserting supplier room type mapping attribute list",
                    ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError
                });
            }
        }

        public List<DataContracts.Mapping.DC_Accommodation_SupplierRoomTypeMap_SearchRS> SupplierRoomTypeMapping_Search(DataContracts.Mapping.DC_Accommodation_SupplierRoomTypeMap_SearchRQ obj)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var roomTypeSearch = from a in context.Accommodation_SupplierRoomTypeMapping
                                         where a.Supplier_Id == (obj.Supplier_Id ?? a.Supplier_Id)
                                         && a.SupplierName.Trim().ToUpper() == (obj.SupplierName.Trim().ToUpper() ?? a.SupplierName.Trim().ToUpper())
                                         select new DC_Accommodation_SupplierRoomTypeMap_SearchRS
                                         {
                                             Accommodation_Id = a.Accommodation_Id,
                                             SupplierName = a.SupplierName,
                                             Accommodation_RoomInfo_Id = a.Accommodation_RoomInfo_Id,
                                             Accommodation_RoomInfo_Name = a.SupplierRoomName,
                                             SupplierRoomName = a.SupplierRoomName,
                                             OldSupplierRoomName = a.SupplierRoomName,
                                             Accommodation_SupplierRoomTypeMapping_Id = a.Accommodation_SupplierRoomTypeMapping_Id,
                                             SupplierProductId = a.SupplierProductId,
                                             MaxAdults = a.MaxAdults,
                                             MaxChild = a.MaxChild,
                                             MapId = a.MapId,
                                             MappingStatus = a.MappingStatus,
                                             MaxGuestOccupancy = a.MaxGuestOccupancy,
                                             MaxInfants = a.MaxInfants,
                                             Quantity = a.Quantity,
                                             SupplierRoomCategory = a.SupplierRoomCategory,
                                             RatePlan = a.RatePlan,
                                             RatePlanCode = a.RatePlanCode,
                                             SupplierProductName = a.SupplierProductName,
                                             SupplierRoomCategoryId = a.SupplierRoomCategoryId,
                                             SupplierRoomId = a.SupplierRoomId,
                                             SupplierRoomTypeCode = a.SupplierRoomTypeCode,
                                             Supplier_Id = a.Supplier_Id,
                                             Tx_ReorderedName = a.Tx_ReorderedName,
                                             TX_RoomName = a.TX_RoomName,
                                             Tx_StrippedName = a.Tx_StrippedName,
                                             RoomDescription = a.RoomDescription,
                                             stg_SupplierHotelRoomMapping_Id = a.stg_SupplierHotelRoomMapping_Id,
                                             Oldstg_SupplierHotelRoomMapping_Id = a.stg_SupplierHotelRoomMapping_Id,
                                             RoomSize = a.RoomSize,
                                             SupplierImporrtFile_Id = a.SupplierImportFile_Id ?? Guid.Empty,
                                             Batch = a.Batch ?? 0,
                                             ReRunSupplierImporrtFile_Id = a.ReRun_SupplierImportFile_Id ?? Guid.Empty,
                                             ReRunBatch = a.ReRun_Batch ?? 0

                                         };
                    var result = roomTypeSearch.ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus
                {
                    ErrorMessage = "Error while searching product supplier mapping",
                    ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError
                });
            }
        }

        public List<DataContracts.Mapping.DC_Accommodation_SupplierRoomTypeMap_SearchRS> AccomodationSupplierRoomTypeMapping_Search(DataContracts.Mapping.DC_Accommodation_SupplierRoomTypeMap_SearchRQ obj)
        {
            try
            {
                StringBuilder sbSelect = new StringBuilder();
                StringBuilder sbFrom = new StringBuilder();
                StringBuilder sbWhere = new StringBuilder();

                sbWhere.Append(" WHERE 1=1");

                if (obj.Supplier_Id != null)
                {
                    sbWhere.Append(" and asrtm.Supplier_Id='" + obj.Supplier_Id + "' ");
                }

                if (!string.IsNullOrWhiteSpace(obj.SupplierRoomName))
                {
                    sbWhere.Append(" and asrtm.SupplierRoomName like '%" + obj.SupplierRoomName + "%' ");
                }

                if (!string.IsNullOrWhiteSpace(obj.Status))
                {
                    sbWhere.Append(" and asrtm.MappingStatus='" + obj.Status + "' ");
                }

                if (!string.IsNullOrWhiteSpace(obj.ProductName))
                {
                    sbWhere.Append(" and acco.HotelName = '" + obj.ProductName + "' ");
                }

                if (obj.Country != null)
                {
                    sbWhere.Append(" and acco.Country_Id = '" + obj.Country + "' ");
                    sbWhere.Append(" and CoM.Country_Id = '" + obj.Country + "' ");
                }

                if (obj.City != null)
                {
                    sbWhere.Append(" and acco.City_Id = '" + obj.City + "' ");
                    sbWhere.Append(" and CiM.City_Id = '" + obj.City + "' ");
                }

                if (obj.CompanyHotelID != null)
                {
                    sbWhere.Append(" and acco.CompanyHotelID = " + obj.CompanyHotelID + " ");
                }

                if (obj.TLGXAccoId != null)
                {
                    sbWhere.Append(" and acco.TLGXAccoId = '" + obj.TLGXAccoId + "' ");
                }

                if (obj.TLGXAccoRoomId != null)
                {
                    sbWhere.Append(" and ari.TLGXAccoRoomId = '" + obj.TLGXAccoRoomId + "' ");
                }

                if (obj.Priority != null)
                {
                    sbWhere.Append(" and acco.Priority = " + obj.Priority.Value);
                }
                if (!string.IsNullOrWhiteSpace(obj.Source))
                {
                    sbWhere.Append(" and asrtm.Source = '" + obj.Source + "' ");
                }

                sbFrom.Append(@" FROM  [dbo].[Accommodation_SupplierRoomTypeMapping] AS  asrtm WITH (NOLOCK)
                                INNER JOIN [dbo].[Accommodation] AS acco WITH (NOLOCK) ON  asrtm.[Accommodation_Id] = acco.[Accommodation_Id] AND ISNULL(acco.IsActive,0) = 1
                                INNER JOIN [dbo].[m_CountryMaster] AS CoM WITH (NOLOCK) ON acco.[Country_Id] = CoM.[Country_Id]
                                INNER JOIN [dbo].[m_CityMaster] AS CiM WITH (NOLOCK) ON acco.[City_Id] = CiM.[City_Id] ");

                //LEFT OUTER JOIN[dbo].[Accommodation_RoomInfo] AS ari WITH(NOLOCK) ON(ari.[IsActive] = 1)
                //AND acco.[Accommodation_Id] = ari.[Accommodation_Id]
                //AND asrtm.[Accommodation_RoomInfo_Id] = ari.[Accommodation_RoomInfo_Id]

                int skip = 0;
                int total = 0;
                skip = obj.PageSize * obj.PageNo;

                StringBuilder sbsqlselectcount = new StringBuilder();
                sbsqlselectcount.Append("select count(1) ");
                sbsqlselectcount.Append(" " + sbFrom);
                sbsqlselectcount.Append(" " + sbWhere);

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    context.Configuration.AutoDetectChangesEnabled = false;
                    try { total = context.Database.SqlQuery<int>(sbsqlselectcount.ToString()).FirstOrDefault(); } catch (Exception ex) { }
                }

                List<DataContracts.Mapping.DC_Accommodation_SupplierRoomTypeMap_SearchRS> result = new List<DataContracts.Mapping.DC_Accommodation_SupplierRoomTypeMap_SearchRS>();

                if (total > 0)
                {
                    if (total <= skip)
                    {
                        int PageIndex = 0;
                        int intReminder = total % obj.PageSize;
                        int intQuotient = total / obj.PageSize;

                        if (intReminder > 0 || (intReminder == 0 && intQuotient == 0))
                        {
                            PageIndex = intQuotient;
                        }
                        else if (intReminder == 0 && intQuotient > 0)
                        {
                            PageIndex = intQuotient - 1;
                        }

                        skip = obj.PageSize * PageIndex;
                    }
                    //else
                    //    sbsqlselect.Append(Convert.ToString(obj.PageNo) + " As PageIndex ");

                    StringBuilder sbOrderby = new StringBuilder();
                    sbOrderby.Append(" ORDER BY asrtm.Accommodation_SupplierRoomTypeMapping_Id  ");
                    sbOrderby.Append(" OFFSET ");
                    sbOrderby.Append(skip.ToString());
                    sbOrderby.Append(" ROWS FETCH NEXT ");
                    sbOrderby.Append(obj.PageSize.ToString());
                    sbOrderby.Append(" ROWS ONLY ");

                    #region select Query
                    //sbSelect.Append(@" select asrtm.[Accommodation_Id] AS [Accommodation_Id],
                    // ari.[Accommodation_RoomInfo_Id] AS [Accommodation_RoomInfo_Id], 
                    // ari.[RoomName] AS  [Accommodation_RoomInfo_Name],
                    // ari.[RoomCategory] As [Accommodation_RoomInfo_Category],
                    // asrtm.[Accommodation_SupplierRoomTypeMapping_Id] AS [Accommodation_SupplierRoomTypeMapping_Id], 
                    // CAST(acco.[CompanyHotelID] AS nvarchar(200)) AS CommonProductId, 
                    // CiM.Name + '(' + CoM.Code + ')' As [Location],
                    // asrtm.[MapId] AS [MapId], 
                    // asrtm.[MappingStatus] AS [MappingStatus], 
                    // asrtm.[MaxAdults] AS [MaxAdults], 
                    // asrtm.[MaxChild] AS [MaxChild], 
                    // asrtm.[MaxGuestOccupancy] AS [MaxGuestOccupancy], 
                    // asrtm.[MaxInfants] AS [MaxInfants], 
                    //(Select count(1) from Accommodation_RoomInfo WITH (NOLOCK) where Accommodation_Id = asrtm.[Accommodation_Id]) As NumberOfRooms,
                    // acco.[HotelName] AS [ProductName],
                    // asrtm.[Quantity] AS [Quantity], 
                    // asrtm.[RatePlan] AS [RatePlan], 
                    // asrtm.[RatePlanCode] AS [RatePlanCode],
                    // --NULL AS RoomTypeAttributes, --Need to append
                    // asrtm.[SupplierName] AS [SupplierName], 
                    // asrtm.[SupplierProductId] AS [SupplierProductId], 
                    // asrtm.[SupplierProductName] AS [SupplierProductName], 
                    // asrtm.[SupplierRoomCategory] AS [SupplierRoomCategory], 
                    // asrtm.[SupplierRoomCategoryId] AS [SupplierRoomCategoryId], 
                    // asrtm.[SupplierRoomId] AS [SupplierRoomId],
                    // asrtm.[SupplierRoomName] AS [SupplierRoomName],  
                    // asrtm.[SupplierRoomTypeCode] AS [SupplierRoomTypeCode],
                    // asrtm.[Supplier_Id] AS [Supplier_Id], 
                    // asrtm.[Tx_ReorderedName] AS [Tx_ReorderedName], 
                    // asrtm.[TX_RoomName] AS [TX_RoomName], 
                    // asrtm.[RoomDescription] AS [RoomDescription],
                    // asrtm.[RoomSize] AS [RoomSize], 
                    // ISNULL(asrtm.[SupplierImportFile_Id],CAST(CAST(0 AS BINARY) AS UNIQUEIDENTIFIER)) AS [SupplierImportFile_Id],
                    // ISNULL(asrtm.[Batch],0) AS [Batch], 
                    // ISNULL(asrtm.[ReRun_SupplierImportFile_Id],CAST(CAST(0 AS BINARY) AS UNIQUEIDENTIFIER)) AS [ReRun_SupplierImportFile_Id], 
                    // ISNULL(asrtm.[ReRun_Batch],0) AS [ReRun_Batch], 
                    // asrtm.[BeddingConfig] AS [BeddingConfig], 
                    // asrtm.[PromotionalVendorCode] AS [PromotionalVendorCode], 
                    // asrtm.[MinGuestOccupancy] AS [MinGuestOccupancy],
                    // asrtm.[BedTypeCode] AS [BedTypeCode], 
                    // asrtm.[Smoking] AS [Smoking], 
                    // asrtm.[Bedrooms] AS [Bedrooms], 
                    // asrtm.[ExtraBed] AS [ExtraBed], 
                    // asrtm.[ChildAge] AS [ChildAge], 
                    // asrtm.[RoomLocationCode] AS [RoomLocationCode], 
                    // asrtm.[Amenities] AS [Amenities], 
                    // asrtm.[BathRoomType] AS [BathRoomType], 
                    // asrtm.[RoomViewCode] AS [RoomViewCode], 
                    // asrtm.[FloorName] AS [FloorName], 
                    // asrtm.[FloorNumber] AS [FloorNumber], 
                    // asrtm.[SupplierProvider] AS [SupplierProvider], 
                    // asrtm.[CityName] AS [CityName], 
                    // asrtm.[CityCode] AS [CityCode], 
                    // asrtm.[StateName] AS [StateName], 
                    // asrtm.[StateCode] AS [StateCode], 
                    // asrtm.[CountryName] AS [CountryName], 
                    // asrtm.[CountryCode] AS [CountryCode] , 
                    // CoM.[Country_Id] AS [Country_Id], 
                    // CoM.[Code] AS [Code], 
                    // CiM.[City_Id] AS [City_Id], 
                    // CiM.[Name] AS [Name],
                    //    Tx_ReorderedName = asrtm.Tx_ReorderedName,
                    //    TX_RoomName = asrtm.TX_RoomName,
                    //    Tx_StrippedName = asrtm.Tx_StrippedName, 
                    //    ISNULL(asrtm.IsNotTraining,0) as IsNotTraining, ");

                    sbSelect.Append(@" select asrtm.[Accommodation_Id] AS [Accommodation_Id],
	                    null AS [Accommodation_RoomInfo_Id], 
	                    null AS  [Accommodation_RoomInfo_Name],
                        null As [Accommodation_RoomInfo_Category],
	                    asrtm.[Accommodation_SupplierRoomTypeMapping_Id] AS [Accommodation_SupplierRoomTypeMapping_Id], 
	                    CAST(acco.[CompanyHotelID] AS nvarchar(200)) AS CommonProductId, 
	                    CiM.Name + '(' + CoM.Code + ')' As [Location],
	                    asrtm.[MapId] AS [MapId], 
	                    asrtm.[MappingStatus] AS [MappingStatus], 
	                    asrtm.[MaxAdults] AS [MaxAdults], 
	                    asrtm.[MaxChild] AS [MaxChild], 
	                    asrtm.[MaxGuestOccupancy] AS [MaxGuestOccupancy], 
	                    asrtm.[MaxInfants] AS [MaxInfants], 
	                   (Select count(1) from Accommodation_RoomInfo WITH (NOLOCK) where Accommodation_Id = asrtm.[Accommodation_Id]) As NumberOfRooms,
	                    acco.[HotelName] AS [ProductName],
	                    asrtm.[Quantity] AS [Quantity], 
	                    asrtm.[RatePlan] AS [RatePlan], 
	                    asrtm.[RatePlanCode] AS [RatePlanCode],
	                    --NULL AS RoomTypeAttributes, --Need to append
	                    asrtm.[SupplierName] AS [SupplierName], 
	                    asrtm.[SupplierProductId] AS [SupplierProductId], 
	                    asrtm.[SupplierProductName] AS [SupplierProductName], 
	                    asrtm.[SupplierRoomCategory] AS [SupplierRoomCategory], 
	                    asrtm.[SupplierRoomCategoryId] AS [SupplierRoomCategoryId], 
	                    asrtm.[SupplierRoomId] AS [SupplierRoomId],
	                    asrtm.[SupplierRoomName] AS [SupplierRoomName],  
	                    asrtm.[SupplierRoomTypeCode] AS [SupplierRoomTypeCode],
	                    asrtm.[Supplier_Id] AS [Supplier_Id], 
	                    asrtm.[Tx_ReorderedName] AS [Tx_ReorderedName], 
	                    asrtm.[TX_RoomName] AS [TX_RoomName], 
	                    asrtm.[RoomDescription] AS [RoomDescription],
	                    asrtm.[RoomSize] AS [RoomSize], 
	                    ISNULL(asrtm.[SupplierImportFile_Id],CAST(CAST(0 AS BINARY) AS UNIQUEIDENTIFIER)) AS [SupplierImportFile_Id],
	                    ISNULL(asrtm.[Batch],0) AS [Batch], 
	                    ISNULL(asrtm.[ReRun_SupplierImportFile_Id],CAST(CAST(0 AS BINARY) AS UNIQUEIDENTIFIER)) AS [ReRun_SupplierImportFile_Id], 
	                    ISNULL(asrtm.[ReRun_Batch],0) AS [ReRun_Batch], 
	                    asrtm.[BeddingConfig] AS [BeddingConfig], 
	                    asrtm.[PromotionalVendorCode] AS [PromotionalVendorCode], 
	                    asrtm.[MinGuestOccupancy] AS [MinGuestOccupancy],
	                    asrtm.[BedTypeCode] AS [BedTypeCode], 
	                    asrtm.[Smoking] AS [Smoking], 
	                    asrtm.[Bedrooms] AS [Bedrooms], 
	                    asrtm.[ExtraBed] AS [ExtraBed], 
	                    asrtm.[ChildAge] AS [ChildAge], 
	                    asrtm.[RoomLocationCode] AS [RoomLocationCode], 
	                    asrtm.[Amenities] AS [Amenities], 
	                    asrtm.[BathRoomType] AS [BathRoomType], 
	                    asrtm.[RoomViewCode] AS [RoomViewCode], 
	                    asrtm.[FloorName] AS [FloorName], 
	                    asrtm.[FloorNumber] AS [FloorNumber], 
	                    asrtm.[SupplierProvider] AS [SupplierProvider], 
	                    asrtm.[CityName] AS [CityName], 
	                    asrtm.[CityCode] AS [CityCode], 
	                    asrtm.[StateName] AS [StateName], 
	                    asrtm.[StateCode] AS [StateCode], 
	                    asrtm.[CountryName] AS [CountryName], 
	                    asrtm.[CountryCode] AS [CountryCode] , 
	                    CoM.[Country_Id] AS [Country_Id], 
	                    CoM.[Code] AS [Code], 
	                    CiM.[City_Id] AS [City_Id], 
	                    CiM.[Name] AS [Name],
                        Tx_ReorderedName = asrtm.Tx_ReorderedName,
                        TX_RoomName = asrtm.TX_RoomName,
                        Tx_StrippedName = asrtm.Tx_StrippedName, 
                        ISNULL(asrtm.IsNotTraining,0) as IsNotTraining, ");

                    sbSelect.Append(total + " AS TotalRecords ");

                    #endregion select Query

                    StringBuilder sbfinalQuery = new StringBuilder();
                    sbfinalQuery.Append(sbSelect + " ");
                    sbfinalQuery.Append(" " + sbFrom + " ");
                    sbfinalQuery.Append(" " + sbWhere + " ");
                    sbfinalQuery.Append(" " + sbOrderby);


                    // For RoomTypeAttributes
                    StringBuilder sbRoomTypeSelect = new StringBuilder();
                    sbRoomTypeSelect.Append(@"Select 
                                        asrta.RoomTypeMapAttribute_Id AS Accommodation_SupplierRoomTypeMapAttribute_Id,
                                        asrta.RoomTypeMap_Id AS Accommodation_SupplierRoomTypeMap_Id,
                                        asrta.SupplierRoomTypeAttribute AS SupplierRoomTypeAttribute,
                                        asrta.SystemAttributeKeyword AS SystemAttributeKeyword,
                                        asrta.SystemAttributeKeyword_Id AS SystemAttributeKeyword_Id,
                                        keyw.Icon AS IconClass");

                    StringBuilder sbRoomTypeJoin = new StringBuilder();
                    sbRoomTypeJoin.Append(@" FROM  [dbo].[Accommodation_SupplierRoomTypeAttributes] asrta WITH (NOLOCK) 
                                         INNER Join [dbo].[m_Keyword] Keyw WITH (NOLOCK) on Keyw.Keyword_Id = asrta.SystemAttributeKeyword_Id ");

                    StringBuilder sbRoomTypefinalQuery = new StringBuilder();
                    sbRoomTypefinalQuery.Append(sbRoomTypeSelect + " ");
                    sbRoomTypefinalQuery.Append(" " + sbRoomTypeJoin + " ");
                    sbRoomTypefinalQuery.Append(" where asrta.RoomTypeMap_Id IN ( ");

                    List<DataContracts.Mapping.DC_SupplierRoomTypeAttributes> resultRT = new List<DataContracts.Mapping.DC_SupplierRoomTypeAttributes>();
                    List<DC_SupplierRoomInfo_ForSuggestion> resultRinfo = new List<DC_SupplierRoomInfo_ForSuggestion>();
                    StringBuilder sbRoomTypeMapId = new StringBuilder();
                    StringBuilder sbAccommodationRoomInfoSelect = new StringBuilder();
                    StringBuilder sbAccoid = new StringBuilder();

                    sbAccommodationRoomInfoSelect.Append(" select Accommodation_RoomInfo_Id, RoomCategory, Accommodation_Id from Accommodation_RoomInfo where Accommodation_Id IN  ( ");

                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        context.Configuration.AutoDetectChangesEnabled = false;
                        try
                        {
                            result = context.Database.SqlQuery<DataContracts.Mapping.DC_Accommodation_SupplierRoomTypeMap_SearchRS>(sbfinalQuery.ToString()).ToList();

                            if (result != null && result.Count > 0)
                            {
                                foreach (var id in result)
                                {
                                    sbRoomTypeMapId.Append("'" + id.Accommodation_SupplierRoomTypeMapping_Id + "',");
                                    if (id.Accommodation_Id != null)
                                    {
                                        sbAccoid.Append("'" + id.Accommodation_Id + "',");
                                    }
                                }
                                sbRoomTypefinalQuery.Append(sbRoomTypeMapId.ToString().TrimEnd(',') + ")");
                                sbAccommodationRoomInfoSelect.Append(sbAccoid.ToString().TrimEnd(',') + ")");

                                sbAccoid = new StringBuilder(string.Join(",", sbAccoid.ToString().Split(',').Distinct()));

                                resultRT = context.Database.SqlQuery<DataContracts.Mapping.DC_SupplierRoomTypeAttributes>(sbRoomTypefinalQuery.ToString()).ToList();
                                resultRinfo = context.Database.SqlQuery<DataContracts.Mapping.DC_SupplierRoomInfo_ForSuggestion>(sbAccommodationRoomInfoSelect.ToString()).ToList();
                                foreach (var item in result)
                                {
                                    item.RoomTypeAttributes = resultRT.Where(w => w.Accommodation_SupplierRoomTypeMap_Id == item.Accommodation_SupplierRoomTypeMapping_Id).ToList();

                                    if (string.IsNullOrWhiteSpace(obj.CalledFromTLGX))
                                    {
                                        item.MappedRoomInfo = (from asrtmv in context.Accommodation_SupplierRoomTypeMapping_Values.AsNoTracking()
                                                               join ar in context.Accommodation_RoomInfo.AsNoTracking()
                                                               on asrtmv.Accommodation_RoomInfo_Id equals ar.Accommodation_RoomInfo_Id
                                                               where asrtmv.Accommodation_SupplierRoomTypeMapping_Id == item.Accommodation_SupplierRoomTypeMapping_Id
                                                               && (((asrtmv.SystemEditDate ?? DateTime.MinValue) > (asrtmv.UserEditDate ?? DateTime.MinValue)) ? (asrtmv.SystemMappingStatus ?? "UNMAPPED") : (asrtmv.UserMappingStatus ?? "UNMAPPED")) != "UNMAPPED"
                                                               select new DC_MappedRoomInfo
                                                               {
                                                                   Accommodation_RoomInfo_Category = ar.RoomCategory,
                                                                   Accommodation_RoomInfo_Id = asrtmv.Accommodation_RoomInfo_Id ?? Guid.Empty,
                                                                   Accommodation_RoomInfo_Name = ar.RoomName,
                                                                   Accommodation_SupplierRoomTypeMap_Id = asrtmv.Accommodation_SupplierRoomTypeMapping_Id ?? Guid.Empty,
                                                                   MappingStatus = ((asrtmv.SystemEditDate ?? DateTime.MinValue) > (asrtmv.UserEditDate ?? DateTime.MinValue)) ? asrtmv.SystemMappingStatus : asrtmv.UserMappingStatus,
                                                                   MatchingScore = asrtmv.MatchingScore
                                                               }).ToList();

                                        //    if (item.Accommodation_RoomInfo_Id == null)
                                        //    {
                                        //        if (!string.IsNullOrWhiteSpace(item.Tx_StrippedName))
                                        //        {
                                        //            // var resultRoomCategory = context.Accommodation_RoomInfo.Where(w => w.Accommodation_Id == item.Accommodation_Id && w.RoomCategory.ToLower().Replace("room", string.Empty).Replace("rooms", string.Empty).Trim() == item.Tx_StrippedName.ToLower().Replace("room", string.Empty).Replace("rooms", string.Empty).Trim()).Select(s => s).FirstOrDefault();
                                        //            var resultRoomCategory = resultRinfo.Where(w => w.Accommodation_Id == item.Accommodation_Id && w.Accommodation_RoomInfo_Name.ToLower().Replace("room", string.Empty).Replace("rooms", string.Empty).Trim() == item.Tx_StrippedName.ToLower().Replace("room", string.Empty).Replace("rooms", string.Empty).Trim()).Select(s => s).FirstOrDefault();
                                        //            if (resultRoomCategory != null)
                                        //            {
                                        //                item.Accommodation_RoomInfo_Id = resultRoomCategory.Accommodation_RoomInfo_Id;
                                        //                item.Accommodation_RoomInfo_Name = resultRoomCategory.Accommodation_RoomInfo_Name;
                                        //            }
                                        //        }
                                        //    }
                                    }

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }

                return result;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public DataContracts.DC_Message AccomodationSupplierRoomTypeMapping_UpdateMap(List<DataContracts.Mapping.DC_Accommodation_SupplierRoomTypeMap_Update> obj)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    context.Database.CommandTimeout = 0;

                    foreach (DC_Accommodation_SupplierRoomTypeMap_Update item in obj)
                    {
                        if (item.Accommodation_SupplierRoomTypeMapping_Id != null && !string.IsNullOrWhiteSpace(item.Status))
                        {
                            var accoSuppRoomTypeMap = context.Accommodation_SupplierRoomTypeMapping.Find(item.Accommodation_SupplierRoomTypeMapping_Id);
                            var acco = context.Accommodation.Find(item.Accommodation_Id);

                            if (acco != null && accoSuppRoomTypeMap != null)
                            {
                                //item.Status -- ADD
                                if (item.Status.ToUpper() == "ADD")
                                {
                                    //Check if Same room already exists. If yes, fetch and map to existing instead of adding a new one.
                                    var Acco_RoomInfo = (from a in context.Accommodation_RoomInfo select a).AsQueryable();
                                    Acco_RoomInfo = Acco_RoomInfo.Where(w => w.Accommodation_Id == item.Accommodation_Id && w.RoomCategory.ToUpper().Trim() == item.RoomCategory.ToUpper().Trim()).Select(s => s);
                                    var ExistingRoomInfo = Acco_RoomInfo.ToList();
                                    if (ExistingRoomInfo.Count > 0)
                                    {
                                        //Update existing Room ID to SRTM table
                                        item.Accommodation_RoomInfo_Id = ExistingRoomInfo[0].Accommodation_RoomInfo_Id;
                                        item.Status = "MAPPED";

                                    }
                                    else
                                    {
                                        Guid _newRoomId = Guid.NewGuid();

                                        Accommodation_RoomInfo _newObj = new Accommodation_RoomInfo();
                                        _newObj.Accommodation_RoomInfo_Id = _newRoomId;
                                        _newObj.Accommodation_Id = acco.Accommodation_Id;
                                        _newObj.Legacy_Htl_Id = acco.CompanyHotelID;
                                        _newObj.RoomId = CommonFunctions.GenerateRoomId(Guid.Parse(item.Accommodation_Id.ToString()));
                                        _newObj.NoOfRooms = accoSuppRoomTypeMap.Quantity;
                                        _newObj.Description = accoSuppRoomTypeMap.SupplierRoomName + " : " + accoSuppRoomTypeMap.RoomDescription;
                                        _newObj.RoomCategory = item.RoomCategory;

                                        _newObj.Create_User = item.Edit_User;
                                        _newObj.Create_Date = DateTime.Now;
                                        _newObj.IsActive = true;

                                        var accoSuppRoomTypeMapAttributes = context.Accommodation_SupplierRoomTypeAttributes.Where(w => w.RoomTypeMap_Id == item.Accommodation_SupplierRoomTypeMapping_Id).ToList();
                                        if (accoSuppRoomTypeMapAttributes != null)
                                        {
                                            if (accoSuppRoomTypeMapAttributes.Count > 0)
                                            {
                                                foreach (var attribute in accoSuppRoomTypeMapAttributes)
                                                {
                                                    var keyword = context.m_keyword.Find(attribute.SystemAttributeKeyword_Id);
                                                    if (keyword != null)
                                                    {
                                                        if (!string.IsNullOrWhiteSpace(keyword.AttributeLevel))
                                                        {
                                                            if (keyword.AttributeLevel.ToUpper() == "ROOM AMENITY")
                                                            {
                                                                Guid _newFacilityId = Guid.NewGuid();
                                                                Accommodation_RoomFacility RF = new Accommodation_RoomFacility();
                                                                RF.Accommodation_Id = acco.Accommodation_Id;
                                                                RF.Accommodation_RoomFacility_Id = _newFacilityId;
                                                                RF.Accommodation_RoomInfo_Id = _newRoomId;
                                                                RF.AmenityName = keyword.Keyword;
                                                                RF.AmenityType = keyword.AttributeSubLevel;
                                                                RF.Create_Date = DateTime.Now;
                                                                RF.Create_User = item.Edit_User;
                                                                context.Accommodation_RoomFacility.Add(RF);
                                                                RF = null;
                                                                attribute.Accommodation_RoomFacility_Id = _newFacilityId;
                                                            }
                                                            else if (keyword.AttributeLevel.ToUpper() == "ROOM INFO")
                                                            {
                                                                if (!string.IsNullOrWhiteSpace(keyword.AttributeSubLevel))
                                                                {
                                                                    if (keyword.AttributeSubLevel.ToUpper() == "NUMBER OF ROOMS")
                                                                    {
                                                                        int NoOfRooms = 0;
                                                                        int.TryParse(attribute.SystemAttributeKeyword, out NoOfRooms);
                                                                        _newObj.NoOfRooms = NoOfRooms;
                                                                    }
                                                                    else if (keyword.AttributeSubLevel.ToUpper() == "ROOM CATEGORY")
                                                                    {
                                                                        _newObj.RoomCategory = keyword.AttributeSubLevelValue;
                                                                    }
                                                                    else if (keyword.AttributeSubLevel.ToUpper() == "FLOOR NAME")
                                                                    {
                                                                        _newObj.FloorName = attribute.SystemAttributeKeyword;
                                                                    }
                                                                    else if (keyword.AttributeSubLevel.ToUpper() == "FLOOR NUMBER")
                                                                    {
                                                                        _newObj.FloorNumber = attribute.SystemAttributeKeyword;
                                                                    }
                                                                    else if (keyword.AttributeSubLevel.ToUpper() == "ROOM VIEW")
                                                                    {
                                                                        _newObj.RoomView = attribute.SystemAttributeKeyword;
                                                                    }
                                                                    else if (keyword.AttributeSubLevel.ToUpper() == "ROOM DECOR")
                                                                    {
                                                                        _newObj.RoomDecor = attribute.SystemAttributeKeyword;
                                                                    }
                                                                    else if (keyword.AttributeSubLevel.ToUpper() == "BED TYPE")
                                                                    {
                                                                        _newObj.BedType = keyword.AttributeSubLevelValue;
                                                                    }
                                                                    else if (keyword.AttributeSubLevel.ToUpper() == "BATHROOM TYPE")
                                                                    {
                                                                        _newObj.BathRoomType = keyword.AttributeSubLevelValue;
                                                                    }
                                                                    else if (keyword.AttributeSubLevel.ToUpper() == "SMOKING")
                                                                    {
                                                                        _newObj.Smoking = ((keyword.AttributeSubLevelValue ?? string.Empty) == "YES" ? true : false);
                                                                    }
                                                                    else if (keyword.AttributeSubLevel.ToUpper() == "ROOM SIZE")
                                                                    {
                                                                        _newObj.RoomSize = attribute.SystemAttributeKeyword;
                                                                    }
                                                                    else if (keyword.AttributeSubLevel.ToUpper() == "INTER ROOMS")
                                                                    {
                                                                        int NoOfInterRooms = 0;
                                                                        int.TryParse(attribute.SystemAttributeKeyword, out NoOfInterRooms);
                                                                        _newObj.NoOfInterconnectingRooms = NoOfInterRooms;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        context.Accommodation_RoomInfo.Add(_newObj);

                                        //Update new Room ID to SRTM table
                                        item.Accommodation_RoomInfo_Id = _newRoomId;
                                        item.Status = "MAPPED";
                                    }
                                }
                                else if (item.Status.ToUpper() == "UNMAPPED")
                                {
                                    item.Accommodation_RoomInfo_Id = null;
                                }

                                accoSuppRoomTypeMap.Accommodation_RoomInfo_Id = item.Accommodation_RoomInfo_Id;
                                accoSuppRoomTypeMap.MappingStatus = item.Status.ToUpper();
                                accoSuppRoomTypeMap.Edit_Date = DateTime.Now;
                                accoSuppRoomTypeMap.Edit_User = item.Edit_User;

                            }
                        }
                        // context.SaveChanges();
                    }

                }

                return new DataContracts.DC_Message { StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Success, StatusMessage = "All Valid Records are successfully updated." };
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating accomodation product supplier mapping" + ex.Message, ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool SupplierRoomTypeMapping_InsertUpdate(List<DataContracts.Mapping.DC_Accommodation_SupplierRoomTypeMap_SearchRS> lstobj)
        {
            bool blnInserted = false;
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    foreach (DC_Accommodation_SupplierRoomTypeMap_SearchRS obj in lstobj)
                    {
                        if (obj.Accommodation_SupplierRoomTypeMapping_Id == null)
                        {
                            continue;
                        }

                        var search = (from a in context.Accommodation_SupplierRoomTypeMapping
                                      where a.Accommodation_SupplierRoomTypeMapping_Id == obj.Accommodation_SupplierRoomTypeMapping_Id
                                      select a).FirstOrDefault();
                        if (search == null)
                        {
                            DataLayer.Accommodation_SupplierRoomTypeMapping objNew = new DataLayer.Accommodation_SupplierRoomTypeMapping()
                            {
                                Accommodation_SupplierRoomTypeMapping_Id = obj.Accommodation_SupplierRoomTypeMapping_Id,
                                Accommodation_Id = (context.Accommodation_ProductMapping.Where(w => w.SupplierProductReference == obj.SupplierProductId && w.Supplier_Id == obj.Supplier_Id).Select(a => a.Accommodation_Id).FirstOrDefault()),
                                Accommodation_RoomInfo_Id = null,
                                SupplierProductId = obj.SupplierProductId,
                                Create_Date = DateTime.Now,
                                Create_User = "TLGX",
                                Edit_Date = null,
                                Edit_User = null,
                                MapId = null,
                                MappingStatus = obj.MappingStatus,
                                MaxAdults = obj.MaxAdults,
                                MaxChild = obj.MaxChild,
                                MaxGuestOccupancy = obj.MaxGuestOccupancy,
                                MaxInfants = obj.MaxInfants,
                                Quantity = obj.Quantity,
                                RatePlan = obj.RatePlan,
                                RatePlanCode = obj.RatePlanCode,
                                //stg_SupplierHotelRoomMapping_Id = obj.stg_SupplierHotelRoomMapping_Id,
                                SupplierName = obj.SupplierName,
                                SupplierProductName = obj.SupplierProductName,
                                SupplierRoomCategory = obj.SupplierRoomCategory,
                                SupplierRoomCategoryId = obj.SupplierRoomCategoryId,
                                SupplierRoomId = obj.SupplierRoomId,
                                SupplierRoomName = obj.SupplierRoomName,
                                SupplierRoomTypeCode = obj.SupplierRoomTypeCode,
                                Supplier_Id = obj.Supplier_Id,
                                Tx_ReorderedName = null,
                                TX_RoomName = null,
                                Tx_StrippedName = null,
                                RoomSize = obj.RoomSize,
                                SupplierImportFile_Id = obj.SupplierImporrtFile_Id,
                                Batch = obj.Batch,
                                ReRun_SupplierImportFile_Id = obj.ReRunSupplierImporrtFile_Id,
                                ReRun_Batch = obj.ReRunBatch,
                                RoomDescription = obj.RoomDescription,
                                BathRoomType = obj.BathRoomType,
                                RoomViewCode = obj.RoomViewCode,
                                FloorName = obj.FloorName,
                                FloorNumber = obj.FloorNumber,
                                SupplierProvider = obj.SupplierProvider,
                                Amenities = obj.Amenities,
                                RoomLocationCode = obj.RoomLocationCode,
                                ChildAge = obj.ChildAge,
                                ExtraBed = obj.ExtraBed,
                                Bedrooms = obj.Bedrooms,
                                Smoking = obj.Smoking,
                                BedTypeCode = obj.BedTypeCode,
                                PromotionalVendorCode = obj.PromotionalVendorCode,
                                BeddingConfig = obj.BeddingConfig,
                                MinGuestOccupancy = obj.MinGuestOccupancy,
                                CityName = obj.CityName,
                                CityCode = obj.CityCode,
                                StateName = obj.StateName,
                                StateCode = obj.StateCode,
                                CountryName = obj.CountryName,
                                CountryCode = obj.CountryCode,
                                Source = "StaticFile"

                            };
                            context.Accommodation_SupplierRoomTypeMapping.Add(objNew);
                            blnInserted = true;
                        }
                        else
                        {
                            search.Accommodation_RoomInfo_Id = obj.Accommodation_RoomInfo_Id;
                            search.MappingStatus = obj.MappingStatus;
                            search.Edit_Date = DateTime.Now;
                            search.Edit_User = "TLGX";
                            //search.stg_SupplierHotelRoomMapping_Id = obj.stg_SupplierHotelRoomMapping_Id;
                            context.SaveChanges();
                        }
                    }
                    context.SaveChanges();
                    if (blnInserted)
                    {
                        context.USP_UpdateMapID("roomtype");
                    }
                    //context.USP_UpdateMapID("roomtype");
                }
                return true;
            }
            catch (Exception e)
            {
                LogErrorMessage(lstobj[0].SupplierImporrtFile_Id, e, "SupplierRoomTypeMapping_InsertUpdate", "DL_Mapping", "SupplierRoomTypeMapping_InsertUpdate", (int)Error_Enums_DataHandler.ErrorCodes.RoomTypeConfig_Generic, "", "SupplierRoomTypeMapping_InsertUpdate failed");
                //throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus
                //{
                //    ErrorMessage = "Error while updating accomodation product supplier mapping",
                //    ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError
                //});
                return false;
            }
        }

        public List<DC_SupplierRoomType_TTFU_RQ> GetRoomTypeMapping_For_TTFU(DataContracts.Masters.DC_Supplier obj)
        {
            try
            {
                string CurSupplierName = obj.Name;
                Guid CurSupplier_Id = Guid.Parse(obj.Supplier_Id.ToString());

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    context.Configuration.AutoDetectChangesEnabled = false;
                    context.Database.CommandTimeout = 0;

                    if ((obj.CurrentBatch ?? 0) != 0)
                    {
                        var res = (from a in context.Accommodation_SupplierRoomTypeMapping.AsNoTracking()
                                   where a.ReRun_SupplierImportFile_Id == obj.File_Id && a.ReRun_Batch == obj.CurrentBatch
                                   select new DC_SupplierRoomType_TTFU_RQ
                                   {
                                       Acco_RoomTypeMap_Id = a.Accommodation_SupplierRoomTypeMapping_Id
                                       //Edit_User = a.Edit_User
                                   }).ToList();

                        return res;
                    }
                    else
                    {
                        var res = (from a in context.Accommodation_SupplierRoomTypeMapping.AsNoTracking()
                                   where a.ReRun_SupplierImportFile_Id == obj.File_Id && (obj.CurrentBatch ?? 0) == 0
                                   select new DC_SupplierRoomType_TTFU_RQ
                                   {
                                       Acco_RoomTypeMap_Id = a.Accommodation_SupplierRoomTypeMapping_Id
                                       //Edit_User = a.Edit_User
                                   }).ToList();

                        return res;
                    }
                    //var res = (from a in context.Accommodation_SupplierRoomTypeMapping
                    //           join j in context.STG_Mapping_TableIds on a.Accommodation_SupplierRoomTypeMapping_Id equals j.Mapping_Id
                    //           //join s in context.stg_SupplierHotelRoomMapping on j.STG_Id equals s.stg_SupplierHotelRoomMapping_Id  //a.stg_SupplierHotelRoomMapping_Id equals s.stg_SupplierHotelRoomMapping_Id
                    //           where ((j.Batch == obj.CurrentBatch && (obj.CurrentBatch ?? 0) != 0) || ((obj.CurrentBatch ?? 0) == 0))
                    //           select new DC_SupplierRoomType_TTFU_RQ
                    //           {
                    //               Acco_RoomTypeMap_Id = a.Accommodation_SupplierRoomTypeMapping_Id,
                    //               Edit_User = a.Edit_User
                    //           }).ToList();
                }
            }
            catch (Exception ex)
            {
                return new List<DC_SupplierRoomType_TTFU_RQ>();
            }


        }

        public DataContracts.DC_Message AccomodationSupplierRoomTypeMapping_TTFUALL(List<DC_SupplierRoomType_TTFU_RQ> Acco_RoomTypeMap_Ids)
        {
            try
            {
                PLog.SupplierImportFileProgress_Id = Guid.NewGuid();
                PLog.SupplierImportFile_Id = Acco_RoomTypeMap_Ids[0].File_Id;
                PLog.Step = "KEYWORD";
                PLog.Status = "KEYWORDREPLACE";
                PLog.CurrentBatch = Acco_RoomTypeMap_Ids[0].CurrentBatch ?? 0;
                PLog.TotalBatch = Acco_RoomTypeMap_Ids[0].TotalBatch ?? 0;
                bool ISProgressLog = false;

                if (Acco_RoomTypeMap_Ids[0].File_Id.HasValue)
                    ISProgressLog = true;

                #region Get All Keywords & Room Names

                List<DataContracts.Masters.DC_Keyword> Keywords = new List<DataContracts.Masters.DC_Keyword>();
                using (DL_Masters objDL = new DL_Masters())
                {
                    Keywords = objDL.SearchKeyword(new DataContracts.Masters.DC_Keyword_RQ { EntityFor = "RoomType", PageNo = 0, PageSize = int.MaxValue, Status = "ACTIVE", AliasStatus = "ACTIVE" });
                }

                //Get All Supplier Room Type Name
                List<DC_SupplierRoomName_Details> asrtmd;
                string Edit_User = string.Empty;
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    context.Database.CommandTimeout = 0;

                    if (Acco_RoomTypeMap_Ids != null)
                    {
                        var MapIdsFilter = (from a in Acco_RoomTypeMap_Ids select a.Acco_RoomTypeMap_Id).ToList();

                        asrtmd = context.Accommodation_SupplierRoomTypeMapping.AsNoTracking()
                                     .Where(w => MapIdsFilter.Contains(w.Accommodation_SupplierRoomTypeMapping_Id))
                                     //.OrderBy(o => o.Accommodation_SupplierRoomTypeMapping_Id)
                                     .Select(s => new DC_SupplierRoomName_Details
                                     {
                                         RoomTypeMap_Id = s.Accommodation_SupplierRoomTypeMapping_Id,
                                         SupplierRoomName = s.SupplierRoomName,
                                         SupplierRoomDescription = s.RoomDescription
                                     }).ToList();
                        if (ISProgressLog)
                        {
                            PLog.PercentageValue = 25;
                            USD.AddStaticDataUploadProcessLog(PLog);
                        }
                        if (asrtmd.Count > 0)
                        {
                            Edit_User = Acco_RoomTypeMap_Ids[0].Edit_User;
                        }
                        else
                        {
                            Edit_User = "TTFU BY SYSTEM";
                        }

                    }
                    else
                    {
                        asrtmd = (from a in context.Accommodation_SupplierRoomTypeMapping.AsNoTracking()
                                  orderby a.Accommodation_SupplierRoomTypeMapping_Id
                                  select new DC_SupplierRoomName_Details
                                  {
                                      RoomTypeMap_Id = a.Accommodation_SupplierRoomTypeMapping_Id,
                                      SupplierRoomName = a.SupplierRoomName,
                                      SupplierRoomDescription = a.RoomDescription
                                  }).ToList();
                        if (ISProgressLog)
                        {
                            PLog.PercentageValue = 25;
                            USD.AddStaticDataUploadProcessLog(PLog);
                        }
                        Edit_User = "TTFU BY SYSTEM";
                    }
                }

                if (ISProgressLog)
                {
                    PLog.PercentageValue = 15;
                    USD.AddStaticDataUploadProcessLog(PLog);
                }
                #endregion


                int i = 0;
                List<DC_SupplierRoomName_AttributeList> AttributeList;

                foreach (DC_SupplierRoomName_Details srn in asrtmd)
                {
                    i = i + 1;

                    string TX_SupplierRoomName = string.Empty;
                    string TX_SupplierRoomName_Stripped = string.Empty;
                    string TX_SupplierRoomDesc = string.Empty;
                    string TX_SupplierRoomDesc_Stripped = string.Empty;

                    AttributeList = new List<DC_SupplierRoomName_AttributeList>();

                    string BaseRoomName = string.Empty;
                    string RoomDescription = string.Empty;

                    if (string.IsNullOrWhiteSpace(srn.SupplierRoomName))
                        continue;
                    else
                    {
                        BaseRoomName = srn.SupplierRoomName;
                        RoomDescription = srn.SupplierRoomDescription;
                    }

                    BaseRoomName = CommonFunctions.TTFU(ref Keywords, ref AttributeList, ref TX_SupplierRoomName, ref TX_SupplierRoomName_Stripped, BaseRoomName, new string[] { });

                    //Value assignment
                    srn.TX_SupplierRoomName = TX_SupplierRoomName;
                    srn.TX_SupplierRoomName_Stripped = TX_SupplierRoomName_Stripped;
                    srn.TX_SupplierRoomName_Stripped_ReOrdered = TX_SupplierRoomName_Stripped;

                    //Perform TTFU on Room Description to extract the Attributes.
                    RoomDescription = CommonFunctions.TTFU(ref Keywords, ref AttributeList, ref TX_SupplierRoomDesc, ref TX_SupplierRoomDesc_Stripped, RoomDescription, new string[] { });

                    //Assign the final Attribute List
                    srn.AttributeList = AttributeList;

                    #region UpdateToDB

                    //Update Room Name Stripped and Attributes
                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        //Remove Existing Attribute List Records
                        context.Accommodation_SupplierRoomTypeAttributes.RemoveRange(context.Accommodation_SupplierRoomTypeAttributes.Where(w => w.RoomTypeMap_Id == srn.RoomTypeMap_Id));

                        context.Accommodation_SupplierRoomTypeAttributes.AddRange((from a in srn.AttributeList
                                                                                   select new Accommodation_SupplierRoomTypeAttributes
                                                                                   {
                                                                                       RoomTypeMapAttribute_Id = Guid.NewGuid(),
                                                                                       RoomTypeMap_Id = srn.RoomTypeMap_Id,
                                                                                       SupplierRoomTypeAttribute = a.SupplierRoomTypeAttribute,
                                                                                       SystemAttributeKeyword = a.SystemAttributeKeyword,
                                                                                       SystemAttributeKeyword_Id = a.SystemAttributeKeywordID
                                                                                   }).ToList());

                        var srnm = context.Accommodation_SupplierRoomTypeMapping.Find(srn.RoomTypeMap_Id);
                        if (srnm != null)
                        {
                            srnm.TX_RoomName = srn.TX_SupplierRoomName;
                            srnm.Tx_StrippedName = srn.TX_SupplierRoomName_Stripped;
                            srnm.Tx_ReorderedName = srn.TX_SupplierRoomName_Stripped_ReOrdered;
                            srnm.Edit_Date = DateTime.Now;
                            srnm.Edit_User = Edit_User;
                        }

                        context.SaveChanges();

                    }

                    #endregion

                    #region Update Progress Log
                    if (ISProgressLog)
                    {
                        if (i % 5 == 0)
                        {
                            PLog.PercentageValue = 25 + ((60 * i) / asrtmd.Count);
                            USD.AddStaticDataUploadProcessLog(PLog);
                        }
                    }
                    #endregion
                }

                #region Update No Of Hits
                var updatableAliases = (from k in Keywords
                                        from ka in k.Alias
                                        where ka.NewHits != 0
                                        select ka).ToList();
                if (updatableAliases.Count > 0)
                {
                    using (DL_Masters objDL = new DL_Masters())
                    {
                        objDL.DataHandler_Keyword_Update_NoOfHits(updatableAliases);
                    }
                }

                #endregion

                if (!ISProgressLog)
                {
                    IncomingWebRequestContext woc = WebOperationContext.Current.IncomingRequest;
                    string CallingAgent = woc.Headers["CallingAgent"];
                    string CallingUser = woc.Headers["CallingUser"];

                    if (!string.IsNullOrWhiteSpace(CallingAgent))
                    {
                        if (CallingAgent == "MDM")
                            UpdateRoomTypeMappingStatus_GetAndProcessData(null, asrtmd.Select(s => s.RoomTypeMap_Id).ToList());
                    }
                }

                #region Update Progress Log
                if (ISProgressLog)
                {
                    PLog.PercentageValue = 100;
                    USD.AddStaticDataUploadProcessLog(PLog);
                }
                #endregion

                return new DataContracts.DC_Message { StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Success, StatusMessage = "Keyword Replace and Attribute Extraction has been done." };

            }
            catch (Exception ex)
            {
                return new DataContracts.DC_Message { StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Failed, StatusMessage = ex.Message };
                //throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while TTFU", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public IList<DataContracts.DC_SRT_ML_Response_Syntactic> GetRTM_ML_Suggestions_Syntactic(Guid Accomodation_SupplierRoomTypeMapping_Id)
        {
            DataContracts.DC_SRT_ML_Request_Syntactic RQ = new DataContracts.DC_SRT_ML_Request_Syntactic();
            List<DataContracts.DC_SRT_ML_Response_Syntactic> RS = new List<DataContracts.DC_SRT_ML_Response_Syntactic>();
            try
            {

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    RQ.supplier_data = (from srt in context.Accommodation_SupplierRoomTypeMapping
                                        where srt.Accommodation_SupplierRoomTypeMapping_Id == Accomodation_SupplierRoomTypeMapping_Id
                                        select new DataContracts.DC_SRT_ML_supplier_data_Syntactic
                                        {
                                            matching_string = srt.SupplierRoomName,
                                            Accommodation_Id = srt.Accommodation_Id ?? Guid.Empty,
                                            Supplier_Id = srt.SupplierName
                                        }).ToList();
                    RQ.skip_words = new List<string>();
                    RQ.system_room_categories = (from srt in context.Accommodation_SupplierRoomTypeMapping
                                                 join ari in context.Accommodation_RoomInfo on srt.Accommodation_Id equals ari.Accommodation_Id
                                                 where srt.Accommodation_SupplierRoomTypeMapping_Id == Accomodation_SupplierRoomTypeMapping_Id
                                                 select new DataContracts.DC_SRT_ML_system_room_categories_Syntactic
                                                 {
                                                     system_room_name = ari.RoomName ?? "",
                                                     AccommodationRoomInfo_Id = ari.Accommodation_RoomInfo_Id
                                                 }).ToList();


                    var request = (HttpWebRequest)WebRequest.Create(System.Configuration.ConfigurationManager.AppSettings["MLSVCURL_Syntactic"]);
                    var proxyAddress = System.Configuration.ConfigurationManager.AppSettings["ProxyUri"];

                    if (proxyAddress != null)
                    {
                        WebProxy myProxy = new WebProxy();
                        Uri newUri = new Uri(proxyAddress);
                        // Associate the newUri object to 'myProxy' object so that new myProxy settings can be set.
                        myProxy.Address = newUri;
                        // Create a NetworkCredential object and associate it with the 
                        // Proxy property of request object.
                        //myProxy.Credentials = new NetworkCredential(username, password);
                        request.Proxy = myProxy;
                    }

                    request.Method = "POST";
                    request.ContentType = "application/json";
                    request.KeepAlive = false;
                    //request.Credentials = CredentialCache.DefaultCredentials;

                    DataContractJsonSerializer serializerToUpload = new DataContractJsonSerializer(typeof(DataContracts.DC_SRT_ML_Request_Syntactic));

                    using (var memoryStream = new MemoryStream())
                    {
                        using (var reader = new StreamReader(memoryStream))
                        {
                            serializerToUpload.WriteObject(memoryStream, RQ);
                            memoryStream.Position = 0;
                            string body = reader.ReadToEnd();

                            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                            {
                                streamWriter.Write(body);
                            }
                        }
                    }

                    var response = request.GetResponse();

                    if (((System.Net.HttpWebResponse)response).StatusCode != HttpStatusCode.OK)
                    {
                        RS = null;
                    }
                    else
                    {
                        var stream = response.GetResponseStream();
                        var encoding = ASCIIEncoding.UTF8;
                        using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                        {
                            string responseText = reader.ReadToEnd();
                            RS = JsonConvert.DeserializeObject<List<DataContracts.DC_SRT_ML_Response_Syntactic>>(responseText);
                        }
                        //deserialize here


                        stream = null;
                    }

                    serializerToUpload = null;

                    response.Dispose();
                    response = null;
                    request = null;

                }
                return RS;
            }
            catch (Exception ex)
            {
                return RS;
                // throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching accomodation product supplier mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }

        }

        public IList<DataContracts.DC_SRT_ML_Response_Semantic> GetRTM_ML_Suggestions_Semantic(Guid Accomodation_SupplierRoomTypeMapping_Id)
        {
            DataContracts.DC_SRT_ML_Request_Semantic RQ = new DataContracts.DC_SRT_ML_Request_Semantic();
            List<DataContracts.DC_SRT_ML_Response_Semantic> RS = new List<DataContracts.DC_SRT_ML_Response_Semantic>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    RQ.supplier_data = (from srt in context.Accommodation_SupplierRoomTypeMapping
                                        where srt.Accommodation_SupplierRoomTypeMapping_Id == Accomodation_SupplierRoomTypeMapping_Id
                                        select new DataContracts.DC_SRT_ML_supplier_data_Semantic
                                        {
                                            matching_string = srt.SupplierRoomName,
                                            Accommodation_Id = srt.Accommodation_Id ?? Guid.Empty,
                                            Supplier_Id = srt.SupplierName
                                        }).ToList();
                    RQ.skip_words = new List<string>();
                    RQ.system_room_categories = (from srt in context.Accommodation_SupplierRoomTypeMapping
                                                 join ari in context.Accommodation_RoomInfo on srt.Accommodation_Id equals ari.Accommodation_Id
                                                 where srt.Accommodation_SupplierRoomTypeMapping_Id == Accomodation_SupplierRoomTypeMapping_Id
                                                 select new DataContracts.DC_SRT_ML_system_room_categories_Semantic
                                                 {
                                                     system_room_name = ari.RoomName ?? "",
                                                     AccommodationRoomInfo_Id = ari.Accommodation_RoomInfo_Id
                                                 }).ToList();


                    var request = (HttpWebRequest)WebRequest.Create(System.Configuration.ConfigurationManager.AppSettings["MLSVCURL_Semantic"]);
                    var proxyAddress = System.Configuration.ConfigurationManager.AppSettings["ProxyUri"];

                    if (proxyAddress != null)
                    {
                        WebProxy myProxy = new WebProxy();
                        Uri newUri = new Uri(proxyAddress);
                        // Associate the newUri object to 'myProxy' object so that new myProxy settings can be set.
                        myProxy.Address = newUri;
                        // Create a NetworkCredential object and associate it with the 
                        // Proxy property of request object.
                        //myProxy.Credentials = new NetworkCredential(username, password);
                        request.Proxy = myProxy;
                    }

                    request.Method = "POST";
                    request.ContentType = "application/json";
                    request.KeepAlive = false;
                    //request.Credentials = CredentialCache.DefaultCredentials;

                    DataContractJsonSerializer serializerToUpload = new DataContractJsonSerializer(typeof(DataContracts.DC_SRT_ML_Request_Semantic));

                    using (var memoryStream = new MemoryStream())
                    {
                        using (var reader = new StreamReader(memoryStream))
                        {
                            serializerToUpload.WriteObject(memoryStream, RQ);
                            memoryStream.Position = 0;
                            string body = reader.ReadToEnd();

                            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                            {
                                streamWriter.Write(body);
                            }
                        }
                    }

                    var response = request.GetResponse();

                    if (((System.Net.HttpWebResponse)response).StatusCode != HttpStatusCode.OK)
                    {
                        RS = null;
                    }
                    else
                    {
                        var stream = response.GetResponseStream();
                        var encoding = ASCIIEncoding.UTF8;
                        using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                        {
                            string responseText = reader.ReadToEnd();
                            RS = JsonConvert.DeserializeObject<List<DataContracts.DC_SRT_ML_Response_Semantic>>(responseText);
                        }
                        //deserialize here


                        stream = null;
                    }

                    serializerToUpload = null;

                    response.Dispose();
                    response = null;
                    request = null;

                }
                return RS.OrderBy(x => x.matches = x.matches.OrderByDescending(y => y.score).Select(s => s).ToList()).ToList();
            }
            catch (Exception ex)
            {
                return RS;
                //throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching accomodation product supplier mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }

        }

        public IList<DataContracts.DC_SRT_ML_Response_Supervised_Semantic> GetRTM_ML_Suggestions_Supervised_Semantic(Guid Accomodation_SupplierRoomTypeMapping_Id)
        {
            DataContracts.DC_SRT_ML_Request_Supervised_Semantic RQ = new DataContracts.DC_SRT_ML_Request_Supervised_Semantic();
            List<DataContracts.DC_SRT_ML_Response_Supervised_Semantic> RS = new List<DataContracts.DC_SRT_ML_Response_Supervised_Semantic>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    RQ.supplier_data = (from srt in context.Accommodation_SupplierRoomTypeMapping
                                        where srt.Accommodation_SupplierRoomTypeMapping_Id == Accomodation_SupplierRoomTypeMapping_Id
                                        select new DataContracts.DC_SRT_ML_supplier_data_Supervised_Semantic
                                        {
                                            matching_string = srt.SupplierRoomName,
                                            Accommodation_Id = srt.Accommodation_Id ?? Guid.Empty,
                                            Supplier_Id = srt.SupplierName
                                        }).ToList();
                    RQ.skip_words = new List<string>();
                    RQ.system_room_categories = (from srt in context.Accommodation_SupplierRoomTypeMapping
                                                 join ari in context.Accommodation_RoomInfo on srt.Accommodation_Id equals ari.Accommodation_Id
                                                 where srt.Accommodation_SupplierRoomTypeMapping_Id == Accomodation_SupplierRoomTypeMapping_Id
                                                 select new DataContracts.DC_SRT_ML_system_room_categories_Supervised_Semantic
                                                 {
                                                     system_room_name = ari.RoomName ?? "",
                                                     AccommodationRoomInfo_Id = ari.Accommodation_RoomInfo_Id
                                                 }).ToList();


                    var request = (HttpWebRequest)WebRequest.Create(System.Configuration.ConfigurationManager.AppSettings["MLSVCURL_Supervised_Semantic"]);
                    var proxyAddress = System.Configuration.ConfigurationManager.AppSettings["ProxyUri"];

                    if (proxyAddress != null)
                    {
                        WebProxy myProxy = new WebProxy();
                        Uri newUri = new Uri(proxyAddress);
                        // Associate the newUri object to 'myProxy' object so that new myProxy settings can be set.
                        myProxy.Address = newUri;
                        // Create a NetworkCredential object and associate it with the 
                        // Proxy property of request object.
                        //myProxy.Credentials = new NetworkCredential(username, password);
                        request.Proxy = myProxy;
                    }

                    request.Method = "POST";
                    request.ContentType = "application/json";
                    request.KeepAlive = false;
                    request.Timeout = System.Threading.Timeout.Infinite;
                    request.ReadWriteTimeout = System.Threading.Timeout.Infinite;
                    //request.Credentials = CredentialCache.DefaultCredentials;

                    DataContractJsonSerializer serializerToUpload = new DataContractJsonSerializer(typeof(DataContracts.DC_SRT_ML_Request_Supervised_Semantic));

                    using (var memoryStream = new MemoryStream())
                    {
                        using (var reader = new StreamReader(memoryStream))
                        {
                            serializerToUpload.WriteObject(memoryStream, RQ);
                            memoryStream.Position = 0;
                            string body = reader.ReadToEnd();

                            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                            {
                                streamWriter.Write(body);
                            }
                        }
                    }

                    var response = request.GetResponse();

                    if (((System.Net.HttpWebResponse)response).StatusCode != HttpStatusCode.OK)
                    {
                        RS = null;
                    }
                    else
                    {
                        var stream = response.GetResponseStream();
                        var encoding = ASCIIEncoding.UTF8;
                        using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                        {
                            string responseText = reader.ReadToEnd();
                            RS = JsonConvert.DeserializeObject<List<DataContracts.DC_SRT_ML_Response_Supervised_Semantic>>(responseText);
                        }
                        //deserialize here


                        stream = null;
                    }

                    serializerToUpload = null;

                    response.Dispose();
                    response = null;
                    request = null;

                }

                return RS.OrderBy(x => x.matches = x.matches.OrderByDescending(y => y.score).Select(s => s).ToList()).ToList();

            }
            catch (Exception ex)
            {
                return RS;
                //throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching accomodation product supplier mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }

        }

        public DataContracts.DC_SRT_ML_Response GetRTM_ML_Suggestions(Guid Accomodation_SupplierRoomTypeMapping_Id)
        {
            var resultSem = GetRTM_ML_Suggestions_Semantic(Accomodation_SupplierRoomTypeMapping_Id);
            var resultSupSem = GetRTM_ML_Suggestions_Supervised_Semantic(Accomodation_SupplierRoomTypeMapping_Id);
            var resultSyn = GetRTM_ML_Suggestions_Syntactic(Accomodation_SupplierRoomTypeMapping_Id);


            DataContracts.DC_SRT_ML_Response _objresponse = new DataContracts.DC_SRT_ML_Response();
            if (resultSem != null && resultSem.Count > 0)
            {
                List<DataContracts.DC_SRT_ML_AccommodationRoomInfo_Semantic> _lstrmi;
                List<DataContracts.DC_SRT_ML_Match_Semantic> _lstmastch;

                foreach (var item in resultSem)
                {
                    _lstrmi = new List<DataContracts.DC_SRT_ML_AccommodationRoomInfo_Semantic>();
                    foreach (var itemlist in item.AccommodationRoomInfo_Id)
                    {
                        _lstrmi.Add(new DataContracts.DC_SRT_ML_AccommodationRoomInfo_Semantic
                        {
                            AccommodationRoomInfo_Id = itemlist.AccommodationRoomInfo_Id,
                            system_room_name = itemlist.system_room_name
                        });
                    }
                    _lstmastch = new List<DataContracts.DC_SRT_ML_Match_Semantic>();
                    foreach (var itemlist in item.matches)
                    {
                        _lstmastch.Add(new DataContracts.DC_SRT_ML_Match_Semantic
                        {
                            matched_string = itemlist.matched_string,
                            score = itemlist.score
                        });
                    }

                    _objresponse._objMLSem =
                        new DataContracts.DC_SRT_ML_Response_Semantic
                        {
                            matches = _lstmastch,
                            AccommodationRoomInfo_Id = _lstrmi,
                            Accommodation_Id = item.Accommodation_Id,
                            Supplier_Id = item.Supplier_Id,
                            matching_string = item.matching_string
                        };
                }
            }
            if (resultSyn != null && resultSyn.Count > 0)
            {
                List<DataContracts.DC_SRT_ML_AccommodationRoomInfo_Syntactic> _lstrmi;
                List<DataContracts.DC_SRT_ML_Match_Syntactic> _lstmastch;

                foreach (var item in resultSyn)
                {
                    _lstrmi = new List<DataContracts.DC_SRT_ML_AccommodationRoomInfo_Syntactic>();
                    foreach (var itemlist in item.AccommodationRoomInfo_Id)
                    {
                        _lstrmi.Add(new DataContracts.DC_SRT_ML_AccommodationRoomInfo_Syntactic
                        {
                            AccommodationRoomInfo_Id = itemlist.AccommodationRoomInfo_Id,
                            system_room_name = itemlist.system_room_name
                        });
                    }
                    _lstmastch = new List<DataContracts.DC_SRT_ML_Match_Syntactic>();
                    foreach (var itemlist in item.matches)
                    {
                        _lstmastch.Add(new DataContracts.DC_SRT_ML_Match_Syntactic
                        {
                            matched_string = itemlist.matched_string,
                            score = itemlist.score
                        });
                    }

                    _objresponse._objMLSyn =
                        new DataContracts.DC_SRT_ML_Response_Syntactic
                        {
                            matches = _lstmastch,
                            AccommodationRoomInfo_Id = _lstrmi,
                            Accommodation_Id = item.Accommodation_Id,
                            Supplier_Id = item.Supplier_Id,
                            matching_string = item.matching_string
                        };
                }
            }
            if (resultSupSem != null && resultSupSem.Count > 0)
            {
                List<DataContracts.DC_SRT_ML_AccommodationRoomInfo_Supervised_Semantic> _lstrmi;
                List<DataContracts.DC_SRT_ML_Match_Supervised_Semantic> _lstmastch;

                foreach (var item in resultSem)
                {
                    _lstrmi = new List<DataContracts.DC_SRT_ML_AccommodationRoomInfo_Supervised_Semantic>();
                    foreach (var itemlist in item.AccommodationRoomInfo_Id)
                    {
                        _lstrmi.Add(new DataContracts.DC_SRT_ML_AccommodationRoomInfo_Supervised_Semantic
                        {
                            AccommodationRoomInfo_Id = itemlist.AccommodationRoomInfo_Id,
                            system_room_name = itemlist.system_room_name
                        });
                    }
                    _lstmastch = new List<DataContracts.DC_SRT_ML_Match_Supervised_Semantic>();
                    foreach (var itemlist in item.matches)
                    {
                        _lstmastch.Add(new DataContracts.DC_SRT_ML_Match_Supervised_Semantic
                        {
                            matched_string = itemlist.matched_string,
                            score = itemlist.score
                        });
                    }

                    _objresponse._objMLSupSem =
                        new DataContracts.DC_SRT_ML_Response_Supervised_Semantic
                        {
                            matches = _lstmastch,
                            AccommodationRoomInfo_Id = _lstrmi,
                            Accommodation_Id = item.Accommodation_Id,
                            Supplier_Id = item.Supplier_Id,
                            matching_string = item.matching_string
                        };
                }
            }

            return _objresponse;
        }

        /// <summary>
        /// Supplier Room Type matching batch wise
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool UpdateRoomTypeMappingStatus(DC_MappingMatch obj)
        {
            try
            {
                return UpdateRoomTypeMappingStatus_GetAndProcessData(obj, null);
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating hotel mapping" + Environment.NewLine + e.Message, ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        private bool UpdateRoomTypeMappingStatus_GetAndProcessData(DC_MappingMatch obj, List<Guid> AccoRoomMap_Ids)
        {
            IncomingWebRequestContext woc = WebOperationContext.Current.IncomingRequest;
            string CallingAgent = woc.Headers["CallingAgent"];
            string CallingUser = woc.Headers["CallingUser"];

            bool IsCalledFromTTFU = false;
            Guid? curSupplier_Id = Guid.Empty;
            string curSupplier = string.Empty;
            Guid File_Id = Guid.Empty;
            StringBuilder sbSupplierRoomTypeMap_Ids = new StringBuilder();
            DataContracts.Masters.DC_Supplier supdata = new DataContracts.Masters.DC_Supplier();

            if (obj == null && AccoRoomMap_Ids == null)
            {
                return false;
            }
            else if (obj != null && AccoRoomMap_Ids != null)
            {
                return false;
            }
            else if (obj == null && AccoRoomMap_Ids != null)
            {
                IsCalledFromTTFU = true;
                foreach (var id in AccoRoomMap_Ids)
                {
                    sbSupplierRoomTypeMap_Ids.Append("'" + id.ToString() + "',");
                }

            }
            else if (obj != null && AccoRoomMap_Ids == null)
            {
                supdata = obj.SupplierDetail;
                if (supdata != null)
                {
                    curSupplier = supdata.Name;
                    curSupplier_Id = supdata.Supplier_Id;
                }
                File_Id = obj.File_Id ?? Guid.NewGuid();
            }

            try
            {
                if (!IsCalledFromTTFU)
                {
                    PLog.SupplierImportFileProgress_Id = Guid.NewGuid();
                    PLog.SupplierImportFile_Id = obj.File_Id;
                    PLog.Step = "MATCH";
                    PLog.Status = "MATCHING";
                    PLog.CurrentBatch = obj.CurrentBatch ?? 0;
                    PLog.TotalBatch = obj.TotalBatch ?? 0;
                    PLog.PercentageValue = 10;
                    USD.AddStaticDataUploadProcessLog(PLog);
                }

                #region Update all review records to UnMapped in the running batch - Not required as of now

                //StringBuilder sbUpdateRecordsToUnMap = new StringBuilder();
                //sbUpdateRecordsToUnMap.Append("Update Accommodation_SupplierRoomTypeMapping Set MappingStatus = 'UNMAPPED' WHERE ");
                //sbUpdateRecordsToUnMap.Append("ReRun_SupplierImportFile_Id = '" + Convert.ToString(supdata.File_Id) + "' AND ");
                //sbUpdateRecordsToUnMap.Append("Supplier_Id = '" + Convert.ToString(curSupplier_Id) + "' AND ");
                //sbUpdateRecordsToUnMap.Append("MappingStatus = 'REVIEW' AND ");
                //sbUpdateRecordsToUnMap.Append("ReRun_Batch = '" + Convert.ToString(obj.CurrentBatch) + "' ");
                //try
                //{
                //    using (ConsumerEntities context = new ConsumerEntities())
                //    {
                //        var UpdateCount = context.Database.ExecuteSqlCommand(sbUpdateRecordsToUnMap.ToString());
                //    }
                //}
                //catch (Exception ex)
                //{
                //    CallLogVerbose(File_Id, "MATCH", ex.Message, obj.CurrentBatch);
                //}

                #endregion

                List<DC_Accommodation_SupplierRoomTypeMap_SearchRS> resSupplierRoomTypeMap = new List<DC_Accommodation_SupplierRoomTypeMap_SearchRS>();
                List<DC_SupplierRoomTypeAttributes> AttributeList = new List<DC_SupplierRoomTypeAttributes>();
                DataContracts.DC_SRT_ML_Request_Broker RQ_Broker = new DataContracts.DC_SRT_ML_Request_Broker();
                DataContracts.DC_SRT_ML_Response_Broker RS_Broker = new DataContracts.DC_SRT_ML_Response_Broker();

                if (!IsCalledFromTTFU)
                {
                    CallLogVerbose(File_Id, "MATCH", "Fetch All the UnMapped/Review room data", obj.CurrentBatch);
                }

                #region Fetch All the UnMapped/Review room data in the running batch
                try
                {
                    StringBuilder sbGetAllSupplierRooms = new StringBuilder();
                    sbGetAllSupplierRooms.Append(@"SELECT  
                                                ASRTM.MapId, 
                                                ASRTM.Accommodation_SupplierRoomTypeMapping_Id,
                                                ASRTM.SupplierRoomId, 
                                                ASRTM.SupplierRoomTypeCode,
                                                ASRTM.SupplierRoomName, 
                                                ASRTM.TX_RoomName,
                                                ASRTM.SupplierRoomCategory, 
                                                ASRTM.SupplierRoomCategoryId,
                                                ASRTM.MaxAdults, 
                                                ASRTM.MaxChild,
                                                ASRTM.MaxInfants,
                                                ASRTM.MaxGuestOccupancy,
                                                ASRTM.Quantity,
                                                ASRTM.RatePlan,
                                                ASRTM.RatePlanCode,
                                                ASRTM.SupplierProductName,
                                                ASRTM.SupplierProductId,
                                                ASRTM.Tx_StrippedName,
                                                ASRTM.Tx_ReorderedName,
                                                ASRTM.MappingStatus,
                                                ASRTM.Accommodation_RoomInfo_Id,
                                                ASRTM.RoomSize,
                                                ASRTM.BathRoomType,
                                                ASRTM.RoomViewCode,
                                                ASRTM.FloorName,
                                                ASRTM.FloorNumber,
                                                ASRTM.Amenities,
                                                ASRTM.RoomLocationCode,
                                                ASRTM.ChildAge,
                                                ASRTM.ExtraBed,
                                                ASRTM.Bedrooms,
                                                ASRTM.Smoking,
                                                ASRTM.BedTypeCode,
                                                ASRTM.MinGuestOccupancy,
                                                ASRTM.PromotionalVendorCode,
                                                ASRTM.BeddingConfig,
                                                ASRTM.Accommodation_Id,
                                                ISNULL(ASRTM.ReRun_SupplierImportFile_Id, cast(cast(0 as binary) as uniqueidentifier)) AS ReRunSupplierImporrtFile_Id,
                                                ASRTM.Supplier_Id,
                                                ASRTM.SupplierName,
                                                ISNULL(ASRTM.ReRun_Batch,0) as ReRunBatch,
                                                ISNULL(ASRTM.Batch,0) as Batch,
                                                ASRTM.SupplierProvider,
                                                ASRTM.CityName,
                                                ASRTM.CityCode,
                                                ASRTM.StateName,
                                                ASRTM.StateCode,
                                                ASRTM.CountryName,
                                                ASRTM.CountryCode
                                                FROM Accommodation_SupplierRoomTypeMapping ASRTM WITH (NOLOCK) 
                                                WHERE ");

                    //Convert(varchar, ACCO.CompanyHotelID) AS CommonProductId,
                    //ACCO.HotelName as ProductName,
                    //ACCO.Location,
                    //ARI.RoomName as Accommodation_RoomInfo_Name,
                    //JOIN Accommodation ACCO WITH (NOLOCK) ON ASRTM.Accommodation_Id = ACCO.Accommodation_Id
                    //LEFT JOIN Accommodation_RoomInfo ARI WITH (NOLOCK) ON ASRTM.Accommodation_RoomInfo_Id = ARI.Accommodation_RoomInfo_Id

                    if (IsCalledFromTTFU)
                    {
                        sbGetAllSupplierRooms.Append("ASRTM.Accommodation_SupplierRoomTypeMapping_Id IN (" + sbSupplierRoomTypeMap_Ids.ToString().TrimEnd(',') + ");");
                    }
                    else
                    {
                        //Commented below line as FileId filter is unique across run. So no need for SupplierId filter
                        //sbGetAllSupplierRooms.Append("ASRTM.Supplier_Id = '" + Convert.ToString(curSupplier_Id) + "' AND ");
                        sbGetAllSupplierRooms.Append("ASRTM.Accommodation_Id IS NOT NULL AND ");
                        sbGetAllSupplierRooms.Append("ASRTM.ReRun_SupplierImportFile_Id = '" + Convert.ToString(supdata.File_Id) + "' AND ");
                        //sbGetAllSupplierRooms.Append("ASRTM.MappingStatus IN ('REVIEW','UNMAPPED','ADD') AND ");
                        sbGetAllSupplierRooms.Append("ASRTM.ReRun_Batch = '" + Convert.ToString(obj.CurrentBatch) + "';");
                    }

                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        context.Database.CommandTimeout = 0;
                        resSupplierRoomTypeMap = context.Database.SqlQuery<DataContracts.Mapping.DC_Accommodation_SupplierRoomTypeMap_SearchRS>(sbGetAllSupplierRooms.ToString()).ToList();
                    }
                }
                catch (Exception ex)
                {
                    if (IsCalledFromTTFU)
                    {
                        throw ex;
                    }
                    else
                    {
                        CallLogVerbose(File_Id, "MATCH", ex.Message, obj.CurrentBatch);
                    }
                }
                #endregion

                if (!IsCalledFromTTFU)
                {
                    PLog.PercentageValue = 20;
                    USD.AddStaticDataUploadProcessLog(PLog);

                    CallLogVerbose(File_Id, "MATCH", "Fetch all the room attributes", obj.CurrentBatch);
                }

                #region Fetch all the room attributes in the running batch
                if (resSupplierRoomTypeMap != null && resSupplierRoomTypeMap.Count > 0)
                {
                    try
                    {
                        StringBuilder sbSupplierRoomTypeAttributes = new StringBuilder();
                        sbSupplierRoomTypeAttributes.Append(@"Select 
                                                            RoomTypeMapAttribute_Id AS Accommodation_SupplierRoomTypeMapAttribute_Id,
                                                            RoomTypeMap_Id AS Accommodation_SupplierRoomTypeMap_Id,
                                                            SupplierRoomTypeAttribute,
                                                            SystemAttributeKeyword,
                                                            SystemAttributeKeyword_Id,
                                                            '' AS IconClass
                                                            FROM Accommodation_SupplierRoomTypeAttributes WITH (NOLOCK)
                                                            WHERE RoomTypeMap_Id IN (");

                        foreach (var id in resSupplierRoomTypeMap)
                        {
                            sbSupplierRoomTypeAttributes.Append("'" + id.Accommodation_SupplierRoomTypeMapping_Id + "',");
                        }

                        using (ConsumerEntities context = new ConsumerEntities())
                        {
                            context.Database.CommandTimeout = 0;
                            AttributeList = context.Database.SqlQuery<DataContracts.Mapping.DC_SupplierRoomTypeAttributes>(sbSupplierRoomTypeAttributes.ToString().TrimEnd(',') + ");").ToList();
                        }
                    }
                    catch (Exception ex)
                    {
                        if (IsCalledFromTTFU)
                        {
                            throw ex;
                        }
                        else
                        {
                            CallLogVerbose(File_Id, "MATCH", ex.Message, obj.CurrentBatch);
                        }
                    }
                }
                #endregion

                if (!IsCalledFromTTFU)
                {
                    PLog.PercentageValue = 30;
                    USD.AddStaticDataUploadProcessLog(PLog);

                    CallLogVerbose(File_Id, "MATCH", "Loop through the Supplier Room info to generate Request for Broker", obj.CurrentBatch);
                }

                #region Loop through the Supplier Room info to generate Request for Broker
                try
                {
                    if (resSupplierRoomTypeMap != null && resSupplierRoomTypeMap.Count > 0)
                    {
                        #region Request Structure creation for Broker call

                        if (IsCalledFromTTFU)
                        {
                            RQ_Broker.Mode = "MDM UI";
                            RQ_Broker.BatchId = "1";
                            RQ_Broker.Transaction = Guid.NewGuid().ToString().ToUpper();
                        }
                        else
                        {
                            RQ_Broker.Mode = "Offline";
                            RQ_Broker.BatchId = Convert.ToString(obj.CurrentBatch) ?? Guid.NewGuid().ToString().ToUpper();
                            RQ_Broker.Transaction = Guid.NewGuid().ToString().ToUpper();//(Convert.ToString(supdata.File_Id) ?? Guid.NewGuid().ToString()).ToUpper();
                        }

                        List<DataContracts.DC_HotelRoomTypeMappingRequest> _lstHotelRoomTypeMappingRequests = new List<DataContracts.DC_HotelRoomTypeMappingRequest>();

                        foreach (var AccoId in resSupplierRoomTypeMap.Select(s => s.Accommodation_Id).Distinct())
                        {
                            var newHotelRoomTypeMappingRequest = new DC_HotelRoomTypeMappingRequest
                            {
                                AccommodationId = (Convert.ToString(AccoId) ?? string.Empty).ToUpper(),
                                TLGXCommonHotelId = resSupplierRoomTypeMap.Where(w => w.Accommodation_Id == AccoId).Select(s => (s.CommonProductId ?? string.Empty).ToUpper()).First(),
                            };

                            var RoomInfoByAccoId = resSupplierRoomTypeMap.Where(w => w.Accommodation_Id == AccoId).ToList();
                            List<DataContracts.DC_SupplierData> _lstSupplierData = new List<DataContracts.DC_SupplierData>();

                            foreach (var SupplierId in RoomInfoByAccoId.Select(s => s.Supplier_Id).Distinct())
                            {
                                var newSupplierData = new DC_SupplierData
                                {
                                    SupplierName = RoomInfoByAccoId.Where(w => w.Supplier_Id == SupplierId).Select(s => s.SupplierName ?? string.Empty).First(),
                                    SupplierId = (Convert.ToString(SupplierId) ?? string.Empty).ToUpper()
                                };

                                var RoomInfoBySupplierId = RoomInfoByAccoId.Where(w => w.Supplier_Id == SupplierId).ToList();
                                List<DataContracts.DC_SupplierRoomType> _lstSupplierRoomType = new List<DataContracts.DC_SupplierRoomType>();

                                foreach (var RoomMapId in RoomInfoBySupplierId.Select(s => s.Accommodation_SupplierRoomTypeMapping_Id).Distinct())
                                {
                                    var oneSupplierRoomType = RoomInfoBySupplierId.Where(w => w.Accommodation_SupplierRoomTypeMapping_Id == RoomMapId).First();
                                    var newSupplierRoomType = new DC_SupplierRoomType
                                    {
                                        MapId = Convert.ToString(oneSupplierRoomType.MapId) ?? string.Empty,
                                        AccommodationSupplierRoomTypeMappingId = (Convert.ToString(oneSupplierRoomType.Accommodation_SupplierRoomTypeMapping_Id) ?? string.Empty).ToUpper(),
                                        SupplierRoomId = Convert.ToString(oneSupplierRoomType.SupplierRoomId) ?? string.Empty,
                                        SupplierRoomTypeCode = Convert.ToString(oneSupplierRoomType.SupplierRoomTypeCode) ?? string.Empty,
                                        SupplierRoomName = Convert.ToString(oneSupplierRoomType.SupplierRoomName) ?? string.Empty,
                                        TXRoomName = Convert.ToString(oneSupplierRoomType.TX_RoomName) ?? string.Empty,
                                        SupplierRoomCategory = Convert.ToString(oneSupplierRoomType.SupplierRoomCategory) ?? string.Empty,
                                        SupplierRoomCategoryId = Convert.ToString(oneSupplierRoomType.SupplierRoomCategoryId) ?? string.Empty,
                                        MaxAdults = Convert.ToString(oneSupplierRoomType.MaxAdults) ?? string.Empty,
                                        MaxChild = Convert.ToString(oneSupplierRoomType.MaxChild) ?? string.Empty,
                                        MaxInfants = Convert.ToString(oneSupplierRoomType.MaxInfants) ?? string.Empty,
                                        MaxGuestOccupancy = Convert.ToString(oneSupplierRoomType.MaxGuestOccupancy) ?? string.Empty,
                                        Quantity = Convert.ToString(oneSupplierRoomType.Quantity) ?? string.Empty,
                                        RatePlan = Convert.ToString(oneSupplierRoomType.RatePlan) ?? string.Empty,
                                        RatePlanCode = Convert.ToString(oneSupplierRoomType.RatePlanCode) ?? string.Empty,
                                        SupplierProductName = Convert.ToString(oneSupplierRoomType.SupplierProductName) ?? string.Empty,
                                        SupplierProductId = Convert.ToString(oneSupplierRoomType.SupplierProductId) ?? string.Empty,
                                        TxStrippedName = Convert.ToString(oneSupplierRoomType.Tx_StrippedName) ?? string.Empty,
                                        TxReorderedName = Convert.ToString(oneSupplierRoomType.Tx_ReorderedName) ?? string.Empty,
                                        MappingStatus = Convert.ToString(oneSupplierRoomType.MappingStatus) ?? string.Empty,
                                        AccommodationRoomInfoId = Convert.ToString(oneSupplierRoomType.Accommodation_RoomInfo_Id) ?? string.Empty,
                                        RoomSize = Convert.ToString(oneSupplierRoomType.RoomSize) ?? string.Empty,
                                        BathRoomType = Convert.ToString(oneSupplierRoomType.BathRoomType) ?? string.Empty,
                                        RoomViewCode = Convert.ToString(oneSupplierRoomType.RoomViewCode) ?? string.Empty,
                                        FloorName = Convert.ToString(oneSupplierRoomType.FloorName) ?? string.Empty,
                                        FloorNumber = Convert.ToString(oneSupplierRoomType.FloorNumber) ?? string.Empty,
                                        Amenities = Convert.ToString(oneSupplierRoomType.Amenities) ?? string.Empty,
                                        RoomLocationCode = Convert.ToString(oneSupplierRoomType.RoomLocationCode) ?? string.Empty,
                                        ChildAge = Convert.ToString(oneSupplierRoomType.ChildAge) ?? string.Empty,
                                        ExtraBed = Convert.ToString(oneSupplierRoomType.ExtraBed) ?? string.Empty,
                                        Bedrooms = Convert.ToString(oneSupplierRoomType.Bedrooms) ?? string.Empty,
                                        Smoking = Convert.ToString(oneSupplierRoomType.Smoking) ?? string.Empty,
                                        BedType = Convert.ToString(oneSupplierRoomType.BedTypeCode) ?? string.Empty,
                                        MinGuestOccupancy = Convert.ToString(oneSupplierRoomType.MinGuestOccupancy) ?? string.Empty,
                                        PromotionalVendorCode = Convert.ToString(oneSupplierRoomType.PromotionalVendorCode) ?? string.Empty,
                                        BeddingConfig = Convert.ToString(oneSupplierRoomType.BeddingConfig) ?? string.Empty,
                                        SupplierRoomExtractedAttributes = AttributeList.Where(w => w.Accommodation_SupplierRoomTypeMap_Id == RoomMapId).Select(s => new DC_SupplierRoomExtractedAttribute
                                        {
                                            Key = s.SystemAttributeKeyword,
                                            Value = s.SupplierRoomTypeAttribute
                                        }).ToList()
                                    };
                                    _lstSupplierRoomType.Add(newSupplierRoomType);
                                }

                                newSupplierData.SupplierRoomTypes = _lstSupplierRoomType;

                                _lstSupplierData.Add(newSupplierData);
                            }

                            newHotelRoomTypeMappingRequest.SupplierData = _lstSupplierData;

                            _lstHotelRoomTypeMappingRequests.Add(newHotelRoomTypeMappingRequest);
                        }

                        RQ_Broker.HotelRoomTypeMappingRequests = _lstHotelRoomTypeMappingRequests;

                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    if (IsCalledFromTTFU)
                    {
                        throw ex;
                    }
                    else
                    {
                        CallLogVerbose(File_Id, "MATCH", ex.Message, obj.CurrentBatch);
                    }
                }
                #endregion

                if (!IsCalledFromTTFU)
                {
                    PLog.PercentageValue = 40;
                    USD.AddStaticDataUploadProcessLog(PLog);

                    CallLogVerbose(File_Id, "MATCH", "Calling Broker API, Waiting for response..", obj.CurrentBatch);
                }

                #region GetResponse From Broker API

                try
                {
                    RS_Broker = null;
                    DL_UploadStaticData _objUploadStaticData = new DL_UploadStaticData();
                    RS_Broker = _objUploadStaticData.DataHandler_UpdateSRTByML(RQ_Broker);
                }
                catch (Exception ex)
                {
                    if (IsCalledFromTTFU)
                    {
                        throw ex;
                    }
                    else
                    {
                        CallLogVerbose(File_Id, "MATCH", ex.Message, obj.CurrentBatch);
                    }
                }

                if (!IsCalledFromTTFU)
                {
                    CallLogVerbose(File_Id, "MATCH", "Received response from Broker API", obj.CurrentBatch);
                }

                if (RS_Broker != null)
                {
                    if (RS_Broker.HotelRoomTypeMappingResponses != null)
                    {
                        if (!IsCalledFromTTFU)
                        {
                            CallLogVerbose(File_Id, "MATCH", "Returned Hotel RoomType Mapping Responses count : " + RS_Broker.HotelRoomTypeMappingResponses.Count().ToString(), obj.CurrentBatch);
                        }
                    }
                    else
                    {
                        if (IsCalledFromTTFU)
                        {
                            return false;
                        }
                        else
                        {
                            CallLogVerbose(File_Id, "MATCH", "Returned NULL Hotel RoomType Mapping Responses.", obj.CurrentBatch);
                        }
                    }
                }
                else
                {
                    if (IsCalledFromTTFU)
                    {
                        return false;
                    }
                    else
                    {
                        CallLogVerbose(File_Id, "MATCH", "Returned NULL from Broker.", obj.CurrentBatch);
                    }
                }

                #endregion

                if (!IsCalledFromTTFU)
                {
                    PLog.PercentageValue = 70;
                    USD.AddStaticDataUploadProcessLog(PLog);

                    CallLogVerbose(File_Id, "MATCH", "Read the response and update the data fetched in the batch", obj.CurrentBatch);
                }

                #region Read the response and update the data fetched in the batch
                List<DC_Accommodation_SupplierRoomTypeMapping_Values> ListOfMappedRooms = new List<DC_Accommodation_SupplierRoomTypeMapping_Values>();
                try
                {
                    if (RS_Broker != null)
                    {
                        if (RS_Broker.HotelRoomTypeMappingResponses != null)
                        {
                            foreach (var itemResponses in RS_Broker.HotelRoomTypeMappingResponses)
                            {
                                if (itemResponses.SupplierData != null)
                                {
                                    foreach (var itemsupplierData in itemResponses.SupplierData)
                                    {
                                        if (itemsupplierData.SupplierRoomTypes != null)
                                        {
                                            foreach (var itemRoomTypes in itemsupplierData.SupplierRoomTypes)
                                            {
                                                var recordToUpdate = resSupplierRoomTypeMap.Where(w => w.Accommodation_SupplierRoomTypeMapping_Id == Guid.Parse(itemRoomTypes.AccommodationSupplierRoomTypeMappingId)).FirstOrDefault();

                                                if (recordToUpdate != null)
                                                {
                                                    if (itemRoomTypes.ProposedMappings != null && itemRoomTypes.ProposedMappings.Count > 0)
                                                    {
                                                        recordToUpdate.Edit_Date = DateTime.Now;
                                                        recordToUpdate.Edit_User = "PENDING_TO_UPDATE";

                                                        foreach (var mappingItem in itemRoomTypes.ProposedMappings)
                                                        {
                                                            var MappingStatus = string.Empty;
                                                            if (mappingItem.Status.Trim().ToUpper() == "MAPPED") //Automapped
                                                            {
                                                                MappingStatus = "AUTOMAPPED";
                                                            }
                                                            else if (mappingItem.Status.Trim().ToUpper() == "MATCHED") //Review
                                                            {
                                                                MappingStatus = "REVIEW";
                                                            }
                                                            else if (mappingItem.Status.Trim().ToUpper() == "POTENTIALNEW") //ADD
                                                            {
                                                                recordToUpdate.MappingStatus = "ADD";
                                                            }
                                                            else if (mappingItem.Status.Trim().ToUpper() == "NOMATCH") //Unmapped
                                                            {
                                                                MappingStatus = "UNMAPPED";
                                                            }
                                                            else
                                                            {
                                                                MappingStatus = mappingItem.Status.Trim().ToUpper();
                                                            }

                                                            if (!string.IsNullOrWhiteSpace(MappingStatus))
                                                            {
                                                                if (MappingStatus.Length > 100)
                                                                {
                                                                    MappingStatus = MappingStatus.Substring(0, 100);
                                                                }
                                                            }

                                                            Guid Accommodation_RoomInfo_Id;
                                                            if (Guid.TryParse(mappingItem.AccommodationRoomInfoId, out Accommodation_RoomInfo_Id))
                                                            {
                                                                ListOfMappedRooms.Add(new DC_Accommodation_SupplierRoomTypeMapping_Values
                                                                {
                                                                    Accommodation_SupplierRoomTypeMapping_Value_Id = Guid.NewGuid(),
                                                                    Accommodation_SupplierRoomTypeMapping_Id = Guid.Parse(itemRoomTypes.AccommodationSupplierRoomTypeMappingId),
                                                                    Accommodation_RoomInfo_Id = Accommodation_RoomInfo_Id,
                                                                    MatchingScore = Convert.ToString(mappingItem.Score),
                                                                    SystemMappingStatus = MappingStatus,
                                                                    Edit_SystemUser = "ML_BROKER_API"
                                                                });
                                                            }

                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    if (IsCalledFromTTFU)
                    {
                        throw ex;
                    }
                    else
                    {
                        CallLogVerbose(File_Id, "MATCH", ex.Message, obj.CurrentBatch);
                    }
                }
                #endregion

                if (!IsCalledFromTTFU)
                {
                    PLog.PercentageValue = 80;
                    USD.AddStaticDataUploadProcessLog(PLog);

                    CallLogVerbose(File_Id, "MATCH", "Update the Supplier Room Type mapping data into DB", obj.CurrentBatch);
                }

                #region Update the Supplier Room Type mapping data into DB
                foreach (var itemToUpdate in resSupplierRoomTypeMap.Where(w => w.Edit_User == "PENDING_TO_UPDATE").ToList())
                {
                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        context.Database.CommandTimeout = 0;
                        var SupplierRoomTypeId = itemToUpdate.Accommodation_SupplierRoomTypeMapping_Id;

                        var ASRTM = context.Accommodation_SupplierRoomTypeMapping.Find(SupplierRoomTypeId);

                        var ExistingMappedRecords = context.Accommodation_SupplierRoomTypeMapping_Values.Where(w => w.Accommodation_SupplierRoomTypeMapping_Id == SupplierRoomTypeId).ToList();
                        var CurrentMappedRecords = ListOfMappedRooms.Where(w => w.Accommodation_SupplierRoomTypeMapping_Id == SupplierRoomTypeId).ToList();

                        int AUTOMAPPED_CNT = CurrentMappedRecords.Where(w => w.SystemMappingStatus == "AUTOMAPPED").Count();
                        var MAPPED_CNT = CurrentMappedRecords.Where(w => w.SystemMappingStatus == "MAPPED").Count();
                        var REVIEW_CNT = CurrentMappedRecords.Where(w => w.SystemMappingStatus == "REVIEW").Count();
                        if (ASRTM != null)
                        {
                            if (itemToUpdate.MappingStatus == "ADD" && ASRTM.MappingStatus != itemToUpdate.MappingStatus)
                            {
                                ASRTM.MappingStatus = itemToUpdate.MappingStatus;
                                ASRTM.Edit_Date = DateTime.Now;
                                ASRTM.Edit_User = "ML_BROKER_API";
                            }
                            else if (AUTOMAPPED_CNT != 0 && ASRTM.MappingStatus != "AUTOMAPPED")
                            {
                                ASRTM.MappingStatus = "AUTOMAPPED";
                                ASRTM.Edit_User = CallingUser;
                                ASRTM.Edit_Date = DateTime.Now;
                            }
                            else if (AUTOMAPPED_CNT == 0 && MAPPED_CNT != 0 && ASRTM.MappingStatus != "MAPPED")
                            {
                                ASRTM.MappingStatus = "MAPPED";
                                ASRTM.Edit_User = CallingUser;
                                ASRTM.Edit_Date = DateTime.Now;
                            }
                            else if (AUTOMAPPED_CNT == 0 && MAPPED_CNT == 0 && REVIEW_CNT != 0 && ASRTM.MappingStatus != "REVIEW")
                            {
                                ASRTM.MappingStatus = "REVIEW";
                                ASRTM.Edit_User = CallingUser;
                                ASRTM.Edit_Date = DateTime.Now;
                            }
                            else if (AUTOMAPPED_CNT == 0 && MAPPED_CNT == 0 && REVIEW_CNT == 0 && ASRTM.MappingStatus != "UNMAPPED")
                            {
                                ASRTM.MappingStatus = "UNMAPPED";
                                ASRTM.Edit_User = CallingUser;
                                ASRTM.Edit_Date = DateTime.Now;
                            }
                        }

                        foreach (var currentmap in CurrentMappedRecords)
                        {
                            var Found = ExistingMappedRecords.Where(w => w.Accommodation_SupplierRoomTypeMapping_Id == currentmap.Accommodation_SupplierRoomTypeMapping_Id && w.Accommodation_RoomInfo_Id == currentmap.Accommodation_RoomInfo_Id).FirstOrDefault();
                            if (Found != null)
                            {
                                if (!string.IsNullOrWhiteSpace(CallingUser))
                                {
                                    Found.Edit_User = CallingUser;
                                }
                                Found.Edit_SystemUser = "ML_BROKER_API";
                                Found.SystemEditDate = DateTime.Now;
                                Found.SystemMappingStatus = currentmap.SystemMappingStatus;
                                Found.MatchingScore = Convert.ToDouble(currentmap.MatchingScore);
                            }
                            else
                            {
                                context.Accommodation_SupplierRoomTypeMapping_Values.Add(new Accommodation_SupplierRoomTypeMapping_Values
                                {
                                    Accommodation_SupplierRoomTypeMapping_Value_Id = Guid.NewGuid(),
                                    Accommodation_RoomInfo_Id = currentmap.Accommodation_RoomInfo_Id,
                                    MatchingScore = Convert.ToDouble(currentmap.MatchingScore),
                                    Accommodation_SupplierRoomTypeMapping_Id = currentmap.Accommodation_SupplierRoomTypeMapping_Id,
                                    Create_Date = DateTime.Now,
                                    Create_User = "ML_BROKER_API",
                                    Edit_User = CallingUser,
                                    Edit_SystemUser = "ML_BROKER_API",
                                    SystemEditDate = DateTime.Now,
                                    SystemMappingStatus = currentmap.SystemMappingStatus
                                });
                            }
                        }

                        foreach (var existingmap in ExistingMappedRecords)
                        {
                            if (existingmap.SystemEditDate == null || existingmap.SystemEditDate < existingmap.UserEditDate)
                            {
                                if (!string.IsNullOrWhiteSpace(CallingUser))
                                {
                                    existingmap.Edit_User = CallingUser;
                                }
                                existingmap.Edit_SystemUser = "ML_BROKER_API";
                                existingmap.SystemEditDate = DateTime.Now;
                                existingmap.SystemMappingStatus = "UNMAPPED";
                                existingmap.MatchingScore = null;
                            }
                        }

                        try
                        {
                            context.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            if (!IsCalledFromTTFU)
                            {
                                CallLogVerbose(File_Id, "MATCH", ex.Message, obj.CurrentBatch);
                            }
                        }

                    }
                }
                #endregion

                if (!IsCalledFromTTFU)
                {
                    PLog.PercentageValue = 100;
                    USD.AddStaticDataUploadProcessLog(PLog);
                }
            }

            catch (Exception e)
            {
                //throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating hotel mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
                return false;
            }

            return true;
        }

        #endregion

        #region Country Mapping
        //public List<DataContracts.Mapping.DC_CountryMapping> GetCountryMapping(int PageNo, int PageSize, Guid Supplier_Id, Guid Country_Id, string sts, string SortBy)
        public List<DataContracts.Mapping.DC_CountryMapping> GetCountryMapping(DataContracts.Mapping.DC_CountryMappingRQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var prodMapSearch = from a in context.m_CountryMapping select a;

                    if (RQ.Supplier_Id != Guid.Empty)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.Supplier_Id == RQ.Supplier_Id
                                        select a;
                    }

                    //if (RQ.Country_Id != Guid.Empty)
                    //{
                    //    prodMapSearch = from a in prodMapSearch
                    //                    where a.Country_Id == RQ.Country_Id
                    //                    select a;
                    //}
                    if (!string.IsNullOrWhiteSpace(RQ.SystemCountryName))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.CountryName.Contains(RQ.SystemCountryName)
                                        select a;
                    }

                    if (RQ.Status.ToUpper().IndexOf("ALL") == -1)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.Status == RQ.Status
                                        select a;
                    }

                    if (RQ.StatusExcept != null)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.Status.ToUpper() != RQ.StatusExcept.ToUpper()
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.SupplierCountryName))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.CountryName.Contains(RQ.SupplierCountryName)
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.SupplierCountryCode))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.CountryCode.Trim() == RQ.SupplierCountryCode.Trim()
                                        select a;
                    }

                    int total;

                    total = prodMapSearch.Count();

                    var skip = RQ.PageSize * RQ.PageNo;

                    IQueryable<DataContracts.Mapping.DC_CountryMapping> prodMapList;

                    prodMapList = (from a in prodMapSearch
                                   join m in context.m_CountryMaster on a.Country_Id equals m.Country_Id into j
                                   from jd in j.DefaultIfEmpty()
                                       //orderby (SortBy)
                                   orderby a.CountryName
                                   select new DataContracts.Mapping.DC_CountryMapping
                                   {
                                       CountryMapping_Id = a.CountryMapping_Id,
                                       Country_Id = a.Country_Id,
                                       TotalRecord = total,
                                       Supplier_Id = a.Supplier_Id,
                                       CountryCode = a.CountryCode,
                                       OldCountryCode = a.CountryCode,
                                       CountryName = a.CountryName,
                                       Create_Date = a.Create_Date,
                                       Create_User = a.Create_User,
                                       Edit_Date = a.Edit_Date,
                                       Edit_User = a.Edit_User,
                                       MapID = a.MapID,
                                       Status = a.Status,
                                       SupplierName = a.SupplierName,
                                       Code = jd.Code,
                                       Name = jd.Name,
                                       Latitude = a.Latitude,
                                       Longitude = a.Longitude,
                                       ContinentCode = a.ContinentCode,
                                       ContinentName = a.ContinentName,
                                       SupplierImporrtFile_Id = a.SupplierImportFile_Id ?? Guid.Empty,
                                       Batch = a.Batch ?? 0,
                                       ReRunSupplierImporrtFile_Id = a.ReRun_SupplierImportFile_Id ?? Guid.Empty,
                                       ReRunBatch = a.ReRun_Batch ?? 0
                                   }).Skip(skip).Take(RQ.PageSize);
                    var result = prodMapList.ToList();

                    if (!string.IsNullOrWhiteSpace(RQ.Status))
                    {

                        foreach (var item in result)
                        {
                            if (RQ.Status.ToUpper().Trim() == "UNMAPPED" && string.IsNullOrWhiteSpace(item.Name))
                            {
                                if (item.CountryName != null && item.CountryCode != null)
                                {
                                    var countryName = item.CountryName.Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("country", "").ToUpper();
                                    var countryCode = item.CountryCode.Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("country", "").ToUpper();
                                    var searchCountry = context.m_CountryMaster.Where(a => (a.Name.ToUpper().Trim() == countryName.ToUpper().Trim())).FirstOrDefault();
                                    if (searchCountry == null)
                                    {
                                        searchCountry = context.m_CountryMaster.Where(a => a.Name.ToUpper().Trim().Contains(countryName.ToUpper().Trim())).FirstOrDefault();
                                        if (searchCountry == null)
                                        {
                                            searchCountry = context.m_CountryMaster.Where(a => (a.Code.ToUpper().Trim() == countryCode.ToUpper().Trim())).FirstOrDefault();
                                            if (searchCountry == null)
                                                searchCountry = context.m_CountryMaster.Where(a => a.ISO3166_1_Alpha_2.ToUpper().Trim().Equals(countryCode.ToUpper().Trim())).FirstOrDefault();
                                            if (searchCountry == null)
                                                searchCountry = context.m_CountryMaster.Where(a => a.ISO3166_1_Alpha_3.ToUpper().Trim().Equals(countryCode.ToUpper().Trim())).FirstOrDefault();
                                        }
                                    }
                                    if (searchCountry != null)
                                    {
                                        item.MasterNameWithCode = searchCountry.Name + " (" + searchCountry.Code + ")";
                                        item.Code = searchCountry.Code;
                                        item.MasterCountry_Id = Convert.ToString(searchCountry.Country_Id);
                                    }
                                }
                            }
                        }
                    }

                    return result;
                }
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching country mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool CountryMappingMatch(DataContracts.Masters.DC_Supplier obj)
        {
            bool ret = true;
            Guid File_Id = new Guid();
            File_Id = Guid.Parse(obj.File_Id.ToString());
            PLog.SupplierImportFileProgress_Id = Guid.NewGuid();
            PLog.SupplierImportFile_Id = obj.File_Id;
            PLog.Step = "MAP";
            PLog.Status = "MAPPING";
            PLog.CurrentBatch = obj.CurrentBatch ?? 0;
            PLog.TotalBatch = obj.TotalBatch ?? 0;

            DL_UploadStaticData staticdata = new DL_UploadStaticData();
            List<DC_SupplierImportFileDetails> file = new List<DC_SupplierImportFileDetails>();
            DC_SupplierImportFileDetails_RQ fileRQ = new DC_SupplierImportFileDetails_RQ();
            fileRQ.SupplierImportFile_Id = File_Id;
            file = staticdata.GetStaticDataFileDetail(fileRQ);

            if (obj != null)
            {
                string CurSupplierName = obj.Name;
                Guid CurSupplier_Id = Guid.Parse(obj.Supplier_Id.ToString());

                List<DataContracts.STG.DC_stg_SupplierCountryMapping> clsSTGCountry = new List<DataContracts.STG.DC_stg_SupplierCountryMapping>();
                List<DataContracts.STG.DC_stg_SupplierCountryMapping> clsSTGCountryInsert = new List<DataContracts.STG.DC_stg_SupplierCountryMapping>();
                List<DC_CountryMapping> clsMappingCountry = new List<DC_CountryMapping>();

                CallLogVerbose(File_Id, "MAP", "Fetching Staged Countries.", obj.CurrentBatch);
                DataContracts.STG.DC_stg_SupplierCountryMapping_RQ RQ = new DataContracts.STG.DC_stg_SupplierCountryMapping_RQ();
                RQ.SupplierName = CurSupplierName;
                RQ.PageNo = 0;
                RQ.PageSize = int.MaxValue;
                RQ.SupplierImportFile_Id = File_Id;
                //Get Data from STG Country data with File id and SupplierName
                clsSTGCountry = staticdata.GetSTGCountryData(RQ);

                PLog.PercentageValue = 15;
                USD.AddStaticDataUploadProcessLog(PLog);

                CallLogVerbose(File_Id, "MAP", "Fetching Existing Mapping Data.", obj.CurrentBatch);

                //Getting Country Data from mapping Table For Supplier ID
                #region
                DC_CountryMappingRQ RQMapping = new DC_CountryMappingRQ();
                if (CurSupplier_Id != Guid.Empty)
                    RQMapping.Supplier_Id = CurSupplier_Id;
                RQMapping.PageNo = 0;
                RQMapping.PageSize = int.MaxValue;
                RQMapping.Status = "ALL";
                clsMappingCountry = GetCountryMapping_ForHandler(RQMapping);

                #endregion
                PLog.PercentageValue = 15;
                USD.AddStaticDataUploadProcessLog(PLog);

                CallLogVerbose(File_Id, "MAP", "Updating Existing Countries.", obj.CurrentBatch);
                clsMappingCountry = clsMappingCountry.Select(c =>
                {
                    c.CountryCode = (clsSTGCountry.Where(s => (s.CountryName ?? s.CountryCode) == (c.CountryName ?? c.CountryCode)).Select(s1 => s1.CountryCode).FirstOrDefault()) ?? c.CountryCode;
                    c.Edit_Date = DateTime.Now;
                    c.Edit_User = "TLGX";
                    c.ActionType = "UPDATE";
                    c.stg_Country_Id = (clsSTGCountry.Where(s => (s.CountryName ?? s.CountryCode) == (c.CountryName ?? c.CountryCode)).Select(s1 => s1.stg_Country_Id).FirstOrDefault());
                    c.ContinentCode = (clsSTGCountry.Where(s => (s.CountryName ?? s.CountryCode) == (c.CountryName ?? c.CountryCode)).Select(s1 => s1.ContinentCode).FirstOrDefault()) ?? c.ContinentCode;
                    c.ContinentName = (clsSTGCountry.Where(s => (s.CountryName ?? s.CountryCode) == (c.CountryName ?? c.CountryCode)).Select(s1 => s1.ContinentName).FirstOrDefault()) ?? c.ContinentName;
                    return c;
                }).ToList();

                //List<DataContracts.STG.DC_STG_Mapping_Table_Ids> lstobj = new List<DataContracts.STG.DC_STG_Mapping_Table_Ids>();
                //lstobj.InsertRange(lstobj.Count, clsMappingCountry.Where(a => ((a.stg_Country_Id == Guid.Empty) ? Guid.Empty : a.stg_Country_Id) != Guid.Empty && a.ActionType == "UPDATE"
                //    && (a.stg_Country_Id ?? Guid.Empty) != Guid.Empty).Select
                //   (g => new DataContracts.STG.DC_STG_Mapping_Table_Ids
                //   {
                //       STG_Mapping_Table_Id = Guid.NewGuid(),
                //       File_Id = obj.File_Id,
                //       STG_Id = g.stg_Country_Id,
                //       Mapping_Id = g.CountryMapping_Id,
                //       Batch = obj.CurrentBatch ?? 0
                //   }));

                CallLogVerbose(File_Id, "MAP", "Checking for New Countries in File.", obj.CurrentBatch);
                clsSTGCountryInsert = clsSTGCountry.Where(p => !clsMappingCountry.Any(p2 => (p2.SupplierName.ToString().Trim().ToUpper() == p.SupplierName.ToString().Trim().ToUpper())
                && (
                    (p.CountryCode != null && p2.CountryCode == p.CountryCode)
                    || (p.CountryCode == null && p2.CountryName.ToString().Trim().ToUpper() == p.CountryName.ToString().Trim().ToUpper())
                ))).ToList();

                PLog.PercentageValue = 48;
                USD.AddStaticDataUploadProcessLog(PLog);

                CallLogVerbose(File_Id, "MAP", "Removing UnEdited Data.", obj.CurrentBatch);

                clsSTGCountry.RemoveAll(p => clsSTGCountryInsert.Any(p2 => (p2.stg_Country_Id == p.stg_Country_Id)));
                clsMappingCountry.RemoveAll(p => p.CountryCode == p.OldCountryCode);

                PLog.PercentageValue = 53;
                USD.AddStaticDataUploadProcessLog(PLog);

                CallLogVerbose(File_Id, "MAP", "Inserting New Countries.", obj.CurrentBatch);
                clsMappingCountry.InsertRange(clsMappingCountry.Count, clsSTGCountryInsert.Select
                (g => new DC_CountryMapping
                {
                    CountryMapping_Id = Guid.NewGuid(),
                    Country_Id = null,
                    Supplier_Id = CurSupplier_Id, //Guid.Parse(DictionaryLookup(mappingPrefix, "Supplier_Id", stgPrefix, Cursupplier_Id.ToString())),
                    SupplierName = g.SupplierName, //DictionaryLookup(mappingPrefix, "SupplierName", stgPrefix, CurSupplierName), 
                    CountryName = g.CountryName,  //DictionaryLookup(mappingPrefix, "CountryName", stgPrefix, g.CountryName), 
                    CountryCode = g.CountryCode, //DictionaryLookup(mappingPrefix, "CountryCode", stgPrefix, g.CountryCode), 
                    Status = "UNMAPPED", //DictionaryLookup(mappingPrefix, "Status", stgPrefix, "UNMAPPED"),                    
                    Create_Date = DateTime.Now,
                    Create_User = "TLGX_DataHandler",
                    Edit_Date = null,
                    Edit_User = null,
                    MapID = null,
                    Latitude = g.Latitude,
                    Longitude = g.Longitude,
                    ActionType = "INSERT",
                    stg_Country_Id = g.stg_Country_Id,
                    ContinentCode = g.ContinentCode,
                    ContinentName = g.ContinentName,
                    SupplierImporrtFile_Id = obj.File_Id ?? Guid.Empty,
                    Batch = obj.CurrentBatch ?? 0,
                    ReRunSupplierImporrtFile_Id = obj.File_Id ?? Guid.Empty,
                    ReRunBatch = obj.CurrentBatch ?? 0,
                    Remarks = "" //DictionaryLookup(mappingPrefix, "Remarks", stgPrefix, "")
                }));

                // lstobj.InsertRange(lstobj.Count, clsMappingCountry.Where(a => a.stg_Country_Id != null && a.ActionType == "INSERT"
                // && (a.stg_Country_Id ?? Guid.Empty) != Guid.Empty)
                //.Select
                //   (g => new DataContracts.STG.DC_STG_Mapping_Table_Ids
                //   {
                //       STG_Mapping_Table_Id = Guid.NewGuid(),
                //       File_Id = obj.File_Id,
                //       STG_Id = g.stg_Country_Id,
                //       Mapping_Id = g.CountryMapping_Id,
                //       Batch = obj.CurrentBatch ?? 0
                //   }));
                // bool idinsert = AddSTGMappingTableIDs(lstobj);

                PLog.PercentageValue = 58;
                USD.AddStaticDataUploadProcessLog(PLog);
                CallLogVerbose(File_Id, "MAP", "Updating / Inserting Database.", obj.CurrentBatch);
                if (clsMappingCountry.Count > 0)
                {
                    ret = UpdateCountryMapping(clsMappingCountry);
                    /* if (obj.CurrentBatch == 1)
                     {
                         DataContracts.UploadStaticData.DC_SupplierImportFile_Statistics objStat = new DC_SupplierImportFile_Statistics();
                         objStat.SupplierImportFile_Statistics_Id = Guid.NewGuid();
                         objStat.SupplierImportFile_Id = obj.File_Id;
                         objStat.FinalStatus = file[0].STATUS;
                         objStat.TotalRows = clsMappingCountry.Count;
                         objStat.Process_Date = DateTime.Now;
                         objStat.Process_User = file[0].PROCESS_USER;
                         DataContracts.DC_Message stat = USD.AddStaticDataUploadStatistics(objStat);
                     }*/
                }
            }

            PLog.PercentageValue = 100;
            USD.AddStaticDataUploadProcessLog(PLog);
            CallLogVerbose(File_Id, "MAP", "MAP Process Complete for Batch.", obj.CurrentBatch);
            return ret;
        }

        public List<DC_CountryMapping> UpdateCountryMappingStatus(DataContracts.Mapping.DC_MappingMatch obj)
        {
            DataContracts.Masters.DC_Supplier supdata = new DataContracts.Masters.DC_Supplier();
            supdata = obj.SupplierDetail;
            List<DC_CountryMapping> CMS = obj.lstCountryMapping;
            List<DC_SupplierImportAttributeValues> configs = obj.lstConfigs;
            string configWhere = "";
            string curSupplier = "";
            Guid? curSupplier_Id = Guid.Empty;
            configWhere = "";
            Guid File_Id = new Guid();
            File_Id = Guid.Parse(obj.File_Id.ToString());

            int totPriorities = obj.TotalPriorities;
            int curPriority = obj.CurrentPriority;
            int totConfigs = 0;
            int curConfig = 0;
            if (totPriorities <= 0)
                totPriorities = 1;
            int PerForEachPriority = 60 / totPriorities;

            if (supdata != null)
            {
                curSupplier = supdata.Name;
                curSupplier_Id = supdata.Supplier_Id;
            }
            List<DC_CountryMapping> ret = new List<DC_CountryMapping>();
            //foreach (var CM in CMS)
            //{
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var prodMapSearch = (from a in context.m_CountryMapping
                                             //join s in context.STG_Mapping_TableIds.AsNoTracking() on a.CountryMapping_Id equals s.Mapping_Id
                                         where a.ReRun_SupplierImportFile_Id == obj.File_Id && a.Country_Id == null && a.Supplier_Id == curSupplier_Id
                                         && a.Status == "UNMAPPED" && a.ReRun_Batch == obj.CurrentBatch
                                         select a);

                    bool isCodeCheck = false;
                    bool isNameCheck = false;
                    bool isLatLongCheck = false;

                    totConfigs = configs.Count;
                    curConfig = 0;

                    PLog.SupplierImportFileProgress_Id = Guid.NewGuid();
                    PLog.SupplierImportFile_Id = obj.File_Id;
                    PLog.Step = "MATCH";
                    PLog.Status = "MATCHING";
                    PLog.CurrentBatch = curPriority;
                    PLog.TotalBatch = totPriorities;
                    CallLogVerbose(File_Id, "MATCH", "Applying Matching Combination " + curPriority.ToString() + " out of Total " + totPriorities + " Combinations.", obj.CurrentBatch);

                    foreach (DC_SupplierImportAttributeValues config in configs)
                    {
                        configWhere = configWhere + " " + config.AttributeValue.Replace("m_CountryMaster.", "").Trim() + ",";
                    }
                    configWhere = configWhere.Remove(configWhere.Length - 1);
                    CallLogVerbose(File_Id, "MATCH", "Matching Combination " + curPriority.ToString() + " consist of Match by " + configWhere, obj.CurrentBatch);

                    foreach (DC_SupplierImportAttributeValues config in configs)
                    {
                        curConfig = curConfig + 1;
                        string CurrConfig = "";
                        CurrConfig = config.AttributeValue.Replace("m_CountryMaster.", "").Trim().ToUpper();
                        CallLogVerbose(File_Id, "MATCH", "Applying Check for " + CurrConfig, obj.CurrentBatch);
                        if (CurrConfig == "CODE")
                        {
                            isCodeCheck = true;
                            prodMapSearch = (from a in prodMapSearch
                                             join m in context.m_CountryMaster on a.CountryCode equals m.Code
                                             where a.CountryCode != null && m.Code != null
                                             select a);
                        }
                        if (CurrConfig == "NAME")
                        {
                            isNameCheck = true;
                            prodMapSearch = (from a in prodMapSearch
                                             join m in context.m_CountryMaster on a.CountryName equals m.Name
                                             where a.CountryName != null && m.Name != null
                                             select a);
                        }
                        if (CurrConfig == "ISO3166-1-Alpha-2".Trim().ToUpper())
                        {
                            isCodeCheck = true;
                            prodMapSearch = (from a in prodMapSearch
                                             join m in context.m_CountryMaster on a.CountryCode equals m.ISO3166_1_Alpha_2
                                             where a.CountryCode != null && m.ISO3166_1_Alpha_2 != null
                                             select a);
                        }
                        if (CurrConfig == "ISO3166-1-Alpha-3".Trim().ToUpper())
                        {
                            isCodeCheck = true;
                            prodMapSearch = (from a in prodMapSearch
                                             join m in context.m_CountryMaster on a.CountryCode equals m.ISO3166_1_Alpha_3
                                             where a.CountryCode != null && m.ISO3166_1_Alpha_3 != null
                                             select a);
                        }
                        if (CurrConfig == "LATITUDE")
                        {
                            isLatLongCheck = true;
                            prodMapSearch = (from a in prodMapSearch
                                             join m in context.m_CountryMaster on new { a.Latitude, a.Longitude } equals new { m.Latitude, m.Longitude }
                                             where m.Latitude != null && a.Latitude != null && m.Longitude != null && a.Longitude != null
                                             select a);
                        }
                        //PLog.PercentageValue = (70 / totPriorities) / totConfigs;
                        //PLog.PercentageValue = (PerForEachPriority * (curPriority - 1)) + (PerForEachPriority / totConfigs);
                        PLog.PercentageValue = (PerForEachPriority * (curPriority - 1)) + ((PerForEachPriority / totConfigs) * curConfig);
                        USD.AddStaticDataUploadProcessLog(PLog);
                    }

                    List<DC_CountryMapping> res = new List<DC_CountryMapping>();

                    if (isNameCheck || isCodeCheck || isLatLongCheck)
                    {
                        res = (from a in prodMapSearch
                               select new DataContracts.Mapping.DC_CountryMapping
                               {
                                   CountryMapping_Id = a.CountryMapping_Id,
                                   Country_Id = a.Country_Id,
                                   Supplier_Id = a.Supplier_Id,
                                   CountryCode = a.CountryCode,
                                   CountryName = a.CountryName,
                                   Create_Date = a.Create_Date,
                                   Create_User = a.Create_User,
                                   Edit_Date = a.Edit_Date,
                                   Edit_User = a.Edit_User,
                                   MapID = a.MapID,
                                   Status = a.Status,
                                   SupplierName = a.SupplierName,
                                   Latitude = a.Latitude,
                                   Longitude = a.Longitude
                               }).ToList();

                        if (totPriorities == curPriority)
                        {
                            PLog.PercentageValue = 65;
                            USD.AddStaticDataUploadProcessLog(PLog);
                        }

                        CallLogVerbose(File_Id, "MATCH", "Looking for Match in Master Data for Matching Combination " + curPriority.ToString() + ".", obj.CurrentBatch);
                        res = res.Select(c =>
                        {
                            c.Country_Id = (context.m_CountryMaster
                                            .Where(s => (
                                                            //(isCodeCheck && s.Code == c.CountryCode && isNameCheck && s.Name == c.CountryName) ||
                                                            //(isCodeCheck && (s.Code == c.CountryCode || s.ISO3166_1_Alpha_2 == c.CountryCode || s.ISO3166_1_Alpha_3 == c.CountryCode) && (!isNameCheck)) ||
                                                            //((!isCodeCheck) && isNameCheck && s.Name == c.CountryName)

                                                            ((isCodeCheck && (s.Code == c.CountryCode || s.ISO3166_1_Alpha_2 == c.CountryCode || s.ISO3166_1_Alpha_3 == c.CountryCode)) || (!isCodeCheck)) &&
                                                            ((isNameCheck && s.Name == c.CountryName) || (!isNameCheck)) &&
                                                            ((isLatLongCheck && s.Latitude == c.Latitude && s.Longitude == c.Longitude) || (!isLatLongCheck))
                                                        )
                                                   )
                                            .Select(s1 => s1.Country_Id)
                                            .FirstOrDefault()
                                            );
                            return c;
                        }).ToList();

                        if (totPriorities == curPriority)
                        {
                            PLog.PercentageValue = 70;
                            USD.AddStaticDataUploadProcessLog(PLog);
                        }

                        CallLogVerbose(File_Id, "MATCH", "Setting Appropriate Status.", obj.CurrentBatch);

                        res.RemoveAll(p => p.Country_Id == Guid.Empty);
                        res = res.Select(c =>
                        {
                            c.MatchedBy = curPriority - 1;
                            c.Status = ("REVIEW"); return c;
                        }).ToList();

                        if (totPriorities == curPriority)
                        {
                            PLog.PercentageValue = 75;
                            USD.AddStaticDataUploadProcessLog(PLog);
                        }
                        CallLogVerbose(File_Id, "MATCH", res.Count.ToString() + " Matches Found for Combination " + curPriority.ToString() + ".", obj.CurrentBatch);
                        CallLogVerbose(File_Id, "MATCH", "Updating into Database.", obj.CurrentBatch);
                        if (UpdateCountryMapping(res))
                        {
                            if ((obj.FileMode ?? "ALL") == "ALL" && totPriorities == curPriority)
                            {
                                DataContracts.UploadStaticData.DC_SupplierImportFile_Statistics objStat = new DC_SupplierImportFile_Statistics();
                                objStat.SupplierImportFile_Statistics_Id = Guid.NewGuid();
                                objStat.SupplierImportFile_Id = obj.File_Id;
                                objStat.From = "MATCHING";
                                DataContracts.DC_Message stat = USD.AddStaticDataUploadStatistics(objStat);

                            }

                            if (curSupplier_Id != null)
                            {
                                DataContracts.Mapping.DC_CountryMappingRQ RQ = new DC_CountryMappingRQ();
                                RQ.Supplier_Id = curSupplier_Id;
                                RQ.PageNo = 0;
                                RQ.PageSize = int.MaxValue;
                                RQ.Status = "UNMAPPED";
                                res = GetCountryMapping(RQ);
                            }
                        }

                        if (totPriorities == curPriority)
                        {
                            PLog.PercentageValue = 100;
                            USD.AddStaticDataUploadProcessLog(PLog);
                        }
                    }
                    else
                    {
                        if (curSupplier_Id != null)
                        {
                            DataContracts.Mapping.DC_CountryMappingRQ RQ = new DC_CountryMappingRQ();
                            RQ.Supplier_Id = curSupplier_Id;
                            RQ.PageNo = 0;
                            RQ.PageSize = int.MaxValue;
                            RQ.Status = "UNMAPPED";
                            res = GetCountryMapping(RQ);
                        }
                    }
                    CallLogVerbose(File_Id, "MATCH", "Update Done.", obj.CurrentBatch);
                    return res;

                }

            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating country mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool UpdateCountryMapping(List<DataContracts.Mapping.DC_CountryMapping> obj)
        {
            foreach (var CM in obj)
            {
                if (CM.CountryMapping_Id == null || CM.Supplier_Id == null)
                {
                    continue;
                }

                try
                {
                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        var search = (from a in context.m_CountryMapping
                                      where a.CountryMapping_Id == CM.CountryMapping_Id
                                      select a).FirstOrDefault();
                        if (search != null)
                        {
                            //if (CM.Status != (search.Status ?? string.Empty))
                            //{
                            //    search.Status = CM.Status;
                            //    search.Edit_Date = CM.Edit_Date;
                            //    search.Edit_User = CM.Edit_User;
                            //}
                            //else
                            //{
                            search.Country_Id = CM.Country_Id;
                            search.Supplier_Id = CM.Supplier_Id;
                            if (CM.MatchedBy != null)
                                search.MatchedBy = CM.MatchedBy;
                            search.Status = CM.Status;
                            search.Edit_Date = CM.Edit_Date;
                            search.Edit_User = CM.Edit_User;
                            search.Remarks = CM.Remarks;
                            if (CM.ContinentCode != null)
                                search.ContinentCode = CM.ContinentCode;
                            if (CM.ContinentName != null)
                                search.ContinentName = CM.ContinentName;
                            //}

                            context.SaveChanges();
                        }
                        else
                        {
                            DataLayer.m_CountryMapping objNew = new m_CountryMapping();
                            objNew.CountryMapping_Id = CM.CountryMapping_Id;
                            objNew.Country_Id = CM.Country_Id;
                            objNew.Supplier_Id = CM.Supplier_Id;
                            objNew.SupplierName = CM.SupplierName;
                            objNew.CountryName = CM.CountryName;
                            objNew.CountryCode = CM.CountryCode;
                            objNew.Status = CM.Status;
                            objNew.Create_Date = CM.Create_Date;
                            objNew.Create_User = CM.Create_User;
                            objNew.Edit_Date = CM.Edit_Date;
                            objNew.Edit_User = CM.Edit_User;
                            objNew.MapID = CM.MapID;
                            objNew.Remarks = CM.Remarks;
                            objNew.Latitude = CM.Latitude;
                            objNew.Longitude = CM.Longitude;
                            objNew.ContinentCode = CM.ContinentCode;
                            objNew.ContinentName = CM.ContinentName;
                            objNew.SupplierImportFile_Id = CM.SupplierImporrtFile_Id;
                            objNew.Batch = CM.Batch;
                            objNew.ReRun_SupplierImportFile_Id = CM.ReRunSupplierImporrtFile_Id;
                            objNew.ReRun_Batch = CM.ReRunBatch;
                            context.m_CountryMapping.Add(objNew);
                            context.SaveChanges();

                            context.USP_UpdateMapID("country");
                        }

                        //return true;
                    }
                }
                catch
                {
                    throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating country mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
                }
            }
            return true;
        }

        #endregion

        #region City Mapping
        public List<DataContracts.Mapping.DC_CityMapping> GetCityMapping(DataContracts.Mapping.DC_CityMapping_RQ param)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {


                    var prodMapSearch = (from a in context.m_CityMapping.AsNoTracking() select a).AsQueryable();
                    var countrymaster = context.m_CountryMaster.AsNoTracking().Select(s => s).AsQueryable();

                    var SuppliersMaster = context.Supplier.AsNoTracking().Select(s => s).AsQueryable();
                    var CityMaster = context.m_CityMaster.AsNoTracking().Select(s => s).AsQueryable();

                    var EntityMaster = context.m_CityMapping_EntityCount.AsNoTracking().AsQueryable();

                    if (param.CityMapping_Id != null)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.CityMapping_Id == (param.CityMapping_Id ?? Guid.Empty)
                                        select a;
                    }
                    if (param.Supplier_Id != null)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.Supplier_Id == param.Supplier_Id
                                        select a;
                    }

                    if (param.City_Id != null)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.City_Id == param.City_Id
                                        select a;
                    }

                    if (param.Country_Id != null)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.Country_Id == param.Country_Id
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(param.CityCode))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.CityCode == param.CityCode
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(param.CityName))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.CityName == param.CityName
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(param.Status))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.Status == param.Status
                                        select a;
                    }

                    if (param.StatusExcept != null)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.Status.ToUpper() != param.StatusExcept.ToUpper()
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(param.SupplierCountryName))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        join c in countrymaster on a.Country_Id equals c.Country_Id
                                        where (a.CountryName.ToUpper().Contains(param.SupplierCountryName.ToUpper())
                                        || c.Name.ToUpper().Contains(param.SupplierCountryName.ToUpper())
                                        )
                                        select a;
                        if (param.IsExact)
                        {
                            prodMapSearch = from a in prodMapSearch
                                            join c in countrymaster on a.Country_Id equals c.Country_Id
                                            where (a.CountryName.ToUpper() == param.SupplierCountryName.ToUpper()
                                            || c.Name.ToUpper() == param.SupplierCountryName.ToUpper()
                                            )
                                            select a;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(param.SupplierCityName))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.CityName.Contains(param.SupplierCityName)
                                        select a;
                        if (param.IsExact)
                        {
                            prodMapSearch = from a in prodMapSearch
                                            where a.CityName == param.SupplierCityName
                                            select a;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(param.SupplierCityCode))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.CityCode.Contains(param.SupplierCityCode)
                                        select a;
                        if (param.IsExact)
                        {
                            prodMapSearch = from a in prodMapSearch
                                            where a.CityCode == param.SupplierCityCode
                                            select a;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(param.EntityType))
                    {
                        string[] FilterArray = param.EntityType.Split(',');
                        var newEntityMaster = (from x in EntityMaster
                                               where FilterArray.Contains(x.EntityType)
                                               select x).AsQueryable();
                        prodMapSearch = (from p in prodMapSearch
                                         join e in newEntityMaster on p.CityMapping_Id equals e.CityMapping_Id
                                         select p);
                    }


                    int total;

                    total = prodMapSearch.Count();

                    var skip = param.PageSize * param.PageNo;

                    var canPage = skip < total;

                    //if (!canPage)
                    //    return null;
                    if (!string.IsNullOrWhiteSpace(param.ResultSet))
                    {
                        total = (from a in prodMapSearch
                                 join s in SuppliersMaster on a.Supplier_Id equals s.Supplier_Id
                                 join ct in CityMaster on a.City_Id equals ct.City_Id into ctl
                                 from ctld in ctl.DefaultIfEmpty()
                                 join m in countrymaster on a.Country_Id equals m.Country_Id into j
                                 from jd in j.DefaultIfEmpty()
                                 select a.CityName).Distinct().Count();
                        var prodMapList = (from a in prodMapSearch
                                           join s in SuppliersMaster on a.Supplier_Id equals s.Supplier_Id
                                           join ct in CityMaster on a.City_Id equals ct.City_Id into ctl
                                           from ctld in ctl.DefaultIfEmpty()
                                           join m in countrymaster on a.Country_Id equals m.Country_Id into j
                                           from jd in j.DefaultIfEmpty()
                                           select new DataContracts.Mapping.DC_CityMapping
                                           {
                                               CityName = a.CityName,
                                               TotalRecords = total
                                           }).Distinct().OrderBy(o => o.CityName).Skip(skip).Take(param.PageSize);
                        return prodMapList.ToList();
                    }
                    else
                    {
                        var CityMapList = (from a in prodMapSearch
                                               //join s in SuppliersMaster on a.Supplier_Id equals s.Supplier_Id
                                           join ct in CityMaster on a.City_Id equals ct.City_Id into ctl
                                           from ctld in ctl.DefaultIfEmpty()
                                           join m in countrymaster on a.Country_Id equals m.Country_Id into j
                                           from jd in j.DefaultIfEmpty()
                                               //orderby (param.SortBy)
                                           orderby a.CityName
                                           select new DataContracts.Mapping.DC_CityMapping
                                           {
                                               CityMapping_Id = a.CityMapping_Id,
                                               City_Id = a.City_Id,
                                               CityCode = a.CityCode,
                                               CityName = a.CityName,
                                               oldCityName = a.CityName,
                                               Country_Id = a.Country_Id,
                                               Create_Date = a.Create_Date,
                                               Create_User = a.Create_User,
                                               Edit_Date = a.Edit_Date,
                                               Edit_User = a.Edit_User,
                                               MapID = a.MapID,
                                               Supplier_Id = a.Supplier_Id,
                                               Status = a.Status,
                                               TotalRecords = total,
                                               SupplierName = a.SupplierName,
                                               CountryCode = a.CountryCode,
                                               CountryName = a.CountryName,
                                               MasterCountryCode = jd.Code,
                                               MasterCountryName = jd.Name,
                                               MasterCityCode = ctld.Code,
                                               Master_CityName = ctld.Name,
                                               Master_City_id = ctld.City_Id,
                                               MasterStateName = ctld.StateName,
                                               MasterStateCode = ctld.StateCode,
                                               StateCode = a.StateCode,
                                               StateName = a.StateName,
                                               Latitude = a.Latitude,
                                               Longitude = a.Longitude,
                                               SupplierImporrtFile_Id = a.SupplierImportFile_Id ?? Guid.Empty,
                                               Batch = a.Batch ?? 0,
                                               ReRunSupplierImporrtFile_Id = a.ReRun_SupplierImportFile_Id ?? Guid.Empty,
                                               ReRunBatch = a.ReRun_Batch ?? 0,
                                               EntityTypeFlag = (EntityMaster.Where(w => w.CityMapping_Id == a.CityMapping_Id)
                                                                .Select(s => new DC_CityMapping_EntityCount { EntityCityMapping_Id = s.EntityCityMapping_Id, CityMapping_Id = s.CityMapping_Id, EntityType = s.EntityType, Count = s.Count })
                                                                .OrderBy(p => p.EntityType).ToList()),
                                           }).Skip(skip).Take(param.PageSize).ToList();

                        //CityMapList = CityMapList.Select(c =>
                        //{
                        //    c.MasterStateName = (context.m_States
                        //                    .Where(s => (s.Country_Id == c.Country_Id))
                        //                    .Select(s => s.StateName)
                        //                    .FirstOrDefault()
                        //                    );
                        //    return c;
                        //}).ToList();

                        //if (param.CalledFromTLGX == null || (param.CalledFromTLGX.ToString() != "TLGX"))
                        if (string.IsNullOrWhiteSpace(param.CalledFromTLGX))
                        {
                            if (!string.IsNullOrWhiteSpace(param.Status))
                            {

                                foreach (var item in CityMapList)
                                {
                                    if (param.Status.ToUpper().Trim() == "UNMAPPED" && string.IsNullOrWhiteSpace(item.Master_CityName))
                                    {
                                        if (item.CityName != null && item.CityCode != null)
                                        {
                                            var cityName = item.CityName.Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("city", "").ToUpper();
                                            var cityCode = item.CityCode.Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("city", "").ToUpper();
                                            var searchCity = CityMaster.Where(a => a.Country_Id == item.Country_Id && (a.Name.ToUpper().Trim() == cityName.ToUpper().Trim())).FirstOrDefault();
                                            if (searchCity == null)
                                            {
                                                searchCity = CityMaster.Where(a => a.Country_Id == item.Country_Id && a.Name.ToUpper().Trim().Contains(cityName.ToUpper().Trim())).FirstOrDefault();
                                                //if (searchCity == null)
                                                //{
                                                //    searchCity = CityMaster.Where(a => a.Country_Id == item.Country_Id && (a.Name.ToUpper().Trim() == cityCode.ToUpper().Trim())).FirstOrDefault();
                                                //    if (searchCity == null)
                                                //        searchCity = CityMaster.Where(a => a.Country_Id == item.Country_Id && a.Name.ToUpper().Trim().Contains(cityCode.ToUpper().Trim())).FirstOrDefault();
                                                //}
                                            }
                                            if (searchCity != null)
                                            {
                                                item.Master_CityName = searchCity.Name;
                                                item.MasterCityCode = searchCity.Code;
                                                item.City_Id = searchCity.City_Id;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        return CityMapList;
                    }
                }
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching city mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool CityMappingMatch(DataContracts.Masters.DC_Supplier obj)
        {
            bool ret = false;
            DL_UploadStaticData staticdata = new DL_UploadStaticData();
            Guid File_Id = new Guid();
            File_Id = Guid.Parse(obj.File_Id.ToString());
            PLog.SupplierImportFileProgress_Id = Guid.NewGuid();
            PLog.SupplierImportFile_Id = obj.File_Id;
            PLog.Step = "MAP";
            PLog.Status = "MAPPING";
            PLog.CurrentBatch = obj.CurrentBatch ?? 0;
            PLog.TotalBatch = obj.TotalBatch ?? 0;
            List<DC_SupplierImportFileDetails> file = new List<DC_SupplierImportFileDetails>();
            DC_SupplierImportFileDetails_RQ fileRQ = new DC_SupplierImportFileDetails_RQ();
            fileRQ.SupplierImportFile_Id = File_Id;
            file = staticdata.GetStaticDataFileDetail(fileRQ);
            if (obj != null)
            {

                string CurSupplierName = obj.Name;
                Guid CurSupplier_Id = Guid.Parse(obj.Supplier_Id.ToString());

                List<DataContracts.STG.DC_stg_SupplierCityMapping> clsSTGCity = new List<DataContracts.STG.DC_stg_SupplierCityMapping>();
                List<DataContracts.STG.DC_stg_SupplierCityMapping> clsSTGCityInsert = new List<DataContracts.STG.DC_stg_SupplierCityMapping>();
                List<DC_CityMapping> clsMappingCity = new List<DC_CityMapping>();

                CallLogVerbose(File_Id, "MAP", "Fetching Staged Cities.", obj.CurrentBatch);
                DataContracts.STG.DC_stg_SupplierCityMapping_RQ RQ = new DataContracts.STG.DC_stg_SupplierCityMapping_RQ();
                RQ.SupplierName = CurSupplierName;
                RQ.PageNo = 0;
                RQ.PageSize = int.MaxValue;
                RQ.SupplierImportFile_Id = File_Id;
                clsSTGCity = staticdata.GetSTGCityData(RQ);
                PLog.PercentageValue = 15;
                USD.AddStaticDataUploadProcessLog(PLog);

                CallLogVerbose(File_Id, "MAP", "Fetching Existing Mapping Data.", obj.CurrentBatch);
                //DC_CityMapping_RQ RQCity = new DC_CityMapping_RQ();
                //if (CurSupplier_Id != Guid.Empty)
                //    RQCity.Supplier_Id = CurSupplier_Id;
                //RQCity.PageNo = 0;
                //RQCity.PageSize = int.MaxValue;
                //RQCity.CalledFromTLGX = "TLGX";
                ////RQ.Status = "ALL";
                //clsMappingCity = GetCityMapping(RQCity);
                PLog.PercentageValue = 26;
                USD.AddStaticDataUploadProcessLog(PLog);

                CallLogVerbose(File_Id, "MAP", "Updating Existing Cities.", obj.CurrentBatch);

                //Update Already ExistCity Data
                UpdateExistCity(obj, out clsMappingCity);
                //Commented by Ajay doing in diffent method
                //clsMappingCity = clsMappingCity.Select(c =>
                //{
                //    c.CityName = (clsSTGCity.Where(s => (s.CityCode ?? s.CityName) == (c.CityCode ?? c.CityName) && s.Country_Id == c.Country_Id)
                //    //.Where(s => s.CityCode == c.CityCode)
                //    .Select(s1 => s1.CityName)
                //    .FirstOrDefault()
                //    ) ?? c.CityName;
                //    c.Edit_Date = DateTime.Now;
                //    c.Edit_User = "TLGX_DataHandler";
                //    c.ActionType = "UPDATE";
                //    c.stg_City_Id = (clsSTGCity
                //    .Where(s => (s.CityCode ?? s.CityName) == (c.CityCode ?? c.CityName) && s.Country_Id == c.Country_Id)
                //    //.Where(s => s.CityCode == c.CityCode)
                //    .Select(s1 => s1.stg_City_Id)
                //    .FirstOrDefault()
                //    );
                //    c.StateCode = (clsSTGCity
                //    .Where(s => (s.CityCode ?? s.CityName) == (c.CityCode ?? c.CityName) && s.Country_Id == c.Country_Id)
                //    //.Where(s => s.CityCode == c.CityCode)
                //    .Select(s1 => s1.StateCode)
                //    .FirstOrDefault()
                //    ) ?? c.StateCode;
                //    c.StateName = (clsSTGCity
                //    .Where(s => (s.CityCode ?? s.CityName) == (c.CityCode ?? c.CityName) && s.Country_Id == c.Country_Id)
                //    //.Where(s => s.CityCode == c.CityCode)
                //    .Select(s1 => s1.StateName)
                //    .FirstOrDefault()
                //    ) ?? c.StateName;
                //    return c;
                //}).ToList();

                //List<DataContracts.STG.DC_STG_Mapping_Table_Ids> lstobj = new List<DataContracts.STG.DC_STG_Mapping_Table_Ids>();
                //lstobj.InsertRange(lstobj.Count, clsMappingCity.Where(a => a.stg_City_Id != null && a.ActionType == "UPDATE"
                //&& (a.stg_City_Id ?? Guid.Empty) != Guid.Empty).Select
                //   (g => new DataContracts.STG.DC_STG_Mapping_Table_Ids
                //   {
                //       STG_Mapping_Table_Id = Guid.NewGuid(),
                //       File_Id = obj.File_Id,
                //       STG_Id = g.stg_City_Id,
                //       Mapping_Id = g.CityMapping_Id,
                //       Batch = obj.CurrentBatch ?? 0
                //   }));
                PLog.PercentageValue = 37;
                USD.AddStaticDataUploadProcessLog(PLog);

                CallLogVerbose(File_Id, "MAP", "Checking for New Cities in File.", obj.CurrentBatch);
                clsSTGCityInsert = clsSTGCity.Where(p => !clsMappingCity.Any(p2 => (p2.SupplierName.ToString().Trim().ToUpper() == p.SupplierName.ToString().Trim().ToUpper())
                    && (
                        //(((p2.StateName ?? string.Empty).ToString().Trim().ToUpper() == (p.StateName ?? string.Empty).ToString().Trim().ToUpper()))

                        //&& (p2.Country_Id == p.Country_Id)
                        (p2.CityCode ?? p2.CityName) == (p.CityCode ?? p.CityName)
                        && p2.Country_Id == p.Country_Id
                    //&& (((p2.CityName ?? string.Empty).ToString().Trim().ToUpper() == (p.CityName ?? string.Empty).ToString().Trim().ToUpper()))
                    ))).ToList();
                PLog.PercentageValue = 48;
                USD.AddStaticDataUploadProcessLog(PLog);

                #region "Commented Code"
                //(p.CityCode != null && p.CityName != null && p2.CityName.ToString().Trim().ToUpper() == p.CityName.ToString().Trim().ToUpper() && p2.CityCode == p.CityCode) 
                //|| (p.CityCode == null && p.CityName != null && p2.CityName.ToString().Trim().ToUpper() == p.CityName.ToString().Trim().ToUpper())
                //|| (p.CityCode != null && p.CityName == null && p2.CityCode == p.CityCode)

                //&& (
                //    (p.CityCode != null && p.CountryCode == null && p2.CityCode == p.CityCode)
                //    || (p.CityCode != null && p.CountryCode != null && p2.CountryCode == p.CountryCode && p2.CityCode == p.CityCode)
                //    || (p.CityCode == null && p.CountryCode == null && p2.CityName.ToString().Trim().ToUpper() == p.CityName.ToString().Trim().ToUpper())
                //    || (p.CityCode == null && p.CountryCode != null && p2.CountryCode == p.CountryCode && p2.CityName.ToString().Trim().ToUpper() == p.CityName.ToString().Trim().ToUpper())

                //    || (p.CityCode != null && p.CountryCode == null && p.CountryName == null && p2.CityCode == p.CityCode)
                //    || (p.CityCode != null && p.CountryCode == null && p.CountryName != null && p2.CountryName == p.CountryName && p2.CityCode == p.CityCode)
                //    || (p.CityCode == null && p.CountryCode == null && p.CountryName == null && p2.CityName.ToString().Trim().ToUpper() == p.CityName.ToString().Trim().ToUpper())
                //    || (p.CityCode == null && p.CountryCode == null && p.CountryName != null && p2.CountryName.ToString().Trim().ToUpper() == p.CountryName.ToString().Trim().ToUpper() && p2.CityName.ToString().Trim().ToUpper() == p.CityName.ToString().Trim().ToUpper())

                //    || (p.CityCode != null && p.CountryCode != null && p.CountryName == null && p2.CountryCode == p.CountryCode && p2.CityCode == p.CityCode)
                //    || (p.CityCode != null && p.CountryCode != null && p.CountryName != null && p2.CountryCode == p.CountryCode && p2.CountryName == p.CountryName && p2.CityCode == p.CityCode)
                //    || (p.CityCode == null && p.CountryCode != null && p.CountryName == null && p2.CountryCode == p.CountryCode && p2.CityName.ToString().Trim().ToUpper() == p.CityName.ToString().Trim().ToUpper())
                //    || (p.CityCode == null && p.CountryCode != null && p.CountryName != null && p2.CountryCode == p.CountryCode && p2.CountryName.ToString().Trim().ToUpper() == p.CountryName.ToString().Trim().ToUpper() && p2.CityName.ToString().Trim().ToUpper() == p.CityName.ToString().Trim().ToUpper())

                //   ))).ToList();

                #endregion

                CallLogVerbose(File_Id, "MAP", "Removing UnEdited Data.", obj.CurrentBatch);
                clsSTGCity.RemoveAll(p => clsSTGCityInsert.Any(p2 => (p2.stg_City_Id == p.stg_City_Id)));



                //clsMappingCity = clsMappingCity.Where(a => a.oldCityName != a.CityName).ToList();
                //Commented to not remove any data on the basic of cityname --Ajay
                //clsMappingCity.RemoveAll(p => (p.CityName == p.oldCityName));
                PLog.PercentageValue = 53;
                USD.AddStaticDataUploadProcessLog(PLog);


                CallLogVerbose(File_Id, "MAP", "Inserting New Cities.", obj.CurrentBatch);
                clsMappingCity.InsertRange(clsMappingCity.Count, clsSTGCityInsert.Select
                    (g => new DC_CityMapping
                    {
                        CityMapping_Id = Guid.NewGuid(),
                        City_Id = null,
                        CityCode = g.CityCode,
                        CityName = g.CityName,
                        Country_Id = null,
                        Supplier_Id = CurSupplier_Id,
                        SupplierName = g.SupplierName,
                        CountryName = g.CountryName,
                        CountryCode = g.CountryCode,
                        Status = "UNMAPPED",
                        Create_Date = DateTime.Now,
                        Create_User = "TLGX_DataHandler",
                        Edit_Date = null,
                        Edit_User = null,
                        MapID = null,
                        Latitude = g.Latitude,
                        Longitude = g.Longitude,
                        oldCityName = g.CityName,
                        ActionType = "INSERT",
                        stg_City_Id = g.stg_City_Id,
                        Remarks = "", //DictionaryLookup(mappingPrefix, "Remarks", stgPrefix, "")
                        StateCode = g.StateCode,
                        StateName = g.StateName,
                        SupplierImporrtFile_Id = obj.File_Id ?? Guid.Empty,
                        Batch = obj.CurrentBatch ?? 0,
                        ReRunSupplierImporrtFile_Id = obj.File_Id ?? Guid.Empty,
                        ReRunBatch = obj.CurrentBatch ?? 0
                    }));

                // lstobj.InsertRange(lstobj.Count, clsMappingCity.Where(a => a.stg_City_Id != null && a.ActionType == "INSERT"
                // && (a.stg_City_Id ?? Guid.Empty) != Guid.Empty)
                //.Select
                //   (g => new DataContracts.STG.DC_STG_Mapping_Table_Ids
                //   {
                //       STG_Mapping_Table_Id = Guid.NewGuid(),
                //       File_Id = obj.File_Id,
                //       STG_Id = g.stg_City_Id,
                //       Mapping_Id = g.CityMapping_Id,
                //       Batch = obj.CurrentBatch ?? 0
                //   }));
                // bool idinsert = AddSTGMappingTableIDs(lstobj);
                PLog.PercentageValue = 58;
                USD.AddStaticDataUploadProcessLog(PLog);
                CallLogVerbose(File_Id, "MAP", "Updating / Inserting Database.", obj.CurrentBatch);
                if (clsMappingCity.Count > 0)
                {
                    ret = UpdateCityMappingMatch(clsMappingCity, File_Id);
                    /*if (obj.CurrentBatch == 1)
                    {
                        DataContracts.UploadStaticData.DC_SupplierImportFile_Statistics objStat = new DC_SupplierImportFile_Statistics();
                        objStat.SupplierImportFile_Statistics_Id = Guid.NewGuid();
                        objStat.SupplierImportFile_Id = obj.File_Id;
                        objStat.FinalStatus = file[0].STATUS;
                        objStat.TotalRows = clsMappingCity.Count;
                        objStat.Process_Date = DateTime.Now;
                        objStat.Process_User = file[0].PROCESS_USER;
                        DataContracts.DC_Message stat = USD.AddStaticDataUploadStatistics(objStat);
                    }*/
                }
                else
                    ret = true;
            }
            PLog.PercentageValue = 100;
            USD.AddStaticDataUploadProcessLog(PLog);
            CallLogVerbose(File_Id, "MAP", "MAP Process Complete for Batch.", obj.CurrentBatch);
            return ret;
        }

        private void UpdateExistCity(DataContracts.Masters.DC_Supplier obj, out List<DC_CityMapping> clsMappingCity)
        {
            try
            {
                List<DataContracts.STG.DC_stg_SupplierCityMapping> clsSTGCity = new List<DataContracts.STG.DC_stg_SupplierCityMapping>();
                DL_UploadStaticData staticdata = new DL_UploadStaticData();
                string CurSupplierName = obj.Name;
                Guid CurSupplier_Id = Guid.Parse(obj.Supplier_Id.ToString());
                Guid File_Id = new Guid();
                File_Id = Guid.Parse(obj.File_Id.ToString());
                DataContracts.STG.DC_stg_SupplierCityMapping_RQ RQ = new DataContracts.STG.DC_stg_SupplierCityMapping_RQ();
                RQ.SupplierName = CurSupplierName;
                RQ.PageNo = 0;
                RQ.PageSize = int.MaxValue;
                RQ.SupplierImportFile_Id = File_Id;
                clsSTGCity = staticdata.GetSTGCityData(RQ);

                List<DC_CityMapping> clsMappingCityLocal = new List<DC_CityMapping>();
                DC_CityMapping_RQ RQCity = new DC_CityMapping_RQ();
                if (CurSupplier_Id != Guid.Empty)
                    RQCity.Supplier_Id = CurSupplier_Id;
                RQCity.PageNo = 0;
                RQCity.PageSize = int.MaxValue;
                RQCity.CalledFromTLGX = "TLGX";
                //RQ.Status = "ALL";
                clsMappingCityLocal = GetCityMapping(RQCity);

                var result = (from x in clsMappingCityLocal
                              join y in clsSTGCity on new { CityCode = (x.CityCode ?? x.CityName), x.Country_Id }
                                                    equals new { CityCode = (y.CityCode ?? y.CityName), y.Country_Id }
                              select y);

                foreach (var item in result)
                {
                    foreach (var itemMapped in clsMappingCityLocal)
                    {
                        if (itemMapped.Country_Id == item.Country_Id && ((itemMapped.CityCode ?? itemMapped.CityName) == (item.CityCode ?? item.CityName)))
                        {
                            itemMapped.CityName = item.CityName ?? itemMapped.CityName;
                            itemMapped.StateName = item.StateName ?? itemMapped.StateName;
                            itemMapped.StateCode = item.StateCode ?? itemMapped.StateCode;
                            itemMapped.stg_City_Id = item.stg_City_Id;
                            itemMapped.ActionType = "UPDATE";
                            itemMapped.Edit_User = "TLGX_DataHandler";
                            itemMapped.Edit_Date = DateTime.Now;
                            itemMapped.Latitude = item.Latitude ?? itemMapped.Latitude;
                            itemMapped.Longitude = item.Longitude ?? itemMapped.Longitude;
                            break;
                        }
                    }
                }
                clsMappingCity = clsMappingCityLocal;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool UpdateCityMappingMatch(List<DataContracts.Mapping.DC_CityMapping> obj, Guid File_Id)
        {
            bool ret = false;
            if (obj.Count > 0)
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    foreach (var CM in obj)
                    {
                        if (CM.CityMapping_Id == null || CM.Supplier_Id == null)
                        {
                            continue;
                        }

                        try
                        {

                            var search = (from a in context.m_CityMapping
                                          where a.CityMapping_Id == CM.CityMapping_Id
                                          select a).FirstOrDefault();
                            if (search != null)
                            {
                                try
                                {
                                    //if (CM.Status != (search.Status ?? string.Empty))
                                    //{
                                    //    search.City_Id = CM.City_Id;
                                    //    search.Status = CM.Status;
                                    //    search.Edit_Date = CM.Edit_Date;
                                    //    search.Edit_User = CM.Edit_User;
                                    //}
                                    //else
                                    //{
                                    search.City_Id = CM.City_Id;
                                    search.Country_Id = CM.Country_Id;
                                    search.Supplier_Id = CM.Supplier_Id;
                                    search.Status = CM.Status;
                                    search.Edit_Date = CM.Edit_Date;
                                    search.Edit_User = CM.Edit_User;
                                    search.Remarks = CM.Remarks;
                                    if (search.StateCode == null)
                                        search.StateCode = CM.StateCode;
                                    if (search.StateName == null)
                                        search.StateName = CM.StateName;
                                    if (!string.IsNullOrEmpty(CM.Latitude))
                                        search.Latitude = CM.Latitude;
                                    if (!string.IsNullOrEmpty(CM.Latitude))
                                        search.Longitude = CM.Longitude;
                                    if (!string.IsNullOrEmpty(CM.Latitude))
                                        search.Latitude = CM.Latitude;
                                    //}
                                    context.SaveChanges();
                                }
                                catch (Exception e)
                                {
                                    DC_SupplierImportFile_ErrorLog objE = new DC_SupplierImportFile_ErrorLog();
                                    objE.SupplierImportFile_ErrorLog_Id = Guid.NewGuid();
                                    objE.SupplierImportFile_Id = File_Id;
                                    objE.ErrorCode = 0;
                                    objE.ErrorDescription = e.Message.ToString() + ", " + e.StackTrace;
                                    objE.ErrorMessage_UI = "Error while updating city data for " + (CM.CountryName ?? (CM.CountryCode ?? "")) + " - " + (CM.CityName ?? (CM.CityCode ?? "")).ToString() + " - " + CM.City_Id.ToString();
                                    objE.Error_DATE = DateTime.Now;
                                    objE.Error_USER = "TLGX_DataHandler";
                                    DL_UploadStaticData ups = new DL_UploadStaticData();
                                    DataContracts.DC_Message dc = new DataContracts.DC_Message();
                                    dc = ups.AddStaticDataUploadErrorLog(objE);
                                }
                                //context.SaveChanges();
                            }
                            else
                            {
                                try
                                {
                                    DataLayer.m_CityMapping objNew = new m_CityMapping();
                                    objNew.CityMapping_Id = CM.CityMapping_Id;
                                    objNew.City_Id = CM.City_Id;
                                    objNew.CityName = CM.CityName;
                                    objNew.CityCode = CM.CityCode;
                                    objNew.Supplier_Id = CM.Supplier_Id;
                                    objNew.SupplierName = CM.SupplierName;
                                    objNew.CountryName = CM.CountryName;
                                    objNew.CountryCode = CM.CountryCode;
                                    objNew.Status = CM.Status;
                                    objNew.Create_Date = CM.Create_Date;
                                    objNew.Create_User = CM.Create_User;
                                    objNew.Edit_Date = CM.Edit_Date;
                                    objNew.Edit_User = CM.Edit_User;
                                    objNew.MapID = CM.MapID;
                                    objNew.Remarks = CM.Remarks;
                                    objNew.Latitude = CM.Latitude;
                                    objNew.Longitude = CM.Longitude;
                                    objNew.StateCode = CM.StateCode;
                                    objNew.StateName = CM.StateName;
                                    objNew.stg_City_Id = CM.stg_City_Id;
                                    objNew.SupplierImportFile_Id = CM.SupplierImporrtFile_Id;
                                    objNew.Batch = CM.Batch;
                                    objNew.ReRun_SupplierImportFile_Id = CM.SupplierImporrtFile_Id;
                                    objNew.ReRun_Batch = CM.Batch;
                                    // objNew.Country_Id = CM.Country_Id;
                                    objNew.Country_Id = ((from a in context.m_CountryMapping.AsNoTracking()
                                                          where a.Supplier_Id == CM.Supplier_Id &&
                                                          ((a.CountryName.Trim().ToUpper() == CM.CountryName.Trim().ToUpper()) && a.CountryName != null && CM.CountryName != null)
                                                          //&& ((CM.CountryName != null && a.CountryName == CM.CountryName) || CM.CountryName == null)
                                                          //&& ((CM.CountryCode != null && a.CountryCode == CM.CountryCode) || CM.CountryCode == null)
                                                          //&& a.Supplier_Id == CM.Supplier_Id
                                                          select a.Country_Id).FirstOrDefault()) ?? ((from a in context.m_CountryMapping.AsNoTracking()
                                                                                                      where a.Supplier_Id == CM.Supplier_Id &&
                                                                                                      ((a.CountryCode == CM.CountryCode) && a.CountryCode != null && CM.CountryCode != null)
                                                                                                      select a.Country_Id).FirstOrDefault());
                                    context.m_CityMapping.Add(objNew);
                                }
                                catch (Exception e)
                                {
                                    DC_SupplierImportFile_ErrorLog objE = new DC_SupplierImportFile_ErrorLog();
                                    objE.SupplierImportFile_ErrorLog_Id = Guid.NewGuid();
                                    objE.SupplierImportFile_Id = File_Id;
                                    objE.ErrorCode = 0;
                                    objE.ErrorDescription = e.Message.ToString() + ", " + e.StackTrace;
                                    objE.ErrorMessage_UI = "Error while inserting city data for " + (CM.CountryName ?? (CM.CountryCode ?? "")) + " - " + (CM.CityName ?? (CM.CityCode ?? "")).ToString();
                                    objE.Error_DATE = DateTime.Now;
                                    objE.Error_USER = "TLGX_DataHandler";
                                    DL_UploadStaticData ups = new DL_UploadStaticData();
                                    DataContracts.DC_Message dc = new DataContracts.DC_Message();
                                    dc = ups.AddStaticDataUploadErrorLog(objE);
                                }
                            }

                            ret = true;


                        }
                        catch
                        {
                            ret = false;
                            throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating city mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
                        }
                    }
                    if (ret)
                    {
                        context.SaveChanges();
                        context.USP_UpdateMapID("city");
                    }
                }
            }
            else
                ret = true;
            return ret;
        }

        //public List<DC_CityMapping> UpdateCityMappingStatus(DataContracts.Mapping.DC_MappingMatch obj)
        public bool UpdateCityMappingStatus(DataContracts.Mapping.DC_MappingMatch obj)
        {
            bool retrn = false;
            DataContracts.Masters.DC_Supplier supdata = new DataContracts.Masters.DC_Supplier();
            supdata = obj.SupplierDetail;
            //List<DC_CityMapping> CMS = obj.lstCityMapping;
            List<DC_SupplierImportAttributeValues> configs = obj.lstConfigs;
            string configWhere = "";
            string curSupplier = "";
            Guid? curSupplier_Id = Guid.Empty;
            configWhere = "";
            Guid File_Id = new Guid();
            File_Id = Guid.Parse(obj.File_Id.ToString());



            int totPriorities = obj.TotalPriorities;
            int curPriority = obj.CurrentPriority;
            int totConfigs = 0;
            int curConfig = 0;

            if (totPriorities <= 0)
                totPriorities = 1;
            int PerForEachPriority = 60 / totPriorities;

            if (supdata != null)
            {
                curSupplier = supdata.Name;
                curSupplier_Id = supdata.Supplier_Id;
            }
            List<DC_CityMapping> ret = new List<DC_CityMapping>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var prodMapSearch = (from a in context.m_CityMapping
                                             //join s in context.STG_Mapping_TableIds.AsNoTracking() on a.CityMapping_Id equals s.Mapping_Id
                                         where a.ReRun_SupplierImportFile_Id == obj.File_Id && a.City_Id == null && a.Supplier_Id == curSupplier_Id
                                         && a.Status == "UNMAPPED" && a.ReRun_Batch == obj.CurrentBatch
                                         select a);

                    bool isCountryCodeCheck = false;
                    bool isCountryNameCheck = false;
                    bool isCodeCheck = false;
                    bool isNameCheck = false;
                    bool isStateNameCheck = false;
                    bool isLatLongCheck = false;
                    totConfigs = configs.Count;
                    curConfig = 0;

                    PLog.SupplierImportFileProgress_Id = Guid.NewGuid();
                    PLog.SupplierImportFile_Id = obj.File_Id;
                    PLog.Step = "MATCH";
                    PLog.Status = "MATCHING";
                    PLog.CurrentBatch = curPriority;
                    PLog.TotalBatch = totPriorities;
                    CallLogVerbose(File_Id, "MATCH", "Applying Matching Combination " + curPriority.ToString() + " out of Total " + totPriorities + " Combinations.", obj.CurrentBatch);

                    foreach (DC_SupplierImportAttributeValues config in configs)
                    {
                        configWhere = configWhere + " " + config.AttributeValue.Replace("m_CityMaster.", "").Trim() + ",";
                    }
                    configWhere = configWhere.Remove(configWhere.Length - 1);
                    CallLogVerbose(File_Id, "MATCH", "Matching Combination " + curPriority.ToString() + " consist of Match by " + configWhere, obj.CurrentBatch);

                    foreach (DC_SupplierImportAttributeValues config in configs)
                    {
                        curConfig = curConfig + 1;
                        string CurrConfig = "";
                        CurrConfig = config.AttributeValue.Replace("m_CityMaster.", "").Trim().ToUpper();
                        CallLogVerbose(File_Id, "MATCH", "Applying Check for " + CurrConfig, obj.CurrentBatch);

                        if (CurrConfig == "COUNTRYCODE")
                        {
                            isCountryCodeCheck = true;
                            prodMapSearch = (from a in prodMapSearch
                                             join m in context.m_CountryMapping on a.Supplier_Id equals m.Supplier_Id
                                             where a.CountryCode != null && m.CountryCode != null
                                             && a.CountryCode.Trim().ToUpper() == m.CountryCode.Trim().ToUpper()
                                             select a);
                            //join cm in context.m_CountryMapping on new { a.Supplier_Id, a.CountryCode } equals new { cm.Supplier_Id, cm.CountryCode }
                            //join m in context.m_CountryMaster on cm.Country_Id equals m.Country_Id
                            //select a);
                        }
                        if (CurrConfig == "COUNTRYNAME")
                        {
                            isCountryNameCheck = true;
                            prodMapSearch = (from a in prodMapSearch
                                             join m in context.m_CountryMapping on a.Supplier_Id equals m.Supplier_Id
                                             where a.CountryName != null && m.CountryName != null
                                             && a.CountryName.Trim().ToUpper() == m.CountryName.Trim().ToUpper()
                                             select a);
                            //join cm in context.m_CountryMapping on new { a.Supplier_Id, a.CountryName } equals new { cm.Supplier_Id, cm.CountryName }
                            //join m in context.m_CountryMaster on cm.Country_Id equals m.Country_Id
                            //select a);
                        }
                        if (CurrConfig == "CODE")
                        {
                            isCodeCheck = true;
                            prodMapSearch = (from a in prodMapSearch
                                             join mc in context.m_CountryMaster on a.Country_Id equals mc.Country_Id
                                             join m in context.m_CityMaster on mc.Country_Id equals m.Country_Id
                                             //join mc in context.m_CountryMaster on m.Country_Id equals mc.Country_Id
                                             //join cm in context.m_CountryMapping on new { a.Country_Id, a.Supplier_Id } equals new { cm.Country_Id, cm.Supplier_Id }
                                             where a.CityCode != null && m.Code != null
                                              && a.CityCode == m.Code
                                             select a);
                        }
                        if (CurrConfig == "NAME")
                        {
                            isNameCheck = true;
                            prodMapSearch = (from a in prodMapSearch
                                             join mc in context.m_CountryMaster on a.Country_Id equals mc.Country_Id
                                             //join m in context.m_CityMaster on mc.Country_Id equals m.Country_Id // a.CityName equals m.Name
                                             where a.CityName != null //&& m.Name != null
                                                                      //&& a.CityName == m.Name
                                             select a);
                        }
                        if (CurrConfig == "STATENAME")
                        {
                            isStateNameCheck = true;
                            prodMapSearch = (from a in prodMapSearch
                                             join mc in context.m_CountryMaster on a.Country_Id equals mc.Country_Id
                                             //join m in context.m_CityMaster on mc.Country_Id equals m.Country_Id // a.CityName equals m.Name
                                             where a.CityName != null //&& m.Name != null
                                                                      //&& a.CityName == m.Name
                                             select a);
                        }
                        if (CurrConfig == "LATITUDE")
                        {
                            isLatLongCheck = true;
                            prodMapSearch = (from a in prodMapSearch
                                             join m in context.m_CityMaster on new { a.Latitude, a.Longitude } equals new { m.Latitude, m.Longitude }
                                             join mc in context.m_CountryMaster on a.Country_Id equals mc.Country_Id
                                             where mc.Country_Id == m.Country_Id
                                             && m.Latitude != null && a.Latitude != null && m.Longitude != null && a.Longitude != null
                                             select a);
                        }
                        //PLog.PercentageValue = (70 / totPriorities) / totConfigs;
                        PLog.PercentageValue = (PerForEachPriority * (curPriority - 1)) + ((PerForEachPriority / totConfigs) * curConfig);
                        USD.AddStaticDataUploadProcessLog(PLog);

                    }

                    List<DC_CityMapping> res = new List<DC_CityMapping>();

                    if (isCountryCodeCheck || isCountryNameCheck || isCodeCheck || isNameCheck || isLatLongCheck || isStateNameCheck)
                    {
                        res = (from a in prodMapSearch
                               join ms in context.m_CountryMaster on a.Country_Id equals ms.Country_Id
                               select new DataContracts.Mapping.DC_CityMapping
                               {
                                   CityMapping_Id = a.CityMapping_Id,
                                   CityCode = a.CityCode,
                                   CityName = a.CityName,
                                   //CityName = (a.CityName.Replace(a.CountryName.Trim(),"") == "") ? a.CityName : a.CityName.Replace(a.CountryName.Trim(), ""),
                                   City_Id = a.City_Id,
                                   StateCode = a.StateCode,
                                   StateName = a.StateName,
                                   Country_Id = a.Country_Id,
                                   Supplier_Id = a.Supplier_Id,
                                   CountryCode = ms.Code,
                                   CountryName = ms.Name,
                                   Create_Date = a.Create_Date,
                                   Create_User = a.Create_User,
                                   Edit_Date = a.Edit_Date,
                                   Edit_User = a.Edit_User,
                                   MapID = a.MapID,
                                   Status = a.Status,
                                   SupplierName = a.SupplierName,
                                   Latitude = a.Latitude,
                                   Longitude = a.Longitude
                               }).ToList();

                        if (totPriorities == curPriority)
                        {
                            PLog.PercentageValue = 65;
                            USD.AddStaticDataUploadProcessLog(PLog);
                        }

                        CallLogVerbose(File_Id, "MATCH", "Looking for Match in Master Data for Matching Combination " + curPriority.ToString() + ".", obj.CurrentBatch);
                        res = res.Select(c =>
                        {
                            c.City_Id = (context.m_CityMaster
                                            .Where(s => (
                                                            ((isCountryCodeCheck && s.Country_Id == c.Country_Id) || (!isCountryCodeCheck)) &&
                                                            ((isCountryNameCheck && s.Country_Id == c.Country_Id) || (!isCountryNameCheck)) &&
                                                            ((isCodeCheck && s.Code == c.CityCode) || (!isCodeCheck)) &&
                                                            ((isNameCheck && s.Name.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("[", "").Replace("]", "").Replace(" ", "").Replace("-", "") == ((c.CityName.Replace(c.CountryName.Trim(), "") == "") ? c.CityName : c.CityName.Replace(c.CountryName.Trim(), "")).Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("[", "").Replace("]", "").Replace(" ", "").Replace("-", "")) || (!isNameCheck)) &&
                                                            ((isStateNameCheck && s.StateName.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("[", "").Replace("]", "").Replace(" ", "").Replace("-", "") == c.StateName.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("[", "").Replace("]", "").Replace(" ", "").Replace("-", "")) || (!isStateNameCheck)) &&
                                                            ((isLatLongCheck && s.Latitude == c.Latitude && s.Longitude == c.Longitude) || (!isLatLongCheck))
                                                        )
                                                   )
                                            .Select(s1 => s1.City_Id)
                                            .FirstOrDefault()
                                            );
                            return c;
                        }).ToList();

                        if (totPriorities == curPriority)
                        {
                            PLog.PercentageValue = 70;
                            USD.AddStaticDataUploadProcessLog(PLog);
                        }

                        CallLogVerbose(File_Id, "MATCH", "Setting Appropriate Status.", obj.CurrentBatch);
                        res.RemoveAll(p => p.City_Id == Guid.Empty);
                        res = res.Select(c =>
                        {
                            c.MatchedBy = curPriority - 1;
                            c.Status = ("REVIEW"); return c;
                        }).ToList();


                        CallLogVerbose(File_Id, "MATCH", res.Count.ToString() + " Matches Found for Combination " + curPriority.ToString() + ".", obj.CurrentBatch);
                        CallLogVerbose(File_Id, "MATCH", "Updating into Database.", obj.CurrentBatch);
                        if (UpdateCityMapping(res))
                        {
                            if ((obj.FileMode ?? "ALL") == "ALL" && totPriorities == curPriority)
                            {
                                PLog.PercentageValue = 75;
                                USD.AddStaticDataUploadProcessLog(PLog);

                                DataContracts.UploadStaticData.DC_SupplierImportFile_Statistics objStat = new DC_SupplierImportFile_Statistics();
                                objStat.SupplierImportFile_Statistics_Id = Guid.NewGuid();
                                objStat.SupplierImportFile_Id = obj.File_Id;
                                objStat.From = "MATCHING";
                                DataContracts.DC_Message stat = USD.AddStaticDataUploadStatistics(objStat);

                            }
                            retrn = true;
                            CallLogVerbose(File_Id, "MATCH", "Update Done.", obj.CurrentBatch);

                        }
                        if (totPriorities == curPriority)
                        {
                            PLog.PercentageValue = 100;
                            USD.AddStaticDataUploadProcessLog(PLog);
                        }
                    }
                    else
                    {
                        retrn = true;

                    }
                }
                return retrn;
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating city mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool UpdateCityMapping(List<DataContracts.Mapping.DC_CityMapping> obj)
        {
            bool ret = false;
            if (obj.Count > 0)
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var CountryMapping = context.m_CountryMapping.Where(w => w.CountryName != null || w.CountryCode != null).AsQueryable();
                    var CityMapping = context.m_CityMapping.Select(s => s).AsQueryable();
                    foreach (var CM in obj)
                    {
                        if (CM.CityMapping_Id == null || CM.Supplier_Id == null)
                        {
                            continue;
                        }

                        try
                        {

                            var search = (from a in CityMapping
                                          where a.CityMapping_Id == CM.CityMapping_Id
                                          select a).FirstOrDefault();
                            if (search != null)
                            {
                                //if (CM.Status != (search.Status ?? string.Empty))
                                //{
                                //    search.City_Id = CM.City_Id;
                                //    search.Status = CM.Status;
                                //    search.Edit_Date = CM.Edit_Date;
                                //    search.Edit_User = CM.Edit_User;
                                //}
                                //else
                                //{
                                search.City_Id = CM.City_Id;
                                search.Country_Id = CM.Country_Id;
                                search.Supplier_Id = CM.Supplier_Id;

                                if (CM.MatchedBy != null)
                                    search.MatchedBy = CM.MatchedBy;
                                search.Status = CM.Status;
                                search.Edit_Date = CM.Edit_Date;
                                search.Edit_User = CM.Edit_User;
                                search.Remarks = CM.Remarks;
                                if (search.StateCode == null)
                                    search.StateCode = CM.StateCode;
                                if (search.StateName == null)
                                    search.StateName = CM.StateName;
                                //}
                                context.SaveChanges();

                                //context.SaveChanges();
                            }
                            else
                            {

                                DataLayer.m_CityMapping objNew = new m_CityMapping();
                                objNew.CityMapping_Id = CM.CityMapping_Id;
                                objNew.City_Id = CM.City_Id;
                                objNew.CityName = CM.CityName;
                                objNew.CityCode = CM.CityCode;
                                objNew.Supplier_Id = CM.Supplier_Id;
                                objNew.SupplierName = CM.SupplierName;
                                objNew.CountryName = CM.CountryName;
                                objNew.CountryCode = CM.CountryCode;
                                objNew.Status = CM.Status;
                                objNew.Create_Date = CM.Create_Date;
                                objNew.Create_User = CM.Create_User;
                                objNew.Edit_Date = CM.Edit_Date;
                                objNew.Edit_User = CM.Edit_User;
                                objNew.MapID = CM.MapID;
                                objNew.Remarks = CM.Remarks;
                                objNew.Latitude = CM.Latitude;
                                objNew.Longitude = CM.Longitude;
                                objNew.StateCode = CM.StateCode;
                                objNew.StateName = CM.StateName;
                                objNew.SupplierImportFile_Id = CM.SupplierImporrtFile_Id;
                                objNew.Batch = CM.Batch;
                                objNew.ReRun_SupplierImportFile_Id = CM.SupplierImporrtFile_Id;
                                objNew.ReRun_Batch = CM.Batch;

                                // objNew.Country_Id = CM.Country_Id;
                                objNew.Country_Id = ((from a in CountryMapping
                                                      where a.Supplier_Id == CM.Supplier_Id && a.CountryName == CM.CountryName
                                                      //((a.CountryName == CM.CountryName) && a.CountryName != null && CM.CountryName != null)
                                                      //&& ((CM.CountryName != null && a.CountryName == CM.CountryName) || CM.CountryName == null)
                                                      //&& ((CM.CountryCode != null && a.CountryCode == CM.CountryCode) || CM.CountryCode == null)
                                                      //&& a.Supplier_Id == CM.Supplier_Id
                                                      select a.Country_Id).FirstOrDefault()) ?? ((from a in CountryMapping
                                                                                                  where a.Supplier_Id == CM.Supplier_Id && a.CountryCode == CM.CountryCode
                                                                                                  select a.Country_Id).FirstOrDefault());
                                context.m_CityMapping.Add(objNew);
                            }

                            ret = true;


                        }
                        catch
                        {
                            ret = false;
                            throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating city mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
                        }
                    }
                    if (ret)
                    {
                        context.SaveChanges();
                        context.USP_UpdateMapID("city");
                    }
                }
            }
            else
                ret = true;
            return ret;
        }
        #endregion

        #region Mapping Stats
        public List<DataContracts.Mapping.DC_MappingStats> GetMappingStatistics(Guid SupplierID, int Priority, string ProductCategory, bool isMDM)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    context.Database.CommandTimeout = 0;

                    DataContracts.Mapping.DC_MappingStats newmapstats = new DataContracts.Mapping.DC_MappingStats();

                    List<DataContracts.Mapping.DC_MappingStats> returnObj = new List<DataContracts.Mapping.DC_MappingStats>();
                    List<DataContracts.Mapping.DC_MappingStatsFor> newmapstatsforList = new List<DataContracts.Mapping.DC_MappingStatsFor>();

                    var MappingStats = context.Dashboard_MappingStat.ToList();
                    var MapForList = (from s in MappingStats select s.MappingFor).ToList().Distinct();

                    if (SupplierID != Guid.Empty)
                    {
                        //SupplierName List
                        MappingStats = MappingStats.Where(w => w.Supplier_Id == SupplierID).ToList();
                        newmapstats.SupplierNames = MappingStats.Where(w => w.Supplier_Id == SupplierID).Select(s => s.SupplierName).Distinct().ToList();
                        newmapstats.SupplierId = SupplierID;
                        newmapstats.SupplierName = MappingStats.Where(w => w.Supplier_Id == SupplierID).Select(s => s.SupplierName).FirstOrDefault();
                        newmapstats.NextRun = Convert.ToString((from sn in context.Schedule_NextOccurance
                                                                where sn.Schedule_ID == SupplierID
                                                                select sn.Execution_StartDate).FirstOrDefault());
                    }
                    else if (SupplierID == Guid.Empty)
                    {
                        newmapstats.SupplierName = "ALL";
                        newmapstats.SupplierId = SupplierID;
                        var suppliermaster = (from m in context.Supplier
                                              join sp in context.Supplier_ProductCategory on m.Supplier_Id equals sp.Supplier_Id into l
                                              from p in l.DefaultIfEmpty()
                                              where (Priority > 0 ? m.Priority == Priority : m.Priority == m.Priority)
                                                      && (ProductCategory != "0" ? p.ProductCategory == ProductCategory : p.ProductCategory == p.ProductCategory)
                                                      && m.StatusCode.ToUpper().Trim() == "ACTIVE"
                                              select new
                                              {
                                                  SupplierName = m.Name,
                                                  m.Supplier_Id
                                              }).Distinct().ToList();

                        MappingStats = (from p in MappingStats
                                        join q in suppliermaster on p.Supplier_Id equals q.Supplier_Id
                                        select p).ToList();

                        if (Priority == 0 && ProductCategory == "0")
                            newmapstats.SupplierNames = new List<string> { "ALL" };

                        else
                            newmapstats.SupplierNames = MappingStats.Select(s => s.SupplierName).Distinct().ToList();

                        newmapstats.MappingStatsForSuppliers = (from m in MappingStats
                                                                where (m.Status == "UNMAPPED" || m.Status == "REVIEW")
                                                                orderby (m.SupplierName)
                                                                group m by new { m.SupplierName, m.Supplier_Id, m.MappingFor } into g

                                                                select new DC_MappingStatsForSuppliers
                                                                {
                                                                    SupplierName = g.Key.SupplierName,
                                                                    //SupplierId = group.Key.supplier_id,
                                                                    Mappingfor = g.Key.MappingFor,
                                                                    totalcount = g.Sum(x => x.TotalCount) ?? 0
                                                                }).ToList();

                    }

                    foreach (var mapfor in MapForList)
                    {
                        DataContracts.Mapping.DC_MappingStatsFor newmapstatsfor = new DataContracts.Mapping.DC_MappingStatsFor();

                        newmapstatsfor.MappingFor = mapfor;

                        int AllCount = MappingStats.Where(w => w.MappingFor == mapfor).Sum(s => s.TotalCount) ?? 0;
                        int MapCount = MappingStats.Where(w => w.MappingFor == mapfor && w.Status == "MAPPED").Sum(s => s.TotalCount) ?? 0;
                        int AutoMapCount = MappingStats.Where(w => w.MappingFor == mapfor && w.Status == "AUTOMAPPED").Sum(s => s.TotalCount) ?? 0;
                        int MappedCount = MapCount + AutoMapCount;

                        if (AllCount == 0)
                        {
                            newmapstatsfor.MappedPercentage = Convert.ToDecimal(0);
                        }
                        else
                        {
                            newmapstatsfor.MappedPercentage = Math.Round((Convert.ToDecimal(MappedCount) / Convert.ToDecimal(AllCount) * Convert.ToDecimal(100)), 2);
                        }

                        //suppliercount
                        int supCount = MappingStats.Where(w => w.MappingFor == mapfor).Select(s => s.Supplier_Id).Distinct().Count();
                        //end SupCount

                        newmapstatsfor.MappingData = (from s in MappingStats
                                                      where s.MappingFor == mapfor
                                                      group s by new { s.Status } into sg
                                                      orderby sg.Key.Status
                                                      select new DataContracts.Mapping.DC_MappingData
                                                      {
                                                          Status = sg.Key.Status,
                                                          TotalCount = sg.Sum(x => x.TotalCount) ?? 0,
                                                          SuppliersCount = supCount,
                                                      }).ToList();


                        newmapstatsfor.MappingData.Add(new DataContracts.Mapping.DC_MappingData
                        {
                            Status = "ALL",
                            TotalCount = (MappingStats.Where(w => w.MappingFor == mapfor).Sum(s => s.TotalCount) ?? 0),
                            SuppliersCount = supCount,
                        });

                        newmapstatsforList.Add(newmapstatsfor);
                    }
                    newmapstats.MappingStatsFor = newmapstatsforList;

                    returnObj.Add(newmapstats);

                    return returnObj;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching mapping statistics", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        #endregion

        #region Export Supplier Data
        public List<DataContracts.Mapping.DC_SupplierExportDataReport> GetSupplierDataForExport(int? AccoPriority, Guid? Supplier_id, bool IsMdmDataOnly, int? SuppPriority)
        {
            try
            {
                List<DC_SupplierExportDataReport> ReturnResult = new List<DC_SupplierExportDataReport>();

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    context.Database.CommandTimeout = 0;

                    //List<Dashboard_MappingStat> MappingData = new List<Dashboard_MappingStat>();
                    List<USP_MappingStatus_Result> MappingData = new List<USP_MappingStatus_Result>();

                    if (SuppPriority == 0)
                    {
                        SuppPriority = null;
                    }

                    //var temp = context.USP_MappingStatus();
                    var suppliermaster = context.Supplier.Where(w => w.StatusCode == "ACTIVE" && w.Priority == (SuppPriority == null ? w.Priority : SuppPriority) && w.Supplier_Id == (Supplier_id == Guid.Empty ? w.Supplier_Id : Supplier_id)).Select(s => new
                    {
                        s.Supplier_Id,
                        s.Name,
                        s.Priority
                    }).OrderBy(o => o.Name).ToList();

                    if (AccoPriority == 0)
                    {
                        AccoPriority = null;
                    }
                    if (Supplier_id == Guid.Empty)
                    {
                        Supplier_id = null;
                    }

                    MappingData = context.USP_MappingStatus(AccoPriority, IsMdmDataOnly, Supplier_id).ToList();



                    //if (Supplier_id != Guid.Empty)
                    //{
                    //    if (IsMdmDataOnly)
                    //    {
                    //        MappingDataIsMdm = context.vwMappingStatsMdmOnly.Where(x => x.Supplier_Id == Supplier_id).ToList();
                    //        MappingData = MappingDataIsMdm.Select(s => new Dashboard_MappingStat
                    //        {
                    //            MappingFor = s.MappinFor,
                    //            RowId = s.RowId,
                    //            Status = s.Status,
                    //            SupplierName = s.SupplierName,
                    //            Supplier_Id = s.Supplier_Id,
                    //            TotalCount = s.totalcount
                    //        }).ToList();
                    //    }
                    //    else
                    //    {
                    //        MappingData = context.Dashboard_MappingStat.Where(x => x.Supplier_Id == Supplier_id).ToList();
                    //    }
                    //}
                    //else
                    //{
                    //    if (IsMdmDataOnly)
                    //    {
                    //        MappingDataIsMdm = context.vwMappingStatsMdmOnly.ToList();
                    //        MappingData = MappingDataIsMdm.Select(s => new Dashboard_MappingStat
                    //        {
                    //            MappingFor = s.MappinFor,
                    //            RowId = s.RowId,
                    //            Status = s.Status,
                    //            SupplierName = s.SupplierName,
                    //            Supplier_Id = s.Supplier_Id,
                    //            TotalCount = s.totalcount
                    //        }).ToList();
                    //    }
                    //    else
                    //    {
                    //        MappingData = context.Dashboard_MappingStat.ToList();
                    //    }
                    //}

                    List<Guid> RoomSuppliers = context.Accommodation_SupplierRoomTypeMapping.AsNoTracking().Select(s => s.Supplier_Id ?? Guid.Empty).Distinct().ToList();

                    var probableRoomType = (from p in context.Accommodation_SupplierRoomTypeMapping
                                            where (p.MappingStatus == "ADD")
                                            group p by new { p.Supplier_Id } into g
                                            select new { Supplier_id = g.Key.Supplier_Id, Totalcount = g.Count() }
                                            ).ToList();

                    var roomsFromSupplier = (from ac in context.Accommodation_SupplierRoomTypeMapping.AsNoTracking()
                                             group ac by new { ac.Supplier_Id } into g
                                             select new { g.Key.Supplier_Id, Totalcount = g.Count() }).ToList();


                    IQueryable<Accommodation_RoomInfo> AccoRoomsEligible = context.Accommodation_RoomInfo.AsNoTracking().AsQueryable();

                    if (AccoPriority != null)
                    {
                        AccoRoomsEligible = from ar in AccoRoomsEligible
                                            join ap in context.Accommodation on ar.Accommodation_Id equals ap.Accommodation_Id
                                            where ap.Priority == AccoPriority
                                            select ar;
                    }

                    if (IsMdmDataOnly)
                    {
                        AccoRoomsEligible = from ar in AccoRoomsEligible
                                            join ap in context.Accommodation on ar.Accommodation_Id equals ap.Accommodation_Id
                                            where ap.TLGXAccoId != null
                                            select ar;
                    }

                    var eligibleRooms = (from asrtm in context.Accommodation_SupplierRoomTypeMapping.AsNoTracking()
                                         join a in (AccoRoomsEligible.Select(s => s.Accommodation_Id).Distinct())
                                         on asrtm.Accommodation_Id equals a.Value
                                         group asrtm by new { asrtm.Supplier_Id } into g
                                         select new { g.Key.Supplier_Id, TotalEligibleRooms = g.Count() }).ToList();

                    //var lastFetchedDate = (from a in context.Accommodation_SupplierRoomTypeMapping.AsNoTracking()
                    //                       where a.Create_User == "TLGX"
                    //                       group a by new { a.Supplier_Id, a.Create_Date} into g
                    //                       select new { g.Key.Supplier_Id, CreatedDate = g.Max(x => x.Create_Date) }).ToList();

                    foreach (var supplier in suppliermaster)
                    {
                        var supplierResult = new DC_SupplierExportDataReport();

                        supplierResult.Priority = Convert.ToString(supplier.Priority == null ? "" : "P" + supplier.Priority);
                        supplierResult.Supplier_Id = supplier.Supplier_Id;
                        supplierResult.SupplierName = supplier.Name;

                        #region Country Mapping Data
                        //supplierResult.LastFetchedDate = lastFetchedDate.Where(x => x.Supplier_Id == supplier.Supplier_Id).Max(x => x.CreatedDate);
                        supplierResult.Country_TotalRecordReceived = string.Empty;
                        supplierResult.Country_AutoMapped = MappingData.Where(x => x.MappingFor == "Country" && x.Status == "AUTOMAPPED" && x.Supplier_Id == supplier.Supplier_Id).Sum(x => x.TotalCount) ?? 0;
                        supplierResult.Country_MannualMapped = MappingData.Where(x => x.MappingFor == "Country" && x.Status == "MAPPED" && x.Supplier_Id == supplier.Supplier_Id).Sum(x => x.TotalCount) ?? 0;
                        supplierResult.Country_ReviewMapped = MappingData.Where(x => x.MappingFor == "Country" && x.Status == "REVIEW" && x.Supplier_Id == supplier.Supplier_Id).Sum(x => x.TotalCount) ?? 0;
                        supplierResult.Country_Unmapped = MappingData.Where(x => x.MappingFor == "Country" && x.Status == "UNMAPPED" && x.Supplier_Id == supplier.Supplier_Id).Sum(x => x.TotalCount) ?? 0;
                        supplierResult.CountryTotal = supplierResult.Country_AutoMapped + supplierResult.Country_MannualMapped + supplierResult.Country_ReviewMapped + supplierResult.Country_Unmapped;//MappingData.Where(w => w.MappingFor == "Country" && w.Supplier_Id == supplier.Supplier_Id).Sum(s => s.TotalCount) ?? 0;

                        if (supplierResult.CountryTotal > 0)
                        {
                            supplierResult.Country_CompletePercentage = Math.Round((Convert.ToDecimal(supplierResult.Country_AutoMapped + supplierResult.Country_MannualMapped) / Convert.ToDecimal(supplierResult.CountryTotal) * Convert.ToDecimal(100)), 2);
                        }
                        else
                        {
                            supplierResult.Country_CompletePercentage = 0;
                        }
                        #endregion

                        #region City Mapping Data
                        supplierResult.City_TotalRecordReceived = string.Empty;
                        supplierResult.City_AutoMapped = MappingData.Where(x => x.MappingFor == "City" && x.Status == "AUTOMAPPED" && x.Supplier_Id == supplier.Supplier_Id).Sum(x => x.TotalCount) ?? 0;
                        supplierResult.City_MannualMapped = MappingData.Where(x => x.MappingFor == "City" && x.Status == "MAPPED" && x.Supplier_Id == supplier.Supplier_Id).Sum(x => x.TotalCount) ?? 0;
                        supplierResult.City_ReviewMapped = MappingData.Where(x => x.MappingFor == "City" && x.Status == "REVIEW" && x.Supplier_Id == supplier.Supplier_Id).Sum(x => x.TotalCount) ?? 0;
                        supplierResult.City_Unmapped = MappingData.Where(x => x.MappingFor == "City" && x.Status == "UNMAPPED" && x.Supplier_Id == supplier.Supplier_Id).Sum(x => x.TotalCount) ?? 0;
                        supplierResult.CityTotal = supplierResult.City_AutoMapped + supplierResult.City_MannualMapped + supplierResult.City_ReviewMapped + supplierResult.City_Unmapped; //MappingData.Where(w => w.MappingFor == "City" && w.Supplier_Id == supplier.Supplier_Id).Sum(s => s.TotalCount) ?? 0;
                        if (supplierResult.CityTotal > 0)
                        {
                            supplierResult.City_CompletePercentage = Math.Round((Convert.ToDecimal(supplierResult.City_AutoMapped + supplierResult.City_MannualMapped) / Convert.ToDecimal(supplierResult.CityTotal) * Convert.ToDecimal(100)), 2);
                        }
                        else
                        {
                            supplierResult.City_CompletePercentage = 0;
                        }
                        #endregion

                        #region Hotel Mapping Data
                        supplierResult.Hotel_TotalRecordReceived = string.Empty;
                        supplierResult.Hotel_AutoMapped = MappingData.Where(x => x.MappingFor == "Product" && x.Status == "AUTOMAPPED" && x.Supplier_Id == supplier.Supplier_Id).Sum(x => x.TotalCount) ?? 0;
                        supplierResult.Hotel_MannualMapped = MappingData.Where(x => x.MappingFor == "Product" && x.Status == "MAPPED" && x.Supplier_Id == supplier.Supplier_Id).Sum(x => x.TotalCount) ?? 0;
                        supplierResult.Hotel_ReviewMapped = MappingData.Where(x => x.MappingFor == "Product" && x.Status == "REVIEW" && x.Supplier_Id == supplier.Supplier_Id).Sum(x => x.TotalCount) ?? 0;
                        supplierResult.Hotel_Unmapped = MappingData.Where(x => x.MappingFor == "Product" && x.Status == "UNMAPPED" && x.Supplier_Id == supplier.Supplier_Id).Sum(x => x.TotalCount) ?? 0;
                        supplierResult.HotelTotal = supplierResult.Hotel_AutoMapped + supplierResult.Hotel_MannualMapped + supplierResult.Hotel_ReviewMapped + supplierResult.Hotel_Unmapped; //MappingData.Where(w => w.MappingFor == "Product" && w.Supplier_Id == supplier.Supplier_Id).Sum(s => s.TotalCount) ?? 0;
                        if (supplierResult.HotelTotal > 0)
                        {
                            supplierResult.Hotel_CompletePercentage = Math.Round((Convert.ToDecimal(supplierResult.Hotel_AutoMapped + supplierResult.Hotel_MannualMapped) / Convert.ToDecimal(supplierResult.HotelTotal) * Convert.ToDecimal(100)), 2);
                        }
                        else
                        {
                            supplierResult.Hotel_CompletePercentage = 0;
                        }
                        #endregion

                        #region Room Mapping Data
                        supplierResult.AvaialbleFromSupplier = roomsFromSupplier.Where(x => x.Supplier_Id == supplier.Supplier_Id).Sum(s => s.Totalcount);
                        supplierResult.HotelsMapped = supplierResult.Hotel_AutoMapped + supplierResult.Hotel_MannualMapped;

                        if (RoomSuppliers.Contains(supplier.Supplier_Id))
                        {
                            supplierResult.TotalEligibleRoom = eligibleRooms.Where(x => x.Supplier_Id == supplier.Supplier_Id).Sum(s => s.TotalEligibleRooms);
                        }
                        else
                        {
                            supplierResult.TotalEligibleRoom = 0;
                        }


                        supplierResult.Room_AutoMapped = MappingData.Where(x => x.MappingFor == "HotelRoom" && x.Status == "AUTOMAPPED" && x.Supplier_Id == supplier.Supplier_Id).Sum(x => x.TotalCount) ?? 0;
                        supplierResult.Room_MannualMapped = MappingData.Where(x => x.MappingFor == "HotelRoom" && x.Status == "MAPPED" && x.Supplier_Id == supplier.Supplier_Id).Sum(x => x.TotalCount) ?? 0;
                        supplierResult.Room_ReviewMapped = MappingData.Where(x => x.MappingFor == "HotelRoom" && x.Status == "REVIEW" && x.Supplier_Id == supplier.Supplier_Id).Sum(x => x.TotalCount) ?? 0;
                        supplierResult.Room_Add = probableRoomType.Where(x => x.Supplier_id == supplier.Supplier_Id).Sum(s => s.Totalcount);
                        supplierResult.Room_Unmapped = MappingData.Where(x => x.MappingFor == "HotelRoom" && x.Status == "UNMAPPED" && x.Supplier_Id == supplier.Supplier_Id).Sum(x => x.TotalCount) ?? 0;
                        supplierResult.RoomTotal = supplierResult.Room_AutoMapped + supplierResult.Room_MannualMapped + supplierResult.Room_ReviewMapped + supplierResult.Room_Unmapped; //(MappingData.Where(w => w.MappingFor == "HotelRoom" && w.Supplier_Id == supplier.Supplier_Id).Sum(s => s.TotalCount) ?? 0);

                        if (supplierResult.RoomTotal > 0)
                        {
                            supplierResult.Room_CompletePercentage = Math.Round((Convert.ToDecimal(supplierResult.Room_AutoMapped + supplierResult.Room_MannualMapped) / Convert.ToDecimal(supplierResult.RoomTotal) * Convert.ToDecimal(100)), 2);
                        }
                        else
                        {
                            supplierResult.Room_CompletePercentage = 0;
                        }
                        #endregion


                        ReturnResult.Add(supplierResult);
                    }
                    if (Supplier_id == null)
                    {
                        #region Insert Record for GrandTotal
                        var GrandTotal = new DC_SupplierExportDataReport();

                        GrandTotal.Priority = string.Empty;
                        GrandTotal.Supplier_Id = Guid.Empty;
                        GrandTotal.SupplierName = "Grand Total";

                        #region Country Mapping Data
                        //GrandTotal.LastFetchedDate = null;
                        GrandTotal.Country_TotalRecordReceived = string.Empty;
                        GrandTotal.Country_AutoMapped = ReturnResult.Sum(s => s.Country_AutoMapped); //MappingData.Where(x => x.MappingFor == "Country" && x.Status == "AUTOMAPPED").Sum(x => x.TotalCount) ?? 0;
                        GrandTotal.Country_MannualMapped = ReturnResult.Sum(s => s.Country_MannualMapped); //MappingData.Where(x => x.MappingFor == "Country" && x.Status == "MAPPED").Sum(x => x.TotalCount) ?? 0;
                        GrandTotal.Country_ReviewMapped = ReturnResult.Sum(s => s.Country_ReviewMapped); //MappingData.Where(x => x.MappingFor == "Country" && x.Status == "REVIEW").Sum(x => x.TotalCount) ?? 0;
                        GrandTotal.Country_Unmapped = ReturnResult.Sum(s => s.Country_Unmapped); //MappingData.Where(x => x.MappingFor == "Country" && x.Status == "UNMAPPED").Sum(x => x.TotalCount) ?? 0;
                        GrandTotal.CountryTotal = GrandTotal.Country_AutoMapped + GrandTotal.Country_MannualMapped + GrandTotal.Country_ReviewMapped + GrandTotal.Country_Unmapped;  //ReturnResult.Sum(s => s.CountryTotal);

                        if (GrandTotal.CountryTotal > 0)
                        {
                            GrandTotal.Country_CompletePercentage = Math.Round((Convert.ToDecimal(GrandTotal.Country_AutoMapped + GrandTotal.Country_MannualMapped) / Convert.ToDecimal(GrandTotal.CountryTotal) * Convert.ToDecimal(100)), 2);
                        }
                        else
                        {
                            GrandTotal.Country_CompletePercentage = 0;
                        }
                        #endregion

                        #region City Mapping Data
                        //Need to check with actual data
                        GrandTotal.City_TotalRecordReceived = string.Empty;
                        GrandTotal.City_AutoMapped = ReturnResult.Sum(s => s.City_AutoMapped); //MappingData.Where(x => x.MappingFor == "City" && x.Status == "AUTOMAPPED").Sum(x => x.TotalCount) ?? 0;
                        GrandTotal.City_MannualMapped = ReturnResult.Sum(s => s.City_MannualMapped); //MappingData.Where(x => x.MappingFor == "City" && x.Status == "MAPPED").Sum(x => x.TotalCount) ?? 0;
                        GrandTotal.City_ReviewMapped = ReturnResult.Sum(s => s.City_ReviewMapped); //MappingData.Where(x => x.MappingFor == "City" && x.Status == "REVIEW").Sum(x => x.TotalCount) ?? 0;
                        GrandTotal.City_Unmapped = ReturnResult.Sum(s => s.City_Unmapped); //MappingData.Where(x => x.MappingFor == "City" && x.Status == "UNMAPPED").Sum(x => x.TotalCount) ?? 0;
                        GrandTotal.CityTotal = GrandTotal.City_AutoMapped + GrandTotal.City_MannualMapped + GrandTotal.City_ReviewMapped + GrandTotal.City_Unmapped; //ReturnResult.Sum(s => s.CityTotal);
                        if (GrandTotal.CityTotal > 0)
                        {
                            GrandTotal.City_CompletePercentage = Math.Round((Convert.ToDecimal(GrandTotal.City_AutoMapped + GrandTotal.City_MannualMapped) / Convert.ToDecimal(GrandTotal.CityTotal) * Convert.ToDecimal(100)), 2);
                        }
                        else
                        {
                            GrandTotal.City_CompletePercentage = 0;
                        }
                        #endregion

                        #region Hotel Mapping Data
                        //Need to check with actual data
                        GrandTotal.Hotel_TotalRecordReceived = string.Empty;
                        GrandTotal.Hotel_AutoMapped = ReturnResult.Sum(s => s.Hotel_AutoMapped); //MappingData.Where(x => x.MappingFor == "Product" && x.Status == "AUTOMAPPED").Sum(x => x.TotalCount) ?? 0;
                        GrandTotal.Hotel_MannualMapped = ReturnResult.Sum(s => s.Hotel_MannualMapped); //MappingData.Where(x => x.MappingFor == "Product" && x.Status == "MAPPED").Sum(x => x.TotalCount) ?? 0;
                        GrandTotal.Hotel_ReviewMapped = ReturnResult.Sum(s => s.Hotel_ReviewMapped); //MappingData.Where(x => x.MappingFor == "Product" && x.Status == "REVIEW").Sum(x => x.TotalCount) ?? 0;
                        GrandTotal.Hotel_Unmapped = ReturnResult.Sum(s => s.Hotel_Unmapped); //MappingData.Where(x => x.MappingFor == "Product" && x.Status == "UNMAPPED").Sum(x => x.TotalCount) ?? 0;
                        GrandTotal.HotelTotal = GrandTotal.Hotel_AutoMapped + GrandTotal.Hotel_MannualMapped + GrandTotal.Hotel_ReviewMapped + GrandTotal.Hotel_Unmapped; //ReturnResult.Sum(s => s.HotelTotal); ;
                        if (GrandTotal.HotelTotal > 0)
                        {
                            GrandTotal.Hotel_CompletePercentage = Math.Round((Convert.ToDecimal(GrandTotal.Hotel_AutoMapped + GrandTotal.Hotel_MannualMapped) / Convert.ToDecimal(GrandTotal.HotelTotal) * Convert.ToDecimal(100)), 2);
                        }
                        else
                        {
                            GrandTotal.Hotel_CompletePercentage = 0;
                        }
                        #endregion

                        #region Room Mapping Data
                        GrandTotal.AvaialbleFromSupplier = MappingData.Where(w => w.MappingFor == "HotelRoom").Sum(s => s.TotalCount) ?? 0;
                        GrandTotal.HotelsMapped = (MappingData.Where(x => x.MappingFor == "Product" && x.Status == "AUTOMAPPED").Sum(s => s.TotalCount) ?? 0) + (MappingData.Where(x => x.MappingFor == "Product" && x.Status == "MAPPED").Sum(s => s.TotalCount) ?? 0);
                        GrandTotal.TotalEligibleRoom = eligibleRooms.Sum(s => s.TotalEligibleRooms);
                        GrandTotal.Room_AutoMapped = ReturnResult.Sum(s => s.Room_AutoMapped); //MappingData.Where(x => x.MappingFor == "HotelRoom" && x.Status == "AUTOMAPPED").Sum(x => x.TotalCount) ?? 0;
                        GrandTotal.Room_MannualMapped = ReturnResult.Sum(s => s.Room_MannualMapped); //MappingData.Where(x => x.MappingFor == "HotelRoom" && x.Status == "MAPPED").Sum(x => x.TotalCount) ?? 0;
                        GrandTotal.Room_ReviewMapped = ReturnResult.Sum(s => s.Room_ReviewMapped); //MappingData.Where(x => x.MappingFor == "HotelRoom" && x.Status == "REVIEW").Sum(x => x.TotalCount) ?? 0;
                        GrandTotal.Room_Add = probableRoomType.Sum(s => s.Totalcount);
                        GrandTotal.Room_Unmapped = ReturnResult.Sum(s => s.Room_Unmapped); //MappingData.Where(x => x.MappingFor == "HotelRoom" && x.Status == "UNMAPPED").Sum(x => x.TotalCount) ?? 0;
                        GrandTotal.RoomTotal = GrandTotal.Room_AutoMapped + GrandTotal.Room_MannualMapped + GrandTotal.Room_ReviewMapped + GrandTotal.Room_Unmapped; //ReturnResult.Sum(s => s.RoomTotal); ;

                        if (GrandTotal.RoomTotal > 0)
                        {
                            GrandTotal.Room_CompletePercentage = Math.Round((Convert.ToDecimal(GrandTotal.Room_AutoMapped + GrandTotal.Room_MannualMapped) / Convert.ToDecimal(GrandTotal.RoomTotal) * Convert.ToDecimal(100)), 2);
                        }
                        else
                        {
                            GrandTotal.Room_CompletePercentage = 0;
                        }
                        #endregion

                        #endregion
                        ReturnResult.Add(GrandTotal);
                    }
                    return ReturnResult;
                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
        #endregion

        #region roll_off_reports
        public List<DataContracts.Mapping.DC_RollOffReportRule> getStatisticforRuleReport(DataContracts.Mapping.DC_RollOFParams parm)
        {
            List<DataContracts.Mapping.DC_RollOffReportRule> objLst = new List<DC_RollOffReportRule>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    DateTime fd = Convert.ToDateTime(parm.Fromdate);
                    DateTime td = Convert.ToDateTime(parm.ToDate);
                    var search = (from t in context.Accommodation_RuleInfo
                                  join t1 in context.Accommodation on t.Accommodation_Id equals t1.Accommodation_Id
                                  where (t.Create_Date >= fd && t.Edit_Date <= td)
                                  select new
                                  {
                                      HotelID = t1.CompanyHotelID,
                                      HotelName = t1.HotelName,
                                      RuleName = t.RuleType,
                                      Description = t.Description,
                                      flag = t.IsInternal == true ? "YES" : "NO",
                                      LupdateDate = t.Edit_Date == null ? t.Create_Date : t.Edit_Date,
                                      LupdateBy = t.Edit_User == null ? t.Create_User : t.Edit_User
                                  }).ToList();
                    foreach (var item in search)
                    {

                        DC_RollOffReportRule obj = new DC_RollOffReportRule();
                        obj.Hotelid = item.HotelID ?? 0;
                        obj.Hotelname = item.HotelName;
                        obj.RuleName = item.RuleName;
                        obj.Description = item.Description;
                        obj.Internal_Flag = item.flag;
                        if (item.LupdateDate != null)
                        {
                            obj.LastupdateDate = item.LupdateDate.Value.ToString("dd/MM/yyyy");
                        }
                        else { obj.LastupdateDate = ""; }
                        obj.LastupdatedBy = item.LupdateBy;
                        objLst.Add(obj);
                    }
                }

            }
            catch
            {

            }
            return objLst;
        }

        public List<DataContracts.Mapping.DC_RollOffReportStatus> getStatisticforStatusReport(DataContracts.Mapping.DC_RollOFParams parm)
        {
            List<DataContracts.Mapping.DC_RollOffReportStatus> objLst = new List<DC_RollOffReportStatus>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    DateTime fd = Convert.ToDateTime(parm.Fromdate);
                    DateTime td = Convert.ToDateTime(parm.ToDate);
                    var search = (from t in context.Accommodation_Status
                                  join t1 in context.Accommodation on t.Accommodation_Id equals t1.Accommodation_Id
                                  where (t.From >= fd && t.To <= td)
                                  select new
                                  {
                                      HotelID = t1.CompanyHotelID,
                                      HotelName = t1.HotelName,
                                      market = t.CompanyMarket,
                                      status = t.Status,
                                      fromd = t.From,
                                      tod = t.To,
                                      reason = t.DeactivationReason,
                                      // flag = t.IsInternal == true ? "YES" : "NO",
                                      LupdateDate = t.Edit_Date == null ? t.Create_Date : t.Edit_Date,
                                      LupdateBy = t.Edit_User == null ? t.Create_User : t.Edit_User
                                  }).ToList();
                    foreach (var item in search)
                    {

                        DC_RollOffReportStatus obj = new DC_RollOffReportStatus();
                        obj.Hotelid = item.HotelID ?? 0;
                        obj.Hotelname = item.HotelName;
                        obj.Companymarket = item.market;
                        obj.Status = item.status;

                        if (item.fromd != null)
                        {
                            obj.Validfrom = item.fromd.Value.ToString("dd/MM/yyyy");
                        }
                        else { obj.Validfrom = ""; }
                        if (item.tod != null)
                        {
                            obj.Validto = item.tod.Value.ToString("dd/MM/yyyy");
                        }
                        else { obj.Validto = ""; }
                        if (item.LupdateDate != null)
                        {
                            obj.LastupdateDate = item.LupdateDate.Value.ToString("dd/MM/yyyy");
                        }
                        else { obj.LastupdateDate = ""; }
                        //obj.Validto = item.tod.Value.ToString("dd/MM/yyyy") ?? "";
                        //obj.LastupdateDate = item.LupdateDate.Value.ToString("dd/MM/yyyy") ?? "";
                        obj.Reason = item.reason;
                        // obj.Internal_Flag = item.flag;
                        obj.LastupdatedBy = item.LupdateBy;
                        objLst.Add(obj);
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return objLst;
        }

        public List<DataContracts.Mapping.DC_RollOffReportUpdate> getStatisticforUpdateReport(DataContracts.Mapping.DC_RollOFParams parm)
        {
            List<DataContracts.Mapping.DC_RollOffReportUpdate> objLst = new List<DC_RollOffReportUpdate>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    DateTime fd = Convert.ToDateTime(parm.Fromdate);
                    DateTime td = Convert.ToDateTime(parm.ToDate);
                    var search = (from t in context.Accommodation_HotelUpdates
                                  join t1 in context.Accommodation on t.Accommodation_Id equals t1.Accommodation_Id
                                  where (t.FromDate >= fd && t.ToDate <= td)
                                  select new
                                  {
                                      HotelID = t1.CompanyHotelID,
                                      HotelName = t1.HotelName,
                                      HotelUpdate = t.Description,
                                      source = t.Source,
                                      fromd = t.FromDate,
                                      tod = t.ToDate,
                                      flag = t.IsInternal == true ? "YES" : "NO",
                                      LupdateDate = t.Edit_Date == null ? t.Create_Date : t.Edit_Date,
                                      LupdateBy = t.Edit_User == null ? t.Create_User : t.Edit_User
                                  }).ToList();
                    foreach (var item in search)
                    {

                        DC_RollOffReportUpdate obj = new DC_RollOffReportUpdate();
                        obj.Hotelid = item.HotelID ?? 0;
                        obj.Hotelname = item.HotelName;
                        obj.Hotelupdate = item.HotelUpdate;
                        obj.Descriptionsource = item.source;
                        if (item.fromd != null)
                        {
                            obj.Validfrom = item.fromd.Value.ToString("dd/MM/yyyy");
                        }
                        else { obj.Validfrom = ""; }
                        if (item.tod != null)
                        {
                            obj.Validto = item.tod.Value.ToString("dd/MM/yyyy");
                        }
                        else { obj.Validto = ""; }
                        if (item.LupdateDate != null)
                        {
                            obj.LastupdateDate = item.LupdateDate.Value.ToString("dd/MM/yyyy");
                        }
                        else { obj.LastupdateDate = ""; }
                        // obj.Validfrom = Convert.ToString(item.fromd);
                        // obj.Validto = Convert.ToString(item.tod);
                        obj.Internal_Flag = item.flag;
                        // obj.LastupdateDate = Convert.ToString(item.LupdateDate);
                        obj.LastupdatedBy = item.LupdateBy;
                        objLst.Add(obj);
                    }
                }

            }
            catch
            {

            }
            return objLst;
        }
        #endregion

        #region rdlc reports
        public List<DataContracts.Mapping.DC_supplierwiseUnmappedReport> GetsupplierwiseUnmappedDataReport(Guid SupplierID)
        {
            List<DataContracts.Mapping.DC_supplierwiseUnmappedReport> _objList = new List<DC_supplierwiseUnmappedReport>();
            DataContracts.Mapping.DC_supplierwiseUnmappedReport _obj = new DC_supplierwiseUnmappedReport();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (SupplierID != Guid.Empty)
                    {
                        var searchcountry = (from s in context.m_CountryMapping
                                             where s.Supplier_Id == SupplierID
        && (s.Status == "UNMAPPED" || s.Status == "REVIEW")
                                             select s).ToList();
                        var searchcity = (from s in context.m_CityMapping
                                          where s.Supplier_Id == SupplierID && (s.Status == "UNMAPPED" || s.Status == "REVIEW")
                                          select s).ToList();
                        var searchproduct = (from s in context.Accommodation_ProductMapping
                                             where s.Supplier_Id == SupplierID && (s.Status == "UNMAPPED" || s.Status == "REVIEW")
                                             select s).ToList();
                        var searchactivity = (from s in context.Activity_SupplierProductMapping
                                              where s.Supplier_ID == SupplierID && (s.MappingStatus == "UNMAPPED" || s.MappingStatus == "REVIEW")
                                              select s).ToList();
                        List<DC_UnmappedCountryReport> _lstCountry = new List<DC_UnmappedCountryReport>();
                        foreach (var item in searchcountry)
                        {
                            DC_UnmappedCountryReport objCo = new DC_UnmappedCountryReport();
                            objCo.Countrycode = item.CountryCode;
                            objCo.Contryname = item.CountryName;
                            _lstCountry.Add(objCo);

                        }
                        _obj.Unmappedcountry = _lstCountry;
                        List<DC_UnmappedCityReport> _lstCity = new List<DC_UnmappedCityReport>();
                        foreach (var item in searchcity)
                        {
                            DC_UnmappedCityReport objcity = new DC_UnmappedCityReport();
                            objcity.Countrycode = item.CountryCode;
                            objcity.Countryname = item.CountryName;
                            objcity.Citycode = item.CityCode;
                            objcity.Cityname = item.CityName;
                            _lstCity.Add(objcity);

                        }
                        _obj.Unmappedcity = _lstCity;

                        List<DC_unmappedProductReport> _lstproduct = new List<DC_unmappedProductReport>();
                        foreach (var item in searchproduct)
                        {
                            DC_unmappedProductReport objpro = new DC_unmappedProductReport();
                            objpro.Hotelname = item.ProductName;
                            objpro.Country = item.CountryName;
                            objpro.City = item.CityName;
                            objpro.Address = item.address;
                            _lstproduct.Add(objpro);

                        }
                        _obj.Unmappedproduct = _lstproduct;

                        List<DC_unmappedActivityReport> _lstactivity = new List<DC_unmappedActivityReport>();

                        foreach (var item in searchactivity)
                        {
                            DC_unmappedActivityReport objact = new DC_unmappedActivityReport();
                            objact.Activityname = item.SupplierProductName;
                            objact.Country = item.SupplierCountryName;
                            objact.City = item.SupplierCityName;
                            objact.Address = item.Address;
                            _lstactivity.Add(objact);
                        }
                        _obj.Unmappedactivity = _lstactivity;
                        _objList.Add(_obj);

                    }
                }

            }
            catch (Exception ex)
            {

            }
            return _objList;
        }

        public List<DataContracts.Mapping.DC_UnmappedCountryReport> GetsupplierwiseUnmappedCountryReport(Guid SupplierID)
        {
            List<DataContracts.Mapping.DC_UnmappedCountryReport> _objList = new List<DC_UnmappedCountryReport>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (SupplierID != Guid.Empty)
                    {
                        var searchcountry = (from s in context.m_CountryMapping
                                             where s.Supplier_Id == SupplierID && (s.Status == "UNMAPPED" || s.Status == "REVIEW")
                                             select s).ToList();
                        foreach (var item in searchcountry)
                        {
                            DC_UnmappedCountryReport objCo = new DC_UnmappedCountryReport();
                            objCo.Countrycode = item.CountryCode;
                            objCo.Contryname = item.CountryName;
                            _objList.Add(objCo);

                        }

                    }
                }

            }
            catch (Exception ex)
            {

            }
            return _objList;

        }

        public List<DataContracts.Mapping.DC_UnmappedCityReport> GetsupplierwiseUnmappedCityReport(Guid SupplierID)
        {
            List<DataContracts.Mapping.DC_UnmappedCityReport> _objList = new List<DC_UnmappedCityReport>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (SupplierID != Guid.Empty)
                    {
                        var searchcity = (from s in context.m_CityMapping
                                          where s.Supplier_Id == SupplierID && (s.Status == "UNMAPPED" || s.Status == "REVIEW")
                                          select s).ToList();
                        foreach (var item in searchcity)
                        {
                            DC_UnmappedCityReport objCi = new DC_UnmappedCityReport();
                            objCi.Countrycode = item.CountryCode;
                            objCi.Countryname = item.CountryName;
                            objCi.Citycode = item.CityCode;
                            objCi.Cityname = item.CityName;
                            _objList.Add(objCi);

                        }

                    }
                }

            }
            catch (Exception ex)
            {

            }
            return _objList;

        }

        public List<DataContracts.Mapping.DC_unmappedProductReport> GetsupplierwiseUnmappedProductReport(Guid SupplierID)
        {
            List<DataContracts.Mapping.DC_unmappedProductReport> _objList = new List<DC_unmappedProductReport>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (SupplierID != Guid.Empty)
                    {
                        var searchproduct = (from s in context.Accommodation_ProductMapping
                                             where s.Supplier_Id == SupplierID && (s.Status == "UNMAPPED" || s.Status == "REVIEW")
                                             select s).ToList();
                        foreach (var item in searchproduct)
                        {
                            DC_unmappedProductReport objPi = new DC_unmappedProductReport();
                            objPi.Hotelname = item.ProductName;
                            objPi.Country = item.CountryName;
                            objPi.City = item.CityName;
                            objPi.Address = item.address;
                            objPi.SupplierName = item.SupplierName;
                            objPi.SupplierHotelId = item.SupplierProductReference;
                            _objList.Add(objPi);

                        }

                    }
                }

            }
            catch (Exception ex)
            {

            }
            return _objList;

        }

        public List<DataContracts.Mapping.DC_unmappedActivityReport> GetsupplierwiseUnmappedActivityReport(Guid SupplierID)
        {
            List<DataContracts.Mapping.DC_unmappedActivityReport> _objList = new List<DC_unmappedActivityReport>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (SupplierID != Guid.Empty)
                    {
                        var searchactivity = (from s in context.Activity_SupplierProductMapping
                                              where s.Supplier_ID == SupplierID && (s.MappingStatus == "UNMAPPED" || s.MappingStatus == "REVIEW")
                                              select s).ToList();
                        foreach (var item in searchactivity)
                        {
                            DC_unmappedActivityReport objac = new DC_unmappedActivityReport();
                            objac.Activityname = item.SupplierProductName;
                            objac.Country = item.SupplierCountryName;
                            objac.City = item.SupplierCityName;
                            objac.Address = item.Address;
                            objac.SupplierName = item.SupplierName;
                            objac.SupplierActivityId = item.SupplierCode;
                            _objList.Add(objac);

                        }

                    }
                }

            }
            catch (Exception ex)
            {

            }
            return _objList;

        }

        public List<DataContracts.Mapping.DC_supplierwisesummaryReport> GetsupplierwiseSummaryReport()
        {
            List<DataContracts.Mapping.DC_supplierwisesummaryReport> _objList = new List<DC_supplierwisesummaryReport>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {

                    var searchasummary = context.Dashboard_MappingStat.
                         Where(c => c.SupplierName != "ALL" && c.SupplierName != null && (c.Status == "UNMAPPED" || c.Status == "REVIEW") && c.Supplier_Id != Guid.Empty)
                        .GroupBy(c => new { c.Supplier_Id, c.SupplierName })
                        .Select(g => new
                        {
                            Country = g.Where(c => c.MappingFor == "Country").Sum(c => c.TotalCount),
                            City = g.Where(c => c.MappingFor == "City").Sum(c => c.TotalCount),
                            Product = g.Where(c => c.MappingFor == "Product").Sum(c => c.TotalCount),
                            hotelroom = g.Where(c => c.MappingFor == "HotelRoom").Sum(c => c.TotalCount),
                            activity = g.Where(c => c.MappingFor == "Activity").Sum(c => c.TotalCount),
                            suppliername = g.Key.SupplierName,
                        }
                        ).ToList();
                    foreach (var item in searchasummary)
                    {
                        DC_supplierwisesummaryReport objsu = new DC_supplierwisesummaryReport();
                        objsu.Country = item.Country.GetValueOrDefault();
                        objsu.City = item.City.GetValueOrDefault();
                        objsu.Product = item.Product.GetValueOrDefault();
                        objsu.Activity = item.activity.GetValueOrDefault();
                        objsu.Hotelrooom = item.hotelroom.GetValueOrDefault();
                        objsu.Suppliername = item.suppliername;
                        _objList.Add(objsu);

                    }

                }


            }
            catch (Exception ex)
            {

            }
            return _objList;

        }

        public List<DataContracts.Mapping.DC_supplierwiseunmappedsummaryReport> GetsupplierwiseUnmappedSummaryReport(Guid SupplierID)
        {
            List<DataContracts.Mapping.DC_supplierwiseunmappedsummaryReport> _objList = new List<DC_supplierwiseunmappedsummaryReport>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (SupplierID != Guid.Empty)
                    {
                        var searchasummary = (from cm in context.m_CountryMapping
                                              join ctm in context.m_CityMapping on
                                                  new { cm.CountryName, cm.Supplier_Id } equals new { ctm.CountryName, ctm.Supplier_Id }
                                              let prod = (from apm in context.Accommodation_ProductMapping
                                                          where (apm.Status == "UNMAPPED" || apm.Status == "REVIEW") &&
                                                          apm.CityName == ctm.CityName && apm.CountryName == cm.CountryName
                                                          && apm.Supplier_Id == ctm.Supplier_Id
                                                          select apm
                                                    ).Count()
                                              where (ctm.Status == "UNMAPPED" || ctm.Status == "REVIEW")
                                                     && ctm.Supplier_Id == SupplierID
                                              select new DC_supplierwiseunmappedsummaryReport
                                              {
                                                  Suppliername = cm.SupplierName,
                                                  Noofproducts = prod,
                                                  Cityname = ctm.CityName,
                                                  Countryname = cm.CountryName
                                              }).Distinct().ToList();
                        foreach (var item in searchasummary)
                        {
                            DC_supplierwiseunmappedsummaryReport objsu = new DC_supplierwiseunmappedsummaryReport();
                            objsu.Suppliername = item.Suppliername;
                            objsu.Noofproducts = item.Noofproducts;
                            objsu.Cityname = item.Cityname;
                            objsu.Countryname = item.Countryname;
                            _objList.Add(objsu);

                        }

                    }
                }

            }
            catch (Exception ex)
            {

            }
            return _objList;
        }
        #endregion

        #region Master Attribute Mapping
        public List<DataContracts.Mapping.DC_MasterAttributeMapping_RS> SearchMasterAttributeMapping(DataContracts.Mapping.DC_MasterAttributeMapping_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {

                    var search = from a in context.m_MasterAttributeMapping select a;
                    if (RQ.MasterAttributeType_Id.HasValue)
                    {
                        search = from a in search
                                 where a.SystemMasterAttribute_Id == RQ.MasterAttributeType_Id
                                 select a;
                    }

                    if (RQ.Supplier_Id.HasValue)
                    {
                        search = from a in search
                                 where a.Supplier_Id == RQ.Supplier_Id
                                 select a;
                    }

                    int total;

                    total = search.Count();

                    var skip = RQ.PageSize * RQ.PageNo;

                    var searchList = (from a in search
                                      join sup in context.Supplier on a.Supplier_Id equals sup.Supplier_Id
                                      join ma in context.m_masterattribute on a.SystemMasterAttribute_Id equals ma.MasterAttribute_Id
                                      orderby sup.Name
                                      select new DataContracts.Mapping.DC_MasterAttributeMapping_RS
                                      {
                                          MasterAttributeMapping_Id = a.MasterAttributeMapping_Id,
                                          Supplier_Attribute_Type = a.SupplierMasterAttribute,
                                          Supplier_Code = sup.Code,
                                          Supplier_Name = sup.Name,
                                          System_Attribute_Type = ma.Name,
                                          Status = a.Status,
                                          IsActive = a.IsActive,
                                          TotalRecords = total
                                      }).Skip(skip).Take(RQ.PageSize).ToList();

                    return searchList;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching master attribute mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public DataContracts.Mapping.DC_MasterAttributeMapping GetMasterAttributeMapping(Guid MasterAttributeMapping_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = (from a in context.m_MasterAttributeMapping
                                  join sup in context.Supplier on a.Supplier_Id equals sup.Supplier_Id
                                  join mav in context.m_masterattributevalue on a.SystemMasterAttribute_Id equals mav.MasterAttributeValue_Id
                                  where a.MasterAttributeMapping_Id == MasterAttributeMapping_Id
                                  select new DataContracts.Mapping.DC_MasterAttributeMapping
                                  {
                                      MasterAttributeMapping_Id = a.MasterAttributeMapping_Id,
                                      Create_Date = a.Create_Date,
                                      Create_User = a.Create_User,
                                      Edit_Date = a.Edit_Date,
                                      Edit_User = a.Edit_User,
                                      IsActive = a.IsActive,
                                      Status = a.Status,
                                      SupplierMasterAttribute = a.SupplierMasterAttribute,
                                      Supplier_Code = sup.Code,
                                      Supplier_Id = a.Supplier_Id,
                                      Supplier_Name = sup.Name,
                                      SystemMasterAttribute = mav.AttributeValue,
                                      SystemMasterAttribute_Id = a.SystemMasterAttribute_Id
                                  }).FirstOrDefault();

                    return search;

                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching master attribute mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public List<DataContracts.Mapping.DC_MasterAttributeValueMappingRS> GetMasterAttributeValueMapping(DataContracts.Mapping.DC_MasterAttributeValueMapping_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {

                    var m_MasterAttributeMapping = (from a in context.m_MasterAttributeMapping select a).AsQueryable();
                    var m_masterattribute = (from a in context.m_masterattribute select a).AsQueryable();
                    var m_masterattributevalue = (from a in context.m_masterattributevalue select a).AsQueryable();
                    var m_masterattributevalueGlobal = (from a in context.m_masterattributevalue select a).AsQueryable();
                    var m_MasterAttributeValueMapping = (from a in context.m_MasterAttributeValueMapping select a).AsQueryable();

                    if (RQ.MasterAttributeMapping_Id != null)
                    {
                        if (RQ.MasterAttributeMapping_Id != Guid.Empty)
                        {
                            m_MasterAttributeMapping = from a in m_MasterAttributeMapping where a.MasterAttributeMapping_Id == RQ.MasterAttributeMapping_Id select a;
                            // m_MasterAttributeValueMapping = from a in m_MasterAttributeValueMapping where a.MasterAttributeMapping_Id == RQ.MasterAttributeMapping_Id select a;
                        }
                    }

                    if (RQ.SystemMasterAttributeValue_Id != null)
                    {
                        if (RQ.SystemMasterAttributeValue_Id != Guid.Empty)
                        {
                            m_masterattributevalue = from a in m_masterattributevalue where a.MasterAttributeValue_Id == RQ.SystemMasterAttributeValue_Id select a;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.SystemMasterAttributeValue))
                    {
                        m_masterattributevalue = from a in m_masterattributevalue where a.AttributeValue.ToUpper().Trim() == RQ.SystemMasterAttributeValue.ToUpper().Trim() select a;
                    }

                    int total = (from map in m_MasterAttributeMapping
                                 join ma in m_masterattribute on map.SystemMasterAttribute_Id equals ma.MasterAttribute_Id
                                 join mav in m_masterattributevalue on ma.MasterAttribute_Id equals mav.MasterAttribute_Id
                                 where mav.IsActive == true
                                 select mav).Count();

                    var skip = RQ.PageSize * RQ.PageNo;

                    var searchReturn = (from map in m_MasterAttributeMapping
                                        join ma in m_masterattribute on map.SystemMasterAttribute_Id equals ma.MasterAttribute_Id
                                        join mav in m_masterattributevalue on ma.MasterAttribute_Id equals mav.MasterAttribute_Id
                                        join mavp in m_masterattributevalueGlobal on (mav.ParentAttributeValue_Id ?? Guid.Empty) equals mavp.MasterAttributeValue_Id into mavploj
                                        from mavplojr in mavploj.DefaultIfEmpty()
                                        where mav.IsActive == true
                                        orderby (mavplojr.AttributeValue ?? string.Empty).Trim(), mav.AttributeValue.Trim()
                                        select new DataContracts.Mapping.DC_MasterAttributeValueMappingRS
                                        {
                                            MasterAttributeMapping_Id = map.MasterAttributeMapping_Id,
                                            ParentAttributeValue = (mavplojr.AttributeValue ?? string.Empty).Trim(),
                                            SystemMasterAttributeValue = mav.AttributeValue.Trim(),
                                            SystemMasterAttributeValue_Id = mav.MasterAttributeValue_Id,
                                            TotalRecords = total,
                                            SupplierAttributeValues = (from imavm in m_MasterAttributeValueMapping
                                                                       where imavm.MasterAttributeMapping_Id == map.MasterAttributeMapping_Id
                                                                       && imavm.SystemMasterAttributeValue_Id == mav.MasterAttributeValue_Id
                                                                       select new DataContracts.Mapping.DC_SupplierAttributeValues
                                                                       {
                                                                           MasterAttributeValueMapping_Id = imavm.MasterAttributeValueMapping_Id,
                                                                           SupplierMasterAttributeValue = imavm.SupplierMasterAttributeValue.Trim(),
                                                                           SupplierMasterAttributeCode = imavm.SupplierMasterAttributeCode.Trim(),
                                                                           Create_User = imavm.Create_User,
                                                                           Create_Date = imavm.Create_Date,
                                                                           Edit_Date = imavm.Edit_Date,
                                                                           Edit_User = imavm.Edit_User,
                                                                           IsActive = imavm.IsActive
                                                                       }).ToList()
                                        }).Skip(skip).Take(RQ.PageSize);

                    return searchReturn.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching master attribute value mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public DC_MasterAttributeMappingAdd_RS AddMasterAttributeMapping(DataContracts.Mapping.DC_MasterAttributeMapping param)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var duplicateSearch = from a in context.m_MasterAttributeMapping
                                          where a.Supplier_Id == param.Supplier_Id
                                          && a.SystemMasterAttribute_Id == param.SystemMasterAttribute_Id
                                          select a;
                    if (duplicateSearch.Count() > 0)
                    {
                        return new DC_MasterAttributeMappingAdd_RS
                        {
                            Message = new DataContracts.DC_Message
                            {
                                StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Duplicate,
                                StatusMessage = "Data already exist."
                            }
                        };
                    }
                    else
                    {
                        m_MasterAttributeMapping newObj = new m_MasterAttributeMapping();

                        if (param.MasterAttributeMapping_Id == Guid.Empty)
                        {
                            param.MasterAttributeMapping_Id = Guid.NewGuid();
                        }

                        newObj.MasterAttributeMapping_Id = param.MasterAttributeMapping_Id;
                        newObj.SupplierMasterAttribute = param.SupplierMasterAttribute;
                        newObj.Supplier_Id = param.Supplier_Id;
                        newObj.SystemMasterAttribute_Id = param.SystemMasterAttribute_Id;
                        newObj.Status = param.Status;
                        newObj.IsActive = true;
                        newObj.Create_Date = DateTime.Now;
                        newObj.Create_User = param.Create_User;

                        context.m_MasterAttributeMapping.Add(newObj);
                        context.SaveChanges();

                        return new DC_MasterAttributeMappingAdd_RS
                        {
                            Message = new DataContracts.DC_Message
                            {
                                StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Success,
                                StatusMessage = "Data saved successfully."
                            },
                            AttributeMapping_Id = param.MasterAttributeMapping_Id
                        };

                    }

                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while adding master attribute mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public DataContracts.DC_Message UpdateMasterAttributeMapping(DataContracts.Mapping.DC_MasterAttributeMapping param)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var duplicateSearch = from a in context.m_MasterAttributeMapping
                                          where a.SupplierMasterAttribute == param.SupplierMasterAttribute.Trim()
                                          && a.Supplier_Id == param.Supplier_Id
                                          && a.SystemMasterAttribute_Id == param.SystemMasterAttribute_Id
                                          && a.MasterAttributeMapping_Id != param.MasterAttributeMapping_Id
                                          select a;
                    if (duplicateSearch.Count() > 0)
                    {
                        return new DataContracts.DC_Message
                        {
                            StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Duplicate,
                            StatusMessage = "Data already exist."
                        };
                    }
                    else
                    {
                        DataContracts.DC_Message msg = new DataContracts.DC_Message();

                        var search = context.m_MasterAttributeMapping.Find(param.MasterAttributeMapping_Id);

                        if (search != null)
                        {
                            if (search.IsActive != param.IsActive)
                            {
                                search.IsActive = param.IsActive;

                                if (param.IsActive)
                                {
                                    msg.StatusMessage = "Data undeleted successfully";
                                }
                                else
                                {
                                    msg.StatusMessage = "Data deleted successfully";
                                }
                            }
                            else
                            {
                                search.Status = param.Status;
                                search.SupplierMasterAttribute = param.SupplierMasterAttribute;

                                msg.StatusMessage = "Data updated successfully";
                            }

                            search.Edit_Date = DateTime.Now;
                            search.Edit_User = param.Edit_User;

                        }

                        context.SaveChanges();

                        msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Success;
                        return msg;
                    }

                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating master attribute mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public DataContracts.DC_Message UpdateMasterAttributeValueMapping(List<DataContracts.Mapping.DC_MasterAttributeValueMapping> paramList)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    foreach (var param in paramList)
                    {
                        var search = context.m_MasterAttributeValueMapping.Find(param.MasterAttributeValueMapping_Id);

                        if (search != null)
                        {
                            search.IsActive = param.IsActive ?? false;
                            search.Edit_Date = DateTime.Now;
                            search.Edit_User = param.Edit_User;
                            search.SupplierMasterAttributeValue = param.SupplierMasterAttributeValue;
                            search.SupplierMasterAttributeCode = param.SupplierMasterAttributeCode;
                            context.SaveChanges();
                        }
                        else
                        {
                            m_MasterAttributeValueMapping newObj = new m_MasterAttributeValueMapping();
                            newObj.MasterAttributeMapping_Id = param.MasterAttributeMapping_Id;
                            newObj.MasterAttributeValueMapping_Id = param.MasterAttributeValueMapping_Id ?? Guid.NewGuid();
                            newObj.SupplierMasterAttributeValue = param.SupplierMasterAttributeValue;
                            newObj.SupplierMasterAttributeCode = param.SupplierMasterAttributeCode;
                            newObj.SystemMasterAttributeValue_Id = param.SystemMasterAttributeValue_Id;
                            newObj.IsActive = param.IsActive ?? false;
                            newObj.Create_Date = DateTime.Now;
                            newObj.Create_User = param.Create_User;
                            context.m_MasterAttributeValueMapping.Add(newObj);
                            context.SaveChanges();
                        }
                    }

                    return new DataContracts.DC_Message
                    {
                        StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Success,
                        StatusMessage = "Data saved successfully."
                    };
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating master attribute value mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public DataContracts.DC_Message DeleteMasterAttributeValueMapping(DC_SupplierAttributeValues_RQ param)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (param.MasterAttributeValueMapping_Id != null)
                    {
                        var search = context.m_MasterAttributeValueMapping.Find(param.MasterAttributeValueMapping_Id);
                        if (search != null)
                        {
                            context.m_MasterAttributeValueMapping.Remove(search);
                        }
                        context.SaveChanges();
                        return new DataContracts.DC_Message
                        {
                            StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Success,
                            StatusMessage = "Data Deleted successfully."
                        };
                    }
                    else
                    {
                        return new DataContracts.DC_Message
                        {
                            StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Warning,
                            StatusMessage = "No data found to delete."
                        };
                    }
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while Deleting Supplier attribute value", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        #endregion

        #region Activity Mapping
        public DataContracts.Mapping.DC_Acitivity_SupplierProductMapping GetActivitySupplierProductMappingById(Guid ActivitySupplierProductMapping_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var prodMapSearch = from a in context.Activity_SupplierProductMapping select a;
                    //System.Linq.IQueryable<DataContracts.Mapping.DC_Acitivity_SupplierProductMapping> prodMapList;

                    var result = (from a in prodMapSearch
                                  where a.ActivitySupplierProductMapping_Id == ActivitySupplierProductMapping_Id
                                  orderby a.SupplierName
                                  select new DataContracts.Mapping.DC_Acitivity_SupplierProductMapping
                                  {
                                      Activity_ID = a.Activity_ID,
                                      Supplier_ID = a.Supplier_ID,
                                      ActivitySupplierProductMapping_Id = a.ActivitySupplierProductMapping_Id,
                                      SupplierCode = a.SupplierCode,
                                      SupplierName = a.SupplierName,
                                      SuplierProductCode = a.SuplierProductCode,
                                      SupplierProductType = a.SupplierProductType,
                                      SupplierType = a.SupplierType,
                                      SupplierLocationId = a.SupplierLocationId,
                                      SupplierLocationName = a.SupplierLocationName,
                                      SupplierCountryName = a.SupplierCountryName,
                                      SupplierCityName = a.SupplierCityName,
                                      SupplierCountryCode = a.SupplierCountryCode,
                                      SupplierCityCode = a.SupplierCityCode,
                                      SupplierStateName = a.SupplierStateName,
                                      SupplierStateCode = a.SupplierStateCode,
                                      SupplierCityIATACode = a.SupplierCityIATACode,
                                      Duration = a.Duration,
                                      SupplierProductName = a.SupplierProductName,
                                      SupplierDataLangaugeCode = a.SupplierDataLangaugeCode,
                                      Introduction = a.Introduction,
                                      Conditions = a.Conditions,
                                      Inclusions = a.Inclusions,
                                      Exclusions = a.Exclusions,
                                      AdditionalInformation = a.AdditionalInformation,
                                      DeparturePoint = a.DeparturePoint,
                                      TicketingDetails = a.TicketingDetails,
                                      Currency = a.Currency,
                                      DepartureTime = a.DepartureTime,
                                      DepartureDate = a.DepartureDate,
                                      DateFrom = a.DateFrom,
                                      DateTo = a.DateTo,
                                      BlockOutDateFrom = a.BlockOutDateFrom,
                                      BlockOutDateTo = a.BlockOutDateTo,
                                      OptionTitle = a.OptionTitle,
                                      OptionCode = a.OptionCode,
                                      OptionDescription = a.OptionDescription,
                                      TourActivityLangauageCode = a.TourActivityLangauageCode,
                                      ProductDescription = a.ProductDescription,
                                      TourActivityLanguage = a.TourActivityLanguage,
                                      ImgURL = a.ImgURL,
                                      ProductValidFor = a.ProductValidFor,
                                      Address = a.Address,
                                      Latitude = a.Latitude,
                                      Longitude = a.Longitude,
                                      DayPattern = a.DayPattern,
                                      Theme = a.Theme,
                                      Distance = a.Distance,
                                      SupplierTourType = a.SupplierTourType,
                                      MappingStatus = a.MappingStatus,
                                      Create_Date = a.Create_Date,
                                      Edit_Date = a.Edit_Date,
                                      Create_User = a.Create_User,
                                      Edit_User = a.Edit_User,
                                      MapID = a.MapID
                                  }).FirstOrDefault();
                    if (result != null && !string.IsNullOrWhiteSpace(result.SupplierCountryName) && !string.IsNullOrWhiteSpace(result.SupplierCityName))
                    {
                        var resultCity = context.m_CityMapping.Where(a => (a.Supplier_Id == result.Supplier_ID) && (a.Status == "MAPPED")).ToList();
                        if (resultCity != null && resultCity.Count > 0)
                        {
                            var citymaster = (from ct in resultCity where ct.CityName == result.SupplierCityName select ct).FirstOrDefault();
                            if (citymaster != null)
                            {
                                var mastercityName = (from a in context.m_CityMaster where a.City_Id == citymaster.City_Id select a.Name).FirstOrDefault();
                                if (mastercityName != null)
                                    result.SystemCityName = mastercityName;
                            }
                        }


                    }
                    return result;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching accomodation product mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public List<DataContracts.Mapping.DC_Acitivity_SupplierProductMapping> GetActivitySupplierProductMappingSearch(DataContracts.Mapping.DC_Acitivity_SupplierProductMapping_Search_RQ obj)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var prodMapSearch = from a in context.Activity_SupplierProductMapping select a; //where a.Accommodation_Id == null 

                    if (obj.ActivitySupplierProductMappling_Id != Guid.Empty)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.ActivitySupplierProductMapping_Id == obj.ActivitySupplierProductMappling_Id
                                        select a;
                    }
                    if (obj.Activity_ID != Guid.Empty)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.Activity_ID == obj.Activity_ID
                                        select a;
                    }
                    if (obj.Supplier_ID != Guid.Empty)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.Supplier_ID == obj.Supplier_ID
                                        select a;
                    }
                    if (!string.IsNullOrWhiteSpace(obj.SystemCountryName))
                    {
                        var distCountryMapping = (from a in context.m_CountryMapping select new { a.Country_Id, a.CountryName }).Distinct();
                        prodMapSearch = from a in prodMapSearch
                                        join mas in distCountryMapping on a.SupplierCountryName.ToLower().Trim() equals mas.CountryName.ToLower().Trim()
                                        join mct in context.m_CountryMaster on mas.Country_Id equals mct.Country_Id
                                        where mct.Name == obj.SystemCountryName
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.SystemCityName))
                    {
                        var distCityMapping = (from a in context.m_CityMapping select new { a.City_Id, a.CityName }).Distinct();
                        prodMapSearch = from a in prodMapSearch
                                        join mas in distCityMapping on a.SupplierCityName.ToLower().Trim() equals mas.CityName.ToLower().Trim()
                                        join mct in context.m_CityMaster on mas.City_Id equals mct.City_Id
                                        where mct.Name == obj.SystemCityName
                                        select a;
                    }
                    //if (!string.IsNullOrWhiteSpace(obj.SearchFor) &&  obj.SearchFor.ToLower() == "activitymapping")
                    //{
                    //    //Get Supplier name 
                    //    var result = GetActivitySupplierProductMappingSearchForDDL(new DC_Acitivity_SupplierProductMapping_Search_RQ() { SupplierCountryName = obj.SupplierCountryName, SupplierCityName = obj.SupplierCityName});
                    //    prodMapSearch = from a in prodMapSearch
                    //                    join lstsupplier in result on a.Supplier_ID equals lstsupplier.Supplier_ID
                    //                    select a;
                    //}

                    if (!string.IsNullOrWhiteSpace(obj.MappingStatus))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.MappingStatus.ToLower().Trim() == obj.MappingStatus.ToLower().Trim()
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.SupplierName))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.SupplierName == obj.SupplierName
                                        select a;
                    }


                    if (!string.IsNullOrWhiteSpace(obj.SupplierCountryName))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.SupplierCountryName.ToLower().Trim() == obj.SupplierCountryName.ToLower().Trim()
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.SupplierCityName))
                    {

                        prodMapSearch = from a in prodMapSearch
                                        where a.SupplierCityName.ToLower().Trim() == obj.SupplierCityName.ToLower().Trim()
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.SupplierProductName))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.SupplierProductName.Contains(obj.SupplierProductName)
                                        select a;
                    }
                    //Bykeyword
                    if (!string.IsNullOrWhiteSpace(obj.KeyWord))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.SupplierProductName.Contains(obj.KeyWord) || a.Inclusions.Contains(obj.KeyWord) || a.Exclusions.Contains(obj.KeyWord)
                                        select a;
                    }

                    if (obj.StatusExcept != null)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.MappingStatus.ToUpper() != obj.StatusExcept.ToUpper()
                                        select a;
                    }

                    int total;

                    total = prodMapSearch.Count();

                    var skip = obj.PageSize * obj.PageNo;

                    var prodMap = (from a in prodMapSearch
                                   join act in context.Activities on a.Activity_ID equals act.Activity_Id into ja
                                   from jda in ja.DefaultIfEmpty()
                                   orderby a.SupplierName
                                   select new DataContracts.Mapping.DC_Acitivity_SupplierProductMapping
                                   {
                                       Activity_ID = a.Activity_ID,
                                       Supplier_ID = a.Supplier_ID,
                                       ActivitySupplierProductMapping_Id = a.ActivitySupplierProductMapping_Id,
                                       SupplierCode = a.SupplierCode,
                                       SupplierName = a.SupplierName,
                                       SuplierProductCode = a.SuplierProductCode,
                                       SupplierProductType = a.SupplierProductType,
                                       SupplierType = a.SupplierType,
                                       SupplierLocationId = a.SupplierLocationId,
                                       SupplierLocationName = a.SupplierLocationName,
                                       SupplierCountryName = a.SupplierCountryName,
                                       SupplierCityName = a.SupplierCityName,
                                       SupplierCountryCode = a.SupplierCountryCode,
                                       SupplierCityCode = a.SupplierCityCode,
                                       SupplierStateName = a.SupplierStateName,
                                       SupplierStateCode = a.SupplierStateCode,
                                       SupplierCityIATACode = a.SupplierCityIATACode,
                                       Duration = a.Duration,
                                       SupplierProductName = a.SupplierProductName,
                                       SupplierDataLangaugeCode = a.SupplierDataLangaugeCode,
                                       Introduction = a.Introduction,
                                       Conditions = a.Conditions,
                                       Inclusions = a.Inclusions,
                                       Exclusions = a.Exclusions,
                                       AdditionalInformation = a.AdditionalInformation,
                                       DeparturePoint = a.DeparturePoint,
                                       TicketingDetails = a.TicketingDetails,
                                       Currency = a.Currency,
                                       DepartureTime = a.DepartureTime,
                                       DepartureDate = a.DepartureDate,
                                       DateFrom = a.DateFrom,
                                       DateTo = a.DateTo,
                                       BlockOutDateFrom = a.BlockOutDateFrom,
                                       BlockOutDateTo = a.BlockOutDateTo,
                                       OptionTitle = a.OptionTitle,
                                       OptionCode = a.OptionCode,
                                       OptionDescription = a.OptionDescription,
                                       TourActivityLangauageCode = a.TourActivityLangauageCode,
                                       ProductDescription = a.ProductDescription,
                                       TourActivityLanguage = a.TourActivityLanguage,
                                       ImgURL = a.ImgURL,
                                       ProductValidFor = a.ProductValidFor,
                                       Address = a.Address,
                                       Latitude = a.Latitude,
                                       Longitude = a.Longitude,
                                       DayPattern = a.DayPattern,
                                       Theme = a.Theme,
                                       Distance = a.Distance,
                                       SupplierTourType = a.SupplierTourType,
                                       MappingStatus = a.MappingStatus,
                                       Create_Date = a.Create_Date,
                                       Edit_Date = a.Edit_Date,
                                       Create_User = a.Create_User,
                                       Edit_User = a.Edit_User,
                                       MapID = a.MapID,
                                       ourRef = jda.CommonProductID,
                                       SystemCountryName = jda.Country,
                                       SystemCityName = jda.City,
                                       CKIS_Master = jda.Product_Name,
                                       TotalCount = total
                                   }).Skip(skip ?? 0).Take(obj.PageSize ?? total);

                    var prodMapList = prodMap.ToList();

                    if (!string.IsNullOrWhiteSpace(obj.MappingStatus))
                    {
                        foreach (var item in prodMapList)
                        {
                            if (obj.MappingStatus.ToUpper().Trim() == "UNMAPPED" && !string.IsNullOrWhiteSpace(item.SupplierProductName))
                            {
                                if (item.SupplierCityName != null && item.SupplierCountryName != null)
                                {
                                    var _supplierProductName = item.SupplierProductName.Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("activity", "");
                                    var searchActivity = context.Activities.Where(a => a.Country.ToUpper().Trim() == item.SupplierCountryName.ToUpper().Trim() && a.City.ToUpper().Trim() == item.SupplierCityName.ToUpper().Trim() && a.Product_Name.Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("activity", "").ToUpper().Trim() == _supplierProductName.ToUpper().Trim()).FirstOrDefault();
                                    if (searchActivity == null)
                                    {
                                        searchActivity = context.Activities.Where(a => a.Country.ToUpper().Trim() == item.SupplierCountryName.ToUpper().Trim() && a.City.ToUpper().Trim() == item.SupplierCityName.ToUpper().Trim() && a.Product_Name.Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("activity", "").ToUpper().Trim().Contains(_supplierProductName.ToUpper().Trim())).FirstOrDefault();
                                    }

                                    if (searchActivity != null)
                                    {
                                        item.CKIS_Master = searchActivity.Product_Name;
                                        item.Activity_ID = searchActivity.Activity_Id;
                                    }
                                }
                            }
                        }
                    }
                    return prodMapList;

                }
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching accomodation product mapping by supplier search", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public List<DataContracts.Mapping.DC_Acitivity_SupplierProductMapping> GetActivitySupplierProductMappingSearchForMapping(DataContracts.Mapping.DC_Acitivity_SupplierProductMapping_Search_RQ obj)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var prodMapSearch = from a in context.Activity_SupplierProductMapping select a; //where a.Accommodation_Id == null 

                    if (obj.ActivitySupplierProductMappling_Id != Guid.Empty)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.ActivitySupplierProductMapping_Id == obj.ActivitySupplierProductMappling_Id
                                        select a;
                    }
                    if (obj.Activity_ID != Guid.Empty)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.Activity_ID == obj.Activity_ID
                                        select a;
                    }
                    if (obj.Supplier_ID != Guid.Empty)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.Supplier_ID == obj.Supplier_ID
                                        select a;
                    }
                    if (!string.IsNullOrWhiteSpace(obj.SystemCountryName))
                    {
                        var distCountryMapping = (from a in context.m_CountryMapping select new { a.Country_Id, a.CountryName }).Distinct();
                        prodMapSearch = from a in prodMapSearch
                                        join mas in distCountryMapping on a.SupplierCountryName.ToLower().Trim() equals mas.CountryName.ToLower().Trim()
                                        join mct in context.m_CountryMaster on mas.Country_Id equals mct.Country_Id
                                        where mct.Name == obj.SystemCountryName
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.SystemCityName))
                    {
                        var distCityMapping = (from a in context.m_CityMapping select new { a.City_Id, a.CityName }).Distinct();
                        prodMapSearch = from a in prodMapSearch
                                        join mas in distCityMapping on a.SupplierCityName.ToLower().Trim() equals mas.CityName.ToLower().Trim()
                                        join mct in context.m_CityMaster on mas.City_Id equals mct.City_Id
                                        where mct.Name == obj.SystemCityName
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.MappingStatus))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.MappingStatus.ToLower().Trim() == obj.MappingStatus.ToLower().Trim()
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.SupplierName))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.SupplierName == obj.SupplierName
                                        select a;
                    }


                    if (!string.IsNullOrWhiteSpace(obj.SupplierCountryName))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.SupplierCountryName.ToLower().Trim() == obj.SupplierCountryName.ToLower().Trim()
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.SupplierCityName))
                    {

                        prodMapSearch = from a in prodMapSearch
                                        where a.SupplierCityName.ToLower().Trim() == obj.SupplierCityName.ToLower().Trim()
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.SupplierProductName))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.SupplierProductName.Contains(obj.SupplierProductName)
                                        select a;
                    }
                    //Bykeyword
                    if (!string.IsNullOrWhiteSpace(obj.KeyWord))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.SupplierProductName.Contains(obj.KeyWord) || a.Inclusions.Contains(obj.KeyWord) || a.Exclusions.Contains(obj.KeyWord)
                                        select a;
                    }

                    if (obj.StatusExcept != null)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.MappingStatus.ToUpper() != obj.StatusExcept.ToUpper()
                                        select a;
                    }
                    prodMapSearch = from a in prodMapSearch
                                    join sup in context.Supplier on a.Supplier_ID equals sup.Supplier_Id
                                    select a;

                    int total;

                    total = prodMapSearch.Count();

                    var skip = obj.PageSize * obj.PageNo;

                    var prodMap = (from a in prodMapSearch
                                   join act in context.Activities on a.Activity_ID equals act.Activity_Id into ja
                                   from jda in ja.DefaultIfEmpty()
                                   orderby a.SupplierName
                                   select new DataContracts.Mapping.DC_Acitivity_SupplierProductMapping
                                   {
                                       Activity_ID = a.Activity_ID,
                                       Supplier_ID = a.Supplier_ID,
                                       ActivitySupplierProductMapping_Id = a.ActivitySupplierProductMapping_Id,
                                       SupplierCode = a.SupplierCode,
                                       SupplierName = a.SupplierName,
                                       SuplierProductCode = a.SuplierProductCode,
                                       SupplierProductType = a.SupplierProductType,
                                       SupplierType = a.SupplierType,
                                       SupplierLocationId = a.SupplierLocationId,
                                       SupplierLocationName = a.SupplierLocationName,
                                       SupplierCountryName = a.SupplierCountryName,
                                       SupplierCityName = a.SupplierCityName,
                                       SupplierCountryCode = a.SupplierCountryCode,
                                       SupplierCityCode = a.SupplierCityCode,
                                       SupplierStateName = a.SupplierStateName,
                                       SupplierStateCode = a.SupplierStateCode,
                                       SupplierCityIATACode = a.SupplierCityIATACode,
                                       Duration = a.Duration,
                                       SupplierProductName = a.SupplierProductName,
                                       SupplierDataLangaugeCode = a.SupplierDataLangaugeCode,
                                       Introduction = a.Introduction,
                                       Conditions = a.Conditions,
                                       Inclusions = a.Inclusions,
                                       Exclusions = a.Exclusions,
                                       AdditionalInformation = a.AdditionalInformation,
                                       DeparturePoint = a.DeparturePoint,
                                       TicketingDetails = a.TicketingDetails,
                                       Currency = a.Currency,
                                       DepartureTime = a.DepartureTime,
                                       DepartureDate = a.DepartureDate,
                                       DateFrom = a.DateFrom,
                                       DateTo = a.DateTo,
                                       BlockOutDateFrom = a.BlockOutDateFrom,
                                       BlockOutDateTo = a.BlockOutDateTo,
                                       OptionTitle = a.OptionTitle,
                                       OptionCode = a.OptionCode,
                                       OptionDescription = a.OptionDescription,
                                       TourActivityLangauageCode = a.TourActivityLangauageCode,
                                       ProductDescription = a.ProductDescription,
                                       TourActivityLanguage = a.TourActivityLanguage,
                                       ImgURL = a.ImgURL,
                                       ProductValidFor = a.ProductValidFor,
                                       Address = a.Address,
                                       Latitude = a.Latitude,
                                       Longitude = a.Longitude,
                                       DayPattern = a.DayPattern,
                                       Theme = a.Theme,
                                       Distance = a.Distance,
                                       SupplierTourType = a.SupplierTourType,
                                       MappingStatus = a.MappingStatus,
                                       Create_Date = a.Create_Date,
                                       Edit_Date = a.Edit_Date,
                                       Create_User = a.Create_User,
                                       Edit_User = a.Edit_User,
                                       MapID = a.MapID,
                                       ourRef = jda.CommonProductID,
                                       SystemCountryName = jda.Country,
                                       SystemCityName = jda.City,
                                       CKIS_Master = jda.Product_Name,
                                       TotalCount = total
                                   }).Skip(skip ?? 0).Take(obj.PageSize ?? total);

                    var prodMapList = prodMap.ToList();

                    if (!string.IsNullOrWhiteSpace(obj.MappingStatus))
                    {
                        foreach (var item in prodMapList)
                        {
                            if (obj.MappingStatus.ToUpper().Trim() == "UNMAPPED" && !string.IsNullOrWhiteSpace(item.SupplierProductName))
                            {
                                if (item.SupplierCityName != null && item.SupplierCountryName != null)
                                {
                                    var _supplierProductName = item.SupplierProductName.Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("activity", "");
                                    var searchActivity = context.Activities.Where(a => a.Country.ToUpper().Trim() == item.SupplierCountryName.ToUpper().Trim() && a.City.ToUpper().Trim() == item.SupplierCityName.ToUpper().Trim() && a.Product_Name.Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("activity", "").ToUpper().Trim() == _supplierProductName.ToUpper().Trim()).FirstOrDefault();
                                    if (searchActivity == null)
                                    {
                                        searchActivity = context.Activities.Where(a => a.Country.ToUpper().Trim() == item.SupplierCountryName.ToUpper().Trim() && a.City.ToUpper().Trim() == item.SupplierCityName.ToUpper().Trim() && a.Product_Name.Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("activity", "").ToUpper().Trim().Contains(_supplierProductName.ToUpper().Trim())).FirstOrDefault();
                                    }

                                    if (searchActivity != null)
                                    {
                                        item.CKIS_Master = searchActivity.Product_Name;
                                        item.Activity_ID = searchActivity.Activity_Id;
                                    }
                                }
                            }
                        }
                    }
                    return prodMapList;

                }
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching accomodation product mapping by supplier search", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool IsMappedWithSupplier(Guid masterActivityID, Guid supplierID)
        {
            bool isMapped = false;
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (supplierID != Guid.Empty && masterActivityID != Guid.Empty)
                    {
                        var result = context.Activity_SupplierProductMapping.Where(x => x.Supplier_ID == supplierID && x.MappingStatus.ToLower() == "mapped" && x.Activity_ID == masterActivityID).Select(x => x.ActivitySupplierProductMapping_Id).ToList();
                        if (result != null && result.Count > 0)
                        {
                            isMapped = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return isMapped;
        }

        public List<DataContracts.Mapping.DC_Acitivity_SupplierProductMappingForDDL> GetActivitySupplierProductMappingSearchForDDL(DataContracts.Mapping.DC_Acitivity_SupplierProductMapping_Search_RQ obj)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {

                    var prodMapSearch = from a in context.Activity_SupplierProductMapping select a;

                    if (!string.IsNullOrWhiteSpace(obj.SystemCountryName))
                    {
                        var distCountryMapping = (from a in context.m_CountryMapping select new { a.Country_Id, a.CountryName, a.Supplier_Id }).Distinct();
                        prodMapSearch = from a in prodMapSearch
                                        join mas in distCountryMapping on a.SupplierCountryName.ToLower().Trim() equals mas.CountryName.ToLower().Trim()
                                        join mct in context.m_CountryMaster on mas.Country_Id equals mct.Country_Id
                                        join sup in context.Supplier on a.Supplier_ID equals sup.Supplier_Id
                                        where mct.Name == obj.SystemCountryName
                                        select a;
                    }
                    var result = prodMapSearch.GroupBy(x => x.Supplier_ID).Select(x => x.FirstOrDefault());

                    int total;

                    total = result.Count();

                    var skip = obj.PageSize * obj.PageNo;

                    var prodMapList = result.ToList();

                    var prodMap = (from a in prodMapList
                                   orderby a.SupplierName
                                   select new DataContracts.Mapping.DC_Acitivity_SupplierProductMappingForDDL
                                   {
                                       Supplier_ID = a.Supplier_ID,
                                       SupplierName = a.SupplierName,
                                   }).Skip(skip ?? 0).Take(obj.PageSize ?? total);

                    return prodMap.ToList();

                }
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching accomodation product mapping by supplier search", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public List<DataContracts.Mapping.DC_Acitivity_SupplierProductMapping> GetActivitySupplierProductMapping(int PageNo, int PageSize, Guid Activity_Id, string Status)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var prodMapSearch = from a in context.Activity_SupplierProductMapping select a;

                    if (Activity_Id != Guid.Empty)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.Activity_ID == Activity_Id
                                        select a;
                    }

                    if (Status.Trim().ToUpper() != "ALL")
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where (a.MappingStatus ?? "UNMAPPED") == Status
                                        select a;
                    }

                    int total;

                    total = prodMapSearch.Count();

                    var skip = PageSize * PageNo;

                    var canPage = skip < total;

                    //if (!canPage)
                    //    return null;
                    System.Linq.IQueryable<DataContracts.Mapping.DC_Acitivity_SupplierProductMapping> prodMapList;

                    if (Activity_Id != Guid.Empty)
                    {
                        prodMapList = (from a in prodMapSearch
                                       join act in context.Activities on a.Activity_ID equals act.Activity_Id
                                       where act.Activity_Id == Activity_Id
                                       orderby a.SupplierName
                                       select new DataContracts.Mapping.DC_Acitivity_SupplierProductMapping
                                       {
                                           Activity_ID = a.Activity_ID,
                                           Supplier_ID = a.Supplier_ID,
                                           ActivitySupplierProductMapping_Id = a.ActivitySupplierProductMapping_Id,
                                           SupplierCode = a.SupplierCode,
                                           SupplierName = a.SupplierName,
                                           SuplierProductCode = a.SuplierProductCode,
                                           SupplierProductType = a.SupplierProductType,
                                           SupplierType = a.SupplierType,
                                           SupplierLocationId = a.SupplierLocationId,
                                           SupplierLocationName = a.SupplierLocationName,
                                           SupplierCountryName = a.SupplierCountryName,
                                           SupplierCityName = a.SupplierCityName,
                                           SupplierCountryCode = a.SupplierCountryCode,
                                           SupplierCityCode = a.SupplierCityCode,
                                           SupplierStateName = a.SupplierStateName,
                                           SupplierStateCode = a.SupplierStateCode,
                                           SupplierCityIATACode = a.SupplierCityIATACode,
                                           Duration = a.Duration,
                                           SupplierProductName = a.SupplierProductName,
                                           SupplierDataLangaugeCode = a.SupplierDataLangaugeCode,
                                           Introduction = a.Introduction,
                                           Conditions = a.Conditions,
                                           Inclusions = a.Inclusions,
                                           Exclusions = a.Exclusions,
                                           AdditionalInformation = a.AdditionalInformation,
                                           DeparturePoint = a.DeparturePoint,
                                           TicketingDetails = a.TicketingDetails,
                                           Currency = a.Currency,
                                           DepartureTime = a.DepartureTime,
                                           DepartureDate = a.DepartureDate,
                                           DateFrom = a.DateFrom,
                                           DateTo = a.DateTo,
                                           BlockOutDateFrom = a.BlockOutDateFrom,
                                           BlockOutDateTo = a.BlockOutDateTo,
                                           OptionTitle = a.OptionTitle,
                                           OptionCode = a.OptionCode,
                                           OptionDescription = a.OptionDescription,
                                           TourActivityLangauageCode = a.TourActivityLangauageCode,
                                           ProductDescription = a.ProductDescription,
                                           TourActivityLanguage = a.TourActivityLanguage,
                                           ImgURL = a.ImgURL,
                                           ProductValidFor = a.ProductValidFor,
                                           Address = a.Address,
                                           Latitude = a.Latitude,
                                           Longitude = a.Longitude,
                                           DayPattern = a.DayPattern,
                                           Theme = a.Theme,
                                           Distance = a.Distance,
                                           SupplierTourType = a.SupplierTourType,
                                           MappingStatus = a.MappingStatus,
                                           Create_Date = a.Create_Date,
                                           Edit_Date = a.Edit_Date,
                                           Create_User = a.Create_User,
                                           Edit_User = a.Edit_User,
                                           MapID = a.MapID,
                                           SystemCountryName = act.Country,
                                           SystemCityName = act.City,
                                           ourRef = act.CommonProductID,
                                           CKIS_Master = act.Product_Name,
                                           TotalCount = total
                                       }).Skip(skip).Take(PageSize);
                    }
                    else
                    {

                        prodMapList = (from a in prodMapSearch
                                       orderby a.SupplierName
                                       select new DataContracts.Mapping.DC_Acitivity_SupplierProductMapping
                                       {
                                           Activity_ID = a.Activity_ID,
                                           Supplier_ID = a.Supplier_ID,
                                           ActivitySupplierProductMapping_Id = a.ActivitySupplierProductMapping_Id,
                                           SupplierCode = a.SupplierCode,
                                           SupplierName = a.SupplierName,
                                           SuplierProductCode = a.SuplierProductCode,
                                           SupplierProductType = a.SupplierProductType,
                                           SupplierType = a.SupplierType,
                                           SupplierLocationId = a.SupplierLocationId,
                                           SupplierLocationName = a.SupplierLocationName,
                                           SupplierCountryName = a.SupplierCountryName,
                                           SupplierCityName = a.SupplierCityName,
                                           SupplierCountryCode = a.SupplierCountryCode,
                                           SupplierCityCode = a.SupplierCityCode,
                                           SupplierStateName = a.SupplierStateName,
                                           SupplierStateCode = a.SupplierStateCode,
                                           SupplierCityIATACode = a.SupplierCityIATACode,
                                           Duration = a.Duration,
                                           SupplierProductName = a.SupplierProductName,
                                           SupplierDataLangaugeCode = a.SupplierDataLangaugeCode,
                                           Introduction = a.Introduction,
                                           Conditions = a.Conditions,
                                           Inclusions = a.Inclusions,
                                           Exclusions = a.Exclusions,
                                           AdditionalInformation = a.AdditionalInformation,
                                           DeparturePoint = a.DeparturePoint,
                                           TicketingDetails = a.TicketingDetails,
                                           Currency = a.Currency,
                                           DepartureTime = a.DepartureTime,
                                           DepartureDate = a.DepartureDate,
                                           DateFrom = a.DateFrom,
                                           DateTo = a.DateTo,
                                           BlockOutDateFrom = a.BlockOutDateFrom,
                                           BlockOutDateTo = a.BlockOutDateTo,
                                           OptionTitle = a.OptionTitle,
                                           OptionCode = a.OptionCode,
                                           OptionDescription = a.OptionDescription,
                                           TourActivityLangauageCode = a.TourActivityLangauageCode,
                                           ProductDescription = a.ProductDescription,
                                           TourActivityLanguage = a.TourActivityLanguage,
                                           ImgURL = a.ImgURL,
                                           ProductValidFor = a.ProductValidFor,
                                           Address = a.Address,
                                           Latitude = a.Latitude,
                                           Longitude = a.Longitude,
                                           DayPattern = a.DayPattern,
                                           Theme = a.Theme,
                                           Distance = a.Distance,
                                           SupplierTourType = a.SupplierTourType,
                                           MappingStatus = a.MappingStatus,
                                           Create_Date = a.Create_Date,
                                           Edit_Date = a.Edit_Date,
                                           Create_User = a.Create_User,
                                           Edit_User = a.Edit_User,
                                           MapID = a.MapID,
                                           TotalCount = total
                                       }).Skip(skip).Take(PageSize);
                    }

                    return prodMapList.ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching accomodation product mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public DataContracts.DC_Message UpdateActivitySupplierProductMapping(List<DataContracts.Mapping.DC_Acitivity_SupplierProductMapping> obj)
        {
            DataContracts.DC_Message _msg = new DataContracts.DC_Message();
            string strStatus = string.Empty;
            foreach (var PM in obj)
            {
                if (PM.MappingStatus != "UNMAPPED")
                {
                    if (PM.ActivitySupplierProductMapping_Id == null || PM.Activity_ID == null) //|| PM.Supplier_Id == null
                    {
                        continue;
                    }
                }

                try
                {
                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        var search = (from a in context.Activity_SupplierProductMapping
                                      where a.ActivitySupplierProductMapping_Id == PM.ActivitySupplierProductMapping_Id
                                      select a).FirstOrDefault();
                        if (search != null)
                        {
                            strStatus = PM.MappingStatus.ToLower();
                            search.Activity_ID = PM.Activity_ID;
                            search.MappingStatus = PM.MappingStatus;
                            search.Edit_Date = PM.Edit_Date;
                            search.Edit_User = PM.Edit_User;

                            context.SaveChanges();
                        }


                    }
                }
                catch
                {
                    //_msg.StatusMessage = "Something went wrong. Please contact system administrator!";
                    //_msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Success;
                    throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating accomodation product mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
                }
            }
            _msg.StatusMessage = "Activity " + strStatus + " successfully";
            _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Success;
            return _msg;
        }
        #endregion

        #region hotel report
        public List<DataContracts.Mapping.DC_newHotelsReport> getNewHotelsAddedReport(DataContracts.Mapping.DC_RollOFParams parm)
        {
            List<DataContracts.Mapping.DC_newHotelsReport> objLst = new List<DC_newHotelsReport>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    DateTime fd = Convert.ToDateTime(parm.Fromdate);
                    DateTime td = Convert.ToDateTime(parm.ToDate);
                    var search = (from t in context.Accommodation
                                  where (t.Create_Date >= fd && t.Create_Date <= td) && (t.InsertFrom == true)
                                  select new
                                  {
                                      HotelID = t.CompanyHotelID,
                                      HotelName = t.HotelName,
                                      country = t.country,
                                      city = t.city,
                                      createdate = t.Create_Date,
                                      createby = t.Create_User

                                  }).ToList();
                    foreach (var item in search)
                    {

                        DC_newHotelsReport obj = new DC_newHotelsReport();
                        obj.Hotelid = item.HotelID ?? 0;
                        obj.Hotelname = item.HotelName;
                        obj.Country = item.country;
                        obj.City = item.city;

                        if (item.createdate != null)
                        {
                            obj.Createdate = item.createdate.Value.ToString("dd/MM/yyyy");
                        }
                        else { obj.Createdate = ""; }
                        obj.Createdby = item.createby;
                        objLst.Add(obj);
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return objLst;
        }
        #endregion

        #region velocity Dashboard
        public List<DataContracts.Mapping.DC_VelocityMappingStats> GetVelocityDashboard(DataContracts.Mapping.DC_VelocityReport parm)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    List<DC_AllSupplierMappedData> searchResult = new List<DC_AllSupplierMappedData>();

                    context.Database.CommandTimeout = 0;
                    List<DataContracts.Mapping.DC_VelocityMappingStats> returnObj = new List<DataContracts.Mapping.DC_VelocityMappingStats>();
                    DataContracts.Mapping.DC_VelocityMappingStats newmapstats = new DataContracts.Mapping.DC_VelocityMappingStats();
                    List<DataContracts.Mapping.DC_VelocityMappingStatsFor> newmapstatsforList = new List<DataContracts.Mapping.DC_VelocityMappingStatsFor>();


                    newmapstats.SupplierId = parm.SupplierID;

                    var unmapData = (from s in context.Dashboard_MappingStat
                                     where s.Status == "UNMAPPED" || s.Status == "REVIEW"
                                     select s
                                     ).ToList();

                    var MappedStats = (from p in context.vwUserwisemappedStats
                                       where p.SupplierName != null && (p.EditDate >= parm.Fromdate && p.EditDate <= parm.ToDate)
                                       group p by new { p.supplier_id, p.Username, p.MappinFor, p.SupplierName } into g
                                       select new DC_AllSupplierMappedData
                                       {
                                           supplierid = g.Key.supplier_id,
                                           SupplierName = g.Key.SupplierName,
                                           Username = g.Key.Username,
                                           totalcount = (g.Sum(x => x.totalcount) ?? 0),
                                           MappinFor = g.Key.MappinFor
                                       }).ToList();

                    if (parm.SupplierID != Guid.Empty)
                    {
                        MappedStats = MappedStats.Where(w => w.supplierid == parm.SupplierID).ToList();

                        unmapData = (from s in unmapData
                                     where s.Supplier_Id == parm.SupplierID
                                     select s).ToList();

                        newmapstats.SupplierName = (from s in MappedStats where s.supplierid == parm.SupplierID select s.SupplierName).FirstOrDefault();
                    }
                    else if (parm.SupplierID == Guid.Empty)
                    {
                        var suppliermaster = (from s in context.Supplier
                                              where (s.StatusCode.ToUpper().Trim() == "ACTIVE") && (parm.Priority > 0 ? s.Priority == parm.Priority : s.Priority == s.Priority)
                                              select s).ToList();

                        MappedStats = (from m in MappedStats
                                       join s in suppliermaster on m.supplierid equals s.Supplier_Id
                                       group m by new { m.MappinFor, m.Username } into g
                                       select new DC_AllSupplierMappedData
                                       {
                                           Username = g.Key.Username,
                                           SupplierName = "ALL",
                                           supplierid = Guid.Empty,
                                           MappinFor = g.Key.MappinFor,
                                           totalcount = g.Sum(x => x.totalcount) ?? 0
                                       }).ToList();

                        newmapstats.SupplierName = "ALL";
                    }

                    var MapForList = (from s in MappedStats select s.MappinFor).ToList().Distinct();

                    foreach (var mapfor in MapForList)
                    {
                        DataContracts.Mapping.DC_VelocityMappingStatsFor newmapstatsfor = new DataContracts.Mapping.DC_VelocityMappingStatsFor();

                        newmapstatsfor.MappingFor = mapfor;
                        newmapstatsfor.Unmappeddata = unmapData.Where(w => w.MappingFor == mapfor).Sum(s => s.TotalCount) ?? 0;

                        int totalmappeddata = MappedStats.Where(w => w.MappinFor == mapfor).Sum(s => s.totalcount) ?? 0;

                        if (newmapstatsfor.Unmappeddata > 0 && totalmappeddata > 0)
                        {
                            double days;
                            if (parm.Fromdate == parm.ToDate)
                                days = 1;
                            else
                                days = (Convert.ToDateTime(parm.ToDate) - Convert.ToDateTime(parm.Fromdate)).TotalDays;
                            var perday = (totalmappeddata / days);
                            newmapstatsfor.Estimate = Convert.ToInt32(newmapstatsfor.Unmappeddata / perday);
                        }
                        else
                        {
                            newmapstatsfor.Estimate = 0;
                        }

                        if (totalmappeddata > 0)
                        {
                            newmapstatsfor.MappingData = (from s in MappedStats
                                                          where s.MappinFor == mapfor
                                                          orderby s.Username
                                                          group s by new { s.Username } into g
                                                          select new DataContracts.Mapping.DC_VelocityMappingdata
                                                          { Username = g.Key.Username, Totalcount = g.Sum(x => x.totalcount) ?? 0 }).ToList();
                        }

                        newmapstatsforList.Add(newmapstatsfor);
                    }
                    newmapstats.MappingStatsFor = newmapstatsforList;
                    returnObj.Add(newmapstats);
                    return returnObj;
                }

            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching  Velocity mapping statistics", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region GetHotelListByCityCode
        public List<DC_HotelListByCityCode> GetHotelListByCityCode(DataContracts.Mapping.DC_HotelListByCityCode_RQ param)
        {
            try
            {
                List<DC_HotelListByCityCode> _lstresult = new List<DC_HotelListByCityCode>();

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (!string.IsNullOrWhiteSpace(param.CityMapping_Id))
                    {
                        string strGoFor = string.IsNullOrWhiteSpace(param.GoFor) ? string.Empty : param.GoFor.Trim().ToUpper();
                        IQueryable<Accommodation_ProductMapping> query;
                        //context.Accommodation_ProductMapping.Where(w => false).Select(s => s).AsQueryable();

                        Guid _CityMapId = Guid.Parse(param.CityMapping_Id);
                        var selectedcity = context.m_CityMapping.Find(_CityMapId);
                        if (selectedcity != null)
                        {
                            var supplierid = selectedcity.Supplier_Id;
                            var suppliercitycode = (selectedcity.CityCode ?? string.Empty).Trim().ToLower();
                            var suppliercityname = (selectedcity.CityName ?? string.Empty).Trim().ToLower();
                            var supplierCountryname = (selectedcity.CountryName ?? string.Empty).Trim().ToLower();
                            var supplierCountryCode = (selectedcity.CountryCode ?? string.Empty).Trim().ToLower();

                            query = context.Accommodation_ProductMapping.Where(w => w.Supplier_Id == supplierid).Select(s => s).AsQueryable();
                            if (strGoFor == "CITYCODE")
                            {
                                if (!string.IsNullOrWhiteSpace(suppliercitycode))
                                {
                                    //query = context.Accommodation_ProductMapping.Where(w => w.CityCode.Trim().ToLower() == suppliercitycode && w.Supplier_Id == supplierid).Select(s => s).AsQueryable();
                                    query = query.Where(w => w.CityCode.Trim().ToLower() == suppliercitycode).Select(s => s).AsQueryable();
                                }

                            }
                            else if (strGoFor == "CITYNAME")
                            {
                                if (!string.IsNullOrWhiteSpace(suppliercityname))
                                {
                                    //query = context.Accommodation_ProductMapping.Where(w => w.CityName.Trim().ToLower() == suppliercityname && w.Supplier_Id == supplierid).Select(s => s).AsQueryable();
                                    query = query.Where(w => w.CityName.Trim().ToLower() == suppliercityname).Select(s => s).AsQueryable();
                                    if (!string.IsNullOrWhiteSpace(supplierCountryCode))
                                    {
                                        query = query.Where(w => w.CountryCode.Trim().ToLower() == supplierCountryCode).Select(s => s);
                                    }
                                    else if (!string.IsNullOrWhiteSpace(supplierCountryname))
                                    {
                                        query = query.Where(w => w.CountryName.Trim().ToLower() == supplierCountryname).Select(s => s);
                                    }
                                }

                            }
                            else if (strGoFor == string.Empty)
                            {
                                if (!string.IsNullOrWhiteSpace(suppliercitycode))
                                {
                                    query = query.Where(w => w.CityCode.Trim().ToLower() == suppliercitycode).Select(s => s).AsQueryable();
                                    //&& w.Supplier_Id == supplierid

                                }
                                else if (!string.IsNullOrWhiteSpace(suppliercityname))
                                {
                                    query = query.Where(w => w.CityName.Trim().ToLower() == suppliercityname).Select(s => s).AsQueryable();
                                    //&& w.Supplier_Id == supplierid
                                    if (!string.IsNullOrWhiteSpace(supplierCountryCode))
                                    {
                                        query = query.Where(w => w.CountryCode.Trim().ToLower() == supplierCountryCode).Select(s => s);
                                    }
                                    else if (!string.IsNullOrWhiteSpace(supplierCountryname))
                                    {
                                        query = query.Where(w => w.CountryName.Trim().ToLower() == supplierCountryname).Select(s => s);
                                    }
                                }
                            }


                            int total = query.Count();

                            //Supplier Country Filter check
                            /*if (total > 0)
                            {
                                if (!string.IsNullOrWhiteSpace(supplierCountryCode))
                                {
                                    if (query.Where(w => w.CountryCode.Trim().ToLower() == supplierCountryCode).Select(s => s).Count() > 0)
                                    {
                                        query = query.Where(w => w.CountryCode.Trim().ToLower() == supplierCountryCode).Select(s => s);
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrWhiteSpace(supplierCountryname))
                                        {
                                            if (query.Where(w => w.CountryName.Trim().ToLower() == supplierCountryname).Select(s => s).Count() > 0)
                                            {
                                                query = query.Where(w => w.CountryName.Trim().ToLower() == supplierCountryname).Select(s => s);
                                            }
                                            else
                                            {
                                                return _lstresult;
                                            }
                                        }
                                        else
                                        {
                                            return _lstresult;
                                        }
                                    }
                                }
                                else if (!string.IsNullOrWhiteSpace(supplierCountryname))
                                {
                                    if (query.Where(w => w.CountryName.Trim().ToLower() == supplierCountryname).Select(s => s).Count() > 0)
                                    {
                                        query = query.Where(w => w.CountryName.Trim().ToLower() == supplierCountryname).Select(s => s);
                                    }
                                    else
                                    {
                                        return _lstresult;
                                    }
                                }
                                else
                                {
                                    return _lstresult;
                                }
                            }

                            total = query.Count();*/

                            if (total > 0)
                            {
                                var skip = param.PageSize * param.PageNo;

                                _lstresult = (from CM in query
                                              orderby CM.address_tx.Length descending
                                              select new DC_HotelListByCityCode
                                              {
                                                  HotelName = CM.ProductName,
                                                  Address = (CM.address ?? string.Empty)
                                                            + "," + (CM.CityName ?? string.Empty) + "(" + (CM.CityCode ?? string.Empty) + ")"
                                                            + "," + (CM.StateName ?? string.Empty) + "(" + (CM.StateCode ?? string.Empty) + ")"
                                                            + "," + (CM.CountryName ?? string.Empty) + "(" + (CM.CountryCode ?? string.Empty) + ")",
                                                  CityMapping_Id = param.CityMapping_Id,
                                                  GoFor = strGoFor,
                                                  TotalRecords = total
                                              }).Skip(skip).Take(param.PageSize).ToList();

                                //var res = (from CM in query
                                //           orderby CM.Street.Length descending
                                //           select CM
                                //       ).Skip(skip).Take(param.PageSize).ToList();

                                //foreach (var item in res)
                                //{
                                //    StringBuilder sb = new StringBuilder();
                                //    sb.Append(item.Street);
                                //    sb.Append(", ");
                                //    sb.Append(item.Street2);
                                //    sb.Append(", ");
                                //    sb.Append(item.Street3);
                                //    sb.Append(", ");
                                //    sb.Append(item.CityName);
                                //    sb.Append(", ");
                                //    sb.Append(item.StateName);
                                //    sb.Append(", ");
                                //    sb.Append(item.StateCode);
                                //    sb.Append(", ");
                                //    sb.Append(item.CountryName);
                                //    sb.Append(", ");
                                //    sb.Append(item.CountryCode);
                                //    sb.Append(", ");
                                //    sb.Append(item.PostCode);
                                //    _lstresult.Add(new DC_HotelListByCityCode
                                //    {
                                //        HotelName = item.ProductName,
                                //        Address = Convert.ToString(sb),
                                //        CityMapping_Id = param.CityMapping_Id,
                                //        GoFor = strGoFor,
                                //        TotalRecords = total
                                //    });
                                //    sb.Clear();

                                //}

                                return _lstresult;
                            }
                            else
                            {
                                return _lstresult;
                            }
                        }
                        else
                        {
                            return _lstresult;
                        }
                    }
                    else
                    {
                        return _lstresult;
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region DataHandler
        public List<DataContracts.Mapping.DC_CountryMapping> GetCountryMapping_ForHandler(DataContracts.Mapping.DC_CountryMappingRQ RQ)
        {
            try
            {
                StringBuilder sbselect = new StringBuilder();
                StringBuilder sbfrom = new StringBuilder();
                StringBuilder sbwhere = new StringBuilder();
                List<DataContracts.Mapping.DC_CountryMapping> result = new List<DataContracts.Mapping.DC_CountryMapping>();

                sbfrom.Append(@" FROM m_CountryMapping CMP 
                                 LEFT JOIN m_CountryMaster CM ON CMP.Country_Id = CM.Country_Id");

                #region Get where Clause
                sbwhere.Append(" WHERE 1=1 ");
                if (RQ.Supplier_Id != Guid.Empty)
                {
                    sbwhere.Append(" AND CMP.Supplier_Id = '" + RQ.Supplier_Id + "'");
                }
                if (!string.IsNullOrWhiteSpace(RQ.SystemCountryName))
                {
                    sbwhere.Append(" AND CMP.CountryName LIKE '%" + RQ.SystemCountryName + "%' ");
                }
                if (RQ.Status.ToUpper().IndexOf("ALL") == -1)
                {
                    sbwhere.Append(" AND CMP.Status='" + RQ.Status + "' ");
                }
                if (RQ.StatusExcept != null)
                {
                    sbwhere.Append(" AND CMP.Status !='" + RQ.Status + "' ");
                }
                if (!string.IsNullOrWhiteSpace(RQ.SupplierCountryName))
                {
                    sbwhere.Append(" AND CMP.CountryName='" + RQ.SupplierCountryName + "' ");
                }
                if (!string.IsNullOrWhiteSpace(RQ.SupplierCountryCode))
                {
                    sbwhere.Append(" AND CMP.CountryCode='" + RQ.SupplierCountryCode.Trim() + "' ");
                }

                #endregion

                int skip = 0;
                int total = 0;
                skip = RQ.PageSize * RQ.PageNo;

                StringBuilder sbsqlselectcount = new StringBuilder();
                sbsqlselectcount.Append("select count(CMP.CountryMapping_Id) ");
                sbsqlselectcount.Append(" " + sbfrom);
                sbsqlselectcount.Append(" " + sbwhere);

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    context.Configuration.AutoDetectChangesEnabled = false;
                    try { total = context.Database.SqlQuery<int>(sbsqlselectcount.ToString()).FirstOrDefault(); } catch (Exception ex) { }
                }

                if (total > 0)
                {

                    #region -- Select query
                    sbselect.Append(@"  SELECT 
                                    CMP.CountryMapping_Id   AS  CountryMapping_Id, 
                                    CMP.Country_Id  AS  Country_Id,
                                    CMP.Supplier_Id  AS  Supplier_Id,
                                    CMP.CountryCode  AS  CountryCode,
                                    CMP.CountryCode  AS OldCountryCode,
                                    CMP.CountryName  AS  CountryName,
                                    CMP.Create_Date  AS  Create_Date,
                                    CMP.Create_User  AS  Create_User,
                                    CMP.Edit_Date  AS  Edit_Date,
                                    CMP.Edit_User  AS  Edit_User,
                                    CMP.MapID  AS  MapID,
                                    CMP.Status  AS  Status,
                                    CMP.SupplierName  AS  SupplierName,
                                    CM.Code  AS  Code,
                                    CM.Name  AS  Name,
                                    CMP.Latitude  AS  Latitude,
                                    CMP.Longitude  AS  Longitude,
                                    CMP.ContinentCode  AS  ContinentCode,
                                    CMP.ContinentName  AS ContinentName ,
                                    CMP.SupplierImportFile_Id AS  SupplierImporrtFile_Id,
                                    ISNULL(CMP.Batch, 0)  AS  Batch,
                                    CMP.ReRun_SupplierImportFile_Id  AS  ReRunSupplierImporrtFile_Id,
                                    ISNULL(CMP.ReRun_Batch,0) AS ReRunBatch,");
                    sbselect.Append(total.ToString() + " AS  TotalRecord ");

                    #endregion

                    if (total <= skip)
                    {
                        int PageIndex = 0;
                        int intReminder = total % RQ.PageSize;
                        int intQuotient = total / RQ.PageSize;

                        if (intReminder > 0)
                        {
                            PageIndex = intQuotient + 1;
                        }
                        else
                        {
                            PageIndex = intQuotient;
                        }

                        skip = RQ.PageSize * (PageIndex - 1);
                    }

                    StringBuilder sbOrderby = new StringBuilder();
                    sbOrderby.Append(" ORDER BY CMP.CountryName ");
                    sbOrderby.Append(" OFFSET ");
                    sbOrderby.Append((skip).ToString());
                    sbOrderby.Append(" ROWS FETCH NEXT ");
                    sbOrderby.Append(RQ.PageSize.ToString());
                    sbOrderby.Append(" ROWS ONLY ");

                    StringBuilder sbfinalQuery = new StringBuilder();
                    sbfinalQuery.Append(sbselect + " ");
                    sbfinalQuery.Append(" " + sbfrom + " ");
                    sbfinalQuery.Append(" " + sbwhere + " ");
                    sbfinalQuery.Append(" " + sbOrderby);

                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        context.Configuration.AutoDetectChangesEnabled = false;
                        try { result = context.Database.SqlQuery<DataContracts.Mapping.DC_CountryMapping>(sbfinalQuery.ToString()).ToList(); } catch (Exception ex) { }
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching country mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public void LogErrorMessage(Guid FileId, object Err_Object, string Err_Namespace, string Err_ClassName, string Err_MethodName, int Err_ErrorCode, string Err_Type, string Err_SimpleMessage)
        {
            DC_ErrorLog_Format Err = new DC_ErrorLog_Format();
            Err.Err_SupplierImportFile_Id = FileId;
            Err.Err_Object = Err_Object;
            Err.Err_Namespace = Err_Namespace;
            Err.Err_ClassName = Err_ClassName;
            Err.Err_MethodName = Err_MethodName;
            Err.Err_ErrorCode = Err_ErrorCode;
            Err.Err_Type = Err_Type;
            Err.Err_SimpleMessage = Err_SimpleMessage;
            LogErrorMessage(Err);
        }

        public void LogErrorMessage(DC_ErrorLog_Format Err)
        {
            Type typ = Err.Err_Object.GetType();
            StringBuilder ErrDescription = new StringBuilder();
            ErrDescription.AppendFormat("Error Occured on Object of Type: {0}", typ.Name);
            ErrDescription.AppendLine();
            ErrDescription.AppendFormat("Error Code: {0}", Err.Err_ErrorCode.ToString());
            ErrDescription.AppendLine();
            ErrDescription.AppendFormat("Namespace: {0}", Err.Err_Namespace);
            ErrDescription.AppendLine();
            ErrDescription.AppendFormat("Class Name: {0}", Err.Err_ClassName);
            ErrDescription.AppendLine();
            ErrDescription.AppendFormat("Method Name: {0}", Err.Err_MethodName);
            ErrDescription.AppendLine();
            DC_SupplierImportFile_ErrorLog obj = new DC_SupplierImportFile_ErrorLog();
            obj.SupplierImportFile_ErrorLog_Id = Guid.NewGuid();
            obj.SupplierImportFile_Id = Err.Err_SupplierImportFile_Id;
            obj.ErrorCode = Err.Err_ErrorCode;
            obj.ErrorDescription = ErrDescription.ToString();
            obj.ErrorMessage_UI = Err.Err_SimpleMessage;
            obj.Error_DATE = DateTime.Now;
            obj.Error_USER = "TLGX_DataHandler";
            DL_UploadStaticData _obj = new DL_UploadStaticData();
            var msg = _obj.AddStaticDataUploadErrorLog(obj);
        }
        #endregion

        #region NewDashBoardReport
        public List<DataContracts.Mapping.DC_NewDashBoardReportCountry_RS> GetNewDashboardReport_CountryWise()
        {
            try
            {
                List<DataContracts.Mapping.DC_NewDashBoardReportCountry_RS> returnObj = new List<DataContracts.Mapping.DC_NewDashBoardReportCountry_RS>();

              // Query to get Country Wise Hotel Counts
                StringBuilder sbSelect = new StringBuilder();
              
                sbSelect.Append(@" Select RegionName, CountryName, 0.0 as ContentScore 
                                    , isnull(sum(Count_APM_Mapped_And_AutoMapped) + sum(Count_APM_Review),0) as TotalSupplierHotels 
                                    , isnull(sum(Count_APM_Mapped_And_AutoMapped),0)Mapped_Hotel
                                    , isnull(sum(Count_APM_Review),0) as Review_Hotel
                                    , 0 as Unmapped_Hotel
                                    ,CASE (sum(Count_APM_Mapped_And_AutoMapped) + sum(Count_APM_Review)) WHEN 0 THEN 0 ELSE CONVERT(DECIMAL(10,1), ROUND(CONVERT(decimal(10,2), sum(Count_APM_Mapped_And_AutoMapped)) / CONVERT(decimal(10,2),(sum(Count_APM_Mapped_And_AutoMapped) + sum(Count_APM_Review))) * 100.00,1)) END as Per_Mapped_Hotel
                                    ,CASE (sum(Count_APM_Mapped_And_AutoMapped) + sum(Count_APM_Review)) WHEN 0 THEN 0 ELSE CONVERT(DECIMAL(10,1), ROUND(CONVERT(decimal(10,2), sum(Count_APM_Review)) / CONVERT(decimal(10,2),(sum(Count_APM_Mapped_And_AutoMapped) + sum(Count_APM_Review))) * 100.00,1)) END as Per_Review_Hotel
                                                                 
                                    , isnull(sum(Count_SRT_Mapped) + sum(Count_SRT_Automapped) + sum(Count_SRT_Review) + sum(Count_SRT_Unmapped) + sum(Count_SRT_Add),0) as TotalSupplierRoom
                                    , isnull(sum(Count_SRT_Mapped),0) as Mapped_Room
                                    , isnull(sum(Count_SRT_Review),0) as Review_Room
                                    , isnull(sum(Count_SRT_Unmapped),0) as Unmapped_Room
                                    , isnull(sum(Count_SRT_Add),0) as Add_Room
                                    , isnull(sum(Count_SRT_Automapped),0) as AutoMapped_Room
                                    , CASE (sum(Count_SRT_Mapped) + sum(Count_SRT_Automapped) + sum(Count_SRT_Review) + sum(Count_SRT_Unmapped) + sum(Count_SRT_Add)) WHEN 0 THEN 0 ELSE CONVERT(DECIMAL(10,1), ROUND(CONVERT(decimal(10,2), sum(Count_SRT_Mapped)) / CONVERT(decimal(10,2),(sum(Count_SRT_Mapped) + sum(Count_SRT_Automapped) + sum(Count_SRT_Review) + sum(Count_SRT_Unmapped) + sum(Count_SRT_Add))) * 100.00,1)) END as Per_Mapped_Room
                                    , CASE (sum(Count_SRT_Mapped) + sum(Count_SRT_Automapped) + sum(Count_SRT_Review) + sum(Count_SRT_Unmapped) + sum(Count_SRT_Add)) WHEN 0 THEN 0 ELSE CONVERT(DECIMAL(10,1), ROUND(CONVERT(decimal(10,2), sum(Count_SRT_Automapped)) / CONVERT(decimal(10,2),(sum(Count_SRT_Mapped) + sum(Count_SRT_Automapped) + sum(Count_SRT_Review) + sum(Count_SRT_Unmapped) + sum(Count_SRT_Add))) * 100.00,1)) END as Per_AutoMapped_Room
                                    , CASE (sum(Count_SRT_Mapped) + sum(Count_SRT_Automapped) + sum(Count_SRT_Review) + sum(Count_SRT_Unmapped) + sum(Count_SRT_Add)) WHEN 0 THEN 0 ELSE CONVERT(DECIMAL(10,1), ROUND(CONVERT(decimal(10,2), sum(Count_SRT_Review)) / CONVERT(decimal(10,2),(sum(Count_SRT_Mapped) + sum(Count_SRT_Automapped) + sum(Count_SRT_Review) + sum(Count_SRT_Unmapped) + sum(Count_SRT_Add))) * 100.00,1)) END as Per_Review_Room
                                    , CASE (sum(Count_SRT_Mapped) + sum(Count_SRT_Automapped) + sum(Count_SRT_Review) + sum(Count_SRT_Unmapped) + sum(Count_SRT_Add)) WHEN 0 THEN 0 ELSE CONVERT(DECIMAL(10,1), ROUND(CONVERT(decimal(10,2), sum(Count_SRT_UnMapped)) / CONVERT(decimal(10,2),(sum(Count_SRT_Mapped) + sum(Count_SRT_Automapped) + sum(Count_SRT_Review) + sum(Count_SRT_Unmapped) + sum(Count_SRT_Add))) * 100.00,1)) END as Per_Unmapped_Room
                                    , CASE (sum(Count_SRT_Mapped) + sum(Count_SRT_Automapped) + sum(Count_SRT_Review) + sum(Count_SRT_Unmapped) + sum(Count_SRT_Add)) WHEN 0 THEN 0 ELSE CONVERT(DECIMAL(10,1), ROUND(CONVERT(decimal(10,2), sum(Count_SRT_Add)) / CONVERT(decimal(10,2),(sum(Count_SRT_Mapped) + sum(Count_SRT_Automapped) + sum(Count_SRT_Review) + sum(Count_SRT_Unmapped) + sum(Count_SRT_Add))) * 100.00,1)) END as Per_Add_Room
                                    ,Country_Id   
                                    FROM [NewDashBoardReport]  with (NoLock)
                                    WHERE  ReportType='HOTEL' and  Country_Id!='00000000-0000-0000-0000-000000000000' 
                                    GROUP BY  RegionName ,CountryName , Country_Id 
                                    ORDER BY CountryName
                                    ");
              

                // CountryWise Distinct SupplierCount
                StringBuilder sbSupplierCount = new StringBuilder();
                sbSupplierCount.Append(@"select isnull(Count_SuppliersAcco,0) as Count_SuppliersAcco , isnull(Count_SuppliersRoom,0) as Count_SuppliersRoom , Country_Id 
                                        from NewDashBoardReport NDBR with(nolock)
                                        where NDBR.ReportType='COUNTRY'");

                // Query To get CountryWise preffered Hotel Count
                StringBuilder sbPrefHotelCount = new StringBuilder();
                sbPrefHotelCount.Append(@"select Country_Id, isnull(count(HotelID) ,0) as PrefHotelCount
                                        from NewDashBoardReport  with(nolock)
                                        where ReportType='HOTEL' and Country_Id<>'00000000-0000-0000-0000-000000000000' AND IsPreferredHotel=1 
                                        GROUP BY Country_Id");


                List<DC_SupCount> TotalSupCounts = new List<DC_SupCount>();
                List<DC_PrefferdHotel> PrefHotCounts = new List<DC_PrefferdHotel>();

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    context.Database.CommandTimeout = 0;
                    context.Configuration.AutoDetectChangesEnabled = false;
                    returnObj = context.Database.SqlQuery<DataContracts.Mapping.DC_NewDashBoardReportCountry_RS>(sbSelect.ToString()).ToList();
                    TotalSupCounts = context.Database.SqlQuery<DataContracts.Mapping.DC_SupCount>(sbSupplierCount.ToString()).ToList();
                    PrefHotCounts = context.Database.SqlQuery<DataContracts.Mapping.DC_PrefferdHotel>(sbPrefHotelCount.ToString()).ToList();
                }

                foreach (var item in returnObj)
                {
                    if (TotalSupCounts != null)
                    {
                        item.NoOfSuppliers_H = TotalSupCounts.Where(p => p.Country_Id == item.Country_id).Select(p => p.Count_SuppliersAcco).SingleOrDefault();
                        item.NoOfSuppliers_R= TotalSupCounts.Where(p => p.Country_Id == item.Country_id).Select(p => p.Count_SuppliersRoom).SingleOrDefault();
                    }
                    if(PrefHotCounts!= null)
                    {
                        item.PreferredHotels = PrefHotCounts.Where(p => p.Country_Id == item.Country_id).Select(p => p.PrefHotelCount).SingleOrDefault();
                    }
                    
                }
              
                return returnObj;
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching   CountryWise New DashBoard Report", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion NewDashBoardReport

        #region 
        public List<DataContracts.Mapping.DC_EzeegoHotelVsSupplierHotelMappingReport> EzeegoHotelVsSupplierHotelMappingReport(DataContracts.Mapping.DC_EzeegoHotelVsSupplierHotelMappingReport_RQ RQ)
        {
            List<DC_EzeegoHotelVsSupplierHotelMappingReport> response = new List<DC_EzeegoHotelVsSupplierHotelMappingReport>();

            #region Construct SQL Query

            StringBuilder sbSelect = new StringBuilder();
            StringBuilder sbFrom = new StringBuilder();
            StringBuilder sbJoin = new StringBuilder();
            StringBuilder sbWhere = new StringBuilder();

            var regionData = String.Join(",", RQ.Region.Select(s => "'" + s + "'"));
            string countryData = String.Join(",", RQ.Country.Select(s => "'" + s + "'"));
            string cityData = String.Join(",", RQ.City.Select(s => "'" + s + "'"));
            string supplierData = String.Join(",", RQ.Supplier.Select(s => "'" + s + "'"));
            string AccoPriority = String.Join(",", RQ.AccoPriority.Select(s => "'" + s + "'"));

            sbSelect.AppendLine(" Select ac.CompanyHotelID As Legacy_HTL_ID, ac.TLGXAccoId,  ac.HotelName,ac.country AS CountryName, ac.city AS CityName, ac.Priority ");
            sbFrom.AppendLine(" From Accommodation ac with(NOLOCK) ");

            if (RQ.Country.Count > 0)
            {
                sbWhere.AppendLine(" where ac.Country_Id in(" + countryData + ") ");
            }

            if (RQ.Supplier.Count > 0)
            {
                sbSelect.AppendLine(" , Sup.Name As SupplierName, CASE WHEN ISNULL(APM.[Status],'UNMAPPED') IN ('AUTOMAPPED','MAPPED') THEN APM.SupplierProductReference ELSE 'Not Mapped' END AS [Status] ");
                sbJoin.AppendLine("cross join (SELECT Supplier_Id, Code, Name, Priority from Supplier where Supplier_Id in (" + supplierData + ")) sup ");
                sbJoin.AppendLine(" LEFT JOIN Accommodation_ProductMapping apm with(nolock) on ac.Accommodation_Id = apm.Accommodation_Id and apm.Supplier_Id = sup.Supplier_Id ");

            }

            if (RQ.City.Count > 0)
            {
                sbWhere.AppendLine(" and ac.City_Id in(" + cityData + ")");
            }

            if (RQ.AccoPriority.Count > 0)
            {
                sbWhere.AppendLine(" and CAST(ac.Priority AS varchar(5)) in(" + AccoPriority + ")");
            }

            if (!string.IsNullOrEmpty(RQ.selectedHotelId))
            {
                sbWhere.AppendLine(" and ac.CompanyHotelID in(" + RQ.selectedHotelId + ")");
            }

            StringBuilder sbfinalquery = new StringBuilder();
            sbfinalquery.AppendLine(sbSelect.ToString());
            sbfinalquery.AppendLine(sbFrom.ToString());
            sbfinalquery.AppendLine(sbJoin.ToString());
            sbfinalquery.AppendLine(sbWhere.ToString());

            # endregion Construct SQL Query

            using (ConsumerEntities context = new ConsumerEntities())
            {
                try
                {
                    context.Database.CommandTimeout = 0;
                    response = context.Database.SqlQuery<DC_EzeegoHotelVsSupplierHotelMappingReport>(sbfinalquery.ToString()).ToList();
                }
                catch (Exception ex)
                {

                }
            }

            return response;

        }
        #endregion


        #region AccoProductMapping Report
        //GAURAV-TMAP-645
        public List<DataContracts.Mapping.DC_SupplierAccoMappingExportDataReport> AccomodationMappingReport(DC_SupplerVSupplier_Report_RQ dC_SupplerVSupplier_Report_RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    List<DataContracts.Mapping.DC_SupplierAccoMappingExportDataReport> lstAcco = new List<DataContracts.Mapping.DC_SupplierAccoMappingExportDataReport>();
                    StringBuilder sb = new StringBuilder();


                    sb.Append(@"
                                Declare @TotalHotel int = (select count(*) from Accommodation_ProductMapping with(Nolock) where Supplier_Id = '" + dC_SupplerVSupplier_Report_RQ.Accommodation_Source_Id + "')");
                    sb.Append(@"     select Priority, Supplier_Id,SupplierName,Accomodation_MannualMapped ,Accomodation_AutoMapped,Accomodation_ReviewMapped,Accomodation_Unmapped,
                                (Accomodation_MannualMapped + Accomodation_AutoMapped + Accomodation_ReviewMapped + Accomodation_Unmapped) as AccomodationTotal,
                                    @TotalHotel as AccoHotelCount, Compared_SupplierName as SourceSupplierName
                                from AccommodationSupplierMappingReport_SupplierVSupplier with(nolock) where Compared_Supplier_Id = '" + dC_SupplerVSupplier_Report_RQ.Accommodation_Source_Id + "' ");

                    if(dC_SupplerVSupplier_Report_RQ.Compare_WithSupplier_Ids != null && dC_SupplerVSupplier_Report_RQ.Compare_WithSupplier_Ids.Count > 0)
                    {
                        string ids = string.Join(",", dC_SupplerVSupplier_Report_RQ.Compare_WithSupplier_Ids
                                            .Select(x => string.Format("'{0}'", x)));
                        

                        sb.Append("and Supplier_Id in (" + ids + ") ");

                    }
                    sb.Append(" order by Priority,SupplierName ");
                    //sb.Append("select top 500 AC.Accommodation_Id,AC.HotelName,AC.city,AC.State_Name  As State,St.StateCode,AC.country from Accommodation AC left outer join m_States St on AC.Country_Id = St.Country_Id and ac.State_Name = st.StateName ");
                    //sb.Append("where AC.IsActive = 1 and AC.HotelName Like '%" + RQ.HotelName + "%'");
                    //if (!string.IsNullOrWhiteSpace(RQ.Country))
                    //{
                    //    sb.Append(" and ( AC.country = '"); sb.Append(RQ.Country); sb.Append("' ");
                    //    sb.Append(" OR AC.Country_Id = '"); sb.Append(RQ.Country_Id); sb.Append("') ");
                    //}
                    //if (!string.IsNullOrWhiteSpace(RQ.State))
                    //{
                    //    sb.Append(" and AC.State_Name = '"); sb.Append(RQ.State); sb.Append("' ");
                    //}
                    //sb.Append(" order by AC.HotelName");
                    try { lstAcco = context.Database.SqlQuery<DataContracts.Mapping.DC_SupplierAccoMappingExportDataReport>(sb.ToString()).ToList(); } catch (Exception ex) { }
                    return lstAcco;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching accomodation for autocomplete", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        #endregion
    }
}
