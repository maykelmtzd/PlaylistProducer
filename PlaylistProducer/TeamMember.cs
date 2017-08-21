using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PlaylistProducers
{
    public class TeamMember
    {
        public List<Song> Songs { get; set; }

        public PlaylistSong NextSong(TimeSpan remainder)
        {
            var nextSong = Songs.GroupBy(song => song.TimesPlayed).OrderBy(group => group.Key)
                                            .SelectMany(group => group.AsEnumerable().OrderByDescending(song => song.Priority))
                                            .FirstOrDefault(song => song.Duration <= remainder);

            if (nextSong != null)
            {
                nextSong.TimesPlayed += 1;
                return nextSong;
            }

            return null;
        }
    }
}