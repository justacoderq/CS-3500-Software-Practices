// <copyright file="Server.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>
// <authors>Kyleigh Messinger and Prachi Aswani</authors>
// <version>8 November, 2024</version>

using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace CS3500.Networking;

/// <summary>
///   Represents a server task that waits for connections on a given
///   port and calls the provided delegate when a connection is made.
/// </summary>
public static class Server
{

	/// <summary>
	///   Wait on a TcpListener for new connections. Alert the main program
	///   via a callback (delegate) mechanism.
	/// </summary>
	/// <param name="handleConnect">
	///   Handler for what the user wants to do when a connection is made.
	///   This should be run asynchronously via a new thread.
	/// </param>
	/// <param name="port"> The port (e.g., 11000) to listen on. </param>
	public static void StartServer(Action<NetworkConnection> handleConnect, int port)
	{
		// Create a new TcpListener to listen for requests
		TcpListener listener = new TcpListener(IPAddress.Any, port);
		listener.Start();
		Console.WriteLine("Server started");

		while (true)
		{
			try
			{
				// Accept a pending client connection
				TcpClient client = listener.AcceptTcpClient();

				Console.WriteLine("A client is connected");

				// Wraps client in a network connection
				NetworkConnection connection = new NetworkConnection(client);

				// Create thread to handle specific client
				new Thread(() => handleConnect(connection)).Start();
			}
			catch
			{
				throw new Exception("Error accepting client connections");
			}
		}
	}
}
