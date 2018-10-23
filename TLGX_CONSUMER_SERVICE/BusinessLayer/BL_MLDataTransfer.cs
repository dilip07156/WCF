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

        public List<DataContracts.ML.DL_ML_DL_EntityStatus> GetMLDataApiTransferStatus()
        {
            using (DL_MLDataTransfer objDL = new DL_MLDataTransfer())
            {
               return objDL.GetMLDataApiTransferStatus();
            }
        }

        #region*** Training Data Delete ***
        public void ML_DataTransfer_DeleteTrainingData(string accommodation_SupplierRoomTypeMapping_Id)
        {
            using (DL_MLDataTransfer objDL = new DL_MLDataTransfer())
            {
                objDL.ML_DataTransfer_DeleteTrainingData(Guid.Parse(accommodation_SupplierRoomTypeMapping_Id));
            }
        }
        #endregion

        #region *** Training Data push(+ve & -ve) ***
        public void ML_DataTransfer_TrainingDataPushToAIML(string accommodation_SupplierRoomTypeMapping_Id)
        {
            using (DL_MLDataTransfer objDL = new DL_MLDataTransfer())
            {
                objDL.ML_DataTransfer_TrainingDataPushToAIML(Guid.Parse(accommodation_SupplierRoomTypeMapping_Id));
            }
        }
        #endregion
    }
}
