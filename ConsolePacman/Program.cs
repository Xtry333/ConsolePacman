using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsolePacman
{
    class Ch
    {
        static public string Dot = "•";
        static public string Full = "█";
        static public string Half = "▒";
        static public string Empty = " ";
    }

    class Ghost
    {

    }

    class FrameControl
    {
        private Stopwatch fpsDelay = new Stopwatch();
        private Stopwatch fpsSw = new Stopwatch();
        
        public double fpsD = 0;
        public int fps = 0;
        public int maxFps = 0;
        private int frame = 0;

        public void Start() { fpsSw.Start(); }
        public void Reset() { fpsSw.Reset(); }

        public void Delay(int delay, bool inTicks = false)
        {
            fpsDelay.Start();
            if (inTicks)
            {
                while (fpsDelay.ElapsedTicks < delay)
                {
                    // Not much...
                }
            }
            else
            {
                while (fpsDelay.ElapsedMilliseconds < delay)
                {
                    // Not much...
                }
            }
            fpsDelay.Reset();
        }

        public void Update()
        {
            frame++;
            if (fpsSw.ElapsedMilliseconds > 1000)
            {                
                fpsD = 1.0 * frame / fpsSw.ElapsedMilliseconds * 1000;
                fps = Convert.ToInt32(fpsD+0.5);
                frame = 0;
                fpsSw.Reset();
            }
            if (maxFps > 0) Limit();
        }

        public void Limit()
        {
            while ((1.0 * frame / fpsSw.ElapsedMilliseconds * 1000) + 1 > maxFps)
            {
                Delay(1);
            }
        }
    }
    
    class Program
    {
        static FrameControl fps = new FrameControl();
        static Random rnd = new Random();
        static bool printAt(int x, int y, string str) // Sets Cursor at x, y and writes given string
        {
            if ((x >= 0) && (x <= Console.WindowWidth - 1) && (y >= 0) && (y <= Console.WindowHeight - 1))
            {
                Console.SetCursorPosition(x, y);
                Console.Write(str);
                return true;
            } else return false;
        }

        static void Main()
        {
            Console.Title = "Pac Man";
            Console.CursorVisible = false;

            bool quit = false;
            fps.maxFps = 0;
            while (!quit)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.Escape) { quit = true; }
                    if (keyInfo.Key == ConsoleKey.C) { Console.Clear(); }
                }
                fps.Start();
                fps.Update();
                printAt(rnd.Next(0, Console.WindowWidth - 1), rnd.Next(0, Console.WindowHeight), rnd.Next(0, 9).ToString());
                //printAt(Console.WindowWidth - 1, Console.WindowHeight, "a");
                printAt(0, 2, "Fps: " + fps.fps + "/" + fps.maxFps + "   \n");
            }
            Console.WriteLine("Exiting...");
            fps.Delay(100);
        }
    }
}
