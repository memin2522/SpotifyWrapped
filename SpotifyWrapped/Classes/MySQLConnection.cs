using MySql.Data.MySqlClient;

namespace SpotifyWrapped.Classes
{
    public class MySQLConnection
    {
        private readonly string cadenaConexion;

        public MySQLConnection(IConfiguration configuration)
        {
            // Lee directamente las credenciales de appsettings.json
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
            try
            {
                // conex.Open(); // Puedes abrir la conexión aquí si es necesario
            }
            catch (MySqlException e)
            {
                // Manejo de excepciones
                Exception exception = e;
            }
            return conex;
        }
    }
}
