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
        int x = Program.Random.Next(0, Console.WindowWidth/2);
        int y = Program.Random.Next(0, Console.WindowHeight);
        int direction = Program.Random.Next(0, 3);
        int timer = 0;

        public void Update()
        {
            if (timer > 0) timer--;
            if (timer <= 0)
            {
                int rnd = Program.Random.Next(0, 100);
                int c = 33;
                if (rnd > 100 - c/2) { direction++; }
                if (rnd < c/2) { direction--; }

                timer = Program.Random.Next(0, 20);
            }

            if (direction > 3) { direction -= 4; }
            if (direction < 0) { direction += 4; }
            Program.printAt(2 * x, y, Ch.Empty + Ch.Empty);
            switch (direction)
            {
                case 0: x++; break;
                case 1: y++; break;
                case 2: x--; break;
                case 3: y--; break;
            }
            if (x < 0) { direction += 2; }
            if (2 * x > Console.WindowWidth - 1) { direction += 2; }
            if (y < 0) { direction += 2; }
            if (y > Console.WindowHeight - 1) { direction += 2; }
            Program.printAt(2 * x, y, Ch.Half + Ch.Half);
        }
    }

    class PacMan
    {
        public void Update()
        {

        }
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
        static FrameControl Fps = new FrameControl();
        public static Random Random = new Random();
        static PacMan PacMan = new PacMan();
        static Ghost[] Ghost = new Ghost[5];
        
        public static string[,] boardTiles = new string[Console.WindowWidth - 1, Console.WindowHeight - 1];

        public static bool printAt(int x, int y, string str, bool addToBoard = false) // Sets Cursor at x, y and writes given string
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

            for (int i = 0; i < Ghost.Length; ++i) { Ghost[i] = new Ghost(); }

            bool quit = false;
            Fps.maxFps = 15;
            while (!quit)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.Escape) { quit = true; }
                    if (keyInfo.Key == ConsoleKey.C) { Console.Clear(); }
                }
                Fps.Start();

                PacMan.Update();
                foreach (Ghost Gh in Ghost) { Gh.Update(); }
                //for (int i = 0; i < Ghost.Length; i++) { Ghost[i].Update(); }

                //Fps.Delay(66);
                //Thread.Sleep(120);
                Fps.Update();
                //Console.Clear();
                printAt(0, 2, "Fps: " + Fps.fps + "   \n");
                //printAt(Random.Next(0, Console.WindowWidth - 1), Random.Next(0, Console.WindowHeight), Random.Next(0, 9).ToString());
                //printAt(Console.WindowWidth - 1, Console.WindowHeight, "a");
            }
            Console.WriteLine("Exiting...");
            Fps.Delay(100);
        }
    }
}
