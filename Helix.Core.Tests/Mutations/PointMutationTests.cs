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
using Helix.Core.Mutations;
using Helix.Core.Tests.Initialisations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Helix.Core.Tests.Mutations
{
  [TestClass]
  public class PointMutationTests
  {
    [TestMethod]
    public void PointMutation_Mutate_ReplacesOneNodeTreeWithAnother()
    {
      ICollection<Type> terminalCollection = new List<Type>
      {
        typeof (FakeTerminal)
      };
      ICollection<Type> functionCollection = new List<Type>();
      var mutation = new PointMutation(functionCollection, terminalCollection,
        1.0d);

      var individual = new Tree(new FakeTerminal(1));
      var mutant = mutation.Mutate(individual);

      Assert.IsNotNull(mutant);
      Assert.IsInstanceOfType(mutant.Node, typeof (ITerminal));
      Assert.AreEqual(0, ((FakeTerminal) mutant.Node).Id);
    }

    [TestMethod]
    public void PointMutation_Mutate_DoesNotExpandOrShrinkTree()
    {
      var individual = new Tree(new FakeFunction(),
        new List<ITree>
        {
          new Tree(new FakeTerminal(1)),
          new Tree(new FakeTerminal(2))
        });
      ICollection<Type> terminalCollection = new List<Type>
      {
        typeof (FakeTerminal)
      };
      ICollection<Type> functionCollection = new List<Type>
      {
        typeof (FakeFunction)
      };
      var mutation = new PointMutation(functionCollection, terminalCollection,
        0.5);

      var mutant = mutation.Mutate(individual);

      Assert.IsNotNull(mutant);
      // Original tree has same depth.
      Assert.AreEqual(individual.Depth, mutant.Depth);
    }
  }
}