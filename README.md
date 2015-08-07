# Helix

## Overview

_Helix_ is a library for implementing genetic programming algorithms. Such
algorithms make use of the concepts of breeding and mutation found in nature to
_evolve_ a better algorithm over a number of generations.

## Building

The solution requires the Microsoft CodeContracts extension in order to run:
https://visualstudiogallery.msdn.microsoft.com/1ec7db13-3363-46c9-851f-1ce455f66970

## Notes

* The solution makes use of the Troschuetz.Random library to provide
  pseudo-random number generators with reliable distributions. The built in
  `System.Random` class is flawed and will probably never be fixed. See the 
  following links for more information:
  * https://connect.microsoft.com/VisualStudio/feedback/details/634761/system-random-serious-bug
  * http://stackoverflow.com/a/6842191/445517
  * http://blogs.msdn.com/b/ericlippert/archive/2012/02/21/generating-random-non-uniform-data-in-c.aspx

## Background Reading

R. Poli, W. B. Langdon, and N. F. McPhee. A field guide to genetic programming.
Published via http://lulu.com and freely available at
http://www.gp-field-guide.org.uk