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
        private DishDTO selectedDish;
        
        public List<CategoryDTO> Categories { get; set; }
        

        public DishDTO SelectedDish
        {
            get => selectedDish;
            set
            {
                selectedDish = value;

            }
        }

        public BasketWindow(DishDTO dish)
        {
            InitializeComponent();
            SelectedDish = dish;

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
            new OrderWindow().Show();
            Close();
        }
    }
}
