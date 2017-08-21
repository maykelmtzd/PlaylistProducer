using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistProducers
{
    public class PlaylistProducer
    {
        public List<TeamMember> TeamMembers;
        public TimeSpan MaxPlayListDuration;

        public PlayList NextPlaylist()
        {
            var playList = new PlayList(MaxPlayListDuration);
            var remainder = MaxPlayListDuration;
            foreach (var teamMember in TeamMembers)
            {
                var playlistSong = teamMember.NextSong(remainder);
                if (playlistSong != null)
                {
                    playList.AddPlaylistSong(playlistSong);
                    remainder -= playlistSong.Duration;
                }
            }

            return playList;
        }
    }
}
