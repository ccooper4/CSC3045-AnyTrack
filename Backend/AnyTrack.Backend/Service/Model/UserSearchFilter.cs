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
    public class UserSearchFilter
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
    }
}
