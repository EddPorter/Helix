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
using Helix.Common;
using Helix.Core.Expressions;
using Troschuetz.Random;

namespace Helix.Core.Mutations
{
  /// <summary>
  ///   Randomly selects nodes and replaces the primitive with a different
  ///   primitive of the same arity.
  /// </summary>
  public class PointMutation : IMutation
  {
    /// <summary>A collection of the function types available for use.</summary>
    private readonly ICollection<Type> _functionCollection;

    /// <summary>
    ///   A pseudo-random number generator used to select the replacement
    ///   primitive for a mutated node.
    /// </summary>
    private readonly ContinuousUniformDistribution _mutationDistribution;

    private readonly double _nodeSelectionProbability;

    /// <summary>
    ///   A pseudo-random number generator with any distribution used to
    ///   determine if a node will be mutated.
    /// </summary>
    private readonly Distribution _selectionDistribution;

    /// <summary>A collection of the terminal types available for use.</summary>
    private readonly ICollection<Type> _terminalCollection;

    /// <summary>Called when creating a new point mutation.</summary>
    /// <param name="functionCollection">
    ///   The collection of function types available for
    ///   use. Each element must derive from <see cref="IFunction" />.
    /// </param>
    /// <param name="terminalCollection">
    ///   The collection of terminals available for use.
    ///   Each element must derive from <see cref="ITerminal" />.
    /// </param>
    /// <param name="nodeSelectionProbability"></param>
    /// <param name="selectionDistribution">
    ///   A random distribution to be used by the
    ///   class to determine whether a node will be mutated. Can be <c>null</c> in
    ///   which case a default uniform distribution is created.
    /// </param>
    /// <param name="mutationDistribution">
    ///   A random uniform distribution to be used by
    ///   the class when selecting the mutation primitive to select. Can  be
    ///   <c>null</c> in which case a default uniform distribution is created.
    /// </param>
    public PointMutation(ICollection<Type> functionCollection,
      ICollection<Type> terminalCollection, double nodeSelectionProbability,
      Distribution selectionDistribution = null,
      ContinuousUniformDistribution mutationDistribution = null)
    {
      #region Code Contracts

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

      Contract.Requires<ArgumentOutOfRangeException>(0.0d <=
                                                     nodeSelectionProbability &&
                                                     nodeSelectionProbability <=
                                                     1.0d);

      Contract.Ensures(_functionCollection ==
                       Contract.OldValue(functionCollection));
      Contract.Ensures(_terminalCollection ==
                       Contract.OldValue(terminalCollection));
      Contract.Ensures(_nodeSelectionProbability == nodeSelectionProbability);
      Contract.Ensures(_selectionDistribution != null);
      Contract.Ensures(_mutationDistribution != null);

      #endregion

      _functionCollection = functionCollection;
      _terminalCollection = terminalCollection;
      _nodeSelectionProbability = nodeSelectionProbability;
      _selectionDistribution = selectionDistribution ??
                               new ContinuousUniformDistribution(
                                 new ALFGenerator());
      _mutationDistribution = mutationDistribution ??
                              new ContinuousUniformDistribution(
                                new ALFGenerator());
    }

    #region IMutation Members

    /// <summary>
    ///   Modifies the given individual to create a new one by choosing random
    ///   nodes and replacing them with different primitives of the same arity.
    /// </summary>
    /// <param name="tree">The individual to mutate.</param>
    /// <returns>The new individual.</returns>
    public ITree Mutate(ITree tree)
    {
      var mutation = tree.Clone();
      mutation.BreadthFirstVisitor(_ => true, currentNode =>
      {
        var result = _selectionDistribution.NextDouble();
        if (result >= _nodeSelectionProbability)
        {
          return;
        }

        IList<IPrimitive> replacementPrimitives;
        var mutant = currentNode.Tree;

        var replaceFunction = mutant.Node is IFunction;
        if (replaceFunction)
        {
          var function = (IFunction) mutant.Node;
          var arity = function.Arity;
          replacementPrimitives =
            _functionCollection.Select(
              primitiveType =>
                ((IFunction) Activator.CreateInstance(primitiveType)))
              .Where(f => f.Arity == arity)
              .Cast<IPrimitive>()
              .ToList();
        }
        else
        {
          replacementPrimitives =
            _terminalCollection.Select(
              type => (ITerminal) Activator.CreateInstance(type))
              .Cast<IPrimitive>()
              .ToList();
        }

        _mutationDistribution.ConfigureDistribution(0,
          replacementPrimitives.Count);
        var newPrimitiveIndex = (int) _mutationDistribution.NextDouble();
        var newPrimitive = replacementPrimitives[newPrimitiveIndex];
        mutant.Node = newPrimitive;
      });

      return mutation;
    }

    #endregion
  }
}