using ConsoleApplication1;
using System.Management;
using USB_TEST;

var Send = new SendMail();
int ConnectedCount;
int MailSentCount = 0;


if (Environment.GetCommandLineArgs().Length > 1)
{
    string Argument = args[0];
    switch (Argument)
    {
        case "/list":
            var usbDevices = GetUSBDevices();
            foreach (var usbDevice in usbDevices)
            {
                Console.WriteLine("Device ID: {0}, PNP Device ID: {1}, Description: {2}",
                    usbDevice.DeviceID, usbDevice.PnpDeviceID, usbDevice.Description);
            }
            Environment.Exit(0);
            break;
        case "/device":
            while (true)
            {
                ConnectedCount = 0;
                foreach (var usbDevice in (List<USBDeviceInfo>?)GetUSBDevices())
                {
                if (usbDevice.DeviceID == args[1])
                {
                        if (ConnectedCount == 0) // is our device connected if so count it, maybe in the future we can do more than one device
                        {
                            ConnectedCount++;
                        }
                    }
                }
                Thread.Sleep(60000);
                if (ConnectedCount == 0)
                {
                    if (MailSentCount == 0) // we only want to send mail once per run.
                    {
                        Send.Send(args[1]);

                        MailSentCount++;
                    }
                }

            }
            break;
    }

    List<USBDeviceInfo> GetUSBDevices()
    {
        List<USBDeviceInfo> devices = new List<USBDeviceInfo>();
        ManagementObjectCollection collection;
        using (var searcher = new ManagementObjectSearcher(@"Select \* From Win32_USBHub"))
            collection = searcher.Get();
        foreach (var device in collection)
        {
            devices.Add(new USBDeviceInfo(
            (string)device.GetPropertyValue("DeviceID"),
            (string)device.GetPropertyValue("PNPDeviceID"),
            (string)device.GetPropertyValue("Description")
            ));
        }
        collection.Dispose();
        return devices;
    }
}
else
{
    Console.WriteLine("Please provide an argument.");
    Console.WriteLine("Argument list below");
    Console.WriteLine("list   shows all connected usb devices");
    Console.WriteLine("Look for the device you want to monitor in the list and copy the device ID make sure to put in quotes.");
    Console.WriteLine("device DEVICE STRING this should be the device id for example \"USB\\VID_18A5 & PID_0243\\07190BCB888D0545\"");
    }
namespace ConsoleApplication1
{
    class USBDeviceInfo
    {
        public USBDeviceInfo(string deviceID, string pnpDeviceID, string description)
        {
            this.DeviceID = deviceID;
            this.PnpDeviceID = pnpDeviceID;
            this.Description = description;
        }
        public string DeviceID { get; private set; }
        public string PnpDeviceID { get; private set; }
        public string Description { get; private set; }
    }
}

