using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.IO;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;


namespace AutoUILibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < Convert.ToInt32(args[1]); i++)
            {
                //
                // Description: This is sample source code to automatically sign-in Chat with valid user account,
                // then add other contact to Deck, send "Hello!! Nice to meet you!!" to that contact, and start Video Chat with him.
                //
                StreamWriter file2 = new StreamWriter(@"e:\RunningTimeAutoUILogin.txt", true);
                Stopwatch stopwatch = Stopwatch.StartNew(); //creates and start the instance of Stopwatch

                // Launch Chat application		
                Directory.SetCurrentDirectory("C:\\Users\\pth\\AppData\\Local\\Personify\\Omni\\");
                Console.WriteLine("\n Begin Automation test chat");
                Process chat = Process.Start("C:\\Users\\pth\\AppData\\Local\\Personify\\Omni\\Personify.exe");

                // Waiting for Chat Login dialog display.
                AutomationElement aeDesktop = AutomationElement.RootElement;
                AutomationElement aeChatLogin = null;
                int numWaits = 0;
                do
                {
                    Console.WriteLine("\n Looking for Chat application...");
                    aeChatLogin = aeDesktop.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "Personify"));
                    ++numWaits;
                    Thread.Sleep(100);
                }
                while (aeChatLogin == null && numWaits < 50);

                Assert.IsNotNull(aeChatLogin);

                // Get all edit boxes from Chat login dialog.
                aeDesktop = AutomationElement.RootElement;
                AutomationElement aeEditUserName = aeChatLogin.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, "Username"));
                AutomationElement aeEditPassword = aeChatLogin.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, "Password"));
                if (aeEditUserName == null || aeEditPassword == null)
                {
                    Assert.Fail("!!Can not find any textbox");
                }


                // Input valid nxchattest1 user account to sign in.            
                AutomationElement aeTemp = TreeWalker.ControlViewWalker.GetNextSibling(aeEditPassword);
                AutomationElement aeSignInButton = TreeWalker.ControlViewWalker.GetNextSibling(aeTemp);
                ValuePattern vpEditUserName = (ValuePattern)aeEditUserName.GetCurrentPattern(ValuePattern.Pattern);
                vpEditUserName.SetValue("");
                ValuePattern vpEditPassword = (ValuePattern)aeEditPassword.GetCurrentPattern(ValuePattern.Pattern);
                vpEditPassword.SetValue("");
                InvokePattern ipClickLogin = (InvokePattern)aeSignInButton.GetCurrentPattern(InvokePattern.Pattern);
                ipClickLogin.Invoke();
                Thread.Sleep(1000);
                vpEditUserName.SetValue("quang.nt@personifyinc.com");
                vpEditPassword.SetValue("123456");
                ipClickLogin.Invoke();
                Thread.Sleep(2000);
                // Waiting for Chat Deck Window display
                AutomationElement aeDeckWindow = null;
                numWaits = 0;
                do
                {
                    Console.WriteLine("\n Looking for Chat Deck...");
                    aeDeckWindow = aeDesktop.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "Home"));
                    ++numWaits;
                    Thread.Sleep(100);
                }
                while (aeDeckWindow == null && numWaits < 50);
                AutomationElement aeTexttemp = aeDeckWindow.FindFirst(TreeScope.Children, new AndCondition(new PropertyCondition(AutomationElement.NameProperty, "Create an immersive recording with your persona and content on screen."), new PropertyCondition(AutomationElement.ControlTypeProperty,ControlType.Text)));
                AutomationElement menu = TreeWalker.ControlViewWalker.GetNextSibling(aeTexttemp);
                TogglePattern toogleclick = (TogglePattern)menu.GetCurrentPattern(TogglePattern.Pattern);
                //ipClickLogin = (InvokePattern)menu.GetCurrentPattern(InvokePattern.Pattern);
                toogleclick.Toggle();
                AutomationElement aeSignout = aeDeckWindow.FindFirst(TreeScope.Descendants, new AndCondition(new PropertyCondition(AutomationElement.NameProperty, "Sign out"), new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.MenuItem)));
                ipClickLogin = (InvokePattern)aeSignout.GetCurrentPattern(InvokePattern.Pattern);
                ipClickLogin.Invoke();
                chat.Kill();
                stopwatch.Stop();
                file2.WriteLine(stopwatch.ElapsedMilliseconds + " miliseconds");
                file2.Close();
            }
        }
    }
}
