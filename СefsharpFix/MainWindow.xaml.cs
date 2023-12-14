using CefSharp;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace СefsharpFix
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private string script = @"   document.addEventListener('click', function (event) {
        var element = event.target;
        if (element === document.body) {

            console.log('body');
            //CefSharp.PostMessage('body');
            return;
        }
        const segments = [];
        while (element && element !== document.body) {
            if (element.nodeType === Node.ELEMENT_NODE) {
                const tag = element.tagName.toLowerCase();
                const index = [...element.parentNode.children].filter(child => child.tagName === tag).indexOf(element) + 1;
                segments.unshift(`${tag}[${index}]`);
            }
            element = element.parentNode;
        }

            console.log(`/${segments.join('/')}`);
            //CefSharp.PostMessage(`/${segments.join('/')}`);
        }, false);";

        public MainWindow()
        {
            InitializeComponent();
            chromiumWebBrowser.Address = "google.com";
            chromiumWebBrowser.FrameLoadEnd += ChromiumWebBrowser_FrameLoadEnd;
            chromiumWebBrowser.JavascriptMessageReceived +=ChromiumWebBrowser_JavascriptMessageReceived;
        }

        private void ChromiumWebBrowser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            chromiumWebBrowser.GetMainFrame().EvaluateScriptAsync(script, null, 0);   
        }
        
        //По неизвестной мне причине после клика ивент вывода срабатывает 4 раза, по этому оставил вывод в js коде
        private void ChromiumWebBrowser_JavascriptMessageReceived(object sender, JavascriptMessageReceivedEventArgs e)
        {
            if (e.Message != null)
            {
                Console.WriteLine(e.Message.ToString());
            }
        }

        // Make sure to dispose of CefSharp when closing the window
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Cef.Shutdown();
        }

    }

}
