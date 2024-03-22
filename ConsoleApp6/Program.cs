using System;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using Modbus.Data;
using Modbus.Device;

namespace ConsoleApp6
{
    internal class Program
    {
        private static ModbusTcpSlave _modbusTcpSlave;
        private static TcpListener _listener;
        public static void Main(string[] args)
        {
            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 502);
            _modbusTcpSlave=ModbusTcpSlave.CreateTcp(1, _listener);
            _modbusTcpSlave.DataStore = DataStoreFactory.CreateDefaultDataStore();
            _modbusTcpSlave.DataStore.DataStoreWrittenTo += DataStoreWrittenToHandle;
            _modbusTcpSlave.ModbusSlaveRequestReceived += ModbusSlaveRequestReceivedHandle;
            _modbusTcpSlave.WriteComplete += WriteCompleteHandle;
            _modbusTcpSlave.ListenAsync();
            Console.ReadKey();
        }

        private static void WriteCompleteHandle(object sender, ModbusSlaveRequestEventArgs e)
        {
            string str = "";
            foreach (var item in e.Message.MessageFrame)
            {
                str += item.ToString("x2").PadLeft(2, '0').ToUpper() + "  ";
            }
            Console.WriteLine("服务器写入完成:  " + str + "\r\n");
        }

        private static void ModbusSlaveRequestReceivedHandle(object sender, ModbusSlaveRequestEventArgs e)
        {
            string str = "";
            foreach (var item in e.Message.MessageFrame)
            {
                str += item.ToString("x2").PadLeft(2, '0').ToUpper() + "  ";    
            }

            Console.WriteLine("服务器收到报文:  " + str + "\r\n");
            
        }

        private static void DataStoreWrittenToHandle(object sender, DataStoreEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}