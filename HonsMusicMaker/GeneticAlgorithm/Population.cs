namespace HonsMusicMaker.GeneticAlgorithm
{
    public class Population {

        Chromosome[] _population;

        public Population(int populationSize, bool initialise) {
            _population = new Chromosome[populationSize];
            if (initialise) {
                for (var i = 0; i < GetPopulationSize(); i++) {
                    var newIndividual = new Chromosome();
                    newIndividual.GenerateSolution();
                    SaveIndividual(i, newIndividual);
                }
            }
        }
        
        public Chromosome GetIndividual(int index) {
            return _population[index];
        }

        public Chromosome GetFittest() {
            var fittest = _population[0];
            for (var i = 0; i < GetPopulationSize(); i++) {
                if (fittest.GetFitness() <= GetIndividual(i).GetFitness()) {
                    fittest = GetIndividual(i);
                }
            }
            return fittest;
        }
        
        public int GetPopulationSize() {
            return _population.Length;
        }
        
        public void SaveIndividual(int index, Chromosome chromosome) {
            _population[index] = chromosome;
        }
    }
}