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
using Helix.Core.Expressions;
using Helix.Core.Initialisers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Helix.Core.Tests.Initialisers
{
  [TestClass]
  public class FullInitialiserStrategyTests
  {
    [TestMethod]
    public void Create_ReturnsNewInstance()
    {
      var strategy = new FullInitialiserStrategy();
      Assert.IsNotNull(strategy);
    }

    [TestMethod]
    [ExpectedException(typeof (ArgumentException))]
    public void
      GenerateRandomExpressionTree_WithEmptyFunctionCollection_ThrowsException()
    {
      var strategy = new FullInitialiserStrategy();
      strategy.GenerateRandomExpressionTree(new List<Type>(),
        new[] {typeof (FakeTerminal)}, 0);
    }

    [TestMethod]
    [ExpectedException(typeof (ArgumentException))]
    public void
      GenerateRandomExpressionTree_WithEmptyTerminalCollection_ThrowsException()
    {
      var strategy = new FullInitialiserStrategy();
      strategy.GenerateRandomExpressionTree(new[] {typeof (FakeFunction)},
        new List<Type>(), 0);
    }

    [TestMethod]
    [ExpectedException(typeof (ArgumentException))]
    public void
      GenerateRandomExpressionTree_WithFunctionCollectionContainingNonFunctionTypes_ThrowsException
      ()
    {
      var strategy = new FullInitialiserStrategy();
      strategy.GenerateRandomExpressionTree(new[] {typeof (FakeTerminal)},
        new[] {typeof (FakeTerminal)}, 0);
    }

    [TestMethod]
    public void
      GenerateRandomExpressionTree_WithMaxDepthZero_ReturnsTreeWithSingleTerminal
      ()
    {
      var strategy = new FullInitialiserStrategy();
      var tree =
        strategy.GenerateRandomExpressionTree(new[] {typeof (FakeFunction)},
          new[] {typeof (FakeTerminal)}, 0);
      Assert.IsInstanceOfType(tree.Node, typeof (ITerminal));
    }

    [TestMethod]
    [ExpectedException(typeof (ArgumentOutOfRangeException))]
    public void
      GenerateRandomExpressionTree_WithNegativeMaxDepth_ThrowsException()
    {
      var strategy = new FullInitialiserStrategy();
      strategy.GenerateRandomExpressionTree(new[] {typeof (FakeFunction)},
        new[] {typeof (FakeTerminal)}, -1);
    }

    [TestMethod]
    public void
      GenerateRandomExpressionTree_WithNonZeroMaxDepth_ReturnsTreeWithCorrectDepth
      ()
    {
      const int depth = 3;
      var strategy = new FullInitialiserStrategy();
      var tree =
        strategy.GenerateRandomExpressionTree(new[] {typeof (FakeFunction)},
          new[] {typeof (FakeTerminal)}, depth);
      Assert.AreEqual(depth, tree.Depth);
    }

    [TestMethod]
    public void
      GenerateRandomExpressionTree_WithNonZeroMaxDepth_ReturnsTreeWithMaxSize()
    {
      var strategy = new FullInitialiserStrategy();
      var tree =
        strategy.GenerateRandomExpressionTree(new[] {typeof (FakeFunction)},
          new[] {typeof (FakeTerminal)}, 3);
      // depth n => size = 2^(n+1) - 1
      Assert.AreEqual(15, tree.Size);
    }

    [TestMethod]
    [ExpectedException(typeof (ArgumentNullException))]
    public void
      GenerateRandomExpressionTree_WithNullFunctionCollection_ThrowsException()
    {
      var strategy = new FullInitialiserStrategy();
      strategy.GenerateRandomExpressionTree(null, new[] {typeof (FakeTerminal)},
        0);
    }

    [TestMethod]
    [ExpectedException(typeof (ArgumentNullException))]
    public void
      GenerateRandomExpressionTree_WithNullTerminalCollection_ThrowsException()
    {
      var strategy = new FullInitialiserStrategy();
      strategy.GenerateRandomExpressionTree(new[] {typeof (FakeFunction)}, null,
        0);
    }

    [TestMethod]
    [ExpectedException(typeof (ArgumentException))]
    public void
      GenerateRandomExpressionTree_WithTerminalCollectionContainingNonFunctionTypes_ThrowsException
      ()
    {
      var strategy = new FullInitialiserStrategy();
      strategy.GenerateRandomExpressionTree(new[] {typeof (FakeFunction)},
        new[] {typeof (FakeFunction)}, 0);
    }
  }
}