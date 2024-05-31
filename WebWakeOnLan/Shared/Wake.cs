using System;
using System.IO;
using System.Net.Sockets;
using System.Net;

namespace WebWakeOnLan.Shared
{
    public class Wake
    {
        private readonly string logFilePath = "log.txt";

        public void PoslatWakeOnLan(string macAdresa)
        {
            try
            {
                Log($"Přijata MAC adresa: {macAdresa}");
                byte[] macBytes = VybratMacAdresu(macAdresa);

                byte[] packet = new byte[102];
                for (int i = 0; i < 6; i++)
                {
                    packet[i] = 0xFF;
                }
                for (int i = 1; i <= 16; i++)
                {
                    Array.Copy(macBytes, 0, packet, i * 6, 6);
                }

                using (UdpClient klient = new UdpClient())
                {
                    klient.Connect(IPAddress.Broadcast, 9);
                    klient.Send(packet, packet.Length);
                    Log("Wake-on-LAN packet úspěšně odeslán.");
                }
            }
            catch (Exception ex)
            {
                Log($"Chyba při odesílání Wake-on-LAN packetu: {ex.Message}");
            }
        }

        private byte[] VybratMacAdresu(string macAdresa)
        {
            try
            {
                Log($"Zpracovávám MAC adresu: {macAdresa}");
                string[] macSegmenty = macAdresa.Split(':', '-');
                if (macSegmenty.Length != 6)
                {
                    throw new ArgumentException("Nesprávný formát MAC Adresy");
                }

                byte[] macBytes = new byte[6];
                for (int i = 0; i < 6; i++)
                {
                    macBytes[i] = Convert.ToByte(macSegmenty[i], 16);
                }

                return macBytes;
            }
            catch (Exception ex)
            {
                Log($"Chyba při zpracování MAC adresy: {ex.Message}");
                throw;
            }
        }

        private void Log(string message)
        {
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now}: {message}");
            }
        }
    }
}
