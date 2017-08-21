using System;

namespace PlaylistProducers
{
    public class PlaylistSong
    {
        public string Name;
        public string GroupName;
        public TimeSpan Duration;

        public static implicit operator PlaylistSong(Song song)
        {
            return new PlaylistSong
            {
                GroupName = song.GroupName,
                Name = song.Name,
                Duration = song.Duration
            };
        }
    }
}