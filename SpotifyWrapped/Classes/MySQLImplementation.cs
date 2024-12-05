using Dapper;
using ETLYelticDashboard.Interfaces;
using MySql.Data.MySqlClient;
using System.Data;

namespace SpotifyWrapped.Classes
{
    public class MySQLImplementation : IDatabase
    {
        private readonly MySQLConnection _mySQLConnection;
        private MySqlConnection _db;
        public MySQLImplementation(MySQLConnection mySQL)
        { 
            _mySQLConnection = mySQL;
            Initialize();
        }

        public void Initialize()
        {
            _db = _mySQLConnection.GetConection();
        }

        public async Task<IEnumerable<T>> GetData<T>(string table, object parameter, string condition)
        {
            if (string.IsNullOrWhiteSpace(table) || string.IsNullOrWhiteSpace(condition))
                throw new ArgumentException("Table name and condition cannot be null or empty.");

            using (_db)
            {
                try
                {
                    await _db.OpenAsync();
                    var query = $"SELECT * FROM {table} WHERE {condition} = @Parameter;";
                    var result = await _db.QueryAsync<T>(query, new { Parameter = parameter });

                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving data from table '{table}': {ex.Message}", ex);
                }
                finally
                {
                    await _db.CloseAsync();
                }
            }
        }

        public async Task<IEnumerable<T>> GetData<T>(string table)
        {
            try
            {
                await _db.OpenAsync();
                var query = $"SELECT * FROM {table}";
                var result = await _db.QueryAsync<T>(query);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener datos de la tabla {table}: {ex.Message}");
                return null;
            }
            finally
            {
                if (_db.State == ConnectionState.Open)
                {
                    await _db.CloseAsync();
                }
            }
        }


        public async Task<bool> InsertData<T>(string table, List<T> objects)
        {
            using (_db) 
            {
                try
                {
                    await _db.OpenAsync();

                    var objType = typeof(T);
                    var properties = objType.GetProperties();

                    var columnNames = string.Join(", ", properties.Select(p => p.Name));

                    var valueRows = new List<string>();
                    var parameterIndex = 0;
                    var parameters = new List<MySqlParameter>();

                    foreach (var obj in objects)
                    {
                        var parameterNames = new List<string>();

                        foreach (var prop in properties)
                        {
                            var paramName = $"@p{parameterIndex++}";
                            parameterNames.Add(paramName);
                            var value = prop.GetValue(obj) ?? DBNull.Value;
                            parameters.Add(new MySqlParameter(paramName, value));
                        }

                        valueRows.Add($"({string.Join(", ", parameterNames)})");
                    }

                    var valuesQuery = string.Join(", ", valueRows);
                    var query = $"INSERT INTO {table} ({columnNames}) VALUES {valuesQuery};";

                    using (var command = new MySqlCommand(query, _db))
                    {
                        command.Parameters.AddRange(parameters.ToArray());
                        await command.ExecuteNonQueryAsync();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al insertar datos en la tabla {table}: {ex.Message}");
                    return false;
                }
                finally
                {
                    if (_db.State == System.Data.ConnectionState.Open)
                    {
                        await _db.CloseAsync();
                    }
                }
            }
        }

        public void TruncateTable(string table)
        {
            throw new NotImplementedException();
        }
    }
}
