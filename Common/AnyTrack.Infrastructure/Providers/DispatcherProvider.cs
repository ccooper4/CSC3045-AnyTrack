using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace AnyTrack.Infrastructure.Providers
{
    /// <summary>
    /// Provides a wrapper around a Dispatcher. 
    /// </summary>
    public class DispatcherProvider
    {
        #region Fields 

        /// <summary>
        /// The dispatcher wrapped by this provider.
        /// </summary>
        private Dispatcher dispatcher; 

        #endregion 

        #region Constructor

        /// <summary>
        /// Constructs a new DispatcherProvider that wraps the specified dispatcher.
        /// </summary>
        /// <param name="dispatcher">The dispatcher to wrap.</param>
        public DispatcherProvider(Dispatcher dispatcher)
        {
            if (dispatcher == null) 
            {
                throw new ArgumentNullException("dispatcher");
            }

            this.dispatcher = dispatcher; 
        }

        #endregion

        #region Methods 

        /// <summary>
        /// Invokes the specified action using the wrapped dispatcher.
        /// </summary>
        /// <param name="action">The action to invoke.</param>
        public virtual void InvokeAction(Action action)
        {
            dispatcher.Invoke(action);
        }

        #endregion
    }
}
