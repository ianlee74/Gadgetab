using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation.Media;
using Pacman;
using Skewworks.Tinkr;
using Skewworks.Tinkr.Controls;

namespace Gadgetab.Forms
{
    public class ZombieDistractorForm : Skewworks.Tinkr.Controls.Form
    {
        public ZombieDistractorForm(string Name) : base(Name)
        {
        }

        public ZombieDistractorForm(string Name, Color BackColor) : base(Name, BackColor)
        {
        }
/*
        protected void Initialize()
        {
            const int gameWidth = 320;
            const int gameHeight = 240;

            var frm = new Form("zombie distractor");

            // Add panel
            var pnl = new Skewworks.Tinkr.Controls.Panel("pnl1", 0, 0, 800, 480);
            pnl.BackgroundImage = Resources.GetBitmap(Resources.BitmapResources.Zombies);
            frm.AddControl(pnl);

            // Add a title.
            var title = new Label("lblTitle", "Zombie Distractor", _fntHuge, frm.Width / 2 - 140, 30) { Color = Gadgeteer.Color.Yellow };
            pnl.AddControl(title);

            // Add the app bar.
            pnl.AddControl(BuildAppBar(frm.Name));

            TinkrCore.ActiveContainer = frm;

            // Add Pacman.
            //var surface = TinkrCore.Screen;
            var surface = new Skewworks.Tinkr.Controls.Picturebox("pacman", null, frm.Width / 2 - gameWidth / 2, frm.Height / 2 - gameHeight / 2);
            _pacmanGame = new PacmanGame(surface.Image);
            //_pacmanGame = new PacmanGame(surface, frm.Width / 2 - gameWidth / 2, frm.Height / 2 - gameHeight / 2);
            _pacmanGame.InputManager.AddInputProvider(new GhiJoystickInputProvider(joystick));
            _pacmanGame.Initialize();
        }
 */ 
    }
}
