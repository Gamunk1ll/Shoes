using Microsoft.EntityFrameworkCore;
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

namespace Shoes
{

    public partial class ListProductWindow : Window
    {
        List<Product> products;
        public ListProductWindow()
        {
            InitializeComponent();
            if (UserSingleton.GetUser != null)
            {
                FullnameTextBlock.Text = $"{UserSingleton.GetUser.Surname} {UserSingleton.GetUser.Name} {UserSingleton.GetUser.Patronymic}";
                if (UserSingleton.GetUser.UserRole == 1)
                {
                    SearchTextBox.Visibility = Visibility.Visible;
                    FilterComboBox.Visibility = Visibility.Visible;
                    SortComboBox.Visibility = Visibility.Visible;
                    DeleteProductButton.Visibility = Visibility.Visible;
                    AddProductButton.Visibility = Visibility.Visible;
                }
                if (UserSingleton.GetUser.UserRole == 2)
                {
                    SearchTextBox.Visibility = Visibility.Visible;
                    FilterComboBox.Visibility = Visibility.Visible;
                    SortComboBox.Visibility = Visibility.Visible;
                }
            }

            try
            {
                products = new ShoesContext().Products.Include(x => x.CategoryNavigation).Include(x => x.ManufacturerNavigation).Include(x => x.SupplierNavigation).ToList();

                foreach (var product in products)
                {
                    ProductListBox.Items.Add(new ProductControl(product));
                }
            }
            catch
            {
                MessageBox.Show("Не удалось взять данные из базы данных", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            UserSingleton.GetUser = null;
            Close();
        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SortFilterSearchProducts();
        }
        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SortFilterSearchProducts();
        }


        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SortFilterSearchProducts();
        }

        public void SortFilterSearchProducts()
        {
        var resultProduct = products;

         if (SortComboBox.SelectedIndex == 0)
           resultProduct = resultProduct.OrderBy(x => x.Amount).ToList();
        else if (SortComboBox.SelectedIndex == 1)
        resultProduct = resultProduct.OrderByDescending(x => x.Amount).ToList();
        if (FilterComboBox.SelectedIndex == 1)
        resultProduct = resultProduct.Where(x => x.Supplier == 1).ToList();
        else if (FilterComboBox.SelectedIndex == 2)
        resultProduct = resultProduct.Where(x => x.Supplier == 2).ToList();
        var searchText = SearchTextBox.Text?.Trim();
        if (!string.IsNullOrEmpty(searchText))
        {
        resultProduct = resultProduct.Where(x =>
            (x.Name != null && x.Name.Contains(searchText)) ||
            (x.Article != null && x.Article.Contains(searchText)) ||
            (x.Description != null && x.Description.Contains(searchText)) ||
            (x.ManufacturerNavigation?.Name != null && x.ManufacturerNavigation.Name.Contains(searchText)) ||
            (x.CategoryNavigation?.Name != null && x.CategoryNavigation.Name.Contains(searchText)) ||
            (x.SupplierNavigation?.Name != null && x.SupplierNavigation.Name.Contains(searchText)))
        .ToList();
        }

        ProductListBox.Items.Clear();
        foreach (var product in resultProduct)
        {
        ProductListBox.Items.Add(new ProductControl(product));
        }
        }

        private void DeleteProductButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите удалить товар?", "ПРЕДУПРЕЖДЕНИЕ",MessageBoxButton.YesNo,MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes) 
            {
                ProductControl productControl = (ProductControl)ProductListBox.SelectedItem;
                if (productControl != null)
                {
                    var product = productControl.CurrentProduct;
                    var DB = new ShoesContext();
                   if( DB.OrderProducts.Where(x=>x.Product == product.Id).FirstOrDefault() == null)
                    {
                        var findProduct = DB.Products.Find(product.Id);
                        DB.Products.Remove(findProduct);
                        DB.SaveChanges();
                        ProductListBox.Items.Remove(productControl);
                    }
                else 
                    {
                        MessageBox.Show("Нельзя удалить товары, присутствующие в заказе!", "ПРЕДУПРЕЖДЕНИЕ", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            new AddProductWindow().Show();
            Close();
        }

        private void ProductListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(UserSingleton.GetUser !=null && UserSingleton.GetUser.UserRole == 1)
            {
                ProductControl productControl = (ProductControl) ProductListBox.SelectedItem;
                if (productControl != null)
                {
                    new AddProductWindow(productControl.CurrentProduct).Show();
                    Close();
                }
            }
        }
    }
}
