using System;
using HonsMusicMaker.Entities;

namespace HonsMusicMaker.GeneticAlgorithm
{
    public class Chromosome {
        //Chromosome is representation of an 8 bar musical phrase
        private static int _chromosomeLength = 8; // Notes over the phrase: up to 16 sixteenth note chords in each bar
        private int[][][] _chromosome;
        private double _fitness = 0;
        private int _amountOfBars = 8;
        private double TOLERANCE = 0.001;

        public Chromosome()
        {
            _chromosome = new int[_chromosomeLength][][];
        }

        // Create a random individual
        public void GenerateSolution()
        {
            _chromosome = MyBar.MakeBars(_amountOfBars);
        }

        public static void SetDefaultGeneLength(int length) {
            _chromosomeLength = length;
        }
    
        public int[][] GetGene(int index) {
            return _chromosome[index];
        }

        //For mutation maybe
        public void SetGene(int index, int[][] value) {
            _chromosome[index] = value;
            _fitness = 0;
        }

        public int[][][] GetChromosome()
        {
            return _chromosome;
        }

        public int GetSize() {
            return _chromosome.Length;
        }

        public double GetFitness() {
            if (Math.Abs(_fitness) < TOLERANCE) {
                _fitness = Fitness.GetFitness(this);
            }
            return _fitness;
        }

        public override string ToString()
        {
            var chromosome = "";

            for (var bar = 0; bar < _chromosome.Length; bar++)
            {
                for (int chord = 0; chord < _chromosome[bar].Length; chord++)
                {
                    if (_chromosome[bar][chord] != null)
                    {
                        Console.WriteLine(_chromosome[bar][chord][0] + " | " + 
                                          _chromosome[bar][chord][1] + " | " + 
                                          _chromosome[bar][chord][2] + " | " + 
                                          _chromosome[bar][chord][3] + " || " + 
                                          _chromosome[bar][chord][4] + " ||| " + 
                                          _chromosome[bar][chord][5]);
                    }
                }
                chromosome += _chromosome[bar].ToString();
            }
            
            return chromosome;
        }
    }
}