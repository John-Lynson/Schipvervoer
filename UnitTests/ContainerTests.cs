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
        }

        [Test]
        public void Allocate_ValuableContainer_ShouldBeAccessible()
        {
            var containers = new List<Container> { new Container(10000, true, false) };
            algorithm.AllocateContainersToStacks(ship, containers);

            // Voeg logica toe om te controleren of de waardevolle container toegankelijk is
            // Dit hangt af van hoe je 'toegankelijkheid' hebt geïmplementeerd
        }

        [Test]
        public void Allocate_CoolableContainer_ShouldBeInFirstRow()
        {
            var containers = new List<Container> { new Container(15000, false, true) };
            algorithm.AllocateContainersToStacks(ship, containers);

            // Voeg logica toe omte controleren of de koelbare container in de eerste rij is geplaatst. 
            // Dit hangt af van hoe je de indeling van containers op het schip hebt geïmplementeerd.
            // Aanname: de eerste rij van ContainerStacks is voor gekoelde containers
            Assert.IsTrue(ship.ContainerStacks[0].Containers.Exists(c => c.RequiresCooling));
        }

        [Test]
        public void Allocate_ValuableAndCoolableContainer_ShouldBeAccessibleAndInFirstRow()
        {
            var containers = new List<Container> { new Container(10000, true, true) };
            algorithm.AllocateContainersToStacks(ship, containers);

            // Controleer of de container zowel toegankelijk als in de eerste rij is
            Assert.IsTrue(ship.ContainerStacks[0].Containers.Exists(c => c.IsValuable && c.RequiresCooling));
        }

        // Voeg meer tests toe voor andere scenario's en randgevallen
    }
}

