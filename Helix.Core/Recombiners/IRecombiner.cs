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
using Helix.Core.Expressions;

namespace Helix.Core.Recombiners
{
  /// <summary>
  ///   Takes two individuals and combines them to create a third individual:
  ///   their offspring.
  /// </summary>
  [ContractClass(typeof (RecombinerContract))]
  public interface IRecombiner
  {
    /// <summary>
    ///   Combines two individuals into a third, which has elements of both
    ///   parents.
    /// </summary>
    /// <param name="first">The first individual to use.</param>
    /// <param name="second">The second individual to use.</param>
    /// <returns>A third individual with elements of the two parents.</returns>
    ITree Recombine(ITree first, ITree second);
  }

  [ContractClassFor(typeof (IRecombiner))]
  internal abstract class RecombinerContract : IRecombiner
  {
    #region IRecombiner Members

    public ITree Recombine(ITree first, ITree second)
    {
      Contract.Requires<ArgumentNullException>(first != null);
      Contract.Requires<ArgumentNullException>(second != null);

      Contract.Ensures(Contract.Result<ITree>() != null);

      return default(ITree);
    }

    #endregion
  }
}