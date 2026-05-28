using Microsoft.Win32;
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
using System.IO;

namespace Shoes
{
    public partial class AddProductWindow : Window
    {
        public Product CurrentProduct;
        public string PathImage = "";
        string editOrAdd = "";

        public AddProductWindow()
        {
            InitializeComponent();
            Title = "Добавление товара";
            editOrAdd = "Товар успешно добавлен";
            TypeComboBox.SelectedIndex = 0;
        }

        public AddProductWindow(Product currentProduct)
        {
            InitializeComponent();
            Title = "Редактирование товара";
            editOrAdd = "Товар успешно изменен";
            CurrentProduct = currentProduct;
            TypeComboBox.SelectedIndex = (int)currentProduct.Category - 1;
            NameTextBox.Text = $"{currentProduct.Name}";
            ArticleTextBox.Text = $"{currentProduct.Article}";
            DescriptionTextBox.Text = $"{currentProduct.Description}";
            ManufactureComboBox.SelectedIndex = (int)currentProduct.Manufacturer - 1;
            SupplierNameTextBox.Text = $"{currentProduct.SupplierNavigation.Name}";
            UnitMetricTextBox.Text = $"{currentProduct.UnitMetric}";
            AmountTextBox.Text = $"{currentProduct.Amount}";
            PriceTextBox.Text = $"{currentProduct.Price}";
            DiscountTextBox.Text = $"{currentProduct.Discount}";

            if (currentProduct.Photo != null)
            {
                PhotoProductImage.Source = new BitmapImage(new Uri($"C:\\Users\\albnu\\source\\repos\\Shoes\\Shoes\\Resources\\{currentProduct.Photo}"));
                PathImage = currentProduct.Photo;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string result = Validate();
            if (result != "")
            {
                MessageBox.Show(result, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Product currentProduct;
            if (CurrentProduct != null)
                currentProduct = new ShoesContext().Products.Find(CurrentProduct.Id);
            else
                currentProduct = new Product();

            currentProduct.Category = TypeComboBox.SelectedIndex + 1;
            currentProduct.Name = NameTextBox.Text;
            currentProduct.Article = ArticleTextBox.Text;
            currentProduct.Description = DescriptionTextBox.Text;
            currentProduct.Manufacturer = ManufactureComboBox.SelectedIndex + 1;
            currentProduct.Supplier = new ShoesContext().Suppliers.Where(x => x.Name == SupplierNameTextBox.Text).FirstOrDefault().Id;
            currentProduct.UnitMetric = UnitMetricTextBox.Text;
            currentProduct.Amount = int.Parse(AmountTextBox.Text);
            currentProduct.Price = decimal.Parse(PriceTextBox.Text);
            currentProduct.Discount = decimal.Parse(DiscountTextBox.Text);

            if (PathImage != "")
                currentProduct.Photo = PathImage;

            var DB = new ShoesContext();
            if (CurrentProduct != null)
                DB.Products.Update(currentProduct);
            else
            {
                currentProduct.Id = DB.Products.Max(x => x.Id) + 1;
                DB.Products.Add(currentProduct);
            }

            DB.SaveChanges();
            MessageBox.Show(editOrAdd, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            new ListProductWindow().Show();
            Close();
        }

        string Validate()
        {
            string result = "";

            if (ArticleTextBox.Text == "" ||
                NameTextBox.Text == "" ||
                DescriptionTextBox.Text == "" ||
                PriceTextBox.Text == "" ||
                DiscountTextBox.Text == "" ||
                AmountTextBox.Text == "")
            {
                result += "Ни одно поле не должно оставаться пустым!\n";
            }

            if (new ShoesContext().Suppliers.Where(x => x.Name == SupplierNameTextBox.Text).FirstOrDefault() == null)
                result += "Указанный поставщик должен существовать!\n";

            if (!decimal.TryParse(PriceTextBox.Text, out decimal price) || price <= 0)
                result += "Цена должна быть положительным числом!\n";

            if (!decimal.TryParse(DiscountTextBox.Text, out decimal discount) || discount < 0)
                result += "Скидка не может быть отрицательной!\n";

            if (!int.TryParse(AmountTextBox.Text, out int amount) || amount < 0)
                result += "Количество на складе не может быть отрицательным!\n";

            return result;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new ListProductWindow().Show();
            Close();
        }

        private void SelectImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string selectedPath = openFileDialog.FileName;
                string selectedName = selectedPath.Split("\\").Last();
                string destination = $"C:\\Users\\albnu\\source\\repos\\Shoes\\Shoes\\Resources\\{selectedName}";

                BitmapImage image = new BitmapImage(new Uri(selectedPath));
                if (image.PixelWidth > 300 || image.PixelHeight > 200)
                {
                    MessageBox.Show("Изображение не должно превышать 300x200 пикселей!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                PathImage = selectedName;
                File.Copy(selectedPath, destination, overwrite: true);
            }
        }
    }
}