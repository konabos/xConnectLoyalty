using System.IO;

namespace Konabos.XConnect.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Generating your model...");

            var model = Konabos.XConnect.Loyalty.Model.Models.LoyaltyModel.Model;

            var serializedModel = Sitecore.XConnect.Serialization.XdbModelWriter.Serialize(model);

            File.WriteAllText("c:\\temp\\" + model.FullName + ".json", serializedModel);

            System.Console.WriteLine("Press any key to continue! Your model is here: " + "c:\\temp\\" + model.FullName + ".json");
            System.Console.ReadKey();
        }
    }
}
