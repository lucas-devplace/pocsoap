using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceReference1;

namespace test03
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BasicHttpBinding basicHttpBinding = null;
            EndpointAddress endpointAddress = null;
            

            basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            endpointAddress = new EndpointAddress(new Uri("url-soap-aqui"));

            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                ServiceReference1.RightNowSyncPortClient syncClient = new RightNowSyncPortClient(basicHttpBinding,endpointAddress);
                syncClient.ClientCredentials.UserName.UserName = "user-aqui";
                syncClient.ClientCredentials.UserName.Password = "senha-aqui";

                Contact contact = new Contact();
                ID cID = new ID();
                cID.id = 0;
                cID.idSpecified = true;
                contact.ID = cID;

                var api = new APIAccessRequestHeader();
                GetProcessingOptions getProcessiongOptions = new GetProcessingOptions();
                getProcessiongOptions.FetchAllNames = false;
                ClientInfoHeader clientInfoHeader = new ClientInfoHeader();
                clientInfoHeader.AppID = "Read Contact";
                RNObject[] orgObjects = new RNObject[] { contact };
                RNObject[] readReturn;

                var resp = syncClient.GetAsync(clientInfoHeader, api,
                    orgObjects, getProcessiongOptions);
                
                
                readReturn  = resp.Result.RNObjectsResult;
                
                Contact readContact = (Contact)readReturn[0];

                Console.WriteLine("Lookup name: " + readContact.LookupName);
                Console.WriteLine("Login: " + readContact.Login);
                Console.Read();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.WriteLine("Digite \"ENTER\" para sair...");
            Console.ReadLine();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
