using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Backend.Service.Model;

namespace AnyTrack.Backend.Service
{
    /// <summary>
    /// Outlines the sprint service.
    /// </summary>
     [ServiceContract]
    public interface ISprintService
    {
         /// <summary>
         /// Creates a sprint and adds it to the project.
         /// </summary>
         /// <param name="projectId">Id of the project to add the sprint to</param>
        /// <param name="sprint">ServiceSprint entity</param>
        [OperationContract]
        void AddSprint(Guid projectId, ServiceSprint sprint);

         /// <summary>
         /// Edits an exiting sprint.
         /// </summary>
         /// <param name="sprintId">Id of the sprint to be edited</param>
        /// <param name="updatedSprint">ServiceSprint entity containing changes</param>
        [OperationContract]
        void EditSprint(Guid sprintId, ServiceSprint updatedSprint);
    }
}
