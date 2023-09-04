using Microsoft.IdentityModel.Tokens;
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

namespace ADO_1.Views
{
    /// <summary>
    /// Interaction logic for CrudGroupsWindow.xaml
    /// </summary>
    public partial class CrudGroupsWindow : Window
    {
        public DAL.Entity.ProductGroup? ProductGroup { get; set; }

        public string name;
        public string description;
        public string picture;
        public bool enabled = false;
        public bool restored = false;

        public CrudGroupsWindow(DAL.Entity.ProductGroup productGroup)
        {
            InitializeComponent();
            this.ProductGroup = productGroup;
            this.DataContext = this.ProductGroup;

            name = ProductGroup.Name;
            description = ProductGroup.Description;
            picture = ProductGroup.Picture;
            SaveButton.IsEnabled = enabled;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductGroup.Id.ToString().IsNullOrEmpty() || ProductGroup.Name.IsNullOrEmpty()
                || ProductGroup.Description.IsNullOrEmpty() || ProductGroup.Picture.IsNullOrEmpty()
                || !(ProductGroup.Picture.EndsWith(".jpg") || ProductGroup.Picture.EndsWith(".png")))
            {
                DialogResult = false;
                MessageBox.Show("Wrong input");
            }
            else
            {
                DialogResult = true;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            ProductGroup = null;
            Close();
        }

        private void NameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            enabled = NameBox.Text != name;
            SaveButton.IsEnabled = enabled;
        }

        private void DescriptionBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            enabled = DescriptionBox.Text != description;
            SaveButton.IsEnabled = enabled;
        }

        private void PictureBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            enabled = PictureBox.Text != picture;
            SaveButton.IsEnabled = enabled;
        }

        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductGroup.Id.ToString().IsNullOrEmpty() || ProductGroup.Name.IsNullOrEmpty()
                || ProductGroup.Description.IsNullOrEmpty() || ProductGroup.Picture.ToString().IsNullOrEmpty())
            {
                DialogResult = false;
                MessageBox.Show("Wrong input");
            }
            else
            {
                DialogResult = true;
                restored = true;
            }
        }
    }
}
