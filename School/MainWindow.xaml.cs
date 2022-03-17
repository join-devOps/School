using School.SQL;
using System.Collections.Generic;
using System.Windows;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System;
using System.Windows.Controls;
using School.Client;
using Word = Microsoft.Office.Interop.Word;

namespace School
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();

            ListService = Base.EM.Service.ToList();

            MaxCountService = (byte)ListService.Count;

            DataContext = this;

        }

        public string GetContentButtonIsAdmin
        {
            get
            {
                if (IsAdmin)
                    return "Выйти из админа";
                return "Войти в админ";
            }
        }

        private bool _IsAdmin = false;
        public bool IsAdmin
        {
            get => _IsAdmin;
            set
            {
                _IsAdmin = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("GetContentButtonIsAdmin"));
                    PropertyChanged(this, new PropertyChangedEventArgs("GetAccessAdmin"));
                }
            }
        }

        public string GetAccessAdmin
        {
            get
            {
                if (IsAdmin)
                    return "Visible";
                return "Collapsed";
            }
        }

        private List<Tuple<string, float, float>> FilterByDiscountValuesList = new List<Tuple<string, float, float>>() {
            Tuple.Create("Все записи", 0f, 1f),
            Tuple.Create("от 0% до 5%", 0f, 0.05f),
            Tuple.Create("от 5% до 15%", 0.05f, 0.15f),
            Tuple.Create("от 15% до 30%", 0.15f, 0.3f),
            Tuple.Create("от 30% до 70%", 0.3f, 0.7f),
            Tuple.Create("от 70% до 100%", 0.7f, 1f)
        };

        public List<string> FilterByDiscountNamesList
        {
            get
            {
                return FilterByDiscountValuesList.Select(item => item.Item1).ToList();
            }
        }

        private Tuple<float, float> _CurrentDiscountFilter = Tuple.Create(float.MinValue, float.MaxValue);

        public Tuple<float, float> CurrentDiscountFilter
        {
            get
            {
                return _CurrentDiscountFilter;
            }
            set
            {
                _CurrentDiscountFilter = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ListService"));
                }
            }
        }

        private List<Service> _ListService;
        public List<Service> ListService
        {
            get
            {
                List<Service> listService = _ListService;
                listService.FindAll(item =>
                    item.DiscountFloat >= CurrentDiscountFilter.Item1 &&
                    item.DiscountFloat < CurrentDiscountFilter.Item2);

                if (SearchInfo != null)
                    listService = listService.Where(item =>
                    item.Title.IndexOf(SearchInfo, StringComparison.OrdinalIgnoreCase) != -1).ToList();

                CountService = (byte)listService.Count;

                if (CodeSort == 0)
                    return listService.OrderBy(item => item.CostWithDiscount).ToList();
                else if (CodeSort == 1)
                    return listService.OrderByDescending(item => item.CostWithDiscount).ToList();
                else return listService.ToList();
            }
            set
            {
                _ListService = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ListService"));
                }
            }
        }

        private byte _MaxCountService;
        public byte MaxCountService
        {
            get => _MaxCountService;
            set
            {
                _MaxCountService = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("MaxCountService"));
            }
        }

        private byte _CountService;
        public byte CountService
        {
            get => _CountService;
            set
            {
                _CountService = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CountService"));
            }
        }

        private string _SearchInfo;
        public string SearchInfo
        {
            get => _SearchInfo;
            set
            {
                _SearchInfo = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ListService"));
            }
        }

        private void TextBox_Name_KeyUp(object sender, KeyEventArgs e)
        {
            SearchInfo = TextBox_Name.Text;
        }

        private string _FilterItems;
        public string FilterItems
        {
            get => _FilterItems;
            set
            {
                _FilterItems = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ListService"));
            }
        }

        private void ComboBox_Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentDiscountFilter = Tuple.Create(
               FilterByDiscountValuesList[ComboBox_Filter.SelectedIndex].Item2,
               FilterByDiscountValuesList[ComboBox_Filter.SelectedIndex].Item3
           );
        }

        private byte _CodeSort;
        public byte CodeSort
        {
            get => _CodeSort;
            set
            {
                _CodeSort = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ListService"));
            }
        }

        private void Button_Sort_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;

            switch (b.Uid)
            {
                case "SortUp":
                    CodeSort = 0;
                    break;
                case "SortDown":
                    CodeSort = 1;
                    break;
            }
        }

        private void Button_ToAdmin_Click(object sender, RoutedEventArgs e)
        {
            if (IsAdmin)
                IsAdmin = false;
            else
            {
                Admin a = new Admin();

                if ((bool)a.ShowDialog())
                    IsAdmin = a.Password == "0000";
            }
        }

        private void Button_AddNewClientService_Click(object sender, RoutedEventArgs e)
        {
            AddServiceForClient asfc = new AddServiceForClient();
            asfc.ShowDialog();
        }

        private void Button_Click_ViewServices(object sender, RoutedEventArgs e)
        {
            ViewServices vs = new ViewServices();
            vs.ShowDialog();
        }

        private void Button_ExportToWord_Click(object sender, RoutedEventArgs e)
        {
            var allServices = ListService.ToList();
            var application = new Word.Application();
            Word.Document document = application.Documents.Add();

            foreach(var services in allServices)
            {
                Word.Paragraph useParagraph = document.Paragraphs.Add();
                Word.Range useRange = useParagraph.Range;

                useRange.Text = services.Title;
                useParagraph.set_Style("Title");
                useRange.InsertParagraphAfter();

                Word.Paragraph tableParagraph = document.Paragraphs.Add();
                Word.Range tableRange = tableParagraph.Range;
                Word.Table newTable = document.Tables.Add(tableRange, allServices.Count() + 1, 3);
                newTable.Borders.InsideLineStyle = newTable.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                newTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
            }
        }
    }
}