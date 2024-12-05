using ETLYelticDashboard.Interfaces;

namespace ETLYelticDashboard.Classes.Database.Generic
{
    public class DatabaseIntermediary : IDatabase
    {
        private IDatabase databaseImplementation;

        public DatabaseIntermediary(IDatabase databaseImplementation)
        {
            this.databaseImplementation = databaseImplementation;
        }

        public async Task<IEnumerable<T>> GetData<T>(string table, object parameter, string condition)
        {
            var result = await databaseImplementation.GetData<T>(table, parameter, condition);
            return result;
        }

        public async Task<IEnumerable<T>> GetData<T>(string table)
        {
            var result = await databaseImplementation.GetData<T>(table);
            return result;
        }

        public async Task<bool> InsertData<T>(string table, List<T> objects)
        {
            var result = await databaseImplementation.InsertData(table, objects);
            return result;
        }

        public void Initialize()
        {
            databaseImplementation.Initialize();
        }

        public void TruncateTable(string table)
        {
            databaseImplementation.TruncateTable(table);
        }
    }
}
