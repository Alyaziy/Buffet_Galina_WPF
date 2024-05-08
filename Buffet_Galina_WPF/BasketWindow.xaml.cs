using Buffet_Galina_WPF.API;
using Buffet_Galina_WPF.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ObservableCollection<NewShit> Items { get; set; }

        public int Count { get => Items.Sum(s => s.Count); }
        public int Price { get => Items.Sum(s => s.Price); }


        public BasketWindow(OrderDTO order)
        {
            InitializeComponent();
            SelectedOrder = order;
            DataContext = this;
            if(order == null)
            {
                Items = new();
                return;
            }
            Items = new ObservableCollection<NewShit>( SelectedOrder.DishDTOs.GroupBy(s => s.Title).Select(s => new NewShit { Count = s.Count(), Dish = s.First(), Price = s.Sum(d => d.Price) }));

            
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void Signal([CallerMemberName] string prop = null) =>
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            
            Close();
        }

        private void Order_Click(object sender, RoutedEventArgs e)
        {
            
            if (Items.Count() == 0)
            { 
                MessageBox.Show("Заказ не оформлен! Выберите блюдо!");
                return;
            }
            else
            {
                new OrderWindow(SelectedOrder).ShowDialog();
                Close();
            }
            


        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {

            Button b = sender as Button;
            SelectedShit = b.Tag as NewShit;
            await Client.Instance.AddDishToOrder(SelectedOrder, SelectedShit.Dish, Count);
            SelectedOrder.DishDTOs.Add(SelectedShit.Dish);
            
            Items = new ObservableCollection<NewShit>(SelectedOrder.DishDTOs.GroupBy(s => s.Title).Select(s => new NewShit { Count = s.Count(), Dish = s.First(), Price=s.Sum(d=>d.Price) }));
            Signal(nameof(Price));
            Signal(nameof(Count));
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            SelectedShit = b.Tag as NewShit;
            try
            {
                await Client.Instance.DeleteDishInOrder(SelectedShit.Dish.Id);
                SelectedOrder.DishDTOs.Remove(SelectedShit.Dish);
                Items.Remove(SelectedShit);
                Items = new ObservableCollection<NewShit>(SelectedOrder.DishDTOs.GroupBy(s => s.Title).Select(s => new NewShit { Count = s.Count(), Dish = s.First(), Price = s.Sum(d => d.Price) }));
                Signal(nameof(Count));
                Signal(nameof(Price));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
