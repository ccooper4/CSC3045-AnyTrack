using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Backend.Service.Model
{
    /// <summary>
    /// Represents the fields that can be used to filter a user search.
    /// </summary>
    public class ServiceUserSearchFilter
    {
        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Scrum Masters should be included in the results. 
        /// </summary>
        public bool? ScrumMaster { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Product Owners should be included in the results. 
        /// </summary>
        public bool? ProductOwner { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Developers shoulde be included in the results.
        /// </summary>
        public bool? Developer { get; set; }

        /// <summary>
        /// Gets or sets a required skillset.
        /// </summary>
        public List<string> Skillset { get; set; } 

        /// <summary>
        /// Gets or sets the start date of a sprint.
        /// </summary>
        public DateTime? SprintStartingDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of a sprint.
        /// </summary>
        public DateTime? SprintEndingDate { get; set; }
    }
}
