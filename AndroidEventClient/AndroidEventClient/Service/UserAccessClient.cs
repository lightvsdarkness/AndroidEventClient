﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.19408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AEC.Service
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Captcha", Namespace="http://schemas.datacontract.org/2004/07/EventService.DTO")]
    public partial class Captcha : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string AnswerField;
        
        private byte[] DataField;
        
        private long IdField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Answer
        {
            get
            {
                return this.AnswerField;
            }
            set
            {
                this.AnswerField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public byte[] Data
        {
            get
            {
                return this.DataField;
            }
            set
            {
                this.DataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long Id
        {
            get
            {
                return this.IdField;
            }
            set
            {
                this.IdField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CategoryShort", Namespace="http://schemas.datacontract.org/2004/07/EventService.DTO")]
    public partial class CategoryShort : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private int ChildrenCountField;
        
        private string DescriptionField;
        
        private long IdField;
        
        private int LevelField;
        
        private string NameField;
        
        private long ParentIdField;
        
        private System.Nullable<long> PrimaryPhotoIdField;
        
        private long RootIdField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ChildrenCount
        {
            get
            {
                return this.ChildrenCountField;
            }
            set
            {
                this.ChildrenCountField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Description
        {
            get
            {
                return this.DescriptionField;
            }
            set
            {
                this.DescriptionField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long Id
        {
            get
            {
                return this.IdField;
            }
            set
            {
                this.IdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Level
        {
            get
            {
                return this.LevelField;
            }
            set
            {
                this.LevelField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name
        {
            get
            {
                return this.NameField;
            }
            set
            {
                this.NameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long ParentId
        {
            get
            {
                return this.ParentIdField;
            }
            set
            {
                this.ParentIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> PrimaryPhotoId
        {
            get
            {
                return this.PrimaryPhotoIdField;
            }
            set
            {
                this.PrimaryPhotoIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long RootId
        {
            get
            {
                return this.RootIdField;
            }
            set
            {
                this.RootIdField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="AccountPut", Namespace="http://schemas.datacontract.org/2004/07/EventService.DTO")]
    public partial class AccountPut : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string AliasField;
        
        private byte[] AvatarField;
        
        private string EMailField;
        
        private long IdField;
        
        private string LoginField;
        
        private string PasswordField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Alias
        {
            get
            {
                return this.AliasField;
            }
            set
            {
                this.AliasField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public byte[] Avatar
        {
            get
            {
                return this.AvatarField;
            }
            set
            {
                this.AvatarField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string EMail
        {
            get
            {
                return this.EMailField;
            }
            set
            {
                this.EMailField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long Id
        {
            get
            {
                return this.IdField;
            }
            set
            {
                this.IdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Login
        {
            get
            {
                return this.LoginField;
            }
            set
            {
                this.LoginField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Password
        {
            get
            {
                return this.PasswordField;
            }
            set
            {
                this.PasswordField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="EventShort", Namespace="http://schemas.datacontract.org/2004/07/EventService.DTO")]
    public partial class EventShort : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private System.DateTime DateField;
        
        private long IdField;
        
        private string NameField;
        
        private string OrganizerNameField;
        
        private System.Nullable<long> PrimaryPhotoIdField;
        
        private System.DateTime PublicationDateField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime Date
        {
            get
            {
                return this.DateField;
            }
            set
            {
                this.DateField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long Id
        {
            get
            {
                return this.IdField;
            }
            set
            {
                this.IdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name
        {
            get
            {
                return this.NameField;
            }
            set
            {
                this.NameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string OrganizerName
        {
            get
            {
                return this.OrganizerNameField;
            }
            set
            {
                this.OrganizerNameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> PrimaryPhotoId
        {
            get
            {
                return this.PrimaryPhotoIdField;
            }
            set
            {
                this.PrimaryPhotoIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime PublicationDate
        {
            get
            {
                return this.PublicationDateField;
            }
            set
            {
                this.PublicationDateField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="EventFullGet", Namespace="http://schemas.datacontract.org/2004/07/EventService.DTO")]
    public partial class EventFullGet : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string[] AddressesField;
        
        private string[] CategoriesField;
        
        private decimal CostField;
        
        private System.DateTime DateField;
        
        private string DescriptionField;
        
        private System.TimeSpan DurationField;
        
        private long IdField;
        
        private string NameField;
        
        private string[] OrganizersField;
        
        private long[] PhotoIdsField;
        
        private System.Nullable<long> PrimaryPhotoIdField;
        
        private System.DateTime PublicationDateField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] Addresses
        {
            get
            {
                return this.AddressesField;
            }
            set
            {
                this.AddressesField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] Categories
        {
            get
            {
                return this.CategoriesField;
            }
            set
            {
                this.CategoriesField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal Cost
        {
            get
            {
                return this.CostField;
            }
            set
            {
                this.CostField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime Date
        {
            get
            {
                return this.DateField;
            }
            set
            {
                this.DateField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Description
        {
            get
            {
                return this.DescriptionField;
            }
            set
            {
                this.DescriptionField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.TimeSpan Duration
        {
            get
            {
                return this.DurationField;
            }
            set
            {
                this.DurationField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long Id
        {
            get
            {
                return this.IdField;
            }
            set
            {
                this.IdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name
        {
            get
            {
                return this.NameField;
            }
            set
            {
                this.NameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] Organizers
        {
            get
            {
                return this.OrganizersField;
            }
            set
            {
                this.OrganizersField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long[] PhotoIds
        {
            get
            {
                return this.PhotoIdsField;
            }
            set
            {
                this.PhotoIdsField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> PrimaryPhotoId
        {
            get
            {
                return this.PrimaryPhotoIdField;
            }
            set
            {
                this.PrimaryPhotoIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime PublicationDate
        {
            get
            {
                return this.PublicationDateField;
            }
            set
            {
                this.PublicationDateField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Photo", Namespace="http://schemas.datacontract.org/2004/07/EventService.DTO")]
    public partial class Photo : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private byte[] DataField;
        
        private long IdField;
        
        private string NameField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public byte[] Data
        {
            get
            {
                return this.DataField;
            }
            set
            {
                this.DataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long Id
        {
            get
            {
                return this.IdField;
            }
            set
            {
                this.IdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name
        {
            get
            {
                return this.NameField;
            }
            set
            {
                this.NameField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="FilterShort", Namespace="http://schemas.datacontract.org/2004/07/EventService.DTO")]
    public partial class FilterShort : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private long IdField;
        
        private string NameField;
        
        private System.Nullable<long> PhotoIdField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long Id
        {
            get
            {
                return this.IdField;
            }
            set
            {
                this.IdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name
        {
            get
            {
                return this.NameField;
            }
            set
            {
                this.NameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> PhotoId
        {
            get
            {
                return this.PhotoIdField;
            }
            set
            {
                this.PhotoIdField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="FilterFullGet", Namespace="http://schemas.datacontract.org/2004/07/EventService.DTO")]
    public partial class FilterFullGet : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private long[] CategoryIdsField;
        
        private long IdField;
        
        private string NameField;
        
        private System.Nullable<long> PhotoIdField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long[] CategoryIds
        {
            get
            {
                return this.CategoryIdsField;
            }
            set
            {
                this.CategoryIdsField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long Id
        {
            get
            {
                return this.IdField;
            }
            set
            {
                this.IdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name
        {
            get
            {
                return this.NameField;
            }
            set
            {
                this.NameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> PhotoId
        {
            get
            {
                return this.PhotoIdField;
            }
            set
            {
                this.PhotoIdField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="FilterFullPut", Namespace="http://schemas.datacontract.org/2004/07/EventService.DTO")]
    public partial class FilterFullPut : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private long[] CategoryIdsField;
        
        private long IdField;
        
        private string NameField;
        
        private byte[] PhotoField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long[] CategoryIds
        {
            get
            {
                return this.CategoryIdsField;
            }
            set
            {
                this.CategoryIdsField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long Id
        {
            get
            {
                return this.IdField;
            }
            set
            {
                this.IdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name
        {
            get
            {
                return this.NameField;
            }
            set
            {
                this.NameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public byte[] Photo
        {
            get
            {
                return this.PhotoField;
            }
            set
            {
                this.PhotoField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ESException", Namespace="http://schemas.datacontract.org/2004/07/EventService.DTO")]
    public partial class ESException : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string DescriptionField;
        
        private bool IsErrorField;
        
        private bool NeedLoginField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Description
        {
            get
            {
                return this.DescriptionField;
            }
            set
            {
                this.DescriptionField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsError
        {
            get
            {
                return this.IsErrorField;
            }
            set
            {
                this.IsErrorField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool NeedLogin
        {
            get
            {
                return this.NeedLoginField;
            }
            set
            {
                this.NeedLoginField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="AEC.Service.IUserAccess")]
    public interface IUserAccess
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRemoteAccess/Login", ReplyAction="http://tempuri.org/IRemoteAccess/LoginResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(AEC.Service.ESException), Action="http://tempuri.org/IRemoteAccess/LoginESExceptionFault", Name="ESException", Namespace="http://schemas.datacontract.org/2004/07/EventService.DTO")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="session")]
        string Login([System.ServiceModel.MessageParameterAttribute(Name="login")] string login1, string password, AEC.Service.Captcha captcha);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRemoteAccess/Logout", ReplyAction="http://tempuri.org/IRemoteAccess/LogoutResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(AEC.Service.ESException), Action="http://tempuri.org/IRemoteAccess/LogoutESExceptionFault", Name="ESException", Namespace="http://schemas.datacontract.org/2004/07/EventService.DTO")]
        void Logout(string session);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserAccess/GetCategoriesListForCurrentAccount", ReplyAction="http://tempuri.org/IUserAccess/GetCategoriesListForCurrentAccountResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(AEC.Service.ESException), Action="http://tempuri.org/IUserAccess/GetCategoriesListForCurrentAccountESExceptionFault" +
            "", Name="ESException", Namespace="http://schemas.datacontract.org/2004/07/EventService.DTO")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="categoriesList")]
        AEC.Service.CategoryShort[] GetCategoriesListForCurrentAccount(string sessionId, long parentId, long rootId, int pos, int count);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserAccess/RegisterNewAccount", ReplyAction="http://tempuri.org/IUserAccess/RegisterNewAccountResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(AEC.Service.ESException), Action="http://tempuri.org/IUserAccess/RegisterNewAccountESExceptionFault", Name="ESException", Namespace="http://schemas.datacontract.org/2004/07/EventService.DTO")]
        void RegisterNewAccount(AEC.Service.AccountPut account, AEC.Service.Captcha captcha);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserAccess/GetEventsList", ReplyAction="http://tempuri.org/IUserAccess/GetEventsListResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(AEC.Service.ESException), Action="http://tempuri.org/IUserAccess/GetEventsListESExceptionFault", Name="ESException", Namespace="http://schemas.datacontract.org/2004/07/EventService.DTO")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="eventsList")]
        AEC.Service.EventShort[] GetEventsList(string sessionId, int pos, int count);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserAccess/GetFilteredEventsList", ReplyAction="http://tempuri.org/IUserAccess/GetFilteredEventsListResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(AEC.Service.ESException), Action="http://tempuri.org/IUserAccess/GetFilteredEventsListESExceptionFault", Name="ESException", Namespace="http://schemas.datacontract.org/2004/07/EventService.DTO")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="eventsList")]
        AEC.Service.EventShort[] GetFilteredEventsList(string sessionId, long filterId, bool onlyNew, int pos, int count);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserAccess/GetEventFullDescription", ReplyAction="http://tempuri.org/IUserAccess/GetEventFullDescriptionResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(AEC.Service.ESException), Action="http://tempuri.org/IUserAccess/GetEventFullDescriptionESExceptionFault", Name="ESException", Namespace="http://schemas.datacontract.org/2004/07/EventService.DTO")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="eventDescription")]
        AEC.Service.EventFullGet GetEventFullDescription(string sessionId, long eventId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserAccess/GetPhoto", ReplyAction="http://tempuri.org/IUserAccess/GetPhotoResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(AEC.Service.ESException), Action="http://tempuri.org/IUserAccess/GetPhotoESExceptionFault", Name="ESException", Namespace="http://schemas.datacontract.org/2004/07/EventService.DTO")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="photo")]
        AEC.Service.Photo GetPhoto(string sessionId, long photoId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserAccess/GetFiltersList", ReplyAction="http://tempuri.org/IUserAccess/GetFiltersListResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(AEC.Service.ESException), Action="http://tempuri.org/IUserAccess/GetFiltersListESExceptionFault", Name="ESException", Namespace="http://schemas.datacontract.org/2004/07/EventService.DTO")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="filtersList")]
        AEC.Service.FilterShort[] GetFiltersList(string sessionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserAccess/GetFilter", ReplyAction="http://tempuri.org/IUserAccess/GetFilterResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(AEC.Service.ESException), Action="http://tempuri.org/IUserAccess/GetFilterESExceptionFault", Name="ESException", Namespace="http://schemas.datacontract.org/2004/07/EventService.DTO")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="filter")]
        AEC.Service.FilterFullGet GetFilter(string sessionId, long filterId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserAccess/CreateFilter", ReplyAction="http://tempuri.org/IUserAccess/CreateFilterResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(AEC.Service.ESException), Action="http://tempuri.org/IUserAccess/CreateFilterESExceptionFault", Name="ESException", Namespace="http://schemas.datacontract.org/2004/07/EventService.DTO")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="filterId")]
        long CreateFilter(string sessionId, AEC.Service.FilterFullPut filter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserAccess/DeleteFilter", ReplyAction="http://tempuri.org/IUserAccess/DeleteFilterResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(AEC.Service.ESException), Action="http://tempuri.org/IUserAccess/DeleteFilterESExceptionFault", Name="ESException", Namespace="http://schemas.datacontract.org/2004/07/EventService.DTO")]
        void DeleteFilter(string sessionId, long filterId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserAccess/UpdateFilterLastEvent", ReplyAction="http://tempuri.org/IUserAccess/UpdateFilterLastEventResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(AEC.Service.ESException), Action="http://tempuri.org/IUserAccess/UpdateFilterLastEventESExceptionFault", Name="ESException", Namespace="http://schemas.datacontract.org/2004/07/EventService.DTO")]
        void UpdateFilterLastEvent(string sessionId, long filterId, long eventId);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IUserAccessChannel : AEC.Service.IUserAccess, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class UserAccessClient : System.ServiceModel.ClientBase<AEC.Service.IUserAccess>, AEC.Service.IUserAccess
    {
        
        public UserAccessClient()
        {
        }
        
        public UserAccessClient(string endpointConfigurationName) : 
                base(endpointConfigurationName)
        {
        }
        
        public UserAccessClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress)
        {
        }
        
        public UserAccessClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress)
        {
        }
        
        public UserAccessClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public string Login(string login1, string password, AEC.Service.Captcha captcha)
        {
            return base.Channel.Login(login1, password, captcha);
        }
        
        public void Logout(string session)
        {
            base.Channel.Logout(session);
        }
        
        public AEC.Service.CategoryShort[] GetCategoriesListForCurrentAccount(string sessionId, long parentId, long rootId, int pos, int count)
        {
            return base.Channel.GetCategoriesListForCurrentAccount(sessionId, parentId, rootId, pos, count);
        }
        
        public void RegisterNewAccount(AEC.Service.AccountPut account, AEC.Service.Captcha captcha)
        {
            base.Channel.RegisterNewAccount(account, captcha);
        }
        
        public AEC.Service.EventShort[] GetEventsList(string sessionId, int pos, int count)
        {
            return base.Channel.GetEventsList(sessionId, pos, count);
        }
        
        public AEC.Service.EventShort[] GetFilteredEventsList(string sessionId, long filterId, bool onlyNew, int pos, int count)
        {
            return base.Channel.GetFilteredEventsList(sessionId, filterId, onlyNew, pos, count);
        }
        
        public AEC.Service.EventFullGet GetEventFullDescription(string sessionId, long eventId)
        {
            return base.Channel.GetEventFullDescription(sessionId, eventId);
        }
        
        public AEC.Service.Photo GetPhoto(string sessionId, long photoId)
        {
            return base.Channel.GetPhoto(sessionId, photoId);
        }
        
        public AEC.Service.FilterShort[] GetFiltersList(string sessionId)
        {
            return base.Channel.GetFiltersList(sessionId);
        }
        
        public AEC.Service.FilterFullGet GetFilter(string sessionId, long filterId)
        {
            return base.Channel.GetFilter(sessionId, filterId);
        }
        
        public long CreateFilter(string sessionId, AEC.Service.FilterFullPut filter)
        {
            return base.Channel.CreateFilter(sessionId, filter);
        }
        
        public void DeleteFilter(string sessionId, long filterId)
        {
            base.Channel.DeleteFilter(sessionId, filterId);
        }
        
        public void UpdateFilterLastEvent(string sessionId, long filterId, long eventId)
        {
            base.Channel.UpdateFilterLastEvent(sessionId, filterId, eventId);
        }
    }
}
