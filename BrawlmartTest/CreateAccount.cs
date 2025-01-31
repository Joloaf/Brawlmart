using System;
using BrawlmartTest.Models;
using System.Text.RegularExpressions;
using System.Text;
using BrawlmartTest.Migrations;

namespace BrawlmartTest
{
    internal static class CreateAccount
    {
        internal static void DisplayCreateAccount(Structure structure, Menu mainMenu)
        {
            Console.Clear();
            Title.DisplayTitle();
            mainMenu.DisplayOptions();
            Console.WriteLine();
            Console.WriteLine("Create Account");
            Console.WriteLine("--------------");

            string userName = ReadUserName();
            if (userName == null) return;

            string password = ReadPassword();
            if (password == null) return;

            string confirmPassword = ReadConfirmPassword(password);
            if (confirmPassword == null) return;

            string firstName = ReadFirstName();
            if (firstName == null) return;

            string lastName = ReadLastName();
            if (lastName == null) return;

            string dateOfBirth = ReadDateOfBirth();
            if (dateOfBirth == null) return;

            string gender = ReadGender();
            if (gender == null) return;

            string email = ReadEmail();
            if (email == null) return;

            string phoneNumber = ReadPhoneNumber();
            if (phoneNumber == null) return;

            string streetAddress = ReadStreetAddress();
            if (streetAddress == null) return;

            string postalCode = ReadPostalCode();
            if (postalCode == null) return;

            string city = ReadCity();
            if (city == null) return;

            string country = ReadCountry();
            if (country == null) return;

            using (var dbContext = new MyDbContext())
            {
                var user = new User
                {
                    UserName = userName,
                    Password = password,
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = DateTime.Parse(dateOfBirth),
                    Gender = gender,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    StreetAddress = streetAddress,
                    PostalCode = postalCode,
                    City = city,
                    Country = country,
                    Admin = false,
                    AccountCreationDate = DateTime.Now
                };

                dbContext.Users.Add(user);
                dbContext.SaveChanges();
                structure.SetCurrentUser(user);
                Console.WriteLine("Account created successfully!");
                Thread.Sleep(500);
            }

            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey(true);
        }

        private static string ReadUserName()
        {
            string userName;
            do
            {
                Console.Write("Username: ");
                userName = ReadInput();
                if (userName == null) return null;
            } while (!IsValidUserName(userName));
            return userName;
        }
        private static bool IsValidUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName) || userName.Length < 8 || userName.Length > 16)
            {
                Console.WriteLine("Username must be between 8 and 16 characters.");
                return false;
            }

            if (!Regex.IsMatch(userName, "^[a-zA-Z0-9]+$"))
            {
                Console.WriteLine("Username can only contain letters and numbers.");
                return false;
            }

            return true;
        }

        private static string ReadPassword()
        {
            string password;
            do
            {
                Console.Write("Password: ");
                password = EncryptPassword();
                if (password == null) return null;
            } while (!IsValidPassword(password));
            return password;
        }
        private static string ReadConfirmPassword(string originalPassword)
        {
            string confirmPassword;
            do
            {
                Console.Write("Confirm Password: ");
                confirmPassword = EncryptPassword();
                if (confirmPassword == null) return null;
                if (confirmPassword != originalPassword)
                {
                    Console.WriteLine("Passwords do not match. Please try again.");
                    originalPassword = ReadPassword();
                    if (originalPassword == null) return null;
                }
            } while (confirmPassword != originalPassword);
            return confirmPassword;
        }
        private static bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 5 || password.Length > 24)
            {
                Console.WriteLine("Password must be between 5 and 24 characters.");
                return false;
            }
            return true;
        }
        private static string EncryptPassword()
        {
            StringBuilder password = new StringBuilder();
            ConsoleKeyInfo keyInfo;

            while (true)
            {
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    return null;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password.Remove(password.Length - 1, 1);
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    password.Append(keyInfo.KeyChar);
                    Console.Write(keyInfo.KeyChar);
                    System.Threading.Thread.Sleep(200);
                    Console.Write("\b*");
                }
            }
            return password.ToString();
        }

        private static string ReadFirstName()
        {
            string firstName;
            do
            {
                Console.Write("First Name: ");
                firstName = ReadInput();
                if (firstName == null) return null;
                if (!IsValidFirstName(firstName))
                {
                    Console.WriteLine("Invalid first name. Please try again.");
                }
            } while (!IsValidFirstName(firstName));
            return firstName;
        }
        private static bool IsValidFirstName(string firstName)
        {
            if (string.IsNullOrEmpty(firstName) || firstName.Length > 30 || !Regex.IsMatch(firstName, "^[a-zA-Z]+$"))
            {
                return false;
            }
            return true;
        }

        private static string ReadLastName()
        {
            string lastName;
            do
            {
                Console.Write("Last Name: ");
                lastName = ReadInput();
                if (lastName == null) return null;
                if (!IsValidLastName(lastName))
                {
                    Console.WriteLine("Invalid last name. Please try again.");
                }
            } while (!IsValidLastName(lastName));
            return lastName;
        }
        private static bool IsValidLastName(string lastName)
        {
            if (string.IsNullOrEmpty(lastName) || lastName.Length > 30 || !Regex.IsMatch(lastName, "^[a-zA-Z]+$"))
            {
                return false;
            }
            return true;
        }

        private static string ReadDateOfBirth()
        {
            string dateOfBirth;
            do
            {
                Console.Write("Date of Birth (yyyy-mm-dd): ");
                dateOfBirth = ReadInput();
                if (dateOfBirth == null) return null;
                if (!IsValidDateOfBirth(dateOfBirth))
                {
                    Console.WriteLine("Invalid date of birth. Please try again.");
                }
            } while (!IsValidDateOfBirth(dateOfBirth));
            return dateOfBirth;
        }
        private static bool IsValidDateOfBirth(string dateOfBirth)
        {
            if (string.IsNullOrEmpty(dateOfBirth) || !DateTime.TryParse(dateOfBirth, out _))
            {
                return false;
            }
            return true;
        }

        private static string ReadGender()
        {
            string gender;
            do
            {
                Console.WriteLine("Select Gender:");
                Console.WriteLine("1. Male");
                Console.WriteLine("2. Female");
                Console.WriteLine("3. Machine");
                Console.WriteLine("4. Other");
                Console.WriteLine("5. Prefer not to disclose");
                Console.Write("Enter the number corresponding to your gender: ");
                string input = ReadInput();
                if (input == null) return null;

                switch (input)
                {
                    case "1":
                        gender = "Male";
                        break;
                    case "2":
                        gender = "Female";
                        break;
                    case "3":
                        gender = "Machine";
                        break;
                    case "4":
                        gender = ReadCustomGender();
                        if (gender == null) return null;
                        break;
                    case "5":
                        gender = "Prefer not to disclose";
                        break;
                    default:
                        Console.WriteLine("Invalid selection. Please try again.");
                        gender = null;
                        break;
                }
            } while (gender == null);
            return gender;
        }
        private static string ReadCustomGender()
        {
            string customGender;
            do
            {
                Console.Write("Please enter your preferred gender: ");
                customGender = ReadInput();
                if (customGender == null) return null;
                if (string.IsNullOrEmpty(customGender) || customGender.Length > 30 || !Regex.IsMatch(customGender, "^[a-zA-Z]+$"))
                {
                    Console.WriteLine("Invalid entry. Please try again.");
                    customGender = null;
                }
            } while (customGender == null);
            return customGender;
        }

        private static string ReadEmail()
        {
            string email;
            do
            {
                Console.Write("Email: ");
                email = ReadInput();
                if (email == null) return null;
                if (!IsValidEmail(email))
                {
                    Console.WriteLine("Invalid email format. Please try again.");
                }
            } while (!IsValidEmail(email));
            return email;
        }
        private static bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9]+@[a-zA-Z0-9]+\.[a-zA-Z0-9]+$";
            return Regex.IsMatch(email, pattern);
        }

        private static string ReadPhoneNumber()
        {
            string phoneNumber;
            do
            {
                Console.Write("PhoneNumber (include country code): ");
                phoneNumber = ReadInput();
                if (phoneNumber == null) return null;
                if (!IsValidPhoneNumber(phoneNumber))
                {
                    Console.WriteLine("Invalid phone number. Please try again.");
                }
            } while (!IsValidPhoneNumber(phoneNumber));
            return "+" + phoneNumber;
        }
        private static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber) || phoneNumber.Length < 8 || phoneNumber.Length > 12)
            {
                return false;
            }

            if (!Regex.IsMatch(phoneNumber, "^[0-9]+$"))
            {
                return false;
            }

            return true;
        }

        private static string ReadStreetAddress()
        {
            string streetAddress;
            do
            {
                Console.Write("StreetAddress: ");
                streetAddress = ReadInput();
                if (streetAddress == null) return null;
                if (!IsValidStreetAddress(streetAddress))
                {
                    Console.WriteLine("Invalid street address. Please try again.");
                }
            } while (!IsValidStreetAddress(streetAddress));
            return streetAddress;
        }
        private static bool IsValidStreetAddress(string streetAddress)
        {
            if (string.IsNullOrEmpty(streetAddress) || !Regex.IsMatch(streetAddress, "[a-zA-Z]") || streetAddress.Length > 50)
            {
                return false;
            }
            return true;
        }

        private static string ReadPostalCode()
        {
            string postalCode;
            do
            {
                Console.Write("PostalCode: ");
                postalCode = ReadInput();
                if (postalCode == null) return null;
                if (!IsValidPostalCode(postalCode))
                {
                    Console.WriteLine("Invalid postal code. Please try again.");
                }
            } while (!IsValidPostalCode(postalCode));
            return postalCode;
        }
        private static bool IsValidPostalCode(string postalCode)
        {
            if (string.IsNullOrEmpty(postalCode) || postalCode.Length < 5 || postalCode.Length > 10)
            {
                return false;
            }

            if (!Regex.IsMatch(postalCode, "^[a-zA-Z0-9]+$"))
            {
                return false;
            }

            return true;
        }

        private static string ReadCity()
        {
            string city;
            do
            {
                Console.Write("City: ");
                city = ReadInput();
                if (city == null) return null;
                if (!IsValidCity(city))
                {
                    Console.WriteLine("Invalid city name. Please try again.");
                }
            } while (!IsValidCity(city));
            return city;
        }
        private static bool IsValidCity(string city)
        {
            if (string.IsNullOrEmpty(city) || city.Length > 50 || !Regex.IsMatch(city, "^[a-zA-Z]"))
            {
                return false;
            }
            return true;
        }

        private static string ReadCountry()
        {
            string country;
            do
            {
                Console.Write("Country: ");
                country = ReadInput();
                if (country == null) return null;
                if (!IsValidCountry(country))
                {
                    Console.WriteLine("Invalid country name. Please try again.");
                }
            } while (!IsValidCountry(country));
            return country;
        }
        private static bool IsValidCountry(string country)
        {
            if (string.IsNullOrEmpty(country) || country.Length > 50 || !Regex.IsMatch(country, "^[a-zA-Z]"))
            {
                return false;
            }
            return true;
        }

        private static string ReadInput()
        {
            StringBuilder input = new StringBuilder();
            ConsoleKeyInfo keyInfo;

            while (true)
            {
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    return null;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (input.Length > 0)
                    {
                        input.Remove(input.Length - 1, 1);
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    input.Append(keyInfo.KeyChar);
                    Console.Write(keyInfo.KeyChar);
                }
            }
            return input.ToString();
        }
    }
}

