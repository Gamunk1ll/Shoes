using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows.Controls;

namespace Shoes
{
    public partial class OrderControl : UserControl
    {
        public Order CurrentOrder;
        public OrderControl(Order order)
        {
            InitializeComponent();
            CurrentOrder = order;

            var orderProduct = new ShoesContext().OrderProducts
                .Include(x => x.ProductNavigation)
                .Where(x => x.NumberOrder == order.Id)
                .FirstOrDefault();

            ArticleTextBlock.Text = $"Артикул заказа: {orderProduct?.ProductNavigation?.Article ?? "Не указан"}";
            OrderStatusTextBlock.Text = $"Статус заказа: {order.OrderStatusNavigation?.OrderStatus1}";
            PickPointAddressTextBlock.Text = $"Адрес пункта выдачи: {order.PickPointAddressNavigation?.City}, {order.PickPointAddressNavigation?.Street}, {order.PickPointAddressNavigation?.Building}";
            OrderDateTextBlock.Text = $"Дата заказа: {order.OrderDate:dd.MM.yyyy}";
            DeliveryDateTextBlock.Text = $"Дата доставки:\n{order.DeliveryDate:dd.MM.yyyy}";
        }
    }
}