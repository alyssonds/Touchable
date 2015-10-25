using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Framework.MultiTouchManager
{
    internal sealed class ClusterManager
    {
        #region Events

        public event EventHandler<ClusterUpdateEventArgs> ClustersToIdentifyHandler;
        public event EventHandler<ClusterUpdateEventArgs> ClustersMovedHandler;
        public event EventHandler<ClusterUpdateEventArgs> ClustersCancelledHandler;

        #endregion 
        #region Private properties
        private static ClusterManager _instance;
        private Dictionary<String,Cluster> clusters = new Dictionary<String, Cluster>();

        private List<Cluster> ClustersToIdentify = new List<Cluster>();
        private List<Cluster> IdentifiedClustersMoved = new List<Cluster>();
        private List<Cluster> IdentifiedClustersCancelled = new List<Cluster>();

        private bool ClustersRequestIdentification = false;
        private bool ClustersRequestMoved = false;
        private bool ClustersRequestCancellation = false;

        #endregion

        #region Public Properties
        public static ClusterManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ClusterManager();
                return _instance;
            }
        }

        public List<Cluster> Clusters
        {
            get
            {
                return clusters.Values.ToList();
            }
        }

        public float ClusterDistThreshold { get; private set; }
        #endregion 

        public ClusterManager() { }

        #region Public Methods
        public ClusterManager Initialize()
        {
            InputServer.Instance.InputUpdated += OnInputsUpdateEventHandler;
            return _instance;
        }

        public void Disable()
        {
            InputServer.Instance.InputUpdated -= OnInputsUpdateEventHandler;
        }

        public void SetClusterDistThreshold(float threshold)
        {
            this.ClusterDistThreshold = threshold;
        }
        #endregion

        #region Private Methods
        private String GetClusterIdFromTouchPoint(int touchPointId)
        {
            foreach(KeyValuePair<String,Cluster> entry in clusters)
            {
                if (entry.Value.PointsIds.Contains(touchPointId))
                    return entry.Key;
            }
            return null;
        }

        private void UpdateClusters()
        {
            float minDist = this.ClusterDistThreshold;
            float dist;
            bool minFound = false;
            List<Cluster> clustersList = this.clusters.Values.ToList();

            int mergeCluster1Index = 0;
            int mergeCluster2Index = 0;
            Cluster r1 = new Cluster();
            Cluster r2 = new Cluster();

            if (clusters.Count > 1)
            {
                while(minDist <= this.ClusterDistThreshold)
                {
                    for(int i=0; i < clustersList.Count; i++)
                    {
                        for(int j=i+1; j < clustersList.Count; j++)
                        {
                            Cluster c1 = clustersList.ElementAt(i);
                            Cluster c2 = clustersList.ElementAt(j);

                            dist = Vector2.Distance(c1.Centroid, c2.Centroid);

                            if(dist < minDist)
                            {
                                minDist = dist;
                                mergeCluster1Index = i;
                                mergeCluster2Index = j;
                                r1 = clustersList[i];
                                r2 = clustersList[j];
                                minFound = true;
                            }
                        }
                    }
                    if (minFound)
                    {
                        minFound = false;
                        Cluster mergedCluster = MergeClusters(clustersList.ElementAt(mergeCluster1Index), clustersList.ElementAt(mergeCluster2Index));
                        //clustersList.RemoveAt(mergeCluster1Index);
                        //clustersList.RemoveAt(mergeCluster2Index);
                        clustersList.Remove(r1);
                        clustersList.Remove(r2);
                        clustersList.Add(mergedCluster);
                    }
                    else
                        minDist *= 2;
                }

                clusters = clustersList.ToDictionary(v => v.Hash, v => v);
            }
        }

        private Cluster MergeClusters(Cluster c1, Cluster c2)
        {
            List<TouchInput> allPoints = c1.Points.Values.ToList();
            allPoints.AddRange(c2.Points.Values.ToList());

            return new Cluster(allPoints);
        }

        private void CheckClustersUpdated()
        {
            foreach(KeyValuePair<String,Cluster> entry in clusters)
            {
                Cluster cluster = entry.Value;
                String idHash = entry.Key;
                switch (cluster.State)
                {
                    case ClusterState.Unidentified:
                        {
                            //Cluster has reached for points and needs to be sent to identifier for check
                            ClustersToIdentify.Add(cluster);
                            clusters[idHash].State = ClusterState.Identidied;
                            ClustersRequestIdentification = true;
                            break;
                        }
                    case ClusterState.Updated:
                        {
                            //Identified cluster has moved
                            IdentifiedClustersMoved.Add(cluster);
                            ClustersRequestMoved = true;
                            break;
                        }
                    case ClusterState.Cancelled:
                        {
                            //Identified cluster was cancelled
                            IdentifiedClustersCancelled.Add(cluster);
                            ClustersRequestCancellation = true;
                            break;
                        }
                    case ClusterState.Invalid:
                        {
                            //This cluster is only made of finger touch points
                            foreach(KeyValuePair<int,TouchInput> record in cluster.Points)
                            {
                                InputManager.AddFingerTouch(record.Value);
                            }
                            break;
                        }
                }
            }
        }

        private void OnClusterToIdentify(ClusterUpdateEventArgs e)
        {
            EventHandler<ClusterUpdateEventArgs> handler;

            handler = ClustersToIdentifyHandler;
            if (handler != null)
            {
                handler(this, e);
            }

        }

        private void OnClustersMoved(ClusterUpdateEventArgs e)
        {
            EventHandler<ClusterUpdateEventArgs> handler;

            handler = ClustersMovedHandler;
            if (handler != null)
            {
                handler(this, e);
            }

        }

        private void OnClustersCancelled(ClusterUpdateEventArgs e)
        {
            EventHandler<ClusterUpdateEventArgs> handler;

            handler = ClustersCancelledHandler;
            if (handler != null)
            {
                handler(this, e);
            }

        }

        private void ResetClustersRequests()
        {
            ClustersToIdentify.Clear();
            IdentifiedClustersMoved.Clear();
            IdentifiedClustersCancelled.Clear();

            ClustersRequestIdentification = false;
            ClustersRequestMoved = false;
            ClustersRequestCancellation = false;

        }
        #endregion

        #region Event Handlers
        private void OnInputsUpdateEventHandler(object sender, InputUpdateEventArgs e)
        {
          
            String clusterHash;

            ResetClustersRequests();

            foreach (int touchId in InternalTouches.CancelledTouchBuffer)
            {
                clusterHash = GetClusterIdFromTouchPoint(touchId);
                if (clusterHash != null)
                {
                    if (clusters[clusterHash].PointsIds.Count > 1)
                    {
                        Cluster updatedCluster = clusters[clusterHash].RemovePoint(touchId);
                        clusters.Remove(clusterHash);
                        clusters.Add(updatedCluster.Hash, updatedCluster);
                    }
                    else
                        clusters.Remove(clusterHash);
                    
                }
                //Remove touch from fingers touch list
                InputManager.RemoveFingerTouch(touchId);

            }

            foreach (int touchId in InternalTouches.MovedTouchBuffer)
            {
                clusterHash = GetClusterIdFromTouchPoint(touchId);
                if (clusterHash != null)
                {
                    Cluster[] updatedCluster = clusters[clusterHash].UpdatePoint(touchId);
                    clusters.Remove(clusterHash);
                    clusters.Add(updatedCluster[0].Hash, updatedCluster[0]);

                    if (updatedCluster[1] != null)
                        clusters.Add(updatedCluster[1].Hash, updatedCluster[1]);

                }
                else
                {
                    Cluster c = new Cluster(InternalTouches.List[touchId]);
                    clusters.Add(c.Hash, c);
                }
            }


            foreach (int touchId in InternalTouches.BaganTouhBuffer)
            {
                Cluster cl = new Cluster(InternalTouches.List[touchId]);
                clusters.Add(cl.Hash, cl);

            }

            if(InternalTouches.CancelledTouchBuffer.Count > 0 || InternalTouches.MovedTouchBuffer.Count > 0 || InternalTouches.BaganTouhBuffer.Count > 0)
            {
                UpdateClusters();

                CheckClustersUpdated();

                if (ClustersRequestIdentification)
                    OnClusterToIdentify(new ClusterUpdateEventArgs("Identification request", ClustersToIdentify));

                if (ClustersRequestMoved)
                    OnClustersMoved(new ClusterUpdateEventArgs("Moved cluster request", IdentifiedClustersMoved));

                if (ClustersRequestCancellation)
                    OnClustersCancelled(new ClusterUpdateEventArgs("Moved cluster request", IdentifiedClustersCancelled)); 

            }
        }

        
        #endregion
    }

    internal class ClusterUpdateEventArgs : EventArgs
    {
        public string EventMsg { get; private set; }
        private List<Cluster> _updatedClusters = new List<Cluster>();

        public ClusterUpdateEventArgs(string msg, List<Cluster> clusters)
        {
            this.EventMsg = msg;
            this._updatedClusters = clusters;
        }

        internal List<Cluster> GetClusters()
        {
            return _updatedClusters;
        }
    }
}
