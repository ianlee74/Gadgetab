﻿
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Gadgeteer Designer.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Gadgeteer;
using GTM = Gadgeteer.Modules;

namespace Gadgetab
{
    public partial class Program : Gadgeteer.Program
    {
        // GTM.Module definitions
        Gadgeteer.Modules.GHIElectronics.Display_CP7 display_CP7;
        Gadgeteer.Modules.GHIElectronics.Bluetooth bluetooth;
        Gadgeteer.Modules.GHIElectronics.Joystick joystick;
        Gadgeteer.Modules.GHIElectronics.RFID rfid;

        public static void Main()
        {
            //Important to initialize the Mainboard first
            Mainboard = new GHIElectronics.Gadgeteer.FEZHydra();			

            Program program = new Program();
            program.InitializeModules();
            program.ProgramStarted();
            program.Run(); // Starts Dispatcher
        }

        private void InitializeModules()
        {   
            // Initialize GTM.Modules and event handlers here.		
            bluetooth = new GTM.GHIElectronics.Bluetooth(4);
		
            display_CP7 = new GTM.GHIElectronics.Display_CP7(10, 11, 12, 5);
		
            rfid = new GTM.GHIElectronics.RFID(7);
		
            joystick = new GTM.GHIElectronics.Joystick(13);

        }
    }
}
