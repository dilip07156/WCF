using DataContracts;
using DataContracts.Masters;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class BL_Masters : IDisposable
    {
        public void Dispose()
        {

        }

        #region CountryMaster

        public List<string> GetCountryNameList(DataContracts.Masters.DC_Country_Search_RQ RQ)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetCountryNameList(RQ);
            }
        }

        public List<DataContracts.Masters.DC_Country> GetCountryMaster(DataContracts.Masters.DC_Country_Search_RQ RQ)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetCountryMaster(RQ);
            }
        }

        public DC_Message AddUpdateCountryMaster(DataContracts.Masters.DC_Country param)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.AddUpdateCountryMaster(param);
            }
        }
        public bool AddCountryMaster(DataContracts.Masters.DC_Country param)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.AddCountryMaster(param);
            }
        }

        public bool UpdateCountryMaster(DataContracts.Masters.DC_Country param)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.UpdateCountryMaster(param);
            }
        }
        #endregion

        #region CityMaster
        public List<string> GetCityNameList(DataContracts.Masters.DC_City_Search_RQ RQ)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetCityNameList(RQ);
            }
        }
        public List<DataContracts.Masters.DC_City> GetCityMaster(DataContracts.Masters.DC_City_Search_RQ RQ)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetCityMaster(RQ);
            }
        }

        public List<DataContracts.Masters.City_AlphaPage> GetCityAlphaPaging(DataContracts.Masters.DC_City_Search_RQ RQ)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetCityAlphaPaging(RQ);
            }
        }

        public DataContracts.DC_Message AddCityMaster(DataContracts.Masters.DC_City param)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.AddCityMaster(param);
            }
        }

        public DataContracts.DC_Message UpdateCityMaster(DataContracts.Masters.DC_City param)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.UpdateCityMaster(param);
            }
        }
        public DC_State_Master_DDL GetStateNameAndCode(DC_State_Master_DDL_RQ _obj)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetStateNameAndCode(_obj);
            }

        }
        public List<DataContracts.Masters.DC_City> GetCountryCityMaster(DataContracts.Masters.DC_City_Search_RQ RQ)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetCountryCityMaster(RQ);
            }
        }
        #endregion

        #region State Master

        public List<DataContracts.Masters.DC_Master_State> GetAllStates()
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetAllStates();
            }
        }

        public List<DataContracts.Masters.DC_Master_State> GetStatesByCountry(string Country_Id)
        {
            Guid gCountry_Id;
            if (Guid.TryParse(Country_Id, out gCountry_Id))
            {
                using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
                {
                    return obj.GetStatesByCountry(gCountry_Id);
                }
            }
            else
            {
                return new List<DC_Master_State>();
            }
        }

        public List<DataContracts.Masters.DC_Master_State> GetStatesMaster(DataContracts.Masters.DC_State_Search_RQ RQ)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetStatesMaster(RQ);
            }
        }

        public List<DataContracts.Masters.State_AlphaPage> GetStatesAlphaPaging(DataContracts.Masters.DC_State_Search_RQ RQ)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetStatesAlphaPaging(RQ);
            }
        }

        public DataContracts.DC_Message AddStatesMaster(DataContracts.Masters.DC_Master_State param)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.AddStatesMaster(param);
            }
        }

        public DataContracts.DC_Message UpdateStatesMaster(DataContracts.Masters.DC_Master_State param)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.UpdateStatesMaster(param);
            }
        }

        #endregion

        #region DynamicAttributes
        public List<DataContracts.Masters.DC_DynamicAttributes> GetDynamicAttributes(DataContracts.Masters.DC_DynamicAttributes RQ)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetDynamicAttributes(RQ);
            }
        }

        public bool UpdateDynamicAttributes(DataContracts.Masters.DC_DynamicAttributes RQ)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.UpdateDynamicAttributes(RQ);
            }
        }

        public bool AddDynamicAttributes(DataContracts.Masters.DC_DynamicAttributes RQ)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.AddDynamicAttributes(RQ);
            }
        }
        #endregion

        #region Team Masters       
        public List<DataContracts.DC_Teams> GetTeamMasterData(Guid Team_Id)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetTeamMasterData(Team_Id);
            }
        }

        public bool UpdateTeamMasterData(DataContracts.DC_Teams RQ)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.UpdateTeamMasterData(RQ);
            }
        }

        public bool AddTeamMasterData(DataContracts.DC_Teams RQ)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.AddTeamMasterData(RQ);
            }
        }
        #endregion

        #region Update Country State City Area Location Hierarchy
        public bool UpdateAddressMasterHierarchy(DataContracts.DC_Address.DC_Country_State_City_Area_Location RQ)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.UpdateAddressMasterHierarchy(RQ);
            }
        }
        #endregion

        #region Port Master
        public List<DataContracts.Masters.DC_PortMaster> GetPort(string PageSize, string PageNo, string Port_Id)
        {
            Guid gPort_Id;
            int iPageSize;
            int iPageNo;

            if (!Guid.TryParse(Port_Id, out gPort_Id) || !int.TryParse(PageSize, out iPageSize) || !int.TryParse(PageNo, out iPageNo))
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
            }
            else
            {
                using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
                {
                    return obj.GetPort(iPageSize, iPageNo, gPort_Id);
                }
            }
        }

        public List<DataContracts.Masters.DC_PortMaster> PortMasterSeach(DataContracts.Masters.DC_PortMaster_RQ RQ)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.PortMasterSeach(RQ);
            }
        }


        public DC_Message UpdatePortMaster(DataContracts.Masters.DC_PortMaster param)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.UpdatePortMaster(param);
            }
        }

        public DC_Message AddPortMaster(DataContracts.Masters.DC_PortMaster param)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.AddPortMaster(param);
            }
        }
        #endregion

        #region Master Attribute
        public IList<DataContracts.Masters.DC_MasterAttribute> GetAllAttributeAndValuesByFOR(DataContracts.Masters.DC_MasterAttribute _obj)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetAllAttributeAndValuesByFOR(_obj);
            }
        }
        public IList<DataContracts.Masters.DC_MasterAttribute> GetAllAttributeAndValuesByParentAttributeValue(DataContracts.Masters.DC_MasterAttribute _obj)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetAllAttributeAndValuesByParentAttributeValue(_obj);
            }
        }

        public IList<DataContracts.Masters.DC_MasterAttribute> GetAllAttributeAndValues(DataContracts.Masters.DC_MasterAttribute _obj)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetAllAttributeAndValues(_obj);
            }
        }
        public IList<DC_M_masterattribute> GetMasterAttributes(DC_M_masterattribute _obj)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetMasterAttributes(_obj);
            }
        }
        public IList<DC_M_masterparentattributes> GetParentAttributes(string sMasterFor)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetParentAttributes(sMasterFor);
            }
        }
        public IList<DC_M_masterparentattributes> GetAttributesForValues(Guid MasterAttribute_id)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetAttributesForValues(MasterAttribute_id);
            }
        }
        public IList<DC_M_masterparentattributes> GetAttributesMasterForValues()
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetAttributesMasterForValues();
            }
        }
        public IList<DC_M_masterattributevalue> GetAttributeValues(string MasterAttribute_Id, int PageSize, int PageNo)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetAttributeValues(MasterAttribute_Id, PageSize, PageNo);
            }
        }

        public IList<DC_M_masterattributevalue> GetDisplaySubType(string MasterFor, string Name)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetDisplaySubType(MasterFor, Name);
            }
        }

        public DC_M_masterattribute GetAttributeDetails(Guid MasterAttribute_Id)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetAttributeDetails(MasterAttribute_Id);
            }
        }
        public DC_Message SaveMasterAttribute(DC_M_masterattribute _obj)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.SaveMasterAttribute(_obj);
            }
        }
        public DC_Message SaveAttributeValue(DC_M_masterattributevalue _obj)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.SaveAttributeValue(_obj);
            }
        }
        public DC_M_masterattributelists GetListAttributeAndValuesByFOR(DC_MasterAttribute _obj)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetListAttributeAndValuesByFOR(_obj);
            }
        }

        #endregion

        #region Supplier
        public IList<DataContracts.Masters.DC_Supplier> GetSupplier(DataContracts.Masters.DC_Supplier_Search_RQ _obj)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetSupplier(_obj);
            }
        }
        public IList<DataContracts.Masters.DC_Supplier_DDL> GetSupplierByEntity(DataContracts.Masters.DC_Supplier_Search_RQ _obj)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetSupplierByEntity(_obj);
            }
        }
        public IList<DataContracts.Masters.DC_SupplierMarket> GetSupplierMarket(DataContracts.Masters.DC_SupplierMarket _obj)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetSupplierMarket(_obj);
            }
        }
        public IList<DataContracts.Masters.DC_Supplier_ProductCategory> GetProductCategoryBySupplier(DataContracts.Masters.DC_Supplier_ProductCategory _obj)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetProductCategoryBySupplier(_obj);
            }
        }

        public DataContracts.DC_Message AddUpdateSupplier(DataContracts.Masters.DC_Supplier _objSup)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.AddUpdateSupplier(_objSup);
            }
        }
        public DataContracts.DC_Message AddUpdateSupplierMarket(DataContracts.Masters.DC_SupplierMarket _objSupMkt)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.AddUpdateSupplierMarket(_objSupMkt);
            }
        }
        public DataContracts.DC_Message AddUpdateSupplier_ProductCategory(DataContracts.Masters.DC_Supplier_ProductCategory _objSupMkt)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.AddUpdateSupplier_ProductCategory(_objSupMkt);
            }
        }

        public DataContracts.DC_Message SupplierMarketSoftDelete(DataContracts.Masters.DC_SupplierMarket _objSup)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.SupplierMarketSoftDelete(_objSup);
            }
        }
        public DataContracts.DC_Message Supplier_ProductCategorySoftDelete(DataContracts.Masters.DC_Supplier_ProductCategory _objSup)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.Supplier_ProductCategorySoftDelete(_objSup);
            }
        }

        public List<DataContracts.Masters.DC_Supplier_ApiLocation> SupplierApiLocation_Search(DataContracts.Masters.DC_Supplier_ApiLocation RQ)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.SupplierApiLocation_Search(RQ);
            }
        }

        public DataContracts.DC_Message SupplierApiLocation_Update(DataContracts.Masters.DC_Supplier_ApiLocation objApiLoc)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.SupplierApiLocation_Update(objApiLoc);
            }
        }

        public DataContracts.DC_Message SupplierApiLocation_Add(DataContracts.Masters.DC_Supplier_ApiLocation objApiLoc)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.SupplierApiLocation_Add(objApiLoc);
            }
        }

        #endregion

        #region Statuses
        public List<DataContracts.Masters.DC_Statuses> GetAllStatuses()
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetAllStatuses();
            }
        }
        #endregion

        #region "Activity Master"
        public List<DataContracts.Masters.DC_Activity> GetActivityMaster(DataContracts.Masters.DC_Activity_Search_RQ RQ)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetActivityMaster(RQ);
            }
        }
        public List<DataContracts.Masters.DC_Activity> GetActivityMasterBySupplier(DataContracts.Masters.DC_Activity_Search_RQ RQ)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetActivityMasterBySupplier(RQ);
            }
        }
        public List<DataContracts.Masters.DC_Activity_Content> GetActivityContentMaster(DataContracts.Masters.DC_Activity_Content RQ)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetActivityContentMaster(RQ);
            }
        }
        public List<string> GetActivityNames(DataContracts.Masters.DC_Activity_Search_RQ RQ)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetActivityNames(RQ);
            }
        }
        #endregion

        #region Common Funciton to Get Codes by Entity Type
        public string GetCodeById(string objName, Guid obj_Id)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetCodeById(objName, obj_Id);
            }
        }
        public string GetRemarksForMapping(string from, Guid Mapping_Id)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetRemarksForMapping(from, Mapping_Id);
            }
        }
        public DC_GenericMasterDetails_ByIDOrName GetDetailsByIdOrName(DC_GenericMasterDetails_ByIDOrName _obj)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetDetailsByIdOrName(_obj);
            }
        }

        #endregion

        #region To Fill DropDown
        public List<DC_Accomodation_DDL> GetProductByCity(DC_Accomodation_DDL _obj)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetProductByCity(_obj);
            }

        }
        public List<DC_State_Master_DDL> GetMasterStateData(Guid Country_Id)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetMasterStateData(Country_Id);
            }

        }
        public List<DC_City_Master_DDL> GetMasterCityData(Guid Country_Id)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetMasterCityData(Country_Id);
            }

        }
        public List<DC_CityAreaLocation> GetMasterCityAreaLocationDetail(Guid CityAreaLocation_Id)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetMasterCityAreaLocationDetail(CityAreaLocation_Id);
            }

        }
        public List<DC_CityArea> GetMasterCityAreaDetail(Guid CityArea_Id)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetMasterCityAreaDetail(CityArea_Id);
            }

        }
        public List<DC_Supplier_DDL> GetSupplierMasterData()
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetSupplierMasterData();
            }

        }

        public DC_Supplier_DDL GetSupplierDataByMapping_Id(string objName, Guid Mapping_Id)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetSupplierDataByMapping_Id(objName, Mapping_Id);
            }

        }
        public List<DC_State_Master_DDL> GetStateByCity(Guid City_Id)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetStateByCity(City_Id);
            }
        }
        public List<DC_CountryMaster> GetCountryCodes(Guid obj_Id)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetCountryCodes(obj_Id);
            }
        }
        public List<DC_CountryMaster> GetMasterCountryDataList()
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetMasterCountryDataList();
            }

        }
        public List<DC_Activity_DDL> GetActivityByCountryCity(string CountryName, string CityName)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetActivityByCountryCity(CountryName, CityName);
            }
        }
        public List<DC_Supplier_DDL> GetSuppliersByProductCategory(string ProductCategory)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetSuppliersByProductCategory(ProductCategory);
            }
        }
        #endregion

        #region City Area and Location
        public bool SaveCityAreaLocation(DC_CityAreaLocation _obj)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.SaveCityAreaLocation(_obj);
            }
        }
        public List<DC_CityAreaLocation> GetMasterCityAreaLocationData(Guid CityArea_Id)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetMasterCityAreaLocationData(CityArea_Id);
            }
        }
        public bool SaveCityArea(DC_CityArea _obj)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.SaveCityArea(_obj);
            }
        }
        public List<DC_CityArea> GetMasterCityAreaData(Guid City_Id)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetMasterCityAreaData(City_Id);
            }
        }
        #endregion

        #region Keyword
        public DC_Message AddUpdateKeyword(DC_Keyword _obj)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.AddUpdateKeyword(_obj);
            }
        }

        public List<DC_Keyword> SearchKeyword(DC_Keyword_RQ RQ)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.SearchKeyword(RQ);
            }
        }

        public List<DC_keyword_alias> SearchKeywordAlias(DC_Keyword_RQ RQ)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.SearchKeywordAlias(RQ);
            }
        }

        public void DataHandler_Keyword_Update_NoOfHits(List<DC_keyword_alias> NoOfHits)
        {
            using (DataLayer.DL_Masters objDL = new DataLayer.DL_Masters())
            {
                objDL.DataHandler_Keyword_Update_NoOfHits(NoOfHits);
            }
        }

        public DC_Message ApplyKeyword(DC_keywordApply_RQ RQ)
        {
            using (DataLayer.DL_Masters objDL = new DataLayer.DL_Masters())
            {
                return objDL.ApplyKeyword(RQ);
            }
        }

        public DC_Message ReRunKeyword(string Entity, string Table, string Supplier_Id)
        {

            Guid gSupplier_Id;
            if (Guid.TryParse(Supplier_Id, out gSupplier_Id))
            {
                using (DataLayer.DL_Masters objDL = new DataLayer.DL_Masters())
                {
                    return objDL.ReRunKeyword(Entity, Table, gSupplier_Id);
                }
            }
            else
            {
                return new DC_Message { StatusMessage = "Invalid Supplier_id", StatusCode = ReadOnlyMessage.StatusCode.Danger };
            }

        }
        #endregion

        public string[] GetColumnNames(string TableName)
        {
            using (DataLayer.DL_Masters objDL = new DataLayer.DL_Masters())
            {
                return objDL.GetColumnNames(TableName);
            }
        }

        public List<string> GetListOfColumnNamesByTable(string TableName)
        {
            using (DataLayer.DL_Masters objDL = new DataLayer.DL_Masters())
            {
                return objDL.GetListOfColumnNamesByTable(TableName);
            }
        }

        #region Zone Master
        public DataContracts.DC_Message AddzoneMaster(DataContracts.Masters.DC_ZoneRQ param)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.AddzoneMaster(param);
            }
        }
        public DataContracts.DC_Message AddZoneCityMapping(DataContracts.Masters.DC_ZoneRQ param)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.AddZoneCityMapping(param);
            }
        }
        public List<DataContracts.Masters.DC_ZoneSearch> SearchZone(DataContracts.Masters.DC_ZoneRQ param)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.SearchZone(param);
            }
        }
        public List<DC_ZoneCitiesSearch> SearchZoneCities(DataContracts.Masters.DC_ZoneRQ param)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.SearchZoneCities(param);
            }
        }

        public DataContracts.DC_Message DeactivateOrActivateZones(DataContracts.Masters.DC_ZoneRQ param)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.DeactivateOrActivateZones(param);
            }
        }
        public List<DC_ZoneHotelList> SearchZoneHotels(DataContracts.Masters.DC_ZoneRQ param)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.SearchZoneHotels(param);
            }
        }
        public DataContracts.DC_Message InsertZoneHotelsInTable(DataContracts.Masters.DC_ZoneRQ param)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.InsertZoneHotelsInTable(param);
            }
        }
        public DataContracts.DC_Message DeleteZoneHotelsInTable(DataContracts.Masters.DC_ZoneRQ param)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.DeleteZoneHotelsInTable(param);
            }
        }
        public DC_Message UpdateZoneHotelsInTable(DataContracts.Masters.DC_ZoneRQ param)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.UpdateZoneHotelsInTable(param);
            }
        }
        public DC_Message IncludeExcludeHotels(DataContracts.Masters.DC_ZoneRQ param)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.IncludeExcludeHotels(param);
            }
        }
        #endregion

        #region
        public List<DataContracts.DC_Master_Country> GetRegionwiseCountriesList(List<string> RegionCodeList)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetRegionwiseCountriesList(RegionCodeList);
            }
        }

        public List<DataContracts.DC_Master_City> GetAllCountrywiseCitiesList(List<Guid> CountryIdListList)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetAllCountrywiseCitiesList(CountryIdListList);
            }
        }

        public List<DataContracts.DC_Master_City> GetCountrywiseCitiesList(DataContracts.Masters.DC_CitywithMultipleCountry_Search_RQ RQ)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetCountrywiseCitiesList(RQ);
            }
        }

        public List<DataContracts.DC_Master_City> GetCitiesDetails(string CountryName, string CityName)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.GetCitiesDetails(CountryName, CityName);
            }
        }
        #endregion

        #region Supplier Static Data Download
        public DataContracts.DC_Message SupplierStaticDataDownload_AddUpdate(DataContracts.Masters.DC_Supplier_StaticDataDownload objStatic)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.SupplierStaticDataDownload_AddUpdate(objStatic);
            }
        }

        public List<DataContracts.Masters.DC_Supplier_StaticDataDownload> SupplierStaticDataDownload_Get(DataContracts.Masters.DC_Supplier_StaticDataDownload objStatic)
        {
            using (DataLayer.DL_Masters obj = new DataLayer.DL_Masters())
            {
                return obj.SupplierStaticDataDownload_Get(objStatic);
            }
        }
        #endregion
    }
}
