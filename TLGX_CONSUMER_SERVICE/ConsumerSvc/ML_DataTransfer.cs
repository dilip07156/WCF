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
        public string ML_DataTransferMasterAccommodation()
        {
            using (BL_MLDataTransfer objBL = new BL_MLDataTransfer())
            {
                return objBL.ML_DataTransferMasterAccommodation();
            }
        }
        #endregion
        #region *** MasterAccommodationRoomFacilities ***
        public string ML_DataTransferMasterAccommodationRoomFacilities()
        {
            using (BL_MLDataTransfer objBL = new BL_MLDataTransfer())
            {
                return objBL.ML_DataTransferMasterAccommodationRoomFacilities();
            }
        }
        #endregion
        #region *** MasterAccommodationRoomInformation ***
        public string ML_DataTransferMasterAccommodationRoomInformation()
        {
            using (BL_MLDataTransfer objBL = new BL_MLDataTransfer())
            {
                return objBL.ML_DataTransferMasterAccommodationRoomInformation();
            }
        }
        #endregion
        #region *** RoomTypeMatching ***
        public string ML_DataTransferRoomTypeMatching()
        {
            using (BL_MLDataTransfer objBL = new BL_MLDataTransfer())
            {
                return objBL.ML_DataTransferRoomTypeMatching();
            }
        }
        #endregion
        #region ***SupplierAccommodationData ***
        public string ML_DataTransferSupplierAccommodationData()
        {
            using (BL_MLDataTransfer objBL = new BL_MLDataTransfer())
            {
                return objBL.ML_DataTransferSupplierAccommodationData();
            }
        }
        #endregion
        #region *** SupplierAccommodationRoomData ***
        public string ML_DataTransferSupplierAccommodationRoomData()
        {
            using (BL_MLDataTransfer objBL = new BL_MLDataTransfer())
            {
                return objBL.ML_DataTransferSupplierAccommodationRoomData();
            }
        }
        #endregion
        #region *** SupplierAccommodationRoomExtendedAttributes ***
        public string ML_DataTransferSupplierAccommodationRoomExtendedAttributes()
        {
            using (BL_MLDataTransfer objBL = new BL_MLDataTransfer())
            {
                return objBL.ML_DataTransferSupplierAccommodationRoomExtendedAttributes();
            }
        }
        #endregion
    }
}