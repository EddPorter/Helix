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

using Helix.Core.Expressions;
using Helix.Core.Recombinations;
using Helix.Core.Tests.Initialisations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Helix.Core.Tests.Recombinations
{
  [TestClass]
  public class UniformSubtreeCrossoverTests
  {
    [TestMethod]
    public void
      UniformSubtreeCrossover_BreadFirstSearch_ReturnsElementFromSingleNodeTree()
    {
      var root = new Tree(new FakeTerminal());
      var result = UniformSubtreeCrossover.BreadthFirstSearch(root, null, 0);

      Assert.AreSame(root, result.Tree);
      Assert.IsNull(result.Parent);
    }

    [TestMethod]
    public void UniformSubtreeCrossover_BreadFirstSearch_ReturnsCorrectNode()
    {
      var root = new Tree(new FakeFunction(),
        new ITree[]
        {new Tree(new FakeTerminal(0)), new Tree(new FakeTerminal(1))});

      var result = UniformSubtreeCrossover.BreadthFirstSearch(root, null, 0);
      Assert.AreSame(root, result.Tree);
      Assert.IsNull(result.Parent);

      result = UniformSubtreeCrossover.BreadthFirstSearch(root, null, 1);
      Assert.AreEqual(0, ((FakeTerminal) result.Tree.Node).Id);
      Assert.AreSame(root, result.Parent);

      result = UniformSubtreeCrossover.BreadthFirstSearch(root, null, 2);
      Assert.AreEqual(1, ((FakeTerminal) result.Tree.Node).Id);
      Assert.AreSame(root, result.Parent);
    }

    [TestMethod]
    public void UniformSubtreeCrossover_Recombine_MergesTwoTrees()
    {
      var firstTree = new Tree(new FakeFunction(),
        new ITree[]
        {new Tree(new FakeTerminal(0)), new Tree(new FakeTerminal(1))});
      var secondTree = new Tree(new FakeTerminal(2));

      var recombiner = new UniformSubtreeCrossover();
      var child = recombiner.Recombine(firstTree, secondTree);

      Assert.AreNotSame(firstTree, child);
      Assert.AreNotSame(secondTree, child);
    }

    [TestMethod]
    public void
      UniformSubtreeCrossover_Recombine_PickingTheRootOfTheFirstReturnsACloneOfTheSecond
      ()
    {
      var firstTree = new Tree(new FakeTerminal(0));
      var secondTree = new Tree(new FakeFunction(),
        new ITree[]
        {new Tree(new FakeTerminal(1)), new Tree(new FakeTerminal(2))});

      var recombiner = new UniformSubtreeCrossover();
      var child = recombiner.Recombine(firstTree, secondTree);

      Assert.AreEqual(secondTree.Size, child.Size);
      Assert.AreEqual(secondTree.Depth, child.Depth);
      Assert.AreNotSame(secondTree, child);
    }
  }
}