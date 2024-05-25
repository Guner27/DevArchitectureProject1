

using Core.Entities;
using System;

namespace Entities.Concrete
{
    public class App : IEntity
    {
        public int Id { get; set; }
        public string AppName { get; set; }
        public string AppStoreURL { get; set; }
        public string PlayStoreURL { get; set; }
    }
}
