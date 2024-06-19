using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IUserRepo
    {
        void AddUser(User user);
        User GetUser(int id);
        IEnumerable<User> GetAllUsers();
        void UpdateUser(User user);
        void DeleteUser(int id);
    }
}