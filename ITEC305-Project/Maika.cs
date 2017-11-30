using Nancy.Authentication.Stateless;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Claims;
using ITEC305_Project.Models;
using ITEC305_Project.Auth;
using Npgsql;

namespace ITEC305_Project
{
    public static class Maika //TODO: Implement Database transactions
    {
        public const string HOST = "maika.luminousvector.com";
        private static readonly DBCredentials dBCredentials = DBCredentials.FromJSON("DB_Credentials.json");

        internal static StatelessAuthenticationConfiguration StatelessConfig { get; private set; } = new StatelessAuthenticationConfiguration(nancyContext =>
        {
            try
            {
                var ApiKey = nancyContext.Request.Cookies.First(c => c.Key == "Token").Value;
                Console.WriteLine($"Login Token: {ApiKey}");
                var i = Authenticator.GetUserIdenity(ApiKey);
                return i;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        });

        private static NpgsqlConnection GetConnection()
        {
            var conn = new NpgsqlConnection(dBCredentials.ConntectionString);
            conn.Open();
            return conn;
        }

        internal static string ValidateUser(UserCredentialsModel login) //TODO: Validate User
        {
            return Authenticator.Authenticate(new UserPrincipal(Authenticator.GenerateToken(), login.Username));
        }

        internal static UserModel CreateUser(UserCredentialsModel user)
        {
            using (var con = GetConnection())
            {
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = $"INSERT INTO project.user (user_id, username, password, email) VALUES ('', '{user.Username}', '{Utils.HashPassword(user.Password)}', '');";
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        internal static UserModel GetUserInfo(string id)
        {
            throw new NotImplementedException();
        }

        internal static bool CheckEmailExists(string email)
        {
            using (var con = GetConnection())
            {
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = $"SELECT username FROM project.user WHERE email = '{email}'";
                    return cmd.ExecuteScalar();
                }
            }
        }

        internal static bool SetUsername(string userid, UserCredentialsModel username)
        {
            using (var con = GetConnection())
            {
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = $"UPDATE project.user SET username = '{username}' WHERE user_id = '{userid}'";
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        internal static RoomModel GetRoomInfo(string roomId)
        {
            using (var con = GetConnection())
            {
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = $"SELECT title, password, type FROM project.room WHERE room_id = '{roomId}'";
                    return cmd.ExecuteReader().HasRows;
                }
            }
        }

        internal static UserModel[] GetRoomMembers(string roomId)
        {
            using (var con = GetConnection())
            {
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = $"SELECT r.room_id, u.user_id FROM project.user u LEFT JOIN project.room r ON u.user_id = r.user_id WHERE room_id = '{roomId}'";
                    return cmd.ExecuteReader().HasRows;
                }
            }
        }

        internal static bool SetRoomName(string roomId, string userId, string roomName)
        {
            using (var con = GetConnection())
            {
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = $"UPDATE project.room SET title = '{roomName}' WHERE room_id = '{roomId}' AND user_id = '{userId}'";
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        internal static bool CloseRoom(string roomId, string userId)
        {
            using (var con = GetConnection())
            {
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = $"DELETE FROM project.room WHERE room_id = '{roomId}' AND user_id = '{userId}'";
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        internal static bool SetOwner(string roomId, string userId, string newOwnerId)
        {
            using (var con = GetConnection())
            {
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = $"UPDATE project.room SET user_id = '{newOwnerId}' WHERE room_id = '{roomId}' AND user_id = '{userId}'";
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        internal static bool DeleteInvite(string inviteId, string userId)
        {
            using (var con = GetConnection())
            {
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = $"DELETE FROM project.invite WHERE invite_id = '{inviteId}' AND user_id = '{userId}'";
                    return cmd.ExecuteNonQuery();
                }
            }
        }
    }
}