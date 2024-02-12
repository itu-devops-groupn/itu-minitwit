// public class DatabaseHandler
// {
//     static readonly string connString = "Data Source=minitwit.db";

//     public static SqliteConnection ConnectDB()
//     {
//         var connection = new SqliteConnection(connString);
//         connection.Open();
//         return connection;
//     }
//     public static void InitDB()
//     {
//         using var connection = ConnectDB();
//         string schema;
//         using (var sr = new StreamReader("schema.sql"))
//         {
//             schema = sr.ReadToEnd();
//         }

//         using (var command = new SqliteCommand(schema, connection))
//         {
//             command.ExecuteNonQuery();
//         }

//         connection.Close();
//     }


//     public static List<Dictionary<string, object>> QueryDB(string query, params object[] args)
//     {
//         using var connection = ConnectDB();
//         using var command = new SqliteCommand(query, connection);
//         var result = new List<Dictionary<string, object>>();
//         using (var reader = command.ExecuteReader())
//         {
//             while (reader.Read())
//             {
//                 var dict = new Dictionary<string, object>();
//                 for (int i = 0; i < reader.FieldCount; i++)
//                 {
//                     dict[reader.GetName(i)] = reader.GetValue(i);
//                 }
//                 result.Add(dict);
//             }
//         }
//         connection.Close();
//         return result;
//     }

//     public static int? GetUserID(string username)
//     {
//         var connection = ConnectDB();

//         var command = connection.CreateCommand();
//         command.CommandText =
//         @"
//         SELECT user_id
//         FROM user
//         WHERE username = $username";
//         command.Parameters.AddWithValue("$username", username);

//         using var reader = command.ExecuteReader();
//         return reader.Read() ? reader.GetInt32(0) : null;
//     }

//     public static string FormatDateTime(int timestamp)
//     {
//         return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
//             .AddSeconds(timestamp)
//             .ToLocalTime()
//             .ToString("yyyy-MM-dd @ HH:mm");
//     }

//     public static string GravatarUrl(string email, int size = 80)
//     {
//         using var md5 = MD5.Create();
//         var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(email));
//         var builder = new StringBuilder();
//         foreach (var b in hash)
//         {
//             builder.Append(b.ToString("x2"));
//         }
//         return $"https://www.gravatar.com/avatar/{builder.ToString()}?d=identicon&s={size}";
//     }
// }
