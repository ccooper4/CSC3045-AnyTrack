﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AnyTrack.Infrastructure.BackendSprintService {
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
        private System.Collections.Generic.List<AnyTrack.Infrastructure.BackendSprintService.ServiceSprintStory> BacklogField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DescriptionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime EndDateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int LengthField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid ProjectIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid SprintIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime StartDateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Collections.Generic.List<string> TeamEmailAddressesField;
        
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
        public System.Collections.Generic.List<AnyTrack.Infrastructure.BackendSprintService.ServiceSprintStory> Backlog {
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
        public System.Collections.Generic.List<string> TeamEmailAddresses {
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
        private System.Guid SprintIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid SprintStoryIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private AnyTrack.Infrastructure.BackendSprintService.ServiceStory StoryField;
        
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
        public AnyTrack.Infrastructure.BackendSprintService.ServiceStory Story {
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ServiceTask", Namespace="http://schemas.datacontract.org/2004/07/AnyTrack.Backend.Service.Model")]
    [System.SerializableAttribute()]
    public partial class ServiceTask : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private AnyTrack.Infrastructure.BackendSprintService.ServiceUser AssigneeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool BlockedField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ConditionsOfSatisfactionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DescriptionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid SprintStoryIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SummaryField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Collections.Generic.List<AnyTrack.Infrastructure.BackendSprintService.ServiceTaskHourEstimate> TaskHourEstimatesField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid TaskIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private AnyTrack.Infrastructure.BackendSprintService.ServiceUser TesterField;
        
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
        public AnyTrack.Infrastructure.BackendSprintService.ServiceUser Assignee {
            get {
                return this.AssigneeField;
            }
            set {
                if ((object.ReferenceEquals(this.AssigneeField, value) != true)) {
                    this.AssigneeField = value;
                    this.RaisePropertyChanged("Assignee");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Blocked {
            get {
                return this.BlockedField;
            }
            set {
                if ((this.BlockedField.Equals(value) != true)) {
                    this.BlockedField = value;
                    this.RaisePropertyChanged("Blocked");
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
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Collections.Generic.List<AnyTrack.Infrastructure.BackendSprintService.ServiceTaskHourEstimate> TaskHourEstimates {
            get {
                return this.TaskHourEstimatesField;
            }
            set {
                if ((object.ReferenceEquals(this.TaskHourEstimatesField, value) != true)) {
                    this.TaskHourEstimatesField = value;
                    this.RaisePropertyChanged("TaskHourEstimates");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid TaskId {
            get {
                return this.TaskIdField;
            }
            set {
                if ((this.TaskIdField.Equals(value) != true)) {
                    this.TaskIdField = value;
                    this.RaisePropertyChanged("TaskId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public AnyTrack.Infrastructure.BackendSprintService.ServiceUser Tester {
            get {
                return this.TesterField;
            }
            set {
                if ((object.ReferenceEquals(this.TesterField, value) != true)) {
                    this.TesterField = value;
                    this.RaisePropertyChanged("Tester");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="ServiceTaskHourEstimate", Namespace="http://schemas.datacontract.org/2004/07/AnyTrack.Backend.Service.Model")]
    [System.SerializableAttribute()]
    public partial class ServiceTaskHourEstimate : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double EstimateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double NewEstimateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid ServiceTaskHourEstimateIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid TaskIdField;
        
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
        public double NewEstimate {
            get {
                return this.NewEstimateField;
            }
            set {
                if ((this.NewEstimateField.Equals(value) != true)) {
                    this.NewEstimateField = value;
                    this.RaisePropertyChanged("NewEstimate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid ServiceTaskHourEstimateId {
            get {
                return this.ServiceTaskHourEstimateIdField;
            }
            set {
                if ((this.ServiceTaskHourEstimateIdField.Equals(value) != true)) {
                    this.ServiceTaskHourEstimateIdField = value;
                    this.RaisePropertyChanged("ServiceTaskHourEstimateId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid TaskId {
            get {
                return this.TaskIdField;
            }
            set {
                if ((this.TaskIdField.Equals(value) != true)) {
                    this.TaskIdField = value;
                    this.RaisePropertyChanged("TaskId");
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
        void AddSprint(System.Guid projectId, AnyTrack.Infrastructure.BackendSprintService.ServiceSprint sprint);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISprintService/AddSprint", ReplyAction="http://tempuri.org/ISprintService/AddSprintResponse")]
        System.Threading.Tasks.Task AddSprintAsync(System.Guid projectId, AnyTrack.Infrastructure.BackendSprintService.ServiceSprint sprint);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISprintService/EditSprint", ReplyAction="http://tempuri.org/ISprintService/EditSprintResponse")]
        void EditSprint(System.Guid sprintId, AnyTrack.Infrastructure.BackendSprintService.ServiceSprint updatedSprint);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISprintService/EditSprint", ReplyAction="http://tempuri.org/ISprintService/EditSprintResponse")]
        System.Threading.Tasks.Task EditSprintAsync(System.Guid sprintId, AnyTrack.Infrastructure.BackendSprintService.ServiceSprint updatedSprint);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISprintService/GetAllTasksForSprintCurrentUser", ReplyAction="http://tempuri.org/ISprintService/GetAllTasksForSprintCurrentUserResponse")]
        System.Collections.Generic.List<AnyTrack.Infrastructure.BackendSprintService.ServiceTask> GetAllTasksForSprintCurrentUser(System.Guid sprintId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISprintService/GetAllTasksForSprintCurrentUser", ReplyAction="http://tempuri.org/ISprintService/GetAllTasksForSprintCurrentUserResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<AnyTrack.Infrastructure.BackendSprintService.ServiceTask>> GetAllTasksForSprintCurrentUserAsync(System.Guid sprintId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISprintService/SaveUpdatedTaskHours", ReplyAction="http://tempuri.org/ISprintService/SaveUpdatedTaskHoursResponse")]
        void SaveUpdatedTaskHours(System.Collections.Generic.List<AnyTrack.Infrastructure.BackendSprintService.ServiceTask> tasks);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISprintService/SaveUpdatedTaskHours", ReplyAction="http://tempuri.org/ISprintService/SaveUpdatedTaskHoursResponse")]
        System.Threading.Tasks.Task SaveUpdatedTaskHoursAsync(System.Collections.Generic.List<AnyTrack.Infrastructure.BackendSprintService.ServiceTask> tasks);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISprintServiceChannel : AnyTrack.Infrastructure.BackendSprintService.ISprintService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SprintServiceClient : System.ServiceModel.ClientBase<AnyTrack.Infrastructure.BackendSprintService.ISprintService>, AnyTrack.Infrastructure.BackendSprintService.ISprintService {
        
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
        
        public void AddSprint(System.Guid projectId, AnyTrack.Infrastructure.BackendSprintService.ServiceSprint sprint) {
            base.Channel.AddSprint(projectId, sprint);
        }
        
        public System.Threading.Tasks.Task AddSprintAsync(System.Guid projectId, AnyTrack.Infrastructure.BackendSprintService.ServiceSprint sprint) {
            return base.Channel.AddSprintAsync(projectId, sprint);
        }
        
        public void EditSprint(System.Guid sprintId, AnyTrack.Infrastructure.BackendSprintService.ServiceSprint updatedSprint) {
            base.Channel.EditSprint(sprintId, updatedSprint);
        }
        
        public System.Threading.Tasks.Task EditSprintAsync(System.Guid sprintId, AnyTrack.Infrastructure.BackendSprintService.ServiceSprint updatedSprint) {
            return base.Channel.EditSprintAsync(sprintId, updatedSprint);
        }
        
        public System.Collections.Generic.List<AnyTrack.Infrastructure.BackendSprintService.ServiceTask> GetAllTasksForSprintCurrentUser(System.Guid sprintId) {
            return base.Channel.GetAllTasksForSprintCurrentUser(sprintId);
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.List<AnyTrack.Infrastructure.BackendSprintService.ServiceTask>> GetAllTasksForSprintCurrentUserAsync(System.Guid sprintId) {
            return base.Channel.GetAllTasksForSprintCurrentUserAsync(sprintId);
        }
        
        public void SaveUpdatedTaskHours(System.Collections.Generic.List<AnyTrack.Infrastructure.BackendSprintService.ServiceTask> tasks) {
            base.Channel.SaveUpdatedTaskHours(tasks);
        }
        
        public System.Threading.Tasks.Task SaveUpdatedTaskHoursAsync(System.Collections.Generic.List<AnyTrack.Infrastructure.BackendSprintService.ServiceTask> tasks) {
            return base.Channel.SaveUpdatedTaskHoursAsync(tasks);
        }
    }
}
