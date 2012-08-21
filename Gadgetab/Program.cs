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

        readonly Font _fntHuge = Resources.GetFont(Resources.FontResources.Amienne48AA);
        readonly Font _fntVerdana12 = Resources.GetFont(Resources.FontResources.Verdana12);
        readonly Font _fntArialBold11 = Resources.GetFont(Resources.FontResources.ArialBold11);
        private readonly Font _fntVerdanaBold24 = Resources.GetFont(Resources.FontResources.VerdanaBold24);

        private readonly string[] _menuItems = new[] { "zombie cannon remote", "zombie twit", "zombie distractor", "zombie health monitor" };

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
        private static void OnAppMenuSelected() //object sender, int id, string value)
        {
            byte[] bin = null;
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
            }
            if (bin == null) return;
            var w = new Waiter("Loading Application", Fonts.Calibri14);
            w.Start();
            Graphics.Host.LaunchApplication(bin);
            IContainer act = Graphics.ActiveContainer;
            w.Stop();
            Graphics.ActiveContainer = act;
            Graphics.ActiveContainer.Invalidate();
        }

/* Old CP7 Setup
#region CP7 Setup

        private Point ptLast;
        private Point _ptDownAt;
        private long _lgDownAt;
        private TouchType _tt;
        private bool _tDown;
        private bool _cancelSwipe;

        private void SetupCP7(Display_CP7 CP7)
        {
            CP7.ScreenPressed += new Display_CP7.TouchEventHandler(display_CP7_ScreenPressed);
            CP7.screenReleased += new Display_CP7.TouchEventHandlerTouchReleased(display_CP7_screenReleased);
            CP7.gestureDetected += new Display_CP7.TouchGestureDetected(display_CP7_gestureDetected);
            CP7.homePressed += new Display_CP7.TouchEventHandlerHomeButton(display_CP7_homePressed);
            CP7.menuPressed += new Display_CP7.TouchEventHandlerMenuButton(display_CP7_menuPressed);
            CP7.backPressed += new Display_CP7.TouchEventHandlerBackButton(display_CP7_backPressed);
        }

        private void display_CP7_ScreenPressed(Display_CP7 sender, Display_CP7.TouchStatus touchStatus)
        {
            ptLast = new Point(touchStatus.touchPos[0].xPos, touchStatus.touchPos[0].yPos);

            if (ptLast.X < 800)
            {

                if (!_tDown)
                {
                    _tDown = true;
                    _tt = TouchType.NoGesture;
                    _cancelSwipe = false;
                    _ptDownAt = ptLast;
                    _lgDownAt = DateTime.Now.Ticks;

                    TinkrCore.Instance.RaiseTouchEvent(TouchType.TouchDown, ptLast);
                }
                else
                {
                    if (!_cancelSwipe)
                        CalcDir(ptLast);

                    TinkrCore.Instance.RaiseTouchEvent(TouchType.TouchMove, ptLast);
                }
            }
        }

        private void display_CP7_screenReleased(Display_CP7 sender)
        {
            _tDown = false;

            if (!_cancelSwipe && _tt != TouchType.NoGesture)
                CalcForce(ptLast);

            if (ptLast.X > 800)
            {
                if (ptLast.Y >= 0 && ptLast.Y <= 50)
                    TinkrCore.Instance.RaiseButtonReleased((int)TinkrCore.ButtonIDs.Up);
                else if (ptLast.Y >= 100 && ptLast.Y <= 150)
                    TinkrCore.Instance.RaiseButtonReleased((int)TinkrCore.ButtonIDs.Select);
                else if (ptLast.Y >= 200 && ptLast.Y <= 250)
                    TinkrCore.Instance.RaiseButtonReleased((int)TinkrCore.ButtonIDs.Down);
            }
            else
                TinkrCore.Instance.RaiseTouchEvent(TouchType.TouchUp, ptLast);
        }

        private void display_CP7_gestureDetected(Display_CP7 sender, Display_CP7.Gesture_ID id)
        {
            switch (id)
            {
                case Display_CP7.Gesture_ID.Move_Down:
                    TinkrCore.Instance.RaiseTouchEvent(TouchType.GestureDown, ptLast);
                    break;
                case Display_CP7.Gesture_ID.Move_Left:
                    TinkrCore.Instance.RaiseTouchEvent(TouchType.GestureLeft, ptLast);
                    break;
                case Display_CP7.Gesture_ID.Move_Right:
                    TinkrCore.Instance.RaiseTouchEvent(TouchType.GestureRight, ptLast);
                    break;
                case Display_CP7.Gesture_ID.Move_Up:
                    TinkrCore.Instance.RaiseTouchEvent(TouchType.GestureUp, ptLast);
                    break;
                case Display_CP7.Gesture_ID.No_Gesture:
                    TinkrCore.Instance.RaiseTouchEvent(TouchType.NoGesture, ptLast);
                    break;
                default:
                    TinkrCore.Instance.RaiseTouchEvent((TouchType)id, ptLast);
                    break;
            }
        }

        private void display_CP7_homePressed(Display_CP7 sender)
        {
            TinkrCore.Instance.RaiseButtonPressed((int)TinkrCore.ButtonIDs.Up);
        }

        private void display_CP7_menuPressed(Display_CP7 sender)
        {
            TinkrCore.Instance.RaiseButtonPressed((int)TinkrCore.ButtonIDs.Select);
        }

        private void display_CP7_backPressed(Display_CP7 sender)
        {
            TinkrCore.Instance.RaiseButtonPressed((int)TinkrCore.ButtonIDs.Down);
        }

        private void CalcDir(Point e)
        {
            TouchType sw = TouchType.NoGesture;
            int d;

            d = (e.Y - _ptDownAt.Y);
            if (d > 50)
                sw = TouchType.GestureDown;
            else if (d < -50)
                sw = TouchType.GestureUp;

            d = (e.X - _ptDownAt.X);
            if (d > 50)
            {
                if (sw == TouchType.GestureUp)
                    sw = TouchType.GestureUpRight;
                else if (sw == TouchType.GestureDown)
                    sw = TouchType.GestureDownRight;
                else
                    sw = TouchType.GestureRight;
            }
            else if (d < -50)
            {
                if (sw == TouchType.GestureUp)
                    sw = TouchType.GestureUpLeft;
                else if (sw == TouchType.GestureDown)
                    sw = TouchType.GestureDownLeft;
                else
                    sw = TouchType.GestureLeft;
            }

            if (_tt == TouchType.NoGesture)
                _tt = sw;
            else if (_tt != sw)
                _cancelSwipe = true;
        }

        private void CalcForce(Point e)
        {
            // Calc by time alone
            float dDiff = DateTime.Now.Ticks - _lgDownAt;

            if (dDiff > TimeSpan.TicksPerSecond * .75)
                return;

            // 1.0 = < 1/7th second
            dDiff = TimeSpan.TicksPerSecond / 7 / dDiff;
            if (dDiff > .99)
                dDiff = .99f;
            else if (dDiff < 0)
                dDiff = 0;

            // Raise TouchEvent
            TinkrCore.Instance.RaiseTouchEvent(_tt, e, dDiff);
        }
#endregion
*/
    }
}
