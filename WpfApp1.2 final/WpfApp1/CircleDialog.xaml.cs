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
    /// Interaction logic for CircleDialog.xaml
    /// </summary>
    public partial class CircleDialog : Window
    {
        Circle circle;
        
        public CircleDialog(Circle circle)
        {
            InitializeComponent();

            this.circle = circle;
            if (circle.limit == CircleLimitations.constRadius)
                constRadiusCheckBox.IsChecked = true;
            else if (circle.limit == CircleLimitations.constCenter)
                constCenterChceckBox.IsChecked = true;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)constRadiusCheckBox.IsChecked)
                circle.limit = CircleLimitations.constRadius;
            else if ((bool)constCenterChceckBox.IsChecked)
                circle.limit = CircleLimitations.constCenter;
            else circle.limit = CircleLimitations.none;
            Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void constRadiusCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            constCenterChceckBox.IsChecked = false;
        }

        private void constCenterChceckBox_Checked(object sender, RoutedEventArgs e)
        {
            constRadiusCheckBox.IsChecked = false;
        }
    }
}
