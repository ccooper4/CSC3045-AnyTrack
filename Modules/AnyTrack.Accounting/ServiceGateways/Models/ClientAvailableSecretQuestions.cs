using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Accounting.ServiceGateways.Models
{
    /// <summary>
    /// Class representing the available secret questions.
    /// </summary>
    public sealed class ClientAvailableSecretQuestions
    {
        /// <summary>
        /// Secret question 1.
        /// </summary>
        public static readonly string MaidenName = "What is your mothers maiden name?";

        /// <summary>
        /// Secret question 2.
        /// </summary>
        public static readonly string FirstSchool = "What is the name of your first school?";

        /// <summary>
        /// Secret question 3.
        /// </summary>
        public static readonly string FirstPet = "What is the name of you first pet?";

        /// <summary>
        /// All the secret questions.
        /// </summary>
        /// <returns>A list of the questions.</returns>
        public static List<string> All()
        {
            List<string> questionsList = new List<string>();
            questionsList.Add(MaidenName);
            questionsList.Add(FirstSchool);
            questionsList.Add(FirstPet);
            return questionsList;
        } 
    }
}