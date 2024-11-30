// <authors> Prachi Aswani </authors>
// <date> 9/13/2024 </date>
namespace CS3500.DevelopmentTests;

using CS3500.DependencyGraph;

/// <summary>
///   This is a test class for DependencyGraphTest and is intended
///   to contain all DependencyGraphTest Unit Tests
/// </summary>
[TestClass]
public class DependencyGraphTests
{
    // Simple tests

    // Tests for AddDependecy() method

    /// <summary>
    /// Tests adding a single dependency between two nodes and verifies
    /// that the dependency is correctly reflected in both directions.
    /// </summary>
    [TestMethod]
    public void AddDependency_TestAddSingleDependency_Valid()
    {
        var graph = new DependencyGraph();
        graph.AddDependency("a", "b");

        Assert.IsTrue(graph.GetDependents("a").Contains("b"));
        Assert.IsTrue(graph.GetDependees("b").Contains("a"));
        Assert.AreEqual(1, graph.Size);
    }

    /// <summary>
    /// Tests adding multiple independent dependencies and verifies
    /// that each dependency is correctly reflected in both directions.
    /// </summary>
    [TestMethod]
    public void AddDependency_TestAddMultipleDependencies_Valid()
    {
        var graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.AddDependency("c", "d");

        Assert.IsTrue(graph.GetDependents("a").Contains("b"));
        Assert.IsTrue(graph.GetDependees("b").Contains("a"));
        Assert.IsTrue(graph.GetDependents("c").Contains("d"));
        Assert.IsTrue(graph.GetDependees("d").Contains("c"));
        Assert.AreEqual(2, graph.Size);
    }

    /// <summary>
    /// Tests adding the same dependency multiple times to ensure
    /// no duplication of dependencies in the graph.
    /// </summary>
    [TestMethod]
    public void AddDependency_AddDependencyAlreadyExists_NoDuplication()
    {
        var graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.AddDependency("a", "b"); // Adding the same dependency again

        Assert.IsTrue(graph.GetDependents("a").Contains("b"));
        Assert.IsTrue(graph.GetDependees("b").Contains("a"));
        Assert.AreEqual(1, graph.Size);
    }

    /// <summary>
    /// Tests adding dependencies with existing nodes to ensure
    /// the graph correctly reflects the new dependencies.
    /// </summary>
    [TestMethod]
    public void AddDependency_TestAddExistingDependencies_Valid()
    {
        var graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.AddDependency("c", "a");

        // Validate that existing dependencies are correctly added
        Assert.IsTrue(graph.GetDependents("c").Contains("a"));
        Assert.IsTrue(graph.GetDependees("a").Contains("c"));
        Assert.IsTrue(graph.GetDependents("a").Contains("b"));
        Assert.IsTrue(graph.GetDependees("b").Contains("a"));
        Assert.AreEqual(2, graph.Size);
    }

    /// <summary>
    /// Tests adding a dependency between non-existent nodes to verify
    /// that the graph handles new nodes correctly.
    /// </summary>
    [TestMethod]
    public void TestAddDependencyWithNonExistentNodes()
    {
        var graph = new DependencyGraph();
        graph.AddDependency("x", "y");

        Assert.IsTrue(graph.GetDependents("x").Contains("y"));
        Assert.IsTrue(graph.GetDependees("y").Contains("x"));
        Assert.AreEqual(1, graph.Size);
    }

    /// <summary>
    /// Tests adding dependencies in different orders to ensure 
    /// bidirectional relationships are handled correctly.
    /// </summary>
    [TestMethod]
    public void AddDependency_AddDuplicateDependencyWithDifferentOrder_Valid()
    {
        var graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.AddDependency("b", "a"); // Adding in reverse order

        Assert.IsTrue(graph.GetDependents("a").Contains("b"));
        Assert.IsTrue(graph.GetDependees("b").Contains("a"));
        Assert.IsTrue(graph.GetDependents("b").Contains("a"));
        Assert.IsTrue(graph.GetDependees("a").Contains("b"));
        Assert.AreEqual(2, graph.Size);
    }

    /// <summary>
    /// Tests adding a self-dependency to verify that the graph correctly
    /// handles nodes that depend on themselves.
    /// </summary>
    [TestMethod]
    public void AddDependency_TestAddSelfDependency_Valid()
    {
        var graph = new DependencyGraph();
        graph.AddDependency("a", "a");

        Assert.IsTrue(graph.GetDependents("a").Contains("a"));
        Assert.IsTrue(graph.GetDependees("a").Contains("a"));
        Assert.AreEqual(1, graph.Size);
    }

    /// <summary>
    /// Tests adding a dependency after removing it to ensure the 
    /// graph correctly re-adds the dependency.
    /// </summary>
    [TestMethod]
    public void TestAddDependencyAfterRemovingExistingOne()
    {
        var graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.RemoveDependency("a", "b");
        graph.AddDependency("a", "b");

        Assert.IsTrue(graph.GetDependents("a").Contains("b"));
        Assert.IsTrue(graph.GetDependees("b").Contains("a"));
        Assert.AreEqual(1, graph.Size);
    }

    /// <summary>
    /// Tests adding a dependency that creates a circular reference to ensure the graph handles circular dependencies correctly.
    /// </summary>
    [TestMethod]
    public void AddDependency_TestAddDependencyWithCircularReference()
    {
        var graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.AddDependency("b", "a"); // Circular reference

        Assert.IsTrue(graph.GetDependents("a").Contains("b"));
        Assert.IsTrue(graph.GetDependees("b").Contains("a"));
        Assert.IsTrue(graph.GetDependents("b").Contains("a"));
        Assert.IsTrue(graph.GetDependees("a").Contains("b"));
        Assert.AreEqual(2, graph.Size);
    }

    // Tests for RemoveDependency() method

    /// <summary>
    /// Tests removing a single dependency and verifies that the 
    /// dependency is correctly removed from both directions.
    /// </summary>
    [TestMethod]
    public void RemoveDependency_TestRemoveSingleDependency_Valid()
    {
        var graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.RemoveDependency("a", "b");

        Assert.IsFalse(graph.GetDependents("a").Contains("b"));
        Assert.IsFalse(graph.GetDependees("b").Contains("a"));
        Assert.AreEqual(0, graph.Size);
    }

    /// <summary>
    /// Tests removing multiple dependencies and verifies that all specified 
    /// dependencies are correctly removed from the graph.
    /// </summary>
    [TestMethod]
    public void RemoveDependency_TestRemoveMultipleDependencies_Valid()
    {
        var graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.AddDependency("c", "d");
        graph.RemoveDependency("a", "b");
        graph.RemoveDependency("c", "d");

        Assert.IsFalse(graph.GetDependents("a").Contains("b"));
        Assert.IsFalse(graph.GetDependees("b").Contains("a"));
        Assert.IsFalse(graph.GetDependents("c").Contains("d"));
        Assert.IsFalse(graph.GetDependees("d").Contains("c"));
        Assert.AreEqual(0, graph.Size);
    }

    /// <summary>
    /// Tests removing a non-existent dependency and ensures that the 
    /// graph remains unchanged when the dependency is not present.
    /// </summary>
    [TestMethod]
    public void RemoveDependency_TestemoveNonExistentDependency_Valid()
    {
        var graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.RemoveDependency("c", "d"); // Non-existent dependency

        Assert.IsTrue(graph.GetDependents("a").Contains("b"));
        Assert.IsTrue(graph.GetDependees("b").Contains("a"));
        Assert.AreEqual(1, graph.Size);
    }

    /// <summary>
    /// Tests removing a dependency from an empty graph, ensuring that 
    /// no exception is thrown and the graph remains valid.
    /// </summary>
    [TestMethod]
    public void RemoveDependency_TestRemoveFromEmptyGraph_Valid()
    {
        var graph = new DependencyGraph();
        graph.RemoveDependency("a", "b"); // Graph is empty

        Assert.AreEqual(0, graph.Size);
    }

    /// <summary>
    /// Tests removing one side of a circular dependency and ensures that 
    /// the remaining half of the circular reference is still intact.
    /// </summary>
    [TestMethod]
    public void RemoveDependency_TestRemoveCircularReference_Valid()
    {
        var graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.AddDependency("b", "a"); // Circular reference

        graph.RemoveDependency("a", "b");

        Assert.IsFalse(graph.GetDependents("a").Contains("b"));
        Assert.IsFalse(graph.GetDependees("b").Contains("a"));
        Assert.IsTrue(graph.GetDependents("b").Contains("a"));
        Assert.IsTrue(graph.GetDependees("a").Contains("b"));
        Assert.AreEqual(1, graph.Size);
    }

    // Tests for Size() method

    /// <summary>
    /// Tests the Size of an empty DependencyGraph and verifies that 
    /// it is initialized to zero.
    /// </summary>
    [TestMethod()]
    public void Size_TestEmptyDG_Zero()
    {
        DependencyGraph t = new DependencyGraph();
        Assert.AreEqual(0, t.Size);
    }

    /// <summary>
    /// Tests the Size of the graph after adding multiple dependencies 
    /// and verifies that the count increases correctly.
    /// </summary>
    [TestMethod]
    public void Size_TestAfterAddingMultipleDependencies_IncreasesCorrectly()
    {
        var graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.AddDependency("c", "d");
        graph.AddDependency("e", "f");

        Assert.AreEqual(3, graph.Size);
    }

    /// <summary>
    /// Tests the Size of the graph after removing a dependency 
    /// and verifies that the count decreases correctly.
    /// </summary>
    [TestMethod]
    public void Size_TestAfterRemovingDependency_DecreasesCorrectly()
    {
        var graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.AddDependency("c", "d");
        graph.RemoveDependency("a", "b");

        Assert.AreEqual(1, graph.Size);
    }

    /// <summary>
    /// Tests the Size of the graph after removing all dependencies 
    /// and ensures that the size becomes zero.
    /// </summary>
    [TestMethod]
    public void Size_TestAfterRemovingAllDependencies_IsZero()
    {
        var graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.AddDependency("c", "d");
        graph.RemoveDependency("a", "b");
        graph.RemoveDependency("c", "d");

        Assert.AreEqual(0, graph.Size);
    }

    /// <summary>
    /// Tests the Size of the graph after adding and removing the same 
    /// dependency multiple times, ensuring that the size remains zero.
    /// </summary>
    [TestMethod]
    public void Size_TestAfterAddingAndRemovingSameDependency_RemainsZero()
    {
        var graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.RemoveDependency("a", "b");
        graph.AddDependency("a", "b");
        graph.RemoveDependency("a", "b");

        Assert.AreEqual(0, graph.Size);
    }

    /// <summary>
    /// Tests the Size of the graph when adding multiple dependencies 
    /// with duplicates, ensuring that the size reflects only unique entries.
    /// </summary>
    [TestMethod]
    public void Size_TestAddMultipleDependenciesWithDuplicates_Valid()
    {
        var graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.AddDependency("a", "c");
        graph.AddDependency("a", "b"); // Duplicate
        graph.AddDependency("c", "b");
        graph.AddDependency("b", "d");
        graph.AddDependency("c", "b"); // Duplicate

        Assert.AreEqual(4, graph.Size);
    }

    // Test for HasDependents() method

    /// <summary>
    /// Tests that a node with no dependencies correctly returns false
    /// for HasDependents.
    /// </summary>
    [TestMethod]
    public void HasDependents_TestNoDependencies_ReturnsFalse()
    {
        var graph = new DependencyGraph();
        Assert.IsFalse(graph.HasDependents("a"));
    }

    /// <summary>
    /// Tests that a node with dependents correctly returns true
    /// for HasDependents.
    /// </summary>
    [TestMethod]
    public void HasDependents_TestNodeHasDependents_ReturnsTrue()
    {
        var graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        Assert.IsTrue(graph.HasDependents("a"));
    }

    /// <summary>
    /// Tests that a node which has no dependents, but is a
    /// dependent itself, returns false.
    /// </summary>
    [TestMethod]
    public void HasDependents_TestNodeHasNoDependents_ReturnsFalse()
    {
        var graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        Assert.IsFalse(graph.HasDependents("b"));
    }

    /// <summary>
    /// Tests that HasDependents returns false for any node in an
    /// empty graph.
    /// </summary>
    [TestMethod]
    public void HasDependents_TestEmptyGraph_ReturnsFalse()
    {
        var graph = new DependencyGraph();
        Assert.IsFalse(graph.HasDependents("z"));
    }

    /// <summary>
    /// Tests that a node with multiple dependents correctly returns true.
    /// </summary>
    [TestMethod]
    public void HasDependents_TestNodeWithMultipleDependents_ReturnsTrue()
    {
        var graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.AddDependency("a", "c");
        Assert.IsTrue(graph.HasDependents("a"));
    }

    /// <summary>
    /// Tests that after removing a dependency, HasDependents returns
    /// false for the node.
    /// </summary>
    [TestMethod]
    public void HasDependents_TestNodeRemovedDependency_ReturnsFalse()
    {
        var graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.RemoveDependency("a", "b");
        Assert.IsFalse(graph.HasDependents("a"));
    }

    /// <summary>
    /// Tests the process of adding and removing dependencies, verifying
    /// the state of dependents.
    /// </summary>
    [TestMethod()]
    public void HasDependents_TestAddAndRemoveDependency_ValidatesDependentsCorrectly()
    {
        DependencyGraph dg = new DependencyGraph();

        // Initially, "a" has no dependents
        Assert.IsFalse(dg.HasDependents("a"));

        // After adding a dependency from "a" to "b", "a" has dependents
        dg.AddDependency("a", "b");
        Assert.IsTrue(dg.HasDependents("a"));

        // "b" should not have dependents since it's a dependent itself
        Assert.IsFalse(dg.HasDependents("b"));
    }

    // Tests for HasDependees() method

    /// <summary>
    /// Tests that a node with no dependees correctly returns false.
    /// </summary>
    [TestMethod()]
    public void HasDependees_TestNoDependees_ReturnsFalse()
    {
        DependencyGraph dg = new DependencyGraph();

        // "a" has no dependees initially
        Assert.IsFalse(dg.HasDependees("a"));
    }

    /// <summary>
    /// Tests that after adding a dependency, the dependent node has the
    /// correct dependee.
    /// </summary>
    [TestMethod()]
    public void HasDependees_TestAddDependency_HasCorrectDependees()
    {
        DependencyGraph dg = new DependencyGraph();

        // Adding a dependency "a" -> "b", so "b" should have "a" as a dependee
        dg.AddDependency("a", "b");
        Assert.IsTrue(dg.HasDependees("b"));

        // "a" has no dependees because it's the dependee, not the dependent
        Assert.IsFalse(dg.HasDependees("a"));
    }

    /// <summary>
    /// Tests that a self-dependency is handled correctly, and the
    /// node returns true for both dependents and dependees.
    /// </summary>
    [TestMethod()]
    public void HasDependees_TestSelfDependency_ReturnsTrue()
    {
        DependencyGraph dg = new DependencyGraph();

        // Adding a self-dependency "a" -> "a"
        dg.AddDependency("a", "a");
        Assert.IsTrue(dg.HasDependees("a"));
    }

    /// <summary>
    /// Tests that removing a dependency correctly updates the dependees
    /// list.
    /// </summary>
    [TestMethod()]
    public void HasDependees_TestRemoveDependency_ReturnsFalse()
    {
        DependencyGraph dg = new DependencyGraph();

        // Adding a dependency "a" -> "b"
        dg.AddDependency("a", "b");
        Assert.IsTrue(dg.HasDependees("b"));

        // Removing the dependency should result in "b" no longer having dependees
        dg.RemoveDependency("a", "b");
        Assert.IsFalse(dg.HasDependees("b"));
    }

    /// <summary>
    /// Tests that HasDependees returns false for any node in an empty graph.
    /// </summary>
    [TestMethod()]
    public void HasDependees_TestEmptyGraph_ReturnsFalse()
    {
        DependencyGraph dg = new DependencyGraph();

        // No nodes or dependencies in the graph, should return false for any node
        Assert.IsFalse(dg.HasDependees("x"));
    }

    // Tests for GetDependents() method

    /// <summary>
    ///   Tests the behavior of GetDependents when a node has a single
    ///   dependent.
    /// </summary>
    [TestMethod]
    public void GetDependents_TestNodeWithSingleDependent_Valid()
    {
        DependencyGraph dg = new DependencyGraph();
        dg.AddDependency("a", "b");

        var dependents = dg.GetDependents("a");
        Assert.IsTrue(dependents.Contains("b"));
        Assert.AreEqual(1, dependents.Count());
    }

    /// <summary>
    ///   Tests the behavior of GetDependents when a node has multiple
    ///   dependents.
    /// </summary>
    [TestMethod]
    public void GetDependents_TestNodeWithMultipleDependents_Valid()
    {
        DependencyGraph dg = new DependencyGraph();
        dg.AddDependency("a", "b");
        dg.AddDependency("a", "c");
        dg.AddDependency("a", "d");

        var dependents = dg.GetDependents("a");
        Assert.IsTrue(dependents.Contains("b"));
        Assert.IsTrue(dependents.Contains("c"));
        Assert.IsTrue(dependents.Contains("d"));
        Assert.AreEqual(3, dependents.Count());
    }

    /// <summary>
    ///   Tests the behavior of GetDependents when a node has
    ///   no dependents.
    /// </summary>
    [TestMethod]
    public void GetDependents_TestNodeWithNoDependents_Empty()
    {
        DependencyGraph dg = new DependencyGraph();
        dg.AddDependency("a", "b");

        var dependents = dg.GetDependents("b");
        Assert.IsFalse(dependents.Any());
    }

    /// <summary>
    ///   Tests the behavior of GetDependents after a dependency
    ///   has been removed.
    /// </summary>
    [TestMethod]
    public void GetDependents_TestNodeThatWasRemoved_Zero()
    {
        DependencyGraph dg = new DependencyGraph();
        dg.AddDependency("a", "b");
        dg.RemoveDependency("a", "b");

        var dependents = dg.GetDependents("a");
        Assert.IsFalse(dependents.Contains("b"));
        Assert.AreEqual(0, dependents.Count());
    }

    /// <summary>
    ///   Tests the behavior of GetDependents on an empty graph.
    /// </summary>
    [TestMethod]
    public void GetDependents_TestEmptyGraph_Empty()
    {
        DependencyGraph dg = new DependencyGraph();

        var dependents = dg.GetDependents("a");
        Assert.IsFalse(dependents.Any());
    }

    /// <summary>
    ///   Tests the behavior of GetDependents for a node that does not
    ///   exist in the graph.
    /// </summary>
    [TestMethod]
    public void GetDependents_TestNodeNotInGraph_Empty()
    {
        DependencyGraph dg = new DependencyGraph();
        dg.AddDependency("a", "b");

        var dependents = dg.GetDependents("c");
        Assert.IsFalse(dependents.Any());
    }

    /// <summary>
    ///   Tests the behavior of GetDependents when duplicate dependencies
    ///   are added.
    /// </summary>
    [TestMethod]
    public void GetDependents_TestDuplicateDependencies_Valid()
    {
        DependencyGraph dg = new DependencyGraph();
        dg.AddDependency("a", "b");
        dg.AddDependency("a", "b"); // Adding duplicate dependency

        var dependents = dg.GetDependents("a");
        Assert.IsTrue(dependents.Contains("b"));
        Assert.AreEqual(1, dependents.Count()); // Should still be 1, even with duplicates
    }

    /// <summary>
    ///   Tests the behavior of GetDependents when circular dependencies
    ///   are present.
    /// </summary>
    [TestMethod]
    public void GetDependents_TestCircularDependency_Valid()
    {
        DependencyGraph dg = new DependencyGraph();
        dg.AddDependency("a", "b");
        dg.AddDependency("b", "c");
        dg.AddDependency("c", "a"); // Circular dependency

        var dependentsA = dg.GetDependents("a");
        var dependentsB = dg.GetDependents("b");
        var dependentsC = dg.GetDependents("c");

        Assert.IsTrue(dependentsA.Contains("b"));
        Assert.IsTrue(dependentsB.Contains("c"));
        Assert.IsTrue(dependentsC.Contains("a"));
    }

    /// <summary>
    ///   Tests the behavior of GetDependents when a node depends on
    ///   itself.
    /// </summary>
    [TestMethod]
    public void GetDependents_TestSelfDependency_False()
    {
        DependencyGraph dg = new DependencyGraph();
        dg.AddDependency("a", "a"); // Self-dependency

        var dependentsA = dg.GetDependents("a");
        Assert.IsTrue(dependentsA.Contains("a")); // Self-dependency should not be listed
    }

    // Tests for GetDependees() method

    [TestMethod]
    public void GetDependees_TestEmptyGraph_NoDependees()
    {
        DependencyGraph dg = new DependencyGraph();

        // Node with no dependencies
        string node = "a";
        Assert.IsFalse(dg.GetDependees(node).Any());
    }

    [TestMethod]
    public void GetDependees_TestSingleDependency_ReturnsDependees()
    {
        DependencyGraph dg = new DependencyGraph();
        dg.AddDependency("a", "b");

        // Check dependees for "B"
        Assert.AreEqual(1, dg.GetDependees("b").Count());
        Assert.IsTrue(dg.GetDependees("b").Contains("a"));
    }

    [TestMethod]
    public void GetDependees_MultipleDependencies_ReturnsAllDependees()
    {
        DependencyGraph dg = new DependencyGraph();
        dg.AddDependency("a", "b");
        dg.AddDependency("c", "b");
        dg.AddDependency("d", "b");

        // Check dependees for "B"
        var dependees = dg.GetDependees("b").ToList();
        Assert.AreEqual(3, dependees.Count);
        Assert.IsTrue(dependees.Contains("a"));
        Assert.IsTrue(dependees.Contains("c"));
        Assert.IsTrue(dependees.Contains("d"));
    }

    [TestMethod]
    public void GetDependees_TestNonExistentNode_ReturnsEmpty()
    {
        DependencyGraph dg = new DependencyGraph();
        Assert.IsFalse(dg.GetDependees("x").Any());
    }

    [TestMethod]
    public void GetDependees_TestSelfDependency_ReturnsSelf()
    {
        DependencyGraph dg = new DependencyGraph();
        dg.AddDependency("a", "a");

        // Check dependees for "A"
        Assert.AreEqual(1, dg.GetDependees("a").Count());
        Assert.IsTrue(dg.GetDependees("a").Contains("a"));
    }

    [TestMethod]
    public void GetDependees_TestCircularDependency_ReturnsCorrectDependees()
    {
        DependencyGraph dg = new DependencyGraph();
        dg.AddDependency("a", "b");
        dg.AddDependency("b", "a");

        // Check dependees for "a"
        var dependeesA = dg.GetDependees("a").ToList();
        Assert.AreEqual(1, dependeesA.Count);
        Assert.IsTrue(dependeesA.Contains("b"));

        // Check dependees for "b"
        var dependeesB = dg.GetDependees("b").ToList();
        Assert.AreEqual(1, dependeesB.Count);
        Assert.IsTrue(dependeesB.Contains("a"));
    }

    // Tests for ReplaceDependents() method

    [TestMethod()]
    public void ReplaceDependents_TestWithNewDependents_ReplacesCorrectly()
    {
        DependencyGraph dg = new DependencyGraph();

        // Adding initial dependencies for "a"
        dg.AddDependency("a", "b");
        dg.AddDependency("a", "c");

        // Replace dependents of "a" with new dependents
        dg.ReplaceDependents("a", new List<string> { "d", "e" });

        // "a" should now only depend on "d" and "e"
        var dependents = dg.GetDependents("a").ToList();
        CollectionAssert.AreEquivalent(new List<string> { "d", "e" }, dependents);
    }

    [TestMethod()]
    public void ReplaceDependents_TestAddingSameDependents_NoChangeInSize()
    {
        DependencyGraph dg = new DependencyGraph();

        // Adding initial dependencies for "a"
        dg.AddDependency("a", "b");
        dg.AddDependency("a", "c");

        int initialSize = dg.Size;

        // Replacing dependents with the same dependents
        dg.ReplaceDependents("a", new List<string> { "b", "c" });

        Assert.AreEqual(initialSize, dg.Size);
        var dependents = dg.GetDependents("a").ToList();
        CollectionAssert.AreEquivalent(new List<string> { "b", "c" }, dependents);
    }

    [TestMethod()]
    public void ReplaceDependents_TestRemoveAllDependents_EmptyDependents()
    {
        DependencyGraph dg = new DependencyGraph();

        // Adding initial dependencies for "a"
        dg.AddDependency("a", "b");
        dg.AddDependency("a", "c");

        // Replacing with an empty list should remove all dependents of "a"
        dg.ReplaceDependents("a", new List<string>());

        Assert.IsFalse(dg.HasDependents("a"));
        Assert.AreEqual(0, dg.Size);
    }

    [TestMethod()]
    public void ReplaceDependents_TestAddingSelfDependency_CorrectlyAdded()
    {
        DependencyGraph dg = new DependencyGraph();

        // Adding a normal dependency first
        dg.AddDependency("a", "b");

        // Replacing dependents with a self-dependency
        dg.ReplaceDependents("a", new List<string> { "a" });

        var dependents = dg.GetDependents("a").ToList();
        CollectionAssert.AreEquivalent(new List<string> { "a" }, dependents);
        Assert.IsTrue(dg.HasDependents("a"));
        Assert.IsTrue(dg.HasDependees("a"));
    }

    [TestMethod()]
    public void ReplaceDependents_TestNodeWithNoInitialDependents_AddsNewDependents()
    {
        DependencyGraph dg = new DependencyGraph();

        // No initial dependents for "a"
        Assert.IsFalse(dg.HasDependents("a"));

        // Replacing dependents with new dependents
        dg.ReplaceDependents("a", new List<string> { "b", "c" });

        var dependents = dg.GetDependents("a").ToList();
        CollectionAssert.AreEquivalent(new List<string> { "b", "c" }, dependents);
        Assert.AreEqual(2, dg.Size);
    }

    // Tests for ReplaceDependees() method

    /// <summary>
    /// Tests that replacing dependees for a node correctly updates the graph.
    /// Existing dependees should be replaced with the new set.
    /// </summary>
    [TestMethod()]
    public void ReplaceDependees_TestWithNewDependees_ReplacesCorrectly()
    {
        DependencyGraph dg = new DependencyGraph();

        // Adding initial dependees for "a"
        dg.AddDependency("b", "a");
        dg.AddDependency("c", "a");

        // Replace dependees of "a" with new dependees
        dg.ReplaceDependees("a", new List<string> { "d", "e" });

        // "a" should now only have "d" and "e" as dependees
        var dependees = dg.GetDependees("a").ToList();
        CollectionAssert.AreEquivalent(new List<string> { "d", "e" }, dependees);
    }

    /// <summary>
    /// Tests that replacing dependees with the same set of dependees does not affect the graph's size.
    /// </summary>
    [TestMethod()]
    public void ReplaceDependees_TestAddingSameDependees_NoChangeInSize()
    {
        DependencyGraph dg = new DependencyGraph();

        // Adding initial dependees for "a"
        dg.AddDependency("b", "a");
        dg.AddDependency("c", "a");

        int initialSize = dg.Size;

        // Replacing dependees with the same dependees
        dg.ReplaceDependees("a", new List<string> { "b", "c" });

        Assert.AreEqual(initialSize, dg.Size);
        var dependees = dg.GetDependees("a").ToList();
        CollectionAssert.AreEquivalent(new List<string> { "b", "c" }, dependees);
    }

    /// <summary>
    /// Tests that replacing dependees with an empty list removes all dependees for a node.
    /// </summary>
    [TestMethod()]
    public void ReplaceDependees_TestRemoveAllDependees_EmptyDependees()
    {
        DependencyGraph dg = new DependencyGraph();

        // Adding initial dependees for "a"
        dg.AddDependency("b", "a");
        dg.AddDependency("c", "a");

        // Replacing with an empty list should remove all dependees of "a"
        dg.ReplaceDependees("a", new List<string>());

        Assert.IsFalse(dg.HasDependees("a"));
        Assert.AreEqual(0, dg.Size);
    }

    /// <summary>
    /// Tests that adding a self-dependency for a node through ReplaceDependees is handled correctly.
    /// </summary>
    [TestMethod()]
    public void ReplaceDependees_TestAddingSelfDependency_CorrectlyAdded()
    {
        DependencyGraph dg = new DependencyGraph();

        // Adding a normal dependency first
        dg.AddDependency("b", "a");

        // Replacing dependees with a self-dependency
        dg.ReplaceDependees("a", new List<string> { "a" });

        var dependees = dg.GetDependees("a").ToList();
        CollectionAssert.AreEquivalent(new List<string> { "a" }, dependees);
        Assert.IsTrue(dg.HasDependees("a"));
        Assert.IsTrue(dg.HasDependents("a"));
    }

    /// <summary>
    /// Tests that a node with no initial dependees correctly adds new dependees.
    /// </summary>
    [TestMethod()]
    public void ReplaceDependees_TestNodeWithNoInitialDependees_AddsNewDependees()
    {
        DependencyGraph dg = new DependencyGraph();

        // No initial dependees for "a"
        Assert.IsFalse(dg.HasDependees("a"));

        // Replacing dependees with new dependees
        dg.ReplaceDependees("a", new List<string> { "b", "c" });

        var dependees = dg.GetDependees("a").ToList();
        CollectionAssert.AreEquivalent(new List<string> { "b", "c" }, dependees);
        Assert.AreEqual(2, dg.Size);
    }

    /// <summary>
    /// Tests a complex series of operations where both ReplaceDependents and ReplaceDependees are used.
    /// Validates that the graph's final state is consistent with the operations.
    /// </summary>
    [TestMethod()]
    public void ReplaceDependentsAndReplaceDependeesTestComplexReplace_Valid()
    {
        DependencyGraph t = new DependencyGraph();
        t.AddDependency("x", "b");
        t.AddDependency("a", "z");
        t.ReplaceDependents("b", new HashSet<string>());
        t.AddDependency("y", "b");
        t.ReplaceDependents("a", new HashSet<string>() { "c" });
        t.AddDependency("w", "d");
        t.ReplaceDependees("b", new HashSet<string>() { "a", "c" });
        t.ReplaceDependees("d", new HashSet<string>() { "b" });
        Assert.AreEqual(4, t.Size);
    }

    // Stress Tests

    /// <summary>
    ///   Tests the behavior of the DependencyGraph with a series of operations including adding, removing,
    ///   and replacing dependencies. This test covers a high volume of dependencies and ensures that
    ///   the DependencyGraph handles various operations correctly.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]  // 2 second run time limit
    public void StressTest()
    {
        DependencyGraph dg = new();

        // Create a set of strings representing nodes
        const int SIZE = 200;
        string[] letters = new string[SIZE];
        for (int i = 0; i < SIZE; i++)
        {
            letters[i] = string.Empty + ((char)('a' + i));
        }

        // Initialize the expected results for dependents and dependees
        HashSet<string>[] dependents = new HashSet<string>[SIZE];
        HashSet<string>[] dependees = new HashSet<string>[SIZE];
        for (int i = 0; i < SIZE; i++)
        {
            dependents[i] = [];
            dependees[i] = [];
        }

        // Add a series of dependencies
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = i + 1; j < SIZE; j++)
            {
                dg.AddDependency(letters[i], letters[j]);
                dependents[i].Add(letters[j]);
                dependees[j].Add(letters[i]);
            }
        }

        // Remove some of the dependencies
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = i + 4; j < SIZE; j += 4)
            {
                dg.RemoveDependency(letters[i], letters[j]);
                dependents[i].Remove(letters[j]);
                dependees[j].Remove(letters[i]);
            }
        }

        // Re-add some dependencies
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = i + 1; j < SIZE; j += 2)
            {
                dg.AddDependency(letters[i], letters[j]);
                dependents[i].Add(letters[j]);
                dependees[j].Add(letters[i]);
            }
        }

        // Remove some more dependecies
        for (int i = 0; i < SIZE; i += 2)
        {
            for (int j = i + 3; j < SIZE; j += 3)
            {
                dg.RemoveDependency(letters[i], letters[j]);
                dependents[i].Remove(letters[j]);
                dependees[j].Remove(letters[i]);
            }
        }

        // Verify that the DependencyGraph's state matches the expected results
        for (int i = 0; i < SIZE; i++)
        {
            Assert.IsTrue(dependents[i].SetEquals(new HashSet<string>(dg.GetDependents(letters[i]))));
            Assert.IsTrue(dependees[i].SetEquals(new HashSet<string>(dg.GetDependees(letters[i]))));
        }
    }

    /// <summary>
    /// Stress test for creating a large number of nodes without any dependencies.
    /// Verifies that all nodes have no dependents or dependees.
    /// </summary>
    [TestMethod]
    [Timeout(2000)] // 2 second run time limit
    public void StressTest_TestNodesWithNoDependencies_Works()
    {
        DependencyGraph dg = new DependencyGraph();

        // Create nodes without adding any dependencies
        const int NUM_NODES = 300;
        string[] nodes = new string[NUM_NODES];
        for (int i = 0; i < NUM_NODES; i++)
        {
            nodes[i] = $"node{i}";
        }

        // Verify that all nodes have no dependents and dependees
        foreach (var node in nodes)
        {
            Assert.IsFalse(dg.HasDependents(node));
            Assert.IsFalse(dg.HasDependees(node));
            Assert.IsFalse(dg.GetDependents(node).Any());
            Assert.IsFalse(dg.GetDependees(node).Any());
        }
    }

    /// <summary>
    /// Test GetDependees method with multiple dependencies.
    /// Validates that the correct dependees are returned for each node.
    /// </summary>
    public void GetDependees_TestMultipleDependencies_ReturnsCorrectDependees()
    {
        DependencyGraph t = new DependencyGraph();
        t.AddDependency("a", "b");
        t.AddDependency("a", "c");
        t.AddDependency("c", "b");
        t.AddDependency("b", "d");

        // Test dependees for node "a" (should have none)
        IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
        Assert.IsFalse(e.MoveNext());

        // Test dependees for node "b"
        e = t.GetDependees("b").GetEnumerator();
        Assert.IsTrue(e.MoveNext());
        String s1 = e.Current;
        Assert.IsTrue(e.MoveNext());
        String s2 = e.Current;
        Assert.IsFalse(e.MoveNext());
        Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));

        // Test dependees for node "c"
        e = t.GetDependees("c").GetEnumerator();
        Assert.IsTrue(e.MoveNext());
        Assert.AreEqual("a", e.Current);
        Assert.IsFalse(e.MoveNext());

        // Test dependees for node "d"
        e = t.GetDependees("d").GetEnumerator();
        Assert.IsTrue(e.MoveNext());
        Assert.AreEqual("b", e.Current);
        Assert.IsFalse(e.MoveNext());
    }

    /// <summary>
    /// Test GetDependents method with multiple dependencies.
    /// Ensures that the correct dependents are returned for each node.
    /// </summary>
    [TestMethod()]
    public void GetDependents_TestMultipleDependencies_ReturnsCorrectDependents()
    {
        DependencyGraph t = new DependencyGraph();
        t.AddDependency("a", "b");
        t.AddDependency("a", "c");
        t.AddDependency("c", "b");
        t.AddDependency("b", "d");

        // Test dependents for node "a"
        IEnumerator<string> e = t.GetDependents("a").GetEnumerator();
        Assert.IsTrue(e.MoveNext());
        String s1 = e.Current;
        Assert.IsTrue(e.MoveNext());
        String s2 = e.Current;
        Assert.IsFalse(e.MoveNext());
        Assert.IsTrue(((s1 == "b") && (s2 == "c")) || ((s1 == "c") && (s2 == "b")));

        // Test dependents for node "b"
        e = t.GetDependents("b").GetEnumerator();
        Assert.IsTrue(e.MoveNext());
        Assert.AreEqual("d", e.Current);
        Assert.IsFalse(e.MoveNext());

        // Test dependents for node "c"
        e = t.GetDependents("c").GetEnumerator();
        Assert.IsTrue(e.MoveNext());
        Assert.AreEqual("b", e.Current);
        Assert.IsFalse(e.MoveNext());

        // Test dependents for node "d"
        e = t.GetDependents("d").GetEnumerator();
        Assert.IsFalse(e.MoveNext());
    }

    /// <summary>
    /// Stress test for adding a large number of dependencies and performing removals/replacements.
    /// Validates that the resulting dependents and dependees match the expected sets.
    /// </summary>
    [TestMethod()]
    public void StressTest_TestLargeNumberOfDependencies_RemovalsAndReplacements()
    {
        // Create a new Dependency graph
        DependencyGraph t = new DependencyGraph();

        // Define a large number of nodes (letters) to use in the test
        const int SIZE = 400;
        string[] letters = new string[SIZE];
        for (int i = 0; i < SIZE; i++)
        {
            letters[i] = ("" + (char)('a' + i));
        }

        // Initialize HashSets to track expected dependents and dependees
        HashSet<string>[] dents = new HashSet<string>[SIZE];
        HashSet<string>[] dees = new HashSet<string>[SIZE];
        for (int i = 0; i < SIZE; i++)
        {
            dents[i] = new HashSet<string>();
            dees[i] = new HashSet<string>();
        }

        // Add dependencies between nodes
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = i + 1; j < SIZE; j++)
            {
                t.AddDependency(letters[i], letters[j]);
                dents[i].Add(letters[j]);
                dees[j].Add(letters[i]);
            }
        }

        // Remove some dependencies to test removal functionality
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = i + 2; j < SIZE; j += 3)
            {
                t.RemoveDependency(letters[i], letters[j]);
                dents[i].Remove(letters[j]);
                dees[j].Remove(letters[i]);
            }
        }

        // Replace dependents for some nodes and update expected results
        for (int i = 0; i < SIZE; i += 2)
        {
            HashSet<string> newDents = new HashSet<String>();
            for (int j = 0; j < SIZE; j += 5)
            {
                newDents.Add(letters[j]);
            }
            t.ReplaceDependents(letters[i], newDents);

            // Update expected dependees for the nodes
            foreach (string s in dents[i])
            {
                dees[s[0] - 'a'].Remove(letters[i]);
            }

            foreach (string s in newDents)
            {
                dees[s[0] - 'a'].Add(letters[i]);
            }

            dents[i] = newDents;
        }

        // Validate that the actual dependents and dependees match the expected results
        for (int i = 0; i < SIZE; i++)
        {
            Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
            Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
        }
    }

    /// <summary>
    /// Stress test for high volume of dependencies.
    /// Verifies correct functionality with 1000 nodes.
    /// </summary>
    [TestMethod]
    [Timeout(3000)] // 3 second run time limit
    public void StressTest_TestHighVolumeOfDependencies()
    {
        DependencyGraph dg = new DependencyGraph();

        // Generate a large number of nodes
        const int NUM_NODES = 1000;
        string[] nodes = new string[NUM_NODES];
        for (int i = 0; i < NUM_NODES; i++)
        {
            nodes[i] = $"node{i}";
        }

        // Add dependencies between each node and every other node
        for (int i = 0; i < NUM_NODES; i++)
        {
            for (int j = i + 1; j < NUM_NODES; j++)
            {
                dg.AddDependency(nodes[i], nodes[j]);
            }
        }

        // Verify dependents and dependees for a random subset of nodes
        for (int i = 0; i < 10; i++)
        {
            string node = nodes[i];
            Assert.AreEqual(NUM_NODES - i - 1, dg.GetDependents(node).Count());
            Assert.AreEqual(i, dg.GetDependees(node).Count());
        }
    }

    /// <summary>
    /// Stress test for GetDependents method with a large number of dependencies.
    /// Ensures the graph can handle and return dependents correctly.
    /// </summary>
    [TestMethod]
    [Timeout(3000)] // 3 second run time limit
    public void TestGetDependents_StressTest_Valid()
    {
        DependencyGraph dg = new DependencyGraph();
        const int SIZE = 1000;
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = i + 1; j < SIZE; j++)
            {
                dg.AddDependency($"Node{i}", $"Node{j}");
            }
        }

        // Check dependents for a specific node
        var dependents = dg.GetDependents("Node0");
        Assert.AreEqual(SIZE - 1, dependents.Count());
    }

    /// <summary>
    /// Test GetDependees with a large number of nodes.
    /// Verifies that the correct dependees are returned for each node.
    /// </summary>
    [TestMethod]
    [Timeout(2000)] // Set timeout for stress test
    public void GetDependees_TestLargeNumberOfNodes_Valid()
    {
        DependencyGraph dg = new DependencyGraph();
        const int NUM_NODES = 500;
        string[] nodes = new string[NUM_NODES];

        for (int i = 0; i < NUM_NODES; i++)
        {
            nodes[i] = $"node{i}";
            if (i > 0)
            {
                dg.AddDependency(nodes[i - 1], nodes[i]);
            }
        }

        // Verify dependees for the last node
        var dependeesLast = dg.GetDependees(nodes[NUM_NODES - 1]).ToList();
        Assert.AreEqual(1, dependeesLast.Count);
        Assert.AreEqual(nodes[NUM_NODES - 2], dependeesLast[0]);
    }
}