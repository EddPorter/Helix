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

using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Helix.Common;

namespace Helix.Core.Expressions
{
  /// <summary>
  ///   An expression tree representing an algorithmic calculation. A
  ///   recursive structured with each node formed from functions and terminals.
  /// </summary>
  [ContractClass(typeof (TreeContract))]
  public interface ITree : ICloneable<ITree>
  {
    /// <summary>The depth of the tree's deepest leaf.</summary>
    [Pure]
    int Depth { get; }

    /// <summary>The primitive element represented by the root node of this tree.</summary>
    [Pure]
    IPrimitive Node { get; set; }

    /// <summary>The number of nodes in the tree.</summary>
    [Pure]
    int Size { get; }

    /// <summary>
    ///   The branches of the tree evaluating to expressions to be provided to a
    ///   function, or null if this is a leaf of the tree.
    /// </summary>
    IList<ITree> Children { get; }
  }

  /// <summary>Specifies the code invariants for the <see cref="ITree" />
  ///   interface and its derived types.</summary>
  [ContractClassFor(typeof (ITree))]
  internal abstract class TreeContract : ITree
  {
    /// <summary>Depth cannot be negative. Leaf nodes can have depth 0.</summary>
    int ITree.Depth
    {
      get
      {
        Contract.Ensures(Contract.Result<int>() >= 0);
        return default(int);
      }
    }

    /// <summary>Every tree must have a valid node element.</summary>
    IPrimitive ITree.Node
    {
      get
      {
        return default(IPrimitive);
      }
      set
      {
        // Can only swap a terminal for a terminal, or a function with a same arity function.
        var node = ((ITree) this).Node;
        Contract.Requires(node == null ||
                          (node is ITerminal && value is ITerminal) ||
                          (node is IFunction && value is IFunction &&
                           ((IFunction) value).Arity == ((IFunction) node).Arity));
      }
    }

    /// <summary>Every tree must have at least one element.</summary>
    int ITree.Size
    {
      get
      {
        Contract.Ensures(Contract.Result<int>() >= 1);
        return default(int);
      }
    }

    IList<ITree> ITree.Children => default(IList<ITree>);

    [ContractInvariantMethod]
    private void ObjectInvariant()
    {
      var tree = (ITree) this;
      Contract.Invariant(tree.Depth >= 0);
      Contract.Invariant(tree.Size >= 1);
      Contract.Invariant(tree.Node != null);
      Contract.Invariant(tree.Node is ITerminal || tree.Node is IFunction);
      Contract.Invariant((tree.Node is ITerminal && tree.Children == null) ||
                         (tree.Node is IFunction && tree.Children != null));
      Contract.Invariant(!(tree.Node is IFunction) ||
                         tree.Children.Count == ((IFunction) tree.Node).Arity);
    }

    #region ICloneable<ITree> Members

    public abstract ITree Clone();

    #endregion
  }
}