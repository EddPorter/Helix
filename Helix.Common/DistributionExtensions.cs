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

using Troschuetz.Random;

namespace Helix.Common
{
  /// <summary>Helper extensions for the Troschuetz.Random Distribution classes.</summary>
  public static class DistributionExtensions
  {
    /// <summary>
    ///   Sets up the random number distribution for the next call to get a
    ///   value.
    /// </summary>
    /// <param name="distribution">The distribution to modify.</param>
    /// <param name="min">The smallest value that can be returned.</param>
    /// <param name="exclusiveMax">
    ///   The upper bound of the value that can be returned.
    ///   This value itself is never returned.
    /// </param>
    public static void ConfigureDistribution(
      this ContinuousUniformDistribution distribution, double min,
      double exclusiveMax)
    {
      // See http://www.codeproject.com/Articles/15102/NET-random-number-generators-and-distributions
      // for details on the distribution configurations.
      distribution.Alpha = min;
      distribution.Beta = exclusiveMax;
    }
  }
}