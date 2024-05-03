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
using System.Windows.Shapes;

namespace Buffet_Galina_WPF
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<DishDTO> Dishes
        {
            get => dishes;
            set { dishes = value; Signal(nameof(Dishes)); }
        }
        public AdminDTO Admin { get; set; }

        public List<CategoryDTO> Categories { get; set; }
        public CategoryDTO SelectedCategories
        { 
            get => selectedCategory; 
            set
            {
                selectedCategory = value;
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

        public AdminWindow(AdminDTO admin)
        {
            InitializeComponent();
            this.Admin = admin;
            DataContext = this;
            LoadDefaultImage();
            LoadDishes();
            LoadCategories();
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
        private ObservableCollection<DishDTO> dishes;
        private CategoryDTO selectedCategory;

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
        void Signal(string prop) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            Close();
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            new AddDish(Admin).ShowDialog();
            await LoadDishes();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedDish == null)
            {
                MessageBox.Show("Выберите блюдо!");
                return;
            }
            new EditDish(Admin, SelectedDish).ShowDialog();
            LoadDishes();
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            await Client.Instance.DeleteDish(SelectedDish.Id);
            await LoadDishes();

        }
    }
}
