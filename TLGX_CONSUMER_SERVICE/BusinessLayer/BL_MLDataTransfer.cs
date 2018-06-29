using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class BL_MLDataTransfer : IDisposable
    {
        public void Dispose()
        {
        }

        #region *** MasterAccommodationRecord ***
        public string ML_DataTransferMasterAccommodation()
        {
            using (DL_MLDataTransfer objDL = new DL_MLDataTransfer())
            {
                return objDL.ML_DataTransferMasterAccommodation();
            }
        }
        #endregion
        #region *** MasterAccommodationRoomFacilities ***
        public string ML_DataTransferMasterAccommodationRoomFacilities()
        {
            using (DL_MLDataTransfer objDL = new DL_MLDataTransfer())
            {
                return objDL.ML_DataTransferMasterAccommodationRoomFacilities();
            }
        }
        #endregion
        #region *** MasterAccommodationRoomInformation ***
        public string ML_DataTransferMasterAccommodationRoomInformation()
        {
            using (DL_MLDataTransfer objDL = new DL_MLDataTransfer())
            {
                return objDL.ML_DataTransferMasterAccommodationRoomInformation();
            }
        }
        #endregion
        #region *** RoomTypeMatching ***
        public string ML_DataTransferRoomTypeMatching()
        {
            using (DL_MLDataTransfer objDL = new DL_MLDataTransfer())
            {
                return objDL.ML_DataTransferRoomTypeMatching();
            }
        }
        #endregion
        #region *** SupplierAccommodationData ***
        public string ML_DataTransferSupplierAccommodationData()
        {
            using (DL_MLDataTransfer objDL = new DL_MLDataTransfer())
            {
                return objDL.ML_DataTransferSupplierAccommodationData();
            }
        }
        #endregion
        #region *** SupplierAccommodationRoomData ***
        public string ML_DataTransferSupplierAccommodationRoomData()
        {
            using (DL_MLDataTransfer objDL = new DL_MLDataTransfer())
            {
                return objDL.ML_DataTransferSupplierAccommodationRoomData();
            }
        }
        #endregion
        #region *** SupplierAccommodationRoomExtendedAttributes ***
        public string ML_DataTransferSupplierAccommodationRoomExtendedAttributes()
        {
            using (DL_MLDataTransfer objDL = new DL_MLDataTransfer())
            {
                return objDL.ML_DataTransferSupplierAccommodationRoomExtendedAttributes();
            }
        }
        #endregion
    }
}
