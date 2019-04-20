using System;
using HonsMusicMaker.Entities;

namespace HonsMusicMaker.GeneticAlgorithm
{
    public static class Fitness
    {
        private const double MaxFitness = 8.0;
        
        private const int LeadToTonic = 100;
        
        private const int SubmediantToSubdominant = 33;
        private const int SubmediantToTonic = 33;
        
        private const int DominantToLead = 25;
        private const int DominantToTonic = 25;
        private const int DominantToSubmediant = 25;
        
        private const int SubdominantToSubmediant = 33;
        private const int SubdominantToTonic = 33;
        
        private const int MediantToSubdominant = 33;
        private const int MediantToSubmediant = 33;
        
        private const int ChordFollowingSubmediant = 34;
        private const int ChordFollowDominant = 25;
        private const int ChordFollowSubdominant = 33;
        private const int ChordFollowMediant = 33;
        private const int ChordFollowSuperTonic = 100;
        private const int ChordFollowTonic = 100;
        
        private const int StartWithTonicWeight = 14;
        private const int EndWithTonicWeight = 14;
        private const int FinalCadenceRhythmWeight = 13;
        private const int MidwayCadenceRhythmWeight = 13;
        private const int MidwayCadenceWeight = 10;
        private const int ChordsFollowWellWeight = 10;
        private const int RhythmIsSensibleWeight = 10;
        private const int ChordsRepeatTooOften = 14;
        
        private const int AmountOfFitnessItems = 8;
        
        private const int AuthenticCadenceWeight = 40;
        private const int HalfCadenceWeight = 20;
        private const int DeceptiveCadenceWeight = 20;
        private const int PlagalCadenceWeight = 20;

        public static double GetFitness(Chromosome individual)
        {
            var fitness = 0.0;
            var chromosomeData = individual.GetChromosome();

            for (int bar = 0; bar < chromosomeData.Length; bar++)
            {
                var thisBar = chromosomeData[bar];
                if (bar == 0)
                {
                    fitness += PhraseStartsWithTonic(thisBar);
                }

                if (bar == chromosomeData.Length - 1)
                {
                    fitness += PhraseEndsWithTonic(thisBar);
                    fitness += FinalRythmSoundsFinal(thisBar);
                }

                if (bar == (chromosomeData.Length / 2) - 1)
                {
                    fitness += PhraseHasCadenceInHalfpoint(thisBar);
                    fitness += HalfRythmSoundsFinal(thisBar);
                }

                fitness += BarRythmIsConsistent(thisBar);
            }
            fitness += BarChordsFollowWell(chromosomeData);
            
            fitness -= ChordsRepeatTooMuch(chromosomeData);
            return fitness;
        }

        private static double ChordsRepeatTooMuch(int[][][] phrase)
        {
            var count = 0;
            var chordNumber = 0;
            var fitness = 0.0;
            for (int bar = 0; bar < phrase.Length; bar++)
            {
                for (int chord = 0; chord < phrase[bar].Length; chord++)
                {
                    if (phrase[bar][chord] != null && phrase[bar][chord][5] == chordNumber)
                    {
                        count++;
                    }
                    else if (phrase[bar][chord] != null && phrase[bar][chord][5] != chordNumber)
                    {
                        chordNumber = phrase[bar][chord][5];
                        count = 1;
                    }

                    switch (count)
                    {
                        case 1: break;
                        case 2: fitness = 20;
                            break;
                        case 3: fitness = 60;
                            break;
                        default: fitness = 80;
                            break;
                    }
                }
            }
            return fitness / 100 * 8;
        }

        private static double BarChordsFollowWell(int[][][] phrase)
        {
            int[] chordOne = null;
            int[] chordTwo = null;
            var evaluationCount = 0;
            var fitness = 0.0;
            
            for (int bar = 0; bar < phrase.Length; bar++)
            {
                for (int chord = 0; chord < phrase[bar].Length; chord++)
                {

                    if (chordOne == null && phrase[bar][chord] != null)
                    {
                        chordOne = phrase[bar][chord];
                    }
                    else if (chordTwo == null && phrase[bar][chord] != null)
                    {
                        chordTwo = phrase[bar][chord];
                        fitness += ChordMayFollow(chordOne, chordTwo);
                        evaluationCount++;
                        chordOne = chordTwo;
                        chordTwo = null;
                    }
                }
            }

            fitness = (fitness / evaluationCount / 100 * ChordsFollowWellWeight) / 100 * 8;

            return fitness;
        }

        private static double BarRythmIsConsistent(int[][] thisBar)
        {
            int barRythm = 0;
            int[] chordArray = null;
            for (int chord = 0; chord < thisBar.Length; chord++)
            {
                if (chordArray == null)
                {
                    chordArray = thisBar[chord];
                    barRythm = chordArray[4];
                }
                else if (thisBar[chord] != null && thisBar[chord][4] != barRythm)
                {
                    return 0;
                }
            }

            var fitness = RhythmIsSensibleWeight / 100 * 8;

            return fitness;
        }

        private static double FinalRythmSoundsFinal(int[][] thisBar)
        {
            int[] chordOne = null;
            int[] chordTwo = null;
            for (int chord = thisBar.Length - 1; chord > 0; chord--)
            {
                if (thisBar[chord] != null)
                {
                    if (chordTwo == null)
                    {
                        chordTwo = thisBar[chord];
                    }
                    else
                    {
                        chordOne = thisBar[chord];
                        break;
                    }
                }
            }

            if (chordOne != null && (chordOne[4] == 2 && chordTwo[4] == 4))
            {
                return FinalCadenceRhythmWeight / 100 * 8;
            }

            return 0;
        }

        private static double HalfRythmSoundsFinal(int[][] thisBar)
        {
            int[] chordOne = null;
            int[] chordTwo = null;
            for (int chord = thisBar.Length - 1; chord > 0; chord--)
            {
                if (thisBar[chord] != null)
                {
                    if (chordTwo == null)
                    {
                        chordTwo = thisBar[chord];
                    }
                    else
                    {
                        chordOne = thisBar[chord];
                        break;
                    }
                }
            }

            if (chordOne != null && (chordOne[4] == 2 && chordTwo[4] == 4))
            {
                return MidwayCadenceRhythmWeight / 100 * 8;
            }

            return 0;
        }

        private static double PhraseHasCadenceInHalfpoint(int[][] thisBar)
        {
            int[] chordOne = null;
            int[] chordTwo = null;
            for (int chord = thisBar.Length - 1; chord > 0; chord--)
            {
                if (thisBar[chord] != null)
                {
                    if (chordTwo == null)
                    {
                        chordTwo = thisBar[chord];
                    }
                    else
                    {
                        chordOne = thisBar[chord];
                        return (ChordsFormCadence(chordOne, chordTwo) / 100 * MidwayCadenceWeight) / 100 * 8;
                    }
                }
            }

            return 0;
        }

        private static int ChordsFormCadence(int[] chordOne, int[] chordTwo)
        {
            var chordOneNumber = chordOne[5];
            var chordTwoNumber = chordTwo[5];
            if (chordOneNumber == 5 && chordTwoNumber == 1)
            {
                return AuthenticCadenceWeight;
            }

            if (chordOneNumber == 5 && chordTwoNumber == 6)
            {
                return DeceptiveCadenceWeight;
            }

            if (chordOneNumber == 4 && chordTwoNumber == 1)
            {
                return PlagalCadenceWeight;
            }

            if (chordTwoNumber == 5)
            {
                return HalfCadenceWeight;
            }

            return 0;
        }

        private static double PhraseEndsWithTonic(int[][] thisBar)
        {
            for (int chord = thisBar.Length - 1; chord > 0; chord--)
            {
                var thisChord = thisBar[chord];
                if (thisChord != null)
                {
                    if (thisChord[5] == 1)
                    {
                        return EndWithTonicWeight / 100 * 8;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }

            return 0;
        }

        private static double PhraseStartsWithTonic(int[][] thisBar)
        {
            for (int chord = 0; chord < thisBar.Length; chord++)
            {
                var thisChord = thisBar[chord];
                if (thisChord != null)
                {
                    if (thisChord[5] == 1)
                    {
                        return StartWithTonicWeight / 100 * 8;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }

            return 0;
        }

        public static double GetMaxFitness()
        {
            return MaxFitness;
        }
        
        private static int ChordMayFollow(int[] chordOne, int[] chordTwo)
        {
            var result = 0;
            var previousChordNumber = chordOne[MyBar.ChordNumberIndex];
            var currentChordNumber = chordTwo[MyBar.ChordNumberIndex];

            switch (previousChordNumber)
            {
                case 1:
                    result = ChordMayFollowTonic(currentChordNumber);
                    break;
                case 2:
                    result = ChordMayFollowSuperTonic(currentChordNumber);
                    break;
                case 3:
                    result = ChordMayFollowMediant(currentChordNumber);
                    break;
                case 4:
                    result = ChordMayFollowSubdominant(currentChordNumber);
                    break;
                case 5:
                    result = ChordMayFollowDominant(currentChordNumber);
                    break;
                case 6:
                    result = ChordMayFollowSubmediant(currentChordNumber);
                    break;
                case 7:
                    result = ChordMayFollowLead(currentChordNumber);
                    break;
            }
            
            return result;
        }

        private static int ChordMayFollowLead(int currentChordNumber)
        {
            if (currentChordNumber == 1)
            {
                return LeadToTonic;
            }
            return 0;
        }

        private static int ChordMayFollowSubmediant(int currentChordNumber)
        {
            if (currentChordNumber == 7)
            {
                return 0;
            }
            else if (currentChordNumber == 4)
            {
                return SubmediantToSubdominant;
            }
            else if (currentChordNumber == 1)
            {
                return SubmediantToTonic;
            }

            return ChordFollowingSubmediant;
        }

        private static int ChordMayFollowDominant(int currentChordNumber)
        {
            if (currentChordNumber == 7)
            {
                return DominantToLead;
            }
            else if (currentChordNumber == 1)
            {
                return DominantToTonic;
            }
            else if (currentChordNumber == 6)
            {
                return DominantToSubmediant;
            }

            return ChordFollowDominant;
        }

        private static int ChordMayFollowSubdominant(int currentChordNumber)
        {
            if (currentChordNumber == 3) 
            {
                return 0;
            }
            else if (currentChordNumber == 6)
            {
                return SubdominantToSubmediant;
            }
            else if (currentChordNumber == 1)
            {
                return SubdominantToTonic;
            }

            return ChordFollowSubdominant;
        }

        private static int ChordMayFollowMediant(int currentChordNumber)
        {
            if (currentChordNumber == 4)
            {
                return MediantToSubdominant;
            }
            else if (currentChordNumber == 6)
            {
                return MediantToSubmediant;
            }

            return ChordFollowMediant;
        }

        private static int ChordMayFollowSuperTonic(int currentChordNumber)
        {
            if (currentChordNumber == 1)
            {
                return 0;
            }
            else
            {
                return ChordFollowSuperTonic;
            }
        }

        private static int ChordMayFollowTonic(int currentChordNumber)
        {
            if (currentChordNumber == 7)
            {
                return 0;
            }
            else
            {
                return ChordFollowTonic;
            }
        }
    }
}