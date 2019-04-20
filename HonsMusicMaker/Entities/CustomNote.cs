using System;
using Melanchall.DryWetMidi.MusicTheory;
using Melanchall.DryWetMidi.Smf.Interaction;
using Note = Melanchall.DryWetMidi.MusicTheory.Note;

namespace HonsMusicMaker.Entities
{
    public class CustomNote
    {
        public static int Range { get; set; } = 4;
        public int Name { get; set; }
        public int Length { get; set; }
        public int Octave { get; set; }

        public CustomNote()
        {
            Name = GetValue();
            Length = GetLength(64);
            Octave = Name % Range + 3;
        }

        public CustomNote(int name)
        {
            Name = name;
            Length = 2;
        }
        
        public CustomNote(int name, int length)
        {
            Name = name;
            Length = length;
            Octave = Name % Range + 3;
        }

        public CustomNote(int name, int octaveShift, int length = 64)
        {
            Name = name;
            Octave = 2 + octaveShift;
        }
        
        public int GetAllowableRange()
        {
            return Range * 12 + 1;
        }
        
        public static int GetLength(int remainingLength)
        {
            return Utilities.GlobalRandom.Next(0, 7);
        }
        
        public static int GetValue()
        {
            return Utilities.GlobalRandom.Next(1, Range*12+1);
        }

        public static int GetOctave(int lowerBound, int upperBound)
        {
            return Utilities.GlobalRandom.Next(lowerBound, upperBound);
        }

        public static MusicalTimeSpan GetNoteLength(int noteLength = -1)
        {
            if (noteLength < 0)
            {
                noteLength = Utilities.GlobalRandom.Next(0, 5);
            }
            
            switch (noteLength)
            {
                case 16: return MusicalTimeSpan.Whole;
                case 8: return MusicalTimeSpan.Half;
                case 4: return MusicalTimeSpan.Quarter;
                case 2: return MusicalTimeSpan.Eighth;
                case 1: return MusicalTimeSpan.Sixteenth;
                default: return MusicalTimeSpan.SixtyFourth;
            }
        }

        public static NoteName GetNoteName(int noteNumber = -1)
        {
            if (noteNumber < 1)
            {
                noteNumber = Utilities.GlobalRandom.Next(1, 13);
            }

            switch (noteNumber % 12)
            {
                case 0: return NoteName.B;
                case 1: return NoteName.C;
                case 2: return NoteName.CSharp;
                case 3: return NoteName.D;
                case 4: return NoteName.DSharp;
                case 5: return NoteName.E;
                case 6: return NoteName.F;
                case 7: return NoteName.FSharp;
                case 8: return NoteName.G;
                case 9: return NoteName.GSharp;
                case 10: return NoteName.A;
                case 11: return NoteName.ASharp;
                default: return NoteName.A;
            }
        }

        public Note GetNote()
        {
            return Note.Get(GetNoteName(Name), Octave);
        }

        public static int GetOctave(int noteValue)
        {
            return (noteValue / 12) + 1;
        }
    }
}