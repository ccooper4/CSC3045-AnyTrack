﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Backend.Service.Model
{
    /// <summary>
    /// Represents a Project entity
    /// </summary>
    public class ServiceProject
    {
        #region Constructor

        /// <summary>
        /// Creates a new Project Entity
        /// </summary>
        public ServiceProject()
        {
            ScrumMasterEmailAddresses = new List<string>();
        }
        #endregion

        /// <summary>
        /// Gets or sets Project ID
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// Gets or sets ServiceStory ID
        /// </summary>
        public ServiceStory ServiceStoryId { get; set; }

        /// <summary>
        /// Gets or sets Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets Version Control
        /// </summary>
        public string VersionControl { get; set; }

        /// <summary>
        /// Gets or sets date Started On
        /// </summary>
        public DateTime StartedOn { get; set; }

        /// <summary>
        /// Gets or sets Project Manager
        /// </summary>
        public string ProjectManagerEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets Product Owner
        /// </summary>
        public string ProductOwnerEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets ScrumMasters
        /// </summary>
        public List<string> ScrumMasterEmailAddresses { get; set; }

        /*
        /// <summary>
        /// Gets or sets Sprints
        /// </summary>
       public ICollection<Sprint> Sprints { get; set; }
        */

        /// <summary>
        /// Gets or sets Stories
        /// </summary>
        public List<ServiceStory> Stories { get; set; }

        #region Methods

        /// <summary>
        /// Create a new ServiceStory Entity
        /// </summary>
        /// <returns>Returns storyId</returns>
        public ServiceStory Story()
        {
            ServiceStoryId = new ServiceStory();
            return ServiceStoryId;
        }
        #endregion Methods
    }
}
