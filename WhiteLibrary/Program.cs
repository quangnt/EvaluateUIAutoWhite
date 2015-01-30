using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Automation;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.Finder;
using TestStack.White.AutomationElementSearch;


namespace WhiteLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < Convert.ToInt32(args[1]); i++)
            {
                StreamWriter file2 = new StreamWriter(@"e:\RunningTimeWhiteLogin.txt", true);
                Stopwatch stopwatch = Stopwatch.StartNew(); //creates and start the instance of Stopwatch

                var psi = new ProcessStartInfo(@"C:\\Users\\pth\\AppData\\Local\\Personify\\Omni\\Personify.exe");
                TestStack.White.Application app = TestStack.White.Application.AttachOrLaunch(psi);
                int numWaits = 0;
                Window window;
                do
                {
                    Console.WriteLine("\n Looking for Chat application...");
                    // Find window with title "Personify"
                    window = app.GetWindow("Personify", TestStack.White.Factory.InitializeOption.NoCache);
                    ++numWaits;
                    Thread.Sleep(100);
                } while (window == null && numWaits < 50);
                Assert.IsNotNull(window);
                TestStack.White.UIItems.TextBox username = window.Get<TestStack.White.UIItems.TextBox>(SearchCriteria.ByAutomationId("Username"));
                TestStack.White.UIItems.TextBox password = window.Get<TestStack.White.UIItems.TextBox>(SearchCriteria.ByAutomationId("Password"));

                if (username == null || password == null)
                {
                    Assert.Fail("!!Can not find any textbox");
                }

                AutomationElement aeTemp = TreeWalker.ControlViewWalker.GetNextSibling(password.AutomationElement);
                // Input valid nxchattest1 user account to sign in.            
                AutomationElement aeSignInButton = TreeWalker.ControlViewWalker.GetNextSibling(aeTemp);
                TestStack.White.UIItems.Button signIn = new TestStack.White.UIItems.Button(aeSignInButton, window.ActionListener);
                username.BulkText = "";
                password.BulkText = "";
                signIn.Click();
                Thread.Sleep(1000);
                username.BulkText = "quang.nt@personifyinc.com";
                password.BulkText = "123456";
                signIn.Click();
                Thread.Sleep(2000);
                // Waiting for Chat Deck Window display
                numWaits = 0;
                do
                {
                    Console.WriteLine("\n Looking for Chat Deck...");
                    window = app.GetWindow("Home", TestStack.White.Factory.InitializeOption.NoCache);
                    ++numWaits;
                    Thread.Sleep(100);
                } while (window == null && numWaits < 50);

                
                //TestStack.White.UIItems.TextBox texttemp = window.Get<TestStack.White.UIItems.TextBox>(SearchCriteria.ByText("Create an immersive recording with your persona and content on screen."));
                TestStack.White.UIItems.Label texttemp = window.Get<TestStack.White.UIItems.Label>(SearchCriteria.ByNativeProperty(AutomationElement.NameProperty, "Create an immersive recording with your persona and content on screen."));
                AutomationElement aemenu = TreeWalker.ControlViewWalker.GetNextSibling(texttemp.AutomationElement);
                TestStack.White.UIItems.Button menubtn = new TestStack.White.UIItems.Button(aemenu, window.ActionListener);
                menubtn.Toggle();
                TestStack.White.UIItems.MenuItems.Menu signout = window.Get<TestStack.White.UIItems.MenuItems.Menu>(SearchCriteria.ByText("Sign out"));
                signout.Click();
                app.Kill();
                stopwatch.Stop();
                file2.WriteLine(stopwatch.ElapsedMilliseconds + " miliseconds");
                file2.Close();
            }
        }
    }
}
