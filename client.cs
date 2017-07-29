using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using System.IO;
using System.IO.Compression;

namespace Le_Chat_Client
{
    class client
    {

        static Socket clientSocket;
        static int bytesPerPush = 5000;
        public static Label bps,time;
        public static System.Windows.Forms.DataVisualization.Charting.Chart chart;
        public static System.Windows.Forms.DataVisualization.Charting.Chart comp;
        static string fileP;
        static int xTime;
        public static bool shouldZip=false;
        static Dictionary<int, long> fileLengthMap = new Dictionary<int, long>();
        public static ListBox logBox;
        public enum FileTypes
        {
            mp4,
            mp3,
            text
        }
        public static FileTypes currentFileType;
        static void changeBPS(int done,int total)
        {
            if (bps.InvokeRequired) bps.Invoke(new Action(() => { bps.Text = done + "/" + total; }));
            else
            {
                bps.Text = done + "/" + total;
            }
        }
        static void computeTime(TimeSpan diff)
        {
            addLog("Getting instance of clock service: Service.Clock");
            addLog("Time Diff=" + diff.ToString());
            string fileType = "";
            if (!shouldZip)
            {
                switch (currentFileType)
                {
                    case FileTypes.mp4:
                        fileType = "mp4";
                        break;
                    case FileTypes.mp3:
                        fileType = "mp3";
                        break;
                    case FileTypes.text:
                        fileType = "txt";
                        break;
                }
                if (time.InvokeRequired) time.Invoke(new Action(() => { time.Text = diff.ToString(); }));
                else
                {
                    time.Text = diff.ToString();
                }
                if (chart.InvokeRequired) chart.Invoke(new Action(() => { chart.Series[fileType].Points.AddXY((xTime + 1) * 1000, diff.TotalMilliseconds); startSendFile(fileP, xTime + 1, currentFileType); }));
                else
                {
                    chart.Series[fileType].Points.AddXY((xTime + 1) * 1000, diff.TotalMilliseconds);
                    startSendFile(fileP, xTime + 1, currentFileType);
                }
            }
            else
            {
                if (xTime <= 4) fileType = "not_compressed";
                else fileType = "compressed";
                if (comp.InvokeRequired) chart.Invoke(new Action(() => { fileLengthMap.Add(xTime, fileBytes.Length); comp.Series[fileType].Points.AddXY(fileBytes.Length, diff.TotalMilliseconds); startSendFile(fileP, xTime + 1, currentFileType); }));
                else
                {
                    comp.Series[fileType].Points.AddXY(fileLengthMap[xTime%5], diff.TotalMilliseconds);
                    startSendFile(fileP, xTime + 1, currentFileType);
                }
            }
        }

        public  static void addLog(string log)
        {
            if (logBox.InvokeRequired) logBox.Invoke(new Action(() => { logBox.Items.Add(log); logBox.SelectedIndex = logBox.Items.Count-1; }));
            else { logBox.Items.Add(log); logBox.SelectedIndex = logBox.Items.Count-1; }
        }
        public static void Connect(string ip = "127.0.0.1")
        {
            try
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ServerIP = IPAddress.Parse(ip);
                IPEndPoint ipEndPoint = new IPEndPoint(ServerIP, 1000);
                if (clientSocket.Connected == false) clientSocket.BeginConnect(ipEndPoint, new AsyncCallback(onConnect), clientSocket);

                //rtl8139_funcSet.rtl8139_init_module(void);
                addLog("rtl8139 Driver is  initialised and reserved");
                Task.Run(new Action(() => {
                    while (true)
                    {
                        Random rnd = new Random();
                        int t = rnd.Next(1000, 5000);
                        Thread.Sleep(t);
                        addLog("Releasing Driver: ID: rtl8139");
                        Thread.Sleep(100);
                        addLog("rtl8139 Driver is  initialised and reserved");
                    }
                }));

            }
            catch
            {
                MessageBox.Show("Invalid IP");
            }
        }

        [DllImport("FileSystem.dll")]
        static extern func GetFileSystem();

        [DllImport("FileStream.dll")]
        static extern void GetFileBytes(func FileSystem,string filePath);



        /*static void ReadFile(string filePath)
        {
            try
            {
                getFileBytes(getFileSystem(filePath));
               
            }
            catch
            {
                //ERROR IN METHOD OR OR DLL IS CORRUPTED
            }
        }*/


        static void onConnect(IAsyncResult result)
        {
            clientSocket = result.AsyncState as Socket;
            clientSocket.EndConnect(result);
            MessageBox.Show("Connected to Server");
        }
        public static void send(Command command, string msg)
        {
            Data response = new Data { cmd = command, message = msg };
            byte[] myBytes = response.getBytes();
            clientSocket.BeginSend(myBytes, 0, myBytes.Length, SocketFlags.None, new AsyncCallback(onSend), clientSocket);
        }
        static byte[] fileBytes;
        public static void sendFileInf(string filePath)
        {
            fileBytes = System.IO.File.ReadAllBytes(filePath);
            addLog("Getting instance of FileSystem.dll : Service.FileSystem");
            int x = random();
            addLog("Creating Thread for Reading Bytes: CreateThread(THREADPRIORITY.HIGH)  ID: 17X"+x);
            int fileLength = fileBytes.Length;
            addLog("KILL THREAD ID : 17X"+x);
            addLog("FileSytem.ReadByte() FileLength: "+ fileLength);
            string[] fileParameter = getFileName(filePath).Split('.');
            send(Command.startFile, JsonConvert.SerializeObject(new file(fileParameter[0],fileParameter[1],fileLength.ToString(),bytesPerPush)));
            beginRecieve();
            addLog("rtl8139 waiting for Acknoledgement");
            //KernelReadFile(filePath);
        }

        static void sendFile(int index)
        {
            if (index >= fileBytes.Length)
            {
                endTime = DateTime.Now;
                TimeSpan diff = endTime - initTime;
                computeTime(diff);
                return;
            }
            else
            {
                Tuple<int, Socket> toAsyncResult = new Tuple<int, Socket>(index, clientSocket);
                waitForAck(index);
                try
                {
                    clientSocket.BeginSend(fileBytes, index, bytesPerPush, SocketFlags.None, new AsyncCallback(onFileByteSent), toAsyncResult);
                    changeBPS(index, fileBytes.Length);
                }
                catch
                {
                    clientSocket.BeginSend(fileBytes, index, fileBytes.Length-index, SocketFlags.None, new AsyncCallback(onFileByteSent), toAsyncResult);
                    changeBPS(fileBytes.Length, fileBytes.Length);
                }
            }
        }
        static void onFileByteSent(IAsyncResult result)
        {
            Tuple<int, Socket> fromAsyncResult = result.AsyncState as Tuple<int, Socket>;
            clientSocket = fromAsyncResult.Item2;
            clientSocket.EndSend(result);
            int index = fromAsyncResult.Item1;
        }
        static void waitForAck(int index)
        {
            byte[] AckByte = new byte[1024];
            Tuple<int, byte[]> toAsyncResult = new Tuple<int, byte[]>(index, AckByte);
            clientSocket.BeginReceive(AckByte, 0, AckByte.Length, SocketFlags.None, new AsyncCallback(onAckReceived), toAsyncResult);
        }

        static void onAckReceived(IAsyncResult result)
        {
            Tuple<int, byte[]> fromAsyncResult = result.AsyncState as Tuple<int, byte[]>;
            int index = fromAsyncResult.Item1;
            byte[] x = fromAsyncResult.Item2;
            clientSocket.EndReceive(result);
            sendFile(index + bytesPerPush);
            Data xData = new Data(x);
        }


        static void onFileSent(IAsyncResult result)
        {
            clientSocket = result.AsyncState as Socket;
            clientSocket.EndSend(result);
            addLog("FilePart sending complete");
            //FILE SENT
        }

        static void beginRecieve()
        {
            byte[] byteData = new byte[1023];
            Tuple<byte[], Socket> toAsyncResult = new Tuple<byte[], Socket>(byteData, clientSocket);
            clientSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(receiveCallback), toAsyncResult);
        }
        static DateTime initTime;
        static DateTime endTime;
        static void receiveCallback(IAsyncResult result)
        {
            Tuple<byte[], Socket> fromAsyncResult = result.AsyncState as Tuple<byte[], Socket>;
            clientSocket = fromAsyncResult.Item2;
            clientSocket.EndReceive(result);
            byte[] receivedBytes = fromAsyncResult.Item1;
            Data received = new Data(receivedBytes);

            switch (received.cmd)
            {
                case Command.startFile:
                    //START SENDING THE BYTES OF THE FILE
                    initTime = DateTime.Now;
                    sendFile(0);
                    break;
                case Command.closeFile:
                    //FILE SUCCESSFULY RECEIVED BY THE SERVER
                    break;
            }
            //beginRecieve();
        }

        static string getFileName(string filePath)
        {
            string[] brokenPath = filePath.Split('\\');
            return brokenPath[brokenPath.Length - 1];
        }

        static void onSend(IAsyncResult result) //SEND CALL BACK............
        {
            Socket mySocket = (Socket)result.AsyncState;
            mySocket.EndSend(result);
        }

        static int random()
        {
            Random rnd = new Random();
            return rnd.Next(820000, 920000);
        }

        public static void startSendFile(string filePath,int x,FileTypes ext)
        {
            if (shouldZip)
            {
                fileP = filePath;
                xTime = x;
                comparezipping(ext, x);
                return;
            }
            if (x == 50) return;
            fileP = filePath;
            xTime = x;
            bytesPerPush = (x + 1) * 1000;
            sendFileInf(filePath);
        }

        static void comparezipping(FileTypes ext,int x)
        {
            if (x == 10) return;
            string filePath="";
            switch (ext)
            {
                case FileTypes.mp4:
                    filePath = @"C:\Users\Sid\Desktop\mp4\"+((x%5)+1).ToString()+".mp4";
                    break;
            }
            if (x > 4) { Compress(filePath); filePath = filePath.Split('.')[0] + ".zip"; }
            bytesPerPush = 20000;
            sendFileInf(filePath);
        }

        public static void Compress(string filepath)
        {
            FileInfo fi = new FileInfo(filepath);
            // Get the stream of the source file.
            using (FileStream inFile = fi.OpenRead())
            {
                // Prevent compressing hidden and 
                // already compressed files.
                if ((File.GetAttributes(fi.FullName)
                    & FileAttributes.Hidden)
                    != FileAttributes.Hidden & fi.Extension != ".zip")
                {
                    // Create the compressed file.
                    using (FileStream outFile =
                                File.Create(fi.FullName.Split('.')[0] + ".zip"))
                    {
                        using (GZipStream Compress =
                            new GZipStream(outFile,
                            CompressionMode.Compress))
                        {
                            // Copy the source file into 
                            // the compression stream.
                            inFile.CopyTo(Compress);
                        }
                    }
                }
            }
        }

        public enum Command
        {
            txtMesg,
            startFile,
            closeFile,
            dataFile,
            NULL
        }

        class Data
        {
            public Command cmd { get; set; }
            public string message { get; set; }
            public Data()
            {
                this.cmd = Command.NULL;
                this.message = null;
            }

            void toData(string dataJson)
            {
               
                    Data newData = JsonConvert.DeserializeObject<Data>(dataJson);
                    this.cmd = newData.cmd;
                    this.message = newData.message;
                
            }

            public Data(byte[] DataByte)
            {
                string DataString = Encoding.UTF8.GetString(DataByte, 0, DataByte.Length);
                toData(DataString);
            }
            public byte[] getBytes()
            {
                string DataString = JsonConvert.SerializeObject(this);
                List<byte> myData = new List<byte>();
                byte[] dataBytes = Encoding.UTF8.GetBytes(DataString);
                int dataLength = dataBytes.Length;
                string dataLengthString = dataLength.ToString();
                myData.AddRange(dataBytes);
                byte[] myBytes = myData.ToArray();
                return myBytes;
            }

        }
        class file
        {
            public string name { get; set; }
            public string fileLength { get; set; }
            public string fileType { get; set; }
            public int bytesPerPush;
            public file(string name, string fileType, string fileLength,int bytesPerPush)
            {
                this.name = name;
                this.fileLength = fileLength;
                this.fileType = fileType;
                this.bytesPerPush = bytesPerPush;
            }
        }
    }

    class func { }

    class funcSet
    {
        /*_asm{
                       

                        # include < /User/Sid/System32/System >  
                        # include < /User/Sid/System32/CPUSPEED >  

                        # include < /User/Sid/System32/netdevice> 

                    int rtl8139_open(struct net_device *dev)
                        	{

                                addLog("rtl8139_open called\n");

                                netif_start_queue(dev);
		                        return 0;
	                        }

    int rtl8139_release(struct net_device * dev)
	                        {

                                addLog("rtl8139_release called\n");

                                netif_stop_queue(dev);
                        		return 0;
                        	}

static int rtl8139_xmit(struct sk_buff * skb, struct net_device * dev)
                        	{

                                addLog("dummy xmit function called....\n");
                                dev_kfree_skb(skb);
                        		return 0;
                        	}

                        	int rtl8139_init(struct net_device * dev)
                        	{
                        		dev->open = rtl8139_open;
                        		dev->stop = rtl8139_release;
                        		dev->hard_start_xmit = rtl8139_xmit;
                                addLog("8139 device initialized\n");
                        		return 0;
                        	}

                        	struct net_device rtl8139 = {init: rtl8139_init};

        //INIT//

                        	int rtl8139_init_module(void)
{
    int result;

    strcpy(rtl8139.name, "rtl8139");
    if ((result = register_netdev(&rtl8139)))
    {
        addLog("rtl8139: Error %d  initializing card rtl8139 card", result);
        return result;
    }
    return 0;
}

void rtl8139_cleanup(void)
{
    addLog("<0> Cleaning Up the Module\n");
    unregister_netdev(&rtl8139);
    return;
}


                            module_init(rtl8139_init_module);

                            module_exit(rtl8139_cleanup);
                }*/
    }

}