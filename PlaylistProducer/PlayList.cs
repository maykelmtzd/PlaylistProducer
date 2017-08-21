using System;
using System.Collections.Generic;

namespace PlaylistProducers
{
    public class PlayList
    {
        private List<PlaylistSong> _playlistSongs;
        private TimeSpan _maxTime;
        public PlayList(TimeSpan maxTime)
        {
            _playlistSongs = new List<PlaylistSong>();
            _maxTime = maxTime;
        }

        public void AddPlaylistSong(PlaylistSong playlistSong)
        {
            throw new NotImplementedException();
        }

        public void AddPlaylistSong(List<PlaylistSong> playlistSong)
        {
            throw new NotImplementedException();
        }

    }
}