using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TaxonomiaWeb.ServiceBmvXblr;
using TaxonomiaWeb.XAMLConverters;
using TaxonomiaWeb.Forms;
using System.Windows.Navigation;

namespace TaxonomiaWeb
{
    public partial class MainPage : UserControl
    {

        public MainPage()
        {
            InitializeComponent();
        }

        // After the Frame navigates, ensure the HyperlinkButton representing the current page is selected
        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            TxtTitlePage.Text = "";
            foreach (UIElement child in LinksStackPanel.Children)
            {
                HyperlinkButton hb = child as HyperlinkButton;
                if (hb != null && hb.NavigateUri != null)
                {
                    if (hb.NavigateUri.ToString().Equals(e.Uri.ToString()))
                    {
                        VisualStateManager.GoToState(hb, "ActiveLink", true);
                    }
                    else
                    {
                        VisualStateManager.GoToState(hb, "InactiveLink", true);
                    }
                }
            }
            foreach (UIElement child in BrandingStackPanel.Children)
            {
                HyperlinkButton hb = child as HyperlinkButton;
                if (hb != null && hb.NavigateUri != null)
                {
                    if (hb.NavigateUri.ToString().Equals(e.Uri.ToString()))
                    {
                        VisualStateManager.GoToState(hb, "ActiveLink", true);
                    }
                    else
                    {
                        VisualStateManager.GoToState(hb, "InactiveLink", true);
                    }
                }
            }
        }

        // If an error occurs during navigation, show an error window
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
            ChildWindow errorWin = new ErrorWindow(e.Uri);
            errorWin.Show();
        }

        public int IdAno { get; set; }
        public int IdTrimestre { get; set; }
        public string Compania { get; set; }

        public void actualizarDatosDeInicio(int idAno, int idTrimestre, string compania)
        {
            IdAno = idAno;
            IdTrimestre = idTrimestre;
            Compania = compania;
        }

        public void actualizarTituloContenidos(string nombrePagina, string nombreBotonInicio, Uri uriBotonInicio)
        {
            if (string.IsNullOrEmpty(nombrePagina) == false)
            {
                TxtTitlePage.Text = nombrePagina;
            }
            if (string.IsNullOrEmpty(nombreBotonInicio) == false && uriBotonInicio != null)
            {
                HlbHome.Content = nombreBotonInicio;
                HlbHome.NavigateUri = uriBotonInicio;
            }
        }

        public void navegarPagina(Uri uri)
        {
            HlbHome.NavigateUri = (uri);
            ContentFrame.Navigate(uri);
        }


    }
}
