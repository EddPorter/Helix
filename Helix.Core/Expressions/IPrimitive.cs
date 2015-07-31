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

namespace Helix.Core.Expressions
{
  /// <summary>
  ///   An element in a syntax tree. For example, a
  ///   <see cref="IFunction" /> or a <see cref="ITerminal" />.
  /// </summary>
  /// <remarks>
  ///   The collection of allowed functions and terminals is known as the
  ///   Primitive Set.
  /// </remarks>
  public interface IPrimitive
  {
  }
}