using System;

namespace gamedev_attempt_01
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new GameCore())
                game.Run();
        }
    }
}
