using GUI.Client.Controllers;
using MySql.Data.MySqlClient;
using System;

public static class GameRecorder
{
    public static int StartGame()
    {
        using (var connection = DatabaseHelper.GetConnection())
        {
            string query = "INSERT INTO Games (StartTime) VALUES (@StartTime)";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@StartTime", DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"));
                cmd.ExecuteNonQuery();
                return (int)cmd.LastInsertedId;
            }
        }
    }

    public static void EndGame(int gameId)
    {
        using (var connection = DatabaseHelper.GetConnection())
        {
            string query = "UPDATE Games SET EndTime = @EndTime WHERE ID = @GameID";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@EndTime", DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"));
                cmd.Parameters.AddWithValue("@GameID", gameId);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public static void AddOrUpdatePlayer(int gameId, int playerId, string playerName, int score, DateTime enterTime)
    {
        using (var connection = DatabaseHelper.GetConnection())
        {
            string checkQuery = "SELECT MaxScore FROM Players WHERE ID = @PlayerID AND GameID = @GameID";
            using (var cmd = new MySqlCommand(checkQuery, connection))
            {
                cmd.Parameters.AddWithValue("@PlayerID", playerId);
                cmd.Parameters.AddWithValue("@GameID", gameId);

                var existingScore = cmd.ExecuteScalar();
                if (existingScore != null)
                {
                    int maxScore = Math.Max(score, Convert.ToInt32(existingScore));
                    string updateQuery = "UPDATE Players SET MaxScore = @MaxScore WHERE ID = @PlayerID AND GameID = @GameID";
                    using (var updateCmd = new MySqlCommand(updateQuery, connection))
                    {
                        updateCmd.Parameters.AddWithValue("@MaxScore", maxScore);
                        updateCmd.Parameters.AddWithValue("@PlayerID", playerId);
                        updateCmd.Parameters.AddWithValue("@GameID", gameId);
                        updateCmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    string insertQuery = "INSERT INTO Players (ID, Name, MaxScore, EnterTime, GameID) VALUES (@PlayerID, @Name, @MaxScore, @EnterTime, @GameID)";
                    using (var insertCmd = new MySqlCommand(insertQuery, connection))
                    {
                        insertCmd.Parameters.AddWithValue("@PlayerID", playerId);
                        insertCmd.Parameters.AddWithValue("@Name", playerName);
                        insertCmd.Parameters.AddWithValue("@MaxScore", score);
                        insertCmd.Parameters.AddWithValue("@EnterTime", enterTime.ToString("yyyy-MM-dd H:mm:ss"));
                        insertCmd.Parameters.AddWithValue("@GameID", gameId);
                        insertCmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }

    public static void SetPlayerLeaveTime(int playerId, int gameId, DateTime leaveTime)
    {
        using (var connection = DatabaseHelper.GetConnection())
        {
            string query = "UPDATE Players SET leaveTime = @leaveTime WHERE ID = @PlayerID AND GameID = @GameID";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@leaveTime", leaveTime.ToString("yyyy-MM-dd H:mm:ss"));
                cmd.Parameters.AddWithValue("@PlayerID", playerId);
                cmd.Parameters.AddWithValue("@GameID", gameId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
