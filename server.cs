using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using Newtonsoft.Json;

namespace Lechat_Server_File
{
    class server
    {
        public static ListBox logView;
        static Socket serverSocket;
        static int bytesPerPush = 5000;
        public static System.Windows.Forms.DataVisualization.Charting.Chart chart;
        public static void initServer()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, 1000));
            serverSocket.Listen(5);
            serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
            addLog("Listening for the client");
        }
        static void addLog(string log)
        {
            if (logView.InvokeRequired)logView.Invoke(new Action(() => { logView.Items.Add(log + "     " + DateTime.Now.ToString()); logView.SelectedIndex = logView.Items.Count - 1; }));
            else
            {
                logView.Items.Add(log + "     " + DateTime.Now.ToString());
                logView.SelectedIndex = logView.Items.Count - 1;
            }
        }
        static void AcceptCallback(IAsyncResult ar)
        {
            serverSocket=serverSocket.EndAccept(ar);
            beginReceive();
            addLog("Connected to Client");
        }
        static void beginReceive()
        {
            byte[] byteData = new byte[1023];
            Tuple<byte[], Socket> toAsyncResult = new Tuple<byte[], Socket>(byteData, serverSocket);
            serverSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(receiveCallback), toAsyncResult);
        }

        static void beginReceiveFile(int index)
        {
            if (index >= toReceiveFileBytes.Length)
            {
                System.IO.File.WriteAllBytes(@"C:\Users\Sid\Desktop\received." + toReceiveFileType, toReceiveFileBytes);
                addLog("File Transfer Complete");
                beginReceive();
                return;
            }
            byte[] fileBytes = new byte[bytesPerPush];
            Tuple<byte[], int> toAsyncResult = new Tuple<byte[], int>(fileBytes, index);
            serverSocket.BeginReceive(fileBytes, 0, fileBytes.Length, SocketFlags.None, new AsyncCallback(onFileByteReceived), toAsyncResult);
            //RECEIVE FILE BYTES
        }
        static string toReceiveFilename;
        static string toReceiveFileType;
        static byte[] toReceiveFileBytes;
        static void onFileByteReceived(IAsyncResult result)
        {
            Tuple<byte[], int> fromAsyncResult = result.AsyncState as Tuple<byte[], int>;
            int index = fromAsyncResult.Item2;
            serverSocket.EndReceive(result);
            byte[] fileBytes = fromAsyncResult.Item1;
            for(int i = 0; i < fileBytes.Length; i++)
            {
                try
                {
                    toReceiveFileBytes[index + i] = fileBytes[i];
                }
                catch
                {
                    //INDEX OUT OF BOUNDS
                    break;
                }
            }
            //FILE BYTES RECEIVED
            beginReceiveFile(index + bytesPerPush);
            send(Command.NULL, null);
        }
        static void receiveCallback(IAsyncResult result)
        {
            Tuple<byte[], Socket> fromAsyncResult = result.AsyncState as Tuple<byte[], Socket>;
            Socket socket = fromAsyncResult.Item2;
            byte[] gotBytes = fromAsyncResult.Item1;
            try
            {
            socket.EndReceive(result);
            Data receivedData = new Data(gotBytes);
            switch (receivedData.cmd)
            {
                case Command.txtMesg:
                        addLog(receivedData.cmd.ToString() + ":" + receivedData.message);
                        beginReceive();
                        break;
                case Command.startFile:
                        addLog("Request Received");
                        file fileToReceive = JsonConvert.DeserializeObject<file>(receivedData.message);
                        addLog(receivedData.message);
                        toReceiveFilename = fileToReceive.name;
                        toReceiveFileType = fileToReceive.fileType;
                        toReceiveFileBytes = new byte[Convert.ToInt32(fileToReceive.fileLength)];
                        bytesPerPush = fileToReceive.bytesPerPush;
                        beginReceiveFile(0);
                        send(Command.startFile, "");
                        addLog("File Transfer in Progress");
                        break;
            }
            }
            catch(Exception E)
            {
                addLog(E.ToString());
                //CLIENT IS DESCONNECTED
                addLog("Client Disconnected");
                //initServer();
                return;
            }
        }

        

        private static void send(Command command, string msg)
        {
            Data response = new Data { cmd = command, message = msg };
            byte[] myBytes = response.getBytes();
            serverSocket.BeginSend(myBytes, 0, myBytes.Length, SocketFlags.None, new AsyncCallback(onSend), serverSocket);
        }

        private static void onSend(IAsyncResult result) //SEND CALL BACK............
        {
            Socket mySocket = (Socket)result.AsyncState;
            mySocket.EndSend(result);
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
            public file(string name, string fileType, string fileLength, int bytesPerPush)
            {
                this.name = name;
                this.fileLength = fileLength;
                this.fileType = fileType;
                this.bytesPerPush = bytesPerPush;
            }
        }
    }
}
