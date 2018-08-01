using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public class DC_SqlTableColumnInfo
    {
        public string COLUMN_NAME { get; set; }
        public int CHARACTER_MAXIMUM_LENGTH { get; set; }
        public string COLUMNDEFAULT { get; set; }
        public string IS_NULLABLE { get; set; }
        public int NUMERIC_PRECISION { get; set; }
        public int NUMERIC_SCALE { get; set; }
    }
}
