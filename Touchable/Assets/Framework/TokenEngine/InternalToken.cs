/*
 * @author Francesco Strada
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Framework.MultiTouchManager;

namespace Assets.Framework.TokenEngine
{
    internal sealed class InternalToken : IToken
    {
        #region Public Properties

        /// <inheritdoc />
        public int Id { get { return _id; } }
        
        /// <inheritdoc />
        public int Class { get { return _class; } }

        /// <summary>
        /// Unique hash inherited from cluster. <seealso cref="Cluster.Hash"/>
        /// </summary>
        public string HashId { get { return _hashId; } }

        /// <inheritdoc />
        public Vector2 Position { get { return _position; } }

        /// <inheritdoc />
        public Vector2 DeltaPosition { get { return _deltaPosition; } }

        /// <inheritdoc />
        public float Angle { get { return _angle; } }
        
        /// <inheritdoc />
        public float DeltaAngle { get { return _deltaAngle; } }
        

        #endregion

        #region Private Fields

        //Inherited properties from IToken interface
        private int _id;
        private int _class;
        private string _hashId;
        private Vector2 _position;
        private Vector2 _deltaPosition;
        private float _angle;
        private float _deltaAngle;

        //Token properties which are need for internal operations
        HashSet<int> _markersIds = new HashSet<int>();
        Dictionary<int, TokenMarker> markers = new Dictionary<int, TokenMarker>();
        Dictionary<MarkerType, TokenMarker> typeMarkers = new Dictionary<MarkerType, TokenMarker>();

        private Vector3 XAxisVector;
        private Vector3 YAxisVector;
        

        #endregion

        public InternalToken(string hashID, Dictionary<int, TokenMarker> markers)
        {
            this._hashId = hashID;
            this.markers = markers;
            this.typeMarkers = markers.Values.ToDictionary(m => m.Type, m => m);
            this.XAxisVector = typeMarkers[MarkerType.XAxis].Position - typeMarkers[MarkerType.Origin].Position;
            this.YAxisVector = typeMarkers[MarkerType.YAxis].Position - typeMarkers[MarkerType.Origin].Position;

            //For the moment position will be the origin marker

            _position = typeMarkers[MarkerType.Origin].Position;

            _deltaPosition = Vector2.zero;
            _deltaAngle = 0.0f;

        }

        #region Public Methods

        public void Update(Cluster cluster)
        {
            int markerId;
            Vector2 newPostion;
            foreach(KeyValuePair<int,TouchInput> entry in cluster.Points)
            {
                markerId = entry.Key;
                newPostion = entry.Value.Position;

                this.markers[markerId].UpdatePosition(newPostion);
            }

            UpdateMarkersTypeList();
            UpdateTokenAxis();
            UpdateAngle();
            _position = typeMarkers[MarkerType.Origin].Position;

        }

        public InternalToken UpdateAngle()
        {
            float newAngle;
            float direction = Vector3.Cross(Vector3.right, XAxisVector).normalized.z;
            float tmpAngle = Vector3.Angle(Vector3.right, XAxisVector);

            if (direction > 0)
               newAngle = tmpAngle;
            else
            {
                newAngle = 180 + (180 - tmpAngle);
            }

            this._deltaAngle = newAngle - this._angle;
            this._angle = newAngle;

            return this;
        }

        #region Private Methods

        private void UpdateMarkersTypeList()
        {
            this.typeMarkers.Clear();
            this.typeMarkers = markers.Values.ToDictionary(m => m.Type, m => m);
        }

        private void UpdateTokenAxis()
        {
            this.XAxisVector = typeMarkers[MarkerType.XAxis].Position - typeMarkers[MarkerType.Origin].Position;
            this.YAxisVector = typeMarkers[MarkerType.YAxis].Position - typeMarkers[MarkerType.Origin].Position;
        }

        #endregion

        #endregion
    }
}
