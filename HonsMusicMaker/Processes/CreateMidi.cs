using System;
using HonsMusicMaker.Entities;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Smf;
using Melanchall.DryWetMidi.Smf.Interaction;
using TheoryNote = Melanchall.DryWetMidi.MusicTheory.Note;
using Note = Melanchall.DryWetMidi.Smf.Interaction.Note;

namespace HonsMusicMaker.Processes
{
    public static class CreateMidi
    {
        private static readonly SevenBitNumber Instrument = (SevenBitNumber) 1; // Piano
        private static readonly int BeatsPerMinute = 96;

        public static void CreateMidiFromIntBars(int[][][] bars, string fileName)
        {
            //Create Instrument Program
            var programInstrument = new ProgramChangeEvent(Instrument);
            //Create Music chunk for chosen instrument
            var musicChunk = new TrackChunk(programInstrument);
            //Create Tempo for Chunk
            var tempoMap = TempoMap.Create(Tempo.FromBeatsPerMinute(BeatsPerMinute));

            var noteManagerUtilities = new NoteManagerUtilities(musicChunk, tempoMap);

            var pattern = noteManagerUtilities.InitialiseChordsFromBars(bars, tempoMap);

            CreateMidiFile(tempoMap, pattern, fileName);
        }
        
        /*public static void CreateMidiFromResult(CustomNote[][] getMusic)
        {
            //Create Instrument Program
                var programInstrument = new ProgramChangeEvent(Instrument);
            //Create Music chunk for chosen instrument
                var musicChunk = new TrackChunk(programInstrument);
            //Create Tempo for Chunk
                var tempoMap = TempoMap.Create(Tempo.FromBeatsPerMinute(BeatsPerMinute));

            var noteManagerUtilities = new NoteManagerUtilities(musicChunk, tempoMap);

            var pattern = noteManagerUtilities.InitialisePattern(getMusic, tempoMap);

            CreateMidiFile(tempoMap, pattern, "Pattern01");
            for (var i = 0; i < getMusic.Length; i++)
            {
                for (var j = 0; j < getMusic[i].Length; j++)
                {
                    Console.Out.Write("Name: " + getMusic[i][j].Name + " - Length: " + getMusic[i][j].Length +
                                      " - Octave: " + getMusic[i][j].Octave + " | ");
                }

                Console.Out.WriteLine();
                Console.Out.WriteLine();
            }
        }

        public static void CreateMidiFile(TempoMap tempo, TrackChunk music, string fileName)
        {
            var midiFile = new MidiFile();

            midiFile.Chunks.Add(music);

            midiFile.ReplaceTempoMap(tempo);
            midiFile.Write(fileName + ".mid", false);
        }*/

        public static void CreateMidiFile(TempoMap tempo, Pattern music, string fileName)
        {
            var midiFile = music.ToFile(tempo);

            midiFile.Write(fileName + ".mid", true);
        }
    }
}