namespace SpotifyWrapped.Models
{
   public class DbSong
    {
        public DbSong(string id_song, string name, string artist, string spotify_link)
        {
            this.id_song = id_song;
            this.name = name;
            this.artist = artist;
            this.spotify_link = spotify_link;
        }

        public string id_song { get; set; }
        public string name { get; set; }
        public string artist { get; set; }
        public string spotify_link { get; set; }

    }

    public class DBActivity
    {
        public DBActivity(DateTime listen_date, string id_song)
        {
            this.listen_date = listen_date;
            this.id_song = id_song;
        }

        public DateTime listen_date { get; set; }
        public string id_song { get; set; }
    }
}
