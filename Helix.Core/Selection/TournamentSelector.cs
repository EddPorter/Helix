//  Helix - A Genetic Programming Library
//  Copyright (C) 2015 Edd Porter<helix@eddporter.com>
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Helix.Common;
using Helix.Core.Expressions;
using Troschuetz.Random;

namespace Helix.Core.Selection
{
  public class TournamentSelector : ISelector
  {
    /// <summary>
    ///   The percentage of the population that should be included in the
    ///   tournament. This is a limiting factor.
    /// </summary>
    private const float SelectionPercentage = 0.1f;

    /// <summary>
    ///   The minimum tournament size. This ensures a good number of candidates
    ///   are tested in each tournament, if the population size allows.
    /// </summary>
    /// <remarks>
    ///   The value 8 has been selected arbitrarily. It should ideally be a
    ///   power of two.
    /// </remarks>
    private const int MinimumTournamentSize = 1 << 3; // 8

    /// <summary>A uniformly distributed pseudo-random number generator.</summary>
    private readonly ContinuousUniformDistribution _uniformDistribution;

    /// <summary>
    ///   Creates a new <see cref="TournamentSelector" /> for choosing an
    ///   individual from a population.
    /// </summary>
    /// <param name="fitnessFunc">
    ///   A function that provides the fitness for a given
    ///   individual.
    /// </param>
    /// <param name="uniformDistribution">
    ///   A random uniform distribution to be used by
    ///   the class. Can be <c>null</c> in which case a default distribution is
    ///   created.
    /// </param>
    public TournamentSelector(Func<ITree, float> fitnessFunc,
      ContinuousUniformDistribution uniformDistribution = null)
    {
      Contract.Requires<ArgumentNullException>(fitnessFunc != null,
        "A function to determine the fitness of an individual must be provided.");
      Contract.Ensures(FitnessFunc != null);
      Contract.Ensures(_uniformDistribution != null);

      FitnessFunc = fitnessFunc;
      _uniformDistribution = uniformDistribution ??
                             new ContinuousUniformDistribution(
                               new ALFGenerator());
    }

    private Func<ITree, float> FitnessFunc { get; }

    private static int GetNumberOfIndividualsToCompare(
      ICollection<Tree> population)
    {
      Contract.Requires(population != null);
      Contract.Ensures(0 < Contract.Result<int>() &&
                       Contract.Result<int>() <= population.Count);

      var desiredTournamentSize = (int) (population.Count*SelectionPercentage);
      Contract.Assume(0 <= desiredTournamentSize &&
                      desiredTournamentSize <= population.Count);

      var boostedTournamentSize = Math.Max(desiredTournamentSize,
        MinimumTournamentSize);
      return Math.Min(boostedTournamentSize, population.Count);
    }

    [ContractInvariantMethod]
    private void ObjectInvariant()
    {
      Contract.Invariant(0.0f <= SelectionPercentage &&
                         SelectionPercentage <= 1.0f);
      Contract.Invariant(1 <= MinimumTournamentSize);
      Contract.Invariant(FitnessFunc != null);
    }

    private static ITree RunTournament(IList<Tuple<ITree, float>> results)
    {
      Contract.Requires(results != null);
      Contract.Requires(results.Count > 0);
      Contract.Ensures(Contract.Result<ITree>() != null);

      var length = results.Count;
      while (length > 1)
      {
        for (var n = 0; n < length - 1; n += 2)
        {
          var individual1 = results[n];
          var individual2 = results[n + 1];
          if (individual1.Item2 > individual2.Item2)
          {
            results[n/2] = individual1;
          }
          else if (individual1.Item2 < individual2.Item2)
          {
            results[n/2] = individual2;
          }
          else if (individual2.Item1 == null)
          {
            results[n/2] = individual1;
          }
          else
          {
            results[n/2] = individual2;
          }
        }
        length /= 2;
      }

      return results[0].Item1;
    }

    private Tuple<ITree, float>[] SetupTournament(ICollection<Tree> population,
      int individualsToCompare)
    {
      Contract.Requires(population != null);
      Contract.Requires(0 < individualsToCompare);

      // Make sure the tournament size is a power of 2.
      var log2 = Math.Log(individualsToCompare, 2.0);
      int tournamentSize;
      checked
      {
        tournamentSize = 1 << (short) Math.Ceiling(log2);
      }
      Contract.Assume(individualsToCompare <= tournamentSize);

      var individuals = new List<ITree>(population);

      // Set up the tournament. Each two consecutive items will be played off
      // against each other and the result placed back in the resuls list.
      var results = new Tuple<ITree, float>[tournamentSize];
      for (var n = 0; n < individualsToCompare; ++n)
      {
        _uniformDistribution.ConfigureDistribution(0, individuals.Count);
        var individualIndex = (int) _uniformDistribution.NextDouble();
        Contract.Assume(0 <= individualIndex &&
                        individualIndex < individuals.Count);

        var individual = individuals[individualIndex];
        var fitness = FitnessFunc(individual);
        results[n] = new Tuple<ITree, float>(individual, fitness);

        individuals.RemoveAt(individualIndex);

        Contract.Assert(individuals.Count == population.Count - n - 1);
        Contract.Assert(individuals.Count >= individualsToCompare - n - 1);
      }
      // Add sentinel values for bye entries.
      for (var n = individualsToCompare; n < tournamentSize; ++n)
      {
        results[n] = new Tuple<ITree, float>(null, float.MinValue);
      }
      return results;
    }

    #region ISelector Members

    /// <summary>
    ///   Chooses a number of individuals at random from the population. These
    ///   are compared with each other and the best of them is chosen to be the parent.
    /// </summary>
    /// <param name="population">A collection of individuals from which to select one.</param>
    /// <returns>An individual from the population.</returns>
    public ITree Select(ICollection<Tree> population)
    {
      var individualsToCompare = GetNumberOfIndividualsToCompare(population);
      var results = SetupTournament(population, individualsToCompare);
      return RunTournament(results);
    }

    #endregion
  }
}