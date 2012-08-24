// Copyright (c) 2012 Chris Taylor

using System.Reflection;
using Microsoft.SPOT;
using dotnetwarrior.NetMF.Diagnostics;
using Gadgeteer.Modules.GHIElectronics;

namespace Pacman
{
  public partial class Program
  {
    private PacmanGame _pacmanGame = null;

    void ProgramStarted()
    {      
      // Initialize the profiler
      // No auto snapshot, 1000 queued profile records, max 10 levels of nesting.
      // MiniProfiler.Initialize(0, 1000, 10);
  
      var surface = (Bitmap)(display.SimpleGraphics.GetType().GetField("_display", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(display.SimpleGraphics));
      
      _pacmanGame = new PacmanGame(surface);
      _pacmanGame.InputManager.AddInputProvider(new GhiJoystickInputProvider(joystick));
      _pacmanGame.Initialize();
    }    
  }
}
