using System;

using System.Threading;
using Microsoft.SPOT;
using GT = Gadgeteer;

using Skewworks.Standards.NETMF.Applications;
using Skewworks.Standards.NETMF.Graphics;
using Skewworks.Tinkr;
using Skewworks.Tinkr.Controls;

namespace ZombieHealthMonitor
{
    [Serializable]
    public class Class1 : MarshalByRefObject, IApplication
    {
        private Appbar _ai;
        private GT.Timer _tweetTimer;

        readonly Font _fntHuge = Resources.GetFont(Resources.FontResources.Amienne48AA);
        //readonly Font _fntVerdana12 = Resources.GetFont(Resources.FontResources.Verdana12);
        //readonly Font _fntArialBold11 = Resources.GetFont(Resources.FontResources.ArialBold11);
        //private readonly Font _fntVerdanaBold24 = Resources.GetFont(Resources.FontResources.VerdanaBold24);

        public void Main(string applicationPath, string[] args)
        {
#if DEBUG
            Debug.Print("Main");
#endif
            var frm = new Form("zombie health monitor");

            // Add panel
            var pnl = new Skewworks.Tinkr.Controls.Panel("pnl1", 0, 0, 800, 480);
            pnl.BackgroundImage = Resources.GetBitmap(Resources.BitmapResources.Zombies);
            frm.AddControl(pnl);


            // Add a title.
            var title = new Label("lblTitle", "Zombie Health Monitor", _fntHuge, frm.Width / 2 - 140, 30) { Color = Gadgeteer.Color.Yellow };
            pnl.AddControl(title);

            // Add Heart Rate Graph.
            var graph = new Picturebox("heartRateGraph", null, 100, 200, 600, 200);
            pnl.AddControl(graph);

            Graphics.ActiveContainer = frm;

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
            if (_tweetTimer != null)
            {
                _tweetTimer.Stop();
                _tweetTimer = null;
            }
        }

        public ProductDetails ProductDetails
        {
            get
            {
                return new ProductDetails("Zombie Twit", string.Empty, "Houseoflees", "2012 Ian Lee, Sr.", "1.0.0.0");
            }
        }
    }
}
