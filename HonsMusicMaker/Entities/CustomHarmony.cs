namespace HonsMusicMaker.Entities
{
    public class CustomHarmony
    {
        private const int MinorThird = 3;
        private const int MajorThird = 4;
        private const int Fifth = 7;
        private const int Octave = 12;

        public int[] GetRandomMajorHarmony()
        {
            var chosenIndex = Utilities.GlobalRandom.Next(0, 2);
            int[][] options =
            {
                new int[] {1, 5, 8, 13},
                new int[] {6, 10, 13, 17},
                new int[] {8, 12, 15, 20}
            };
            return options[chosenIndex];
        }

        public int[] GetRandomMinorHarmony()
        {
            var chosenIndex = Utilities.GlobalRandom.Next(0, 2);
            int[][] options =
            {
                new int[] {3, 6, 10, 15},
                new int[] {5, 8, 12, 17},
                new int[] {10, 13, 17, 22}
            };
            return options[chosenIndex];
        }

        public CustomNote[] GetHarmony(int partSize, int harmonyLength, bool major)
        {
            if (partSize <= 1 || partSize > 4)
            {
                return null;
            }

            var range = CustomNote.Range;
            CustomNote.Range = CustomNote.Range - 1;

            var harmony = new CustomNote[partSize];
            var note = new CustomNote();
            note.Length = harmonyLength;
            harmony[0] = note;

            if (major)
            {
                harmony[1] = new CustomNote(GetMajorThird(note.Name), harmonyLength);
            }
            else
            {
                harmony[1] = new CustomNote(GetMinorThird(note.Name), harmonyLength);
            }

            if (partSize >= 3)
            {
                var nextNote = Utilities.GlobalRandom.Next(1, 2);
                if (partSize > 3 || nextNote == 1)
                    harmony[2] = new CustomNote(note.Name + Fifth, harmonyLength);
                else
                    harmony[2] = new CustomNote(note.Name + Octave, harmonyLength);
            }

            if (partSize >= 4)
            {
                harmony[3] = new CustomNote(note.Name + Octave, harmonyLength);
            }


            return harmony;
        }

        private int GetMinorThird(int baseNote)
        {
            return baseNote + MinorThird;
        }

        private int GetMajorThird(int baseNote)
        {
            return baseNote + MajorThird;
        }
    }
}