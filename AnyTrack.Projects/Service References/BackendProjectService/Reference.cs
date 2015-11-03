﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AnyTrack.Projects.BackendProjectService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ServiceProject", Namespace="http://schemas.datacontract.org/2004/07/AnyTrack.Backend.Service.Model")]
    [System.SerializableAttribute()]
    public partial class ServiceProject : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DescriptionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private AnyTrack.Infrastructure.BackendAccountService.NewUser ProductOwnerField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid ProjectIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private AnyTrack.Infrastructure.BackendAccountService.NewUser ProjectManagerField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Collections.Generic.List<AnyTrack.Infrastructure.BackendAccountService.NewUser> ScrumMastersField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime StartedOnField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string VersionControlField;
        
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
        public string Description {
            get {
                return this.DescriptionField;
            }
            set {
                if ((object.ReferenceEquals(this.DescriptionField, value) != true)) {
                    this.DescriptionField = value;
                    this.RaisePropertyChanged("Description");
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
        public AnyTrack.Infrastructure.BackendAccountService.NewUser ProductOwner {
            get {
                return this.ProductOwnerField;
            }
            set {
                if ((object.ReferenceEquals(this.ProductOwnerField, value) != true)) {
                    this.ProductOwnerField = value;
                    this.RaisePropertyChanged("ProductOwner");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid ProjectId {
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
        public AnyTrack.Infrastructure.BackendAccountService.NewUser ProjectManager {
            get {
                return this.ProjectManagerField;
            }
            set {
                if ((object.ReferenceEquals(this.ProjectManagerField, value) != true)) {
                    this.ProjectManagerField = value;
                    this.RaisePropertyChanged("ProjectManager");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Collections.Generic.List<AnyTrack.Infrastructure.BackendAccountService.NewUser> ScrumMasters {
            get {
                return this.ScrumMastersField;
            }
            set {
                if ((object.ReferenceEquals(this.ScrumMastersField, value) != true)) {
                    this.ScrumMastersField = value;
                    this.RaisePropertyChanged("ScrumMasters");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime StartedOn {
            get {
                return this.StartedOnField;
            }
            set {
                if ((this.StartedOnField.Equals(value) != true)) {
                    this.StartedOnField = value;
                    this.RaisePropertyChanged("StartedOn");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string VersionControl {
            get {
                return this.VersionControlField;
            }
            set {
                if ((object.ReferenceEquals(this.VersionControlField, value) != true)) {
                    this.VersionControlField = value;
                    this.RaisePropertyChanged("VersionControl");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="UserSearchFilter", Namespace="http://schemas.datacontract.org/2004/07/AnyTrack.Backend.Service.Model")]
    [System.SerializableAttribute()]
    public partial class UserSearchFilter : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EmailAddressField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<bool> ProductOwnerField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<bool> ScrumMasterField;
        
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
        public System.Nullable<bool> ProductOwner {
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
        public System.Nullable<bool> ScrumMaster {
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
    [System.Runtime.Serialization.DataContractAttribute(Name="UserSearchInfo", Namespace="http://schemas.datacontract.org/2004/07/AnyTrack.Backend.Service.Model")]
    [System.SerializableAttribute()]
    public partial class UserSearchInfo : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EmailAddressField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FullNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid UserIDField;
        
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
        public string FullName {
            get {
                return this.FullNameField;
            }
            set {
                if ((object.ReferenceEquals(this.FullNameField, value) != true)) {
                    this.FullNameField = value;
                    this.RaisePropertyChanged("FullName");
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
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="BackendProjectService.IProjectService")]
    public interface IProjectService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProjectService/AddProject", ReplyAction="http://tempuri.org/IProjectService/AddProjectResponse")]
        void AddProject(AnyTrack.Projects.BackendProjectService.ServiceProject project);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProjectService/AddProject", ReplyAction="http://tempuri.org/IProjectService/AddProjectResponse")]
        System.Threading.Tasks.Task AddProjectAsync(AnyTrack.Projects.BackendProjectService.ServiceProject project);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProjectService/UpdateProject", ReplyAction="http://tempuri.org/IProjectService/UpdateProjectResponse")]
        void UpdateProject(AnyTrack.Projects.BackendProjectService.ServiceProject updatedProject);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProjectService/UpdateProject", ReplyAction="http://tempuri.org/IProjectService/UpdateProjectResponse")]
        System.Threading.Tasks.Task UpdateProjectAsync(AnyTrack.Projects.BackendProjectService.ServiceProject updatedProject);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProjectService/DeleteProject", ReplyAction="http://tempuri.org/IProjectService/DeleteProjectResponse")]
        void DeleteProject(System.Guid projectId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProjectService/DeleteProject", ReplyAction="http://tempuri.org/IProjectService/DeleteProjectResponse")]
        System.Threading.Tasks.Task DeleteProjectAsync(System.Guid projectId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProjectService/GetProject", ReplyAction="http://tempuri.org/IProjectService/GetProjectResponse")]
        AnyTrack.Projects.BackendProjectService.ServiceProject GetProject(System.Guid projectId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProjectService/GetProject", ReplyAction="http://tempuri.org/IProjectService/GetProjectResponse")]
        System.Threading.Tasks.Task<AnyTrack.Projects.BackendProjectService.ServiceProject> GetProjectAsync(System.Guid projectId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProjectService/GetProjects", ReplyAction="http://tempuri.org/IProjectService/GetProjectsResponse")]
        System.Collections.Generic.List<AnyTrack.Projects.BackendProjectService.ServiceProject> GetProjects();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProjectService/GetProjects", ReplyAction="http://tempuri.org/IProjectService/GetProjectsResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<AnyTrack.Projects.BackendProjectService.ServiceProject>> GetProjectsAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProjectService/SearchUsers", ReplyAction="http://tempuri.org/IProjectService/SearchUsersResponse")]
        System.Collections.Generic.List<AnyTrack.Projects.BackendProjectService.UserSearchInfo> SearchUsers(AnyTrack.Projects.BackendProjectService.UserSearchFilter filter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProjectService/SearchUsers", ReplyAction="http://tempuri.org/IProjectService/SearchUsersResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<AnyTrack.Projects.BackendProjectService.UserSearchInfo>> SearchUsersAsync(AnyTrack.Projects.BackendProjectService.UserSearchFilter filter);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IProjectServiceChannel : AnyTrack.Projects.BackendProjectService.IProjectService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ProjectServiceClient : System.ServiceModel.ClientBase<AnyTrack.Projects.BackendProjectService.IProjectService>, AnyTrack.Projects.BackendProjectService.IProjectService {
        
        public ProjectServiceClient() {
        }
        
        public ProjectServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ProjectServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ProjectServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ProjectServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void AddProject(AnyTrack.Projects.BackendProjectService.ServiceProject project) {
            base.Channel.AddProject(project);
        }
        
        public System.Threading.Tasks.Task AddProjectAsync(AnyTrack.Projects.BackendProjectService.ServiceProject project) {
            return base.Channel.AddProjectAsync(project);
        }
        
        public void UpdateProject(AnyTrack.Projects.BackendProjectService.ServiceProject updatedProject) {
            base.Channel.UpdateProject(updatedProject);
        }
        
        public System.Threading.Tasks.Task UpdateProjectAsync(AnyTrack.Projects.BackendProjectService.ServiceProject updatedProject) {
            return base.Channel.UpdateProjectAsync(updatedProject);
        }
        
        public void DeleteProject(System.Guid projectId) {
            base.Channel.DeleteProject(projectId);
        }
        
        public System.Threading.Tasks.Task DeleteProjectAsync(System.Guid projectId) {
            return base.Channel.DeleteProjectAsync(projectId);
        }
        
        public AnyTrack.Projects.BackendProjectService.ServiceProject GetProject(System.Guid projectId) {
            return base.Channel.GetProject(projectId);
        }
        
        public System.Threading.Tasks.Task<AnyTrack.Projects.BackendProjectService.ServiceProject> GetProjectAsync(System.Guid projectId) {
            return base.Channel.GetProjectAsync(projectId);
        }
        
        public System.Collections.Generic.List<AnyTrack.Projects.BackendProjectService.ServiceProject> GetProjects() {
            return base.Channel.GetProjects();
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.List<AnyTrack.Projects.BackendProjectService.ServiceProject>> GetProjectsAsync() {
            return base.Channel.GetProjectsAsync();
        }
        
        public System.Collections.Generic.List<AnyTrack.Projects.BackendProjectService.UserSearchInfo> SearchUsers(AnyTrack.Projects.BackendProjectService.UserSearchFilter filter) {
            return base.Channel.SearchUsers(filter);
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.List<AnyTrack.Projects.BackendProjectService.UserSearchInfo>> SearchUsersAsync(AnyTrack.Projects.BackendProjectService.UserSearchFilter filter) {
            return base.Channel.SearchUsersAsync(filter);
        }
    }
}