using System.ServiceModel;
using System.ServiceModel.Web;
using System.Collections.Generic;
using System;

namespace OperationContracts
{
    [ServiceContract]
    public interface IConsumer : IAccomodation, IMasters, IGeoLocation, IMapping, IAdmin, IStaticData, ISchedule, IUploadStaticData, IPentaho, IActivity
    {

    }
}
