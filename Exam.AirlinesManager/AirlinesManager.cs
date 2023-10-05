using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.DeliveriesManager
{
    public class AirlinesManager : IAirlinesManager
    {
        private Dictionary<string, Airline> airlines = new Dictionary<string, Airline>();
        private Dictionary<string, Flight> flights = new Dictionary<string, Flight>();
        private Dictionary<Airline, int> airlineWithCountOfFlights = new Dictionary<Airline, int>();
        private Dictionary<Airline,Flight> AirlinesWithFLightsAttached = new Dictionary<Airline, Flight>();

        public void AddAirline(Airline airline)
        {
            airlines.Add(airline.Id, airline);
            airlineWithCountOfFlights.Add(airline, 0);
        }

        public void AddFlight(Airline airline, Flight flight)
        {
            if (!airlineWithCountOfFlights.ContainsKey(airline) || !flights.ContainsKey(flight.Id))
            {
                throw new ArgumentException();
            }
            else
            {
                airlineWithCountOfFlights[airline] += 1;
                AirlinesWithFLightsAttached.Add(airline, flight);
            }
        }

        public bool Contains(Airline airline)
        {
            return airlines.ContainsKey(airline.Id);
        }

        public bool Contains(Flight flight)
        {
            return flights.ContainsKey(flight.Id);
        }

        public void DeleteAirline(Airline airline)
        {
            if (!airlines.ContainsKey(airline.Id))
            {
                throw new ArgumentException();
            }
            else
            {
                airlines.Remove(airline.Id);
                airlineWithCountOfFlights.Remove(airline);
            }
        }

        public IEnumerable<Airline> GetAirlinesOrderedByRatingThenByCountOfFlightsThenByName()
        {
            List<Airline> result = new List<Airline>();
            result = airlines.OrderByDescending(x => x.Value.Rating)
                .OrderByDescending(x => airlineWithCountOfFlights[x.Value])
                .OrderBy(x => x.Value.Name)
                .Select(x => x.Value)
                .ToList();
            return result;
        }

        public IEnumerable<Airline> GetAirlinesWithFlightsFromOriginToDestination(string origin, string destination)
        {
            List<Airline> result = new List<Airline>();
            result = AirlinesWithFLightsAttached.Where(x => x.Value.IsCompleted == false).Where(x => x.Value.Origin == origin && x.Value.Destination == destination).Select(x => x.Key).ToList();
            return result;
        }

        public IEnumerable<Flight> GetAllFlights()
        {
            return flights.Values;
        }

        public IEnumerable<Flight> GetCompletedFlights()
        {
            return new List<Flight>(flights.Values).Where(flights => flights.IsCompleted == true);

        }

        public IEnumerable<Flight> GetFlightsOrderedByCompletionThenByNumber()
        {
            return new List<Flight>(flights.Values).OrderBy(x => x.Number).ThenBy(x => x.IsCompleted);

        }
        public Flight PerformFlight(Airline airline, Flight flight)

        {
            if(!airlineWithCountOfFlights.ContainsKey(airline) || !flights.ContainsKey(flight.Id))
            {
                throw new ArgumentException();
            }
            else
            {
                flight.IsCompleted = true;
                return flight;
            }
        }
    }
}
