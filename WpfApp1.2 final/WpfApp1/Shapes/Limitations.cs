using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public enum EdgeLimitations
    {
        none,
        constLength,
        equalLength,
        contactedCircle,
        perpendicularEdge
    }

    public enum CircleLimitations
    {
        none,
        constRadius,
        constCenter
    }
}
