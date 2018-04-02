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
using DataContracts.Masters;

namespace ConsumerSvc
{
    public partial class Consumer : IConsumer
    {
        #region Country Master
        public IList<DC_Master_Country> GetAllCountries()
        {
            using (BL_Master_Country objBL = new BL_Master_Country())
            {
                List<DC_Master_Country> searchResults = new List<DC_Master_Country>();
                searchResults = objBL.GetCountryList();

                if (searchResults.Count == 0)
                {
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                }

                return searchResults;
            }
        }


        public IList<string> GetCountryNameList(DataContracts.Masters.DC_Country_Search_RQ RQ)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetCountryNameList(RQ);
            }
        }

        public IList<DataContracts.Masters.DC_Country> GetCountryMaster(DataContracts.Masters.DC_Country_Search_RQ RQ)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetCountryMaster(RQ);
            }
        }
        public DC_Message AddUpdateCountryMaster(DataContracts.Masters.DC_Country param)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.AddUpdateCountryMaster(param);
            }
        }
        public bool AddCountryMaster(DataContracts.Masters.DC_Country param)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.AddCountryMaster(param);
            }
        }

        public bool UpdateCountryMaster(DataContracts.Masters.DC_Country param)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.UpdateCountryMaster(param);
            }
        }
        #endregion

        #region City Master
        public IList<DC_Master_City> GetAllCities()
        {
            using (BL_Master_City objBL = new BL_Master_City())
            {
                List<DC_Master_City> searchResults = new List<DC_Master_City>();
                searchResults = objBL.GetCityList();

                if (searchResults.Count == 0)
                {
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                }

                return searchResults;
            }

        }
        public IList<string> GetCityNameList(DataContracts.Masters.DC_City_Search_RQ RQ)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetCityNameList(RQ);
            }
        }

        public IList<DC_Master_City> GetCitiesByCountry(string Country_Id)
        {
            using (BL_Master_City objBL = new BL_Master_City())
            {
                List<DC_Master_City> searchResults = new List<DC_Master_City>();
                searchResults = objBL.GetCityList(Guid.Parse(Country_Id));

                if (searchResults.Count == 0)
                {
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                }

                return searchResults;
            }
        }

        public IList<DataContracts.Masters.DC_City> GetCityMaster(DataContracts.Masters.DC_City_Search_RQ RQ)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetCityMaster(RQ);
            }
        }

        public IList<DataContracts.Masters.City_AlphaPage> GetCityAlphaPaging(DataContracts.Masters.DC_City_Search_RQ RQ)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetCityAlphaPaging(RQ);
            }
        }

        public DataContracts.DC_Message AddCityMaster(DataContracts.Masters.DC_City param)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.AddCityMaster(param);
            }
        }

        public DataContracts.DC_Message UpdateCityMaster(DataContracts.Masters.DC_City param)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.UpdateCityMaster(param);
            }
        }

        public DC_State_Master_DDL GetStateNameAndCode(DC_State_Master_DDL_RQ _obj)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetStateNameAndCode(_obj);
            }
        }

        #endregion

        #region State Master
        public IList<DC_Master_State> GetAllStates()
        {
            using (BL_Masters objBL = new BL_Masters())
            {
                List<DC_Master_State> searchResults = new List<DC_Master_State>();
                searchResults = objBL.GetAllStates();

                if (searchResults.Count == 0)
                {
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                }

                return searchResults;
            }

        }

        public IList<DC_Master_State> GetStatesByCountry(string Country_Id)
        {
            using (BL_Masters objBL = new BL_Masters())
            {
                List<DC_Master_State> searchResults = new List<DC_Master_State>();
                searchResults = objBL.GetStatesByCountry(Guid.Parse(Country_Id));

                if (searchResults.Count == 0)
                {
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                }

                return searchResults;
            }
        }

        public IList<DC_Master_State> GetStatesMaster(DC_State_Search_RQ RQ)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetStatesMaster(RQ);
            }
        }

        public IList<State_AlphaPage> GetStatesAlphaPaging(DC_State_Search_RQ RQ)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetStatesAlphaPaging(RQ);
            }
        }

        public DataContracts.DC_Message AddStatesMaster(DC_Master_State param)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.AddStatesMaster(param);
            }
        }

        public DataContracts.DC_Message UpdateStatesMaster(DC_Master_State param)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.UpdateStatesMaster(param);
            }
        }

        #endregion

        #region DynamicAttribute
        public IList<DataContracts.Masters.DC_DynamicAttributes> GetDynamicAttributes(DataContracts.Masters.DC_DynamicAttributes RQ)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                List<DataContracts.Masters.DC_DynamicAttributes> searchResults = new List<DataContracts.Masters.DC_DynamicAttributes>();
                searchResults = obj.GetDynamicAttributes(RQ);

                if (searchResults.Count == 0)
                {
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                }

                return searchResults;
            }
        }

        public bool UpdateDynamicAttributes(DataContracts.Masters.DC_DynamicAttributes RQ)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.UpdateDynamicAttributes(RQ);
            }
        }

        public bool AddDynamicAttributes(DataContracts.Masters.DC_DynamicAttributes RQ)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.AddDynamicAttributes(RQ);
            }
        }
        #endregion

        #region Team Master Data
        public IList<DC_Teams> GetTeamMasterData(string Team_Id)
        {
            using (BL_Masters objBL = new BL_Masters())
            {
                List<DC_Teams> searchResults = new List<DC_Teams>();
                searchResults = objBL.GetTeamMasterData(Guid.Parse(Team_Id));

                if (searchResults.Count == 0)
                {
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                }

                return searchResults;
            }
        }

        public bool UpdateTeamMasterData(DataContracts.DC_Teams RQ)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.UpdateTeamMasterData(RQ);
            }
        }

        public bool AddTeamMasterData(DataContracts.DC_Teams RQ)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.AddTeamMasterData(RQ);
            }
        }
        #endregion

        #region Update Country State City Area Location Hierarchy
        public bool UpdateAddressMasterHierarchy(DataContracts.DC_Address.DC_Country_State_City_Area_Location RQ)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.UpdateAddressMasterHierarchy(RQ);
            }
        }
        #endregion

        #region Port Master
        public IList<DataContracts.Masters.DC_PortMaster> GetPort(string PageSize, string PageNo, string Port_Id)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetPort(PageSize, PageNo, Port_Id);
            }
        }

        public IList<DataContracts.Masters.DC_PortMaster> PortMasterSeach(DataContracts.Masters.DC_PortMaster_RQ RQ)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.PortMasterSeach(RQ);
            }
        }


        public DC_Message UpdatePortMaster(DataContracts.Masters.DC_PortMaster param)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.UpdatePortMaster(param);
            }
        }
        public DC_Message AddPortMaster(DataContracts.Masters.DC_PortMaster param)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.AddPortMaster(param);
            }
        }
        #endregion

        #region Master Attribute
        public IList<DataContracts.Masters.DC_MasterAttribute> GetAllAttributeAndValuesByFOR(DataContracts.Masters.DC_MasterAttribute _obj)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetAllAttributeAndValuesByFOR(_obj);
            }
        }
        public IList<DataContracts.Masters.DC_MasterAttribute> GetAllAttributeAndValuesByParentAttributeValue(DataContracts.Masters.DC_MasterAttribute _obj)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetAllAttributeAndValuesByParentAttributeValue(_obj);
            }
        }
        
        public IList<DataContracts.Masters.DC_MasterAttribute> GetAllAttributeAndValues(DataContracts.Masters.DC_MasterAttribute _obj)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetAllAttributeAndValues(_obj);
            }
        }



        public IList<DC_M_masterattribute> GetMasterAttributes(DC_M_masterattribute _obj)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetMasterAttributes(_obj);
            }
        }
        public IList<DC_M_masterparentattributes> GetParentAttributes(string sMasterFor)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetParentAttributes(sMasterFor);
            }
        }
        public IList<DC_M_masterparentattributes> GetAttributesForValues(string MasterAttribute_id)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetAttributesForValues(Guid.Parse(MasterAttribute_id));
            }
        }
        public IList<DC_M_masterparentattributes> GetAttributesMasterForValues()
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetAttributesMasterForValues();
            }
        }
        public IList<DC_M_masterattributevalue> GetAttributeValues(string MasterAttribute_Id, string PageSize, string PageNo)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetAttributeValues(MasterAttribute_Id, Convert.ToInt32(PageSize), Convert.ToInt32(PageNo));
            }
        }
        public DC_M_masterattribute GetAttributeDetails(string MasterAttribute_Id)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetAttributeDetails(Guid.Parse(MasterAttribute_Id));
            }
        }
        public DC_M_masterattributelists GetListAttributeAndValuesByFOR(DC_MasterAttribute _obj)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetListAttributeAndValuesByFOR(_obj);
            }
        }

        public DC_Message SaveMasterAttribute(DC_M_masterattribute _obj)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.SaveMasterAttribute(_obj);
            }
        }
        public DC_Message SaveAttributeValue(DC_M_masterattributevalue _obj)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.SaveAttributeValue(_obj);
            }
        }
        #endregion

        #region Supplier
        public IList<DataContracts.Masters.DC_Supplier> GetSupplier(DataContracts.Masters.DC_Supplier_Search_RQ _obj)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetSupplier(_obj);
            }
        }
        public IList<DataContracts.Masters.DC_Supplier_DDL> GetSupplierByEntity(DataContracts.Masters.DC_Supplier_Search_RQ _obj)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetSupplierByEntity(_obj);
            }
        }
        public IList<DataContracts.Masters.DC_SupplierMarket> GetSupplierMarket(DataContracts.Masters.DC_SupplierMarket _obj)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetSupplierMarket(_obj);
            }
        }

        public IList<DataContracts.Masters.DC_Supplier_ProductCategory> GetProductCategoryBySupplier(DataContracts.Masters.DC_Supplier_ProductCategory _obj)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetProductCategoryBySupplier(_obj);
            }
        }
        public DataContracts.DC_Message AddUpdateSupplier_ProductCategory(DataContracts.Masters.DC_Supplier_ProductCategory _objSup)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.AddUpdateSupplier_ProductCategory(_objSup);
            }
        }
        public DataContracts.DC_Message AddUpdateSupplierMarket(DataContracts.Masters.DC_SupplierMarket _objSupMkt)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.AddUpdateSupplierMarket(_objSupMkt);
            }
        }

        public DataContracts.DC_Message AddUpdateSupplier(DataContracts.Masters.DC_Supplier _objSup)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.AddUpdateSupplier(_objSup);
            }
        }

        public DataContracts.DC_Message SupplierMarketSoftDelete(DataContracts.Masters.DC_SupplierMarket _objSup)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.SupplierMarketSoftDelete(_objSup);
            }
        }

        public DataContracts.DC_Message Supplier_ProductCategorySoftDelete(DataContracts.Masters.DC_Supplier_ProductCategory _objSup)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.Supplier_ProductCategorySoftDelete(_objSup);
            }
        }

        public List<DataContracts.Masters.DC_Supplier_ApiLocation> SupplierApiLocation_Search(DataContracts.Masters.DC_Supplier_ApiLocation RQ)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.SupplierApiLocation_Search(RQ);
            }
        }

        public DataContracts.DC_Message SupplierApiLocation_Update(DataContracts.Masters.DC_Supplier_ApiLocation objApiLoc)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.SupplierApiLocation_Update(objApiLoc);
            }
        }

        public DataContracts.DC_Message SupplierApiLocation_Add(DataContracts.Masters.DC_Supplier_ApiLocation objApiLoc)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.SupplierApiLocation_Add(objApiLoc);
            }
        }
        #endregion

        #region Statuses
        public IList<DC_Statuses> GetAllStatuses()
        {
            using (BL_Masters objBL = new BL_Masters())
            {
                List<DC_Statuses> searchResults = new List<DC_Statuses>();
                searchResults = objBL.GetAllStatuses();
                return searchResults;
            }

        }

        #endregion

        #region "Activity Master"
        public IList<DataContracts.Masters.DC_Activity> GetActivityMaster(DataContracts.Masters.DC_Activity_Search_RQ RQ)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetActivityMaster(RQ);
            }
        }
        public IList<DataContracts.Masters.DC_Activity> GetActivityMasterBySupplier(DataContracts.Masters.DC_Activity_Search_RQ RQ)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetActivityMasterBySupplier(RQ);
            }
        }
        public IList<DataContracts.Masters.DC_Activity_Content> GetActivityContentMaster(DataContracts.Masters.DC_Activity_Content RQ)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetActivityContentMaster(RQ);
            }
        }
        public IList<string> GetActivityNames(DataContracts.Masters.DC_Activity_Search_RQ RQ)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetActivityNames(RQ);
            }
        }

        #endregion

        #region Common Funciton to Get Codes by Entity Type
        public string GetCodeById(string objName, string obj_Id)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetCodeById(objName, Guid.Parse(obj_Id));
            }
        }
        public string GetRemarksForMapping(string from, string Mapping_Id)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetRemarksForMapping(from, Guid.Parse(Mapping_Id));
            }
        }
        public DC_GenericMasterDetails_ByIDOrName GetDetailsByIdOrName(DC_GenericMasterDetails_ByIDOrName _obj)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetDetailsByIdOrName(_obj);
            }
        }


        #endregion

        #region To Fill DropDown
        public List<DC_Accomodation_DDL> GetProductByCity(DC_Accomodation_DDL _obj)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetProductByCity(_obj);
            }
        }
        public List<DC_State_Master_DDL> GetMasterStateData(string country_id)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetMasterStateData(Guid.Parse(country_id));
            }
        }
        public List<DC_City_Master_DDL> GetMasterCityData(string country_id)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetMasterCityData(Guid.Parse(country_id));
            }
        }
        public List<DC_CityAreaLocation> GetMasterCityAreaLocationDetail(string CityAreaLocation_Id)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetMasterCityAreaLocationDetail(Guid.Parse(CityAreaLocation_Id));
            }
        }
        public List<DC_CityArea> GetMasterCityAreaDetail(string CityArea_Id)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetMasterCityAreaDetail(Guid.Parse(CityArea_Id));
            }
        }
        public DC_Supplier_DDL GetSupplierDataByMapping_Id(string objName, string Mapping_Id)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetSupplierDataByMapping_Id(objName, Guid.Parse(Mapping_Id));
            }
        }
        public List<DC_Supplier_DDL> GetSupplierMasterData()
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetSupplierMasterData();
            }
        }
        public List<DC_State_Master_DDL> GetStateByCity(string City_Id)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetStateByCity(Guid.Parse(City_Id));
            }
        }
        public List<DC_CountryMaster> GetCountryCodes(string obj_Id)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetCountryCodes(Guid.Parse(obj_Id));
            }
        }
        public List<DC_CountryMaster> GetMasterCountryDataList()
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetMasterCountryDataList();
            }
        }
        public List<DC_Activity_DDL> GetActivityByCountryCity(string CountryName, string CityName)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetActivityByCountryCity(CountryName,CityName);
            }
        }
        public List<DC_Supplier_DDL> GetSuppliersByProductCategory(string ProductCategory)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetSuppliersByProductCategory(ProductCategory);
            }
        }
        #endregion

        #region City Area and Location
        public bool SaveCityAreaLocation(DC_CityAreaLocation _obj)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.SaveCityAreaLocation(_obj);
            }

        }
        public List<DC_CityAreaLocation> GetMasterCityAreaLocationData(string CityArea_Id)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetMasterCityAreaLocationData(Guid.Parse(CityArea_Id));
            }
        }
        public bool SaveCityArea(DC_CityArea _obj)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.SaveCityArea(_obj);
            }

        }

        public List<DC_CityArea> GetMasterCityAreaData(string City_Id)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetMasterCityAreaData(Guid.Parse(City_Id));
            }
        }
        #endregion

        #region Keyword
        public DC_Message AddUpdateKeyword(DC_Keyword _obj)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.AddUpdateKeyword(_obj);
            }
        }

        public List<DC_Keyword> SearchKeyword(DC_Keyword_RQ RQ)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.SearchKeyword(RQ);
            }
        }

        public List<DC_keyword_alias> SearchKeywordAlias(DC_Keyword_RQ RQ)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.SearchKeywordAlias(RQ);
            }
        }

        public DC_Message ApplyKeyword(DC_keywordApply_RQ RQ)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.ApplyKeyword(RQ);
            }
        }
        #endregion

        //public string[] GetColumnNames(string TableName)
        //{
        //    using (BusinessLayer.BL_Masters obj = new BL_Masters())
        //    {
        //        return obj.GetColumnNames(TableName);
        //    }
        //}

        public List<string> GetListOfColumnNamesByTable(string TableName)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetListOfColumnNamesByTable(TableName);
            }
        }

        #region Zone Master
        //Add
        public DataContracts.DC_Message AddzoneMaster(DataContracts.Masters.DC_ZoneRQ param)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.AddzoneMaster(param);
            }
        }
        public DataContracts.DC_Message AddZoneCityMapping(DataContracts.Masters.DC_ZoneRQ param)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.AddZoneCityMapping(param);
            }
        }

        public IList<DataContracts.Masters.DC_ZoneSearch>SearchZone(DataContracts.Masters.DC_ZoneRQ param)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.SearchZone(param);
            }
        }
        public IList<DC_ZoneCitiesSearch>SearchZoneCities(DataContracts.Masters.DC_ZoneRQ param)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.SearchZoneCities(param);
            }
        }
      
        public DataContracts.DC_Message DeactivateOrActivateZones(DataContracts.Masters.DC_ZoneRQ param)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.DeactivateOrActivateZones(param);
            }
        }

        public IList<DC_ZoneHotelList> SearchZoneHotels(DataContracts.Masters.DC_ZoneRQ param)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.SearchZoneHotels(param);
            }
        }
        public DataContracts.DC_Message InsertZoneHotelsInTable(DataContracts.Masters.DC_ZoneRQ param)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.InsertZoneHotelsInTable(param);
            }
        }
        public DataContracts.DC_Message DeleteZoneHotelsInTable(DataContracts.Masters.DC_ZoneRQ param)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.DeleteZoneHotelsInTable(param);
            }
        }
        public DC_Message UpdateZoneHotelsInTable(DataContracts.Masters.DC_ZoneRQ param)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.UpdateZoneHotelsInTable(param);
            }
        }
        public DC_Message IncludeExcludeHotels(DataContracts.Masters.DC_ZoneRQ param)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.IncludeExcludeHotels(param);
            }
        }
        #endregion
    }
}