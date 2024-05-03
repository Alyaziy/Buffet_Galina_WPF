using Buffet_Galina_WPF.API;
using Buffet_Galina_WPF.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Buffet_Galina_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<DishDTO> Dishes { get; set; }
        public List<CategoryDTO> Categories { get; set; }
        public CategoryDTO SelectedCategories { 
            get => selectedCategories;
            set
            {
                selectedCategories = value;
                LoadDishes(SelectedCategories);
            }
        }
        public DishDTO selectedDish { get; set; }

        public DishDTO SelectedDish
        {
            get => selectedDish;
            set
            {
                selectedDish = value;

            }
        }




        public MainWindow()
        {

            InitializeComponent();
            DataContext = this;
            LoadDishes();
            LoadCategories();
            LoadDefaultImage();


        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private async Task LoadDishes()
        {
            Client client = new Client();
            await LoadDishes(client);
        }

        private async Task LoadDishes(Client client)
        {
            Dishes = new ObservableCollection<DishDTO>(await client.GetDish());
            foreach (var d in Dishes)
                if (d.Image == null)
                    d.Image = defaultImage;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Dishes)));
        }

        private async Task LoadDishes(CategoryDTO category)
        { 
            Client client = new Client();
            if (category == null || category.Id == 0)
            {
                LoadDishes(client);
                return;
            }
           
            Dishes = new ObservableCollection<DishDTO>(await client.GetDish(category.Id));
            foreach (var d in Dishes)
                if (d.Image == null)
                    d.Image = defaultImage;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Dishes)));
        }

        byte[] defaultImage;
        private CategoryDTO selectedCategories;

        private void LoadDefaultImage()
        {
            var stream = Application.GetResourceStream(new Uri("Images\\picture.png", UriKind.Relative));
            defaultImage = new byte[stream.Stream.Length];
            stream.Stream.Read(defaultImage, 0, defaultImage.Length);
        }

        private async Task LoadCategories()
        {
            try
            {
                var client = new Client();
                Categories = await client.GetCategories();
                Categories.Insert(0, new CategoryDTO { Title = "Все категории" });
                SelectedCategories = Categories.First();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Categories)));
            }
            catch { }

        }



        private void Basket_Click(object sender, RoutedEventArgs e)
        {
            new BasketWindow(order).Show();
            Close();

        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            Close();
        }

        public OrderDTO order;

        private async void AddBasket_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            SelectedDish = b.Tag as DishDTO;
           if(order == null)
            {
                order = new OrderDTO();
                await Client.Instance.AddOrderAsync(order);
            }

            order.DishDTOs.Add(SelectedDish);
            await Client.Instance.AddDishToOrder(order, SelectedDish, 1);
                      
            new BasketWindow(order).ShowDialog();
            LoadDishes();
        }

        private void Description_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            SelectedDish = b.Tag as DishDTO;
            new DescriptionWindow(SelectedDish).ShowDialog();
            LoadDishes();
        }
    }
}
