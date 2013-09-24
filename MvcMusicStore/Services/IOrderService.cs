using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcMusicStore.Models;

namespace MvcMusicStore.Services
{
    public interface IOrderService
    {
        void AddOrder(Order orderToAdd);

    }
}