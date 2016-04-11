using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;

namespace TaxonomiaWeb.Utils
{
    public class SharedEvents
    {

        #region Funciones de cada celda
        private bool IsNumberKey(Key inKey)
        {

            if (inKey < Key.D0 || inKey > Key.D9)
            {
                if (inKey < Key.NumPad0 || inKey > Key.NumPad9)
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsBooleanKey(Key inKey)
        {

            if (inKey == Key.S || inKey == Key.I || inKey == Key.N || inKey == Key.O)
            {
                return true;
            }
            return false;
        }
        private bool IsActionKey(Key inKey)
        {
            return inKey == Key.Enter || inKey == Key.Delete || inKey == Key.Back || inKey == Key.Tab || Keyboard.Modifiers.HasFlag(ModifierKeys.Alt) || inKey == Key.Left || inKey == Key.Right || inKey == Key.Up || inKey == Key.Down || inKey == Key.Subtract;
        }

        private bool IsDecimalActionKey(Key inKey, int platformKeyCode)
        {
            //PlatformKeyCode = para el valor . en Windows es 190 en Mac es 47
            return inKey == Key.Enter || inKey == Key.Delete || inKey == Key.Back || inKey == Key.Tab || Keyboard.Modifiers.HasFlag(ModifierKeys.Alt) || inKey == Key.Left || inKey == Key.Right || inKey == Key.Up || inKey == Key.Down || inKey == Key.Subtract || inKey == Key.Decimal || platformKeyCode == 190 || platformKeyCode == 47;
        }

        private bool IsBooleanActionKey(Key inKey)
        {
            return inKey == Key.Enter || inKey == Key.Delete || inKey == Key.Back || inKey == Key.Tab || Keyboard.Modifiers.HasFlag(ModifierKeys.Alt) || inKey == Key.Left || inKey == Key.Right || inKey == Key.Up || inKey == Key.Down;
        }

        private string LeaveOnlyNumbers(String inString)
        {
            String tmp = inString;
            foreach (char c in inString.ToCharArray())
            {
                if (IsDigit(c) == false && c.Equals("-") == true)
                {
                    tmp = tmp.Replace(c.ToString(), "");
                }
            }
            return tmp;
        }

        private string LeaveOnlyDecimals(String inString)
        {
            String tmp = inString;
            char[] arrayChars = inString.ToCharArray();
            int cantPoint = 0;
            for (int i = 0; i <= arrayChars.Length - 1; i++)
            {
                char c = arrayChars[i];
                if (IsDigit(c) == false)
                {
                    if (c.Equals('-') == true)
                    {
                        if (tmp.Count(x => x == '-') > 1 || i != 0)
                        {
                            tmp = tmp.Remove(i, 1);
                        }
                    }
                    else if (c.Equals('.') == true)
                    {
                        if ((tmp.Count(x => x == '.') > 1 || i == 0) && cantPoint > 0)
                        {
                            tmp = tmp.Remove(i, 1);
                        }
                        cantPoint += 1;
                    }
                    else
                    {
                        tmp = tmp.Replace(c.ToString(), "");
                    }

                }
            }
            if (tmp.Length == 2 && tmp.Contains("-") && tmp.Contains("."))
            {
                tmp = tmp.Replace(".", "").Replace("-", "");
            }
            else if (tmp.Length == 1 && tmp.Contains("."))
            {
                tmp = tmp.Replace(".", "");
            }

            return tmp;
        }

        private string LeaveOnlyBoolean(String inString)
        {
            String tmp = inString;

            if (tmp.ToUpper().StartsWith("S") || tmp.ToUpper().StartsWith("N"))
            {
                if (tmp.Length == 1 || tmp.Length > 2)
                {
                    string subs = tmp.ToUpper();
                    subs = subs.Substring(0, 1);
                    if (subs.Contains("S"))
                    {
                        tmp = "SI";
                    }
                    else if (subs.Contains("N"))
                    {
                        tmp = "NO";
                    }
                    return tmp;
                }
                else if (tmp.Length == 2 && tmp.ToUpper().Contains("S"))
                {
                    if (tmp.ToUpper().EndsWith("I"))
                    {
                        return tmp;
                    }
                    else 
                    {
                        tmp = tmp.Remove(1, 1);
                    }
                 
                }
                else if (tmp.Length == 2 && tmp.ToUpper().Contains("N"))
                {
                    if (tmp.ToUpper().EndsWith("O"))
                    {
                        return tmp;
                    }
                    else
                    {
                        tmp = tmp.Remove(1, 1);
                    }

                }
                else 
                {
                    if (tmp.Length > 0)
                    {
                        tmp = tmp.Remove(0, 1);
                    }
                }
            }
            else
            {
                if (tmp.Length > 0)
                {
                    tmp = tmp.Remove(0, 1);
                }
            }
            return tmp;
        }


        private bool IsDigit(char c)
        {
            return (c >= '0' && c <= '9');
        }

        public void NumericOnCellKeyDown(object sender, KeyEventArgs e)
        {

            if (Keyboard.Modifiers == ModifierKeys.Shift)
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = IsNumberKey(e.Key) == false && IsActionKey(e.Key) == false;
            }

        }

        public void NumericOnCellTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            string value = LeaveOnlyNumbers(txt.Text);
            int n;
            bool isNumeric = int.TryParse(value, out n);
            txt.Text = isNumeric ? Convert.ToString(n) : value;
        }

        public void NumericDecimalOnCellKeyDown(object sender, KeyEventArgs e)
        {

            if (Keyboard.Modifiers == ModifierKeys.Shift)
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = IsNumberKey(e.Key) == false && IsDecimalActionKey(e.Key, e.PlatformKeyCode) == false;
            }

        }

        public void NumericDecimalOnCellTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            string value = LeaveOnlyDecimals(txt.Text);
            int n;
            bool isNumeric = int.TryParse(value, out n);
            txt.Text = isNumeric ? Convert.ToString(n) : value;
        }

        public void BooleanOnCellKeyDown(object sender, KeyEventArgs e)
        {

            if (Keyboard.Modifiers == ModifierKeys.Shift)
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = IsBooleanKey(e.Key) == false && IsBooleanActionKey(e.Key) == false;
            }

        }

        public void BooleanOnCellTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            string value = LeaveOnlyBoolean(txt.Text);
            //int n;
            //bool isNumeric = int.TryParse(value, out n);
            txt.Text = value;
        }
        #endregion
    }
}
