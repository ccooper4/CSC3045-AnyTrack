using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace AnyTrack.Backend.Security
{
    /// <summary>
    /// An extension for Unity that enables automatic construction of the principal.
    /// </summary>
    public class BuildPrincipalUnityExtension : UnityContainerExtension
    {
        #region Methods 

        /// <summary>
        /// Allows for the custom logic to be added to the container.
        /// </summary>
        protected override void Initialize()
        {
            this.Context.Strategies.Add(new BuildPrincipalUnityStrategy(Container), UnityBuildStage.PostInitialization);
        }

        #endregion
    }
}
