using System.Linq;

namespace HonsMusicMaker.Entities
{
    public static class CustomChord
    {
        private static int[] _chord;

        public static void InitCustomChord(int chordSize)
        {
            if (chordSize > 0 && chordSize <= 4)
            {
                _chord = new int[chordSize];    
            }
            else
            {
                throw new System.ArgumentOutOfRangeException(nameof(chordSize));
            }
        }

        public static int[] GetChord(int step, int inversion)
        {
            if (step == 2 || step == 3 || step == 6)
            {
                GetMinorOn(step);
            }
            else if (step == 1 || step == 4 || step == 5)
            {
                GetMajorOn(step);
            }
            else if (step == 7)
            {
                GetSeventh();
            }

            GetInversion(inversion);
            
            return _chord;
        }

        private static void GetInversion(int inversion)
        {
            inversion = inversion % 3;
            var temp = 0;

            for (var i = 0; i < _chord.Length; i++) {
                for (var j = 0; j < _chord.Length - 1; j++) {
                    if (_chord[j] > _chord[j + 1]) {
                        temp = _chord[j + 1];
                        _chord[j + 1] = _chord[j];
                        _chord[j] = temp;
                    }
                }
            }
            
            for (var invert = 0; invert < inversion; invert++)
            {
                temp = _chord[0];
                for (var i = 0; i < _chord.Length-1; i++)
                {
                    _chord[i] = _chord[i + 1];
                }

                _chord[_chord.Length - 1] = temp+12;
            }
        }

        private static void GetSeventh()
        {
            throw new System.NotImplementedException();
        }

        private static void GetMajorOn(int step)
        {
            switch (_chord.Length)
            {
                case 1:
                {
                    _chord[0] = step;
                    break;
                }
                case 2:
                {
                    _chord[0] = step;
                    _chord[1] = step + 4;
                    break;
                }
                case 3:
                {
                    _chord[0] = step;
                    _chord[1] = step + 4;
                    _chord[2] = step + 7;
                    break;
                }
                case 4:
                {
                    _chord[0] = step;
                    _chord[1] = step + 4;
                    _chord[2] = step + 7;
                    _chord[3] = step + 12;
                    break;
                }
            }
        }

        private static void GetMinorOn(int step)
        {
            switch (_chord.Length)
            {
                case 1:
                {
                    _chord[0] = step;
                    break;
                }
                case 2:
                {
                    _chord[0] = step;
                    _chord[1] = step + 3;
                    break;
                }
                case 3:
                {
                    _chord[0] = step;
                    _chord[1] = step + 3;
                    _chord[2] = step + 7;
                    break;
                }
                case 4:
                {
                    _chord[0] = step;
                    _chord[1] = step + 3;
                    _chord[2] = step + 7;
                    _chord[3] = step + 12;
                    break;
                }
            }
        }

        public static double EvaluateRelationship(int chordOne, int chordTwo)
        {
            if (chordTwo == 7)
            {
                return 0;
            }
            
            if (chordOne == 1)
            {
                return 1;
            }

            if (chordOne == 5 && chordTwo == 1)
            {
                return 1;
            }

            if (chordOne == chordTwo)
            {
                return 0;
            }

            if (chordOne == 2 && (chordTwo == 5 || chordTwo == 6))
            {
                return 0.8;
            }

            return -1;
        }
    }
}