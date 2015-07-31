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
using System.Linq;
using Helix.Core.Expressions;
using Troschuetz.Random;

namespace Helix.Core.Initialisers
{
  /// <summary>Strategy for initialising a population.</summary>
  public abstract class AbstractInitialiserStrategy : IInitialiserStrategy
  {
    /// <summary>
    ///   A uniformly distributed pseudo-random number generator. The built in
    ///   <see cref="System.Random" /> class is flawed and will probably never be
    ///   fixed.
    ///   https://connect.microsoft.com/VisualStudio/feedback/details/634761/system-random-serious-bug
    ///   http://stackoverflow.com/a/6842191/445517
    ///   http://blogs.msdn.com/b/ericlippert/archive/2012/02/21/generating-random-non-uniform-data-in-c.aspx
    /// </summary>
    private readonly ContinuousUniformDistribution _uniformDistribution =
      new ContinuousUniformDistribution(new ALFGenerator());

    /// <summary>Creates a new expression tree from scratch.</summary>
    /// <param name="functionCollection">
    ///   The collection of function types available for
    ///   use. Each element must derive from <see cref="IFunction" />.
    /// </param>
    /// <param name="terminalCollection">
    ///   The collection of terminals available for use.
    ///   Each element must derive from <see cref="ITerminal" />.
    /// </param>
    /// <param name="maxDepth">
    ///   The maximum allowed depth for expressions. Must be
    ///   non-negative.
    /// </param>
    /// <returns>The newly generated expression tree.</returns>
    public abstract ITree GenerateRandomExpressionTree(
      ICollection<Type> functionCollection, ICollection<Type> terminalCollection,
      int maxDepth);

    /// <summary>Picks a uniformaly random element from a collection of primitives.</summary>
    /// <param name="primitiveCollection">The collection of primitives to pick from.</param>
    /// <returns>A random element from the collection.</returns>
    protected IPrimitive ChooseRandomPrimitive(
      ICollection<Type> primitiveCollection)
    {
      #region Contracts

      Contract.Requires<ArgumentNullException>(primitiveCollection != null,
        "A valid collection of primitives must be provided.");
      Contract.Requires<ArgumentOutOfRangeException>(
        0 < primitiveCollection.Count,
        "At least one primitive must be provided to choose from.");
      Contract.Ensures(Contract.Result<IPrimitive>() != null);

      #endregion

      var nthElement = GetRandomValueInRange(0, primitiveCollection.Count);
      var enumerable = primitiveCollection.Skip(nthElement);

      // ReSharper disable PossibleMultipleEnumeration
      // This is true since we know nthElement is less than the number of
      // elements in the collection, so we can never skip all of them. We also
      // know that the collection has at least one element.
      Contract.Assume(enumerable.Any());

      var primitiveType = enumerable.First();
      // ReSharper restore PossibleMultipleEnumeration
      Contract.Assume(primitiveType != null);
      return (IPrimitive) Activator.CreateInstance(primitiveType);
    }

    /// <summary>Returns a random number uniformly distributed between 0 and 1.</summary>
    /// <returns>A number between 0 and 1.</returns>
    protected double GetRandomSample()
    {
      ConfigureDistribution(0.0, 1.0);
      return _uniformDistribution.NextDouble();
    }

    /// <summary>
    ///   Sets up the random number distribution for the next call to get a
    ///   value.
    /// </summary>
    /// <param name="min">The smallest value that can be returned.</param>
    /// <param name="exclusiveMax">
    ///   The upper bound of the value that can be returned.
    ///   This value itself is never returned.
    /// </param>
    private void ConfigureDistribution(double min, double exclusiveMax)
    {
      // See http://www.codeproject.com/Articles/15102/NET-random-number-generators-and-distributions
      // for details on the distribution configurations.
      _uniformDistribution.Alpha = min;
      _uniformDistribution.Beta = exclusiveMax;
    }

    /// <summary>
    ///   Returns a random number in the given range: [
    ///   <paramref name="min" />..<paramref name="exclusiveMax" />).
    /// </summary>
    /// <param name="min">The smallest value that can be returned.</param>
    /// <param name="exclusiveMax">
    ///   The upper bound of the value that can be returned.
    ///   This value itself is never returned.
    /// </param>
    /// <returns>
    ///   A uniformly random number in the range: [<paramref name="min" />..
    ///   <paramref name="exclusiveMax" />).
    /// </returns>
    private int GetRandomValueInRange(int min, int exclusiveMax)
    {
      #region Contracts

      Contract.Requires<ArgumentOutOfRangeException>(min < exclusiveMax);
      Contract.Ensures(min <= Contract.Result<int>() &&
                       Contract.Result<int>() < exclusiveMax);

      #endregion

      ConfigureDistribution(min, exclusiveMax);
      var randomValueInRange = _uniformDistribution.NextDouble();

      // The random value provider library doesn't provide post-conditions, so
      // we must add them ourselves.
      Contract.Assume(
        min <= randomValueInRange && randomValueInRange < exclusiveMax,
        "Error in the third-party Troschuetz.Random library.");

      return (int) randomValueInRange;
    }
  }
}