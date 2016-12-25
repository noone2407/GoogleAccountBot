using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace GoogleAccountBot
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("user-data-dir=C:/Users/Dung/Desktop/User Data");
            options.AddArgument("--start-maximized");
            ChromeDriver driverOne = new ChromeDriver(options);
            driverOne.Navigate().GoToUrl("http://192.168.1.1:8080");
        }
    }
}
