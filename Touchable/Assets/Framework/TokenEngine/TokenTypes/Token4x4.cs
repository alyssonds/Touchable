﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Framework.Utils;
using Assets.Framework.Utils.Attributes;

namespace Assets.Framework.TokenEngine.TokenTypes
{
    internal sealed class Token4x4 : TokenType
    {
        internal override float SetOriginToAxisDistance()
        {
            return ScreenUtils.CmToPixels(TokenAttributes.TOKEN_4X4_ORIGIN_TO_AXIS_MARKERS_DST);
        }

        internal override float SetOriginToCenterDistance()
        {
            return ScreenUtils.CmToPixels(TokenAttributes.TOKEN_4X4_ORIGIN_TO_CENTER_DST);
        }
    }
}
