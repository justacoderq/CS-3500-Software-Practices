// <authors>Kyleigh Messinger and Prachi Aswani</authors>
// <version>22 November, 2024</version>

namespace GUI.Client.Models
{
    /// <summary>
    /// Provides fixed values that are used throughout the game, such as time duration
    /// and object dimensions.
    /// </summary>
    public class FixedConstants
    {
        /// <summary>
        /// Time duration of blast effect of PowerUps, which is also used
        /// to show how long blast effects remain.
        /// </summary>
        public static int blastTime = 150;
        
        /// <summary>
        /// Used to specify the width of the wall objects of the game,
        /// used for collision detection.
        /// </summary>
        public static int wallWidth = 50;
    }
}
