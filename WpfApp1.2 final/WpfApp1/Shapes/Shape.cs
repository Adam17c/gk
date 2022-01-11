using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    public abstract class Shape
    {
        public bool selected = false;
        protected int clickArea = 5;
        protected static int relationsCount = 0;

        public abstract void MoveShape(Point startMove, Point endMove);

        public abstract bool IsClicked(Point mousePos);

    }
}
