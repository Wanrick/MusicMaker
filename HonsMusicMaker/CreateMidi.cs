using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.MusicTheory;
using Melanchall.DryWetMidi.Smf;
using Melanchall.DryWetMidi.Smf.Interaction;
using TheoryNote = Melanchall.DryWetMidi.MusicTheory.Note;
using Note = Melanchall.DryWetMidi.Smf.Interaction.Note;

namespace HonsMusicMaker
{
    public static class CreateMidi
    {
        public static void CreateMidiFile(TempoMap tempo, TrackChunk music, string fileName)
        {
            var midiFile = new MidiFile();
            
            midiFile.Chunks.Add(music);

            midiFile.ReplaceTempoMap(tempo);
            midiFile.Write(fileName + ".mid", true);
        }

        /*private static TrackChunk BuildFirstTrackChunk(TempoMap tempoMap)
        {
            var trackChunk = new PatternBuilder()
                .SetNoteLength(MusicalTimeSpan.Eighth)
                .Note(Octave.Get(2).A)
                .Note(Octave.Get(3).A)
                .Repeat(2, 1)
                .SetNoteLength(new MetricTimeSpan(0, 0, 2))
                .SetRootNote(TheoryNote.Get(NoteName.C, 4))
                .Note(Interval.Nine) // C4 + 9
                .Note(Interval.Seven) // C4 + 7
                .Note(-Interval.One) // C4 - 1
                .Note(Interval.Three) // C4 + 3
                .StepForward(new BarBeatTimeSpan(2, 0))
                .Chord(new[]
                    {
                        Interval.Two, // B2 + 2
                        Interval.Three, // B2 + 3
                        -Interval.Twelve, // B2 - 12
                    },
                    Octave.Get(2).B)
                .StepForward(MusicalTimeSpan.Half.Triplet().SingleDotted())
                .Chord(new[]
                {
                    TheoryNote.Get(NoteName.B, 3), // B3
                    TheoryNote.Get(NoteName.C, 4), // C4
                })
                .Build()
                .ToTrackChunk(tempoMap);

            using (var timedEventsManager = trackChunk.ManageTimedEvents())
            {
                timedEventsManager.Events.AddEvent(
                    new ProgramChangeEvent((SevenBitNumber) 26), // 'Acoustic Guitar (steel)' in GM
                    time: 0);
            }

            return trackChunk;
        }

        private static TrackChunk BuildSecondTrackChunk(TempoMap tempoMap)
        {
            // We can create a track chunk and put events in it via its constructor

            var trackChunk = new TrackChunk(
                new ProgramChangeEvent((SevenBitNumber) 1)); // 'Acoustic Grand Piano' in GM

            // Insert notes via NotesManager class. See https://github.com/melanchall/drywetmidi/wiki/Notes
            // to learn more about managing notes

            using (var notesManager = trackChunk.ManageNotes())
            {
                var notes = notesManager.Notes;

                // Convert time span of 1 minute and 30 seconds to MIDI ticks. See
                // https://github.com/melanchall/drywetmidi/wiki/Time-and-length to learn more
                // about time and length representations and conversion between them

                var oneAndHalfMinute = TimeConverter.ConvertFrom(new MetricTimeSpan(0, 1, 30), tempoMap);

                // Insert two notes:
                // - A2 with length of 4/15 at 1 minute and 30 seconds from a file start
                // - B4 with length of 4 beats (1 beat = 1 quarter length at this case) at the start of a file

                notes.Add(new Note(noteName: NoteName.A,
                        octave: 2,
                        length: LengthConverter.ConvertFrom(new MusicalTimeSpan(4, 15),
                            time: oneAndHalfMinute,
                            tempoMap: tempoMap),
                        time: oneAndHalfMinute),
                    new Note(noteName: NoteName.B,
                        octave: 4,
                        length: LengthConverter.ConvertFrom(new BarBeatTimeSpan(0, 4),
                            time: 0,
                            tempoMap: tempoMap),
                        time: 0));
            }

            // Insert chords via ChordsManager class. See https://github.com/melanchall/drywetmidi/wiki/Chords
            // to learn more about managing chords

            using (var chordsManager = trackChunk.ManageChords())
            {
                var chords = chordsManager.Chords;

                // Define notes for a chord:
                // - C2 with length of 30 seconds and 600 milliseconds
                // - C#3 with length of 300 milliseconds

                var notes = new[]
                {
                    new Note(noteName: NoteName.C,
                        octave: 2,
                        length: LengthConverter.ConvertFrom(new MetricTimeSpan(0, 0, 30, 600),
                            time: 0,
                            tempoMap: tempoMap)),
                    new Note(noteName: NoteName.CSharp,
                        octave: 3,
                        length: LengthConverter.ConvertFrom(new MetricTimeSpan(0, 0, 0, 300),
                            time: 0,
                            tempoMap: tempoMap))
                };

                // Insert the chord at different times:
                // - at the start of a file
                // - at 10 bars and 2 beats from a file start
                // - at 10 hours from a file start

                chords.Add(new Chord(notes,
                        time: 0),
                    new Chord(notes,
                        time: TimeConverter.ConvertFrom(new BarBeatTimeSpan(10, 2),
                            tempoMap)),
                    new Chord(notes,
                        time: TimeConverter.ConvertFrom(new MetricTimeSpan(10, 0, 0),
                            tempoMap)));
            }

            return trackChunk;
        }*/
    }
}