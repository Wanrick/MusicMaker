using System;
using HonsMusicMaker.Entities;
using HonsMusicMaker.GeneticAlgorithm;
using HonsMusicMaker.Processes;

namespace HonsMusicMaker
{
    class Program
    {
        const int PopulationSize = 1000;
        private static void Main(string[] args)
        {
            for (int i = 0; i < 50; i++)
            {
                var population = new int[PopulationSize][][][];
                var maxIter = 500;
                var maxFitness = 0.0;
                var myPop = new Population(1000, true);
                var generationCount = 0;
                
                while (myPop.GetFittest().GetFitness() < Fitness.GetMaxFitness() && generationCount <= maxIter) {
                    generationCount++;
                    maxFitness = myPop.GetFittest().GetFitness();
                    Console.WriteLine("Generation: " + generationCount + " Fittest: " + maxFitness);
                    myPop = Algorithm.EvolvePopulation(myPop);
                }
                
                Console.WriteLine(("Solution found!"));
                Console.WriteLine(("Generation: " + generationCount));
                Console.WriteLine(("Genes:"));
                Console.Write((myPop.GetFittest()));
                
                CreateMidi.CreateMidiFromIntBars(myPop.GetFittest().GetChromosome(), "Solution_" + i + "_" + maxFitness);
            }
            
            





            /*CustomNote.Range = 4;
            var numBars = 4;
            var voices = 2;
            

            var builder = new MusicBuilder();
            builder.Create(numBars, voices);
            var music = builder.GetMusic();
            CreateMidi.CreateMidiFromResult(music);*/
        }
    }
}