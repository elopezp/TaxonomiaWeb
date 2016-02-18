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
    public partial class Inicio : Page
    {

        private MainPage mainPage = null;

        public Inicio()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            mainPage = (MainPage)Application.Current.RootVisual;
            mainPage.actualizarTituloContenidos(null, "BMV", new Uri("/Inicio", UriKind.Relative));
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
            servBmvXblr.GetAllTrimestersCompleted += servBmvXblr_GetAllTrimestersCompleted;
            servBmvXblr.GetAllTrimestersAsync();
            servBmvXblr.GetAllYearsCompleted += servBmvXblr_GetAllYearsCompleted;
            servBmvXblr.GetAllYearsAsync();
            if (string.IsNullOrEmpty(mainPage.Compania) == false)
            {
                TxtCompania.Text = mainPage.Compania;
            }
            else 
            {
                TxtCompania.Text = "QUMMA";
            }
        }


        #region Funciones de cada celda

        #endregion


        #region Llamadas a servicios asincronos WCF

        void servBmvXblr_GetAllYearsCompleted(object sender, GetAllYearsCompletedEventArgs e)
        {
            if (e.Result != null && e.Result.Any() == true)
            {
                PeriodoAno predeterminado = new PeriodoAno();
                predeterminado.Ano = "-";
                predeterminado.IdAno = -1;
                e.Result.Add(predeterminado);
                CmbAno.ItemsSource = e.Result;
                CmbAno.DisplayMemberPath = "Ano";
                CmbAno.SelectedValuePath = "IdAno";
                if (mainPage.IdAno > 0)
                {
                    CmbAno.SelectedValue = mainPage.IdAno;
                }
                else 
                {
                    CmbAno.SelectedItem = predeterminado;
                }
             
                
            }
        }

        void servBmvXblr_GetAllTrimestersCompleted(object sender, GetAllTrimestersCompletedEventArgs e)
        {
            if (e.Result != null && e.Result.Any() == true)
            {
                PeriodoTrimestre predeterminado = new PeriodoTrimestre();
                predeterminado.Descripcion = "-";
                predeterminado.IdTrimestre = -1;
                e.Result.Add(predeterminado);
                CmbTrimestre.ItemsSource = e.Result;
                CmbTrimestre.DisplayMemberPath = "Descripcion";
                CmbTrimestre.SelectedValuePath = "IdTrimestre";
                if (mainPage.IdAno > 0)
                {
                    CmbTrimestre.SelectedValue = mainPage.IdTrimestre;
                }
                else
                {
                    CmbTrimestre.SelectedItem = predeterminado;
                }
            }
        }

        #endregion



        #region Metodos de clase

        private void BtnEnviar_Click(object sender, RoutedEventArgs e)
        {
            if (validarDatos() == true)
            {
                int idAno = Int32.Parse(CmbAno.SelectedValue.ToString());
                int idTrimestre = Int32.Parse(CmbTrimestre.SelectedValue.ToString());
                string compania = TxtCompania.Text;
                Uri uri = new Uri(string.Format("/Home?trim={0}&ejerc={1}&comp={2}", idTrimestre.ToString(), idAno.ToString(),compania), UriKind.Relative);
                mainPage.actualizarDatosDeInicio(idAno,idTrimestre,compania);
                mainPage.navegarPagina(uri);
          
            }

        }

        private bool validarDatos()
        {
            bool res = false;
            LblError.Text = "";

            if (string.IsNullOrEmpty(TxtCompania.Text) == true)
            {
                TxtCompania.Focus();
                LblError.Text = "La razón social no debe de estar vacia";
                return res;
            }

            if (CmbAno.SelectedValue != null && Int32.Parse(CmbAno.SelectedValue.ToString()) == -1)
            {
                CmbAno.Focus();
                LblError.Text = "Debe elegir un año";
                return res;
            }

            if (CmbTrimestre.SelectedValue != null && Int32.Parse(CmbTrimestre.SelectedValue.ToString()) == -1)
            {
                CmbTrimestre.Focus();
                LblError.Text = "Debe elegir un trimestre";
                return res;
            }
            res = true;
            return res;
        }

        #endregion

    }
}