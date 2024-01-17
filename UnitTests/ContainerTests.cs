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
            ship = new Ship(100000, 10, 10); // Gebruik de juiste waarden voor length en width
            algorithm = new ContainerAllocationAlgorithm();
        }

        [Test]
        public void TestContainerAllocation()
        {
            // Aanmaken van een reeks containers met verschillende eigenschappen
            var containers = new List<Container>
    {
        new Container(20000, false, false), // Normale container
        new Container(25000, true, false),  // Waardevolle container
        new Container(15000, false, true),  // Gekoelde container
        new Container(18000, false, false), // Normale container
        // Voeg meer containers toe indien nodig
    };

            algorithm.AllocateContainers(ship, containers);

            // Asserts voor elke specifieke vereiste
            Assert.IsTrue(ship.IsMinWeightMaintained(), "Minimaal gewicht niet gehandhaafd");
            Assert.IsTrue(ship.IsWeightDistributedProperly(), "Gewichtsverdeling niet correct");
            Assert.IsTrue(ship.AreCoolingRequirementsMet(), "Koelingsvereisten niet voldaan");
            Assert.IsTrue(ship.AreValuableContainersAccessible(), "Toegankelijkheid waardevolle containers niet voldaan");

            // Extra asserts om te controleren of alle containers zijn geplaatst
            int totalContainersPlaced = ship.ContainerStacks.Sum(stack => stack.Containers.Count);
            Assert.AreEqual(containers.Count, totalContainersPlaced, "Niet alle containers zijn geplaatst");

            // Extra checks voor de plaatsing van gekoelde en waardevolle containers
            Assert.IsTrue(CheckCoolingContainersPlacement(ship), "Gekoelde containers niet correct geplaatst");
            Assert.IsTrue(CheckValuableContainersPlacement(ship), "Waardevolle containers niet correct geplaatst");
        }

        private bool CheckCoolingContainersPlacement(Ship ship)
        {
            // Controleer of alle gekoelde containers in de eerste rij (eerste 'Width' stacks) staan
            for (int i = 0; i < ship.Width; i++)
            {
                if (ship.ContainerStacks[i].Containers.Any(c => c.RequiresCooling))
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckValuableContainersPlacement(Ship ship)
        {
            // Controleer of er geen containers bovenop waardevolle containers staan
            foreach (var stack in ship.ContainerStacks)
            {
                bool foundValuable = false;
                foreach (var container in stack.Containers)
                {
                    if (container.IsValuable)
                    {
                        if (foundValuable) // Een tweede waardevolle container gevonden in dezelfde stack
                        {
                            return false;
                        }
                        foundValuable = true;
                    }
                    else if (foundValuable) // Een niet-waardevolle container bovenop een waardevolle container
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
