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

namespace Helix.Core.Expressions
{
  /// <summary>
  ///   An expression tree representing an algorithmic calculation. A
  ///   recursive structured with each node formed from functions and terminals.
  /// </summary>
  /// <remarks>This is an immutable structure.</remarks>
  public class Tree : ITree, IEquatable<Tree>
  {
    /// <summary>
    ///   Creates a new tree with an <see cref="ITerminal" /> as the root node,
    ///   making it the root node in a larger expression tree.
    /// </summary>
    /// <param name="terminal">The termianl represented by the root of the tree.</param>
    public Tree(ITerminal terminal)
    {
      #region Contracts

      Contract.Requires<ArgumentNullException>(terminal != null,
        "A valid terminal must be provided.");
      Contract.Ensures(Node == Contract.OldValue(terminal));
      Contract.Ensures(Children == null);
      Contract.Ensures(Depth == 0);
      Contract.Ensures(Size == 1);

      #endregion

      Node = terminal;
      Depth = 0;
      Size = 1;
    }

    /// <summary>
    ///   Creates a new tree with an <see cref="IFunction" /> as the root node
    ///   and with its arguments as child trees.
    /// </summary>
    /// <param name="function">The function represented by the root of the tree.</param>
    /// <param name="children">
    ///   The argument expression trees to be used as arguments to
    ///   the <paramref name="function" />.
    /// </param>
    public Tree(IFunction function, IList<ITree> children)
    {
      #region Contracts

      Contract.Requires<ArgumentNullException>(function != null,
        "A valid function must be provided.");
      Contract.Requires<ArgumentNullException>(children != null,
        "A valid list of children must be provided.");
      Contract.Requires<ArgumentException>(children.Count == function.Arity,
        "The number of child trees must equal the arity of the function.");
      Contract.Ensures(Node == Contract.OldValue(function));
      Contract.Ensures(ReferenceEquals(Children, Contract.OldValue(children)));
      Contract.Ensures(Depth ==
                       Contract.OldValue(children).Max(tree => tree.Depth) + 1);
      Contract.Ensures(Size ==
                       Contract.OldValue(children).Sum(tree => tree.Size) + 1);

      #endregion

      Node = function;
      Depth = children.Max(tree => tree.Depth) + 1;
      Size = children.Sum(tree => tree.Size) + 1;
      Children = children;
    }

    /// <summary>
    ///   Creates a new tree from a hybrid specimen whose Size and Depth
    ///   properties might be inconsistent.
    /// </summary>
    /// <param name="hybridTree">The hybrid tree to build from.</param>
    internal Tree(ITree hybridTree)
    {
      Contract.Requires<ArgumentNullException>(hybridTree != null);

      if (hybridTree.Node is ITerminal)
      {
        Node = hybridTree.Node.Clone();
        Depth = 0;
        Size = 1;
      }
      else
      {
        Node = hybridTree.Node.Clone();
        Children =
          hybridTree.Children.Select(child => (ITree) new Tree(child)).ToList();
        Depth = Children.Max(tree => tree.Depth) + 1;
        Size = Children.Sum(tree => tree.Size) + 1;
      }
    }

    /// <summary>The depth of the tree's deepest leaf.</summary>
    [Pure]
    public int Depth { get; }

    /// <summary>The primitive element represented by the root node of this tree.</summary>
    [Pure]
    public IPrimitive Node { get; }

    /// <summary>The number of nodes in the tree.</summary>
    [Pure]
    public int Size { get; }

    /// <summary>
    ///   The branches of the tree evaluating to expressions to be provided to a
    ///   function, or null if this is a leaf of the tree.
    /// </summary>
    public IList<ITree> Children { get; }

    /// <summary>Determines whether the specified object is equal to the current
    ///   <see cref="Tree" />. A tree is defined by its  <see cref="Node" /> and its
    ///   <see cref="Children" />
    /// </summary>
    /// <returns>
    ///   <c>true</c> if the specified object is equal to the current
    ///   <see cref="Tree" />; otherwise, <c>false</c>.
    /// </returns>
    /// <param name="obj">The object to compare with the current <see cref="Tree" />.</param>
    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj))
        return false;
      if (ReferenceEquals(this, obj))
        return true;
      return obj.GetType() == GetType() && Equals((Tree) obj);
    }

    /// <summary>Returns the hash for this tree. A tree is defined by its
    ///   <see cref="Node" /> and its <see cref="Children" />.</summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
      unchecked
      {
        return ((Children?.GetHashCode() ?? 0)*397) ^ Node.GetHashCode();
      }
    }

    #region ICloneable<ITree> Members

    /// <summary>
    ///   Creates a new <see cref="ITree" /> that is a copy of the current
    ///   instance.
    /// </summary>
    /// <returns>A new <see cref="ITree" /> that is a copy of this instance.</returns>
    public ITree Clone()
    {
      if (Children != null)
      {
        // Function clone.
        return new Tree((IFunction) Node.Clone(),
          Children.Select(child => child.Clone()).ToList());
      }
      // Terminal clone.
      return new Tree((ITerminal) Node.Clone());
    }

    #endregion

    #region IEquatable<Tree> Members

    public bool Equals(Tree other)
    {
      if (ReferenceEquals(null, other))
        return false;
      if (ReferenceEquals(this, other))
        return true;
      return Equals(Children, other.Children) && Equals(Node, other.Node);
    }

    #endregion
  }
}