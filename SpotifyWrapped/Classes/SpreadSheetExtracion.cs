using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Sheets.v4;
using System;

namespace SpotifyWrapped.Classes
{
    public class SpreadSheetExtracion
    {

        private readonly string credentialsPath = @"Credentials\meminvarios-4dcc1a5a93d1.json";
        private readonly string spreadsheetId = "1bAkP1PsTfn3PPYAsdsjowV_3lBsdfSD4lFIwY_ri1EA"; // Reemplaza con tu Spreadsheet ID
        private readonly string range = "register!A1:I500"; // Ajusta el rango según sea necesario

        public async Task<List<>> CheckAccesSpotifySpreadsheet()
        {
            try
            {
                var credential = GoogleCredential.FromFile(credentialsPath) // Reemplaza con la ruta correcta
                                                 .CreateScoped(SheetsService.Scope.Spreadsheets);

                var service = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Google Sheets API Example"
                });

                // Leer los datos
                SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, range);
                ValueRange response = request.Execute();

                // Obtener los valores
                var values = response.Values;

                // Verificar si hay datos
                if (values != null && values.Count > 0)
                {
                    Console.WriteLine("Datos obtenidos del rango especificado.");
                }
                else
                {
                    Console.WriteLine("No se encontraron datos en el rango especificado.");
                }
            }
            catch (Google.GoogleApiException ex)
            {
                // Capturar y mostrar el error de la API de Google
                Console.WriteLine($"Error de la API de Google: {ex.Message}");
                if (ex.Error != null)
                {
                    Console.WriteLine($"Código del error: {ex.Error.Code}");
                    Console.WriteLine($"Mensaje del error: {ex.Error.Message}");
                    Console.WriteLine($"Detalles del error: {string.Join("; ", ex.Error.Errors.Select(e => e.Message))}");
                }
            }
            catch (Exception ex)
            {
                // Capturar y mostrar errores generales
                Console.WriteLine($"Error general: {ex.Message}");
            }

        }
    }
}
    
        
