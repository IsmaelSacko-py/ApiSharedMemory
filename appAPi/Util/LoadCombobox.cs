/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiSharedMemory.Models;

namespace appAPi.Utils
{
    public class LoadComboBox
    {
        BdKi db = new BdKireneEntities();

        public List<ListSelectedViewModel> loadCategorie()
        {
            List<ListSelectedViewModel> LaList = new List<ListSelectedViewModel>();
            LaList.Add(new ListSelectedViewModel()
            {
                Value = null,
                Text = "Selectionner..."

            });

            var liste = db.Categorie.ToList();
            foreach (var categorie in liste)
            {
                LaList.Add(new ListSelectedViewModel()
                {
                    Value = categorie.CodeCategorie,
                    Text = categorie.LibelleCategorie

                });
            }
            return LaList;
        }

        public List<ListSelectedViewModel> loadProfil()
        {
            List<ListSelectedViewModel> LaList = new List<ListSelectedViewModel>();
            LaList.Add(new ListSelectedViewModel()
            {
                Value = null,
                Text = "Selectionner..."

            });

            var liste = db.Profil.ToList();
            foreach (var profil in liste)
            {
                LaList.Add(new ListSelectedViewModel()
                {
                    Value = profil.CodeProfil,
                    Text = profil.LibelleProfil

                });
            }
            return LaList;
        }

    }
}
*/