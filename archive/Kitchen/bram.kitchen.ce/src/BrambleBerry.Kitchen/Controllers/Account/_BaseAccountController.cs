using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Merchello.Core;
using Merchello.Core.Models;

namespace BrambleBerry.Kitchen.Controllers.Account
{
    [Authorize]
    public class BaseAccountController : MerchelloControllerBase
    {/// <summary>
        /// Initializes a new instance of the <see cref="BaseAccountController"/> class.
        /// </summary>
        public BaseAccountController()
            : this(MerchelloContext.Current)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseAccountController"/> class.
        /// </summary>
        /// <param name="merchelloContext">
        /// The merchello context.
        /// </param>
        public BaseAccountController(IMerchelloContext merchelloContext)
            : base(merchelloContext)
        { 
            
        }

        private ICustomer _customer;

        internal ICustomer Customer
        {
            get
            {
                if (_customer == null)
                {
                    _customer = CurrentCustomer as ICustomer;
                }
                return _customer;
            }
        }
    }
}
