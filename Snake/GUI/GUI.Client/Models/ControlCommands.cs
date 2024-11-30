// <authors>Kyleigh Messinger and Prachi Aswani</authors>
// <version>22 November, 2024</version>

namespace GUI.Client.Models
{
    /// <summary>
    /// Represents the control commands sent by the client to the server,
    /// indicating player movement direction
    /// </summary>
    public class ControlCommands
    {
        /// <summary>
        /// Specifies the direction of movement
        /// </summary>
        public string moving { get; set; }

        /// <summary>
        /// Initializes a new instance if the <see cref="ControlCommands"/> class with 
        /// a default movement of "none"
        /// </summary>
        public ControlCommands()
        {
            // Set the default movement direction to "none"
            moving = "none";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlCommands"/> class with 
        /// a default movement of "none"
        /// </summary>
        /// <param name="_moving"></param>
        public ControlCommands(string _moving)
        {
            // Assign the specified movement direction
            moving = _moving;
        }
    }
}
