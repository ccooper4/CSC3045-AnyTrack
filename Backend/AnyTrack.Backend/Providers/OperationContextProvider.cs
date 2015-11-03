using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Backend.Providers
{
    /// <summary>
    /// Provides a wrapper around the WCF Operation Context
    /// </summary>
    public class OperationContextProvider
    {
        #region Properties 

        /// <summary>
        /// Gets the Incoming Message Properties from WCF.
        /// </summary>
        public virtual MessageProperties IncomingMessageProperties
        {
            get
            {
                if (OperationContext.Current != null)
                {
                    return OperationContext.Current.IncomingMessageProperties;
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion 
    }
}
