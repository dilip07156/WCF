﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataLayer.DHSVC {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DC_SupplierImportFileDetails_TestProcess", Namespace="http://schemas.datacontract.org/2004/07/TLGX_DataHandler_WcfService.DataContracts" +
        "")]
    [System.SerializableAttribute()]
    public partial class DC_SupplierImportFileDetails_TestProcess : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Data.DataSet DataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EntityField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int No_Of_Records_ToProcessField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PROCESS_USERField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string STATUSField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SavedFilePathField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid SupplierImportFile_IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid Supplier_IdField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Data.DataSet Data {
            get {
                return this.DataField;
            }
            set {
                if ((object.ReferenceEquals(this.DataField, value) != true)) {
                    this.DataField = value;
                    this.RaisePropertyChanged("Data");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Entity {
            get {
                return this.EntityField;
            }
            set {
                if ((object.ReferenceEquals(this.EntityField, value) != true)) {
                    this.EntityField = value;
                    this.RaisePropertyChanged("Entity");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int No_Of_Records_ToProcess {
            get {
                return this.No_Of_Records_ToProcessField;
            }
            set {
                if ((this.No_Of_Records_ToProcessField.Equals(value) != true)) {
                    this.No_Of_Records_ToProcessField = value;
                    this.RaisePropertyChanged("No_Of_Records_ToProcess");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PROCESS_USER {
            get {
                return this.PROCESS_USERField;
            }
            set {
                if ((object.ReferenceEquals(this.PROCESS_USERField, value) != true)) {
                    this.PROCESS_USERField = value;
                    this.RaisePropertyChanged("PROCESS_USER");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string STATUS {
            get {
                return this.STATUSField;
            }
            set {
                if ((object.ReferenceEquals(this.STATUSField, value) != true)) {
                    this.STATUSField = value;
                    this.RaisePropertyChanged("STATUS");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SavedFilePath {
            get {
                return this.SavedFilePathField;
            }
            set {
                if ((object.ReferenceEquals(this.SavedFilePathField, value) != true)) {
                    this.SavedFilePathField = value;
                    this.RaisePropertyChanged("SavedFilePath");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid SupplierImportFile_Id {
            get {
                return this.SupplierImportFile_IdField;
            }
            set {
                if ((this.SupplierImportFile_IdField.Equals(value) != true)) {
                    this.SupplierImportFile_IdField = value;
                    this.RaisePropertyChanged("SupplierImportFile_Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid Supplier_Id {
            get {
                return this.Supplier_IdField;
            }
            set {
                if ((this.Supplier_IdField.Equals(value) != true)) {
                    this.Supplier_IdField = value;
                    this.RaisePropertyChanged("Supplier_Id");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="DHSVC.IDataHandlerService")]
    public interface IDataHandlerService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataHandlerService/StaticFileUploadProcessFile", ReplyAction="http://tempuri.org/IDataHandlerService/StaticFileUploadProcessFileResponse")]
        void StaticFileUploadProcessFile(DataLayer.DHSVC.DC_SupplierImportFileDetails_TestProcess obj);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataHandlerService/StaticFileUploadProcessFile", ReplyAction="http://tempuri.org/IDataHandlerService/StaticFileUploadProcessFileResponse")]
        System.Threading.Tasks.Task StaticFileUploadProcessFileAsync(DataLayer.DHSVC.DC_SupplierImportFileDetails_TestProcess obj);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataHandlerService/StaticFileUpload_TestFile_Read", ReplyAction="http://tempuri.org/IDataHandlerService/StaticFileUpload_TestFile_ReadResponse")]
        System.Data.DataSet StaticFileUpload_TestFile_Read(DataLayer.DHSVC.DC_SupplierImportFileDetails_TestProcess obj);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataHandlerService/StaticFileUpload_TestFile_Read", ReplyAction="http://tempuri.org/IDataHandlerService/StaticFileUpload_TestFile_ReadResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> StaticFileUpload_TestFile_ReadAsync(DataLayer.DHSVC.DC_SupplierImportFileDetails_TestProcess obj);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataHandlerService/StaticFileUpload_TestFile_Transform", ReplyAction="http://tempuri.org/IDataHandlerService/StaticFileUpload_TestFile_TransformRespons" +
            "e")]
        System.Data.DataSet StaticFileUpload_TestFile_Transform(DataLayer.DHSVC.DC_SupplierImportFileDetails_TestProcess obj);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataHandlerService/StaticFileUpload_TestFile_Transform", ReplyAction="http://tempuri.org/IDataHandlerService/StaticFileUpload_TestFile_TransformRespons" +
            "e")]
        System.Threading.Tasks.Task<System.Data.DataSet> StaticFileUpload_TestFile_TransformAsync(DataLayer.DHSVC.DC_SupplierImportFileDetails_TestProcess obj);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IDataHandlerServiceChannel : DataLayer.DHSVC.IDataHandlerService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class DataHandlerServiceClient : System.ServiceModel.ClientBase<DataLayer.DHSVC.IDataHandlerService>, DataLayer.DHSVC.IDataHandlerService {
        
        public DataHandlerServiceClient() {
        }
        
        public DataHandlerServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public DataHandlerServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DataHandlerServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DataHandlerServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void StaticFileUploadProcessFile(DataLayer.DHSVC.DC_SupplierImportFileDetails_TestProcess obj) {
            base.Channel.StaticFileUploadProcessFile(obj);
        }
        
        public System.Threading.Tasks.Task StaticFileUploadProcessFileAsync(DataLayer.DHSVC.DC_SupplierImportFileDetails_TestProcess obj) {
            return base.Channel.StaticFileUploadProcessFileAsync(obj);
        }
        
        public System.Data.DataSet StaticFileUpload_TestFile_Read(DataLayer.DHSVC.DC_SupplierImportFileDetails_TestProcess obj) {
            return base.Channel.StaticFileUpload_TestFile_Read(obj);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> StaticFileUpload_TestFile_ReadAsync(DataLayer.DHSVC.DC_SupplierImportFileDetails_TestProcess obj) {
            return base.Channel.StaticFileUpload_TestFile_ReadAsync(obj);
        }
        
        public System.Data.DataSet StaticFileUpload_TestFile_Transform(DataLayer.DHSVC.DC_SupplierImportFileDetails_TestProcess obj) {
            return base.Channel.StaticFileUpload_TestFile_Transform(obj);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> StaticFileUpload_TestFile_TransformAsync(DataLayer.DHSVC.DC_SupplierImportFileDetails_TestProcess obj) {
            return base.Channel.StaticFileUpload_TestFile_TransformAsync(obj);
        }
    }
}
