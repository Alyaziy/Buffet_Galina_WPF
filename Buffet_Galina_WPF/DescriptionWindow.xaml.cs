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
    /// Логика взаимодействия для DescriptionWindow.xaml
    /// </summary>
    public partial class DescriptionWindow : Window, INotifyPropertyChanged
    {
        private DishDTO selectedDish;

        

        private ObservableCollection<ProductDTO> selectedProduct;
        public List<ProductDTO> Products { get; set; }
        public ObservableCollection<ProductDTO> SelectedProducts
        {
            get => selectedProduct;
            set
            {
                selectedProduct = value;
                Signal();
            }
        }

        private CategoryDTO selectedCategory;
        public List<CategoryDTO> Categories { get; set; }
        public CategoryDTO SelectedCategories
        {
            get => selectedCategory;
            set
            {
                selectedCategory = value;
                Signal();
            }
        }


        public DishDTO SelectedDish
        {
            get => selectedDish;
            set
            {
                selectedDish = value;


            }
        }
        
       
       

        public event PropertyChangedEventHandler? PropertyChanged;
        public void Signal([CallerMemberName] string prop = null) =>
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        public DescriptionWindow(DishDTO dish)
        {
            InitializeComponent();
            SelectedDish = dish;
            LoadProducts();
            LoadCategories();
            DataContext = this;
        }

        private async Task LoadCategories()
        {
            var client = new Client();
            Categories = await client.GetCategories();
            SelectedCategories = Categories.FirstOrDefault(s => s.Id == SelectedDish.CategoryId);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Categories)));
        }

        private async Task LoadProducts()
        {
            var client = new Client();
            Products = await client.GetProducts();
            SelectedProducts = new ObservableCollection<ProductDTO>(SelectedDish.Products);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Products)));
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }
    }
}
