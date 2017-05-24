using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePointSampleDataGenerator {
  class Program {

    static void Main() {

      SharePointListFactory.CreateProductsLists();
      SharePointListFactory.CreateExpensesLists();
      SharePointListFactory.CreateCustomersList(250, 50);

    }

  }
}
