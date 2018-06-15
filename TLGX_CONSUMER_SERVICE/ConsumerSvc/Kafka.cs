using BusinessLayer;
using DataContracts;
using OperationContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataContracts.Masters;


namespace ConsumerSvc
{
    public partial class Consumer : IConsumer
    {

        public DC_Message InsertKafkaInfo(DataContracts.STG.DC_Stg_Kafka KafkaInfo)
        {
            using (BL_Kafka objBL = new BL_Kafka())
            {
                return objBL.InsertKafkaInfo(KafkaInfo);
            }
        }

        public DC_Message UpdateKafkaInfo(DataContracts.STG.DC_Stg_Kafka KafkaInfo)
        {
            using (BL_Kafka objBL = new BL_Kafka())
            {
                return objBL.UpdateKafkaInfo(KafkaInfo);
            }
        }
               
        public List<DataContracts.STG.DC_Stg_Kafka> GetPollData()
        {
            using (BL_Kafka objBL = new BL_Kafka())
            {
                return objBL.GetPollData();
            }
        }

        public List<DataContracts.STG.DC_Stg_Kafka> SelectKafkaInfo(string Row_Id)
        {
            using (BL_Kafka objBL = new BL_Kafka())
            {
                return objBL.SelectKafkaInfo(Row_Id);
            }
        }
    }
}