// <authors>Kyleigh Messinger and Prachi Aswani</authors>
// <version>22 November, 2024</version>

using System.Text.Json.Serialization;

namespace GUI.Client.Models
{
    /// <summary>
    /// Represents a snake entity in the game. Including some attributes related to it.
    /// It also contains methods to identify snake parts.
    /// </summary>
    public class Snake
    {
        /// <summary>
        /// The down direction constant
        /// </summary>
        private Point2D down = new Point2D(0, 1);

        /// <summary>
        /// The up direction constant
        /// </summary>
        private Point2D up = new Point2D(0, -1);

        /// <summary>
        /// The right direction constant
        /// </summary>
        private Point2D right = new Point2D(1, 0);

        /// <summary>
        /// The left direction constant
        /// </summary>
        private Point2D left = new Point2D(-1, 0);

        /// <summary>
        /// The unique identifier for the snake.
        /// </summary>
        [JsonInclude]
        public int snake { get; private set; }

        /// <summary>
        /// The name of the snake.
        /// </summary>
        [JsonInclude]
        public string name { get; set; }

        /// <summary>
        /// A list of points representing the snakes body.
        /// </summary>
        [JsonInclude]
        public List<Point2D> body { get; private set; }

        /// <summary>
        /// The current direction of the snakes movement.
        /// </summary>
        [JsonInclude]
        public Point2D dir { get; private set; }

        /// <summary>
        /// The current score of the player, associated with the snake.
        /// </summary>
        [JsonInclude]
        public int score { get; set; }

        /// <summary>
        /// Indicates whether the snake is dead.
        /// </summary>
        [JsonInclude]
        public bool died { get; private set; }

        /// <summary>
        /// Indicates whether the snake is alive.
        /// </summary>
        [JsonInclude]
        public bool alive  { get; private set; }

        /// <summary>
        /// Indicates whether the snake is disconnected.
        /// </summary>
        [JsonInclude]
        public bool dc {  get; set; }

        /// <summary>
        /// Indicates if the snake has joined the game.
        /// </summary>
        [JsonInclude]
        private bool join { get; set; }

        [JsonIgnore]
        public int maxScore { get; set; }

        /// <summary>
        /// Represents the position of the snakes head. Default is (0,0) if no body is
        /// present.
        /// </summary>
        public Point2D position => body != null && body.Any() ? body.FirstOrDefault() : new Point2D(0, 0);

        /// <summary>
        /// Constructor that initializes new instance of the class with default attributes.
        /// </summary>
        public Snake()
        {
            snake = 0;
            name = string.Empty;
            body = new List<Point2D>();
            dir = new Point2D();
            score = 0;
            died = false; 
            alive = false;
            dc = false;
            join = false;
        }

        /// <summary>
        /// Constructor that initializes new instance of the class with specified attributes.
        /// </summary>
        /// <param name="_snake"> Unique identifier for the snake </param>
        /// <param name="_name"> name of the snake </param>
        /// <param name="_body"> initial body of the snake </param>
        /// <param name="_dir"> initial direction of the snake </param>
        /// <param name="_score"> score of the player </param>
        /// <param name="_died"> indicates dead/alive status </param>
        /// <param name="_alive"> indicates alive status </param>
        /// <param name="_dc"> indicates disconnected status </param>
        /// <param name="_join"> indicates joined status </param>
        public Snake(int _snake, string _name, List<Point2D> _body, Point2D _dir, int _score, bool _died, bool _alive, bool _dc, bool _join)
        {
            snake = _snake;
            name = _name;
            body = _body;
            dir = _dir; 
            score = _score; 
            died = _died;
            alive = _alive; 
            dc = _dc;
            join = _join;
        }

        /// <summary>
        /// Property for the head of the snake. (Last body point would be the head)
        /// </summary>
        public Point2D head
        {
            get => body.Count > 0 ? body.Last() : null;
            set
            {
                if (body.Count > 0)
                {
                    body[body.Count - 1] = value;
                }
            }
        }

        /// <summary>
        /// Property for the tail of the snake. (First body point would be the head)
        /// </summary>
        public Point2D tail
        {
            get => body.Count > 0 ? body.First() : null;
            set
            {
                if (body.Count > 0)
                {
                    body[0]=  value;
                }
            }
        }

        /// <summary>
        /// Gets mid point of the snakes body.(Second to last point of the body)
        /// </summary>
        public Point2D mid => body.Count > 1 ? body[body.Count - 2] : null;

        /// <summary>
        /// Computes the current movement direction of the snake.
        /// </summary>
        public Point2D direction
        {
            get
            {
                if(body.Count < 2) return new Point2D(0,0);
                Point2D tempHead = head;
                Point2D tempMid = mid;
                return new Point2D(tempHead.X-tempMid.X, tempHead.Y-tempMid.Y);
            }
        }
        
        /// <summary>
        /// Moves the snake one step forward in the current direction.
        /// </summary>
        public void moveToFront()
        {
            // Moves the head.
            head += direction;
            if (body.Count > 1)
            {
                // Update the tail position
                tail += body[1] - tail;
                if (tail == body[1])
                    // Remove redundant tail point
                    body.RemoveAt(0);
            }
        }

        /// <summary>
        /// Changes the direction of the snake based on the input key or arrow direction.
        /// </summary>
        /// <param name="direction"> the new direction as a string </param>
        public void directionChange(string direction)
        {
            // Adds segment in the "down" direction
            if ((direction == "s" || direction == "ArrowDown") && this.direction != down)
                body.Add(head + down);
            // Adds segment in the "up" direction
            else if ((direction == "w" || direction =="ArrowUp") && this.direction != up)
                body.Add(head + up);
            // Adds segment in the "right" direction
            else if ((direction == "d" || direction=="ArrowRight") && this.direction != right)
                body.Add(head + right);
            // Adds segment in the "left" direction
            else if ((direction == "a" || direction == "ArrowLeft") && this.direction != left)
                body.Add(head + left);
            
        }
    }
}
