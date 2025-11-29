using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Sheets.v4;
using System;
using SpotifyWrapped.Models;
using Microsoft.AspNetCore.Http;

namespace SpotifyWrapped.Classes
{
    public class DataExtraction
    {

        private readonly string credentialsPath = @"Credentials\SheetsServiceAccount.json";
        private readonly string spreadsheetId = "1Rkvlr6JMwN41WfsTKMQj70TcrwSn5QZuUVcOtB6oOQY";
        private readonly string range = "Hoja 1!A1:I2000";

        private SheetsService _sheetsService;

        private string[] spreadsheetIds = new string[]
        {
            "1Rkvlr6JMwN41WfsTKMQj70TcrwSn5QZuUVcOtB6oOQY",
        };

        private SheetsService GetGoogleSheetService()
        {
            if (_sheetsService == null)
            {
                var credential = GoogleCredential.FromFile(credentialsPath)
                                                 .CreateScoped(SheetsService.Scope.Spreadsheets);

                var service = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Google Sheets API Example"
                });

                _sheetsService = service;
                
            }
            return _sheetsService;
        }

        public async Task<List<SpotifySong>> ExtractionCicle()
        {
            var extractedContent = await ExtractSpotifySongFromSpreadsheet();
            var processSongs = ProcessSpotifySongs(extractedContent);
            Console.WriteLine("Extraction Completed");

            return processSongs;
        }


        public async Task<List<IList<object>>> ExtractSpotifySongFromSpreadsheet()
        {
            var combinedValues = new List<IList<object>>();

            try
            {
                var service = GetGoogleSheetService();

                foreach (string id in spreadsheetIds)
                {
                    try
                    {
                        Console.WriteLine($"Processing Spreadsheet ID: {id}");

                        var request = service.Spreadsheets.Values.Get(id, range);
                        var response = request.Execute();
                        var values = response.Values ?? new List<IList<object>>();

                        if (values.Count > 0)
                        {
                            Console.WriteLine($"Data found in spreadsheet {id}: {values.Count} rows");
                            combinedValues.AddRange(values);
                        }
                        else
                        {
                            Console.WriteLine($"No data found in spreadsheet {id}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing spreadsheet {id}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fatal error: {ex.Message}");
            }

            Console.WriteLine($"Total rows combined: {combinedValues.Count}");
            return combinedValues;
        }

        public List<SpotifySong> ProcessSpotifySongs(IList<IList<object>> values)
        {
            string format = "MMMM d, yyyy 'at' hh:mmtt";
            var spotifySongs = values
                .Select(row => new SpotifySong
                (
                    DateTime.ParseExact(row[0].ToString(), format, System.Globalization.CultureInfo.InvariantCulture),
                    row[1].ToString(),
                    row[2].ToString(),
                    row[3].ToString(),
                    row[4].ToString()
                ))
                .ToList();

            return spotifySongs;
        }
        public async Task CleanSpreadsheet()
        {
            var service = GetGoogleSheetService();

            // 1. Eliminar valores
            var clearRequest = new ClearValuesRequest();
            await service.Spreadsheets.Values.Clear(clearRequest, spreadsheetId, "Hoja 1").ExecuteAsync();

            // 2. Reducir la hoja a 1 fila y 1 columna (resetea el contador real)
            var request = new Request
            {
                UpdateSheetProperties = new UpdateSheetPropertiesRequest
                {
                    Properties = new SheetProperties
                    {
                        SheetId = 0,  // si la hoja es la primera
                        GridProperties = new GridProperties
                        {
                            RowCount = 1,
                            ColumnCount = 1
                        }
                    },
                    Fields = "gridProperties(rowCount,columnCount)"
                }
            };

            var batch = new BatchUpdateSpreadsheetRequest
            {
                Requests = new List<Request> { request }
            };

            await service.Spreadsheets.BatchUpdate(batch, spreadsheetId).ExecuteAsync();
        }
    }
}
    
        
