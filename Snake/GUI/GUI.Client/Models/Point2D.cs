// <authors>Kyleigh Messinger and Prachi Aswani</authors>
// <version>22 November, 2024</version>

namespace GUI.Client.Models
{
    /// <summary>
    /// Represents a 2DPoint with X and Y cordinates that support basic
    /// vector operations.
    /// </summary>
    public class Point2D
    {
        /// <summary>
        /// The X coordinate of the point.
        /// </summary>
        public int X { get; set; }
        
        /// <summary>
        /// The Y coordinate of the point.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Basic constructor, initializes a new instance of the class.
        /// </summary>
        public Point2D()
        {
            X = -1;
            Y = -1;
        }

        /// <summary>
        /// Constructor the initializes a new instance, specifying the X and Y.
        /// </summary>
        /// <param name="x"> The X coordinate of the point </param>
        /// <param name="y"> The Y coordinate of the point </param>
        public Point2D(int x, int y)
        {
            X = x; 
            Y = y;
        }

        /// <summary>
        /// Constructor which copies a give Point2D object.
        /// </summary>
        /// <param name="other"> The Point whose coordinates will be copied </param>
        public Point2D(Point2D other)
        {
            X = other.X;
            Y = other.Y;
        }

        /// <summary>
        /// Overides the addition operator for Point2D, adds two instances of Points.
        /// </summary>
        /// <param name="a"> the first Point2D object to be added </param>
        /// <param name="b"> the second Point2D object to be added </param>
        /// <returns> A new Point2D object which is the sum of a and b </returns>
        public static Point2D operator +(Point2D a, Point2D b) => new Point2D(a.X + b.X, a.Y + b.Y);

        /// <summary>
        /// Overides the subtraction operator for Point2D, subtracts
        /// two instances of Points.
        /// </summary>
        /// <param name="a"> the first Point2D object to be subtracted </param>
        /// <param name="b"> the second Point2D object to be subtracted </param>
        /// <returns> A new Point2D object which is the difference of a and b </returns>
        public static Point2D operator -(Point2D a, Point2D b) => new Point2D(a.X - b.X, a.Y - b.Y);

        /// <summary>
        /// Normalizes the point returning a unit vector that is the same 
        /// direction as the original point.
        /// </summary>
        /// <returns> A unit vector if the length is non zero, else it returns zero vector</returns>
        public Point2D normalize()
        {
            // Calculates the length of the vector
            int length = (int)Math.Sqrt(X * X + Y * Y);
            return length>0? new Point2D(X/length, Y/length): new Point2D(0, 0);
        }
    }
}
