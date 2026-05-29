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
            OrderDateTextBox.Text = order.OrderDate?.ToString("dd.MM.yyyy") ?? "";
            DeliveryDateTextBox.Text = order.DeliveryDate?.ToString("dd.MM.yyyy") ?? "";

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

            var pickPoints = DB.PickPoints.ToList();
            PickPointComboBox.ItemsSource = pickPoints;
            PickPointComboBox.DisplayMemberPath = "FullAddress";
            PickPointComboBox.SelectedValuePath = "Id";
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrderStatusComboBox.SelectedValue == null ||
                PickPointComboBox.SelectedValue == null ||
                string.IsNullOrEmpty(OrderDateTextBox.Text) ||
                string.IsNullOrEmpty(DeliveryDateTextBox.Text))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!DateTime.TryParse(OrderDateTextBox.Text, out DateTime orderDate))
            {
                MessageBox.Show("Неверный формат даты заказа! Используйте дд.мм.гггг", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!DateTime.TryParse(DeliveryDateTextBox.Text, out DateTime deliveryDate))
            {
                MessageBox.Show("Неверный формат даты доставки! Используйте дд.мм.гггг", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (deliveryDate <= orderDate)
            {
                MessageBox.Show("Дата доставки должна быть позже даты заказа!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
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
            order.OrderDate = orderDate;
            order.DeliveryDate = deliveryDate;

            if (CurrentOrder != null)
                DB.Orders.Update(order);
            else
                DB.Orders.Add(order);

            DB.SaveChanges();
            MessageBox.Show(editOrAdd, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new ListOrderWindow().Show();
            Close();
        }
    }
}