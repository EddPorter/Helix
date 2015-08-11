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

namespace Helix.Core.Mutations
{
  /// <summary>Modifies a given individual to create a new one.</summary>
  [ContractClass(typeof (MutationContract))]
  public interface IMutation
  {
    /// <summary>Modifies the given individual to create a new one.</summary>
    /// <param name="tree">The individual to mutate.</param>
    /// <returns>The new individual.</returns>
    ITree Mutate(ITree tree);
  }

  [ContractClassFor(typeof (IMutation))]
  internal abstract class MutationContract : IMutation
  {
    #region IMutation Members

    ITree IMutation.Mutate(ITree tree)
    {
      Contract.Requires<ArgumentNullException>(tree != null);
      Contract.Ensures(Contract.Result<ITree>() != null);

      return default(ITree);
    }

    #endregion
  }
}