using MySql.Data.MySqlClient;

public static class DatabaseManager
{
    public static string host = "ger-by-mysql.rawpower.network";
    public static string username = "u13628_Q60mGZ8F0J";
    public static string password = "qLZelLY1!!X3vjnmkU8wvnrx";
    public static string tablename = "s13628_melony";

    public static MySqlConnection conn;

    public static void Initialize()
    {
        string conStr = $"server={host};uid={username};pwd={password};database={tablename}";

        try
        {
            conn = new MySqlConnection();
            conn.ConnectionString = conStr;
            conn.Open();
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}