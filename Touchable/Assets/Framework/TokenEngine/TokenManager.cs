﻿/*
 * @author Francesco Strada
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Framework.MultiTouchManager;
using Assets.Framework.TokenEngine.TokenTypes;

namespace Assets.Framework.TokenEngine
{
    internal sealed class TokenManager
    {
        #region Events
        internal event EventHandler<InternalTokenIdentifiedEventArgs> TokenIdentifiedEvent;
        internal event EventHandler<InternalTokenCancelledEventArgs> TokenCancelledEvent;

        internal event EventHandler<ApplicationTokenEventArgs> TokenPlacedOnScreen;
        internal event EventHandler<ApplicationTokenEventArgs> ScreenTokenUpdated;
        internal event EventHandler<ApplicationTokenEventArgs> TokenRemovedFromScreen;
        #endregion

        #region Private Fields

        private static readonly TokenManager _instance = new TokenManager();

        private Dictionary<string, InternalToken> tokens = new Dictionary<string, InternalToken>();
        private HashSet<int> tokenIds = new HashSet<int>();

        internal static TokenType CurrentTokenType;

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

        public void SetApplicationTokenType(TokenType t)
        {
            CurrentTokenType = t;
        }

        #endregion

        #region Private Methods

        private int GetFirstAvailableTokenId()
        {
            int defValue = 0;
            for (int i = 0; i < int.MaxValue; i++)
            {
                if (!tokenIds.Contains(i))
                    return i;
            }
            return defValue;
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
                    //Calculate TokenClass
                    token.ComputeTokenClass(ClassComputeReferenceSystem.MeanSqure, ClassComputeDimension.Pixels);
                    //Cluster Identification succesfull
                    //Set Token ID
                    token.SetTokenId(GetFirstAvailableTokenId());
                    tokenIds.Add(token.Id);

                    //Add Token to internal List
                    tokens.Add(token.HashId, token);

                    //Add Token To Global List
                    InputManager.AddToken(new Token(token));                 

                    //Notify CM cluster has been identified
                    LaunchTokenIdentified(new InternalTokenIdentifiedEventArgs(token.HashId,true));

                    //Fire event token identified
                    LaunchTokenPlacedOnScreen(new ApplicationTokenEventArgs(new Token(token)));

                }
                else
                {
                    //Cluser Identification failed, need to report back to CM
                    LaunchTokenIdentified(new InternalTokenIdentifiedEventArgs(cluster.Hash, false));

                }
            }
        }

        private void OnClustersMoved(object sender, ClusterUpdateEventArgs e)
        {
            foreach(Cluster cluster in e.GetClusters())
            {
                //Update internally the token
                InternalToken internalToken;
                if(tokens.TryGetValue(cluster.Hash,out internalToken))
                {
                    internalToken.Update(cluster);
                    tokens.Remove(cluster.Hash);
                    tokens.Add(internalToken.HashId, internalToken);

                    //Update Global Token
                    InputManager.GetToken(internalToken.Id).UpdateToken(internalToken);

                    //Here check deltas in order to fire or not Events
                    LaunchScreenTokenUpdated(new ApplicationTokenEventArgs(new Token(internalToken)));

                }
            }
        }

        private void OnClustersCancelled(object sender, ClusterUpdateEventArgs e)
        {
            foreach(Cluster cluster in e.GetClusters())
            {
                //Cancle cluster according to point
                //Here is very delicate because it must be considered also the possibility of not removing the token untill not all points have been removed
                InternalToken token;
                if (tokens.TryGetValue(cluster.CancelledClusterHash, out token))
                {
                    tokenIds.Remove(token.Id);
                    InputManager.RemoveToken(token.Id);

                    tokens.Remove(cluster.CancelledClusterHash);

                    //Launch CallBack to CM
                    LaunchTokenCancelled(new InternalTokenCancelledEventArgs(cluster.Hash));

                    //Lauch Application Token Cancelled
                    LaunchTokenRemovedFromScreen(new ApplicationTokenEventArgs(new Token(token)));
                }

            }
        }

        #endregion

        #region Event Launchers

        private void LaunchTokenIdentified(InternalTokenIdentifiedEventArgs e)
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

        private void LaunchTokenCancelled(InternalTokenCancelledEventArgs e)
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

        private void LaunchTokenPlacedOnScreen(ApplicationTokenEventArgs e)
        {
            EventHandler<ApplicationTokenEventArgs> handler;
            lock (TokenCallBackLock)
            {
                handler = TokenPlacedOnScreen;
            }

            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void LaunchScreenTokenUpdated(ApplicationTokenEventArgs e)
        {
            EventHandler<ApplicationTokenEventArgs> handler;
            lock (TokenCallBackLock)
            {
                handler = ScreenTokenUpdated;
            }

            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void LaunchTokenRemovedFromScreen(ApplicationTokenEventArgs e)
        {
            EventHandler<ApplicationTokenEventArgs> handler;
            lock (TokenCallBackLock)
            {
                handler = TokenRemovedFromScreen;
            }

            if (handler != null)
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

    public class ApplicationTokenEventArgs : EventArgs
    {
        private Token _token;
        public Token Token { get { return _token; } }

        internal ApplicationTokenEventArgs(Token token)
        {
            this._token = token;
        }
    }
}
