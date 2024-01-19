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
            ship = new Ship(100000, 4, 8); 
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
            };

            algorithm.AllocateContainers(ship, containers);

            Assert.IsTrue(ship.IsMinWeightMaintained(), "Minimaal gewicht niet gehandhaafd");
            Assert.IsTrue(ship.IsWeightDistributedProperly(), "Gewichtsverdeling niet correct");
            Assert.IsTrue(ship.AreCoolingRequirementsMet(), "Koelingsvereisten niet voldaan");
            Assert.IsTrue(ship.AreValuableContainersAccessible(), "Toegankelijkheid waardevolle containers niet voldaan");

            int totalContainersPlaced = CountTotalContainersPlaced(ship);
            Assert.AreEqual(containers.Count, totalContainersPlaced, "Niet alle containers zijn geplaatst");
        }

        [Test]
        public void TestMaxStackHeight()
        {
            var containers = new List<Container>
    {
        new Container(30000, false, false), // Zware container
        new Container(4000, false, false),  // Lichtere container
        new Container(10000, true, false),  // Waardevolle container
        new Container(15000, false, true),  // Gekoelde container
    };

            algorithm.AllocateContainers(ship, containers);

            foreach (var stack in ship.ShipLayout)
            {
                int totalWeight = stack.TotalWeight();
                Assert.LessOrEqual(totalWeight, ContainerStack.MaxWeightOnTop, "Stapel overschrijdt maximale gewicht");
            }
        }


        [Test]
        public void TestWeightDistribution()
        {
            var containers = new List<Container>
    {
        new Container(10000, false, false),
        new Container(15000, false, false),
        new Container(20000, false, false),
        new Container(25000, true, false),  // Waardevolle container
        new Container(15000, false, true),  // Gekoelde container
    };

            algorithm.AllocateContainers(ship, containers);

            int leftWeight = ship.CalculateSideWeight(0, ship.Width / 2);
            int rightWeight = ship.CalculateSideWeight(ship.Width / 2, ship.Width);
            int tolerance = (int)(ship.MaxWeight * 0.2);

            Assert.LessOrEqual(Math.Abs(leftWeight - rightWeight), tolerance, "Gewichtsverdeling is niet binnen de tolerantie");
        }


        [Test]
        public void TestSpecialContainerRequirements()
        {
            var containers = new List<Container>
    {
        new Container(20000, true, false), // Waardevolle container
        new Container(15000, false, true), // Gekoelde container
        new Container(10000, false, false),
        new Container(25000, false, false),
    };

            algorithm.AllocateContainers(ship, containers);

            Assert.IsTrue(ship.AreCoolingRequirementsMet(), "Koelingsvereisten niet voldaan");
            Assert.IsTrue(ship.AreValuableContainersAccessible(), "Toegankelijkheid waardevolle containers niet voldaan");
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
