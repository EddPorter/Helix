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

using System;
using System.Diagnostics.Contracts;
using Helix.Core.Expressions;

namespace Helix.Core.Initialisations
{
  [ContractClass(typeof (InitialiserStrategyContract))]
  internal interface IInitialiserStrategy
  {
    /// <summary>Creates a new expression tree from scratch.</summary>
    /// <param name="maxDepth">
    ///   The maximum allowed depth for expressions. Must be
    ///   non-negative.
    /// </param>
    /// <returns>The newly generated expression tree.</returns>
    ITree GenerateRandomExpressionTree(int maxDepth);
  }

  [ContractClassFor(typeof (IInitialiserStrategy))]
  internal abstract class InitialiserStrategyContract : IInitialiserStrategy
  {
    #region IInitialiserStrategy Members

    ITree IInitialiserStrategy.GenerateRandomExpressionTree(int maxDepth)
    {
      Contract.Requires<ArgumentOutOfRangeException>(maxDepth >= 0);

      Contract.Ensures(Contract.Result<ITree>() != null);
      Contract.Ensures(Contract.Result<ITree>().Depth <= maxDepth);

      return default(ITree);
    }

    #endregion
  }
}