using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TestingClientApp
{
    internal class UserAccess
    {
        private Dictionary<string, User> users;

        public bool AccessDenied { get; private set; }
        public string CurrentUserLogin { get; private set; }


        public UserAccess()
        {
            users = new Dictionary<string, User>();
            AtNewUserLogIn();
            LoadUsersFromFile();
        }

        private void LoadUsersFromFile()
        {
            if (File.Exists(Properties.Settings.Default.UsersDataFileName/* Const.UsersDataFileName*/))
            {
                string usersAsJson = File.ReadAllText(Properties.Settings.Default.UsersDataFileName/*Const.UsersDataFileName*/);
                Dictionary<string, User> users = JsonConvert.DeserializeObject<Dictionary<string, User>>(usersAsJson);
                if (users != null)
                {
                    this.users = users;
                }
            }
        }
        private void SaveUsersToFile()
        {
            string usersAsJson = JsonConvert.SerializeObject(users, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(Properties.Settings.Default.UsersDataFileName/*Const.UsersDataFileName*/, usersAsJson);
        }
        private void AtNewUserLogIn()
        {
            AccessDenied = true;
            CurrentUserLogin = string.Empty;
        }
        public bool LogIn(string login, string password)
        {
            AtNewUserLogIn();

            if (users.ContainsKey(login) && users[login].Password.Equals(password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool SignUp(User user)
        {
            AtNewUserLogIn();

            if (!users.ContainsKey(user.Login) &&
                !string.IsNullOrEmpty(user.Login) &&
                !string.IsNullOrEmpty(user.Password) &&
                !string.IsNullOrEmpty(user.Email) &&
                user.Email.Contains("@") &&
                // max user age - 120 years
                DateTime.Now - user.BirthDate <= new TimeSpan(365 * 120, 0, 0, 0)
                )
            {
                users.Add(user.Login, new User(user.Login, user.Email, user.Password, user.BirthDate));
                SaveUsersToFile();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
