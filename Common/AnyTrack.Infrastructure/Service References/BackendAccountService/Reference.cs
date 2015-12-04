﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AnyTrack.Infrastructure.BackendAccountService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ServiceUser", Namespace="http://schemas.datacontract.org/2004/07/AnyTrack.Backend.Service.Model")]
    [System.SerializableAttribute()]
    public partial class ServiceUser : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool DeveloperField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EmailAddressField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FirstNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LastNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PasswordField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool ProductOwnerField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool ScrumMasterField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SecretAnswerField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SecretQuestionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SkillsField;
        
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
        public bool Developer {
            get {
                return this.DeveloperField;
            }
            set {
                if ((this.DeveloperField.Equals(value) != true)) {
                    this.DeveloperField = value;
                    this.RaisePropertyChanged("Developer");
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
        public string FirstName {
            get {
                return this.FirstNameField;
            }
            set {
                if ((object.ReferenceEquals(this.FirstNameField, value) != true)) {
                    this.FirstNameField = value;
                    this.RaisePropertyChanged("FirstName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LastName {
            get {
                return this.LastNameField;
            }
            set {
                if ((object.ReferenceEquals(this.LastNameField, value) != true)) {
                    this.LastNameField = value;
                    this.RaisePropertyChanged("LastName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Password {
            get {
                return this.PasswordField;
            }
            set {
                if ((object.ReferenceEquals(this.PasswordField, value) != true)) {
                    this.PasswordField = value;
                    this.RaisePropertyChanged("Password");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool ProductOwner {
            get {
                return this.ProductOwnerField;
            }
            set {
                if ((this.ProductOwnerField.Equals(value) != true)) {
                    this.ProductOwnerField = value;
                    this.RaisePropertyChanged("ProductOwner");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool ScrumMaster {
            get {
                return this.ScrumMasterField;
            }
            set {
                if ((this.ScrumMasterField.Equals(value) != true)) {
                    this.ScrumMasterField = value;
                    this.RaisePropertyChanged("ScrumMaster");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SecretAnswer {
            get {
                return this.SecretAnswerField;
            }
            set {
                if ((object.ReferenceEquals(this.SecretAnswerField, value) != true)) {
                    this.SecretAnswerField = value;
                    this.RaisePropertyChanged("SecretAnswer");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SecretQuestion {
            get {
                return this.SecretQuestionField;
            }
            set {
                if ((object.ReferenceEquals(this.SecretQuestionField, value) != true)) {
                    this.SecretQuestionField = value;
                    this.RaisePropertyChanged("SecretQuestion");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Skills {
            get {
                return this.SkillsField;
            }
            set {
                if ((object.ReferenceEquals(this.SkillsField, value) != true)) {
                    this.SkillsField = value;
                    this.RaisePropertyChanged("Skills");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="UserAlreadyExistsFault", Namespace="http://schemas.datacontract.org/2004/07/AnyTrack.Backend.Faults")]
    [System.SerializableAttribute()]
    public partial class UserAlreadyExistsFault : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
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
    [System.Runtime.Serialization.DataContractAttribute(Name="ServiceUserCredential", Namespace="http://schemas.datacontract.org/2004/07/AnyTrack.Backend.Service.Model")]
    [System.SerializableAttribute()]
    public partial class ServiceUserCredential : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EmailAddressField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PasswordField;
        
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
        public string Password {
            get {
                return this.PasswordField;
            }
            set {
                if ((object.ReferenceEquals(this.PasswordField, value) != true)) {
                    this.PasswordField = value;
                    this.RaisePropertyChanged("Password");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="ServiceLoginResult", Namespace="http://schemas.datacontract.org/2004/07/AnyTrack.Backend.Service.Model")]
    [System.SerializableAttribute()]
    public partial class ServiceLoginResult : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private AnyTrack.Infrastructure.BackendAccountService.ServiceRoleInfo[] AssignedRolesField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool DeveloperField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EmailAddressField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FirstNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LastNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool ProductOwnerField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool ScrumMasterField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool SuccessField;
        
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
        public AnyTrack.Infrastructure.BackendAccountService.ServiceRoleInfo[] AssignedRoles {
            get {
                return this.AssignedRolesField;
            }
            set {
                if ((object.ReferenceEquals(this.AssignedRolesField, value) != true)) {
                    this.AssignedRolesField = value;
                    this.RaisePropertyChanged("AssignedRoles");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Developer {
            get {
                return this.DeveloperField;
            }
            set {
                if ((this.DeveloperField.Equals(value) != true)) {
                    this.DeveloperField = value;
                    this.RaisePropertyChanged("Developer");
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
        public string FirstName {
            get {
                return this.FirstNameField;
            }
            set {
                if ((object.ReferenceEquals(this.FirstNameField, value) != true)) {
                    this.FirstNameField = value;
                    this.RaisePropertyChanged("FirstName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LastName {
            get {
                return this.LastNameField;
            }
            set {
                if ((object.ReferenceEquals(this.LastNameField, value) != true)) {
                    this.LastNameField = value;
                    this.RaisePropertyChanged("LastName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool ProductOwner {
            get {
                return this.ProductOwnerField;
            }
            set {
                if ((this.ProductOwnerField.Equals(value) != true)) {
                    this.ProductOwnerField = value;
                    this.RaisePropertyChanged("ProductOwner");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool ScrumMaster {
            get {
                return this.ScrumMasterField;
            }
            set {
                if ((this.ScrumMasterField.Equals(value) != true)) {
                    this.ScrumMasterField = value;
                    this.RaisePropertyChanged("ScrumMaster");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Success {
            get {
                return this.SuccessField;
            }
            set {
                if ((this.SuccessField.Equals(value) != true)) {
                    this.SuccessField = value;
                    this.RaisePropertyChanged("Success");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="ServiceRoleInfo", Namespace="http://schemas.datacontract.org/2004/07/AnyTrack.Backend.Service.Model")]
    [System.SerializableAttribute()]
    public partial class ServiceRoleInfo : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<System.Guid> ProjectIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string RoleField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<System.Guid> SprintIdField;
        
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
        public System.Nullable<System.Guid> ProjectId {
            get {
                return this.ProjectIdField;
            }
            set {
                if ((this.ProjectIdField.Equals(value) != true)) {
                    this.ProjectIdField = value;
                    this.RaisePropertyChanged("ProjectId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Role {
            get {
                return this.RoleField;
            }
            set {
                if ((object.ReferenceEquals(this.RoleField, value) != true)) {
                    this.RoleField = value;
                    this.RaisePropertyChanged("Role");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<System.Guid> SprintId {
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
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="BackendAccountService.IAccountService")]
    public interface IAccountService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccountService/CreateAccount", ReplyAction="http://tempuri.org/IAccountService/CreateAccountResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(AnyTrack.Infrastructure.BackendAccountService.UserAlreadyExistsFault), Action="http://tempuri.org/IAccountService/CreateAccountUserAlreadyExistsFaultFault", Name="UserAlreadyExistsFault", Namespace="http://schemas.datacontract.org/2004/07/AnyTrack.Backend.Faults")]
        void CreateAccount(AnyTrack.Infrastructure.BackendAccountService.ServiceUser user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccountService/CreateAccount", ReplyAction="http://tempuri.org/IAccountService/CreateAccountResponse")]
        System.Threading.Tasks.Task CreateAccountAsync(AnyTrack.Infrastructure.BackendAccountService.ServiceUser user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccountService/LogIn", ReplyAction="http://tempuri.org/IAccountService/LogInResponse")]
        AnyTrack.Infrastructure.BackendAccountService.ServiceLoginResult LogIn(AnyTrack.Infrastructure.BackendAccountService.ServiceUserCredential credential);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccountService/LogIn", ReplyAction="http://tempuri.org/IAccountService/LogInResponse")]
        System.Threading.Tasks.Task<AnyTrack.Infrastructure.BackendAccountService.ServiceLoginResult> LogInAsync(AnyTrack.Infrastructure.BackendAccountService.ServiceUserCredential credential);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccountService/RefreshLoginPrincipal", ReplyAction="http://tempuri.org/IAccountService/RefreshLoginPrincipalResponse")]
        AnyTrack.Infrastructure.BackendAccountService.ServiceLoginResult RefreshLoginPrincipal();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccountService/RefreshLoginPrincipal", ReplyAction="http://tempuri.org/IAccountService/RefreshLoginPrincipalResponse")]
        System.Threading.Tasks.Task<AnyTrack.Infrastructure.BackendAccountService.ServiceLoginResult> RefreshLoginPrincipalAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IAccountServiceChannel : AnyTrack.Infrastructure.BackendAccountService.IAccountService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class AccountServiceClient : System.ServiceModel.ClientBase<AnyTrack.Infrastructure.BackendAccountService.IAccountService>, AnyTrack.Infrastructure.BackendAccountService.IAccountService {
        
        public AccountServiceClient() {
        }
        
        public AccountServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public AccountServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AccountServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AccountServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void CreateAccount(AnyTrack.Infrastructure.BackendAccountService.ServiceUser user) {
            base.Channel.CreateAccount(user);
        }
        
        public System.Threading.Tasks.Task CreateAccountAsync(AnyTrack.Infrastructure.BackendAccountService.ServiceUser user) {
            return base.Channel.CreateAccountAsync(user);
        }
        
        public AnyTrack.Infrastructure.BackendAccountService.ServiceLoginResult LogIn(AnyTrack.Infrastructure.BackendAccountService.ServiceUserCredential credential) {
            return base.Channel.LogIn(credential);
        }
        
        public System.Threading.Tasks.Task<AnyTrack.Infrastructure.BackendAccountService.ServiceLoginResult> LogInAsync(AnyTrack.Infrastructure.BackendAccountService.ServiceUserCredential credential) {
            return base.Channel.LogInAsync(credential);
        }
        
        public AnyTrack.Infrastructure.BackendAccountService.ServiceLoginResult RefreshLoginPrincipal() {
            return base.Channel.RefreshLoginPrincipal();
        }
        
        public System.Threading.Tasks.Task<AnyTrack.Infrastructure.BackendAccountService.ServiceLoginResult> RefreshLoginPrincipalAsync() {
            return base.Channel.RefreshLoginPrincipalAsync();
        }
    }
}
