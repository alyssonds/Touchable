using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Framework.TokenEngine;
using Assets.Framework.MultiTouchManager;

namespace Assets.Framework.Utils
{
    internal static class TokenUtils
    {

        public static Dictionary<int,TokenMarker> ConvertTouchInputToMarkers(int[] orderedIndexes, Dictionary<int,TouchInput> clusterPoints)
        {
            Dictionary<int, TokenMarker> result = new Dictionary<int, TokenMarker>();

            for (int i = 0; i < orderedIndexes.Length; i++)
            {
                TouchInput ti = clusterPoints[orderedIndexes[i]];

                switch (i)
                {
                    case 0:
                        {
                            TokenMarker tm = new TokenMarker(ti.Id, ti.Position, ti.State, MarkerType.Origin);
                            result.Add(tm.Id, tm);
                            break;
                        }
                    case 1:
                        {
                            TokenMarker tm = new TokenMarker(ti.Id, ti.Position, ti.State, MarkerType.XAxis);
                            result.Add(tm.Id, tm);
                            break;
                        }
                    case 2:
                        {
                            TokenMarker tm = new TokenMarker(ti.Id, ti.Position, ti.State, MarkerType.YAxis);
                            result.Add(tm.Id, tm);
                            break;
                        }
                    case 3:
                        {
                            TokenMarker tm = new TokenMarker(ti.Id, ti.Position, ti.State, MarkerType.Data);
                            result.Add(tm.Id, tm);
                            break;
                        }
                }
            }

            return result;
        }
    }
}
