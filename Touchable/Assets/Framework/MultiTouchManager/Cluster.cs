using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Framework.Utils;

namespace Assets.Framework.MultiTouchManager
{
    internal sealed class Cluster
    {
        #region Public Properties

        public int Id { get; private set; }

        public String Hash { get; private set; }

        public Vector2 Centroid { get { return _centroid; } private set {} }

        public Dictionary<int,TouchInput> Points
        {
            get
            {
                return _points;
            }
            private set {}
        }

        public HashSet<int> PointsIds
        {
            get
            {
                return _pointsIds;
            }
            private set {}
        }

        public ClusterState State { get; set; }

        #endregion

        public Cluster(TouchInput point)
        {
            this.AddPoint(point.Id);
            this.Hash = "#" + point.Id;
            this.Centroid = point.Position;
            State = ClusterState.Invalid;
        }
        public Cluster(List<TouchInput> touchPoints)
        {
            foreach(TouchInput t in touchPoints)
            {
                _pointsIds.Add(t.Id);
                _points.Add(t.Id, t);
            }
            UpdateCentroid();
            this.Hash = ClusterUtils.GetPointsHash(_pointsIds.ToArray<int>());
            if (_pointsIds.Count == 4)
                this.State = ClusterState.Unidentified;
            else
                this.State = ClusterState.Invalid; 
        }

        public Cluster() { }

        #region Public Methods

        public Cluster AddPoint(int touchId)
        {
            TouchInput touch = new TouchInput(touchId, InternalTouches.List[touchId].Position, InternalTouches.List[touchId].State);
            _pointsIds.Add(touchId);
            _points.Add(touchId, touch);
            UpdateCentroid();
            this.Hash = ClusterUtils.GetPointsHash(_pointsIds.ToArray<int>());

            if (_pointsIds.Count == 4)
                State = ClusterState.Unidentified;

            else if (_pointsIds.Count > 4)
                State = ClusterState.Invalid;

            return this;
        }

        public Cluster[] UpdatePoint(int touchId)
        {
            Cluster newCluster;
            if(Vector2.Distance(this.Centroid, InternalTouches.List[touchId].Position) < ClusterManager.Instance.ClusterDistThreshold)
            {
                //Point still in clustrer
                _points[touchId] = new TouchInput(touchId, InternalTouches.List[touchId].Position, TouchState.Moved);
                newCluster = null;

                if (State == ClusterState.Identidied)
                    State = ClusterState.Updated;
            }
            else
            {
                _pointsIds.Remove(touchId);
                _points.Remove(touchId);
                newCluster = new Cluster(InternalTouches.List[touchId]);

                if (_pointsIds.Count == 4)
                    State = ClusterState.Unidentified;
                else
                    State = ClusterState.Invalid;
            }

            UpdateCentroid();            
            this.Hash = ClusterUtils.GetPointsHash(_pointsIds.ToArray<int>());
            
            return new Cluster[] { this ,newCluster };

        }

        public Cluster RemovePoint(int touchId)
        {
            _pointsIds.Remove(touchId);
            _points.Remove(touchId);
            UpdateCentroid();
            this.Hash = ClusterUtils.GetPointsHash(_pointsIds.ToArray<int>());

            switch (State)
            {
                case ClusterState.Identidied:
                    {
                        State = ClusterState.Cancelled;
                        break;
                    }
                default:
                    {
                        State = ClusterState.Invalid;
                        break;
                    }
            }

            return this;
        }


        #endregion

        #region Private Methods
        private void UpdateCentroid()
        {
            float xTmp = 0f;
            float yTmp = 0f; 
            foreach(KeyValuePair<int,TouchInput> entry in _points)
            {
                xTmp += entry.Value.Position.x;
                yTmp += entry.Value.Position.y;                
            }

           if(_points.Count != 0)
            {
                _centroid.x = xTmp / _points.Count;
                _centroid.y = yTmp / _points.Count;
            }
           
        }
        #endregion

        #region Private Variables

        private Dictionary<int, TouchInput> _points = new Dictionary<int, TouchInput>();
        private HashSet<int> _pointsIds = new HashSet<int>();
        private Vector2 _centroid;
        #endregion
    }
    
    public enum ClusterState
    {
        Unidentified,

        Identidied,

        Updated,

        Cancelled,

        Invalid
    }
}