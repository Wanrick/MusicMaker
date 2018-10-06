using System;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Smf;
using Melanchall.DryWetMidi.Smf.Interaction;

namespace HonsMusicMaker
{
    class Program
    {
        private static readonly SevenBitNumber Instrument = (SevenBitNumber) 1; // Piano
        private static readonly int BeatsPerMinute = 96;
        
        static void Main(string[] args)
        {
            //Create Instrument Program
            var programInstrument = new ProgramChangeEvent(Instrument);
            
            //Create Music chunk for chosen instrument
            var musicChunk= new TrackChunk(programInstrument);
            
            //Create Tempo for Chunk
            var tempoMap = TempoMap.Create(Tempo.FromBeatsPerMinute(BeatsPerMinute));
            
            GafNoteManager noteManager = new GafNoteManager(musicChunk, tempoMap);
            noteManager.InitialiseNotes(10, 2, 6);
            //noteManager.InitialiseChords(10, 2, 6);
            
            CreateMidi.CreateMidiFile(tempoMap, musicChunk, "Try01");
        }
    }
}