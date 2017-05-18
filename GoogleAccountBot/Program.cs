using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;


namespace GoogleAccountBot
{
    static class Program
    {
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_CLOSE = 0xF060;

        [STAThread]
        static void Main()
        {
            Process[] chromeInstances = Process.GetProcessesByName("chrome");
            foreach (Process process in chromeInstances)
            {
                SendMessage(process.MainWindowHandle, WM_SYSCOMMAND, SC_CLOSE, 0);
                Thread.Sleep(200);
                process.Close();
            }
            string localData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string userData = Path.Combine(localData, "Google\\Chrome\\User Data");
            string profileName = "Profile 11";
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--user-data-dir=" + userData);
            options.AddArgument("--profile-directory=" + profileName);
            options.AddArgument("--start-maximized");
            //    options.AddArgument("--disable-extensions");
            options.AddArgument("--disable-popup-blocking");
            options.AddArgument("--disable-background-networking");
            options.AddArgument("--disable-client-side-phishing-detection");
            options.AddArgument("--disable-default-apps");
            options.AddArgument("--disable-hang-monitor");
            options.AddArgument("--disable-infobars");
            options.AddArgument("--disable-prompt-on-repost");
            options.AddArgument("--disable-sync");
            options.AddArgument("--disable-web-resources");
            options.AddArgument("--disable-logging");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--ignore-certificate-errors");
            options.AddArgument("--log-level=0");
            ChromeDriver driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);

            List<string> urlList = new List<string>();
            driver.Navigate().GoToUrl("https://vozforums.com/search.php?do=finduser&u=1521370");
            for (int i = 1; i < 5; i++)
            {
                var topics = driver.FindElementsByXPath("//a[starts-with(@href, 'showthread.php?p=')]");
                foreach (var t in topics)
                {
                    string href = t.GetAttribute("href");
                    if (!urlList.Contains(href))
                    {
                        urlList.Add(href);
                    }
                }
                string url = driver.Url;
                if (url.Contains("&page="))
                {
                    int startIndex = url.IndexOf("&page=") + "&page=".Length;
                    int length = url.Length - startIndex;
                    url = url.Remove(startIndex, length);
                    url += (i + 1);
                }
                else
                {
                    url += "&page=" + (i + 1);
                }
                driver.Navigate().GoToUrl(url);
            }
            while (urlList.Count > 0)
            {
                string url = urlList.FirstOrDefault();
                driver.Navigate().GoToUrl(url);
                IWebElement editButton;
                try
                {
                    editButton = driver.FindElementByXPath("//a[starts-with(@href, 'editpost.php?do=editpost&p=')]");
                }
                catch (Exception)
                {
                    editButton = null;
                }
                if (editButton != null)
                {
                    string e2Href = editButton.GetAttribute("href");
                    driver.Navigate().GoToUrl(e2Href);
                    var inputDeleteRadio =
                        driver.FindElementByXPath(
                            "//*[@id='collapseobj_editpost_delete']/tr/td/div[1]/div/fieldset/div/div[2]/label/input");
                    inputDeleteRadio.Click();
                    var inputReason = driver.FindElementByXPath("//input[@name='reason']");
                    inputReason.SendKeys("dcm vcl");
                    var inputDeleteButton =
                        driver.FindElementByXPath("//*[@id='collapseobj_editpost_delete']/tr/td/div[2]/input");
                    inputDeleteButton.Click();
                }
                Thread.Sleep(1000);
                urlList.Remove(url);
            }
            driver.Close();
            driver.Quit();
        }
    }
}
