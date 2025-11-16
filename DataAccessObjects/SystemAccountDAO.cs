using BusinessObjects;
using BusinessObjects.Models;
using System;
using DataAccessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObjects
{
    public class SystemAccountDAO
    {
        private static SystemAccountDAO instance = null;
        private static readonly object instanceLock = new object();
        private SystemAccountDAO() { }

        public static SystemAccountDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new SystemAccountDAO();
                    }
                    return instance;
                }
            }
        }
        public SystemAccount GetAccountByEmail(string email)
        {
            using (var context = new FunewsManagementContext())
            {
                return context.SystemAccounts.FirstOrDefault(a => a.AccountEmail.Equals(email));
            }
        }

        public List<SystemAccount> GetAccounts()
        {
            using (var context = new FunewsManagementContext())
            {
                return context.SystemAccounts.ToList();
            }
        }

        public void AddAccount(SystemAccount account)
        {
            using (var context = new FunewsManagementContext())
            {
                context.SystemAccounts.Add(account);
                context.SaveChanges();
            }
        }

        public void UpdateAccount(SystemAccount account)
        {
            using (var context = new FunewsManagementContext())
            {
                context.Entry(account).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public SystemAccount GetAccountById(short id)
        {
            using (var context = new FunewsManagementContext())
            {
                return context.SystemAccounts.Find(id);
            }
        }

        public void DeleteAccount(short id)
        {
            using (var context = new FunewsManagementContext())
            {
                var account = context.SystemAccounts.Find(id);
                if (account != null)
                {
                    context.SystemAccounts.Remove(account);
                    context.SaveChanges();
                }
            }
        }
    }
}