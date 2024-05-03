using Buffet_Galina_WPF.API;
using Buffet_Galina_WPF.DTO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
    /// Логика взаимодействия для AddDish.xaml
    /// </summary>
    public partial class AddDish : Window, INotifyPropertyChanged
    {
        public AdminDTO Admin;
        public DishDTO dishDTO = new DishDTO();
        public DishDTO DishDTO { get => dishDTO; set => dishDTO = value; }

        public List<ProductDTO> Products { get; set; }

        private ObservableCollection<ProductDTO> selectedProduct =new();
        public ObservableCollection<ProductDTO> SelectedProducts
        {
            get => selectedProduct;
            set
            {
                selectedProduct = value;
                Signal();
            }
        }

        public ProductDTO SelectProductAdd { get; set; }
        public ProductDTO SelectProductRemove { get; set; }


        public List<CategoryDTO> Categories { get; set; }
        public CategoryDTO SelectedCategories { get; set; }

        public AddDish(AdminDTO admin)
        {
            InitializeComponent();
            this.Admin = admin;
            LoadCategories();
            LoadProducts();
            DataContext = this;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void Signal([CallerMemberName] string prop = null) =>
          PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        private async Task LoadCategories()
        {
            var client = new Client();
            Categories = await client.GetCategories();

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Categories)));
        }

        private async Task LoadProducts()
        {
            var client = new Client();
            Products = await client.GetProducts();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Products)));
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            new AdminWindow(Admin).ShowDialog();
            Close();
        }

        private async void SaveClose_Click(object sender, RoutedEventArgs e)
        {
            if (dishDTO.Title == null || SelectedCategories == null || dishDTO.Price == null || dishDTO.Image == null || SelectedProducts == null)
            {
                MessageBox.Show("Не все поля заполнены!!");
                return;
            }
            
            await Client.Instance.AddDishAsync(new DishDTO
            {
                Title= dishDTO.Title,
                CategoryId = SelectedCategories.Id,
                Category = SelectedCategories.Title,
                Price = dishDTO.Price,
                Image= dishDTO.Image,
                Products = SelectedProducts.ToList()
            });

            Close();
        }

        private void SelectPhoto(object sender, RoutedEventArgs e)
        {
            string dir = Environment.CurrentDirectory + @"\Images\";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Images|*.jpg;";
            if (dlg.ShowDialog() == true)
            {
                var test = new BitmapImage(new Uri(dlg.FileName));
                if (test.PixelWidth > 2000 || test.PixelHeight > 2000)
                {
                    MessageBox.Show("Картинка слишком большая");
                    return;
                }
                string newFile = dir + new FileInfo(dlg.FileName).Name;
                File.Copy(dlg.FileName, newFile, true);
                DishDTO.Image = File.ReadAllBytes(newFile);
                Signal("DishDTO");
            }
        }

        private void AddPro_Click(object sender, RoutedEventArgs e)
        {
            if (SelectProductAdd != null)
            {
                SelectedProducts.Add(SelectProductAdd);
            }
            else
            {
                MessageBox.Show("Выберите продукт");
            }
        }

        private void RemovePro_Click(object sender, RoutedEventArgs e)
        {
            if (SelectProductRemove != null)
            {
                SelectedProducts.Remove(SelectProductRemove);
            }
            else
            {
                MessageBox.Show("Выберите продукт");
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            SelectedProducts.Clear();   
        }
    }
}
