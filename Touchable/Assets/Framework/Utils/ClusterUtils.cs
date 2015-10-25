using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Framework.Utils
{
    internal static class ClusterUtils
    {
        private static StringBuilder hashString = new StringBuilder();

        public static String GetPointsHash(int[] pointIds)
        {
            hashString.Remove(0, hashString.Length);
            for (int i = 0; i < pointIds.Length; i++)
            {
                hashString.Append("#");
                hashString.Append(pointIds[i]);
            }
            return hashString.ToString();
        } 
    }
}
