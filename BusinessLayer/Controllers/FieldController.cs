using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.BusinessObjects.Entities;
using DataAccess.Repositories;

namespace BusinessLayer.Controllers
{
    class FieldController
    {
        public Dictionary<string, string> ShowField(Dictionary<string, string> data, string token)
        {
            if (new UserController().CheckRank(token) != Rank.Administrator)
                return new Dictionary<string, string>
                {
                    ["error"] = "Brak uprawnień do przeglądania kierunków",
                };

            if (!data.ContainsKey("id") || !int.TryParse(data["id"], out int id) || id < 1)
                return new Dictionary<string, string>
                {
                    ["error"] = "Nieprawidłowe id",
                };

            var fRepo = new RepositoryFactory().GetRepository<Field>();
            var field = fRepo.GetDetail(p => p.Id == id);

            if (field == null)
                return new Dictionary<string, string>
                {
                    ["error"] = "Kurs o podanym id nie istnieje",
                };

            return new Dictionary<string, string>
            {
                ["name"] = field.Name,
                ["id"] = field.Id.ToString(),
            };
        }
    }
}