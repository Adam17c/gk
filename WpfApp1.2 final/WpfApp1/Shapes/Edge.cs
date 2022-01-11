using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    public enum LastMoved
    {
        start,
        end
    }
    
    public class Edge : Shape
    {
        public Point startPoint;
        public Point endPoint;
        public EdgeLimitations limit;
        public Edge pairedEdge;
        public Circle pairedCircle;
        public int relationNumber;
        public double? constLength;
        public LastMoved lastMoved;
        // LastMoved służy tylko do tego żeby przy przywracaniu stałej długości
        // wiedzieć który wierzchołek ma się dostosować
        // (bo tego ostatnio ruszanego nie przywarcamy, bo wtedy algorytm się 'zatka')

        public Edge(Point _startPoint, Point _endPoint)
        {
            startPoint = _startPoint;
            endPoint = _endPoint;
            limit = EdgeLimitations.none;
            pairedEdge = null;
            pairedCircle = null;
            relationNumber = 0;
            selected = false;
            constLength = null;
        }

        public double GetLength()
        {
            return Point.Subtract(startPoint, endPoint).Length;
        }

        public override bool IsClicked(Point mousePos)
        {
            return DistanceFromPointToEdge(mousePos, this) < clickArea;
        }

        public static double DistanceFromPointToEdge(Point point, Edge edge)
        {
            //http://mathworld.wolfram.com/Point-LineDistance2-Dimensional.html
            Point l1 = edge.startPoint;
            Point l2 = edge.endPoint;

            return Math.Abs((l2.X - l1.X) * (l1.Y - point.Y) - (l1.X - point.X) * (l2.Y - l1.Y)) /
                    Math.Sqrt(Math.Pow(l2.X - l1.X, 2) + Math.Pow(l2.Y - l1.Y, 2));
        }

        public override void MoveShape(Point startMove, Point endMove)
        {
            double dx = endMove.X - startMove.X;
            double dy = endMove.Y - startMove.Y;

            startPoint.X += dx;
            startPoint.Y += dy;
            endPoint.X += dx;
            endPoint.Y += dy;
        }

        public void SetRelation(EdgeLimitations relation, Edge newPair)
        {
            if (pairedEdge != null)
            {
                pairedEdge.relationNumber = 0;
                pairedEdge.limit = EdgeLimitations.none;
                pairedEdge.pairedEdge = null;
            }
            if (pairedCircle != null)
            {
                pairedCircle.pairedEdge = null;
                pairedCircle.relationNumber = 0;
            }

            if (relation != EdgeLimitations.none) relationNumber = ++relationsCount;
            limit = relation;
            pairedEdge = newPair;
            pairedCircle = null;
            if(relation == EdgeLimitations.constLength) constLength = GetLength();

            if (newPair != null)
            {
                newPair.pairedEdge = this;
                newPair.relationNumber = relationsCount;
                newPair.limit = limit;
            }
        }

        public void SetRelation(Circle circle)
        {
            if (pairedEdge != null)
            {
                pairedEdge.relationNumber = 0;
                pairedEdge.limit = EdgeLimitations.none;
                pairedEdge.pairedEdge = null;
            }
            if (pairedCircle != null)
            {
                pairedCircle.pairedEdge = null;
                pairedCircle.relationNumber = 0;
            }
            relationNumber = ++relationsCount;
            limit = EdgeLimitations.contactedCircle;
            pairedEdge = null;
            pairedCircle = circle;
            if (circle != null)
            {
                if (circle.pairedEdge != null) circle.pairedEdge.SetRelation(EdgeLimitations.none, null);
                circle.pairedEdge = this;
                circle.relationNumber = relationNumber;
            }
        }
    }
}
