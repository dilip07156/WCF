﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.FileTransfer
{
    [DataContract]
    public class DC_FileUploadResponse
    {

        public bool UploadSucceeded { get; set; }

        [DataMember]
        public string UploadedPath { get; set; }
    }
}
