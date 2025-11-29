using MySql.Data.MySqlClient;

namespace SpotifyWrapped.Classes
{
    public class MySQLConnection
    {
        private readonly string cadenaConexion;

        public MySQLConnection(IConfiguration configuration)
        {
            var servidor = configuration["MySQL:Server"];
            var database = configuration["MySQL:Database"];
            var usuario = configuration["MySQL:User"];
            var password = configuration["MySQL:Password"];
            var puerto = configuration["MySQL:Port"];

            cadenaConexion = $"Server={servidor};Port={puerto};Database={database};User ID={usuario};Password={password};CharSet=utf8mb4;";
        }

        public MySqlConnection GetConection()
        {
            MySqlConnection conex = new MySqlConnection(cadenaConexion);
            return conex;
        }
    }
}
