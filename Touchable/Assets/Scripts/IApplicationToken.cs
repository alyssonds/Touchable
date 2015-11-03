using Assets.Framework.TokenEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    interface IApplicationToken
    {
        int TokenClass { get; set; }

        void OnTokenPlacedOnScreen(object sender, ApplicationTokenEventArgs e);

        void OnTokenRemovedFromScreen(object sender, ApplicationTokenEventArgs e);

        void OnTokenUpdated(object sender, ApplicationTokenEventArgs e);
    }
}
