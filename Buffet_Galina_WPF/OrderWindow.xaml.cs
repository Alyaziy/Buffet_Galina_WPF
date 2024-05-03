using Buffet_Galina_WPF.API;
using Buffet_Galina_WPF.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace Buffet_Galina_WPF
{
    /// <summary>
    /// Логика взаимодействия для OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window, INotifyPropertyChanged
    {
        private OrderDTO selectedOrder;

        public List<CategoryDTO> Categories { get; set; }

        public OrderDTO SelectedOrder
        {
            get => selectedOrder;
            set
            {
                selectedOrder = value;
            }
        }

        public List<NewShit> Items { get; set; }
        public int Count { get => Items.Sum(s => s.Count); }
        public int Price { get => Items.Sum(s => s.Price); }

        public OrderWindow(OrderDTO order)
        {
            InitializeComponent();
            SelectedOrder = order;
            Items = SelectedOrder.DishDTOs.GroupBy(s => s.Title).Select(s => new NewShit { /*Price = s.Sum(),*/ Count = s.Count(), Dish = s.First() }).ToList();
            DataContext = this;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void Signal([CallerMemberName] string prop = null) =>
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }

        public class NewShit
        {
            public int Id { get; internal set; }
            public int Count { get; internal set; }
            public DishDTO Dish { get; internal set; }
            public int Price { get; internal set; }
        }

        
    }
}
