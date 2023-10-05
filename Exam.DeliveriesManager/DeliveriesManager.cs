using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.DeliveriesManager
{
    public class DeliveriesManager : IDeliveriesManager
    {
        private Dictionary<string,Deliverer> IdDeliverers = new Dictionary<string, Deliverer>();
        private Dictionary<string, Package> IdPackages = new Dictionary<string, Package>();
        private Dictionary<Deliverer,int> delivererWithPachage = new Dictionary<Deliverer, int>();
        private HashSet<Package> assignedPackages = new HashSet<Package>();

        public void AddDeliverer(Deliverer deliverer)
        {
            
            IdDeliverers.Add(deliverer.Id, deliverer);
            delivererWithPachage.Add(deliverer, 0);
        }

        public void AddPackage(Package package)
        {
            IdPackages.Add(package.Id, package);
        }

        public void AssignPackage(Deliverer deliverer, Package package)
        {
            if(!IdDeliverers.ContainsKey(deliverer.Id) || !IdPackages.ContainsKey(package.Id))
            {
                throw new ArgumentException();
            }
            else if(delivererWithPachage.ContainsKey(deliverer))
            {
                delivererWithPachage[deliverer] += 1;
            }
           assignedPackages.Add(package);
          
        }

        public bool Contains(Deliverer deliverer)
        {
            return IdDeliverers.ContainsKey(deliverer.Id);
        }

        public bool Contains(Package package)
        {
            return IdPackages.ContainsKey(package.Id);
        }

        public IEnumerable<Deliverer> GetDeliverers()
        {
           return IdDeliverers.Values;
        }

        public IEnumerable<Package> GetPackages()
        {
            return IdPackages.Values;
        }
        public IEnumerable<Deliverer> GetDeliverersOrderedByCountOfPackagesThenByName()
        {

            List<Deliverer> result = new List<Deliverer>();
            result =  delivererWithPachage.OrderByDescending(x => x.Value).ThenBy(x => x.Key.Name).Select(x => x.Key).ToList();
            return result;
        }


        public IEnumerable<Package> GetPackagesOrderedByWeightThenByReceiver()
        {
            List<Package> result = new List<Package>();
            result = IdPackages.Values.OrderByDescending(x => x.Weight).ThenBy(x => x.Receiver).ToList();
            return result;
        }

        public IEnumerable<Package> GetUnassignedPackages()
        {
            List<Package> result = new List<Package>();
            result = IdPackages.Values.Where(x => !assignedPackages.Contains(x)).ToList();
            return result;
        }
    }
}
