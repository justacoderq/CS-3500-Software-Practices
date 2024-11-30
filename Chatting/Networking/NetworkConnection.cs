// <copyright file="NetworkConnection.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>
// <authors>Kyleigh Messinger and Prachi Aswani</authors>
// <version>8 November, 2024</version>

using System.Globalization;
using System.Net.Sockets;
using System.Text;
namespace CS3500.Networking;

/// <summary>
///   Wraps the StreamReader/Writer/TcpClient together so we
///   don't have to keep creating all three for network actions.
/// </summary>
public sealed class NetworkConnection : IDisposable
{
	/// <summary>
	///   The connection/socket abstraction
	/// </summary>
	private TcpClient _tcpClient = new();

	/// <summary>
	///   Reading end of the connection
	/// </summary>
	private StreamReader? _reader = null;

	/// <summary>
	///   Writing end of the connection
	/// </summary>
	private StreamWriter? _writer = null;

	/// <summary>
	///   Initializes a new instance of the <see cref="NetworkConnection"/> class.
	///   <para>
	///     Create a network connection object.
	///   </para>
	/// </summary>
	/// <param name="tcpClient">
	///   An already existing TcpClient
	/// </param>
	public NetworkConnection(TcpClient tcpClient)
	{
		_tcpClient = tcpClient;
		if (IsConnected)
		{
			// Only establish the reader/writer if the provided TcpClient is already connected.
			_reader = new StreamReader(_tcpClient.GetStream(), Encoding.UTF8);
			_writer = new StreamWriter(_tcpClient.GetStream(), Encoding.UTF8) { AutoFlush = true }; // AutoFlush ensures data is sent immediately
		}
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="NetworkConnection"/> class.
	///   <para>
	///     Create a network connection object.  The tcpClient will be unconnected at the start.
	///   </para>
	/// </summary>
	public NetworkConnection()
		: this(new TcpClient())
	{
	}

	/// <summary>
	/// Gets a value indicating whether the socket is connected.
	/// </summary>
	public bool IsConnected
	{
		get
		{
			try
			{
				// Checks if Tcpclient and underlying socket is not null and connected
				return _tcpClient != null && _tcpClient.Client != null && _tcpClient.Client.Connected;
			}
			catch
			{
				return false;
			}
		}
	}


	/// <summary>
	///   Try to connect to the given host:port. 
	/// </summary>
	/// <param name="host"> The URL or IP address, e.g., www.cs.utah.edu, or  127.0.0.1. </param>
	/// <param name="port"> The port, e.g., 11000. </param>
	public void Connect(string host, int port)
	{
		if (!IsConnected)
		{
			// Always create a new TcpClient for a new connection
			_tcpClient = new TcpClient();
			_tcpClient.Connect(host, port);
			// Initialize stream reader and writer for the connected client
			_reader = new StreamReader(_tcpClient.GetStream(), Encoding.UTF8);
			_writer = new StreamWriter(_tcpClient.GetStream(), Encoding.UTF8)
			{ AutoFlush = true };
		}
	}


	/// <summary>
	///   Send a message to the remote server.  If the <paramref name="message"/> contains
	///   new lines, these will be treated on the receiving side as multiple messages.
	///   This method should attach a newline to the end of the <paramref name="message"/>
	///   (by using WriteLine).
	///   If this operation can not be completed (e.g. because this NetworkConnection is not
	///   connected), throw an InvalidOperationException.
	/// </summary>
	/// <param name="message"> The string of characters to send. </param>
	public void Send(string message)
	{
		// Throws exception if the connection is not established
		if (!IsConnected || _writer == null)
		{
			throw new InvalidOperationException("Cannot be connected");
		}
		// Writes the message followed by a new line
		_writer.WriteLine(message);
	}


	/// <summary>
	///   Read a message from the remote side of the connection.  The message will contain
	///   all characters up to the first new line. See <see cref="Send"/>.
	///   If this operation can not be completed (e.g. because this NetworkConnection is not
	///   connected), throw an InvalidOperationException.
	/// </summary>
	/// <returns> The contents of the message. </returns>
	public string ReadLine()
	{
		// Throws exception if the connection is not established
		if (!IsConnected || _reader == null)
		{
			throw new InvalidOperationException("Cannot be connect");
		}

        try
        {
            string? line = _reader.ReadLine();

            // If the line is null, it means the end of the stream or the connection was lost
            if (line == null)
            {
                throw new InvalidOperationException("Connection lost or stream ended.");
            }

            // Return the line if it's successfully read
            return line;
        }
        catch (Exception ex)
        {
            // Catch any other exceptions that may occur
            Console.WriteLine($"Error while reading from connection: {ex.Message}");
            throw new InvalidOperationException("Error occurred while reading from the network connection.", ex);
        }
    }

	/// <summary>
	///   If connected, disconnect the connection and clean 
	///   up (dispose) any streams.
	/// </summary>
	public void Disconnect()
	{
		if (IsConnected)
		{
			// Disposes stream reader and writier and closes TcpClient
			_reader?.Dispose();
			_writer?.Dispose();
			_tcpClient.Close();
		}
	}

	/// <summary>
	///   Automatically called with a using statement (see IDisposable)
	/// </summary>
	public void Dispose()
	{
		Disconnect();
	}
}
