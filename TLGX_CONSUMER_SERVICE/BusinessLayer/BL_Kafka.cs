using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class BL_Kafka : IDisposable
    {
        public void Dispose()
        {
           
        }

        public DC_Message InsertKafkaInfo(DataContracts.STG.DC_Stg_Kafka KafkaInfo)
        {
            using (DataLayer.DL_Kafka obj = new DataLayer.DL_Kafka())
            {
                return obj.InsertKafkaInfo(KafkaInfo);
            }
        }

        public DC_Message UpdateKafkaInfo(DataContracts.STG.DC_Stg_Kafka KafkaInfo)
        {
            using (DataLayer.DL_Kafka obj = new DataLayer.DL_Kafka())
            {
                return obj.UpdateKafkaInfo(KafkaInfo);
            }
        }

        public IList<DataContracts.STG.DC_Stg_Kafka> SelectKafkaInfo(string Row_Id)
        {
            Guid gRow_Id;

            if (!Guid.TryParse(Row_Id, out gRow_Id))
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
            }
            else
            {
                using (DataLayer.DL_Kafka objBL = new DataLayer.DL_Kafka())
                {
                    return objBL.SelectKafkaInfo(gRow_Id);
                }
            }

        }
    }
}
