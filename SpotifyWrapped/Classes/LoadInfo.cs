using ETLYelticDashboard.Classes.Database.Generic;
using SpotifyWrapped.Controllers;
using SpotifyWrapped.Models;

namespace SpotifyWrapped.Classes
{
    public class LoadInfo
    {
        private readonly DatabaseIntermediary _dbSQL;

        public LoadInfo(DatabaseIntermediary databaseIntermediary)
        {
            _dbSQL = databaseIntermediary;
        }
        public async Task<bool> ProcessRawData(List<SpotifySong> extractedSongs)
        {
            try
            {
                await ProcessSongs(extractedSongs);
                await ProcessEvents(extractedSongs);
                return true;
            }
            catch
            {
                return false;
            }
            
        }

        public async Task ProcessSongs(List<SpotifySong> extractedSongs)
        {
            var currentSongsInDb = await _dbSQL.GetData<DbSong>("song");
            var currentSongs = currentSongsInDb.ToList();
            List<DbSong> newSongs = new List<DbSong>();

            foreach (SpotifySong extractedSong in extractedSongs)
            {
                var existingSong = currentSongs.FirstOrDefault(song => song.id_song == extractedSong.songId);
                if (existingSong == null && !newSongs.Any(song => song.id_song == extractedSong.songId))
                {
                    DbSong newSong = new DbSong
                    (
                        id_song: extractedSong.songId,
                        name: extractedSong.songName,
                        artist: extractedSong.artistName,
                        spotify_link: extractedSong.songUrl
                    );

                    newSongs.Add(newSong);
                }

            }

            if (newSongs.Any())
            {
                await _dbSQL.InsertData<DbSong>("song", newSongs);
            }
        }


        public async Task ProcessEvents(List<SpotifySong> extractedSongs)
        {
            List<DBActivity> activityList = new List<DBActivity>();
            foreach (var song in extractedSongs)
            {
                var activity = new DBActivity(song.listenDate, song.songId);
                activityList.Add(activity);
            }

            if (activityList.Any())
            {
                await _dbSQL.InsertData<DBActivity>("activity", activityList);
            }

        }
    }
}
