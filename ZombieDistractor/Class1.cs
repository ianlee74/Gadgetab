using System;
using System.IO.Ports;
using System.Threading;
using Microsoft.SPOT;
using Pacman;
using Skewworks.Standards.NETMF.Applications;
using Skewworks.Standards.NETMF.Graphics;

using Skewworks.Tinkr;
using Skewworks.Tinkr.Controls;

namespace ZombieDistractor
{
    [Serializable]
    public class Class1 : MarshalByRefObject, IApplication
    {
        private Appbar _ai;
        private PacmanGame _pacmanGame = null;
        private Gadgeteer.Modules.GHIElectronics.Joystick _joystick;

        readonly Font _fntHuge = Resources.GetFont(Resources.FontResources.Amienne48AA);
        //readonly Font _fntVerdana12 = Resources.GetFont(Resources.FontResources.Verdana12);
        //readonly Font _fntArialBold11 = Resources.GetFont(Resources.FontResources.ArialBold11);
        //private readonly Font _fntVerdanaBold24 = Resources.GetFont(Resources.FontResources.VerdanaBold24);

        public void Main(string ApplicationPath, string[] Args)
        {
#if DEBUG
            Debug.Print("Main");
#endif
            const int gameWidth = 320;
            const int gameHeight = 240;

            var frm = new Form("zombie distractor");

            // Add panel
            var pnl = new Panel("pnl1", 0, 0, 800, 480);
            //pnl.BackgroundImage = Resources.GetBitmap(Resources.BitmapResources.Zombies);
            frm.AddControl(pnl);

            // Add the app bar.
            //pnl.AddControl(BuildAppBar(frm.Name));

            // Add a title.
            var title = new Label("lblTitle", "Zombie Distractor", _fntHuge, frm.Width / 2 - 140, 30) { Color = Gadgeteer.Color.Yellow };
            pnl.AddControl(title);

            Graphics.ActiveContainer = frm;

            // Add Pacman.
            var surface = Graphics.Screen;
            _pacmanGame = new PacmanGame(surface, frm.Width / 2 - gameWidth / 2, frm.Height / 2 - gameHeight / 2);
            _pacmanGame.InputManager.AddInputProvider(new GhiJoystickInputProvider(_joystick));
            _pacmanGame.Initialize();

        }

        public string SendMessage(object sender, string message, object args = null)
        {
            switch (message)
            {
                case "Appbar":
                    //_ai = (Appbar)sender;
                    //_ai.AddControl(_aiLink);
                    return "OK";
                case "Serial":
                    return "OK";
                case "BluetoothAppbarIcon":
                    //_aiBluetooth = (AppbarIcon)sender;
                    return "OK";
                case "FormBackgroundImage":
                    ((Panel)Graphics.ActiveContainer.GetChildByName("pnl1")).BackgroundImage = (Bitmap)sender;
                    return "OK";
            }
            return "OK";
        }

        public Image32 SizedApplicationIcon(IconSize RequestedSize)
        {
            switch (RequestedSize)
            {
                case IconSize.Large64x64:
                case IconSize.Medium48x48:
                    //return new Image32(Resources.GetBytes(Resources.BinaryResources.large));
                case IconSize.Normal32x32:
                case IconSize.Small16x16:
                    //return new Image32(Resources.GetBytes(Resources.BinaryResources.normal));
                default:
                    return null;
            }
        }

        public void Terminate()
        {
            if (_pacmanGame == null) return;
            _pacmanGame.Enabled = false;
            _pacmanGame.Stop();
            _pacmanGame = null;
        }

        public ProductDetails ProductDetails
        {
            get
            {
                return new ProductDetails("Zombie Distractor", string.Empty, "Houseoflees", "2012 Ian Lee, Sr.", "1.0.0.0");
            }
        }
    }
}
