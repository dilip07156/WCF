using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.ML
{
    public class DC_ML_DL_Response
    {
        public string Mode { get; set; }
        public string BatchId { get; set; }
        public string Transaction { get; set; }
        public string Status { get; set; }
    }
}
