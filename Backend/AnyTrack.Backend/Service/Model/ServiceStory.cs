using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Backend.Service.Model
{
    /// <summary>
    /// Represents a story entity.
    /// </summary>
    public class ServiceStory
    {
        /// <summary>
        /// Creates a new ServiceStory Entity
        /// </summary>
        public ServiceStory()
        {
            StoryId = new Guid();
        }

        /// <summary>
        /// Gets or sets Story ID
        /// </summary>
        public Guid StoryId { get; set; }

        /// <summary>
        /// Gets or sets the story summary
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets the projectId
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the conditions of satisfaction
        /// </summary>
        public string ConditionsOfSatisfaction { get; set; }

        /// <summary>
        /// Gets or sets the asA value.
        /// </summary>
        public string AsA { get; set; }

        /// <summary>
        /// Gets or sets the iWant value.
        /// </summary>
        public string IWant { get; set; }

        /// <summary>
        /// Gets or sets the soThat value.
        /// </summary>
        public string SoThat { get; set; }

        /// <summary>
        /// Sets the story breakdown.
        /// </summary>
        /// <param name="asA">The As A field</param>
        /// <param name="iWant">The I Want field</param>
        /// <param name="soThat">The So That field</param>
        public void SetStoryBreakdown(string asA, string iWant, string soThat)
        {
            this.AsA = asA;
            this.IWant = iWant;
            this.SoThat = soThat;
        }
    }
}
