using BusinessLayer;
using OperationContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConsumerSvc
{
    public partial class Consumer : IConsumer
    {
        #region *** MasterAccommodationRecord ***
        public void ML_DataTransferMasterAccommodation(string Logid)
        {
            using (BL_MLDataTransfer objBL = new BL_MLDataTransfer())
            {
                objBL.ML_DataTransferMasterAccommodation(Guid.Parse(Logid));
            }
        }
        #endregion
        #region *** MasterAccommodationRoomFacilities ***
        public void ML_DataTransferMasterAccommodationRoomFacilities(string Logid)
        {
            using (BL_MLDataTransfer objBL = new BL_MLDataTransfer())
            {
                objBL.ML_DataTransferMasterAccommodationRoomFacilities(Guid.Parse(Logid));
            }
        }
        #endregion
        #region *** MasterAccommodationRoomInformation ***
        public void ML_DataTransferMasterAccommodationRoomInformation(string Logid)
        {
            using (BL_MLDataTransfer objBL = new BL_MLDataTransfer())
            {
                objBL.ML_DataTransferMasterAccommodationRoomInformation(Guid.Parse(Logid));
            }
        }
        #endregion
        #region *** RoomTypeMatching ***
        public void ML_DataTransferRoomTypeMatching(string Logid)
        {
            using (BL_MLDataTransfer objBL = new BL_MLDataTransfer())
            {
                objBL.ML_DataTransferRoomTypeMatching(Guid.Parse(Logid));
            }
        }
        #endregion
        #region ***SupplierAccommodationData ***
        public void ML_DataTransferSupplierAccommodationData(string Logid)
        {
            using (BL_MLDataTransfer objBL = new BL_MLDataTransfer())
            {
                objBL.ML_DataTransferSupplierAccommodationData(Guid.Parse(Logid));
            }
        }
        #endregion
        #region *** SupplierAccommodationRoomData ***
        public void ML_DataTransferSupplierAccommodationRoomData(string Logid)
        {
            using (BL_MLDataTransfer objBL = new BL_MLDataTransfer())
            {
                objBL.ML_DataTransferSupplierAccommodationRoomData(Guid.Parse(Logid));
            }
        }
        #endregion
        #region *** SupplierAccommodationRoomExtendedAttributes ***
        public void ML_DataTransferSupplierAccommodationRoomExtendedAttributes(string Logid)
        {
            using (BL_MLDataTransfer objBL = new BL_MLDataTransfer())
            {
                objBL.ML_DataTransferSupplierAccommodationRoomExtendedAttributes(Guid.Parse(Logid));
            }
        }
        #endregion

        public List<DataContracts.ML.DL_ML_DL_EntityStatus> GetMLDataApiTransferStatus()
        {
            using (BL_MLDataTransfer objBL = new BL_MLDataTransfer())
            {
                return objBL.GetMLDataApiTransferStatus();
            }

        }
    }
}