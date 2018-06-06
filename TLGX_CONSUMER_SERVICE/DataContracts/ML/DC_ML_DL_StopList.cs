using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.ML
{
    public class DC_ML_DL_StopList
    {
        public List<DC_ML_DL_StopList_Data> data { get; set; }
    }

    public class DC_ML_DL_StopList_Data
    {
        public string StopWordId { get; set; }
        public string StopWord { get; set; }
        public string StopType { get; set; }
        public string CreateDate { get; set; }
        public string CreateUser { get; set; }
        public string EditDate { get; set; }
        public string Edituser { get; set; }
    }

}
