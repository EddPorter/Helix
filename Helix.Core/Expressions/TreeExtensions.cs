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
using Troschuetz.Random;

namespace Helix.Core.Expressions
{
  internal static class TreeExtensions
  {
    internal static TreeAndParent BreadthFirstFindNode(ITree tree, ITree parent,
      int nodeIndex)
    {
      Contract.Requires(tree != null);
      Contract.Requires(0 <= nodeIndex && nodeIndex < tree.Size);

      var queue = new Queue<TreeAndParent>();

      queue.Enqueue(new TreeAndParent(tree, parent));
      while (nodeIndex > 0)
      {
        tree = queue.Dequeue().Tree;
        if (tree.Node is IFunction)
        {
          foreach (var child in tree.Children)
          {
            queue.Enqueue(new TreeAndParent(child, tree));
          }
        }
        --nodeIndex;
      }

      return queue.Dequeue();
    }

    internal static TreeAndParent PickPointInTree(this ITree tree,
      ContinuousUniformDistribution distribution)
    {
      Contract.Requires(tree != null);
      Contract.Requires(distribution != null);
      Contract.Ensures(Contract.Result<TreeAndParent>() != null);

      distribution.ConfigureDistribution(0, tree.Size);
      var nodeIndex = (int) distribution.NextDouble();

      return BreadthFirstFindNode(tree, null, nodeIndex);
    }
  }
}