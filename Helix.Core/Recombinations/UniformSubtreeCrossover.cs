﻿//  Helix - A Genetic Programming Library
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

using Helix.Core.Expressions;
using Troschuetz.Random;

namespace Helix.Core.Recombinations
{
  /// <summary>
  ///   A uniform subtree crossover recombiner. Two uniformly random points on
  ///   each parent tree are picked and the subtree from second swapped into the
  ///   chosen node on the first.
  /// </summary>
  /// <remarks>
  ///   A uniform selection strategy is not usually the best recombiner as a
  ///   in a typical tree the majority of nodes will be leaves. Thus, trees
  ///   frequently exchange only small amounts of "genetic material".
  /// </remarks>
  public class UniformSubtreeCrossover : IRecombiner
  {
    /// <summary>A uniformly distributed pseudo-random number generator.</summary>
    private readonly ContinuousUniformDistribution _uniformDistribution;

    /// <summary>Called when creating a new uniform subtree crossover recombiner.</summary>
    /// <param name="uniformDistribution">
    ///   A random uniform distribution to be used by
    ///   the class. Can be <c>null</c> in which case a default distribution is
    ///   created.
    /// </param>
    public UniformSubtreeCrossover(
      ContinuousUniformDistribution uniformDistribution = null)
    {
      _uniformDistribution = uniformDistribution ??
                             new ContinuousUniformDistribution(
                               new ALFGenerator());
    }

    #region IRecombiner Members

    /// <summary>
    ///   Combines two individuals into a third, which has elements of both
    ///   parents.
    /// </summary>
    /// <param name="first">The first individual to use.</param>
    /// <param name="second">The second individual to use.</param>
    /// <returns>A third individual with elements of the two parents.</returns>
    public ITree Recombine(ITree first, ITree second)
    {
      // TODO: This code matches that used in
      // HeadlessChickenCrossoverMutation.Mutate. Need to refactor into helper.

      var child = first.Clone();
      var firstPoint = child.PickPointInTree(_uniformDistribution);
      var secondPoint = second.PickPointInTree(_uniformDistribution);

      var parent = firstPoint.Parent;
      if (parent == null)
      {
        // The root of the first tree was picked to be replaced. Just return a
        // copy of the second tree.
        return second.Clone();
      }

      var index = parent.Children.IndexOf(firstPoint.Tree);
      parent.Children[index] = secondPoint.Tree;

      return new Tree(child);
    }

    #endregion
  }
}