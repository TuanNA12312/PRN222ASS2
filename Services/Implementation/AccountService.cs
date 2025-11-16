using BusinessObjects;
using BusinessObjects.Models;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly ISystemAccountRepository _repository;

        public AccountService(ISystemAccountRepository repository)
        {
            _repository = repository;
        }

        public SystemAccount GetAccountByEmail(string email) => _repository.GetAccountByEmail(email);

        public List<SystemAccount> GetAccounts(string? searchQuery)
        {
            var accounts = _repository.GetAccounts();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                string lowerQuery = searchQuery.ToLower();
                accounts = accounts.Where(a =>
                    (a.AccountName != null && a.AccountName.ToLower().Contains(lowerQuery)) ||
                    (a.AccountEmail != null && a.AccountEmail.ToLower().Contains(lowerQuery))
                ).ToList();
            }
            return accounts;
        }

        public void AddAccount(SystemAccount account) => _repository.AddAccount(account);

        public void UpdateAccount(SystemAccount account) => _repository.UpdateAccount(account);

        public void DeleteAccount(short id) => _repository.DeleteAccount(id);

        public SystemAccount GetAccountById(short id) => _repository.GetAccountById(id);
    }
}