/*
 * @author Francesco Strada
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Framework.MultiTouchManager;

namespace Assets.Framework.TokenEngine
{
    internal sealed class TokenManager
    {
        #region Events
        internal event EventHandler<InternalTokenIdentifiedEventArgs> TokenIdentifiedEvent;
        internal event EventHandler<InternalTokenCancelledEventArgs> TokenCancelledEvent;
        #endregion

        #region Private Fields

        private static readonly TokenManager _instance = new TokenManager();

        private Dictionary<string, InternalToken> tokens = new Dictionary<string, InternalToken>();

        readonly object TokenCallBackLock = new object();

        #endregion

        #region Public Properties

        public static TokenManager Instance
        {
            get
            {
                return _instance;
            }
        }

        public List<InternalToken> Tokens
        {
            get
            {
                return tokens.Values.ToList();
            }
        }

        #endregion

        //Private constructor
        private TokenManager() { }

        #region Public Methods

        public TokenManager Initialize()
        {
            ClusterManager.Instance.ClustersToIdentifyEvent += OnClustersToIdentify;
            ClusterManager.Instance.ClustersMovedEvent += OnClustersMoved;
            ClusterManager.Instance.ClustersCancelledEvent += OnClustersCancelled;

            return _instance;
        }

        #endregion

        #region Event Handlers

        private void OnClustersToIdentify(object sender, ClusterUpdateEventArgs e)
        {
            foreach(Cluster cluster in e.GetClusters())
            {
                //TODO this function requires updates
                InternalToken token = TokenIdentification.Instance.IdentifyCluster(cluster);
                
                if(token != null)
                {
                    //Cluster Identification succesfull
                    //Add new token here 
                    tokens.Add(token.HashId, token);
                    //Add Token To Global List

                    //Fire event token identified
                    
                    //Notify CM cluster has been identified
                    FireTokenIdentified(new InternalTokenIdentifiedEventArgs(token.HashId,true));
                }
                else
                {
                    //Cluser Identification failed, need to report back to CM
                    FireTokenIdentified(new InternalTokenIdentifiedEventArgs(cluster.Hash, false));

                }
            }
        }

        private void OnClustersMoved(object sender, ClusterUpdateEventArgs e)
        {
            foreach(Cluster cluster in e.GetClusters())
            {
                //Update internally the token
                InternalToken token;
                if(tokens.TryGetValue(cluster.Hash,out token))
                {
                    token.Update(cluster);
                    tokens.Remove(cluster.Hash);
                    tokens.Add(token.HashId, token);
                    //Here check deltas in order to fire or not Events
                }
            }
        }

        private void OnClustersCancelled(object sender, ClusterUpdateEventArgs e)
        {
            foreach(Cluster cluster in e.GetClusters())
            {
                //Cancle cluster according to point
                //Here is very delicate because it must be considered also the possibility of not removing the token untill not all points have been removed

                if (tokens.ContainsKey(cluster.CancelledClusterHash))
                {
                    tokens.Remove(cluster.CancelledClusterHash);
                    FireTokenCancelled(new InternalTokenCancelledEventArgs(cluster.Hash));
                }

            }
        }

        #endregion

        #region Event Launchers

        private void FireTokenIdentified(InternalTokenIdentifiedEventArgs e)
        {
            EventHandler<InternalTokenIdentifiedEventArgs> handler;

            lock (TokenCallBackLock)
            {
                handler = TokenIdentifiedEvent;
            }
            if(handler != null)
            {
                handler(this, e);
            }
        }

        private void FireTokenCancelled(InternalTokenCancelledEventArgs e)
        {
            EventHandler<InternalTokenCancelledEventArgs> handler;
            lock (TokenCallBackLock)
            {
                handler = TokenCancelledEvent;
            }
            
            if(handler != null)
            {
                handler(this, e);
            }
        }
        #endregion
    }

    internal class InternalTokenIdentifiedEventArgs : EventArgs
    {
        private string _tokenHashId;
        private bool _success;

        internal string TokenHashId { get { return _tokenHashId; } }
        internal bool Success { get { return _success; } }

        internal InternalTokenIdentifiedEventArgs(string tokenHashId, bool success)
        {
            this._tokenHashId = tokenHashId;
            this._success = success;
        }
    }

    internal class InternalTokenCancelledEventArgs : EventArgs
    {
        private string _tokenHashId;

        internal string TokenHashId { get { return _tokenHashId; } }

        internal InternalTokenCancelledEventArgs(string tokenHashId)
        {
            this._tokenHashId = tokenHashId;
        }
    }
}
