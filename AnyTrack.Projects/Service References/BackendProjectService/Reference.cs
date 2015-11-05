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
        private string ProductOwnerEmailAddressField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid ProjectIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ProjectManagerEmailAddressField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Collections.Generic.List<string> ScrumMasterEmailAddressesField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private AnyTrack.Projects.BackendProjectService.ServiceStory ServiceStoryIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime StartedOnField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Collections.Generic.List<AnyTrack.Projects.BackendProjectService.ServiceStory> StoriesField;
        
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
        public string ProductOwnerEmailAddress {
            get {
                return this.ProductOwnerEmailAddressField;
            }
            set {
                if ((object.ReferenceEquals(this.ProductOwnerEmailAddressField, value) != true)) {
                    this.ProductOwnerEmailAddressField = value;
                    this.RaisePropertyChanged("ProductOwnerEmailAddress");
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
        public string ProjectManagerEmailAddress {
            get {
                return this.ProjectManagerEmailAddressField;
            }
            set {
                if ((object.ReferenceEquals(this.ProjectManagerEmailAddressField, value) != true)) {
                    this.ProjectManagerEmailAddressField = value;
                    this.RaisePropertyChanged("ProjectManagerEmailAddress");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Collections.Generic.List<string> ScrumMasterEmailAddresses {
            get {
                return this.ScrumMasterEmailAddressesField;
            }
            set {
                if ((object.ReferenceEquals(this.ScrumMasterEmailAddressesField, value) != true)) {
                    this.ScrumMasterEmailAddressesField = value;
                    this.RaisePropertyChanged("ScrumMasterEmailAddresses");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public AnyTrack.Projects.BackendProjectService.ServiceStory ServiceStoryId {
            get {
                return this.ServiceStoryIdField;
            }
            set {
                if ((object.ReferenceEquals(this.ServiceStoryIdField, value) != true)) {
                    this.ServiceStoryIdField = value;
                    this.RaisePropertyChanged("ServiceStoryId");
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
        public System.Collections.Generic.List<AnyTrack.Projects.BackendProjectService.ServiceStory> Stories {
            get {
                return this.StoriesField;
            }
            set {
                if ((object.ReferenceEquals(this.StoriesField, value) != true)) {
                    this.StoriesField = value;
                    this.RaisePropertyChanged("Stories");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="ServiceStory", Namespace="http://schemas.datacontract.org/2004/07/AnyTrack.Backend.Service.Model")]
    [System.SerializableAttribute()]
    public partial class ServiceStory : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AsAField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ConditionsOfSatisfactionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IWantField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid ProjectIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SoThatField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid StoryIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SummaryField;
        
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
        public string AsA {
            get {
                return this.AsAField;
            }
            set {
                if ((object.ReferenceEquals(this.AsAField, value) != true)) {
                    this.AsAField = value;
                    this.RaisePropertyChanged("AsA");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ConditionsOfSatisfaction {
            get {
                return this.ConditionsOfSatisfactionField;
            }
            set {
                if ((object.ReferenceEquals(this.ConditionsOfSatisfactionField, value) != true)) {
                    this.ConditionsOfSatisfactionField = value;
                    this.RaisePropertyChanged("ConditionsOfSatisfaction");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string IWant {
            get {
                return this.IWantField;
            }
            set {
                if ((object.ReferenceEquals(this.IWantField, value) != true)) {
                    this.IWantField = value;
                    this.RaisePropertyChanged("IWant");
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
        public string SoThat {
            get {
                return this.SoThatField;
            }
            set {
                if ((object.ReferenceEquals(this.SoThatField, value) != true)) {
                    this.SoThatField = value;
                    this.RaisePropertyChanged("SoThat");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid StoryId {
            get {
                return this.StoryIdField;
            }
            set {
                if ((this.StoryIdField.Equals(value) != true)) {
                    this.StoryIdField = value;
                    this.RaisePropertyChanged("StoryId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Summary {
            get {
                return this.SummaryField;
            }
            set {
                if ((object.ReferenceEquals(this.SummaryField, value) != true)) {
                    this.SummaryField = value;
                    this.RaisePropertyChanged("Summary");
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ProjectDetails", Namespace="http://schemas.datacontract.org/2004/07/AnyTrack.Backend.Service.Model")]
    [System.SerializableAttribute()]
    public partial class ProjectDetails : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid ProjectIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ProjectNameField;
        
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
    [System.Runtime.Serialization.DataContractAttribute(Name="StoryDetails", Namespace="http://schemas.datacontract.org/2004/07/AnyTrack.Backend.Service.Model")]
    [System.SerializableAttribute()]
    public partial class StoryDetails : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid ProjectIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid StoryIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SummaryField;
        
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
        public System.Guid StoryId {
            get {
                return this.StoryIdField;
            }
            set {
                if ((this.StoryIdField.Equals(value) != true)) {
                    this.StoryIdField = value;
                    this.RaisePropertyChanged("StoryId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Summary {
            get {
                return this.SummaryField;
            }
            set {
                if ((object.ReferenceEquals(this.SummaryField, value) != true)) {
                    this.SummaryField = value;
                    this.RaisePropertyChanged("Summary");
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
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProjectService/DeleteStoryFromProductBacklog", ReplyAction="http://tempuri.org/IProjectService/DeleteStoryFromProductBacklogResponse")]
        void DeleteStoryFromProductBacklog(System.Guid projectId, System.Guid storyId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProjectService/DeleteStoryFromProductBacklog", ReplyAction="http://tempuri.org/IProjectService/DeleteStoryFromProductBacklogResponse")]
        System.Threading.Tasks.Task DeleteStoryFromProductBacklogAsync(System.Guid projectId, System.Guid storyId);
        
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
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProjectService/GetProjectNames", ReplyAction="http://tempuri.org/IProjectService/GetProjectNamesResponse")]
        System.Collections.Generic.List<AnyTrack.Projects.BackendProjectService.ProjectDetails> GetProjectNames();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProjectService/GetProjectNames", ReplyAction="http://tempuri.org/IProjectService/GetProjectNamesResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<AnyTrack.Projects.BackendProjectService.ProjectDetails>> GetProjectNamesAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProjectService/GetProjectStoryDetails", ReplyAction="http://tempuri.org/IProjectService/GetProjectStoryDetailsResponse")]
        System.Collections.Generic.List<AnyTrack.Projects.BackendProjectService.StoryDetails> GetProjectStoryDetails(System.Guid projectId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProjectService/GetProjectStoryDetails", ReplyAction="http://tempuri.org/IProjectService/GetProjectStoryDetailsResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<AnyTrack.Projects.BackendProjectService.StoryDetails>> GetProjectStoryDetailsAsync(System.Guid projectId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProjectService/AddStoryToProject", ReplyAction="http://tempuri.org/IProjectService/AddStoryToProjectResponse")]
        void AddStoryToProject(System.Guid projectGuid, AnyTrack.Projects.BackendProjectService.ServiceStory story);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProjectService/AddStoryToProject", ReplyAction="http://tempuri.org/IProjectService/AddStoryToProjectResponse")]
        System.Threading.Tasks.Task AddStoryToProjectAsync(System.Guid projectGuid, AnyTrack.Projects.BackendProjectService.ServiceStory story);
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
        
        public void DeleteStoryFromProductBacklog(System.Guid projectId, System.Guid storyId) {
            base.Channel.DeleteStoryFromProductBacklog(projectId, storyId);
        }
        
        public System.Threading.Tasks.Task DeleteStoryFromProductBacklogAsync(System.Guid projectId, System.Guid storyId) {
            return base.Channel.DeleteStoryFromProductBacklogAsync(projectId, storyId);
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
        
        public System.Collections.Generic.List<AnyTrack.Projects.BackendProjectService.ProjectDetails> GetProjectNames() {
            return base.Channel.GetProjectNames();
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.List<AnyTrack.Projects.BackendProjectService.ProjectDetails>> GetProjectNamesAsync() {
            return base.Channel.GetProjectNamesAsync();
        }
        
        public System.Collections.Generic.List<AnyTrack.Projects.BackendProjectService.StoryDetails> GetProjectStoryDetails(System.Guid projectId) {
            return base.Channel.GetProjectStoryDetails(projectId);
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.List<AnyTrack.Projects.BackendProjectService.StoryDetails>> GetProjectStoryDetailsAsync(System.Guid projectId) {
            return base.Channel.GetProjectStoryDetailsAsync(projectId);
        }
        
        public void AddStoryToProject(System.Guid projectGuid, AnyTrack.Projects.BackendProjectService.ServiceStory story) {
            base.Channel.AddStoryToProject(projectGuid, story);
        }
        
        public System.Threading.Tasks.Task AddStoryToProjectAsync(System.Guid projectGuid, AnyTrack.Projects.BackendProjectService.ServiceStory story) {
            return base.Channel.AddStoryToProjectAsync(projectGuid, story);
        }
    }
}
