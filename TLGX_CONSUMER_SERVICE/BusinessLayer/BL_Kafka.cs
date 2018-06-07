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

        public List<DataContracts.STG.DC_Stg_Kafka> GetPollData()
        {           
                using (DataLayer.DL_Kafka objBL = new DataLayer.DL_Kafka())
                {
                    return objBL.GetPollData();
                }
        }
    }
}
