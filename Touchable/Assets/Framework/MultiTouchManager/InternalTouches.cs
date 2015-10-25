using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Framework.MultiTouchManager
{
    internal static class InternalTouches
    {
        #region Constants
        private const int MAX_CONTEMPORARY_TOUCHES = 11;
        #endregion

        //TODO make sure lists are never null in case they can be empty
        #region Public Properties

        public static Dictionary<int,TouchInput> List
        {
            get
            {
                return _touches;
            }
            private set {;}
        }

        public static HashSet<int> BaganTouhBuffer
        {
            get
            {
                return _beganBuffer;
            }
            private set {; }
        }
        public static HashSet<int> MovedTouchBuffer
        {
            get
            {
                return _movedBuffer;
            }
            private set {; }
        }
        public static HashSet<int> CancelledTouchBuffer
        {
            get
            {
                return _cancelledBuffer;
            }
            private set {; }
        }

        #endregion

        #region Private variables
        private static HashSet<int> touchIds = new HashSet<int>();
        private static Dictionary<int, TouchInput> _touches = new Dictionary<int, TouchInput>(MAX_CONTEMPORARY_TOUCHES);

        private static HashSet<int> _beganBuffer = new HashSet<int>();
        private static HashSet<int> _movedBuffer = new HashSet<int>();
        private static HashSet<int> _cancelledBuffer = new HashSet<int>();

        private static TouchInput tmpTouchInput;

        private static bool updateBeganTouches = false;
        private static bool updateMovedTouches = false;
        private static bool updateEndedTouches = false;
        private static bool updateCanceledTouches = false;
        #endregion

        #region Public Methods

        /// <summary>
        /// Return true if there have been updates in TouchInputs
        /// </summary>
        /// <returns></returns>
        public static bool Updated()
        {
            return updateBeganTouches || updateMovedTouches || updateEndedTouches || updateCanceledTouches;
        }

        public static void ResetBuffers()
        {
            _beganBuffer.Clear();
            _movedBuffer.Clear();
            _cancelledBuffer.Clear();

            updateBeganTouches = false;
            updateMovedTouches = false;
            updateEndedTouches = false;
            updateCanceledTouches = false;

        }

        public static void UpdateTouchInput(Touch incomingTouch)
        {
            switch (incomingTouch.phase)
            {
                case TouchPhase.Began:
                    {
                        if (!touchIds.Contains(incomingTouch.fingerId))
                        {
                          //  lock (_beganBuffer)
                          //  {
                                //New TouchInput has arrives
                                touchIds.Add(incomingTouch.fingerId);
                                AddTouchToList(incomingTouch, TouchState.Began);

                                //Add ID to BeganBuffer
                                _beganBuffer.Add(incomingTouch.fingerId);
                                updateBeganTouches = true;
                                break;
                          //  }
                        }
                        else
                        {
                         //   lock(_beganBuffer)
                         //   {
                                //We missed a frame
                                AddTouchToList(incomingTouch, TouchState.Began);
                                _cancelledBuffer.Add(incomingTouch.fingerId);
                                _beganBuffer.Add(incomingTouch.fingerId);
                                updateBeganTouches = true;
                                break;
                          //  }
                        }
                    }
                case TouchPhase.Moved:
                    {
                        if (!touchIds.Contains(incomingTouch.fingerId))
                        {
                           // lock (_beganBuffer)
                          //  {
                                //We missed TouchInput Began state
                                touchIds.Add(incomingTouch.fingerId);
                                AddTouchToList(incomingTouch, TouchState.Moved);
                                _beganBuffer.Add(incomingTouch.fingerId);
                                updateBeganTouches = true;

                                break;
                           // }
                        }
                        else
                        {
                          //  lock (_movedBuffer)
                          //  {
                                //An update has occurred of TouchInput
                                AddTouchToList(incomingTouch, TouchState.Moved);
                                _movedBuffer.Add(incomingTouch.fingerId);
                                updateMovedTouches = true;
                                break;
                          //  }
                        }
                    }
                case TouchPhase.Ended:
                    {
                        if (!touchIds.Contains(incomingTouch.fingerId))
                        {
                            //We completly missed TouchInput begin-end transitions
                            break;
                        }
                        else
                        {
                          //  lock (_cancelledBuffer)
                           // {
                                touchIds.Remove(incomingTouch.fingerId);
                                _touches.Remove(incomingTouch.fingerId);
                                _cancelledBuffer.Add(incomingTouch.fingerId);
                                updateEndedTouches = true;
                                break;
                           // }
                        }
                    }
                case TouchPhase.Canceled:
                    {
                        if (!touchIds.Contains(incomingTouch.fingerId))
                        {
                            //We completly missed TouchInput begin-end transitions
                            break;
                        }
                        else
                        {
                          //  lock (_cancelledBuffer)
                          //  {
                                touchIds.Remove(incomingTouch.fingerId);
                                _touches.Remove(incomingTouch.fingerId);
                                _cancelledBuffer.Add(incomingTouch.fingerId);
                                updateCanceledTouches = true;
                                break;
                           // }
                        }
                    }
                case TouchPhase.Stationary:
                    {
                        if (!touchIds.Contains(incomingTouch.fingerId))
                        {
                            //We missed TouchInput Began Phase
                            touchIds.Add(incomingTouch.fingerId);
                            AddTouchToList(incomingTouch, TouchState.Stationary);

                            //Add ID to BeganBuffer
                            _beganBuffer.Add(incomingTouch.fingerId);
                            updateBeganTouches = true;

                            break;
                        }
                        else
                        {
                            AddTouchToList(incomingTouch, TouchState.Stationary);
                            break;
                        }
                    }
            }
        }
        #endregion

        #region Private Methods

        private static void AddTouchToList(Touch t, TouchState state)
        {
            _touches[t.fingerId] = new TouchInput(t.fingerId, t.position, state);
        }

        #endregion
    }
}
