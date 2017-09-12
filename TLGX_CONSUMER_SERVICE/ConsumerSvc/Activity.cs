using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using OperationContracts;
using DataContracts;
using BusinessLayer;
using DataContracts.Masters;

namespace ConsumerSvc
{
    public partial class Consumer : IConsumer
    {
        #region Activity Search
        public IList<DC_ActivitySearch_RS> ActivitySearch(DC_Activity_Search_RQ Activity_Request)
        {
            using (BL_Activity objBL = new BL_Activity())
            {
                List<DC_ActivitySearch_RS> searchResults = new List<DC_ActivitySearch_RS>();
                searchResults = objBL.ActivitySearch(Activity_Request);

                if (searchResults == null)
                {
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                }
                return searchResults;
            }
        }
        #endregion
    }
}