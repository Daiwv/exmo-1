using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exmo.Common
{
    using System.Collections.ObjectModel;

    using Windows.UI;
    using Windows.UI.Xaml.Media;

    using Exmo.Core.Models.Enums;
    using Exmo.ViewModels;

    using Syncfusion.UI.Xaml.Charts;

    public static class Converters
    {
        public static string FormatPersents(this decimal persent)
        {
            return (persent > 0 ? "\x2191 " : persent < 0 ? "\x2193 " : string.Empty) + Math.Abs(Math.Round(persent, 2)) + "%";
        }

        public static string FormatCurrency(this decimal currency)
        {
            return currency.ToString("###,###,##0.00###");
        }


        public static SolidColorBrush PersentsColor(this decimal persent)
        {
            return persent < 0 ? new SolidColorBrush { Color = Color.FromArgb(255, 218, 54, 86) } : new SolidColorBrush { Color = Color.FromArgb(255, 83, 126, 247) };
        }

        public static string FormatCurrencyPair(this string currencyPair)
        {
            return currencyPair?.Replace('_', '/');
        }

        public static IEnumerable<TickerViewModel> OrderTicker(this ObservableCollection<TickerViewModel> persent)
        {
            return persent.OrderBy(p => p.CurrencyPair);
        }

        public static bool IsTimeFrameChecked(this ChartTimeFrame frame, int value)
        {
            return frame == (ChartTimeFrame)value;
        }

        public static string RefreshTimeFormatter(this DateTime? lastRefresh)
        {
            return lastRefresh.HasValue ? $"Refresh ({lastRefresh:HH:mm})" : "Refresh";
        }

        public static string DateTimeShort(this DateTime date)
        {
            return date.ToString("dd.MM HH:mm");
        }

        public static SolidColorBrush SellTypeColor(this string type)
        {
            return type == "sell" ? new SolidColorBrush { Color = Color.FromArgb(255, 218, 54, 86) } : new SolidColorBrush { Color = Color.FromArgb(255, 83, 126, 247) };
        }
    }
}
