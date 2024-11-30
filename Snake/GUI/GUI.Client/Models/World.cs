// <authors>Kyleigh Messinger and Prachi Aswani</authors>
// <version>22 November, 2024</version>

namespace GUI.Client.Models
{
    /// <summary>
    /// Represents the game world, containing collections of snakes, walls, and powerups.
    /// </summary>
    public class World
    {
        /// <summary>
        /// Dictionary storing all the snakes in the game.
        /// </summary>
        public Dictionary<int, Snake> Snakes;
        
        /// <summary>
        /// Dictionary stroing all walls in the game.
        /// </summary>
        public Dictionary<int, Wall> Walls;

        /// <summary>
        /// Dictionary stroing all powerups in the game.
        /// </summary>
        public Dictionary<int,Powerups> Powerups;

        /// <summary>
        /// Property for the height of the world.
        /// </summary>
        public int height {  get; set; }

        /// <summary>
        /// Property for the width of the world.
        /// </summary>
        public int width { get; set; }

        /// <summary>
        /// Property for the unique identifier for the player, within the game world.
        /// </summary>
        public int playerID { get; set; }

        /// <summary>
        /// The default constructor for the game world.
        /// </summary>
        public World()
        {
            Snakes = new Dictionary<int, Snake>();
            Walls = new Dictionary<int, Wall>();
            Powerups = new Dictionary<int, Powerups>();
            // Default dimentions for the world.
            height = 1000;// Can be adjusted
            width = 1000;// Can be adjusted
        }

        /// <summary>
        /// Constructor to make a copy of another world.
        /// </summary>
        /// <param name="other"> The world to be copied. </param>
        public World(World other)
        {
            Snakes = new Dictionary<int, Snake>(other.Snakes);
            Walls = new Dictionary<int, Wall>(other.Walls);
            Powerups = new Dictionary<int, Powerups>(other.Powerups);
            height = other.height;
            width = other.width;
            playerID = other.playerID;
        }
    }
}
