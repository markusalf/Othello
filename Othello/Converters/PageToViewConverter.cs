using Othello.Enums;
using Othello.Views;
using Othello.Views.Pages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Othello.Converters
{
    
        internal class PageToViewConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is Page && value != null)
                {
                    var Page = (Page)value;
                    return Page switch
                    {
                        Page.Start => new StartPage(),
                        Page.Rules => new RulesPage(),
                        Page.Game => new GameView(),
                        Page.End => new EndPage(),

                        _ => new StartPage(),
                    };
                }
                return new StartPage();
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }

