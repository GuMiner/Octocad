using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Octocad_2D
{
    /// <summary>
    /// Ties together endpoints of different items for constraints.
    /// </summary>
    class Knot
    {
        public List<int> primitiveIndexes;
        public List<int?> endpointIndexes;

        /// <summary>
        /// Whether the endpoint this not is associated with is an intersection. The 
        /// primitives with null indexes are those forming the intersection.
        /// </summary>
        public bool isIntersectionKnot;
        
        /// <summary>
        /// Whether or not these endpoints are stuck at their current location.
        /// </summary>
        public bool locked;

        public Knot()
        {
            primitiveIndexes = new List<int>();
            endpointIndexes = new List<int?>();
            locked = false;
            isIntersectionKnot = false;

        }
    }
}
