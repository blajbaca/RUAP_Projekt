// This code requires the Nuget package Microsoft.AspNet.WebApi.Client to be installed.
// Instructions for doing this in Visual Studio:
// Tools -> Nuget Package Manager -> Package Manager Console
// Install-Package Microsoft.AspNet.WebApi.Client

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static System.Formats.Asn1.AsnWriter;

namespace CallRequestResponseService
{

    public class StringTable
    {
        public string[] ColumnNames { get; set; }
        public string[,] Values { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            InvokeRequestResponseService().Wait();
        }

        static async Task InvokeRequestResponseService()
        {
            Console.WriteLine("Gender (M/F):");
            string gender = Console.ReadLine();
            Console.WriteLine("\nCaste (G, ST, SC, OBC, MOBC):");
            string caste = Console.ReadLine();
            Console.WriteLine("\nClass X Percentage (Best, Vg, Good, Pass, Fail)");
            string classPercentage = Console.ReadLine();
            Console.WriteLine("\nClass XII Percentage (Best, Vg, Good, Pass, Fail)");
            string classXIIPercentage = Console.ReadLine();
            Console.WriteLine("\nInternal Asessment Percentage (Best, Vg, Good, Pass, Fail):");
            string internalAssessmentPercentage = Console.ReadLine();
            Console.WriteLine("\nWhether the studnet has a back or arear papers (Y, N):");
            string papers = Console.ReadLine();
            Console.WriteLine("\nMarital Status(Married/Unmarried):");
            string maritalStatus = Console.ReadLine();
            Console.WriteLine("\nLived in Town or Village T, V?:");
            string livingResidency = Console.ReadLine();
            Console.WriteLine("\nAdmission Category (Free,Paid):");
            string admissionCategory = Console.ReadLine();
            Console.WriteLine("\nFamily Monthly Income(Vh, High, Am, Medium, Low):");
            string income = Console.ReadLine();
            Console.WriteLine("\nLFamily Size(Large,Average,Small):");
            string familySize = Console.ReadLine();
            Console.WriteLine("\nFather Qualification (IL=Illiterate, UM=Under Class X, 10, 12 , Degree, PG ):");
            string fatherQualification = Console.ReadLine();
            Console.WriteLine("\nMother Qualification (IL=Illiterate, UM=Under Class X, 10, 12 , Degree, PG ):");
            string motherQualification = Console.ReadLine();
            Console.WriteLine("\nFather Occupation (Service, Business, Retired, Farmer, Others):");
            string fatherOccupation = Console.ReadLine();
            Console.WriteLine("\nMother Occupation (Service, Business, Retired, Farmer, Others):");
            string motherOccupation = Console.ReadLine();
            Console.WriteLine("\nNumber of Friends (Large, Average, Small)");
            string nOfFriends = Console.ReadLine();
            Console.WriteLine("\nStudy Hours (Good, Average, Poor):");
            string studyHours = Console.ReadLine();
            Console.WriteLine("\nSchool Type ( Govt, Private):");
            string schoolType = Console.ReadLine();
            Console.WriteLine("\nMedium (Eng,Asm,Hin,Ben):");
            string medium = Console.ReadLine();
            Console.WriteLine("\nHome to College Travel Time ( Large, Average, Small ):");
            string travelTime = Console.ReadLine();
            Console.WriteLine("\nClass Attendance Percentage(Good, Average, Poor):");
            string attendance = Console.ReadLine();

            using (var client = new HttpClient())
            {
                var scoreRequest = new
                {

                    Inputs = new Dictionary<string, StringTable>() {
                        {
                            "input1",
                            new StringTable()
                            {
                                ColumnNames = new string[] {"ge", "cst", "tnp", "twp", "iap", "arr", "ms", "ls", "as", "fmi", "fs", "fq", "mq", "fo", "mo", "nf", "sh", "ss", "me", "tt", "atd"},
                                Values = new string[,] { { gender, caste, classPercentage, classXIIPercentage, internalAssessmentPercentage, papers, maritalStatus, livingResidency, admissionCategory, income, familySize, fatherQualification, motherQualification, fatherOccupation, motherOccupation, nOfFriends, studyHours, schoolType, medium, travelTime, attendance }, }
                            }
                        },
                    },
                    GlobalParameters = new Dictionary<string, string>()
                    {
                    }
                };
                const string apiKey = "JMGr1v8e911R3uPy+gQ6dyfUQ57Lx7WkEc1CUXHnsf2Q9xGFJiTgLL7iPKKVKmIwzteI2bGgjozy+AMCeaZdxQ=="; // Replace this with the API key for the web service
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/18f99f70cffc44a1b1d366b35164c2e1/services/196275dfe2b44da2a873d9d9eaf1521c/execute?api-version=2.0&details=true");

                // WARNING: The 'await' statement below can result in a deadlock if you are calling this code from the UI thread of an ASP.Net application.
                // One way to address this would be to call ConfigureAwait(false) so that the execution does not attempt to resume on the original context.
                // For instance, replace code such as:
                //      result = await DoSomeTask()
                // with the following:
                //      result = await DoSomeTask().ConfigureAwait(false)


                HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    var res=JsonConvert.SerializeObject(result, Formatting.Indented);
                    dynamic data = JsonConvert.DeserializeObject(result);
                    var values = data.Results.output1.value.Values[0];
                    Console.WriteLine("Scored Probabilities for Class Best - Scored Probabilities for Class Vg - Scored Probabilities for Class Good - Scored Probabilities for Class Pass - Scored Probabilities for Class Fail");
                    Console.WriteLine(string.Join(", ", values.ToObject<List<string>>().GetRange(21, 6)));

                }
                else
                {
                    Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));

                    // Print the headers - they include the requert ID and the timestamp, which are useful for debugging the failure
                    Console.WriteLine(response.Headers.ToString());

                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseContent);
                }
                

            }
        }
    }
}
