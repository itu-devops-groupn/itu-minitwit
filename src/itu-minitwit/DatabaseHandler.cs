using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.Sqlite;

namespace itu_minitwit;
public class DatabaseHandler
{
    static readonly string connString = "Data Source=/tmp/minitwit.db";
    static readonly int PER_PAGE = 30;

    public static SqliteConnection ConnectDB()
    {
        var connection = new SqliteConnection(connString);
        connection.Open();
        return connection;
    }
    public static void InitDB()
    {
        using var connection = ConnectDB();
        string schema;
        using (var sr = new StreamReader("schema.sql"))
        {
            schema = sr.ReadToEnd();
        }

        using (var command = new SqliteCommand(schema, connection))
        {
            command.ExecuteNonQuery();
        }

        connection.Close();
    }


    public static List<Dictionary<string, string>> QueryDB(string query, params object[] args)
    {
        using var connection = ConnectDB();
        using var command = new SqliteCommand(query, connection);
        command.Parameters.AddRange(args);
        var result = new List<Dictionary<string, string>>();
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var dict = new Dictionary<string, string>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    dict[reader.GetName(i)] = reader.GetValue(i).ToString();
                }
                result.Add(dict);
            }
        }
        connection.Close();
        return result;
    }

    public static int? GetUserID(string username)
    {
        using var connection = ConnectDB();
        using var command = connection.CreateCommand();
        command.CommandText =
        @"
        SELECT user_id
        FROM user
        WHERE username = $username";
        command.Parameters.AddWithValue("$username", username);

        using var reader = command.ExecuteReader();
        return reader.Read() ? reader.GetInt32(0) : null;
    }

    public static string FormatDateTime(int timestamp)
    {
        return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            .AddSeconds(timestamp)
            .ToLocalTime()
            .ToString("yyyy-MM-dd @ HH:mm");
    }

    public static string GravatarUrl(string email, int size = 80)
    {
        var hash = MD5.HashData(Encoding.UTF8.GetBytes(email));
        var builder = new StringBuilder();
        foreach (var b in hash)
        {
            builder.Append(b.ToString("x2"));
        }
        return $"https://www.gravatar.com/avatar/{builder.ToString()}?d=identicon&s={size}";
    }
}
