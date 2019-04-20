using System;
using System.Collections.Generic;

namespace HonsMusicMaker.Entities
{
    public class CustomBar
    {
        private int[][] _bar;

        public CustomBar(int chordsInBar, int chordSize)
        {
            _bar = new int[chordsInBar][];
            CustomChord.InitCustomChord(chordSize);
        }

        public int[][] GetStartBar()
        {
            for (var i = 0; i < _bar.Length; i++)
            {
                _bar[i] = CustomChord.GetChord(1,0);
            }

            return _bar;
        }

        public int[][] GetIntermediateBar()
        {
            for (var i = 0; i < _bar.Length; i++)
            {
                _bar[i] = CustomChord.GetChord(4,0);
            }

            return _bar;
        }

        public int[][] GetFinalBar()
        {
            for (var i = 0; i < _bar.Length; i++)
            {
                _bar[i] = CustomChord.GetChord(5,0);
            }

            return _bar;
        }
    }
}