using System;
using Melanchall.DryWetMidi.MusicTheory;
using Melanchall.DryWetMidi.Smf;
using Melanchall.DryWetMidi.Smf.Interaction;
using Note = Melanchall.DryWetMidi.Smf.Interaction.Note;

namespace HonsMusicMaker
{
    public class GafNoteManager
    {
        private TrackChunk Track { get; set; }
        private TempoMap TrackTempo { get; set; }

        public GafNoteManager(TrackChunk trackChunk, TempoMap trackTempo)
        {
            Track = trackChunk;
            TrackTempo = trackTempo;
        }

        public void InitialiseNotes(int length, int lowerOctave, int upperOctave)
        {
            using (var melody = Track.ManageNotes())
            {
                var melodyNotes = melody.Notes;
                var ticks = TimeConverter.ConvertFrom(new BarBeatTimeSpan(0,0,1), TrackTempo); //1
                var beatCounter = TimeConverter.ConvertFrom(new BarBeatTimeSpan(0,1), TrackTempo); //96
                var startPoint = TimeConverter.ConvertFrom(new BarBeatTimeSpan(1,0), TrackTempo); //384
                long previousNoteLength = 0;
                
                
                for (var i = 0; i < length; i++)
                {
                 
                    var noteLengthMultiplier = 1;
                    var noteLengthInBeats = new BarBeatTimeSpan(0, 1).Multiply(noteLengthMultiplier);
                    var beat = startPoint + (beatCounter * i);
                    var noteTiming = previousNoteLength + beat;
                    var noteLength = LengthConverter.ConvertFrom
                    (
                        noteLengthInBeats,
                        time: noteTiming,
                        tempoMap: TrackTempo
                    );
                    previousNoteLength = previousNoteLength + noteLength;
                    
                    var nextNote = new Note
                    (
                        noteName: getNoteName(),
                        octave: getOctave(lowerOctave, upperOctave),
                        length: noteLength,
                        time: noteTiming
                    );
                    melodyNotes.Add(nextNote);
                }
            }
            
        }

        public void InitialiseChords(int length, int lowerOctave, int upperOctave)
        {
            
            using (var chordsManager = Track.ManageChords())
            {
                var chords = chordsManager.Chords;
                var beatCounter = TimeConverter.ConvertFrom(new BarBeatTimeSpan(0,1), TrackTempo);
                
                for (var i = 0; i < length; i++)
                {
                    var beat = beatCounter * i;
                    var notes = new[]
                    {
                        new Note(noteName: getNoteName(),
                            octave: getOctave(lowerOctave, upperOctave),
                            length: LengthConverter.ConvertFrom(
                                new BarBeatTimeSpan(0, 1),
                                time: beat,
                                tempoMap: TrackTempo
                            ),
                            time: beat
                        ),
                        new Note(noteName: getNoteName(),
                            octave: getOctave(lowerOctave, upperOctave),
                            length: LengthConverter.ConvertFrom(
                                new BarBeatTimeSpan(0, 1),
                                time: beat,
                                tempoMap: TrackTempo
                            ),
                            time: beat
                        )

                    };
                    chords.Add(new Chord(notes,
                        time: beat));

                }
            }
        }
        
        private int getOctave(int lowerBound, int upperBound)
        {
            var rnd = new Random();
            return rnd.Next(lowerBound, upperBound);
        }

        private MusicalTimeSpan getNoteLength()
        {
            var rnd = new Random();
            var noteLength = rnd.Next(0, 5);

            switch (noteLength)
            {
                case 0 : return MusicalTimeSpan.Whole;
                case 1 : return MusicalTimeSpan.Half;
                case 2 : return MusicalTimeSpan.Quarter;
                case 3 : return MusicalTimeSpan.Eighth;
                case 4 : return MusicalTimeSpan.Sixteenth;
                case 5 : return MusicalTimeSpan.ThirtySecond;
                default: return MusicalTimeSpan.SixtyFourth;
            }
        }

        private NoteName getNoteName()
        {
            var rnd = new Random();
            var noteNumber = rnd.Next(1, 13);

            switch (noteNumber)
            {
                case 1 : return NoteName.C;
                case 2 : return NoteName.CSharp;
                case 3 : return NoteName.D;
                case 4 : return NoteName.DSharp;
                case 5 : return NoteName.E;
                case 6 : return NoteName.F;
                case 7 : return NoteName.FSharp;
                case 8 : return NoteName.G;
                case 9 : return NoteName.GSharp;
                case 10 : return NoteName.A;
                case 11 : return NoteName.ASharp;
                case 12 : return NoteName.B;
                default: return NoteName.A;
            }
        }
    }
}