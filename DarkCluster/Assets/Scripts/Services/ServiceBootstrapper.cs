using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DarkCluster.Core.Services
{
    public class ServiceBootstrapper
    {
        // Keep alive
        private static IEnumerable<IService> _services;

        public static IEnumerable<IService> GetServices()
        {
            if (_services == null)
                return _services = _GetServices().ToList();
            else
                return _services;
        }

        public static IEnumerable<IService> _GetServices()
        {
            foreach (var serviceType in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetInterfaces().Any(i => i == typeof(IService))))
            {
                yield return (IService)Activator.CreateInstance(serviceType);
            }
        }
    }
}
