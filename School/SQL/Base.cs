using System;
using System.Windows.Media;

namespace School.SQL
{
    class Base
    {
        public static Entities EM = new Entities();
    }

    public partial class ClientService
    {
        public SolidColorBrush GetColorClientService
        {
            get
            {
                DateTime _dt = DateTime.Now;
                _dt.ToString("dd.MM.yyyy hh:mm:ss");
                DateTime timeStart = Convert.ToDateTime(GetTime);
                TimeSpan span = timeStart.Subtract(_dt);

                if (span.Hours < 1)
                    return Brushes.Red;
                return null;
            }
        }

        public string GetTime
        {
            get => StartTime.ToString("dd.MM.yyyy hh:mm:ss");
        }

        public string GetNameClient
        {
            get => Client.FirstName + " " + Client.LastName + " " + Client.Patronymic;
        }
    }


    public partial class Service
    {
        public float DiscountFloat
        {
            get
            {
                return Convert.ToSingle(Discount ?? 0);
            }
        }
        public SolidColorBrush GetColor
        {
            get
            {
                if (HasDiscount == true)
                    return Brushes.LightGreen;
                return null;
            }
        }

        public string GetImage
        {
            get => "\\" + MainImagePath;
        }

        public bool HasDiscount
        {
            get => Discount > 0;
        }

        public string CostWithDiscount
        {
            get => (Cost * Convert.ToDecimal(1 - Discount ?? 0)).ToString("#.## рублей");
        }

        public string GetDiscount
        {
            get
            {
                if (HasDiscount == true)
                    return "*скидка " + Discount;
                return null;
            }
        }


        public string GetTextDecoration
        {
            get => HasDiscount ? "Strikethrough" : "None";
        }

        public string GetCost
        {
            get
            {
                if (HasDiscount == true)
                    return Cost.ToString("#.##");
                return null;
            }

        }

        public string GetMinutes
        {
            get => (DurationInSeconds / 60).ToString("за # минут");
        }

    }

}