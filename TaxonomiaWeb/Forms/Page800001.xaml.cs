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
using System.Windows.Data;

namespace TaxonomiaWeb.Forms
{
    public partial class Page800001 : Page
    {
        private List<string> listHiddenColumns = null;
        private Dictionary<int, List<int>> listTotal = null;
        private Service1Client servBmvXblr = null;
        private ObservableCollection<Bmv800001> listaBmv = null;
        private ObservableCollection<Bmv800001> listaBmvAgrupada = null;
        private ObservableCollection<Bmv800001> listaBmvElementosEliminados = null;
        private ObservableCollection<BmvDetalleSuma> listBmvSuma = null;
        private const int IDENTIFICADOR_FILA_SUMA = -1;
        private const string TOTAL_INSTITUCION = "TOTAL";
        private MainPage mainPage = null;

        public Page800001()
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
            if (listaBmvElementosEliminados == null)
            {
                listaBmvElementosEliminados = new ObservableCollection<Bmv800001>();
            }
            fillHiddenColumns();
            servBmvXblr = new Service1Client();
            servBmvXblr.GetBmv800001Completed += servBmvXblr_GetBmv800001Completed;
            servBmvXblr.GetBmv800001Async(mainPage.NumTrimestre, mainPage.IdAno);
            //Agregamos los manejadores de eventos del datagrid
            //Se dispara cuando se comienza a editar una celda
            this.DgvTaxo.PreparingCellForEdit += DgvTaxo_PreparingCellForEdit;
            //Se dispara cuando se cargan las filas en el datagrid
            this.DgvTaxo.LoadingRow += DgvTaxo_LoadingRow;
            //Para que con un solo click o con el teclado entre en modo editar
            this.DgvTaxo.CurrentCellChanged += DgvTaxo_CurrentCellChanged;

            this.DgvTaxo.LayoutUpdated += DgvTaxo_LayoutUpdated;

            this.DgvTaxo.CellEditEnded += DgvTaxo_CellEditEnded;

        }

        void DgvTaxo_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            LblError.Text = "";
        }

        void DgvTaxo_LayoutUpdated(object sender, EventArgs e)
        {
            DataGrid grid = this.DgvTaxo;
            if (grid != null)
            {
                grid.FrozenColumnCount = 3;
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

                        case AppConsts.COL_INSTITUCION:
                            item.DisplayIndex = 3;
                            break;

                        case AppConsts.COL_INSTITUCIONEXTRANJERASINO:
                            item.DisplayIndex = 4;
                            break;

                        case AppConsts.COL_FECHADEFIRMACONTRATO:
                            item.DisplayIndex = 5;
                            break;

                        case AppConsts.COL_FECHADEVENCIMIENTO:
                            item.DisplayIndex = 6;
                            break;

                        case AppConsts.COL_TASADEINTERESYOSOBRETASA:
                            item.DisplayIndex = 7;
                            break;

                        case AppConsts.COL_MONEDANACIONALANOACTUAL:
                            item.DisplayIndex = 8;
                            break;

                        case AppConsts.COL_MONEDANACIONALHASTA1ANO:
                            item.DisplayIndex = 9;
                            break;

                        case AppConsts.COL_MONEDANACIONALHASTA2ANOS:
                            item.DisplayIndex = 10;
                            break;

                        case AppConsts.COL_MONEDANACIONALHASTA3ANOS:
                            item.DisplayIndex = 11;
                            break;

                        case AppConsts.COL_MONEDANACIONALHASTA4ANOS:
                            item.DisplayIndex = 12;
                            break;

                        case AppConsts.COL_MONEDANACIONALHASTA5ANOSOMAS:
                            item.DisplayIndex = 13;
                            break;

                        case AppConsts.COL_MONEDAEXTRANJERAANOACTUAL:
                            item.DisplayIndex = 14;
                            break;

                        case AppConsts.COL_MONEDAEXTRANJERAHASTA1ANO:
                            item.DisplayIndex = 15;
                            break;
                        case AppConsts.COL_MONEDAEXTRANJERAHASTA2ANOS:
                            item.DisplayIndex = 16;
                            break;

                        case AppConsts.COL_MONEDAEXTRANJERAHASTA3ANOS:
                            item.DisplayIndex = 17;
                            break;

                        case AppConsts.COL_MONEDAEXTRANJERAHASTA4ANOS:
                            item.DisplayIndex = 18;
                            break;

                        case AppConsts.COL_MONEDAEXTRANJERAHASTA5ANOSOMAS:
                            item.DisplayIndex = 19;
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
                            dgColumn.MaxWidth = AppConsts.MAXWIDTH_COL_DESCRIPCION;
                            break;

                        case AppConsts.COL_INSTITUCION:
                            dgColumn.Width = new DataGridLength(250, DataGridLengthUnitType.Pixel);
                            break;

                        case AppConsts.COL_INSTITUCIONEXTRANJERASINO:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;

                        case AppConsts.COL_FECHADEFIRMACONTRATO:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;

                        case AppConsts.COL_FECHADEVENCIMIENTO:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;

                        case AppConsts.COL_TASADEINTERESYOSOBRETASA:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;

                        case AppConsts.COL_MONEDANACIONALANOACTUAL:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;

                        case AppConsts.COL_MONEDANACIONALHASTA1ANO:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;

                        case AppConsts.COL_MONEDANACIONALHASTA2ANOS:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;

                        case AppConsts.COL_MONEDANACIONALHASTA3ANOS:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;

                        case AppConsts.COL_MONEDANACIONALHASTA4ANOS:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;

                        case AppConsts.COL_MONEDANACIONALHASTA5ANOSOMAS:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;

                        case AppConsts.COL_MONEDAEXTRANJERAANOACTUAL:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;

                        case AppConsts.COL_MONEDAEXTRANJERAHASTA1ANO:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;
                        case AppConsts.COL_MONEDAEXTRANJERAHASTA2ANOS:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;

                        case AppConsts.COL_MONEDAEXTRANJERAHASTA3ANOS:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;

                        case AppConsts.COL_MONEDAEXTRANJERAHASTA4ANOS:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;

                        case AppConsts.COL_MONEDAEXTRANJERAHASTA5ANOSOMAS:
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

                    if (e.PropertyType == typeof(DateTime?))
                    {
                        DataGridBoundColumn obj = e.Column as DataGridBoundColumn;
                        if (obj != null && obj.Binding != null)
                        {
                            obj.Binding.StringFormat = "{0:yyyy-MM-dd}";
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
                System.Reflection.PropertyInfo[] listProp = typeof(Bmv800001).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                Bmv800001 row = dgr.DataContext as Bmv800001;
                foreach (var prop in listProp)
                {
                    DataGridColumn column = DgvTaxo.Columns.SingleOrDefault(x => Regex.Replace(x.Header == null ? "" : x.Header.ToString(), @"\s+", "").Equals(prop.Name));
                    if (column != null)
                    {
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
                            if (cellElement.GetType() == typeof(CheckBox))
                            {
                                CheckBox ch = (CheckBox)cellElement;
                                ch.IsEnabled = false;

                            }
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
                        row.Institucion = "";
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


                Bmv800001 row = e.Row.DataContext as Bmv800001;
                TextBox textBox = (e.EditingElement as TextBox);

                if (textBox != null)
                {
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
                        SharedEvents se = new SharedEvents();
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
                                    case AppConsts.COL_INSTITUCION:
                                    case AppConsts.COL_FECHADEFIRMACONTRATO:
                                    case AppConsts.COL_FECHADEVENCIMIENTO:
                                        break;
                                    default:
                                        switch (row.FormatoCampo)
                                        {
                                            case AppConsts.FORMAT_X:
                                            case AppConsts.FORMAT_X_NEGATIVE:
                                            case AppConsts.FORMAT_SHARES:
                                            case AppConsts.FORMAT_SUM:
                                                textBox.KeyDown += se.NumericOnCellKeyDown;
                                                textBox.TextChanged += se.NumericOnCellTextChanged;
                                                break;
                                            case AppConsts.FORMAT_XXX:
                                                textBox.KeyDown += se.NumericDecimalOnCellKeyDown;
                                                textBox.TextChanged += se.NumericDecimalOnCellTextChanged;
                                                break;
                                        }
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
                var row = ctl.DataContext as Bmv800001;
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
                var row = ctl.DataContext as Bmv800001;
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

        #region Llamadas a servicios asincronos WCF
        void servBmvXblr_GetBmv800001Completed(object sender, GetBmv800001CompletedEventArgs e)
        {
            if (e.Result != null)
            {
                listaBmv = e.Result;
                listaBmvAgrupada = bindAndGroupList(listaBmv);
                listaBmvAgrupada = generarTotalesCamposDinamicos(listaBmvAgrupada);

                var orderby = listaBmvAgrupada.OrderBy(x => x.Orden).ThenByDescending(x => x.IdentificadorFila);
                listaBmvAgrupada = new ObservableCollection<Bmv800001>(orderby);
                foreach (var item in listaBmvAgrupada)
                {
                    item.PropertyChanged += new PropertyChangedEventHandler(bmv_PropertyChanged);
                }
                servBmvXblr = new Service1Client();
                servBmvXblr.GetBmvDetalleSumaCompleted += servBmvXblr_GetBmvDetalleSumaCompleted;
                servBmvXblr.GetBmvDetalleSumaAsync("800001");

            }
        }


        void servBmvXblr_GetBmvDetalleSumaCompleted(object sender, GetBmvDetalleSumaCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                listBmvSuma = e.Result;
                listTotal = (from p in listBmvSuma
                             group p.IdTaxonomiaHijo by p.IdTaxonomiaPadre into g
                             select new { IdPadre = g.Key, ListaIdHijos = g.ToList() }).ToDictionary(p => p.IdPadre, p => p.ListaIdHijos);
                DgvTaxo.ItemsSource = listaBmvAgrupada;

            }
        }

        private string getTituloTotalDescripcion(string descripcion)
        {
            int totalTabs = Utilerias.countTabs(descripcion);
            return Utilerias.tabSpaces(totalTabs) + "Total " + descripcion.Trim();
        }

        private ObservableCollection<Bmv800001> generarTotalesCamposDinamicos(ObservableCollection<Bmv800001> listaBmvAgrupada)
        {
            ObservableCollection<Bmv800001> listaBmvAgrupadaConTotales = new ObservableCollection<Bmv800001>();

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
                            bmvTotal.Institucion = "";
                            bmvTotal.CampoDinamico = false;
                        }
                        else
                        {
                            //Obtenemos cualquier registro para hacer una copia del nuevo registro
                            var bmvCopia = itemsBmv.FirstOrDefault();
                            bmvTotal = new Bmv800001();
                            bmvTotal.MonedaExtranjeraAnoActual = itemsBmv.Sum(c => c.MonedaExtranjeraAnoActual);
                            bmvTotal.MonedaExtranjeraHasta1Ano = itemsBmv.Sum(c => c.MonedaExtranjeraHasta1Ano); ;
                            bmvTotal.MonedaExtranjeraHasta2Anos = itemsBmv.Sum(c => c.MonedaExtranjeraHasta2Anos); ;
                            bmvTotal.MonedaExtranjeraHasta3Anos = itemsBmv.Sum(c => c.MonedaExtranjeraHasta3Anos); ;
                            bmvTotal.MonedaExtranjeraHasta4Anos = itemsBmv.Sum(c => c.MonedaExtranjeraHasta4Anos); ;
                            bmvTotal.MonedaExtranjeraHasta5AnosOMas = itemsBmv.Sum(c => c.MonedaExtranjeraHasta5AnosOMas); ;
                            bmvTotal.MonedaNacionalAnoActual = itemsBmv.Sum(c => c.MonedaNacionalAnoActual); ;
                            bmvTotal.MonedaNacionalHasta1Ano = itemsBmv.Sum(c => c.MonedaNacionalHasta1Ano); ;
                            bmvTotal.MonedaNacionalHasta2Anos = itemsBmv.Sum(c => c.MonedaNacionalHasta2Anos); ;
                            bmvTotal.MonedaNacionalHasta3Anos = itemsBmv.Sum(c => c.MonedaNacionalHasta3Anos); ;
                            bmvTotal.MonedaNacionalHasta4Anos = itemsBmv.Sum(c => c.MonedaNacionalHasta4Anos); ;
                            bmvTotal.MonedaNacionalHasta5AnosOMas = itemsBmv.Sum(c => c.MonedaNacionalHasta5AnosOMas);
                            bmvTotal.FechaDeFirmaContrato = null;
                            bmvTotal.FechaDeVencimiento = null;
                            bmvTotal.Institucion = "";
                            bmvTotal.InstitucionExtranjera = false;
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
                            bmvTotal.Institucion = "";
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
            var user = sender as Bmv800001;
            Bmv800001 result = listaBmvAgrupada.SingleOrDefault(s => s == user);
            System.Reflection.PropertyInfo[] listProp = typeof(Bmv800001).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            if (result != null)
            {
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
                                                       where list.Contains(o.IdTaxonomiaDetalle) && o.IdentificadorFila != IDENTIFICADOR_FILA_SUMA
                                                       select o;
                                    double subtotalPositivo = 0;
                                    double subtotalNegativo = 0;
                                    if (filteredList != null)
                                    {
                                        subtotalPositivo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == false).Sum(x => Convert.ToDouble(x.MonedaExtranjeraAnoActual));
                                        subtotalNegativo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == true).Sum(x => Convert.ToDouble(x.MonedaExtranjeraAnoActual));
                                        //Actualizamos el campo
                                        foreach (var u in listaBmv.Where(u => u.IdTaxonomiaDetalle == value.Key))
                                        {
                                            u.MonedaExtranjeraAnoActual = subtotalPositivo - subtotalNegativo;
                                        }
                                        subtotalPositivo = 0;
                                        subtotalNegativo = 0;
                                        subtotalPositivo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == false).Sum(x => Convert.ToDouble(x.MonedaExtranjeraHasta1Ano));
                                        subtotalNegativo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == true).Sum(x => Convert.ToDouble(x.MonedaExtranjeraHasta1Ano));
                                        //Actualizamos el campo
                                        foreach (var u in listaBmv.Where(u => u.IdTaxonomiaDetalle == value.Key))
                                        {
                                            u.MonedaExtranjeraHasta1Ano = subtotalPositivo - subtotalNegativo;
                                        }
                                        subtotalPositivo = 0;
                                        subtotalNegativo = 0;
                                        subtotalPositivo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == false).Sum(x => Convert.ToDouble(x.MonedaExtranjeraHasta2Anos));
                                        subtotalNegativo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == true).Sum(x => Convert.ToDouble(x.MonedaExtranjeraHasta2Anos));
                                        //Actualizamos el campo
                                        foreach (var u in listaBmv.Where(u => u.IdTaxonomiaDetalle == value.Key))
                                        {
                                            u.MonedaExtranjeraHasta2Anos = subtotalPositivo - subtotalNegativo;
                                        }
                                        subtotalPositivo = 0;
                                        subtotalNegativo = 0;
                                        subtotalPositivo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == false).Sum(x => Convert.ToDouble(x.MonedaExtranjeraHasta3Anos));
                                        subtotalNegativo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == true).Sum(x => Convert.ToDouble(x.MonedaExtranjeraHasta3Anos));
                                        //Actualizamos el campo
                                        foreach (var u in listaBmv.Where(u => u.IdTaxonomiaDetalle == value.Key))
                                        {
                                            u.MonedaExtranjeraHasta3Anos = subtotalPositivo - subtotalNegativo;
                                        }
                                        subtotalPositivo = 0;
                                        subtotalNegativo = 0;
                                        subtotalPositivo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == false).Sum(x => Convert.ToDouble(x.MonedaExtranjeraHasta4Anos));
                                        subtotalNegativo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == true).Sum(x => Convert.ToDouble(x.MonedaExtranjeraHasta4Anos));
                                        //Actualizamos el campo
                                        foreach (var u in listaBmv.Where(u => u.IdTaxonomiaDetalle == value.Key))
                                        {
                                            u.MonedaExtranjeraHasta4Anos = subtotalPositivo - subtotalNegativo;
                                        }
                                        subtotalPositivo = 0;
                                        subtotalNegativo = 0;
                                        subtotalPositivo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == false).Sum(x => Convert.ToDouble(x.MonedaExtranjeraHasta5AnosOMas));
                                        subtotalNegativo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == true).Sum(x => Convert.ToDouble(x.MonedaExtranjeraHasta5AnosOMas));
                                        //Actualizamos el campo
                                        foreach (var u in listaBmv.Where(u => u.IdTaxonomiaDetalle == value.Key))
                                        {
                                            u.MonedaExtranjeraHasta5AnosOMas = subtotalPositivo - subtotalNegativo;
                                        }
                                        subtotalPositivo = 0;
                                        subtotalNegativo = 0;
                                        subtotalPositivo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == false).Sum(x => Convert.ToDouble(x.MonedaNacionalAnoActual));
                                        subtotalNegativo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == true).Sum(x => Convert.ToDouble(x.MonedaNacionalAnoActual));
                                        //Actualizamos el campo
                                        foreach (var u in listaBmv.Where(u => u.IdTaxonomiaDetalle == value.Key))
                                        {
                                            u.MonedaNacionalAnoActual = subtotalPositivo - subtotalNegativo;
                                        }
                                        subtotalPositivo = 0;
                                        subtotalNegativo = 0;
                                        subtotalPositivo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == false).Sum(x => Convert.ToDouble(x.MonedaNacionalHasta1Ano));
                                        subtotalNegativo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == true).Sum(x => Convert.ToDouble(x.MonedaNacionalHasta1Ano));
                                        //Actualizamos el campo
                                        foreach (var u in listaBmv.Where(u => u.IdTaxonomiaDetalle == value.Key))
                                        {
                                            u.MonedaNacionalHasta1Ano = subtotalPositivo - subtotalNegativo;
                                        }
                                        subtotalPositivo = 0;
                                        subtotalNegativo = 0;
                                        subtotalPositivo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == false).Sum(x => Convert.ToDouble(x.MonedaNacionalHasta2Anos));
                                        subtotalNegativo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == true).Sum(x => Convert.ToDouble(x.MonedaNacionalHasta2Anos));
                                        //Actualizamos el campo
                                        foreach (var u in listaBmv.Where(u => u.IdTaxonomiaDetalle == value.Key))
                                        {
                                            u.MonedaNacionalHasta2Anos = subtotalPositivo - subtotalNegativo;
                                        }
                                        subtotalPositivo = 0;
                                        subtotalNegativo = 0;
                                        subtotalPositivo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == false).Sum(x => Convert.ToDouble(x.MonedaNacionalHasta3Anos));
                                        subtotalNegativo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == true).Sum(x => Convert.ToDouble(x.MonedaNacionalHasta3Anos));
                                        //Actualizamos el campo
                                        foreach (var u in listaBmv.Where(u => u.IdTaxonomiaDetalle == value.Key))
                                        {
                                            u.MonedaNacionalHasta3Anos = subtotalPositivo - subtotalNegativo;
                                        }
                                        subtotalPositivo = 0;
                                        subtotalNegativo = 0;
                                        subtotalPositivo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == false).Sum(x => Convert.ToDouble(x.MonedaNacionalHasta4Anos));
                                        subtotalNegativo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == true).Sum(x => Convert.ToDouble(x.MonedaNacionalHasta4Anos));
                                        //Actualizamos el campo
                                        foreach (var u in listaBmv.Where(u => u.IdTaxonomiaDetalle == value.Key))
                                        {
                                            u.MonedaNacionalHasta4Anos = subtotalPositivo - subtotalNegativo;
                                        }
                                        subtotalPositivo = 0;
                                        subtotalNegativo = 0;
                                        subtotalPositivo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == false).Sum(x => Convert.ToDouble(x.MonedaNacionalHasta5AnosOMas));
                                        subtotalNegativo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == true).Sum(x => Convert.ToDouble(x.MonedaNacionalHasta5AnosOMas));
                                        //Actualizamos el campo
                                        foreach (var u in listaBmv.Where(u => u.IdTaxonomiaDetalle == value.Key))
                                        {
                                            u.MonedaNacionalHasta5AnosOMas = subtotalPositivo - subtotalNegativo;
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

        private void actualizarRegistroTotalCampoDinamico(Bmv800001 bmv)
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
                    bmvTotal.MonedaExtranjeraAnoActual = listTaxonomiaDetalle.Sum(c => c.MonedaExtranjeraAnoActual);
                    bmvTotal.MonedaExtranjeraHasta1Ano = listTaxonomiaDetalle.Sum(c => c.MonedaExtranjeraHasta1Ano); ;
                    bmvTotal.MonedaExtranjeraHasta2Anos = listTaxonomiaDetalle.Sum(c => c.MonedaExtranjeraHasta2Anos); ;
                    bmvTotal.MonedaExtranjeraHasta3Anos = listTaxonomiaDetalle.Sum(c => c.MonedaExtranjeraHasta3Anos); ;
                    bmvTotal.MonedaExtranjeraHasta4Anos = listTaxonomiaDetalle.Sum(c => c.MonedaExtranjeraHasta4Anos); ;
                    bmvTotal.MonedaExtranjeraHasta5AnosOMas = listTaxonomiaDetalle.Sum(c => c.MonedaExtranjeraHasta5AnosOMas); ;
                    bmvTotal.MonedaNacionalAnoActual = listTaxonomiaDetalle.Sum(c => c.MonedaNacionalAnoActual); ;
                    bmvTotal.MonedaNacionalHasta1Ano = listTaxonomiaDetalle.Sum(c => c.MonedaNacionalHasta1Ano); ;
                    bmvTotal.MonedaNacionalHasta2Anos = listTaxonomiaDetalle.Sum(c => c.MonedaNacionalHasta2Anos); ;
                    bmvTotal.MonedaNacionalHasta3Anos = listTaxonomiaDetalle.Sum(c => c.MonedaNacionalHasta3Anos); ;
                    bmvTotal.MonedaNacionalHasta4Anos = listTaxonomiaDetalle.Sum(c => c.MonedaNacionalHasta4Anos); ;
                    bmvTotal.MonedaNacionalHasta5AnosOMas = listTaxonomiaDetalle.Sum(c => c.MonedaNacionalHasta5AnosOMas);
                }
            }
        }

        private ObservableCollection<Bmv800001> bindAndGroupList(ObservableCollection<Bmv800001> listaBmv)
        {
            //Agrupamos por registro de taxonomia detalle y obtenemos solo un registro
            var tempListaBmvAgrupada = from element in listaBmv
                                       group element by element.IdTaxonomiaDetalle into groups
                                       select groups.OrderBy(p => p.IdReporte).First();
            ObservableCollection<Bmv800001> listaBmvAgrupada = new ObservableCollection<Bmv800001>(tempListaBmvAgrupada);
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
                            case AppConsts.COL_INSTITUCION:
                                itemAgrupado.Institucion = string.IsNullOrEmpty(subItems.Valor) == true ? "" : subItems.Valor;
                                break;

                            case AppConsts.COL_INSTITUCIONEXTRANJERASINO:
                                itemAgrupado.InstitucionExtranjera = string.IsNullOrEmpty(subItems.Valor) == true ? false : Boolean.Parse(subItems.Valor);
                                break;

                            case AppConsts.COL_FECHADEFIRMACONTRATO:
                                DateTime dteResFirmaContrato = new DateTime(2015,10,23);
                                bool isDateTimeFirmaContrato = DateTime.TryParse(string.IsNullOrEmpty(subItems.Valor) == true ? "" : subItems.Valor, out dteResFirmaContrato);
                                if (isDateTimeFirmaContrato)
                                {
                                    itemAgrupado.FechaDeFirmaContrato = dteResFirmaContrato;
                                }
                                else 
                                {
                                    itemAgrupado.FechaDeFirmaContrato = null;
                                }
                               
                                break;

                            case AppConsts.COL_FECHADEVENCIMIENTO:
                                DateTime dteResFechaVencimiento = new DateTime(2015,10,23);
                                bool isDateTimeFechaVencimiento = DateTime.TryParse(string.IsNullOrEmpty(subItems.Valor) == true ? "" : subItems.Valor, out dteResFechaVencimiento);
                                if (isDateTimeFechaVencimiento)
                                {
                                    itemAgrupado.FechaDeVencimiento = dteResFechaVencimiento;
                                }
                                else
                                {
                                    itemAgrupado.FechaDeVencimiento = null;
                                }
                                break;

                            case AppConsts.COL_TASADEINTERESYOSOBRETASA:
                                itemAgrupado.TasaDeInteresYOSobreTasa = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;

                            case AppConsts.COL_MONEDANACIONALANOACTUAL:
                                itemAgrupado.MonedaNacionalAnoActual = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;

                            case AppConsts.COL_MONEDANACIONALHASTA1ANO:
                                itemAgrupado.MonedaNacionalHasta1Ano = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;

                            case AppConsts.COL_MONEDANACIONALHASTA2ANOS:
                                itemAgrupado.MonedaNacionalHasta2Anos = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;

                            case AppConsts.COL_MONEDANACIONALHASTA3ANOS:
                                itemAgrupado.MonedaNacionalHasta3Anos = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;

                            case AppConsts.COL_MONEDANACIONALHASTA4ANOS:
                                itemAgrupado.MonedaNacionalHasta4Anos = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;

                            case AppConsts.COL_MONEDANACIONALHASTA5ANOSOMAS:
                                itemAgrupado.MonedaNacionalHasta5AnosOMas = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;

                            case AppConsts.COL_MONEDAEXTRANJERAANOACTUAL:
                                itemAgrupado.MonedaExtranjeraAnoActual = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;

                            case AppConsts.COL_MONEDAEXTRANJERAHASTA1ANO:
                                itemAgrupado.MonedaExtranjeraHasta1Ano = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;
                            case AppConsts.COL_MONEDAEXTRANJERAHASTA2ANOS:
                                itemAgrupado.MonedaExtranjeraHasta2Anos = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;

                            case AppConsts.COL_MONEDAEXTRANJERAHASTA3ANOS:
                                itemAgrupado.MonedaExtranjeraHasta3Anos = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;

                            case AppConsts.COL_MONEDAEXTRANJERAHASTA4ANOS:
                                itemAgrupado.MonedaExtranjeraHasta4Anos = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;

                            case AppConsts.COL_MONEDAEXTRANJERAHASTA5ANOSOMAS:
                                itemAgrupado.MonedaExtranjeraHasta5AnosOMas = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;
                            default:
                                break;
                        }
                    }
                }

            }

            return listaBmvAgrupada;
        }


        private ObservableCollection<ReporteDetalle> sortReport(ObservableCollection<Bmv800001> listaBmvAgrupada, ObservableCollection<Bmv800001> listaBmv)
        {
            ObservableCollection<ReporteDetalle> sortedList = new ObservableCollection<ReporteDetalle>();

            ObservableCollection<Bmv800001> listaSinInstitucion = new ObservableCollection<Bmv800001>();
            //Actualizamos sus totales en caso que la institucion venga vacia con esto se ejecuta el metodo de propertychangued
            foreach (var itemAgrupado in listaBmvAgrupada.Where(x => x.CampoDinamico == true && string.IsNullOrEmpty(x.Institucion) == true))
            {
                //En caso en que la institucion venga vacio actualizamos sus valores
                itemAgrupado.FechaDeFirmaContrato = null;
                itemAgrupado.FechaDeVencimiento = null;
                itemAgrupado.Institucion = "";
                itemAgrupado.InstitucionExtranjera = false;
                itemAgrupado.MonedaExtranjeraAnoActual = 0;
                itemAgrupado.MonedaExtranjeraHasta1Ano = 0;
                itemAgrupado.MonedaExtranjeraHasta2Anos = 0;
                itemAgrupado.MonedaExtranjeraHasta3Anos = 0;
                itemAgrupado.MonedaExtranjeraHasta4Anos = 0;
                itemAgrupado.MonedaExtranjeraHasta5AnosOMas = 0;
                itemAgrupado.MonedaNacionalAnoActual = 0;
                itemAgrupado.MonedaNacionalHasta1Ano = 0;
                itemAgrupado.MonedaNacionalHasta2Anos = 0;
                itemAgrupado.MonedaNacionalHasta3Anos = 0;
                itemAgrupado.MonedaNacionalHasta4Anos = 0;
                itemAgrupado.MonedaNacionalHasta5AnosOMas = 0;
                itemAgrupado.TasaDeInteresYOSobreTasa = 0;
                
            }

            foreach (var itemAgrupado in listaBmvAgrupada.ToList())
            {
                if (itemAgrupado.CampoDinamico == true && string.IsNullOrEmpty(itemAgrupado.Institucion) == true)
                {
                    //En caso de que ya exista en la bd, se agrega en lista de eliminados para borrarlo por completo. 
                    if (itemAgrupado.IdReporteDetalle != null)
                    {
                        listaBmvElementosEliminados.Add(itemAgrupado);
                    }
                    listaBmvAgrupada.Remove(itemAgrupado);
                }
            }


            foreach (var itemAgrupado in listaBmvAgrupada)
            {
                if (string.IsNullOrEmpty(itemAgrupado.FormatoCampo) == false)
                {
                    int idFila = itemAgrupado.IdentificadorFila.HasValue == true ? itemAgrupado.IdentificadorFila.Value : 0;
                    if (idFila != IDENTIFICADOR_FILA_SUMA)
                    {
                        if (string.IsNullOrEmpty(itemAgrupado.Institucion) == false && itemAgrupado.IdReporteDetalle == null)
                        {
                            var maxvalue = listaBmvAgrupada.Where(x => x.IdTaxonomiaDetalle == itemAgrupado.IdTaxonomiaDetalle).OrderByDescending(i => i.IdentificadorFila).FirstOrDefault();
                            itemAgrupado.IdentificadorFila = maxvalue != null ? (maxvalue.IdentificadorFila + 1) : 1;
                        }

                    }
                    IEnumerable<Bmv800001> itemsBmv = null;
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
                            case AppConsts.COL_INSTITUCION:
                                valor = Convert.ToString(itemAgrupado.Institucion);
                                break;

                            case AppConsts.COL_INSTITUCIONEXTRANJERASINO:
                                valor = Convert.ToString(itemAgrupado.InstitucionExtranjera).ToLower();
                                break;

                            case AppConsts.COL_FECHADEFIRMACONTRATO:
                                valor = itemAgrupado.FechaDeFirmaContrato == null ? "" : itemAgrupado.FechaDeFirmaContrato.Value.ToString("yyyy-MM-dd");
                                break;

                            case AppConsts.COL_FECHADEVENCIMIENTO:
                                valor = itemAgrupado.FechaDeVencimiento == null ? "" : itemAgrupado.FechaDeVencimiento.Value.ToString("yyyy-MM-dd");
                                break;

                            case AppConsts.COL_TASADEINTERESYOSOBRETASA:
                                valor = Convert.ToString(itemAgrupado.TasaDeInteresYOSobreTasa);
                                break;

                            case AppConsts.COL_MONEDANACIONALANOACTUAL:
                                valor = Convert.ToString(itemAgrupado.MonedaNacionalAnoActual);
                                break;

                            case AppConsts.COL_MONEDANACIONALHASTA1ANO:
                                valor = Convert.ToString(itemAgrupado.MonedaNacionalHasta1Ano);
                                break;

                            case AppConsts.COL_MONEDANACIONALHASTA2ANOS:
                                valor = Convert.ToString(itemAgrupado.MonedaNacionalHasta2Anos);
                                break;

                            case AppConsts.COL_MONEDANACIONALHASTA3ANOS:
                                valor = Convert.ToString(itemAgrupado.MonedaNacionalHasta3Anos);
                                break;

                            case AppConsts.COL_MONEDANACIONALHASTA4ANOS:
                                valor = Convert.ToString(itemAgrupado.MonedaNacionalHasta4Anos);
                                break;

                            case AppConsts.COL_MONEDANACIONALHASTA5ANOSOMAS:
                                valor = Convert.ToString(itemAgrupado.MonedaNacionalHasta5AnosOMas);
                                break;

                            case AppConsts.COL_MONEDAEXTRANJERAANOACTUAL:
                                valor = Convert.ToString(itemAgrupado.MonedaExtranjeraAnoActual);
                                break;

                            case AppConsts.COL_MONEDAEXTRANJERAHASTA1ANO:
                                valor = Convert.ToString(itemAgrupado.MonedaExtranjeraHasta1Ano);
                                break;
                            case AppConsts.COL_MONEDAEXTRANJERAHASTA2ANOS:
                                valor = Convert.ToString(itemAgrupado.MonedaExtranjeraHasta2Anos);
                                break;

                            case AppConsts.COL_MONEDAEXTRANJERAHASTA3ANOS:
                                valor = Convert.ToString(itemAgrupado.MonedaExtranjeraHasta3Anos);
                                break;

                            case AppConsts.COL_MONEDAEXTRANJERAHASTA4ANOS:
                                valor = Convert.ToString(itemAgrupado.MonedaExtranjeraHasta4Anos);
                                break;

                            case AppConsts.COL_MONEDAEXTRANJERAHASTA5ANOSOMAS:
                                valor = Convert.ToString(itemAgrupado.MonedaExtranjeraHasta5AnosOMas);
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
                        ////Agregamos los registros totales pero solo los miembros moneda nacional y moneda extranjera.
                        ////Incluyendo los tipos totales.
                        if (itemAgrupado.IdentificadorFila == IDENTIFICADOR_FILA_SUMA || (listTotal != null && listTotal.ContainsKey(itemAgrupado.IdTaxonomiaDetalle) == true))
                        {
                            switch (subItems.AtributoColumna)
                            {
                                case AppConsts.COL_INSTITUCION:
                                    rd.Valor = TOTAL_INSTITUCION;
                                    break;
                                default:
                                    break;
                            }
                        }
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
                                case AppConsts.COL_INSTITUCION:
                                    rd.Valor = Convert.ToString(itemEliminado.Institucion);
                                    break;

                                case AppConsts.COL_INSTITUCIONEXTRANJERASINO:
                                    rd.Valor = Convert.ToString(itemEliminado.InstitucionExtranjera).ToLower();
                                    break;

                                case AppConsts.COL_FECHADEFIRMACONTRATO:
                                    rd.Valor = itemEliminado.FechaDeFirmaContrato == null ? "" : itemEliminado.FechaDeFirmaContrato.Value.ToString("yyyy-MM-dd");
                                    break;

                                case AppConsts.COL_FECHADEVENCIMIENTO:
                                    rd.Valor = itemEliminado.FechaDeVencimiento == null ? "" : itemEliminado.FechaDeVencimiento.Value.ToString("yyyy-MM-dd");
                                    break;

                                case AppConsts.COL_TASADEINTERESYOSOBRETASA:
                                    rd.Valor = Convert.ToString(itemEliminado.TasaDeInteresYOSobreTasa);
                                    break;

                                case AppConsts.COL_MONEDANACIONALANOACTUAL:
                                    rd.Valor = Convert.ToString(itemEliminado.MonedaNacionalAnoActual);
                                    break;

                                case AppConsts.COL_MONEDANACIONALHASTA1ANO:
                                    rd.Valor = Convert.ToString(itemEliminado.MonedaNacionalHasta1Ano);
                                    break;

                                case AppConsts.COL_MONEDANACIONALHASTA2ANOS:
                                    rd.Valor = Convert.ToString(itemEliminado.MonedaNacionalHasta2Anos);
                                    break;

                                case AppConsts.COL_MONEDANACIONALHASTA3ANOS:
                                    rd.Valor = Convert.ToString(itemEliminado.MonedaNacionalHasta3Anos);
                                    break;

                                case AppConsts.COL_MONEDANACIONALHASTA4ANOS:
                                    rd.Valor = Convert.ToString(itemEliminado.MonedaNacionalHasta4Anos);
                                    break;

                                case AppConsts.COL_MONEDANACIONALHASTA5ANOSOMAS:
                                    rd.Valor = Convert.ToString(itemEliminado.MonedaNacionalHasta5AnosOMas);
                                    break;

                                case AppConsts.COL_MONEDAEXTRANJERAANOACTUAL:
                                    rd.Valor = Convert.ToString(itemEliminado.MonedaExtranjeraAnoActual);
                                    break;

                                case AppConsts.COL_MONEDAEXTRANJERAHASTA1ANO:
                                    rd.Valor = Convert.ToString(itemEliminado.MonedaExtranjeraHasta1Ano);
                                    break;
                                case AppConsts.COL_MONEDAEXTRANJERAHASTA2ANOS:
                                    rd.Valor = Convert.ToString(itemEliminado.MonedaExtranjeraHasta2Anos);
                                    break;

                                case AppConsts.COL_MONEDAEXTRANJERAHASTA3ANOS:
                                    rd.Valor = Convert.ToString(itemEliminado.MonedaExtranjeraHasta3Anos);
                                    break;

                                case AppConsts.COL_MONEDAEXTRANJERAHASTA4ANOS:
                                    rd.Valor = Convert.ToString(itemEliminado.MonedaExtranjeraHasta4Anos);
                                    break;

                                case AppConsts.COL_MONEDAEXTRANJERAHASTA5ANOSOMAS:
                                    rd.Valor = Convert.ToString(itemEliminado.MonedaExtranjeraHasta5AnosOMas);
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

        private Bmv800001 copyBaseEntity(Bmv800001 bmv)
        {
                var entity = new Bmv800001();
                if (bmv != null)
                {
                    entity.FechaDeFirmaContrato = null;
                    entity.FechaDeVencimiento = null;
                    entity.Institucion = "";
                    entity.InstitucionExtranjera = false;
                    entity.MonedaExtranjeraAnoActual = 0;
                    entity.MonedaExtranjeraHasta1Ano = 0;
                    entity.MonedaExtranjeraHasta2Anos = 0;
                    entity.MonedaExtranjeraHasta3Anos = 0;
                    entity.MonedaExtranjeraHasta4Anos = 0;
                    entity.MonedaExtranjeraHasta5AnosOMas = 0;
                    entity.MonedaNacionalAnoActual = 0;
                    entity.MonedaNacionalHasta1Ano = 0;
                    entity.MonedaNacionalHasta2Anos = 0;
                    entity.MonedaNacionalHasta3Anos = 0;
                    entity.MonedaNacionalHasta4Anos = 0;
                    entity.MonedaNacionalHasta5AnosOMas = 0;
                    entity.TasaDeInteresYOSobreTasa = 0;
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
                        if (renglon.CampoDinamico == true && string.IsNullOrEmpty(renglon.Institucion) == false)
                        {
                            if (renglon.FechaDeFirmaContrato.HasValue == false)
                            {
                                    int indexOf = listaBmv.IndexOf(renglon);
                                    DgvTaxo.SelectedItem = renglon;
                                    DataGridColumn dgc = Utilerias.FindColumnByName(DgvTaxo.Columns, AppConsts.COL_FECHADEFIRMACONTRATO, true);
                                    if (dgc != null)
                                    {
                                        DgvTaxo.CurrentColumn = dgc;
                                        DgvTaxo.Dispatcher.BeginInvoke(() => { DgvTaxo.ScrollIntoView(renglon, dgc); });
                                    }
                                    DgvTaxo.Focus();
                                    DgvTaxo.BeginEdit();
                                    LblError.Text = "La institución debe tener una fecha de firma de contrato en el siguiente formato aaaa-mm-dd";
                                    return res;
                            }
                            else if(renglon.FechaDeVencimiento.HasValue == false)
                            {
                                    int indexOf = listaBmv.IndexOf(renglon);
                                    DgvTaxo.SelectedItem = renglon;
                                    DataGridColumn dgc = Utilerias.FindColumnByName(DgvTaxo.Columns, AppConsts.COL_FECHADEVENCIMIENTO, true);
                                    if (dgc != null)
                                    {
                                        DgvTaxo.CurrentColumn = dgc;
                                        DgvTaxo.Dispatcher.BeginInvoke(() => { DgvTaxo.ScrollIntoView(renglon, dgc); });
                                    }
                                    DgvTaxo.Focus();
                                    DgvTaxo.BeginEdit();
                                    LblError.Text = "La institución debe tener una fecha de vencimiento en el siguiente formato aaaa-mm-dd";
                                    return res;
                            }
                        }
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
            LblError.Text = "";
            res = true;
            return res;
        }

    }
        #endregion
}
