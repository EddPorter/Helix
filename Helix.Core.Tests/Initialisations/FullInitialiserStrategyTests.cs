﻿//  Helix - A Genetic Programming Library
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

using Helix.Core.Expressions;
using Helix.Core.Initialisations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Helix.Core.Tests.Initialisations
{
  [TestClass]
  public class FullInitialiserStrategyTests
  {
    [TestMethod]
    public void
      GenerateRandomExpressionTree_WithMaxDepthZero_ReturnsTreeWithSingleTerminal
      ()
    {
      var functionCollection = new[] {typeof (FakeFunction)};
      var terminalCollection = new[] {typeof (FakeTerminal)};
      var strategy = new FullInitialiserStrategy(functionCollection,
        terminalCollection);

      var tree = strategy.GenerateRandomExpressionTree(0);

      Assert.IsInstanceOfType(tree.Node, typeof (ITerminal));
    }

    [TestMethod]
    public void
      GenerateRandomExpressionTree_WithNonZeroMaxDepth_ReturnsTreeWithCorrectDepth
      ()
    {
      const int depth = 3;
      var functionCollection = new[] {typeof (FakeFunction)};
      var terminalCollection = new[] {typeof (FakeTerminal)};
      var strategy = new FullInitialiserStrategy(functionCollection,
        terminalCollection);

      var tree = strategy.GenerateRandomExpressionTree(depth);

      Assert.AreEqual(depth, tree.Depth);
    }

    [TestMethod]
    public void
      GenerateRandomExpressionTree_WithNonZeroMaxDepth_ReturnsTreeWithMaxSize()
    {
      var functionCollection = new[] {typeof (FakeFunction)};
      var terminalCollection = new[] {typeof (FakeTerminal)};
      var strategy = new FullInitialiserStrategy(functionCollection,
        terminalCollection);

      var tree = strategy.GenerateRandomExpressionTree(3);

      // depth n => size = 2^(n+1) - 1
      Assert.AreEqual(15, tree.Size);
    }
  }
}