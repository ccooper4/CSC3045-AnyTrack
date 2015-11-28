using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Backend.Security
{
    /// <summary>
    /// An attribute used to mark a service that should auto-build it's principal.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class CreatePrincipalAttribute : Attribute
    {
        // No logic - used as a marker.
    }
}
