using System;
using System.Collections;
using System.Reflection;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Touch;

using Gadgeteer.Networking;
using Skewworks.Standards.NETMF.Applications;
using Skewworks.Tinkr;
using Skewworks.Tinkr.Controls;
using Skewworks.Tinkr.GadgeteerHelpers;
using Skewworks.Tinkr.Modals;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;
using Colors = Skewworks.Tinkr.Colors;
using HorizontalAlignment = Skewworks.Tinkr.HorizontalAlignment;
using Panel = Microsoft.SPOT.Presentation.Controls.Panel;

namespace Gadgetab
{
    public partial class Program
    {
        private static Form _mainForm;
        private static Appbar _appBar;
        private static CP7TouchHandler CP7Handler;
        private static IApplication _usingBluetooth = null;

        private static readonly Font _fntHuge = Resources.GetFont(Resources.FontResources.Amienne48AA);
        private static readonly Font _fntVerdana12 = Resources.GetFont(Resources.FontResources.Verdana12);
        private static readonly Font _fntArialBold11 = Resources.GetFont(Resources.FontResources.ArialBold11);
        private static readonly Font _fntVerdanaBold24 = Resources.GetFont(Resources.FontResources.VerdanaBold24);

        private readonly string[] _menuItems = new[] { "zombie cannon remote", "zombie twit", "zombie distractor", "zombie health monitor", "load app from SD" };

        // This method is run when the mainboard is powered up or reset.   
        void ProgramStarted()
        {
            Debug.Print("Gadgetab 1.0 Beta");
            Debug.Print("");

            // Initialize Touch
            Graphics.Initialize(TouchCollectionMode.UserHandledOrCP7);
            CP7Handler = new CP7TouchHandler(display_CP7);
            Debug.Print("Touch: CP7");

            // Subscribe to application events
            Graphics.Host.ApplicationLaunched += Host_ApplicationLaunched;
            Graphics.Host.ApplicationClosing += Host_ApplicationClosing;

            // SD Card
            var sdMonitor = new GT.Timer(3000);
            sdMonitor.Tick += timer => { if (sdCard.IsCardInserted && !sdCard.IsCardMounted) sdCard.MountSDCard(); };
            sdMonitor.Start();
            
            // Launch Home
            new Thread(Home).Start();
        }

        void Home()
        {
            _mainForm = new Form("frmMain") 
                {BackgroundImage = Resources.GetBitmap(Resources.BitmapResources.Zombies)};
            //frmMain.Backcolor = Colors.White;

            // Appbar
            _appBar = new Appbar("ab1", Fonts.Calibri9, Fonts.Calibri24);
            _appBar.AddMenuItems(_menuItems);
//            _appBar.AppMenuSelected += OnAppMenuSelected;
            _appBar.AppMenuSelected += new OnAppMenuSelected((object sender, int id, string value) => new Thread(OnAppMenuSelected).Start());

            _mainForm.AddControl(_appBar);

            // Bluetooth Icon
            _appBar.AddControl(new AppbarIcon("aiBluetooth", Resources.GetBitmap(Resources.BitmapResources.bluetooth_off)));

            // Activate
            Graphics.ActiveContainer = _mainForm;
        }

        /*
        void AppbarItemSelected()
        {
            Waiter w;
            switch (_appBar.SelectedIndex)
            {
                case 0:
                    w = new Waiter("Loading Application", Fonts.Calibri14);
                    w.Start();
                    Graphics.Host.LaunchApplication(Resources.GetBytes(Resources.BinaryResources.ZombieTwit));
                    IContainer act = Graphics.ActiveContainer;
                    w.Stop();
                    Graphics.ActiveContainer = act;
                    Graphics.ActiveContainer.Invalidate();
                    break;
                case 1:
                    w = new Waiter("Terminating Application", Fonts.Calibri14);
                    w.Start();
                    Graphics.Host.TerminateApplication(Graphics.Host.RunningApplications[0]);
                    w.Stop();
                    Graphics.ActiveContainer.Invalidate();
                    break;
            }
        }
        */

        void Host_ApplicationClosing(object sender, IApplication app)
        {
            Graphics.ActiveContainer = _mainForm;
            if (_usingBluetooth == app)
            {
                _usingBluetooth = null;
                AppbarIcon bi = (AppbarIcon)_appBar.GetChildByName("aiBluetooth");
                bi.Image = Resources.GetBitmap(Resources.BitmapResources.bluetooth_off);
            }
        }

        void Host_ApplicationLaunched(object sender, IApplication app)
        {
            app.SendMessage(_appBar, "Appbar");
            app.SendMessage(((Form)Graphics.ActiveContainer).BackgroundImage, "FormBackgroundImage");

            // Assign Bluetooth Serial
            if (_usingBluetooth == null)
            {
                if (app.SendMessage(bluetooth.serialPort, "Serial") == "USING")
                {
                    _usingBluetooth = app;
                    app.SendMessage(_appBar.GetChildByName("aiBluetooth"), "BluetoothAppbarIcon");
                }
            }
        }
/*
        private Appbar BuildAppBar(string currentForm)
        {
            var appBar = new Appbar("appBar",_fntVerdana12, _fntVerdanaBold24);

            // Add menu items.
            foreach (var menuItem in _menuItems)
            {
                if (menuItem != currentForm)
                {
                    appBar.AddMenuItem(menuItem);
                }
            }
            appBar.AppMenuSelected += OnAppMenuSelected;
            return appBar;
        }
*/
        private void OnAppMenuSelected() //object sender, int id, string value)
        {
            byte[] bin = null;
            string filePath = null;
            switch (_appBar.SelectedIndex)
            {
                case 0: // "zombie cannon remote":
                    bin = Resources.GetBytes(Resources.BinaryResources.ZombieCannonRemote);
                    break;
                case 1: // "zombie twit":
                    bin = Resources.GetBytes(Resources.BinaryResources.ZombieTwit);
                    break;
                case 2: //"zombie distractor":
                    //bin = Resources.GetBytes(Resources.BinaryResources.ZombieDistractor);
                    break;
                case 3: //"zombie health monitor":
                    bin = Resources.GetBytes(Resources.BinaryResources.ZombieHealthMonitor);
                    break;
                case 4: // "load app from SD"
                    filePath = LoadAppFromSD();
#if DEBUG
                    Debug.Print("App selected: " + filePath);
#endif
                    break;
            }
            if (bin == null) return;
            var w = new Waiter("Loading Application", Fonts.Calibri14);
            w.Start();
            if (filePath == null)
            {
                Graphics.Host.LaunchApplication(bin);
            }
            else
            {
                Graphics.Host.LaunchApplication(filePath);
            }
            IContainer act = Graphics.ActiveContainer;
            w.Stop();
            Graphics.ActiveContainer = act;
            Graphics.ActiveContainer.Invalidate();
        }

        private string LoadAppFromSD()
        {
            if (!sdCard.IsCardMounted && !sdCard.IsCardInserted)
            {
                var result = Prompt.Show("Insert SD", "Cannot run without SD card and files", Fonts.Calibri18Bold, Fonts.Calibri14, PromptType.AbortContinue);
                if(result == PromptResult.Abort) return null;
                sdCard.MountSDCard();
            }           

            return FileDialog.OpenFile("Load an App", _fntVerdana12, _fntVerdana12);
        }
    }
}
