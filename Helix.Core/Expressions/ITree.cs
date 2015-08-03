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

using System.Diagnostics.Contracts;

namespace Helix.Core.Expressions
{
  /// <summary>
  ///   An expression tree representing an algorithmic calculation. A
  ///   recursive structured with each node formed from functions and terminals.
  /// </summary>
  [ContractClass(typeof (TreeContract))]
  public interface ITree
  {
    /// <summary>The depth of the tree's deepest leaf.</summary>
    [Pure]
    int Depth { get; }

    /// <summary>The primitive element represented by the root node of this tree.</summary>
    [Pure]
    IPrimitive Node { get; }

    /// <summary>The number of nodes in the tree.</summary>
    [Pure]
    int Size { get; }
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
        return 0;
      }
    }

    /// <summary>Every tree must have a valid node element.</summary>
    IPrimitive ITree.Node
    {
      get
      {
        Contract.Ensures(Contract.Result<IPrimitive>() != null);
        return default(IPrimitive);
      }
    }

    /// <summary>Every tree must have at least one element.</summary>
    int ITree.Size
    {
      get
      {
        Contract.Ensures(Contract.Result<int>() >= 1);
        return 1;
      }
    }
  }
}