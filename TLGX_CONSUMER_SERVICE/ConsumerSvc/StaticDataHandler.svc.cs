using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DataContracts;
using DataContracts.UploadStaticData;
using OperationContracts;
using BusinessLayer;
using DataContracts.STG;
using DataContracts.Masters;
using DataContracts.Mapping;

namespace ConsumerSvc
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "StaticDataHandler" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select StaticDataHandler.svc or StaticDataHandler.svc.cs at the Solution Explorer and start debugging.
    public class StaticDataHandler : IStaticDataHandler
    {
        public List<DC_SupplierImportFileDetails> GetStaticDataFileDetail(DC_SupplierImportFileDetails_RQ obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.GetStaticDataFileDetail(obj);
            }
        }

        public List<DC_SupplierImportAttributes> GetStaticDataMappingAttributes(DC_SupplierImportAttributes_RQ obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.GetStaticDataMappingAttributes(obj);
            }
        }

        public List<DC_SupplierImportAttributeValues> GetStaticDataMappingAttributeValues(DC_SupplierImportAttributeValues_RQ obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.GetStaticDataMappingAttributeValues(obj);
            }
        }

        public DC_Message UpdateStaticDataFileDetailStatus(DC_SupplierImportFileDetails obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.UpdateStaticDataFileDetailStatus(obj);
            }
        }
        public DC_Message AddStaticDataUploadErrorLog(DC_SupplierImportFile_ErrorLog obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.AddStaticDataUploadErrorLog(obj);
            }
        }

        public DC_Message AddSTGCountryData(List<DC_stg_SupplierCountryMapping> obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.AddSTGCountryData(obj);
            }
        }

        public IList<DC_Supplier> GetSupplier(DC_Supplier_Search_RQ RQ)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetSupplier(RQ);
            }
        }

        public DC_Message AddSTGCityData(List<DC_stg_SupplierCityMapping> obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.AddSTGCityData(obj);
            }
        }

        public DC_Message AddSTGProductData(List<DC_stg_SupplierProductMapping> obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.AddSTGProductData(obj);
            }
        }

        public List<DC_stg_SupplierCountryMapping> GetSTGCountryData(DC_stg_SupplierCountryMapping_RQ obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.GetSTGCountryData(obj);
            }
        }

        public List<DC_CountryMapping> GetMappingCountryData(DC_CountryMappingRQ obj)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetCountryMapping(obj);
            }
        }

        public bool UpdateCountryMapping(List<DC_CountryMapping> CM)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.UpdateCountryMapping(CM);
            }
        }

        public List<DC_CountryMapping> UpdateCountryMappingStatus(DataContracts.Mapping.DC_MappingMatch obj)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.UpdateCountryMappingStatus(obj);
            }
        }

        public List<DC_stg_SupplierCityMapping> GetSTGCityData(DC_stg_SupplierCityMapping_RQ obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.GetSTGCityData(obj);
            }
        }
        public List<DC_CityMapping> GetMappingCityData(DC_CityMapping_RQ obj)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetCityMapping(obj);
            }
        }

        public bool UpdateCityMapping(List<DC_CityMapping> CM)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.UpdateCityMapping(CM);
            }
        }

        public List<DC_CityMapping> UpdateCityMappingStatus(DataContracts.Mapping.DC_MappingMatch obj)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.UpdateCityMappingStatus(obj);
            }
        }


        public List<DC_stg_SupplierProductMapping> GetSTGHotelData(DC_stg_SupplierProductMapping_RQ obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.GetSTGHotelData(obj);
            }
        }

        public List<DC_Accomodation_ProductMapping> GetMappingHotelData(DC_Mapping_ProductSupplier_Search_RQ obj)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                //return objBL.GetProductSupplierMappingSearch(obj);
                return objBL.GetMappingHotelData(obj);
            }
        }

        public bool UpdateHotelMapping(List<DataContracts.Mapping.DC_Accomodation_ProductMapping> CM)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.UpdateAccomodationProductMapping(CM);
            }
        }

        public List<DC_Accomodation_ProductMapping> UpdateHotelMappingStatus(DC_MappingMatch obj)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.UpdateHotelMappingStatus(obj);
            }
        }
    }
}
