using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace MMChatEngine
{
    public class UserManager
    {
        private readonly Dictionary<string, UserInfoWithPrivateInfo> _users = new Dictionary<string, UserInfoWithPrivateInfo>();
        private const string _fileName = "users.xml";
        private static readonly Lazy<UserManager> _lazy = new Lazy<UserManager>(() => new UserManager());

        private UserManager()
        {}

        public static UserManager Instance => _lazy.Value;

        /// <summary>
        /// Load users from xml file
        /// </summary>
        public void Load()
        {
            lock (_users)
            {
                if (!File.Exists(_fileName))
                    return;

                XmlDocument doc = new XmlDocument();
               
                doc.Load(_fileName);
                          
                XmlNodeList xmlNodeList = doc.SelectNodes("users/user");

                if (xmlNodeList == null)
                    return;

                foreach (XmlNode selectNode in xmlNodeList)
                {
                    XmlAttribute loginAttribute = selectNode.Attributes?["login"];
                    string login = loginAttribute.Value;
                    string pass = selectNode.SelectSingleNode("password").InnerText;
                    string nick = selectNode.SelectSingleNode("nick").InnerText;
                    Sex sex;
                    Enum.TryParse(selectNode.SelectSingleNode("sex").InnerText, out sex);
                    DateTime birthdate = DateTime.ParseExact(selectNode.SelectSingleNode("birthdate").InnerText, "dd.MM.yyyy", null);
                    _users[login] = new UserInfoWithPrivateInfo(pass, nick, sex, birthdate);
                }
            }
        }
      
        public void Update(string login, UserInfoWithPrivateInfo userInfoWithPrivateInfo)
        {
            lock (_users)
            {
                XElement doc = XElement.Load(_fileName);
                var items = from item in doc.Descendants("user")
                            where item.Attribute("login").Value == login
                            select item;
                XElement xElement = items.First();
                xElement.Element("password").ReplaceNodes(new XCData(userInfoWithPrivateInfo.Password));
                xElement.Element("nick").ReplaceNodes(new XCData(userInfoWithPrivateInfo.Nick));
                xElement.Element("sex").ReplaceNodes(new XCData(userInfoWithPrivateInfo.Sex.ToString()));
                xElement.Element("birthdate").ReplaceNodes(new XCData(userInfoWithPrivateInfo.Birthdate.ToString("dd.MM.yyyy")));
                doc.Save(_fileName);
            }
        }

        public UserInfoWithPrivateInfo GetUserInfoWithPrivateInfo(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
                throw new ArgumentException("Login can't be null or empty.", nameof(login));

            lock (_users)
            {
                return _users[login];
            }
        }

        public UserInfo GetUserInfo(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
                throw new ArgumentException("Login can't be null or empty.", nameof(login));

            lock (_users)
            {
                return _users[login];
            }
        }

        public bool TryGetUserInfoWithPrivateInfo(string login, out UserInfoWithPrivateInfo userInfoWithPrivateInfo)
        {
            if (string.IsNullOrWhiteSpace(login))
            {
                userInfoWithPrivateInfo = null;
                return false;
            }

            lock (_users)
            {
                return _users.TryGetValue(login, out userInfoWithPrivateInfo);
            }
        }

        public bool ContainsLogin(string login)
        {
            lock (_users)
            {
                return _users.ContainsKey(login);
            }
        }

        public void Add(string login, UserInfoWithPrivateInfo userInfoWithPrivateInfo)
        {
            lock (_users)
            {
                _users.Add(login, userInfoWithPrivateInfo);
                XDocument doc = XDocument.Load(_fileName);
                XElement user = new XElement("user");
                user.Add(new XAttribute("login", login));
                user.Add(new XElement("password", new XCData(userInfoWithPrivateInfo.Password)));
                user.Add(new XElement("nick", new XCData(userInfoWithPrivateInfo.Nick)));
                user.Add(new XElement("sex", new XCData(userInfoWithPrivateInfo.Sex.ToString())));
                user.Add(new XElement("birthdate", new XCData(userInfoWithPrivateInfo.Birthdate.ToString("dd.MM.yyyy"))));
                doc.Element("users")?.Add(user);
                doc.Save(_fileName);
            }
        }
    }
}