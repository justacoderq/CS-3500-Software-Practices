// <copyright file="ChatServer.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>
// <authors>Kyleigh Messinger and Prachi Aswani</authors>
// <version>8 November, 2024</version>

using CS3500.Networking;
using System.Net.NetworkInformation;
using System.Text;

namespace CS3500.Chatting;

/// <summary>
/// A simple ChatServer that handles clients separately and replies with a static message.
/// </summary>
public partial class ChatServer
{
    /// <summary>
    /// Dictionary to keep track of clients and their associated usernames.
    /// </summary>
    private static Dictionary<NetworkConnection, string> _clients = new();

    /// <summary>
    /// The main program entry point.
    /// </summary>
    /// <param name="args">Command-line arguments (ignored).</param>
    /// <returns>A Task (not really used in this context).</returns>
    private static void Main(string[] args)
    {
        // Start the server on port 11000 and handle new connections using HandleConnect.
        Server.StartServer(HandleConnect, 11_000);
        Console.Read(); // Keep the program running.
    }


    /// <summary>
    /// When a new connection is established, enters a loop that receives from and
    /// replies to a client. Handles communication and broadcasting messages.
    /// </summary>
    private static void HandleConnect(NetworkConnection connection)
    {
        try
        {
            // Reads the initial message (expected to be the username) from the client.
            var username = connection.ReadLine();

            // Add the new client and their username to the dictionary.
            lock (_clients)
            {
                _clients[connection] = username;
            }

            // Continuously read and broadcast messages from the client.
            while (true)
            {
                var message = connection.ReadLine();
                // Broadcast the message to all clients (including the sender).
                BroadcastMessage($"{username}: {message}", connection);
            }
        }
        catch (Exception)
        {
            // Handle client disconnection.
            lock (_clients)
            {
                if (_clients.TryGetValue(connection, out var username))
                {
                    // Remove the disconnected client from the list.
                    connection.Disconnect();
                    _clients.Remove(connection);

                    // Log and notify other clients of the disconnection.
                    Console.WriteLine($"{username} has disconnected.");
                    BroadcastMessage($"{username} has left the chat.", connection);
                }
            }
        }
    }

    /// <summary>
    /// Broadcasts a message to all connected clients, including the sender.
    /// </summary>
    /// <param name="message">The message to broadcast.</param>
    /// <param name="sender">The client who sent the message. </param>
    private static void BroadcastMessage(string message, NetworkConnection sender)
    {
        lock (_clients)
        {
            foreach (var client in _clients.Keys)
            {
                // Send the message to all clients, including the sender.
                client.Send(message);
            }
        }
    }
}
