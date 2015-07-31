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

namespace Helix.Core.Initialisers
{
  internal interface IInitialiserStrategy
  {
    /// <summary>Creates a new expression tree from scratch.</summary>
    /// <param name="functionCollection">
    ///   The collection of function types available for
    ///   use. Each element must derive from <see cref="IFunction" />.
    /// </param>
    /// <param name="terminalCollection">
    ///   The collection of terminals available for use.
    ///   Each element must derive from <see cref="ITerminal" />.
    /// </param>
    /// <param name="maxDepth">
    ///   The maximum allowed depth for expressions. Must be
    ///   non-negative.
    /// </param>
    /// <returns>The newly generated expression tree.</returns>
    ITree GenerateRandomExpressionTree(ICollection<Type> functionCollection,
      ICollection<Type> terminalCollection, int maxDepth);
  }
}