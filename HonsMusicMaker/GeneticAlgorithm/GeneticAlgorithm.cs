using System;
using HonsMusicMaker.Entities;

namespace HonsMusicMaker.GeneticAlgorithm
{
    public class Algorithm
    {
        /* Parameters */
        private static double _uniformRate = 0.5;
        private static double _mutationRate = 0.015;
        private static int _tournamentSize = 5;
        private static bool _elitism = false;

        public static Population EvolvePopulation(Population population)
        {
            var newPopulation = new Population(population.GetPopulationSize(), false);

            if (_elitism)
            {
                newPopulation.SaveIndividual(0, population.GetFittest());
            }

            int elitismOffset;
            if (_elitism)
            {
                elitismOffset = 1;
            }
            else
            {
                elitismOffset = 0;
            }

            for (var i = elitismOffset; i < population.GetPopulationSize(); i++)
            {
                var indiv1 = TournamentSelection(population);
                var indiv2 = TournamentSelection(population);
                var newIndiv = Crossover(indiv1, indiv2);
                newPopulation.SaveIndividual(i, newIndiv);
            }

            for (var i = elitismOffset; i < newPopulation.GetPopulationSize(); i++)
            {
                Mutate(newPopulation.GetIndividual(i));
            }

            return newPopulation;
        }

        private static Chromosome Crossover(Chromosome indiv1, Chromosome indiv2)
        {
            var newSol = new Chromosome();
            for (var i = 0; i < indiv1.GetSize(); i++)
            {
                if (Utilities.GlobalRandom.NextDouble() <= _uniformRate)
                {
                    newSol.SetGene(i, indiv1.GetGene(i));
                }
                else
                {
                    newSol.SetGene(i, indiv2.GetGene(i));
                }
            }

            return newSol;
        }

        private static void Mutate(Chromosome indiv)
        {
            for (var i = 0; i < indiv.GetSize(); i++)
            {
                if (Utilities.GlobalRandom.NextDouble() <= _mutationRate)
                {
                    var gene = MyBar.GetBarNotes();
                    indiv.SetGene(i, gene);
                }
            }
        }

        private static Chromosome TournamentSelection(Population pop)
        {
            // Create a tournament population
            var tournament = new Population(_tournamentSize, false);
            // For each place in the tournament get a random individual
            for (var i = 0; i < _tournamentSize; i++)
            {
                var randomId = Utilities.GlobalRandom.Next(1 * pop.GetPopulationSize());
                tournament.SaveIndividual(i, pop.GetIndividual(randomId));
            }

            // Get the fittest
            var fittest = tournament.GetFittest();
            return fittest;
        }
    }
}