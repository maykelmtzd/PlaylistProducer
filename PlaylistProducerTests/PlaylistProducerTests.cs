using System;
using System.Collections.Generic;
using PlaylistProducers;
using Xunit;

namespace PlaylistProducerTests
{
    public class PlaylistProducerTests
    {
        [Fact]
        public void PlayListShouldContainSongsInPriorityOrderWhenPossible()
        {
            var teamMember1 = new TeamMember { Songs = new List<Song>
            {
                new Song{ Priority = 2, Name = "song2", Duration = TimeSpan.FromSeconds(200)},
                new Song{ Priority = 1, Name = "song1", Duration = TimeSpan.FromSeconds(180)},
                new Song{ Priority = 3, Name = "song3", Duration = TimeSpan.FromSeconds(220)}
            }};
            var teamMember2 = new TeamMember { Songs = new List<Song>
            {
                new Song{ Priority = 3, Name = "song6", Duration = TimeSpan.FromSeconds(300)},
                new Song{ Priority = 1, Name = "song4", Duration = TimeSpan.FromSeconds(150)},
                new Song{ Priority = 2, Name = "song5", Duration = TimeSpan.FromSeconds(230)}
            }};

            var playlistProducer = new PlaylistProducer{ TeamMembers = new List<TeamMember>{ teamMember1, teamMember2 }};
            var playlist = playlistProducer.NextPlaylist();

            var  list = new List<PlaylistSong>
            {
                new PlaylistSong {Name = "song1"},
                new PlaylistSong {Name = "song4"},
                new PlaylistSong {Name = "song2"},
                new PlaylistSong {Name = "song5"},
                new PlaylistSong {Name = "song3"},
                new PlaylistSong {Name = "song6"},
            };
            var expectedPlaylist = new PlayList(TimeSpan.FromSeconds(500));
            expectedPlaylist.AddPlaylistSong(list);

            Assert.Equal(expectedPlaylist, playlist);
        }
    }
}
