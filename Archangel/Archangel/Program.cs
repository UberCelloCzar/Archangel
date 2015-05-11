#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace Archangel
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var game = new Game1();
            while (game.newGame == true)
            {
                using (game)
                    game.Run();
                if (game.newGame == true)
                {
                    game = new Game1();
                }
            }
        }
    }
#endif
}
