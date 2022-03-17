using System.Windows;

namespace School
{
    /// <summary>
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        public string Password { get; set; }

        public Admin()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void Button_ToAdmin_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}