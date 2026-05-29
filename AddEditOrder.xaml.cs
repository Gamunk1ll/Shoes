using System;
using System.Linq;
using System.Windows;

namespace Shoes
{
    public partial class AddEditOrder : Window
    {
        Order CurrentOrder;
        string editOrAdd = "";

        public AddEditOrder()
        {
            InitializeComponent();
            Title = "Добавление заказа";
            editOrAdd = "Заказ успешно добавлен";
            LoadComboBoxes();
        }

        public AddEditOrder(Order order)
        {
            InitializeComponent();
            Title = "Редактирование заказа";
            editOrAdd = "Заказ успешно изменён";
            CurrentOrder = order;
            LoadComboBoxes();

            OrderStatusComboBox.SelectedValue = order.OrderStatus;
            PickPointComboBox.SelectedValue = order.PickPointAddress;
            OrderDatePicker.SelectedDate = order.OrderDate;
            DeliveryDatePicker.SelectedDate = order.DeliveryDate;

            var orderProduct = new ShoesContext().OrderProducts
                .Where(x => x.NumberOrder == order.Id)
                .FirstOrDefault();
            ArticleTextBox.Text = orderProduct?.ProductNavigation?.Article ?? "";
        }

        void LoadComboBoxes()
        {
            var DB = new ShoesContext();

            OrderStatusComboBox.ItemsSource = DB.OrderStatuses.ToList();
            OrderStatusComboBox.DisplayMemberPath = "OrderStatus1";
            OrderStatusComboBox.SelectedValuePath = "Id";
            PickPointComboBox.ItemsSource = DB.PickPoints.ToList();
            PickPointComboBox.DisplayMemberPath = "City";
            PickPointComboBox.SelectedValuePath = "Id";
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrderStatusComboBox.SelectedValue == null ||
                PickPointComboBox.SelectedValue == null ||
                OrderDatePicker.SelectedDate == null ||
                DeliveryDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Заполните все поля!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var DB = new ShoesContext();
            Order order;

            if (CurrentOrder != null)
            {
                order = DB.Orders.Find(CurrentOrder.Id);
            }
            else
            {
                order = new Order();
                order.Id = DB.Orders.Max(x => x.Id) + 1;
                order.UserFullname = UserSingleton.GetUser.Id;
                order.Code = 0;
            }

            order.OrderStatus = (int)OrderStatusComboBox.SelectedValue;
            order.PickPointAddress = (int)PickPointComboBox.SelectedValue;
            order.OrderDate = OrderDatePicker.SelectedDate.Value;
            order.DeliveryDate = DeliveryDatePicker.SelectedDate.Value;

            if (CurrentOrder != null)
                DB.Orders.Update(order);
            else
                DB.Orders.Add(order);

            DB.SaveChanges();
            MessageBox.Show(editOrAdd, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}