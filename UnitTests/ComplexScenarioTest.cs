using NUnit.Framework;
using Schipvervoer.Logic;
using Schipvervoer.Models;
using System.Collections.Generic;

namespace Schipvervoer.Tests
{
    [TestFixture]
    public class ComplexScenarioTest
    {
        private Ship ship;
        private ContainerAllocationAlgorithm algorithm;

        [SetUp]
        public void Setup()
        {
            // Pas deze waarden aan op basis van uw specifieke testvereisten
            ship = new Ship(100000, 10, 10); // Voorbeeld: max gewicht, lengte, breedte
            algorithm = new ContainerAllocationAlgorithm();
        }

        [Test]
        public void TestComplexContainerAllocation()
        {
            var containers = new List<Container>
{
    new Container(30000, false, false), // Zware container
    new Container(4000, true, false),   // Waardevolle, lichte container
    new Container(15000, false, true),  // Gekoelde container
    new Container(28000, false, false), // Bijna maximale gewicht container
    new Container(5000, false, false),  // Gemiddeld gewicht container
    new Container(4000, false, true),   // Lichtgewicht, gekoelde container
    new Container(25000, true, false),  // Zware, waardevolle container
    new Container(18000, false, false), // Gemiddeld gewicht container
    new Container(12000, false, true),  // Gemiddeld gewicht, gekoelde container
    new Container(8000, true, false),   // Lichtgewicht, waardevolle container
    // Voeg eventueel meer containers toe voor extra dekking
};


            algorithm.AllocateContainers(ship, containers);

            // Controleer of het maximum gewicht bovenop een container niet wordt overschreden
            foreach (var stack in ship.ShipLayout)
            {
                int totalWeight = stack.TotalWeight();
                Assert.LessOrEqual(totalWeight, ContainerStack.MaxWeightOnTop, "Stapel overschrijdt maximale gewicht");
            }

            // Controleer of waardevolle containers correct zijn geplaatst
            Assert.IsTrue(ship.AreValuableContainersAccessible(), "Toegankelijkheid waardevolle containers niet voldaan");

            // Controleer of gekoelde containers in de eerste rij zijn geplaatst
            Assert.IsTrue(ship.AreCoolingRequirementsMet(), "Koelingsvereisten niet voldaan");

            // Controleer of het schip ten minste 50% van het maximum gewicht draagt
            Assert.IsTrue(ship.IsMinWeightMaintained(), "Minimaal gewicht niet gehandhaafd");

            // Controleer of het gewicht gelijkmatig is verdeeld
            int leftWeight = ship.CalculateSideWeight(0, ship.Width / 2);
            int rightWeight = ship.CalculateSideWeight(ship.Width / 2, ship.Width);
            int tolerance = (int)(ship.MaxWeight * 0.2);
            Assert.LessOrEqual(Math.Abs(leftWeight - rightWeight), tolerance, "Gewichtsverdeling is niet binnen de tolerantie");
        }
    }
}
