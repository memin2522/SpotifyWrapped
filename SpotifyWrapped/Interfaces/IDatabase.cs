namespace ETLYelticDashboard.Interfaces
{
    public interface IDatabase
    {
        public void Initialize();
        public Task<bool> InsertData<T>(string table, List<T> objects);
        public Task<IEnumerable<T>> GetData<T>(string table, object parameter, string condition);
        public Task<IEnumerable<T>> GetData<T>(string table);
        public void TruncateTable(string table);
    }
}
