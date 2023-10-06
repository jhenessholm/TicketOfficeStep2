


using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

enum Place
{
    Seated,
    Standing
}
class Ticket
{
    public int Age { get; set; }
    public Place Place { get; set; }
    public int Number { get; private set; }
    public Ticket(int age, Place place)
    {
        Age = age;
        Place = place;
    }
    public void SetTicketNumber(int number)
    {
        Number = number;
    }
    public int Price()
    {
        int price = 0;
        if (Age <= 11)
        {
            price = (Place == Place.Seated) ? 50 : 25;
        }
        else if (Age >= 12 && Age <= 64)
        {
            price = (Place == Place.Seated) ? 170 : 110;
        }
        else if (Age >= 65)
        {
            price = (Place == Place.Seated) ? 100 : 60;
        }
        return price;
    }
    public decimal Tax()
    {
        decimal price = Price();
        decimal tax = Math.Round((1 - 1 / 1.06m) * price, 2);
        return tax;
    }
}
class TicketOffice
{
    static HashSet<int> usedPlaceNumbers = new HashSet<int>();
    static Random random = new Random();
    static int TicketNumberGenerator()
    {
        return random.Next(1, 8001);
    }
    static void Main(string[] args)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("sv-SE");
        Console.WriteLine("Ticket Office - Calculate Ticket Price");
        Console.WriteLine("--------------------------------------");
        bool continueProgram = true;
        while (continueProgram)
        {
            Console.WriteLine("\nPlease select an option:");
            Console.WriteLine("1. Calculate Ticket");
            Console.WriteLine("2. Check Place Availability");
            Console.WriteLine("3. Display Used Place Numbers");
            Console.WriteLine("4. Exit");
            Console.Write("\nEnter your choice (1/2/3/4): ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Console.Write("\nEnter your age: ");
                    int age;
                    if (!int.TryParse(Console.ReadLine(), out age) || age < 0 || age > 110)
                    {
                        Console.WriteLine(" Invalid age input.");
                        continue;
                    }
                    Console.Write("Enter ticket type (Seated or Standing): ");
                    string placeInput = Console.ReadLine().ToLower();
                    Place place;
                    if (placeInput == "seated")
                    {
                        place = Place.Seated;
                    }
                    else if (placeInput == "standing")
                    {
                        place = Place.Standing;
                    }
                    else
                    {
                        Console.WriteLine(" Invalid place preference.");
                        continue;
                    }
                    Ticket ticket = new Ticket(age, place);
                    int ticketNumber = TicketNumberGenerator();
                    ticket.SetTicketNumber(ticketNumber);
                    int ticketPrice = ticket.Price();
                    decimal tax = ticket.Tax();
                    Console.WriteLine("\nReceipt:");
                    Console.WriteLine($" Age: {age}");
                    Console.WriteLine($" Ticket Type: {placeInput}");
                    Console.WriteLine($" Ticket Price: {ticketPrice:C}");
                    Console.WriteLine($" Tax (6%): {tax:C}");
                    Console.WriteLine($" Total Price: {(ticketPrice + tax):C}");
                    Console.WriteLine($" Ticket Number: {ticketNumber}");
                    usedPlaceNumbers.Add(ticketNumber);
                    break;
                case "2":
                    Console.Write("\nEnter the place number to check availability: ");
                    int placeNumberToCheck;
                    if (int.TryParse(Console.ReadLine(), out placeNumberToCheck))
                    {
                        if (CheckPlaceAvailability(placeNumberToCheck))
                        {
                            Console.WriteLine($" Place {placeNumberToCheck} is available.");
                        }
                        else
                        {
                            Console.WriteLine($" Place {placeNumberToCheck} is already taken.");
                        }
                    }
                    else
                    {
                        Console.WriteLine(" Invalid place number input.");
                    }
                    break;
                case "3":
                    DisplayUsedPlaceNumbers();
                    break;
                case "4":
                    continueProgram = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please select a valid option (1/2/3/4).");
                    break;
            }
        }
        Console.WriteLine("\nThank you for using the Ticket Office. Goodbye!");
    }
    static bool CheckPlaceAvailability(int placeNumber)
    {
        return !usedPlaceNumbers.Contains(placeNumber);
    }
    static void DisplayUsedPlaceNumbers()
    {
        Console.WriteLine("\nUsed Place Numbers:");
        if (usedPlaceNumbers.Count == 0)
        {
            Console.WriteLine(" There are no place numbers yet.");
        }
        else
        {
            foreach (int placeNumber in usedPlaceNumbers)
            {
                Console.Write($"{placeNumber}, ");
            }
            Console.WriteLine();
        }
    }
}
