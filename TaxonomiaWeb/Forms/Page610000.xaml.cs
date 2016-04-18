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
    public partial class Page610000 : Page
    {
        private List<string> listHiddenColumns = null;
        private Dictionary<int, List<int>> listTotal = null;
        private Service1Client servBmvXblr = null;
        private ObservableCollection<Bmv610000> listaBmv = null;
        private ObservableCollection<Bmv610000> listaBmvAgrupada = null;
        private ObservableCollection<BmvDetalleSuma> listBmvSuma = null;
        private MainPage mainPage = null;

        public Page610000()
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
            servBmvXblr.GetBmv610000Completed += servBmvXblr_GetBmv610000Completed;
            servBmvXblr.GetBmv610000Async(mainPage.NumTrimestre, mainPage.IdAno);
            //Agregamos los manejadores de eventos del datagrid
            //Se dispara cuando se comienza a editar una celda
            this.DgvTaxo.PreparingCellForEdit += DgvTaxo_PreparingCellForEdit;
            //Se dispara cuando se cargan las filas en el datagrid
            this.DgvTaxo.LoadingRow += DgvTaxo_LoadingRow;
            //Para que con un solo click o con el teclado entre en modo editar
            this.DgvTaxo.CurrentCellChanged += DgvTaxo_CurrentCellChanged;

            this.DgvTaxo.LayoutUpdated += DgvTaxo_LayoutUpdated;
        }


        #region Eventos del Datagrid

        void DgvTaxo_LayoutUpdated(object sender, EventArgs e)
        {
            DataGrid grid = this.DgvTaxo;
            if (grid != null)
            {
                grid.FrozenColumnCount = 1;
                ObservableCollection<DataGridColumn> listColumns = grid.Columns;
                foreach (var item in listColumns)
                {

                    string headerName = Regex.Replace(item.Header == null ? "" : item.Header.ToString(), @"\s+", "");
                    //Orden de la columnas mostradas
                    switch (headerName)
                    {
                        case AppConsts.COL_DESCRIPCION:
                            item.DisplayIndex = 0;
                            break;
                        case AppConsts.COL_CAPITALSOCIAL:
                            item.DisplayIndex = 1;
                            break;
                        case AppConsts.COL_PRIMAENEMISIONDEACCIONES:
                            item.DisplayIndex = 2;
                            break;
                        case AppConsts.COL_ACCIONESENTESORERIA:
                            item.DisplayIndex = 3;
                            break;
                        case AppConsts.COL_UTILIDADESACUMULADAS:
                            item.DisplayIndex = 4;
                            break;
                        case AppConsts.COL_SUPERAVITDEREVALUACION:
                            item.DisplayIndex = 5;
                            break;
                        case AppConsts.COL_EFECTOPORCONVERSION:
                            item.DisplayIndex = 6;
                            break;
                        case AppConsts.COL_COBERTURASDEFLUJOSDEEFECTIVO:
                            item.DisplayIndex = 7;
                            break;
                        case AppConsts.COL_UTILIDADPERDIDAENINSTRUMENTOSDECOBERTURAQUECUBRENINVERSIONESENINSTRUMENTOSDECAPITAL:
                            item.DisplayIndex = 8;
                            break;
                        case AppConsts.COL_VARIACIONENELVALORTEMPORALDELASOPCIONES:
                            item.DisplayIndex = 9;
                            break;
                        case AppConsts.COL_VARIACIONENELVALORDECONTRATOSAFUTURO:
                            item.DisplayIndex = 10;
                            break;
                        case AppConsts.COL_VARIACIONENELVALORDEMÁRGENESCONBASEENMONEDAEXTRANJERA:
                            item.DisplayIndex = 11;
                            break;
                        case AppConsts.COL_UTILIDADPERDIDAPORCAMBIOSENVALORRAZONABLEDEACTIVOSFINANCIEROSDISPONIBLESPARALAVENTA:
                            item.DisplayIndex = 12;
                            break;
                        case AppConsts.COL_PAGOSBASADOSENACCIONES:
                            item.DisplayIndex = 13;
                            break;
                        case AppConsts.COL_NUEVASMEDICIONESDEPLANESDEBENEFICIOSDEFINIDOS:
                            item.DisplayIndex = 14;
                            break;
                        case AppConsts.COL_IMPORTESRECONOCIDOSENOTRORESULTADOINTEGRALYACUMULADOSENELCAPITALCONTABLERELATIVOSAACTIVOSNOCORRIENTESOGRUPOSDEACTIVOSPARASUDISPOSICIONMANTENIDOSPARALAVENTA:
                            item.DisplayIndex = 15;
                            break;
                        case AppConsts.COL_UTILIDADPERDIDAPORINVERSIONESENINSTRUMENTOSDECAPITAL:
                            item.DisplayIndex = 16;
                            break;
                        case AppConsts.COL_RESERVAPARACAMBIOSENELVALORRAZONABLEDEPASIVOSFINANCIEROSATRIBUIBLESACAMBIOSENELRIESGODECREDITODEPASIVO:
                            item.DisplayIndex = 17;
                            break;
                        case AppConsts.COL_RESERVAPARACATASTROFES:
                            item.DisplayIndex = 18;
                            break;
                        case AppConsts.COL_RESERVAPARAESTABILIZACION:
                            item.DisplayIndex = 19;
                            break;
                        case AppConsts.COL_RESERVADECOMPONENTESDEPARTICIPACIONDISCRECIONAL:
                            item.DisplayIndex = 20;
                            break;
                        case AppConsts.COL_OTROSRESULTADOSINTEGRALES:
                            item.DisplayIndex = 21;
                            break;
                        case AppConsts.COL_OTROSRESULTADOSINTEGRALESACUMULADOS:
                            item.DisplayIndex = 22;
                            break;
                        case AppConsts.COL_CAPITALCONTABLEDELAPARTICIPACIONCONTROLADORA:
                            item.DisplayIndex = 23;
                            break;
                        case AppConsts.COL_PARTICIPACIONNOCONTROLADORA:
                            item.DisplayIndex = 24;
                            break;
                        case AppConsts.COL_CAPITALCONTABLE:
                            item.DisplayIndex = 25;
                            break;

                        default:
                            break;
                    }
                }
            }
        }


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
                        case AppConsts.COL_CAPITALSOCIAL:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;
                        case AppConsts.COL_PRIMAENEMISIONDEACCIONES:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;
                        case AppConsts.COL_ACCIONESENTESORERIA:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;
                        case AppConsts.COL_UTILIDADESACUMULADAS:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;
                        case AppConsts.COL_SUPERAVITDEREVALUACION:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;
                        case AppConsts.COL_EFECTOPORCONVERSION:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;
                        case AppConsts.COL_COBERTURASDEFLUJOSDEEFECTIVO:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;
                        case AppConsts.COL_UTILIDADPERDIDAENINSTRUMENTOSDECOBERTURAQUECUBRENINVERSIONESENINSTRUMENTOSDECAPITAL:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;
                        case AppConsts.COL_VARIACIONENELVALORTEMPORALDELASOPCIONES:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;
                        case AppConsts.COL_VARIACIONENELVALORDECONTRATOSAFUTURO:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;
                        case AppConsts.COL_VARIACIONENELVALORDEMÁRGENESCONBASEENMONEDAEXTRANJERA:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;
                        case AppConsts.COL_UTILIDADPERDIDAPORCAMBIOSENVALORRAZONABLEDEACTIVOSFINANCIEROSDISPONIBLESPARALAVENTA:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;
                        case AppConsts.COL_PAGOSBASADOSENACCIONES:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;
                        case AppConsts.COL_NUEVASMEDICIONESDEPLANESDEBENEFICIOSDEFINIDOS:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;
                        case AppConsts.COL_IMPORTESRECONOCIDOSENOTRORESULTADOINTEGRALYACUMULADOSENELCAPITALCONTABLERELATIVOSAACTIVOSNOCORRIENTESOGRUPOSDEACTIVOSPARASUDISPOSICIONMANTENIDOSPARALAVENTA:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;
                        case AppConsts.COL_UTILIDADPERDIDAPORINVERSIONESENINSTRUMENTOSDECAPITAL:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;
                        case AppConsts.COL_RESERVAPARACAMBIOSENELVALORRAZONABLEDEPASIVOSFINANCIEROSATRIBUIBLESACAMBIOSENELRIESGODECREDITODEPASIVO:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;
                        case AppConsts.COL_RESERVAPARACATASTROFES:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;
                        case AppConsts.COL_RESERVAPARAESTABILIZACION:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;
                        case AppConsts.COL_RESERVADECOMPONENTESDEPARTICIPACIONDISCRECIONAL:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;
                        case AppConsts.COL_OTROSRESULTADOSINTEGRALES:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;
                        case AppConsts.COL_OTROSRESULTADOSINTEGRALESACUMULADOS:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;
                        case AppConsts.COL_CAPITALCONTABLEDELAPARTICIPACIONCONTROLADORA:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;
                        case AppConsts.COL_PARTICIPACIONNOCONTROLADORA:
                            dgColumn.Width = DataGridLength.SizeToHeader;
                            break;
                        case AppConsts.COL_CAPITALCONTABLE:
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
                System.Reflection.PropertyInfo[] listProp = typeof(Bmv610000).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                foreach (var prop in listProp)
                {
                    DataGridColumn column = DgvTaxo.Columns.SingleOrDefault(x => Regex.Replace(x.Header.ToString(), @"\s+", "").Equals(prop.Name));
                    if (column != null)
                    {
                        Bmv610000 row = dgr.DataContext as Bmv610000;
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
                DataGridColumn columnDesc = DgvTaxo.Columns.SingleOrDefault(x => Regex.Replace(x.Header.ToString(), @"\s+", "").Equals(AppConsts.COL_DESCRIPCION));
                if (columnDesc != null)
                {
                    Bmv610000 row = dgr.DataContext as Bmv610000;
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
                Bmv610000 row = e.Row.DataContext as Bmv610000;
                TextBox textBox = (e.EditingElement as TextBox);
                //Si la celda a editar es lectura o es la columna de descripcion o es un total no se podra editar.
                if (row.Lectura == true || e.Column.Header.ToString().Equals(AppConsts.COL_DESCRIPCION) == true || (listTotal != null && listTotal.ContainsKey(row.IdTaxonomiaDetalle) == true))
                {
                    //textBox.Text = "";
                    textBox.IsReadOnly = true;
                    textBox.Background = new SolidColorBrush(Colors.Gray);
                }
                else
                {
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
                        case AppConsts.FORMAT_SHARES:
                        case AppConsts.FORMAT_SUM:
                            textBox.KeyDown += se.NumericOnCellKeyDown;
                            textBox.TextChanged += se.NumericOnCellTextChanged;
                            break;
                        case AppConsts.FORMAT_XXX:
                            textBox.KeyDown += se.NumericDecimalOnCellKeyDown;
                            textBox.TextChanged += se.NumericDecimalOnCellTextChanged;
                            break;
                        default:
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
        #endregion

        #region Eventos de otros controles

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (validarDatos() == true)
            {
                //Generamos nueva lista con los valores actualizados
                ObservableCollection<ReporteDetalle> sortedList = sortReport(listaBmvAgrupada, listaBmv);
                servBmvXblr = new Service1Client();
                servBmvXblr.SaveBmvReporteCompleted += servBmvXblr_SaveBmvReporteCompleted;
                servBmvXblr.SaveBmvReporteAsync(sortedList, mainPage.Compania,  mainPage.IdAno, mainPage.NumTrimestre);
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
        void servBmvXblr_GetBmv610000Completed(object sender, GetBmv610000CompletedEventArgs e)
        {
            if (e.Result != null)
            {
                listaBmv = e.Result;
                listaBmvAgrupada = bindAndGroupList(listaBmv);
                foreach (var item in listaBmvAgrupada)
                {
                    item.PropertyChanged += new PropertyChangedEventHandler(bmv_PropertyChanged);
                }
                servBmvXblr = new Service1Client();
                servBmvXblr.GetBmvDetalleSumaCompleted += servBmvXblr_GetBmvDetalleSumaCompleted;
                servBmvXblr.GetBmvDetalleSumaAsync("610000");
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


        void servBmvXblr_SaveBmvReporteCompleted(object sender, SaveBmvReporteCompletedEventArgs e)
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
            var user = sender as Bmv610000;
            Bmv610000 result = listaBmvAgrupada.SingleOrDefault(s => s == user);
            System.Reflection.PropertyInfo[] listProp = typeof(Bmv610000).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            if (result != null)
            {
                //Suma de totales por columna dentro de la misma fila
                    result.OtrosResultadosIntegralesAcumulados = result.SuperavitDeRevaluacion + result.EfectoPorConversion + result.CoberturasDeFlujosDeEfectivo + 
                        result.UtilidadPerdidaEnInstrumentosDeCoberturaQueCubrenInversionesEnInstrumentosDeCapital + result.VariacionEnElValorTemporalDeLasOpciones + result.VariacionEnElValorDeContratosAFuturo + 
                        result.VariacionEnElValorDeMárgenesConBaseEnMonedaExtranjera + result.UtilidadPerdidaPorCambiosEnValorRazonableDeActivosFinancierosDisponiblesParaLaVenta + result.PagosBasadosEnAcciones + 
                        result.NuevasMedicionesDePlanesDeBeneficiosDefinidos + result.ImportesReconocidosEnOtroResultadoIntegralYAcumuladosEnElCapitalContableRelativosAActivosNoCorrientesOGruposDeActivosParaSuDisposicionMantenidosParaLaVenta + 
                        result.UtilidadPerdidaPorInversionesEnInstrumentosDeCapital + result.ReservaParaCambiosEnElValorRazonableDePasivosFinancierosAtribuiblesACambiosEnElRiesgoDeCreditoDePasivo +
                        result.ReservaParaCatastrofes + result.ReservaParaEstabilizacion + result.ReservaDeComponentesDeParticipacionDiscrecional + result.OtrosResultadosIntegrales;
                    result.CapitalContableDeLaParticipacionControladora = result.CapitalSocial + result.PrimaEnEmisionDeAcciones + result.AccionesEnTesoreria + result.UtilidadesAcumuladas + result.OtrosResultadosIntegralesAcumulados;
                    result.CapitalContable = result.CapitalContableDeLaParticipacionControladora + result.ParticipacionNoControladora;

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
                                double subtotalPositivo = 0;
                                double subtotalNegativo = 0;
                                if (filteredList != null)
                                {
                                    subtotalPositivo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == false).Sum(x => Convert.ToDouble(x.AccionesEnTesoreria));
                                    subtotalNegativo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == true).Sum(x => Convert.ToDouble(x.AccionesEnTesoreria));
                                    //Actualizamos el campo
                                    foreach (var u in listaBmv.Where(u => u.IdTaxonomiaDetalle == value.Key))
                                    {
                                        u.AccionesEnTesoreria = subtotalPositivo - subtotalNegativo;
                                    }
                                    subtotalPositivo = 0;
                                    subtotalNegativo = 0;
                                    subtotalPositivo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == false).Sum(x => Convert.ToDouble(x.CapitalContable));
                                    subtotalNegativo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == true).Sum(x => Convert.ToDouble(x.CapitalContable));
                                    //Actualizamos el campo
                                    foreach (var u in listaBmv.Where(u => u.IdTaxonomiaDetalle == value.Key))
                                    {
                                        u.CapitalContable = subtotalPositivo - subtotalNegativo;
                                    }
                                    subtotalPositivo = 0;
                                    subtotalNegativo = 0;
                                    subtotalPositivo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == false).Sum(x => Convert.ToDouble(x.CapitalContableDeLaParticipacionControladora));
                                    subtotalNegativo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == true).Sum(x => Convert.ToDouble(x.CapitalContableDeLaParticipacionControladora));
                                    //Actualizamos el campo
                                    foreach (var u in listaBmv.Where(u => u.IdTaxonomiaDetalle == value.Key))
                                    {
                                        u.CapitalContableDeLaParticipacionControladora = subtotalPositivo - subtotalNegativo;
                                    }
                                    subtotalPositivo = 0;
                                    subtotalNegativo = 0;
                                    subtotalPositivo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == false).Sum(x => Convert.ToDouble(x.CapitalSocial));
                                    subtotalNegativo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == true).Sum(x => Convert.ToDouble(x.CapitalSocial));
                                    //Actualizamos el campo
                                    foreach (var u in listaBmv.Where(u => u.IdTaxonomiaDetalle == value.Key))
                                    {
                                        u.CapitalSocial = subtotalPositivo - subtotalNegativo;
                                    }
                                    subtotalPositivo = 0;
                                    subtotalNegativo = 0;
                                    subtotalPositivo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == false).Sum(x => Convert.ToDouble(x.CoberturasDeFlujosDeEfectivo));
                                    subtotalNegativo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == true).Sum(x => Convert.ToDouble(x.CoberturasDeFlujosDeEfectivo));
                                    //Actualizamos el campo
                                    foreach (var u in listaBmv.Where(u => u.IdTaxonomiaDetalle == value.Key))
                                    {
                                        u.CoberturasDeFlujosDeEfectivo = subtotalPositivo - subtotalNegativo;
                                    }
                                    subtotalPositivo = 0;
                                    subtotalNegativo = 0;
                                    subtotalPositivo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == false).Sum(x => Convert.ToDouble(x.EfectoPorConversion));
                                    subtotalNegativo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == true).Sum(x => Convert.ToDouble(x.EfectoPorConversion));
                                    //Actualizamos el campo
                                    foreach (var u in listaBmv.Where(u => u.IdTaxonomiaDetalle == value.Key))
                                    {
                                        u.EfectoPorConversion = subtotalPositivo - subtotalNegativo;
                                    }
                                    subtotalPositivo = 0;
                                    subtotalNegativo = 0;
                                    subtotalPositivo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == false).Sum(x => Convert.ToDouble(x.ImportesReconocidosEnOtroResultadoIntegralYAcumuladosEnElCapitalContableRelativosAActivosNoCorrientesOGruposDeActivosParaSuDisposicionMantenidosParaLaVenta));
                                    subtotalNegativo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == true).Sum(x => Convert.ToDouble(x.ImportesReconocidosEnOtroResultadoIntegralYAcumuladosEnElCapitalContableRelativosAActivosNoCorrientesOGruposDeActivosParaSuDisposicionMantenidosParaLaVenta));
                                    //Actualizamos el campo
                                    foreach (var u in listaBmv.Where(u => u.IdTaxonomiaDetalle == value.Key))
                                    {
                                        u.ImportesReconocidosEnOtroResultadoIntegralYAcumuladosEnElCapitalContableRelativosAActivosNoCorrientesOGruposDeActivosParaSuDisposicionMantenidosParaLaVenta = subtotalPositivo - subtotalNegativo;
                                    }
                                    subtotalPositivo = 0;
                                    subtotalNegativo = 0;
                                    subtotalPositivo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == false).Sum(x => Convert.ToDouble(x.NuevasMedicionesDePlanesDeBeneficiosDefinidos));
                                    subtotalNegativo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == true).Sum(x => Convert.ToDouble(x.NuevasMedicionesDePlanesDeBeneficiosDefinidos));
                                    //Actualizamos el campo
                                    foreach (var u in listaBmv.Where(u => u.IdTaxonomiaDetalle == value.Key))
                                    {
                                        u.NuevasMedicionesDePlanesDeBeneficiosDefinidos = subtotalPositivo - subtotalNegativo;
                                    }
                                    subtotalPositivo = 0;
                                    subtotalNegativo = 0;
                                    subtotalPositivo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == false).Sum(x => Convert.ToDouble(x.OtrosResultadosIntegrales));
                                    subtotalNegativo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == true).Sum(x => Convert.ToDouble(x.OtrosResultadosIntegrales));
                                    //Actualizamos el campo
                                    foreach (var u in listaBmv.Where(u => u.IdTaxonomiaDetalle == value.Key))
                                    {
                                        u.OtrosResultadosIntegrales = subtotalPositivo - subtotalNegativo;
                                    }
                                    subtotalPositivo = 0;
                                    subtotalNegativo = 0;
                                    subtotalPositivo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == false).Sum(x => Convert.ToDouble(x.OtrosResultadosIntegralesAcumulados));
                                    subtotalNegativo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == true).Sum(x => Convert.ToDouble(x.OtrosResultadosIntegralesAcumulados));
                                    //Actualizamos el campo
                                    foreach (var u in listaBmv.Where(u => u.IdTaxonomiaDetalle == value.Key))
                                    {
                                        u.OtrosResultadosIntegralesAcumulados = subtotalPositivo - subtotalNegativo;
                                    }
                                    subtotalPositivo = 0;
                                    subtotalNegativo = 0;
                                    subtotalPositivo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == false).Sum(x => Convert.ToDouble(x.PagosBasadosEnAcciones));
                                    subtotalNegativo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == true).Sum(x => Convert.ToDouble(x.PagosBasadosEnAcciones));
                                    //Actualizamos el campo
                                    foreach (var u in listaBmv.Where(u => u.IdTaxonomiaDetalle == value.Key))
                                    {
                                        u.PagosBasadosEnAcciones = subtotalPositivo - subtotalNegativo;
                                    }
                                    subtotalPositivo = 0;
                                    subtotalNegativo = 0;
                                    subtotalPositivo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == false).Sum(x => Convert.ToDouble(x.ParticipacionNoControladora));
                                    subtotalNegativo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == true).Sum(x => Convert.ToDouble(x.ParticipacionNoControladora));
                                    //Actualizamos el campo
                                    foreach (var u in listaBmv.Where(u => u.IdTaxonomiaDetalle == value.Key))
                                    {
                                        u.ParticipacionNoControladora = subtotalPositivo - subtotalNegativo;
                                    }
                                    subtotalPositivo = 0;
                                    subtotalNegativo = 0;
                                    subtotalPositivo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == false).Sum(x => Convert.ToDouble(x.PrimaEnEmisionDeAcciones));
                                    subtotalNegativo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == true).Sum(x => Convert.ToDouble(x.PrimaEnEmisionDeAcciones));
                                    //Actualizamos el campo
                                    foreach (var u in listaBmv.Where(u => u.IdTaxonomiaDetalle == value.Key))
                                    {
                                        u.PrimaEnEmisionDeAcciones = subtotalPositivo - subtotalNegativo;
                                    }
                                    subtotalPositivo = 0;
                                    subtotalNegativo = 0;
                                    subtotalPositivo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == false).Sum(x => Convert.ToDouble(x.ReservaDeComponentesDeParticipacionDiscrecional));
                                    subtotalNegativo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == true).Sum(x => Convert.ToDouble(x.ReservaDeComponentesDeParticipacionDiscrecional));
                                    //Actualizamos el campo
                                    foreach (var u in listaBmv.Where(u => u.IdTaxonomiaDetalle == value.Key))
                                    {
                                        u.ReservaDeComponentesDeParticipacionDiscrecional = subtotalPositivo - subtotalNegativo;
                                    }
                                    subtotalPositivo = 0;
                                    subtotalNegativo = 0;
                                    subtotalPositivo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == false).Sum(x => Convert.ToDouble(x.ReservaParaCambiosEnElValorRazonableDePasivosFinancierosAtribuiblesACambiosEnElRiesgoDeCreditoDePasivo));
                                    subtotalNegativo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == true).Sum(x => Convert.ToDouble(x.ReservaParaCambiosEnElValorRazonableDePasivosFinancierosAtribuiblesACambiosEnElRiesgoDeCreditoDePasivo));
                                    //Actualizamos el campo
                                    foreach (var u in listaBmv.Where(u => u.IdTaxonomiaDetalle == value.Key))
                                    {
                                        u.ReservaParaCambiosEnElValorRazonableDePasivosFinancierosAtribuiblesACambiosEnElRiesgoDeCreditoDePasivo = subtotalPositivo - subtotalNegativo;
                                    }
                                    subtotalPositivo = 0;
                                    subtotalNegativo = 0;
                                    subtotalPositivo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == false).Sum(x => Convert.ToDouble(x.ReservaParaCatastrofes));
                                    subtotalNegativo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == true).Sum(x => Convert.ToDouble(x.ReservaParaCatastrofes));
                                    //Actualizamos el campo
                                    foreach (var u in listaBmv.Where(u => u.IdTaxonomiaDetalle == value.Key))
                                    {
                                        u.ReservaParaCatastrofes = subtotalPositivo - subtotalNegativo;
                                    }
                                    subtotalPositivo = 0;
                                    subtotalNegativo = 0;
                                    subtotalPositivo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == false).Sum(x => Convert.ToDouble(x.ReservaParaEstabilizacion));
                                    subtotalNegativo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == true).Sum(x => Convert.ToDouble(x.ReservaParaEstabilizacion));
                                    //Actualizamos el campo
                                    foreach (var u in listaBmv.Where(u => u.IdTaxonomiaDetalle == value.Key))
                                    {
                                        u.ReservaParaEstabilizacion = subtotalPositivo - subtotalNegativo;
                                    }
                                    subtotalPositivo = 0;
                                    subtotalNegativo = 0;
                                    subtotalPositivo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == false).Sum(x => Convert.ToDouble(x.SuperavitDeRevaluacion));
                                    subtotalNegativo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == true).Sum(x => Convert.ToDouble(x.SuperavitDeRevaluacion));
                                    //Actualizamos el campo
                                    foreach (var u in listaBmv.Where(u => u.IdTaxonomiaDetalle == value.Key))
                                    {
                                        u.SuperavitDeRevaluacion = subtotalPositivo - subtotalNegativo;
                                    }
                                    subtotalPositivo = 0;
                                    subtotalNegativo = 0;
                                    subtotalPositivo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == false).Sum(x => Convert.ToDouble(x.UtilidadesAcumuladas));
                                    subtotalNegativo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == true).Sum(x => Convert.ToDouble(x.UtilidadesAcumuladas));
                                    //Actualizamos el campo
                                    foreach (var u in listaBmv.Where(u => u.IdTaxonomiaDetalle == value.Key))
                                    {
                                        u.UtilidadesAcumuladas = subtotalPositivo - subtotalNegativo;
                                    }
                                    subtotalPositivo = 0;
                                    subtotalNegativo = 0;
                                    subtotalPositivo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == false).Sum(x => Convert.ToDouble(x.UtilidadPerdidaEnInstrumentosDeCoberturaQueCubrenInversionesEnInstrumentosDeCapital));
                                    subtotalNegativo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == true).Sum(x => Convert.ToDouble(x.UtilidadPerdidaEnInstrumentosDeCoberturaQueCubrenInversionesEnInstrumentosDeCapital));
                                    //Actualizamos el campo
                                    foreach (var u in listaBmv.Where(u => u.IdTaxonomiaDetalle == value.Key))
                                    {
                                        u.UtilidadPerdidaEnInstrumentosDeCoberturaQueCubrenInversionesEnInstrumentosDeCapital = subtotalPositivo - subtotalNegativo;
                                    }
                                    subtotalPositivo = 0;
                                    subtotalNegativo = 0;
                                    subtotalPositivo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == false).Sum(x => Convert.ToDouble(x.UtilidadPerdidaPorInversionesEnInstrumentosDeCapital));
                                    subtotalNegativo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == true).Sum(x => Convert.ToDouble(x.UtilidadPerdidaPorInversionesEnInstrumentosDeCapital));
                                    //Actualizamos el campo
                                    foreach (var u in listaBmv.Where(u => u.IdTaxonomiaDetalle == value.Key))
                                    {
                                        u.UtilidadPerdidaPorInversionesEnInstrumentosDeCapital = subtotalPositivo - subtotalNegativo;
                                    }
                                    subtotalPositivo = 0;
                                    subtotalNegativo = 0;
                                    subtotalPositivo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == false).Sum(x => Convert.ToDouble(x.VariacionEnElValorDeContratosAFuturo));
                                    subtotalNegativo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == true).Sum(x => Convert.ToDouble(x.VariacionEnElValorDeContratosAFuturo));
                                    //Actualizamos el campo
                                    foreach (var u in listaBmv.Where(u => u.IdTaxonomiaDetalle == value.Key))
                                    {
                                        u.VariacionEnElValorDeContratosAFuturo = subtotalPositivo - subtotalNegativo;
                                    }
                                    subtotalPositivo = 0;
                                    subtotalNegativo = 0;
                                    subtotalPositivo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == false).Sum(x => Convert.ToDouble(x.VariacionEnElValorDeMárgenesConBaseEnMonedaExtranjera));
                                    subtotalNegativo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == true).Sum(x => Convert.ToDouble(x.VariacionEnElValorDeMárgenesConBaseEnMonedaExtranjera));
                                    //Actualizamos el campo
                                    foreach (var u in listaBmv.Where(u => u.IdTaxonomiaDetalle == value.Key))
                                    {
                                        u.VariacionEnElValorDeMárgenesConBaseEnMonedaExtranjera = subtotalPositivo - subtotalNegativo;
                                    }
                                    subtotalPositivo = 0;
                                    subtotalNegativo = 0;
                                    subtotalPositivo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == false).Sum(x => Convert.ToDouble(x.VariacionEnElValorTemporalDeLasOpciones));
                                    subtotalNegativo = filteredList.Where(x => x.FormatoCampo.Equals(AppConsts.FORMAT_X_NEGATIVE) == true).Sum(x => Convert.ToDouble(x.VariacionEnElValorTemporalDeLasOpciones));
                                    //Actualizamos el campo
                                    foreach (var u in listaBmv.Where(u => u.IdTaxonomiaDetalle == value.Key))
                                    {
                                        u.VariacionEnElValorTemporalDeLasOpciones = subtotalPositivo - subtotalNegativo;
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


        private ObservableCollection<Bmv610000> bindAndGroupList(ObservableCollection<Bmv610000> listaBmv)
        {
            //Agrupamos por registro de taxonomia detalle y obtenemos solo un registro
            var tempListaBmvAgrupada = from element in listaBmv
                                       group element by element.IdTaxonomiaDetalle
                                           into groups
                                           select groups.OrderBy(p => p.IdReporte).First();
            ObservableCollection<Bmv610000> listaBmvAgrupada = new ObservableCollection<Bmv610000>(tempListaBmvAgrupada);
            //Actualizamos cada campo del registro
            foreach (var itemAgrupado in listaBmvAgrupada)
            {
                if (string.IsNullOrEmpty(itemAgrupado.FormatoCampo) == false)
                {
                    var itemsBmv = from o in listaBmv
                                   where o.IdTaxonomiaDetalle == itemAgrupado.IdTaxonomiaDetalle
                                   select o;
                    foreach (var subItems in itemsBmv)
                    {
                        switch (subItems.AtributoColumna)
                        {
                            case AppConsts.COL_ACCIONESENTESORERIA:
                                itemAgrupado.AccionesEnTesoreria = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;

                            case AppConsts.COL_CAPITALCONTABLE:
                                itemAgrupado.CapitalContable = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;
                            case AppConsts.COL_CAPITALCONTABLEDELAPARTICIPACIONCONTROLADORA:
                                itemAgrupado.CapitalContableDeLaParticipacionControladora = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;

                            case AppConsts.COL_CAPITALSOCIAL:
                                itemAgrupado.CapitalSocial = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;
                            case AppConsts.COL_COBERTURASDEFLUJOSDEEFECTIVO:
                                itemAgrupado.CoberturasDeFlujosDeEfectivo = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;

                            case AppConsts.COL_EFECTOPORCONVERSION:
                                itemAgrupado.EfectoPorConversion = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;
                            case AppConsts.COL_IMPORTESRECONOCIDOSENOTRORESULTADOINTEGRALYACUMULADOSENELCAPITALCONTABLERELATIVOSAACTIVOSNOCORRIENTESOGRUPOSDEACTIVOSPARASUDISPOSICIONMANTENIDOSPARALAVENTA:
                                itemAgrupado.ImportesReconocidosEnOtroResultadoIntegralYAcumuladosEnElCapitalContableRelativosAActivosNoCorrientesOGruposDeActivosParaSuDisposicionMantenidosParaLaVenta = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;

                            case AppConsts.COL_NUEVASMEDICIONESDEPLANESDEBENEFICIOSDEFINIDOS:
                                itemAgrupado.NuevasMedicionesDePlanesDeBeneficiosDefinidos = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;
                            case AppConsts.COL_OTROSRESULTADOSINTEGRALES:
                                itemAgrupado.OtrosResultadosIntegrales = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;

                            case AppConsts.COL_OTROSRESULTADOSINTEGRALESACUMULADOS:
                                itemAgrupado.OtrosResultadosIntegralesAcumulados = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;
                            case AppConsts.COL_PAGOSBASADOSENACCIONES:
                                itemAgrupado.PagosBasadosEnAcciones = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;

                            case AppConsts.COL_PARTICIPACIONNOCONTROLADORA:
                                itemAgrupado.ParticipacionNoControladora = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;
                            case AppConsts.COL_PRIMAENEMISIONDEACCIONES:
                                itemAgrupado.PrimaEnEmisionDeAcciones = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;

                            case AppConsts.COL_RESERVADECOMPONENTESDEPARTICIPACIONDISCRECIONAL:
                                itemAgrupado.ReservaDeComponentesDeParticipacionDiscrecional = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;
                            case AppConsts.COL_RESERVAPARACAMBIOSENELVALORRAZONABLEDEPASIVOSFINANCIEROSATRIBUIBLESACAMBIOSENELRIESGODECREDITODEPASIVO:
                                itemAgrupado.ReservaParaCambiosEnElValorRazonableDePasivosFinancierosAtribuiblesACambiosEnElRiesgoDeCreditoDePasivo = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;

                            case AppConsts.COL_RESERVAPARACATASTROFES:
                                itemAgrupado.ReservaParaCatastrofes = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;
                            case AppConsts.COL_RESERVAPARAESTABILIZACION:
                                itemAgrupado.ReservaParaEstabilizacion = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;

                            case AppConsts.COL_SUPERAVITDEREVALUACION:
                                itemAgrupado.SuperavitDeRevaluacion = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;
                            case AppConsts.COL_UTILIDADESACUMULADAS:
                                itemAgrupado.UtilidadesAcumuladas = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;

                            case AppConsts.COL_UTILIDADPERDIDAENINSTRUMENTOSDECOBERTURAQUECUBRENINVERSIONESENINSTRUMENTOSDECAPITAL:
                                itemAgrupado.UtilidadPerdidaEnInstrumentosDeCoberturaQueCubrenInversionesEnInstrumentosDeCapital = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;
                            case AppConsts.COL_UTILIDADPERDIDAPORCAMBIOSENVALORRAZONABLEDEACTIVOSFINANCIEROSDISPONIBLESPARALAVENTA:
                                itemAgrupado.UtilidadPerdidaPorCambiosEnValorRazonableDeActivosFinancierosDisponiblesParaLaVenta = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;

                            case AppConsts.COL_UTILIDADPERDIDAPORINVERSIONESENINSTRUMENTOSDECAPITAL:
                                itemAgrupado.UtilidadPerdidaPorInversionesEnInstrumentosDeCapital = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;
                            case AppConsts.COL_VARIACIONENELVALORDECONTRATOSAFUTURO:
                                itemAgrupado.VariacionEnElValorDeContratosAFuturo = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;

                            case AppConsts.COL_VARIACIONENELVALORDEMÁRGENESCONBASEENMONEDAEXTRANJERA:
                                itemAgrupado.VariacionEnElValorDeMárgenesConBaseEnMonedaExtranjera = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;
                            case AppConsts.COL_VARIACIONENELVALORTEMPORALDELASOPCIONES:
                                itemAgrupado.VariacionEnElValorTemporalDeLasOpciones = string.IsNullOrEmpty(subItems.Valor) == true ? 0 : Convert.ToDouble(subItems.Valor);
                                break;

                            default:
                                break;
                        }
                    }
                }

            }

            return listaBmvAgrupada;
        }


        private ObservableCollection<ReporteDetalle> sortReport(ObservableCollection<Bmv610000> listaBmvAgrupada, ObservableCollection<Bmv610000> listaBmv)
        {
            ObservableCollection<ReporteDetalle> sortedList = new ObservableCollection<ReporteDetalle>();
            //Variable que sirve si alguno de los miembros de el eje Aplicación retroactiva y reexpresión retroactiva es mayor a 0, si es mayor a 0. Insertar en la base de datos, en caso contrario no.
            bool isValorDiferenteCeroAplicaciónRetroactivaYReexpresionRetroactiva = contieneValorDiferenterCeroAplicaciónRetroactivaYReexpresionRetroactiva(listaBmvAgrupada, listaBmv);

            foreach (var itemAgrupado in listaBmvAgrupada)
            {
                if (string.IsNullOrEmpty(itemAgrupado.FormatoCampo) == false)
                {
                    var itemsBmv = from o in listaBmv
                                   where o.IdTaxonomiaDetalle == itemAgrupado.IdTaxonomiaDetalle
                                   select o;
                    foreach (var subItems in itemsBmv)
                    {

                        ReporteDetalle rd = new ReporteDetalle();
                        switch (subItems.AtributoColumna)
                        {
                            case AppConsts.COL_ACCIONESENTESORERIA:
                                rd.Valor = Convert.ToString(itemAgrupado.AccionesEnTesoreria);
                                break;

                            case AppConsts.COL_CAPITALCONTABLE:
                                rd.Valor = Convert.ToString(itemAgrupado.CapitalContable);
                                break;
                            case AppConsts.COL_CAPITALCONTABLEDELAPARTICIPACIONCONTROLADORA:
                                rd.Valor = Convert.ToString(itemAgrupado.CapitalContableDeLaParticipacionControladora);
                                break;

                            case AppConsts.COL_CAPITALSOCIAL:
                                rd.Valor = Convert.ToString(itemAgrupado.CapitalSocial);
                                break;
                            case AppConsts.COL_COBERTURASDEFLUJOSDEEFECTIVO:
                                rd.Valor = Convert.ToString(itemAgrupado.CoberturasDeFlujosDeEfectivo);
                                break;

                            case AppConsts.COL_EFECTOPORCONVERSION:
                                rd.Valor = Convert.ToString(itemAgrupado.EfectoPorConversion);
                                break;
                            case AppConsts.COL_IMPORTESRECONOCIDOSENOTRORESULTADOINTEGRALYACUMULADOSENELCAPITALCONTABLERELATIVOSAACTIVOSNOCORRIENTESOGRUPOSDEACTIVOSPARASUDISPOSICIONMANTENIDOSPARALAVENTA:
                                rd.Valor = Convert.ToString(itemAgrupado.ImportesReconocidosEnOtroResultadoIntegralYAcumuladosEnElCapitalContableRelativosAActivosNoCorrientesOGruposDeActivosParaSuDisposicionMantenidosParaLaVenta);
                                break;

                            case AppConsts.COL_NUEVASMEDICIONESDEPLANESDEBENEFICIOSDEFINIDOS:
                                rd.Valor = Convert.ToString(itemAgrupado.NuevasMedicionesDePlanesDeBeneficiosDefinidos);
                                break;
                            case AppConsts.COL_OTROSRESULTADOSINTEGRALES:
                                rd.Valor = Convert.ToString(itemAgrupado.OtrosResultadosIntegrales);
                                break;

                            case AppConsts.COL_OTROSRESULTADOSINTEGRALESACUMULADOS:
                                rd.Valor = Convert.ToString(itemAgrupado.OtrosResultadosIntegralesAcumulados);
                                break;
                            case AppConsts.COL_PAGOSBASADOSENACCIONES:
                                rd.Valor = Convert.ToString(itemAgrupado.PagosBasadosEnAcciones);
                                break;

                            case AppConsts.COL_PARTICIPACIONNOCONTROLADORA:
                                rd.Valor = Convert.ToString(itemAgrupado.ParticipacionNoControladora);
                                break;
                            case AppConsts.COL_PRIMAENEMISIONDEACCIONES:
                                rd.Valor = Convert.ToString(itemAgrupado.PrimaEnEmisionDeAcciones);
                                break;

                            case AppConsts.COL_RESERVADECOMPONENTESDEPARTICIPACIONDISCRECIONAL:
                                rd.Valor = Convert.ToString(itemAgrupado.ReservaDeComponentesDeParticipacionDiscrecional);
                                break;
                            case AppConsts.COL_RESERVAPARACAMBIOSENELVALORRAZONABLEDEPASIVOSFINANCIEROSATRIBUIBLESACAMBIOSENELRIESGODECREDITODEPASIVO:
                                rd.Valor = Convert.ToString(itemAgrupado.ReservaParaCambiosEnElValorRazonableDePasivosFinancierosAtribuiblesACambiosEnElRiesgoDeCreditoDePasivo);
                                break;

                            case AppConsts.COL_RESERVAPARACATASTROFES:
                                rd.Valor = Convert.ToString(itemAgrupado.ReservaParaCatastrofes);
                                break;
                            case AppConsts.COL_RESERVAPARAESTABILIZACION:
                                rd.Valor = Convert.ToString(itemAgrupado.ReservaParaEstabilizacion);
                                break;

                            case AppConsts.COL_SUPERAVITDEREVALUACION:
                                rd.Valor = Convert.ToString(itemAgrupado.SuperavitDeRevaluacion);
                                break;
                            case AppConsts.COL_UTILIDADESACUMULADAS:
                                rd.Valor = Convert.ToString(itemAgrupado.UtilidadesAcumuladas);
                                break;

                            case AppConsts.COL_UTILIDADPERDIDAENINSTRUMENTOSDECOBERTURAQUECUBRENINVERSIONESENINSTRUMENTOSDECAPITAL:
                                rd.Valor = Convert.ToString(itemAgrupado.UtilidadPerdidaEnInstrumentosDeCoberturaQueCubrenInversionesEnInstrumentosDeCapital);
                                break;
                            case AppConsts.COL_UTILIDADPERDIDAPORCAMBIOSENVALORRAZONABLEDEACTIVOSFINANCIEROSDISPONIBLESPARALAVENTA:
                                rd.Valor = Convert.ToString(itemAgrupado.UtilidadPerdidaPorCambiosEnValorRazonableDeActivosFinancierosDisponiblesParaLaVenta);
                                break;

                            case AppConsts.COL_UTILIDADPERDIDAPORINVERSIONESENINSTRUMENTOSDECAPITAL:
                                rd.Valor = Convert.ToString(itemAgrupado.UtilidadPerdidaPorInversionesEnInstrumentosDeCapital);
                                break;
                            case AppConsts.COL_VARIACIONENELVALORDECONTRATOSAFUTURO:
                                rd.Valor = Convert.ToString(itemAgrupado.VariacionEnElValorDeContratosAFuturo);
                                break;

                            case AppConsts.COL_VARIACIONENELVALORDEMÁRGENESCONBASEENMONEDAEXTRANJERA:
                                rd.Valor = Convert.ToString(itemAgrupado.VariacionEnElValorDeMárgenesConBaseEnMonedaExtranjera);
                                break;
                            case AppConsts.COL_VARIACIONENELVALORTEMPORALDELASOPCIONES:
                                rd.Valor = Convert.ToString(itemAgrupado.VariacionEnElValorTemporalDeLasOpciones);
                                break;

                            default:
                                break;
                        }
                        rd.FormatoCampo = subItems.FormatoCampo;
                        rd.IdReporte = subItems.IdReporte;
                        rd.IdReporteDetalle = subItems.IdReporteDetalle;

                        //Si el valor de todos los campos es igual a 0 en los campos del eje Aplicación retroactiva y reexpresión retroactiva se borra de la bd. En caso contrario se guarda o se actualiza.
                        if (isValorDiferenteCeroAplicaciónRetroactivaYReexpresionRetroactiva == false && subItems.IdAxisPadre.HasValue == true)
                        {
                            rd.Estado = false;
                        }
                        else
                        {
                            rd.Estado = true;
                        }
                        if (rd.Estado == true)
                        {
                            sortedList.Add(rd);
                        }
                        else
                        {
                            if (subItems.IdReporteDetalle != null)
                            {
                                sortedList.Add(rd); 
                            }
                        }
                      
                    }
                }

            }
            return sortedList;


        }


        private bool contieneValorDiferenterCeroAplicaciónRetroactivaYReexpresionRetroactiva(ObservableCollection<Bmv610000> listaBmvAgrupada, ObservableCollection<Bmv610000> listaBmv)
        {
            var listaContieneMayorCeroAplicaciónRetroactivaYReexpresionRetroactiva = from e in listaBmvAgrupada
                                                                                     where e.IdAxisPadre.HasValue == true
                                                                                     select e;
            if (listaContieneMayorCeroAplicaciónRetroactivaYReexpresionRetroactiva != null && listaContieneMayorCeroAplicaciónRetroactivaYReexpresionRetroactiva.Any() == true)
            {
                foreach (var itemContieneMayorCeroAplicaciónRetroactivaYReexpresionRetroactiva in listaContieneMayorCeroAplicaciónRetroactivaYReexpresionRetroactiva)
                {
                    var itemsBmv = from o in listaBmv
                                   where o.IdTaxonomiaDetalle == itemContieneMayorCeroAplicaciónRetroactivaYReexpresionRetroactiva.IdTaxonomiaDetalle
                                   select o;
                    foreach (var subItems in itemsBmv)
                    {
                        double valor = 0;
                        switch (subItems.AtributoColumna)
                        {
                            case AppConsts.COL_ACCIONESENTESORERIA:
                                valor = itemContieneMayorCeroAplicaciónRetroactivaYReexpresionRetroactiva.AccionesEnTesoreria;
                                break;

                            case AppConsts.COL_CAPITALCONTABLE:
                                valor = itemContieneMayorCeroAplicaciónRetroactivaYReexpresionRetroactiva.CapitalContable;
                                break;
                            case AppConsts.COL_CAPITALCONTABLEDELAPARTICIPACIONCONTROLADORA:
                                valor = itemContieneMayorCeroAplicaciónRetroactivaYReexpresionRetroactiva.CapitalContableDeLaParticipacionControladora;
                                break;

                            case AppConsts.COL_CAPITALSOCIAL:
                                valor = itemContieneMayorCeroAplicaciónRetroactivaYReexpresionRetroactiva.CapitalSocial;
                                break;
                            case AppConsts.COL_COBERTURASDEFLUJOSDEEFECTIVO:
                                valor = itemContieneMayorCeroAplicaciónRetroactivaYReexpresionRetroactiva.CoberturasDeFlujosDeEfectivo;
                                break;

                            case AppConsts.COL_EFECTOPORCONVERSION:
                                valor = itemContieneMayorCeroAplicaciónRetroactivaYReexpresionRetroactiva.EfectoPorConversion;
                                break;
                            case AppConsts.COL_IMPORTESRECONOCIDOSENOTRORESULTADOINTEGRALYACUMULADOSENELCAPITALCONTABLERELATIVOSAACTIVOSNOCORRIENTESOGRUPOSDEACTIVOSPARASUDISPOSICIONMANTENIDOSPARALAVENTA:
                                valor = itemContieneMayorCeroAplicaciónRetroactivaYReexpresionRetroactiva.ImportesReconocidosEnOtroResultadoIntegralYAcumuladosEnElCapitalContableRelativosAActivosNoCorrientesOGruposDeActivosParaSuDisposicionMantenidosParaLaVenta;
                                break;

                            case AppConsts.COL_NUEVASMEDICIONESDEPLANESDEBENEFICIOSDEFINIDOS:
                                valor = itemContieneMayorCeroAplicaciónRetroactivaYReexpresionRetroactiva.NuevasMedicionesDePlanesDeBeneficiosDefinidos;
                                break;
                            case AppConsts.COL_OTROSRESULTADOSINTEGRALES:
                                valor = itemContieneMayorCeroAplicaciónRetroactivaYReexpresionRetroactiva.OtrosResultadosIntegrales;
                                break;

                            case AppConsts.COL_OTROSRESULTADOSINTEGRALESACUMULADOS:
                                valor = itemContieneMayorCeroAplicaciónRetroactivaYReexpresionRetroactiva.OtrosResultadosIntegralesAcumulados;
                                break;
                            case AppConsts.COL_PAGOSBASADOSENACCIONES:
                                valor = itemContieneMayorCeroAplicaciónRetroactivaYReexpresionRetroactiva.PagosBasadosEnAcciones;
                                break;

                            case AppConsts.COL_PARTICIPACIONNOCONTROLADORA:
                                valor = itemContieneMayorCeroAplicaciónRetroactivaYReexpresionRetroactiva.ParticipacionNoControladora;
                                break;
                            case AppConsts.COL_PRIMAENEMISIONDEACCIONES:
                                valor = itemContieneMayorCeroAplicaciónRetroactivaYReexpresionRetroactiva.PrimaEnEmisionDeAcciones;
                                break;

                            case AppConsts.COL_RESERVADECOMPONENTESDEPARTICIPACIONDISCRECIONAL:
                                valor = itemContieneMayorCeroAplicaciónRetroactivaYReexpresionRetroactiva.ReservaDeComponentesDeParticipacionDiscrecional;
                                break;
                            case AppConsts.COL_RESERVAPARACAMBIOSENELVALORRAZONABLEDEPASIVOSFINANCIEROSATRIBUIBLESACAMBIOSENELRIESGODECREDITODEPASIVO:
                                valor = itemContieneMayorCeroAplicaciónRetroactivaYReexpresionRetroactiva.ReservaParaCambiosEnElValorRazonableDePasivosFinancierosAtribuiblesACambiosEnElRiesgoDeCreditoDePasivo;
                                break;

                            case AppConsts.COL_RESERVAPARACATASTROFES:
                                valor = itemContieneMayorCeroAplicaciónRetroactivaYReexpresionRetroactiva.ReservaParaCatastrofes;
                                break;
                            case AppConsts.COL_RESERVAPARAESTABILIZACION:
                                valor = itemContieneMayorCeroAplicaciónRetroactivaYReexpresionRetroactiva.ReservaParaEstabilizacion;
                                break;

                            case AppConsts.COL_SUPERAVITDEREVALUACION:
                                valor = itemContieneMayorCeroAplicaciónRetroactivaYReexpresionRetroactiva.SuperavitDeRevaluacion;
                                break;
                            case AppConsts.COL_UTILIDADESACUMULADAS:
                                valor = itemContieneMayorCeroAplicaciónRetroactivaYReexpresionRetroactiva.UtilidadesAcumuladas;
                                break;

                            case AppConsts.COL_UTILIDADPERDIDAENINSTRUMENTOSDECOBERTURAQUECUBRENINVERSIONESENINSTRUMENTOSDECAPITAL:
                                valor = itemContieneMayorCeroAplicaciónRetroactivaYReexpresionRetroactiva.UtilidadPerdidaEnInstrumentosDeCoberturaQueCubrenInversionesEnInstrumentosDeCapital;
                                break;
                            case AppConsts.COL_UTILIDADPERDIDAPORCAMBIOSENVALORRAZONABLEDEACTIVOSFINANCIEROSDISPONIBLESPARALAVENTA:
                                valor = itemContieneMayorCeroAplicaciónRetroactivaYReexpresionRetroactiva.UtilidadPerdidaPorCambiosEnValorRazonableDeActivosFinancierosDisponiblesParaLaVenta;
                                break;

                            case AppConsts.COL_UTILIDADPERDIDAPORINVERSIONESENINSTRUMENTOSDECAPITAL:
                                valor = itemContieneMayorCeroAplicaciónRetroactivaYReexpresionRetroactiva.UtilidadPerdidaPorInversionesEnInstrumentosDeCapital;
                                break;
                            case AppConsts.COL_VARIACIONENELVALORDECONTRATOSAFUTURO:
                                valor = itemContieneMayorCeroAplicaciónRetroactivaYReexpresionRetroactiva.VariacionEnElValorDeContratosAFuturo;
                                break;

                            case AppConsts.COL_VARIACIONENELVALORDEMÁRGENESCONBASEENMONEDAEXTRANJERA:
                                valor = itemContieneMayorCeroAplicaciónRetroactivaYReexpresionRetroactiva.VariacionEnElValorDeMárgenesConBaseEnMonedaExtranjera;
                                break;
                            case AppConsts.COL_VARIACIONENELVALORTEMPORALDELASOPCIONES:
                                valor = itemContieneMayorCeroAplicaciónRetroactivaYReexpresionRetroactiva.VariacionEnElValorTemporalDeLasOpciones;
                                break;

                            default:
                                break;
                        }
                        if (valor != 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;

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
