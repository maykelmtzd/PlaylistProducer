using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlaylistProducers;
using Xunit;

namespace PlaylistProducerTests
{
    public class TeamMemberTests
    {
        [Fact]
        public void NextSongShouldFollowPrioritiesAsLongAsThereIsEnoughTime()
        {
            var teamMember = new TeamMember
            {
                Songs = new List<Song>
                {
                    new Song{ Name = "Song1", GroupName = "Group1", Duration = TimeSpan.FromSeconds(240), TimesPlayed = 0},
                    new Song{ Name = "Song2", GroupName = "Group2", Duration = TimeSpan.FromSeconds(200), TimesPlayed = 0}
                }
            };

            var expectedPlaylistSong = new PlaylistSong{ Name = "Song1", GroupName = "Group1", Duration = TimeSpan.FromSeconds(240) };

            var actual = teamMember.NextSong(TimeSpan.FromSeconds(500));
            Assert.Equal(expectedPlaylistSong, actual, new PlaylistSongComparer());

            expectedPlaylistSong = new PlaylistSong { Name = "Song2", GroupName = "Group2", Duration = TimeSpan.FromSeconds(200) };
            actual = teamMember.NextSong(TimeSpan.FromSeconds(500));
            Assert.Equal(expectedPlaylistSong, actual, new PlaylistSongComparer());
        }

        [Fact]
        public void NextSongShouldSkipPriorityWhenSongDoesNotFit()
        {
            var teamMember = new TeamMember
            {
                Songs = new List<Song>
                {
                    new Song{ Name = "Song1", GroupName = "Group1", Duration = TimeSpan.FromSeconds(240), TimesPlayed = 0},
                    new Song{ Name = "Song2", GroupName = "Group2", Duration = TimeSpan.FromSeconds(200), TimesPlayed = 0}
                }
            };

            var expectedPlaylistSong = new PlaylistSong { Name = "Song2", GroupName = "Group2", Duration = TimeSpan.FromSeconds(200) };
            var actual = teamMember.NextSong(TimeSpan.FromSeconds(220));

            Assert.Equal(expectedPlaylistSong, actual, new PlaylistSongComparer());
        }

        [Fact]
        public void NextSongShouldNotRepeatSongsIfThereIsTimeToPlayASongThatHasNotBeenPlayed()
        {
            var teamMember = new TeamMember
            {
                Songs = new List<Song>
                {
                    new Song{ Name = "Song1", GroupName = "Group1", Duration = TimeSpan.FromSeconds(240), TimesPlayed = 1},
                    new Song{ Name = "Song2", GroupName = "Group2", Duration = TimeSpan.FromSeconds(200), TimesPlayed = 0}
                }
            };

            var expectedPlaylistSong = new PlaylistSong { Name = "Song2", GroupName = "Group2", Duration = TimeSpan.FromSeconds(200) };
            var actual = teamMember.NextSong(TimeSpan.FromSeconds(500));

            Assert.Equal(expectedPlaylistSong, actual, new PlaylistSongComparer());
        }

        [Fact]
        public void NextSongShouldRepeatSongsIfThereIsNoTimeToPlayASongThatHasNotBeenPlayed()
        {
            var teamMember = new TeamMember
            {
                Songs = new List<Song>
                {
                    new Song{ Name = "Song1", GroupName = "Group1", Duration = TimeSpan.FromSeconds(200), TimesPlayed = 1},
                    new Song{ Name = "Song2", GroupName = "Group2", Duration = TimeSpan.FromSeconds(240), TimesPlayed = 0}
                }
            };

            var expectedPlaylistSong = new PlaylistSong { Name = "Song1", GroupName = "Group1", Duration = TimeSpan.FromSeconds(200) };
            var actual = teamMember.NextSong(TimeSpan.FromSeconds(220));

            Assert.Equal(expectedPlaylistSong, actual, new PlaylistSongComparer());
        }

        [Fact]
        public void NextSongShouldReturnASongThatHasBeenHeardFewerTimesThanAnyOtherConsideringPrioritiesAmongSongsWithSameTimesHeardAttribute()
        {
            var teamMember = new TeamMember
            {
                Songs = new List<Song>
                {
                    new Song{ Name = "Song1", GroupName = "Group1", Duration = TimeSpan.FromSeconds(200), TimesPlayed = 4, Priority = 5},
                    new Song{ Name = "Song2", GroupName = "Group2", Duration = TimeSpan.FromSeconds(240), TimesPlayed = 3, Priority = 4},
                    new Song{ Name = "Song3", GroupName = "Group3", Duration = TimeSpan.FromSeconds(150), TimesPlayed = 3, Priority = 3}
                }
            };

            var expectedPlaylistSong = new PlaylistSong { Name = "Song2", GroupName = "Group2", Duration = TimeSpan.FromSeconds(240) };
            var actual = teamMember.NextSong(TimeSpan.FromSeconds(500));

            Assert.Equal(expectedPlaylistSong, actual, new PlaylistSongComparer());
        }

        [Fact]
        public void NextSongShouldIncreaseTimesPlayedForReturnedSong()
        {
            var teamMember = new TeamMember
            {
                Songs = new List<Song>
                {
                    new Song{ Name = "Song1", GroupName = "Group1", Duration = TimeSpan.FromSeconds(240), TimesPlayed = 0},
                    new Song{ Name = "Song2", GroupName = "Group2", Duration = TimeSpan.FromSeconds(200), TimesPlayed = 0}
                }
            };

            var firstResult = teamMember.NextSong(TimeSpan.FromSeconds(500));
            Assert.Equal(1, teamMember.Songs[0].TimesPlayed);
            Assert.Equal(0, teamMember.Songs[1].TimesPlayed);

            var secondResult = teamMember.NextSong(TimeSpan.FromSeconds(260));
            Assert.Equal(1, teamMember.Songs[0].TimesPlayed);
            Assert.Equal(1, teamMember.Songs[1].TimesPlayed);
        }

        
    }
    
    public class PlaylistSongComparer : IEqualityComparer<PlaylistSong>
    {
        public bool Equals(PlaylistSong playlistSong1, PlaylistSong playlistSong2)
        {
            if (playlistSong1 == playlistSong2)
                return true;

            if (playlistSong1 == null || playlistSong2 == null)
                return false;

            if (playlistSong1.Name == playlistSong2.Name &&
                playlistSong1.GroupName == playlistSong2.GroupName &&
                playlistSong1.Duration == playlistSong2.Duration)
                return true;

            return false;
        }

        public int GetHashCode(PlaylistSong obj)
        {
            return obj.Name.GetHashCode() + obj.GroupName.GetHashCode();
        }
    }
}
