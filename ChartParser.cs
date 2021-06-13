using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace FunnyFriday
{
    class Notes
    {
        public bool mustHitSection { get; set; }
        public int typeOfSection { get; set; }
        public int lengthInSteps { get; set; }
        public int bpm { get; set; }
        public bool changeBPM { get; set; }
        public bool altAnim { get; set; }
        public List<double[]> sectionNotes { get; set; }
    }

    class SongData
    {
        public string song { get; set; }
        public Notes[] notes { get; set; }
        public int bpm { get; set; }
        public int sections { get; set; }
        public bool needsVoices { get; set; }
        public string player1 { get; set; }
        public string player2 { get; set; }
        public int[] sectionLengths { get; set; }
        public double speed { get; set; }
    }

    class Song
    {
        public SongData song { get; set; }
    }

    class ChartParser
    {
        Song song;


        public ChartParser(string songName)
        {
            string fileName = songName + ".json";
            string jsonString = File.ReadAllText(fileName);
            song = JsonSerializer.Deserialize<Song>(jsonString);
        }

        public Song GetSong()
        {
            return song;
        }
    }
}
