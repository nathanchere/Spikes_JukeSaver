using System;
using SFML.Graphics;
using SFML.Window;

namespace StarterKits.SFMLDotNet
{
    public static class Program
    {        
        private static void Main(string[] args)
        {
            var contextSettings = new ContextSettings { DepthBits = 32 };
            var window = new RenderWindow(new VideoMode(640, 480), "JukeSaver spike: SFML Basic", Styles.Default, contextSettings);
            window.SetActive();

            window.Closed += new EventHandler(OnClosed);
            window.KeyPressed += new EventHandler<KeyEventArgs>(OnKeyPressed);

            int r=0,g=0,b=0;
            
            while (window.IsOpen())
            {
                window.DispatchEvents();                
                window.Clear(new Color((byte)r, (byte)g, (byte)b));
                window.Display();
            }
        }

        static void OnClosed(object sender, EventArgs e)
        {
            var window = (Window)sender;
            window.Close();
        }
        
        static void OnKeyPressed(object sender, KeyEventArgs e)
        {
            var window = (Window)sender;
            if (e.Code == Keyboard.Key.Escape) window.Close();
        }        
        
    }
}
