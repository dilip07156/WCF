using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using DataContracts.Masters;
using DataContracts;
using System.Text.RegularExpressions;
using System.Diagnostics;
using DataContracts.Mapping;

namespace DataLayer
{
    public class DL_Masters : IDisposable
    {
        public enum operation
        {
            Save,
            Update
        }

        public void Dispose()
        { }

        #region CountryMaster

        public List<string> GetCountryNameList(DataContracts.Masters.DC_Country_Search_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from c in context.m_CountryMaster
                                 select c;

                    if (!string.IsNullOrWhiteSpace(RQ.Country_Name))
                    {
                        search = from a in search
                                 where a.Name.Contains(RQ.Country_Name)
                                 select a;
                    }

                    var country = (from a in search
                                   where a.Status.ToUpper() == "ACTIVE"
                                   orderby a.Name
                                   select a.Name).ToList();

                    return country;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching Country Master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public List<DataContracts.Masters.DC_Country> GetCountryMaster(DataContracts.Masters.DC_Country_Search_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from c in context.m_CountryMaster
                                 select c;

                    if (RQ.Country_Id != null)
                    {
                        search = from a in search
                                 where a.Country_Id == RQ.Country_Id
                                 select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.Country_Code))
                    {
                        search = from a in search
                                 where a.Code == RQ.Country_Code
                                 select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.Country_Name))
                    {
                        search = from a in search
                                 where a.Name.StartsWith(RQ.Country_Name)
                                 select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.RegionCode))
                    {
                        search = from a in search
                                 where a.RegionCode == RQ.RegionCode
                                 select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.Key))
                    {
                        search = from a in search
                                 where a.Key == RQ.Key
                                 select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.Rank))
                    {
                        search = from a in search
                                 where a.Rank == RQ.Rank
                                 select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.Priority))
                    {
                        search = from a in search
                                 where a.Priority == RQ.Priority
                                 select a;
                    }

                    int total = search.Count();
                    int skip = (RQ.PageNo ?? 0) * (RQ.PageSize ?? 0);

                    var result = from a in search
                                 select new DataContracts.Masters.DC_Country
                                 {
                                     Name = a.Name,
                                     Code = a.Code,
                                     Country_Id = a.Country_Id,
                                     Create_Date = a.Create_Date,
                                     Create_User = a.Create_User,
                                     Dial = a.Dial,
                                     DS = a.DS,
                                     Edit_Date = a.Edit_Date,
                                     Edit_User = a.Edit_User,
                                     GooglePlaceID = a.GooglePlaceID,
                                     ISO3166_1_Alpha_2 = a.ISO3166_1_Alpha_2,
                                     ISO3166_1_Alpha_3 = a.ISO3166_1_Alpha_3,
                                     ISO3166_1_Capital = a.ISO3166_1_Capital,
                                     ISO3166_1_Continent = a.ISO3166_1_Continent,
                                     ISO3166_1_EDGAR = a.ISO3166_1_EDGAR,
                                     ISO3166_1_Geoname_ID = a.ISO3166_1_Geoname_ID,
                                     ISO3166_1_ITU = a.ISO3166_1_ITU,
                                     ISO3166_1_Languages = a.ISO3166_1_Languages,
                                     ISO3166_1_M49 = a.ISO3166_1_M49,
                                     ISO3166_1_TLD = a.ISO3166_1_TLD,
                                     ISO4217_currency_alphabetic_code = a.ISO4217_currency_alphabetic_code,
                                     ISO4217_currency_country_name = a.ISO4217_currency_country_name,
                                     ISO4217_currency_minor_unit = a.ISO4217_currency_minor_unit,
                                     ISO4217_currency_name = a.ISO4217_currency_name,
                                     ISO4217_currency_numeric_code = a.ISO4217_currency_numeric_code,
                                     ISOofficial_name_en = a.ISOofficial_name_en,
                                     MARC = a.MARC,
                                     Status = a.Status,
                                     WMO = a.WMO,
                                     TotalRecords = total,
                                     Latitude = a.Latitude,
                                     Longitude = a.Longitude,
                                     RegionCode = a.RegionCode,
                                     RegionName = a.RegionName,
                                     Key = a.Key,
                                     Rank = a.Rank,
                                     Priority = a.Priority

                                 };

                    return result.OrderBy(p => p.Name).Skip(skip).Take((RQ.PageSize ?? total)).ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching Country Master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        //public List<DataContracts.Masters.DC_Field> GetFields(DataContracts.Masters.DC_Field_Search_RQ RQ)
        //{
        //    try
        //    {
        //        using (ConsumerEntities context = new ConsumerEntities())
        //        {
        //            var search = from c in context.m_CountryMaster
        //                         select c;

        //            if (RQ.RegionCode != null)
        //            {
        //                search = from a in search
        //                         where a.RegionCode == RQ.RegionCode
        //                         select a;
        //            }


        //            int total = search.Count();
        //            //int skip = (RQ.PageNo ?? 0) * (RQ.PageSize ?? 0);

        //            var result = from a in search
        //                         select new DataContracts.Masters.DC_Field
        //                         {
        //                             Name = a.Name,
        //                             Code = a.Code,
        //                             Country_Id = a.Country_Id,
        //                             Create_Date = a.Create_Date,
        //                             Create_User = a.Create_User,
        //                             Dial = a.Dial,
        //                             DS = a.DS,
        //                             Edit_Date = a.Edit_Date,
        //                             Edit_User = a.Edit_User,
        //                             GooglePlaceID = a.GooglePlaceID,
        //                             ISO3166_1_Alpha_2 = a.ISO3166_1_Alpha_2,
        //                             ISO3166_1_Alpha_3 = a.ISO3166_1_Alpha_3,
        //                             ISO3166_1_Capital = a.ISO3166_1_Capital,
        //                             ISO3166_1_Continent = a.ISO3166_1_Continent,
        //                             ISO3166_1_EDGAR = a.ISO3166_1_EDGAR,
        //                             ISO3166_1_Geoname_ID = a.ISO3166_1_Geoname_ID,
        //                             ISO3166_1_ITU = a.ISO3166_1_ITU,
        //                             ISO3166_1_Languages = a.ISO3166_1_Languages,
        //                             ISO3166_1_M49 = a.ISO3166_1_M49,
        //                             ISO3166_1_TLD = a.ISO3166_1_TLD,
        //                             ISO4217_currency_alphabetic_code = a.ISO4217_currency_alphabetic_code,
        //                             ISO4217_currency_country_name = a.ISO4217_currency_country_name,
        //                             ISO4217_currency_minor_unit = a.ISO4217_currency_minor_unit,
        //                             ISO4217_currency_name = a.ISO4217_currency_name,
        //                             ISO4217_currency_numeric_code = a.ISO4217_currency_numeric_code,
        //                             ISOofficial_name_en = a.ISOofficial_name_en,
        //                             MARC = a.MARC,
        //                             Status = a.Status,
        //                             WMO = a.WMO,
        //                             TotalRecords = total,
        //                             Latitude = a.Latitude,
        //                             Longitude = a.Longitude,
        //                             RegionCode = a.RegionCode,
        //                             RegionName = a.RegionName,
        //                             Key = a.Key,
        //                             Rank = a.Rank,
        //                             Priority = a.Priority

        //                         };

        //            return result.OrderBy(p => p.Name).Skip(skip).Take((RQ.PageSize ?? total)).ToList();
        //        }
        //    }
        //    catch
        //    {
        //        throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching Country Master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
        //    }
        //}

        public DC_Message AddUpdateCountryMaster(DataContracts.Masters.DC_Country param)
        {
            DC_Message _objMsg = new DC_Message();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    //Check duplicate 
                    var isduplicate = (from attr in context.m_CountryMaster
                                       where attr.Country_Id != param.Country_Id && attr.Name == param.Name
                                       select attr).Count() == 0 ? false : true;
                    if (isduplicate)
                    {
                        _objMsg.StatusCode = ReadOnlyMessage.StatusCode.Duplicate;
                        _objMsg.StatusMessage = param.Name + ReadOnlyMessage.strAlreadyExist;
                        return _objMsg;
                    }
                    if (param.Action == "Save")
                    {
                        DataLayer.m_CountryMaster objNew = new m_CountryMaster();
                        objNew.Name = param.Name;
                        objNew.Code = param.Code;
                        objNew.Dial = param.Dial;

                        objNew.RegionCode = param.RegionCode;
                        objNew.RegionName = param.RegionName;
                        objNew.Key = param.Key;
                        objNew.Rank = param.Rank;
                        objNew.Priority = param.Priority;

                        objNew.DS = param.DS;
                        objNew.Create_Date = param.Create_Date;
                        objNew.Create_User = param.Create_User;
                        objNew.GooglePlaceID = param.GooglePlaceID;
                        objNew.ISO3166_1_Alpha_2 = param.ISO3166_1_Alpha_2;
                        objNew.ISO3166_1_Alpha_3 = param.ISO3166_1_Alpha_3;
                        objNew.ISO3166_1_Capital = param.ISO3166_1_Capital;
                        objNew.ISO3166_1_Continent = param.ISO3166_1_Continent;
                        objNew.ISO3166_1_EDGAR = param.ISO3166_1_EDGAR;
                        objNew.ISO3166_1_Geoname_ID = param.ISO3166_1_Geoname_ID;
                        objNew.ISO3166_1_ITU = param.ISO3166_1_ITU;
                        objNew.ISO3166_1_Languages = param.ISO3166_1_Languages;
                        objNew.ISO3166_1_M49 = param.ISO3166_1_M49;
                        objNew.ISO3166_1_TLD = param.ISO3166_1_TLD;
                        objNew.ISO4217_currency_alphabetic_code = param.ISO4217_currency_alphabetic_code;
                        objNew.ISO4217_currency_country_name = param.ISO4217_currency_country_name;
                        objNew.ISO4217_currency_minor_unit = param.ISO4217_currency_minor_unit;
                        objNew.ISO4217_currency_name = param.ISO4217_currency_name;
                        objNew.ISO4217_currency_numeric_code = param.ISO4217_currency_numeric_code;
                        objNew.ISOofficial_name_en = param.ISOofficial_name_en;
                        objNew.MARC = param.MARC;
                        objNew.Status = param.Status;
                        objNew.WMO = param.WMO;
                        objNew.Country_Id = param.Country_Id;
                        objNew.Latitude = param.Latitude;
                        objNew.Longitude = param.Longitude;
                        context.m_CountryMaster.Add(objNew);
                    }
                    else if (param.Action == "Update")
                    {
                        var search = context.m_CountryMaster.Find(param.Country_Id);
                        if (search != null)
                        {
                            search.Name = param.Name;
                            search.Code = param.Code;
                            search.Dial = param.Dial;

                            search.RegionName = param.RegionName;
                            search.RegionCode = param.RegionCode;
                            search.Key = param.Key;
                            search.Rank = param.Rank;
                            search.Priority = param.Priority;

                            search.DS = param.DS;
                            search.Edit_Date = param.Edit_Date;
                            search.Edit_User = param.Edit_User;
                            search.GooglePlaceID = param.GooglePlaceID;
                            search.ISO3166_1_Alpha_2 = param.ISO3166_1_Alpha_2;
                            search.ISO3166_1_Alpha_3 = param.ISO3166_1_Alpha_3;
                            search.ISO3166_1_Capital = param.ISO3166_1_Capital;
                            search.ISO3166_1_Continent = param.ISO3166_1_Continent;
                            search.ISO3166_1_EDGAR = param.ISO3166_1_EDGAR;
                            search.ISO3166_1_Geoname_ID = param.ISO3166_1_Geoname_ID;
                            search.ISO3166_1_ITU = param.ISO3166_1_ITU;
                            search.ISO3166_1_Languages = param.ISO3166_1_Languages;
                            search.ISO3166_1_M49 = param.ISO3166_1_M49;
                            search.ISO3166_1_TLD = param.ISO3166_1_TLD;
                            search.ISO4217_currency_alphabetic_code = param.ISO4217_currency_alphabetic_code;
                            search.ISO4217_currency_country_name = param.ISO4217_currency_country_name;
                            search.ISO4217_currency_minor_unit = param.ISO4217_currency_minor_unit;
                            search.ISO4217_currency_name = param.ISO4217_currency_name;
                            search.ISO4217_currency_numeric_code = param.ISO4217_currency_numeric_code;
                            search.ISOofficial_name_en = param.ISOofficial_name_en;
                            search.MARC = param.MARC;
                            search.Status = param.Status;
                            search.WMO = param.WMO;
                        }
                        else
                        {
                            _objMsg.StatusMessage = ReadOnlyMessage.strFailed;
                            _objMsg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                            return _objMsg;
                        }
                    }
                    context.SaveChanges();
                    _objMsg.StatusMessage = param.Action == "Update" ? ReadOnlyMessage.strUpdatedSuccessfully : ReadOnlyMessage.strAddedSuccessfully;
                    _objMsg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                }
                return _objMsg;
            }

            catch (Exception)
            {
                _objMsg.StatusMessage = ReadOnlyMessage.strFailed;
                _objMsg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                return _objMsg;
            }
        }
        public bool AddCountryMaster(DataContracts.Masters.DC_Country param)
        {
            try
            {
                if (param.Country_Id == null)
                {
                    param.Country_Id = Guid.NewGuid();
                }
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    DataLayer.m_CountryMaster objNew = new m_CountryMaster();

                    objNew.Name = param.Name;
                    objNew.Code = param.Code;
                    objNew.Country_Id = param.Country_Id;
                    objNew.Dial = param.Dial;
                    objNew.DS = param.DS;
                    objNew.Create_Date = param.Create_Date;
                    objNew.Create_User = param.Create_User;
                    objNew.GooglePlaceID = param.GooglePlaceID;
                    objNew.ISO3166_1_Alpha_2 = param.ISO3166_1_Alpha_2;
                    objNew.ISO3166_1_Alpha_3 = param.ISO3166_1_Alpha_3;
                    objNew.ISO3166_1_Capital = param.ISO3166_1_Capital;
                    objNew.ISO3166_1_Continent = param.ISO3166_1_Continent;
                    objNew.ISO3166_1_EDGAR = param.ISO3166_1_EDGAR;
                    objNew.ISO3166_1_Geoname_ID = param.ISO3166_1_Geoname_ID;
                    objNew.ISO3166_1_ITU = param.ISO3166_1_ITU;
                    objNew.ISO3166_1_Languages = param.ISO3166_1_Languages;
                    objNew.ISO3166_1_M49 = param.ISO3166_1_M49;
                    objNew.ISO3166_1_TLD = param.ISO3166_1_TLD;
                    objNew.ISO4217_currency_alphabetic_code = param.ISO4217_currency_alphabetic_code;
                    objNew.ISO4217_currency_country_name = param.ISO4217_currency_country_name;
                    objNew.ISO4217_currency_minor_unit = param.ISO4217_currency_minor_unit;
                    objNew.ISO4217_currency_name = param.ISO4217_currency_name;
                    objNew.ISO4217_currency_numeric_code = param.ISO4217_currency_numeric_code;
                    objNew.ISOofficial_name_en = param.ISOofficial_name_en;
                    objNew.MARC = param.MARC;
                    objNew.Status = param.Status;
                    objNew.WMO = param.WMO;
                    objNew.Latitude = param.Latitude;
                    objNew.Longitude = param.Longitude;
                    context.m_CountryMaster.Add(objNew);
                    context.SaveChanges();

                    objNew = null;
                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while adding country master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool UpdateCountryMaster(DataContracts.Masters.DC_Country param)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = context.m_CountryMaster.Find(param.Country_Id);
                    if (search != null)
                    {
                        search.Name = param.Name;
                        search.Code = param.Code;
                        search.Dial = param.Dial;
                        search.DS = param.DS;
                        search.Edit_Date = param.Edit_Date;
                        search.Edit_User = param.Edit_User;
                        search.GooglePlaceID = param.GooglePlaceID;
                        search.ISO3166_1_Alpha_2 = param.ISO3166_1_Alpha_2;
                        search.ISO3166_1_Alpha_3 = param.ISO3166_1_Alpha_3;
                        search.ISO3166_1_Capital = param.ISO3166_1_Capital;
                        search.ISO3166_1_Continent = param.ISO3166_1_Continent;
                        search.ISO3166_1_EDGAR = param.ISO3166_1_EDGAR;
                        search.ISO3166_1_Geoname_ID = param.ISO3166_1_Geoname_ID;
                        search.ISO3166_1_ITU = param.ISO3166_1_ITU;
                        search.ISO3166_1_Languages = param.ISO3166_1_Languages;
                        search.ISO3166_1_M49 = param.ISO3166_1_M49;
                        search.ISO3166_1_TLD = param.ISO3166_1_TLD;
                        search.ISO4217_currency_alphabetic_code = param.ISO4217_currency_alphabetic_code;
                        search.ISO4217_currency_country_name = param.ISO4217_currency_country_name;
                        search.ISO4217_currency_minor_unit = param.ISO4217_currency_minor_unit;
                        search.ISO4217_currency_name = param.ISO4217_currency_name;
                        search.ISO4217_currency_numeric_code = param.ISO4217_currency_numeric_code;
                        search.ISOofficial_name_en = param.ISOofficial_name_en;
                        search.MARC = param.MARC;
                        search.Status = param.Status;
                        search.WMO = param.WMO;

                        context.SaveChanges();
                    }
                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating country master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region StateMaster
        public List<DC_Master_State> GetAllStates()
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var state = from s in context.m_States
                                orderby s.StateName
                                select new DC_Master_State
                                {
                                    State_Id = s.State_Id,
                                    State_Name = s.StateName,
                                    Country_Id = ((s.Country_Id.HasValue) ? s.Country_Id.Value : Guid.Empty),
                                    Create_Date = s.Create_Date,
                                    Create_User = s.Create_User,
                                    Edit_Date = s.Edit_Date,
                                    Edit_User = s.Edit_User,
                                    State_Code = s.StateCode,
                                    StateName_LocalLanguage = s.StateNameLocalLanguage
                                };
                    return state.ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching state master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public List<DC_Master_State> GetStatesByCountry(Guid Country_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var state = from s in context.m_States
                                orderby s.StateName
                                where s.Country_Id == Country_Id
                                select new DC_Master_State
                                {
                                    State_Id = s.State_Id,
                                    State_Name = s.StateName,
                                    Country_Id = ((s.Country_Id.HasValue) ? s.Country_Id.Value : Guid.Empty),
                                    Create_Date = s.Create_Date,
                                    Create_User = s.Create_User,
                                    Edit_Date = s.Edit_Date,
                                    Edit_User = s.Edit_User,
                                    State_Code = s.StateCode,
                                    StateName_LocalLanguage = s.StateNameLocalLanguage
                                };
                    return state.ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching state master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public List<DC_Master_State> GetStatesMaster(DC_State_Search_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from s in context.m_States
                                 select s;

                    if (RQ.Country_Id != null)
                    {
                        search = from a in search
                                 where a.Country_Id == RQ.Country_Id
                                 select a;
                    }
                    if (RQ.State_Id != null)
                    {
                        search = from a in search
                                 where a.State_Id == RQ.State_Id
                                 select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.Country_Name))
                    {
                        search = from a in search
                                 join c in context.m_CountryMaster on a.Country_Id equals c.Country_Id
                                 where c.Name == RQ.Country_Name
                                 select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.State_code))
                    {
                        search = from a in search
                                 where a.StateCode == RQ.State_code
                                 select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.State_Name))
                    {
                        search = from a in search
                                 where a.StateName.StartsWith(RQ.State_Name)
                                 select a;
                    }

                    int total = search.Count();
                    int skip = (RQ.PageNo ?? 0) * (RQ.PageSize ?? 0);

                    var result = from a in search
                                 orderby a.StateName
                                 select new DC_Master_State
                                 {
                                     State_Id = a.State_Id,
                                     State_Name = a.StateName,
                                     Country_Id = ((a.Country_Id.HasValue) ? a.Country_Id.Value : Guid.Empty),
                                     Create_Date = a.Create_Date,
                                     Create_User = a.Create_User,
                                     Edit_Date = a.Edit_Date,
                                     Edit_User = a.Edit_User,
                                     State_Code = a.StateCode,
                                     StateName_LocalLanguage = a.StateNameLocalLanguage,
                                     TotalRecords = total
                                 };

                    return result.OrderBy(p => p.State_Name).Skip(skip).Take((RQ.PageSize ?? total)).ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching Country Master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public List<State_AlphaPage> GetStatesAlphaPaging(DC_State_Search_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {

                    var search = from s in context.m_States
                                 select s;

                    if (RQ.Country_Id != null)
                    {
                        search = from a in search
                                 where a.Country_Id == RQ.Country_Id
                                 select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.Country_Name))
                    {
                        search = from a in search
                                 join c in context.m_CountryMaster on a.Country_Id equals c.Country_Id
                                 where c.Name == RQ.Country_Name
                                 select a;
                    }

                    if (RQ.State_Id != null)
                    {
                        search = from a in search
                                 where a.Country_Id == RQ.Country_Id
                                 select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.State_Name))
                    {
                        search = from a in search
                                 where a.StateName.StartsWith(RQ.State_Name)
                                 select a;
                    }
                    var result = from a in search
                                 orderby a.StateName
                                 select new State_AlphaPage
                                 {
                                     Alpha = a.StateName.Substring(0, 1)
                                 };
                    return result.Distinct().OrderBy(p => p.Alpha).ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching City Master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public DataContracts.DC_Message AddStatesMaster(DC_Master_State param)
        {
            DataContracts.DC_Message _msg = new DataContracts.DC_Message();
            bool isExist = false;
            if (param.Country_Id == Guid.Empty || param.Country_Id == null)
            {
                _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Warning;
                _msg.StatusMessage = DataContracts.ReadOnlyMessage.strFailed;
                return _msg;
            }

            try
            {
                if (param.State_Id == Guid.Empty || param.State_Id == null)
                {
                    param.State_Id = Guid.NewGuid();
                }
                DataContracts.Masters.DC_State_Search_RQ RQ = new DataContracts.Masters.DC_State_Search_RQ();
                RQ.State_Name = param.State_Name;
                RQ.Country_Id = param.Country_Id;
                List<DC_Master_State> existState = GetStatesMaster(RQ);

                if (existState != null)
                {
                    if (existState.Count > 0)
                    {
                        _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Duplicate;
                        _msg.StatusMessage = param.State_Name + DataContracts.ReadOnlyMessage.strAlreadyExist;
                        return _msg;

                    }
                    else
                        isExist = false;
                }
                else
                    isExist = false;
                if (!isExist)
                {
                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        DataLayer.m_States objNew = new m_States();

                        objNew.State_Id = param.State_Id;
                        objNew.StateName = param.State_Name;
                        objNew.StateCode = param.State_Code;
                        objNew.Country_Id = param.Country_Id;
                        objNew.Create_Date = param.Create_Date;
                        objNew.Create_User = param.Create_User;
                        objNew.StateNameLocalLanguage = param.StateName_LocalLanguage;
                        context.m_States.Add(objNew);
                        context.SaveChanges();
                        objNew = null;

                        _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Success;
                        _msg.StatusMessage = param.State_Name + DataContracts.ReadOnlyMessage.strAddedSuccessfully;
                        return _msg;
                    }
                }
                else
                {
                    _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Duplicate;
                    _msg.StatusMessage = param.State_Name + DataContracts.ReadOnlyMessage.strAlreadyExist;
                    return _msg;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while adding state master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public DataContracts.DC_Message UpdateStatesMaster(DC_Master_State param)
        {
            DataContracts.DC_Message _msg = new DataContracts.DC_Message();
            bool IsExist = false;
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    //To check dupicate state
                    DC_State_Search_RQ RQ = new DC_State_Search_RQ();
                    RQ.State_Name = param.State_Name;
                    RQ.Country_Id = param.Country_Id;
                    var result = GetStatesMaster(RQ);
                    if (result != null)
                        if (result.Count > 0)
                        {
                            var State_ID = result[0].State_Id;
                            if (State_ID != param.State_Id)
                                IsExist = true;
                        }

                    if (!IsExist)
                    {
                        var search = context.m_States.Find(param.State_Id);
                        if (search != null)
                        {
                            search.StateName = param.State_Name;
                            search.StateCode = param.State_Code;
                            search.Country_Id = param.Country_Id;
                            search.Create_Date = param.Create_Date;
                            search.Create_User = param.Create_User;
                            search.StateNameLocalLanguage = param.StateName_LocalLanguage;
                            context.SaveChanges();
                        }
                        _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Success;
                        _msg.StatusMessage = param.State_Name + DataContracts.ReadOnlyMessage.strUpdatedSuccessfully;
                        return _msg;
                    }
                    else
                    {
                        _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Duplicate;
                        _msg.StatusMessage = param.State_Name + DataContracts.ReadOnlyMessage.strAlreadyExist;
                        return _msg;
                    }
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating state master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }


        #endregion

        #region CityMaster
        public List<string> GetCityNameList(DataContracts.Masters.DC_City_Search_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from c in context.m_CityMaster
                                 select c;

                    if (!string.IsNullOrWhiteSpace(RQ.Country_Name))
                    {
                        search = from a in search
                                 where a.CountryName.Contains(RQ.Country_Name)
                                 select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.City_Name))
                    {
                        search = from a in search
                                 where a.Name.Contains(RQ.City_Name)
                                 select a;
                    }

                    var city = (from a in search
                                where a.Status.ToUpper() == "ACTIVE"
                                orderby a.Name
                                select a.Name).ToList();

                    return city;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching Country Master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public List<DataContracts.Masters.City_AlphaPage> GetCityAlphaPaging(DataContracts.Masters.DC_City_Search_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {

                    var search = from c in context.m_CityMaster
                                 select c;

                    if (RQ.Country_Id != null)
                    {
                        search = from a in search
                                 where a.Country_Id == RQ.Country_Id
                                 select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.Country_Name))
                    {
                        search = from a in search
                                 where a.CountryName == RQ.Country_Name
                                 select a;
                    }

                    if (RQ.City_Id != null)
                    {
                        search = from a in search
                                 where a.City_Id == RQ.City_Id
                                 select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.City_Name))
                    {
                        search = from a in search
                                 where a.Name.Contains(RQ.City_Name)
                                 select a;
                    }
                    var result = from a in search
                                 select new DataContracts.Masters.City_AlphaPage
                                 {
                                     Alpha = a.Name.Substring(0, 1)
                                 };
                    return result.Distinct().OrderBy(p => p.Alpha).ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching City Master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public List<DataContracts.Masters.DC_City> GetCityMaster(DataContracts.Masters.DC_City_Search_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {

                    context.Database.CommandTimeout = 0;

                    var search = from c in context.m_CityMaster.AsNoTracking()
                                 select c;

                    var searchHotel = from h in context.Accommodation.AsNoTracking()
                                      select h;

                    var searchActivity = from ac in context.Activity_Flavour.AsNoTracking()
                                         select ac;

                    var searchSupplierCity = from ct in context.m_CityMapping.AsNoTracking()
                                             select ct;

                    if (RQ.Country_Id != null)
                    {
                        search = from a in search
                                 where a.Country_Id == RQ.Country_Id
                                 select a;
                        searchHotel = from a in searchHotel
                                      where a.Country_Id == RQ.Country_Id
                                      select a;


                        searchActivity = from ac in context.Activity_Flavour
                                         where ac.Country_Id == RQ.Country_Id
                                         select ac;

                        searchSupplierCity = from ct in searchSupplierCity
                                             where ct.Country_Id == RQ.Country_Id
                                             select ct;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.Country_Name))
                    {
                        search = from a in search
                                 where a.CountryName == RQ.Country_Name
                                 select a;
                    }

                    if (RQ.State_Id != null)
                    {
                        search = from a in search
                                 where a.State_Id == RQ.State_Id
                                 select a;
                    }

                    if (!String.IsNullOrWhiteSpace(RQ.State_Name))
                    {
                        search = from a in search
                                 where a.StateName == RQ.State_Name
                                 select a;
                    }

                    if (RQ.City_Id != null)
                    {
                        search = from a in search
                                 where a.City_Id == RQ.City_Id
                                 select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.City_Name))
                    {
                        search = from a in search
                                 where a.Name.Contains(RQ.City_Name)
                                 select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.Key))
                    {
                        search = from a in search
                                 where a.Key == RQ.Key
                                 select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.Rank))
                    {
                        search = from a in search
                                 where a.Rank == RQ.Rank
                                 select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.Priority))
                    {
                        search = from a in search
                                 where a.Priority == RQ.Priority
                                 select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.AlphaPageIndex))
                    {
                        if (RQ.AlphaPageIndex != "All")
                        {
                            search = from a in search
                                     where a.Name.StartsWith(RQ.AlphaPageIndex)
                                     select a;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(RQ.Status))
                    {
                        search = from a in search
                                 where a.Status.ToUpper() == "ACTIVE"
                                 select a;
                    }

                    int total = search.Count();
                    int skip = (RQ.PageNo ?? 0) * (RQ.PageSize ?? 0);

                    var lstCityHotelCount = (from h in searchHotel
                                             where h.City_Id != null
                                             group h by h.City_Id into grp
                                             select new
                                             {
                                                 City_Id = grp.Key ?? Guid.Empty,
                                                 Count = grp.Count(x => x.City_Id != null)
                                             }).ToList();

                    var lstCityActivityCount = (from h in searchActivity
                                                where h.City_Id != null
                                                group h by h.City_Id into grp
                                                select new
                                                {
                                                    City_Id = grp.Key ?? Guid.Empty,
                                                    ACTCount = grp.Count(x => x.City_Id != null)
                                                }).ToList();

                    var lstSupplierCityCount = (from h in searchSupplierCity
                                                where h.City_Id != null
                                                group h by h.City_Id into grp
                                                select new
                                                {
                                                    City_Id = grp.Key,
                                                    SCTCount = grp.Count()
                                                }).ToList();

                    var result = from a in search
                                 select new DataContracts.Masters.DC_City
                                 {
                                     City_Id = a.City_Id,
                                     CountryName = a.CountryName,
                                     Create_User = a.Create_User,
                                     Code = a.Code,
                                     Country_Id = a.Country_Id,
                                     Create_Date = a.Create_Date,
                                     Edit_Date = a.Edit_Date,
                                     Edit_User = a.Edit_User,
                                     Google_PlaceId = a.Google_PlaceId,
                                     Name = a.Name,
                                     StateCode = a.StateCode,
                                     StateName = a.StateName,
                                     State_Id = a.State_Id,
                                     Status = a.Status,
                                     TotalRecords = total,
                                     Latitude = a.Latitude,
                                     Longitude = a.Longitude,
                                     Key = a.Key,
                                     Rank = a.Rank,
                                     Priority = a.Priority
                                 };

                    List<DC_City> ret = new List<DC_City>();

                    ret = result.OrderBy(p => p.Name).Skip(skip).Take((RQ.PageSize ?? total)).ToList();

                    var returnList = (from a in ret
                                      join lst in lstCityHotelCount on a.City_Id equals lst.City_Id into data
                                      from dlst0 in data.DefaultIfEmpty()
                                      join lstAct in lstCityActivityCount on a.City_Id equals lstAct.City_Id into dAct
                                      from dlstAct in dAct.DefaultIfEmpty()
                                      join lstSupplierCity in lstSupplierCityCount on a.City_Id equals lstSupplierCity.City_Id into dSct
                                      from dlst in dSct.DefaultIfEmpty()
                                          //from dAct in 
                                      select new DataContracts.Masters.DC_City
                                      {
                                          City_Id = a.City_Id,
                                          CountryName = a.CountryName,
                                          Create_User = a.Create_User,
                                          Code = a.Code,
                                          Country_Id = a.Country_Id,
                                          Create_Date = a.Create_Date,
                                          Edit_Date = a.Edit_Date,
                                          Edit_User = a.Edit_User,
                                          Google_PlaceId = a.Google_PlaceId,
                                          Name = a.Name,
                                          StateCode = a.StateCode,
                                          StateName = a.StateName,
                                          State_Id = a.State_Id,
                                          Status = a.Status,
                                          TotalRecords = total,
                                          Latitude = a.Latitude,
                                          Longitude = a.Longitude,
                                          Key = a.Key,
                                          Rank = a.Rank,
                                          Priority = a.Priority,
                                          TotalHotelRecords = (dlst0 == null ? 0 : dlst0.Count),
                                          TotalAttractionsRecords = (dlstAct == null ? 0 : dlstAct.ACTCount),
                                          TotalSupplierCityRecords = (dlst == null ? 0 : dlst.SCTCount) //context.m_CityMapping.AsNoTracking().Where(w => w.City_Id == a.City_Id).Count() //
                                      }).ToList();

                    return returnList;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching City Master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public List<DataContracts.Masters.DC_City> GetCountryCityMaster(DataContracts.Masters.DC_City_Search_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    //var search = from c in context.m_CityMaster
                    //             select c;                                     

                    StringBuilder sbSelect = new StringBuilder();
                    StringBuilder sbSelectCity = new StringBuilder();
                    StringBuilder sbwhere = new StringBuilder();
                    StringBuilder sbfrom = new StringBuilder();
                    StringBuilder sbwhereCity = new StringBuilder();
                    sbSelectCity.Append("Select  City_Id , CountryName ,Create_User , Country_Id, Create_Date,Edit_Date ,Edit_User, Google_PlaceId,Name ,StateCode ,StateName,State_Id,Status");
                    sbSelect.Append("Select   CountryName ,Create_User , Country_Id, Create_Date,Edit_Date ,Edit_User, Google_PlaceId,Name ,StateCode ,StateName,State_Id,Status");
                    sbwhere.Append(" WHERE 1 = 1 ");
                    sbfrom.Append("from [m_CityMaster] a with (NoLock)");

                    if (!string.IsNullOrWhiteSpace(RQ.City_Name))
                    {
                        sbwhereCity.Append(" WHERE 1 = 1 ");
                        sbwhereCity.Append("AND a.Name ='" + RQ.City_Name + "'");
                    }
                    if (!string.IsNullOrWhiteSpace(RQ.Country_Name))
                    {
                        sbwhereCity.Append(" AND a.CountryName= '" + RQ.Country_Name + "'");
                    }
                    StringBuilder sbCityQuery = new StringBuilder();
                    sbCityQuery.Append(sbSelectCity + " ");
                    sbCityQuery.Append(" from [m_CityMaster] a with (NoLock)");
                    sbCityQuery.Append(" " + sbwhereCity + " ");


                    var retCity = context.Database.SqlQuery<DataContracts.Masters.DC_City>(sbCityQuery.ToString()).ToList();

                    if (retCity.Count == 0)
                    {
                        if (!string.IsNullOrWhiteSpace(RQ.Country_Name))
                        {
                            sbwhere.Append(" AND a.CountryName= '" + RQ.Country_Name + "'");
                        }
                        if (!String.IsNullOrWhiteSpace(RQ.State_Name))
                        {
                            sbwhere.Append("AND a.StateName = '" + (RQ.State_Name)
                                .Replace("District", "").Replace("province", "").Replace("Governorate", "").Replace("County", "").Replace("City", "").Replace("District", "")
                              .Replace("Region", "").Replace("Islands", "").Replace("Regional", "").Replace("Council", "").Replace("Prefecture", "").Replace("Territory", "")
                              .Replace("Atoll", "").Replace("North", "").Replace("Oblast", "").Replace("Republic", "").Replace("Subject", "").Replace("Krai", "").Replace("Dalmatia", "")
                             .Replace("Department", "").Replace("Area", "").Replace("Voivodeship", "").Replace("(", "").Replace(")", "").Replace("[", "").Replace("]", "").Replace(" ", "").Replace("-", "") + "'");

                        }
                        if (!string.IsNullOrWhiteSpace(RQ.Status))
                        {
                            sbwhere.Append("AND a.Status = 'ACTIVE'");
                        }
                        StringBuilder sbfinalQuery = new StringBuilder();
                        sbfinalQuery.Append(sbSelect + " ");
                        sbfinalQuery.Append(" " + sbfrom);
                        sbfinalQuery.Append(" " + sbwhere + " ");
                        var ret = context.Database.SqlQuery<DataContracts.Masters.DC_City>(sbfinalQuery.ToString()).ToList();
                        return ret;
                    }
                    else
                        return retCity;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching City Master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public string GetNextCityCodeNumber(string codePrefix)
        {
            string ret = "000";

            if (!string.IsNullOrWhiteSpace(codePrefix))
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var code = (from a in context.m_CityMaster
                                    //where CommonFunctions.RemoveNumbers(a.Code.ToString()) == CommonFunctions.RemoveNumbers(codePrefix.ToString().ToUpper())
                                where a.Code.ToString().ToUpper().StartsWith(codePrefix.ToString().ToUpper())
                                orderby a.Code descending
                                //select CommonFunctions.ReturnNumbersFromString(a.Code)).FirstOrDefault();
                                select a.Code).FirstOrDefault();

                    if (!string.IsNullOrWhiteSpace(code))
                    {
                        code = code.Replace(codePrefix, "");
                        Match m = Regex.Match(code, @"\d+");
                        int number = Convert.ToInt32(m.Value);
                        number = number + 1;
                        ret = CommonFunctions.NumberTo3CharString(number);
                    }
                }
            }

            return ret;
        }
        public DataContracts.DC_Message AddCityMaster(DataContracts.Masters.DC_City param)
        {
            DataContracts.DC_Message ret = new DataContracts.DC_Message();
            bool isExist = false;
            if (param.Country_Id == null)
            {
                ret.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Warning;
                ret.StatusMessage = DataContracts.ReadOnlyMessage.strFailed;
                return ret;
            }

            try
            {
                if (param.City_Id == null)
                {
                    param.City_Id = Guid.NewGuid();
                }
                DataContracts.Masters.DC_City_Search_RQ RQ = new DataContracts.Masters.DC_City_Search_RQ();
                RQ.City_Name = param.Name;
                RQ.State_Id = param.State_Id;
                RQ.Country_Id = param.Country_Id;
                List<DataContracts.Masters.DC_City> existCity = GetCityMaster(RQ);

                if (existCity != null)
                {
                    if (existCity.Count > 0)
                    {
                        ret.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Duplicate;
                        ret.StatusMessage = "City" + DataContracts.ReadOnlyMessage.strAlreadyExist;
                        return ret;
                    }
                    else
                        isExist = false;
                }
                else
                    isExist = false;
                if (!isExist)
                {
                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        DataLayer.m_CityMaster objNew = new m_CityMaster();

                        objNew.City_Id = param.City_Id;
                        objNew.CountryName = param.CountryName;
                        objNew.Code = CommonFunctions.GenerateCityCode(param); //param.Code;
                        objNew.Country_Id = param.Country_Id;
                        objNew.CountryCode = param.CountryCode;
                        objNew.Create_Date = param.Create_Date;
                        objNew.Create_User = param.Create_User;
                        objNew.Google_PlaceId = param.Google_PlaceId;
                        objNew.Name = param.Name;
                        objNew.Key = param.Key;
                        objNew.Rank = param.Rank;
                        objNew.Priority = param.Priority;
                        objNew.StateCode = param.StateCode;
                        objNew.StateName = param.StateName;
                        objNew.State_Id = param.State_Id;
                        objNew.Status = param.Status;
                        objNew.Latitude = param.Latitude;
                        objNew.Longitude = param.Longitude;
                        context.m_CityMaster.Add(objNew);
                        context.SaveChanges();

                        objNew = null;
                        ret.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Success;
                        ret.StatusMessage = "City" + DataContracts.ReadOnlyMessage.strAddedSuccessfully;
                        return ret;
                    }
                }
                else
                {
                    ret.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Duplicate;
                    ret.StatusMessage = "City" + DataContracts.ReadOnlyMessage.strAlreadyExist;
                    return ret;
                }
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while adding city master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public DC_Message UpdateCityMaster(DataContracts.Masters.DC_City param)
        {
            DC_Message _objMsg = new DC_Message();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    bool isDuplicate = context.m_CityMaster.Where(a => a.City_Id != param.City_Id && a.Name == param.Name && a.Country_Id == param.Country_Id && a.State_Id == param.State_Id).Count() > 0 ? true : false;

                    if (isDuplicate)
                    {
                        _objMsg.StatusCode = ReadOnlyMessage.StatusCode.Duplicate;
                        _objMsg.StatusMessage = param.Name + ReadOnlyMessage.strAlreadyExist;
                        return _objMsg;
                    }
                    var search = context.m_CityMaster.Find(param.City_Id);
                    if (search != null)
                    {
                        //search.CountryName = param.CountryName; //Commented, because inserting null value from city Manager.
                        //search.Code = param.Code;
                        search.Country_Id = param.Country_Id;
                        search.Edit_Date = param.Edit_Date;
                        search.Edit_User = param.Edit_User;
                        search.Google_PlaceId = param.Google_PlaceId;
                        search.Name = param.Name;
                        search.Key = param.Key;
                        search.Rank = param.Rank;
                        search.Priority = param.Priority;
                        search.StateCode = param.StateCode;
                        search.StateName = param.StateName;
                        //search.State_Id = param.State_Id;
                        //search.Status = param.Status;

                    }
                    else
                    {
                        _objMsg.StatusMessage = ReadOnlyMessage.strFailed;
                        _objMsg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                        return _objMsg;
                    }
                    context.SaveChanges();
                    _objMsg.StatusMessage = ReadOnlyMessage.strUpdatedSuccessfully;
                    _objMsg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                }
                return _objMsg;
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating city master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region DynamicAttributes
        public List<DataContracts.Masters.DC_DynamicAttributes> GetDynamicAttributes(DataContracts.Masters.DC_DynamicAttributes RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from DA in context.DynamicAttributes
                                 select DA;

                    if (RQ.AttributeClass != null)
                    {
                        search = from a in search
                                 where a.AttributeClass == RQ.AttributeClass
                                 select a;
                    }

                    if (RQ.AttributeName != null)
                    {
                        search = from a in search
                                 where a.AttributeName == RQ.AttributeName
                                 select a;
                    }

                    if (RQ.AttributeValue != null)
                    {
                        search = from a in search
                                 where a.AttributeValue == RQ.AttributeValue
                                 select a;
                    }

                    if (RQ.DynamicAttribute_Id != null)
                    {
                        if (RQ.DynamicAttribute_Id != Guid.Empty)
                            search = from a in search
                                     where a.DynamicAttribute_Id == RQ.DynamicAttribute_Id
                                     select a;
                    }

                    if (RQ.ObjectType != null)
                    {
                        search = from a in search
                                 where a.ObjectType == RQ.ObjectType
                                 select a;
                    }

                    if (RQ.Object_Id != null)
                    {
                        if (RQ.Object_Id != Guid.Empty)
                            search = from a in search
                                     where a.Object_Id == RQ.Object_Id
                                     select a;
                    }

                    if (RQ.ObjectSubElement_Id != null)
                    {
                        if (RQ.ObjectSubElement_Id != Guid.Empty)
                            search = from a in search
                                     where a.ObjectSubElement_Id == RQ.Object_Id
                                     select a;
                    }

                    var result = from a in search
                                 select new DataContracts.Masters.DC_DynamicAttributes
                                 {
                                     AttributeClass = a.AttributeClass,
                                     AttributeName = a.AttributeName,
                                     AttributeValue = a.AttributeValue,
                                     DynamicAttribute_Id = a.DynamicAttribute_Id,
                                     ObjectSubElement_Id = a.ObjectSubElement_Id,
                                     ObjectType = a.ObjectType,
                                     Object_Id = a.Object_Id,
                                     IsActive = (a.IsActive ?? true),
                                     Create_Date = a.Create_Date,
                                     Create_User = a.Create_User,
                                     Edit_Date = a.Edit_Date,
                                     Edit_User = a.Edit_User
                                 };

                    return result.ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching dynamic attributes", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool UpdateDynamicAttributes(DataContracts.Masters.DC_DynamicAttributes obj)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = (from da in context.DynamicAttributes
                                  where da.DynamicAttribute_Id == obj.DynamicAttribute_Id
                                  select da).FirstOrDefault();
                    if (search != null)
                    {
                        if ((obj.IsActive) != (search.IsActive ?? true))
                        {
                            search.IsActive = obj.IsActive;
                            search.Edit_Date = obj.Edit_Date;
                            search.Edit_User = obj.Edit_User;
                        }
                        else
                        {
                            search.AttributeClass = obj.AttributeClass;
                            search.AttributeName = obj.AttributeName;
                            search.AttributeValue = obj.AttributeValue;
                            search.ObjectSubElement_Id = obj.ObjectSubElement_Id;
                            search.ObjectType = obj.ObjectType;
                            search.Object_Id = obj.Object_Id;
                            search.IsActive = obj.IsActive;
                            search.Edit_Date = obj.Edit_Date;
                            search.Edit_User = obj.Edit_User;
                        }

                        context.SaveChanges();
                    }
                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating dynamic attributes", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool AddDynamicAttributes(DataContracts.Masters.DC_DynamicAttributes obj)
        {
            try
            {
                if (obj.DynamicAttribute_Id == null)
                {
                    obj.DynamicAttribute_Id = Guid.NewGuid();
                }
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    DataLayer.DynamicAttribute objNew = new DynamicAttribute();
                    objNew.DynamicAttribute_Id = obj.DynamicAttribute_Id;
                    objNew.AttributeClass = obj.AttributeClass;
                    objNew.AttributeName = obj.AttributeName;
                    objNew.AttributeValue = obj.AttributeValue;
                    objNew.ObjectSubElement_Id = obj.ObjectSubElement_Id;
                    objNew.ObjectType = obj.ObjectType;
                    objNew.Object_Id = obj.Object_Id;
                    objNew.IsActive = obj.IsActive;
                    objNew.Create_Date = obj.Create_Date;
                    objNew.Create_User = obj.Create_User;

                    context.DynamicAttributes.Add(objNew);
                    context.SaveChanges();

                    objNew = null;
                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while adding dynamic attributes", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region Team Masters
        public List<DataContracts.DC_Teams> GetTeamMasterData(Guid Team_ID)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from DA in context.m_Teams
                                 select DA;

                    if (Team_ID != Guid.Empty)
                    {
                        search = from a in search
                                 where a.Team_ID == Team_ID
                                 select a;
                    }
                    else
                    {
                        search = from a in search
                                 select a;
                    }
                    var teams = from c in search
                                orderby c.Team_Name
                                select new DataContracts.DC_Teams
                                {
                                    Team_ID = c.Team_ID,
                                    Team_Name = c.Team_Name,
                                    Status = c.Status,
                                    Create_Date = c.CREATE_DATE,
                                    Create_User = c.CREATE_USER,
                                    Edit_Date = c.UPDATE_DATE,
                                    Edit_User = c.UPDATE_USER
                                };
                    return teams.ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching Team master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool UpdateTeamMasterData(DataContracts.DC_Teams obj)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = (from da in context.m_Teams
                                  where da.Team_ID == obj.Team_ID
                                  select da).First();
                    if (search != null)
                    {
                        if (obj.Status != search.Status)
                        {
                            search.Status = obj.Status;
                            search.UPDATE_DATE = obj.Edit_Date;
                            search.UPDATE_USER = obj.Edit_User;
                        }
                        else
                        {
                            search.Team_Name = obj.Team_Name;
                            search.Status = obj.Status;
                            search.UPDATE_DATE = obj.Edit_Date;
                            search.UPDATE_USER = obj.Edit_User;
                        }

                        context.SaveChanges();
                    }
                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating Team Master Data", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool AddTeamMasterData(DataContracts.DC_Teams obj)
        {
            try
            {
                if (obj.Team_ID == null)
                {
                    obj.Team_ID = Guid.NewGuid();
                }
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    DataLayer.m_Teams objNew = new m_Teams();
                    objNew.Team_ID = obj.Team_ID;
                    objNew.Team_Name = obj.Team_Name;
                    objNew.Status = obj.Status;
                    objNew.CREATE_DATE = obj.Create_Date;
                    objNew.CREATE_USER = obj.Create_User;

                    context.m_Teams.Add(objNew);
                    context.SaveChanges();

                    objNew = null;
                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while adding Team Master Data", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region Update Country State City Area Location Hierarchy
        public bool UpdateAddressMasterHierarchy(DataContracts.DC_Address.DC_Country_State_City_Area_Location RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    Guid Country_Id;
                    Guid State_Id;
                    Guid City_Id;
                    Guid Area_Id;
                    Guid Location_Id;
                    string User = "SYSTEM GEO LOOKUP";

                    if (string.IsNullOrWhiteSpace(RQ.Country) || string.IsNullOrWhiteSpace(RQ.City))
                    {
                        return false;
                    }

                    if (string.IsNullOrWhiteSpace(RQ.Area) && !string.IsNullOrWhiteSpace(RQ.Location))
                    {
                        return false;
                    }

                    //Country
                    var Country = (from country in context.m_CountryMaster
                                   where country.Name.Trim() == RQ.Country.Trim()
                                   select country).FirstOrDefault();

                    if (Country == null)
                    {
                        m_CountryMaster cm = new m_CountryMaster();
                        cm.Country_Id = Guid.NewGuid();
                        Country_Id = cm.Country_Id;
                        cm.Name = RQ.Country.Trim();
                        cm.Create_User = User;
                        cm.Create_Date = DateTime.Now;
                        cm.Status = "ACTIVE";

                        context.m_CountryMaster.Add(cm);

                        context.SaveChanges();

                        cm = null;
                    }
                    else if (Country.Status != "ACTIVE")
                    {
                        Country.Status = "ACTIVE";
                        Country.Edit_Date = DateTime.Now;
                        Country.Edit_User = User;

                        Country_Id = Country.Country_Id;

                        context.SaveChanges();
                    }
                    else
                    {
                        Country_Id = Country.Country_Id;
                    }
                    Country = null;


                    //State
                    if (!string.IsNullOrWhiteSpace(RQ.State))
                    {
                        var State = (from states in context.m_States
                                     where states.StateName.Trim() == RQ.State.Trim()
                                     select states).FirstOrDefault();

                        if (State == null)
                        {
                            m_States st = new m_States();
                            st.State_Id = Guid.NewGuid();
                            State_Id = st.State_Id;
                            st.StateName = RQ.State.Trim();
                            st.Create_User = User;
                            st.Create_Date = DateTime.Now;
                            st.Country_Id = Country_Id;

                            context.m_States.Add(st);

                            context.SaveChanges();

                            st = null;
                        }
                        else
                        {
                            State_Id = State.State_Id;
                        }
                        State = null;
                    }


                    //City

                    var City = (from city in context.m_CityMaster
                                where city.Country_Id == Country_Id
                                && city.Name.Trim() == RQ.City.Trim()
                                select city).FirstOrDefault();

                    if (City == null)
                    {
                        m_CityMaster cm = new m_CityMaster();
                        cm.City_Id = Guid.NewGuid();
                        City_Id = cm.City_Id;
                        cm.CountryName = (from c in context.m_CountryMaster where c.Country_Id == Country_Id select c.Name).First();
                        cm.Name = RQ.City.Trim();
                        cm.Country_Id = Country_Id;
                        cm.Create_Date = DateTime.Now;
                        cm.Create_User = User;
                        cm.Status = "ACTIVE";

                        context.m_CityMaster.Add(cm);

                        context.SaveChanges();

                        cm = null;
                    }
                    else if (City.Status != "ACTIVE")
                    {
                        City.Status = "ACTIVE";
                        City.Edit_Date = DateTime.Now;
                        City.Edit_User = User;

                        City_Id = City.City_Id;

                        context.SaveChanges();
                    }
                    else
                    {
                        City_Id = City.City_Id;
                    }
                    City = null;


                    if (!string.IsNullOrWhiteSpace(RQ.Area))
                    {
                        //Area
                        var Area = (from area in context.m_CityArea
                                    where area.City_Id == City_Id
                                    && area.Name.Trim() == RQ.Area.Trim()
                                    select area).FirstOrDefault();

                        if (Area == null)
                        {
                            m_CityArea ca = new m_CityArea();
                            ca.CityArea_Id = Guid.NewGuid();
                            Area_Id = ca.CityArea_Id;
                            ca.City_Id = City_Id;
                            ca.Create_Date = DateTime.Now;
                            ca.Create_User = User;
                            ca.Name = RQ.Area.Trim();

                            context.m_CityArea.Add(ca);

                            context.SaveChanges();

                            ca = null;
                        }
                        else
                        {
                            Area_Id = Area.CityArea_Id;
                        }
                        Area = null;


                        //Location
                        if (!string.IsNullOrWhiteSpace(RQ.Location))
                        {
                            var Location = (from loc in context.m_CityAreaLocation
                                            where loc.CityArea_Id == Area_Id
                                            && loc.City_Id == City_Id
                                            && loc.Name.Trim() == RQ.Location.Trim()
                                            select loc).FirstOrDefault();

                            if (Location == null)
                            {
                                m_CityAreaLocation cal = new m_CityAreaLocation();
                                cal.CityAreaLocation_Id = Guid.NewGuid();
                                Location_Id = cal.CityAreaLocation_Id;
                                cal.CityArea_Id = Area_Id;
                                cal.City_Id = City_Id;
                                cal.Create_Date = DateTime.Now;
                                cal.Create_User = User;
                                cal.Name = RQ.Location.Trim();

                                context.m_CityAreaLocation.Add(cal);

                                context.SaveChanges();

                                cal = null;
                            }
                            else
                            {
                                Location_Id = Location.CityAreaLocation_Id;
                            }
                            Location = null;
                        }
                    }

                    return true;

                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating address master hierarchy", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region Port Master
        public List<DataContracts.Masters.DC_PortMaster> GetPort(int PageSize, int PageNo, Guid Port_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var portSearch = from a in context.m_PortMaster select a;

                    if (Port_Id != Guid.Empty)
                    {
                        portSearch = from a in portSearch
                                     where a.m_Port_Id == Port_Id
                                     select a;
                    }

                    int total;

                    total = portSearch.Count();

                    var skip = PageSize * PageNo;

                    var canPage = skip < total;

                    var portList = (from a in portSearch
                                    join cn in context.m_CountryMaster on a.Country_Id equals cn.Country_Id into cn_temp
                                    from cn_data in cn_temp.DefaultIfEmpty()
                                    join ct in context.m_CityMaster on a.City_Id equals ct.City_Id into ct_temp
                                    from ct_data in ct_temp.DefaultIfEmpty()
                                    join st in context.m_States on a.State_Id equals st.State_Id into st_temp
                                    from st_data in st_temp.DefaultIfEmpty()
                                    join mct in context.m_CityMaster on a.MultiCity_Id equals mct.City_Id into mct_temp
                                    from mct_data in mct_temp.DefaultIfEmpty()
                                    orderby a.oag_portname
                                    select new DataContracts.Masters.DC_PortMaster
                                    {
                                        Port_Id = a.m_Port_Id,
                                        CityCode = ct_data.Code,
                                        CityName = ct_data.Name,
                                        City_Id = a.City_Id,
                                        CountryCode = cn_data.Code,
                                        CountryName = cn_data.Code,
                                        Country_Id = a.Country_Id,
                                        MappingStatus = a.MappingStatus,
                                        Oag_ctry = a.oag_ctry,
                                        Oag_ctryname = a.oag_ctryname,
                                        Oag_inactive = a.oag_inactive,
                                        Oag_lat = a.oag_lat,
                                        OAG_loc = a.OAG_loc,
                                        Oag_lon = a.oag_lon,
                                        OAG_multicity = a.OAG_multicity,
                                        Oag_name = a.oag_name,
                                        Oag_portname = a.oag_portname,
                                        Oag_state = a.oag_state,
                                        Oag_subctry = a.oag_subctry,
                                        Oag_substate = a.oag_substate,
                                        OAG_subtype = a.OAG_subtype,
                                        Oag_timediv = a.oag_timediv,
                                        OAG_type = a.OAG_type,
                                        StateCode = st_data.StateCode,
                                        StateName = st_data.StateName,
                                        State_Id = a.State_Id,
                                        MultiCity_Id = a.MultiCity_Id,
                                        MultiCityCode = mct_data.Code,
                                        MultiCityName = mct_data.Name,
                                        TotalRecords = total
                                    }).Skip(skip).Take(PageSize);


                    return portList.ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching port master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public List<DataContracts.Masters.DC_PortMaster> PortMasterSeach(DataContracts.Masters.DC_PortMaster_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var portSearch = from a in context.m_PortMaster select a;

                    if (!string.IsNullOrWhiteSpace(RQ.Port_Country_Name))
                    {
                        portSearch = from a in portSearch
                                     where a.oag_ctryname.StartsWith(RQ.Port_Country_Name)
                                     select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.Port_City_Name))
                    {
                        portSearch = from a in portSearch
                                     where a.oag_name.StartsWith(RQ.Port_City_Name)
                                     select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.Mapping_Status))
                    {
                        portSearch = from a in portSearch
                                     where a.MappingStatus == RQ.Mapping_Status
                                     select a;
                    }
                    if (RQ.Country_Id.HasValue)
                    {
                        if (RQ.Country_Id.Value != Guid.Empty)
                        {
                            portSearch = from a in portSearch
                                         where a.Country_Id == RQ.Country_Id
                                         select a;
                        }
                    }
                    if (RQ.City_Id.HasValue)
                    {
                        if (RQ.City_Id.Value != Guid.Empty)
                        {
                            portSearch = from a in portSearch
                                         where a.City_Id == RQ.City_Id
                                         select a;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(RQ.Oag_portname))
                    {

                        portSearch = from a in portSearch
                                     where a.oag_portname == RQ.Oag_portname
                                     select a;
                    }
                    if (RQ.Port_Id.HasValue)
                    {
                        if (RQ.Port_Id.Value != Guid.Empty)
                        {
                            portSearch = from a in portSearch
                                         where a.m_Port_Id == RQ.Port_Id
                                         select a;
                        }
                    }

                    int total;

                    total = portSearch.Count();
                    if (RQ.PageSize == 0)
                        RQ.PageSize = 10;

                    var skip = RQ.PageSize * RQ.PageNo;

                    var canPage = skip < total;

                    var portList = (from a in portSearch
                                    join cn in context.m_CountryMaster on a.Country_Id equals cn.Country_Id into cn_temp
                                    from cn_data in cn_temp.DefaultIfEmpty()
                                    join ct in context.m_CityMaster on a.City_Id equals ct.City_Id into ct_temp
                                    from ct_data in ct_temp.DefaultIfEmpty()
                                    join st in context.m_States on a.State_Id equals st.State_Id into st_temp
                                    from st_data in st_temp.DefaultIfEmpty()
                                    join mct in context.m_CityMaster on a.MultiCity_Id equals mct.City_Id into mct_temp
                                    from mct_data in mct_temp.DefaultIfEmpty()
                                    orderby a.oag_portname
                                    select new DataContracts.Masters.DC_PortMaster
                                    {
                                        Port_Id = a.m_Port_Id,
                                        CityCode = ct_data.Code,
                                        CityName = ct_data.Name,
                                        City_Id = a.City_Id,
                                        CountryCode = cn_data.Code,
                                        CountryName = cn_data.Name,
                                        Country_Id = a.Country_Id,
                                        MappingStatus = a.MappingStatus,
                                        Oag_ctry = a.oag_ctry,
                                        Oag_ctryname = a.oag_ctryname,
                                        Oag_inactive = a.oag_inactive,
                                        Oag_lat = a.oag_lat,
                                        OAG_loc = a.OAG_loc,
                                        Oag_lon = a.oag_lon,
                                        OAG_multicity = a.OAG_multicity,
                                        Oag_name = a.oag_name,
                                        Oag_portname = a.oag_portname,
                                        Oag_state = a.oag_state,
                                        Oag_subctry = a.oag_subctry,
                                        Oag_substate = a.oag_substate,
                                        OAG_subtype = a.OAG_subtype,
                                        Oag_timediv = a.oag_timediv,
                                        OAG_type = a.OAG_type,
                                        StateCode = st_data.StateCode,
                                        StateName = st_data.StateName,
                                        State_Id = a.State_Id,
                                        MultiCity_Id = a.MultiCity_Id,
                                        MultiCityCode = mct_data.Code,
                                        MultiCityName = mct_data.Name,
                                        TotalRecords = total

                                    }).Skip(skip).Take(RQ.PageSize);


                    return portList.ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching port master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }


        public DC_Message UpdatePortMaster(DC_PortMaster param)
        {
            DC_Message _msg = new DC_Message();
            try
            {
                if (param.Port_Id == null)
                {
                    _msg.StatusMessage = ReadOnlyMessage.strFailed;
                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                    return _msg;
                }

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var portSearch = context.m_PortMaster.Find(param.Port_Id);
                    if (portSearch == null)
                    {
                        _msg.StatusMessage = ReadOnlyMessage.strFailed;
                        _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                        return _msg;
                    }
                    else
                    {
                        //Find duplicate port name
                        var result = PortMasterSeach(new DC_PortMaster_RQ() { Oag_portname = param.Oag_portname, City_Id = param.City_Id, Country_Id = param.Country_Id });
                        if (result != null)
                        {
                            if (result.Count < 2)
                            {
                                portSearch.City_Id = param.City_Id;
                                portSearch.Country_Id = param.Country_Id;
                                portSearch.MultiCity_Id = param.MultiCity_Id;
                                portSearch.State_Id = param.State_Id;
                                portSearch.MappingStatus = param.MappingStatus;
                                context.SaveChanges();
                            }
                            else
                            {
                                _msg.StatusMessage = ReadOnlyMessage.strAlreadyExist;
                                _msg.StatusCode = ReadOnlyMessage.StatusCode.Duplicate;
                                return _msg;
                            }
                        }
                    }
                    _msg.StatusMessage = ReadOnlyMessage.strUpdatedSuccessfully;
                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                    return _msg;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus
                {
                    ErrorMessage = "Error while saving port master",
                    ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError
                });
            }
        }
        public DC_Message AddPortMaster(DC_PortMaster param)
        {
            DC_Message _msg = new DC_Message();
            try
            {
                if (param.Port_Id == null)
                    param.Port_Id = Guid.NewGuid();

                //Find Duplicate
                DC_PortMaster_RQ _newPortSearch = new DC_PortMaster_RQ();
                _newPortSearch.Oag_portname = param.Oag_portname;
                _newPortSearch.Country_Id = param.Country_Id;
                _newPortSearch.City_Id = param.City_Id;
                var result = PortMasterSeach(_newPortSearch);
                if (result != null || result.Count == 0)
                {
                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        DataLayer.m_PortMaster _newObj = new DataLayer.m_PortMaster();
                        _newObj.m_Port_Id = param.Port_Id;
                        _newObj.OAG_loc = param.OAG_loc;
                        _newObj.OAG_multicity = param.OAG_multicity;
                        _newObj.OAG_type = param.OAG_type;
                        _newObj.OAG_subtype = param.OAG_subtype;
                        _newObj.oag_name = param.Oag_name;
                        _newObj.oag_portname = param.Oag_portname;
                        _newObj.oag_ctry = param.Oag_ctry;
                        _newObj.oag_subctry = param.Oag_subctry;
                        _newObj.oag_ctryname = param.Oag_ctryname;
                        _newObj.oag_state = param.Oag_state;
                        _newObj.oag_substate = param.Oag_substate;
                        _newObj.oag_timediv = param.Oag_timediv;
                        _newObj.oag_lat = param.Oag_lat;
                        _newObj.oag_lon = param.Oag_lon;
                        _newObj.oag_inactive = param.Oag_inactive;
                        _newObj.Country_Id = param.Country_Id;
                        _newObj.State_Id = param.State_Id;
                        _newObj.City_Id = param.City_Id;
                        _newObj.MappingStatus = param.MappingStatus;
                        _newObj.MultiCity_Id = param.MultiCity_Id;
                        context.m_PortMaster.Add(_newObj);
                        context.SaveChanges();
                        _msg.StatusMessage = param.Oag_portname + ReadOnlyMessage.strAddedSuccessfully;
                        _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        return _msg;
                    }
                }
                else
                {
                    _msg.StatusMessage = param.Oag_portname + ReadOnlyMessage.strAlreadyExist;
                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Duplicate;
                    return _msg;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while saving port master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region Master Attribute
        public IList<DataContracts.Masters.DC_MasterAttribute> GetAllAttributeAndValuesByFOR(DataContracts.Masters.DC_MasterAttribute _obj)
        {
            try
            {
                var retclass = new DC_MasterAttribute();
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from ma in context.m_masterattribute
                                 join mac in context.m_masterattributevalue on ma.MasterAttribute_Id equals mac.MasterAttribute_Id
                                 where ma.MasterFor == _obj.MasterFor
                                 orderby mac.AttributeValue
                                 select new DC_MasterAttribute
                                 {
                                     MasterAttributeValue_Id = mac.MasterAttributeValue_Id,
                                     AttributeValue = mac.AttributeValue
                                 };

                    return search.ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching port master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public IList<DataContracts.Masters.DC_MasterAttribute> GetAllAttributeAndValuesByParentAttributeValue(DataContracts.Masters.DC_MasterAttribute _obj)
        {
            try
            {
                List<DC_MasterAttribute> _lstMasterAttribute = new List<DC_MasterAttribute>();

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from ma in context.m_masterattribute select ma;
                    if (!string.IsNullOrWhiteSpace(_obj.MasterFor))
                        search = from ma in search where ma.MasterFor == _obj.MasterFor select ma;

                    if (_obj.ParentAttributeValue_Id.HasValue)
                    {
                        if (_obj.ParentAttributeValue_Id.Value != Guid.Empty)
                            _lstMasterAttribute = (from ma in search
                                                   join mac in context.m_masterattributevalue on ma.MasterAttribute_Id equals mac.MasterAttribute_Id
                                                   where mac.ParentAttributeValue_Id == _obj.ParentAttributeValue_Id
                                                   orderby mac.AttributeValue
                                                   select new DC_MasterAttribute
                                                   {
                                                       MasterAttributeValue_Id = mac.MasterAttributeValue_Id,
                                                       AttributeValue = mac.AttributeValue
                                                   }).ToList();
                    }
                    if (_obj.ParentAttributeValue_Id.HasValue)
                    {
                        //Get partent_id 
                        //Guid _prentAttributeValue_Id = context.m_masterattributevalue.Where(p => p.AttributeValue.ToLower() == _obj.AttributeValue.ToLower()).FirstOrDefault().MasterAttributeValue_Id;
                        Guid _prentAttributeValue_Id = _obj.ParentAttributeValue_Id.Value;
                        if (_prentAttributeValue_Id != Guid.Empty)
                        {
                            _lstMasterAttribute = (from mac in context.m_masterattributevalue
                                                   where mac.ParentAttributeValue_Id == _prentAttributeValue_Id
                                                   select new DC_MasterAttribute
                                                   {
                                                       MasterAttributeValue_Id = mac.MasterAttributeValue_Id,
                                                       AttributeValue = mac.AttributeValue
                                                   }).ToList();
                        }

                    }
                    return _lstMasterAttribute;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IList<DataContracts.Masters.DC_MasterAttribute> GetAllAttributeAndValues(DataContracts.Masters.DC_MasterAttribute _obj)
        {
            try
            {
                var retclass = new DC_MasterAttribute();
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = (from ma in context.m_masterattribute
                                  where ma.MasterFor == _obj.MasterFor
                                  select ma);

                    if (!String.IsNullOrEmpty(_obj.Name))
                    {
                        search = from ma in search
                                 where ma.Name.Trim().ToLower() == _obj.Name.Trim().ToLower()
                                 select ma;
                    }

                    var searchresult = from ma in search
                                       join mac in context.m_masterattributevalue on ma.MasterAttribute_Id equals mac.MasterAttribute_Id
                                       orderby mac.AttributeValue
                                       select new DC_MasterAttribute
                                       {
                                           MasterAttributeValue_Id = mac.MasterAttributeValue_Id,
                                           AttributeValue = mac.AttributeValue,
                                           ParentAttributeValue_Id = mac.ParentAttributeValue_Id
                                       };

                    if (!string.IsNullOrEmpty(Convert.ToString(_obj.ParentAttributeValue_Id)))
                    {
                        searchresult = from mac in searchresult
                                       where mac.ParentAttributeValue_Id == _obj.ParentAttributeValue_Id
                                       select mac;
                    }

                    return searchresult.ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching port master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public IList<DC_M_masterattribute> GetMasterAttributes(DC_M_masterattribute _obj)
        {
            using (ConsumerEntities context = new ConsumerEntities())
            {
                var search = (from d in context.m_masterattribute select d).AsQueryable();

                bool isActive = Convert.ToBoolean(Convert.ToInt32(_obj.IsActive));
                if (!string.IsNullOrWhiteSpace(_obj.MasterFor))
                {
                    search = (from x in search where x.MasterFor == _obj.MasterFor select x);
                }
                if (!string.IsNullOrWhiteSpace(_obj.OTA_CodeTableCode))
                    search = (from x in search where x.OTA_CodeTableCode == _obj.OTA_CodeTableCode select x);
                if (!string.IsNullOrWhiteSpace(_obj.OTA_CodeTableName))
                    search = (from x in search where x.OTA_CodeTableName == _obj.OTA_CodeTableName select x);
                if (!string.IsNullOrWhiteSpace(_obj.Name))
                    search = (from x in search where x.Name.ToString().ToLower().Contains(_obj.Name.ToLower()) select x);
                //if (_obj.MasterAttribute_Id != Guid.Empty)
                //    search = (from x in search where x.MasterAttribute_Id == _obj.MasterAttribute_Id select x);
                if (_obj.ParentAttribute_Id.HasValue)
                    if (_obj.ParentAttribute_Id != Guid.Empty)
                        search = (from x in search where x.ParentAttribute_Id == _obj.ParentAttribute_Id select x);
                if (!String.IsNullOrWhiteSpace(_obj.IsActive))
                    search = (from x in search where x.IsActive == isActive select x);

                int total = search.Count();
                int skip = (_obj.PageNo ?? 0) * (_obj.PageSize ?? 0);


                var result = from d in search
                             join pa in context.m_masterattribute on d.ParentAttribute_Id equals pa.MasterAttribute_Id into ps
                             from pa in ps.DefaultIfEmpty()
                             orderby d.Name
                             select new DC_M_masterattribute
                             {
                                 MasterAttribute_Id = d.MasterAttribute_Id,
                                 Name = d.Name,
                                 MasterFor = d.MasterFor ?? "",
                                 OTA_CodeTableCode = d.OTA_CodeTableCode ?? "",
                                 OTA_CodeTableName = d.OTA_CodeTableName ?? "",
                                 ParentAttributeName = pa == null ? "" : pa.Name,
                                 ParentAttribute_Id = d.ParentAttribute_Id ?? Guid.Empty,
                                 IsActive = d.IsActive ?? false == true ? "Y" : "N",
                                 TotalRecords = total
                             };
                return result.OrderBy(p => p.Name).Skip(skip).Take((_obj.PageSize ?? total)).ToList();
            }
        }
        public IList<DC_M_masterparentattributes> GetParentAttributes(string sMasterFor)
        {
            using (ConsumerEntities context = new ConsumerEntities())
            {
                var result = from ma in context.m_masterattribute
                             orderby ma.Name
                             where ma.MasterFor == sMasterFor
                             select new DC_M_masterparentattributes
                             {
                                 ParentAttribute_Id = ma.MasterAttribute_Id,
                                 ParentAttributeName = ma.Name,
                                 IsActive = ma.IsActive ?? true
                             };
                return result.ToList();
            }
        }
        public IList<DC_M_masterparentattributes> GetAttributesForValues(Guid MasterAttribute_id)
        {
            using (ConsumerEntities context = new ConsumerEntities())
            {
                var result = from ma in context.m_masterattribute
                             join mav in context.m_masterattributevalue on ma.MasterAttribute_Id equals mav.MasterAttribute_Id
                             orderby mav.AttributeValue
                             where mav.MasterAttribute_Id == MasterAttribute_id
                             select new DC_M_masterparentattributes
                             {
                                 ParentAttribute_Id = mav.MasterAttributeValue_Id,
                                 ParentAttributeName = mav.AttributeValue,
                                 IsActive = mav.IsActive ?? true
                             };
                return result.ToList();
            }
        }
        public IList<DC_M_masterparentattributes> GetAttributesMasterForValues()
        {
            using (ConsumerEntities context = new ConsumerEntities())
            {
                var result = from ma in context.m_masterattribute
                             join mav in context.m_masterattributevalue on ma.MasterAttribute_Id equals mav.MasterAttribute_Id
                             orderby mav.AttributeValue
                             where ma.Name == "SystemConfig"
                             select new DC_M_masterparentattributes
                             {
                                 ParentAttribute_Id = mav.MasterAttributeValue_Id,
                                 ParentAttributeName = mav.AttributeValue,
                                 IsActive = mav.IsActive ?? true
                             };
                return result.ToList();
            }
        }
        public IList<DC_M_masterattributevalue> GetAttributeValues(string MasterAttribute_Id, int PageSize, int PageNo)
        {
            Guid gMasterAttribute_Id = Guid.Parse(MasterAttribute_Id);
            using (ConsumerEntities context = new ConsumerEntities())
            {
                var total = (from mav in context.m_masterattributevalue
                             where mav.MasterAttribute_Id == gMasterAttribute_Id
                             select mav).Count();

                int skip = (PageNo) * (PageSize);

                var search = from mav in context.m_masterattributevalue
                             join pav in context.m_masterattributevalue on mav.ParentAttributeValue_Id equals pav.MasterAttributeValue_Id into paval
                             from pavalid in paval.DefaultIfEmpty()
                             orderby mav.AttributeValue
                             where mav.MasterAttribute_Id == gMasterAttribute_Id
                             select new DC_M_masterattributevalue
                             {
                                 MasterAttribute_Id = mav.MasterAttribute_Id,
                                 MasterAttributeValue_Id = mav.MasterAttributeValue_Id,
                                 AttributeValue = mav.AttributeValue ?? "",
                                 OTA_CodeTableValue = mav.OTA_CodeTableValue ?? "",
                                 IsActive = mav.IsActive ?? false == true ? "Y" : "N",
                                 ParentAttributeValue_Id = pavalid.MasterAttributeValue_Id,
                                 ParentAttributeValue = pavalid.AttributeValue,
                                 TotalCount = total
                             };


                return search.OrderBy(p => p.AttributeValue).Skip(skip).Take(PageSize == 0 ? total : PageSize).ToList();
            }
        }
        public DC_M_masterattribute GetAttributeDetails(Guid MasterAttribute_Id)
        {
            // Guid _MasterAttribute_Id = Guid.Parse(MasterAttribute_Id.ToString());
            using (ConsumerEntities context = new ConsumerEntities())
            {
                var result = context.m_masterattribute.Find(MasterAttribute_Id);
                return new DC_M_masterattribute
                {
                    IsActive = result.IsActive ?? true ? "Y" : "N",
                    MasterAttribute_Id = result.MasterAttribute_Id,
                    Name = result.Name,
                    OTA_CodeTableCode = result.OTA_CodeTableCode,
                    OTA_CodeTableName = result.OTA_CodeTableName,
                    MasterFor = result.MasterFor,
                    ParentAttribute_Id = result.ParentAttribute_Id
                };
            }
        }
        public DC_Message SaveMasterAttribute(DC_M_masterattribute obj)
        {
            DC_Message _objMsg = new DC_Message();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    //Check duplicate 
                    var isduplicate = (from attr in context.m_masterattribute
                                       where attr.MasterAttribute_Id != obj.MasterAttribute_Id && attr.Name == obj.Name && attr.MasterFor == obj.MasterFor
                                       select attr).Count() == 0 ? false : true;
                    if (isduplicate)
                    {
                        _objMsg.StatusCode = ReadOnlyMessage.StatusCode.Duplicate;
                        _objMsg.StatusMessage = obj.Name + ReadOnlyMessage.strAlreadyExist;
                        return _objMsg;
                    }
                    if (obj.Action == "Save")
                    {
                        m_masterattribute objIns = new m_masterattribute();
                        objIns.MasterAttribute_Id = obj.MasterAttribute_Id;
                        objIns.Name = obj.Name;
                        objIns.MasterFor = obj.MasterFor;
                        objIns.OTA_CodeTableCode = obj.OTA_CodeTableCode;
                        objIns.OTA_CodeTableName = obj.OTA_CodeTableName;
                        objIns.ParentAttribute_Id = obj.ParentAttribute_Id;
                        objIns.IsActive = obj.IsActive == "Y" ? true : false;
                        context.m_masterattribute.Add(objIns);
                    }
                    else if (obj.Action == "Update")
                    {
                        var data = context.m_masterattribute.Find(obj.MasterAttribute_Id);
                        data.MasterFor = obj.MasterFor;
                        data.Name = obj.Name;
                        data.OTA_CodeTableCode = obj.OTA_CodeTableCode;
                        data.OTA_CodeTableName = obj.OTA_CodeTableName;
                        data.ParentAttribute_Id = obj.ParentAttribute_Id;
                        data.IsActive = obj.IsActive == "Y" ? true : false;

                    }
                    context.SaveChanges();
                    _objMsg.StatusMessage = obj.Action == "Update" ? ReadOnlyMessage.strUpdatedSuccessfully : ReadOnlyMessage.strAddedSuccessfully;
                    _objMsg.StatusCode = ReadOnlyMessage.StatusCode.Success;


                }
                return _objMsg;
            }
            catch
            {
                _objMsg.StatusMessage = ReadOnlyMessage.strFailed;
                _objMsg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                return _objMsg;
            }

        }
        public DC_Message SaveAttributeValue(DC_M_masterattributevalue obj)
        {
            DC_Message _objMsg = new DC_Message();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {

                    //Check duplicate 
                    var isduplicate = (from attr in context.m_masterattributevalue
                                       where attr.MasterAttributeValue_Id != obj.MasterAttributeValue_Id && attr.ParentAttributeValue_Id == obj.ParentAttributeValue_Id && attr.MasterAttribute_Id == obj.MasterAttribute_Id && attr.AttributeValue == obj.AttributeValue
                                       select attr).Count() == 0 ? false : true;
                    if (isduplicate)
                    {
                        _objMsg.StatusCode = ReadOnlyMessage.StatusCode.Duplicate;
                        _objMsg.StatusMessage = obj.AttributeValue + ReadOnlyMessage.strAlreadyExist;
                        return _objMsg;
                    }
                    if (obj.Action == "Save")
                    {
                        m_masterattributevalue objIns = new m_masterattributevalue();
                        objIns.MasterAttribute_Id = obj.MasterAttribute_Id;
                        objIns.MasterAttributeValue_Id = obj.MasterAttributeValue_Id;
                        objIns.AttributeValue = obj.AttributeValue;
                        objIns.OTA_CodeTableValue = obj.OTA_CodeTableValue;
                        objIns.IsActive = obj.IsActive == "Y" ? true : false;
                        objIns.ParentAttributeValue_Id = obj.ParentAttributeValue_Id;
                        context.m_masterattributevalue.Add(objIns);
                    }
                    else if (obj.Action == "Update")
                    {
                        var result = (from mav in context.m_masterattributevalue
                                      where mav.MasterAttributeValue_Id == obj.MasterAttributeValue_Id
                                      select mav).First();
                        result.MasterAttribute_Id = obj.MasterAttribute_Id;
                        result.AttributeValue = obj.AttributeValue;
                        result.OTA_CodeTableValue = obj.OTA_CodeTableValue;
                        result.ParentAttributeValue_Id = obj.ParentAttributeValue_Id;
                        result.IsActive = obj.IsActive == "Y" ? true : false;
                    }

                    context.SaveChanges();
                    _objMsg.StatusMessage = obj.Action == "Update" ? ReadOnlyMessage.strUpdatedSuccessfully : ReadOnlyMessage.strAddedSuccessfully;
                    _objMsg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                }

                return _objMsg;
            }
            catch
            {
                _objMsg.StatusMessage = ReadOnlyMessage.strFailed;
                _objMsg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                return _objMsg;
            }

        }
        public DC_M_masterattributelists GetListAttributeAndValuesByFOR(DC_MasterAttribute _obj)
        {
            var retclass = new DC_M_masterattributelists();
            using (ConsumerEntities context = new ConsumerEntities())
            {
                var MasterAttribute = context.m_masterattribute.Where(w => w.IsActive ?? false).AsQueryable();
                if (!string.IsNullOrWhiteSpace(_obj.Name))
                {
                    MasterAttribute = MasterAttribute.Where(w => w.Name.Trim() == _obj.Name.Trim());
                }
                if (!string.IsNullOrWhiteSpace(_obj.MasterFor))
                {
                    MasterAttribute = MasterAttribute.Where(w => w.MasterFor.Trim() == _obj.MasterFor.Trim());
                }

                var MasterAttributeValues = context.m_masterattributevalue.Where(w => w.IsActive ?? false).AsQueryable();

                retclass.MasterAttributes = (from ma in MasterAttribute
                                             join pma in MasterAttribute on ma.ParentAttribute_Id equals pma.ParentAttribute_Id into pmaval
                                             from pmavalid in pmaval.DefaultIfEmpty()
                                             orderby ma.Name
                                             select new DC_M_masterattribute
                                             {
                                                 MasterAttribute_Id = ma.MasterAttribute_Id,
                                                 Name = ma.Name.Trim(),
                                                 MasterFor = ma.MasterFor.Trim(),
                                                 OTA_CodeTableCode = ma.OTA_CodeTableCode.Trim(),
                                                 OTA_CodeTableName = ma.OTA_CodeTableName.Trim(),
                                                 ParentAttribute_Id = ma.ParentAttribute_Id,
                                                 ParentAttributeName = pmavalid.Name.Trim(),
                                                 IsActive = "Y"
                                             }).ToList();

                retclass.MasterAttributeValues = (from mav in MasterAttributeValues
                                                  join ma in MasterAttribute on mav.MasterAttribute_Id equals ma.MasterAttribute_Id
                                                  join pav in MasterAttributeValues on mav.ParentAttributeValue_Id equals pav.MasterAttributeValue_Id into paval
                                                  from pavalid in paval.DefaultIfEmpty()
                                                  orderby mav.AttributeValue
                                                  where (mav.IsActive ?? false) == true
                                                  select new DC_M_masterattributevalue
                                                  {
                                                      MasterAttributeValue_Id = mav.MasterAttributeValue_Id,
                                                      AttributeValue = mav.AttributeValue.Trim(),
                                                      MasterAttribute_Id = mav.MasterAttribute_Id,
                                                      MasterAttribute_Name = mav.MasterAttribute_Name.Trim(),
                                                      OTA_CodeTableValue = mav.OTA_CodeTableValue.Trim(),
                                                      IsActive = "Y",
                                                      ParentAttributeValue = pavalid.AttributeValue,
                                                      ParentAttributeValue_Id = pavalid.MasterAttributeValue_Id
                                                  }).ToList();
            }
            return retclass;
        }


        #endregion

        #region Supplier

        public IList<DataContracts.Masters.DC_Supplier> GetSupplier(DataContracts.Masters.DC_Supplier_Search_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from sup in context.Supplier
                                 select sup;

                    if (RQ.Supplier_Id.HasValue)
                    {
                        if (RQ.Supplier_Id.Value != Guid.Empty)
                        {
                            search = from sup in search
                                     where sup.Supplier_Id == RQ.Supplier_Id
                                     select sup;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.Name))
                    {
                        search = from sup in search
                                 where sup.Name.StartsWith(RQ.Name)
                                 select sup;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.Code))
                    {
                        search = from sup in search
                                 where sup.Code.StartsWith(RQ.Code)
                                 select sup;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.ProductCategory_ID))
                    {
                        search = from sup in search
                                 join pro in context.Supplier_ProductCategory on sup.Supplier_Id equals pro.Supplier_Id
                                 where pro.ProductCategory == RQ.ProductCategory_ID
                                 select sup;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.CategorySubType_ID))
                    {
                        search = from sup in search
                                 join pro in context.Supplier_ProductCategory on sup.Supplier_Id equals pro.Supplier_Id
                                 where pro.ProductCategorySubType == RQ.CategorySubType_ID
                                 select sup;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.SupplierType))
                    {
                        search = from sup in search
                                 where sup.SupplierType == RQ.SupplierType
                                 select sup;
                    }
                    if (!string.IsNullOrWhiteSpace(RQ.StatusCode))
                    {
                        search = from sup in search
                                 where sup.StatusCode.Trim().Substring(0, 3).ToUpper() == RQ.StatusCode.Trim().Substring(0, 3).ToUpper()
                                 select sup;
                    }

                    int total;

                    total = search.Count();

                    if (RQ.PageSize == 0)
                        RQ.PageSize = 10;

                    int skip = (RQ.PageNo ?? 0) * (RQ.PageSize ?? 0);

                    var canPage = skip < total;


                    var result = from a in search
                                     //join b in context.Supplier_ProductCategory on a.Supplier_Id equals b.Supplier_Id into supp1
                                     //from sup1 in supp1.DefaultIfEmpty()
                                     //join c in context.m_masterattributevalue on sup1.ProductCategory equals Convert.ToString(c.MasterAttributeValue_Id) into supp2
                                     //from sup2 in supp2.DefaultIfEmpty()
                                     //join d in context.m_masterattributevalue on sup1.ProductCategorySubType equals Convert.ToString(d.MasterAttributeValue_Id) into supp3
                                     //from sup3 in supp3.DefaultIfEmpty()
                                 orderby a.Name
                                 select new DataContracts.Masters.DC_Supplier
                                 {
                                     Supplier_Id = a.Supplier_Id,
                                     Name = a.Name,
                                     Code = a.Code,
                                     Create_Date = a.Create_Date,
                                     Create_User = a.Create_User,
                                     Edit_Date = a.Edit_Date,
                                     Edit_User = a.Edit_User,
                                     StatusCode = a.StatusCode,
                                     SupplierType = a.SupplierType,
                                     SupplierOwner = a.SupplierOwner,
                                     ProductCategory = string.Empty, //sup2.AttributeValue,
                                     CategorySubType = string.Empty, //sup3.AttributeValue,
                                     Priority = a.Priority,
                                     TotalRecords = total,
                                     IsFullPull = a.IsFullPull
                                 };

                    return result.OrderBy(p => p.Name).Skip(skip).Take((RQ.PageSize ?? total)).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching City Master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public IList<DataContracts.Masters.DC_Supplier_DDL> GetSupplierByEntity(DataContracts.Masters.DC_Supplier_Search_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from sup in context.Supplier
                                 where sup.StatusCode.ToLower() == "active"
                                 select sup;

                    if (!string.IsNullOrEmpty(RQ.EntityType))
                    {
                        search = (from sup in search
                                  join supcat in context.Supplier_ProductCategory on sup.Supplier_Id equals supcat.Supplier_Id
                                  where supcat.ProductCategory.ToLower() == RQ.EntityType.ToLower()
                                  select sup).Distinct();
                    }

                    if (RQ.Supplier_Id.HasValue)
                    {
                        if (RQ.Supplier_Id.Value != Guid.Empty)
                        {
                            search = from sup in search
                                     where sup.Supplier_Id == RQ.Supplier_Id
                                     select sup;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.Name))
                    {
                        search = from sup in search
                                 where sup.Name.StartsWith(RQ.Name)
                                 select sup;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.Code))
                    {
                        search = from sup in search
                                 where sup.Code.StartsWith(RQ.Code)
                                 select sup;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.ProductCategory_ID))
                    {
                        search = from sup in search
                                 join pro in context.Supplier_ProductCategory on sup.Supplier_Id equals pro.Supplier_Id
                                 where pro.ProductCategory == RQ.ProductCategory_ID
                                 select sup;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.CategorySubType_ID))
                    {
                        search = from sup in search
                                 join pro in context.Supplier_ProductCategory on sup.Supplier_Id equals pro.Supplier_Id
                                 where pro.ProductCategorySubType == RQ.CategorySubType_ID
                                 select sup;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.SupplierType))
                    {
                        search = from sup in search
                                 where sup.SupplierType == RQ.SupplierType
                                 select sup;
                    }
                    if (!string.IsNullOrWhiteSpace(RQ.StatusCode))
                    {
                        search = from sup in search
                                 where sup.StatusCode.Trim().Substring(0, 3).ToUpper() == RQ.StatusCode.Trim().Substring(0, 3).ToUpper()
                                 select sup;
                    }


                    int total;
                    total = search.Count();
                    var result = from a in search
                                 orderby a.Name
                                 select new DataContracts.Masters.DC_Supplier_DDL
                                 {
                                     Supplier_Id = a.Supplier_Id,
                                     Name = a.Name,
                                     Code = a.Code,
                                     Priority = a.Priority ?? 0,
                                     IsFullPull = a.IsFullPull
                                 };

                    return result.OrderBy(p => p.Name).ToList();

                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching City Master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public IList<DataContracts.Masters.DC_SupplierMarket> GetSupplierMarket(DataContracts.Masters.DC_SupplierMarket RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from sup in context.Supplier_Market
                                 select sup;

                    if (RQ.Supplier_Id.HasValue)
                    {
                        if (RQ.Supplier_Id.Value != Guid.Empty)
                        {
                            search = from sup in search
                                     where sup.Supplier_Id == RQ.Supplier_Id
                                     select sup;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.Name))
                    {
                        search = from sup in search
                                 where sup.Name == RQ.Name
                                 select sup;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.Code))
                    {
                        search = from sup in search
                                 where sup.Code == RQ.Code
                                 select sup;
                    }

                    if (RQ.Supplier_Market_Id.HasValue)
                    {
                        if (RQ.Supplier_Market_Id.Value != Guid.Empty)
                        {
                            search = from sup in search
                                     where sup.Supplier_Market_Id == RQ.Supplier_Market_Id
                                     select sup;
                        }
                    }

                    int total;

                    total = search.Count();

                    if (RQ.PageSize == 0)
                        RQ.PageSize = 10;

                    int skip = (RQ.PageNo ?? 0) * (RQ.PageSize ?? 0);

                    var canPage = skip < total;


                    var result = from a in search
                                 orderby a.Name
                                 select new DataContracts.Masters.DC_SupplierMarket
                                 {
                                     Supplier_Id = a.Supplier_Id,
                                     Name = a.Name,
                                     Code = a.Code,
                                     Create_Date = a.Create_Date,
                                     Create_User = a.Create_User,
                                     Edit_Date = a.Edit_Date,
                                     Edit_User = a.Edit_User,
                                     Status = a.Status,
                                     IsActive = a.IsActive,
                                     Supplier_Market_Id = a.Supplier_Market_Id,
                                     TotalRecords = total
                                 };

                    return result.OrderBy(p => p.Name).Skip(skip).Take((RQ.PageSize ?? total)).ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching City Master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public IList<DataContracts.Masters.DC_Supplier_ProductCategory> GetProductCategoryBySupplier(DataContracts.Masters.DC_Supplier_ProductCategory RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from sup in context.Supplier_ProductCategory
                                 select sup;

                    if (RQ.Supplier_Id.HasValue)
                    {
                        if (RQ.Supplier_Id.Value != Guid.Empty)
                        {
                            search = from sup in search
                                     where sup.Supplier_Id == RQ.Supplier_Id
                                     select sup;
                        }
                    }
                    if (RQ.Supplier_ProductCategory_Id.HasValue)
                    {
                        if (RQ.Supplier_ProductCategory_Id != Guid.Empty)
                        {
                            search = from sup in search
                                     where sup.Supplier_ProductCategory_Id == RQ.Supplier_ProductCategory_Id
                                     select sup;
                        }
                    }

                    int total;

                    total = search.Count();

                    if (RQ.PageSize == 0)
                        RQ.PageSize = 10;

                    int skip = (RQ.PageNo ?? 0) * (RQ.PageSize ?? 0);

                    var canPage = skip < total;


                    var result = from a in search
                                 orderby a.ProductCategory, a.ProductCategorySubType
                                 select new DataContracts.Masters.DC_Supplier_ProductCategory
                                 {
                                     Supplier_ProductCategory_Id = a.Supplier_ProductCategory_Id,
                                     ProductCategory = a.ProductCategory,
                                     ProductCategorySubType = a.ProductCategorySubType,
                                     IsDefaultSupplier = (a.IsDefaultSupplier ?? false),
                                     IsActive = (a.IsActive ?? false),
                                     TotalRecords = total
                                 };

                    return result.OrderBy(p => p.ProductCategory).Skip(skip).Take((RQ.PageSize ?? total)).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching City Master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public DataContracts.DC_Message AddUpdateSupplier(DataContracts.Masters.DC_Supplier _objSup)
        {
            DC_Message _msg = new DC_Message();
            using (ConsumerEntities context = new ConsumerEntities())
            {
                var isduplicate = (from sup in context.Supplier
                                   where sup.Supplier_Id != (_objSup.Supplier_Id ?? Guid.Empty) && sup.Name == _objSup.Name
                                   select sup).Count() == 0 ? false : true;

                if (isduplicate)
                {
                    _msg.StatusMessage = ReadOnlyMessage.strAlreadyExist;
                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Duplicate;
                    return _msg;
                }

                if (_objSup.Supplier_Id.HasValue && _objSup.Supplier_Id.Value != Guid.Empty)
                {
                    var result = context.Supplier.Find(_objSup.Supplier_Id);

                    if (result != null)
                    {
                        result.Name = _objSup.Name;
                        result.Code = _objSup.Code;
                        result.Edit_Date = _objSup.Edit_Date;
                        result.Edit_User = _objSup.Edit_User;
                        result.StatusCode = _objSup.StatusCode;
                        result.SupplierOwner = _objSup.SupplierOwner;
                        result.SupplierType = _objSup.SupplierType;
                        result.Priority = _objSup.Priority;
                        result.IsFullPull = _objSup.IsFullPull;

                        if (context.SaveChanges() == 1)
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strUpdatedSuccessfully;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        }
                        else
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strFailed;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                        }
                    }
                    return _msg;

                }
                else
                {
                    Supplier _obj = new Supplier
                    {
                        Supplier_Id = Guid.NewGuid(),
                        Name = _objSup.Name,
                        Code = _objSup.Code,
                        Create_Date = _objSup.Create_Date,
                        Create_User = _objSup.Create_User,
                        SupplierOwner = _objSup.SupplierOwner,
                        SupplierType = _objSup.SupplierType,
                        StatusCode = _objSup.StatusCode,
                        Priority = _objSup.Priority,
                        IsFullPull = _objSup.IsFullPull
                    };

                    context.Supplier.Add(_obj);
                    if (context.SaveChanges() == 1)
                    {
                        _msg.StatusMessage = ReadOnlyMessage.strUpdatedSuccessfully;
                        _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                    }
                    else
                    {
                        _msg.StatusMessage = ReadOnlyMessage.strFailed;
                        _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                    }

                }
            }
            return _msg;

        }
        public DataContracts.DC_Message AddUpdateSupplierMarket(DataContracts.Masters.DC_SupplierMarket _objSupMkt)
        {
            DC_Message _msg = new DC_Message();
            using (ConsumerEntities context = new ConsumerEntities())
            {
                var isduplicate = (from sup in context.Supplier
                                   join supmkt in context.Supplier_Market on sup.Supplier_Id equals supmkt.Supplier_Id
                                   where supmkt.Supplier_Market_Id != (_objSupMkt.Supplier_Market_Id ?? Guid.Empty) && supmkt.Name == _objSupMkt.Name
                                   && supmkt.Supplier_Id == _objSupMkt.Supplier_Id
                                   select sup).Count() == 0 ? false : true;

                if (isduplicate)
                {
                    _msg.StatusMessage = ReadOnlyMessage.strAlreadyExist;
                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Duplicate;
                    return _msg;
                }

                if (_objSupMkt.Supplier_Market_Id == null)
                {

                    Supplier_Market _obj = new Supplier_Market
                    {
                        Supplier_Market_Id = Guid.NewGuid(),
                        Supplier_Id = _objSupMkt.Supplier_Id,
                        IsActive = true,
                        Name = _objSupMkt.Name,
                        Code = _objSupMkt.Code,
                        Create_Date = _objSupMkt.Create_Date,
                        Create_User = _objSupMkt.Create_User,
                        Status = _objSupMkt.Status
                    };
                    context.Supplier_Market.Add(_obj);
                    if ((context.SaveChanges() == 1))
                    {
                        _msg.StatusMessage = ReadOnlyMessage.strAddedSuccessfully;
                        _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                    }
                    else
                    {
                        _msg.StatusMessage = ReadOnlyMessage.strFailed;
                        _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                    }
                    return _msg;

                }
                else
                {
                    //Update
                    var result = context.Supplier_Market.Find(_objSupMkt.Supplier_Market_Id);
                    //var result = GetSupplierMarket(new DC_SupplierMarket() { Supplier_Id = _objSupMkt.Supplier_Id, Supplier_Market_Id = _objSupMkt.Supplier_Market_Id });
                    if (result != null)
                    {
                        result.Name = _objSupMkt.Name;
                        result.Code = _objSupMkt.Code;
                        result.Edit_Date = _objSupMkt.Edit_Date;
                        result.Edit_User = _objSupMkt.Edit_User;
                        result.Status = _objSupMkt.Status;
                        result.IsActive = _objSupMkt.IsActive;
                        if (context.SaveChanges() == 1)
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strUpdatedSuccessfully;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        }
                        else
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strFailed;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                        }
                    }
                }
            }
            return _msg;

        }
        public DataContracts.DC_Message SupplierMarketSoftDelete(DataContracts.Masters.DC_SupplierMarket _objSupMkt)
        {
            DC_Message _msg = new DC_Message();
            using (ConsumerEntities context = new ConsumerEntities())
            {
                var result = context.Supplier_Market.Find(_objSupMkt.Supplier_Market_Id);

                result.IsActive = _objSupMkt.IsActive;
                result.Edit_Date = DateTime.Now;
                result.Edit_User = _objSupMkt.Edit_User;
                if (context.SaveChanges() == 1)
                {
                    _msg.StatusMessage = ReadOnlyMessage.strUpdatedSuccessfully;
                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                }
                else
                {
                    _msg.StatusMessage = ReadOnlyMessage.strFailed;
                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                }
            }
            return _msg;

        }
        public DataContracts.DC_Message AddUpdateSupplier_ProductCategory(DataContracts.Masters.DC_Supplier_ProductCategory _objSupCat)
        {
            DC_Message _msg = new DC_Message();
            using (ConsumerEntities context = new ConsumerEntities())
            {
                //var isduplicate = (from sup in context.Supplier
                //                   join supcat in context.Supplier_ProductCategory on sup.Supplier_Id equals supcat.Supplier_Id
                //                   where supcat.Supplier_ProductCategory_Id != (_objSupCat.Supplier_ProductCategory_Id ?? Guid.Empty) 
                //                   && supcat.Supplier_Id == _objSupCat.Supplier_Id 
                //                   select sup).Count() == 0 ? false : true;

                var isduplicate = (from supcat in context.Supplier_ProductCategory
                                   join sup in context.Supplier on supcat.Supplier_Id equals sup.Supplier_Id
                                   where supcat.ProductCategory == _objSupCat.ProductCategory && supcat.ProductCategorySubType == _objSupCat.ProductCategorySubType &&
                                   supcat.Supplier_ProductCategory_Id != (_objSupCat.Supplier_ProductCategory_Id ?? Guid.Empty)
                                   && supcat.Supplier_Id == _objSupCat.Supplier_Id
                                   select supcat).Count() == 0 ? false : true;


                if (isduplicate)
                {
                    _msg.StatusMessage = ReadOnlyMessage.strAlreadyExist;
                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Duplicate;
                    return _msg;
                }

                if (_objSupCat.Supplier_ProductCategory_Id == null)
                {

                    Supplier_ProductCategory _obj = new Supplier_ProductCategory
                    {
                        Supplier_ProductCategory_Id = Guid.NewGuid(),
                        Supplier_Id = _objSupCat.Supplier_Id,
                        ProductCategory = _objSupCat.ProductCategory,
                        ProductCategorySubType = _objSupCat.ProductCategorySubType,
                        IsDefaultSupplier = _objSupCat.IsDefaultSupplier,
                        IsActive = _objSupCat.IsActive,
                        Create_Date = _objSupCat.Create_Date,
                        Create_User = _objSupCat.Create_User,
                        Status = _objSupCat.Status
                    };
                    context.Supplier_ProductCategory.Add(_obj);
                    if ((context.SaveChanges() == 1))
                    {
                        _msg.StatusMessage = ReadOnlyMessage.strAddedSuccessfully;
                        _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                    }
                    else
                    {
                        _msg.StatusMessage = ReadOnlyMessage.strFailed;
                        _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                    }
                    return _msg;

                }
                else
                {
                    //Update
                    var result = context.Supplier_ProductCategory.Find(_objSupCat.Supplier_ProductCategory_Id);
                    //var result = GetSupplierMarket(new DC_SupplierMarket() { Supplier_Id = _objSupMkt.Supplier_Id, Supplier_Market_Id = _objSupMkt.Supplier_Market_Id });
                    if (result != null)
                    {
                        result.Supplier_Id = _objSupCat.Supplier_Id;
                        result.ProductCategory = _objSupCat.ProductCategory;
                        result.ProductCategorySubType = _objSupCat.ProductCategorySubType;
                        result.IsDefaultSupplier = _objSupCat.IsDefaultSupplier;
                        result.IsActive = _objSupCat.IsActive;
                        result.Edit_Date = _objSupCat.Edit_Date;
                        result.Edit_User = _objSupCat.Edit_User;
                        result.Status = _objSupCat.Status;
                        if (context.SaveChanges() == 1)
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strUpdatedSuccessfully;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        }
                        else
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strFailed;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                        }
                    }
                }
            }
            return _msg;

        }
        public DataContracts.DC_Message Supplier_ProductCategorySoftDelete(DataContracts.Masters.DC_Supplier_ProductCategory _objSupCat)
        {
            DC_Message _msg = new DC_Message();
            using (ConsumerEntities context = new ConsumerEntities())
            {
                var result = context.Supplier_ProductCategory.Find(_objSupCat.Supplier_ProductCategory_Id);

                result.IsActive = _objSupCat.IsActive;
                result.Edit_Date = DateTime.Now;
                result.Edit_User = _objSupCat.Edit_User;
                if (context.SaveChanges() == 1)
                {
                    _msg.StatusMessage = ReadOnlyMessage.strUpdatedSuccessfully;
                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                }
                else
                {
                    _msg.StatusMessage = ReadOnlyMessage.strFailed;
                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                }
            }
            return _msg;

        }

        public List<DataContracts.Masters.DC_Supplier_ApiLocation> SupplierApiLocation_Search(DataContracts.Masters.DC_Supplier_ApiLocation RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from supApi in context.Supplier_APILocation
                                 select supApi;

                    if (RQ.Supplier_Id != Guid.Empty)
                    {
                        search = from sup in search
                                 where sup.Supplier_Id == RQ.Supplier_Id
                                 select sup;
                    }

                    if (RQ.ApiLocation_Id != Guid.Empty)
                    {
                        search = from sup in search
                                 where sup.Supplier_APILocation_Id == RQ.ApiLocation_Id
                                 select sup;
                    }

                    //int total;

                    //total = search.Count();

                    //if (RQ.PageSize == 0)
                    //    RQ.PageSize = 10;

                    //int skip = (RQ.PageNo ?? 0) * (RQ.PageSize ?? 0);

                    //var canPage = skip < total;


                    var result = from a in search
                                 join mav in context.m_masterattributevalue on a.Entity_Id equals mav.MasterAttributeValue_Id
                                 join sup in context.Supplier on a.Supplier_Id equals sup.Supplier_Id
                                 select new DataContracts.Masters.DC_Supplier_ApiLocation
                                 {
                                     Supplier_Id = a.Supplier_Id ?? Guid.Empty,
                                     ApiEndPoint = a.API_Path,
                                     ApiLocation_Id = a.Supplier_APILocation_Id,
                                     Create_Date = a.CREATE_DATE,
                                     Create_User = a.CREATE_USER,
                                     Edit_Date = a.EDIT_DATE,
                                     Edit_User = a.EDIT_USER,
                                     Entity = mav.AttributeValue,
                                     Entity_Id = a.Entity_Id,
                                     IsActive = false,
                                     Status = a.STATUS,
                                     Supplier_Name = sup.Name
                                 };

                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching Supplier Api Location", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }

        }

        public DataContracts.DC_Message SupplierApiLocation_Update(DataContracts.Masters.DC_Supplier_ApiLocation objApiLoc)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var SupplierApiLocation = context.Supplier_APILocation.Find(objApiLoc.ApiLocation_Id);
                    if (SupplierApiLocation != null)
                    {
                        var dupeRecord = context.Supplier_APILocation.Where(w => w.API_Path.ToLower().Trim() == objApiLoc.ApiEndPoint.Trim().ToLower() && w.Supplier_APILocation_Id != objApiLoc.ApiLocation_Id).Select(r => r).FirstOrDefault();

                        if (dupeRecord != null)
                        {
                            string Supplier = context.Supplier.Where(w => w.Supplier_Id == dupeRecord.Supplier_Id).Select(s => s.Name).FirstOrDefault();
                            string Entity = context.m_masterattributevalue.Where(w => w.MasterAttributeValue_Id == dupeRecord.Entity_Id).Select(s => s.AttributeValue).FirstOrDefault();

                            return new DC_Message { StatusCode = ReadOnlyMessage.StatusCode.Duplicate, StatusMessage = "This Api Location already exists for Supplier : " + Supplier + " and Entity : " + Entity };
                        }
                        else
                        {
                            SupplierApiLocation.Supplier_Id = objApiLoc.Supplier_Id;
                            SupplierApiLocation.API_Path = objApiLoc.ApiEndPoint;
                            SupplierApiLocation.EDIT_DATE = objApiLoc.Edit_Date ?? DateTime.Now;
                            SupplierApiLocation.EDIT_USER = objApiLoc.Edit_User;
                            SupplierApiLocation.Entity_Id = objApiLoc.Entity_Id;
                            SupplierApiLocation.STATUS = objApiLoc.Status;
                            context.SaveChanges();
                            return new DC_Message { StatusCode = ReadOnlyMessage.StatusCode.Success, StatusMessage = "Record updated." };
                        }
                    }
                    else
                    {
                        return new DC_Message { StatusCode = ReadOnlyMessage.StatusCode.Warning, StatusMessage = "Invalid Record." };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating Supplier Api Location", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public DataContracts.DC_Message SupplierApiLocation_Add(DataContracts.Masters.DC_Supplier_ApiLocation objApiLoc)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (objApiLoc.Supplier_Id == null || objApiLoc.Entity_Id == null || string.IsNullOrWhiteSpace(objApiLoc.ApiEndPoint))
                    {
                        return new DC_Message { StatusCode = ReadOnlyMessage.StatusCode.Warning, StatusMessage = "Invalid Record." };
                    }

                    if (objApiLoc.Supplier_Id == Guid.Empty || objApiLoc.Entity_Id == Guid.Empty)
                    {
                        return new DC_Message { StatusCode = ReadOnlyMessage.StatusCode.Warning, StatusMessage = "Invalid Record." };
                    }

                    var dupeRecord = context.Supplier_APILocation.Where(w => w.API_Path.ToLower().Trim() == objApiLoc.ApiEndPoint.Trim().ToLower()).Select(r => r).FirstOrDefault();
                    if (dupeRecord != null)
                    {
                        string Supplier = context.Supplier.Where(w => w.Supplier_Id == dupeRecord.Supplier_Id).Select(s => s.Name).FirstOrDefault();
                        string Entity = context.m_masterattributevalue.Where(w => w.MasterAttributeValue_Id == dupeRecord.Entity_Id).Select(s => s.AttributeValue).FirstOrDefault();

                        return new DC_Message { StatusCode = ReadOnlyMessage.StatusCode.Duplicate, StatusMessage = "This Api Location already exists for Supplier : " + Supplier + " and Entity : " + Entity };
                    }

                    dupeRecord = null;
                    dupeRecord = context.Supplier_APILocation.Where(w => w.Supplier_Id == objApiLoc.Supplier_Id && w.Entity_Id == objApiLoc.Entity_Id).Select(r => r).FirstOrDefault();
                    if (dupeRecord != null)
                    {
                        return new DC_Message { StatusCode = ReadOnlyMessage.StatusCode.Duplicate, StatusMessage = "Duplicate entry for this supplier and entity." };
                    }

                    if (objApiLoc.ApiLocation_Id != null)
                    {
                        if (objApiLoc.ApiLocation_Id != Guid.Empty)
                        {
                            var SupplierApiLocation = context.Supplier_APILocation.Find(objApiLoc.ApiLocation_Id);
                            if (SupplierApiLocation != null)
                            {
                                return new DC_Message { StatusCode = ReadOnlyMessage.StatusCode.Duplicate, StatusMessage = "Duplicate primary key." };
                            }
                        }
                    }

                    context.Supplier_APILocation.Add(new Supplier_APILocation
                    {
                        API_Path = objApiLoc.ApiEndPoint,
                        CREATE_DATE = objApiLoc.Create_Date ?? DateTime.Now,
                        CREATE_USER = objApiLoc.Create_User,
                        STATUS = objApiLoc.Status,
                        Entity_Id = objApiLoc.Entity_Id,
                        Supplier_Id = objApiLoc.Supplier_Id,
                        Supplier_APILocation_Id = objApiLoc.ApiLocation_Id == Guid.Empty ? Guid.NewGuid() : objApiLoc.ApiLocation_Id
                    });
                    context.SaveChanges();
                    return new DC_Message { StatusCode = ReadOnlyMessage.StatusCode.Success, StatusMessage = "Record Added." };
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while adding Supplier Api Location", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region Statuses
        public List<DC_Statuses> GetAllStatuses()
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var state = from s in context.m_Statuses
                                orderby s.Status_Name
                                select new DC_Statuses
                                {
                                    Status_ID = s.Status_ID,
                                    Status_Name = s.Status_Name,
                                    Status_Short = s.Status_Short,
                                    CREATE_USER = s.CREATE_USER,
                                    CREATE_DATE = s.CREATE_DATE
                                };
                    return state.ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching state master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region "Activity Master"
        public List<DataContracts.Masters.DC_Activity> GetActivityMaster(DataContracts.Masters.DC_Activity_Search_RQ RQ)
        {
            try
            {
                List<DataContracts.Masters.DC_Activity> res = new List<DC_Activity>();
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from a in context.Activities
                                 select a;

                    if (RQ.Activity_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_Id == RQ.Activity_Id
                                 select a;
                    }

                    if (RQ.Country != null)
                    {
                        search = from a in search
                                 where a.Country.Trim().TrimStart().ToUpper() == RQ.Country.Trim().TrimStart().ToUpper()
                                 select a;
                    }

                    if (RQ.City != null)
                    {
                        search = from a in search
                                 where a.City.Trim().TrimStart().ToUpper() == RQ.City.Trim().TrimStart().ToUpper()
                                 select a;
                    }

                    if (RQ.Name != null)
                    {
                        search = from a in search
                                 where a.Product_Name.Trim().TrimStart().ToUpper() == RQ.Name.Trim().TrimStart().ToUpper()
                                 select a;
                    }

                    if (RQ.ProductCategory != null)
                    {
                        search = from a in search
                                 where a.ProductCategory.Trim().TrimStart().ToUpper() == RQ.ProductCategory.Trim().TrimStart().ToUpper()
                                 select a;
                    }

                    if (RQ.ProductCategorySubType != null)
                    {
                        search = from a in search
                                 where a.ProductCategorySubType.Trim().TrimStart().ToUpper() == RQ.ProductCategorySubType.Trim().TrimStart().ToUpper()
                                 select a;
                    }

                    if (RQ.ProductType != null)
                    {
                        search = from a in search
                                 where a.ProductType.Trim().TrimStart().ToUpper() == RQ.ProductType.Trim().TrimStart().ToUpper()
                                 select a;
                    }

                    if (RQ.ProductNameSubType != null)
                    {
                        search = from a in search
                                 join at in context.Activity_Flavour on a.Activity_Id equals at.Activity_Id
                                 where at.ProductNameSubType.Trim().TrimStart().ToUpper() == RQ.ProductNameSubType.Trim().TrimStart().ToUpper()
                                 select a;
                    }

                    if (RQ.Status != null)
                    {
                        bool IsActive = false;
                        if (RQ.Status.ToString().Trim().TrimStart().ToUpper() == "ACTIVE")
                            IsActive = true;

                        search = from a in search
                                 where (a.IsActive ?? false) == IsActive
                                 select a;
                    }

                    //For Keyword
                    if (!string.IsNullOrWhiteSpace(RQ.Keyword))
                    {
                        search = from a in search
                                 join b in context.Activity_Content on a.Activity_Id equals b.Activity_Id
                                 where a.Product_Name.Contains(RQ.Keyword) || b.Content_Text.Contains(RQ.Keyword)
                                 select a;
                    }

                    int total = search.Count();
                    int skip = (RQ.PageNo ?? 0) * (RQ.PageSize ?? 0);

                    var result = from a in search
                                 orderby a.Product_Name
                                 select new DataContracts.Masters.DC_Activity
                                 {
                                     Activity_Id = a.Activity_Id,
                                     CommonProductID = a.CommonProductID,
                                     Legacy_Product_ID = a.Legacy_Product_ID,
                                     Product_Name = a.Product_Name,
                                     Display_Name = a.Display_Name,
                                     Country = a.Country,
                                     City = a.City,
                                     ProductType = a.ProductType,
                                     ProductCategory = a.ProductCategory,
                                     ProductCategorySubType = a.ProductCategorySubType,
                                     Create_Date = a.Create_Date,
                                     Edit_Date = a.Edit_Date,
                                     Create_User = a.Create_User,
                                     Edit_User = a.Edit_User,
                                     IsActive = a.IsActive,
                                     CompanyRecommended = a.CompanyRecommended,
                                     Latitude = a.Latitude,
                                     Longitude = a.Longitude,
                                     TourType = a.TourType,
                                     Parent_Legacy_Id = a.Parent_Legacy_Id,
                                     TotalRecord = total,
                                     Affiliation = a.Affiliation,
                                     Country_Id = a.Country_Id,
                                     City_Id = a.City_Id,
                                     CompanyProductID = a.CompanyProductID,
                                     CompanyRating = a.CompanyRating,
                                     Mode_Of_Transport = a.Mode_Of_Transport,
                                     ProductRating = a.ProductRating,
                                     Remarks = a.Remarks
                                 };
                    return result.OrderBy(p => p.Product_Name).Skip(skip).Take((RQ.PageSize ?? total)).ToList();
                }

            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching Attr Master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public List<DataContracts.Masters.DC_Activity> GetActivityMasterBySupplier(DataContracts.Masters.DC_Activity_Search_RQ RQ)
        {
            try
            {
                List<DataContracts.Masters.DC_Activity> res = new List<DC_Activity>();
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from a in context.Activities
                                 select a;

                    ////excluding supplier tagged activity

                    //if (!string.IsNullOrWhiteSpace(RQ.Supplier_Id))
                    //{
                    //    Guid _Suppier_Id = Guid.Parse(RQ.Supplier_Id);
                    //    if (_Suppier_Id != Guid.Empty)
                    //    {
                    //        search = search.Where(i => !(from ab in context.Activity_SupplierProductMapping where ab.Supplier_ID == _Suppier_Id && ab.MappingStatus == "MAPPED" && ab.Activity_ID != null select ab.Activity_ID).Contains(i.Activity_Id));
                    //    }
                    //}
                    if (RQ.Activity_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_Id == RQ.Activity_Id
                                 select a;
                    }

                    if (RQ.Country != null)
                    {
                        search = from a in search
                                 where a.Country.Trim().TrimStart().ToUpper() == RQ.Country.Trim().TrimStart().ToUpper()
                                 select a;
                    }

                    if (RQ.City != null)
                    {
                        search = from a in search
                                 where a.City.Trim().TrimStart().ToUpper() == RQ.City.Trim().TrimStart().ToUpper()
                                 select a;
                    }

                    if (RQ.Name != null)
                    {
                        search = from a in search
                                 where a.Product_Name.Trim().TrimStart().ToUpper() == RQ.Name.Trim().TrimStart().ToUpper()
                                 select a;
                    }

                    if (RQ.ProductCategory != null)
                    {
                        search = from a in search
                                 where a.ProductCategory.Trim().TrimStart().ToUpper() == RQ.ProductCategory.Trim().TrimStart().ToUpper()
                                 select a;
                    }

                    if (RQ.ProductCategory != null)
                    {
                        search = from a in search
                                 where a.ProductCategory.Trim().TrimStart().ToUpper() == RQ.ProductCategory.Trim().TrimStart().ToUpper()
                                 select a;
                    }

                    if (RQ.ProductCategorySubType != null)
                    {
                        search = from a in search
                                 where a.ProductCategorySubType.Trim().TrimStart().ToUpper() == RQ.ProductCategorySubType.Trim().TrimStart().ToUpper()
                                 select a;
                    }

                    if (RQ.ProductType != null)
                    {
                        search = from a in search
                                 where a.ProductType.Trim().TrimStart().ToUpper() == RQ.ProductType.Trim().TrimStart().ToUpper()
                                 select a;
                    }

                    if (RQ.ProductNameSubType != null)
                    {
                        search = from a in search
                                 join at in context.Activity_Flavour on a.Activity_Id equals at.Activity_Id
                                 where at.ProductNameSubType.Trim().TrimStart().ToUpper() == RQ.ProductNameSubType.Trim().TrimStart().ToUpper()
                                 select a;
                    }

                    if (RQ.Status != null)
                    {
                        bool IsActive = false;
                        if (RQ.Status.ToString().Trim().TrimStart().ToUpper() == "ACTIVE")
                            IsActive = true;

                        search = from a in search
                                 where (a.IsActive ?? false) == IsActive
                                 select a;
                    }
                    //For Keyword
                    if (!string.IsNullOrWhiteSpace(RQ.Keyword))
                    {
                        search = from a in search
                                 join b in context.Activity_Content on a.Activity_Id equals b.Activity_Id
                                 where a.Product_Name.Contains(RQ.Keyword) || b.Content_Text.Contains(RQ.Keyword)
                                 select a;
                    }
                    int total = search.Count();
                    int skip = (RQ.PageNo ?? 0) * (RQ.PageSize ?? 0);

                    var result = from a in search
                                 where a.Product_Name != null
                                 orderby a.Product_Name
                                 select new DataContracts.Masters.DC_Activity
                                 {
                                     Activity_Id = a.Activity_Id,
                                     CommonProductID = a.CommonProductID,
                                     Legacy_Product_ID = a.Legacy_Product_ID,
                                     Product_Name = a.Product_Name,
                                     Display_Name = a.Display_Name,
                                     Country = a.Country,
                                     City = a.City,
                                     ProductType = a.ProductType,
                                     ProductCategory = a.ProductCategory,
                                     ProductCategorySubType = a.ProductCategorySubType,
                                     Create_Date = a.Create_Date,
                                     Edit_Date = a.Edit_Date,
                                     Create_User = a.Create_User,
                                     Edit_User = a.Edit_User,
                                     IsActive = a.IsActive,
                                     CompanyRecommended = a.CompanyRecommended,
                                     Latitude = a.Latitude,
                                     Longitude = a.Longitude,
                                     TourType = a.TourType,
                                     Parent_Legacy_Id = a.Parent_Legacy_Id,
                                     TotalRecord = total,
                                     Affiliation = a.Affiliation,
                                     Country_Id = a.Country_Id,
                                     City_Id = a.City_Id,
                                     CompanyProductID = a.CompanyProductID,
                                     CompanyRating = a.CompanyRating,
                                     Mode_Of_Transport = a.Mode_Of_Transport,
                                     ProductRating = a.ProductRating,
                                     Remarks = a.Remarks

                                 };
                    return result.OrderBy(p => p.Product_Name).Skip(skip).Take((RQ.PageSize ?? total)).ToList();
                }

            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching Attr Master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public List<DataContracts.Masters.DC_Activity_Content> GetActivityContentMaster(DataContracts.Masters.DC_Activity_Content RQ)
        {
            try
            {
                List<DataContracts.Masters.DC_Activity_Content> res = new List<DC_Activity_Content>();
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from a in context.Activity_Content
                                 select a;

                    if (RQ.Activity_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_Id == RQ.Activity_Id
                                 select a;
                    }
                    int total = search.Count();

                    var result = from a in search
                                 orderby a.Content_Type
                                 select new DataContracts.Masters.DC_Activity_Content
                                 {
                                     ActivityContent_Id = a.ActivityContent_Id,
                                     Activity_Id = a.Activity_Id,
                                     Legacy_Id = a.Legacy_Id,
                                     Content_Type = a.Content_Type,
                                     Content_Text = a.Content_Text,
                                     Status = a.Status,
                                     Create_Date = a.Create_Date,
                                     Create_User = a.Create_User,
                                     Edit_Date = a.Edit_Date,
                                     Edit_User = a.Edit_User,
                                     IsInternal = a.IsInternal,
                                     TotalRecord = total
                                 };


                    return result.ToList();
                }

            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching Attr Content Master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public List<string> GetActivityNames(DataContracts.Masters.DC_Activity_Search_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {

                    var actSearch = from a in context.Activities
                                    where a.IsActive == true
                                    select a;

                    if (!string.IsNullOrWhiteSpace(RQ.Status))
                    {
                        bool isActive = (RQ.Status == "ACTIVE" ? true : false);
                        actSearch = from a in actSearch
                                    where a.IsActive == isActive
                                    select a;
                    }
                    if (!string.IsNullOrWhiteSpace(RQ.Country))
                    {
                        if (RQ.Country.Length > 0)
                        {
                            actSearch = from a in actSearch
                                        where a.Country == RQ.Country
                                        select a;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.City))
                    {
                        if (RQ.City.Length > 0)
                        {
                            actSearch = from a in actSearch
                                        where a.City == RQ.City
                                        select a;
                        }
                    }
                    if (RQ.ProductCategory != null)
                    {
                        actSearch = from a in actSearch
                                    where a.ProductCategory.Trim().TrimStart().ToUpper() == RQ.ProductCategory.Trim().TrimStart().ToUpper()
                                    select a;
                    }

                    if (RQ.ProductCategorySubType != null)
                    {
                        actSearch = from a in actSearch
                                    where a.ProductCategorySubType.Trim().TrimStart().ToUpper() == RQ.ProductCategorySubType.Trim().TrimStart().ToUpper()
                                    select a;
                    }

                    if (RQ.ProductType != null)
                    {
                        actSearch = from a in actSearch
                                    where a.ProductType.Trim().TrimStart().ToUpper() == RQ.ProductType.Trim().TrimStart().ToUpper()
                                    select a;
                    }

                    if (RQ.ProductNameSubType != null)
                    {
                        actSearch = from a in actSearch
                                    join at in context.Activity_Flavour on a.Activity_Id equals at.Activity_Id
                                    where at.ProductNameSubType.Trim().TrimStart().ToUpper() == RQ.ProductNameSubType.Trim().TrimStart().ToUpper()
                                    select a;
                    }

                    var acco = (from a in actSearch
                                where a.Product_Name.Contains(RQ.Name)
                                orderby a.Product_Name
                                select a.Product_Name).ToList();

                    return acco;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching accomodation list", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region Common Funciton to Get Codes by Entity Type
        public string GetCodeById(string objName, Guid obj_Id)
        {
            string ObjCode = string.Empty;
            using (ConsumerEntities context = new ConsumerEntities())
            {
                if (objName == "supplier")
                {
                    ObjCode = context.Supplier.Find(obj_Id).Code;
                }
                if (objName == "country")
                {
                    ObjCode = context.m_CountryMaster.Find(obj_Id).Code;
                }
                if (objName == "city")
                {
                    ObjCode = context.m_CityMaster.Find(obj_Id).Code;
                }
                if (objName == "state")
                {
                    ObjCode = context.m_States.Find(obj_Id).StateCode;
                }
                if (objName == "product")
                {
                    ObjCode = context.Accommodation.Find(obj_Id).CompanyHotelID.ToString();
                }
                return ObjCode;
            }
        }
        public string GetRemarksForMapping(string from, Guid Mapping_Id)
        {
            string Remarks = string.Empty;
            using (ConsumerEntities context = new ConsumerEntities())
            {
                if (from == "country")
                {
                    Remarks = context.m_CountryMapping.Find(Mapping_Id).Remarks;
                }
                if (from == "city")
                {
                    Remarks = context.m_CityMapping.Find(Mapping_Id).Remarks;
                }
                if (from == "product")
                {
                    Remarks = context.Accommodation_ProductMapping.Find(Mapping_Id).Remarks;
                }
            }
            return Remarks;
        }
        public DC_GenericMasterDetails_ByIDOrName GetDetailsByIdOrName(DC_GenericMasterDetails_ByIDOrName _obj)
        {
            DC_GenericMasterDetails_ByIDOrName _result = new DC_GenericMasterDetails_ByIDOrName();
            using (ConsumerEntities context = new ConsumerEntities())
            {
                if (_obj.ObjName == EntityType.country)
                {
                    if (_obj.WhatFor == DetailsWhatFor.CodeById)
                    {
                        _result.Code = context.m_CountryMaster.Find(_obj.ID).Code;
                    }
                    else if (_obj.WhatFor == DetailsWhatFor.RemarksForMapping)
                    {
                        _result.Remark = context.m_CountryMapping.Find(_obj.ID).Remarks;
                    }
                    else if (_obj.WhatFor == DetailsWhatFor.IDByName)
                    {
                        if (!string.IsNullOrWhiteSpace(_obj.Name))
                        {
                            var country = (from ct in context.m_CountryMaster
                                           where ct.Status.ToUpper() == "ACTIVE" && ct.Name.ToUpper().Trim() == _obj.Name.Trim().ToUpper()
                                           select new { ct.Country_Id }).FirstOrDefault();
                            _result.ID = country.Country_Id;
                        }
                    }
                }
                else if (_obj.ObjName == EntityType.city)
                {
                    if (_obj.WhatFor == DetailsWhatFor.CodeById)
                    {
                        _result.Code = context.m_CityMaster.Find(_obj.ID).Code;
                    }
                    else if (_obj.WhatFor == DetailsWhatFor.RemarksForMapping)
                    {
                        _result.Remark = context.m_CityMapping.Find(_obj.ID).Remarks;
                    }
                    else if (_obj.WhatFor == DetailsWhatFor.IDByName)
                    {
                        if (!string.IsNullOrWhiteSpace(_obj.Optional1))
                        {
                            //Get Country_Id from Country master 
                            Guid? Country_id = Guid.Empty;
                            Country_id = (from a in context.m_CountryMaster.AsNoTracking() where a.Name.ToLower().Trim() == _obj.Optional1.ToLower().Trim() select a.Country_Id).FirstOrDefault();

                            _result.ID = (from ct in context.m_CityMaster.AsNoTracking()
                                          where ct.Status.ToUpper() == "ACTIVE" && _obj.Name.Trim().ToUpper() == ct.Name.ToUpper().Trim()
                                          && ct.Country_Id == Country_id
                                          select ct.City_Id).FirstOrDefault();
                        }
                    }
                }
                else if (_obj.ObjName == EntityType.product)
                {
                    if (_obj.WhatFor == DetailsWhatFor.CodeById)
                    {
                        _result.Code = context.Accommodation.Find(_obj.ID).CompanyHotelID.ToString();
                    }
                    else if (_obj.WhatFor == DetailsWhatFor.RemarksForMapping)
                    {
                        _result.Remark = context.Accommodation_ProductMapping.Find(_obj.ID).Remarks;
                    }
                }
                else if (_obj.ObjName == EntityType.supplier)
                {
                    _result.Code = context.Supplier.Find(_obj.ID).Code;
                }
                else if (_obj.ObjName == EntityType.state)
                {
                    if (_obj.WhatFor == DetailsWhatFor.IDByName)
                    {
                        if (!string.IsNullOrWhiteSpace(_obj.Name))
                        {
                            //Get Country_Id from Country master 
                            Guid? Country_id = Guid.Empty;
                            Country_id = (from a in context.m_CountryMaster.AsNoTracking() where a.Name.ToLower().Trim() == _obj.Optional1.ToLower().Trim() select a.Country_Id).FirstOrDefault();

                            _result.ID = (from st in context.m_States.AsNoTracking()
                                          where st.StateName.ToLower() == _obj.Name.ToLower() && st.Country_Id == Country_id
                                          select st.State_Id).FirstOrDefault();
                        }
                    }
                    else
                    {
                        _result.Code = context.m_States.Find(_obj.ID).StateCode;
                    }
                }
            }
            return _result;
        }
        #endregion

        #region To Fill DropDown
        public List<DC_Accomodation_DDL> GetProductByCity(DC_Accomodation_DDL _obj)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var productmaster = (from ct in context.Accommodation.AsNoTracking()
                                         where ct.city.Trim().ToUpper() == _obj.CityName.Trim().ToUpper()
                                         && ct.country.Trim().ToUpper() == _obj.CountryName.Trim().ToUpper()
                                         && ct.IsActive == _obj.IsActive
                                         orderby ct.HotelName ascending
                                         select new DC_Accomodation_DDL
                                         {
                                             Accommodation_Id = ct.Accommodation_Id,
                                             HotelName = ct.HotelName
                                         }).ToList();
                    return productmaster;
                }

            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching product master for dropdown", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public List<DC_State_Master_DDL> GetMasterStateData(Guid _guidCountry_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var stateMaster = (from ct in context.m_States.AsNoTracking()
                                       where ct.Country_Id == _guidCountry_Id
                                       orderby ct.StateName ascending
                                       select new DC_State_Master_DDL
                                       {
                                           StateCode = ct.StateCode,
                                           StateName = ct.StateName,
                                       }).ToList();
                    return stateMaster;
                }

            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching product master for dropdown", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public List<DC_City_Master_DDL> GetMasterCityData(Guid _guidCountry_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var citymaster = (from ct in context.m_CityMaster.AsNoTracking()
                                      where ct.Status.ToUpper() == "ACTIVE"
                                          && ct.Country_Id == _guidCountry_Id
                                      select new DC_City_Master_DDL
                                      {
                                          Name = ct.Name,
                                          City_Id = ct.City_Id,
                                          Code = ct.Code
                                      }).ToList();

                    citymaster = citymaster.OrderBy(o => o.Name).ToList();
                    return citymaster;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching product master for dropdown", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public List<DC_CityAreaLocation> GetMasterCityAreaLocationDetail(Guid CityAreaLocation_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var cityAreaLocationMaster = (from ct in context.m_CityAreaLocation.AsNoTracking()
                                                  where ct.CityAreaLocation_Id == CityAreaLocation_Id
                                                  orderby ct.Name ascending
                                                  select new DC_CityAreaLocation
                                                  {
                                                      Code = ct.Code,
                                                      Name = ct.Name,
                                                      CityAreaLocation_Id = ct.CityAreaLocation_Id,
                                                      CityArea_Id = ct.CityArea_Id,
                                                      Create_User = ct.Create_User,
                                                      Create_Date = ct.Create_Date,
                                                      Edit_User = ct.Edit_User,
                                                      Edit_Date = ct.Edit_Date
                                                  }).ToList();
                    return cityAreaLocationMaster;
                }

            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error in GetMasterCityAreaLocationDetail for dropdown", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public List<DC_CityArea> GetMasterCityAreaDetail(Guid CityArea_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var cityAreaMaster = (from ct in context.m_CityArea.AsNoTracking()
                                          where ct.CityArea_Id == CityArea_Id
                                          orderby ct.Name ascending
                                          select new DC_CityArea
                                          {
                                              Code = ct.Code,
                                              Name = ct.Name,
                                              CityArea_Id = ct.CityArea_Id,
                                              Create_User = ct.Create_User,
                                              Create_Date = ct.Create_Date,
                                              Edit_User = ct.Edit_User,
                                              Edit_Date = ct.Edit_Date
                                          }).ToList();
                    return cityAreaMaster;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error in GetMasterCityAreaDetail for dropdown", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public List<DC_Supplier_DDL> GetSupplierMasterData()
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var suppliers = (from sm in context.Supplier.AsNoTracking()
                                     where sm.StatusCode.ToUpper().Trim() == "ACTIVE"
                                     orderby sm.Name ascending
                                     select new DC_Supplier_DDL
                                     {
                                         Supplier_Id = sm.Supplier_Id,
                                         Name = sm.Name,
                                         Code = sm.Code
                                     }
                                    ).ToList();
                    return suppliers;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DC_Supplier_DDL GetSupplierDataByMapping_Id(string objName, Guid Mapping_Id)
        {
            DC_Supplier_DDL supdata = new DC_Supplier_DDL();
            using (ConsumerEntities context = new ConsumerEntities())
            {
                var SuppliersMaster = context.Supplier.Select(s => s).AsQueryable();
                var CountryMappingMaster = context.m_CountryMapping.Select(s => s).AsQueryable();
                var CityMappingMaster = context.m_CityMapping.Select(s => s).AsQueryable();

                if (objName.ToUpper().Trim() == "COUNTRY")
                {
                    var sdata = (from m in CountryMappingMaster
                                 join s in SuppliersMaster on m.Supplier_Id equals s.Supplier_Id
                                 where m.CountryMapping_Id == Mapping_Id
                                 orderby s.Name ascending
                                 select new DC_Supplier_DDL
                                 {
                                     Supplier_Id = s.Supplier_Id,
                                     Name = s.Name,
                                     Code = s.Code
                                 }).ToList();
                    supdata = sdata[0];
                }
                if (objName.ToUpper().Trim() == "CITY")
                {
                    var sdata = (from m in CityMappingMaster
                                 join s in SuppliersMaster on m.Supplier_Id equals s.Supplier_Id
                                 where m.CityMapping_Id == Mapping_Id
                                 orderby s.Name ascending
                                 select new DC_Supplier_DDL
                                 {
                                     Supplier_Id = s.Supplier_Id,
                                     Name = s.Name,
                                     Code = s.Code
                                 }).ToList();
                    supdata = sdata[0];
                }
            }
            return supdata;
        }
        public List<DC_State_Master_DDL> GetStateByCity(Guid City_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var citycode = (from ct in context.m_CityMaster.AsNoTracking()
                                    where ct.City_Id == City_Id
                                    select new DC_State_Master_DDL
                                    {
                                        StateCode = ct.StateCode,
                                        StateName = ct.StateName
                                    }).ToList();
                    return citycode;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public List<DC_CountryMaster> GetCountryCodes(Guid obj_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var countrycode = (from ct in context.m_CountryMaster.AsNoTracking()
                                       where ct.Country_Id == obj_Id
                                       select new DC_CountryMaster
                                       {
                                           Country_ID = ct.Country_Id,
                                           Name = ct.Name,
                                           Code = ct.Code,
                                           ISO3166_1_Alpha_2 = ct.ISO3166_1_Alpha_2,
                                           ISO3166_1_Alpha_3 = ct.ISO3166_1_Alpha_3
                                       }).ToList();

                    return countrycode;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<DC_CountryMaster> GetMasterCountryDataList()
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var countrycode = (from ct in context.m_CountryMaster.AsNoTracking()
                                       where ct.Status.ToUpper() == "ACTIVE"
                                       orderby ct.Name ascending
                                       select new DC_CountryMaster
                                       {
                                           Country_ID = ct.Country_Id,
                                           Name = ct.Name,
                                           Code = ct.Code,
                                           NameWithCode = ct.Name + " (" + ct.Code + ")",
                                           ISO3166_1_Alpha_2 = ct.ISO3166_1_Alpha_2 ?? "",
                                           ISO3166_1_Alpha_3 = ct.ISO3166_1_Alpha_3 ?? ""
                                       }).ToList();

                    return countrycode;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<DC_Activity_DDL> GetActivityByCountryCity(string CountryName, string CityName)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var resultActivity = (from a in context.Activities.AsNoTracking() where a.Product_Name != null select a);

                    if (!string.IsNullOrWhiteSpace(CountryName))
                    {
                        if (CountryName != "null")
                        {
                            resultActivity = from a in resultActivity where a.Country.ToLower().Trim() == CountryName select a;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(CityName))
                    {
                        if (CityName != "null")
                            resultActivity = from a in resultActivity where a.City.ToLower().Trim() == CityName select a;
                    }

                    var result = (from act in resultActivity
                                  where act.IsActive == true
                                  orderby act.Product_Name ascending
                                  select new DC_Activity_DDL
                                  {
                                      Activity_Id = act.Activity_Id,
                                      Product_Name = act.Product_Name,
                                  }).ToList();
                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public DC_State_Master_DDL GetStateNameAndCode(DC_State_Master_DDL_RQ RQ)
        {
            DC_State_Master_DDL _obj = new DC_State_Master_DDL();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var stateMaster = context.m_States.Select(s => s).AsQueryable();
                    if (!string.IsNullOrEmpty(RQ.State_ID))
                    {

                        Guid _stateId = Guid.Parse(RQ.State_ID);
                        if (_stateId != Guid.Empty)
                        {
                            stateMaster = from x in stateMaster where x.State_Id == _stateId select x;
                        }
                    }
                    if (!string.IsNullOrEmpty(RQ.StateName))
                    {
                        stateMaster = from x in stateMaster where x.StateName.ToLower() == RQ.StateName.ToLower() select x;
                    }
                    if (!string.IsNullOrEmpty(RQ.StateCode))
                    {
                        stateMaster = from x in stateMaster where x.StateCode.ToLower() == RQ.StateCode.ToLower() select x;
                    }

                    if (stateMaster.Count() == 1)
                    {
                        _obj.StateCode = stateMaster.Select(s => s.StateCode).FirstOrDefault();
                        _obj.StateName = stateMaster.Select(s => s.StateName).FirstOrDefault();
                    }
                }
            }
            catch
            {

            }
            return _obj;
        }
        public List<DC_Supplier_DDL> GetSuppliersByProductCategory(string ProductCategory)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var suppliers = (from sm in context.Supplier.AsNoTracking()
                                     where sm.StatusCode.ToUpper().Trim() == "ACTIVE"
                                     orderby sm.Name ascending
                                     select new DC_Supplier_DDL
                                     {
                                         Supplier_Id = sm.Supplier_Id,
                                         Name = sm.Name,
                                         Code = sm.Code,
                                         Priority = sm.Priority ?? 0
                                     }
                                    ).ToList();
                    if (ProductCategory != "0")
                    {
                        suppliers = (from sm in context.Supplier.AsNoTracking()
                                     join pm in context.Supplier_ProductCategory.AsNoTracking() on sm.Supplier_Id equals pm.Supplier_Id
                                     where sm.StatusCode.ToUpper().Trim() == "ACTIVE" &&
                                            pm.ProductCategory == ProductCategory
                                     orderby sm.Name ascending
                                     select new DC_Supplier_DDL
                                     {
                                         Supplier_Id = sm.Supplier_Id,
                                         Name = sm.Name,
                                         Code = sm.Code,
                                         Priority = sm.Priority ?? 0
                                     }
                                   ).Distinct().ToList();
                    }

                    return suppliers;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region City Area and Location
        public bool SaveCityAreaLocation(DC_CityAreaLocation obj)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (obj.Option.ToUpper() == "SAVE")
                    {
                        m_CityAreaLocation myCAl = new m_CityAreaLocation();
                        myCAl.CityAreaLocation_Id = obj.CityAreaLocation_Id;
                        myCAl.CityArea_Id = obj.CityArea_Id;
                        myCAl.City_Id = obj.City_Id;
                        myCAl.Code = obj.Code;
                        myCAl.Name = obj.Name;
                        myCAl.Create_Date = DateTime.Now;
                        myCAl.Create_User = System.Web.HttpContext.Current.User.Identity.Name;
                        context.m_CityAreaLocation.Add(myCAl);
                    }
                    else if (obj.Option.ToUpper() == "UPDATE")
                    {
                        var xD = (from myCa in context.m_CityAreaLocation
                                  where myCa.CityAreaLocation_Id == obj.CityAreaLocation_Id
                                  select myCa).First();

                        xD.Code = obj.Code;
                        xD.Name = obj.Name;
                        xD.Edit_Date = DateTime.Now;
                        xD.Edit_User = System.Web.HttpContext.Current.User.Identity.Name;
                    }
                    context.SaveChanges();

                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<DC_CityAreaLocation> GetMasterCityAreaLocationData(Guid CityArea_Id)
        {
            using (ConsumerEntities context = new ConsumerEntities())
            {
                var cityAreaLocationMaster = (from ct in context.m_CityAreaLocation.AsNoTracking()
                                              where ct.CityArea_Id == CityArea_Id
                                              orderby ct.Name ascending
                                              select new DC_CityAreaLocation
                                              {
                                                  CityAreaLocation_Id = ct.CityAreaLocation_Id,
                                                  City_Id = ct.City_Id,
                                                  Code = ct.Code,
                                                  Name = ct.Name,
                                                  Create_Date = ct.Create_Date,
                                                  Create_User = ct.Create_User,
                                                  Edit_Date = ct.Edit_Date,
                                                  Edit_User = ct.Edit_User
                                              }).ToList();
                return cityAreaLocationMaster;
            }
        }
        public bool SaveCityArea(DC_CityArea obj)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (obj.Option.ToUpper() == "SAVE")
                    {
                        m_CityArea myCA = new m_CityArea();

                        myCA.CityArea_Id = obj.CityArea_Id;
                        myCA.City_Id = obj.City_Id;
                        myCA.Code = obj.Code;
                        myCA.Name = obj.Name;
                        myCA.Create_Date = DateTime.Now;
                        myCA.Create_User = System.Web.HttpContext.Current.User.Identity.Name;
                        context.m_CityArea.Add(myCA);
                    }
                    else if (obj.Option.ToUpper() == "UPDATE")
                    {
                        var xD = (from myCa in context.m_CityArea
                                  where myCa.CityArea_Id == obj.CityArea_Id
                                  select myCa).First();

                        xD.City_Id = obj.City_Id;
                        xD.Code = obj.Code;
                        xD.Name = obj.Name;
                        xD.Edit_Date = DateTime.Now;
                        xD.Edit_User = System.Web.HttpContext.Current.User.Identity.Name;
                    }
                    context.SaveChanges();

                }

                return true;

            }
            catch
            {
                return false;
            }
        }
        public List<DC_CityArea> GetMasterCityAreaData(Guid City_Id)
        {
            using (ConsumerEntities context = new ConsumerEntities())
            {
                var cityAreaMaster = (from ct in context.m_CityArea.AsNoTracking()
                                      where ct.City_Id == City_Id
                                      orderby ct.Name ascending
                                      select new DC_CityArea
                                      {
                                          Code = ct.Code,
                                          Name = ct.Name,
                                          CityArea_Id = ct.CityArea_Id,
                                          Create_User = ct.Create_User,
                                          Create_Date = ct.Create_Date,
                                          Edit_User = ct.Edit_User,
                                          Edit_Date = ct.Edit_Date,
                                          City_Id = ct.City_Id,
                                      }).ToList();
                return cityAreaMaster;
            }
        }
        #endregion

        #region Keyword

        public DC_Message AddUpdateKeyword(DC_Keyword item)
        {
            DataContracts.DC_Message ret = new DataContracts.DC_Message();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {

                    var searchKeywordDuplicate = context.m_keyword.Where(a => a.Keyword == item.Keyword && a.EntityFor == item.EntityFor && a.Keyword_Id != item.Keyword_Id).FirstOrDefault();
                    if (searchKeywordDuplicate != null)
                    {
                        ret.StatusMessage = "Keyword: " + item.Keyword + " already exists.";
                        ret.StatusCode = ReadOnlyMessage.StatusCode.Duplicate;
                        return ret;
                    }

                    var keyword = context.m_keyword.Find(item.Keyword_Id);
                    if (keyword != null) //Update Keyword
                    {
                        if (keyword.Status != item.Status)
                        {
                            keyword.Status = item.Status;
                            keyword.Edit_Date = DateTime.Now;
                            keyword.Edit_User = item.Edit_User;
                            if (item.Status == "INACTIVE")
                            {
                                ret.StatusMessage = "Keyword" + ReadOnlyMessage.strDeleted;
                            }
                            else
                            {
                                ret.StatusMessage = "Keyword" + ReadOnlyMessage.strUnDeleted;
                            }
                        }
                        else
                        {
                            keyword.Keyword = item.Keyword;
                            keyword.Attribute = item.Attribute;
                            keyword.Edit_Date = DateTime.Now;
                            keyword.Edit_User = item.Edit_User;
                            keyword.Sequence = item.Sequence;
                            keyword.Status = item.Status;
                            keyword.Icon = item.Icon;
                            keyword.EntityFor = item.EntityFor;
                            keyword.AttributeType = item.AttributeType;
                            keyword.AttributeLevel = item.AttributeLevel;
                            keyword.AttributeSubLevel = item.AttributeSubLevel;
                            keyword.AttributeSubLevelValue = item.AttributeSubLevelValue;

                            ret.StatusMessage = "Keyword " + ReadOnlyMessage.strUpdatedSuccessfully;
                        }
                    }
                    else //insert keyword
                    {
                        m_keyword newKeyword = new m_keyword
                        {
                            Attribute = item.Attribute,
                            Sequence = item.Sequence,
                            Create_Date = DateTime.Now,
                            Create_User = item.Create_User,
                            Keyword = item.Keyword,
                            Keyword_Id = item.Keyword_Id,
                            Extra = item.Extra,
                            Missing = item.Missing,
                            Status = item.Status,
                            Icon = item.Icon,
                            EntityFor = item.EntityFor,
                            AttributeType = item.AttributeType,
                            AttributeLevel = item.AttributeLevel,
                            AttributeSubLevel = item.AttributeSubLevel,
                            AttributeSubLevelValue = item.AttributeSubLevelValue
                        };
                        context.m_keyword.Add(newKeyword);

                        ret.StatusMessage = "Keyword " + ReadOnlyMessage.strAddedSuccessfully;
                    }

                    if (item.Alias != null)
                    {
                        foreach (var alias in item.Alias)
                        {
                            var searchKeywordAliasDuplicate = context.m_keyword_alias.Where(a => a.Value == alias.Value && a.Keyword_Id == alias.Keyword_Id && a.KeywordAlias_Id != alias.KeywordAlias_Id).FirstOrDefault();
                            if (searchKeywordAliasDuplicate != null)
                            {
                                ret.StatusMessage = "Keyword Alias: " + alias.Value + " already exists.";
                                ret.StatusCode = ReadOnlyMessage.StatusCode.Duplicate;
                                return ret;
                            }

                            var keywordAlias = context.m_keyword_alias.Find(alias.KeywordAlias_Id);
                            if (keywordAlias != null) //update alias
                            {
                                if (keywordAlias.Status != alias.Status)
                                {
                                    keywordAlias.Status = alias.Status;
                                    keywordAlias.Edit_Date = DateTime.Now;
                                    keywordAlias.Edit_User = alias.Edit_User;
                                    if (alias.Status == "INACTIVE")
                                    {
                                        ret.StatusMessage = "Keyword Alias" + ReadOnlyMessage.strDeleted;
                                    }
                                    else
                                    {
                                        ret.StatusMessage = "Keyword Alias" + ReadOnlyMessage.strUnDeleted;
                                    }
                                }
                                else
                                {
                                    keywordAlias.Value = alias.Value;
                                    keywordAlias.Sequence = alias.Sequence;
                                    keywordAlias.Edit_Date = DateTime.Now;
                                    keywordAlias.Edit_User = alias.Edit_User;
                                    keywordAlias.Status = alias.Status;
                                    ret.StatusMessage = "Keyword Alias" + ReadOnlyMessage.strUpdatedSuccessfully;
                                }
                            }
                            else//add alias
                            {
                                m_keyword_alias newKeywordalias = new m_keyword_alias
                                {
                                    KeywordAlias_Id = alias.KeywordAlias_Id,
                                    Value = alias.Value,
                                    Sequence = alias.Sequence,
                                    Create_Date = DateTime.Now,
                                    Create_User = alias.Create_User,
                                    Keyword_Id = alias.Keyword_Id,
                                    Status = alias.Status
                                };
                                context.m_keyword_alias.Add(newKeywordalias);

                                ret.StatusMessage = "Keyword Alias" + ReadOnlyMessage.strAddedSuccessfully;
                            }
                        }
                    }

                    context.SaveChanges();
                }

                ret.StatusCode = ReadOnlyMessage.StatusCode.Success;
                return ret;
            }
            catch
            {
                ret.StatusMessage = "Keyword " + ReadOnlyMessage.strFailed;
                ret.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                return ret;
            }
        }

        public List<DataContracts.Masters.DC_Keyword> SearchKeyword(DC_Keyword_RQ obj)
        {
            try
            {
                if (obj == null)
                {
                    obj = new DC_Keyword_RQ();
                    obj.PageNo = 0;
                    obj.PageSize = int.MaxValue;
                    obj.Status = "ACTIVE";
                    obj.AliasStatus = "ACTIVE";
                    obj.Attribute = null;
                }

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = (from ka in context.m_keyword
                                  select ka).AsQueryable();

                    if (obj.Keyword_Id != null)
                    {
                        search = from r in search
                                 where r.Keyword_Id == obj.Keyword_Id
                                 select r;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.systemWord))
                    {
                        search = from r in search
                                 where r.Keyword.Trim().ToUpper().Contains(obj.systemWord.Trim().ToUpper())
                                 select r;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.Status))
                    {
                        search = from r in search
                                 where r.Status.Trim().ToUpper() == obj.Status.Trim().ToUpper()
                                 select r;
                    }

                    if (obj.Attribute != null)
                    {
                        search = from r in search
                                 where (r.Attribute ?? false) == obj.Attribute
                                 select r;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.EntityFor))
                    {
                        var EntityForArray = obj.EntityFor.Split(',');

                        List<DynamicFilter> filter = new List<DynamicFilter>();

                        foreach (var entity in EntityForArray)
                        {
                            filter.Add(new DynamicFilter
                            {
                                PropertyName = "EntityFor",
                                Operation = DynamicFilterOp.Contains,
                                Value = entity
                            });
                        }

                        var deleg = ExpressionBuilder.GetExpressionOr<m_keyword>(filter).Compile();
                        search = search.Where(deleg).AsQueryable();
                    }

                    int total = search.Count();
                    int skip = obj.PageNo * obj.PageSize;

                    var searchAlias = from a in context.m_keyword_alias
                                      select a;
                    if (!string.IsNullOrWhiteSpace(obj.Alias))
                    {
                        searchAlias = from a in searchAlias
                                      where a.Value.Contains(obj.Alias)
                                      select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.AliasStatus))
                    {
                        searchAlias = from a in searchAlias
                                      where a.Status == obj.AliasStatus
                                      select a;
                    }

                    var result = from a in search
                                 orderby a.Sequence ?? 0, a.Keyword
                                 select new DataContracts.Masters.DC_Keyword
                                 {
                                     Keyword_Id = a.Keyword_Id,
                                     Keyword = a.Keyword,
                                     Missing = a.Missing,
                                     Extra = a.Extra,
                                     Status = a.Status,
                                     Create_Date = a.Create_Date,
                                     Create_User = a.Create_User,
                                     Edit_Date = a.Edit_Date,
                                     Edit_User = a.Edit_User,
                                     TotalRecords = total,
                                     Attribute = a.Attribute ?? false,
                                     Sequence = a.Sequence ?? 0,
                                     Icon = a.Icon,
                                     EntityFor = a.EntityFor,
                                     AttributeType = a.AttributeType,
                                     AttributeLevel = a.AttributeLevel,
                                     AttributeSubLevel = a.AttributeSubLevel,
                                     AttributeSubLevelValue = a.AttributeSubLevelValue,
                                     Alias = (from al in searchAlias
                                              where al.Keyword_Id == a.Keyword_Id
                                              orderby (al.Sequence ?? 0), al.Value
                                              select new DC_keyword_alias
                                              {
                                                  KeywordAlias_Id = al.KeywordAlias_Id,
                                                  Keyword_Id = a.Keyword_Id,
                                                  Create_Date = al.Create_Date,
                                                  Create_User = al.Create_User,
                                                  Edit_Date = al.Edit_Date,
                                                  Edit_User = al.Edit_User,
                                                  Status = al.Status,
                                                  Value = al.Value,
                                                  Sequence = al.Sequence ?? 0,
                                                  NoOfHits = al.NoOfHits ?? 0,
                                                  NewHits = 0
                                              }).ToList()
                                 };


                    return result.Skip(skip).Take(obj.PageSize).ToList();


                }
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error While Searching", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });

            }
        }

        public List<DC_keyword_alias> SearchKeywordAlias(DC_Keyword_RQ obj)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from ka in context.m_keyword_alias
                                 where ka.Keyword_Id == obj.Keyword_Id
                                 select ka;

                    if (!string.IsNullOrWhiteSpace(obj.Alias))
                    {
                        search = from r in search
                                 where r.Value.Trim().ToUpper().Contains(obj.Alias.Trim().ToUpper())
                                 select r;
                    }

                    int total = search.Count();
                    int skip = obj.PageNo * obj.PageSize;

                    var result = from a in search
                                 orderby a.Sequence ?? 0, a.Value
                                 select new DataContracts.Masters.DC_keyword_alias
                                 {
                                     Keyword_Id = a.Keyword_Id ?? Guid.Empty,
                                     Status = a.Status,
                                     Create_Date = a.Create_Date,
                                     Create_User = a.Create_User,
                                     Edit_Date = a.Edit_Date,
                                     Edit_User = a.Edit_User,
                                     TotalRecords = total,
                                     Sequence = a.Sequence ?? 0,
                                     KeywordAlias_Id = a.KeywordAlias_Id,
                                     Value = a.Value,
                                     NoOfHits = a.NoOfHits ?? 0,
                                     NewHits = 0
                                 };


                    return result.Skip(skip).Take(obj.PageSize).ToList();
                }
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error While Searching", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });

            }
        }

        public void DataHandler_Keyword_Update_NoOfHits(List<DC_keyword_alias> NoOfHits)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    foreach (var item in NoOfHits)
                    {
                        var KeywordAliasItem = context.m_keyword_alias.Find(item.KeywordAlias_Id);
                        if (KeywordAliasItem != null)
                        {
                            KeywordAliasItem.NoOfHits = (KeywordAliasItem.NoOfHits ?? 0) + item.NewHits;
                        }
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error While Updating Keyword Alias Number Of Hits", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });

            }
        }

        public DC_Message ApplyKeyword(DC_keywordApply_RQ RQ)
        {
            try
            {
                bool bIsProgressLog = false;

                if (RQ.File_Id.HasValue)
                    bIsProgressLog = true;

                DataContracts.UploadStaticData.DC_SupplierImportFile_Progress PLog = new DataContracts.UploadStaticData.DC_SupplierImportFile_Progress();
                DL_UploadStaticData USD = new DL_UploadStaticData();

                if (bIsProgressLog)
                {
                    PLog.SupplierImportFileProgress_Id = Guid.NewGuid();
                    PLog.SupplierImportFile_Id = RQ.File_Id;
                    PLog.Step = "KEYWORD";
                    PLog.Status = "KEYWORDREPLACE";
                    PLog.CurrentBatch = RQ.CurrentBatch;
                    PLog.TotalBatch = RQ.TotalBatch;
                }

                #region Get all entity related Keywords 

                List<DataContracts.Masters.DC_Keyword> Keywords = new List<DataContracts.Masters.DC_Keyword>();
                using (DL_Masters objDL = new DL_Masters())
                {
                    Keywords = objDL.SearchKeyword(new DC_Keyword_RQ { EntityFor = RQ.KeywordEntity, PageNo = 0, PageSize = int.MaxValue, Status = "ACTIVE", AliasStatus = "ACTIVE" });
                }

                List<DataContracts.Masters.DC_Keyword> Attributes = Keywords.Where(w => w.Attribute == true).ToList();

                #endregion

                #region Update Progress to 15%

                if (bIsProgressLog)
                {
                    PLog.PercentageValue = 15;
                    USD.AddStaticDataUploadProcessLog(PLog);
                }

                #endregion

                #region Fetch the data which needs to be TTFU

                if (string.IsNullOrWhiteSpace(RQ.SearchTable) || string.IsNullOrWhiteSpace(RQ.TakeColumn) || string.IsNullOrWhiteSpace(RQ.UpdateColumn) || RQ.TablePrimaryKeys.Length == 0)
                {
                    return new DC_Message { StatusCode = ReadOnlyMessage.StatusCode.Danger, StatusMessage = "Bad Request" };
                }

                List<DC_keywordApplyToTarget> targetStructure = new List<DC_keywordApplyToTarget>();
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    context.Configuration.AutoDetectChangesEnabled = false;
                    context.Database.CommandTimeout = 0;

                    if (RQ.SearchTable.Trim().ToUpper() == "ACCOMMODATION" && RQ.TakeColumn.Trim().ToUpper() == "FULLADDRESS")
                    {
                        var PKIdsFilter = RQ.TablePrimaryKeys.Select(x => Guid.Parse(x)).ToList();
                        using (var transaction = context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                        {
                            targetStructure = context.Accommodation.AsQueryable()
                                    .Where(w => PKIdsFilter.Contains(w.Accommodation_Id))
                                    .Select(s => new DC_keywordApplyToTarget
                                    {
                                        EditUser = RQ.EditUser,
                                        PrimaryKey = s.Accommodation_Id.ToString(),
                                        SourceColumnName = RQ.TakeColumn,
                                        SourceColumnValue = s.FullAddress,
                                        TableName = RQ.SearchTable,
                                        TargetColumnName = RQ.UpdateColumn
                                    }).ToList();
                        }
                    }
                    else if (RQ.SearchTable.Trim().ToUpper() == "ACCOMMODATION_PRODUCTMAPPING" && RQ.TakeColumn.Trim().ToUpper() == "ADDRESS")
                    {
                        var PKIdsFilter = RQ.TablePrimaryKeys.Select(x => Guid.Parse(x)).ToList();
                        using (var transaction = context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                        {
                            targetStructure = context.Accommodation_ProductMapping.AsQueryable()
                                    .Where(w => PKIdsFilter.Contains(w.Accommodation_ProductMapping_Id))
                                    .Select(s => new DC_keywordApplyToTarget
                                    {
                                        EditUser = RQ.EditUser,
                                        PrimaryKey = s.Accommodation_ProductMapping_Id.ToString(),
                                        SourceColumnName = RQ.TakeColumn,
                                        SourceColumnValue = s.address,
                                        TableName = RQ.SearchTable,
                                        TargetColumnName = RQ.UpdateColumn
                                    }).ToList();
                        }
                    }
                }

                #endregion

                #region Update Progress to 25%
                if (bIsProgressLog)
                {
                    PLog.PercentageValue = 25;
                    USD.AddStaticDataUploadProcessLog(PLog);
                }
                #endregion

                #region Loop through the data to TTFU
                int i = 0;
                foreach (var inputData in targetStructure)
                {
                    i = i + 1;

                    try
                    {
                        List<DataContracts.Mapping.DC_SupplierRoomName_AttributeList> AttributeList = new List<DataContracts.Mapping.DC_SupplierRoomName_AttributeList>();

                        string BaseValue = inputData.SourceColumnValue;
                        string TX_Value = string.Empty;
                        string SX_Value = string.Empty;

                        BaseValue = CommonFunctions.TTFU(ref Keywords, ref AttributeList, ref TX_Value, ref SX_Value, BaseValue, new string[] { });

                        //Value assignment
                        inputData.TargetColumnValue = BaseValue;
                        //inputData.AttributeList = AttributeList.ToList(); //This may come later on so, kept the structure

                        #region UpdateToDB

                        //Update keyword replaced values to DB
                        using (ConsumerEntities context = new ConsumerEntities())
                        {
                            #region ForFutureUse
                            //Remove Existing Attribute List Records
                            //context.Accommodation_SupplierRoomTypeAttributes.RemoveRange(context.Accommodation_SupplierRoomTypeAttributes.Where(w => w.RoomTypeMap_Id == srn.RoomTypeMap_Id));

                            //context.Accommodation_SupplierRoomTypeAttributes.AddRange((from a in inputData.AttributeList
                            //                                                           select new Accommodation_SupplierRoomTypeAttributes
                            //                                                           {
                            //                                                               RoomTypeMapAttribute_Id = Guid.NewGuid(),
                            //                                                               RoomTypeMap_Id = inputData.RoomTypeMap_Id,
                            //                                                               SupplierRoomTypeAttribute = a.SupplierRoomTypeAttribute,
                            //                                                               SystemAttributeKeyword = a.SystemAttributeKeyword,
                            //                                                               SystemAttributeKeyword_Id = a.SystemAttributeKeywordID
                            //                                                           }).ToList());
                            #endregion

                            if (inputData.TableName.Trim().ToUpper() == "ACCOMMODATION" && inputData.TargetColumnName.Trim().ToUpper() == "ADDRESS_TX")
                            {
                                var search = context.Accommodation.Find(Guid.Parse(inputData.PrimaryKey));
                                if (search != null)
                                {
                                    search.Address_Tx = inputData.TargetColumnValue;
                                }
                            }

                            else if (inputData.TableName.Trim().ToUpper() == "ACCOMMODATION_PRODUCTMAPPING" && inputData.TargetColumnName.Trim().ToUpper() == "ADDRESS_TX")
                            {
                                var search = context.Accommodation_ProductMapping.Find(Guid.Parse(inputData.PrimaryKey));
                                if (search != null)
                                {
                                    search.address_tx = inputData.TargetColumnValue;
                                }
                            }

                            context.SaveChanges();
                        }

                        #endregion
                    }
                    catch
                    {
                        continue;
                    }

                    if (bIsProgressLog)
                    {
                        if (i % 5 == 0)
                        {
                            PLog.PercentageValue = 25 + ((60 * i) / targetStructure.Count);
                            USD.AddStaticDataUploadProcessLog(PLog);
                        }
                    }
                }
                #endregion

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

                if (bIsProgressLog)
                {
                    PLog.PercentageValue = 100;
                    USD.AddStaticDataUploadProcessLog(PLog);
                }

                return new DataContracts.DC_Message { StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Success, StatusMessage = "Keyword Replace has been done." };

            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while Keyword Replace", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public DC_Message ReRunKeyword(string Entity, string Table, Guid Supplier_id)
        {
            try
            {
                DC_Message msg = new DC_Message();
                List<DataContracts.Masters.DC_Keyword> Keywords = new List<DataContracts.Masters.DC_Keyword>();

                #region Get all entity related Keywords 
                using (DL_Masters objDL = new DL_Masters())
                {
                    Keywords = objDL.SearchKeyword(new DataContracts.Masters.DC_Keyword_RQ { EntityFor = Entity, PageNo = 0, PageSize = int.MaxValue, Status = "ACTIVE", AliasStatus = "ACTIVE" });
                }
                #endregion

                if (Entity.Trim().ToUpper() == "HOTELNAME" && Table == "MASTER")
                {
                    Keywords = ReRunKeyword_HotelName_Acco(Keywords);
                    msg = new DC_Message { StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Success, StatusMessage = "Keyword ReRun has been done." };
                }
                else if (Entity.Trim().ToUpper() == "HOTELNAME" && Table == "SUPPLIER")
                {
                    Keywords = ReRunKeyword_HotelName_AccoProdMap(Supplier_id, Keywords);
                    msg = new DC_Message { StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Success, StatusMessage = "Keyword ReRun has been done." };
                }
                else if (Entity.Trim().ToUpper() == "HOTELADDRESS" && Table == "MASTER")
                {
                    Keywords = ReRunKeyword_HotelAddress_Acco(Keywords);
                    msg = new DC_Message { StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Success, StatusMessage = "Keyword ReRun has been done." };
                }
                else if (Entity.Trim().ToUpper() == "HOTELADDRESS" && Table == "SUPPLIER")
                {
                    Keywords = ReRunKeyword_HotelAddress_AccoProdMap(Supplier_id, Keywords);
                    msg = new DC_Message { StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Success, StatusMessage = "Keyword ReRun has been done." };
                }
                else if (Entity.Trim().ToUpper() == "ROOMTYPE" && Table == "SUPPLIER")
                {
                    Keywords = ReRunKeyword_RoomName_AccoRoomMap(Supplier_id, Keywords);
                    msg = new DC_Message { StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Success, StatusMessage = "Keyword ReRun has been done." };
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

                return msg;

            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while Keyword ReRun" + Environment.NewLine + ex.Message, ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public List<DataContracts.Masters.DC_Keyword> ReRunKeyword_HotelName_Acco(List<DataContracts.Masters.DC_Keyword> Keywords)
        {
            try
            {
                #region Fetch and Process the data which needs to be TTFU

                List<DC_SupplierRoomName_AttributeList> AttributeList = new List<DC_SupplierRoomName_AttributeList>();
                List<DC_KeyWordReRun> keywordReRun = new List<DC_KeyWordReRun>();
                string TT_Value = string.Empty;
                string TX_Value = string.Empty;
                string SX_Value = string.Empty;

                #region For Accommodation
                keywordReRun = new List<DC_KeyWordReRun>();

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    context.Configuration.AutoDetectChangesEnabled = false;
                    context.Database.CommandTimeout = 0;

                    using (var transaction = context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                    {
                        keywordReRun = context.Accommodation.AsNoTracking()
                                    .Select(s => new DC_KeyWordReRun
                                    {
                                        RowId = s.Accommodation_Id,
                                        OriginalValue = s.HotelName,
                                        CityName = s.city ?? string.Empty,
                                        CountryName = s.country ?? string.Empty
                                    }).ToList();
                    }
                }

                foreach (var data in keywordReRun)
                {
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(data.OriginalValue))
                        {
                            TT_Value = string.Empty;
                            TX_Value = string.Empty;
                            SX_Value = string.Empty;

                            TT_Value = CommonFunctions.TTFU(ref Keywords, ref AttributeList, ref TX_Value, ref SX_Value, data.OriginalValue, new string[] { data.CityName, data.CountryName });

                            using (ConsumerEntities context = new ConsumerEntities())
                            {
                                context.Database.ExecuteSqlCommand("UPDATE Accommodation SET HotelName_Tx = '" + TT_Value + "', Edit_Date = GETDATE(), Edit_User = 'KeywordReRun' WHERE Accommodation_Id = '" + data.RowId.ToString() + "';");
                                //var entity = context.Accommodation.Find(data.RowId);
                                //if (entity != null)
                                //{
                                //    entity.HotelName_Tx = TT_Value;
                                //    entity.Edit_Date = DateTime.Now;
                                //    entity.Edit_User = "KeywordReRun";
                                //    context.SaveChanges();
                                //    entity = null;
                                //}
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
                #endregion

                #endregion

                return Keywords;
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while Keyword ReRun", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public List<DataContracts.Masters.DC_Keyword> ReRunKeyword_HotelName_AccoProdMap(Guid Supplier_id, List<DataContracts.Masters.DC_Keyword> Keywords)
        {
            try
            {
                string TT_Value = string.Empty;
                string TX_Value = string.Empty;
                string SX_Value = string.Empty;

                List<DC_SupplierRoomName_AttributeList> AttributeList = new List<DC_SupplierRoomName_AttributeList>();
                List<DC_KeyWordReRun> keywordReRun = new List<DC_KeyWordReRun>();

                #region Fetch and Process the data which needs to be TTFU

                #region For Accommodation Product Mapping
                keywordReRun = new List<DC_KeyWordReRun>();
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    context.Configuration.AutoDetectChangesEnabled = false;
                    context.Database.CommandTimeout = 0;
                    using (var transaction = context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                    {
                        if (Supplier_id == Guid.Empty)
                        {
                            keywordReRun = context.Accommodation_ProductMapping.AsNoTracking()
                                    .Select(s => new DC_KeyWordReRun
                                    {
                                        RowId = s.Accommodation_ProductMapping_Id,
                                        OriginalValue = s.ProductName,
                                        CityName = s.CityName ?? string.Empty,
                                        CountryName = s.CountryName ?? string.Empty
                                    }).ToList();
                        }
                        else
                        {
                            keywordReRun = context.Accommodation_ProductMapping.AsNoTracking()
                            .Where(w => w.Supplier_Id == Supplier_id)
                                    .Select(s => new DC_KeyWordReRun
                                    {
                                        RowId = s.Accommodation_ProductMapping_Id,
                                        OriginalValue = s.ProductName,
                                        CityName = s.CityName ?? string.Empty,
                                        CountryName = s.CountryName ?? string.Empty
                                    }).ToList();
                        }

                    }
                }

                foreach (var data in keywordReRun)
                {
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(data.OriginalValue))
                        {
                            TT_Value = string.Empty;
                            TX_Value = string.Empty;
                            SX_Value = string.Empty;

                            TT_Value = CommonFunctions.TTFU(ref Keywords, ref AttributeList, ref TX_Value, ref SX_Value, data.OriginalValue, new string[] { data.CityName, data.CountryName });

                            using (ConsumerEntities context = new ConsumerEntities())
                            {
                                context.Database.ExecuteSqlCommand("UPDATE Accommodation_ProductMapping SET HotelName_Tx = '" + TT_Value + "', Edit_Date = GETDATE(), Edit_User = 'KeywordReRun' WHERE Accommodation_ProductMapping_Id = '" + data.RowId.ToString() + "';");
                                //var entity = context.Accommodation_ProductMapping.Find(data.RowId);
                                //if (entity != null)
                                //{
                                //    entity.HotelName_Tx = TT_Value;
                                //    entity.Edit_Date = DateTime.Now;
                                //    entity.Edit_User = "KeywordReRun";
                                //    context.SaveChanges();
                                //    entity = null;
                                //}
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
                #endregion

                #endregion

                return Keywords;
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while Keyword ReRun", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public List<DataContracts.Masters.DC_Keyword> ReRunKeyword_HotelAddress_Acco(List<DataContracts.Masters.DC_Keyword> Keywords)
        {
            try
            {
                string TT_Value = string.Empty;
                string TX_Value = string.Empty;
                string SX_Value = string.Empty;

                List<DC_SupplierRoomName_AttributeList> AttributeList = new List<DC_SupplierRoomName_AttributeList>();
                List<DC_KeyWordReRun> keywordReRun = new List<DC_KeyWordReRun>();

                #region Fetch and Process the data which needs to be TTFU

                #region For Accommodation
                keywordReRun = new List<DC_KeyWordReRun>();

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    context.Configuration.AutoDetectChangesEnabled = false;
                    context.Database.CommandTimeout = 0;
                    using (var transaction = context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                    {
                        keywordReRun = context.Accommodation.AsNoTracking()
                                .Select(s => new DC_KeyWordReRun
                                {
                                    RowId = s.Accommodation_Id,
                                    OriginalValue = s.FullAddress
                                }).ToList();
                    }
                }

                foreach (var data in keywordReRun)
                {
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(data.OriginalValue))
                        {
                            TT_Value = string.Empty;
                            TX_Value = string.Empty;
                            SX_Value = string.Empty;

                            TT_Value = CommonFunctions.TTFU(ref Keywords, ref AttributeList, ref TX_Value, ref SX_Value, data.OriginalValue, new string[] { });

                            using (ConsumerEntities context = new ConsumerEntities())
                            {
                                context.Database.ExecuteSqlCommand("UPDATE Accommodation SET Address_Tx = '" + TT_Value + "', Edit_Date = GETDATE(), Edit_User = 'KeywordReRun' WHERE Accommodation_Id = '" + data.RowId.ToString() + "';");
                                //var entity = context.Accommodation.Find(data.RowId);
                                //if (entity != null)
                                //{
                                //    entity.Address_Tx = TT_Value;
                                //    entity.Edit_Date = DateTime.Now;
                                //    entity.Edit_User = "KeywordReRun";
                                //    context.SaveChanges();
                                //    entity = null;
                                //}
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
                #endregion

                #endregion

                return Keywords;
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while Keyword ReRun", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public List<DataContracts.Masters.DC_Keyword> ReRunKeyword_HotelAddress_AccoProdMap(Guid Supplier_id, List<DataContracts.Masters.DC_Keyword> Keywords)
        {
            try
            {
                string TT_Value = string.Empty;
                string TX_Value = string.Empty;
                string SX_Value = string.Empty;

                List<DC_SupplierRoomName_AttributeList> AttributeList = new List<DC_SupplierRoomName_AttributeList>();
                List<DC_KeyWordReRun> keywordReRun = new List<DC_KeyWordReRun>();

                #region Fetch and Process the data which needs to be TTFU

                #region For Accommodation Product Mapping
                keywordReRun = new List<DC_KeyWordReRun>();
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    context.Configuration.AutoDetectChangesEnabled = false;
                    context.Database.CommandTimeout = 0;
                    using (var transaction = context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                    {
                        if (Supplier_id == Guid.Empty)
                        {
                            keywordReRun = context.Accommodation_ProductMapping.AsNoTracking()
                                .Select(s => new DC_KeyWordReRun
                                {
                                    RowId = s.Accommodation_ProductMapping_Id,
                                    OriginalValue = s.address
                                }).ToList();
                        }
                        else
                        {
                            keywordReRun = context.Accommodation_ProductMapping.AsNoTracking()
                                .Where(w => w.Supplier_Id == Supplier_id)
                                .Select(s => new DC_KeyWordReRun
                                {
                                    RowId = s.Accommodation_ProductMapping_Id,
                                    OriginalValue = s.address
                                }).ToList();
                        }


                    }
                }

                foreach (var data in keywordReRun)
                {
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(data.OriginalValue))
                        {
                            TT_Value = string.Empty;
                            TX_Value = string.Empty;
                            SX_Value = string.Empty;

                            TT_Value = CommonFunctions.TTFU(ref Keywords, ref AttributeList, ref TX_Value, ref SX_Value, data.OriginalValue, new string[] { });

                            using (ConsumerEntities context = new ConsumerEntities())
                            {
                                context.Database.ExecuteSqlCommand("UPDATE Accommodation_ProductMapping SET address_tx = '" + TT_Value + "', Edit_Date = GETDATE(), Edit_User = 'KeywordReRun' WHERE Accommodation_ProductMapping_Id = '" + data.RowId.ToString() + "';");
                                //var entity = context.Accommodation_ProductMapping.Find(data.RowId);
                                //if (entity != null)
                                //{
                                //    entity.address_tx = TT_Value;
                                //    entity.Edit_Date = DateTime.Now;
                                //    entity.Edit_User = "KeywordReRun";
                                //    context.SaveChanges();
                                //    entity = null;
                                //}
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
                #endregion

                #endregion

                return Keywords;
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while Keyword ReRun", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public List<DataContracts.Masters.DC_Keyword> ReRunKeyword_RoomName_AccoRoomMap(Guid Supplier_id, List<DataContracts.Masters.DC_Keyword> Keywords)
        {
            try
            {
                string TT_Value = string.Empty;
                string TX_Value = string.Empty;
                string SX_Value = string.Empty;

                List<DC_SupplierRoomName_AttributeList> AttributeList = new List<DC_SupplierRoomName_AttributeList>();
                List<DC_KeyWordReRun> keywordReRun = new List<DC_KeyWordReRun>();

                #region Fetch and Process the data which needs to be TTFU

                keywordReRun = new List<DC_KeyWordReRun>();

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    context.Configuration.AutoDetectChangesEnabled = false;
                    context.Database.CommandTimeout = 0;
                    using (var transaction = context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                    {
                        if (Supplier_id == Guid.Empty)
                        {
                            keywordReRun = context.Accommodation_SupplierRoomTypeMapping.AsNoTracking()
                                .Select(s => new DC_KeyWordReRun
                                {
                                    RowId = s.Accommodation_SupplierRoomTypeMapping_Id,
                                    OriginalValue = s.SupplierRoomName
                                }).ToList();
                        }
                        else
                        {
                            keywordReRun = context.Accommodation_SupplierRoomTypeMapping.AsNoTracking()
                            .Where(w => w.Supplier_Id == Supplier_id)

                                .Select(s => new DC_KeyWordReRun
                                {
                                    RowId = s.Accommodation_SupplierRoomTypeMapping_Id,
                                    OriginalValue = s.SupplierRoomName
                                }).ToList();
                        }


                    }
                }

                foreach (var data in keywordReRun)
                {
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(data.OriginalValue))
                        {
                            TT_Value = string.Empty;
                            TX_Value = string.Empty;
                            SX_Value = string.Empty;
                            AttributeList = new List<DC_SupplierRoomName_AttributeList>();

                            TT_Value = CommonFunctions.TTFU(ref Keywords, ref AttributeList, ref TX_Value, ref SX_Value, data.OriginalValue, new string[] { });

                            //Update Room Name Stripped and Attributes
                            using (ConsumerEntities context = new ConsumerEntities())
                            {
                                //Remove Existing Attribute List Records
                                context.Accommodation_SupplierRoomTypeAttributes.RemoveRange(context.Accommodation_SupplierRoomTypeAttributes.Where(w => w.RoomTypeMap_Id == data.RowId));

                                context.Accommodation_SupplierRoomTypeAttributes.AddRange((from a in AttributeList
                                                                                           select new Accommodation_SupplierRoomTypeAttributes
                                                                                           {
                                                                                               RoomTypeMapAttribute_Id = Guid.NewGuid(),
                                                                                               RoomTypeMap_Id = data.RowId,
                                                                                               SupplierRoomTypeAttribute = a.SupplierRoomTypeAttribute,
                                                                                               SystemAttributeKeyword = a.SystemAttributeKeyword,
                                                                                               SystemAttributeKeyword_Id = a.SystemAttributeKeywordID
                                                                                           }).ToList());

                                var srnm = context.Accommodation_SupplierRoomTypeMapping.Find(data.RowId);
                                if (srnm != null)
                                {
                                    srnm.TX_RoomName = TT_Value;
                                    srnm.Tx_StrippedName = SX_Value;
                                    srnm.Tx_ReorderedName = SX_Value;
                                    srnm.Edit_Date = DateTime.Now;
                                    srnm.Edit_User = "KeywordReRun";
                                }

                                context.SaveChanges();
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
                #endregion

                return Keywords;
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while Keyword ReRun", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        #endregion

        public string[] GetColumnNames(string TableName)
        {

            //using (ConsumerEntities context = new ConsumerEntities())
            //{
            //    var query = from meta in context.MetadataWorkspace.GetItems(DataSpace.CSpace)
            //      .Where(m => m.BuiltInTypeKind == BuiltInTypeKind.EntityType)
            //                let m = (meta as EntityType)
            //                where m.BaseType != null
            //                select new
            //                {
            //                    m.Name,
            //                    BaseTypeName = m.BaseType != null ? m.BaseType.Name : null,
            //                };
            //}
            return null;
        }

        public List<string> GetListOfColumnNamesByTable(string TableName)
        {
            List<string> _lstColumnName = new List<string>();
            if (TableName != null)
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    string strtablename = TableName.ToLower().Trim();
                    switch (strtablename)
                    {
                        case "stg_suppliercitymapping":
                            _lstColumnName = typeof(stg_SupplierCityMapping).GetProperties().Select(property => property.Name).ToList();
                            break;
                        case "m_citymaster":
                            _lstColumnName = typeof(m_CityMaster).GetProperties().Select(property => property.Name).ToList();
                            break;
                        case "m_citymapping":
                            _lstColumnName = typeof(m_CityMapping).GetProperties().Select(property => property.Name).ToList();
                            break;
                        case "stg_suppliercountrymapping":
                            _lstColumnName = typeof(stg_SupplierCountryMapping).GetProperties().Select(property => property.Name).ToList();
                            break;
                        case "m_countrymapping":
                            _lstColumnName = typeof(m_CountryMapping).GetProperties().Select(property => property.Name).ToList();
                            break;
                        case "m_countrymaster":
                            _lstColumnName = typeof(m_CountryMaster).GetProperties().Select(property => property.Name).ToList();
                            break;
                        case "stg_supplieractivitymapping":
                            _lstColumnName = typeof(stg_SupplierActivityMapping).GetProperties().Select(property => property.Name).ToList();
                            break;
                        case "activity":
                            _lstColumnName = typeof(Activity).GetProperties().Select(property => property.Name).ToList();
                            break;
                        case "activity_supplierproductmapping":
                            _lstColumnName = typeof(Activity_SupplierProductMapping).GetProperties().Select(property => property.Name).ToList();
                            break;
                        case "stg_supplierproductmapping":
                            _lstColumnName = typeof(stg_SupplierProductMapping).GetProperties().Select(property => property.Name).ToList();
                            break;
                        case "accommodation_productmapping":
                            _lstColumnName = typeof(Accommodation_ProductMapping).GetProperties().Select(property => property.Name).ToList();
                            _lstColumnName.Add("HotelName_Rank");
                            _lstColumnName.Add("Address_Rank");
                            _lstColumnName.Add("GeoLocation_Distance");
                            break;
                        case "accommodation":
                            _lstColumnName = typeof(Accommodation).GetProperties().Select(property => property.Name).ToList();
                            _lstColumnName.Add("HotelName_Rank");
                            _lstColumnName.Add("Address_Rank");
                            _lstColumnName.Add("GeoLocation_Distance");
                            break;
                        case "accommodation_supplierroomtypemapping":
                            _lstColumnName = typeof(Accommodation_SupplierRoomTypeMapping).GetProperties().Select(property => property.Name).ToList();
                            break;
                        case "accommodation_roominfo":
                            _lstColumnName = typeof(Accommodation_RoomInfo).GetProperties().Select(property => property.Name).ToList();
                            break;
                        case "stg_supplierhotelroommapping":
                            _lstColumnName = typeof(stg_SupplierHotelRoomMapping).GetProperties().Select(property => property.Name).ToList();
                            break;
                    }
                }
            }
            return _lstColumnName;
        }

        #region Zone Master
        //Add
        public DataContracts.DC_Message AddzoneMaster(DataContracts.Masters.DC_ZoneRQ param)
        {
            DC_Message _msg = new DC_Message();
            using (ConsumerEntities context = new ConsumerEntities())
            {
                try
                {
                    if (param.Action == "ADD")
                    {
                        if (param.Zone_id == Guid.Empty || param.Zone_id == null)
                        {
                            param.Zone_id = Guid.NewGuid();
                        }
                        m_ZoneMaster obj = new m_ZoneMaster()
                        {
                            Zone_id = param.Zone_id ?? Guid.NewGuid(),
                            Zone_Name = param.Zone_Name,
                            Zone_Type = param.Zone_Type,
                            Latitude = param.Latitude,
                            Longitude = param.Longitude,
                            IsActive = true,//param.Status
                            Status = "Active",
                            Zone_Radius = (decimal)param.Zone_Radius,
                            Create_Date = param.Create_Date,
                            Create_User = param.Create_User,
                            Country_Id = param.Country_id,
                            Zone_SubType = param.Zone_SubType
                        };
                        if (param.City_id != null && param.City_id != Guid.Empty)
                        {
                            ZoneCity_Mapping zcm = new ZoneCity_Mapping()
                            {
                                ZoneCityMapping_Id = Guid.NewGuid(),
                                City_Id = param.City_id,
                                Zone_Id = param.Zone_id,
                                IsActive = true,//param.Status
                                Status = "Active",
                                Create_Date = param.Create_Date,
                                Create_User = param.Create_User
                            };
                            context.ZoneCity_Mapping.Add(zcm);
                        }
                        context.m_ZoneMaster.Add(obj);

                    }
                    else if (param.Action == "UPDATE")
                    {
                        var search = context.m_ZoneMaster.Find(param.Zone_id);
                        if (search != null)
                        {
                            search.Zone_Name = param.Zone_Name;
                            search.Zone_Type = param.Zone_Type;
                            search.Latitude = param.Latitude;
                            search.Longitude = param.Longitude;
                            search.Edit_Date = param.Edit_Date;
                            search.Edit_User = param.Edit_User;
                            search.Zone_Radius = (decimal)param.Zone_Radius;
                            search.Country_Id = param.Country_id;
                            search.Zone_SubType = param.Zone_SubType;
                        }
                        else
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strFailed;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                        }
                    }
                    if (context.SaveChanges() > 0)
                    {
                        _msg.StatusMessage = param.Action == "UPDATE" ? ReadOnlyMessage.strUpdatedSuccessfully : ReadOnlyMessage.strAddedSuccessfully;
                        _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                    }
                    else
                    {
                        _msg.StatusMessage = ReadOnlyMessage.strFailed;
                        _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                    }
                }
                catch (Exception e)
                {
                    throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while adding zone master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
                }
            }
            return _msg;
        }
        public DataContracts.DC_Message AddZoneCityMapping(DataContracts.Masters.DC_ZoneRQ param)
        {
            DC_Message _msg = new DC_Message();
            if (param.Zone_id == Guid.Empty)
            {
                _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Warning;
                _msg.StatusMessage = DataContracts.ReadOnlyMessage.strFailed;
                return _msg;
            }
            using (ConsumerEntities context = new ConsumerEntities())
            {
                try
                {
                    int dupsearch = (from a in context.ZoneCity_Mapping
                                     where a.Zone_Id == param.Zone_id && a.City_Id == param.City_id
                                     select a).Count();
                    if (dupsearch > 0)
                    {
                        _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Duplicate;
                        _msg.StatusMessage = DataContracts.ReadOnlyMessage.strFailed;
                        return _msg;
                    }
                    else
                    {
                        if (param.ZoneCityMapping_Id == null || param.ZoneCityMapping_Id == Guid.Empty)
                        {
                            param.ZoneCityMapping_Id = Guid.NewGuid();
                        }
                        ZoneCity_Mapping zcm = new ZoneCity_Mapping()
                        {
                            ZoneCityMapping_Id = param.ZoneCityMapping_Id ?? Guid.NewGuid(),
                            IsActive = true,
                            Status = "Active",
                            City_Id = param.City_id,
                            Zone_Id = param.Zone_id,
                            Create_Date = param.Create_Date,
                            Create_User = param.Create_User
                        };
                        context.ZoneCity_Mapping.Add(zcm);
                        if (context.SaveChanges() == 1)
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strAddedSuccessfully;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        }
                        else
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strFailed;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                        }
                    }

                }
                catch (Exception e)
                {
                    throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while adding city master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
                }
            }
            return _msg;


        }
        //search
        public List<DataContracts.Masters.DC_ZoneSearch> SearchZone(DataContracts.Masters.DC_ZoneRQ param)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    context.Configuration.AutoDetectChangesEnabled = false;

                    var CityMasterIQ = context.m_CityMaster.AsNoTracking().AsQueryable();
                    var ZonemasterIQ = context.m_ZoneMaster.AsNoTracking().AsQueryable();
                    var ZoneCityMasterIQ = context.ZoneCity_Mapping.AsNoTracking().AsQueryable().Where(s => s.IsActive == true);
                    var CountrymasterIQ = context.m_CountryMaster.AsNoTracking().AsQueryable();

                    if (param.Zone_id != Guid.Empty && param.Zone_id != null)
                    {
                        ZonemasterIQ = ZonemasterIQ.Where(x => x.Zone_id == param.Zone_id);
                    }

                    if (param.City_id != Guid.Empty && param.City_id != null)
                    {
                        CityMasterIQ = CityMasterIQ.Where(x => x.City_Id == param.City_id);
                        ZoneCityMasterIQ = ZoneCityMasterIQ.Where(x => x.City_Id == param.City_id);
                        ZonemasterIQ = (from az in ZonemasterIQ
                                        join bz in ZoneCityMasterIQ on az.Zone_id equals bz.Zone_Id
                                        select az);
                    }

                    if (param.Country_id != Guid.Empty && param.Country_id != null)
                    {
                        ZonemasterIQ = ZonemasterIQ.Where(x => x.Country_Id == param.Country_id);
                        //CountrymasterIQ = CountrymasterIQ.Where(y => y.Country_Id == param.Country_id);
                    }

                    if (!string.IsNullOrWhiteSpace(param.Zone_Type))
                    {
                        ZonemasterIQ = ZonemasterIQ.Where(x => x.Zone_Type == param.Zone_Type);
                    }

                    if (!string.IsNullOrWhiteSpace(param.Zone_Name))
                    {
                        ZonemasterIQ = ZonemasterIQ.Where(x => x.Zone_Name.Contains(param.Zone_Name));
                    }

                    if (!string.IsNullOrWhiteSpace(param.Status))
                    {
                        ZonemasterIQ = ZonemasterIQ.Where(x => x.Status == param.Status);
                    }

                    if (!string.IsNullOrWhiteSpace(param.Zone_SubType))
                    {
                        ZonemasterIQ = ZonemasterIQ.Where(x => x.Zone_SubType == param.Zone_SubType);
                    }

                    int total = ZonemasterIQ.Count();
                    int skip = (param.PageNo ?? 0) * (param.PageSize ?? 0);
                    var search = (from zm in ZonemasterIQ
                                  join zcm in ZoneCityMasterIQ on zm.Zone_id equals zcm.Zone_Id into list1
                                  from l1 in list1.DefaultIfEmpty()
                                  join cm in CityMasterIQ on l1.City_Id equals cm.City_Id into list2
                                  from l2 in list2.DefaultIfEmpty()
                                  join com in CountrymasterIQ on zm.Country_Id equals com.Country_Id into list3
                                  from l3 in list3.DefaultIfEmpty()
                                  select (new DC_ZoneSearch
                                  {
                                      CountryName = l3.Name,
                                      CityName = l2.Name,
                                      Zone_Name = zm.Zone_Name,
                                      Zone_Type = zm.Zone_Type,
                                      Zone_id = zm.Zone_id,
                                      City_id = l2.City_Id,
                                      Country_id = l3.Country_Id,
                                      IsActive = zm.IsActive,
                                      Status = zm.Status,
                                      Latitude = zm.Latitude,
                                      Longitude = zm.Longitude,
                                      Zone_Radius = (double)zm.Zone_Radius,
                                      //NoOfHotels = (context.ZoneProduct_Mapping.Where(x => x.Zone_Id == zm.Zone_id && (x.Included ?? false) == true).Count()),
                                      Zone_SubType = zm.Zone_SubType,
                                      TotalRecords = total
                                  })).OrderBy(x => x.Zone_Name).Skip(skip).Take((param.PageSize ?? total)).ToList();


                    foreach (var item in search)
                    {
                        item.NoOfHotels = (context.ZoneProduct_Mapping.Where(x => x.Zone_Id == item.Zone_id && (x.Included ?? false)).Count());
                    }

                    return search;

                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching Zone Master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public List<DC_ZoneCitiesSearch> SearchZoneCities(DataContracts.Masters.DC_ZoneRQ param)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = context.ZoneCity_Mapping
                        .Join(context.m_CityMaster, zcm => zcm.City_Id, cm => cm.City_Id, (zcm, cm) => new { zcm, cm })
                       //.Join(context.m_ZoneMaster, zcmZ => zcmZ.zcm.Zone_Id, zm => zm.Zone_id, (zcmZ, zm) => new { zcmZ, zm })
                       .Where(p => p.zcm.Zone_Id == param.Zone_id)
                        .Select(m => new DC_ZoneCitiesSearch
                        {
                            CityName = m.cm.Name,
                            Zone_id = m.zcm.Zone_Id,
                            City_id = m.zcm.City_Id,
                            IsActive = m.zcm.IsActive,
                            Status = m.zcm.Status,
                            ZoneCityMapping_Id = m.zcm.ZoneCityMapping_Id
                        }).OrderBy(x => x.CityName).ToList();

                    int total = search.Count();
                    param.PageNo = 0;
                    param.PageSize = total;
                    int skip = (param.PageNo ?? 0) * (param.PageSize ?? 0);

                    var result = from a in search
                                 select new DC_ZoneCitiesSearch
                                 {
                                     CityName = a.CityName,
                                     Zone_id = a.Zone_id,
                                     City_id = a.City_id,
                                     IsActive = a.IsActive,
                                     Status = a.Status,
                                     ZoneCityMapping_Id = a.ZoneCityMapping_Id,
                                     TotalRecords = total
                                 };

                    return result.OrderBy(p => p.CityName).Skip(skip).Take((param.PageSize ?? total)).ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching Zone cities", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public DataContracts.DC_Message DeactivateOrActivateZones(DataContracts.Masters.DC_ZoneRQ param)
        {
            DC_Message _msg = new DC_Message();
            if (param.Zone_id == Guid.Empty)
            {
                _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Warning;
                _msg.StatusMessage = DataContracts.ReadOnlyMessage.strFailed;
                return _msg;
            }
            using (ConsumerEntities context = new ConsumerEntities())
            {
                try
                {
                    var search = (from a in context.m_ZoneMaster where a.Zone_id == param.Zone_id select a).SingleOrDefault();
                    var boolIsActive = (param.IsActive == true) ? 1 : 0;
                    var Newstatus = (boolIsActive == 1) ? "Active" : "Inactive";
                    if (search != null)
                    {
                        if (param.Action == "ZoneMaster")
                        {
                            if (search.IsActive != param.IsActive)
                            {
                                search.IsActive = param.IsActive;
                                search.Edit_Date = param.Edit_Date;
                                search.Edit_User = param.Edit_User;
                            }
                            var searchcities = (from b in context.ZoneCity_Mapping where b.Zone_Id == param.Zone_id select b).Count();
                            if (searchcities > 0)
                            {
                                int setstatus = 0;
                                string setNewStatus = "UPDATE ZoneCity_Mapping ";
                                setNewStatus = setNewStatus + " SET Edit_Date = GETDATE(),  IsActive = " + boolIsActive;
                                setNewStatus = setNewStatus + " ,  Edit_User = " + "'" + param.Edit_User + "'";
                                setNewStatus = setNewStatus + " WHERE  Zone_Id = " + "'" + param.Zone_id + "'";
                                try { setstatus = context.Database.ExecuteSqlCommand(setNewStatus); } catch (Exception ex) { }
                            }
                            if (context.SaveChanges() > 0)
                            {
                                _msg.StatusMessage = ReadOnlyMessage.strUpdatedSuccessfully;
                                _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                            }
                            else
                            {
                                _msg.StatusMessage = ReadOnlyMessage.strFailed;
                                _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                            }

                        }
                        else
                        {
                            if (param.ZoneCityMapping_Id != Guid.Empty)
                            {
                                int setCount = 0;
                                string setupdateCity = "UPDATE ZoneCity_Mapping ";
                                setupdateCity = setupdateCity + " SET Edit_Date = GETDATE(),  IsActive = " + boolIsActive;
                                setupdateCity = setupdateCity + " ,  Edit_User = " + "'" + param.Edit_User + "'";
                                setupdateCity = setupdateCity + " ,  Status = " + "'" + Newstatus + "'";
                                setupdateCity = setupdateCity + " WHERE  ZoneCityMapping_Id = " + "'" + param.ZoneCityMapping_Id + "'";
                                try { setCount = context.Database.ExecuteSqlCommand(setupdateCity); } catch (Exception ex) { }
                                if (setCount > 0)
                                {
                                    _msg.StatusMessage = ReadOnlyMessage.strDeleted;
                                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                                }
                                else
                                {
                                    _msg.StatusMessage = ReadOnlyMessage.strFailed;
                                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                                }
                            }
                            else
                            {
                                _msg.StatusMessage = ReadOnlyMessage.strFailed;
                                _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                            }
                        }
                    }

                    else
                    {
                        _msg.StatusMessage = ReadOnlyMessage.strFailed;
                        _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                    }
                }
                catch (Exception e)
                {
                    throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while deactivating zone master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
                }
            }
            return _msg;
        }


        public List<DC_ZoneHotelList> SearchZoneHotels(DataContracts.Masters.DC_ZoneRQ param)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    context.Database.CommandTimeout = 0;

                    var Search = (from s in context.ZoneProduct_Mapping
                                  join a in context.Accommodation on s.Product_Id equals a.Accommodation_Id
                                  where s.Zone_Id == param.Zone_id
                                  select new DC_ZoneHotelList
                                  {
                                      Accommodation_Id = s.Product_Id ?? Guid.Empty,
                                      ZoneProductMapping_Id = s.ZoneProductMapping_Id,
                                      HotelName = a.HotelName,
                                      City = a.city,
                                      Distance = (double)(s.Distance),
                                      StarRating = a.CompanyRating,
                                      Latitude = a.Latitude,
                                      Longitude = a.Longitude,
                                      Included = s.Included
                                  });
                    return Search.OrderBy(p => p.Distance).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching Hotels within Zone", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public DC_Message InsertZoneHotelsInTable(DataContracts.Masters.DC_ZoneRQ param)
        {
            DC_Message _msg = new DC_Message();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    context.Database.CommandTimeout = 0;
                    var search = (from s in context.ZoneProduct_Mapping where s.Zone_Id == param.Zone_id select s).FirstOrDefault();
                    if (search != null)
                    {
                        _msg.StatusMessage = ReadOnlyMessage.strAlreadyExist;
                        _msg.StatusCode = ReadOnlyMessage.StatusCode.Duplicate;

                    }
                    else
                    {
                        int srid = 4326; //spatial reference ID
                        int DistanceUpto = 10000;
                        StringBuilder SearchZoneHotelQuery = new StringBuilder();
                        SearchZoneHotelQuery.Append(" DECLARE @FromPoint geography ");
                        SearchZoneHotelQuery.AppendLine(" SELECT @FromPoint = geography::Point('" + param.Latitude + "'" + ",'" + param.Longitude + "'," + srid + ") ");
                        SearchZoneHotelQuery.AppendLine(" Select   (@FromPoint.STDistance(geography::Point(Latitude, Longitude, " + srid + "))/1000) as Distance, Accommodation_Id,ProductCategorySubType");
                        SearchZoneHotelQuery.AppendLine(" from Accommodation where country = " + "'" + param.CountryName + "'");
                        // SearchZoneHotelQuery.AppendLine(" and Latitude IS NOT NULL and Longitude IS NOT NULL ");
                        SearchZoneHotelQuery.AppendLine(" and @FromPoint.STDistance(geography::Point(Latitude, Longitude, " + srid + ")) <= " + DistanceUpto);
                        SearchZoneHotelQuery.AppendLine("  AND TRY_CONVERT(float, Latitude) IS NOT NULL AND TRY_CONVERT(float, Longitude) IS NOT NULL ");
                        SearchZoneHotelQuery.AppendLine(" and (TRY_CONVERT(float, Latitude) >= -90 AND TRY_CONVERT(float, Latitude) <= 90) ");
                        SearchZoneHotelQuery.AppendLine(" and (TRY_CONVERT(float, Longitude) >= -15069 AND TRY_CONVERT(float, Longitude) <= 15069) ");



                        var res = context.Database.SqlQuery<DC_ZoneHotelList>(SearchZoneHotelQuery.ToString()).ToList();
                        //insert data in table
                        foreach (var item in res)
                        {
                            ZoneProduct_Mapping zpm = new ZoneProduct_Mapping();
                            zpm.Zone_Id = param.Zone_id;
                            zpm.Product_Id = item.Accommodation_Id;
                            zpm.ZoneProductMapping_Id = Guid.NewGuid();
                            zpm.ProductType = item.ProductCategorySubType;
                            zpm.Distance = Convert.ToDecimal(item.Distance);
                            zpm.IsActive = true;
                            zpm.Included = (item.Distance <= param.Zone_Radius) ? true : false;
                            zpm.Create_Date = DateTime.Now;
                            zpm.Create_User = param.Create_User;
                            context.ZoneProduct_Mapping.Add(zpm);
                        }
                        if (context.SaveChanges() > 0)
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strAddedSuccessfully;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        }
                        else
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strFailed;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while inserting Hotels within Zone", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
            return _msg;
        }

        public DC_Message DeleteZoneHotelsInTable(DataContracts.Masters.DC_ZoneRQ param)
        {
            DC_Message _msg = new DC_Message();
            if (param.Zone_id == Guid.Empty)
            {
                _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Warning;
                _msg.StatusMessage = DataContracts.ReadOnlyMessage.strFailed;
                return _msg;
            }
            using (ConsumerEntities context = new ConsumerEntities())
            {
                try
                {
                    var search = (from a in context.ZoneProduct_Mapping where a.Zone_Id == param.Zone_id select a);
                    if (search != null && search.Count() > 0)
                    {
                        context.ZoneProduct_Mapping.RemoveRange(search);
                    }
                    if (context.SaveChanges() > 0)
                    {
                        _msg.StatusMessage = ReadOnlyMessage.strDeleted;
                        _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                    }
                    else
                    {
                        _msg.StatusMessage = ReadOnlyMessage.strFailed;
                        _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                    }
                }
                catch (Exception e)
                {
                    throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while deleting zone master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
                }
            }
            return _msg;
        }

        public DC_Message UpdateZoneHotelsInTable(DataContracts.Masters.DC_ZoneRQ param)
        {
            DC_Message _msg = new DC_Message();
            if (param.Zone_id == Guid.Empty)
            {
                _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Warning;
                _msg.StatusMessage = DataContracts.ReadOnlyMessage.strFailed;
                return _msg;
            }
            using (ConsumerEntities context = new ConsumerEntities())
            {
                try
                {
                    var search = (from s in context.ZoneProduct_Mapping where s.Zone_Id == param.Zone_id select s).FirstOrDefault();
                    if (search != null)
                    {
                        string UpdateIsIncluded = " UPDATE ZoneProduct_Mapping SET Included = ( case when Distance <=  " + param.Zone_Radius + "  then '1' else '0' end ) WHERE zone_id ='" + param.Zone_id + "'";
                        int count = context.Database.ExecuteSqlCommand(UpdateIsIncluded);
                        if (count > 0)
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strUpdatedSuccessfully;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        }
                        else
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strFailed;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                        }
                    }
                    else
                    {
                        _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Failed;
                        _msg.StatusMessage = DataContracts.ReadOnlyMessage.strFailed;
                    }
                }
                catch (Exception e)
                {
                    throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while Updating zone master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
                }
            }
            return _msg;
        }
        public DC_Message IncludeExcludeHotels(DataContracts.Masters.DC_ZoneRQ param)
        {
            DC_Message _msg = new DC_Message();
            if (param.ZoneProductMapping_Id == null || param.ZoneProductMapping_Id == Guid.Empty)
            {
                _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Warning;
                _msg.StatusMessage = DataContracts.ReadOnlyMessage.strFailed;
                return _msg;
            }
            using (ConsumerEntities context = new ConsumerEntities())
            {
                try
                {
                    var search = (from a in context.ZoneProduct_Mapping where param.ZoneProductMapping_Id == a.ZoneProductMapping_Id select a).First();
                    if (search != null)
                    {
                        search.Included = param.Included;
                        search.Edit_Date = DateTime.Now;
                        search.Edit_User = param.Edit_User;
                        context.SaveChanges();
                        _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Success;
                        _msg.StatusMessage = DataContracts.ReadOnlyMessage.strUpdatedSuccessfully;
                    }
                    else
                    {
                        _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Failed;
                        _msg.StatusMessage = DataContracts.ReadOnlyMessage.strFailed;
                    }
                }
                catch (Exception e)
                {
                    throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while includeing/ExcludingZone", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
                }
            }
            return _msg;
        }
        #endregion


        #region MultiSelectDropdown

        public List<DataContracts.DC_Master_Country> GetRegionwiseCountriesList(List<string> RegionCodeList)
        {
            try
            {
                List<DataContracts.DC_Master_Country> countryList = new List<DC_Master_Country>();
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var country = from c in context.m_CountryMaster
                                  orderby c.Name
                                  where c.Status == "ACTIVE" && RegionCodeList.Distinct().Contains(c.RegionCode)
                                  select new DataContracts.DC_Master_Country { Country_Id = c.Country_Id, Country_Name = c.Name, Country_Code = c.Code, RegionCode = c.RegionCode };

                    return country.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DataContracts.DC_Master_City> GetCountrywiseCitiesList(List<Guid> CountryIdList)
        {
            try
            {
                List<DataContracts.DC_Master_City> countryList = new List<DC_Master_City>();
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var City = from c in context.m_CityMaster
                               orderby c.Name
                               where c.Status == "ACTIVE" && CountryIdList.Distinct().Contains(c.Country_Id)
                               select new DataContracts.DC_Master_City { City_Id = c.City_Id, City_Name = c.CountryName + "->" + c.Name, City_Code = c.Code };

                    return City.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
