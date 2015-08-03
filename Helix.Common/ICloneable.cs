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

using System.Diagnostics.Contracts;

namespace Helix.Common
{
  /// <summary>
  ///   An object that can create a deep copy of itself, that is a new
  ///   instance of the class with the same value as an existing instance.
  /// </summary>
  /// <typeparam name="T">
  ///   The type of the current object. It is possible to specify
  ///   any type here and use the method as a converter, but this should be avoided
  ///   as it does not fit the semantic use of the interface.
  /// </typeparam>
  [ContractClass(typeof (CloneableContract<>))]
  public interface ICloneable<out T>
  {
    /// <summary>
    ///   Creates a new <typeparamref name="T" /> that is a copy of the current
    ///   instance.
    /// </summary>
    /// <returns>A new <typeparamref name="T" /> that is a copy of this instance.</returns>
    T Clone();
  }

  [ContractClassFor(typeof (ICloneable<>))]
  internal abstract class CloneableContract<T> : ICloneable<T>
  {
    /// <summary>The resulting copy cannot be the same as the original object.</summary>
    T ICloneable<T>.Clone()
    {
      Contract.Ensures(Contract.Result<T>() != null);
      Contract.Ensures(!ReferenceEquals(Contract.Result<T>(), this));
      return default(T);
    }
  }
}