using System;
using System.Windows;
using Microsoft.Web.WebView2.Core;
using ModernWpf.Controls;

namespace NetflixUnOfficial
{
    /// <summary>
    /// Interaction logic for NetflixShell.xaml
    /// </summary>
    public partial class NetflixShell : Window
    {
        private WindowState windowState;

        public NetflixShell()
        {
            InitializeComponent();
            InitializeAsync();
        }

        async void InitializeAsync()
        {
            await WebView.EnsureCoreWebView2Async(null);
            WebView.CoreWebView2.Settings.IsPasswordAutosaveEnabled = true;
            WebView.CoreWebView2.Settings.IsStatusBarEnabled = false;
            WebView.CoreWebView2.ContainsFullScreenElementChanged += CoreWebView2OnContainsFullScreenElementChanged;

            WebView.CoreWebView2.Navigate("https://www.netflix.com/");
        }


        private string GetScrollbarStyleJs()
        {
            return @"
                        var style = document.createElement('style');
                        style.type = 'text/css';
                        style.innerHTML = `

::-webkit-scrollbar-track
{
	-webkit-box-shadow: inset 0 0 6px rgba(0,0,0,0.3);
	border-radius: 10px;
	background-color: transparent;
}

::-webkit-scrollbar
{
	width: 8px;
	background-color: transparent;
}

::-webkit-scrollbar-thumb
{
	border-radius: 10px;
	-webkit-box-shadow: inset 0 0 6px rgba(0,0,0,.3);
	background-color: #D62929;
}
                                            
                                            `;
document.getElementsByTagName('body')[0].appendChild(style);";
        }

        private void CoreWebView2OnContainsFullScreenElementChanged(object sender, object e)
        {
            if (WebView.CoreWebView2.ContainsFullScreenElement)
            {
                Toggle_FullScreen(true);
            }
            else
            {
                Toggle_FullScreen(false);
            }
        }

        private void WebView_OnNavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            try
            {
                ShowLoader(true);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void WebView_OnNavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            try
            {
                WebView.CoreWebView2.ExecuteScriptAsync(GetScrollbarStyleJs());
                ShowLoader(false);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }


        private void ShowLoader(bool isShow)
        {
            if (isShow)
            {
                GridWebView.Visibility = Visibility.Collapsed;
                GridLoader.Visibility = Visibility.Visible;
            }
            else
            {
                GridWebView.Visibility = Visibility.Visible;
                GridLoader.Visibility = Visibility.Collapsed;
            }
        }

        private void Toggle_FullScreen(bool fullScreen)
        {
            if (fullScreen)
            {
                windowState = WindowState;
                TitleBar.SetStyle(this, new Style
                {
                    Setters =
                    {
                        new Setter
                        {
                            Property = HeightProperty,
                            Value = 0.0
                        }
                    }
                });

                AppTitleBar.Visibility = Visibility.Collapsed;
                WindowState = WindowState.Maximized;
                ResizeMode = ResizeMode.NoResize;
                Topmost = true;
            }
            else
            {
                TitleBar.SetStyle(this, new Style
                {
                    Setters =
                    {
                        new Setter
                        {
                            Property = HeightProperty,
                            Value = 32.0
                        }
                    }
                });

                AppTitleBar.Visibility = Visibility.Visible;
                WindowState = windowState;
                ResizeMode = ResizeMode.CanResizeWithGrip;
                Topmost = false;
            }
        }

        private void Toggle_DeveloperPage(object sender, RoutedEventArgs e)
        {
            WebView.CoreWebView2.Navigate("https://github.com/RaoHammas/Netflix");
        }

        private void Refresh_Netflix(object sender, RoutedEventArgs e)
        {
            WebView.CoreWebView2.Navigate("https://www.netflix.com/");
        }
    }
}