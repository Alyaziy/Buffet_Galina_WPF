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
    /// Логика взаимодействия для BasketWindow.xaml
    /// </summary>
    public partial class BasketWindow : Window, INotifyPropertyChanged
    {
        public List<CategoryDTO> Categories { get; set; }

        private OrderDTO selectedOrder;
        private NewShit selectedShit;

        public OrderDTO SelectedOrder
        {
            get => selectedOrder;
            set
            {
                selectedOrder = value;
            }
        }

        public NewShit SelectedShit 
        { 
            get => selectedShit; 
            set
            {
                selectedShit = value;
                Signal();
            }
            
        }

        public List<NewShit> Items { get; set; }

        public int Count { get => Items.Sum(s => s.Count); }
        public int Price { get => Items.Sum(s => s.Price); }


        public BasketWindow(OrderDTO order)
        {
            InitializeComponent();
            SelectedOrder = order;

            Items = SelectedOrder.DishDTOs.GroupBy(s => s.Title).Select(s => new NewShit { /*Price = s.Sum(),*/ Count = s.Count(), Dish = s.First() }).ToList();

            DataContext = this;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void Signal([CallerMemberName] string prop = null) =>
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }

        private void Order_Click(object sender, RoutedEventArgs e)
        {
            new OrderWindow(SelectedOrder).Show();
            Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {

            Button b = sender as Button;
            SelectedShit = b.Tag as NewShit;
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            SelectedShit = b.Tag as NewShit;
            try
            {
                await Client.Instance.DeleteOrder(SelectedShit.Dish.Id);
                SelectedOrder.DishDTOs.Remove(SelectedShit.Dish);
            }
            catch (Exception ex)
            {

            }
            
        }
    }

    public class NewShit
    {
        public int Count { get; internal set; }
        public DishDTO Dish { get; internal set; }
        public int Price { get; internal set; }
    }

}
