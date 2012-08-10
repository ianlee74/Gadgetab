using System;
using System.Collections;
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

        // This method is run when the mainboard is powered up or reset.   
        void ProgramStarted()
        {
            Debug.Print("Program Started");

            SetupCP7(display_CP7);
          
            //TinkrCore.Instance.TouchEvent += (sender, type, pt) => Debug.Print("Touched!");

            //var appIcon = new AppbarIcon("abIcon1", Resources.GetBitmap(Resources.BitmapResources.arrow));
            //appBar.AddControl(appIcon);


            ZombieTwitForm();
            //Form1();
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
            var frm = new Form("zombie distractor");

            // Add panel
            var pnl = new Skewworks.Tinkr.Controls.Panel("pnl1", 0, 0, 800, 480);
            pnl.BackgroundImage = Resources.GetBitmap(Resources.BitmapResources.Zombies);
            frm.AddControl(pnl);

            // Add the app bar.
            frm.AddControl(BuildAppBar(frm.Name));

            // Add a title.
            var title = new Label("lblTitle", "Zombie Distractor", _fntHuge, frm.Width / 2 - 140, 30) { Color = Gadgeteer.Color.Yellow };
            frm.AddControl(title);

            // Add Pacman.
            var pnl1 = new Skewworks.Tinkr.Controls.Panel("launchPnl", frm.Width/2 - 150, frm.Height/2 - 150, 300, 300);
            frm.AddControl(pnl1);

            TinkrCore.ActiveContainer = frm;
        }

        private void ZombieCannonRemoteForm()
        {
            var frm = new Form("zombie cannon remote");

            // Add panel
            var pnl = new Skewworks.Tinkr.Controls.Panel("pnl1", 0, 0, 800, 480);
            pnl.BackgroundImage = Resources.GetBitmap(Resources.BitmapResources.Zombies);
            frm.AddControl(pnl);

            // Add the app bar.
            frm.AddControl(BuildAppBar(frm.Name));

            // Add a title.
            var title = new Label("lblTitle", "Zombie Cannon Remote", _fntHuge, frm.Width/2 - 175, 30)
                            {Color = Gadgeteer.Color.Yellow};
            frm.AddControl(title);

            // Add a fire button.
            var pic1 = new Picturebox("launchBtn", Resources.GetBitmap(Resources.BitmapResources.LaunchButton), frm.Width / 2 - 150, frm.Height/2 - 150, 300, 300);
            //pic1.ButtonPressed += (sender, id) => Debug.Print("FIRE!");
            frm.AddControl(pic1);

            TinkrCore.ActiveContainer = frm;
        }


        private void ZombieTwitForm()
        {
            var frm = new Form("zombie twit");

            // Add panel
            var pnl = new Skewworks.Tinkr.Controls.Panel("pnl1", 0, 0, 800, 480);
            pnl.BackgroundImage = Resources.GetBitmap(Resources.BitmapResources.Zombies);
            frm.AddControl(pnl);

            // Add the app bar.
            frm.AddControl(BuildAppBar(frm.Name));

            // Add a title.
            var title = new Label("lblTitle", "Zombie Twit", _fntHuge, frm.Width / 2 - 100, 50) { Color = Gadgeteer.Color.Yellow };
            frm.AddControl(title);

            // Add a listbox
            var lb = new Listbox("lb1", _fntArialBold11, frm.Width / 2 - 200, frm.Height / 2 - 100, 400, 200,
                                    new[]{@"@zombieHunter Zombies are coming!"
                                          ,@"@zombieHunter Zombies are getting closer!"
                                          ,@"@zombieHunter THEY'RE HERE!!!"
                                          ,@"@zombieHunter Send the Gadgets!!!"
                                          ,@"@zombieHunter Tell my wife and kids..."
                                          ,@"@zombieHunter ...I'll eat them later!"});
            frm.AddControl(lb);


            TinkrCore.ActiveContainer = frm;

            var timer = new GT.Timer(2000);
            timer.Tick += timer1 =>
            {
                lb.InsertAt("Tick.", 1);
            };
            timer.Start();
        }

        private void Form1()
        {
            // Create form
            Form frm1 = new Form("frm1");

            // Add Panel
            Skewworks.Tinkr.Controls.Panel pnl1 = new Skewworks.Tinkr.Controls.Panel("pnl1", frm1.Width / 2 - 182, frm1.Height / 2 - 116, 369, 232);
            pnl1.BackgroundImage = Resources.GetBitmap(Resources.BitmapResources.mynameis);
            frm1.AddControl(pnl1);

            // Add Textbox
            Textbox txt1 = new Textbox("txt1", "Type Name Here", _fntHuge, Colors.LightGray, 4, 140 - _fntHuge.Height / 2, 361, _fntHuge.Height + 7);
            txt1.TextAlignment = HorizontalAlignment.Center;
            txt1.EditorTitle = "Hello, My Name Is";
            txt1.EditorFont = _fntVerdana12;
            txt1.TextEditorStart += new OnTextEditorStart(txt1_TextEditorStart);
            txt1.TextEditorClosing += new OnTextEditorClosing(txt1_TextEditorClosing);
            txt1.TextChanged += new OnTextChanged(txt1_TextChanged);
            pnl1.AddControl(txt1);

            // Add Picturebox
            Picturebox pb1 = new Picturebox("pb1", Resources.GetBitmap(Resources.BitmapResources.arrow_disabled), frm1.Width - 46, frm1.Height / 2 - 38, BorderStyle.BorderNone);
            pb1.Tap += new OnTap(pb1_Tap);
            pb1.Enabled = false;
            frm1.AddControl(pb1);

            // Activate form
            TinkrCore.ActiveContainer = frm1;
        }

        private static void txt1_TextEditorStart(object sender, ref TextEditorArgs args)
        {
            if (args.DefaultValue == "Type Name Here")
                args.DefaultValue = string.Empty;
        }

        private static void txt1_TextEditorClosing(object sender, ref string Text, ref bool Cancel)
        {
            Font fnt = Resources.GetFont(Resources.FontResources.Verdana12);

            if (Text == string.Empty || Text == null)
                Text = "Type Name Here";
            else if (Text.Length == 1)
            {
                Alert.Show("Liar", "Your name is so not '" + Text + "', put in a real one, ok?", fnt, fnt);
                Cancel = true;
            }
        }

        private static void txt1_TextChanged(object sender, string Text)
        {

            Picturebox pb = (Picturebox)TinkrCore.ActiveContainer.GetChildByName("pb1");

            if (Text == string.Empty || Text == null || Text == "Type Name Here")
            {
                pb.Image = Resources.GetBitmap(Resources.BitmapResources.arrow_disabled);
                pb.Enabled = false;
            }
            else
            {
                Font fnt = Resources.GetFont(Resources.FontResources.Verdana12);
                Alert.Show("Welcome, " + Text + "!", "Welcome to the Tinkr demo!\n\nThis is an 'Alert'; which you can close by pressing the red 'X' to the right.\n\n" +
                    "After you close this alert, you can press the arrow on the right of the screen to continue.", fnt, fnt);
                pb.Image = Resources.GetBitmap(Resources.BitmapResources.arrow);
                pb.Enabled = true;
            }
        }

        private static void pb1_Tap(object sender, Point e)
        {
            _ani = Transitions.None;
            new Thread(Form2).Start();
        }

        private static void Form2()
        {
            Font fnt = Resources.GetFont(Resources.FontResources.Verdana12);
            Font fnt2 = Resources.GetFont(Resources.FontResources.Amienne48AA);

            string[] values = new string[] { "Apples", "Oranges", "Grapes", "Pears", "Grapefruits", "Tangerines", "Bananas" };

            // Create form
            Form frm2 = new Form("frm2");

            // Add Label
            Label lblHeader = new Label("lblWelcome", "Welcome to Tinkr!\nOn this screen we're going to have a look at the Combobox and RadioButton controls.", fnt, 4, 4, frm2.Width - 8, Colors.Charcoal);
            frm2.AddControl(lblHeader);

            // Add Small RadioButtons
            RadioButton rdo1 = new RadioButton("rdo1", 4, lblHeader.Y + lblHeader.Height + 24, true, "smalls");
            frm2.AddControl(rdo1);
            frm2.AddControl(new Label("lbl1", "Drop-down", fnt, 27, rdo1.Y, false));
            RadioButton rdo2 = new RadioButton("rdo2", 144, lblHeader.Y + lblHeader.Height + 24, false, "smalls");
            frm2.AddControl(rdo2);
            frm2.AddControl(new Label("lbl2", "Pop-out", fnt, 167, rdo1.Y, false));

            // Add Small Combobox
            Combobox cbo1 = new Combobox("cbo1", fnt, 4, rdo1.Y + rdo1.Height + 10, 240, values);
            frm2.AddControl(cbo1);

            // Add Large RadioButtons
            RadioButton rdo3 = new RadioButton("rdo3", 4, rdo1.Y + rdo1.Height + 60, true, "bigs", true);
            frm2.AddControl(rdo3);
            frm2.AddControl(new Label("lbl3", "Drop-down", fnt, 43, rdo3.Y + 7, false));
            RadioButton rdo4 = new RadioButton("rdo4", 164, rdo3.Y, false, "bigs", true);
            frm2.AddControl(rdo4);
            frm2.AddControl(new Label("lbl4", "Pop-out", fnt, 203, rdo3.Y + 7, false));

            // Add Large Combobox (with Radio Buttons)
            Combobox cbo2 = new Combobox("cbo2", fnt2, 4, rdo4.Y + rdo4.Height + 10, 240, values, DisplayMode.Large);
            frm2.AddControl(cbo2);

            // Add last label
            frm2.AddControl(new Label("lbl5", "*Too big to drop down", fnt, 4, cbo2.Y + cbo2.Height + 10, frm2.Width - 8, Colors.DarkRed, false));

            // Bind Events
            rdo1.CheckChanged += new OnCheckChanged((object sender, bool Checked) => cbo1.AlwaysPopup = !Checked);
            rdo3.CheckChanged += new OnCheckChanged((object sender, bool Checked) => cbo2.AlwaysPopup = !Checked);

            // Next Button
            Button btn1 = new Button("btn1", "Continue", fnt, 0, 0);
            btn1.X = frm2.Width - btn1.Width - 4;
            btn1.Y = frm2.Height - btn1.Height - 4;
            btn1.Tap += new OnTap((object sender, Point e) => new Thread(Form3).Start());
            frm2.AddControl(btn1);

            // Activate form
            TinkrCore.Instance.ScreenTransition(frm2, _ani);
        }

        private static bool bLock1;
        private static bool bLock2;

        private static void Form3()
        {
            Font fnt = Resources.GetFont(Resources.FontResources.Verdana12);
            Font fnt2 = Resources.GetFont(Resources.FontResources.Amienne48AA);

            // Create form
            Form frm3 = new Form("frm3");

            // Add Label
            Label lblHeader = new Label("lblWelcome", "On this screen we're going to have a look at the Checkbox, NumericUpDown and Progressbar controls.", fnt, 4, 4, frm3.Width - 8, fnt.Height * 2, Colors.Charcoal);
            frm3.AddControl(lblHeader);

            // Add Label & Progressbars
            Label lbl1 = new Label("lbl1", "These progressbars are bound to a thread.", fnt, 4, lblHeader.Height + 28);
            frm3.AddControl(lbl1);
            frm3.AddControl(new Progressbar("prog1", 4, lbl1.Y + lbl1.Height + 4, frm3.Width - 8, false, 0, 100, 33));
            frm3.AddControl(new Progressbar("prog2", 4, lbl1.Y + lbl1.Height + 24, frm3.Width - 8, true));

            // Add Numeric Up/Downs
            frm3.AddControl(new NumericUpDown("nud1", fnt, 4, lbl1.Y + lbl1.Height + 60, 130, false));
            frm3.AddControl(new NumericUpDown("nud2", fnt2, 4, lbl1.Y + lbl1.Height + 90, 130, true));

            // Add Checkboxes
            Checkbox chk1 = new Checkbox("chk1", 138, lbl1.Y + lbl1.Height + 62);
            chk1.CheckChanged += new OnCheckChanged((object sender, bool value) => bLock1 = value);
            frm3.AddControl(chk1);
            Checkbox chk2 = new Checkbox("chk2", 138, lbl1.Y + lbl1.Height + 110, false, true);
            chk2.CheckChanged += new OnCheckChanged((object sender, bool value) => bLock2 = value);
            frm3.AddControl(chk2);

            // Activate form
            TinkrCore.ActiveContainer = frm3;

            // Begin Updating
            new Thread(UpdateProgressbars).Start();

        }

        private static void UpdateProgressbars()
        {
            Progressbar pg1 = (Progressbar)TinkrCore.ActiveContainer.GetChildByName("prog1");
            Progressbar pg2 = (Progressbar)TinkrCore.ActiveContainer.GetChildByName("prog2");

            NumericUpDown nud1 = (NumericUpDown)TinkrCore.ActiveContainer.GetChildByName("nud1");
            NumericUpDown nud2 = (NumericUpDown)TinkrCore.ActiveContainer.GetChildByName("nud2");

            int v1 = pg1.Value;
            int v2 = pg2.Value;
            while (true)
            {
                v1 += 1;
                v2 += 1;
                if (v1 > pg1.Maximum)
                    v1 = 0;
                if (v2 > pg2.Maximum)
                    v2 = 0;

                pg1.Value = v1;
                pg2.Value = v2;

                if (bLock1)
                    nud1.Value = pg1.Value;
                if (bLock2)
                    nud2.Value = pg2.Value;

                Thread.Sleep(100);
            }
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
