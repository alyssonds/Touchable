using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Framework.MultiTouchManager;

namespace Assets.Framework
{
    public static class InputManager
    {
        private static SortedDictionary<int, FingerTouch> _touches = new SortedDictionary<int, FingerTouch>();

        public static List<FingerTouch> Touches
        {
            get
            {
                return _touches.Values.ToList();
            }
        }

        public static FingerTouch GetTouch(int id)
        {
            return _touches[id];
        }

        public static int FingersCount()
        {
            return _touches.Count;
        }


        internal static void AddFingerTouch(TouchInput t)
        {
            _touches[t.Id] = new FingerTouch(t);
        }

        internal static void RemoveFingerTouch(int id)
        {
            if(_touches.ContainsKey(id))
                _touches.Remove(id);
        }


        
          
    }
}
