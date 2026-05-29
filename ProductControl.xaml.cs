using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
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

namespace Shoes
{
    public partial class ProductControl : UserControl
    {
        public Product CurrentProduct;
        public ProductControl(Product currentProduct)
        {
            InitializeComponent();
            CurrentProduct = currentProduct;
            TypeNameTextBlock.Text = $"{currentProduct.CategoryNavigation.Name}| {currentProduct.Name}";
            DescriptionTextBlock.Text = $"{currentProduct.Description}";
            ManufacturerNameTextBlock.Text = $"{currentProduct.ManufacturerNavigation.Name}";
            SupplierNameTextBlock.Text = $"{currentProduct.SupplierNavigation.Name}";
            UnitMetricTextBlock.Text = $"{currentProduct.UnitMetric}";
            AmountTextBlock.Text = $"{currentProduct.Amount}";
            DiscountTextBlock.Text = $"{currentProduct.Discount}";

            if (currentProduct.Discount > 15)
                this.Background = Brushes.SeaGreen;

            if (currentProduct.Amount == 0)
                AmountTextBlock.Background = Brushes.LightBlue;

            if (currentProduct.Discount > 0)
            {
                OldPrice.Text = $"{currentProduct.Price}";
                OldPrice.Foreground = Brushes.Red;
                OldPrice.TextDecorations = TextDecorations.Strikethrough;
                NewPrice.Text = $"{currentProduct.Price - currentProduct.Price * currentProduct.Discount / 100}";
            }
            else
            {
                PriceTextBlock.Text = $"{currentProduct.Price}";
            }

            if (currentProduct.Photo != null)
            {
                string exeDir = AppDomain.CurrentDomain.BaseDirectory;
                string fullPath = System.IO.Path.Combine(exeDir, "Resources", currentProduct.Photo);
                if (System.IO.File.Exists(fullPath))
                    PhotoProductImage.Source = new BitmapImage(new Uri(fullPath));
            }
        }
    }
}