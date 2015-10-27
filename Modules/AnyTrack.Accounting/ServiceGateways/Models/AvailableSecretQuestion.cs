using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Accounting.ServiceGateways.Models
{
    /// <summary>
    /// Class for the secret questions used during registration.
    /// </summary>
    public class AvailableSecretQuestion
    {
        /// <summary>
        /// Gets or sets the Secret question.
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// Gets or sets the Guid for the secret question.
        /// </summary>
        public Guid Id { get; set; }
    }
}
