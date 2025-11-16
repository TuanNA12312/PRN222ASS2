using BusinessObjects;
using BusinessObjects.Models;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IAccountService
    {
        SystemAccount GetAccountByEmail(string email);
        List<SystemAccount> GetAccounts(string? searchQuery);
        void AddAccount(SystemAccount account);
        void UpdateAccount(SystemAccount account);
        void DeleteAccount(short id);
        SystemAccount GetAccountById(short id);
    }
}