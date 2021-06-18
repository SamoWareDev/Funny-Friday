using System;
using SFML;
using SFML.Window;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using System.Threading;

namespace FunnyFriday
{
    class Program
    {

        public static int SCREEN_WIDTH = 1280;
        public static int SCREEN_HEIGTH = 720;

        private static void WindowClosed(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Close();
        }

        static void Main(string[] args)
        {
            Clock deltaTime = new Clock();

            RenderWindow wnd = new RenderWindow(new VideoMode((uint)SCREEN_WIDTH, (uint)SCREEN_HEIGTH), "GetIt", Styles.Titlebar | Styles.Default);
            wnd.SetFramerateLimit(144);
            wnd.Closed += WindowClosed;

            var stateMachine = new StateMachine();
            stateMachine.AddStack(new Intro(wnd));

            while (wnd.IsOpen)
            {
                
                wnd.DispatchEvents();
                wnd.Clear(Color.Black);

                stateMachine.GetActiveStack().Update(ref deltaTime);
                stateMachine.GetActiveStack().InputHandling(stateMachine, ref deltaTime);
                stateMachine.GetActiveStack().Draw();

                wnd.Display();
            }
        }
    }
}
