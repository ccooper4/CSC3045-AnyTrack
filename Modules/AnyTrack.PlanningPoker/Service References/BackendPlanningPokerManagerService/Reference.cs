﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AnyTrack.PlanningPoker.BackendPlanningPokerManagerService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ServiceSessionChangeInfo", Namespace="http://schemas.datacontract.org/2004/07/AnyTrack.Backend.Service.Model")]
    [System.SerializableAttribute()]
    public partial class ServiceSessionChangeInfo : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ProjectNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool SessionAvailableField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<System.Guid> SessionIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid SprintIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SprintNameField;
        
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
        public string ProjectName {
            get {
                return this.ProjectNameField;
            }
            set {
                if ((object.ReferenceEquals(this.ProjectNameField, value) != true)) {
                    this.ProjectNameField = value;
                    this.RaisePropertyChanged("ProjectName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool SessionAvailable {
            get {
                return this.SessionAvailableField;
            }
            set {
                if ((this.SessionAvailableField.Equals(value) != true)) {
                    this.SessionAvailableField = value;
                    this.RaisePropertyChanged("SessionAvailable");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<System.Guid> SessionId {
            get {
                return this.SessionIdField;
            }
            set {
                if ((this.SessionIdField.Equals(value) != true)) {
                    this.SessionIdField = value;
                    this.RaisePropertyChanged("SessionId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid SprintId {
            get {
                return this.SprintIdField;
            }
            set {
                if ((this.SprintIdField.Equals(value) != true)) {
                    this.SprintIdField = value;
                    this.RaisePropertyChanged("SprintId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SprintName {
            get {
                return this.SprintNameField;
            }
            set {
                if ((object.ReferenceEquals(this.SprintNameField, value) != true)) {
                    this.SprintNameField = value;
                    this.RaisePropertyChanged("SprintName");
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ServicePlanningPokerSession", Namespace="http://schemas.datacontract.org/2004/07/AnyTrack.Backend.Service.Model")]
    [System.SerializableAttribute()]
    public partial class ServicePlanningPokerSession : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid ActiveStoryField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid HostIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ProjectNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid SessionIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid SprintIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SprintNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.ServicePlanningPokerSessionState StateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.ServicePlanningPokerUser[] UsersField;
        
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
        public System.Guid ActiveStory {
            get {
                return this.ActiveStoryField;
            }
            set {
                if ((this.ActiveStoryField.Equals(value) != true)) {
                    this.ActiveStoryField = value;
                    this.RaisePropertyChanged("ActiveStory");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid HostID {
            get {
                return this.HostIDField;
            }
            set {
                if ((this.HostIDField.Equals(value) != true)) {
                    this.HostIDField = value;
                    this.RaisePropertyChanged("HostID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ProjectName {
            get {
                return this.ProjectNameField;
            }
            set {
                if ((object.ReferenceEquals(this.ProjectNameField, value) != true)) {
                    this.ProjectNameField = value;
                    this.RaisePropertyChanged("ProjectName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid SessionID {
            get {
                return this.SessionIDField;
            }
            set {
                if ((this.SessionIDField.Equals(value) != true)) {
                    this.SessionIDField = value;
                    this.RaisePropertyChanged("SessionID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid SprintID {
            get {
                return this.SprintIDField;
            }
            set {
                if ((this.SprintIDField.Equals(value) != true)) {
                    this.SprintIDField = value;
                    this.RaisePropertyChanged("SprintID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SprintName {
            get {
                return this.SprintNameField;
            }
            set {
                if ((object.ReferenceEquals(this.SprintNameField, value) != true)) {
                    this.SprintNameField = value;
                    this.RaisePropertyChanged("SprintName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.ServicePlanningPokerSessionState State {
            get {
                return this.StateField;
            }
            set {
                if ((this.StateField.Equals(value) != true)) {
                    this.StateField = value;
                    this.RaisePropertyChanged("State");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.ServicePlanningPokerUser[] Users {
            get {
                return this.UsersField;
            }
            set {
                if ((object.ReferenceEquals(this.UsersField, value) != true)) {
                    this.UsersField = value;
                    this.RaisePropertyChanged("Users");
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ServicePlanningPokerSessionState", Namespace="http://schemas.datacontract.org/2004/07/AnyTrack.Backend.Service.Model")]
    public enum ServicePlanningPokerSessionState : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Pending = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Started = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        GettingEstimates = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ShowingEstimates = 3,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ServicePlanningPokerUser", Namespace="http://schemas.datacontract.org/2004/07/AnyTrack.Backend.Service.Model")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(string[]))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.ServiceSessionChangeInfo))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.ServicePlanningPokerSession))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.ServicePlanningPokerSessionState))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.ServicePlanningPokerUser[]))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.ServicePlanningPokerEstimate))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.ServiceChatMessage))]
    public partial class ServicePlanningPokerUser : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private object ClientChannelField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EmailAddressField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.ServicePlanningPokerEstimate EstimateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string RoleSummaryField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid UserIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string[] UserRolesField;
        
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
        public object ClientChannel {
            get {
                return this.ClientChannelField;
            }
            set {
                if ((object.ReferenceEquals(this.ClientChannelField, value) != true)) {
                    this.ClientChannelField = value;
                    this.RaisePropertyChanged("ClientChannel");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string EmailAddress {
            get {
                return this.EmailAddressField;
            }
            set {
                if ((object.ReferenceEquals(this.EmailAddressField, value) != true)) {
                    this.EmailAddressField = value;
                    this.RaisePropertyChanged("EmailAddress");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.ServicePlanningPokerEstimate Estimate {
            get {
                return this.EstimateField;
            }
            set {
                if ((object.ReferenceEquals(this.EstimateField, value) != true)) {
                    this.EstimateField = value;
                    this.RaisePropertyChanged("Estimate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RoleSummary {
            get {
                return this.RoleSummaryField;
            }
            set {
                if ((object.ReferenceEquals(this.RoleSummaryField, value) != true)) {
                    this.RoleSummaryField = value;
                    this.RaisePropertyChanged("RoleSummary");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid UserID {
            get {
                return this.UserIDField;
            }
            set {
                if ((this.UserIDField.Equals(value) != true)) {
                    this.UserIDField = value;
                    this.RaisePropertyChanged("UserID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] UserRoles {
            get {
                return this.UserRolesField;
            }
            set {
                if ((object.ReferenceEquals(this.UserRolesField, value) != true)) {
                    this.UserRolesField = value;
                    this.RaisePropertyChanged("UserRoles");
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ServicePlanningPokerEstimate", Namespace="http://schemas.datacontract.org/2004/07/AnyTrack.Backend.Service.Model")]
    [System.SerializableAttribute()]
    public partial class ServicePlanningPokerEstimate : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double EstimateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid SessionIDField;
        
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
        public double Estimate {
            get {
                return this.EstimateField;
            }
            set {
                if ((this.EstimateField.Equals(value) != true)) {
                    this.EstimateField = value;
                    this.RaisePropertyChanged("Estimate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid SessionID {
            get {
                return this.SessionIDField;
            }
            set {
                if ((this.SessionIDField.Equals(value) != true)) {
                    this.SessionIDField = value;
                    this.RaisePropertyChanged("SessionID");
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ServiceChatMessage", Namespace="http://schemas.datacontract.org/2004/07/AnyTrack.Backend.Service.Model")]
    [System.SerializableAttribute()]
    public partial class ServiceChatMessage : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MessageField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid SessionIDField;
        
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
        public string Message {
            get {
                return this.MessageField;
            }
            set {
                if ((object.ReferenceEquals(this.MessageField, value) != true)) {
                    this.MessageField = value;
                    this.RaisePropertyChanged("Message");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid SessionID {
            get {
                return this.SessionIDField;
            }
            set {
                if ((this.SessionIDField.Equals(value) != true)) {
                    this.SessionIDField = value;
                    this.RaisePropertyChanged("SessionID");
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
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="BackendPlanningPokerManagerService.IPlanningPokerManagerService", CallbackContract=typeof(AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.IPlanningPokerManagerServiceCallback))]
    public interface IPlanningPokerManagerService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPlanningPokerManagerService/SubscribeToNewSessionMessages", ReplyAction="http://tempuri.org/IPlanningPokerManagerService/SubscribeToNewSessionMessagesResp" +
            "onse")]
        AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.ServiceSessionChangeInfo SubscribeToNewSessionMessages(System.Guid sprintId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPlanningPokerManagerService/SubscribeToNewSessionMessages", ReplyAction="http://tempuri.org/IPlanningPokerManagerService/SubscribeToNewSessionMessagesResp" +
            "onse")]
        System.Threading.Tasks.Task<AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.ServiceSessionChangeInfo> SubscribeToNewSessionMessagesAsync(System.Guid sprintId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPlanningPokerManagerService/StartNewPokerSession", ReplyAction="http://tempuri.org/IPlanningPokerManagerService/StartNewPokerSessionResponse")]
        System.Guid StartNewPokerSession(System.Guid sprintId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPlanningPokerManagerService/StartNewPokerSession", ReplyAction="http://tempuri.org/IPlanningPokerManagerService/StartNewPokerSessionResponse")]
        System.Threading.Tasks.Task<System.Guid> StartNewPokerSessionAsync(System.Guid sprintId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPlanningPokerManagerService/CancelPendingPokerSession", ReplyAction="http://tempuri.org/IPlanningPokerManagerService/CancelPendingPokerSessionResponse" +
            "")]
        void CancelPendingPokerSession(System.Guid sessionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPlanningPokerManagerService/CancelPendingPokerSession", ReplyAction="http://tempuri.org/IPlanningPokerManagerService/CancelPendingPokerSessionResponse" +
            "")]
        System.Threading.Tasks.Task CancelPendingPokerSessionAsync(System.Guid sessionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPlanningPokerManagerService/JoinSession", ReplyAction="http://tempuri.org/IPlanningPokerManagerService/JoinSessionResponse")]
        AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.ServicePlanningPokerSession JoinSession(System.Guid sessionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPlanningPokerManagerService/JoinSession", ReplyAction="http://tempuri.org/IPlanningPokerManagerService/JoinSessionResponse")]
        System.Threading.Tasks.Task<AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.ServicePlanningPokerSession> JoinSessionAsync(System.Guid sessionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPlanningPokerManagerService/RetrieveSessionInfo", ReplyAction="http://tempuri.org/IPlanningPokerManagerService/RetrieveSessionInfoResponse")]
        AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.ServicePlanningPokerSession RetrieveSessionInfo(System.Guid sessionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPlanningPokerManagerService/RetrieveSessionInfo", ReplyAction="http://tempuri.org/IPlanningPokerManagerService/RetrieveSessionInfoResponse")]
        System.Threading.Tasks.Task<AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.ServicePlanningPokerSession> RetrieveSessionInfoAsync(System.Guid sessionId);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IPlanningPokerManagerService/SubmitMessageToServer")]
        void SubmitMessageToServer(AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.ServiceChatMessage msg);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IPlanningPokerManagerService/SubmitMessageToServer")]
        System.Threading.Tasks.Task SubmitMessageToServerAsync(AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.ServiceChatMessage msg);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IPlanningPokerManagerService/SubmitEstimateToServer")]
        void SubmitEstimateToServer(AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.ServicePlanningPokerEstimate estimate);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IPlanningPokerManagerService/SubmitEstimateToServer")]
        System.Threading.Tasks.Task SubmitEstimateToServerAsync(AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.ServicePlanningPokerEstimate estimate);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IPlanningPokerManagerServiceCallback {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPlanningPokerManagerService/NotifyClientOfSession", ReplyAction="http://tempuri.org/IPlanningPokerManagerService/NotifyClientOfSessionResponse")]
        void NotifyClientOfSession(AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.ServiceSessionChangeInfo sessionInfo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPlanningPokerManagerService/NotifyClientOfTerminatedSession", ReplyAction="http://tempuri.org/IPlanningPokerManagerService/NotifyClientOfTerminatedSessionRe" +
            "sponse")]
        void NotifyClientOfTerminatedSession();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPlanningPokerManagerService/NotifyClientOfSessionUpdate", ReplyAction="http://tempuri.org/IPlanningPokerManagerService/NotifyClientOfSessionUpdateRespon" +
            "se")]
        void NotifyClientOfSessionUpdate(AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.ServicePlanningPokerSession newSession);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPlanningPokerManagerService/SendMessageToClient", ReplyAction="http://tempuri.org/IPlanningPokerManagerService/SendMessageToClientResponse")]
        void SendMessageToClient(AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.ServiceChatMessage msg);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPlanningPokerManagerService/NotifyClientToClearStoryPointEsti" +
            "mateFromServer", ReplyAction="http://tempuri.org/IPlanningPokerManagerService/NotifyClientToClearStoryPointEsti" +
            "mateFromServerResponse")]
        void NotifyClientToClearStoryPointEstimateFromServer();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPlanningPokerManagerService/SendSessionToClient", ReplyAction="http://tempuri.org/IPlanningPokerManagerService/SendSessionToClientResponse")]
        void SendSessionToClient(AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.ServicePlanningPokerSession session);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IPlanningPokerManagerServiceChannel : AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.IPlanningPokerManagerService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class PlanningPokerManagerServiceClient : System.ServiceModel.DuplexClientBase<AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.IPlanningPokerManagerService>, AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.IPlanningPokerManagerService {
        
        public PlanningPokerManagerServiceClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public PlanningPokerManagerServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public PlanningPokerManagerServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public PlanningPokerManagerServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public PlanningPokerManagerServiceClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.ServiceSessionChangeInfo SubscribeToNewSessionMessages(System.Guid sprintId) {
            return base.Channel.SubscribeToNewSessionMessages(sprintId);
        }
        
        public System.Threading.Tasks.Task<AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.ServiceSessionChangeInfo> SubscribeToNewSessionMessagesAsync(System.Guid sprintId) {
            return base.Channel.SubscribeToNewSessionMessagesAsync(sprintId);
        }
        
        public System.Guid StartNewPokerSession(System.Guid sprintId) {
            return base.Channel.StartNewPokerSession(sprintId);
        }
        
        public System.Threading.Tasks.Task<System.Guid> StartNewPokerSessionAsync(System.Guid sprintId) {
            return base.Channel.StartNewPokerSessionAsync(sprintId);
        }
        
        public void CancelPendingPokerSession(System.Guid sessionId) {
            base.Channel.CancelPendingPokerSession(sessionId);
        }
        
        public System.Threading.Tasks.Task CancelPendingPokerSessionAsync(System.Guid sessionId) {
            return base.Channel.CancelPendingPokerSessionAsync(sessionId);
        }
        
        public AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.ServicePlanningPokerSession JoinSession(System.Guid sessionId) {
            return base.Channel.JoinSession(sessionId);
        }
        
        public System.Threading.Tasks.Task<AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.ServicePlanningPokerSession> JoinSessionAsync(System.Guid sessionId) {
            return base.Channel.JoinSessionAsync(sessionId);
        }
        
        public AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.ServicePlanningPokerSession RetrieveSessionInfo(System.Guid sessionId) {
            return base.Channel.RetrieveSessionInfo(sessionId);
        }
        
        public System.Threading.Tasks.Task<AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.ServicePlanningPokerSession> RetrieveSessionInfoAsync(System.Guid sessionId) {
            return base.Channel.RetrieveSessionInfoAsync(sessionId);
        }
        
        public void SubmitMessageToServer(AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.ServiceChatMessage msg) {
            base.Channel.SubmitMessageToServer(msg);
        }
        
        public System.Threading.Tasks.Task SubmitMessageToServerAsync(AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.ServiceChatMessage msg) {
            return base.Channel.SubmitMessageToServerAsync(msg);
        }
        
        public void SubmitEstimateToServer(AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.ServicePlanningPokerEstimate estimate) {
            base.Channel.SubmitEstimateToServer(estimate);
        }
        
        public System.Threading.Tasks.Task SubmitEstimateToServerAsync(AnyTrack.PlanningPoker.BackendPlanningPokerManagerService.ServicePlanningPokerEstimate estimate) {
            return base.Channel.SubmitEstimateToServerAsync(estimate);
        }
    }
}
