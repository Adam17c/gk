using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private void DrawPolygon(Polygon polygon, bool bresenham)
        {
            Ellipse point;
            Brush lineColor = polygon.selected ? Brushes.Blue : Brushes.Black;

            if(bresenham)
                foreach (Edge edge in polygon.edges)
                {
                    point = new Ellipse();
                    point.Fill = Brushes.Red;
                    point.Stroke = null;
                    point.Width = vertexWidth;
                    point.Height = vertexHeight;

                    Canvas.SetLeft(point, edge.endPoint.X - point.Width / 2);
                    Canvas.SetTop(point, edge.endPoint.Y - point.Height / 2);
                    canvas.Children.Add(point);

                    BresenhamLine(edge.startPoint, edge.endPoint, lineColor);
                }
            else
                foreach (Edge edge in polygon.edges)
                {
                    point = new Ellipse();
                    point.Fill = Brushes.Red;
                    point.Stroke = null;
                    point.Width = vertexWidth;
                    point.Height = vertexHeight;

                    Canvas.SetLeft(point, edge.endPoint.X - point.Width / 2);
                    Canvas.SetTop(point, edge.endPoint.Y - point.Height / 2);
                    canvas.Children.Add(point);

                    Line line = new Line();
                    line.Stroke = edge.selected ? Brushes.Blue : lineColor;
                    line.X1 = edge.startPoint.X;
                    line.Y1 = edge.startPoint.Y;
                    line.X2 = edge.endPoint.X;
                    line.Y2 = edge.endPoint.Y;
                    canvas.Children.Add(line);

                    if (edge.limit != EdgeLimitations.none && edge.relationNumber > 0)
                    {
                        string relSymbol = null;

                        switch (edge.limit)
                        {
                            case EdgeLimitations.constLength:
                                relSymbol = "| ";
                                break;
                            case EdgeLimitations.equalLength:
                                relSymbol = "= ";
                                break;
                            case EdgeLimitations.contactedCircle:
                                relSymbol = "|o ";
                                break;
                            case EdgeLimitations.perpendicularEdge:
                                relSymbol = "⊥ ";
                                break;
                        }

                        TextBlock relText = new TextBlock();
                        relText.Text = relSymbol + edge.relationNumber.ToString(); 
                        relText.Foreground = Brushes.Black;
                        Canvas.SetLeft(relText, (edge.startPoint.X < edge.endPoint.X ? edge.startPoint.X : edge.endPoint.X) + Math.Abs(edge.startPoint.X - edge.endPoint.X) / 2);
                        Canvas.SetTop(relText, (edge.startPoint.Y < edge.endPoint.Y ? edge.startPoint.Y : edge.endPoint.Y) + Math.Abs(edge.startPoint.Y - edge.endPoint.Y) / 2);
                        canvas.Children.Add(relText);
                    }
                }
        }

        private void BresenhamLine(Point startPoint, Point endPoint, Brush color)
        {
            int x1 = (int)startPoint.X, y1 = (int)startPoint.Y, x2 = (int)endPoint.X, y2 = (int)endPoint.Y;
            
            int d, dx, dy, ai, bi, xi, yi;
            int x = x1, y = y1;
            if (x1 < x2)
            {
                xi = 1;
                dx = x2 - x1;
            }
            else
            {
                xi = -1;
                dx = x1 - x2;
            }
            if (y1 < y2)
            {
                yi = 1;
                dy = y2 - y1;
            }
            else
            {
                yi = -1;
                dy = y1 - y2;
            }
            PutPixel(x, y, color);
            if (dx > dy)
            {
                ai = (dy - dx) * 2;
                bi = dy * 2;
                d = bi - dx;
                while (x != x2)
                {
                    if (d >= 0)
                    {
                        x += xi;
                        y += yi;
                        d += ai;
                    }
                    else
                    {
                        d += bi;
                        x += xi;
                    }
                    PutPixel(x, y, color);
                }
            }
            else
            {
                ai = (dx - dy) * 2;
                bi = dx * 2;
                d = bi - dy;
                while (y != y2)
                {
                    if (d >= 0)
                    {
                        x += xi;
                        y += yi;
                        d += ai;
                    }
                    else
                    {
                        d += bi;
                        y += yi;
                    }
                    PutPixel(x, y, color);
                }
            }
        }
        // https://pl.m.wikipedia.org/wiki/Algorytm_Bresenhama

        private void PutPixel(int x, int y, Brush color)
        {
            Rectangle pixel = new Rectangle();
            pixel.Fill = color;
            pixel.Height = 1;
            pixel.Width = 1;
            Canvas.SetLeft(pixel, x);
            Canvas.SetTop(pixel, y);
            canvas.Children.Add(pixel);
        }

        private void DrawCircle(Circle circle)
        {
            Brush lineColor = circle.selected ? Brushes.Blue : Brushes.Black;
            
            Ellipse c = new Ellipse();
            c.Fill = null;
            c.Stroke = lineColor;
            c.Width = 2 * circle.radius;
            c.Height = 2 * circle.radius;

            Canvas.SetLeft(c, circle.center.X - circle.radius);
            Canvas.SetTop(c, circle.center.Y - circle.radius);
            canvas.Children.Add(c);

            c = new Ellipse();
            c.Fill = Brushes.Red;
            c.Stroke = null;
            c.Width = vertexWidth;
            c.Height = vertexHeight;

            Canvas.SetLeft(c, circle.center.X - c.Width / 2);
            Canvas.SetTop(c, circle.center.Y - c.Height / 2);

            if (circle.relationNumber > 0 || circle.limit != CircleLimitations.none)
            {
                string relSymbol = "";
                
                switch (circle.limit)
                {
                    case CircleLimitations.constCenter:
                        relSymbol = "· ";
                        break;
                    case CircleLimitations.constRadius:
                        relSymbol = "O ";
                        break;
                }

                if (circle.pairedEdge != null)
                    relSymbol += "|o ";
                if(circle.relationNumber > 0)
                    relSymbol += circle.relationNumber.ToString();

                TextBlock relSign = new TextBlock();
                relSign.Text = relSymbol;
                relSign.Foreground = Brushes.Black;
                Canvas.SetLeft(relSign, circle.center.X + circle.radius);
                Canvas.SetTop(relSign, circle.center.Y);
                canvas.Children.Add(relSign);
            }

            canvas.Children.Add(c);
        }

        private void DrawNewPolygon()
        {
            Ellipse point;

            if (newPolygon == null) return;
            if (newPolygon.edges.Count() == 0 || newPolygon.edges.Count() == 1)
            {
                point = new Ellipse();
                point.Fill = Brushes.Red;
                point.Stroke = null;
                point.Width = vertexWidth;
                point.Height = vertexHeight;

                Canvas.SetLeft(point, firstPoint.X - point.Width / 2);
                Canvas.SetTop(point, firstPoint.Y - point.Height / 2);

                canvas.Children.Add(point);

            }
            DrawPolygon(newPolygon, true);
        }

        private void DrawNewCircle()
        {
            Ellipse centerPoint;
            centerPoint = new Ellipse();
            centerPoint.Fill = Brushes.Red;
            centerPoint.Stroke = null;
            centerPoint.Width = vertexWidth;
            centerPoint.Height = vertexHeight;

            Canvas.SetLeft(centerPoint, firstPoint.X - centerPoint.Width / 2);
            Canvas.SetTop(centerPoint, firstPoint.Y - centerPoint.Height / 2);

            canvas.Children.Add(centerPoint);
        }

        private void DrawShapes()
        {
            canvas.Children.Clear();

            foreach (Polygon polygon in polygons)
                DrawPolygon(polygon, false);

            foreach (Circle circle in circles)
                DrawCircle(circle);
        }
    }
}
