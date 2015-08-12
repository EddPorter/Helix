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
using System.Diagnostics.Contracts;
using Helix.Core.Expressions;
using Helix.Core.Initialisations;
using Troschuetz.Random;

namespace Helix.Core.Mutations
{
  internal class HeadlessChickenCrossoverMutation : IMutation
  {
    private readonly IInitialiserStrategy _initialiserStrategy;
    private readonly int _mutationMaxDepth;

    /// <summary>A uniformly distributed pseudo-random number generator.</summary>
    private readonly ContinuousUniformDistribution _uniformDistribution;

    /// <summary>Called when creating a new headless chicken crossover mutation.</summary>
    /// <param name="initialiser">
    ///   The initialiser to use to create the new tree that
    ///   will be the mutation introduced.
    /// </param>
    /// <param name="mutationMaxDepth">
    ///   The maximum depth of the new mutation tree that
    ///   will be added to the individual. Must be non-negative.
    /// </param>
    /// <param name="uniformDistribution">
    ///   A random uniform distribution to be used by
    ///   the class. Can be <c>null</c> in which case a default distribution is
    ///   created.
    /// </param>
    public HeadlessChickenCrossoverMutation(IInitialiserStrategy initialiser,
      int mutationMaxDepth,
      ContinuousUniformDistribution uniformDistribution = null)
    {
      Contract.Requires<ArgumentNullException>(initialiser != null);
      Contract.Requires<ArgumentOutOfRangeException>(mutationMaxDepth >= 0);
      Contract.Ensures(_initialiserStrategy == initialiser);
      Contract.Ensures(_uniformDistribution != null);

      _initialiserStrategy = initialiser;
      _mutationMaxDepth = mutationMaxDepth;
      _uniformDistribution = uniformDistribution ??
                             new ContinuousUniformDistribution(
                               new ALFGenerator());
    }

    #region IMutation Members

    /// <summary>
    ///   Modifies the given individual to create a new one by randomly picking
    ///   a point and substituting a new tree.
    /// </summary>
    /// <param name="tree">The individual to mutate.</param>
    /// <returns>The new individual.</returns>
    public ITree Mutate(ITree tree)
    {
      // TODO: This code matches that used in UniformSubtreeCrossover.Recombine
      // Need to refactor into helper.

      var child = tree.Clone();
      var point = child.PickPointInTree(_uniformDistribution);
      var mutation =
        _initialiserStrategy.GenerateRandomExpressionTree(_mutationMaxDepth);

      var parent = point.Parent;
      if (parent == null)
      {
        // The root of the first tree was picked to be replaced. Just return a
        // copy of the second tree.
        return mutation;
      }

      var index = parent.Children.IndexOf(point.Tree);
      parent.Children[index] = mutation;

      return new Tree(child);
    }

    #endregion
  }
}