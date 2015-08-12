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
using Helix.Common;
using Troschuetz.Random;

namespace Helix.Core.Expressions
{
  internal static class TreeExtensions
  {
    internal static TreeAndParent BreadthFirstFindNode(this ITree tree,
      int nodeIndex)
    {
      Contract.Requires(tree != null);
      Contract.Requires(0 <= nodeIndex && nodeIndex < tree.Size);

      return BreadthFirstVisitor(tree, _ => nodeIndex > 0,
        currentNode => --nodeIndex);
    }

    internal static TreeAndParent BreadthFirstVisitor(this ITree tree,
      Func<TreeAndParent, bool> continueFunc, Action<TreeAndParent> processNode)
    {
      var queue = new Queue<TreeAndParent>();

      queue.Enqueue(new TreeAndParent(tree, null));

      while (queue.Count > 0)
      {
        var current = queue.Dequeue();
        if (!continueFunc(current))
        {
          return current;
        }

        // Perform function.
        processNode(current);

        // Queue children.
        var currentTree = current.Tree;
        if (currentTree.Node is IFunction)
        {
          foreach (var child in currentTree.Children)
          {
            queue.Enqueue(new TreeAndParent(child, currentTree));
          }
        }
      }
      return null;
    }

    internal static TreeAndParent PickPointInTree(this ITree tree,
      ContinuousUniformDistribution distribution)
    {
      Contract.Requires(tree != null);
      Contract.Requires(distribution != null);
      Contract.Ensures(Contract.Result<TreeAndParent>() != null);

      distribution.ConfigureDistribution(0, tree.Size);
      var nodeIndex = (int) distribution.NextDouble();

      return BreadthFirstFindNode(tree, nodeIndex);
    }
  }
}