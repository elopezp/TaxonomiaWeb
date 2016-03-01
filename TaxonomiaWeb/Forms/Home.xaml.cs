using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaxonomiaWeb.ServiceBmvXblr;

namespace TaxonomiaWeb.Forms
{
    public partial class Home : Page
    {
        private MainPage mainPage = null;

        public Home()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(MainPage_Loaded);
            ListBox.ItemContainerGenerator.ItemsChanged += ItemContainerGenerator_ItemsChanged;
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            mainPage = (MainPage)Application.Current.RootVisual;
            try
            {
            mainPage.actualizarTituloContenidos(null, "BMV", new Uri("/Inicio", UriKind.Relative));
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
            }
            catch(Exception ex)
            {
                 ChildWindow errorWin = new ErrorWindow("URL no válida", "La URL especificada no es válida. Se direccionará a la página de inicio");
                 errorWin.Show();
                 mainPage.navegarPagina(new Uri("/Inicio", UriKind.Relative));
            }
        }

        private Service1Client servBmvXblr = null;
        private ObservableCollection<FormContenido> listFormContenido = null;

        void ItemContainerGenerator_ItemsChanged(object sender, System.Windows.Controls.Primitives.ItemsChangedEventArgs e)
        {
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //this.TxtTitle.Text = "Forma 210000";
            servBmvXblr = new Service1Client();
            servBmvXblr.GetAllFormsCompleted += servBmvXblr_GetAllFormsCompleted;
            servBmvXblr.GetAllFormsAsync();

        }

        void servBmvXblr_GetAllFormsCompleted(object sender, GetAllFormsCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                listFormContenido = e.Result;
                ListBox.SelectedIndex = -1;
                ListBox.ItemsSource = listFormContenido;

            }
        }

        private void BtnGenerarXblr_Click(object sender, RoutedEventArgs e)
        {
            servBmvXblr = new Service1Client();
            servBmvXblr.GenerarContextosCompleted += servBmvXblr_GenerarContextosCompleted;
            servBmvXblr.GenerarContextosAsync(mainPage.NumTrimestre,mainPage.IdAno);
        }

        void servBmvXblr_GenerarContextosCompleted(object sender, GenerarContextosCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                int cantidadContextos = e.Result;

            }
        }

        private void ListBox_Loaded(object sender, RoutedEventArgs e)
        {
            ItemCollection ic = ListBox.Items;

            int counter = 1;
            foreach (ListBoxItem lbi in ic)
            {
                if (counter % 2 == 1)
                {
                    lbi.Background = new SolidColorBrush(Color.FromArgb(255, 139, 0, 139));
                }
                else
                {
                    lbi.Background = new SolidColorBrush(Color.FromArgb(255, 153, 50, 204));
                }
                counter++;
            }
        }


        #region Eventos del Datagrid

        #endregion

        #region Funciones de cada celda
        #endregion


        #region Llamadas a servicios asincronos WCF
        #endregion



        #region Metodos de clase


        void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FormContenido formContenido = (FormContenido)ListBox.SelectedItem;
            if (formContenido != null)
            {
                MainPage m = (MainPage)Application.Current.RootVisual;
                string parameterPageName = formContenido.Descripcion + " - " + formContenido.Contenido;
                string strUri = string.Format("/Page{0}?name={1}&trim={2}&ejerc={3}&comp={4}", formContenido.Contenido.Replace(" ", "_"), parameterPageName,m.NumTrimestre,m.IdAno,m.Compania);
                Uri uri = new Uri(strUri, UriKind.Relative);
                m.navegarPagina(uri);
            }
        }

        #endregion

    }
}