﻿using System;
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
using System.Net;
using System.Runtime.Serialization.Json;
using System.IO;
using Newtonsoft.Json;
using DataContracts.Mapping;
using System.Transactions;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core;
using System.Threading;
using System.Data.Entity.SqlServer;

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
                                            join sup in context.Supplier on a.Supplier_Id equals sup.Supplier_Id
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
                                         join sup in context.Supplier on a.Supplier_Id equals sup.Supplier_Id into s
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
                                            join s in context.Supplier on at.Supplier_Id equals s.Supplier_Id
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
                                             TotalRecords = total,
                                             AttributeValueType = a.AttributeValueType,
                                             Comparison = a.Comparison
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
                    string issue = "";
                    string AppendMsg = string.Empty;

                    var isDuplicate = false;

                    switch (obj.AttributeType.ToLower())
                    {
                        case "filedetails":

                            switch (obj.AttributeValue.ToLower())
                            {
                                case "firstrowisheader":
                                case "format":
                                case "keyword":
                                case "delimiter":
                                case "textqualifier":
                                case "run matching":
                                    // Check Duplicate Records for Main Flag Off.
                                    isDuplicate = (from attr in context.m_SupplierImportAttributeValues
                                                   where (attr.AttributeType.Trim().TrimStart().ToUpper() == obj.AttributeType.Trim().TrimStart().ToUpper() &&
                                                   attr.AttributeValue.Trim().TrimStart().ToUpper() == obj.AttributeValue.Trim().TrimStart().ToUpper() &&
                                                   attr.SupplierImportAttribute_Id == obj.SupplierImportAttribute_Id && attr.Priority == obj.Priority)
                                                   select attr).Count() == 0 ? false : true;
                                    // Check Duplicate for Delete State
                                    issue = (from attr in context.m_SupplierImportAttributeValues
                                             where attr.SupplierImportAttributeValue_Id == obj.SupplierImportAttributeValue_Id ||
                                             (attr.AttributeType.Trim().TrimStart().ToUpper() == obj.AttributeType.Trim().TrimStart().ToUpper() &&
                                             attr.AttributeValue.Trim().TrimStart().ToUpper() == obj.AttributeValue.Trim().TrimStart().ToUpper() &&
                                              attr.STATUS.Trim().TrimStart().ToUpper() == "ACTIVE" &&
                                              attr.SupplierImportAttribute_Id == obj.SupplierImportAttribute_Id && attr.Priority == obj.Priority
                                             )
                                             select attr.STATUS).Count() > 0 ? "ACTIVE" : "INACTIVE";
                                    // Check Duplicate Records for Exact Issue in Value combinations.
                                    AppendMsg = (from attr in context.m_SupplierImportAttributeValues
                                                 where attr.SupplierImportAttributeValue_Id == obj.SupplierImportAttributeValue_Id ||
                                                 (attr.AttributeType.Trim().TrimStart().ToUpper() == obj.AttributeType.Trim().TrimStart().ToUpper() &&
                                                 attr.AttributeName.Trim().TrimStart().ToUpper() != obj.AttributeName.Trim().TrimStart().ToUpper() &&
                                                  attr.SupplierImportAttribute_Id == obj.SupplierImportAttribute_Id && attr.Priority == obj.Priority
                                                 )
                                                 select attr.STATUS).Count() > 0 ? " with different Value " : "";
                                    break;
                                case "numberofcolumns":
                                    // Check Duplicate Records for Main Flag Off.
                                    isDuplicate = (from attr in context.m_SupplierImportAttributeValues
                                                   where (attr.AttributeType.Trim().TrimStart().ToUpper() == obj.AttributeType.Trim().TrimStart().ToUpper() &&
                                                   attr.AttributeValue.Trim().TrimStart().ToUpper() == obj.AttributeValue.Trim().TrimStart().ToUpper() &&
                                                   attr.SupplierImportAttribute_Id == obj.SupplierImportAttribute_Id && attr.Priority == obj.Priority)
                                                   select attr).Count() == 0 ? false : true;
                                    // Check Duplicate for Delete State
                                    issue = (from attr in context.m_SupplierImportAttributeValues
                                             where attr.SupplierImportAttributeValue_Id == obj.SupplierImportAttributeValue_Id ||
                                             (attr.AttributeType.Trim().TrimStart().ToUpper() == obj.AttributeType.Trim().TrimStart().ToUpper() &&
                                             attr.AttributeValue.Trim().TrimStart().ToUpper() == obj.AttributeValue.Trim().TrimStart().ToUpper() &&
                                             attr.STATUS.Trim().TrimStart().ToUpper() == "ACTIVE" &&
                                             attr.SupplierImportAttribute_Id == obj.SupplierImportAttribute_Id && attr.Priority == obj.Priority

                                             )
                                             select attr.STATUS).Count() > 0 ? "ACTIVE" : "INACTIVE";

                                    // Check Duplicate Records for Exact Issue in Value combinations.
                                    AppendMsg = (from attr in context.m_SupplierImportAttributeValues
                                                 where attr.SupplierImportAttributeValue_Id == obj.SupplierImportAttributeValue_Id ||
                                                 (attr.AttributeType.Trim().TrimStart().ToUpper() == obj.AttributeType.Trim().TrimStart().ToUpper() &&
                                                 attr.AttributeValue.Trim().TrimStart().ToUpper() == obj.AttributeValue.Trim().TrimStart().ToUpper() &&
                                                  attr.SupplierImportAttribute_Id == obj.SupplierImportAttribute_Id && attr.Priority == obj.Priority
                                                 )
                                                 select attr.STATUS).Count() > 0 ? " with different Value " : "";
                                    break;
                                default:
                                    break;
                            }

                            break;
                        case "keyword":

                            // Check Duplicate Records for Main Flag Off.
                            isDuplicate = (from attr in context.m_SupplierImportAttributeValues
                                           where (attr.AttributeType.Trim().TrimStart().ToUpper() == obj.AttributeType.Trim().TrimStart().ToUpper() &&
                                           attr.AttributeValue.Trim().TrimStart().ToUpper() == obj.AttributeValue.Trim().TrimStart().ToUpper() &&
                                           attr.SupplierImportAttribute_Id == obj.SupplierImportAttribute_Id && attr.Priority == obj.Priority)
                                           select attr).Count() == 0 ? false : true;

                            // Check Duplicate for Delete State
                            issue = (from attr in context.m_SupplierImportAttributeValues
                                     where attr.SupplierImportAttributeValue_Id == obj.SupplierImportAttributeValue_Id ||
                                     (attr.AttributeType.Trim().TrimStart().ToUpper() == obj.AttributeType.Trim().TrimStart().ToUpper() &&
                                     attr.AttributeValue.Trim().TrimStart().ToUpper() == obj.AttributeValue.Trim().TrimStart().ToUpper() &&
                                      attr.STATUS.Trim().TrimStart().ToUpper() == "ACTIVE" &&
                                      attr.SupplierImportAttribute_Id == obj.SupplierImportAttribute_Id && attr.Priority == obj.Priority
                                     )
                                     select attr.STATUS).Count() > 0 ? "ACTIVE" : "INACTIVE";
                            // Check Duplicate Records for Exact Issue in Value combinations.
                            AppendMsg = (from attr in context.m_SupplierImportAttributeValues
                                         where attr.SupplierImportAttributeValue_Id == obj.SupplierImportAttributeValue_Id ||
                                         (attr.AttributeType.Trim().TrimStart().ToUpper() == obj.AttributeType.Trim().TrimStart().ToUpper() &&
                                         attr.AttributeName.Trim().TrimStart().ToUpper() != obj.AttributeName.Trim().TrimStart().ToUpper() &&
                                          attr.SupplierImportAttribute_Id == obj.SupplierImportAttribute_Id && attr.Priority == obj.Priority
                                         )
                                         select attr.STATUS).Count() > 0 ? " with different Value " : "";
                            break;
                        case "map":

                            // Check Duplicate Records for Main Flag Off.
                            isDuplicate = (from attr in context.m_SupplierImportAttributeValues
                                           where (attr.AttributeType.Trim().TrimStart().ToUpper() == obj.AttributeType.Trim().TrimStart().ToUpper() &&
                                           attr.AttributeName.Trim().TrimStart().ToUpper() == obj.AttributeName.Trim().TrimStart().ToUpper() &&
                                           attr.SupplierImportAttribute_Id == obj.SupplierImportAttribute_Id && attr.Priority == obj.Priority)
                                           select attr).Count() == 0 ? false : true;

                            // Check Duplicate for Delete State
                            issue = (from attr in context.m_SupplierImportAttributeValues
                                     where attr.SupplierImportAttributeValue_Id == obj.SupplierImportAttributeValue_Id ||
                                     (attr.AttributeType.Trim().TrimStart().ToUpper() == obj.AttributeType.Trim().TrimStart().ToUpper() &&
                                     attr.AttributeName.Trim().TrimStart().ToUpper() == obj.AttributeName.Trim().TrimStart().ToUpper() &&
                                      attr.STATUS.Trim().TrimStart().ToUpper() == "ACTIVE" &&
                                      attr.SupplierImportAttribute_Id == obj.SupplierImportAttribute_Id && attr.Priority == obj.Priority
                                     )
                                     select attr.STATUS).Count() > 0 ? "ACTIVE" : "INACTIVE";

                            // Check Duplicate Records for Exact Issue in Value combinations.
                            AppendMsg = (from attr in context.m_SupplierImportAttributeValues
                                         where attr.SupplierImportAttributeValue_Id == obj.SupplierImportAttributeValue_Id ||
                                         (attr.AttributeType.Trim().TrimStart().ToUpper() == obj.AttributeType.Trim().TrimStart().ToUpper() &&
                                         attr.AttributeName.Trim().TrimStart().ToUpper() != obj.AttributeName.Trim().TrimStart().ToUpper() &&
                                          attr.SupplierImportAttribute_Id == obj.SupplierImportAttribute_Id && attr.Priority == obj.Priority
                                         )
                                         select attr.STATUS).Count() > 0 ? " with different Value " : "";
                            break;
                        case "format":
                            isDuplicate = (from attr in context.m_SupplierImportAttributeValues
                                           where attr.SupplierImportAttributeValue_Id == obj.SupplierImportAttributeValue_Id ||
                                           (attr.AttributeType.Trim().TrimStart().ToUpper() == obj.AttributeType.Trim().TrimStart().ToUpper() &&
                                            // attr.AttributeName.Trim().TrimStart().ToUpper() == obj.AttributeName.Trim().TrimStart().ToUpper() &&
                                            //attr.AttributeValue_ID.Value == obj.AttributeValue_ID.Value &&
                                            attr.AttributeValue.Trim().TrimStart().ToUpper() == obj.AttributeValue.Trim().TrimStart().ToUpper() &&
                                            attr.SupplierImportAttribute_Id == obj.SupplierImportAttribute_Id &&
                                            attr.Priority == obj.Priority
                                           )
                                           select attr).Count() == 0 ? false : true;

                            issue = (from attr in context.m_SupplierImportAttributeValues
                                     where attr.SupplierImportAttributeValue_Id == obj.SupplierImportAttributeValue_Id ||
                                     (attr.AttributeType.Trim().TrimStart().ToUpper() == obj.AttributeType.Trim().TrimStart().ToUpper() &&
                                      attr.AttributeValue.Trim().TrimStart().ToUpper() == obj.AttributeValue.Trim().TrimStart().ToUpper() &&
                                      attr.SupplierImportAttribute_Id == obj.SupplierImportAttribute_Id && attr.Priority == obj.Priority
                                     )
                                     select attr.STATUS).Count() > 0 ? "ACTIVE" : "INACTIVE";
                            break;
                        default:
                            isDuplicate = (from attr in context.m_SupplierImportAttributeValues
                                           where attr.SupplierImportAttributeValue_Id == obj.SupplierImportAttributeValue_Id ||
                                           (attr.AttributeType.Trim().TrimStart().ToUpper() == obj.AttributeType.Trim().TrimStart().ToUpper() &&
                                            attr.AttributeName.Trim().TrimStart().Replace(@"""", "").Replace("&quot;", "").Replace("&nbsp;", "").ToUpper() ==
                                            obj.AttributeName.Trim().TrimStart().Replace(@"""", "").Replace("&quot;", "").Replace("&nbsp;", "").ToUpper() &&
                                            //attr.AttributeValue_ID.Value.Replace(@"""", "").Replace("&quot;", "").Replace("&nbsp;", "") == obj.AttributeValue_ID.Value.Replace(@"""", "").Replace("&quot;", "").Replace("&nbsp;", "") &&
                                            attr.AttributeValue.Trim().TrimStart().Replace(@"""", "").Replace("&quot;", "").Replace("&nbsp;", "").ToUpper() == obj.AttributeValue.Trim().TrimStart().Replace(@"""", "").Replace("&quot;", "").Replace("&nbsp;", "").ToUpper() &&
                                            attr.SupplierImportAttribute_Id == obj.SupplierImportAttribute_Id &&
                                            attr.Priority == obj.Priority
                                           )


                                           select attr).Count() == 0 ? false : true;

                            issue = (from attr in context.m_SupplierImportAttributeValues
                                     where attr.SupplierImportAttributeValue_Id == obj.SupplierImportAttributeValue_Id ||
                                     (attr.AttributeType.Trim().TrimStart().ToUpper() == obj.AttributeType.Trim().TrimStart().ToUpper() &&
                                      attr.AttributeName.Trim().TrimStart().ToUpper() == obj.AttributeName.Trim().TrimStart().ToUpper() &&
                                      attr.AttributeValue.Trim().TrimStart().Replace("&quot;", "").ToUpper() == obj.AttributeValue.Trim().TrimStart().Replace(@"\""", "").ToUpper() &&
                                      attr.SupplierImportAttribute_Id == obj.SupplierImportAttribute_Id &&
                                      attr.Priority == obj.Priority && attr.STATUS.Trim().TrimStart().ToUpper() == "ACTIVE"
                                     )
                                     select attr.STATUS).Count() > 0 ? "ACTIVE" : "INACTIVE";
                            break;
                    }


                    if (isDuplicate)
                    {
                        dc.StatusCode = ReadOnlyMessage.StatusCode.Duplicate;
                        dc.StatusMessage = "Supplier Mapping Attribute" + (issue == "ACTIVE" ? ReadOnlyMessage.strAlreadyExist.Replace("system", "system" + AppendMsg) : ReadOnlyMessage.strAlreadyExist.Replace("system.", "system in Delete state.").Replace("system", "system" + AppendMsg));
                        // ReadOnlyMessage.strAlreadyExist.Replace("system.","system in Delete state.") + issue;
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
                        objNew.AttributeValueType = obj.AttributeValueType;
                        objNew.Comparison = obj.Comparison;
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

        public DataContracts.DC_Message AddStaticDataMappingAttributeValues_Original(DataContracts.UploadStaticData.DC_SupplierImportAttributeValues obj)
        {
            DataContracts.DC_Message dc = new DataContracts.DC_Message();

            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    //Check duplicate
                    string issue = "";
                    var isDuplicate = false;
                    if (obj.AttributeValue.ToLower() == "format")
                    {
                        isDuplicate = (from attr in context.m_SupplierImportAttributeValues
                                       where attr.SupplierImportAttributeValue_Id == obj.SupplierImportAttributeValue_Id ||
                                       (attr.AttributeType.Trim().TrimStart().ToUpper() == obj.AttributeType.Trim().TrimStart().ToUpper() &&
                                        // attr.AttributeName.Trim().TrimStart().ToUpper() == obj.AttributeName.Trim().TrimStart().ToUpper() &&
                                        //attr.AttributeValue_ID.Value == obj.AttributeValue_ID.Value &&
                                        attr.AttributeValue.Trim().TrimStart().ToUpper() == obj.AttributeValue.Trim().TrimStart().ToUpper() &&
                                        attr.SupplierImportAttribute_Id == obj.SupplierImportAttribute_Id &&
                                        attr.Priority == obj.Priority
                                       )
                                       select attr).Count() == 0 ? false : true;

                        issue = (from attr in context.m_SupplierImportAttributeValues
                                 where attr.SupplierImportAttributeValue_Id == obj.SupplierImportAttributeValue_Id ||
                                 (attr.AttributeType.Trim().TrimStart().ToUpper() == obj.AttributeType.Trim().TrimStart().ToUpper() &&
                                  attr.AttributeValue.Trim().TrimStart().ToUpper() == obj.AttributeValue.Trim().TrimStart().ToUpper() &&
                                  attr.SupplierImportAttribute_Id == obj.SupplierImportAttribute_Id && attr.Priority == obj.Priority
                                 )
                                 select attr.STATUS).Count() > 0 ? "ACTIVE" : "INACTIVE";
                    }
                    else
                    {
                        isDuplicate = (from attr in context.m_SupplierImportAttributeValues
                                       where attr.SupplierImportAttributeValue_Id == obj.SupplierImportAttributeValue_Id ||
                                       (attr.AttributeType.Trim().TrimStart().ToUpper() == obj.AttributeType.Trim().TrimStart().ToUpper() &&
                                        attr.AttributeName.Trim().TrimStart().Replace(@"""", "").Replace("&quot;", "").Replace("&nbsp;", "").ToUpper() ==
                                        obj.AttributeName.Trim().TrimStart().Replace(@"""", "").Replace("&quot;", "").Replace("&nbsp;", "").ToUpper() &&
                                        //attr.AttributeValue_ID.Value.Replace(@"""", "").Replace("&quot;", "").Replace("&nbsp;", "") == obj.AttributeValue_ID.Value.Replace(@"""", "").Replace("&quot;", "").Replace("&nbsp;", "") &&
                                        attr.AttributeValue.Trim().TrimStart().Replace(@"""", "").Replace("&quot;", "").Replace("&nbsp;", "").ToUpper() == obj.AttributeValue.Trim().TrimStart().Replace(@"""", "").Replace("&quot;", "").Replace("&nbsp;", "").ToUpper() &&
                                        attr.SupplierImportAttribute_Id == obj.SupplierImportAttribute_Id &&
                                        attr.Priority == obj.Priority
                                       )


                                       select attr).Count() == 0 ? false : true;

                        issue = (from attr in context.m_SupplierImportAttributeValues
                                 where attr.SupplierImportAttributeValue_Id == obj.SupplierImportAttributeValue_Id ||
                                 (attr.AttributeType.Trim().TrimStart().ToUpper() == obj.AttributeType.Trim().TrimStart().ToUpper() &&
                                  attr.AttributeName.Trim().TrimStart().ToUpper() == obj.AttributeName.Trim().TrimStart().ToUpper() &&
                                  attr.AttributeValue.Trim().TrimStart().Replace("&quot;", "").ToUpper() == obj.AttributeValue.Trim().TrimStart().Replace(@"\""", "").ToUpper() &&
                                  attr.SupplierImportAttribute_Id == obj.SupplierImportAttribute_Id &&
                                  attr.Priority == obj.Priority && attr.STATUS.Trim().TrimStart().ToUpper() == "ACTIVE"
                                 )


                                 select attr.STATUS).Count() > 0 ? "ACTIVE" : "INACTIVE";
                    }
                    if (isDuplicate)
                    {
                        dc.StatusCode = ReadOnlyMessage.StatusCode.Duplicate;
                        dc.StatusMessage = "Supplier Mapping Attribute" + (issue == "ACTIVE" ? ReadOnlyMessage.strAlreadyExist : ReadOnlyMessage.strAlreadyExist.Replace("system.", "system in Delete state."));
                        // ReadOnlyMessage.strAlreadyExist.Replace("system.","system in Delete state.") + issue;
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
                        objNew.AttributeValueType = obj.AttributeValueType;
                        objNew.Comparison = obj.Comparison;
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
                                search.AttributeValueType = obj.AttributeValueType;
                                search.Comparison = obj.Comparison;
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

        #region Upload File

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
                                     join s in context.Supplier on a.Supplier_Id equals s.Supplier_Id
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

                    if (!string.IsNullOrWhiteSpace(RQ.Mode))
                    {
                        FileSearch = from a in FileSearch
                                     where a.Mode.Trim().TrimStart().ToUpper() == RQ.Mode.Trim().TrimStart().ToUpper()
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
                                            join s in context.Supplier on a.Supplier_Id equals s.Supplier_Id
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
                                                Mode = a.Mode,
                                                IsActive = a.IsActive ?? true,
                                                TotalRecords = total,
                                                IsStopped = a.IsStopped,
                                                IsPaused = a.IsPaused,
                                                IsRestarted = a.IsRestarted,
                                                IsResumed = a.IsResumed,
                                                CurrentBatch = a.CurrentBatch
                                            }
                                        ).Skip(skip).Take(RQ.PageSize).ToList();

                    return FileSearchResult;
                }
            }
            catch (Exception ex)
            {
                DL_Mapping _obj = new DL_Mapping();
                _obj.LogErrorMessage(RQ.SupplierImportFile_Id ?? Guid.Empty, ex, "GetStaticDataFileDetail", "DL_UploadStaticData", "GetStaticDataFileDetail", (int)Error_Enums_DataHandler.ErrorCodes.RoomTypeConfig_Generic, "", "GetStaticDataFileDetail failed");
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
                    //Search if record is already scheduled
                    int count = 0;

                    if (obj != null)
                    {
                        if (obj.STATUS == "SCHEDULED")
                        {
                            count = (from a in context.SupplierImportFileDetails
                                     where a.Supplier_Id == obj.Supplier_Id
                                     && a.Entity == obj.Entity
                                     && a.STATUS == "SCHEDULED"
                                     select a).Count();
                            if (count > 0)
                            {
                                dc.StatusCode = ReadOnlyMessage.StatusCode.Duplicate;
                                dc.StatusMessage = "Supplier File has already been scheduled for Re-Run.";
                            }
                        }

                        if (count == 0)
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
                            objNew.Mode = obj.Mode;
                            objNew.IsActive = true;

                            context.SupplierImportFileDetails.Add(objNew);
                            context.SaveChanges();

                            dc.StatusCode = ReadOnlyMessage.StatusCode.Success;
                            dc.StatusMessage = "Supplier File " + ReadOnlyMessage.strAddedSuccessfully;

                        }
                    }
                    return dc;
                }


            }
            catch (Exception)
            {
                dc.StatusMessage = ReadOnlyMessage.strFailed;
                dc.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                return dc;
            }
        }

        public DataContracts.UploadStaticData.DC_SupplierImportFileDetails AddStaticDataFileDetailForMongo(string SupplierId, string Entity)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {

                    Guid Supplier_Id = context.Supplier.Where(w => (w.Name == SupplierId || w.Code == SupplierId) && w.StatusCode == "ACTIVE").Select(s => s.Supplier_Id).FirstOrDefault();

                    if (Supplier_Id != Guid.Empty)
                    {
                        DataLayer.SupplierImportFileDetail objNew = new DataLayer.SupplierImportFileDetail
                        {
                            SupplierImportFile_Id = Guid.NewGuid(),
                            Supplier_Id = Supplier_Id,
                            Entity = Entity,
                            STATUS = "UPLOADED",
                            ArchiveFilePath = "",
                            OriginalFilePath = SupplierId.ToLower() + "." + "mongo",
                            SavedFilePath = SupplierId.ToLower() + "." + "mongo",
                            CREATE_DATE = DateTime.Now,
                            CREATE_USER = "TLGX_DataHandler",
                            Mode = "ALL",
                            IsActive = true
                        };

                        context.SupplierImportFileDetails.Add(objNew);
                        context.SaveChanges();

                        return new DC_SupplierImportFileDetails
                        {
                            Supplier_Id = objNew.Supplier_Id,
                            ArchiveFilePath = objNew.ArchiveFilePath,
                            CREATE_DATE = objNew.CREATE_DATE,
                            CREATE_USER = objNew.CREATE_USER,
                            Entity = objNew.Entity,
                            IsActive = objNew.IsActive,
                            Mode = objNew.Mode,
                            OriginalFilePath = objNew.OriginalFilePath,
                            PROCESS_DATE = objNew.PROCESS_DATE,
                            PROCESS_USER = objNew.PROCESS_USER,
                            SavedFilePath = objNew.SavedFilePath,
                            STATUS = objNew.STATUS,
                            Supplier = SupplierId,
                            SupplierImportFile_Id = objNew.SupplierImportFile_Id,
                            TotalRecords = 1
                        };
                    }
                    else
                    {
                        return new DC_SupplierImportFileDetails();
                    }
                }
            }
            catch (Exception)
            {
                return new DC_SupplierImportFileDetails();
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
                        search.CurrentBatch = obj.CurrentBatch;
                        search.IsPaused = obj.IsPaused;
                        search.IsRestarted = obj.IsRestarted;
                        obj.IsResumed = obj.IsResumed;
                        obj.IsStopped = obj.IsStopped;
                    }
                    context.SaveChanges();
                    if (obj.IsStopped == true)
                    {
                        dc.StatusCode = ReadOnlyMessage.StatusCode.Stopped;
                        dc.StatusMessage = "Supplier File Status" + ReadOnlyMessage.strStopped;
                    }
                    else if (obj.IsPaused == true)
                    {
                        dc.StatusCode = ReadOnlyMessage.StatusCode.Paused;
                        dc.StatusMessage = "Supplier File Status" + ReadOnlyMessage.strPaused;
                    }
                    else
                    {
                        dc.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        dc.StatusMessage = "Supplier File Status" + ReadOnlyMessage.strUpdatedSuccessfully;
                    }
                }
            }
            catch (Exception ex)
            {
                dc.StatusMessage = ReadOnlyMessage.strFailed;
                dc.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                return dc;
            }

            return dc;
        }

        #endregion

        #region Logging

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
                                     join s in context.Supplier.AsNoTracking() on f.Supplier_Id equals s.Supplier_Id
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
                                      join s in context.Supplier.AsNoTracking() on f.Supplier_Id equals s.Supplier_Id
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
                                                   //join j in context.STG_Mapping_TableIds on m.CountryMapping_Id equals j.Mapping_Id
                                               where m.ReRun_SupplierImportFile_Id == obj.SupplierImportFile_Id
                                               group m by new { m.Status } into g
                                               select new { g.Key.Status, count = g.Count() };

                                if ((obj.From ?? "") == "MATCHING")
                                {
                                    stat.FinalStatus = (from a in context.SupplierImportFileDetails where a.SupplierImportFile_Id == obj.SupplierImportFile_Id select a.STATUS).FirstOrDefault();
                                    stat.Unmapped = (stat.Unmapped ?? 0) + getcount.Where(g => g.Status.Trim().ToUpper() == "UNMAPPED").Select(a => a.count).FirstOrDefault();
                                    stat.Mapped = (stat.Mapped ?? 0) + getcount.Where(g => g.Status.Trim().ToUpper() == "MAPPED" || g.Status.Trim().ToUpper() == "REVIEW").Select(a => a.count).FirstOrDefault();
                                }
                                else
                                {
                                    stat.TotalRows = getcount.Select(a => a.count).Sum(); //obj.TotalRows ?? stat.TotalRows;
                                }
                            }
                            else if (resstat[0].Entity.Trim().ToUpper() == "CITY")
                            {
                                var getcount = from m in context.m_CityMapping
                                                   //join j in context.STG_Mapping_TableIds on m.CityMapping_Id equals j.Mapping_Id
                                               where m.ReRun_SupplierImportFile_Id == obj.SupplierImportFile_Id
                                               group m by new { m.Status } into g
                                               select new { g.Key.Status, count = g.Count() };
                                if ((obj.From ?? "") == "MATCHING")
                                {
                                    stat.FinalStatus = (from a in context.SupplierImportFileDetails where a.SupplierImportFile_Id == obj.SupplierImportFile_Id select a.STATUS).FirstOrDefault();
                                    stat.Unmapped = (stat.Unmapped ?? 0) + getcount.Where(g => g.Status.Trim().ToUpper() == "UNMAPPED").Select(a => a.count).FirstOrDefault();
                                    stat.Mapped = (stat.Mapped ?? 0) + getcount.Where(g => g.Status.Trim().ToUpper() == "MAPPED" || g.Status.Trim().ToUpper() == "REVIEW").Select(a => a.count).FirstOrDefault();
                                }
                                else
                                {
                                    stat.TotalRows = stat.TotalRows + getcount.Select(a => a.count).Sum(); //obj.TotalRows ?? stat.TotalRows;
                                }
                            }
                            else if (resstat[0].Entity.Trim().ToUpper() == "HOTEL")
                            {
                                var getcount = from m in context.Accommodation_ProductMapping
                                                   //join j in context.STG_Mapping_TableIds on m.Accommodation_ProductMapping_Id equals j.Mapping_Id
                                               where m.ReRun_SupplierImportFile_Id == obj.SupplierImportFile_Id
                                               group m by new { m.Status } into g
                                               select new { g.Key.Status, count = g.Count() };

                                if ((obj.From ?? "") == "MATCHING")
                                {
                                    stat.FinalStatus = (from a in context.SupplierImportFileDetails where a.SupplierImportFile_Id == obj.SupplierImportFile_Id select a.STATUS).FirstOrDefault();
                                    stat.Unmapped = (stat.Unmapped ?? 0) + getcount.Where(g => g.Status.Trim().ToUpper() == "UNMAPPED").Select(a => a.count).FirstOrDefault();
                                    stat.Mapped = (stat.Mapped ?? 0) + getcount.Where(g => g.Status.Trim().ToUpper() == "MAPPED" || g.Status.Trim().ToUpper() == "REVIEW").Select(a => a.count).FirstOrDefault();
                                }
                                else
                                {
                                    stat.TotalRows = getcount.Select(a => a.count).Sum(); //obj.TotalRows ?? stat.TotalRows;
                                }
                            }
                            else if (resstat[0].Entity.Trim().ToUpper() == "ROOMTYPE")
                            {
                                var getcount = from m in context.Accommodation_SupplierRoomTypeMapping
                                                   //join j in context.STG_Mapping_TableIds on m.Accommodation_SupplierRoomTypeMapping_Id equals j.Mapping_Id
                                               where m.ReRun_SupplierImportFile_Id == obj.SupplierImportFile_Id
                                               group m by new { m.MappingStatus } into g
                                               select new { g.Key.MappingStatus, count = g.Count() };

                                if ((obj.From ?? "") == "MATCHING")
                                {
                                    stat.FinalStatus = (from a in context.SupplierImportFileDetails where a.SupplierImportFile_Id == obj.SupplierImportFile_Id select a.STATUS).FirstOrDefault();
                                    stat.Unmapped = (stat.Unmapped ?? 0) + getcount.Where(g => g.MappingStatus.Trim().ToUpper() == "UNMAPPED").Select(a => a.count).FirstOrDefault();
                                    stat.Mapped = (stat.Mapped ?? 0) + getcount.Where(g => g.MappingStatus.Trim().ToUpper() == "MAPPED" || g.MappingStatus.Trim().ToUpper() == "REVIEW").Select(a => a.count).FirstOrDefault();
                                }
                                else
                                {
                                    stat.TotalRows = getcount.Select(a => a.count).Sum(); //obj.TotalRows ?? stat.TotalRows;
                                }
                            }
                        }
                        else
                        {
                            DataLayer.SupplierImportFile_Statistics newObj = new DataLayer.SupplierImportFile_Statistics()
                            {
                                SupplierImportFile_Statistics_Id = Guid.NewGuid(),
                                SupplierImportFile_Id = obj.SupplierImportFile_Id,
                                FinalStatus = obj.FinalStatus,
                                //TotalRows = 0,
                                //Mapped = 0,
                                //Unmapped = 0,
                                TotalRows = obj.TotalRows ?? 0,
                                Mapped = obj.Mapped ?? 0,
                                Unmapped = obj.Unmapped ?? 0,
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
                        TimeStamp = obj.TimeStamp,
                        BatchNumber = obj.BatchNumber
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

                    if (RQ.PageSize == null)
                        RQ.PageSize = 250;
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
                                             TotalCount = total,
                                             BatchNumber = a.BatchNumber
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

        #region STG Tables (Add / Get)

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
                        Guid File_Id = lstobj[0].SupplierImportFile_Id;

                        //var oldRecords = (from y in context.stg_SupplierCountryMapping
                        //                  where y.SupplierName.Trim().ToUpper() == mySupplier.Trim().ToUpper()
                        //                  && y.SupplierImportFile_Id == File_Id
                        //                  select y).ToList();
                        //context.stg_SupplierCountryMapping.RemoveRange(oldRecords);
                        //context.SaveChanges();

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
                                objNew.ContinentCode = obj.ContinentCode;
                                objNew.ContinentName = obj.ContinentName;
                                objNew.SupplierImportFile_Id = (obj.ComboFile_Id == Guid.Empty ? obj.SupplierImportFile_Id : obj.ComboFile_Id);
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

        public DataContracts.DC_Message AddSTGCityData(List<DataContracts.STG.DC_stg_SupplierCityMapping> lstobj)
        {
            DataContracts.DC_Message dc = new DataContracts.DC_Message();
            var executionStrategy = new SqlAzureExecutionStrategy();

            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    string mySupplier = lstobj[0].SupplierName;
                    Guid? mySupplier_Id = lstobj[0].Supplier_Id;

                    if (!string.IsNullOrWhiteSpace(mySupplier))
                    {
                        //var oldRecords = (from y in context.stg_SupplierCityMapping
                        //                  where y.SupplierName.Trim().ToUpper() == mySupplier.Trim().ToUpper()
                        //                  && y.SupplierImportFile_Id == File_Id
                        //                  select y).ToList();
                        //context.stg_SupplierCityMapping.RemoveRange(oldRecords);
                        //context.SaveChanges();

                        List<DataContracts.STG.DC_stg_SupplierCityMapping> dstobj = new List<DC_stg_SupplierCityMapping>();
                        dstobj = lstobj.GroupBy(a => new { a.CityCode, a.CityName, a.CountryCode, a.CountryName, a.StateCode, a.StateName }).Select(grp => grp.First()).ToList();

                        var geo = (from g in context.m_CountryMapping
                                   where g.Supplier_Id == mySupplier_Id
                                   select new
                                   {
                                       g.Country_Id,
                                       g.CountryName,
                                       g.CountryCode
                                   }
                                 ).Distinct().ToList();

                        foreach (DataContracts.STG.DC_stg_SupplierCityMapping obj in dstobj)
                        {
                            DataLayer.stg_SupplierCityMapping objNew = new DataLayer.stg_SupplierCityMapping();
                            objNew.stg_City_Id = Guid.NewGuid();
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

                            if (objNew.CountryCode == null)
                            {
                                objNew.CountryCode = geo.Where(s => s.Country_Id == objNew.Country_Id).Select(s1 => s1.CountryCode).FirstOrDefault();
                            }
                            if (objNew.CountryName == null)
                            {
                                objNew.CountryName = geo.Where(s => s.Country_Id == objNew.Country_Id).Select(s1 => s1.CountryName).FirstOrDefault();
                            }
                            objNew.Supplier_Id = mySupplier_Id;
                            objNew.SupplierImportFile_Id = (obj.ComboFile_Id == Guid.Empty ? obj.SupplierImportFile_Id : obj.ComboFile_Id);
                            context.stg_SupplierCityMapping.Add(objNew);

                        }

                        using (var trn = context.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                        {
                            context.SaveChanges();

                            trn.Commit();
                        }



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
                string mySupplier = lstobj[0].SupplierName;
                Guid? mySupplier_Id = lstobj[0].Supplier_Id;

                if (!string.IsNullOrWhiteSpace(mySupplier))
                {
                    //using (ConsumerEntities context = new ConsumerEntities())
                    //{
                    //    var oldRecords = (from y in context.stg_SupplierProductMapping //.AsNoTracking()
                    //                      where y.Supplier_Id == mySupplier_Id
                    //                      && y.SupplierImportFile_Id == File_Id
                    //                      select y);
                    //    if (oldRecords.Count() > 0)
                    //    {
                    //        context.stg_SupplierProductMapping.RemoveRange(oldRecords);
                    //        context.SaveChanges();
                    //    }
                    //}

                    List<DataContracts.STG.DC_stg_SupplierProductMapping> dstobj = new List<DC_stg_SupplierProductMapping>();

                    dstobj = lstobj.GroupBy(a => new { a.ProductId, a.CityCode, a.CityName }).Select(grp => grp.First()).ToList();

                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        var geoCity = (from g in context.m_CityMapping.AsNoTracking()
                                       where g.Supplier_Id == mySupplier_Id
                                       select new
                                       {
                                           City_Id = g.City_Id,
                                           CityName = g.CityName,
                                           CityCode = g.CityCode,
                                           Country_Id = g.Country_Id,
                                           StateCode = g.StateCode,
                                           StateName = g.StateName
                                       }).ToList().Distinct();

                        var geoCountry = (from g in context.m_CountryMapping.AsNoTracking()
                                          where g.Supplier_Id == mySupplier_Id
                                          select new
                                          {
                                              Country_Id = g.Country_Id,
                                              CountryName = g.CountryName,
                                              CountryCode = g.CountryCode
                                          }).ToList().Distinct();

                        List<DataLayer.stg_SupplierProductMapping> lstobjNew = new List<DataLayer.stg_SupplierProductMapping>();

                        foreach (DataContracts.STG.DC_stg_SupplierProductMapping obj in dstobj)
                        {
                            DataLayer.stg_SupplierProductMapping objNew = new DataLayer.stg_SupplierProductMapping();

                            objNew.CountryCode = obj.CountryCode;
                            objNew.CountryName = obj.CountryName;
                            objNew.CityCode = obj.CityCode;
                            objNew.CityName = obj.CityName;
                            objNew.StateCode = obj.StateCode;
                            objNew.StateName = obj.StateName;

                            var CountryLookup = (from c in geoCountry
                                                 where c.CountryCode == obj.CountryCode
                                                 && obj.CountryCode != null
                                                 select c).ToList();

                            if (CountryLookup.Count() != 1 && obj.CountryName != null)
                            {
                                //check with country name
                                CountryLookup = (from c in geoCountry
                                                 where c.CountryName == obj.CountryName
                                                 select c).ToList();
                            }

                            //If Country Code or Country Name is given and in lookup one country found proceed else skip country City lookup
                            if (CountryLookup.Count() == 1)
                            {
                                objNew.CountryCode = obj.CountryCode ?? CountryLookup.First().CountryCode;
                                objNew.CountryName = obj.CountryName ?? CountryLookup.First().CountryName;
                                objNew.Country_Id = CountryLookup.First().Country_Id;

                                //CityLookup
                                var bCityFound = false;

                                var CityLookup = (from ct in geoCity
                                                  where ct.Country_Id == objNew.Country_Id && ct.CityCode == obj.CityCode
                                                  select ct).ToList();

                                if (CityLookup.Count() == 1)
                                {
                                    bCityFound = true;
                                }
                                else
                                {
                                    CityLookup = (from ct in geoCity
                                                  where ct.Country_Id == objNew.Country_Id && ct.CityName == obj.CityName
                                                  select ct).ToList();

                                    if (CityLookup.Count() == 1)
                                    {
                                        bCityFound = true;
                                    }
                                    else
                                    {
                                        CityLookup = (from ct in geoCity
                                                      where ct.Country_Id == objNew.Country_Id && ct.CityName == obj.CityName && ct.StateName == obj.StateName
                                                      select ct).ToList();

                                        if (CityLookup.Count() == 1)
                                        {
                                            bCityFound = true;
                                        }
                                        else
                                        {
                                            CityLookup = (from ct in geoCity
                                                          where ct.Country_Id == objNew.Country_Id && ct.CityName == obj.CityName && ct.StateCode == obj.StateCode
                                                          select ct).ToList();
                                        }
                                    }
                                }

                                if (bCityFound)
                                {
                                    objNew.City_Id = CityLookup.First().City_Id;
                                    objNew.CityCode = obj.CityCode ?? CityLookup.First().CityCode;
                                    objNew.CityName = obj.CityName ?? CityLookup.First().CityName;
                                    objNew.StateCode = obj.StateCode ?? CityLookup.First().StateCode;
                                    objNew.StateName = obj.StateName ?? CityLookup.First().StateName;
                                }
                            }

                            //CityLookup without country (if country lookup failed)
                            if (CountryLookup.Count() == 0)
                            {
                                var bCityFound = false;

                                var CityLookup = (from ct in geoCity
                                                  where ct.CityCode == obj.CityCode
                                                  select ct).ToList();

                                if (CityLookup.Count() == 1)
                                {
                                    bCityFound = true;
                                }
                                else
                                {
                                    CityLookup = (from ct in geoCity
                                                  where ct.CityName == obj.CityName
                                                  select ct).ToList();

                                    if (CityLookup.Count() == 1)
                                    {
                                        bCityFound = true;
                                    }
                                    else
                                    {
                                        CityLookup = (from ct in geoCity
                                                      where ct.CityName == obj.CityName && ct.StateName == obj.StateName
                                                      select ct).ToList();

                                        if (CityLookup.Count() == 1)
                                        {
                                            bCityFound = true;
                                        }
                                        else
                                        {
                                            CityLookup = (from ct in geoCity
                                                          where ct.CityName == obj.CityName && ct.StateCode == obj.StateCode
                                                          select ct).ToList();
                                        }
                                    }
                                }

                                if (bCityFound)
                                {
                                    objNew.CityCode = obj.CityCode ?? CityLookup.First().CityCode;
                                    objNew.CityName = obj.CityName ?? CityLookup.First().CityName;
                                    objNew.City_Id = CityLookup.First().City_Id;

                                    var CountryLookupFromCity = (from c in geoCountry
                                                                 where c.Country_Id == CityLookup.First().Country_Id
                                                                 select c).FirstOrDefault();

                                    objNew.Country_Id = CityLookup.First().Country_Id;
                                    objNew.CountryCode = obj.CountryCode ?? CountryLookupFromCity.CountryCode;
                                    objNew.CountryName = obj.CountryName ?? CountryLookupFromCity.CountryName;

                                    objNew.StateCode = obj.StateCode ?? CityLookup.First().StateCode;
                                    objNew.StateName = obj.StateName ?? CityLookup.First().StateName;
                                }
                            }

                            objNew.stg_AccoMapping_Id = obj.stg_AccoMapping_Id;
                            objNew.SupplierId = obj.SupplierId;
                            objNew.Supplier_Id = obj.Supplier_Id;
                            objNew.SupplierName = obj.SupplierName;
                            objNew.ProductId = obj.ProductId;
                            objNew.ProductName = obj.ProductName;
                            objNew.StarRating = obj.StarRating;

                            objNew.Address = obj.Address;
                            objNew.StreetNo = obj.StreetNo;
                            objNew.StreetName = obj.StreetName;
                            objNew.Street2 = obj.Street2;
                            objNew.Street3 = obj.Street3;
                            objNew.Street4 = obj.Street4;
                            objNew.Street5 = obj.Street5;
                            objNew.PostalCode = obj.PostalCode;

                            objNew.Latitude = obj.Latitude;
                            objNew.Longitude = obj.Longitude;

                            objNew.TelephoneNumber = obj.TelephoneNumber;
                            objNew.Fax = obj.Fax;
                            objNew.Email = obj.Email;
                            objNew.Website = obj.Website;

                            objNew.Location = obj.Location;
                            objNew.InsertDate = obj.InsertDate;

                            objNew.TX_COUNTRYNAME = obj.TX_COUNTRYNAME;
                            objNew.ActiveFrom = obj.ActiveFrom;
                            objNew.ActiveTo = obj.ActiveTo;
                            objNew.Action = obj.Action;
                            objNew.UpdateType = obj.UpdateType;
                            objNew.ActionText = obj.ActionText;
                            objNew.ProductType = obj.ProductType;
                            objNew.SupplierImportFile_Id = (obj.ComboFile_Id == Guid.Empty ? obj.SupplierImportFile_Id : obj.ComboFile_Id);

                            lstobjNew.Add(objNew);
                        }
                        context.stg_SupplierProductMapping.AddRange(lstobjNew);
                        context.SaveChanges();
                    }
                    dc.StatusCode = ReadOnlyMessage.StatusCode.Success;
                    dc.StatusMessage = "Product Static Data " + ReadOnlyMessage.strAddedSuccessfully;
                }
                else
                {
                    dc.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                    dc.StatusMessage = "Supplier Not Found";
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
                        //var oldRecords = (from y in context.stg_SupplierHotelRoomMapping
                        //                  where y.SupplierName.Trim().ToUpper() == mySupplier.Trim().ToUpper()
                        //                  && y.SupplierImportFile_Id == File_Id
                        //                  select y).ToList();
                        //context.stg_SupplierHotelRoomMapping.RemoveRange(oldRecords);
                        //context.SaveChanges();

                        List<DataContracts.STG.DC_stg_SupplierHotelRoomMapping> dstobj = new List<DC_stg_SupplierHotelRoomMapping>();

                        dstobj = lstobj.GroupBy(a => new { a.RoomName, a.SupplierID, a.SupplierName, a.SupplierProductId, a.SupplierProductName, a.SupplierRoomCategory, a.SupplierRoomTypeCode, a.RatePlanCode, a.BedTypeCode, a.CityName, a.CityCode, a.CountryCode, a.CountryName }).Select(grp => grp.First()).ToList();

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
                                MinGuestOccupancy = obj.MinGuestOccupancy,
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
                                TX_RoomName = obj.TX_RoomName,
                                SupplierImportFile_Id = (obj.ComboFile_Id == Guid.Empty ? obj.SupplierImportFile_Id : obj.ComboFile_Id),
                                CityName = obj.CityName,
                                CityCode = obj.CityCode,
                                StateCode = obj.StateCode,
                                StateName = obj.StateName,
                                CountryCode = obj.CountryCode,
                                CountryName = obj.CountryName
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

                    if (RQ.SupplierImportFile_Id.HasValue)
                    {
                        stgSearch = from a in stgSearch
                                    where a.SupplierImportFile_Id == RQ.SupplierImportFile_Id
                                    select a;
                    }

                    if (!(RQ.Supplier_Id == Guid.Empty))
                    {
                        stgSearch = from a in stgSearch
                                    join s in context.Supplier on a.SupplierName.Trim().TrimStart().ToUpper() equals s.Name.Trim().TrimStart().ToUpper()
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
                                     orderby a.stg_Country_Id
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
                                         Longitude = a.Longitude,
                                         ContinentCode = a.ContinentCode,
                                         ContinentName = a.ContinentName,
                                         SupplierImportFile_Id = a.SupplierImportFile_Id ?? Guid.Empty
                                     }).Skip(skip).Take(RQ.PageSize).ToList();

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
                    var stgSearch = from a in context.stg_SupplierCityMapping.AsNoTracking() select a;

                    if (RQ.stg_City_Id.HasValue)
                    {
                        stgSearch = from a in stgSearch
                                    where a.stg_City_Id == RQ.stg_City_Id
                                    select a;
                    }

                    if (RQ.SupplierImportFile_Id.HasValue)
                    {
                        stgSearch = from a in stgSearch
                                    where a.SupplierImportFile_Id == RQ.SupplierImportFile_Id
                                    select a;
                    }

                    if (!(RQ.Supplier_Id == Guid.Empty))
                    {
                        stgSearch = from a in stgSearch
                                    join s in context.Supplier on a.SupplierName.Trim().TrimStart().ToUpper() equals s.Name.Trim().TrimStart().ToUpper()
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
                                     orderby a.stg_City_Id
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
                                         Supplier_Id = a.Supplier_Id,
                                         SupplierImportFile_Id = a.SupplierImportFile_Id ?? Guid.Empty
                                     }).Skip(skip).Take(RQ.PageSize).ToList();

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

        public List<DC_stg_SupplierProductMapping> GetSTGHotelData(DC_stg_SupplierProductMapping_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var stgSearch = from a in context.stg_SupplierProductMapping.AsNoTracking() select a;

                    if (RQ.stg_AccoMapping_Id.HasValue)
                    {
                        stgSearch = from a in stgSearch
                                    where a.stg_AccoMapping_Id == RQ.stg_AccoMapping_Id
                                    select a;
                    }

                    if (RQ.SupplierImportFile_Id.HasValue)
                    {
                        stgSearch = from a in stgSearch
                                    where a.SupplierImportFile_Id == RQ.SupplierImportFile_Id
                                    select a;
                    }
                    if (RQ.Supplier_Id != Guid.Empty)
                    {
                        stgSearch = from a in stgSearch
                                    where a.Supplier_Id == RQ.Supplier_Id
                                    select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.SupplierName))
                    {
                        stgSearch = from a in stgSearch
                                    where a.SupplierName.Trim().ToUpper() == RQ.SupplierName.Trim().ToUpper()
                                    select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.CountryCode))
                    {
                        stgSearch = from a in stgSearch
                                    where a.CountryCode.Trim().ToUpper() == RQ.CountryCode.Trim().ToUpper()
                                    select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.CountryName))
                    {
                        stgSearch = from a in stgSearch
                                    where a.CountryName.Trim().ToUpper() == RQ.CountryName.Trim().ToUpper()
                                    select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.StateCode))
                    {
                        stgSearch = from a in stgSearch
                                    where a.StateCode.Trim().ToUpper() == RQ.StateCode.Trim().ToUpper()
                                    select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.StateName))
                    {
                        stgSearch = from a in stgSearch
                                    where a.StateName.Trim().ToUpper() == RQ.StateName.Trim().ToUpper()
                                    select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.CityCode))
                    {
                        stgSearch = from a in stgSearch
                                    where a.CityCode.Trim().ToUpper() == RQ.CityCode.Trim().ToUpper()
                                    select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.CityName))
                    {
                        stgSearch = from a in stgSearch
                                    where a.CityName.Trim().ToUpper() == RQ.CityName.Trim().ToUpper()
                                    select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.ProductId))
                    {
                        stgSearch = from a in stgSearch
                                    where a.ProductId.Trim().ToUpper() == RQ.ProductId.Trim().ToUpper()
                                    select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.ProductName))
                    {
                        stgSearch = from a in stgSearch
                                    where a.ProductName.Trim().ToUpper() == RQ.ProductName.Trim().ToUpper()
                                    select a;
                    }
                    if (!string.IsNullOrWhiteSpace(RQ.Latitude))
                    {
                        stgSearch = from a in stgSearch
                                    where a.Latitude.Trim().ToUpper() == RQ.Latitude.Trim().ToUpper()
                                    select a;
                    }
                    if (!string.IsNullOrWhiteSpace(RQ.Longitude))
                    {
                        stgSearch = from a in stgSearch
                                    where a.Longitude.Trim().ToUpper() == RQ.Longitude.Trim().ToUpper()
                                    select a;
                    }
                    if (!string.IsNullOrWhiteSpace(RQ.StarRating))
                    {
                        stgSearch = from a in stgSearch
                                    where a.StarRating.Trim().ToUpper() == RQ.StarRating.Trim().ToUpper()
                                    select a;
                    }

                    List<DataContracts.STG.DC_stg_SupplierProductMapping> stgResult = new List<DC_stg_SupplierProductMapping>();

                    using (var t = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                    {
                        IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                    }))
                    {
                        stgResult = (from a in stgSearch
                                     orderby a.stg_AccoMapping_Id
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
                                         Supplier_Id = a.Supplier_Id,
                                         ProductType = a.ProductType,
                                         SupplierImportFile_Id = a.SupplierImportFile_Id ?? Guid.Empty
                                     }).Skip(RQ.PageNo * RQ.PageSize).Take(RQ.PageSize).ToList();
                    }

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

                    if (RQ.SupplierImportFile_Id.HasValue)
                    {
                        stgSearch = from a in stgSearch
                                    where a.SupplierImportFile_Id == RQ.SupplierImportFile_Id
                                    select a;
                    }

                    if (!(RQ.Supplier_Id == Guid.Empty))
                    {
                        stgSearch = from a in stgSearch
                                    join s in context.Supplier on a.SupplierName.Trim().TrimStart().ToUpper() equals s.Name.Trim().TrimStart().ToUpper()
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
                                     orderby a.stg_SupplierHotelRoomMapping_Id
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
                                         TX_RoomName = a.TX_RoomName,
                                         SupplierImportFile_Id = a.SupplierImportFile_Id ?? Guid.Empty,
                                         MinGuestOccupancy = a.MinGuestOccupancy,
                                         CityName = a.CityName,
                                         CityCode = a.CityCode,
                                         CountryName = a.CountryName,
                                         CountryCode = a.CountryCode,
                                         StateName = a.StateName,
                                         StateCode = a.StateCode
                                     }).Skip(skip).Take(RQ.PageSize).ToList();

                    return stgResult;
                }
            }
            catch (Exception e)
            {
                DL_Mapping _obj = new DL_Mapping();
                _obj.LogErrorMessage(RQ.SupplierImportFile_Id ?? Guid.Empty, e, "GetSTGRoomTypeData", "DL_UploadStaticData", "GetSTGRoomTypeData", (int)Error_Enums_DataHandler.ErrorCodes.RoomTypeConfig_Generic, "", "GetSTGRoomTypeData failed");
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus
                {
                    ErrorMessage = "Error while searching Supplier Data",
                    ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError
                });
            }
        }

        public List<DC_Accommodation_SupplierRoomTypeMapping_Online> RoomTypeMappingOnline_Insert(List<DC_Accommodation_SupplierRoomTypeMapping_Online> obj)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    //Mapp ID
                    var counterRTM = context.Counters.Where(w => w.TableName == "Accommodation_SupplierRoomTypeMapping" && w.ColumnName == "MapId").Select(s => s).FirstOrDefault();
                    int CurrentCounter = 0;
                    int CounterIncreaseBy = 0;

                    if (counterRTM != null)
                    {
                        CurrentCounter = counterRTM.LastCounterNo ?? 0;
                        CounterIncreaseBy = counterRTM.IncreaseBy ?? 0;
                    }

                    Guid Supplier_Id = Guid.Empty;

                    foreach (var data in obj)
                    {
                        //Check SupplierId
                        if (Supplier_Id == Guid.Empty)
                        {
                            Supplier_Id = context.Supplier.Where(w => w.Name == data.SupplierId || w.Code == data.SupplierId).Select(s => s.Supplier_Id).FirstOrDefault();
                            if (Supplier_Id == Guid.Empty)
                            {
                                continue;
                            }
                        }

                        //Check Accommodation_id
                        var AccommodationSearch = context.Accommodation.Where(w => w.TLGXAccoId == data.TLGXCommonHotelId).Select(s => new { s.Accommodation_Id, s.CompanyHotelID }).FirstOrDefault();

                        //check duplicate
                        var srtm = context.Accommodation_SupplierRoomTypeMapping.AsQueryable();

                        srtm = srtm.Where(w => w.Supplier_Id == Supplier_Id);

                        if (AccommodationSearch != null)
                        {
                            data.Accommodation_Id = AccommodationSearch.Accommodation_Id.ToString().ToUpper();

                            srtm = srtm.Where(w => w.Accommodation_Id == AccommodationSearch.Accommodation_Id);
                        }
                        else
                        {
                            srtm = srtm.Where(w => w.SupplierProductId == data.SupplierProductId);
                        }

                        if (!string.IsNullOrWhiteSpace(data.SupplierRoomTypeCode))
                        {
                            srtm = srtm.Where(w => w.SupplierRoomTypeCode == data.SupplierRoomTypeCode);
                        }
                        else
                        {
                            srtm = srtm.Where(w => w.SupplierRoomId == data.SupplierRoomId);
                        }

                        var dupeRecordFound = srtm.FirstOrDefault();

                        if (dupeRecordFound != null)
                        {
                            data.Accommodation_SupplierRoomType_Id = dupeRecordFound.Accommodation_SupplierRoomTypeMapping_Id.ToString().ToUpper();
                            data.Status = dupeRecordFound.MappingStatus;
                            data.SystemRoomTypeMapId = dupeRecordFound.MapId;
                            data.SystemProductCode = AccommodationSearch.CompanyHotelID;

                            if (dupeRecordFound.MatchingScore != null)
                            {
                                float matchingscore;
                                if (float.TryParse(Convert.ToString(dupeRecordFound.MatchingScore), out matchingscore))
                                {
                                    data.MatchingScore = data.MatchingScore;
                                }
                            }

                            dupeRecordFound.SupplierImportFile_Id = Guid.Parse(data.ProcessBatchId);
                            dupeRecordFound.ReRun_SupplierImportFile_Id = Guid.Parse(data.ProcessBatchId);
                            dupeRecordFound.Batch = data.ProcessBatchNo ?? 0;
                            dupeRecordFound.ReRun_Batch = data.ProcessBatchNo ?? 0;

                        }
                        else
                        {
                            Guid ASRTM_ID = Guid.NewGuid();
                            CurrentCounter = CurrentCounter + CounterIncreaseBy;

                            Accommodation_SupplierRoomTypeMapping newRTM = new Accommodation_SupplierRoomTypeMapping();

                            newRTM.Accommodation_Id = AccommodationSearch.Accommodation_Id;
                            newRTM.Accommodation_SupplierRoomTypeMapping_Id = ASRTM_ID;

                            if (data.Amenities != null)
                            {
                                newRTM.Amenities = string.Join(",", data.Amenities);
                            }

                            newRTM.BathRoomType = data.BathRoomType;
                            newRTM.BeddingConfig = data.BeddingConfig;
                            newRTM.Bedrooms = data.Bedrooms;
                            newRTM.BedTypeCode = data.BedType;

                            if (int.TryParse(data.ChildAge, out int ChildAge))
                            {
                                newRTM.ChildAge = ChildAge;
                            }

                            newRTM.CityCode = data.CityCode;
                            newRTM.CityName = data.CityName;
                            newRTM.CountryCode = data.CountryCode;
                            newRTM.CountryName = data.CountryName;
                            newRTM.StateCode = data.StateCode;
                            newRTM.StateName = data.StateName;

                            newRTM.Create_Date = DateTime.Now;
                            newRTM.Create_User = "TLGX_Datahandler";
                            newRTM.Edit_Date = null;
                            newRTM.Edit_User = null;
                            newRTM.ExtraBed = data.ExtraBed;
                            newRTM.FloorName = data.FloorName;

                            if (int.TryParse(data.FloorNumber, out int FloorNumber))
                            {
                                newRTM.FloorNumber = FloorNumber;
                            }

                            newRTM.MapId = CurrentCounter;
                            newRTM.MappingStatus = "UNMAPPED";
                            newRTM.MatchingScore = null;

                            if (int.TryParse(data.MaxAdults, out int MaxAdults))
                            {
                                newRTM.MaxAdults = MaxAdults;
                            }

                            if (int.TryParse(data.MaxChild, out int MaxChild))
                            {
                                newRTM.MaxChild = MaxChild;
                            }

                            if (int.TryParse(data.MaxGuestOccupancy, out int MaxGuestOccupancy))
                            {
                                newRTM.MaxGuestOccupancy = MaxGuestOccupancy;
                            }

                            if (int.TryParse(data.MaxInfants, out int MaxInfants))
                            {
                                newRTM.MaxInfants = MaxInfants;
                            }

                            if (int.TryParse(data.MinGuestOccupancy, out int MinGuestOccupancy))
                            {
                                newRTM.MinGuestOccupancy = MinGuestOccupancy;
                            }

                            if (int.TryParse(data.Quantity, out int Quantity))
                            {
                                newRTM.Quantity = Quantity;
                            }

                            newRTM.PromotionalVendorCode = data.PromotionalVendorCode;
                            newRTM.RatePlan = data.RatePlan;
                            newRTM.RatePlanCode = data.RatePlanCode;

                            newRTM.RoomDescription = data.RoomDescription;
                            newRTM.RoomLocationCode = data.RoomLocationCode;
                            newRTM.RoomSize = data.RoomSize;


                            newRTM.RoomViewCode = data.RoomView;
                            newRTM.Smoking = data.Smoking;

                            newRTM.stg_SupplierHotelRoomMapping_Id = null;

                            newRTM.SupplierImportFile_Id = Guid.Parse(data.ProcessBatchId);
                            newRTM.ReRun_Batch = data.ProcessBatchNo;
                            newRTM.ReRun_SupplierImportFile_Id = Guid.Parse(data.ProcessBatchId);
                            newRTM.Batch = data.ProcessBatchNo;

                            newRTM.SupplierName = data.SupplierId;
                            newRTM.SupplierProductId = data.SupplierProductId;
                            newRTM.SupplierProductName = data.SupplierProductName;
                            newRTM.SupplierProvider = data.SupplierProvider;
                            newRTM.SupplierRoomCategory = data.SupplierRoomCategory;
                            newRTM.SupplierRoomCategoryId = data.SupplierRoomCategoryId;
                            newRTM.SupplierRoomName = data.SupplierRoomName;
                            newRTM.SupplierRoomTypeCode = data.SupplierRoomTypeCode;
                            newRTM.Supplier_Id = Supplier_Id;

                            newRTM.Tx_ReorderedName = null;
                            newRTM.TX_RoomName = null;
                            newRTM.Tx_StrippedName = null;

                            //Added source in SRTM
                            newRTM.Source = "Online";

                            data.Accommodation_SupplierRoomType_Id = ASRTM_ID.ToString().ToUpper();
                            data.Status = "UNMAPPED";
                            data.SystemRoomTypeMapId = CurrentCounter;
                            if (AccommodationSearch != null)
                            {
                                data.SystemProductCode = AccommodationSearch.CompanyHotelID;
                                data.Accommodation_Id = AccommodationSearch.Accommodation_Id.ToString().ToUpper();
                            }

                            context.Accommodation_SupplierRoomTypeMapping.Add(newRTM);
                        }

                        context.SaveChanges();
                    }

                    //update counter
                    if (counterRTM != null)
                    {
                        var counterRow = context.Counters.Find(counterRTM.Counter_Id);
                        if (counterRow != null)
                        {
                            counterRow.LastCounterNo = CurrentCounter;
                            context.SaveChanges();
                        }
                    }
                }

                return obj;
            }
            catch (Exception e)
            {
                return obj;
            }
        }

        #endregion STG Tables

        #region De-Dupe Mapping data from STG tables

        public DataContracts.DC_Message DeDupe_EntityMapping_FromSTG(Guid SupplierImportFile_Id, string Entity)
        {
            DataContracts.DC_Message dc = new DataContracts.DC_Message();
            try
            {
                if (Entity == "Country")
                {
                    dc = DeDupe_CountryMapping_FromSTG(SupplierImportFile_Id);
                }
                else if (Entity == "City")
                {
                    dc = DeDupe_CityMapping_FromSTG(SupplierImportFile_Id);
                }
                else if (Entity == "Hotel")
                {
                    dc = DeDupe_ProductMapping_FromSTG(SupplierImportFile_Id);
                }
                else if (Entity == "RoomType")
                {
                    dc = DeDupe_RoomMapping_FromSTG(SupplierImportFile_Id);
                }
                else
                {
                    dc = new DC_Message { StatusCode = ReadOnlyMessage.StatusCode.Warning, StatusMessage = "Invalid Entity Type" };
                }
            }
            catch (Exception e)
            {
                dc.StatusMessage = ReadOnlyMessage.strFailed;
                dc.StatusCode = ReadOnlyMessage.StatusCode.Failed;

            }
            return dc;
        }

        public DataContracts.DC_Message DeDupe_CountryMapping_FromSTG(Guid SupplierImportFile_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    return new DC_Message { StatusCode = ReadOnlyMessage.StatusCode.Success, StatusMessage = "Success" };
                }
            }
            catch (Exception e)
            {
                return new DC_Message { StatusMessage = e.Message + Environment.NewLine + e.InnerException.Message, StatusCode = ReadOnlyMessage.StatusCode.Failed };
            }
        }

        public DataContracts.DC_Message DeDupe_CityMapping_FromSTG(Guid SupplierImportFile_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    return new DC_Message { StatusCode = ReadOnlyMessage.StatusCode.Success, StatusMessage = "Success" };
                }
            }
            catch (Exception e)
            {
                return new DC_Message { StatusMessage = e.Message + Environment.NewLine + e.InnerException.Message, StatusCode = ReadOnlyMessage.StatusCode.Failed };
            }

        }

        public DataContracts.DC_Message DeDupe_ProductMapping_FromSTG(Guid SupplierImportFile_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {

                    var SupplierFileDetails = context.SupplierImportFileDetails.Find(SupplierImportFile_Id);

                    if (SupplierFileDetails != null)
                    {
                        Guid SupplierId = SupplierFileDetails.Supplier_Id;

                        if (SupplierId != Guid.Empty)
                        {
                            StringBuilder sql = new StringBuilder();
                            sql.AppendLine("DECLARE @SupplierImportFile_Id AS UNIQUEIDENTIFIER = '" + SupplierImportFile_Id.ToString() + "' ");
                            sql.AppendLine("DECLARE @Supplier_Id AS UNIQUEIDENTIFIER = '" + SupplierId.ToString() + "' ");
                            sql.AppendLine("DECLARE @Country_Id AS UNIQUEIDENTIFIER ");
                            sql.AppendLine("DECLARE db_cursor CURSOR FOR ");
                            sql.AppendLine("SELECT DISTINCT Country_Id FROM stg_SupplierProductMapping WHERE SupplierImportFile_Id = @SupplierImportFile_Id ");
                            sql.AppendLine("OPEN db_cursor");
                            sql.AppendLine("FETCH NEXT FROM db_cursor INTO @Country_Id ");
                            sql.AppendLine("WHILE @@FETCH_STATUS = 0 ");
                            sql.AppendLine("BEGIN  ");
                            sql.AppendLine("UPDATE APM SET APM.IsActive = 0 FROM Accommodation_ProductMapping APM WITH(NOLOCK) ");
                            sql.AppendLine("LEFT JOIN(Select stg_AccoMapping_Id, ProductId from stg_SupplierProductMapping WITH (NOLOCK) where ");
                            sql.AppendLine("SupplierImportFile_Id = @SupplierImportFile_Id and Supplier_Id = @Supplier_Id and Country_Id = @Country_Id) STG ");
                            sql.AppendLine("ON STG.ProductId = APM.SupplierProductReference ");
                            sql.AppendLine("WHERE APM.Supplier_Id = @Supplier_Id AND APM.Country_Id = @Country_Id AND STG.stg_AccoMapping_Id IS NULL ");
                            sql.AppendLine("FETCH NEXT FROM db_cursor INTO @Country_Id  ");
                            sql.AppendLine("END ");
                            sql.AppendLine("CLOSE db_cursor  ");
                            sql.AppendLine("DEALLOCATE db_cursor ");

                            context.Database.CommandTimeout = 0;

                            context.Database.ExecuteSqlCommand(sql.ToString());

                            return new DC_Message { StatusCode = ReadOnlyMessage.StatusCode.Success, StatusMessage = "Success" };
                        }
                        else
                        {
                            return new DC_Message { StatusCode = ReadOnlyMessage.StatusCode.Warning, StatusMessage = "Invalid Supplier Id" };
                        }
                    }
                    else
                    {
                        return new DC_Message { StatusCode = ReadOnlyMessage.StatusCode.Warning, StatusMessage = "Invalid Supplier Import File Id" };
                    }
                }
            }
            catch (Exception e)
            {
                return new DC_Message { StatusMessage = e.Message + Environment.NewLine + e.InnerException.Message, StatusCode = ReadOnlyMessage.StatusCode.Failed };
            }

        }

        public DataContracts.DC_Message DeDupe_RoomMapping_FromSTG(Guid SupplierImportFile_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    return new DC_Message { StatusCode = ReadOnlyMessage.StatusCode.Success, StatusMessage = "Success" };
                }
            }
            catch (Exception e)
            {
                return new DC_Message { StatusMessage = e.Message + Environment.NewLine + e.InnerException.Message, StatusCode = ReadOnlyMessage.StatusCode.Failed };
            }
        }

        #endregion

        #region STG Cleanup
        public DataContracts.DC_Message STG_Cleanup(Guid SupplierImportFile_Id, string Entity)
        {
            DataContracts.DC_Message dc = new DataContracts.DC_Message();
            try
            {
                if (Entity == "Country")
                {
                    dc = STG_Country_Cleanup(SupplierImportFile_Id);
                }
                else if (Entity == "City")
                {
                    dc = STG_City_Cleanup(SupplierImportFile_Id);
                }
                else if (Entity == "Hotel")
                {
                    dc = STG_Hotel_Cleanup(SupplierImportFile_Id);
                }
                else if (Entity == "RoomType")
                {
                    dc = STG_Room_Cleanup(SupplierImportFile_Id);
                }
                else
                {
                    dc = new DC_Message { StatusCode = ReadOnlyMessage.StatusCode.Warning, StatusMessage = "Invalid Entity Type" };
                }
            }
            catch (Exception e)
            {
                dc.StatusMessage = ReadOnlyMessage.strFailed;
                dc.StatusCode = ReadOnlyMessage.StatusCode.Failed;

            }
            return dc;
        }

        public DataContracts.DC_Message STG_Country_Cleanup(Guid SupplierImportFile_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    int x = context.Database.ExecuteSqlCommand("Delete from stg_SupplierCountryMapping where SupplierImportFile_Id = @p0", SupplierImportFile_Id);
                    return new DC_Message { StatusCode = ReadOnlyMessage.StatusCode.Success, StatusMessage = x + " Rows affected." };
                }
            }
            catch (Exception e)
            {
                return new DC_Message { StatusMessage = e.Message + Environment.NewLine + e.InnerException.Message, StatusCode = ReadOnlyMessage.StatusCode.Failed };
            }
        }

        public DataContracts.DC_Message STG_City_Cleanup(Guid SupplierImportFile_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    int x = context.Database.ExecuteSqlCommand("Delete from stg_SupplierCityMapping where SupplierImportFile_Id = @p0", SupplierImportFile_Id);
                    return new DC_Message { StatusCode = ReadOnlyMessage.StatusCode.Success, StatusMessage = x + " Rows affected." };
                }
            }
            catch (Exception e)
            {
                return new DC_Message { StatusMessage = e.Message + Environment.NewLine + e.InnerException.Message, StatusCode = ReadOnlyMessage.StatusCode.Failed };
            }
        }

        public DataContracts.DC_Message STG_Hotel_Cleanup(Guid SupplierImportFile_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    int x = context.Database.ExecuteSqlCommand("Delete from stg_SupplierProductMapping where SupplierImportFile_Id = @p0", SupplierImportFile_Id);
                    return new DC_Message { StatusCode = ReadOnlyMessage.StatusCode.Success, StatusMessage = x + " Rows affected." };
                }
            }
            catch (Exception e)
            {
                return new DC_Message { StatusMessage = e.Message + Environment.NewLine + e.InnerException.Message, StatusCode = ReadOnlyMessage.StatusCode.Failed };
            }
        }

        public DataContracts.DC_Message STG_Room_Cleanup(Guid SupplierImportFile_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    int x = context.Database.ExecuteSqlCommand("Delete from stg_SupplierHotelRoomMapping where SupplierImportFile_Id = @p0", SupplierImportFile_Id);
                    return new DC_Message { StatusCode = ReadOnlyMessage.StatusCode.Success, StatusMessage = x + " Rows affected." };
                }
            }
            catch (Exception e)
            {
                return new DC_Message { StatusMessage = e.Message + Environment.NewLine + e.InnerException.Message, StatusCode = ReadOnlyMessage.StatusCode.Failed };
            }
        }

        #endregion

        #region Get Record Count from STG tables

        public int Get_STG_Record_Count(Guid SupplierImportFile_Id, string Entity)
        {
            int RecordCount = 0;
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (Entity == "Country")
                    {
                        RecordCount = context.stg_SupplierCountryMapping.AsNoTracking().Where(w => w.SupplierImportFile_Id == SupplierImportFile_Id).Count();
                    }
                    else if (Entity == "City")
                    {
                        RecordCount = context.stg_SupplierCityMapping.AsNoTracking().Where(w => w.SupplierImportFile_Id == SupplierImportFile_Id).Count();
                    }
                    else if (Entity == "Hotel")
                    {
                        RecordCount = context.stg_SupplierProductMapping.AsNoTracking().Where(w => w.SupplierImportFile_Id == SupplierImportFile_Id).Count();
                    }
                    else if (Entity == "RoomType")
                    {
                        RecordCount = context.stg_SupplierHotelRoomMapping.AsNoTracking().Where(w => w.SupplierImportFile_Id == SupplierImportFile_Id).Count();
                    }
                    else
                    {
                        RecordCount = 0;
                    }
                }
            }
            catch (Exception e)
            {
                RecordCount = 0;
            }
            return RecordCount;
        }

        #endregion

        #region Process Or Test Uploaded Files
        public DataContracts.DC_Message StaticFileUploadProcessFile(DataContracts.UploadStaticData.DC_SupplierImportFileDetails obj)
        {
            DataContracts.DC_Message dc = new DataContracts.DC_Message();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var FileRecord = (from a in context.SupplierImportFileDetails
                                      where a.SupplierImportFile_Id == obj.SupplierImportFile_Id && a.STATUS == "UPLOADED" || a.STATUS == "SCHEDULED" || a.STATUS == "RESUMED"
                                      select a).FirstOrDefault();

                    if (FileRecord != null)
                    {
                        FileRecord.STATUS = "PROCESSING";
                        FileRecord.PROCESS_USER = obj.PROCESS_USER;
                        FileRecord.PROCESS_DATE = DateTime.Now;
                        context.SaveChanges();
                    }
                    else
                    {
                        return new DC_Message { StatusCode = ReadOnlyMessage.StatusCode.Duplicate, StatusMessage = "File is already processed." };
                    }
                }

                DHSVC.DC_SupplierImportFileDetails_TestProcess file = new DHSVC.DC_SupplierImportFileDetails_TestProcess();
                file.SupplierImportFile_Id = obj.SupplierImportFile_Id;
                file.Supplier_Id = obj.Supplier_Id;
                file.SavedFilePath = obj.SavedFilePath;
                file.PROCESS_USER = obj.PROCESS_USER;
                file.Entity = obj.Entity;
                file.STATUS = obj.STATUS;
                file.Supplier = obj.Supplier;
                file.Mode = obj.Mode;

                file.IsStopped = obj.IsStopped;
                file.IsRestarted = obj.IsRestarted;
                file.IsPaused = obj.IsPaused;
                file.IsResumed = obj.IsResumed;
                file.CurrentBatch = obj.CurrentBatch;


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

        #region Update Supplier ImportFile Details
        public DataContracts.DC_Message UpdateSupplierImportFileDetails(DataContracts.UploadStaticData.DC_SupplierImportFileDetails RQ)
        {
            DataContracts.DC_Message dc = new DataContracts.DC_Message();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var searchFileId = context.SupplierImportFileDetails.Find(RQ.SupplierImportFile_Id);

                    if (searchFileId != null)
                    {
                        searchFileId.IsStopped = RQ.IsStopped;
                        searchFileId.IsRestarted = RQ.IsRestarted;
                        searchFileId.IsPaused = RQ.IsPaused;
                        searchFileId.IsResumed = RQ.IsResumed;
                        searchFileId.CurrentBatch = RQ.CurrentBatch;

                        if (RQ.IsRestarted ?? false == true)
                        {
                            //check for mode ALL/RERUN
                            if (searchFileId.Mode == null || searchFileId.Mode == "ALL")
                            {
                                searchFileId.STATUS = "UPLOADED";
                            }
                            else if (searchFileId.Mode == "RE_RUN")
                            {
                                searchFileId.STATUS = "SCHEDULED";
                            }

                            //Clear All the logs
                            context.SupplierImportFile_ErrorLog.RemoveRange(context.SupplierImportFile_ErrorLog.Where(w => w.SupplierImportFile_Id == RQ.SupplierImportFile_Id));
                            context.SupplierImportFile_Progress.RemoveRange(context.SupplierImportFile_Progress.Where(w => w.SupplierImportFile_Id == RQ.SupplierImportFile_Id));
                            context.SupplierImportFile_Statistics.RemoveRange(context.SupplierImportFile_Statistics.Where(w => w.SupplierImportFile_Id == RQ.SupplierImportFile_Id));
                            context.SupplierImportFile_VerboseLog.RemoveRange(context.SupplierImportFile_VerboseLog.Where(w => w.SupplierImportFile_Id == RQ.SupplierImportFile_Id));

                        }
                        else if (RQ.IsResumed ?? false == true)
                        {
                            searchFileId.STATUS = "RESUMED";
                        }
                        else if (RQ.IsPaused ?? false == true)
                        {
                            searchFileId.STATUS = "PAUSED";
                        }
                        else if (RQ.IsStopped ?? false == true)
                        {
                            searchFileId.STATUS = "STOPPED";
                        }

                        context.SaveChanges();
                    }
                }
                return new DC_Message { StatusCode = ReadOnlyMessage.StatusCode.Success, StatusMessage = "Status Updated Successfully" };
            }
            catch (Exception ex)
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while updating supplier import details status", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }

        }


        public DataContracts.DC_Message UpdateSupplierImportFileDetailsFromNewToUploaded(DataContracts.UploadStaticData.DC_SupplierImportFileDetails RQ)
        {
            DataContracts.DC_Message dc = new DataContracts.DC_Message();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {


                    if (RQ != null)
                    {
                        StringBuilder sbUpdateSRTMStatus = new StringBuilder();


                        sbUpdateSRTMStatus.Clear();
                        sbUpdateSRTMStatus.Append(" UPDATE SupplierImportFileDetails SET STATUS = 'UPLOADED' where Supplier_Id = '" + RQ.Supplier_Id + "'");
                        sbUpdateSRTMStatus.Append(" and Entity='" + RQ.Entity + "' and IsActive = 1 and STATUS = '" + RQ.STATUS + "';");

                        if (sbUpdateSRTMStatus.Length > 0)
                        {
                            try { context.Database.ExecuteSqlCommand(sbUpdateSRTMStatus.ToString()); } catch (Exception ex) { }
                        }
                    }
                }
                return new DC_Message { StatusCode = ReadOnlyMessage.StatusCode.Success, StatusMessage = "Status Updated Successfully" };
            }
            catch (Exception ex)
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while updating supplier import details status", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }

        }
        #endregion

        #region Update SRT By ML
        public DataContracts.DC_SRT_ML_Response_Broker DataHandler_UpdateSRTByML(DataContracts.DC_SRT_ML_Request_Broker RQ)
        {
            DataContracts.DC_SRT_ML_Response_Broker _objRS = new DataContracts.DC_SRT_ML_Response_Broker();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var request = (HttpWebRequest)WebRequest.Create(System.Configuration.ConfigurationManager.AppSettings["MLSVCURL_Broker"]);
                    request.Timeout = System.Threading.Timeout.Infinite;
                    request.ReadWriteTimeout = System.Threading.Timeout.Infinite;
                    request.KeepAlive = false;

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
                    //request.Credentials = CredentialCache.DefaultCredentials;

                    DataContractJsonSerializer serializerToUpload = new DataContractJsonSerializer(typeof(DataContracts.DC_SRT_ML_Request_Broker));

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
                        _objRS = null;
                    }
                    else
                    {
                        var encoding = ASCIIEncoding.UTF8;
                        using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                        {
                            string responseText = reader.ReadToEnd();

                            var jsonSerializerSettings = new JsonSerializerSettings
                            {
                                MissingMemberHandling = MissingMemberHandling.Ignore
                            };
                            _objRS = JsonConvert.DeserializeObject<DataContracts.DC_SRT_ML_Response_Broker>(responseText, jsonSerializerSettings);
                        }
                    }

                    serializerToUpload = null;
                    response.Dispose();
                    response = null;
                    request = null;
                }
                return _objRS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region File Processing Check
        //GAURAV_TMAP_746
        public DataContracts.DC_Message FileProcessingCheckInSupplierImportFileDetails(string SupplierId)
        {
            DataContracts.DC_Message dc = new DataContracts.DC_Message();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    int count = 0;
                    StringBuilder sb = new StringBuilder();


                    sb.Append(@" Select Count(1) from SupplierImportFileDetails where status not in ('Scheduled', 'UPloaded', 'New', 'Stopped', 'Paused', 'Error', 'Processed', 'Resumed', 'PROCESS LATER')
                    and Supplier_Id = '" + SupplierId + "'");



                    try { count = context.Database.SqlQuery<int>(sb.ToString()).FirstOrDefault(); } catch (Exception ex) { }

                    if (count > 0)
                    {
                        dc.StatusMessage = "RUNNING";
                    }
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching accomodation for autocomplete", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }

            return dc;
        }
        #endregion
    }
}
