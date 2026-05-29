using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Shoes
{
    public partial class ListOrderWindow : Window
    {
        List<Order> orders;

        public ListOrderWindow()
        {
            InitializeComponent();

            if (UserSingleton.GetUser?.UserRole == 1)
            {
                AddOrderButton.Visibility = Visibility.Visible;
                DeleteOrderButton.Visibility = Visibility.Visible;
            }

            LoadOrders();
        }

        public void LoadOrders()
        {
            orders = new ShoesContext().Orders
                .Include(x => x.OrderStatusNavigation)
                .Include(x => x.PickPointAddressNavigation)
                .ToList();

            OrderListBox.Items.Clear();
            foreach (var order in orders)
                OrderListBox.Items.Add(new OrderControl(order));
        }

        private void AddOrderButton_Click(object sender, RoutedEventArgs e)
        {
            new AddEditOrder().Show();
            Close();
        }

        private void DeleteOrderButton_Click(object sender, RoutedEventArgs e)
        {
            OrderControl orderControl = (OrderControl)OrderListBox.SelectedItem;
            if (orderControl == null)
            {
                MessageBox.Show("Выберите заказ для удаления!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var result = MessageBox.Show("Вы уверены, что хотите удалить заказ?", "ПРЕДУПРЕЖДЕНИЕ", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                var DB = new ShoesContext();
                var orderProducts = DB.OrderProducts.Where(x => x.NumberOrder == orderControl.CurrentOrder.Id).ToList();
                DB.OrderProducts.RemoveRange(orderProducts);
                var findOrder = DB.Orders.Find(orderControl.CurrentOrder.Id);
                DB.Orders.Remove(findOrder);

                DB.SaveChanges();
                LoadOrders();
                MessageBox.Show("Заказ успешно удален!","УСПЕШНО", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void OrderListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (UserSingleton.GetUser?.UserRole == 1)
            {
                OrderControl orderControl = (OrderControl)OrderListBox.SelectedItem;
                if (orderControl != null)
                {

                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new ListProductWindow().Show();
            Close();
        }
    }
}