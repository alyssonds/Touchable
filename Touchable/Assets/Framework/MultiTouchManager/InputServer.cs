using System;
using UnityEngine;

namespace Assets.Framework.MultiTouchManager
{ 
    internal sealed class InputServer
    {
        #region Events

        public event EventHandler<InputUpdateEventArgs> InputUpdated;

        #endregion

        #region Private Properties

        private static InputServer _Instance;
        private bool UpdatedInputs = false;

        readonly object UpdateInputsLock = new object();

        #endregion

        #region Public Properties
    
        /// <summary>
        /// Instance of the InputServer 
        /// </summary>
        public static InputServer Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new InputServer();
                return _Instance;
            }
        }

        #endregion

        private InputServer() { }

        #region Public Methods

        /// <summary>
        /// Update method to be called every frame in order to dispatch Unity's Input.touches()
        /// </summary>
        public void Update()
        {
            InternalTouches.ResetBuffers();

            foreach (Touch t in Input.touches)
            {
                InternalTouches.UpdateTouchInput(t);

            }

            if (InternalTouches.Updated())
            {
                // There has been at least one updated from previous frame
                OnInputsUpdated(new InputUpdateEventArgs("Successfull Update"));
            }
        }

        #endregion

        #region Event Handlers

        private void OnInputsUpdated(InputUpdateEventArgs e)
        {
            EventHandler<InputUpdateEventArgs> handler;

            lock (UpdateInputsLock)
            {
                handler = InputUpdated;
            }
            if(handler != null)
            {
                handler(this,e);
            }
        }

        #endregion 
    }

    public class InputUpdateEventArgs : EventArgs
    {
        public String EventMsg { get; private set; }

        public InputUpdateEventArgs(String msg)
        {
            EventMsg = msg;
        }
    }
}