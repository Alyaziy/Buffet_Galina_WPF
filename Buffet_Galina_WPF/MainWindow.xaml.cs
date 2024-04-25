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
        public CategoryDTO SelectedCategories { get; set; }
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

        byte[] defaultImage;
        private void LoadDefaultImage()
        {
            var stream = Application.GetResourceStream(new Uri("Images\\picture.png", UriKind.Relative));
            defaultImage = new byte[stream.Stream.Length];
            stream.Stream.Read(defaultImage, 0, defaultImage.Length);
        }

        private async Task LoadCategories()
        {
            var client = new Client();
            Categories = await client.GetCategories();
            
        }

        private void Basket_Click(object sender, RoutedEventArgs e)
        {
            new BasketWindow(SelectedDish).Show();
            Close();
            
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            Close();
        }

        private void AddBasket_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            SelectedDish = b.Tag as DishDTO;
            new BasketWindow(SelectedDish).ShowDialog();
            LoadDishes();
        }

    }
}
