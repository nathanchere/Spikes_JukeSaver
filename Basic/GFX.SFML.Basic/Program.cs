using System;
using SFML.Graphics;
using SFML.Window;

namespace GFX.SFML.Basic
{
    public static class Program
    {
        const float pulseModifier = (float)Math.PI * 0.002f; // To convert a vealue from 0-1000 to a smooth 'pulse'

        private static void Main(string[] args)
        {
            var contextSettings = new ContextSettings {
                DepthBits = 32
            };
            var window = new RenderWindow(new VideoMode(640, 480), "JukeSaver spike: SFML Basic", Styles.Default, contextSettings);
            window.SetActive();

            window.Closed += OnClosed;
            window.KeyPressed += OnKeyPressed;

            int r = 0, g = 0, b = 0;
            var shape = new CircleShape() {
                Position = new Vector2f(320, 240),
            };

            while (window.IsOpen()) {
                window.DispatchEvents();
                window.Clear(new Color((byte)r, (byte)g, (byte)b));

                shape.Radius = (float)(80.0 + GetPulse() * 40.0);
                shape.Origin = new Vector2f(shape.Radius * 0.5f, shape.Radius * 0.5f);
                shape.Position = new Vector2f(320 - shape.Radius * 0.5f, 240 - shape.Radius * 0.5f);

                shape.FillColor = new Color(50, (byte)(160 + 80 * GetPulse()), (byte)(40 - (40 * GetPulse())));

                window.Draw(shape);
                window.Display();
            }
        }

        private static double GetPulse()
        {
            var tick = DateTime.Now.Millisecond;
            return Math.Sin(pulseModifier * tick);
        }

        static void OnClosed(object sender, EventArgs e)
        {
            var window = (Window)sender;
            window.Close();
        }

        static void OnKeyPressed(object sender, KeyEventArgs e)
        {
            var window = (Window)sender;
            if (e.Code == Keyboard.Key.Escape)
                window.Close();
        }

    }
}
