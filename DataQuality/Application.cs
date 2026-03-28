using DataQuality.Data;
using DataQuality.Models;
using DataQuality.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System.Data.Common;
using System.Diagnostics.Metrics;
using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataQuality
{
    public class Application
    {
        public void Run()
        {
            // ====================================================================
            // Skapa databasen
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true);
            var config = builder.Build();

            var options = new DbContextOptionsBuilder<AppDbContext>();
            var connectionString = config.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connectionString);

            using (var dbContext = new AppDbContext(options.Options))
            {
                var dataInitiaizer = new DataInitializer();
                dataInitiaizer.MigrateDatabase(dbContext);
            }

            // ====================================================================
            // Hämta Customers
            var loadCustomers = new LoadInitialData();

            var filepathCustomers = "..\\..\\..\\OriginalDataInExcel\\Customers.csv";
            var customers = loadCustomers.ReadCustomers(filepathCustomers);

            // Hämta Orders
            var filepathOrders = "..\\..\\..\\OriginalDataInExcel\\Orders.csv";
            var orders = loadCustomers.ReadOrders(filepathOrders);

            // ====================================================================
            // Quality Checks Quality Checks Quality Checks Quality Checks Quality Checks
            // Quality Checks Quality Checks Quality Checks Quality Checks Quality Checks
            // Quality Checks Quality Checks Quality Checks Quality Checks Quality Checks
            // Quality Checks Quality Checks Quality Checks Quality Checks Quality Checks

            // I denna app hanterar vi endast Extraction och Load i ETL processen
            // Denna app ger en rapport på samtliga felakigheter som hittas i datan,
            // I en "riktig" app skulle vi även ger möjligheten att "rätta" datan innan den laddas in i databasen

            // 1. Unikhet (Uniqueness)
            // Kontrollera att identifierare är unika.
            // Detta är den kontroll vi precis skrev kod för.

            // Check: Finns det flera kunder med samma CustomerUniqueId?
            // I vår data: Anna Andersson(ID 101) förekommer två gånger.
            // Diskussion: Ska man radera dubbletten, eller uppdatera den befintliga raden?

            // Kontrollera Id dubbletter i både tabeller
            new DuplicateChecksId().CheckForDuplicateIds(customers, orders);
            Console.WriteLine("================================================");

            // 2.Referensintegritet(Referential Integrity)
            // Kontrollera att kopplingar mellan tabeller stämmer.
            // Check: Har alla rader i Orders.csv en motsvarande Customer i Customers.csv?
            // I vår data: Order 5004 tillhör CustomerUniqueId 999, men kund 999 finns inte i kundlistan.
            //Diskussion: Vad händer om vi försöker spara en order utan kund i en SQL - databas med Foreign Keys ? (Det kraschar).

            // Kontrollera Id dubbletter i både tabeller
            new ReferentialIntegrity().CheckReferentialIntegrity(customers, orders);
            Console.WriteLine("================================================");

            // 3.Fullständighet(Completeness)
            // Kontrollera om obligatoriska fält saknas(Null - checks).
            // Check: Finns det tomma fält för "email" eller "city" där det borde finnas data?
            // I din data: David Dahl(ID 104) saknar e-postadress.City saknas på Order 5004.
            // Diskussion: Är e-post obligatoriskt för att skapa en faktura? Om ja, hur hanterar vi kunden ?

            // Kontrollera tomma fält i både tabeller
            new Completeness().CheckCompleteness(customers, orders);
            Console.WriteLine("================================================");

            // 4.Giltighet & Format(Validity)
            // Kontrollera att datan följer förväntade mönster eller typer.
            // Check: Är e-postadressen korrekt formaterad (innehåller den @ och. )?
            // I din data: Cecilia Carlsson(ID 103) har e-post cissi_c @outlook(saknar.com/.se).
            //Diskussion: Hur kan vi använda Regular Expressions(Regex) för att validera detta?

            new Validity().CheckValidity(customers, orders);
            Console.WriteLine("================================================");

            // 5.Konsistens(Consistency)
            // Kontrollera att samma typ av information skrivs på samma sätt.
            // Check: Skrivs länder och telefonnummer enhetligt?
            // I din data: Landet Sverige skrivs som "Sweden", "SE" och "Sverige".Telefonnummer har olika format(070 - xxx, 467xxx, 73xxx).
            // Diskussion: Hur "tvättar" vi datan så att allt blir "Sweden" innan det når databasen?

            new ConsistencyCountry().CheckConsistency(customers);
            Console.WriteLine("================================================");

            // 6.Rimlighet(Reasonability / Accuracy)
            // Kontrollera om värdena är logiskt möjliga.
            // Check: Är datum och belopp rimliga?
            // I din data: Order 5005 har ett negativt belopp(-500.00) och ett datum i framtiden(2030 - 12 - 24).
            // Diskussion: Kan en order ha ett negativt värde ? (Kanske en retur, men tillåter vårt system det ?).

            new Accuracy().CheckAccuracy(orders);
            Console.WriteLine("================================================");

            // LOAD ================================================================
            // Populate Database

            // Efter att ha kört alla kvalitetskontroller och manuellt "rättat" datan, kan vi nu ladda in den i databasen.
            // Jag rensade datan manuellt efter mn rapport och skapade nya "cleaned" csv filer,
            // I en "riktig" app skulle vi ha en process för att "rätta" datan automatiskt, innan den laddas in i databasen.

            // Hämta "Cleaned" Customers
            var loadCustomersCleaned = new LoadInitialData();

            var filepathCustomersCleaned = "..\\..\\..\\OriginalDataInExcel\\CustomersCleaned.csv";
            var customersCleaned = loadCustomers.ReadCustomers(filepathCustomersCleaned);

            // Hämta "Cleaned" Orders
            var filepathOrdersCleaned = "..\\..\\..\\OriginalDataInExcel\\OrdersCleaned.csv";
            var ordersCleaned = loadCustomers.ReadOrders(filepathOrdersCleaned);

            using (var dbContext = new AppDbContext(options.Options))
            {
                var dataInitiaizer = new DataInitializer();
                dataInitiaizer.PopulateDatabase(dbContext, customersCleaned, ordersCleaned);
            }
        }
    }
}
