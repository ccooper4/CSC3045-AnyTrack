using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Backend.Data.Model;

namespace AnyTrack.Backend.Service.Model
{
    /// <summary>
    /// Represents a Sprint Entity.
    /// </summary>
    public class ServiceSprint
    {
        #region Fields

        /// <summary>
        /// Sprint start date.
        /// </summary>
        private DateTime startDate;

        /// <summary>
        /// Sprint end date.
        /// </summary>
        private DateTime endDate;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new ServiceSprint Entity.
        /// </summary>
        public ServiceSprint()
        {
            TeamEmailAddresses = new List<string>();
            Backlog = new List<ServiceSprintStory>();
            Length = CalculateSprintLength();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the sprint's id.
        /// </summary>
        public Guid SprintId { get; set; }

        /// <summary>
        /// Gets or sets the sprint's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the sprint's start date.
        /// </summary>
        public DateTime StartDate
        {
            get
            {
                return startDate;
            }

            set
            {
                startDate = value;

                Length = CalculateSprintLength();
            }
        }

        /// <summary>
        /// Gets or sets the sprint's end date. 
        /// </summary>
        public DateTime EndDate
        {
            get
            {
                return endDate;
            }

            set
            {
                endDate = value;

                Length = CalculateSprintLength();
            }
        }

        /// <summary>
        /// Gets or sets the sprint's length in days
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Gets or sets the sprint's descriptions.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a collection of the sprint's team members.
        /// </summary>
        public List<string> TeamEmailAddresses { get; set; }

        /// <summary>
        /// Gets or sets a collection of the sprint's backlog of stories.
        /// </summary>
        public List<ServiceSprintStory> Backlog { get; set; }

        #endregion
        
        #region Methods

        /// <summary>
        /// Calculates the sprint length in days.
        /// </summary>
        /// <returns>Length of days</returns>
        public int CalculateSprintLength()
        {
            return (EndDate - StartDate).Days + 1;
        }

        #endregion
    }
}
