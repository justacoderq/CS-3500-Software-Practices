// <authors>Kyleigh Messinger and Prachi Aswani</authors>
// <version>22 November, 2024</version>

using System.Text.Json.Serialization;

namespace GUI.Client.Models
{
    /// <summary>
    /// Represents a powerup in the game, which includes its type, location and status.
    /// </summary>
    public class Powerups
    {
        /// <summary>
        /// Remaining time for the powerup to be active.
        /// </summary>
        public int timeLeft = -1;

        /// <summary>
        /// The unique identifier for the powerup type
        /// </summary>
        [JsonInclude]
        public int power {  get; private set; }

        /// <summary>
        /// The location of the powerup, represented as a 2D point.
        /// </summary>
        [JsonInclude]
        public Point2D loc { get; private set; }

        /// <summary>
        /// Indicates if the powerup has been consumed or expired.
        /// </summary>
        [JsonInclude]
        public bool died { get; private set; }

        /// <summary>
        /// Zero parameter constructor that initializes the unique identifier, the 
        /// location and the status.
        /// </summary>
        public Powerups()
        {
            power = 0;
            loc = new Point2D();
            died = false;
        }

        /// <summary>
        /// Initializes a new instance of the class with specified parameters.
        /// </summary>
        /// <param name="_power"> The unique identifier </param>
        /// <param name="_loc"> The location </param>
        /// <param name="_died"> Indicates whether powerup has been consumed </param>
        public Powerups(int _power, Point2D _loc, bool _died)
        {
            power = _power;
            loc = _loc;
            died = _died;
        }
    }
}
