using System;
using System.Collections.Generic;

namespace Login
{
    class Program
    {
        static List<User> listUser = new List<User>();
        static List<User> searchResult = new List<User>();
        static void Main(string[] args)
        {
            int menuChoice = 0;
            //listUser.Add(new User("naufal", "hafiz", "asd", "naha"));
            //listUser.Add(new User("hafiz", "naufal", "asd", "hana"));
            do
            {
                Console.Clear();
                try
                {
                    PrintMenu();
                    menuChoice = int.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Masukkan angka saja! [Press Enter to continue]");
                    char ch = Console.ReadKey(true).KeyChar;
                }

                switch (menuChoice)
                {
                    case 1:
                        Console.Clear();
                        CreateUser();
                        break;
                    case 2:
                        Console.Clear();
                        ShowUser(listUser, "show");
                        break;
                    case 3:
                        Console.Clear();
                        SearchUsername();
                        break;
                    case 4:
                        Console.Clear();
                        DoLogin();
                        break;


                }
            } while (menuChoice != 6);
        }
        static void ShowUser(List<User> li, string type)
        {
            //Console.Clear();
            if (li.Count == 0)
            {
                Console.WriteLine("There is no data !");
                char ch = Console.ReadKey(true).KeyChar;
            }
            else
            {
                if (type == "show")
                {
                    Console.WriteLine("==SHOW USER==");

                }
                else
                {
                    //Console.WriteLine("");
                }
                foreach (User user in li)
                {
                    Console.WriteLine("====================");
                    Console.WriteLine($"NAME : {user.FirstName} {user.LastName}");
                    Console.WriteLine($"USERNAME : {user.Username}");
                    Console.WriteLine($"PASSWORD : {user.CheckPassword()}");
                    Console.WriteLine("====================");
                }
                char ch = Console.ReadKey(true).KeyChar;
            }
        }

        static void CreateUser()
        {
            System.Random random = new System.Random();
            string newFirstName, newLastName;
            string newPassword = "";
            List<string> usernameRecomendation = new List<string>();
            string newUsername;

            do
            {
                Console.Clear();
                Console.WriteLine("===CREATE USER===");
                Console.Write("Firstname : ");
                newFirstName = Console.ReadLine();  //memperoleh firstname
                Console.Write("Lastname : ");
                newLastName = Console.ReadLine();   //memperoleh lastname
                Console.Write("Password : ");
                newPassword = Console.ReadLine();   //memperoleh password

                //Cek input firstname, laestname, and password have length minimum of 2
                if (newFirstName.Length < 2 || newLastName.Length < 2 || newPassword.Length < 2)
                {
                    Console.WriteLine("Error : Input is not valid");
                    char ch = Console.ReadKey(true).KeyChar;
                }
            } while (newFirstName.Length < 2 || newLastName.Length < 2 || newPassword.Length < 2);

            //isername is concaft 2 character from firstname and lastname
            newUsername = $"{newFirstName.Substring(0, 2)}{newLastName.Substring(0, 2)}";
            //mengatasi username yang sama
            if (IsDuplicate(newUsername))
            {
                int usernameChoice = 0;
                //rekomended username
                string newUsername1, newUsername2, newUsername3;
                do
                {
                    newUsername1 = newUsername + random.Next(100);
                } while (IsDuplicate(newUsername1));
                usernameRecomendation.Add(newUsername1);
                do
                {
                    newUsername2 = newUsername + random.Next(100);
                } while (IsDuplicate(newUsername2));
                usernameRecomendation.Add(newUsername2);
                do
                {
                    newUsername3 = newUsername + random.Next(100);
                } while (IsDuplicate(newUsername3));
                usernameRecomendation.Add(newUsername3);

                Console.WriteLine("List username rekomendasi ");
                for (int i = 0; i < usernameRecomendation.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {usernameRecomendation[i]}");
                }
                do
                {
                    Console.Write("Pilihanmu : ");
                    usernameChoice = int.Parse(Console.ReadLine());
                } while (usernameChoice < 1 || usernameChoice > usernameRecomendation.Count);
                usernameChoice--;
                newUsername = usernameRecomendation[usernameChoice];


            }
            //listUser.Add(new User(newFirstName, newLastName, newPassword, newUsername));
            listUser.Add(new User(newFirstName, newLastName, BCrypt.Net.BCrypt.HashPassword(newPassword), newUsername));
        }

        static bool IsDuplicate(string username)
        {
            foreach (User user in listUser)
            {
                if (user.Username == username)
                {
                    return true;
                }
            }
            return false;
        }

        static void PrintMenu()
        {
            Console.WriteLine("==BASIC AUTHENTICATION==");
            Console.WriteLine("1.Create User");
            Console.WriteLine("2.Show User");
            Console.WriteLine("3.Search");
            Console.WriteLine("4.Login");
            //Console.WriteLine("5.Update Password");
            Console.WriteLine("5.Exit");
            Console.Write("Input: ");
        }

        static void DoLogin()
        {
            string loginUsername, loginPassword;
            Console.Clear();
            Console.WriteLine("===LOGIN===");
            Console.Write("USERNAME : ");
            loginUsername = Console.ReadLine();
            Console.Write("PASSWORD : ");
            loginPassword = Console.ReadLine();

            foreach (User user in listUser)
            {
                if (user.Username == loginUsername && BCrypt.Net.BCrypt.Verify(loginPassword, user.CheckPassword()))
                {
                    Console.WriteLine("Message : Login Success !");
                    char ch = Console.ReadKey(true).KeyChar;
                    Console.Clear();
                    try
                    {
                        char ans;
                        string updatePass;
                        Console.WriteLine($"Hallo {user.FirstName} {user.LastName}");
                        Console.Write("Apakah Anda ingin menhhanti password? [Y/N] : ");
                        ans = Console.ReadLine()[0];
                        if (ans == 'y' || ans == 'Y')
                        {
                            do
                            {
                                Console.Write("Password Baru Anda : ");
                                updatePass = Console.ReadLine();
                                ClearLine();
                                if (updatePass.Length < 2)
                                {
                                    Console.WriteLine("Error : Input is not valid");
                                    char check = Console.ReadKey(true).KeyChar;
                                }
                            } while (updatePass.Length < 2);

                            user.UpdatePassword(BCrypt.Net.BCrypt.HashPassword(updatePass));

                        }
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine($"Error : {e.Message}");
                        Console.WriteLine("Masukkan hanya huruf satu karakter! [Press Enter to continue]");
                        char check = Console.ReadKey(true).KeyChar;
                    }
                    return;
                }
                if ((user.Username == loginUsername && !BCrypt.Net.BCrypt.Verify(loginPassword, user.CheckPassword())))
                {
                    Console.WriteLine("Message : Wrong username/password !");
                    char ch = Console.ReadKey(true).KeyChar;
                    return;
                }


            }
            Console.WriteLine("Message : User not found !");
            char chr = Console.ReadKey(true).KeyChar;
            return;
        }

        static void SearchUsername()
        {
            Console.Clear();
            string searchUsername;
            Console.WriteLine("==SEARCH USER==");
            Console.Write("Masukkan username yang ingin dicari : ");
            searchUsername = Console.ReadLine();
            searchResult.Clear();
            foreach (User user in listUser)
            {
                string fullName = $"{user.FirstName} {user.LastName}";
                if (fullName.Contains(searchUsername))
                {
                    searchResult.Add(user);
                }
            }

            Console.WriteLine($"Found Data : {searchResult.Count}");
            ShowUser(searchResult, "search");



        }
        public static void ClearLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }

    }
}
