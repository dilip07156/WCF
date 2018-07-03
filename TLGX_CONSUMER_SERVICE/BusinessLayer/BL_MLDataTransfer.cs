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
        public void ML_DataTransferMasterAccommodation(Guid Logid)
        {
            using (DL_MLDataTransfer objDL = new DL_MLDataTransfer())
            {
                objDL.ML_DataTransferMasterAccommodation(Logid);
            }
        }
        #endregion
        #region *** MasterAccommodationRoomFacilities ***
        public void ML_DataTransferMasterAccommodationRoomFacilities(Guid Logid)
        {
            using (DL_MLDataTransfer objDL = new DL_MLDataTransfer())
            {
                objDL.ML_DataTransferMasterAccommodationRoomFacilities(Logid);
            }
        }
        #endregion
        #region *** MasterAccommodationRoomInformation ***
        public void ML_DataTransferMasterAccommodationRoomInformation(Guid Logid)
        {
            using (DL_MLDataTransfer objDL = new DL_MLDataTransfer())
            {
                objDL.ML_DataTransferMasterAccommodationRoomInformation(Logid);
            }
        }
        #endregion
        #region *** RoomTypeMatching ***
        public void ML_DataTransferRoomTypeMatching(Guid Logid)
        {
            using (DL_MLDataTransfer objDL = new DL_MLDataTransfer())
            {
                objDL.ML_DataTransferRoomTypeMatching(Logid);
            }
        }
        #endregion
        #region *** SupplierAccommodationData ***
        public void ML_DataTransferSupplierAccommodationData(Guid Logid)
        {
            using (DL_MLDataTransfer objDL = new DL_MLDataTransfer())
            {
                objDL.ML_DataTransferSupplierAccommodationData(Logid);
            }
        }
        #endregion
        #region *** SupplierAccommodationRoomData ***
        public void ML_DataTransferSupplierAccommodationRoomData(Guid Logid)
        {
            using (DL_MLDataTransfer objDL = new DL_MLDataTransfer())
            {
                objDL.ML_DataTransferSupplierAccommodationRoomData(Logid);
            }
        }
        #endregion
        #region *** SupplierAccommodationRoomExtendedAttributes ***
        public void ML_DataTransferSupplierAccommodationRoomExtendedAttributes(Guid Logid)
        {
            using (DL_MLDataTransfer objDL = new DL_MLDataTransfer())
            {
                objDL.ML_DataTransferSupplierAccommodationRoomExtendedAttributes(Logid);
            }
        }
        #endregion
    }
}
