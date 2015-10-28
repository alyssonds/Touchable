/*
 * @author Francesco Strada
 */

using Assets.Framework.MultiTouchManager;
using UnityEngine;

namespace Assets.Framework.TokenEngine
{
    internal sealed class TokenMarker : ITouchInput
    {
        public int Id
        {
            get
            {
                return _id;
            }
        }

        public Vector2 Position
        {
            get
            {
                return _position;
            }
        }

        public TouchState State
        {
            get
            {
                return _state;
            }
        }

        public MarkerType Type
        {
            get
            {
                return _type;
            }
        }

        #region Private variables

        private int _id;
        private Vector2 _position;
        //TODO ADD x,y position with respect to Token
        private TouchState _state;
        private MarkerType _type;

        #endregion

        public TokenMarker(int id, Vector2 postion, TouchState state, MarkerType type)
        {
            this._id = id;
            this._position = postion;
            this._state = state;
            this._type = type;
        }

        #region Public methods

        public void UpdatePosition(Vector2 newPostion)
        {
            this._position = newPostion;
        }

        #endregion
    }

    /// <summary>
    /// Different types a Token's marker can be
    /// </summary>
    public enum MarkerType
    {
        /// <summary>
        /// Marker representing the Origin within the Token domain
        /// </summary>
        Origin,

        /// <summary>
        /// Marker representing the X axis within the Token domain
        /// </summary>
        XAxis,

        /// <summary>
        /// Marker representing the Y axis within the Token domain
        /// </summary>
        YAxis,

        /// <summary>
        /// Marker representing the token's unique id, information encoded in its (x,y) coordinates within the token domanin
        /// </summary>
        Data,
        
        /// <summary>
        /// Marker type not identified 
        /// </summary>
        Unknown
    }
}
