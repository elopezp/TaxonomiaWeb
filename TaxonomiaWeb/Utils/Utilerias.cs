using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace TaxonomiaWeb.Utils
{
    public static class Utilerias
    {
        public static DataGridColumn FindColumnByName(ObservableCollection<DataGridColumn> listColumns, string name, bool noSpaces)
        {
            DataGridColumn column = null;
            if (noSpaces == true)
            {
                foreach (var c in listColumns)
                {
                    if (Regex.Replace(c.Header == null ? "" : c.Header.ToString(), @"\s+", "").Equals(name))
                    {
                        column = c;
                        return column;
                    }
                }
            }
            else
            {
                column = listColumns.SingleOrDefault(p => ((string)p.Header).Equals(name));
            }
            return column;
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

        public static string ExportarDataGrid(DataGrid dGrid)
        {
            string nameFile = null;
            try
            {
                SaveFileDialog objSFD = new SaveFileDialog() { DefaultExt = "xls", Filter = "Excel Spreadsheets (*.xls)|*.xls |All files (*.*)|*.*", FilterIndex = 1 };

                if (objSFD.ShowDialog() == true)
                {
                    string strFormat = objSFD.SafeFileName.Substring(objSFD.SafeFileName.IndexOf('.') + 1).ToUpper();
                    if (strFormat.Equals("XLS"))
                    {
                        strFormat = "XML";
                    }
                    StringBuilder strBuilder = new StringBuilder();
                    if (dGrid.ItemsSource == null) return null;
                    var countColumns = (from c in dGrid.Columns
                                        select c).Count();

                    List<string> lstFields = new string[countColumns].ToList();
                    if (dGrid.HeadersVisibility == DataGridHeadersVisibility.Column || dGrid.HeadersVisibility == DataGridHeadersVisibility.All)
                    {
                        foreach (DataGridColumn dgcol in dGrid.Columns)
                        {
                            if (dgcol.Visibility == Visibility.Visible)
                            {
                                int index = dgcol.DisplayIndex;
                                if (index > -1)
                                {
                                    string header = dgcol.Header == null ? "" : dgcol.Header.ToString();
                                    if (string.IsNullOrEmpty(header) == false)
                                    {
                                        lstFields.RemoveAt(index);
                                        lstFields.Insert(index, FormatField(header, strFormat));
                                    }
                                }
                            }
                        }
                        BuildStringOfRow(strBuilder, lstFields, strFormat);
                    }
                    foreach (object data in dGrid.ItemsSource)
                    {
                        lstFields = new string[countColumns].ToList();
                        foreach (DataGridColumn col in dGrid.Columns)
                        {
                            if (col.Visibility == Visibility.Visible)
                            {
                                string strValue = "";
                                Binding objBinding = null;
                                if (col is DataGridBoundColumn)
                                    objBinding = (col as DataGridBoundColumn).Binding;
                                if (col is DataGridTemplateColumn)
                                {
                                    //This is a template column... let us see the underlying dependency object
                                    DependencyObject objDO = (col as DataGridTemplateColumn).CellTemplate.LoadContent();
                                    FrameworkElement oFE = (FrameworkElement)objDO;
                                    FieldInfo oFI = oFE.GetType().GetField("TextProperty");
                                    if (oFI != null)
                                    {
                                        if (oFI.GetValue(null) != null)
                                        {
                                            if (oFE.GetBindingExpression((DependencyProperty)oFI.GetValue(null)) != null)
                                                objBinding = oFE.GetBindingExpression((DependencyProperty)oFI.GetValue(null)).ParentBinding;
                                        }
                                    }
                                }
                                if (objBinding != null)
                                {
                                    if (objBinding.Path.Path != "")
                                    {
                                        PropertyInfo pi = data.GetType().GetProperty(objBinding.Path.Path);
                                        if (pi != null)
                                        {
                                            strValue = pi.GetValue(data, null) != null ? pi.GetValue(data, null).ToString() : "";
                                        }
                                    }
                                    if (objBinding.Converter != null)
                                    {
                                        if (strValue.Equals("") == false)
                                            strValue = objBinding.Converter.Convert(strValue, typeof(string), objBinding.ConverterParameter, objBinding.ConverterCulture).ToString();
                                        //else
                                          //  strValue = objBinding.Converter.Convert(data, typeof(string), objBinding.ConverterParameter, objBinding.ConverterCulture).ToString();
                                    }
                                }
                                int index = col.DisplayIndex;
                                if (index > -1)
                                {
                                    string header = col.Header == null ? "" : col.Header.ToString();
                                    if (string.IsNullOrEmpty(header) == false)
                                    {
                                        lstFields.RemoveAt(index);
                                        lstFields.Insert(index, FormatField(strValue, strFormat));
                                       
                                    }
                                }

                            }
                        }
                        BuildStringOfRow(strBuilder, lstFields, strFormat);
                    }
                    StreamWriter sw = new StreamWriter(objSFD.OpenFile());
                    if (strFormat == "XML")
                    {
                        //Let us write the headers for the Excel XML
                        sw.WriteLine("<?xml version=\"1.0\" " +
                                     "encoding=\"utf-8\"?>");
                        sw.WriteLine("<?mso-application progid" +
                                     "=\"Excel.Sheet\"?>");
                        sw.WriteLine("<Workbook xmlns=\"urn:" +
                                     "schemas-microsoft-com:office:spreadsheet\">");
                        sw.WriteLine("<DocumentProperties " +
                                     "xmlns=\"urn:schemas-microsoft-com:" +
                                     "office:office\">");
                        sw.WriteLine("<Author>Fernandez Editores</Author>");
                        sw.WriteLine("<Created>" +
                                     DateTime.Now.ToLocalTime().ToLongDateString() +
                                     "</Created>");
                        sw.WriteLine("<LastSaved>" +
                                     DateTime.Now.ToLocalTime().ToLongDateString() +
                                     "</LastSaved>");
                        sw.WriteLine("<Company>Fernandez Editores</Company>");
                        sw.WriteLine("<Version>12.00</Version>");
                        sw.WriteLine("</DocumentProperties>");
                        sw.WriteLine("<Worksheet ss:Name=\"BMV\" " +
                           "xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\">");
                        sw.WriteLine("<Table>");
                    }
                    sw.Write(strBuilder.ToString());
                    if (strFormat == "XML")
                    {
                        sw.WriteLine("</Table>");
                        sw.WriteLine("</Worksheet>");
                        sw.WriteLine("</Workbook>");
                    }
                    sw.Close();
                    nameFile = objSFD.SafeFileName;

                }
                else 
                {
                  nameFile = "";
                }
            }
            catch (Exception ex)
            {
 
            }
            return nameFile;
        }

        private static void BuildStringOfRow(StringBuilder strBuilder,
        List<string> lstFields, string strFormat)
        {
            switch (strFormat)
            {
                case "XML":
                    strBuilder.AppendLine("<Row>");
                    strBuilder.AppendLine(String.Join("\r\n", lstFields.ToArray()));
                    strBuilder.AppendLine("</Row>");
                    break;
                case "CSV":
                    strBuilder.AppendLine(String.Join(",", lstFields.ToArray()));
                    break;
            }
        }

        private static string FormatField(string data, string format)
        {
            switch (format)
            {
                case "XML":
                    return String.Format("<Cell><Data ss:Type=\"String" +
                       "\">{0}</Data></Cell>", data);
                case "CSV":
                    return String.Format("\"{0}\"",
                      data.Replace("\"", "\"\"\"").Replace("\n",
                      "").Replace("\r", ""));
            }
            return data;
        }


    }
}
