namespace Helix.Core.Expressions
{
  internal class TreeAndParent
  {
    internal TreeAndParent(ITree tree, ITree parent)
    {
      Tree = tree;
      Parent = parent;
    }

    internal ITree Tree { get; }
    internal ITree Parent { get; }
  }
}