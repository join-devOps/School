using School.SQL;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.ComponentModel;

namespace School.Client
{
    /// <summary>
    /// Логика взаимодействия для AddServiceForClient.xaml
    /// </summary>
    public partial class AddServiceForClient : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public AddServiceForClient()
        {
            InitializeComponent();
            DataContext = this;
        }

        public ClientService CurrentClientService { get; set; }

        public List<string> GetItemsName
        {
            get => Base.EM.Client.Select(items => items.FirstName + " " + items.LastName + " " + items.Patronymic).ToList();
        }

        public List<string> GetItemsServices
        {
            get => Base.EM.Service.Select(items => items.Title).ToList();
        }

        private string _GetTimeOfMinutes;
        public string GetTimeOfMinutes
        {
            get =>_GetTimeOfMinutes;
            set
            {
                _GetTimeOfMinutes = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("GetTimeOfMinutes"));
            }
        }


        private void Button_Close_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}