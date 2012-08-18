using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation.Media;
using Skewworks.Tinkr.Controls;

namespace Gadgetab.Forms
{
    abstract class GadgetabForm : Skewworks.Tinkr.Controls.Form
    {
        protected Skewworks.Tinkr.Controls.Panel _mainPanel;

        protected GadgetabForm(string Name) : base(Name)
        {
        }

        protected GadgetabForm(string Name, Color BackColor) : base(Name, BackColor)
        {
        }
/*
        protected void BuildForm()
        {
            // Add panel
            _mainPanel = new Panel("pnl1", 0, 0, 800, 480);
            _mainPanel.BackgroundImage = Resources.GetBitmap(Resources.BitmapResources.Zombies);
            AddControl(_mainPanel);

            // Add the app bar.
            _mainPanel.AddControl(BuildAppBar(Name));    
        }

        private Appbar BuildAppBar(string currentForm)
        {
            var appBar = new Appbar("appBar", _fntVerdana12, _fntVerdanaBold24);

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
    }
}
