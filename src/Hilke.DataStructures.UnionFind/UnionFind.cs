namespace Hilke.DataStructures;

/// <summary>
///   Represents a collection of disjoint sets, where sets can be efficiently merged and a unique representant of each disjoint set queried.
/// </summary>
public sealed class UnionFind<TElement> where TElement : notnull
{
    private readonly Dictionary<TElement, Node> _nodes;

    /// <summary>Initialize a collection of singleton sets.</summary>
    /// <param name="elements">The collection of elements that are used for the singletons.</param>
    public UnionFind(IEnumerable<TElement> elements, IEqualityComparer<TElement> comparer = null) =>
        _nodes = elements.ToDictionary(element => element, element => new Node(element), comparer ?? EqualityComparer<TElement>.Default);

    /// <summary>Initializes a collection of zero disjoint set.</summary>
    public UnionFind(IEqualityComparer<TElement> comparer = null) => _nodes = new Dictionary<TElement, Node>(comparer ?? EqualityComparer<TElement>.Default);

    /// <summary>Gets the representant elements of all the disjoint sets.</summary>
    /// <returns>A collection where each element is a representant of one of the disjoint set.</returns>
    public IEnumerable<TElement> GetRepresentants() =>
        _nodes.Values.Where(n => ReferenceEquals(n, n.Parent)).Select(n => n.Element);

    /// <summary>Gets the union of all the disjoint sets.</summary>
    public IEnumerable<TElement> GetElements() => _nodes.Keys;

    /// <summary>Gets the representant of the disjoint set to which <paramref name="element" /> belongs.</summary>
    /// <param name="element">At element of the disjoint set whose representant is looked for.</param>
    /// <returns>The representant of the disjoint set to which <paramref name="element" /> belongs.</returns>
    /// <remarks>
    ///   If <paramref name="element" /> is not part of any disjoint set, it is added to the collection of
    ///   disjoint sets as a singleton set. In this case, since <paramref name="element"/> is the
    ///   representant of its own singleton set, <paramref name="element" /> is returned.
    /// </remarks>
    public TElement Find(TElement element) => FindNode(element).Element;

    /// <summary>Merges the two disjoint sets to whom <paramref name="element1" /> and <paramref name="element2"> belong.</summary>
    /// <param name="element1">An element of the first disjoint set.</param>
    /// <param name="element2">An element of the second disjoint set.</param>
    /// <remarks>
    ///   When <paramref name="element1" /> and <paramref name="element2" /> belong to the same disjoint
    ///   set, <c>Union</c> has no effect. In other word, <c>Union</c> is idempotent.
    /// </remarks>
    public void Union(TElement element1, TElement element2)
    {
        var node1 = FindNode(element1);
        var node2 = FindNode(element2);

        if (!ReferenceEquals(node1, node2))
        {
            if (node1.Rank < node2.Rank)
            {
                node1.Parent = node2;
            }
            else if (node2.Rank < node1.Rank)
            {
                node2.Parent = node1;
            }
            else
            {
                node2.Parent = node1;
                node1.Rank += 1;
            }
        }
    }

    private Node FindNode(TElement element)
    {
        if (!_nodes.TryGetValue(element, out var node))
        {
            node = new Node(element);
            _nodes.Add(element, node);
        }

        return FindNodeRec(node);
    }

    private Node FindNodeRec(Node node)
    {
        if (!ReferenceEquals(node, node.Parent))
        {
            node.Parent = FindNodeRec(node.Parent);
        }

        return node.Parent;
    }

    private sealed class Node
    {
        public Node(TElement element)
        {
            Element = element;
            Parent = this;
        }

        public TElement Element { get; }

        public Node Parent { get; set; }

        public int Rank { get; set; }
    }
}
