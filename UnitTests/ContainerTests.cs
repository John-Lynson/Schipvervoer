using NUnit.Framework;
using Schipvervoer.Logic;
using Schipvervoer.Models;
using System.Collections.Generic;
using System.Linq;

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
            ship = new Ship(100000, 10, 10); // Gebruik de juiste waarden voor length en width
            algorithm = new ContainerAllocationAlgorithm();
        }

        [Test]
        public void TestContainerAllocation()
        {
            var containers = new List<Container>
            {
                new Container(20000, false, false), // Normale container
                new Container(25000, true, false),  // Waardevolle container
                new Container(15000, false, true),  // Gekoelde container
                new Container(18000, false, false), // Normale container
                // Voeg meer containers toe indien nodig
            };

            algorithm.AllocateContainers(ship, containers);

            Assert.IsTrue(ship.IsMinWeightMaintained(), "Minimaal gewicht niet gehandhaafd");
            Assert.IsTrue(ship.IsWeightDistributedProperly(), "Gewichtsverdeling niet correct");
            Assert.IsTrue(ship.AreCoolingRequirementsMet(), "Koelingsvereisten niet voldaan");
            Assert.IsTrue(ship.AreValuableContainersAccessible(), "Toegankelijkheid waardevolle containers niet voldaan");

            int totalContainersPlaced = CountTotalContainersPlaced(ship);
            Assert.AreEqual(containers.Count, totalContainersPlaced, "Niet alle containers zijn geplaatst");
        }

        private int CountTotalContainersPlaced(Ship ship)
        {
            int count = 0;
            foreach (var stack in ship.ShipLayout)
            {
                count += stack.Containers.Count;
            }
            return count;
        }
    }
}
