using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBiRestApiDemo {

  public class Dataset {
    public string id { get; set; }
    public string name { get; set; }
  }

  public class DatasetCollection {
    public List<Dataset> value { get; set; }
  }

  public class CountryRow {
    public string Country { get; set; }
    public int Population { get; set; }
    public string Continent { get; set; }
  }

  class CountryTableRows {
    public CountryRow[] rows { get; set; }
  }

  public class StateRow {
    public string State { get; set; }
    public string Abbreviation { get; set; }
    public int Founded { get; set; }
    public int SquareMiles { get; set; }
    public int Population { get; set; }
    public double PopulationDensity { get; set; }
    public string CapitalCity { get; set; }
  }

  class StateTableRows {
    public StateRow[] rows { get; set; }
  }


  class SampleData {

    public static CountryTableRows GetCountries() {
      CountryRow[] Countries = {
        new CountryRow { Country="China", Population=1385566537, Continent="Asia" },
        new CountryRow { Country="India", Population=1252139596, Continent="Asia" },
        new CountryRow { Country="United States", Population=320050716, Continent="North America" },
        new CountryRow { Country="Indonesia", Population=249865631, Continent="Asia" },
        new CountryRow { Country="Brazil", Population=200361925, Continent="South America" },
        new CountryRow { Country="Pakistan", Population=182142594, Continent="Asia" },
        new CountryRow { Country="Nigeria", Population=173615345, Continent="Africa" },
        new CountryRow { Country="Bangladesh", Population=156594962, Continent="Asia" },
        new CountryRow { Country="Russia", Population=142833689, Continent="Asia" },
        new CountryRow { Country="Japan", Population=127143577, Continent="Asia" },
        #region "More countries"
        new CountryRow { Country="Mexico", Population=122332399, Continent="North America" },
        new CountryRow { Country="Philippines", Population=98393574, Continent="Asia" },
        new CountryRow { Country="Ethiopia", Population=94100756, Continent="Africa" },
        new CountryRow { Country="Vietnam", Population=91679733, Continent="Asia" },
        new CountryRow { Country="Germany", Population=82726626, Continent="Europe" },
        new CountryRow { Country="Egypt", Population=82056378, Continent="Africa" },
        new CountryRow { Country="Iran", Population=77447168, Continent="Asia" },
        new CountryRow { Country="Turkey", Population=74932641, Continent="Europe" },
        new CountryRow { Country="Democratic Republic of the Congo", Population=67513677, Continent="Africa" },
        new CountryRow { Country="Thailand", Population=67010502, Continent="Asia" },
        new CountryRow { Country="France", Population=64291280, Continent="Europe" },
        new CountryRow { Country="United Kingdom", Population=63136265, Continent="Europe" },
        new CountryRow { Country="Italy", Population=60990277, Continent="Europe" },
        new CountryRow { Country="Myanmar", Population=53259018, Continent="Asia" },
        new CountryRow { Country="South Africa", Population=52776130, Continent="Africa" },
        new CountryRow { Country="Korea, South", Population=49262698, Continent="Asia" },
        new CountryRow { Country="Tanzania", Population=49253126, Continent="Africa" },
        new CountryRow { Country="Colombia", Population=48321405, Continent="South America" },
        new CountryRow { Country="Spain", Population=46926963, Continent="Europe" },
        new CountryRow { Country="Ukraine", Population=45238805, Continent="Europe" },
        new CountryRow { Country="Kenya", Population=44353691, Continent="Africa" },
        new CountryRow { Country="Argentina", Population=41446246, Continent="South America" },
        new CountryRow { Country="Algeria", Population=39208194, Continent="Africa" },
        new CountryRow { Country="Poland", Population=38216635, Continent="Europe" },
        new CountryRow { Country="Sudan", Population=37964306, Continent="Africa" },
        new CountryRow { Country="Uganda", Population=37578876, Continent="Africa" },
        new CountryRow { Country="Canada", Population=35181704, Continent="North America" },
        new CountryRow { Country="Iraq", Population=33765232, Continent="Asia" },
        new CountryRow { Country="Morocco", Population=33008150, Continent="Africa" },
        new CountryRow { Country="Afghanistan", Population=30551674, Continent="Asia" },
        new CountryRow { Country="Venezuela", Population=30405207, Continent="South America" },
        new CountryRow { Country="Peru", Population=30375603, Continent="South America" },
        new CountryRow { Country="Malaysia", Population=29716965, Continent="Asia" },
        new CountryRow { Country="Uzbekistan", Population=28934102, Continent="Asia" },
        new CountryRow { Country="Saudi Arabia", Population=28828870, Continent="Asia" },
        new CountryRow { Country="Nepal", Population=27797457, Continent="Asia" },
        new CountryRow { Country="Ghana", Population=25904598, Continent="Africa" },
        new CountryRow { Country="Mozambique", Population=25833752, Continent="Africa" },
        new CountryRow { Country="North Korea", Population=24895480, Continent="Asia" },
        new CountryRow { Country="Yemen", Population=24407381, Continent="Asia" },
        new CountryRow { Country="Australia", Population=23342553, Continent="Oceania" },
        new CountryRow { Country="Taiwan", Population=23329772, Continent="Asia" },
        new CountryRow { Country="Madagascar", Population=22924851, Continent="Africa" },
        new CountryRow { Country="Cameroon", Population=22253959, Continent="Africa" },
        new CountryRow { Country="Syria", Population=21898061, Continent="Asia" },
        new CountryRow { Country="Romania", Population=21698585, Continent="Europe" },
        new CountryRow { Country="Angola", Population=21471618, Continent="Africa" },
        new CountryRow { Country="Sri Lanka", Population=21273228, Continent="Asia" },
        new CountryRow { Country="Niger", Population=17831270, Continent="Africa" },
        new CountryRow { Country="Chile", Population=17619708, Continent="South America" },
        new CountryRow { Country="Netherlands", Population=16759229, Continent="Europe" },
        new CountryRow { Country="Kazakhstan", Population=16440586, Continent="Asia" },
        new CountryRow { Country="Malawi", Population=16362567, Continent="Africa" },
        new CountryRow { Country="Ecuador", Population=15737878, Continent="South America" },
        new CountryRow { Country="Guatemala", Population=15468203, Continent="North America" },
        new CountryRow { Country="Mali", Population=15301650, Continent="Africa" },
        new CountryRow { Country="Cambodia", Population=15135169, Continent="Asia" },
        new CountryRow { Country="Zambia", Population=14538640, Continent="Africa" },
        new CountryRow { Country="Zimbabwe", Population=14149648, Continent="Africa" },
        new CountryRow { Country="Chad", Population=12825314, Continent="Africa" },
        new CountryRow { Country="Rwanda", Population=11776522, Continent="Africa" },
        new CountryRow { Country="Guinea", Population=11745189, Continent="Africa" },
        new CountryRow { Country="South Sudan", Population=11296173, Continent="Africa" },
        new CountryRow { Country="Cuba", Population=11265629, Continent="North America" },
        new CountryRow { Country="Greece", Population=11127990, Continent="Europe" },
        new CountryRow { Country="Belgium", Population=11104476, Continent="Europe" },
        new CountryRow { Country="Tunisia", Population=10996515, Continent="Africa" },
        new CountryRow { Country="Czech Republic", Population=10702197, Continent="Europe" },
        new CountryRow { Country="Bolivia", Population=10671200, Continent="South America" },
        new CountryRow { Country="Portugal", Population=10608156, Continent="Europe" },
        new CountryRow { Country="Somalia", Population=10495583, Continent="Africa" },
        new CountryRow { Country="Dominican Republic", Population=10403761, Continent="North America" },
        new CountryRow { Country="Haiti", Population=10317461, Continent="North America" },
        new CountryRow { Country="Hungary", Population=9954941, Continent="Europe" },
        new CountryRow { Country="Sweden", Population=9571105, Continent="Europe" },
        new CountryRow { Country="Serbia", Population=9510506, Continent="Europe" },
        new CountryRow { Country="United Arab Emirates", Population=9346129, Continent="Asia" },
        new CountryRow { Country="Austria", Population=8495145, Continent="Europe" },
        new CountryRow { Country="Tajikistan", Population=8207834, Continent="Asia" },
        new CountryRow { Country="Honduras", Population=8097688, Continent="North America" },
        new CountryRow { Country="Switzerland", Population=8077833, Continent="Europe" },
        new CountryRow { Country="Israel", Population=7733144, Continent="Asia" },
        new CountryRow { Country="Papua New Guinea", Population=7321262, Continent="Oceania" },
        new CountryRow { Country="Jordan", Population=7273799, Continent="Africa" },
        new CountryRow { Country="Bulgaria", Population=7222943, Continent="Europe" },
        new CountryRow { Country="Hong Kong", Population=7203836, Continent="Asia" },
        new CountryRow { Country="Togo", Population=6816982, Continent="Africa" },
        new CountryRow { Country="Paraguay", Population=6802295, Continent="South America" },
        new CountryRow { Country="Laos", Population=6769727, Continent="Asia" },
        new CountryRow { Country="El Salvador", Population=6340454, Continent="North America" },
        new CountryRow { Country="Libya", Population=6201521, Continent="Africa" },
        new CountryRow { Country="Nicaragua", Population=6080478, Continent="North America" },
        new CountryRow { Country="Denmark", Population=5619096, Continent="Europe" },
        new CountryRow { Country="Kyrgyzstan", Population=5547548, Continent="Asia" },
        new CountryRow { Country="Slovakia", Population=5450223, Continent="Europe" },
        new CountryRow { Country="Finland", Population=5426323, Continent="Europe" },
        new CountryRow { Country="Singapore", Population=5411737, Continent="Asia" },
        new CountryRow { Country="Turkmenistan", Population=5240072, Continent="Asia" },
        new CountryRow { Country="Norway", Population=5042671, Continent="Europe" },
        new CountryRow { Country="Costa Rica", Population=4872166, Continent="North America" },
        new CountryRow { Country="Lebanon", Population=4821971, Continent="Asia" },
        new CountryRow { Country="Ireland", Population=4627173, Continent="Europe" },
        new CountryRow { Country="Central African Republic", Population=4616417, Continent="Africa" },
        new CountryRow { Country="New Zealand", Population=4505761, Continent="Oceania" },
        new CountryRow { Country="Republic of the Congo", Population=4447632, Continent="Africa" },
        new CountryRow { Country="Georgia", Population=4340895, Continent="Europe" },
        new CountryRow { Country="Palestine", Population=4326295, Continent="Asia" },
        new CountryRow { Country="Liberia", Population=4294077, Continent="Africa" },
        new CountryRow { Country="Croatia", Population=4289714, Continent="Europe" },
        new CountryRow { Country="Panama", Population=3864170, Continent="North America" },
        new CountryRow { Country="Puerto Rico", Population=3688318, Continent="North America" },
        new CountryRow { Country="Oman", Population=3632444, Continent="Asia" },
        new CountryRow { Country="Moldova", Population=3487204, Continent="Europe" },
        new CountryRow { Country="Uruguay", Population=3407062, Continent="South America" },
        new CountryRow { Country="Kuwait", Population=3368572, Continent="Asia" },
        new CountryRow { Country="Albania", Population=3173271, Continent="Europe" },
        new CountryRow { Country="Lithuania", Population=3016933, Continent="Europe" },
        new CountryRow { Country="Armenia", Population=2976566, Continent="Europe" },
        new CountryRow { Country="Mongolia", Population=2839073, Continent="Asia" },
        new CountryRow { Country="Jamaica", Population=2783888, Continent="North America" },
        new CountryRow { Country="Namibia", Population=2303315, Continent="Africa" },
        new CountryRow { Country="Qatar", Population=2168673, Continent="Asia" },
        new CountryRow { Country="Macedonia", Population=2107158, Continent="Europe" },
        new CountryRow { Country="Lesotho", Population=2074465, Continent="Africa" },
        new CountryRow { Country="Slovenia", Population=2071997, Continent="Europe" },
        new CountryRow { Country="Latvia", Population=2050317, Continent="Europe" },
        new CountryRow { Country="Botswana", Population=2021144, Continent="Africa" }
        #endregion
      };
      return new CountryTableRows { rows = Countries };
    }

    public static StateTableRows GetStates() {

      StateRow[] States = {
      new StateRow{ State="New Jersey", Abbreviation="NJ", Founded=1787, Population=8791894, SquareMiles=7419,  CapitalCity="Trenton, NJ"},
      new StateRow{ State="Connecticut", Abbreviation="CT", Founded=1788, Population=3574097, SquareMiles=4845,  CapitalCity="Hartford, CT"},
      new StateRow{ State="Delaware", Abbreviation="DE", Founded=1788, Population=897934, SquareMiles=1982,  CapitalCity="Dover, DE"},
      new StateRow{ State="Georgia", Abbreviation="GA", Founded=1788, Population=9687653, SquareMiles=57919,  CapitalCity="Atlanta, GA"},
      new StateRow{ State="Maryland", Abbreviation="MD", Founded=1788, Population=5773552, SquareMiles=9775,  CapitalCity="Annapolis, MD"},
      new StateRow{ State="Massachusetts", Abbreviation="MA", Founded=1788, Population=6547629, SquareMiles=7838,  CapitalCity="Boston, MA"},
      new StateRow{ State="New Hampshire", Abbreviation="NH", Founded=1788, Population=1316472, SquareMiles=8969,  CapitalCity="Concord, NH"},
      new StateRow{ State="New York", Abbreviation="NY", Founded=1788, Population=19378104, SquareMiles=47224,  CapitalCity="Albany, NY"},
      new StateRow{ State="Pennsylvania", Abbreviation="PA", Founded=1788, Population=12702379, SquareMiles=44820,  CapitalCity="Harrisburg, PA"},
      new StateRow{ State="South Carolina", Abbreviation="SC", Founded=1788, Population=4625364, SquareMiles=30111,  CapitalCity="Columbia, SC"},
      new StateRow{ State="Virginia", Abbreviation="VA", Founded=1788, Population=8001024, SquareMiles=39598,  CapitalCity="Richmond, VA"},
      new StateRow{ State="North Carolina", Abbreviation="NC", Founded=1789, Population=9535475, SquareMiles=48718,  CapitalCity="Raleigh, NC"},
      new StateRow{ State="Rhode Island", Abbreviation="RI", Founded=1790, Population=1052567, SquareMiles=1045,  CapitalCity="Providence, RI"},
      new StateRow{ State="Vermont", Abbreviation="VT", Founded=1791, Population=625741, SquareMiles=9249,  CapitalCity="Montpelier, VT"},
      new StateRow{ State="Kentucky", Abbreviation="KY", Founded=1798, Population=4339362, SquareMiles=39732,  CapitalCity="Frankfort, KY"},
      new StateRow{ State="Tennessee", Abbreviation="TN", Founded=1802, Population=6346110, SquareMiles=41220,  CapitalCity="Nashville, TN"},
      new StateRow{ State="Ohio", Abbreviation="OH", Founded=1806, Population=11536502, SquareMiles=40953,  CapitalCity="Columbus, OH"},
      new StateRow{ State="Louisiana", Abbreviation="LA", Founded=1812, Population=4533372, SquareMiles=43566,  CapitalCity="Baton Rouge, LA"},
      new StateRow{ State="Indiana", Abbreviation="IN", Founded=1817, Population=6483800, SquareMiles=35870,  CapitalCity="Indianapolis, IN"},
      new StateRow{ State="Mississippi", Abbreviation="MS", Founded=1818, Population=2967297, SquareMiles=46914,  CapitalCity="Jackson, MS"},
      new StateRow{ State="Alabama", Abbreviation="AL", Founded=1819, Population=4779735, SquareMiles=50750,  CapitalCity="Montgomery, AL"},
      new StateRow{ State="Maine", Abbreviation="ME", Founded=1820, Population=1328361, SquareMiles=30865,  CapitalCity="Augusta, ME"},
      new StateRow{ State="Missouri", Abbreviation="MO", Founded=1821, Population=5988927, SquareMiles=68898,  CapitalCity="Jefferson City, MO"},
      new StateRow{ State="Illinois", Abbreviation="IL", Founded=1822, Population=12830632, SquareMiles=55593,  CapitalCity="Springfield, IL"},
      new StateRow{ State="Arkansas", Abbreviation="AR", Founded=1836, Population=2915921, SquareMiles=52075,  CapitalCity="Little Rock, AR"},
      new StateRow{ State="Michigan", Abbreviation="MI", Founded=1837, Population=9883635, SquareMiles=58110,  CapitalCity="Lansing, MI"},
      new StateRow{ State="Texas", Abbreviation="TX", Founded=1845, Population=25145561, SquareMiles=261914,  CapitalCity="Austin, TX"},
      new StateRow{ State="Florida", Abbreviation="FL", Founded=1846, Population=18801311, SquareMiles=53997,  CapitalCity="Tallahassee, FL"},
      new StateRow{ State="Iowa", Abbreviation="IA", Founded=1846, Population=3046350, SquareMiles=55875,  CapitalCity="Des Moines, IA"},
      new StateRow{ State="Wisconsin", Abbreviation="WI", Founded=1848, Population=5686986, SquareMiles=54314,  CapitalCity="Madison, WI"},
      new StateRow{ State="California", Abbreviation="CA", Founded=1851, Population=37253956, SquareMiles=155973,  CapitalCity="Sacramento, CA"},
      new StateRow{ State="Minnesota", Abbreviation="MN", Founded=1858, Population=5303925, SquareMiles=79617,  CapitalCity="St. Paul, MN"},
      new StateRow{ State="Oregon", Abbreviation="OR", Founded=1859, Population=3831074, SquareMiles=96003,  CapitalCity="Salem, OR"},
      new StateRow{ State="Kansas", Abbreviation="KS", Founded=1861, Population=2853118, SquareMiles=81823,  CapitalCity="Topeka, KS"},
      new StateRow{ State="West Virginia", Abbreviation="WV", Founded=1863, Population=1852996, SquareMiles=24087,  CapitalCity="Charleston, WV"},
      new StateRow{ State="Nevada", Abbreviation="NV", Founded=1864, Population=2700551, SquareMiles=109806,  CapitalCity="Carson City, NV"},
      new StateRow{ State="Nebraska", Abbreviation="NE", Founded=1870, Population=1826341, SquareMiles=76664,  CapitalCity="Lincoln, NE"},
      new StateRow{ State="Colorado", Abbreviation="CO", Founded=1884, Population=5029196, SquareMiles=103730,  CapitalCity="Denver, CO"},
      new StateRow{ State="Montana", Abbreviation="MT", Founded=1890, Population=989415, SquareMiles=145556,  CapitalCity="Helena, MT"},
      new StateRow{ State="Washington", Abbreviation="WA", Founded=1890, Population=6724540, SquareMiles=66582,  CapitalCity="Olympia, WA"},
      new StateRow{ State="Wyoming", Abbreviation="WY", Founded=1890, Population=563626, SquareMiles=97105,  CapitalCity="Cheyenne, WY"},
      new StateRow{ State="Idaho", Abbreviation="ID", Founded=1892, Population=1567582, SquareMiles=82751,  CapitalCity="Boise, ID"},
      new StateRow{ State="North Dakota", Abbreviation="ND", Founded=1894, Population=672591, SquareMiles=68994,  CapitalCity="Bismark, ND"},
      new StateRow{ State="South Dakota", Abbreviation="SD", Founded=1894, Population=814180, SquareMiles=75898,  CapitalCity="Pierre, SD"},
      new StateRow{ State="Utah", Abbreviation="UT", Founded=1896, Population=2763885, SquareMiles=82168,  CapitalCity="Salt Lake City, UT"},
      new StateRow{ State="Oklahoma", Abbreviation="OK", Founded=1907, Population=3751354, SquareMiles=68679,  CapitalCity="Oklahoma City, OK"},
      new StateRow{ State="Arizona", Abbreviation="AZ", Founded=1912, Population=6329013, SquareMiles=114000,  CapitalCity="Phoenix, AZ"},
      new StateRow{ State="New Mexico", Abbreviation="NM", Founded=1912, Population=2059180, SquareMiles=121365,  CapitalCity="Santa Fe, NM"},
      new StateRow{ State="Alaska", Abbreviation="AK", Founded=1959, Population=710231, SquareMiles=570374,  CapitalCity="Juneau, AK"},
      new StateRow{ State="Hawaii", Abbreviation="HI", Founded=1959, Population=1360301, SquareMiles=6243,  CapitalCity="Honolulu, HI"}
      };

      // calculate population density
      foreach (var State in States) {
        State.PopulationDensity = Math.Round( ((double)State.Population/(double)State.SquareMiles), 2);
      }

      return new StateTableRows { rows = States };
    }
  }
}


