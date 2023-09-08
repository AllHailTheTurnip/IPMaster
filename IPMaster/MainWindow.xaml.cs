using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Management;

namespace IPMaster
{
    public class GeneralData 
    {
        public string NetworkName { get; set; }
        public string CurrentIP { get; set; }
        public string NewIP { get; set; }
        public string SubnetMask { get; set; }
        public List<string> AvailableNetworks { get; set; }

        public List<ManagementObject> ManagementObjects { get; set; } = new List<ManagementObject>();

        public GeneralData()
        {
            AvailableNetworks = new List<string>();
        }
    }

    public partial class MainWindow : Window
    {
        public GeneralData data = new GeneralData();
        public ManagementClass management = new ManagementClass("Win32_NetworkAdapterConfiguration");

        public MainWindow()
        {
            InitializeComponent();
            DataContext = data;

            ManagementObjectCollection managementObjects = management.GetInstances();

            foreach (ManagementObject obj in managementObjects) 
            {
                if (obj["IPAddress"] != null)
                {
                    data.AvailableNetworks.Add(obj["Description"].ToString());
                    data.ManagementObjects.Add(obj);
                }
            }
            
        }

        private void ListViewItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Get the name of the network by splitting it on the colon and removing the first whitespace.
            data.NetworkName = e.Source.ToString().Split(':')[1].Remove(0, 1);
            NetworkName.Text = data.NetworkName;
        }

        private void NetworkName_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Get matching managementObject from ManagementObjects by network name.
            ManagementObject foundObject = data.ManagementObjects.Find(x => { return x.GetPropertyValue("Description").ToString() == data.NetworkName; });
            string[] ipAddress = foundObject["IPAddress"] as string[];
            currentIPAddress.Text = ipAddress[0];
            
        }
    }
}
