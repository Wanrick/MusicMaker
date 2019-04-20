using System.Collections.Generic;
using HonsMusicMaker.Entities;

namespace HonsMusicMaker.Processes
{
    public class MusicBuilder
    {
        private CustomBar[] Bars { get; set; }
        private int NumVoices { get; set; }

        public void Create(int numOfBars, int voices)
        {
            NumVoices = voices;
            var line = new CustomBar[numOfBars];
            /*for (var i = 0; i < line.Length; i++)
            {
                line[i] = new CustomBar();
                line[i].CreateBar(NumVoices);
            }*/
            Bars = line;
        }

        public CustomNote[][] GetMusic()
        {
            var voices = new List<List<CustomNote>>();
            var music = new CustomNote[NumVoices][];

            /*for (var i = 0; i < NumVoices; i++)
            {
                var voicesList = new List<CustomNote>();
                voices.Add(voicesList);
                foreach (var bar in Bars)
                {
                    foreach (var note in bar.Voices[i].Melody)
                    {
                        voices[i].Add(note);
                    }
                }

                music[i] = voices[i].ToArray();
            }
            */

            
            return music;
        }
    }
}