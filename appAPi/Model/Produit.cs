using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appAPi.Model
{
    public class Produit
    {
        public int idProduit { get; set; }
        public string CodeProduit { get; set; }
        public string DesignationProduit { get; set; }
        public Nullable<double> PrixUnitaire { get; set; }
        public Nullable<int> QuantiteMinimale { get; set; }
        public Nullable<int> QuantiteMaximale { get; set; }
        public string CodeCategorie { get; set; }
        public Nullable<double> PrixAchat { get; set; }
    }
}
