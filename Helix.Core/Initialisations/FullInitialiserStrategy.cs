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

namespace Helix.Core.Initialisations
{
  /// <summary>
  ///   Initialises an expression tree such that every path to a leaf has the
  ///   same depth.
  /// </summary>
  /// <remarks>
  ///   Algorithm taken from R. Poli, W. B. Langdon, and N. F. McPhee. A field
  ///   guide to genetic programming. Published via http://lulu.com and freely
  ///   available at http://www.gp-field-guide.org.uk, 2008. (With contributions by
  ///   J. R. Koza). pg14.
  /// </remarks>
  public class FullInitialiserStrategy : AbstractInitialiserStrategy
  {
    /// <summary>A collection of the function types available for use.</summary>
    private readonly ICollection<Type> _functionCollection;

    /// <summary>A collection of the terminal types available for use.</summary>
    private readonly ICollection<Type> _terminalCollection;

    /// <summary>Creates a new initialiser strategy using the full method.</summary>
    /// <param name="functionCollection">
    ///   The collection of function types available for
    ///   use. Each element must derive from <see cref="IFunction" />.
    /// </param>
    /// <param name="terminalCollection">
    ///   The collection of terminals available for use.
    ///   Each element must derive from <see cref="ITerminal" />.
    /// </param>
    public FullInitialiserStrategy(ICollection<Type> functionCollection,
      ICollection<Type> terminalCollection)
    {
      #region Code Contracts

      Contract.Requires<ArgumentNullException>(functionCollection != null,
        "A collection of functions must be provided.");

      Contract.Requires<ArgumentException>(
        Contract.ForAll(functionCollection,
          type => typeof (IFunction).IsAssignableFrom(type)),
        "Elements in the collection of functions must derive from the IFunction interface.");

      Contract.Requires<ArgumentException>(functionCollection.Count > 0,
        "At least one function type must be provided.");

      Contract.Requires<ArgumentNullException>(terminalCollection != null,
        "A colleciton of terminals must be provided.");

      Contract.Requires<ArgumentException>(
        Contract.ForAll(terminalCollection,
          type => typeof (ITerminal).IsAssignableFrom(type)),
        "Elements in the collection of terminals must derive from the ITerminal interface.");

      Contract.Requires<ArgumentException>(terminalCollection.Count > 0,
        "At least one terminal type must be provided.");

      #endregion

      _functionCollection = functionCollection;
      _terminalCollection = terminalCollection;
    }

    /// <summary>
    ///   Creates a new expression tree from scratch. Each element at depths
    ///   less than the maximum depth is picked from the collection of functions
    ///   provided. At the maximum depth, terminals are chosen.
    /// </summary>
    /// <param name="maxDepth">
    ///   The maximum allowed depth for expressions. Must be
    ///   non-negative.
    /// </param>
    /// <returns>The newly generated expression tree.</returns>
    public override ITree GenerateRandomExpressionTree(int maxDepth)
    {
      #region Contracts

      Contract.Ensures(Contract.Result<ITree>().Size ==
                       (2 << Contract.OldValue(maxDepth)) - 1);

      // TODO: Ensure that each leaf is an ITerminal.

      // TODO: Ensure that each node is an IFunction.

      // TODO: Ensure that only leaves are terminals, i.e. every root has maximum depth.

      #endregion

      if (ShouldChooseTerminal(maxDepth))
      {
        return new Tree((ITerminal) ChooseRandomPrimitive(_terminalCollection));
      }

      var function = (IFunction) ChooseRandomPrimitive(_functionCollection);
      var children = new ITree[function.Arity];
      Contract.Assume(((ICollection<ITree>) children).Count == function.Arity);
      for (var i = 0; i < function.Arity; ++i)
      {
        children[i] = GenerateRandomExpressionTree(maxDepth - 1);
      }
      return new Tree(function, children);
    }

    /// <summary>
    ///   Determines if the initialisation algorithm should pick an ITerminal
    ///   next.
    /// </summary>
    /// <param name="maxDepth">The maximum depth of the expression tree being created.</param>
    /// <returns>Whether a terminal should be chosen next.</returns>
    private static bool ShouldChooseTerminal(int maxDepth)
    {
      return maxDepth == 0;
    }
  }
}