﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AnyTrack.Sprints.BackendSprintService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ServiceSprint", Namespace="http://schemas.datacontract.org/2004/07/AnyTrack.Backend.Service.Model")]
    [System.SerializableAttribute()]
    public partial class ServiceSprint : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private AnyTrack.Sprints.BackendSprintService.ServiceSprintStory[] BacklogField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DescriptionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime EndDateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int LengthField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid SprintIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime StartDateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string[] TeamEmailAddressesField;
        
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
        public AnyTrack.Sprints.BackendSprintService.ServiceSprintStory[] Backlog {
            get {
                return this.BacklogField;
            }
            set {
                if ((object.ReferenceEquals(this.BacklogField, value) != true)) {
                    this.BacklogField = value;
                    this.RaisePropertyChanged("Backlog");
                }
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
        public System.DateTime EndDate {
            get {
                return this.EndDateField;
            }
            set {
                if ((this.EndDateField.Equals(value) != true)) {
                    this.EndDateField = value;
                    this.RaisePropertyChanged("EndDate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Length {
            get {
                return this.LengthField;
            }
            set {
                if ((this.LengthField.Equals(value) != true)) {
                    this.LengthField = value;
                    this.RaisePropertyChanged("Length");
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
        public System.DateTime StartDate {
            get {
                return this.StartDateField;
            }
            set {
                if ((this.StartDateField.Equals(value) != true)) {
                    this.StartDateField = value;
                    this.RaisePropertyChanged("StartDate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] TeamEmailAddresses {
            get {
                return this.TeamEmailAddressesField;
            }
            set {
                if ((object.ReferenceEquals(this.TeamEmailAddressesField, value) != true)) {
                    this.TeamEmailAddressesField = value;
                    this.RaisePropertyChanged("TeamEmailAddresses");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="ServiceSprintStory", Namespace="http://schemas.datacontract.org/2004/07/AnyTrack.Backend.Service.Model")]
    [System.SerializableAttribute()]
    public partial class ServiceSprintStory : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid SprintStoryIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private AnyTrack.Sprints.BackendSprintService.ServiceStory StoryField;
        
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
        public System.Guid SprintStoryId {
            get {
                return this.SprintStoryIdField;
            }
            set {
                if ((this.SprintStoryIdField.Equals(value) != true)) {
                    this.SprintStoryIdField = value;
                    this.RaisePropertyChanged("SprintStoryId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public AnyTrack.Sprints.BackendSprintService.ServiceStory Story {
            get {
                return this.StoryField;
            }
            set {
                if ((object.ReferenceEquals(this.StoryField, value) != true)) {
                    this.StoryField = value;
                    this.RaisePropertyChanged("Story");
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="BackendSprintService.ISprintService")]
    public interface ISprintService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISprintService/AddSprint", ReplyAction="http://tempuri.org/ISprintService/AddSprintResponse")]
        void AddSprint(System.Guid projectId, AnyTrack.Sprints.BackendSprintService.ServiceSprint sprint);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISprintService/AddSprint", ReplyAction="http://tempuri.org/ISprintService/AddSprintResponse")]
        System.Threading.Tasks.Task AddSprintAsync(System.Guid projectId, AnyTrack.Sprints.BackendSprintService.ServiceSprint sprint);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISprintService/EditSprint", ReplyAction="http://tempuri.org/ISprintService/EditSprintResponse")]
        void EditSprint(System.Guid sprintId, AnyTrack.Sprints.BackendSprintService.ServiceSprint updatedSprint);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISprintService/EditSprint", ReplyAction="http://tempuri.org/ISprintService/EditSprintResponse")]
        System.Threading.Tasks.Task EditSprintAsync(System.Guid sprintId, AnyTrack.Sprints.BackendSprintService.ServiceSprint updatedSprint);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISprintServiceChannel : AnyTrack.Sprints.BackendSprintService.ISprintService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SprintServiceClient : System.ServiceModel.ClientBase<AnyTrack.Sprints.BackendSprintService.ISprintService>, AnyTrack.Sprints.BackendSprintService.ISprintService {
        
        public SprintServiceClient() {
        }
        
        public SprintServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SprintServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SprintServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SprintServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void AddSprint(System.Guid projectId, AnyTrack.Sprints.BackendSprintService.ServiceSprint sprint) {
            base.Channel.AddSprint(projectId, sprint);
        }
        
        public System.Threading.Tasks.Task AddSprintAsync(System.Guid projectId, AnyTrack.Sprints.BackendSprintService.ServiceSprint sprint) {
            return base.Channel.AddSprintAsync(projectId, sprint);
        }
        
        public void EditSprint(System.Guid sprintId, AnyTrack.Sprints.BackendSprintService.ServiceSprint updatedSprint) {
            base.Channel.EditSprint(sprintId, updatedSprint);
        }
        
        public System.Threading.Tasks.Task EditSprintAsync(System.Guid sprintId, AnyTrack.Sprints.BackendSprintService.ServiceSprint updatedSprint) {
            return base.Channel.EditSprintAsync(sprintId, updatedSprint);
        }
    }
}
