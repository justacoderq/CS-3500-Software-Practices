// <authors>Kyleigh Messinger and Prachi Aswani</authors>
// <version>22 November, 2024</version>

using System.Text.Json.Serialization;

namespace GUI.Client.Models
{
    /// <summary>
    /// Represents a wall in the game, defined by two points and associated properties.
    /// </summary>
    public class Wall
    {
        /// <summary>
        /// The unique identifier for the wall.
        /// </summary>
        [JsonInclude]
        public int wall {  get; private set; }

        /// <summary>
        /// The starting point for the wall.
        /// </summary>
        [JsonInclude]
        private Point2D p1 { get; set; }

        /// <summary>
        /// The ending point for the wall.
        /// </summary>
        [JsonInclude]
        private Point2D p2 { get; set; }

        /// <summary>
        /// Indicates if the wall dementions have been initialized.
        /// </summary>
        private bool initialized = false;
        
        /// <summary>
        /// Store the minimum and maximum X and Y coordinates of the wall.
        /// </summary>
        private int minX, maxX, minY, maxY;

        /// <summary>
        /// Default constructor for the wall.
        /// </summary>
        public Wall()
        {
            wall = 0;
            p1 = new Point2D();
            p2 = new Point2D();
        }

        /// <summary>
        /// Constructor for the wall using specific given properties.
        /// </summary>
        /// <param name="_wall"> The unique identifier </param>
        /// <param name="_p1"> The starting point </param>
        /// <param name="_p2"> The ending point </param>
        public Wall(int _wall, Point2D _p1, Point2D _p2)
        {
            wall = _wall;
            p1 = _p1;
            p2 = _p2;
        }

        /// <summary>
        /// Updates the minimum and maximum X and Y coordinates based on the starting
        /// and ending points of the wall.
        /// </summary>
        private void Update()
        {
            initialized = true;
            // Calculates the minimum and maximum X coordinates
            if(p1.X <= p2.X)
            {
                minX = p1.X;
                maxX = p2.X;
            }
            else
            {
                minX = p2.X;
                maxX = p1.X;
            }

            // Calculates the minimum and maximum Y coordinates
            if (p1.Y <= p2.Y)
            {
                minY = p1.Y;
                maxY = p2.Y;
            }
            else
            {
                minY = p2.Y;
                maxY = p1.Y;
            }
        }
        /// <summary>
        /// Creates and returns a list of parts that represents a wall.
        /// </summary>
        /// <returns> A list of points objects represneting the wall parts. </returns>
        public List<Point2D> createParts()
        {
            if (!initialized)
            {
                Update();
            }

            List<Point2D> parts = new List<Point2D>();
            int x = minX;
            int y = minY;

            // If the wall is vertical
            if (minX == maxX)
            {
                while (y <= maxY)
                {
                    parts.Add(new Point2D(x, y));
                    y += FixedConstants.wallWidth;
                }
            }
            // If the wall is horizontal 
            else
            {
                while(x<= maxX)
                {
                    parts.Add(new Point2D(x, y));
                    x += FixedConstants.wallWidth;
                }
            }
            return parts;
        }
    }
}
