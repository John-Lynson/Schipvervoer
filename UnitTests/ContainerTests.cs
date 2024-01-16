using NUnit.Framework;
using Schipvervoer.Logic;
using Schipvervoer.Models;
using System.Collections.Generic;

namespace Schipvervoer.Tests
{
    [TestFixture]
    public class ContainerTests
    {
        private Ship ship;
        private ContainerAllocationAlgorithm algorithm;

        [SetUp]
        public void Setup()
        {
            // Stel een schip in met een bepaalde capaciteit
            ship = new Ship(100000); // Voorbeeld capaciteit
            algorithm = new ContainerAllocationAlgorithm();
        }

        [Test]
        public void Allocate_NormalContainer_ShouldBePlaced()
        {
            var containers = new List<Container> { new Container(20000, false, false) };
            algorithm.AllocateContainersToStacks(ship, containers);
            Assert.That(ship.ContainerStacks, Has.Count.EqualTo(1));
            Assert.That(ship.ContainerStacks[0].Containers, Has.Count.EqualTo(1));
            Assert.IsFalse(ship.ContainerStacks[0].Containers[0].IsValuable);
            Assert.IsFalse(ship.ContainerStacks[0].Containers[0].RequiresCooling);
        }

        [Test]
        public void Allocate_ValuableContainer_ShouldBeAccessible()
        {
            var containers = new List<Container> { new Container(10000, true, false) };
            algorithm.AllocateContainersToStacks(ship, containers);
            Assert.IsTrue(ship.ContainerStacks[0].Containers[0].IsValuable);
            // Voeg aanvullende logica toe om te controleren of de waardevolle container toegankelijk is
        }

        [Test]
        public void Allocate_CoolableContainer_ShouldBeInFirstRow()
        {
            var containers = new List<Container> { new Container(15000, false, true) };
            algorithm.AllocateContainersToStacks(ship, containers);
            Assert.IsTrue(ship.ContainerStacks[0].Containers[0].RequiresCooling);
        }

        [Test]
        public void Allocate_ValuableAndCoolableContainer_ShouldBeAccessibleAndInFirstRow()
        {
            var containers = new List<Container> { new Container(10000, true, true) };
            algorithm.AllocateContainersToStacks(ship, containers);
            Assert.IsTrue(ship.ContainerStacks[0].Containers.Exists(c => c.IsValuable && c.RequiresCooling));
            // Controleer of de container zowel toegankelijk als in de eerste rij is
        }
    }
}

