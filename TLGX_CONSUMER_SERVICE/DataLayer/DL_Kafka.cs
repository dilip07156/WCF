using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DataContracts.Masters;

namespace DataLayer
{
    public class DL_Kafka : IDisposable
    {
        public void Dispose()
        {

        }

        public DC_Message InsertKafkaInfo(DataContracts.STG.DC_Stg_Kafka KafkaInfo)
        {
            DC_Message _msg = new DC_Message();
            using (ConsumerEntities context = new ConsumerEntities())
            {
                try
                {
                    if (KafkaInfo != null)
                    {
                        Stg_Kafka sk = new Stg_Kafka()
                        {
                            Row_Id = KafkaInfo.Row_Id,
                            Status = KafkaInfo.Status,
                            Topic = KafkaInfo.Topic,
                            PayLoad = KafkaInfo.PayLoad,
                            Error = KafkaInfo.Error,
                            Key = KafkaInfo.Key,
                            Offset = KafkaInfo.Offset,
                            Partion = KafkaInfo.Partion,
                            //TimeStamp = KafkaInfo.TimeStamp.get,
                            TopicPartion = KafkaInfo.TopicPartion,
                            TopicPartionOffset = KafkaInfo.TopicPartionOffset,
                            Create_User = KafkaInfo.Create_User,
                            Create_Date = KafkaInfo.Create_Date,
                            Process_User = KafkaInfo.Process_User,
                            Process_Date = KafkaInfo.Process_Date,

                        };
                        context.Stg_Kafka.Add(sk);
                        if (context.SaveChanges() == 1)
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strAddedSuccessfully;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        }
                        else
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strFailed;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                        }

                    }
                    else
                    {
                        _msg.StatusMessage = ReadOnlyMessage.strFailed;
                        _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                    }
                    return _msg;
                }

                catch (Exception ex)
                {
                    throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while inserting Kafka details", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
                }

            }
        }


        public DC_Message UpdateKafkaInfo(DataContracts.STG.DC_Stg_Kafka KafkaInfo)
        {
            DC_Message _msg = new DC_Message();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = context.Stg_Kafka.Find(KafkaInfo.Row_Id);


                    if (search != null)
                    {
                        if (search.Status != KafkaInfo.Status && KafkaInfo.Status != "") { search.Status = KafkaInfo.Status; };
                        search.Key = KafkaInfo.Key;
                        search.Offset = KafkaInfo.Offset;
                        //search.TimeStamp = KafkaInfo.TimeStamp;
                        search.Partion = KafkaInfo.Partion;
                        search.TopicPartion = KafkaInfo.TopicPartion;
                        search.TopicPartionOffset = KafkaInfo.TopicPartionOffset;
                        search.Process_User = KafkaInfo.Process_User;
                        search.Process_Date = DateTime.Now;
                        context.SaveChanges();
                    }
                    return _msg;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating Kafka details", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

                
        public List<DataContracts.STG.DC_Stg_Kafka> GetPollData()
        {
            try
            {
                List<DataContracts.STG.DC_Stg_Kafka> _lstresult = new List<DataContracts.STG.DC_Stg_Kafka>();
                using (ConsumerEntities context = new ConsumerEntities())
                {
                   
                     _lstresult = (from a in context.Stg_Kafka                                   
                                   select new DataContracts.STG.DC_Stg_Kafka
                                  {
                                      Row_Id=a.Row_Id,
                                      Topic = a.Topic,
                                      PayLoad = a.PayLoad                                      
                                  }).ToList();

                    return _lstresult;

                }
            }

            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while selecting Kafka details", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }

        }

    }
}
