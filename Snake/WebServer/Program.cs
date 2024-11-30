using CS3500.Networking;
using MySql.Data.MySqlClient;
using System.Text;

public class WebServer
{

    private const int Port = 80; // Set a port for the server
    private const string ConnectionString = "server=atr.eng.utah.edu;database=u1479273;uid=u1479273;password=mysqlpassword";// change here to localhost and root

    private const string httpOkHeader = "HTTP/1.1 200 OK\r\n" +
        "Connectino: close\r\n" +
        "Content-Type: text/html; charset=UTF-8\r\n" + "\r\n";

    private const string httpBadHeader = "HTTP/1.1 404 Not Found\r\n" +
        "Connectino: close\r\n" +
        "Content-Type: text/html; charset=UTF-8\r\n" + "\r\n";

    public static void Main(string[] args)
    {
        Server.StartServer(HandleClient, Port);

        Console.Read(); // prevent main from returning
    }

    private static void HandleClient(NetworkConnection connection)
    {
        try
        {
            string request = connection.ReadLine();
            if (string.IsNullOrEmpty(request)) return;

            Console.WriteLine($"Request: {request}");
            string[] requestParts = request.Split(' ');
            if (requestParts.Length < 2) return;

            string route = requestParts[1];
            string response;
            if (route == ("/"))
            {
                response = httpOkHeader;
                response += GenerateHomePage();
            }
            else if (route == ("/games"))
            {
                response = GenerateGamesPage();
            }
            else if (route.StartsWith("/games?gid="))
            {
                int gameId = int.Parse(route.Split('=')[1]);
                response = GenerateGameDetailsPage(gameId);
            }
            else
            {
                response = httpBadHeader;
                response += Generate404Page();
            }

            connection.Send(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error handling client: {ex.Message}");
        }
        finally
        {
            connection.Disconnect();
        }
    }

    private static string GenerateHomePage()
    {
        return @"
<html>
<head>
  <style>
    body {
      font-family: Arial, sans-serif;
      background-color: #f9f9f9;
      margin: 0;
      padding: 0;
    }
    .container {
      text-align: center;
      padding: 50px;
    }
    h1, h3 {
      color: #333;
    }
    h1 {
      margin-bottom: 10px;
    }
    .link-container {
      margin-top: 20px;
    }
    a {
      text-decoration: none;
      font-size: 18px;
      color: #3498db;
      border: 2px solid #3498db;
      padding: 10px 20px;
      border-radius: 5px;
      transition: background-color 0.3s, color 0.3s;
    }
    a:hover {
      background-color: #3498db;
      color: white;
    }
    footer {
      margin-top: 50px;
      color: #777;
      font-size: 14px;
    }
  </style>
</head>
<body>
  <div class='container'>
    <h1>Snake Games Database</h1>
    <h3>Welcome to the hub for all your Snake game stats!</h3>
    <div class='link-container'>
      <a href='/games'>View Games</a>
    </div>
  </div>
  <footer>
    &copy; 2024 Snake Games Inc. All Rights Reserved.
  </footer>
</body>
</html>";
    }


    private static string GenerateGamesPage()
    {
        StringBuilder html = new();
        html.Append(@"
<html>
<head>
  <style>
    table {
      width: 80%;
      border-collapse: collapse;
      margin: 20px auto;
      font-family: Arial, sans-serif;
    }
    th, td {
      border: 1px solid #ddd;
      text-align: center;
      padding: 8px;
    }
    th {
      background-color: #f4f4f4;
      color: #333;
    }
    tr:nth-child(even) {
      background-color: #f9f9f9;
    }
    tr:hover {
      background-color: #f1f1f1;
    }
    a {
      color: #3498db;
      text-decoration: none;
    }
    a:hover {
      text-decoration: underline;
    }
  </style>
</head>
<body>
  <h3 style='text-align: center;'>Game Records</h3>
  <table>
    <thead>
      <tr><th>ID</th><th>Start</th><th>End</th></tr>
    </thead>
    <tbody>");

        using (MySqlConnection conn = new(ConnectionString))
        {
            conn.Open();
            MySqlCommand cmd = new("SELECT ID, StartTime, EndTime FROM Games", conn);
            using MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string start = reader.GetDateTime(1).ToString();
                string? end = null;
                if (!reader.IsDBNull(2))
                {
                    end = reader.GetDateTime(2).ToString();
                }
                html.Append($@"<tr><td><a href='/games?gid={id}'>{id}</a></td><td>{start}</td><td>{end}</td></tr>");
            }
        }

        html.Append("</tbody></table></body></html>");
        return FormatHttpResponse(html.ToString());
    }

    private static string GenerateGameDetailsPage(int gameId)
    {
        StringBuilder html = new();
        html.Append(@"
<html>
<head>
  <style>
    table {
      width: 90%;
      border-collapse: collapse;
      margin: 20px auto;
      font-family: Arial, sans-serif;
    }
    th, td {
      border: 1px solid #ddd;
      text-align: center;
      padding: 8px;
    }
    th {
      background-color: #f4f4f4;
      color: #333;
    }
    tr:nth-child(even) {
      background-color: #f9f9f9;
    }
    tr:hover {
      background-color: #f1f1f1;
    }
  </style>
</head>
<body>
  <table>
    <thead>
      <tr><th>Player ID</th><th>Player Name</th><th>Max Score</th><th>Enter Time</th><th>Leave Time</th></tr>
    </thead>
    <tbody>");

        html.Append($"<h3 style='text-align: center;'>Game Details for Game ID: {gameId}</h3>");

        using (MySqlConnection conn = new(ConnectionString))
        {
            conn.Open();
            MySqlCommand cmd = new($"SELECT ID, Name, MaxScore, EnterTime, LeaveTime FROM Players WHERE GameID = {gameId}", conn);
            using MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int playerId = reader.GetInt32(0);
                string name = reader.GetString(1);
                int maxScore = reader.GetInt32(2);
                string enterTime = reader.GetDateTime(3).ToString();
                string? leaveTime = null;
                if (!reader.IsDBNull(4))
                {
                    leaveTime = reader.GetDateTime(4).ToString();
                }
                html.Append($"<tr><td>{playerId}</td><td>{name}</td><td>{maxScore}</td><td>{enterTime}</td><td>{leaveTime}</td></tr>");
            }
        }

        html.Append("</tbody></table></body></html>");
        return FormatHttpResponse(html.ToString());
    }

    private static string Generate404Page()
    {
        return @"
<html>
<head>
  <style>
    body {
      font-family: Arial, sans-serif;
      background-color: #f9f9f9;
      margin: 0;
      padding: 0;
    }
    .container {
      text-align: center;
      padding: 50px;
    }
    h1 {
      font-size: 48px;
      color: #e74c3c;
      margin-bottom: 10px;
    }
    h3 {
      color: #555;
      margin-bottom: 30px;
    }
    .link-container {
      margin-top: 20px;
    }
    a {
      text-decoration: none;
      font-size: 18px;
      color: #3498db;
      border: 2px solid #3498db;
      padding: 10px 20px;
      border-radius: 5px;
      transition: background-color 0.3s, color 0.3s;
    }
    a:hover {
      background-color: #3498db;
      color: white;
    }
    footer {
      margin-top: 50px;
      color: #777;
      font-size: 14px;
    }
  </style>
</head>
<body>
  <div class='container'>
    <h1>404</h1>
    <h3>Oops! The page you're looking for doesn't exist.</h3>
    <div class='link-container'>
      <a href='/'>Return Home</a>
    </div>
  </div>
  <footer>
    &copy; 2024 Snake Games Inc. All Rights Reserved.
  </footer>
</body>
</html>";
    }


    //private static void SendResponse(NetworkConnection connection, string response)
    //{
    //    connection.Send(response);
    //}

    private static string FormatHttpResponse(string body)
    {
        string headers = $"HTTP/1.1 200 OK\r\nConnection: close\r\nContent-Type: text/html; charset=UTF-8\r\nContent-Length: {body.Length}\r\n\r\n";
        return headers + body;
    }
}
