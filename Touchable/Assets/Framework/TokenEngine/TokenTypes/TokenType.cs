using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Framework.TokenEngine
{
    abstract class TokenType
    {
        internal readonly float DistanceOriginCenter;
        internal readonly float DistanceOriginAxisMarkers;

        //internal float DistanceOriginToCenter { get { return _distanceOriginCenter; } }
        //internal float DistanceOriginToAxis { get { return _distanceOriginAxisMarkers; } }

        protected TokenType()
        {
            DistanceOriginCenter = SetOriginToCenterDistance();
            DistanceOriginAxisMarkers = SetOriginToAxisDistance();
        }

        internal abstract float SetOriginToCenterDistance();
        internal abstract float SetOriginToAxisDistance();

    }
}
