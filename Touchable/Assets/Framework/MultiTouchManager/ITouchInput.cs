using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Framework.MultiTouchManager
{
    internal interface ITouchInput
    {
        int Id { get; }

        TouchState State { get; }

        Vector2 Position { get; }




    }
}
