using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using DataContracts.Masters;
using DataContracts;
using System.Data;
using DataContracts.STG;
using DataContracts.UploadStaticData;

namespace DataLayer
{
    public class DL_UploadStaticData : IDisposable
    {
        public void Dispose()
        {
        }


        #region "Mapping Config Attributes"
        public List<DataContracts.UploadStaticData.DC_SupplierImportAttributes> GetStaticDataMappingAttributes(DataContracts.UploadStaticData.DC_SupplierImportAttributes_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var AttrMapSearch = from a in context.m_SupplierImportAttributes select a;

                    if (RQ.SupplierImportAttribute_Id.HasValue)
                    {
                        AttrMapSearch = from a in AttrMapSearch
                                        where a.SupplierImportAttribute_Id == RQ.SupplierImportAttribute_Id
                                        select a;
                    }

                    if (RQ.Supplier_Id.HasValue)
                    {
                        bool isFor = true;
                        if (!string.IsNullOrWhiteSpace(RQ.For))
                        {
                            if (RQ.For.Trim().TrimStart().ToUpper() == "MATCHING")
                                isFor = false;
                        }
                        if (isFor)
                        {
                            AttrMapSearch = from a in AttrMapSearch
                                            where a.Supplier_Id == RQ.Supplier_Id
                                            select a;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.Supplier))
                    {
                        bool isFor = true;
                        if (!string.IsNullOrWhiteSpace(RQ.For))
                        {
                            if (RQ.For.Trim().TrimStart().ToUpper() == "MATCHING")
                                isFor = false;
                        }
                        if (isFor)
                        {
                            AttrMapSearch = from a in AttrMapSearch
                                            join sup in context.Suppliers on a.Supplier_Id equals sup.Supplier_Id
                                            where sup.Name.Trim().TrimStart().ToUpper() == RQ.Supplier.Trim().TrimStart().ToUpper()
                                            select a;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.Entity))
                    {
                        AttrMapSearch = from a in AttrMapSearch
                                        where a.Entity.Trim().TrimStart().ToUpper() == RQ.Entity.Trim().TrimStart().ToUpper()
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.For))
                    {
                        AttrMapSearch = from a in AttrMapSearch
                                        where a.AttributeFor.Trim().TrimStart().ToUpper() == RQ.For.Trim().TrimStart().ToUpper()
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.Status))
                    {
                        AttrMapSearch = from a in AttrMapSearch
                                        where a.Status.Trim().TrimStart().ToUpper() == RQ.Status.Trim().TrimStart().ToUpper()
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.StatusExcept))
                    {
                        AttrMapSearch = from a in AttrMapSearch
                                        where a.Status.Trim().TrimStart().ToUpper() != RQ.StatusExcept.Trim().TrimStart().ToUpper()
                                        select a;
                    }

                    int total;

                    total = AttrMapSearch.Count();

                    var skip = RQ.PageSize * RQ.PageNo;

                    var AttrMapResult = (from a in AttrMapSearch
                                         join sup in context.Suppliers on a.Supplier_Id equals sup.Supplier_Id into s
                                         from lsup in s.DefaultIfEmpty()
                                         orderby lsup.Name, a.Entity
                                         select new DataContracts.UploadStaticData.DC_SupplierImportAttributes
                                         {
                                             SupplierImportAttribute_Id = a.SupplierImportAttribute_Id,
                                             Supplier = lsup.Name,
                                             Supplier_Id = a.Supplier_Id,
                                             Entity = a.Entity,
                                             Status = a.Status,
                                             For = a.AttributeFor,
                                             CREATE_DATE = a.CREATE_DATE,
                                             CREATE_USER = a.CREATE_USER,
                                             EDIT_DATE = a.EDIT_DATE,
                                             EDIT_USER = a.EDIT_USER,
                                             STG_Table = a.STG_Table,
                                             Master_Table = a.Master_Table,
                                             Mapping_Table = a.Mapping_Table,
                                             TotalRecords = total
                                         }
                                        ).Skip(skip).Take(RQ.PageSize).ToList();

                    return AttrMapResult;
                }
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching Upload Attributes", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public DataContracts.DC_Message AddStaticDataMappingAttribute(DataContracts.UploadStaticData.DC_SupplierImportAttributes obj)
        {
            DataContracts.DC_Message dc = new DataContracts.DC_Message();

            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    //Check duplicate 
                    var isDuplicate = (from attr in context.m_SupplierImportAttributes
                                       where attr.SupplierImportAttribute_Id == obj.SupplierImportAttribute_Id
                                       || (attr.Supplier_Id == obj.Supplier_Id && attr.Entity.Trim().ToUpper() == obj.Entity.Trim().ToUpper() && attr.AttributeFor.Trim().ToUpper() == obj.For.Trim().ToUpper())
                                       select attr).Count() == 0 ? false : true;

                    if (isDuplicate)
                    {
                        dc.StatusCode = ReadOnlyMessage.StatusCode.Duplicate;
                        dc.StatusMessage = "Supplier Mapping Attribute" + ReadOnlyMessage.strAlreadyExist;
                    }
                    else
                    {
                        DataLayer.m_SupplierImportAttributes objNew = new DataLayer.m_SupplierImportAttributes();

                        objNew.SupplierImportAttribute_Id = obj.SupplierImportAttribute_Id;
                        objNew.Supplier_Id = obj.Supplier_Id;
                        objNew.Entity = obj.Entity;
                        objNew.Status = obj.Status;
                        objNew.AttributeFor = obj.For;
                        objNew.CREATE_DATE = obj.CREATE_DATE;
                        objNew.CREATE_USER = obj.CREATE_USER;
                        MappingTableName _objMappingTableName = GetTableDeatils(obj.Entity);
                        if (_objMappingTableName != null)
                        {
                            objNew.STG_Table = _objMappingTableName.STG_Table;
                            objNew.Master_Table = _objMappingTableName.Master_Table;
                            objNew.Mapping_Table = _objMappingTableName.Mapping_Table;
                        }

                        context.m_SupplierImportAttributes.Add(objNew);
                        context.SaveChanges();
                        dc.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        dc.StatusMessage = "Supplier Mapping Attribute" + ReadOnlyMessage.strAddedSuccessfully;
                    }
                }
                return dc;
            }
            catch (Exception)
            {
                dc.StatusMessage = ReadOnlyMessage.strFailed;
                dc.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                return dc;
            }
        }
        #region Helping class MappingTableName
        public class MappingTableName
        {
            public string STG_Table { get; set; }
            public string Mapping_Table { get; set; }
            public string Master_Table { get; set; }

        }
        #endregion
        private MappingTableName GetTableDeatils(string entity)
        {
            MappingTableName _objMappingTableName = new MappingTableName();
            try
            {
                if (!string.IsNullOrWhiteSpace(entity))
                {
                    string strSTG_Table = "STAGING_" + entity.ToUpper();
                    string strMapping_Table = "MAPPING_" + entity.ToUpper();
                    string strMaster_Table = "MASTER_" + entity.ToUpper();
                    string strMasterFor = "MappingFileConfig";
                    string strParent = "MappingProcessType";

                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        var resultSTG =
                                  (from mst in context.m_masterattribute
                                   join mstv in context.m_masterattributevalue on mst.MasterAttribute_Id equals mstv.MasterAttribute_Id
                                   join msttblv in context.m_masterattributevalue on mstv.MasterAttributeValue_Id equals msttblv.ParentAttributeValue_Id
                                   where mst.MasterFor == strMasterFor && mst.Name == strParent
                                   && mstv.AttributeValue == strSTG_Table
                                   select msttblv.AttributeValue
                                   ).FirstOrDefault();

                        _objMappingTableName.STG_Table = resultSTG;
                        var resultMapping =
                            (from mst in context.m_masterattribute
                             join mstv in context.m_masterattributevalue on mst.MasterAttribute_Id equals mstv.MasterAttribute_Id
                             join msttblv in context.m_masterattributevalue on mstv.MasterAttributeValue_Id equals msttblv.ParentAttributeValue_Id
                             where mst.MasterFor == strMasterFor && mst.Name == strParent
                             && mstv.AttributeValue == strMapping_Table
                             select msttblv.AttributeValue
                               ).FirstOrDefault();
                        _objMappingTableName.Mapping_Table = resultMapping;

                        var resultMaster =
                            (from mst in context.m_masterattribute
                             join mstv in context.m_masterattributevalue on mst.MasterAttribute_Id equals mstv.MasterAttribute_Id
                             join msttblv in context.m_masterattributevalue on mstv.MasterAttributeValue_Id equals msttblv.ParentAttributeValue_Id
                             where mst.MasterFor == strMasterFor && mst.Name == strParent
                             && mstv.AttributeValue == strMaster_Table
                             select msttblv.AttributeValue
                               ).FirstOrDefault();
                        _objMappingTableName.Master_Table = resultMaster;

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return _objMappingTableName;
        }

        public DataContracts.DC_Message UpdateStaticDataMappingAttribute(List<DataContracts.UploadStaticData.DC_SupplierImportAttributes> lobj)
        {
            DataContracts.DC_Message dc = new DataContracts.DC_Message();

            foreach (DataContracts.UploadStaticData.DC_SupplierImportAttributes obj in lobj)
            {
                try
                {
                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        var isDuplicate = (from a in context.m_SupplierImportAttributes
                                           where a.Entity.Trim().TrimStart().ToUpper() == obj.Entity.Trim().TrimStart().ToUpper()
                                           && a.SupplierImportAttribute_Id != obj.SupplierImportAttribute_Id
                                           && a.Supplier_Id == obj.Supplier_Id
                                           && a.AttributeFor.Trim().ToUpper() == obj.For.Trim().ToUpper()
                                           select a).Count() == 0 ? false : true;

                        if (isDuplicate)
                        {
                            dc.StatusCode = ReadOnlyMessage.StatusCode.Duplicate;
                            dc.StatusMessage = "Supplier Mapping Attribute With This Combination " + ReadOnlyMessage.strAlreadyExist;

                        }
                        else
                        {
                            var search = context.m_SupplierImportAttributes.Find(obj.SupplierImportAttribute_Id);
                            if (search != null)
                            {

                                search.Supplier_Id = obj.Supplier_Id;
                                search.Entity = obj.Entity;
                                search.Status = obj.Status;
                                search.AttributeFor = obj.For;
                                search.EDIT_DATE = obj.EDIT_DATE;
                                search.EDIT_USER = obj.EDIT_USER;
                            }
                            context.SaveChanges();
                            dc.StatusCode = ReadOnlyMessage.StatusCode.Success;
                            dc.StatusMessage = "Supplier Mapping Attribute" + ReadOnlyMessage.strUpdatedSuccessfully;
                        }
                    }
                }
                catch (Exception)
                {
                    dc.StatusMessage = ReadOnlyMessage.strFailed;
                    dc.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                    return dc;
                }
            }

            return dc;
        }

        public DataContracts.DC_Message UpdateStaticDataMappingAttributeStatus(List<DataContracts.UploadStaticData.DC_SupplierImportAttributes> lobj)
        {
            DataContracts.DC_Message dc = new DataContracts.DC_Message();

            foreach (DataContracts.UploadStaticData.DC_SupplierImportAttributes obj in lobj)
            {
                try
                {
                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        var search = context.m_SupplierImportAttributes.Find(obj.SupplierImportAttribute_Id);
                        if (search != null)
                        {
                            search.Status = obj.Status;
                            search.EDIT_DATE = obj.EDIT_DATE;
                            search.EDIT_USER = obj.EDIT_USER;
                        }
                        context.SaveChanges();
                        dc.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        dc.StatusMessage = "Supplier Mapping Attribute Status" + ReadOnlyMessage.strUpdatedSuccessfully;
                    }
                }
                catch (Exception)
                {
                    dc.StatusMessage = ReadOnlyMessage.strFailed;
                    dc.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                    return dc;
                }
            }

            return dc;
        }
        #endregion


        #region "Mapping Config Attributes Values"
        public List<DC_SupplierImportAttributeValues> GetStaticDataMappingAttributeValues(DataContracts.UploadStaticData.DC_SupplierImportAttributeValues_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var AttrMapSearch = from a in context.m_SupplierImportAttributeValues select a;

                    if (RQ.SupplierImportAttributeValue_Id.HasValue)
                    {
                        AttrMapSearch = from a in AttrMapSearch
                                        where a.SupplierImportAttributeValue_Id == RQ.SupplierImportAttributeValue_Id
                                        select a;
                    }

                    if (RQ.SupplierImportAttribute_Id.HasValue)
                    {
                        AttrMapSearch = from a in AttrMapSearch
                                        where a.SupplierImportAttribute_Id == RQ.SupplierImportAttribute_Id
                                        select a;
                    }

                    if (RQ.Supplier_Id.HasValue)
                    {
                        bool isFor = true;
                        if (!string.IsNullOrWhiteSpace(RQ.For))
                        {
                            if (RQ.For.Trim().TrimStart().ToUpper() == "MATCHING")
                                isFor = false;
                        }
                        if (isFor)
                        {
                            AttrMapSearch = from a in AttrMapSearch
                                            join at in context.m_SupplierImportAttributes on a.SupplierImportAttribute_Id equals at.SupplierImportAttribute_Id
                                            where at.Supplier_Id == RQ.Supplier_Id
                                            select a;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.Supplier))
                    {
                        bool isFor = true;
                        if (!string.IsNullOrWhiteSpace(RQ.For))
                        {
                            if (RQ.For.Trim().TrimStart().ToUpper() == "MATCHING")
                                isFor = false;
                        }
                        if (isFor)
                        {
                            AttrMapSearch = from a in AttrMapSearch
                                            join at in context.m_SupplierImportAttributes on a.SupplierImportAttribute_Id equals at.SupplierImportAttribute_Id
                                            join s in context.Suppliers on at.Supplier_Id equals s.Supplier_Id
                                            where s.Name.Trim().TrimStart().ToUpper() == RQ.Supplier.Trim().TrimStart().ToUpper()
                                            select a;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.Entity))
                    {
                        AttrMapSearch = from a in AttrMapSearch
                                        join at in context.m_SupplierImportAttributes on a.SupplierImportAttribute_Id equals at.SupplierImportAttribute_Id
                                        where at.Entity.Trim().TrimStart().ToUpper() == RQ.Entity.Trim().TrimStart().ToUpper()
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.For))
                    {
                        AttrMapSearch = from a in AttrMapSearch
                                        join at in context.m_SupplierImportAttributes on a.SupplierImportAttribute_Id equals at.SupplierImportAttribute_Id
                                        where at.AttributeFor.Trim().TrimStart().ToUpper() == RQ.For.Trim().TrimStart().ToUpper()
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.AttributeType))
                    {
                        AttrMapSearch = from a in AttrMapSearch
                                        where a.AttributeType.Trim().TrimStart().ToUpper() == RQ.AttributeType.Trim().TrimStart().ToUpper()
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.AttributeName))
                    {
                        AttrMapSearch = from a in AttrMapSearch
                                        where a.AttributeName.Trim().TrimStart().ToUpper() == RQ.AttributeName.Trim().TrimStart().ToUpper()
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.AttributeValue))
                    {
                        AttrMapSearch = from a in AttrMapSearch
                                        where a.AttributeValue.Trim().TrimStart().ToUpper() == RQ.AttributeValue.Trim().TrimStart().ToUpper()
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.Status))
                    {
                        AttrMapSearch = from a in AttrMapSearch
                                        where a.STATUS.Trim().TrimStart().ToUpper() == RQ.Status.Trim().TrimStart().ToUpper()
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.StatusExcept))
                    {
                        AttrMapSearch = from a in AttrMapSearch
                                        where a.STATUS.Trim().TrimStart().ToUpper() != RQ.StatusExcept.Trim().TrimStart().ToUpper()
                                        select a;
                    }
                    if (RQ.Priority != null)
                    {
                        if (RQ.Priority != -1)
                        {
                            AttrMapSearch = from a in AttrMapSearch
                                            where a.Priority == RQ.Priority
                                            select a;
                        }
                    }


                    int total;

                    total = AttrMapSearch.Count();

                    var skip = RQ.PageSize * RQ.PageNo;

                    var AttrMapResult = (from a in AttrMapSearch
                                         orderby (a.Priority ?? 0) descending, a.AttributeType, a.CREATE_DATE descending //, a.AttributeName, a.AttributeValue
                                         select new DataContracts.UploadStaticData.DC_SupplierImportAttributeValues
                                         {
                                             SupplierImportAttributeValue_Id = a.SupplierImportAttributeValue_Id,
                                             SupplierImportAttribute_Id = a.SupplierImportAttribute_Id,
                                             AttributeType = a.AttributeType,
                                             AttributeName = a.AttributeName,
                                             AttributeValue_ID = a.AttributeValue_ID,
                                             AttributeValue = a.AttributeValue,
                                             STATUS = a.STATUS,
                                             CREATE_DATE = a.CREATE_DATE,
                                             CREATE_USER = a.CREATE_USER,
                                             EDIT_DATE = a.EDIT_DATE,
                                             EDIT_USER = a.EDIT_USER,
                                             Description = a.Description,
                                             Priority = a.Priority ?? 0,
                                             TotalRecords = total
                                         }
                                        ).Skip(skip).Take(RQ.PageSize).ToList();

                    return AttrMapResult;
                }
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus
                {
                    ErrorMessage = "Error while searching Upload Attributes",
                    ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError
                });
            }
        }

        public DataContracts.DC_Message AddStaticDataMappingAttributeValues(DataContracts.UploadStaticData.DC_SupplierImportAttributeValues obj)
        {
            DataContracts.DC_Message dc = new DataContracts.DC_Message();

            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    //Check duplicate 
                    var isDuplicate = (from attr in context.m_SupplierImportAttributeValues
                                       where attr.SupplierImportAttributeValue_Id == obj.SupplierImportAttributeValue_Id ||
                                       (attr.AttributeType.Trim().TrimStart().ToUpper() == obj.AttributeType.Trim().TrimStart().ToUpper() &&
                                        attr.AttributeName.Trim().TrimStart().ToUpper() == obj.AttributeName.Trim().TrimStart().ToUpper() &&
                                        attr.AttributeValue_ID.Value == obj.AttributeValue_ID.Value &&
                                        attr.AttributeValue.Trim().TrimStart().ToUpper() == obj.AttributeValue.Trim().TrimStart().ToUpper() &&
                                        attr.SupplierImportAttribute_Id == obj.SupplierImportAttribute_Id &&
                                        attr.Priority == obj.Priority
                                       )
                                       select attr).Count() == 0 ? false : true;

                    if (isDuplicate)
                    {
                        dc.StatusCode = ReadOnlyMessage.StatusCode.Duplicate;
                        dc.StatusMessage = "Supplier Mapping Attribute" + ReadOnlyMessage.strAlreadyExist;
                    }
                    else
                    {
                        DataLayer.m_SupplierImportAttributeValues objNew = new DataLayer.m_SupplierImportAttributeValues();

                        objNew.SupplierImportAttributeValue_Id = obj.SupplierImportAttributeValue_Id;
                        objNew.SupplierImportAttribute_Id = obj.SupplierImportAttribute_Id;
                        objNew.AttributeType = obj.AttributeType;
                        objNew.AttributeName = obj.AttributeName;
                        objNew.AttributeValue = obj.AttributeValue;
                        objNew.AttributeValue_ID = obj.AttributeValue_ID;
                        objNew.CREATE_DATE = obj.CREATE_DATE;
                        objNew.CREATE_USER = obj.CREATE_USER;
                        objNew.STATUS = obj.STATUS;
                        objNew.Priority = obj.Priority;
                        objNew.Description = obj.Description;
                        context.m_SupplierImportAttributeValues.Add(objNew);
                        context.SaveChanges();
                        dc.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        dc.StatusMessage = "Supplier Mapping Attribute Value" + ReadOnlyMessage.strAddedSuccessfully;
                    }
                }
                return dc;
            }
            catch (Exception)
            {
                dc.StatusMessage = ReadOnlyMessage.strFailed;
                dc.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                return dc;
            }
        }

        public DataContracts.DC_Message UpdateStaticDataMappingAttributeValues(List<DataContracts.UploadStaticData.DC_SupplierImportAttributeValues> lobj)
        {
            DataContracts.DC_Message dc = new DataContracts.DC_Message();

            foreach (DataContracts.UploadStaticData.DC_SupplierImportAttributeValues obj in lobj)
            {
                try
                {
                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        var isDuplicate = (from a in context.m_SupplierImportAttributeValues
                                           where a.AttributeType.Trim().TrimStart().ToUpper() == obj.AttributeType.Trim().TrimStart().ToUpper() &&
                                            a.AttributeName.Trim().TrimStart().ToUpper() == obj.AttributeName.Trim().TrimStart().ToUpper() &&
                                            a.AttributeValue_ID.Value == obj.AttributeValue_ID.Value &&
                                            a.AttributeValue.Trim().TrimStart().ToUpper() == obj.AttributeValue.Trim().TrimStart().ToUpper() &&
                                            a.SupplierImportAttribute_Id == obj.SupplierImportAttribute_Id &&
                                            a.SupplierImportAttributeValue_Id != obj.SupplierImportAttributeValue_Id &&
                                            a.Priority != obj.Priority
                                           //&& a.Status.Trim().TrimStart().ToUpper() == obj.Status.Trim().TrimStart().ToUpper()
                                           select a).Count() == 0 ? false : true;

                        if (isDuplicate)
                        {
                            dc.StatusCode = ReadOnlyMessage.StatusCode.Duplicate;
                            dc.StatusMessage = "Supplier Mapping Attribute With This Combination " + ReadOnlyMessage.strAlreadyExist;

                        }
                        else
                        {
                            var search = context.m_SupplierImportAttributeValues.Find(obj.SupplierImportAttributeValue_Id);
                            if (search != null)
                            {
                                search.AttributeType = obj.AttributeType;
                                search.AttributeName = obj.AttributeName;
                                search.AttributeValue_ID = obj.AttributeValue_ID;
                                search.AttributeValue = obj.AttributeValue;
                                search.STATUS = obj.STATUS;
                                search.EDIT_DATE = obj.EDIT_DATE;
                                search.EDIT_USER = obj.EDIT_USER;
                                search.Priority = obj.Priority;
                                search.Description = obj.Description;
                            }
                            context.SaveChanges();
                            dc.StatusCode = ReadOnlyMessage.StatusCode.Success;
                            dc.StatusMessage = "Supplier Mapping Attribute Value" + ReadOnlyMessage.strUpdatedSuccessfully;
                        }
                    }
                }
                catch (Exception)
                {
                    dc.StatusMessage = ReadOnlyMessage.strFailed;
                    dc.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                    return dc;
                }
            }

            return dc;
        }

        public DataContracts.DC_Message UpdateStaticDataMappingAttributeValueStatus(List<DataContracts.UploadStaticData.DC_SupplierImportAttributeValues> lobj)
        {
            DataContracts.DC_Message dc = new DataContracts.DC_Message();

            foreach (DataContracts.UploadStaticData.DC_SupplierImportAttributeValues obj in lobj)
            {
                try
                {
                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        var search = context.m_SupplierImportAttributeValues.Find(obj.SupplierImportAttributeValue_Id);
                        if (search != null)
                        {
                            search.STATUS = obj.STATUS;
                            search.EDIT_DATE = obj.EDIT_DATE;
                            search.EDIT_USER = obj.EDIT_USER;
                        }
                        context.SaveChanges();
                        dc.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        dc.StatusMessage = "Supplier Mapping Attribute Value Status" + ReadOnlyMessage.strUpdatedSuccessfully;
                    }
                }
                catch (Exception)
                {
                    dc.StatusMessage = ReadOnlyMessage.strFailed;
                    dc.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                    return dc;
                }
            }

            return dc;
        }

        public List<string> GetStaticDataMappingAttributeValuesForFilter(DataContracts.UploadStaticData.DC_SupplierImportAttributeValues_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var AttrMapSearch = from a in context.m_SupplierImportAttributeValues select a;

                    if (RQ.SupplierImportAttributeValue_Id.HasValue)
                    {
                        AttrMapSearch = from a in AttrMapSearch
                                        where a.SupplierImportAttributeValue_Id == RQ.SupplierImportAttributeValue_Id
                                        select a;
                    }

                    if (RQ.SupplierImportAttribute_Id.HasValue)
                    {
                        AttrMapSearch = from a in AttrMapSearch
                                        where a.SupplierImportAttribute_Id == RQ.SupplierImportAttribute_Id
                                        select a;
                    }
                    List<string> _lstFilterData = new List<string>();
                    if (RQ.For.ToLower() == "attributetype")
                    {
                        _lstFilterData = AttrMapSearch.Select(a => a.AttributeType).Distinct().ToList();
                    }
                    else if (RQ.For.ToLower() == "priority")
                    {
                        _lstFilterData = AttrMapSearch.Select(a => a.Priority.ToString()).Distinct().ToList();
                    }

                    return _lstFilterData;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus
                {
                    ErrorMessage = "Error while getteing filter data for manageattrinutevalue",
                    ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError
                });
            }

        }
        #endregion


        #region "Upload File"
        public List<DataContracts.UploadStaticData.DC_SupplierImportFileDetails> GetStaticDataFileDetail(DataContracts.UploadStaticData.DC_SupplierImportFileDetails_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var FileSearch = from a in context.SupplierImportFileDetails select a;

                    if (RQ.SupplierImportFile_Id.HasValue)
                    {
                        FileSearch = from a in FileSearch
                                     where a.SupplierImportFile_Id == RQ.SupplierImportFile_Id
                                     select a;
                    }

                    if (RQ.Supplier_Id.HasValue)
                    {
                        FileSearch = from a in FileSearch
                                     where a.Supplier_Id == RQ.Supplier_Id
                                     select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.Supplier))
                    {
                        FileSearch = from a in FileSearch
                                     join s in context.Suppliers on a.Supplier_Id equals s.Supplier_Id
                                     where s.Name.Trim().TrimStart().ToUpper() == RQ.Supplier.Trim().TrimStart().ToUpper()
                                     select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.Entity))
                    {
                        FileSearch = from a in FileSearch
                                     where a.Entity.Trim().TrimStart().ToUpper() == RQ.Entity.Trim().TrimStart().ToUpper()
                                     select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.STATUS))
                    {
                        FileSearch = from a in FileSearch
                                     where a.STATUS.Trim().TrimStart().ToUpper() == RQ.STATUS.Trim().TrimStart().ToUpper()
                                     select a;
                    }

                    if (RQ.From_Date.HasValue && RQ.TO_Date.HasValue)
                    {
                        FileSearch = from a in FileSearch
                                     where a.CREATE_DATE >= RQ.From_Date && a.CREATE_DATE <= RQ.TO_Date
                                     select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.StatusExcept))
                    {
                        FileSearch = from a in FileSearch
                                     where a.STATUS.Trim().TrimStart().ToUpper() != RQ.StatusExcept.Trim().TrimStart().ToUpper()
                                     select a;
                    }


                    int total;

                    total = FileSearch.Count();

                    var skip = RQ.PageSize * RQ.PageNo;
                    if (RQ.PageSize == 0)
                    {
                        RQ.PageSize = int.MaxValue;
                    }

                    var FileSearchResult = (from a in FileSearch
                                            join s in context.Suppliers on a.Supplier_Id equals s.Supplier_Id
                                            where s.StatusCode.ToUpper() == "ACTIVE"
                                            orderby a.CREATE_DATE descending
                                            select new DataContracts.UploadStaticData.DC_SupplierImportFileDetails
                                            {
                                                SupplierImportFile_Id = a.SupplierImportFile_Id,
                                                Supplier_Id = a.Supplier_Id,
                                                Supplier = s.Name,
                                                Entity = a.Entity,
                                                OriginalFilePath = a.OriginalFilePath,
                                                SavedFilePath = a.SavedFilePath,
                                                ArchiveFilePath = a.ArchiveFilePath,
                                                STATUS = a.STATUS,
                                                CREATE_DATE = a.CREATE_DATE,
                                                CREATE_USER = a.CREATE_USER,
                                                PROCESS_DATE = a.PROCESS_DATE,
                                                PROCESS_USER = a.PROCESS_USER,
                                                IsActive = a.IsActive ?? true,
                                                TotalRecords = total
                                            }
                                        ).Skip(skip).Take(RQ.PageSize).ToList();

                    return FileSearchResult;
                }
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus
                {
                    ErrorMessage = "Error while searching Upload File",
                    ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError
                });
            }
        }

        public DataContracts.DC_Message AddStaticDataFileDetail(DataContracts.UploadStaticData.DC_SupplierImportFileDetails obj)
        {
            DataContracts.DC_Message dc = new DataContracts.DC_Message();

            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    DataLayer.SupplierImportFileDetail objNew = new DataLayer.SupplierImportFileDetail();

                    objNew.SupplierImportFile_Id = obj.SupplierImportFile_Id;
                    objNew.Supplier_Id = obj.Supplier_Id;
                    objNew.Entity = obj.Entity;
                    objNew.STATUS = obj.STATUS;
                    objNew.ArchiveFilePath = obj.ArchiveFilePath;
                    objNew.OriginalFilePath = obj.OriginalFilePath;
                    objNew.SavedFilePath = obj.SavedFilePath;
                    objNew.CREATE_DATE = obj.CREATE_DATE;
                    objNew.CREATE_USER = obj.CREATE_USER;
                    context.SupplierImportFileDetails.Add(objNew);
                    context.SaveChanges();
                    dc.StatusCode = ReadOnlyMessage.StatusCode.Success;
                    dc.StatusMessage = "Supplier File " + ReadOnlyMessage.strAddedSuccessfully;
                }
                return dc;
            }
            catch (Exception)
            {
                dc.StatusMessage = ReadOnlyMessage.strFailed;
                dc.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                return dc;
            }
        }

        public DataContracts.DC_Message UpdateStaticDataFileDetail(List<DataContracts.UploadStaticData.DC_SupplierImportFileDetails> lobj)
        {
            DataContracts.DC_Message dc = new DataContracts.DC_Message();

            foreach (DataContracts.UploadStaticData.DC_SupplierImportFileDetails obj in lobj)
            {
                try
                {
                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        var isDuplicate = (from a in context.SupplierImportFileDetails
                                           where a.Entity.Trim().TrimStart().ToUpper() == obj.Entity.Trim().TrimStart().ToUpper() &&
                                            a.Supplier_Id == obj.Supplier_Id &&
                                            a.SupplierImportFile_Id != obj.SupplierImportFile_Id
                                           //&& a.Status.Trim().TrimStart().ToUpper() == obj.Status.Trim().TrimStart().ToUpper()
                                           select a).Count() == 0 ? false : true;

                        if (isDuplicate)
                        {
                            dc.StatusCode = ReadOnlyMessage.StatusCode.Duplicate;
                            dc.StatusMessage = "Supplier File With This Combination " + ReadOnlyMessage.strAlreadyExist;

                        }
                        else
                        {
                            var search = context.SupplierImportFileDetails.Find(obj.SupplierImportFile_Id);
                            if (search != null)
                            {
                                search.Supplier_Id = obj.Supplier_Id;
                                search.Entity = obj.Entity;
                                search.ArchiveFilePath = obj.ArchiveFilePath;
                                search.OriginalFilePath = obj.OriginalFilePath;
                                search.SavedFilePath = obj.SavedFilePath;
                                search.STATUS = obj.STATUS;
                                search.PROCESS_DATE = obj.PROCESS_DATE;
                                search.PROCESS_USER = obj.PROCESS_USER;
                                search.IsActive = obj.IsActive;
                            }
                            context.SaveChanges();
                            dc.StatusCode = ReadOnlyMessage.StatusCode.Success;
                            dc.StatusMessage = "Supplier File " + ReadOnlyMessage.strUpdatedSuccessfully;
                        }
                    }
                }
                catch (Exception)
                {
                    dc.StatusMessage = ReadOnlyMessage.strFailed;
                    dc.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                    return dc;
                }
            }

            return dc;
        }


        public DataContracts.DC_Message UpdateStaticDataFileDetailStatus(DataContracts.UploadStaticData.DC_SupplierImportFileDetails obj)
        {
            DataContracts.DC_Message dc = new DataContracts.DC_Message();

            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = context.SupplierImportFileDetails.Find(obj.SupplierImportFile_Id);
                    if (search != null)
                    {
                        search.ArchiveFilePath = obj.ArchiveFilePath;
                        search.STATUS = obj.STATUS;
                        search.PROCESS_USER = obj.PROCESS_USER;
                        search.PROCESS_DATE = obj.PROCESS_DATE;
                        search.IsActive = obj.IsActive;
                    }
                    context.SaveChanges();
                    dc.StatusCode = ReadOnlyMessage.StatusCode.Success;
                    dc.StatusMessage = "Supplier File Status" + ReadOnlyMessage.strUpdatedSuccessfully;
                }
            }
            catch (Exception)
            {
                dc.StatusMessage = ReadOnlyMessage.strFailed;
                dc.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                return dc;
            }

            return dc;
        }
        #endregion

        #region "Logging"
        public DataContracts.DC_Message AddStaticDataUploadErrorLog(DataContracts.UploadStaticData.DC_SupplierImportFile_ErrorLog obj)
        {
            DataContracts.DC_Message dc = new DataContracts.DC_Message();

            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    DataLayer.SupplierImportFile_ErrorLog objNew = new DataLayer.SupplierImportFile_ErrorLog();
                    objNew.SupplierImportFile_ErrorLog_Id = obj.SupplierImportFile_ErrorLog_Id;
                    objNew.SupplierImportFile_Id = obj.SupplierImportFile_Id;
                    objNew.ErrorCode = obj.ErrorCode;
                    objNew.ErrorType = obj.ErrorType;
                    objNew.ErrorDescription = obj.ErrorDescription;
                    objNew.Error_DATE = obj.Error_DATE;
                    objNew.Error_USER = obj.Error_USER;
                    objNew.ErrorMessage_UI = obj.ErrorMessage_UI;
                    context.SupplierImportFile_ErrorLog.Add(objNew);
                    context.SaveChanges();
                    dc.StatusCode = ReadOnlyMessage.StatusCode.Success;
                    dc.StatusMessage = "ErrorLog " + ReadOnlyMessage.strAddedSuccessfully;
                }
                return dc;
            }
            catch (Exception e)
            {
                dc.StatusMessage = ReadOnlyMessage.strFailed;
                dc.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                return dc;
            }

        }
        public List<DataContracts.UploadStaticData.DC_SupplierImportFile_ErrorLog> GetStaticDataUploadErrorLog(DataContracts.UploadStaticData.DC_SupplierImportFile_ErrorLog_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var AttrMapSearch = from a in context.SupplierImportFile_ErrorLog select a;

                    if (RQ.SupplierImportFile_ErrorLog_Id.HasValue && RQ.SupplierImportFile_ErrorLog_Id != Guid.Empty)
                    {
                        AttrMapSearch = from a in AttrMapSearch
                                        where a.SupplierImportFile_ErrorLog_Id == RQ.SupplierImportFile_ErrorLog_Id
                                        select a;
                    }
                    if (RQ.SupplierImportFile_Id.HasValue && RQ.SupplierImportFile_Id != Guid.Empty)
                    {
                        AttrMapSearch = from a in AttrMapSearch
                                        where a.SupplierImportFile_Id == RQ.SupplierImportFile_Id
                                        select a;
                    }
                    if (!string.IsNullOrWhiteSpace(RQ.ErrorType))
                    {
                        AttrMapSearch = from a in AttrMapSearch
                                        where a.ErrorType.Trim().ToUpper() == RQ.ErrorType.Trim().ToUpper()
                                        select a;
                    }

                    var total = AttrMapSearch.Count();

                    var skip = RQ.PageSize * RQ.PageNo;

                    var AttrMapResult = (from a in AttrMapSearch
                                         orderby a.Error_DATE
                                         select new DataContracts.UploadStaticData.DC_SupplierImportFile_ErrorLog
                                         {
                                             SupplierImportFile_ErrorLog_Id = a.SupplierImportFile_ErrorLog_Id,
                                             SupplierImportFile_Id = a.SupplierImportFile_Id,
                                             ErrorDescription = a.ErrorDescription,
                                             ErrorType = a.ErrorType,
                                             Error_DATE = a.Error_DATE,
                                             Error_USER = a.Error_USER,
                                             ErrorCode = a.ErrorCode,
                                             ErrorMessage_UI = a.ErrorMessage_UI,
                                             TotalCount = total
                                         }).Skip(skip ?? 0).Take(RQ.PageSize ?? total).ToList();

                    return AttrMapResult;
                }
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus
                {
                    ErrorMessage = "Error while searching Error Attributes",
                    ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError
                });
            }
        }

        public DataContracts.DC_Message AddStaticDataUploadProcessLog(DataContracts.UploadStaticData.DC_SupplierImportFile_Progress obj)
        {
            DataContracts.DC_Message dc = new DataContracts.DC_Message();

            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (obj.SupplierImportFile_Id != null && !string.IsNullOrWhiteSpace(obj.Step))
                    {
                        if (obj.CurrentBatch != 0 && obj.Step != "READ")
                        {
                            context.SupplierImportFile_Progress.RemoveRange(context.SupplierImportFile_Progress.Where(w => w.SupplierImportFile_Id == obj.SupplierImportFile_Id && w.CurrentBatch != 0 && w.Step.ToString().ToUpper() == obj.Step.ToString().ToUpper() && w.CurrentBatch != obj.CurrentBatch));
                        }

                        var progress = (from a in context.SupplierImportFile_Progress
                                        where a.SupplierImportFile_Id == obj.SupplierImportFile_Id && a.Step == obj.Step
                                        select a).FirstOrDefault();

                        if (progress != null)
                        {
                            progress.PercentageValue = obj.PercentageValue;
                            progress.LastCheckedOn = DateTime.Now;
                            progress.CurrentBatch = obj.CurrentBatch;
                            progress.TotalBatch = obj.TotalBatch;
                        }
                        else
                        {
                            context.SupplierImportFile_Progress.Add(new SupplierImportFile_Progress
                            {
                                SupplierImportFileProgress_Id = Guid.NewGuid(),
                                SupplierImportFile_Id = obj.SupplierImportFile_Id,
                                PercentageValue = obj.PercentageValue,
                                Status = obj.Status,
                                Step = obj.Step,
                                CurrentBatch = obj.CurrentBatch,
                                LastCheckedOn = DateTime.Now,
                                TotalBatch = obj.TotalBatch
                            });
                        }
                    }

                    context.SaveChanges();
                    dc.StatusCode = ReadOnlyMessage.StatusCode.Success;
                    dc.StatusMessage = "Process Log " + ReadOnlyMessage.strAddedSuccessfully;
                }
                return dc;
            }
            catch (Exception e)
            {
                dc.StatusMessage = ReadOnlyMessage.strFailed;
                dc.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                return dc;
            }
        }

        public List<DataContracts.UploadStaticData.DC_SupplierImportFile_Statistics> GetStaticDataUploadStatistics(DataContracts.UploadStaticData.DC_SupplierImportFile_Statistics_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var StatSearch = from a in context.SupplierImportFile_Statistics select a;

                    if (RQ.SupplierImportFile_Id.HasValue && RQ.SupplierImportFile_Id != Guid.Empty)
                    {
                        StatSearch = from a in StatSearch
                                     where a.SupplierImportFile_Id == RQ.SupplierImportFile_Id
                                     select a;
                    }
                    if (RQ.Supplier_Id.HasValue && RQ.Supplier_Id != Guid.Empty)
                    {
                        StatSearch = from a in StatSearch
                                     join f in context.SupplierImportFileDetails.AsNoTracking() on a.SupplierImportFile_Id equals f.SupplierImportFile_Id
                                     where f.Supplier_Id == RQ.Supplier_Id
                                     select a;
                    }
                    if (!string.IsNullOrWhiteSpace(RQ.Supplier))
                    {
                        StatSearch = from a in StatSearch
                                     join f in context.SupplierImportFileDetails.AsNoTracking() on a.SupplierImportFile_Id equals f.SupplierImportFile_Id
                                     join s in context.Suppliers.AsNoTracking() on f.Supplier_Id equals s.Supplier_Id
                                     where s.Name.Trim().ToUpper() == RQ.Supplier.Trim().ToUpper()
                                     select a;
                    }
                    if (!string.IsNullOrWhiteSpace(RQ.Entity))
                    {
                        StatSearch = from a in StatSearch
                                     join f in context.SupplierImportFileDetails.AsNoTracking() on a.SupplierImportFile_Id equals f.SupplierImportFile_Id
                                     where f.Entity.Trim().ToUpper() == RQ.Entity.Trim().ToUpper()
                                     select a;
                    }
                    if (!string.IsNullOrWhiteSpace(RQ.STATUS))
                    {
                        StatSearch = from a in StatSearch
                                     where a.FinalStatus.Trim().ToUpper() == RQ.STATUS.Trim().ToUpper()
                                     select a;
                    }
                    if (!string.IsNullOrWhiteSpace(RQ.StatusExcept))
                    {
                        StatSearch = from a in StatSearch
                                     where a.FinalStatus.Trim().ToUpper() != RQ.StatusExcept.Trim().ToUpper()
                                     select a;
                    }
                    if (RQ.From_Date != null)
                    {
                        if (RQ.TO_Date != null)
                        {
                            StatSearch = from a in StatSearch
                                         where a.Process_Date >= RQ.From_Date && a.Process_Date <= RQ.TO_Date
                                         select a;
                        }
                        else
                        {
                            StatSearch = from a in StatSearch
                                         where a.Process_Date >= RQ.From_Date
                                         select a;
                        }
                    }
                    else
                    {
                        if (RQ.TO_Date != null)
                        {
                            StatSearch = from a in StatSearch
                                         where a.Process_Date <= RQ.TO_Date
                                         select a;
                        }
                    }

                    var total = StatSearch.Count();

                    var skip = RQ.PageSize * RQ.PageNo;

                    var StatResult = (from a in StatSearch
                                      join f in context.SupplierImportFileDetails.AsNoTracking() on a.SupplierImportFile_Id equals f.SupplierImportFile_Id
                                      join s in context.Suppliers.AsNoTracking() on f.Supplier_Id equals s.Supplier_Id
                                      orderby s.Name, f.Entity
                                      select new DataContracts.UploadStaticData.DC_SupplierImportFile_Statistics
                                      {
                                          SupplierImportFile_Statistics_Id = a.SupplierImportFile_Statistics_Id,
                                          SupplierImportFile_Id = a.SupplierImportFile_Id,
                                          TotalRows = a.TotalRows,
                                          Unmapped = a.Unmapped,
                                          Mapped = a.Mapped,
                                          FinalStatus = a.FinalStatus,
                                          Process_Date = a.Process_Date,
                                          Process_User = a.Process_User,
                                          TotalRecords = total,
                                          Supplier = s.Name,
                                          Entity = f.Entity,
                                          FileName = f.OriginalFilePath,
                                          ProcessedBy = f.PROCESS_USER
                                      }).Skip(skip ?? 0).Take(RQ.PageSize ?? total).ToList();

                    return StatResult;
                }
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus
                {
                    ErrorMessage = "Error while searching Statistics",
                    ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError
                });
            }
        }

        public DataContracts.DC_Message AddStaticDataUploadStatistics(DataContracts.UploadStaticData.DC_SupplierImportFile_Statistics obj)
        {
            DataContracts.DC_Message dc = new DataContracts.DC_Message();

            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {

                    if (obj.SupplierImportFile_Id != null)
                    {
                        DataContracts.UploadStaticData.DC_SupplierImportFile_Statistics_RQ RQ = new DataContracts.UploadStaticData.DC_SupplierImportFile_Statistics_RQ();
                        RQ.SupplierImportFile_Id = obj.SupplierImportFile_Id;
                        RQ.PageNo = 0;
                        RQ.PageSize = int.MaxValue;
                        List<DataContracts.UploadStaticData.DC_SupplierImportFile_Statistics> resstat = new List<DataContracts.UploadStaticData.DC_SupplierImportFile_Statistics>();
                        resstat = GetStaticDataUploadStatistics(RQ);

                        //DataContracts.UploadStaticData.DC_SupplierImportFile_Statistics stat = resstat.FirstOrDefault();
                        var stat = (from a in context.SupplierImportFile_Statistics
                                    where a.SupplierImportFile_Id == obj.SupplierImportFile_Id
                                    select a).FirstOrDefault();

                        if (stat != null)
                        {
                            int mapped = 0;
                            int unmapped = 0;
                            if (resstat[0].Entity.Trim().ToUpper() == "COUNTRY")
                            {
                                var getcount = from m in context.m_CountryMapping
                                               join j in context.STG_Mapping_TableIds on m.CountryMapping_Id equals j.Mapping_Id
                                               where j.File_Id == obj.SupplierImportFile_Id
                                               group m by new { m.Status } into g
                                               select new { g.Key.Status, count = g.Count() };

                                stat.FinalStatus = (from a in context.SupplierImportFileDetails where a.SupplierImportFile_Id == obj.SupplierImportFile_Id select a.STATUS).FirstOrDefault();
                                //stat.TotalRows = obj.TotalRows;
                                //stat.Unmapped = obj.Unmapped;
                                //stat.Mapped = obj.Mapped;
                                stat.Unmapped = getcount.Where(g => g.Status.Trim().ToUpper() == "UNMAPPED").Select(a => a.count).FirstOrDefault();
                                stat.Mapped = getcount.Where(g => g.Status.Trim().ToUpper() == "MAPPED" || g.Status.Trim().ToUpper() == "REVIEW").Select(a => a.count).FirstOrDefault();
                            }
                            else if (resstat[0].Entity.Trim().ToUpper() == "CITY")
                            {
                                var getcount = from m in context.m_CityMapping
                                               join j in context.STG_Mapping_TableIds on m.CityMapping_Id equals j.Mapping_Id
                                               where j.File_Id == obj.SupplierImportFile_Id
                                               group m by new { m.Status } into g
                                               select new { g.Key.Status, count = g.Count() };

                                stat.FinalStatus = (from a in context.SupplierImportFileDetails where a.SupplierImportFile_Id == obj.SupplierImportFile_Id select a.STATUS).FirstOrDefault();
                                //stat.TotalRows = obj.TotalRows;
                                stat.Unmapped = getcount.Where(g => g.Status.Trim().ToUpper() == "UNMAPPED").Select(a => a.count).FirstOrDefault();
                                stat.Mapped = getcount.Where(g => g.Status.Trim().ToUpper() == "MAPPED" || g.Status.Trim().ToUpper() == "REVIEW").Select(a => a.count).FirstOrDefault();
                            }
                            else if (resstat[0].Entity.Trim().ToUpper() == "HOTEL")
                            {
                                var getcount = from m in context.Accommodation_ProductMapping
                                               join j in context.STG_Mapping_TableIds on m.Accommodation_ProductMapping_Id equals j.Mapping_Id
                                               where j.File_Id == obj.SupplierImportFile_Id
                                               group m by new { m.Status } into g
                                               select new { g.Key.Status, count = g.Count() };

                                stat.FinalStatus = (from a in context.SupplierImportFileDetails where a.SupplierImportFile_Id == obj.SupplierImportFile_Id select a.STATUS).FirstOrDefault();
                                //stat.TotalRows = obj.TotalRows;
                                stat.Unmapped = getcount.Where(g => g.Status.Trim().ToUpper() == "UNMAPPED").Select(a => a.count).FirstOrDefault();
                                stat.Mapped = getcount.Where(g => g.Status.Trim().ToUpper() == "MAPPED" || g.Status.Trim().ToUpper() == "REVIEW").Select(a => a.count).FirstOrDefault();
                            }
                            else if (resstat[0].Entity.Trim().ToUpper() == "ROOMTYPE")
                            {
                                var getcount = from m in context.Accommodation_SupplierRoomTypeMapping
                                               join j in context.STG_Mapping_TableIds on m.Accommodation_SupplierRoomTypeMapping_Id equals j.Mapping_Id
                                               where j.File_Id == obj.SupplierImportFile_Id
                                               group m by new { m.MappingStatus } into g
                                               select new { g.Key.MappingStatus, count = g.Count() };

                                stat.FinalStatus = (from a in context.SupplierImportFileDetails where a.SupplierImportFile_Id == obj.SupplierImportFile_Id select a.STATUS).FirstOrDefault();
                                //stat.TotalRows = obj.TotalRows;
                                stat.Unmapped = getcount.Where(g => g.MappingStatus.Trim().ToUpper() == "UNMAPPED").Select(a => a.count).FirstOrDefault();
                                stat.Mapped = getcount.Where(g => g.MappingStatus.Trim().ToUpper() == "MAPPED" || g.MappingStatus.Trim().ToUpper() == "REVIEW").Select(a => a.count).FirstOrDefault();
                            }
                        }
                        else
                        {
                            DataLayer.SupplierImportFile_Statistics newObj = new DataLayer.SupplierImportFile_Statistics()
                            {
                                SupplierImportFile_Statistics_Id = Guid.NewGuid(),
                                SupplierImportFile_Id = obj.SupplierImportFile_Id,
                                FinalStatus = obj.FinalStatus,
                                TotalRows = obj.TotalRows,
                                Mapped = obj.Mapped,
                                Unmapped = obj.Unmapped,
                                Process_Date = DateTime.Now,
                                Process_User = obj.Process_User
                            };
                            context.SupplierImportFile_Statistics.Add(newObj);
                        }
                    }
                    context.SaveChanges();
                    dc.StatusCode = ReadOnlyMessage.StatusCode.Success;
                    dc.StatusMessage = "Process Log " + ReadOnlyMessage.strAddedSuccessfully;

                }
                return dc;

            }
            catch (Exception e)
            {
                dc.StatusMessage = ReadOnlyMessage.strFailed;
                dc.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                return dc;
            }
        }
        public List<DataContracts.UploadStaticData.DC_SupplierImportFile_Progress> GetStaticDataUploadProcessLog(DataContracts.UploadStaticData.DC_SupplierImportFile_Progress_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var ProgLogSearch = from a in context.SupplierImportFile_Progress select a;

                    //if (RQ.SupplierImportFile_Id.HasValue)
                    if (!string.IsNullOrWhiteSpace(RQ.SupplierImportFile_Id))
                    {
                        Guid fileId = new Guid();
                        if (Guid.TryParse(RQ.SupplierImportFile_Id, out fileId))
                        {
                            ProgLogSearch = from a in ProgLogSearch
                                            where a.SupplierImportFile_Id == fileId //RQ.SupplierImportFile_Id
                                            select a;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(RQ.Step))
                    {
                        ProgLogSearch = from a in ProgLogSearch
                                        where a.Step.Trim().ToUpper() == RQ.Step.Trim().ToUpper()
                                        select a;
                    }
                    if (!string.IsNullOrWhiteSpace(RQ.Status))
                    {
                        ProgLogSearch = from a in ProgLogSearch
                                        where a.Status.Trim().ToUpper() == RQ.Status.Trim().ToUpper()
                                        select a;
                    }
                    if (!string.IsNullOrWhiteSpace(RQ.StatusExcept))
                    {
                        ProgLogSearch = from a in ProgLogSearch
                                        where a.Status.Trim().ToUpper() != RQ.StatusExcept.Trim().ToUpper()
                                        select a;
                    }

                    var total = ProgLogSearch.Count();

                    var ProgLogResult = (from a in ProgLogSearch
                                         select new DataContracts.UploadStaticData.DC_SupplierImportFile_Progress
                                         {
                                             SupplierImportFileProgress_Id = a.SupplierImportFileProgress_Id,
                                             SupplierImportFile_Id = a.SupplierImportFile_Id,
                                             Step = a.Step,
                                             Status = a.Status,
                                             PercentageValue = a.PercentageValue,
                                             TotalCount = total,
                                             CurrentBatch = a.CurrentBatch ?? 0,
                                             TotalBatch = a.TotalBatch ?? 0
                                         }).ToList();

                    return ProgLogResult;
                }
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus
                {
                    ErrorMessage = "Error while searching Progress Log",
                    ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError
                });
            }
        }
        public DataContracts.DC_Message AddStaticDataUploadVerboseLog(DataContracts.UploadStaticData.DC_SupplierImportFile_VerboseLog obj)
        {
            DataContracts.DC_Message dc = new DataContracts.DC_Message();

            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    DataLayer.SupplierImportFile_VerboseLog objNew = new DataLayer.SupplierImportFile_VerboseLog()
                    {
                        SupplierImportFile_VerboseLog_Id = obj.SupplierImportFile_VerboseLog_Id,
                        SupplierImportFile_Id = obj.SupplierImportFile_Id,
                        Message = obj.Message,
                        Step = obj.Step,
                        TimeStamp = obj.TimeStamp
                    };
                    context.SupplierImportFile_VerboseLog.Add(objNew);
                    context.SaveChanges();
                    dc.StatusCode = ReadOnlyMessage.StatusCode.Success;
                    dc.StatusMessage = "Verbose Log " + ReadOnlyMessage.strAddedSuccessfully;
                }
                return dc;
            }
            catch (Exception e)
            {
                dc.StatusMessage = ReadOnlyMessage.strFailed;
                dc.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                return dc;
            }
        }
        public List<DataContracts.UploadStaticData.DC_SupplierImportFile_VerboseLog> GetStaticDataUploadVerboseLog(DataContracts.UploadStaticData.DC_SupplierImportFile_VerboseLog_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var ProgLogSearch = from a in context.SupplierImportFile_VerboseLog select a;

                    if (RQ.SupplierImportFile_VerboseLog_Id.HasValue)
                    {
                        ProgLogSearch = from a in ProgLogSearch
                                        where a.SupplierImportFile_VerboseLog_Id == RQ.SupplierImportFile_VerboseLog_Id
                                        select a;
                    }
                    if (RQ.SupplierImportFile_Id.HasValue)
                    {
                        ProgLogSearch = from a in ProgLogSearch
                                        where a.SupplierImportFile_Id == RQ.SupplierImportFile_Id
                                        select a;
                    }
                    if (!string.IsNullOrWhiteSpace(RQ.Step))
                    {
                        ProgLogSearch = from a in ProgLogSearch
                                        where a.Step.Trim().ToUpper() == RQ.Step.Trim().ToUpper()
                                        select a;
                    }

                    var total = ProgLogSearch.Count();

                    var skip = RQ.PageSize * RQ.PageNo;

                    var ProgLogResult = (from a in ProgLogSearch
                                         orderby a.TimeStamp descending
                                         select new DataContracts.UploadStaticData.DC_SupplierImportFile_VerboseLog
                                         {
                                             SupplierImportFile_VerboseLog_Id = a.SupplierImportFile_VerboseLog_Id,
                                             SupplierImportFile_Id = a.SupplierImportFile_Id,
                                             Step = a.Step,
                                             Message = a.Message,
                                             TimeStamp = a.TimeStamp ?? DateTime.Now,
                                             TotalCount = total
                                         }).Skip(skip ?? 0).Take(RQ.PageSize ?? total).ToList();

                    return ProgLogResult;
                }
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus
                {
                    ErrorMessage = "Error while searching Progress Log",
                    ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError
                });
            }
        }
        #endregion

        #region "STG Tables"
        public DataContracts.DC_Message AddSTGCountryData(List<DataContracts.STG.DC_stg_SupplierCountryMapping> lstobj)
        {
            DataContracts.DC_Message dc = new DataContracts.DC_Message();

            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    string mySupplier = lstobj[0].SupplierName;

                    if (!string.IsNullOrWhiteSpace(mySupplier))
                    {
                        var oldRecords = (from y in context.stg_SupplierCountryMapping
                                          where y.SupplierName.Trim().ToUpper() == mySupplier.Trim().ToUpper()
                                          select y).ToList();
                        context.stg_SupplierCountryMapping.RemoveRange(oldRecords);
                        context.SaveChanges();
                        foreach (DataContracts.STG.DC_stg_SupplierCountryMapping obj in lstobj)
                        {
                            var search = (from a in context.stg_SupplierCountryMapping
                                          where a.SupplierName.Trim().ToUpper() == mySupplier.Trim().ToUpper()
                                          && ((a.CountryCode != null && a.CountryCode.Trim().ToUpper() == obj.CountryCode.Trim().ToUpper()) || a.CountryCode == null)
                                          && a.CountryName.Trim().ToUpper() == obj.CountryName.Trim().ToUpper()
                                          select a).FirstOrDefault();
                            if (search == null)
                            {
                                DataLayer.stg_SupplierCountryMapping objNew = new DataLayer.stg_SupplierCountryMapping();
                                objNew.stg_Country_Id = obj.stg_Country_Id;
                                objNew.SupplierId = obj.SupplierId;
                                objNew.SupplierName = obj.SupplierName;
                                objNew.CountryCode = obj.CountryCode;
                                objNew.CountryName = obj.CountryName;
                                objNew.InsertDate = obj.InsertDate;
                                objNew.ActiveFrom = obj.ActiveFrom;
                                objNew.ActiveTo = obj.ActiveTo;
                                objNew.Action = obj.Action;
                                objNew.UpdateType = obj.UpdateType;
                                objNew.ActionText = obj.ActionText;
                                objNew.Latitude = obj.Latitude;
                                objNew.Longitude = obj.Longitude;
                                context.stg_SupplierCountryMapping.Add(objNew);
                                context.SaveChanges();
                            }
                        }
                        dc.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        dc.StatusMessage = "Country Static Data " + ReadOnlyMessage.strAddedSuccessfully;
                    }
                    else
                    {
                        dc.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                        dc.StatusMessage = "Supplier Not Found";
                    }
                }
                return dc;
            }
            catch (Exception e)
            {
                dc.StatusMessage = ReadOnlyMessage.strFailed;
                dc.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                return dc;
            }

        }

        public List<DataContracts.STG.DC_stg_SupplierCountryMapping> GetSTGCountryData(DataContracts.STG.DC_stg_SupplierCountryMapping_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var stgSearch = from a in context.stg_SupplierCountryMapping select a;

                    if (RQ.stg_Country_Id.HasValue)
                    {
                        stgSearch = from a in stgSearch
                                    where a.stg_Country_Id == RQ.stg_Country_Id
                                    select a;
                    }

                    if (!(RQ.Supplier_Id == Guid.Empty))
                    {
                        stgSearch = from a in stgSearch
                                    join s in context.Suppliers on a.SupplierName.Trim().TrimStart().ToUpper() equals s.Name.Trim().TrimStart().ToUpper()
                                    where s.Supplier_Id == RQ.Supplier_Id
                                    select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.SupplierName))
                    {
                        stgSearch = from a in stgSearch
                                    where a.SupplierName.Trim().TrimStart().ToUpper() == RQ.SupplierName.Trim().TrimStart().ToUpper()
                                    select a;
                    }


                    if (!string.IsNullOrWhiteSpace(RQ.CountryCode))
                    {
                        stgSearch = from a in stgSearch
                                    where a.CountryCode.Trim().TrimStart().ToUpper() == RQ.CountryCode.Trim().TrimStart().ToUpper()
                                    select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.CountryName))
                    {
                        stgSearch = from a in stgSearch
                                    where a.CountryName.Trim().TrimStart().ToUpper() == RQ.CountryName.Trim().TrimStart().ToUpper()
                                    select a;
                    }
                    int total;

                    total = stgSearch.Count();

                    var skip = RQ.PageSize * RQ.PageNo;

                    var stgResult = (from a in stgSearch
                                     orderby a.SupplierName, a.CountryName
                                     select new DataContracts.STG.DC_stg_SupplierCountryMapping
                                     {
                                         stg_Country_Id = a.stg_Country_Id,
                                         ActiveFrom = a.ActiveFrom,
                                         ActiveTo = a.ActiveTo,
                                         CountryCode = a.CountryCode,
                                         CountryName = a.CountryName,
                                         InsertDate = a.InsertDate,
                                         SupplierId = a.SupplierId,
                                         SupplierName = a.SupplierName,
                                         TotalRecords = total,
                                         Latitude = a.Latitude,
                                         Longitude = a.Longitude
                                     }
                                        ).Skip(skip).Take(RQ.PageSize).ToList();

                    return stgResult;

                }
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus
                {
                    ErrorMessage = "Error while searching Supplier Data",
                    ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError
                });
            }
        }

        public List<DataContracts.STG.DC_stg_SupplierCityMapping> GetSTGCityData(DataContracts.STG.DC_stg_SupplierCityMapping_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var stgSearch = from a in context.stg_SupplierCityMapping select a;

                    if (RQ.stg_City_Id.HasValue)
                    {
                        stgSearch = from a in stgSearch
                                    where a.stg_City_Id == RQ.stg_City_Id
                                    select a;
                    }

                    if (!(RQ.Supplier_Id == Guid.Empty))
                    {
                        stgSearch = from a in stgSearch
                                    join s in context.Suppliers on a.SupplierName.Trim().TrimStart().ToUpper() equals s.Name.Trim().TrimStart().ToUpper()
                                    where s.Supplier_Id == RQ.Supplier_Id
                                    select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.SupplierName))
                    {
                        stgSearch = from a in stgSearch
                                    where a.SupplierName.Trim().TrimStart().ToUpper() == RQ.SupplierName.Trim().TrimStart().ToUpper()
                                    select a;
                    }


                    if (!string.IsNullOrWhiteSpace(RQ.CountryCode))
                    {
                        stgSearch = from a in stgSearch
                                    where a.CountryCode.Trim().TrimStart().ToUpper() == RQ.CountryCode.Trim().TrimStart().ToUpper()
                                    select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.CountryName))
                    {
                        stgSearch = from a in stgSearch
                                    where a.CountryName.Trim().TrimStart().ToUpper() == RQ.CountryName.Trim().TrimStart().ToUpper()
                                    select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.StateCode))
                    {
                        stgSearch = from a in stgSearch
                                    where a.StateCode.Trim().TrimStart().ToUpper() == RQ.StateCode.Trim().TrimStart().ToUpper()
                                    select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.StateName))
                    {
                        stgSearch = from a in stgSearch
                                    where a.StateName.Trim().TrimStart().ToUpper() == RQ.StateName.Trim().TrimStart().ToUpper()
                                    select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.CityCode))
                    {
                        stgSearch = from a in stgSearch
                                    where a.CityCode.Trim().TrimStart().ToUpper() == RQ.CityCode.Trim().TrimStart().ToUpper()
                                    select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.CityName))
                    {
                        stgSearch = from a in stgSearch
                                    where a.CityName.Trim().TrimStart().ToUpper() == RQ.CityName.Trim().TrimStart().ToUpper()
                                    select a;
                    }
                    int total;

                    total = stgSearch.Count();

                    var skip = RQ.PageSize * RQ.PageNo;

                    var stgResult = (from a in stgSearch
                                     orderby a.SupplierName, a.CountryName
                                     select new DataContracts.STG.DC_stg_SupplierCityMapping
                                     {
                                         stg_City_Id = a.stg_City_Id,
                                         CityCode = a.CityCode,
                                         CityName = a.CityName,
                                         StateCode = a.StateCode,
                                         StateName = a.StateName,
                                         Insert_Date = a.Insert_Date,
                                         ActiveFrom = a.ActiveFrom,
                                         ActiveTo = a.ActiveTo,
                                         CountryCode = a.CountryCode,
                                         CountryName = a.CountryName,
                                         SupplierId = a.SupplierId,
                                         SupplierName = a.SupplierName,
                                         TotalRecords = total,
                                         Latitude = a.Latitude,
                                         Longitude = a.Longitude,
                                         Country_Id = a.Country_Id,
                                         Supplier_Id = a.Supplier_Id
                                     }
                                        ).Skip(skip).Take(RQ.PageSize).ToList();

                    return stgResult;

                }
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus
                {
                    ErrorMessage = "Error while searching Supplier Data",
                    ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError
                });
            }
        }

        public DataContracts.DC_Message AddSTGCityData(List<DataContracts.STG.DC_stg_SupplierCityMapping> lstobj)
        {
            DataContracts.DC_Message dc = new DataContracts.DC_Message();

            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    string mySupplier = lstobj[0].SupplierName;
                    Guid? mySupplier_Id = lstobj[0].Supplier_Id;
                    if (!string.IsNullOrWhiteSpace(mySupplier))
                    {
                        var oldRecords = (from y in context.stg_SupplierCityMapping
                                          where y.SupplierName.Trim().ToUpper() == mySupplier.Trim().ToUpper()
                                          select y).ToList();
                        context.stg_SupplierCityMapping.RemoveRange(oldRecords);
                        context.SaveChanges();
                        List<DataContracts.STG.DC_stg_SupplierCityMapping> dstobj = new List<DC_stg_SupplierCityMapping>();
                        dstobj = lstobj.GroupBy(a => new { a.CityCode, a.CityName, a.CountryCode, a.CountryName, a.StateCode, a.StateName }).Select(grp => grp.First()).ToList();

                        var geo = (from g in context.m_CountryMapping
                                       //join d in dstobj on new { g.Supplier_Id, g.CityName } equals new { d.Supplier_Id, d.CityName }
                                   where g.Supplier_Id == mySupplier_Id
                                   select new
                                   {
                                       g.Country_Id,//Country_Id = Guid.Parse(g.Country_Id.ToString()),
                                       g.CountryName,//CountryName = g.CountryName
                                       g.CountryCode
                                   }
                                 ).Distinct().ToList();

                        foreach (DataContracts.STG.DC_stg_SupplierCityMapping obj in dstobj)
                        {
                            //var search = (from a in context.stg_SupplierCityMapping
                            //              where a.SupplierName.Trim().ToUpper() == mySupplier.Trim().ToUpper()
                            //              && ((a.CityCode != null && a.CityCode.Trim().ToUpper() == obj.CityCode.Trim().ToUpper()) || a.CityCode == null)
                            //              && a.CityName.Trim().ToUpper() == obj.CityName.Trim().ToUpper()
                            //              && a.CountryName.Trim().ToUpper() == obj.CountryName.Trim().ToUpper()
                            //              select a).FirstOrDefault();
                            //if (search == null)
                            //{
                            DataLayer.stg_SupplierCityMapping objNew = new DataLayer.stg_SupplierCityMapping();
                            objNew.stg_City_Id = Guid.NewGuid(); //obj.stg_City_Id;
                            objNew.SupplierId = obj.SupplierId;
                            objNew.SupplierName = obj.SupplierName;
                            objNew.CountryCode = obj.CountryCode;
                            objNew.CountryName = obj.CountryName;
                            objNew.Insert_Date = obj.Insert_Date;
                            objNew.CityCode = obj.CityCode;
                            objNew.CityName = obj.CityName;
                            objNew.StateCode = obj.StateCode;
                            objNew.StateName = obj.StateName;
                            objNew.ActiveFrom = obj.ActiveFrom;
                            objNew.ActiveTo = obj.ActiveTo;
                            objNew.Action = obj.Action;
                            objNew.UpdateType = obj.UpdateType;
                            objNew.ActionText = obj.ActionText;
                            objNew.Latitude = obj.Latitude;
                            objNew.Longitude = obj.Longitude;
                            objNew.Country_Id =
                                ((geo.Where(s => s.CountryName == obj.CountryName).Select(s1 => s1.Country_Id).FirstOrDefault())) ??
                                (geo.Where(s => s.CountryCode == obj.CountryCode).Select(s1 => s1.Country_Id).FirstOrDefault());
                            objNew.Supplier_Id = mySupplier_Id;
                            context.stg_SupplierCityMapping.Add(objNew);

                            //}
                        }
                        context.SaveChanges();
                        dc.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        dc.StatusMessage = "City Static Data " + ReadOnlyMessage.strAddedSuccessfully;
                    }
                    else
                    {
                        dc.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                        dc.StatusMessage = "Supplier Not Found";
                    }
                }
                return dc;
            }
            catch (Exception e)
            {
                dc.StatusMessage = ReadOnlyMessage.strFailed;
                dc.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                return dc;
            }

        }

        public DataContracts.DC_Message AddSTGProductData(List<DataContracts.STG.DC_stg_SupplierProductMapping> lstobj)
        {
            DataContracts.DC_Message dc = new DataContracts.DC_Message();

            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    string mySupplier = lstobj[0].SupplierName;
                    Guid? mySupplier_Id = lstobj[0].Supplier_Id;

                    if (!string.IsNullOrWhiteSpace(mySupplier))
                    {
                        var oldRecords = (from y in context.stg_SupplierProductMapping
                                          where y.SupplierName.Trim().ToUpper() == mySupplier.Trim().ToUpper()
                                          select y).ToList();
                        context.stg_SupplierProductMapping.RemoveRange(oldRecords);
                        context.SaveChanges();
                        List<DataContracts.STG.DC_stg_SupplierProductMapping> dstobj = new List<DC_stg_SupplierProductMapping>();
                        dstobj = lstobj.GroupBy(a => new { a.CityCode, a.CityName, a.CountryCode, a.CountryName, a.StateCode, a.StateName, a.ProductId, a.ProductName, a.PostalCode }).Select(grp => grp.First()).ToList();

                        var geo = (from g in context.m_CityMapping
                                   join c in context.m_CountryMapping on new { g.Supplier_Id, g.Country_Id } equals new { c.Supplier_Id, c.Country_Id }
                                   //join d in dstobj on new { g.Supplier_Id, g.CityName } equals new { d.Supplier_Id, d.CityName }
                                   where g.Supplier_Id == mySupplier_Id
                                   select new
                                   {
                                       g.City_Id,//City_Id = Guid.Parse(g.City_Id.ToString()),
                                       g.CityName, //Name = g.CityName,
                                       g.CityCode,
                                       c.Country_Id,//Country_Id = Guid.Parse(g.Country_Id.ToString()),
                                       c.CountryName,//CountryName = g.CountryName
                                       c.CountryCode
                                   }
                                  ).Distinct().ToList();

                        foreach (DataContracts.STG.DC_stg_SupplierProductMapping obj in dstobj)
                        {
                            DataLayer.stg_SupplierProductMapping objNew = new DataLayer.stg_SupplierProductMapping();
                            objNew.stg_AccoMapping_Id = obj.stg_AccoMapping_Id;
                            objNew.SupplierId = obj.SupplierId;
                            objNew.SupplierName = obj.SupplierName;
                            objNew.ProductId = obj.ProductId;
                            objNew.ProductName = obj.ProductName;
                            objNew.Address = obj.Address;
                            objNew.TelephoneNumber = obj.TelephoneNumber;
                            objNew.CountryCode = obj.CountryCode;
                            objNew.CountryName = obj.CountryName ??
                                (geo.Where(s => s.CountryCode == obj.CountryCode).Select(s1 => s1.CountryName).FirstOrDefault());
                            objNew.CityCode = obj.CityCode;
                            objNew.CityName = obj.CityName;
                            objNew.Location = obj.Location;
                            objNew.InsertDate = obj.InsertDate;
                            objNew.StreetName = obj.StreetName;
                            objNew.Street2 = obj.Street2;
                            objNew.PostalCode = obj.PostalCode;
                            objNew.Street3 = obj.Street3;
                            objNew.Street4 = obj.Street4;
                            objNew.Street5 = obj.Street5;
                            objNew.StateCode = obj.StateCode;
                            objNew.StateName = obj.StateName;
                            objNew.Latitude = obj.Latitude;
                            objNew.Longitude = obj.Longitude;
                            objNew.Fax = obj.Fax;
                            objNew.Email = obj.Email;
                            objNew.Website = obj.Website;
                            objNew.StreetNo = obj.StreetNo;
                            objNew.TX_COUNTRYNAME = obj.TX_COUNTRYNAME;
                            objNew.ActiveFrom = obj.ActiveFrom;
                            objNew.ActiveTo = obj.ActiveTo;
                            objNew.Action = obj.Action;
                            objNew.UpdateType = obj.UpdateType;
                            objNew.ActionText = obj.ActionText;
                            objNew.StarRating = obj.StarRating;
                            objNew.Country_Id =
                                ((geo.Where(s => s.CountryName == obj.CountryName).Select(s1 => s1.Country_Id).FirstOrDefault())) ??
                                (geo.Where(s => s.CountryCode == obj.CountryCode).Select(s1 => s1.Country_Id).FirstOrDefault());
                            objNew.City_Id =
                                ((geo.Where(s => s.CityName == obj.CityName).Select(s1 => s1.City_Id).FirstOrDefault())) ??
                                (geo.Where(s => s.CityCode == obj.CityCode).Select(s1 => s1.City_Id).FirstOrDefault());
                            objNew.Supplier_Id = obj.Supplier_Id;
                            context.stg_SupplierProductMapping.Add(objNew);
                        }
                        context.SaveChanges();
                        dc.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        dc.StatusMessage = "Product Static Data " + ReadOnlyMessage.strAddedSuccessfully;
                    }
                    else
                    {
                        dc.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                        dc.StatusMessage = "Supplier Not Found";
                    }
                }
                return dc;
            }
            catch (Exception e)
            {
                dc.StatusMessage = ReadOnlyMessage.strFailed;
                dc.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                return dc;
            }

        }

        public DataContracts.DC_Message AddSTGRoomTypeData(List<DataContracts.STG.DC_stg_SupplierHotelRoomMapping> lstobj)
        {
            DataContracts.DC_Message dc = new DataContracts.DC_Message();

            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    string mySupplier = lstobj[0].SupplierName;
                    Guid? mySupplier_Id = lstobj[0].Supplier_Id;

                    if (!string.IsNullOrWhiteSpace(mySupplier))
                    {
                        var oldRecords = (from y in context.stg_SupplierHotelRoomMapping
                                          where y.SupplierName.Trim().ToUpper() == mySupplier.Trim().ToUpper()
                                          select y).ToList();
                        context.stg_SupplierHotelRoomMapping.RemoveRange(oldRecords);
                        context.SaveChanges();
                        List<DataContracts.STG.DC_stg_SupplierHotelRoomMapping> dstobj = new List<DC_stg_SupplierHotelRoomMapping>();
                        dstobj = lstobj.GroupBy(a => new { a.RoomName, a.SupplierID, a.SupplierName, a.SupplierProductId, a.SupplierProductName, a.SupplierRoomCategory, a.SupplierRoomTypeCode }).Select(grp => grp.First()).ToList();

                        foreach (DataContracts.STG.DC_stg_SupplierHotelRoomMapping obj in dstobj)
                        {
                            DataLayer.stg_SupplierHotelRoomMapping objNew = new DataLayer.stg_SupplierHotelRoomMapping()
                            {
                                stg_SupplierHotelRoomMapping_Id = obj.stg_SupplierHotelRoomMapping_Id,
                                Amenities = obj.Amenities,
                                BathRoomType = obj.BathRoomType,
                                SupplierRoomTypeCode = obj.SupplierRoomTypeCode,
                                SupplierRoomCategory = obj.SupplierRoomCategory,
                                BeddingConfig = obj.BeddingConfig,
                                Bedrooms = obj.Bedrooms,
                                BedTypeCode = obj.BedTypeCode,
                                ChildAge = obj.ChildAge,
                                ExtraBed = obj.ExtraBed,
                                FloorName = obj.FloorName,
                                FloorNumber = obj.FloorNumber,
                                MaxAdults = obj.MaxAdults,
                                MaxChild = obj.MaxChild,
                                MaxGuestOccupancy = obj.MaxGuestOccupancy,
                                MaxInfant = obj.MaxInfant,
                                PromotionalVendorCode = obj.PromotionalVendorCode,
                                Quantity = obj.Quantity,
                                RatePlan = obj.RatePlan,
                                RatePlanCode = obj.RatePlanCode,
                                RoomDescription = obj.RoomDescription,
                                RoomLocationCode = obj.RoomLocationCode,
                                RoomName = obj.RoomName,
                                RoomSize = obj.RoomSize,
                                RoomViewCode = obj.RoomViewCode,
                                Smoking = obj.Smoking,
                                SupplierID = obj.SupplierID,
                                SupplierName = obj.SupplierName,
                                SupplierProductId = obj.SupplierProductId,
                                SupplierProductName = obj.SupplierProductName,
                                SupplierProvider = obj.SupplierProvider,
                                SupplierRoomCategoryId = obj.SupplierRoomCategoryId,
                                SupplierRoomId = obj.SupplierRoomId,
                                Supplier_Id = obj.Supplier_Id,
                                TX_RoomName = obj.TX_RoomName
                            };
                            context.stg_SupplierHotelRoomMapping.Add(objNew);
                        }
                        context.SaveChanges();
                        dc.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        dc.StatusMessage = "Product Static Data " + ReadOnlyMessage.strAddedSuccessfully;
                    }
                    else
                    {
                        dc.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                        dc.StatusMessage = "Supplier Not Found";
                    }
                }
                return dc;
            }
            catch (Exception e)
            {
                dc.StatusMessage = ReadOnlyMessage.strFailed;
                dc.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                return dc;
            }
        }

        public List<DC_stg_SupplierProductMapping> GetSTGHotelData(DC_stg_SupplierProductMapping_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var stgSearch = from a in context.stg_SupplierProductMapping select a;

                    if (RQ.stg_AccoMapping_Id.HasValue)
                    {
                        stgSearch = from a in stgSearch
                                    where a.stg_AccoMapping_Id == RQ.stg_AccoMapping_Id
                                    select a;
                    }

                    if (!(RQ.Supplier_Id == Guid.Empty))
                    {
                        stgSearch = from a in stgSearch
                                    join s in context.Suppliers on a.SupplierName.Trim().TrimStart().ToUpper() equals s.Name.Trim().TrimStart().ToUpper()
                                    where s.Supplier_Id == RQ.Supplier_Id
                                    select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.SupplierName))
                    {
                        stgSearch = from a in stgSearch
                                    where a.SupplierName.Trim().TrimStart().ToUpper() == RQ.SupplierName.Trim().TrimStart().ToUpper()
                                    select a;
                    }


                    if (!string.IsNullOrWhiteSpace(RQ.CountryCode))
                    {
                        stgSearch = from a in stgSearch
                                    where a.CountryCode.Trim().TrimStart().ToUpper() == RQ.CountryCode.Trim().TrimStart().ToUpper()
                                    select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.CountryName))
                    {
                        stgSearch = from a in stgSearch
                                    where a.CountryName.Trim().TrimStart().ToUpper() == RQ.CountryName.Trim().TrimStart().ToUpper()
                                    select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.StateCode))
                    {
                        stgSearch = from a in stgSearch
                                    where a.StateCode.Trim().TrimStart().ToUpper() == RQ.StateCode.Trim().TrimStart().ToUpper()
                                    select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.StateName))
                    {
                        stgSearch = from a in stgSearch
                                    where a.StateName.Trim().TrimStart().ToUpper() == RQ.StateName.Trim().TrimStart().ToUpper()
                                    select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.CityCode))
                    {
                        stgSearch = from a in stgSearch
                                    where a.CityCode.Trim().TrimStart().ToUpper() == RQ.CityCode.Trim().TrimStart().ToUpper()
                                    select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.CityName))
                    {
                        stgSearch = from a in stgSearch
                                    where a.CityName.Trim().TrimStart().ToUpper() == RQ.CityName.Trim().TrimStart().ToUpper()
                                    select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.ProductId))
                    {
                        stgSearch = from a in stgSearch
                                    where a.ProductId.Trim().TrimStart().ToUpper() == RQ.ProductId.Trim().TrimStart().ToUpper()
                                    select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.ProductName))
                    {
                        stgSearch = from a in stgSearch
                                    where a.ProductName.Trim().TrimStart().ToUpper() == RQ.ProductName.Trim().TrimStart().ToUpper()
                                    select a;
                    }
                    if (!string.IsNullOrWhiteSpace(RQ.Latitude))
                    {
                        stgSearch = from a in stgSearch
                                    where a.Latitude.Trim().TrimStart().ToUpper() == RQ.Latitude.Trim().TrimStart().ToUpper()
                                    select a;
                    }
                    if (!string.IsNullOrWhiteSpace(RQ.Longitude))
                    {
                        stgSearch = from a in stgSearch
                                    where a.Longitude.Trim().TrimStart().ToUpper() == RQ.Longitude.Trim().TrimStart().ToUpper()
                                    select a;
                    }
                    if (!string.IsNullOrWhiteSpace(RQ.StarRating))
                    {
                        stgSearch = from a in stgSearch
                                    where a.StarRating.Trim().TrimStart().ToUpper() == RQ.StarRating.Trim().TrimStart().ToUpper()
                                    select a;
                    }


                    int total;

                    total = stgSearch.Count();

                    var skip = RQ.PageSize * RQ.PageNo;

                    var stgResult = (from a in stgSearch
                                     orderby a.SupplierName, a.CountryName
                                     select new DataContracts.STG.DC_stg_SupplierProductMapping
                                     {
                                         stg_AccoMapping_Id = a.stg_AccoMapping_Id,
                                         CityCode = a.CityCode,
                                         CityName = a.CityName,
                                         StateCode = a.StateCode,
                                         StateName = a.StateName,
                                         ActiveFrom = a.ActiveFrom,
                                         ActiveTo = a.ActiveTo,
                                         CountryCode = a.CountryCode,
                                         CountryName = a.CountryName,
                                         SupplierId = a.SupplierId,
                                         SupplierName = a.SupplierName,
                                         TotalRecords = total,
                                         Latitude = a.Latitude,
                                         Longitude = a.Longitude,
                                         Address = a.Address,
                                         Email = a.Email,
                                         Fax = a.Fax,
                                         InsertDate = a.InsertDate,
                                         Location = a.Location,
                                         PostalCode = a.PostalCode,
                                         ProductId = a.ProductId,
                                         ProductName = a.ProductName,
                                         StarRating = a.StarRating,
                                         Street2 = a.Street2,
                                         Street3 = a.Street3,
                                         Street4 = a.Street4,
                                         Street5 = a.Street5,
                                         StreetName = a.StreetName,
                                         StreetNo = a.StreetNo,
                                         TelephoneNumber = a.TelephoneNumber,
                                         TX_COUNTRYNAME = a.TX_COUNTRYNAME,
                                         Website = a.Website,
                                         Action = a.Action,
                                         ActionText = a.ActionText,
                                         UpdateType = a.UpdateType,
                                         Country_Id = a.Country_Id,
                                         City_Id = a.City_Id,
                                         Google_Place_Id = a.Google_Place_Id,
                                         Supplier_Id = a.Supplier_Id
                                     }
                                        ).Skip(skip).Take(RQ.PageSize).ToList();

                    return stgResult;

                }
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus
                {
                    ErrorMessage = "Error while searching Supplier Data",
                    ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError
                });
            }
        }

        public List<DC_stg_SupplierHotelRoomMapping> GetSTGRoomTypeData(DC_stg_SupplierHotelRoomMapping_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var stgSearch = from a in context.stg_SupplierHotelRoomMapping select a;

                    if (RQ.stg_SupplierHotelRoomMapping_Id.HasValue)
                    {
                        stgSearch = from a in stgSearch
                                    where a.stg_SupplierHotelRoomMapping_Id == RQ.stg_SupplierHotelRoomMapping_Id
                                    select a;
                    }

                    if (!(RQ.Supplier_Id == Guid.Empty))
                    {
                        stgSearch = from a in stgSearch
                                    join s in context.Suppliers on a.SupplierName.Trim().TrimStart().ToUpper() equals s.Name.Trim().TrimStart().ToUpper()
                                    where s.Supplier_Id == RQ.Supplier_Id
                                    select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.SupplierName))
                    {
                        stgSearch = from a in stgSearch
                                    where a.SupplierName.Trim().TrimStart().ToUpper() == RQ.SupplierName.Trim().TrimStart().ToUpper()
                                    select a;
                    }
                    if (!string.IsNullOrWhiteSpace(RQ.SupplierID))
                    {
                        stgSearch = from a in stgSearch
                                    where a.SupplierID.Trim().TrimStart().ToUpper() == RQ.SupplierID.Trim().TrimStart().ToUpper()
                                    select a;
                    }
                    if (!string.IsNullOrWhiteSpace(RQ.SupplierProductId))
                    {
                        stgSearch = from a in stgSearch
                                    where a.SupplierProductId.Trim().TrimStart().ToUpper() == RQ.SupplierProductId.Trim().TrimStart().ToUpper()
                                    select a;
                    }
                    if (!string.IsNullOrWhiteSpace(RQ.SupplierProductName))
                    {
                        stgSearch = from a in stgSearch
                                    where a.SupplierProductName.Trim().TrimStart().ToUpper() == RQ.SupplierProductName.Trim().TrimStart().ToUpper()
                                    select a;
                    }
                    if (!string.IsNullOrWhiteSpace(RQ.SupplierRoomId))
                    {
                        stgSearch = from a in stgSearch
                                    where a.SupplierRoomId.Trim().TrimStart().ToUpper() == RQ.SupplierRoomId.Trim().TrimStart().ToUpper()
                                    select a;
                    }
                    if (!string.IsNullOrWhiteSpace(RQ.SupplierRoomTypeCode))
                    {
                        stgSearch = from a in stgSearch
                                    where a.SupplierRoomTypeCode.Trim().TrimStart().ToUpper() == RQ.SupplierRoomTypeCode.Trim().TrimStart().ToUpper()
                                    select a;
                    }
                    if (!string.IsNullOrWhiteSpace(RQ.RoomName))
                    {
                        stgSearch = from a in stgSearch
                                    where a.RoomName.Trim().TrimStart().ToUpper() == RQ.RoomName.Trim().TrimStart().ToUpper()
                                    select a;
                    }
                    if (!string.IsNullOrWhiteSpace(RQ.SupplierRoomCategory))
                    {
                        stgSearch = from a in stgSearch
                                    where a.SupplierRoomCategory.Trim().TrimStart().ToUpper() == RQ.SupplierRoomCategory.Trim().TrimStart().ToUpper()
                                    select a;
                    }
                    if (!string.IsNullOrWhiteSpace(RQ.RoomName))
                    {
                        stgSearch = from a in stgSearch
                                    where a.SupplierRoomCategoryId.Trim().TrimStart().ToUpper() == RQ.SupplierRoomCategoryId.Trim().TrimStart().ToUpper()
                                    select a;
                    }

                    int total;

                    total = stgSearch.Count();

                    var skip = RQ.PageSize * RQ.PageNo;

                    var stgResult = (from a in stgSearch
                                     orderby a.SupplierName
                                     select new DataContracts.STG.DC_stg_SupplierHotelRoomMapping
                                     {
                                         Amenities = a.Amenities,
                                         BathRoomType = a.BathRoomType,
                                         SupplierID = a.SupplierID,
                                         stg_SupplierHotelRoomMapping_Id = a.stg_SupplierHotelRoomMapping_Id,
                                         BeddingConfig = a.BeddingConfig,
                                         Bedrooms = a.Bedrooms,
                                         BedTypeCode = a.BedTypeCode,
                                         ChildAge = a.ChildAge,
                                         ExtraBed = a.ExtraBed,
                                         FloorName = a.FloorName,
                                         FloorNumber = a.FloorNumber,
                                         MaxAdults = a.MaxAdults,
                                         MaxChild = a.MaxChild,
                                         MaxGuestOccupancy = a.MaxGuestOccupancy,
                                         MaxInfant = a.MaxInfant,
                                         PromotionalVendorCode = a.PromotionalVendorCode,
                                         Quantity = a.Quantity,
                                         RatePlan = a.RatePlan,
                                         RatePlanCode = a.RatePlanCode,
                                         RoomDescription = a.RoomDescription,
                                         RoomLocationCode = a.RoomLocationCode,
                                         RoomName = a.RoomName,
                                         RoomSize = a.RoomSize,
                                         RoomViewCode = a.RoomViewCode,
                                         Smoking = a.Smoking,
                                         SupplierName = a.SupplierName,
                                         SupplierProductId = a.SupplierProductId,
                                         SupplierProductName = a.SupplierProductName,
                                         SupplierProvider = a.SupplierProvider,
                                         SupplierRoomCategory = a.SupplierRoomCategory,
                                         SupplierRoomCategoryId = a.SupplierRoomCategoryId,
                                         SupplierRoomId = a.SupplierRoomId,
                                         SupplierRoomTypeCode = a.SupplierRoomTypeCode,
                                         Supplier_Id = a.Supplier_Id,
                                         TX_RoomName = a.TX_RoomName
                                     }
                                        ).Skip(skip).Take(RQ.PageSize).ToList();

                    return stgResult;
                }
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus
                {
                    ErrorMessage = "Error while searching Supplier Data",
                    ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError
                });
            }
        }
        #endregion

        #region Process Or Test Uploaded Files
        public DataContracts.DC_Message StaticFileUploadProcessFile(DataContracts.UploadStaticData.DC_SupplierImportFileDetails obj)
        {
            DataContracts.DC_Message dc = new DataContracts.DC_Message();
            try
            {
                DHSVC.DC_SupplierImportFileDetails_TestProcess file = new DHSVC.DC_SupplierImportFileDetails_TestProcess();
                file.SupplierImportFile_Id = obj.SupplierImportFile_Id;
                file.Supplier_Id = obj.Supplier_Id;
                file.SavedFilePath = obj.SavedFilePath;
                file.PROCESS_USER = obj.PROCESS_USER;
                file.Entity = obj.Entity;
                file.STATUS = obj.STATUS;
                file.Supplier = obj.Supplier;

                DHSVCProxyAsync DHP = new DHSVCProxyAsync();
                DHP.PostAsync(ProxyFor.DataHandler, System.Configuration.ConfigurationManager.AppSettings["Data_Handler_Process_File"], file, file.GetType());
                DHP = null;
                file = null;

                return new DC_Message { StatusMessage = "File has been sent for processing.", StatusCode = ReadOnlyMessage.StatusCode.Success };
            }
            catch (Exception e)
            {
                dc.StatusMessage = ReadOnlyMessage.strFailed;
                dc.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                return dc;
            }
        }

        public DataSet StaticFileUpload_TestFile_Read(DataContracts.UploadStaticData.DC_SupplierImportFileDetails_TestProcess obj)
        {
            try
            {
                DHSVC.DC_SupplierImportFileDetails_TestProcess file = new DHSVC.DC_SupplierImportFileDetails_TestProcess();
                file.SupplierImportFile_Id = obj.SupplierImportFile_Id;
                file.Supplier_Id = obj.Supplier_Id;
                file.SavedFilePath = obj.SavedFilePath;
                file.PROCESS_USER = obj.PROCESS_USER;
                file.Entity = obj.Entity;
                file.STATUS = obj.STATUS;
                file.No_Of_Records_ToProcess = obj.No_Of_Records_ToProcess;

                object result = null;
                DHSVCProxy.PostData(ProxyFor.DataHandler, System.Configuration.ConfigurationManager.AppSettings["Data_Handler_Read_Test_File"], file, file.GetType(), typeof(DataSet), out result);
                return result as DataSet;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public DataSet StaticFileUpload_TestFile_Transform(DataContracts.UploadStaticData.DC_SupplierImportFileDetails_TestProcess obj)
        {
            try
            {
                DHSVC.DC_SupplierImportFileDetails_TestProcess file = new DHSVC.DC_SupplierImportFileDetails_TestProcess();
                file.SupplierImportFile_Id = obj.SupplierImportFile_Id;
                file.Supplier_Id = obj.Supplier_Id;
                file.SavedFilePath = obj.SavedFilePath;
                file.PROCESS_USER = obj.PROCESS_USER;
                file.Entity = obj.Entity;
                file.STATUS = obj.STATUS;
                file.No_Of_Records_ToProcess = obj.No_Of_Records_ToProcess;
                file.Data = obj.Data;

                object result = null;
                DHSVCProxy.PostData(ProxyFor.DataHandler, System.Configuration.ConfigurationManager.AppSettings["Data_Handler_Transform_Test_File"], file, file.GetType(), typeof(DataSet), out result);
                return result as DataSet;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        #endregion

        #region File Progress DashBoard
        public DataContracts.DC_FileProgressDashboard getFileProgressDashBoardData(Guid fileid)
        {
            try
            {
                DataContracts.DC_FileProgressDashboard obj = new DC_FileProgressDashboard();
                obj.ProgressLog = GetStaticDataUploadProcessLog(new DataContracts.UploadStaticData.DC_SupplierImportFile_Progress_RQ { SupplierImportFile_Id = fileid.ToString() });
                obj.VerboseLog = GetStaticDataUploadVerboseLog(new DataContracts.UploadStaticData.DC_SupplierImportFile_VerboseLog_RQ { SupplierImportFile_Id = fileid });
                obj.FileStatistics = GetStaticDataUploadStatistics(new DataContracts.UploadStaticData.DC_SupplierImportFile_Statistics_RQ { SupplierImportFile_Id = fileid });
                obj.FileDetails = GetStaticDataFileDetail(new DataContracts.UploadStaticData.DC_SupplierImportFileDetails_RQ { SupplierImportFile_Id = fileid });
                obj.ErrorLog = GetStaticDataUploadErrorLog(new DataContracts.UploadStaticData.DC_SupplierImportFile_ErrorLog_RQ { SupplierImportFile_Id = fileid });
                return obj;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        #endregion


    }
}
