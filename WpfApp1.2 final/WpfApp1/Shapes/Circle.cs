using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    public class Circle : Shape
    {
        public Point center;
        public double radius;
        public CircleLimitations limit;
        public int relationNumber;
        public Edge pairedEdge;

        public Circle(Point _center)
        {
            center = _center;
            radius = 0;
            limit = CircleLimitations.none;
            relationNumber = 0;
            pairedEdge = null;
        }

        public Circle(Point _center, double _radius)
        {
            center = _center;
            radius = _radius;
            limit = CircleLimitations.none;
            relationNumber = 0;
            pairedEdge = null;
        }

        public override void MoveShape(Point startMove, Point endMove)
        {
            if (limit == CircleLimitations.constCenter)
            {
                MessageBox.Show("Ustawiono stały środek okręgu", "Nie można poruszać okręgiem", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            double dx = endMove.X - startMove.X;
            double dy = endMove.Y - startMove.Y;
            center.X += dx;
            center.Y += dy;

            Update();
        }

        public override bool IsClicked(Point mousePos)
        {
            double distance = Math.Sqrt(Math.Pow(mousePos.X - center.X, 2) + Math.Pow(mousePos.Y - center.Y, 2));
            return distance <= (radius + clickArea) && distance >= (radius - clickArea);
        }

        public void ChangeRadius(Point mousePos)
        {
            if (limit == CircleLimitations.constRadius)
            {
                MessageBox.Show("Ustawiono stały promień", "Nie można zmienić promienia", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            radius = (int)Point.Subtract(mousePos, center).Length;

            Update();
        }

        public void Update()
        {
            if (pairedEdge == null) return;
            if (limit == CircleLimitations.constCenter)
            {
                double distance = Edge.DistanceFromPointToEdge(center, pairedEdge);
                radius += distance - radius;
            }
            else
            {
                double dx = pairedEdge.endPoint.X - pairedEdge.startPoint.X, dy = pairedEdge.endPoint.Y - pairedEdge.startPoint.Y;
                double changedDistance = Edge.DistanceFromPointToEdge(center, pairedEdge) - radius;
                Vector oldCenter = new Vector(center.X, center.Y), newCenter, perpendicular = new Vector(-dy, dx);
                Vector perpendicularVersor = perpendicular / Math.Sqrt(Math.Pow(dx,2) + Math.Pow(dy,2));

                int sign = Math.Sign((center.X - pairedEdge.startPoint.X) * (pairedEdge.endPoint.Y - pairedEdge.startPoint.Y) -
                    (center.Y - pairedEdge.startPoint.Y) * (pairedEdge.endPoint.X - pairedEdge.startPoint.X));
                newCenter = oldCenter + sign * perpendicularVersor * changedDistance;
                center.X = newCenter.X;
                center.Y = newCenter.Y;
            }

        }
    }
}
