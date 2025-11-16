using BusinessObjects;
using BusinessObjects.Models;
namespace Repositories.Interfaces
{
    public interface ISystemAccountRepository
    {
        SystemAccount GetAccountByEmail(string email);
        List<SystemAccount> GetAccounts();
        void AddAccount(SystemAccount account);
        void UpdateAccount(SystemAccount account);
        void DeleteAccount(short id);
        SystemAccount GetAccountById(short id);
    }
}