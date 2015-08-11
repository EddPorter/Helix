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
using System.Linq;
using Helix.Core.Expressions;
using Helix.Core.Selections;
using Helix.Core.Tests.Initialisations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Helix.Core.Tests.Selections
{
  [TestClass]
  public class TournamentSelectorTests
  {
    [TestMethod]
    public void
      TournamentSelector_Select_Picks_Single_Individual_From_Population_Of_One()
    {
      var selector = new TournamentSelector(tree => 0.0f);
      var item = new Tree(new FakeTerminal());
      var winner = selector.Select(new List<Tree> {item});
      Assert.AreSame(item, winner);
    }

    [TestMethod]
    public void
      TournamentSelector_Select_Returns_An_Individual_From_The_Population()
    {
      var selector = new TournamentSelector(tree => 0.0f);
      var population =
        Enumerable.Range(0, 4)
          .Select(i => new Tree(new FakeTerminal(i)))
          .ToList();
      var winner = selector.Select(population);
      CollectionAssert.Contains(population, winner);
    }

    [TestMethod]
    public void
      TournamentSelector_Select_Returns_Fittest_Individual_From_Small_Population
      ()
    {
      var selector =
        new TournamentSelector(tree => ((FakeTerminal) tree.Node).Id);
      var population =
        Enumerable.Range(0, 4)
          .Select(i => new Tree(new FakeTerminal(i)))
          .ToList();
      var winner = selector.Select(population);
      Assert.AreSame(population.Last(), winner);
    }

    [TestMethod]
    public void
      TournamentSelector_Select_Returns_Fittest_Individual_From_Selection()
    {
      var selector =
        new TournamentSelector(tree => ((FakeTerminal) tree.Node).Id);
      var population =
        Enumerable.Range(0, 9)
          .Select(i => new Tree(new FakeTerminal(i)))
          .ToList();

      // Only eight items will be compared from this list of nine. So the winner will be 7 or 8 depending on the random choice.
      var winner = selector.Select(population);

      Assert.IsTrue(((FakeTerminal) winner.Node).Id >= 7);
    }

    [TestMethod]
    public void
      TournamentSelector_Select_Handles_Byes_And_Does_Not_Return_A_Null_Tree()
    {
      var selector =
        new TournamentSelector(tree => ((FakeTerminal) tree.Node).Id);
      var population =
        Enumerable.Range(0, 100)
          .Select(i => new Tree(new FakeTerminal(i)))
          .ToList();

      // From 100 items, 10% will be selected, i.e. 10. Thus a tournament size of 16 is needed creating 6 byes.
      var winner = selector.Select(population);

      Assert.IsNotNull(winner);
      Assert.IsTrue(((FakeTerminal) winner.Node).Id >= 9);
    }
  }
}