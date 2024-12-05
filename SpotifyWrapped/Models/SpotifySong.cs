namespace SpotifyWrapped.Models
{
    public class SpotifySong
    {
        public DateTime listenDate { get; set; }
        public string songName { get; set; }
        public string artistName { get; set; }
        public string songId { get; set; }
        public string songUrl { get; set; }

        public SpotifySong(DateTime listenDate, string songName, string artistName, string songId, string songUrl)
        {
            this.listenDate = listenDate;
            this.songName = songName;
            this.artistName = artistName;
            this.songId = songId;
            this.songUrl = songUrl;
        }
    }
}
