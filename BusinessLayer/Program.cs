using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.BusinessObjects.Entities;
using DataAccess.Repositories;

namespace BusinessLayer
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Dupa");
            var repo = new RepositoryFactory().GetRepository<Field>();
            var field2 = new Field
            {
                Name = "Cyberbezpieczeństwo"
            };
            repo.Add(field2);
            repo.SaveChanges();
            var kupa = repo.GetOverview();
            foreach (var field1 in kupa)
            {
                Console.WriteLine(field1.Name);
            }

            Console.WriteLine("Dupsztajn");
        }
    }
}