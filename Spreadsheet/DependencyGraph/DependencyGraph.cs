// Skeleton implementation written by Joe Zachary for CS 3500, September 2013
// Version 1.1 - Joe Zachary
//   (Fixed error in comment for RemoveDependency)
// Version 1.2 - Daniel Kopta Fall 2018
//   (Clarified meaning of dependent and dependee)
//   (Clarified names in solution/project structure)
// Version 1.3 - H. James de St. Germain Fall 2024
// <authors> Prachi Aswani </authors>
// <date> 9/13/2024 </date>

using System.Diagnostics;

namespace CS3500.DependencyGraph;

/// <summary>
///   <para>
///     (s1,t1) is an ordered pair of strings, meaning t1 depends on s1.
///     (in other words: s1 must be evaluated before t1.)
///   </para>
///   <para>
///     A DependencyGraph can be modeled as a set of ordered pairs of strings.
///     Two ordered pairs (s1,t1) and (s2,t2) are considered equal if and only
///     if s1 equals s2 and t1 equals t2.
///   </para>
///   <remarks>
///     Recall that sets never contain duplicates.
///     If an attempt is made to add an element to a set, and the element is already
///     in the set, the set remains unchanged.
///   </remarks>
///   <para>
///     Given a DependencyGraph DG:
///   </para>
///   <list type="number">
///     <item>
///       If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
///       (The set of things that depend on s.)
///     </item>
///     <item>
///       If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
///       (The set of things that s depends on.)
///     </item>
///   </list>
///   <para>
///      For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}.
///   </para>
///   <code>
///     dependents("a") = {"b", "c"}
///     dependents("b") = {"d"}
///     dependents("c") = {}
///     dependents("d") = {"d"}
///     dependees("a")  = {}
///     dependees("b")  = {"a"}
///     dependees("c")  = {"a"}
///     dependees("d")  = {"b", "d"}
///   </code>
/// </summary>
public class DependencyGraph
{
    /// <summary>
    /// Dictionary mapping nodes to their set of dependents (those that 
    /// depend on this node). With a string as it's key, and a Hashset
    /// as it's value.
    /// </summary>
    private Dictionary<string, HashSet<string>> dependents;

    /// <summary>
    /// Dictionary mapping nodes to their set of dependees (those that 
    /// this node depends on). With a string as it's key, and a Hashset
    /// as it's value.
    /// </summary>
    private Dictionary<string, HashSet<string>> dependees;

    /// <summary>
    ///   Initializes a new instance of the <see cref="DependencyGraph"/> class.
    ///   The initial DependencyGraph is empty.
    /// </summary>
    public DependencyGraph()
    {
        dependents = new Dictionary<string, HashSet<string>>();
        dependees = new Dictionary<string, HashSet<string>>();
    }

    /// <summary>
    /// The number of ordered pairs in the DependencyGraph.
    /// </summary>
    public int Size
    {
        get;
        private set;
    }

    /// <summary>
    ///   Reports whether the given node has dependents (i.e., other nodes depend on it).
    /// </summary>
    /// <param name="nodeName"> The name of the node.</param>
    /// <returns> true if the node has dependents. </returns>
    public bool HasDependents(string nodeName)
    {
        // Try to get the set of dependents for the given node and check if it has any dependents
        return dependents.TryGetValue(nodeName, out HashSet<string>? value) && value.Count > 0;
        //if (dependents.TryGetValue(nodeName, out HashSet<string>? value))
        //    return value.Count != 0;
        //else
        //    return false;
    }

    /// <summary>
    ///   Reports whether the given node has dependees (i.e., depends on one or more other nodes).
    /// </summary>
    /// <returns> true if the node has dependees.</returns>
    /// <param name="nodeName">The name of the node.</param>
    public bool HasDependees(string nodeName)
    {
        // Return true if the node has dependees
        return dependees.ContainsKey(nodeName);
    }

    /// <summary>
    ///   <para>
    ///     Returns the dependents of the node with the given name.
    ///   </para>
    /// </summary>
    /// <param name="nodeName"> The node we are looking at.</param>
    /// <returns> The dependents of nodeName. </returns>
    public IEnumerable<string> GetDependents(string nodeName)
    {
        // If the node has dependents, return the set of dependents;
        if (dependents.ContainsKey(nodeName))
            return dependents[nodeName];
        else
            // otherwise, return an empty set
            return new HashSet<string>();
    }

    /// <summary>
    ///   <para>
    ///     Returns the dependees of the node with the given name.
    ///   </para>
    /// </summary>
    /// <param name="nodeName"> The node we are looking at.</param>
    /// <returns> The dependees of nodeName. </returns>
    public IEnumerable<string> GetDependees(string nodeName)
    {
        // If the node has dependees, return the set of dependees;
        if (dependees.ContainsKey(nodeName))
            return dependees[nodeName];
        else
            // otherwise, return an empty set
            return new HashSet<string>();
    }

    /// <summary>
    /// <para>Adds the ordered pair (dependee, dependent), if it doesn't exist.</para>
    /// <para>
    ///   This can be thought of as: dependee must be evaluated before dependent
    /// </para>
    /// </summary>
    /// <param name="dependee"> the name of the node that must be evaluated first</param>
    /// <param name="dependent"> the name of the node that cannot be evaluated until after dependee</param>
    public void AddDependency(string dependee, string dependent)
    {
        // Only add the dependency if it does not already exist in both dependents and dependees
        if (!HasSpecificDependent(dependee, dependent))
        {
            // Add the dependee to the dependees of the dependent and vice versa
            AddDependee(dependee, dependent);
            AddDependent(dependee, dependent);
            Size++;
        }
    }

    /// <summary>
    /// Helper method to check if a specific node is a dependent of another.
    /// </summary>
    /// <param name="dependee"></param>
    /// <param name="dependent"></param>
    /// <returns></returns>
    private bool HasSpecificDependent(string dependee, string dependent)
    {
        // Returns true if the dependee has the specified dependent
        return HasDependents(dependee) && dependents[dependee].Contains(dependent);
    }

    /// <summary>
    /// Helper method to add a dependent node to a dependee.
    /// </summary>
    /// <param name="dependee">The node that other nodes depend on.</param>
    /// <param name="dependent">The node that depends on the dependee</param>
    private void AddDependent(string dependee, string dependent)
    {
        // Check if the dependee already has dependents.
        if (HasDependents(dependee))
        {
            // add the new dependent to the existing set of dependents.
            dependents[dependee].Add(dependent);
        }
        else if (!HasDependents(dependee))
        {
            // create a new set for the dependents
            HashSet<string> dependentsHash = new HashSet<string>();
            dependentsHash.Add(dependent);
            dependents.Add(dependee, dependentsHash);
        }
    }

    /// <summary>
    /// Helper method to add a dependee to the dependent's set of dependees.
    /// </summary>
    /// <param name="dependee">The node that must be evaluated first (the dependee).</param>
    /// <param name="dependent">The node that depends on the dependee.</param>
    private void AddDependee(string dependee, string dependent)
    {
        // Check if the dependent already has dependees.
        if (HasDependees(dependent))
        {
            // add the new dependee to the existing set of dependees.
            dependees[dependent].Add(dependee);
        }
        else if (!HasDependees(dependent))
        {
            // create a new set for the dependees
            HashSet<string> dependeesHash = new HashSet<string>();
            dependeesHash.Add(dependee);
            dependees.Add(dependent, dependeesHash);
        }
    }

    /// <summary>
    ///   <para>
    ///     Removes the ordered pair (dependee, dependent), if it exists.
    ///   </para>
    /// </summary>
    /// <param name="dependee"> The name of the node that must be evaluated first</param>
    /// <param name="dependent"> The name of the node that cannot be evaluated until after dependee</param>
    public void RemoveDependency(string dependee, string dependent)
    {
        // if the input exists in the form (dependee, dependent)
        if (HasSpecificDependent(dependee, dependent))
        {
            // Remove the dependent from the dependee's dependents list
            dependents[dependee].Remove(dependent);

            // If the dependee has no more dependents, remove it from the dependents dictionary
            if (dependents[dependee].Count == 0)
                dependents.Remove(dependee);

            // Remove the dependee from the dependent's dependees list
            dependees[dependent].Remove(dependee);

            // If the dependent has no more dependees, remove it from the dependees dictionary
            if (dependees[dependent].Count == 0)
                dependees.Remove(dependent);

            // Decrease the size of the graph
            Size--;
        }
    }

    /// <summary>
    ///   Removes all existing ordered pairs of the form (nodeName, *).  Then, for each
    ///   t in newDependents, adds the ordered pair (nodeName, t).
    /// </summary>
    /// <param name="nodeName"> The name of the node who's dependents are being replaced </param>
    /// <param name="newDependents"> The new dependents for nodeName</param>
    public void ReplaceDependents(string nodeName, IEnumerable<string> newDependents)
    {
        // If the node has existing dependents, remove each dependent relationship
        if (HasDependents(nodeName))
        {
            // Convert to a list to avoid modifying the collection during iteration
            foreach (string dependent in dependents[nodeName].ToList())
            {
                RemoveDependency(nodeName, dependent);
            }
        }

        // Add the new dependents specified in newDependents
        foreach (string dependent in newDependents.ToList())
        {
            AddDependency(nodeName, dependent);
        }
    }

    /// <summary>
    ///   <para>
    ///     Removes all existing ordered pairs of the form (*, nodeName).  Then, for each
    ///     t in newDependees, adds the ordered pair (t, nodeName).
    ///   </para>
    /// </summary>
    /// <param name="nodeName"> The name of the node who's dependees are being replaced</param>
    /// <param name="newDependees"> The new dependees for nodeName</param>
    public void ReplaceDependees(string nodeName, IEnumerable<string> newDependees)
    {
        // If the node has existing dependees, remove each dependee relationship
        if (HasDependees(nodeName))
        {
            // Convert to a list to avoid modifying the collection during iteration
            foreach (string dependee in dependees[nodeName].ToList())
            {
                RemoveDependency(dependee, nodeName);
            }
        }

        // Add the new dependees specified in newDependees
        foreach (string dependee in newDependees.ToList())
        {
            AddDependency(dependee, nodeName);
        }
    }
}