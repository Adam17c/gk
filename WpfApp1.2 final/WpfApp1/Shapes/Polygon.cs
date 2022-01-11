using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace WpfApp1
{
    class Polygon : Shape
    {
        public List<Edge> edges;
        private static Random Random = new Random(0);

        public Polygon()
        {
            edges = new List<Edge>();        
        }

        public Polygon(List<Edge> _edges)
        {
            edges = _edges;
        }

        public override bool IsClicked(Point mousePos)
        {
            foreach (Edge edge in edges)
            {
                if (edge.IsClicked(mousePos)) return true;
            }
            return false;
        }

        public override void MoveShape(Point startMove, Point endMove)
        {   
            foreach(Edge edge in edges)
            {
                edge.MoveShape(startMove, endMove);
                if(edge.limit == EdgeLimitations.contactedCircle) UpdateEdge(edge);
            }
        }

        public bool addVertex(Point mousePos)
        {
            foreach(Edge edge in edges)
                if(edge.IsClicked(mousePos))
                {
                    Point startPoint = edge.startPoint, endPoint = edge.endPoint;
                    Point middlePoint;
                    middlePoint.X = Math.Abs(startPoint.X - endPoint.X) / 2 + Math.Min(startPoint.X,endPoint.X);
                    middlePoint.Y = Math.Abs(startPoint.Y - endPoint.Y) / 2 + Math.Min(startPoint.Y,endPoint.Y);

                    DeleteEdge(edge);
                    edges.Add(new Edge(middlePoint, endPoint));
                    edges.Add(new Edge(startPoint, middlePoint));

                    return true;
                }
            return false;
        }

        public bool removeVertex(Point mousePos)
        {
            Edge firstEdge = null, secondEdge = null;
            foreach (Edge edge in edges)
            {
                Point startPoint = edge.startPoint, endPoint = edge.endPoint;

                if (Math.Abs(endPoint.X - mousePos.X) <= clickArea && Math.Abs(endPoint.Y - mousePos.Y) <= clickArea)
                    firstEdge = edge;
                else if (Math.Abs(startPoint.X - mousePos.X) <= clickArea && Math.Abs(startPoint.Y - mousePos.Y) <= clickArea)
                    secondEdge = edge;
            }

            if(firstEdge != null & secondEdge != null)
            {
                if (edges.Count() < 4)
                {
                    MessageBox.Show("Wielokąt musi posiadać więcej niż dwa wierzchołki", "Uwaga!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                edges.Add(new Edge(firstEdge.startPoint, secondEdge.endPoint));
                DeleteEdge(firstEdge);
                DeleteEdge(secondEdge);
                return true;
            }

            return false;
        }

        public void MoveVertex(Point startMove, Point endMove)
        {
            Edge edge1 = null, edge2 = null;
            
            double dx = endMove.X - startMove.X;
            double dy = endMove.Y - startMove.Y;

            foreach (Edge edge in edges)
            {
                if (Math.Abs(edge.startPoint.X - startMove.X) <= clickArea && Math.Abs(edge.startPoint.Y - startMove.Y) <= clickArea)
                    edge1 = edge;
                if (Math.Abs(edge.endPoint.X - startMove.X) <= clickArea && Math.Abs(edge.endPoint.Y - startMove.Y) <= clickArea)
                    edge2 = edge;

                // sprawdzanie czy wierzcholki nie wchodzą na siebie
                if (Math.Abs(edge.startPoint.X - endMove.X) <= clickArea && Math.Abs(edge.startPoint.Y - endMove.Y) <= clickArea)
                    return;
                if (Math.Abs(edge.endPoint.X - endMove.X) <= clickArea && Math.Abs(edge.endPoint.Y - endMove.Y) <= clickArea)
                    return;
            }

            if(edge1 != null && edge2 != null)
            {

                edge1.startPoint.X += dx;
                edge1.startPoint.Y += dy;
                edge1.lastMoved = LastMoved.start;
                edge2.endPoint.X += dx;
                edge2.endPoint.Y += dy;
                edge2.lastMoved = LastMoved.end;

                UpdateEdge(edge1);
                UpdateEdge(edge2);
            }
        }

        public void MoveEdge(Point startMove, Point endMove)
        {
            Edge MovedEdge = null;

            foreach(Edge edge in edges)
            {
                if (edge.IsClicked(startMove))
                {
                    MovedEdge = edge;
                    break;
                }
            }

            if(MovedEdge != null)
            {
                int dx = (int)endMove.X - (int)startMove.X;
                int dy = (int)endMove.Y - (int)startMove.Y;

                Edge first = GetEdgeFirstNeighbour(MovedEdge), second = GetEdgeSecondNeighbour(MovedEdge);
                first.endPoint.X += dx;
                first.endPoint.Y += dy;
                first.lastMoved = LastMoved.end;
                second.startPoint.X += dx;
                second.startPoint.Y += dy;
                second.lastMoved = LastMoved.start;

                MovedEdge.startPoint.X += dx;
                MovedEdge.startPoint.Y += dy;
                MovedEdge.endPoint.X += dx;
                MovedEdge.endPoint.Y += dy;

                UpdateEdge(MovedEdge);
                UpdateEdge(first);
                UpdateEdge(second);
            }
        }

        public void DeleteEdge(Edge edge)
        {
            if (edge.pairedEdge != null)
            {
                edge.SetRelation(EdgeLimitations.none, null);
            }
            if(edge.pairedCircle != null)
            {
                edge.SetRelation(null);
            }
            edges.Remove(edge);
        }

        public Edge EdgeClicked(Point mousePos)
        {
            foreach(Edge edge in edges)
                if(edge.IsClicked(mousePos))
                    return edge;
            return null;
        }

        public Edge GetEdgeFirstNeighbour(Edge edge)
        {
            foreach(Edge neighbour in edges)
            {
                if (edge.startPoint == neighbour.endPoint && neighbour != edge)
                    return neighbour;
            }
            return null;
        }

        public Edge GetEdgeSecondNeighbour(Edge edge)
        {
            foreach (Edge neighbour in edges)
            {
                if (edge.endPoint == neighbour.startPoint && neighbour != edge)
                    return neighbour;
            }
            return null;
        }
        //poniżej ograniczenia


        void ChangeEdgeLength(Edge edge, double newLength)
        {
            Edge first = GetEdgeFirstNeighbour(edge);
            Edge second = GetEdgeSecondNeighbour(edge);

            double ratio = newLength / edge.GetLength();
            Point staticPoint;
            Point movedPoint;

            int which = first.limit != EdgeLimitations.none && second.limit != EdgeLimitations.none ? Random.Next(100) : -1;
            
            if (second.limit == EdgeLimitations.none || (which > 0 && which < 50))
            {
                staticPoint = edge.startPoint;
                movedPoint = edge.endPoint;

                double dx = movedPoint.X - staticPoint.X;
                double dy = movedPoint.Y - staticPoint.Y;
                double newDx = ratio * dx;
                double newDy = ratio * dy;

                movedPoint = new Point(staticPoint.X + newDx, staticPoint.Y + newDy);

                second.startPoint = movedPoint;
                second.lastMoved = LastMoved.start;
                edge.endPoint = movedPoint;
                edge.lastMoved = LastMoved.end;

                UpdateEdge(second);
            }
            else if (first.limit == EdgeLimitations.none || which > 0)
            {
                movedPoint = edge.startPoint;
                staticPoint = edge.endPoint;

                double dx = movedPoint.X - staticPoint.X;
                double dy = movedPoint.Y - staticPoint.Y;
                double newDx = ratio * dx;
                double newDy = ratio * dy;

                movedPoint = new Point(staticPoint.X + newDx, staticPoint.Y + newDy);

                first.endPoint = movedPoint;
                first.lastMoved = LastMoved.end;
                edge.startPoint = movedPoint;
                edge.lastMoved = LastMoved.start;

                UpdateEdge(first);
            }
        }

        void KeepConstLength(Edge edge)
        {
            Edge first = GetEdgeFirstNeighbour(edge);
            Edge second = GetEdgeSecondNeighbour(edge);

            double ratio = (double)edge.constLength / edge.GetLength();
            Point staticPoint;
            Point movedPoint;

            if(edge.lastMoved == LastMoved.start)
            {
                staticPoint = edge.startPoint;
                movedPoint = edge.endPoint;

                double dx = movedPoint.X - staticPoint.X;
                double dy = movedPoint.Y - staticPoint.Y;
                double newDx = ratio * dx;
                double newDy = ratio * dy;

                movedPoint = new Point(staticPoint.X + newDx, staticPoint.Y + newDy);

                second.startPoint = movedPoint;
                second.lastMoved = LastMoved.start;
                edge.endPoint = movedPoint;
                edge.lastMoved = LastMoved.end;

                UpdateEdge(second);
            }
            else
            {
                movedPoint = edge.startPoint;
                staticPoint = edge.endPoint;

                double dx = movedPoint.X - staticPoint.X;
                double dy = movedPoint.Y - staticPoint.Y;
                double newDx = ratio * dx;
                double newDy = ratio * dy;

                movedPoint = new Point(staticPoint.X + newDx, staticPoint.Y + newDy);

                first.endPoint = movedPoint;
                first.lastMoved = LastMoved.end;
                edge.startPoint = movedPoint;
                edge.lastMoved = LastMoved.start;

                UpdateEdge(first);
            }
        }

        void SetEqualLength(Edge edge, Edge extendedEdge)
        {
            ChangeEdgeLength(extendedEdge, edge.GetLength());
        }

        void SetPerpendicularity(Edge edge, Edge perpendicularEdge)
        {
            double dx = edge.endPoint.X - edge.startPoint.X, dy = edge.endPoint.Y - edge.startPoint.Y;

            Edge first = GetEdgeFirstNeighbour(perpendicularEdge);
            Edge second = GetEdgeSecondNeighbour(perpendicularEdge);

            int which = first.limit != EdgeLimitations.none && second.limit != EdgeLimitations.none ? Random.Next(100) : -1;

            // amieniamy punkt końcowy
            if (second.limit == EdgeLimitations.none || (which > 0 && which < 50))
            {
                Vector perpendicular = new Vector(-dy, dx), startPoint = new Vector(perpendicularEdge.startPoint.X, perpendicularEdge.startPoint.Y), newEndPoint;
                Vector perpendicularVersor = perpendicular / Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));

                int sign = Math.Sign(Edge.DistanceFromPointToEdge(perpendicularEdge.endPoint, edge) - Edge.DistanceFromPointToEdge(perpendicularEdge.startPoint, edge));

                newEndPoint = startPoint + sign * perpendicularVersor * perpendicularEdge.GetLength();

                second.startPoint.X = newEndPoint.X;
                second.startPoint.Y = newEndPoint.Y;
                second.lastMoved = LastMoved.start;

                perpendicularEdge.endPoint.X = newEndPoint.X;
                perpendicularEdge.endPoint.Y = newEndPoint.Y;

                UpdateEdge(second);
            }
            // zmieniamy punkt początkowy
            else if (first.limit == EdgeLimitations.none || which > 0)
            {
                Vector perpendicular = new Vector(-dy, dx), endPoint = new Vector(perpendicularEdge.endPoint.X, perpendicularEdge.endPoint.Y), newStartPoint;
                Vector perpendicularVersor = perpendicular / Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));

                int sign = Math.Sign(Edge.DistanceFromPointToEdge(perpendicularEdge.startPoint, edge) - Edge.DistanceFromPointToEdge(perpendicularEdge.endPoint, edge));

                newStartPoint = endPoint + sign * perpendicularVersor * perpendicularEdge.GetLength();

                first.endPoint.X = newStartPoint.X;
                first.endPoint.Y = newStartPoint.Y;
                first.lastMoved = LastMoved.end;

                perpendicularEdge.startPoint.X = newStartPoint.X;
                perpendicularEdge.startPoint.Y = newStartPoint.Y;

                UpdateEdge(first);
            }
        }

        public void UpdateEdge(Edge edge)
        {
            switch(edge.limit)
            {
                case EdgeLimitations.constLength:
                    KeepConstLength(edge);
                    break;
                case EdgeLimitations.equalLength:
                    SetEqualLength(edge, edge.pairedEdge);
                    break;
                case EdgeLimitations.contactedCircle:
                    edge.pairedCircle.Update();
                    break;
                case EdgeLimitations.perpendicularEdge:
                    SetPerpendicularity(edge, edge.pairedEdge);
                    break;
            }
        }
    }
}
