using System;
using HonsMusicMaker.Entities;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.MusicTheory;
using Melanchall.DryWetMidi.Smf;
using Melanchall.DryWetMidi.Smf.Interaction;
using Note = Melanchall.DryWetMidi.MusicTheory.Note;

namespace HonsMusicMaker.Processes
{
    public class NoteManagerUtilities
    {
        private TrackChunk Track { get; set; }
        private TempoMap TrackTempo { get; set; }

        public NoteManagerUtilities(TrackChunk trackChunk, TempoMap trackTempo)
        {
            Track = trackChunk;
            TrackTempo = trackTempo;
        }

        /*public void InitialiseNotes(int length, int lowerOctave, int upperOctave)
        {
            using (var melody = Track.ManageNotes())
            {
                var melodyNotes = melody.Notes;
                var ticks = TimeConverter.ConvertFrom(new BarBeatTimeSpan(0, 0, 1), TrackTempo); //1
                var beatCounter = TimeConverter.ConvertFrom(new BarBeatTimeSpan(0, 1), TrackTempo); //96
                var startPoint = TimeConverter.ConvertFrom(new BarBeatTimeSpan(1, 0), TrackTempo); //384
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
                        noteName: CustomNote.GetNoteName(),
                        octave: CustomNote.GetOctave(lowerOctave, upperOctave),
                        length: noteLength,
                        time: noteTiming
                    );
                    melodyNotes.Add(nextNote);
                }
            }
        }*/

        /*public void InitialiseChords(int length, int lowerOctave, int upperOctave)
        {
            using (var chordsManager = Track.ManageChords())
            {
                var chords = chordsManager.Chords;
                var beatCounter = TimeConverter.ConvertFrom(new BarBeatTimeSpan(0, 1), TrackTempo);

                for (var i = 0; i < length; i++)
                {
                    var beat = beatCounter * i;
                    var notes = new[]
                    {
                        new Note(noteName: CustomNote.GetNoteName(),
                            octave: CustomNote.GetOctave(lowerOctave, upperOctave),
                            length: LengthConverter.ConvertFrom(
                                new BarBeatTimeSpan(0, 1),
                                time: beat,
                                tempoMap: TrackTempo
                            ),
                            time: beat
                        ),
                        new Note(noteName: CustomNote.GetNoteName(),
                            octave: CustomNote.GetOctave(lowerOctave, upperOctave),
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
        }*/

        public Pattern InitialisePattern(int length, int lowerOctave, int upperOctave, SevenBitNumber instrument)
        {
            var melodyPattern = new PatternBuilder();
            melodyPattern.SetProgram(instrument);

            for (var i = 0; i < length; i++)
            {
                var noteValue = CustomNote.GetNoteName();
                var noteLength = CustomNote.GetNoteLength();
                melodyPattern.Note(noteName: noteValue, length: noteLength,
                    velocity: (SevenBitNumber.MaxValue));
                melodyPattern.StepForward(noteLength);
            }

            return melodyPattern.Build();
        }
        
        public Pattern InitialisePattern(CustomNote[][] getMusic, TempoMap tempoMap)
        {
            var builder = new PatternBuilder();
            builder.Anchor();
            for (var i = 0; i < getMusic.Length; i++)
            {
                for (var j = 0; j < getMusic[i].Length; j++)
                {
                    var noteValue = CustomNote.GetNoteName(getMusic[i][j].Name);
                    var noteLength = CustomNote.GetNoteLength(getMusic[i][j].Length);
                    var noteOctave = getMusic[i][j].Octave;
                    builder.SetOctave(noteOctave);
                    builder.Note(noteValue, noteLength);
                    builder.StepForward(noteLength);
                }

                builder.MoveToFirstAnchor();
            }

            return builder.Build();
        }
        
        public Pattern InitialiseChordsFromBars(int[][][] bars, TempoMap tempoMap)
        {
            var builder = new PatternBuilder();
            for (int bar = 0; bar < bars.Length; bar++)
            {
                for (int chord = 0; chord < bars[bar].Length; chord++)
                {
                    if (bars[bar][chord] == null)
                    {
                        continue;
                    }
                    //Assume chord structure of [note,note,note,note,length,chord number]
                    var cNoteLength = CustomNote.GetNoteLength(bars[bar][chord][4]);
                    builder.SetNoteLength(cNoteLength);
                    builder.Chord(new[]
                    {
                        Note.Get(CustomNote.GetNoteName(bars[bar][chord][0]), CustomNote.GetOctave(bars[bar][chord][0])),
                        Note.Get(CustomNote.GetNoteName(bars[bar][chord][1]), CustomNote.GetOctave(bars[bar][chord][1])),
                        Note.Get(CustomNote.GetNoteName(bars[bar][chord][2]), CustomNote.GetOctave(bars[bar][chord][2])),
                        Note.Get(CustomNote.GetNoteName(bars[bar][chord][3]), CustomNote.GetOctave(bars[bar][chord][3])),
                    });

                    Console.WriteLine(bars[bar][chord][0] + " | " + bars[bar][chord][1] + " | " + bars[bar][chord][2] +
                                " | " + bars[bar][chord][3] + " | " + bars[bar][chord][4] + " | " +
                                bars[bar][chord][5]);
                    
                    builder.StepForward(cNoteLength);
                }
            }

            return builder.Build();
        }
    }
}