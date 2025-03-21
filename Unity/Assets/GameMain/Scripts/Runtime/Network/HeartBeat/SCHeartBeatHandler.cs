/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/2/23 11:28:3
 * Description:
 * Modify Record:
 *************************************************************/

using GameFramework.Network;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class SCHeartBeatHandler : PacketHandlerBase
    {
        public override int Id => 1;

        public override void Handle(object sender, Packet packet)
        {
            var packetImp = packet as SCHeartBeat;
            if (packetImp != null)
                Log.Info($"Receive packet (heart beat : {packetImp.Id.ToString()}).");
        }
    }
}