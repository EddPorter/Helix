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
using System.Diagnostics.Contracts;
using System.Linq;
using Helix.Core.Expressions;
using IPopulation =
  System.Collections.Generic.ICollection<Helix.Core.Expressions.Tree>;

namespace Helix.Core.Selections
{
  /// <summary>Provides a means of selecting individuals from a population.</summary>
  [ContractClass(typeof (SelectorContract))]
  internal interface ISelector
  {
    /// <summary>Given a population of candidate trees, returns a tree from the group.</summary>
    /// <param name="population">A collection of candidate trees to select from.</param>
    /// <returns>A tree from the population.</returns>
    ITree Select(IPopulation population);
  }

  [ContractClassFor(typeof (ISelector))]
  internal abstract class SelectorContract : ISelector
  {
    #region ISelector Members

    /// <summary>The returned tree is a member of the original population.</summary>
    ITree ISelector.Select(IPopulation population)
    {
      Contract.Requires<ArgumentNullException>(population != null,
        "A valid population must be provided.");
      Contract.Requires<ArgumentException>(population.Count > 0,
        "The population must contain at least one member.");

      Contract.Ensures(Contract.Result<ITree>() != null);
      Contract.Ensures(
        Contract.OldValue(population).Contains(Contract.Result<ITree>()));

      return default(ITree);
    }

    #endregion
  }
}