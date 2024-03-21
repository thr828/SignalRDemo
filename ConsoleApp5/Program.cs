using Modbus.Device;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp5
{
    internal class Program
    {
        private static IModbusMaster master;
        private static SerialPort serialPort = new SerialPort();
        static void Main(string[] args)
        {
            serialPort.PortName = "COM1";
            serialPort.BaudRate = 9600;
            serialPort.DataBits = 8;
            serialPort.Parity = Parity.None;
            serialPort.StopBits = StopBits.One;

            master=ModbusSerialMaster.CreateRtu(serialPort);
            if(!serialPort.IsOpen)
                serialPort.Open();

            #region ReadCoils
            // while (true)
            // {
            //     Thread.Sleep(3000);
            //     var result = ReadCoils();
            //     Console.WriteLine("start");
            //     foreach (var p in result)
            //     {
            //         Console.WriteLine(p?1:0);
            //     }
            //
            //     Console.WriteLine("close");
            // }
            

            #endregion

            #region ReadInputs
            // while (true)
            // {
            //     Thread.Sleep(3000);
            //     var result = ReadInputs();
            //     Console.WriteLine("start");
            //     foreach (var p in result)
            //     {
            //         Console.WriteLine(p?1:0);
            //     }
            //
            //     Console.WriteLine("close");
            // }
            #endregion

            #region ReadHoldingRegisters
            // while (true)
            // {
            //     Thread.Sleep(3000);
            //     Console.WriteLine("start");
            //     var result = ReadHoldingRegisters();
            //     foreach (var p in result)
            //     {
            //         Console.WriteLine(p);
            //     }
            //     Console.WriteLine("close");
            // }
            
            #endregion

            #region WriteSingleCoil

            //WriteSingleCoil();

            #endregion

          //  WriteSingleRegister();
           // WriteArrayCoil();
           WriteArrayRegister();
        }

        private static bool[] ReadCoils()
        {
          return  master.ReadCoils(1, 7, 3);
        }

        private static bool[] ReadInputs()
        {
            return master.ReadInputs(1, 7, 3);
        }
 
        private static ushort[] ReadHoldingRegisters()
        {
            return master.ReadHoldingRegisters(1, 0, 10);
        }

        private static ushort[] ReadInputRegisters()
        {
            return master.ReadInputRegisters(1, 0, 10);
        }
        
        private static void WriteSingleCoil()
        {
            master.WriteSingleCoil(1, 2, true);
            master.WriteSingleCoil(1, 3, true);
            master.WriteSingleCoil(1, 4, true);
        }
        
        private static void WriteSingleRegister()
        {
            // Rx:001138-01 06 00 04 00 32 49 DE  收到报文
            // Tx:001139-01 06 00 04 00 32 49 DE  相应报文
            master.WriteSingleRegister(1, 4, 50);
        }
        
        private static void WriteArrayCoil()
        {
            List<bool> result = new List<bool>(){true,false,true,true};
            master.WriteMultipleCoils(1, 0, result.ToArray());
        }
        
        private static void WriteArrayRegister()
        {
            List<ushort> result = new List<ushort>(){30,40,50,60};
            master.WriteMultipleRegisters(1, 0, result.ToArray());
        }
    }
}
