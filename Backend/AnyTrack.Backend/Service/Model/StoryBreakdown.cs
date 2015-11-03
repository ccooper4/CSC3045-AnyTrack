using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Backend.Service.Model
{
    /// <summary>
    /// Class for storing the user story inside a story. i.e. the as.. i would.. so that.. section.
    /// </summary>
    public class StoryBreakdown
    {
        /// <summary>
        /// Constructs a user story.
        /// </summary>
        /// <param name="asA">The as a variable</param>
        /// <param name="i">The i would variable</param>
        /// <param name="so">The so that variable</param>
        public StoryBreakdown(string asA, string i, string so)
        {
            this.AsA = asA;
            this.IWould = i;
            this.SoThat = so;
        }

        /// <summary>
        /// Gets or sets the AsA property.
        /// </summary>
        public string AsA { get; set; }

        /// <summary>
        /// Gets or sets the IWould property.
        /// </summary>
        public string IWould { get; set; }

        /// <summary>
        /// Gets or sets the SoThat property.
        /// </summary>
        public string SoThat { get; set; }
    }
}
