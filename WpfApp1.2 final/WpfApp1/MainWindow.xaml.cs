using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        int vertexWidth = 8;
        int vertexHeight = 8;
        
        List<Polygon> polygons;
        List<Circle> circles;
        Polygon newPolygon;
        Shape selectedShape;
        Edge selectedEdge;
        Point firstPoint;
        Point prevPoint;
        Circle newCircle;
        bool puttingPolygon, puttingCircle, deletingPolygon, deletingCircle, allowMovingPolygon, allowMovingCircle, changingCircleRadius;
        bool addingVertex, removingVertex, movingVertex, movingEdge, pickingEdgeToPair, pickingCircleToPair;

        public MainWindow()
        {
            polygons = new List<Polygon>();
            circles = new List<Circle>();

            puttingPolygon = puttingCircle = deletingPolygon = deletingCircle = allowMovingPolygon = allowMovingCircle = changingCircleRadius = false;
            addingVertex = removingVertex = movingVertex = movingEdge = pickingEdgeToPair = pickingCircleToPair = false;

            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            Circle circle1 = new Circle(new Point(150, 150), 100);
            circle1.limit = CircleLimitations.constRadius;
            Circle circle2 = new Circle(new Point(816, 500), 80);

            Edge e1 = new Edge(new Point(240, 220), new Point(600, 120));
            e1.SetRelation(circle1);
            Edge e2 = new Edge(new Point(600, 120), new Point(1000, 100)), e3 = new Edge(new Point(1000, 100), new Point(800, 300));
            Edge e4 = new Edge(new Point(800, 300), new Point(500, 500));
            e4.SetRelation(EdgeLimitations.constLength, null);
            Edge e5 = new Edge(new Point(500, 500), new Point(240, 220));
            e5.SetRelation(EdgeLimitations.equalLength, e2);

            List<Edge> edgesList1 = new List<Edge>();
            edgesList1.Add(e1);
            edgesList1.Add(e2);
            edgesList1.Add(e3);
            edgesList1.Add(e4);
            edgesList1.Add(e5);
            Polygon polygon1 = new Polygon(edgesList1);

            Edge e6 = new Edge(new Point(750, 450), new Point(700, 550));
            e6.SetRelation(circle2);
            Edge e7 = new Edge(new Point(700, 550), new Point(600, 550));
            e7.SetRelation(EdgeLimitations.constLength, null);
            Edge e8 = new Edge(new Point(600, 550), new Point(750, 450));

            List<Edge> edgeList2 = new List<Edge>();
            edgeList2.Add(e6);
            edgeList2.Add(e7);
            edgeList2.Add(e8);
            Polygon polygon2 = new Polygon(edgeList2);
            
            polygons.Add(polygon1);
            polygons.Add(polygon2);
            circles.Add(circle1);
            circles.Add(circle2);
            DrawShapes();
        }

        private void BuildPolygon(Point mousePos)
        {
            if (newPolygon == null)
            {
                newPolygon = new Polygon();
                firstPoint = mousePos;
            }
            else if (newPolygon.edges.Count() == 0)
            {
                newPolygon.edges.Add(new Edge(firstPoint, mousePos));
                prevPoint = mousePos;
            }
            else if (newPolygon.edges.Count() == 1)
            {
                newPolygon.edges.Add(new Edge(prevPoint, mousePos));
                newPolygon.edges.Add(new Edge(mousePos, firstPoint));
                prevPoint = mousePos;
            }
            else
            {
                newPolygon.edges.RemoveAt(newPolygon.edges.Count - 1);
                newPolygon.edges.Add(new Edge(prevPoint, mousePos));
                newPolygon.edges.Add(new Edge(mousePos, firstPoint));
                prevPoint = mousePos;
            }
        }

        private void BuildCirlce(Point mousePos)
        {
            if (newCircle == null)
            {
                newCircle = new Circle(mousePos);
                firstPoint = mousePos;
                DrawShapes();
                DrawNewCircle();
            }
            else
            {
                newCircle.radius = (int)Point.Subtract(mousePos, newCircle.center).Length;
                circles.Add(newCircle);
                newCircle = null;
                puttingCircle = false;
                addCircle.Content = "Dodaj okrąg";
                DrawShapes();
            }
        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point mousePos = Mouse.GetPosition(canvas);

            if (puttingPolygon)
            {
                BuildPolygon(mousePos);
                DrawShapes();
                DrawNewPolygon();
            }
            
            else if(puttingCircle)
            {
                BuildCirlce(mousePos);
            }

            else if(deletingPolygon)
            {
                foreach(Polygon polygon in polygons)
                {
                    if (polygon.IsClicked(mousePos))
                    {
                        foreach (Edge edge in polygon.edges) if (edge.pairedCircle != null) edge.SetRelation(null);
                        polygons.Remove(polygon);
                        DrawShapes();
                        break;
                    }
                }
            }

            else if(deletingCircle)
            {
                foreach(Circle circle in circles)
                    if(circle.IsClicked(mousePos))
                    {
                        circles.Remove(circle);
                        if (circle.pairedEdge != null) circle.pairedEdge.SetRelation(EdgeLimitations.none, null);
                        DrawShapes();
                        break;
                    }
            }

            else if (allowMovingPolygon)
            {
                foreach (Polygon polygon in polygons)
                {
                    if (polygon.IsClicked(mousePos))
                    {
                        selectedShape = polygon;
                        polygon.selected = true;
                        prevPoint = mousePos;
                        break;
                    }
                }
            }

            else if (allowMovingCircle)
            {
                foreach (Circle circle in circles)
                {
                    if (circle.IsClicked(mousePos))
                    {
                        selectedShape = circle;
                        circle.selected = true;
                        prevPoint = mousePos;
                        break;
                    }
                }
            }

            else if(changingCircleRadius)
            {
                if (selectedShape == null)
                {
                    foreach (Circle circle in circles)
                    {
                        if (circle.IsClicked(mousePos))
                        {
                            selectedShape = circle;
                            circle.selected = true;
                            prevPoint = mousePos;
                            DrawShapes();
                            break;
                        }
                    }
                }
                else
                {
                    selectedShape.selected = false;
                    selectedShape = null;
                    DrawShapes();
                }
            }

            else if(addingVertex)
            {
                foreach (Polygon polygon in polygons)
                {
                    if (polygon.IsClicked(mousePos))
                    {
                        if (polygon.addVertex(mousePos))
                        {
                            DrawShapes();
                            break;
                        }
                    }
                }
            }

            else if(removingVertex)
            {
                foreach (Polygon polygon in polygons)
                {
                    if (polygon.IsClicked(mousePos))
                    {
                        if (polygon.removeVertex(mousePos))
                        {
                            DrawShapes();
                            break;
                        }
                    }
                }
            }

            else if(movingVertex || movingEdge)
            {
                foreach (Polygon polygon in polygons)
                {
                    if (polygon.IsClicked(mousePos))
                    {
                        selectedShape = polygon;
                        prevPoint = Mouse.GetPosition(canvas);
                        break;
                    }
                }
            }

            else if (pickingEdgeToPair)
            {
                foreach (Polygon polygon in polygons)
                {
                    if (polygon.IsClicked(mousePos))
                    {
                        Edge clickedEdge;
                        if ((clickedEdge = polygon.EdgeClicked(mousePos)) != null)
                        {
                            selectedEdge.SetRelation(selectedEdge.limit, clickedEdge);
                            polygon.UpdateEdge(selectedEdge);

                            selectedEdge.selected = false;
                            selectedEdge = null;
                            pickingEdgeToPair = false;
                            DrawShapes();
                        }
                    }
                }
            }

            else if(pickingCircleToPair)
            {
                foreach(Circle circle in circles)
                {
                    if(circle.IsClicked(mousePos))
                    {
                        selectedEdge.SetRelation(circle);
                        circle.Update();

                        selectedEdge.selected = false;
                        selectedEdge = null;
                        pickingCircleToPair = false;
                        DrawShapes();
                    }
                }
            }

            else if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                DoubleClick(mousePos);
            }
        }

        private void DoubleClick(Point mousePos)
        {
            foreach (Polygon polygon in polygons)
            {
                if (polygon.IsClicked(mousePos))
                {
                    Edge clickedEdge;
                    if ((clickedEdge = polygon.EdgeClicked(mousePos)) != null)
                    {
                        DisplayEdgeDialog(clickedEdge);
                    }
                    break;
                }
            }

            foreach (Circle circle in circles)
            {
                if (circle.IsClicked(mousePos))
                {
                    DisplayCircleDialog(circle);
                    break;
                }
            }
        }

        private void DisplayEdgeDialog(Edge clickedEdge)
        {
            EdgeDialog edgeDialog = new EdgeDialog(clickedEdge);
            edgeDialog.Owner = this;
            clickedEdge.selected = true;
            DrawShapes();
            if ((bool)edgeDialog.ShowDialog())
            {
                clickedEdge.selected = false;
                DrawShapes();
                return;
            }

            switch (clickedEdge.limit)
            {
                case EdgeLimitations.constLength:
                    clickedEdge.SetRelation(EdgeLimitations.constLength, null);
                    clickedEdge.selected = false;
                    break;
                case EdgeLimitations.equalLength:
                    selectedEdge = clickedEdge;
                    pickingEdgeToPair = true;
                    break;
                case EdgeLimitations.contactedCircle:
                    selectedEdge = clickedEdge;
                    pickingCircleToPair = true;
                    break;
                case EdgeLimitations.perpendicularEdge:
                    selectedEdge = clickedEdge;
                    pickingEdgeToPair = true;
                    break;

                case EdgeLimitations.none:
                    clickedEdge.SetRelation(EdgeLimitations.none, null);
                    clickedEdge.selected = false;
                    break;
            }
            
            DrawShapes();
        }

        private void DisplayCircleDialog(Circle circle)
        {
            CircleDialog circleDialog = new CircleDialog(circle);
            circleDialog.Owner = this;
            circleDialog.ShowDialog();
            DrawShapes();
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if ((allowMovingPolygon || allowMovingCircle) && selectedShape != null && Mouse.LeftButton == MouseButtonState.Pressed)
            {
                selectedShape.MoveShape(prevPoint, Mouse.GetPosition(canvas));
                prevPoint = Mouse.GetPosition(canvas);
                DrawShapes();
            }
            else if (Mouse.LeftButton == MouseButtonState.Released)
            {
                if (selectedShape != null) selectedShape.selected = false;
                selectedShape = null;
            }

            else if (changingCircleRadius && selectedShape != null && Mouse.LeftButton == MouseButtonState.Pressed)
            {
                ((Circle)selectedShape).ChangeRadius(Mouse.GetPosition(canvas));
                DrawShapes();
            }

            else if (movingVertex && selectedShape != null)
            {
                ((Polygon)selectedShape).MoveVertex(prevPoint, Mouse.GetPosition(canvas));
                prevPoint = Mouse.GetPosition(canvas);
                DrawShapes();
            }
            else if (movingEdge && selectedShape != null)
            {
                ((Polygon)selectedShape).MoveEdge(prevPoint, Mouse.GetPosition(canvas));
                prevPoint = Mouse.GetPosition(canvas);
                DrawShapes();
            }
        }

        private void addPolygon_Click(object sender, RoutedEventArgs e)
        {
            if (puttingPolygon == true)
            {
                if (newPolygon == null || newPolygon.edges.Count < 3)
                {
                    MessageBox.Show("WIelokąt musi zawierać więcej niż dwa wierzchołki", "Uwaga!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                polygons.Add(newPolygon);
                newPolygon = null;
                puttingPolygon = false;
                addPolygon.Content = "Dodaj wielokąt";
            }
            else
            {
                puttingPolygon = true;
                addPolygon.Content = "Zakończ";
            }
        }

        private void addCircle_Click(object sender, RoutedEventArgs e)
        {
            if (puttingCircle == true)
            {
                if (newCircle != null && newCircle.radius > 0) circles.Add(newCircle);
                newCircle = null;
                puttingCircle = false;
                addCircle.Content = "Dodaj okrąg";
            }
            else
            {
                puttingCircle = true;
                addCircle.Content = "Zakończ";
            }
        }

        private void removeCircle_Click(object sender, RoutedEventArgs e)
        {
            ClickButtonHandle(ref deletingCircle, removeCircle, "Usuń okrąg");
        }

        private void removePolygon_Click(object sender, RoutedEventArgs e)
        {
            ClickButtonHandle(ref deletingPolygon, removePolygon, "Usuń wielokąt");
        }

        private void moveVertex_Click(object sender, RoutedEventArgs e)
        {
            if (!movingVertex)
            {
                movingVertex = true;
                moveVertex.Content = "Zakończ";
            }
            else
            {
                movingVertex = false;
                moveVertex.Content = "Przesuń wierzchołek";
                selectedShape = null;
            }
        }

        private void moveEdge_Click(object sender, RoutedEventArgs e)
        {
            if (!movingEdge)
            {
                movingEdge = true;
                moveEdge.Content = "Zakończ";
            }
            else
            {
                movingEdge = false;
                moveEdge.Content = "Przesuń krawędź";
                selectedShape = null;
            }
        }

        private void movePolygon_Click(object sender, RoutedEventArgs e)
        {
            if (!allowMovingPolygon)
            {
                allowMovingPolygon = true;
                movePolygon.Content = "Zakończ";
            } 
            else
            {
                allowMovingPolygon = false;
                movePolygon.Content = "Przesuń wielokąt";
                if (selectedShape != null)
                {
                    selectedShape.selected = false;
                }
                selectedShape = null;
                DrawShapes();
            }
        }

        private void moveCircle_Click(object sender, RoutedEventArgs e)
        {
            if (!allowMovingCircle)
            {
                allowMovingCircle = true;
                moveCircle.Content = "Zakończ";
            }
            else
            {
                allowMovingCircle = false;
                moveCircle.Content = "Przesuń okrąg";
                if(selectedShape != null) selectedShape.selected = false;
                selectedShape = null;
                DrawShapes();
            }
        }

        private void addVertex_Click(object sender, RoutedEventArgs e)
        {
            ClickButtonHandle(ref addingVertex, addVertex, "Dodaj wierzchołek");
        }

        private void removeVertex_Click(object sender, RoutedEventArgs e)
        {
            ClickButtonHandle(ref removingVertex, removeVertex, "Usuń wierzchołek");
        }

        private void changeCircleRadius_Click(object sender, RoutedEventArgs e)
        {
            if (!changingCircleRadius)
            {
                changingCircleRadius = true;
                changeCircleRadius.Content = "Zakończ";
            }
            else
            {
                changingCircleRadius = false;
                changeCircleRadius.Content = "Zmień promień okręgu";
                if (selectedShape != null) selectedShape.selected = false;
                selectedShape = null;
            }
        }

        private void ClickButtonHandle(ref bool action, Button button, string buttonName)
        {
            if (!action)
            {
                action = true;
                button.Content = "Zakończ";
            }
            else
            {
                action = false;
                button.Content = buttonName;
            }
        }
    }
}
