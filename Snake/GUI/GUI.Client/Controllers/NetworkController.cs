// <authors>Kyleigh Messinger and Prachi Aswani</authors>
// <version>22 November, 2024</version>

using CS3500.Networking;
using GUI.Client.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GUI.Client.Controllers
{
    /// <summary>
    /// Handles communication between the client application 
    /// and the server, managing the network connection and 
    /// processing recieved data
    /// </summary>
    public class NetworkController
    {
        /// <summary>
        /// The network connection library to work with TCPClient
        /// </summary>
        private NetworkConnection connection = new NetworkConnection();

        /// <summary>
        /// The world in which the game is played
        /// </summary>
        private World world = new World();

        /// <summary>
        /// Flag for checking if it is connected
        /// </summary>
        private bool connected;

        /// <summary>
        /// Event triggered when a network error occurs
        /// </summary>
        public event Action<Exception> NetworkErrorOccurred;

        /// <summary>
        /// The ID of the current game session
        /// </summary>
        private int currentGameId;

        /// <summary>
        /// Establishes a connection to the server, initializes the world 
        /// dimensions and listens for updates
        /// </summary>
        /// <param name="playerName">The name of the player limited to 16 characters</param>
        /// <param name="port">The post number to connect to</param>
        /// <param name="server">The server address</param>
        /// <param name="world">The game world to update</param>
        /// <exception cref="Exception">Throws an exception for any network errors</exception>
        public void handleNetwork(string playerName, int port, string server, World world)
        {
            try
            {
                // Connect to the server
                connection.Connect(server, port);

                // Ensure the player's name is no longer than 16 characters
                if (playerName.Length > 16)
                {
                    Console.WriteLine("Player name exceeds the 16-character limit. It will be truncated.");
                    playerName = playerName.Substring(0, 16);
                }

                // Send the player name to the server
                connection.Send(playerName);

                // Receive and parse initial game data
                int.TryParse(connection.ReadLine(), out int result);
                string s = connection.ReadLine();

                lock (world)
                {
                    world.width = int.Parse(s);
                    world.height = int.Parse(s);
                    world.playerID = result;
                }

                // Start a new game in the database
                currentGameId = GameRecorder.StartGame();

                connected = true;

                try
                {
                    // Continuously read and process incoming data from the server
                    while (true)
                    {
                        string json = connection.ReadLine();
                        lock (world)
                        {
                            // Attempt to process wall-related data
                            try
                            {
                                if (json.Contains("wall"))
                                {
                                    Wall wall = JsonSerializer.Deserialize<Wall>(json);
                                    world.Walls[wall.wall] = wall;
                                    continue;
                                }
                            }
                            catch
                            {
                                Console.WriteLine("Error here");
                            }
                            // Attempt to process powerup-related updates
                            try
                            {
                                if (json.Contains("power"))
                                {
                                    Powerups powerup = JsonSerializer.Deserialize<Powerups>(json);
                                    if (powerup.died)
                                        powerup.timeLeft = FixedConstants.blastTime;
                                    world.Powerups[powerup.power] = powerup;
                                    continue;
                                }
                            }
                            catch 
                            {
                                Console.WriteLine("Error here1");
                            }

                            // Attempt to process snake-related updates
                            try
                            {
                                if (json.Contains("snake"))
                                {
                                    Snake snake = JsonSerializer.Deserialize<Snake>(json);
                                    world.Snakes[snake.snake] = snake;
                                    if (!world.Snakes.ContainsKey(snake.snake))
                                    {
                                        // New snake (player)
                                        GameRecorder.AddOrUpdatePlayer(
                                            currentGameId, snake.snake, snake.name, snake.score, DateTime.Now);
                                    }
                                    else
                                    {
                                        // Existing snake (update score)
                                        var existingSnake = world.Snakes[snake.snake];
                                        DateTime enterTime = DateTime.Now;
                                        if (snake.score > existingSnake.maxScore)
                                            existingSnake.maxScore = snake.score;

                                        GameRecorder.AddOrUpdatePlayer(
                                            currentGameId, snake.snake, snake.name, existingSnake.maxScore, enterTime);
                                    }

                                    // Handle player disconnection
                                    if (snake.dc)
                                    {
                                        GameRecorder.SetPlayerLeaveTime(snake.snake, currentGameId, DateTime.Now);
                                        world.Snakes.Remove(snake.snake);
                                    }
                                }
                            }
                            catch 
                            {
                                Console.WriteLine("Error here2");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Network error: {ex.Message}", ex);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                NetworkErrorOccurred?.Invoke(new Exception($"Failed to connect or process network data: {ex.Message}", ex));
            }
        }

        /// <summary>
        /// Processes a key press event and sends the corresponding 
        /// command to the server
        /// </summary>
        /// <param name="key">The key pressed by the user</param>
        public void handleKeyPress(string key)
        {
            ControlCommands controlCommand = new ControlCommands("none"); // Default to "none"

            // Update the moving direction based on the key pressed
            if (key == "s" || key == "ArrowDown")
                controlCommand.moving = "down";
            else if (key == "w" || key == "ArrowUp")
                controlCommand.moving = "up";
            else if (key == "d" || key == "ArrowRight")
                controlCommand.moving = "right";
            else if (key == "a" || key == "ArrowLeft")
                controlCommand.moving = "left";

            // Serialize the control command to JSON and send it
            string message = JsonSerializer.Serialize(controlCommand);
            connection.Send(message);
        }

        /// <summary>
        /// Disconnects from the server, ensuring resources are released
        /// </summary>
        public void Disconnect(World TheWorld)
        {
            try
            {
                // Close the network connection
                GameRecorder.EndGame(currentGameId);
                foreach (var snake in TheWorld.Snakes.Values)
                    GameRecorder.SetPlayerLeaveTime(snake.snake, currentGameId, DateTime.Now);
                connection.Disconnect();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during disconnection: {ex.Message}");
            }
        }
    }
}
