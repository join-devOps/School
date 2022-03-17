using School.SQL;
using System.Collections.Generic;
using System.Windows;
using System.ComponentModel;
using System.Linq;

namespace School.Client
{
    /// <summary>
    /// Логика взаимодействия для ViewServices.xaml
    /// </summary>
    public partial class ViewServices : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ViewServices()
        {
            InitializeComponent();

            ClientServiceList = Base.EM.ClientService.ToList();

            DataContext = this;
        }

        private List<ClientService> _ClientServiceList;
        public List<ClientService> ClientServiceList
        {
            get
            {
                return _ClientServiceList.OrderByDescending(items => items.StartTime).ToList();
            }
            set
            {
                _ClientServiceList = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ClientServiceList"));
            }
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}