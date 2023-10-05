using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.DeliveriesManager
{
    public class AirlinesManager : IAirlinesManager
    {
        private Dictionary<string, Airline> airlines = new Dictionary<string, Airline>();
        private Dictionary<string, Flight> flights = new Dictionary<string, Flight>();
        private Dictionary<Airline,List<Flight>> AirlinesWithFLightsAttached = new Dictionary<Airline, List<Flight>>();
        HashSet<Airline> set = new HashSet<Airline>();
        HashSet<Flight> flightsSet = new HashSet<Flight>();

        public void AddAirline(Airline airline)
        {
           airlines.Add(airline.Id, airline);
            set.Add(airline);
        }

        public void AddFlight(Airline airline, Flight flight)
        {
            if (!airlines.ContainsKey(airline.Id))
            {
                throw new ArgumentException();
            }
            flightsSet.Add(flight);
            
            if(AirlinesWithFLightsAttached.ContainsKey(airline))
            {
                AirlinesWithFLightsAttached[airline].Add(flight);
            }
            else
            {
                AirlinesWithFLightsAttached.Add(airline, new List<Flight>());
                AirlinesWithFLightsAttached[airline].Add(flight);
            }
            
            

        }

        public bool Contains(Airline airline)
        {
            return set.Contains(airline);
        }

        public bool Contains(Flight flight)
        {
            return flightsSet.Contains(flight);
        }

        public void DeleteAirline(Airline airline)
        {
           if(!airlines.ContainsKey(airline.Id))
            {
                throw new ArgumentException();
            }
            airlines.Remove(airline.Id);
            AirlinesWithFLightsAttached.Remove(airline);
        }
        public IEnumerable<Flight> GetAllFlights()
        {
            return new List<Flight>(flights.Values);
        }

        public IEnumerable<Airline> GetAirlinesOrderedByRatingThenByCountOfFlightsThenByName()
        {
            List<Airline> result = new List<Airline>();
            result = AirlinesWithFLightsAttached.OrderByDescending(x => x.Key.Rating)
                .OrderByDescending(x => x.Value.Count)
                .ThenBy(x => x.Key.Name)
                .Select(x => x.Key)
                .ToList();
            return result;
        }

        public IEnumerable<Airline> GetAirlinesWithFlightsFromOriginToDestination(string origin, string destination)
        {
            List<Airline> result = new List<Airline>();
            result = AirlinesWithFLightsAttached.Where(x => x.Value.Where(x => x.IsCompleted == false)
            .Any(x => x.Origin == origin && x.Destination == destination))
                .Select(x => x.Key)
                .ToList();
            return result;
        }


        public IEnumerable<Flight> GetCompletedFlights()
        {
           return new List<Flight>(flights.Values).Where(x => x.IsCompleted == true);
        }

        public IEnumerable<Flight> GetFlightsOrderedByCompletionThenByNumber()
        {
            return new List<Flight>(flights.Values).OrderBy(x => x.Number).ThenBy(x => x.IsCompleted);
        }

        public Flight PerformFlight(Airline airline, Flight flight)
        {
            if(!airlines.ContainsKey(airline.Id) || !flights.ContainsKey(flight.Id))
            {
                throw new ArgumentException();
            }
            flight.IsCompleted = true;
            //AirlinesWithFLightsAttached[airline].Remove(flight);
            return flight;

            
        }
    }
}
