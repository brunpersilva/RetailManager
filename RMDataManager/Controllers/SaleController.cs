using Microsoft.AspNet.Identity;
using RMDataManager.Library.DataAcess;
using RMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RMDataManager.Controllers
{
    public class SaleController : ApiController
    {
        [Authorize]
        public void Post(SaleModel saleModel)
        {
            SaleData data = new SaleData();
            string userId = RequestContext.Principal.Identity.GetUserId();
            data.SaveSales(saleModel, userId);
        }

    }
}
