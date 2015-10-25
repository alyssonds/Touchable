using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Framework.MultiTouchManager
{
    /// <summary>
    /// Class which internally represents a touch.
    /// </summary>
    internal sealed class TouchInput : ITouchInput
    {
        #region Constants
        
        #endregion

        #region Public properties

        /// <summary>
        /// Unique ID which identifies TouchPoint
        /// </summary>
        public int Id { get; private set; }
        
        /// <summary>
        /// TouchPoint state at current frame
        /// </summary>
        public TouchState State { get; private set; }
        
        /// <summary>
        /// TouchPoint current position (x,y) screen coordinates
        /// </summary>
        public Vector2 Position { get; private set; }

        /// <summary>
        /// TouchPoint position in previous frame
        /// </summary>
        public Vector2 PreviousPosition { get; private set; }

        /// <summary>
        /// Id reppresenting cluster which TouchPoint is part of
        /// </summary>
        public int? ClusterId { get; private set; }

        //TODO add TimeStamp variable
        #endregion
        
        public TouchInput(int id, Vector2 position, TouchState state)
        {
            this.Id = id;
            this.Position = position;
            this.State = state;

            //ClusterId is initially set to null and updated when first inseted 
            //into a new Cluster 
            this.ClusterId = null;
        }

        public void SetState(TouchState state)
        {
            this.State = state;
        }

        public void SetPosition(Vector2 position)
        {
            this.Position = position;
        }

        public bool Equals(TouchInput other)
        {
            if (this.Position.x == other.Position.x || this.Position.y == other.Position.y)
                return true;
            else
                return false;
        }

        public bool Equals(TouchInput other, float pxThreshold)
        {
            float dist = Vector2.Distance(this.Position, other.Position);

            if (dist <= pxThreshold)
                return true;
            else
                return false;
        }

    }

    /// <summary>
    /// Possible touch states
    /// </summary>
    public enum TouchState
    {
        /// <summary>
        /// TouchPoint started
        /// </summary>
        Began,

        /// <summary>
        /// TouchPoint has not changed since previous frame
        /// </summary>
        Stationary,

        /// <summary>
        /// TouchPoint updated his position since last frame
        /// </summary>
        Moved,

        /// <summary>
        /// TouchPoint was removed from screen
        /// </summary>
        Ended
    }

}
