using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using TaxonomiaWeb.ServiceBmvXblr;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using TaxonomiaWeb.Utils;
using System.Text.RegularExpressions;

namespace TaxonomiaWeb.Forms
{
    public partial class Page800005 : Page
    {
        private List<string> listHiddenColumns = null;
        private Dictionary<int, List<int>> listTotal = null;
        private Service1Client servBmvXblr = null;
        private ObservableCollection<Bmv800005> listaBmv = null;
        private ObservableCollection<Bmv800005> listaBmvAgrupada = null;
        private ObservableCollection<Bmv800005> listaBmvElementosEliminados = null;
        private const int IDENTIFICADOR_FILA_SUMA = -1;
        private MainPage mainPage = null;

        public Page800005()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            mainPage = (MainPage)Application.Current.RootVisual;
            try
            {
                string parameterPageName = string.Empty;
                string trimestre = string.Empty;
                string ano = string.Empty;
                string compania = string.Empty;

                if (NavigationContext.QueryString.TryGetValue("trim", out trimestre))
                {
                    mainPage.NumTrimestre = Int32.Parse(trimestre);
                }
                if (NavigationContext.QueryString.TryGetValue("ejerc", out ano))
                {
                    mainPage.IdAno = Int32.Parse(ano);
                }
                if (NavigationContext.QueryString.TryGetValue("comp", out compania))
                {
                    mainPage.Compania = compania;
                }
                if (NavigationContext.QueryString.TryGetValue("name", out parameterPageName))
                {
                    Uri uri = new Uri(string.Format("/Home?trim={0}&ejerc={1}&comp={2}", mainPage.NumTrimestre.ToString(), mainPage.IdAno.ToString(), mainPage.Compania), UriKind.Relative);
                    mainPage.actualizarTituloContenidos(parameterPageName, "Regresar", uri);
                }
            }
            catch (Exception ex)
            {
                ChildWindow errorWin = new ErrorWindow("URL no válida", "La URL especificada no es válida. Se direccionará a la página de inicio");
                errorWin.Show();
                mainPage.navegarPagina(new Uri("/Inicio", UriKind.Relative));
            }
        }


        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            fillHiddenColumns();
            servBmvXblr = new Service1Client();
            servBmvXblr.GetBmv800005Completed += servBmvXblr_GetBmv800005Completed;
            servBmvXblr.GetBmv800005Async(mainPage.NumTrimestre, mainPage.IdAno);
            //Agregamos los manejadores de eventos del datagrid
            //Se dispara cuando se comienza a editar una celda
            this.DgvTaxo.PreparingCellForEdit += DgvTaxo_PreparingCellForEdit;
            //Se dispara cuando se cargan las filas en el datagrid
            this.DgvTaxo.LoadingRow += DgvTaxo_LoadingRow;
            //Para que con un solo click o con el teclado entre en modo editar
            this.DgvTaxo.CurrentCellChanged += DgvTaxo_CurrentCellChanged;

            this.DgvTaxo.LayoutUpdated += DgvTaxo_LayoutUpdated;

        }

        void DgvTaxo_LayoutUpdated(object sender, EventArgs e)
        {
            DataGrid grid = this.DgvTaxo;
            if (grid != null)
            {
                ObservableCollection<DataGridColumn> listColumns = grid.Columns;
                foreach (var item in listColumns)
                {

                    string headerName = Regex.Replace(item.Header == null ? "" : item.Header.ToString(), @"\s+", "");
                    //Orden de la columnas mostradas
                    switch (headerName)
                    {
                        case AppConsts.COL_DESCRIPCION:

                            item.DisplayIndex = 2;
                            break;

                        case AppConsts.COL_PRINCIPALESMARCAS:
                            item.DisplayIndex = 3;
                            break;

                        case AppConsts.COL_PRINCIPALESPRODUCTOSOLINEADEPRODUCTOS:
                            item.DisplayIndex = 4;
                            break;

                        case AppConsts.COL_INGRESOSNACIONALES:
                            item.DisplayIndex = 5;
                            break;

                        case AppConsts.COL_INGRESOSPOREXPORTACION:
                            item.DisplayIndex = 6;
                            break;

                        case AppConsts.COL_INGRESOSDESUBSIDIARIASENELEXTRANJERO:
                            item.DisplayIndex = 7;
                            break;

                        case AppConsts.COL_INGRESOSTOTALES:
                            item.DisplayIndex = 8;
                            break;
                        default:
                            break;
                    }
                }
            }
        }


        #region Eventos del Datagrid

        private void DgvTaxo_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            //Solo buscamos las columnas visibles, las ocultas las ignoramos. 
            if (listHiddenColumns != null && listHiddenColumns.Contains(e.PropertyName) == false)
            {
                //Checamos que sea una columna tipo DataGridTextColumn
                DataGridTextColumn dgColumn = e.Column as DataGridTextColumn;
                if (dgColumn != null)
                {
                    DataGrid grid = sender as DataGrid;
                    grid.FrozenColumnCount = 3;
                    Style elementStyle = new Style(typeof(TextBlock));
                    elementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
                    Style editingElmentStyle = new Style(typeof(TextBox));
                    editingElmentStyle.Setters.Add(new Setter(TextBox.TextWrappingProperty, TextWrapping.Wrap));
                    editingElmentStyle.Setters.Add(new Setter(TextBox.AcceptsReturnProperty, true));
                    string headerName = string.Concat(e.Column.Header.ToString().Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');
                    dgColumn.EditingElementStyle = editingElmentStyle;
                    dgColumn.ElementStyle = elementStyle;
                    dgColumn.Header = headerName;
                    //Orden de la columnas mostradas
                    switch (e.PropertyName)
                    {
                        case AppConsts.COL_DESCRIPCION:

                            dgColumn.Width = DataGridLength.SizeToCells;
                            break;

                        case AppConsts.COL_PRINCIPALESMARCAS:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;

                        case AppConsts.COL_PRINCIPALESPRODUCTOSOLINEADEPRODUCTOS:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;

                        case AppConsts.COL_INGRESOSNACIONALES:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;

                        case AppConsts.COL_INGRESOSPOREXPORTACION:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;

                        case AppConsts.COL_INGRESOSDESUBSIDIARIASENELEXTRANJERO:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;

                        case AppConsts.COL_INGRESOSTOTALES:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;
                    }

                    if (e.PropertyType == typeof(double))
                    {
                        DataGridBoundColumn obj = e.Column as DataGridBoundColumn;
                        if (obj != null && obj.Binding != null)
                        {
                            obj.Binding.StringFormat = "{0:n0}";
                        }
                    }
                }
            }
            //En caso de que sea una columna oculta, pues la ocultamos.
            else
            {
                hiddenColumn(e.Column);
            }

        }


        void DgvTaxo_CurrentCellChanged(object sender, EventArgs e)
        {

            DgvTaxo.BeginEdit();
        }


        private void DgvTaxo_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            try
            {
                DataGridRow dgr = e.Row;
                System.Reflection.PropertyInfo[] listProp = typeof(Bmv800005).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                foreach (var prop in listProp)
                {
                    DataGridColumn column = DgvTaxo.Columns.SingleOrDefault(x => Regex.Replace(x.Header == null ? "" : x.Header.ToString(), @"\s+", "").Equals(prop.Name));
                    if (column != null)
                    {
                        Bmv800005 row = dgr.DataContext as Bmv800005;
                        FrameworkElement cellElement = column.GetCellContent(dgr);
                        FrameworkElement eleCell = Utilerias.GetElementParent(cellElement, typeof(DataGridCell));
                        DataGridCell dgCell = (DataGridCell)eleCell;
                        //Necesario para ajustar los renglones de cada celda y no crezcan estas mismas.
                        if (cellElement != null)
                        {
                            cellElement.SizeChanged += CellElement_SizeChanged;
                        }
                        if (row.Lectura == true)
                        {
                            //Ocultamos las celdas donde no se debe de editar nada

                            dgCell.IsEnabled = false;
                            dgCell.Foreground = new SolidColorBrush(Colors.Transparent);
                            dgCell.Opacity = 0.2;
                        }
                        else
                        {
                            dgCell.IsEnabled = true;
                            dgCell.Foreground = new SolidColorBrush(Colors.Black);
                            dgCell.Opacity = 1;
                        }

                    }
                }
                DataGridColumn columnDesc = DgvTaxo.Columns.SingleOrDefault(x => Regex.Replace(x.Header == null ? "" : x.Header.ToString(), @"\s+", "").Equals(AppConsts.COL_DESCRIPCION));
                if (columnDesc != null)
                {
                    Bmv800005 row = dgr.DataContext as Bmv800005;
                    FrameworkElement cellElement = columnDesc.GetCellContent(dgr);
                    FrameworkElement eleCell = Utilerias.GetElementParent(cellElement, typeof(DataGridCell));
                    DataGridCell dgCell = (DataGridCell)eleCell;
                    //Agregamos en negrita la sinopsis
                    if (row.Lectura == true)
                    {

                        dgCell.FontWeight = FontWeights.Bold;
                    }
                    //Agregamos en cursiva los totales
                    else if ((listTotal != null && listTotal.ContainsKey(row.IdTaxonomiaDetalle) == true))
                    {
                        dgCell.FontStyle = FontStyles.Italic;
                    }

                }

            }
            catch (Exception ex)
            {


            }
        }

        private void CellElement_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //Fix issue: DataGrid Row Auto-Resize only grows row height but won't shrink
            var dataGrid = Utilerias.GetParentOf<DataGrid>((FrameworkElement)sender);
            if (dataGrid != null && double.IsNaN(dataGrid.RowHeight))
            {
                var row = DataGridRow.GetRowContainingElement((FrameworkElement)sender);

                //Fore recalculating row height
                try
                {
                    dataGrid.RowHeight = 0;
                    row.InvalidateMeasure();
                    row.Measure(row.RenderSize);
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    //Restore RowHeight
                    dataGrid.RowHeight = double.NaN;
                }
            }
        }


        private void DgvTaxo_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            try
            {


                Bmv800005 row = e.Row.DataContext as Bmv800005;
                TextBox textBox = (e.EditingElement as TextBox);
                //Si la celda a editar es lectura o es la columna de descripcion o es un total no se podra editar.
                if (row.Lectura == true || e.Column.Header.ToString().Equals(AppConsts.COL_DESCRIPCION) == true || (listTotal != null && listTotal.ContainsKey(row.IdTaxonomiaDetalle) == true) || row.IdentificadorFila == IDENTIFICADOR_FILA_SUMA)
                {
                    //textBox.Text = "";
                    textBox.IsReadOnly = true;
                    textBox.Background = new SolidColorBrush(Colors.Gray);
                }
                else
                {
                    DataGridColumn dgc = DgvTaxo.CurrentColumn;
                    string headerName = Regex.Replace(dgc.Header == null ? "" : dgc.Header.ToString(), @"\s+", "");
                    //Dependiendo del formato de captura se aplica la plantilla
                    switch (row.FormatoCampo)
                    {
                        case AppConsts.FORMAT_TEXTBLOCK:
                        case AppConsts.FORMAT_TEXT:
                            break;
                        case AppConsts.FORMAT_YYMMDD:
                            break;
                        case AppConsts.FORMAT_X:
                        case AppConsts.FORMAT_X_NEGATIVE:
                        case AppConsts.FORMAT_XXX:
                        case AppConsts.FORMAT_SHARES:
                        case AppConsts.FORMAT_SUM:
                            //Excepciones las columnas que son tipo string
                            switch (headerName)
                            {
                                case AppConsts.COL_PRINCIPALESMARCAS:
                                case AppConsts.COL_PRINCIPALESPRODUCTOSOLINEADEPRODUCTOS:
                                    break;
                                default:
                                    textBox.KeyDown += NumericOnCellKeyDown;
                                    textBox.TextChanged += NumericOnCellTextChanged;
                                    break;
                            }
                            break;
                    }
                    // Binding customBinding = new Binding(e.Column.Header.ToString());
                    // customBinding.Source = row;
                    // customBinding.Mode = BindingMode.TwoWay;
                    // customBinding.ConverterParameter = "C2";
                    // customBinding.Converter = new NumberConverter();
                    // textBox.SetBinding(TextBox.TextProperty, customBinding);
                }
            }
            catch (Exception ex)
            {


            }

        }

        private void DataGridAddRowClick(object sender, RoutedEventArgs e)
        {
            var ctl = e.OriginalSource as Button;
            if (ctl != null)
            {
                var row = ctl.DataContext as Bmv800005;
                var bmv = copyBaseEntity(row);
                    var dataGridRow = DataGridRow.GetRowContainingElement(sender as FrameworkElement);
                    int index = dataGridRow.GetIndex();
                    if (listaBmvAgrupada != null)
                    {
                        //Agregamos el evento de cuadno la celda cambia y lo agregamos al datagrid
                        bmv.PropertyChanged += new PropertyChangedEventHandler(bmv_PropertyChanged);
                        listaBmvAgrupada.Insert(index + 1, bmv);
                    }
            }
        }

        private void DataGridRemoveRowClick(object sender, RoutedEventArgs e)
        {
            var ctl = e.OriginalSource as Button;
            if (ctl != null)
            {
                var row = ctl.DataContext as Bmv800005;
                if (row != null)
                {
                    var dataGridRow = DataGridRow.GetRowContainingElement(sender as FrameworkElement);
                    int index = dataGridRow.GetIndex();
                    if (listaBmvAgrupada != null)
                    {
                        var cantidadRegistros = (from l in listaBmvAgrupada where row.IdTaxonomiaDetalle == l.IdTaxonomiaDetalle && l.IdentificadorFila != IDENTIFICADOR_FILA_SUMA select l).Count();
                        if (cantidadRegistros == 1)
                        {
                            MessageBox.Show("Al menos un registro de este tipo debe de informarse");
                        }
                        else
                        {
                            if (listaBmvElementosEliminados == null)
                            {
                                listaBmvElementosEliminados = new ObservableCollection<Bmv800005>();
                            }
                            listaBmvElementosEliminados.Add(row);
                            listaBmvAgrupada.RemoveAt(index);
                            actualizarRegistroTotalCampoDinamico(row);
                        }

                    }


                }
            }
        }
        #endregion

        #region Eventos de otros controles

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (validarDatos() == true)
            {
                //Generamos nueva lista con los valores actualizados
                ObservableCollection<ReporteDetalle> sortedList = sortReport(listaBmvAgrupada, listaBmv);
                servBmvXblr = new Service1Client();
                servBmvXblr.SaveDinamicoBmvReporteCompleted += servBmvXblr_SaveDinamicoBmvReporteAsync;
                servBmvXblr.SaveDinamicoBmvReporteAsync(sortedList, mainPage.Compania, mainPage.IdAno, mainPage.NumTrimestre);
                busyIndicator.IsBusy = true;
            }
        }
        void BtnExportarExcel_Click(object sender, RoutedEventArgs e)
        {
            string nameFile = Utilerias.ExportarDataGrid(DgvTaxo);
            if (nameFile != null && nameFile.Length > 0)
            {
                MessageBox.Show(string.Format("Archivo {0} exportado correctamente", nameFile));
            }
            else if (nameFile == null)
            {
                MessageBox.Show("Hubo un error al exportar el reporte");
            }
        }
        #endregion
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

        private bool IsActionKey(Key inKey)
        {
            return inKey == Key.Enter || inKey == Key.Delete || inKey == Key.Back || inKey == Key.Tab || Keyboard.Modifiers.HasFlag(ModifierKeys.Alt) || inKey == Key.Left || inKey == Key.Right || inKey == Key.Up || inKey == Key.Down || inKey == Key.Subtract;
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


        public bool IsDigit(char c)
        {
            return (c >= '0' && c <= '9');
        }

        protected void NumericOnCellKeyDown(object sender, KeyEventArgs e)
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

        protected void NumericOnCellTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            string value = LeaveOnlyNumbers(txt.Text);
            int n;
            bool isNumeric = int.TryParse(value, out n);
            txt.Text = isNumeric ? Convert.ToString(n) : value;
        }
        #endregion


        #region Llamadas a servicios asincronos WCF
        void servBmvXblr_GetBmv800005Completed(object sender, GetBmv800005CompletedEventArgs e)
        {
            if (e.Result != null)
            {
                listaBmv = e.Result;
                listaBmvAgrupada = bindAndGroupList(listaBmv);
                listaBmvAgrupada = generarTotalesCamposDinamicos(listaBmvAgrupada);

                var orderby = listaBmvAgrupada.OrderBy(x => x.Orden).ThenByDescending(x => x.IdentificadorFila);
                listaBmvAgrupada = new ObservableCollection<Bmv800005>(orderby);
                foreach (var item in listaBmvAgrupada)
                {
                    item.PropertyChanged += new PropertyChangedEventHandler(bmv_PropertyChanged);
                }
                DgvTaxo.ItemsSource = listaBmvAgrupada;
            }
        }

        private string getTituloTotalDescripcion(string descripcion)
        {
            int totalTabs = Utilerias.countTabs(descripcion);
            return Utilerias.tabSpaces(totalTabs) + "Total " + descripcion.Trim();
        }

        private ObservableCollection<Bmv800005> generarTotalesCamposDinamicos(ObservableCollection<Bmv800005> listaBmvAgrupada)
        {
           ObservableCollection<Bmv800005> listaBmvAgrupadaConTotales = new ObservableCollection<Bmv800005>();

            var tempListaBmvAgrupada = from element in listaBmv
                                       group element by element.IdTaxonomiaDetalle into groups
                                       select groups.OrderBy(p => p.IdReporte).First();
            if (tempListaBmvAgrupada != null && tempListaBmvAgrupada.Any() == true)
            {
                foreach (var itemAgrupado in tempListaBmvAgrupada)
                {

                    var itemsBmv = from o in listaBmvAgrupada
                                   where o.IdTaxonomiaDetalle == itemAgrupado.IdTaxonomiaDetalle && string.IsNullOrEmpty(o.FormatoCampo) == false && o.CampoDinamico == true
                                   && o.IdentificadorFila != IDENTIFICADOR_FILA_SUMA
                                   group o by o.IdentificadorFila into groups
                                   select groups.First();
                    var bmvTotal = (from o in listaBmvAgrupada
                                    where o.IdTaxonomiaDetalle == itemAgrupado.IdTaxonomiaDetalle && string.IsNullOrEmpty(o.FormatoCampo) == false && o.CampoDinamico == true
                                    && o.IdentificadorFila == IDENTIFICADOR_FILA_SUMA
                                    group o by o.IdentificadorFila into groups
                                    select groups.First()).SingleOrDefault();

                    bool isTieneTotal = bmvTotal != null ? true : false;
                    if (itemsBmv != null && itemsBmv.Any() == true)
                    {
                        if (bmvTotal != null)
                        {
                            bmvTotal.Descripcion = getTituloTotalDescripcion(bmvTotal.Descripcion);
                            bmvTotal.CampoDinamico = false;
                        }
                        else
                        {
                        //Obtenemos cualquier registro para hacer una copia del nuevo registro
                        var bmvCopia = itemsBmv.FirstOrDefault();
                        bmvTotal.IngresosDeSubsidiariasEnElExtranjero = itemsBmv.Sum(c => c.IngresosDeSubsidiariasEnElExtranjero);
                        bmvTotal.IngresosNacionales = itemsBmv.Sum(c => c.IngresosNacionales);
                        bmvTotal.IngresosPorExportacion = itemsBmv.Sum(c => c.IngresosPorExportacion);
                        bmvTotal.IngresosTotales = itemsBmv.Sum(c => c.IngresosTotales);
                        bmvTotal.PrincipalesMarcas = "";
                        bmvTotal.PrincipalesProductosOLineaDeProductos = "";
                        bmvTotal.AtributoColumna = bmvCopia.AtributoColumna;
                        bmvTotal.CampoDinamico = false;
                        bmvTotal.Contenido = bmvCopia.Contenido;
                        bmvTotal.Descripcion = getTituloTotalDescripcion(bmvCopia.Descripcion);
                        bmvTotal.FormatoCampo = bmvCopia.FormatoCampo;
                        bmvTotal.IdReporte = bmvCopia.IdReporte;
                        bmvTotal.IdReporteDetalle = null;
                        bmvTotal.IdTaxonomiaDetalle = bmvCopia.IdTaxonomiaDetalle;
                        //Necesario para que distingamos que es un campo de tipo suma
                        bmvTotal.IdentificadorFila = IDENTIFICADOR_FILA_SUMA;
                        bmvTotal.Lectura = false;
                        bmvTotal.Orden = bmvCopia.Orden;
                        bmvTotal.Valor = "";
                        }
                        foreach (var item in itemsBmv)
                        {
                            if (isTieneTotal == false)
                            {
                                item.IdentificadorFila = 0;
                            }
                            listaBmvAgrupadaConTotales.Add(item);
                        }
                        listaBmvAgrupadaConTotales.Add(bmvTotal);
                    }
                    else
                    {
                        //Si ya contiene totales de la bd pero no tiene registro predeterminado se crea uno.
                        if (isTieneTotal == true)
                        {
                            var bmvCopia = copyBaseEntity(bmvTotal);
                            bmvCopia.IdentificadorFila = 0;
                            listaBmvAgrupadaConTotales.Add(bmvCopia);
                            bmvTotal.Descripcion = getTituloTotalDescripcion(bmvTotal.Descripcion);
                            bmvTotal.CampoDinamico = false;
                            listaBmvAgrupadaConTotales.Add(bmvTotal);
                        }
                        //Si no tiene totales es otro registro que no es campo dinamico, por lo tanto se agrega.
                        else
                        {
                            listaBmvAgrupadaConTotales.Add(itemAgrupado);
                        }
                    }
                }
            }

            return listaBmvAgrupadaConTotales;
        }


        void servBmvXblr_SaveDinamicoBmvReporteAsync(object sender, SaveDinamicoBmvReporteCompletedEventArgs e)
        {
            busyIndicator.IsBusy = false;
            if (e.Error == null)
            {
                bool res = (bool)e.Result;
                if (res == true)
                {
                    MessageBox.Show("Datos almacenados correctamente");
                    //Para que se vuelvan a acomodar los renglones y columnas, cargamos de nuevo todo
                    MainPage m = (MainPage)Application.Current.RootVisual;
                    m.ContentFrame.Refresh();
                }
                else
                {
                    MessageBox.Show("Ocurrio un error al guardar los datos");
                }
            }
            else
            {
                MessageBox.Show(string.Format("Ocurrio un error al guardar los datos {0}", e.Error.Message));
            }
        }
        #endregion

        #region Evento cuando la celda cambia
        private void bmv_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var user = sender as Bmv800005;
            Bmv800005 result = listaBmvAgrupada.SingleOrDefault(s => s == user);
            System.Reflection.PropertyInfo[] listProp = typeof(Bmv800005).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            if (result != null)
            {
                result.IngresosTotales = result.IngresosNacionales + result.IngresosPorExportacion + result.IngresosDeSubsidiariasEnElExtranjero;
                actualizarRegistroTotalCampoDinamico(result);

                if (listTotal != null)
                {
                    foreach (KeyValuePair<int, List<int>> value in listTotal)
                    {
                        List<int> list = value.Value as List<int>;
                        if (list != null)
                        {
                            foreach (int order in list)
                            {
                                //Si contiene order en lista
                                if (order == result.IdTaxonomiaDetalle)
                                {

                                    var filteredList = from o in listaBmvAgrupada
                                                       where list.Contains(o.IdTaxonomiaDetalle)
                                                       select o;
                                    long subtotalPositivo = 0;
                                    long subtotalNegativo = 0;
                                    if (filteredList != null)
                                    {
                                        subtotalPositivo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == false).Sum(x => Convert.ToInt32(x.IngresosNacionales));
                                        subtotalNegativo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == true).Sum(x => Convert.ToInt32(x.IngresosNacionales));
                                        //Actualizamos el campo
                                        foreach (var u in listaBmv.Where(u => u.IdTaxonomiaDetalle == value.Key))
                                        {
                                            u.IngresosNacionales = subtotalPositivo - subtotalNegativo;
                                        }
                                    }

                                }

                            }
                        }
                    }
                }

            }
        }

        #endregion

        #region Metodos de clase

        private void fillHiddenColumns()
        {
            System.Reflection.PropertyInfo[] listProp = typeof(BmvBase).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            listHiddenColumns = new List<string>();
            foreach (var prop in listProp)
            {
                if (prop.Name.Equals(AppConsts.COL_DESCRIPCION) == false)
                {
                    listHiddenColumns.Add(prop.Name);
                }
            }
        }

        private void hiddenColumn(DataGridColumn dgColumn)
        {
            bool res = false;
            res = listHiddenColumns.Contains(dgColumn.Header.ToString());
            if (res == true)
            {
                dgColumn.Visibility = Visibility.Collapsed;
            }
        }

        private void actualizarRegistroTotalCampoDinamico(Bmv800005 bmv)
        {
            if (listaBmvAgrupada != null && listaBmvAgrupada.Any() == true)
            {
                var listTaxonomiaDetalle = from o in listaBmvAgrupada
                                           where bmv.IdTaxonomiaDetalle == o.IdTaxonomiaDetalle && string.IsNullOrEmpty(o.FormatoCampo) == false && o.CampoDinamico == true
                                           select o;
                //Obtenemos el registro total de el campo dinamico
                var bmvTotal = (from o in listaBmvAgrupada
                                where bmv.IdTaxonomiaDetalle == o.IdTaxonomiaDetalle && string.IsNullOrEmpty(o.FormatoCampo) == false && o.IdentificadorFila == IDENTIFICADOR_FILA_SUMA
                                select o).SingleOrDefault();
                if (listTaxonomiaDetalle != null && listTaxonomiaDetalle.Any() == true && bmvTotal != null)
                {
                    bmvTotal.IngresosDeSubsidiariasEnElExtranjero = listTaxonomiaDetalle.Sum(c => c.IngresosDeSubsidiariasEnElExtranjero);
                    bmvTotal.IngresosNacionales = listTaxonomiaDetalle.Sum(c => c.IngresosNacionales);
                    bmvTotal.IngresosPorExportacion = listTaxonomiaDetalle.Sum(c => c.IngresosPorExportacion);
                    bmvTotal.IngresosTotales = listTaxonomiaDetalle.Sum(c => c.IngresosTotales);
                }
            }
        }

        private ObservableCollection<Bmv800005> bindAndGroupList(ObservableCollection<Bmv800005> listaBmv)
        {
            //Agrupamos por registro de taxonomia detalle y obtenemos solo un registro
            var tempListaBmvAgrupada = from element in listaBmv
                                       group element by element.IdTaxonomiaDetalle into groups
                                       select groups.OrderBy(p => p.IdReporte).First();
            ObservableCollection<Bmv800005> listaBmvAgrupada = new ObservableCollection<Bmv800005>(tempListaBmvAgrupada);
            //Agregamos los registros que son de la misma categoria y que sean campo dinamico
            foreach (var itemAgrupado in tempListaBmvAgrupada)
            {
                if (string.IsNullOrEmpty(itemAgrupado.FormatoCampo) == false)
                {
                    var itemsBmv = from o in listaBmv
                                   where o.IdTaxonomiaDetalle == itemAgrupado.IdTaxonomiaDetalle && o.IdentificadorFila != itemAgrupado.IdentificadorFila && string.IsNullOrEmpty(o.FormatoCampo) == false && o.CampoDinamico == true
                                   group o by o.IdentificadorFila into groups
                                   select groups.First();
                    foreach (var subItems in itemsBmv)
                    {
                        listaBmvAgrupada.Add(subItems);
                    }
                }
            }


            //Actualizamos cada campo del registro
            foreach (var itemAgrupado in listaBmvAgrupada)
            {
                if (string.IsNullOrEmpty(itemAgrupado.FormatoCampo) == false)
                {
                    var itemsBmv = from o in listaBmv
                                   where o.IdTaxonomiaDetalle == itemAgrupado.IdTaxonomiaDetalle && o.IdentificadorFila == itemAgrupado.IdentificadorFila
                                   select o;
                    foreach (var subItems in itemsBmv)
                    {
                        switch (subItems.AtributoColumna)
                        {
                            case AppConsts.COL_PRINCIPALESMARCAS:
                                itemAgrupado.PrincipalesMarcas = string.IsNullOrEmpty(subItems.Valor) == true ? "" : subItems.Valor;
                                break;

                            case AppConsts.COL_PRINCIPALESPRODUCTOSOLINEADEPRODUCTOS:
                                itemAgrupado.PrincipalesProductosOLineaDeProductos = string.IsNullOrEmpty(subItems.Valor) == true ? "" : subItems.Valor;
                                break;

                            case AppConsts.COL_INGRESOSNACIONALES:
                                itemAgrupado.IngresosNacionales = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;

                            case AppConsts.COL_INGRESOSPOREXPORTACION:
                                itemAgrupado.IngresosPorExportacion = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;

                            case AppConsts.COL_INGRESOSDESUBSIDIARIASENELEXTRANJERO:
                                itemAgrupado.IngresosDeSubsidiariasEnElExtranjero = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;

                            case AppConsts.COL_INGRESOSTOTALES:
                                itemAgrupado.IngresosTotales = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;
                            default:
                                break;
                        }
                    }
                }

            }

            return listaBmvAgrupada;
        }


        private ObservableCollection<ReporteDetalle> sortReport(ObservableCollection<Bmv800005> listaBmvAgrupada, ObservableCollection<Bmv800005> listaBmv)
        {
            ObservableCollection<ReporteDetalle> sortedList = new ObservableCollection<ReporteDetalle>();
            foreach (var itemAgrupado in listaBmvAgrupada)
            {
                if (string.IsNullOrEmpty(itemAgrupado.FormatoCampo) == false)
                {
                    int idFila = itemAgrupado.IdentificadorFila.HasValue == true ? itemAgrupado.IdentificadorFila.Value : 0;
                    if (idFila != IDENTIFICADOR_FILA_SUMA)
                    {
                        if (itemAgrupado.IdReporteDetalle == null)
                        {
                            var maxvalue = listaBmvAgrupada.Where(x => x.IdTaxonomiaDetalle == itemAgrupado.IdTaxonomiaDetalle).OrderByDescending(i => i.IdentificadorFila).FirstOrDefault();
                            itemAgrupado.IdentificadorFila = maxvalue != null ? (maxvalue.IdentificadorFila + 1) : 1;
                        }

                    }
                    IEnumerable<Bmv800005> itemsBmv = null;
                    if (itemAgrupado.IdentificadorFila.HasValue == true && itemAgrupado.IdReporteDetalle != null)
                    {
                        itemsBmv = from o in listaBmv
                                   where o.IdTaxonomiaDetalle == itemAgrupado.IdTaxonomiaDetalle
                                   && o.IdentificadorFila == itemAgrupado.IdentificadorFila
                                   group o by o.IdReporte into groups
                                   select groups.First();
                    }
                    else
                    {
                        itemsBmv = from o in listaBmv
                                   where o.IdTaxonomiaDetalle == itemAgrupado.IdTaxonomiaDetalle
                                   group o by o.IdReporte into groups
                                   select groups.First();
                    }
                    foreach (var subItems in itemsBmv)
                    {
                        string valor = string.Empty;
                        switch (subItems.AtributoColumna)
                        {
                            case AppConsts.COL_PRINCIPALESMARCAS:
                                valor = Convert.ToString(itemAgrupado.PrincipalesMarcas);
                                break;

                            case AppConsts.COL_PRINCIPALESPRODUCTOSOLINEADEPRODUCTOS:
                                valor = Convert.ToString(itemAgrupado.PrincipalesProductosOLineaDeProductos);
                                break;

                            case AppConsts.COL_INGRESOSNACIONALES:
                                valor = Convert.ToString(itemAgrupado.IngresosNacionales);
                                break;

                            case AppConsts.COL_INGRESOSPOREXPORTACION:
                                valor = Convert.ToString(itemAgrupado.IngresosPorExportacion);
                                break;

                            case AppConsts.COL_INGRESOSDESUBSIDIARIASENELEXTRANJERO:
                                valor = Convert.ToString(itemAgrupado.IngresosDeSubsidiariasEnElExtranjero);
                                break;

                            case AppConsts.COL_INGRESOSTOTALES:
                                valor = Convert.ToString(itemAgrupado.IngresosTotales);
                                break;
                            default:
                                break;
                        }
                        ReporteDetalle rd = new ReporteDetalle();
                        rd.Valor = valor;
                        rd.FormatoCampo = subItems.FormatoCampo;
                        rd.IdReporte = subItems.IdReporte;
                        if (itemAgrupado.IdReporteDetalle != null)
                        {
                            rd.IdReporteDetalle = subItems.IdReporteDetalle;
                        }
                        else
                        {
                            rd.IdReporteDetalle = itemAgrupado.IdReporteDetalle;
                        }
                        rd.IdentificadorFila = itemAgrupado.IdentificadorFila;
                        rd.Estado = true;
                        sortedList.Add(rd);
                    }
                }
            }


            //Por ultimo anadimos los registros eliminados
            if (listaBmvElementosEliminados != null && listaBmvElementosEliminados.Any() == true)
            {
                foreach (var itemEliminado in listaBmvElementosEliminados)
                {
                    //Actualizamos los registros qeu ya estaban
                    var itemsBmv = from o in listaBmv
                                   where o.IdTaxonomiaDetalle == itemEliminado.IdTaxonomiaDetalle && o.IdentificadorFila == itemEliminado.IdentificadorFila
                                   select o;
                    if (itemsBmv.Any() == true)
                    {
                        foreach (var subItems in itemsBmv)
                        {
                            ReporteDetalle rd = new ReporteDetalle();
                            switch (subItems.AtributoColumna)
                            {
                                case AppConsts.COL_PRINCIPALESMARCAS:
                                    rd.Valor = Convert.ToString(itemEliminado.PrincipalesMarcas);
                                    break;

                                case AppConsts.COL_PRINCIPALESPRODUCTOSOLINEADEPRODUCTOS:
                                    rd.Valor = Convert.ToString(itemEliminado.PrincipalesProductosOLineaDeProductos);
                                    break;

                                case AppConsts.COL_INGRESOSNACIONALES:
                                    rd.Valor = Convert.ToString(itemEliminado.IngresosNacionales);
                                    break;

                                case AppConsts.COL_INGRESOSPOREXPORTACION:
                                    rd.Valor = Convert.ToString(itemEliminado.IngresosPorExportacion);
                                    break;

                                case AppConsts.COL_INGRESOSDESUBSIDIARIASENELEXTRANJERO:
                                    rd.Valor = Convert.ToString(itemEliminado.IngresosDeSubsidiariasEnElExtranjero);
                                    break;

                                case AppConsts.COL_INGRESOSTOTALES:
                                    rd.Valor = Convert.ToString(itemEliminado.IngresosTotales);
                                    break;
                                default:
                                    break;
                            }
                            rd.FormatoCampo = subItems.FormatoCampo;
                            rd.IdReporte = subItems.IdReporte;
                            rd.IdReporteDetalle = subItems.IdReporteDetalle;
                            rd.IdentificadorFila = subItems.IdentificadorFila;
                            rd.Estado = false;
                            sortedList.Add(rd);
                        }

                    }
                }
            }

            return sortedList;

        }

        private Bmv800005 copyBaseEntity(Bmv800005 bmv)
        {
            var entity = new Bmv800005();
            if (bmv != null)
            {
                entity.IngresosDeSubsidiariasEnElExtranjero = 0;
                entity.IngresosNacionales = 0;
                entity.IngresosPorExportacion = 0;
                entity.IngresosTotales = 0;
                entity.PrincipalesMarcas = "";
                entity.PrincipalesProductosOLineaDeProductos = "";
                entity.AtributoColumna = bmv.AtributoColumna;
                entity.CampoDinamico = bmv.CampoDinamico;
                entity.Contenido = bmv.Contenido;
                entity.Descripcion = bmv.Descripcion;
                entity.FormatoCampo = bmv.FormatoCampo;
                entity.IdReporte = bmv.IdReporte;
                entity.IdTaxonomiaDetalle = bmv.IdTaxonomiaDetalle;
                entity.Lectura = bmv.Lectura;
                entity.Orden = bmv.Orden;
                entity.Valor = "";
                entity.IdReporteDetalle = null;
            }
            return entity;
        }

        private bool validarDatos()
        {
            bool res = false;
            if (listaBmvAgrupada != null)
            {
                foreach (var renglon in listaBmvAgrupada)
                {
                    //Si son editables se tienen que validar los campos
                    if (renglon.Lectura == false)
                    {
                        //if (renglon.TrimestreActual == null)
                        //{
                        //    int indexOf = listaBmv.IndexOf(renglon);
                        //    DgvTaxo.SelectedItem = renglon;
                        //    DataGridColumn dgc = Utilerias.FindColumnByName(DgvTaxo.Columns, AppConsts.COL_TRIMESTREACTUAL);
                        //    if (dgc != null)
                        //    {
                        //        DgvTaxo.CurrentColumn = dgc;
                        //        DgvTaxo.Dispatcher.BeginInvoke(() => { DgvTaxo.ScrollIntoView(renglon, dgc); });
                        //    }
                        //    DgvTaxo.Focus();
                        //    DgvTaxo.BeginEdit();
                        //    return res;

                        //}

                    }
                }
            }
            else
            {
                return res;
            }
            res = true;
            return res;
        }

    }
        #endregion
}
