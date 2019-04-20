using System;
using Melanchall.DryWetMidi.MusicTheory;

namespace HonsMusicMaker.Entities
{
    public class MyBar
    {
        private static readonly int MinNoteLength = 8;
        private static readonly int LengthIndexInChord = 4;
        internal static readonly int ChordNumberIndex = 5;
        private static readonly int[][] PossibleChords = new int[][]
        {
            new int[] {1,   5,  8,  13, 0, 1}, // Index 4 indicates Length (initialised in Bar mutation)
            new int[] {3,   6,  10, 15, 0, 2}, // Index 4 indicates Length (initialised in Bar mutation)
            new int[] {5,   8,  12, 17, 0, 3}, // Index 4 indicates Length (initialised in Bar mutation)
            new int[] {6,   10, 13, 18, 0, 4}, // Index 4 indicates Length (initialised in Bar mutation)
            new int[] {8,   12, 15, 20, 0, 5}, // Index 4 indicates Length (initialised in Bar mutation)
            new int[] {10,  13, 17, 22, 0, 6}, // Index 4 indicates Length (initialised in Bar mutation)
            new int[] {12,  15, 19, 24, 0, 7}, // Index 4 indicates Length (initialised in Bar mutation)
        };
        private static readonly int[] PossibleLengths = new int[]
        {
            16, 8, 4, 2, 1
        };

        private static int _remainingLength;
        private static int[][][] _thisBar;
        private static int[] _previousChord;

        
        public static int[][][] MakeBars(int howMany)
        {
            _thisBar = new int[howMany][][];
            for (var i = 0; i < howMany; i++)
            {
                _thisBar[i] = GetBarNotes();
            }

            return _thisBar;
        }

        public static int[][] GetBarNotes()
        {
            var barNotes = new int[MinNoteLength][];
            _remainingLength = MinNoteLength;
            for (var i = 0; i < MinNoteLength; i++)
            {
                barNotes[i] = GetChord();
                _previousChord = barNotes[i];
                i += barNotes[i][LengthIndexInChord]-1;
            }

            return barNotes;
        }

        private static int[] GetChord()
        {
            var chordValid = false;
            var chord = new int[6];
            
            while (!chordValid)
            {
                var chordIndex = Utilities.GlobalRandom.Next(0, 6);
                chord = (int[]) PossibleChords[chordIndex].Clone();
                MutateChord(chord);
                chordValid = ValidateChordRelationship(chord);
            }
            
            SetChordLength(chord);
            FixAvailableLengthInBar(chord);
            
            return chord;
        }

        private static bool ValidateChordRelationship(int[] chord)
        {
            if (_previousChord == null)
            {
                return true;
            }
            var result = false;
            result = ChordDistancesValid(chord);
            return result;
        }

        private static bool NoParallelMovement(int[] chord)
        {
            var result = true;
            for (int i = 0; i < 3; i++)
            {
                var currentChordDistance = chord[i + 1] - chord[i];
                var previousChordDistance = _previousChord[i + 1] - _previousChord[i];
                if (currentChordDistance == previousChordDistance && previousChordDistance == 7)
                {
                    result = false;
                }
                if (currentChordDistance == previousChordDistance && previousChordDistance == 12)
                {
                    result = false;
                }
            }

            return result;
        }

        private static bool ChordDistancesValid(int[] chord)
        {
            if (chord[0] > _previousChord[1])
            {
                return false;
            }
            
            if (chord[1] > _previousChord[2])
            {
                return false;
            }
            
            if (chord[2] > _previousChord[3])
            {
                return false;
            }
            

            if (chord[1] < _previousChord[0])
            {
                return false;
            }
            
            if (chord[2] < _previousChord[1])
            {
                return false;
            }
            
            if (chord[3] < _previousChord[2])
            {
                return false;
            }
            
            return true;
        }

        private static void MutateChord(int[] chord)
        {
            SpreadChord(chord);
            SortChord(chord);
        }

        private static void FixAvailableLengthInBar(int[] chord)
        {
            _remainingLength = _remainingLength - chord[LengthIndexInChord];
        }

        private static void SetChordLength(int[] chord)
        {
            var availableSpaceInBar = _remainingLength;
            var lengthIndexBound = -1;
            if (availableSpaceInBar == 0)
            {
                return;
            }
            else if (availableSpaceInBar < 2)
            {
                lengthIndexBound = 4;
            }
            else if (availableSpaceInBar < 3)
            {
                lengthIndexBound = 3;
            }
            else if (availableSpaceInBar < 5)
            {
                lengthIndexBound = 2;
            }
            else if (availableSpaceInBar < 9)
            {
                lengthIndexBound = 1;
            }
            else
            {
                lengthIndexBound = 0;
            }

            var thisChordLength = Utilities.GlobalRandom.Next(lengthIndexBound, 4);
            chord[LengthIndexInChord] = PossibleLengths[thisChordLength];
        }

        private static void SortChord(int[] chord)
        {
            var chordLastIndex = 4;
            for (var i = 0; i < chordLastIndex; i++)
            {
                for (var sort = 0; sort < chordLastIndex - 1; sort++) {
                    if (chord[sort] > chord[sort + 1]) {
                        var temp = chord[sort + 1];
                        chord[sort + 1] = chord[sort];
                        chord[sort] = temp;
                    }
                }   
            }
        }

        private static void SpreadChord(int[] chord)
        {
            for (var note = 0; note < 4; note++)
            {
                var set = false;
                while (!set)
                {
                    var voice = Utilities.GlobalRandom.Next(1, 4) * 12;
                    var tempNote = chord[note];
                    chord[note] = voice + chord[note];
                    if (chord[note] <= 40 || chord[note] >= 6)
                    {
                        set = true;
                    }
                    else
                    {
                        chord[note] = tempNote;
                    }
                }
            }
        }
    }
}