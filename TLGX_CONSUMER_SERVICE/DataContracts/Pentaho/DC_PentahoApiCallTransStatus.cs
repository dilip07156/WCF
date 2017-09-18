using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace DataContracts.Pentaho
{
    [XmlRoot(ElementName = "stepstatus")]
    [DataContract]
    public class DC_PentahoTransStatus_Stepstatus
    {

        [DataMember]
        [XmlElement(ElementName = "stepname")]
        public string Stepname { get; set; }

        [DataMember]
        [XmlElement(ElementName = "copy")]
        public string Copy { get; set; }

        [DataMember]
        [XmlElement(ElementName = "linesRead")]
        public string LinesRead { get; set; }

        [DataMember]
        [XmlElement(ElementName = "linesWritten")]
        public string LinesWritten { get; set; }

        [DataMember]
        [XmlElement(ElementName = "linesInput")]
        public string LinesInput { get; set; }

        [DataMember]
        [XmlElement(ElementName = "linesOutput")]
        public string LinesOutput { get; set; }

        [DataMember]
        [XmlElement(ElementName = "linesUpdated")]
        public string LinesUpdated { get; set; }

        [DataMember]
        [XmlElement(ElementName = "linesRejected")]
        public string LinesRejected { get; set; }

        [DataMember]
        [XmlElement(ElementName = "errors")]
        public string Errors { get; set; }

        [DataMember]
        [XmlElement(ElementName = "statusDescription")]
        public string StatusDescription { get; set; }

        [DataMember]
        [XmlElement(ElementName = "seconds")]
        public string Seconds { get; set; }

        [DataMember]
        [XmlElement(ElementName = "speed")]
        public string Speed { get; set; }

        [DataMember]
        [XmlElement(ElementName = "priority")]
        public string Priority { get; set; }

        [DataMember]
        [XmlElement(ElementName = "stopped")]
        public string Stopped { get; set; }

        [DataMember]
        [XmlElement(ElementName = "paused")]
        public string Paused { get; set; }
    }

    [XmlRoot(ElementName = "stepstatuslist")]
    [DataContract]
    public class DC_PentahoTransStatus_Stepstatuslist
    {

        [DataMember]
        [XmlElement(ElementName = "stepstatus")]
        public List<DC_PentahoTransStatus_Stepstatus> Stepstatus { get; set; }
    }

    [XmlRoot(ElementName = "result-file")]
    [DataContract]
    public class DC_PentahoTransStatus_Resultfile
    {
        [DataMember]
        [XmlElement(ElementName = "type")]
        public string Type { get; set; }

        [DataMember]
        [XmlElement(ElementName = "file")]
        public string File { get; set; }

        [DataMember]
        [XmlElement(ElementName = "parentorigin")]
        public string Parentorigin { get; set; }

        [DataMember]
        [XmlElement(ElementName = "origin")]
        public string Origin { get; set; }

        [DataMember]
        [XmlElement(ElementName = "comment")]
        public string Comment { get; set; }

        [DataMember]
        [XmlElement(ElementName = "timestamp")]
        public string Timestamp { get; set; }
    }

    [XmlRoot(ElementName = "result")]
    [DataContract]
    public class DC_PentahoTransStatus_Result
    {

        [DataMember]
        [XmlElement(ElementName = "lines_input")]
        public string Lines_input { get; set; }

        [DataMember]
        [XmlElement(ElementName = "lines_output")]
        public string Lines_output { get; set; }

        [DataMember]
        [XmlElement(ElementName = "lines_read")]
        public string Lines_read { get; set; }

        [DataMember]
        [XmlElement(ElementName = "lines_written")]
        public string Lines_written { get; set; }

        [DataMember]
        [XmlElement(ElementName = "lines_updated")]
        public string Lines_updated { get; set; }

        [DataMember]
        [XmlElement(ElementName = "lines_rejected")]
        public string Lines_rejected { get; set; }

        [DataMember]
        [XmlElement(ElementName = "lines_deleted")]
        public string Lines_deleted { get; set; }

        [DataMember]
        [XmlElement(ElementName = "nr_errors")]
        public string Nr_errors { get; set; }

        [DataMember]
        [XmlElement(ElementName = "nr_files_retrieved")]
        public string Nr_files_retrieved { get; set; }

        [DataMember]
        [XmlElement(ElementName = "entry_nr")]
        public string Entry_nr { get; set; }

        [DataMember]
        [XmlElement(ElementName = "result")]
        public string sResult { get; set; }

        [DataMember]
        [XmlElement(ElementName = "exit_status")]
        public string Exit_status { get; set; }

        [DataMember]
        [XmlElement(ElementName = "is_stopped")]
        public string Is_stopped { get; set; }

        [DataMember]
        [XmlElement(ElementName = "log_channel_id")]
        public string Log_channel_id { get; set; }

        [DataMember]
        [XmlElement(ElementName = "log_text")]
        public string Log_text { get; set; }

        [DataMember]
        [XmlElement(ElementName = "result-file")]
        public List<DC_PentahoTransStatus_Resultfile> Resultfile { get; set; }

        [DataMember]
        [XmlElement(ElementName = "result-rows")]
        public string Resultrows { get; set; }
    }

    [XmlRoot(ElementName = "transstatus")]
    [DataContract]
    public class DC_PentahoTransStatus_TransStatus
    {
        [DataMember]
        [XmlElement(ElementName = "transname")]
        public string Transname { get; set; }

        [DataMember]
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }

        [DataMember]
        [XmlElement(ElementName = "status_desc")]
        public string Status_desc { get; set; }

        [DataMember]
        [XmlElement(ElementName = "error_desc")]
        public string Error_desc { get; set; }

        [DataMember]
        [XmlElement(ElementName = "log_date")]
        public string Log_date { get; set; }

        [DataMember]
        [XmlElement(ElementName = "paused")]
        public string Paused { get; set; }

        [DataMember]
        [XmlElement(ElementName = "stepstatuslist")]
        public DC_PentahoTransStatus_Stepstatuslist Stepstatuslist { get; set; }

        [DataMember]
        [XmlElement(ElementName = "first_log_line_nr")]
        public string First_log_line_nr { get; set; }

        [DataMember]
        [XmlElement(ElementName = "last_log_line_nr")]
        public string Last_log_line_nr { get; set; }

        [DataMember]
        [XmlElement(ElementName = "result")]
        public DC_PentahoTransStatus_Result Result { get; set; }

        [DataMember]
        [XmlElement(ElementName = "logging_string")]
        public string Logging_string { get; set; }
    }
}
