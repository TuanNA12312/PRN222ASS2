using BusinessObjects;
using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.Interfaces;

namespace Repositories.Implementation
{
    public class SystemAccountRepository : ISystemAccountRepository
    {
        public SystemAccount GetAccountByEmail(string email) => SystemAccountDAO.Instance.GetAccountByEmail(email);
        public List<SystemAccount> GetAccounts() => SystemAccountDAO.Instance.GetAccounts();
        public void AddAccount(SystemAccount account) => SystemAccountDAO.Instance.AddAccount(account);
        public void UpdateAccount(SystemAccount account) => SystemAccountDAO.Instance.UpdateAccount(account);
        public void DeleteAccount(short id) => SystemAccountDAO.Instance.DeleteAccount(id);
        public SystemAccount GetAccountById(short id) => SystemAccountDAO.Instance.GetAccountById(id);
    }
}