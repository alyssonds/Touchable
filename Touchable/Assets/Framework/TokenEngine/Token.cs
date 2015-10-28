using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Framework.TokenEngine
{
    public class Token : IToken
    {
        #region Private Properties
        private int _id;
        private int _class;
        private Vector2 _position;
        private Vector2 _deltaPosition;
        private float _angle;
        private float _deltaAngle;
        #endregion
        #region Public Fields 
        /// <inheritdoc />
        public int Id { get { return _id; } }

        /// <inheritdoc />
        public int Class { get { return _class; } }

        /// <inheritdoc />
        public Vector2 Position { get { return _position; } }

        /// <inheritdoc />
        public Vector2 DeltaPosition { get { return _deltaPosition; } }

        /// <inheritdoc />
        public float Angle { get { return _angle; } }

        /// <inheritdoc />
        public float DeltaAngle { get { return _deltaAngle; } }

        #endregion

        internal Token(InternalToken internalToken)
        {
            this._id = internalToken.Id;
            this._class = internalToken.Class;
            this._position = internalToken.Position;
            this._angle = internalToken.Angle;
            
        }
    }
}
