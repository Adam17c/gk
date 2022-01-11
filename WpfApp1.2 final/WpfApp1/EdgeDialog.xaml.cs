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
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for EdgeDialog.xaml
    /// </summary>
    public partial class EdgeDialog : Window
    {
        Edge edge;
        
        public EdgeDialog(Edge edge)
        {
            InitializeComponent();

            this.edge = edge;

            switch(edge.limit)
            {
                case EdgeLimitations.constLength:
                    specifiedLengthCheckBox.IsChecked = true;
                    break;
                case EdgeLimitations.equalLength:
                    equalLengthCheckBox.IsChecked = true;
                    break;
                case EdgeLimitations.contactedCircle:
                    tangencyCheckBox.IsChecked = true;
                    break;
                case EdgeLimitations.perpendicularEdge:
                    perpendicularityCheckBox.IsChecked = true;
                    break;
            }
        }

        private void specifiedLengthCheckBox_Click(object sender, RoutedEventArgs e)
        {
            edge.limit = EdgeLimitations.constLength;
            Close();
        }


        private void equalLengthCheckBox_Click(object sender, RoutedEventArgs e)
        {
            edge.limit = EdgeLimitations.equalLength;
            Close();
        }

        private void tangencyCheckBox_Click(object sender, RoutedEventArgs e)
        {
            edge.limit = EdgeLimitations.contactedCircle;
            Close();
        }

        private void perpendicularityCheckBox_Click(object sender, RoutedEventArgs e)
        {
            edge.limit = EdgeLimitations.perpendicularEdge;
            Close();
        }

        private void removeRelationButton_Click(object sender, RoutedEventArgs e)
        {
            edge.limit = EdgeLimitations.none;
            Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
