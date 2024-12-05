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

        private readonly string credentialsPath = @"Credentials\meminvarios-4dcc1a5a93d1.json";
        private readonly string spreadsheetId = "1bAkP1PsTfn3PPYAsdsjowV_3lBsdfSD4lFIwY_ri1EA";
        private readonly string range = "register!A1:I500";

        public async Task<List<SpotifySong>> ExtractionCicle()
        {
            var extractedContent = await ExtractSpotifySongFromSpreadsheet();
            var processSongs = ProcessSpotifySongs(extractedContent);
            Console.WriteLine("Extraction Completed");

            return processSongs;
        }


        public async Task<IList<IList<object>>> ExtractSpotifySongFromSpreadsheet()
        {
            try
            {
                var credential = GoogleCredential.FromFile(credentialsPath)
                                                 .CreateScoped(SheetsService.Scope.Spreadsheets);

                var service = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Google Sheets API Example"
                });

                SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, range);
                ValueRange response = request.Execute();
                var values = response.Values;

                if (values != null && values.Count > 0)
                {
                    // Intentar borrar los datos
                    var clearRequest = new ClearValuesRequest();
                    var clearResponse = await service.Spreadsheets.Values.Clear(clearRequest, spreadsheetId, range).ExecuteAsync();


                    Console.WriteLine("Data found, returning info");
                    return values;
                }
                else
                {
                    Console.WriteLine("No data found, returning empty list.");
                    return new List<IList<object>>();
                }
            }
            catch (Google.GoogleApiException ex)
            {
                Console.WriteLine($"Error in the Google API: {ex.Message}");
                if (ex.Error != null)
                {
                    Console.WriteLine($"Error Code: {ex.Error.Code}");
                    Console.WriteLine($"Error Message: {ex.Error.Message}");
                    Console.WriteLine($"Details: {string.Join("; ", ex.Error.Errors.Select(e => e.Message))}");
                }
                return new List<IList<object>>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error: {ex.Message}");
                return new List<IList<object>>();
            }

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
    }
}
    
        
