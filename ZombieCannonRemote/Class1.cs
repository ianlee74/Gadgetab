using System;

using System.Threading;
using Microsoft.SPOT;

using Skewworks.Standards.NETMF.Applications;
using Skewworks.Standards.NETMF.Graphics;
using Skewworks.Tinkr;
using Skewworks.Tinkr.Controls;

namespace ZombieCannonRemote
{
    [Serializable]
    public class Class1 : MarshalByRefObject, IApplication
    {
        private Appbar _ai;

        readonly Font _fntHuge = Resources.GetFont(Resources.FontResources.Amienne48AA);
        //readonly Font _fntVerdana12 = Resources.GetFont(Resources.FontResources.Verdana12);
        //readonly Font _fntArialBold11 = Resources.GetFont(Resources.FontResources.ArialBold11);
        //private readonly Font _fntVerdanaBold24 = Resources.GetFont(Resources.FontResources.VerdanaBold24);

        public void Main(string ApplicationPath, string[] Args)
        {
#if DEBUG
            Debug.Print("Main");
#endif
            var frm = new Form("zombie cannon remote");

            // Add panel
            var pnl = new Skewworks.Tinkr.Controls.Panel("pnl1", 0, 0, 800, 480);
            //pnl.BackgroundImage = Resources.GetBitmap(Resources.BitmapResources.Zombies);
            frm.AddControl(pnl);
            
            // Add a title.
            var title = new Label("lblTitle", "Zombie Cannon Remote", _fntHuge, frm.Width / 2 - 175, 30) { Color = Gadgeteer.Color.Yellow };
            pnl.AddControl(title);

            // Add a fire button.
            var pic1 = new Skewworks.Tinkr.Controls.Panel("launchBtn", frm.Width / 2 - 150, frm.Height / 2 - 150, 300, 300);
            pic1.BackgroundImage = Resources.GetBitmap(Resources.BitmapResources.LaunchButton);
            pic1.Tap += (sender, id) => Debug.Print("FIRE!");
            pnl.AddControl(pic1);

            Skewworks.Tinkr.Graphics.ActiveContainer = frm;
            Thread.Sleep(1000);
        }

        public string SendMessage(object sender, string message, object args = null)
        {
            switch (message)
            {
                case "Appbar":
                    _ai = (Appbar)sender;
                    //_ai.AddControl(_aiLink);
                    Graphics.ActiveContainer.AddControl(_ai);
                    return "OK";
                case "Serial":
                    return "OK";
                case "BluetoothAppbarIcon":
                    //_aiBluetooth = (AppbarIcon)sender;
                    return "OK";
                case "FormBackgroundImage":
                    //((Panel) Graphics.ActiveContainer.GetChildByName("pnl1")).BackgroundImage = (Bitmap)sender;
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

        }

        public ProductDetails ProductDetails
        {
            get
            {
                return new ProductDetails("Zombie Cannon Remote", string.Empty, "Houseoflees", "2012 Ian Lee, Sr.", "1.0.0.0");
            }
        }
/*
        private void AuthenticationForm()
        {
            var frm = new Form("authentication");

            // Add panel
            var pnl = new Skewworks.Tinkr.Controls.Panel("pnl1", 0, 0, 800, 480) { BackgroundImage = Resources.GetBitmap(Resources.BitmapResources.Zombies) };
            frm.AddControl(pnl);

            // Add the app bar.
            pnl.AddControl(BuildAppBar(frm.Name));

            // Add a title.
            var title = new Label("lblTitle", "Authentication is Required", _fntHuge, frm.Width / 2 - 175, frm.Height / 2 - 5) { Color = Gadgeteer.Color.Red };
            pnl.AddControl(title);

            rfid.CardIDRecieved += (sender, id) =>
            {
                title.Text = "Welcome back, Mr. Lee.";
                Thread.Sleep(2000);
                ZombieCannonRemoteForm();
            };
            Graphics.ActiveContainer = frm;
        }
 */
    }
}
