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
using Helix.Core.Expressions;
using Helix.Core.Tests.Initialisations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Helix.Core.Tests.Expressions
{
  [TestClass]
  public class TreeTests
  {
    [TestMethod]
    public void Tree_Clone_AsFunction_Creates_New_Instance_Equal_To_First()
    {
      var tree = new Tree(new FakeFunction(),
        new List<ITree>
        {
          new Tree(new FakeTerminal()),
          new Tree(new FakeTerminal())
        });

      var newTree = tree.Clone();

      Assert.AreNotSame(tree, newTree);
      Assert.AreNotSame(tree.Node, newTree.Node);
      Assert.IsInstanceOfType(newTree.Node, typeof (IFunction));
      Assert.AreEqual(tree.Size, newTree.Size);
      Assert.AreEqual(tree.Depth, newTree.Depth);
    }

    [TestMethod]
    public void Tree_Clone_AsTerminal_Creates_New_Instance_Equal_To_First()
    {
      var tree = new Tree(new FakeTerminal());

      var newTree = tree.Clone();

      Assert.AreNotSame(tree, newTree);
      Assert.AreNotSame(tree.Node, newTree.Node);
      Assert.IsInstanceOfType(newTree.Node, typeof (ITerminal));
      Assert.AreEqual(tree.Size, newTree.Size);
      Assert.AreEqual(tree.Depth, newTree.Depth);
    }
  }
}