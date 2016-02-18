using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TaxonomiaWeb.Utils
{
    public static class Utilerias
    {
        public static DataGridColumn FindColumnByName(ObservableCollection<DataGridColumn> col, string name)
        {
            return col.SingleOrDefault(p => ((string)p.Header).Equals(name));
        }

        /// <summary>
        /// Metodo para encontrar el elemento padre de todos los padres de FrameworkElement 
        /// </summary>
        /// <param name="element"></param>
        /// <returns>T</returns>
        public static T GetParentOf<T>(FrameworkElement element) where T : FrameworkElement
        {
            while (element != null)
            {
                T item = element as T;
                if (item != null)
                {
                    return item;
                }

                element = (FrameworkElement)VisualTreeHelper.GetParent(element);
            }

            return null;
        }

        /// <summary>
        /// Metodo para encontrar el elemento padre inmediato de FrameworkElement 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="targetUIType"></param>
        /// <returns></returns>
        public static FrameworkElement GetElementParent(FrameworkElement element, Type targetUIType)
        {
            object elementParent = element.Parent;
            if (elementParent != null)
            {
                //Check the ElementParaent is of the targetUIType or no (In this case the Target Type is DataGridCell)
                if (elementParent.GetType() == targetUIType)
                {
                    return elementParent as FrameworkElement;
                }
            }
            return null;
        }


        public static string tabSpaces(int numberTab)
        {
            string tabs = string.Empty;
            while (numberTab > 0)
            {
                tabs += "\t";
                numberTab += -1;
            }
            return tabs;
        }

        public static int countTabs(string str)
        {
            return str.Count(ch => ch == '\t');
        }
    }
}
