using System;
using System.Collections;
using System.Reflection;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Touch;

using Gadgeteer.Networking;
using Skewworks.Tinkr;
using Skewworks.Tinkr.Controls;
using Skewworks.Tinkr.Modals;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;
using Colors = Skewworks.Tinkr.Colors;
using HorizontalAlignment = Skewworks.Tinkr.HorizontalAlignment;
using Panel = Microsoft.SPOT.Presentation.Controls.Panel;
using Pacman;

namespace Gadgetab
{
    public partial class Program
    {
        private static Transitions _ani;

        readonly Font _fntHuge = Resources.GetFont(Resources.FontResources.Amienne48AA);
        readonly Font _fntVerdana12 = Resources.GetFont(Resources.FontResources.Verdana12);
        readonly Font _fntArialBold11 = Resources.GetFont(Resources.FontResources.ArialBold11);
        private readonly Font _fntVerdanaBold24 = Resources.GetFont(Resources.FontResources.VerdanaBold24);

        private readonly string[] _menuItems = new[] { "zombie cannon remote", "zombie twit", "zombie distractor" };

        private PacmanGame _pacmanGame = null;

        // This method is run when the mainboard is powered up or reset.   
        void ProgramStarted()
        {
            Debug.Print("Program Started");

            SetupCP7(display_CP7);
            
            ZombieTwitForm();
        }

        private Appbar BuildAppBar(string currentForm)
        {
            var appBar = new Appbar("appBar", _fntVerdanaBold24);

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

        private void OnAppMenuSelected(object sender, int menuid, string menutext)
        {
            if (_pacmanGame != null)
            {
                _pacmanGame.Enabled = false;
                _pacmanGame.Stop();
                _pacmanGame = null;
            }

            switch (menutext)
            {
                case "zombie cannon remote":
                    ZombieCannonRemoteForm();
                    break;
                case "zombie twit":
                    ZombieTwitForm();
                    break;
                case "zombie distractor":
                    ZombieDistractorForm();
                    break;
            }
        }

        private void ZombieDistractorForm()
        {
            const int gameWidth = 320;
            const int gameHeight = 240;

            var frm = new Form("zombie distractor");

            // Add panel
            var pnl = new Skewworks.Tinkr.Controls.Panel("pnl1", 0, 0, 800, 480);
            pnl.BackgroundImage = Resources.GetBitmap(Resources.BitmapResources.Zombies);
            frm.AddControl(pnl);

            // Add the app bar.
            pnl.AddControl(BuildAppBar(frm.Name));

            // Add a title.
            var title = new Label("lblTitle", "Zombie Distractor", _fntHuge, frm.Width / 2 - 140, 30) { Color = Gadgeteer.Color.Yellow };
            pnl.AddControl(title);

            // Add Pacman.
            var pnl1 = new Skewworks.Tinkr.Controls.Panel("launchPnl", frm.Width/2 - gameWidth/2, frm.Height/2 - gameHeight/2, gameWidth, gameHeight);
            pnl.AddControl(pnl1);

            TinkrCore.ActiveContainer = frm;

            //var surface = (Bitmap)(display_CP7.SimpleGraphics.GetType().GetField("_display", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(display_CP7.SimpleGraphics));
            var surface = TinkrCore.Screen;
            _pacmanGame = new PacmanGame(surface, pnl1.Left, pnl1.Top);
            _pacmanGame.InputManager.AddInputProvider(new GhiJoystickInputProvider(joystick));
            _pacmanGame.Initialize();
        }

        private void ZombieCannonRemoteForm()
        {
            var frm = new Form("zombie cannon remote");

            // Add panel
            var pnl = new Skewworks.Tinkr.Controls.Panel("pnl1", 0, 0, 800, 480);
            pnl.BackgroundImage = Resources.GetBitmap(Resources.BitmapResources.Zombies);
            frm.AddControl(pnl);

            // Add the app bar.
            pnl.AddControl(BuildAppBar(frm.Name));

            // Add a title.
            var title = new Label("lblTitle", "Zombie Cannon Remote", _fntHuge, frm.Width/2 - 175, 30)
                            {Color = Gadgeteer.Color.Yellow};
            pnl.AddControl(title);

            // Add a fire button.
            //var pic1 = new Picturebox("launchBtn", Resources.GetBitmap(Resources.BitmapResources.LaunchButton), frm.Width / 2 - 150, frm.Height/2 - 150, 300, 300);
            //pic1.ButtonPressed += (sender, id) => Debug.Print("FIRE!");
            //pnl.AddControl(pic1);
            var pic1 = new Skewworks.Tinkr.Controls.Panel("launchBtn",frm.Width / 2 - 150, frm.Height/2 - 150, 300, 300);
            pic1.BackgroundImage = Resources.GetBitmap(Resources.BitmapResources.LaunchButton);
            pic1.Tap += (sender, id) => Debug.Print("FIRE!");
            pnl.AddControl(pic1);

            TinkrCore.ActiveContainer = frm;
        }


        private void ZombieTwitForm()
        {
            var tweets = new[]{
                                    @"@zombieHunter Zombies are coming!"
                                    , @"@zombieHunter Zombies are getting closer!"
                                    , @"@zombieHunter THEY'RE HERE!!!"
                                    , @"@zombieHunter Send the Gadgets!!!"
                                    , @"@zombieHunter Tell my wife and kids..."
                                    , @"@zombieHunter ...I'll eat them later!"
                                };

            var frm = new Form("zombie twit");

            // Add panel
            var pnl = new Skewworks.Tinkr.Controls.Panel("pnl1", 0, 0, 800, 480);
            pnl.BackgroundImage = Resources.GetBitmap(Resources.BitmapResources.Zombies);
            frm.AddControl(pnl);

            // Add the app bar.
            pnl.AddControl(BuildAppBar(frm.Name));

            // Add a title.
            var title = new Label("lblTitle", "Zombie Twit", _fntHuge, frm.Width / 2 - 100, 50) { Color = Gadgeteer.Color.Yellow };
            pnl.AddControl(title);

            // Add a listbox
            var lb = new Listbox("lb1", _fntArialBold11, frm.Width / 2 - 200, frm.Height / 2 - 100, 400, 200, null);
            pnl.AddControl(lb);


            TinkrCore.ActiveContainer = frm;

            byte lastTweet = 0;
            var timer = new GT.Timer(2000);
            timer.Tick += timer1 =>
                              {
                                  if(lastTweet >= tweets.Length)
                                  {
                                      timer.Stop();
                                      return;
                                  }
                                  //lb.InsertAt(tweets[lastTweet++], 1);
                                  lb.AddItem(tweets[lastTweet++]);
                              };
            timer.Start();
        }


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
                    TinkrCore.Instance.RaiseButtonReleased(TinkrCore.UpButtonID);
                else if (ptLast.Y >= 100 && ptLast.Y <= 150)
                    TinkrCore.Instance.RaiseButtonReleased(TinkrCore.SelectButtonID);
                else if (ptLast.Y >= 200 && ptLast.Y <= 250)
                    TinkrCore.Instance.RaiseButtonReleased(TinkrCore.DownButtonID);
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
            TinkrCore.Instance.RaiseButtonPressed(TinkrCore.UpButtonID);
        }

        private void display_CP7_menuPressed(Display_CP7 sender)
        {
            TinkrCore.Instance.RaiseButtonPressed(TinkrCore.SelectButtonID);
        }

        private void display_CP7_backPressed(Display_CP7 sender)
        {
            TinkrCore.Instance.RaiseButtonPressed(TinkrCore.DownButtonID);
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

    }
}
