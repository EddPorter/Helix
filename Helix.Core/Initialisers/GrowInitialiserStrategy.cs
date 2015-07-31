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
using Helix.Core.Expressions;

namespace Helix.Core.Initialisers
{
  /// <summary>
  ///   Initialises an expression tree by randomly selecting primitive
  ///   elements until the tree reaches a specified depth.
  /// </summary>
  /// <remarks>
  ///   Algorithm taken from R. Poli, W. B. Langdon, and N. F. McPhee. A field
  ///   guide to genetic programming. Published via http://lulu.com and freely
  ///   available at http://www.gp-field-guide.org.uk, 2008. (With contributions by
  ///   J. R. Koza). pg14.
  /// </remarks>
  public class GrowInitialiserStrategy : AbstractInitialiserStrategy
  {
    /// <summary>
    ///   Creates a new expression tree from scratch. Each element is randomly
    ///   picked from the collection of primitives provided until a terminal is
    ///   reached. At the maximum depth, only terminals are chosen.
    /// </summary>
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
    public override ITree GenerateRandomExpressionTree(
      ICollection<Type> functionCollection, ICollection<Type> terminalCollection,
      int maxDepth)
    {
      #region Contracts

      Contract.Requires<ArgumentNullException>(functionCollection != null,
        "A collection of functions must be provided.");

      Contract.Requires<ArgumentException>(
        Contract.ForAll(functionCollection,
          type => typeof (IFunction).IsAssignableFrom(type)),
        "Elements in the collection of functions must derive from the IFunction interface.");

      Contract.Requires<ArgumentNullException>(terminalCollection != null,
        "A colleciton of terminals must be provided.");

      Contract.Requires<ArgumentException>(
        Contract.ForAll(terminalCollection,
          type => typeof (ITerminal).IsAssignableFrom(type)),
        "Elements in the collection of terminals must derive from the ITerminal interface.");

      Contract.Requires<ArgumentException>(terminalCollection.Count > 0,
        "At least one terminal type must be provided.");

      Contract.Requires<ArgumentOutOfRangeException>(maxDepth >= 0);

      Contract.Ensures(Contract.Result<ITree>() != null);

      // TODO: Ensure that each leaf is an ITerminal.

      // TODO: Ensure that each node is an IFunction.

      #endregion

      var functionCount = functionCollection.Count;
      var terminalCount = terminalCollection.Count;
      if (functionCount == 0 ||
          ShouldChooseTerminal(maxDepth, terminalCount,
            terminalCount + functionCount))
      {
        return new Tree((ITerminal) ChooseRandomPrimitive(terminalCollection));
      }

      var function = (IFunction) ChooseRandomPrimitive(functionCollection);
      var children = new ITree[function.Arity];
      Contract.Assume(((ICollection<ITree>) children).Count == function.Arity);
      for (var i = 0; i < function.Arity; ++i)
      {
        children[i] = GenerateRandomExpressionTree(functionCollection,
          terminalCollection, maxDepth - 1);
      }
      return new Tree(function, children);
    }

    /// <summary>
    ///   Determines if the initialisation algorithm should pick an ITerminal
    ///   next.
    /// </summary>
    /// <param name="maxDepth">The maximum depth of the expression tree being created.</param>
    /// <param name="terminalCount">The number of terminal primitives available.</param>
    /// <param name="primitiveCount">The total number of primitives available.</param>
    /// <returns>Whether a terminal should be chosen next.</returns>
    private bool ShouldChooseTerminal(int maxDepth, int terminalCount,
      int primitiveCount)
    {
      var randomPrimitive = GetRandomSample();

      return maxDepth == 0 ||
             randomPrimitive < (double) terminalCount/primitiveCount;
    }
  }
}